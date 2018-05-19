using System;
using System.Configuration;
using System.ServiceModel;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.PoupeDirect.DB;
using Gradual.OMS.PoupeDirect.Lib;
using Gradual.OMS.PoupeDirect.Lib.Mensagens;
using log4net;
using Gradual.OMS.PoupeDirect.Lib.Dados;
using System.Data.Common;
using Gradual.Generico.Dados;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Gradual.OMS.Email.Lib;

namespace Gradual.OMS.PoupeDirect
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoPoupeDirect : IServicoControlavel, IServicoPoupeDirect
    {
        #region | Atributos

        private System.Timers.Timer gTimer = new System.Timers.Timer();

        private System.Timers.Timer gTimerVerificaVencimento = new System.Timers.Timer();

        private System.Timers.Timer gTimerRentabilidade = new System.Timers.Timer();

        private System.Timers.Timer gTimerCobranca = new System.Timers.Timer();

        private ServicoStatus gServicoStatus = ServicoStatus.Indefinido;

        private readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Propriedades


        private static readonly object gSingleton = new object();

        private long GetIntervaloExecucao
        {
            get
            {
                return 58    //--> 58 minutos
                     * 60    //--> convertendo segundo em minuto
                     * 1000; //--> convertendo milissegundos para segundos
            }
        }

        private int GetHoraDeEnvio
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(ConfigurationManager.AppSettings["HorarioDeDisparoDoServico"], out lRetorno))
                    return 4;

                return lRetorno;
            }
        }

        private int GetHoraVerificaVencimento
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(ConfigurationManager.AppSettings["HorarioVerificaVencimento"], out lRetorno))
                    return 1;

                return lRetorno;
            }
        }

        private int GetHoraRentabilidade
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(ConfigurationManager.AppSettings["HorarioRentabilidade"], out lRetorno))
                    return 1;

                return lRetorno;
            }
        }

        private int GetHoraCobrancao
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(ConfigurationManager.AppSettings["HorarioCobranca"], out lRetorno))
                    return 2;

                return lRetorno;
            }
        }

        #endregion

        #region | Construtores

        public ServicoPoupeDirect() { }

        #endregion

        #region | Métodos

        private void gTimer_Elapsed_VerificaVencimento(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.gLogger.DebugFormat("Verificando hora para verificação de vencimento. Horário marcado {0}h / Hora atual {1}h. Às {2}.", this.GetHoraVerificaVencimento.ToString(), DateTime.Now.Hour.ToString(), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"));

            if (DateTime.Now.Hour.Equals(this.GetHoraVerificaVencimento))
            {
                this.InserirClienteVencimentoHistorico();
            }
        }

        private void gTimer_Elapsed_VerificarRentabilidade(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.gLogger.DebugFormat("Rentabilizar os clientes para o plano direct. Horário marcado {0}h / Hora atual {1}h. Às {2}.", this.GetHoraRentabilidade.ToString(), DateTime.Now.Hour.ToString(), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"));

            if (DateTime.Now.Hour.Equals(this.GetHoraRentabilidade))
            {
                this.InserirMovimentoCliente();
            }
        }

        private void gTimer_Elapsed_VerificarCobranca(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.gLogger.DebugFormat("Verifica os clientes que precisam ser cobrados. Horário marcado {0}h / Hora atual {1}h. Às {2}.", this.GetHoraCobrancao.ToString(), DateTime.Now.Hour.ToString(), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"));

            if (DateTime.Now.Hour.Equals(this.GetHoraCobrancao))
            {
                this.GerarArquivoCCOUPoupe();
            }
        }

        private void gTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.gLogger.DebugFormat("Verificando hora de início da migração. Horário marcado {0}h / Hora atual {1}h. Às {2}.", this.GetHoraDeEnvio.ToString(), DateTime.Now.Hour.ToString(), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"));

            if (DateTime.Now.Hour.Equals(this.GetHoraDeEnvio))
            {
                this.ExportarClientePoupe(new ExportarClientePoupeRequest());
            }
        }

        #endregion

        #region | IServicoControlavel Members

        public void IniciarServico()
        {
            try
            {
                this.gTimer.Elapsed += new System.Timers.ElapsedEventHandler(gTimer_Elapsed);
                this.gTimer.Interval = this.GetIntervaloExecucao;
                this.gTimer.Enabled = true;
                this.gTimer.Start();


                this.gTimerVerificaVencimento.Elapsed += new System.Timers.ElapsedEventHandler(gTimer_Elapsed_VerificaVencimento);
                this.gTimerVerificaVencimento.Interval = this.GetIntervaloExecucao;
                this.gTimerVerificaVencimento.Enabled = true;
                this.gTimerVerificaVencimento.Start();


                this.gTimerRentabilidade.Elapsed += new System.Timers.ElapsedEventHandler(gTimer_Elapsed_VerificarRentabilidade);
                this.gTimerRentabilidade.Interval = this.GetIntervaloExecucao;
                this.gTimerRentabilidade.Enabled = true;
                this.gTimerRentabilidade.Start();


                this.gTimerCobranca.Elapsed += new System.Timers.ElapsedEventHandler(gTimer_Elapsed_VerificarCobranca);
                this.gTimerCobranca.Interval = this.GetIntervaloExecucao;
                this.gTimerCobranca.Enabled = true;
                this.gTimerCobranca.Start();

                this.gLogger.InfoFormat("Inciado o serviço PoupeDirect");
                this.gServicoStatus = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                this.gTimer.Enabled = false;
                this.gServicoStatus = ServicoStatus.Erro;
                this.gLogger.ErrorFormat("Parando o serviço de PoupeDirect  [Parada Forçada - Iniciação] - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }

        public void PararServico()
        {
            try
            {
                this.gTimer.Enabled = false;
                this.gTimerVerificaVencimento.Enabled = false;
                this.gServicoStatus = ServicoStatus.Parado;

                this.gLogger.InfoFormat("Parando o serviço de exportação de novos clientes PoupeDirect [Parada Forçada]");
            }
            catch (Exception ex)
            {
                this.gServicoStatus = ServicoStatus.Erro;
                this.gLogger.ErrorFormat("Parando o serviço de PoupeDirect  [Parada Forçada - ERRO] - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return this.gServicoStatus;
        }

        #endregion

        #region | IServicoPoupeDirect Members

        public ClienteProdutoResponse InserirAtualizarClienteProduto(ClienteProdutoRequest pRequest)
        {
            ClienteProdutoResponse lRetorno = new ClienteProdutoResponse();
            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.InserirAtualizarClienteProduto(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no InserirAtualizarClienteProduto e inseriu um produto"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirAtualizarClienteProduto - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ClienteProdutoResponse InserirClienteProdutoCompleto(ClienteProdutoRequest pRequest)
        {
            ClienteProdutoResponse lRetorno = new ClienteProdutoResponse();
            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.InserirClienteProdutoCompleto(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no InserirClienteProdutoCompleto e inseriu um produto"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirClienteProdutoCompleto - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ClienteSolicitacaoHistoricoResponse InserirClienteSolicitacaoHistorico(ClienteSolicitacaoHistoricoRequest pRequest)
        {
            ClienteSolicitacaoHistoricoResponse lRetorno = new ClienteSolicitacaoHistoricoResponse();
            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.InserirClienteSolicitacaoHistorico(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no InserirClienteSolicitacaoHistorico e inseriu um produto"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirClienteSolicitacaoHistorico - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ResgateResponse InserirAtualizarResgate(ResgateRequest pRequest)
        {
            ResgateResponse lRetorno = new ResgateResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.InserirAtualizarResgate(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no InserirAtualizarResgate e inseriu um produto"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirAtualizarResgate - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public AplicacaoResponse InserirAtualizarAplicacao(AplicacaoRequest pRequest)
        {
            AplicacaoResponse lRetorno = new AplicacaoResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.InserirAtualizarAplicacao(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no InserirAtualizarAplicacao e inseriu um produto"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirAtualizarAplicacao - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ClienteVencimentoResponse InserirClienteVencimento(ClienteVencimentoRequest pRequest)
        {
            ClienteVencimentoResponse lRetorno = new ClienteVencimentoResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.InserirClienteVencimento(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no InserirClienteVencimento e inseriu um cliente vencimento"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirClienteVencimento - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ClienteVencimentoHistoricoResponse InserirClienteVencimentoHistorico()
        {
            ClienteVencimentoHistoricoResponse lRetorno = new ClienteVencimentoHistoricoResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lDb.InserirClienteVencimentoHistorico();

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no InserirClienteVencimentoHistorico"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em InserirClienteVencimentoHistorico - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ResgateResponse SelecionarResgate(ResgateRequest pRequest)
        {
            ResgateResponse lRetorno = new ResgateResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.SelecionarResgate(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no SelecionarResgate para selecionar uma lista de Resgate"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarResgate - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public AplicacaoResponse SelecionarAplicacao(AplicacaoRequest pRequest)
        {
            AplicacaoResponse lRetorno = new AplicacaoResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.SelecionarAplicacao(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no SelecionarAplicacao para selecionar uma lista de aplicação"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarAplicacao - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ClienteProdutoResponse SelecionarClienteProduto(ClienteProdutoRequest pRequest)
        {
            ClienteProdutoResponse lRetorno = new ClienteProdutoResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.SelecionarClienteProduto(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no SelecionarClienteProduto para selecionar uma lista de Clientes com seus produtos"));
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarClienteProduto - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ProdutoResponse SelecionarProduto(ProdutoRequest pRequest)
        {
            ProdutoResponse lRetorno = new ProdutoResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.SelecionarProduto(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no SelecionarProduto para selecionar uma lista de produtos"));

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarProduto - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ProdutoClienteResponse SelecionarProdutoCliente(ProdutoClienteRequest pRequest)
        {
            ProdutoClienteResponse lRetorno = new ProdutoClienteResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.SelecionarProdutoCliente(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no SelecionarProdutoCliente para selecionar uma lista de produtos para uma cliente."));

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarProdutoCliente - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ProdutoClienteResponse SelecionarProdutoClienteOperador(ProdutoClienteRequest pRequest)
        {
            ProdutoClienteResponse lRetorno = new ProdutoClienteResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                gLogger.Info("Chek operador " + pRequest.ProdutoCliente.CheckOperador.ToString());

                lRetorno = lDb.SelecionarProdutoClienteOperador(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no SelecionarProdutoClienteOperador para selecionar uma lista de produtos para o operador que vai comprar os ativos."));

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarProdutoClienteOperador - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public StatusResponse SelecionarStatusAplicacaoResgate(StatusRequest pRequest)
        {
            StatusResponse lRetorno = new StatusResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.SelecionarStatusAplicacaoResgate(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no SelecionarStatusAplicacaoResgate para selecionar uma lista de Status para Aplicação e Resgate."));

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarStatusAplicacaoResgate - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ClienteVencimentoResponse SelecionarClienteVencimento(ClienteVencimentoRequest pRequest)
        {
            ClienteVencimentoResponse lRetorno = new ClienteVencimentoResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.SelecionarClienteVencimento(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                gLogger.Info(string.Concat("Entrou no SelecionarClienteVencimento para selecionar uma lista de cliente vencimento."));

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarClienteVencimento - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        public ExportarClientePoupeResponse ExportarClientePoupe(ExportarClientePoupeRequest pRequest)
        {
            ExportarClientePoupeResponse lResposta = new ExportarClientePoupeResponse();

            try
            {
                this.gLogger.Debug("Serviço de Exportação inciado.");

                lResposta = new PersistenciaDbExporcacaoClientePoupe().ExportarClientePoupe(new ExportarClientePoupeRequest());
            }
            catch (Exception ex)
            {
                lResposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResposta.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro ao ExportarClientePoupe - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }


            return lResposta;
        }

        public CustodiaValorizadaResponse SelecionarCustodiaValorizada(CustodiaValorizadaRequest pRequest)
        {
            CustodiaValorizadaResponse lRetorno = new CustodiaValorizadaResponse();

            try
            {
                PersistenciaDB lDb = new PersistenciaDB();

                lRetorno = lDb.SelecionarCustodiaValorizada(pRequest);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;

                

            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.ToString();

                gLogger.ErrorFormat("Erro em SelecionarCustodiaValorizada - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }


        public void InserirMovimentoCliente()
        {
            PersistenciaDB lDb = new PersistenciaDB();

            MovimentoCCSinacorResponse _movimentoCCSinacor;
            CompraETFResponse _CompraETFResponse;
            ClienteMovCCRequest _clienteMovCCRequest = new ClienteMovCCRequest();
            CustodiaValorizadaRequest _CustodiaValorizadaRequest;

            
            decimal ValorAplicacao = 0;
            decimal ValorCompra = 0;
            decimal ValorResquiso = 0;
            decimal ValorPrecoMedio = 0;
            DateTime dataTransacao = DateTime.Now;

            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = PersistenciaDB.ConexaoSQL;

            _AcessaDados.Conexao._ConnectionStringName = PersistenciaDB.ConexaoSQL;

            DbTransaction transacao;

            DbConnection ConnectionSql;

            ConnectionSql = _AcessaDados.Conexao.CreateIConnection();

            ConnectionSql.Open();

            transacao = ConnectionSql.BeginTransaction();


            try
            {
                ClienteProdutoResponse ClienteSemMovimentoMes = lDb.SelecionarClienteComVencimentoHoje(); //seleciona os clientes do poupe direct com data de movimento para hoje
                DateTime dataInicioVencimento = DateTime.MinValue;
                DateTime dataFimVencimento = DateTime.MinValue;
                decimal _CustoPapelHoje = 0;
                decimal _PercentualVariazao = 0;
                decimal _ValorPapelRentabilizado = 0;


                gLogger.InfoFormat("Quantidade de clientes com vencimento hoje - {0} .", ClienteSemMovimentoMes.ListaClienteProduto.Count);


                foreach (ClienteProdutoInfo clienteProduto in ClienteSemMovimentoMes.ListaClienteProduto)
                {
                    _movimentoCCSinacor = new MovimentoCCSinacorResponse();

                    dataFimVencimento       = clienteProduto.DtInicioTrocaPlano.Value.AddDays(-1); 
                    dataInicioVencimento    = clienteProduto.DtVencimento.Value.AddDays(-clienteProduto.QtdDiasVencimento);
                    
                    _movimentoCCSinacor = lDb.SelecionarMovimentoCliente(clienteProduto.CodigoClientePoupe, dataInicioVencimento, dataFimVencimento); //Seleciona do SINACOR movimento do mes corrente

                    _CompraETFResponse = lDb.SelecionarMovimentoCompraETF(clienteProduto.CodigoClientePoupe, dataInicioVencimento, dataFimVencimento, clienteProduto.CodigoAtivo); //Seleciona a compra de ETFs

                    if (_movimentoCCSinacor.ListaMovimentoCCSinacor.Count > 0 && _CompraETFResponse.ListaCompraETF.Count > 0)
                    {
                        gLogger.InfoFormat("Usuário - {0} - vai ser rentabilizado.", clienteProduto.CodigoClientePoupe);

                        ValorAplicacao = _movimentoCCSinacor.ListaMovimentoCCSinacor[0].ValorLancamento;
                        ValorCompra = _CompraETFResponse.ListaCompraETF[0].ValorLiquido < 0 ? _CompraETFResponse.ListaCompraETF[0].ValorLiquido * -1 : _CompraETFResponse.ListaCompraETF[0].ValorLiquido;
                        ValorResquiso = ValorAplicacao - ValorCompra;

                        ValorResquiso = ValorAplicacao - ValorCompra;

                        ValorPrecoMedio = lDb.ObterPrecoMedio(clienteProduto.CodigoClientePoupe, dataInicioVencimento, dataFimVencimento, clienteProduto.CodigoAtivo);

                        _clienteMovCCRequest.ClienteMovCC = new ClienteMovCCInfo();

                        
                        _clienteMovCCRequest.ClienteMovCC.CodigoClienteProduto  = clienteProduto.CodigoClienteProduto;
                        _clienteMovCCRequest.ClienteMovCC.CodigoCliente         = clienteProduto.CodigoCliente;
                        _clienteMovCCRequest.ClienteMovCC.ValorAplicacao        = ValorAplicacao;
                        _clienteMovCCRequest.ClienteMovCC.ValorConsumido        = ValorCompra;
                        _clienteMovCCRequest.ClienteMovCC.ValorResquicio        = ValorResquiso;
                        _clienteMovCCRequest.ClienteMovCC.ValorCorretagem       = _CompraETFResponse.ListaCompraETF[0].ValorCorretagem;//tenho que definir de onde vem
                        _clienteMovCCRequest.ClienteMovCC.DtSistema             = DateTime.Now;
                        _clienteMovCCRequest.ClienteMovCC.DtTransacao           = _CompraETFResponse.ListaCompraETF[0].DataNegocio;
                        _clienteMovCCRequest.ClienteMovCC.DescMovCC             = "Movimento Sinacor para POUPE DIRECT";

                        lDb.InserirAtualizarClienteMovCC(_clienteMovCCRequest, transacao, _AcessaDados); //inseri na tabela tb_cliente_mov_cc

                        _CustodiaValorizadaRequest = new CustodiaValorizadaRequest();
                        _CustodiaValorizadaRequest.CustodiaValorizada = new CustodiaValorizadaInfo();

                        _CustoPapelHoje = lDb.ObterPosicaoFechamentoCotacao(clienteProduto.CodigoAtivo);
                        _PercentualVariazao = ((_CustoPapelHoje - _CompraETFResponse.ListaCompraETF[0].ValorPapel) * 100) / _CompraETFResponse.ListaCompraETF[0].ValorPapel;
                                               

                        _ValorPapelRentabilizado = _CompraETFResponse.ListaCompraETF[0].QuantidadePapel * _CustoPapelHoje;


                        RentabilidadeInfo rentabilidade = lDb.RentabilizarCarteira(clienteProduto.CodigoCliente, clienteProduto.CodigoProduto, _ValorPapelRentabilizado, _PercentualVariazao);
                        
                        
                        _CustodiaValorizadaRequest.CustodiaValorizada.CodigoClienteProduto  = clienteProduto.CodigoClienteProduto;
                        _CustodiaValorizadaRequest.CustodiaValorizada.CodigoCliente         = clienteProduto.CodigoCliente;
                        _CustodiaValorizadaRequest.CustodiaValorizada.CodigoProduto         = clienteProduto.CodigoProduto;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorAplicacao        = ValorAplicacao;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorConsumido        = ValorCompra;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorResquicio        = ValorResquiso;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorCarteira         = rentabilidade.ValorTotalCarteira + ValorResquiso;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorCustoMedio       = ValorPrecoMedio; 
                        _CustodiaValorizadaRequest.CustodiaValorizada.DtRentabilizacao      = DateTime.Now;
                        _CustodiaValorizadaRequest.CustodiaValorizada.QtdTitulos            = _CompraETFResponse.ListaCompraETF[0].QuantidadePapel;
                        _CustodiaValorizadaRequest.CustodiaValorizada.PercentVariacao       = rentabilidade.PercentVariacaoTotal;
                        _CustodiaValorizadaRequest.CustodiaValorizada.CodigoAtivo           = clienteProduto.CodigoAtivo;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorAtivo            = _CompraETFResponse.ListaCompraETF[0].ValorPapel;

                        lDb.InserirAtualizarCustodiaValorizada(_CustodiaValorizadaRequest, transacao, _AcessaDados); //inseri na tabela TB_CUSTODIA_VALORIZADA




                        

                        
                    }

                    

                    

                    //zera as variáveis
                    ValorAplicacao  = 0;
                    ValorCompra     = 0;
                    ValorResquiso   = 0;
                }

                transacao.Commit();
            }

            catch (Exception ex)
            {
                transacao.Rollback();
                gLogger.ErrorFormat("Erro ao Pegar o movimento do cliente para o poupe - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
                
            }
            finally
            {
                ConnectionSql.Close();
                ConnectionSql.Dispose();
                ConnectionSql = null;
                transacao = null;
            }

        }


#region Inserir Rentabilidade retroativa

        public void InserirMovimentoClienteRetroativo()
        {
            PersistenciaDB lDb = new PersistenciaDB();

            MovimentoCCSinacorResponse _movimentoCCSinacor;
            CompraETFResponse _CompraETFResponse;
            ClienteMovCCRequest _clienteMovCCRequest = new ClienteMovCCRequest();
            CustodiaValorizadaRequest _CustodiaValorizadaRequest;


            decimal ValorAplicacao = 0;
            decimal ValorCompra = 0;
            decimal ValorResquiso = 0;
            decimal ValorPrecoMedio = 0;
            DateTime dataTransacao = DateTime.Now;

            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = PersistenciaDB.ConexaoSQL;

            _AcessaDados.Conexao._ConnectionStringName = PersistenciaDB.ConexaoSQL;

            DbTransaction transacao;

            DbConnection ConnectionSql;

            ConnectionSql = _AcessaDados.Conexao.CreateIConnection();

            ConnectionSql.Open();

            transacao = ConnectionSql.BeginTransaction();


            try
            {

                ClienteProdutoResponse ClienteSemMovimentoMes = lDb.SelecionarClienteRentabilidadeRetroativa(48054, new DateTime(2011, 11, 25)); //seleciona os clientes do poupe direct com data de movimento para hoje
                DateTime dataInicioVencimento = DateTime.MinValue;
                DateTime dataFimVencimento = DateTime.MinValue;
                decimal _CustoPapelHoje = 0;
                decimal _PercentualVariazao = 0;
                decimal _ValorPapelRentabilizado = 0;


                gLogger.InfoFormat("Quantidade de clientes com vencimento hoje - {0} .", ClienteSemMovimentoMes.ListaClienteProduto.Count);


                foreach (ClienteProdutoInfo clienteProduto in ClienteSemMovimentoMes.ListaClienteProduto)
                {
                    _movimentoCCSinacor = new MovimentoCCSinacorResponse();

                    dataFimVencimento = clienteProduto.DtInicioTrocaPlano.Value.AddDays(-1);
                    dataInicioVencimento = clienteProduto.DtVencimento.Value.AddDays(-clienteProduto.QtdDiasVencimento);

                    _movimentoCCSinacor = lDb.SelecionarMovimentoCliente(clienteProduto.CodigoClientePoupe, dataInicioVencimento, dataFimVencimento); //Seleciona do SINACOR movimento do mes corrente

                    _CompraETFResponse = lDb.SelecionarMovimentoCompraETF(clienteProduto.CodigoClientePoupe, dataInicioVencimento, dataFimVencimento, clienteProduto.CodigoAtivo); //Seleciona a compra de ETFs

                    if (_movimentoCCSinacor.ListaMovimentoCCSinacor.Count > 0 && _CompraETFResponse.ListaCompraETF.Count > 0)
                    {
                        gLogger.InfoFormat("Usuário - {0} - vai ser rentabilizado.", clienteProduto.CodigoClientePoupe);

                        ValorAplicacao = _movimentoCCSinacor.ListaMovimentoCCSinacor[0].ValorLancamento;
                        ValorCompra = _CompraETFResponse.ListaCompraETF[0].ValorLiquido < 0 ? _CompraETFResponse.ListaCompraETF[0].ValorLiquido * -1 : _CompraETFResponse.ListaCompraETF[0].ValorLiquido;
                        ValorResquiso = ValorAplicacao - ValorCompra;

                        ValorResquiso = ValorAplicacao - ValorCompra;

                        ValorPrecoMedio = lDb.ObterPrecoMedio(clienteProduto.CodigoClientePoupe, dataInicioVencimento, dataFimVencimento, clienteProduto.CodigoAtivo);

                        _clienteMovCCRequest.ClienteMovCC = new ClienteMovCCInfo();


                        _clienteMovCCRequest.ClienteMovCC.CodigoClienteProduto = clienteProduto.CodigoClienteProduto;
                        _clienteMovCCRequest.ClienteMovCC.CodigoCliente = clienteProduto.CodigoCliente;
                        _clienteMovCCRequest.ClienteMovCC.ValorAplicacao = ValorAplicacao;
                        _clienteMovCCRequest.ClienteMovCC.ValorConsumido = ValorCompra;
                        _clienteMovCCRequest.ClienteMovCC.ValorResquicio = ValorResquiso;
                        _clienteMovCCRequest.ClienteMovCC.ValorCorretagem = _CompraETFResponse.ListaCompraETF[0].ValorCorretagem;//tenho que definir de onde vem
                        _clienteMovCCRequest.ClienteMovCC.DtSistema = DateTime.Now;
                        _clienteMovCCRequest.ClienteMovCC.DtTransacao = _CompraETFResponse.ListaCompraETF[0].DataNegocio;
                        _clienteMovCCRequest.ClienteMovCC.DescMovCC = "Movimento Sinacor para POUPE DIRECT";

                        lDb.InserirAtualizarClienteMovCC(_clienteMovCCRequest, transacao, _AcessaDados); //inseri na tabela tb_cliente_mov_cc

                        _CustodiaValorizadaRequest = new CustodiaValorizadaRequest();
                        _CustodiaValorizadaRequest.CustodiaValorizada = new CustodiaValorizadaInfo();

                        _CustoPapelHoje = lDb.ObterPosicaoFechamentoCotacao(clienteProduto.CodigoAtivo);
                        _PercentualVariazao = ((_CustoPapelHoje - _CompraETFResponse.ListaCompraETF[0].ValorPapel) * 100) / _CompraETFResponse.ListaCompraETF[0].ValorPapel;


                        _ValorPapelRentabilizado = _CompraETFResponse.ListaCompraETF[0].QuantidadePapel * _CustoPapelHoje;


                        RentabilidadeInfo rentabilidade = lDb.RentabilizarCarteira(clienteProduto.CodigoCliente, clienteProduto.CodigoProduto, _ValorPapelRentabilizado, _PercentualVariazao);


                        _CustodiaValorizadaRequest.CustodiaValorizada.CodigoClienteProduto = clienteProduto.CodigoClienteProduto;
                        _CustodiaValorizadaRequest.CustodiaValorizada.CodigoCliente = clienteProduto.CodigoCliente;
                        _CustodiaValorizadaRequest.CustodiaValorizada.CodigoProduto = clienteProduto.CodigoProduto;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorAplicacao = ValorAplicacao;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorConsumido = ValorCompra;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorResquicio = ValorResquiso;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorCarteira = rentabilidade.ValorTotalCarteira + ValorResquiso;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorCustoMedio = ValorPrecoMedio;
                        _CustodiaValorizadaRequest.CustodiaValorizada.DtRentabilizacao = DateTime.Now;
                        _CustodiaValorizadaRequest.CustodiaValorizada.QtdTitulos = _CompraETFResponse.ListaCompraETF[0].QuantidadePapel;
                        _CustodiaValorizadaRequest.CustodiaValorizada.PercentVariacao = rentabilidade.PercentVariacaoTotal;
                        _CustodiaValorizadaRequest.CustodiaValorizada.CodigoAtivo = clienteProduto.CodigoAtivo;
                        _CustodiaValorizadaRequest.CustodiaValorizada.ValorAtivo = _CompraETFResponse.ListaCompraETF[0].ValorPapel;

                        lDb.InserirAtualizarCustodiaValorizada(_CustodiaValorizadaRequest, transacao, _AcessaDados); //inseri na tabela TB_CUSTODIA_VALORIZADA







                    }





                    //zera as variáveis
                    ValorAplicacao = 0;
                    ValorCompra = 0;
                    ValorResquiso = 0;
                }

                transacao.Commit();
            }

            catch (Exception ex)
            {
                transacao.Rollback();
                gLogger.ErrorFormat("Erro ao Pegar o movimento do cliente para o poupe - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);

            }
            finally
            {
                ConnectionSql.Close();
                ConnectionSql.Dispose();
                ConnectionSql = null;
                transacao = null;
            }

        }

#endregion


        #region Geração do Arquivo CCOU

        public void GerarArquivoCCOUPoupe()
        {
            try
            {
                 
                string lPath = ConfigurationManager.AppSettings["pathArquivoCobranca"].ToString();

                string lNomeArquivo = "CCOU" + DateTime.Now.ToString("MMdd") + ".DAT";


                List<string> lDetalhes = this.ListarLinhasArquivoCCOUPoupe();

                lock (gSingleton)
                {
                    StreamWriter lStream = new StreamWriter(Path.Combine(lPath, DateTime.Now.ToString("yyyy"), lNomeArquivo));

                    foreach (string linha in lDetalhes)
                        lStream.WriteLine(linha);

                    lStream.Close();

                    if (lDetalhes.Count > 0)
                        this.EnviaNotificacaoArquivoProntoImportacao();
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em GerarArquivo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }
        
        public List<string> ListarLinhasArquivoCCOUPoupe()
        {

            List<string> lRetorno = new List<string>();

            PersistenciaDB lDb = new PersistenciaDB();

            decimal valorDisponivelCC = 0;

            ClienteCCOResponse cliente = lDb.SelecionarClientesArquivoCCOU();

            if (cliente.ListaClienteCCOUInfo.Count > 0)
            {

                //Header do arquivo
                lRetorno.Add("00OUTROS  OUT".PadRight(250, ' '));

                foreach (ClienteCCOUInfo item in cliente.ListaClienteCCOUInfo)
                {
                    valorDisponivelCC = lDb.SelecionaValorDisponivel(item.CodigoClientePrincipal);

                    if (valorDisponivelCC >= item.ValorProduto) //Verifica se o cliente possui valor disponível em conta para débido do produto.
                    {
                        //escreve as linhas de dentro do arquivo
                        lRetorno.Add(this.MontaDetalheArquivoCodigoPrincipal(item));
                        lRetorno.Add(this.MontaDetalheArquivoCodigoPoupe(item));
                    }
                }
                                

                //Trailer do arquivo
                lRetorno.Add("99OUTROSOUT".PadRight(250, ' '));
            }

            return lRetorno;
        }
        
        private string MontaDetalheArquivoCodigoPrincipal(ClienteCCOUInfo Cliente)
        {
            StringBuilder lDetalhe = new StringBuilder();

            PersistenciaDB lDb = new PersistenciaDB();

            DateTime dataArquivo = lDb.RetornarDiaUtil();

            lDetalhe.Append("01");                                                              //-- Tipo de registro FIXO '01'                                                                  

            lDetalhe.Append(dataArquivo.ToString("dd/MM/yyyy"));                               //-- Data vencimento dd/mm/yyyy                                                                  

            lDetalhe.Append(Cliente.CodigoClientePrincipal.ToString().PadLeft(7, '0'));         //-- Código do cliente '7'                                                                  

            lDetalhe.Append("1019");                                                            //-- Histórico: está com 1019 APLICAÇÃO POUPE GRADUAL.
            //Preciso do preço
            lDetalhe.Append(Cliente.ValorProduto.ToString().Replace(".","").Replace(",","").PadLeft(15, '0'));                  //-- Lançamento

            lDetalhe.Append(string.Empty.PadLeft(94, ' '));

            lDetalhe.Append(string.Empty.PadLeft(95, ' '));

            lDetalhe.Append("OUTNOUT 000000000000000");

            return lDetalhe.ToString();
        }

        private string MontaDetalheArquivoCodigoPoupe(ClienteCCOUInfo Cliente)
        {
            StringBuilder lDetalhe = new StringBuilder();

            PersistenciaDB lDb = new PersistenciaDB();

            DateTime dataArquivo = lDb.RetornarDiaUtil();

            lDetalhe.Append("01");                                                              //-- Tipo de registro FIXO '01'                                                                  

            lDetalhe.Append(dataArquivo.ToString("dd/MM/yyyy"));                               //-- Data vencimento dd/mm/yyyy                                                                  

            lDetalhe.Append(Cliente.CodigoClientePoupe.ToString().PadLeft(7, '0'));             //-- Código do cliente '7'                                                                  

            lDetalhe.Append("1021");                                                            //-- Histórico: está com 1021 Descrição aplicação POUPE GRADUAL
            //Preciso do preço
            lDetalhe.Append(Cliente.ValorProduto.ToString().Replace(".", "").Replace(",", "").PadLeft(15, '0'));                  //-- Lançamento

            lDetalhe.Append(string.Empty.PadLeft(94, ' '));

            lDetalhe.Append(string.Empty.PadLeft(95, ' '));

            lDetalhe.Append("OUTNOUT 000000000000000");

            return lDetalhe.ToString();
        }

        public void EnviaNotificacaoArquivoProntoImportacao()
        {
            var lServico = Ativador.Get<IServicoEmail>();

            try
            {
                StreamReader lStream = new StreamReader(Path.Combine(ConfigurationManager.AppSettings["pathArquivoEmailAviso"].ToString(), "NotificacaoGeracaoArquivo.txt"));

                string lCorpoEmail = lStream.ReadToEnd();

                string lNomeArquivo = "CCOU" + DateTime.Now.ToString("MMdd") + ".DAT";

                string lPathArquivoCobranca = Path.Combine(ConfigurationManager.AppSettings["pathArquivoCobranca"].ToString(), DateTime.Now.ToString("yyyy"), lNomeArquivo);

                var lEmail = new EnviarEmailRequest();

                lEmail.Objeto = new EmailInfo();

                lEmail.Objeto.Assunto = "Notificação de arquivo pronto para importação";

                lEmail.Objeto.Destinatarios = new List<string>() { ConfigurationManager.AppSettings["EmailDestinatarioNotificacaoArquivo"].ToString() };

                lEmail.Objeto.Remetente = ConfigurationManager.AppSettings["EmailRemetenteNotificacaoArquivo"].ToString();

                lEmail.Objeto.CorpoMensagem = lCorpoEmail.Replace("##ARQUIVO##", lNomeArquivo);

                EmailAnexoInfo lAnexo = new EmailAnexoInfo()
                {
                    Arquivo = StreamArquivoImportacao(lPathArquivoCobranca),
                    Nome = lNomeArquivo
                };

                lEmail.Objeto.Anexos = new List<EmailAnexoInfo>() { lAnexo };

                gLogger.InfoFormat("Entrou no método de EnviarEmailAviso");

                EnviarEmailResponse lResponse = lServico.Enviar(lEmail);

                if (lResponse.StatusResposta.Equals(MensagemResponseStatusEnum.OK))
                {
                    gLogger.Info("Email disparado com sucesso");
                }
                else
                {
                    gLogger.ErrorFormat("O e-mail não foi disparado - Descrição: {0} ", lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro no método de EnviarEmailAviso - Descrição: {0} - Stacktrace: {1}", ex.Message, ex.StackTrace);
            }
        }

        private byte[] StreamArquivoImportacao(string pNomeArquivo)
        {
            FileStream lStream = new FileStream(pNomeArquivo, FileMode.Open, FileAccess.Read);

            byte[] arrBytes = new byte[lStream.Length];

            lStream.Read(arrBytes, 0, (int)lStream.Length);

            lStream.Close();

            return arrBytes;
        }

        #endregion



        #endregion
    }
}



