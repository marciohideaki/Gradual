using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Timers;
using System.Text;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Globalization;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.ContaCorrente;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.Custodia;
using Gradual.OMS.Custodia.Lib;
using Gradual.OMS.Custodia.Lib.Mensageria;
using Gradual.OMS.Custodia.Lib.Info;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Risco.Custodia;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.CarteiraRecomendada.lib;
using Gradual.OMS.CarteiraRecomendada.lib.Mensageria;
using log4net;

namespace Gradual.OMS.CarteiraRecomendada
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoCarteiraRecomendada : IServicoControlavel, IServicoCarteiraRecomendada
    {
        #region Constantes

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string SUCESSO = "SOLICITACAO ENVIADA COM SUCESSO.";
        public const string ERROPROGRAMA = "OCORREU UM ERRO AO ENVIAR A SOLICITACAO.";

        public string CodigoPortaClienteAssessor
        {
            get
            {
                return ConfigurationManager.AppSettings["CodigoPortaClienteAssessor"].ToString();
            }
        }

        #endregion

        static private Hashtable _emailsClientes = new Hashtable();

        private ServicoStatus _status = ServicoStatus.Parado;
        private bool _bKeepRunning = false;
        private System.Timers.Timer _timer = new System.Timers.Timer();
        private DateTime _proximaAtualizacaoEmails = DateTime.Now;
        private string _horaAtualizacaoEmails = null;
        private string _emailRemetenteNotificacao = null;
        private string _arquivoEmailClienteAderido = null;
        private string _arquivoEmailClienteRenovado = null;
        private string _arquivoEmailClienteCancelado = null;
        private string _arquivoEmailRenovacao = null;
        private string _mockEmail = null;
        private bool _ignoraRisco = false;

        private class SaldoInfo
        {
            public decimal valorSaldo { get; set; }
            public string descricaoSaldo { get; set; }
        }

        public ServicoCarteiraRecomendada()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region IServicoControlavel Members
        /// <summary>
        /// 
        /// </summary>
        public void IniciarServico()
        {
            logger.Info("Iniciando Servico de Carteira Recomendada.....");

            _horaAtualizacaoEmails = ConfigurationManager.AppSettings["AtualizacaoEmails"].ToString();
            _emailRemetenteNotificacao = ConfigurationManager.AppSettings["EmailRemetenteNotificacao"].ToString();
            _arquivoEmailClienteAderido = ConfigurationManager.AppSettings["ArquivoEmailClienteAderido"].ToString();
            _arquivoEmailClienteRenovado = ConfigurationManager.AppSettings["ArquivoEmailClienteRenovado"].ToString();
            _arquivoEmailClienteCancelado = ConfigurationManager.AppSettings["ArquivoEmailClienteCancelado"].ToString();
            _arquivoEmailRenovacao = ConfigurationManager.AppSettings["ArquivoEmailRenovacao"].ToString();

            if (ConfigurationManager.AppSettings["MockEmail"] != null)
                _mockEmail = ConfigurationManager.AppSettings["MockEmail"].ToString();

            if (ConfigurationManager.AppSettings["IgnoraRisco"] != null)
                _ignoraRisco = (ConfigurationManager.AppSettings["IgnoraRisco"].ToString().Equals("true") ? true : false);

            logger.Info("Carregamento inicial dos clientes que possuem email.....");
            AtualizarEmailsClientes();

            _proximaAtualizacaoEmails = DateTime.Parse(
                DateTime.Now.AddDays(1).ToString("yyyy/MM/dd") + " " + _horaAtualizacaoEmails,
                new CultureInfo("pt-BR", false));

            TimeSpan intervaloAtualizacao = _proximaAtualizacaoEmails - DateTime.Now;
            _timer.Interval = intervaloAtualizacao.TotalMilliseconds;
            _timer.Elapsed += new ElapsedEventHandler(OnTimer);
            _timer.Enabled = true;

            logger.Info("Proxima atualizacao dos emails: " + _proximaAtualizacaoEmails.ToString());

            _bKeepRunning = true;
            _status = ServicoStatus.EmExecucao;

            logger.Info("Servico de Carteira Recomendada iniciado!");
        }

        /// <summary>
        /// 
        /// </summary>
        public void PararServico()
        {
            logger.Info("Finalizando Servico de Carteira Recomendada....");

            // Para o monitor de canais e sinaliza para terminar a thread
            _bKeepRunning = false;

            _status = ServicoStatus.Parado;

            logger.Info("Servico de Carteira Recomendada finalizado!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }
        #endregion //IServicoControlavel Members

        #region CarteiraRecomendada Members

        /// <summary>
        /// Listar Carteira Recomendada.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ListarResponse SolicitarLista()
        {
            ListarResponse response = new ListarResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de lista de Carteira Recomendada");
                logger.Info("Tipo de solicitação: Lista");

                logger.Info("Chamando o método Lista() para efetuar a transacao no banco de dados");
                response = persistenciaCarteiraRecomendada.Lista();
                logger.Info("Transacao efetuada com sucesso");

                if (response.Lista.Count == 0)
                {
                    response.bSucesso = false;
                    response.DescricaoResposta = "Nenhuma Carteira Recomendada encontrada";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.bSucesso = true;
                    response.DescricaoResposta = response.Lista.Count.ToString() + " carteira(s) recomendada(s) encontrada(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna a resposta");
            return response;
        }

        /// <summary>
        /// Listar composição da Carteira Recomendada informada.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ListarComposicaoResponse SolicitarListaComposicao(ListarComposicaoRequest request)
        {
            ListarComposicaoResponse response = new ListarComposicaoResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de lista de Composicao de Carteira Recomendada");
                logger.Info("Id da Carteira: " + request.idCarteiraRecomendada.ToString());
                logger.Info("Tipo de solicitação: Lista");

                logger.Info("Chamando o método ListaComposicao() para efetuar a transacao no banco de dados");
                response = persistenciaCarteiraRecomendada.ListaComposicao(request);
                logger.Info("Transacao efetuada com sucesso");

                if (response.listaComposicao.Count == 0)
                {
                    response.bSucesso = false;
                    response.DescricaoResposta = "Nenhuma ativo encontrado na composição da Carteira Recomendada";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.bSucesso = true;
                    response.DescricaoResposta = response.listaComposicao.Count.ToString() + " ativo(s) encontrado(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna a resposta");
            return response;
        }

        /// <summary>
        /// Listar composição da Carteira Recomendada Atual e Anterior.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ListarComposicaoClienteResponse SolicitarListaComposicaoCliente(ListarComposicaoClienteRequest request)
        {
            ListarComposicaoClienteResponse response = new ListarComposicaoClienteResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de lista de Composicao de Carteira Recomendada do Cliente");
                logger.Info("Id do Cliente: " + request.idCliente.ToString());
                logger.Info("Id da Carteira: " + request.idCarteiraRecomendada.ToString());
                logger.Info("Id da Produto: " + request.idProduto.ToString());
                logger.Info("Tipo de solicitação: Lista");

                logger.Info("Chamando o método ListaComposicaoCliente() para efetuar a transacao no banco de dados");
                response = persistenciaCarteiraRecomendada.ListaComposicaoCliente(request);
                logger.Info("Transacao efetuada com sucesso");

                if (response.listaComposicaoNova.Count == 0)
                {
                    response.bSucesso = false;
                    response.DescricaoResposta = "Nenhuma ativo encontrado na composição da Carteira Recomendada Atual";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.bSucesso = true;
                    response.DescricaoResposta = (response.listaComposicaoNova.Count + response.listaComposicaoAtual.Count).ToString() + " ativo(s) encontrado(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna a resposta");
            return response;
        }

        /// <summary>
        /// Inclusao de uma nova carteira recomendada.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public InserirResponse SolicitarInclusao(InserirRequest request)
        {
            InserirResponse response = new InserirResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de inclusao de carteira recomendada e sua composicao");
                logger.Info("Descrição: " + request.carteiraRecomendada.DsCarteira);
                logger.Info("Tipo de solicitação: Inclusão");

                logger.Info("Chamando o método Inclusao() para efetuar a transacao no banco de dados");
                bool bTransacao = persistenciaCarteiraRecomendada.Inclusao(request);
                logger.Info("Transacao efetuada com sucesso");

                response.bSucesso = bTransacao;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;
                response.DescricaoResposta = SUCESSO;
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna resposta");
            return response;
        }

        /// <summary>
        /// Alteracao de uma carteira recomendada existente.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AlterarResponse SolicitarAlteracao(AlterarRequest request)
        {
            AlterarResponse response = new AlterarResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de alteracao de carteira recomendada");
                logger.Info("Descrição: " + request.DsCarteira);
                logger.Info("Tipo de solicitação: Alteração");

                logger.Info("Chamando o método Alteracao() para efetuar a transacao no banco de dados");
                bool bTransacao = persistenciaCarteiraRecomendada.Alteracao(request);
                logger.Info("Transacao efetuada com sucesso");

                response.bSucesso = bTransacao;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;
                response.DescricaoResposta = SUCESSO;
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna resposta");
            return response;
        }

        /// <summary>
        /// Renovar composicao atual de uma carteira recomendada.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RenovarResponse SolicitarRenovacao(RenovarRequest request)
        {
            RenovarResponse response = new RenovarResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de renovação de carteira recomendada");
                logger.Info("Descrição: " + request.dsRenovacao);
                logger.Info("Tipo de solicitação: Renovação");

                logger.Info("Chamando o método Renovacao() para efetuar a transacao no banco de dados");
                int idRenovacao = persistenciaCarteiraRecomendada.Renovacao(request);
                if (idRenovacao == 0)
                {
                    response.bSucesso = false;
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                    response.DescricaoResposta = "Não foi possível efetuar a renovação da Carteira Recomendada";
                }
                else
                {
                    logger.Info("Transacao efetuada com sucesso");

                    logger.Info("Enviando notificações aos clientes de Carteira Recomendada");
                    EnviarNotificacaoRequest requestServico = new EnviarNotificacaoRequest();
                    requestServico.IdCarteiraRecomendada = request.idCarteiraRecomendada;
                    requestServico.IdRenovacao = idRenovacao;
                    EnviarNotificacaoResponse responseServico = EnviarNotificacaoRenovacao(requestServico);
                    if (responseServico.bSucesso)
                    {
                        response.bSucesso = true;
                        response.DataResposta = DateTime.Now;
                        response.CriticaResposta = StatusRespostaEnum.Sucesso;
                        response.DescricaoResposta = SUCESSO;
                    }
                    else
                    {
                        response.bSucesso = false;
                        response.DataResposta = DateTime.Now;
                        response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                        response.DescricaoResposta = "Não foi possível notificar por e-mail os clientes da renovação da Carteira Recomendada";
                    }
                }
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna resposta");
            return response;
        }

        /// <summary>
        /// Adesao do cliente a uma Carteira Recomendada (associada ao produto informado)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AdesaoClienteResponse SolicitarAdesaoCliente(AdesaoClienteRequest request)
        {
            int idCarteiraRecomendada = 0;
            AdesaoClienteResponse response = new AdesaoClienteResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de adesão de cliente a carteira recomendada");
                logger.Info("Código do Cliente: " + request.IdCliente.ToString());
                logger.Info("Código do Produto: " + request.IdProduto.ToString());
                logger.Info("Tipo de solicitação: Inclusão");

                // Verifica se o cliente não pode aderir ao produto solicitado
                logger.Info("Verificando se Cliente pode aderir ao produto");
                bool permiteAdesao = false;
                ListarClienteRequest listarClienteRequest = new ListarClienteRequest();
                listarClienteRequest.IdCliente = request.IdCliente;
                ListarClienteResponse listarClienteResponse = persistenciaCarteiraRecomendada.ListaCliente(listarClienteRequest);
                if (listarClienteResponse.lista.Count != 0)
                {
                    foreach (CarteiraRecomendadaClienteInfo lista in listarClienteResponse.lista)
                    {
                        if (lista.IdProduto == request.IdProduto)
                        {
                            if (lista.PermiteAdesao)
                                permiteAdesao = true;
                            break;
                        }
                    }
                }
                if (!permiteAdesao)
                {
                    response.bSucesso = false;
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                    response.DescricaoResposta = "Cliente não pode efetuar adesão ao produto solicitado";
                    return response;
                }

                // Efetua o cálculo de Risco do Cliente
                if (!_ignoraRisco)
                {
                    logger.Info("Avaliando Risco do Cliente");
                    response = CalcularRiscoAdesaoCliente(request);
                    if (response.bSucesso == false)
                        return response;
                }

                logger.Info("Efetuando ordens de compra dos ativos da Carteira Recomendada para o Roteador");
                response = ExecutarOrdensAdesao(request);
                if (response.bSucesso == false)
                    return response;

                logger.Info("Preenchendo a classe ClienteProdutoInfo");
                ClienteProdutoInfo clienteProdutoInfo = new ClienteProdutoInfo();
                clienteProdutoInfo.IdCliente = request.IdCliente;
                clienteProdutoInfo.IdProduto = request.IdProduto;
                clienteProdutoInfo.StAtivo = 'S';
                clienteProdutoInfo.IP = request.IP;
                clienteProdutoInfo.Descricao = "Cliente[" + request.IdCliente.ToString() + "] aderiu ao Produto[" + request.IdProduto.ToString() + "]";

                logger.Info("Chamando o método AdesaoCliente() para efetuar a transacao no banco de dados");
                idCarteiraRecomendada = persistenciaCarteiraRecomendada.AdesaoCliente(clienteProdutoInfo);
                logger.Info("Transacao efetuada com sucesso");

                logger.Info("Enviando notificação de adesão a Carteira Recomendada");
                EnviarNotificacaoRequest requestServico = new EnviarNotificacaoRequest();
                requestServico.IdCarteiraRecomendada = idCarteiraRecomendada;
                requestServico.IdCliente = request.IdCliente;
                EnviarNotificacaoResponse responseServico = EnviarNotificacaoClienteAderido(requestServico);

                response.bSucesso = true;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;

                if (responseServico.bSucesso)
                {
                    response.DescricaoResposta = SUCESSO;
                }
                else
                {
                    response.DescricaoResposta = "Não foi possível notificar por e-mail a adesão do cliente à Carteira Recomendada";
                    logger.Error("Não foi possível notificar por e-mail a adesão do cliente à Carteira Recomendada");
                }
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna resposta");
            return response;
        }

        /// <summary>
        /// Renovacao do cliente a uma Carteira Recomendada que foi atualizada (associada ao produto informado)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RenovarClienteResponse SolicitarRenovacaoCliente(RenovarClienteRequest request)
        {
            int idCarteiraRecomendada = 0;
            RenovarClienteResponse response = new RenovarClienteResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de renovação de cliente a carteira recomendada");
                logger.Info("Código do Cliente: " + request.IdCliente.ToString());
                logger.Info("Código do Produto: " + request.IdProduto.ToString());
                logger.Info("Tipo de solicitação: Renovação");

                // Verifica se o cliente não pode renovar o produto solicitado
                logger.Info("Verificando se Cliente pode renovar o produto");
                bool permiteRenovacao = false;
                ListarClienteRequest listarClienteRequest = new ListarClienteRequest();
                listarClienteRequest.IdCliente = request.IdCliente;
                ListarClienteResponse listarClienteResponse = persistenciaCarteiraRecomendada.ListaCliente(listarClienteRequest);
                if (listarClienteResponse.lista.Count != 0)
                {
                    foreach (CarteiraRecomendadaClienteInfo lista in listarClienteResponse.lista)
                    {
                        if (lista.IdProduto == request.IdProduto)
                        {
                            if (lista.PermiteRenovacao)
                                permiteRenovacao = true;
                            break;
                        }
                    }
                }
                if (!permiteRenovacao)
                {
                    response.DataResposta = DateTime.Now;
                    response.bSucesso = false;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                    response.DescricaoResposta = "Cliente não pode efetuar renovação do produto solicitado";
                    return response;
                }

                // Efetua o cálculo de Risco do Cliente
                if (!_ignoraRisco)
                {
                    logger.Info("Avaliando Risco do Cliente");
                    response = CalcularRiscoRenovacaoCliente(request);
                    if (response.bSucesso == false)
                        return response;
                }

                logger.Info("Efetuando ordens de compra e venda dos ativos da Carteira Recomendada para o Roteador");
                response = ExecutarOrdensRenovacao(request);
                if (response.bSucesso == false)
                    return response;

                logger.Info("Preenchendo a classe ClienteProdutoInfo");
                ClienteProdutoInfo clienteProdutoInfo = new ClienteProdutoInfo();
                clienteProdutoInfo.IdCliente = request.IdCliente;
                clienteProdutoInfo.IdProduto = request.IdProduto;
                clienteProdutoInfo.StAtivo = 'S';
                clienteProdutoInfo.IP = request.IP;

                logger.Info("Chamando o método RenovacaoCliente() para efetuar a transacao no banco de dados");
                idCarteiraRecomendada = persistenciaCarteiraRecomendada.RenovacaoCliente(clienteProdutoInfo);
                logger.Info("Transacao efetuada com sucesso");

                logger.Info("Enviando notificação de renovação a Carteira Recomendada");
                EnviarNotificacaoRequest requestServico = new EnviarNotificacaoRequest();
                requestServico.IdCarteiraRecomendada = idCarteiraRecomendada;
                requestServico.IdCliente = request.IdCliente;
                EnviarNotificacaoResponse responseServico = EnviarNotificacaoClienteRenovado(requestServico);

                response.bSucesso = true;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;

                if (responseServico.bSucesso)
                {
                    response.DescricaoResposta = SUCESSO;
                }
                else
                {
                    response.DescricaoResposta = "Não foi possível notificar por e-mail a renovação do cliente à Carteira Recomendada";
                    logger.Error("Não foi possível notificar por e-mail a renovação do cliente à Carteira Recomendada");
                }
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna resposta");
            return response;
        }

        /// <summary>
        /// Cancelamento de carteira recomendada
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CancelarResponse SolicitarCancelamento(CancelarRequest request)
        {
            int idCarteiraRecomendada = 0;
            CancelarResponse response = new CancelarResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de cancelamento de cliente de carteira recomendada");
                logger.Info("Código do cliente: " + request.IdCliente.ToString());
                logger.Info("Código do produto: " + request.IdProduto.ToString());
                logger.Info("Tipo de solicitação: Cancelamento");

                // Verifica se o cliente já possui adesão do produto solicitado
                logger.Info("Verificando se Cliente já possui adesão ao produto");
                bool aderido = false;
                ListarClienteRequest listarClienteRequest = new ListarClienteRequest();
                listarClienteRequest.IdCliente = request.IdCliente;
                ListarClienteResponse listarClienteResponse = persistenciaCarteiraRecomendada.ListaCliente(listarClienteRequest);
                if (listarClienteResponse.lista.Count != 0)
                {
                    foreach (CarteiraRecomendadaClienteInfo lista in listarClienteResponse.lista)
                    {
                        if (lista.IdProduto == request.IdProduto && lista.PermiteAdesao == false)
                        {
                            aderido = true;
                            break;
                        }
                    }
                }
                if (!aderido)
                {
                    response.bSucesso = false;
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                    response.DescricaoResposta = "Cliente não possui o produto solicitado para cancelamento";
                    return response;
                }

                logger.Info("Preenchendo a classe ClienteProdutoInfo");
                ClienteProdutoInfo clienteProdutoInfo = new ClienteProdutoInfo();
                clienteProdutoInfo.IdCliente = request.IdCliente;
                clienteProdutoInfo.IdProduto = request.IdProduto;
                clienteProdutoInfo.StAtivo = 'N';
                clienteProdutoInfo.IP = request.IP;
                clienteProdutoInfo.Descricao = "Cliente[" + request.IdCliente.ToString() + "] teve cancelado adesão ao Produto[" + request.IdProduto.ToString() + "]";

                logger.Info("Chamando o método Cancelamento() para efetuar a transacao no banco de dados");
                idCarteiraRecomendada = persistenciaCarteiraRecomendada.Cancelamento(clienteProdutoInfo);
                logger.Info("Transacao efetuada com sucesso");

                logger.Info("Enviando notificação de cancelamento da Carteira Recomendada");
                EnviarNotificacaoRequest requestServico = new EnviarNotificacaoRequest();
                requestServico.IdCarteiraRecomendada = idCarteiraRecomendada;
                requestServico.IdCliente = request.IdCliente;
                EnviarNotificacaoResponse responseServico = EnviarNotificacaoClienteCancelado(requestServico);

                response.bSucesso = true;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;

                if (responseServico.bSucesso)
                {
                    response.DescricaoResposta = SUCESSO;
                }
                else
                {
                    response.DescricaoResposta = "Não foi possível notificar por e-mail o cancelamento do cliente à Carteira Recomendada";
                    logger.Error("Não foi possível notificar por e-mail o cancelamento do cliente à Carteira Recomendada");
                }
            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna resposta");
            return response;
        }

        /// <summary>
        /// Listar as Carteiras Recomendadas do cliente, e as disponíveis para o cliente aderir ou renovar.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ListarClienteResponse SolicitarListaCliente(ListarClienteRequest request)
        {
            ListarClienteResponse response = new ListarClienteResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de lista de Carteira Recomendada para o cliente");
                logger.Info("Código do Cliente: " + request.IdCliente.ToString());
                logger.Info("Tipo de solicitação: Lista");

                logger.Info("Chamando o método ListaCliente() para efetuar a transacao no banco de dados");
                response = persistenciaCarteiraRecomendada.ListaCliente(request);
                logger.Info("Transacao efetuada com sucesso");

                if (response.lista.Count == 0)
                {
                    response.bSucesso = false;
                    response.DescricaoResposta = "Nenhuma Carteira Recomendada encontrada";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.bSucesso = true;
                    response.DescricaoResposta = response.lista.Count.ToString() + " Carteira(s) Recomendada(s) encontrada(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna a resposta");
            return response;
        }

        /// <summary>
        /// Listar todos os assessores existentes no Sinacor.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ListarAssessoresResponse SolicitarListaAssessores()
        {
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();
            ListarAssessoresResponse response = new ListarAssessoresResponse();

            try
            {
                logger.Info("Preparando solicitação de lista de Assessores");
                logger.Info("Tipo de solicitação: Lista");

                logger.Info("Chamando o método ListaAssessores() para efetuar a transacao no banco de dados");
                response = persistenciaCarteiraRecomendada.ListaAssessores();
                logger.Info("Transacao efetuada com sucesso");

                if (response.Lista.Count == 0)
                {
                    response.bSucesso = false;
                    response.DescricaoResposta = "Nenhum Assessor encontrado";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.bSucesso = true;
                    response.DescricaoResposta = response.Lista.Count.ToString() + " assessor(es) encontrado(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna a resposta");
            return response;
        }

        /// <summary>
        /// Listar clientes para acompanhamento pela Intranet das Carteiras Recomendadas.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ListarAcompanhamentoResponse SolicitarListaAcompanhamento(ListarAcompanhamentoRequest request)
        {
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();
            ListarAcompanhamentoResponse response = new ListarAcompanhamentoResponse();

            try
            {
                logger.Info("Preparando solicitação de lista de Acompanhamento");
                logger.Info("Tipo de solicitação: Lista");

                logger.Info("Chamando o método ListaAcompanhamento() para efetuar a transacao no banco de dados");
                response = persistenciaCarteiraRecomendada.ListaAcompanhamento(request);
                logger.Info("Transacao efetuada com sucesso");

                if (response.Lista.Count == 0)
                {
                    response.bSucesso = false;
                    response.DescricaoResposta = "Nenhum Acompanhamento encontrado";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.bSucesso = true;
                    response.DescricaoResposta = response.Lista.Count.ToString() + " Acompanhamento(s) encontrado(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna a resposta");
            return response;
        }

        /// <summary>
        /// Listar todas as ordens enviadas para o cliente e carteira recomendada informados.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OrdensEnviadasResponse SolicitarListaDetalhesAcompanhamento(OrdensEnviadasRequest request)
        {
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();
            OrdensEnviadasResponse response = new OrdensEnviadasResponse();

            try
            {
                logger.Info("Preparando solicitação de lista de Ordens Enviadas");
                logger.Info("Tipo de solicitação: Lista");

                logger.Info("Chamando o método ListaDetalhesAcompanhamento() para efetuar a transacao no banco de dados");
                response = persistenciaCarteiraRecomendada.ListaDetalhesAcompanhamento(request);
                logger.Info("Transacao efetuada com sucesso");

                if (response.Lista.Count == 0)
                {
                    response.bSucesso = false;
                    response.DescricaoResposta = "Nenhuma ordem encontrada";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.bSucesso = true;
                    response.DescricaoResposta = response.Lista.Count.ToString() + " ordem(ns) encontrada(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna a resposta");
            return response;
        }

        #endregion // CarteiraRecomendada Members

        #region Ordens

        /// <summary>
        /// Obtém os ativos da Carteira Recomendada e envia as ordens de compra para o roteador.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private AdesaoClienteResponse ExecutarOrdensAdesao(AdesaoClienteRequest request)
        {
            AdesaoClienteResponse response = new AdesaoClienteResponse();
            try
            {
                ListarComposicaoRequest requestComposicao = new ListarComposicaoRequest();
                requestComposicao.idProduto = request.IdProduto;

                ListarComposicaoResponse responseComposicao = SolicitarListaComposicao(requestComposicao);

                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicao)
                    EnviarOrdem(request.IdCliente, composicao, OrdemDirecaoEnum.Compra);
            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.bSucesso = false;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.Exception = ex;
                response.DescricaoResposta = ex.Message;
                return response;
            }

            response.DataResposta = DateTime.Now;
            response.bSucesso = true;
            response.CriticaResposta = StatusRespostaEnum.Sucesso;
            response.DescricaoResposta = SUCESSO;
            return response;
        }

        /// <summary>
        /// Obtém os ativos da Renovação da Carteira Recomendada e envia as ordens de compra (ativos adicionados na carteira) e 
        /// venda (ativos removidos da carteira) para o roteador.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private RenovarClienteResponse ExecutarOrdensRenovacao(RenovarClienteRequest request)
        {
            RenovarClienteResponse response = new RenovarClienteResponse();
            try
            {
                ListarComposicaoClienteRequest requestComposicao = new ListarComposicaoClienteRequest();
                requestComposicao.idCliente = request.IdCliente;
                requestComposicao.idProduto = request.IdProduto;

                ListarComposicaoClienteResponse responseComposicao = SolicitarListaComposicaoCliente(requestComposicao);

                Hashtable listaAtual = new Hashtable();
                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicaoAtual)
                    listaAtual.Add(composicao.IdAtivo, composicao);

                Hashtable listaNova = new Hashtable();
                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicaoNova)
                    listaNova.Add(composicao.IdAtivo, composicao);

                // Se o ativo da composicao nova não existir na composição atual, envia ordem de compra do ativo
                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicaoNova)
                {
                    if (!listaAtual.ContainsKey(composicao.IdAtivo))
                        EnviarOrdem(request.IdCliente, composicao, OrdemDirecaoEnum.Compra);
                }

                // Se o ativo da composicao atual não existir na composição nova, envia ordem de venda do ativo
                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicaoAtual)
                {
                    if (!listaNova.ContainsKey(composicao.IdAtivo))
                        EnviarOrdem(request.IdCliente, composicao, OrdemDirecaoEnum.Venda);
                }

            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.bSucesso = false;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.Exception = ex;
                response.DescricaoResposta = ex.Message;
                return response;
            }

            response.DataResposta = DateTime.Now;
            response.bSucesso = true;
            response.CriticaResposta = StatusRespostaEnum.Sucesso;
            response.DescricaoResposta = SUCESSO;
            return response;
        }

        private void EnviarOrdem(int idCliente, CarteiraRecomendadaComposicaoInfo ativo, OrdemDirecaoEnum compraVenda)
        {
            try
            {
                EnviarOrdemRequest requestOrdem = new EnviarOrdemRequest
                {
                    ClienteOrdemInfo = new ClienteOrdemInfo
                    {
                        CodigoCliente = idCliente,
                        DataHoraSolicitacao = DateTime.Now,
                        DataValidade = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59),
                        DirecaoOrdem = compraVenda,
                        Instrumento = ativo.IdAtivo,
                        PortaControleOrdem = CodigoPortaClienteAssessor,
                        Quantidade = ativo.Quantidade,
                        TipoDeOrdem = OrdemTipoEnum.MarketWithLeftOverLimit,
                        ValidadeOrdem = OrdemValidadeEnum.ValidaParaODia
                    }
                };

                CarteiraRecomendadaOrdens ordens = new CarteiraRecomendadaOrdens();
                EnviarOrdemResponse responseOrdem =
                    ordens.EnviarOrdem(requestOrdem, ativo.IdCarteiraRecomendada.ToString());

                if (responseOrdem.StatusResposta == Gradual.OMS.Ordens.Lib.Enum.CriticaRiscoEnum.Sucesso)
                {
                    logger.Info("Enviado ordem de " +
                        (compraVenda.Equals(OrdemDirecaoEnum.Compra) ? "compra" : "venda") +
                        " do ativo[" + ativo.IdAtivo +
                        "] quantidade[" + ativo.Quantidade +
                        "] de IdCarteira[" + ativo.IdCarteiraRecomendada + 
                        "] para o cliente[" + idCliente + "]");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Falha no envio da ordem para o cliente[" +
                    idCliente + "] ativo[" + ativo.IdAtivo + "]: " + ex.Message);
            }
        }

        #endregion

        #region Risco

        private SaldoInfo ObterSaldoProjetado(ClienteProdutoInfo request)
        {
            SaldoInfo saldo = new SaldoInfo();
            saldo.valorSaldo = 0;
            saldo.descricaoSaldo = "";

            // Obtendo saldo projetado do cliente
            SaldoContaCorrenteResponse<ContaCorrenteInfo> responseSaldo = new SaldoContaCorrenteResponse<ContaCorrenteInfo>();
            try
            {
                SaldoContaCorrenteRequest requestSaldoContaCorrente = new SaldoContaCorrenteRequest();
                requestSaldoContaCorrente.IdCliente = request.IdCliente;

                IServicoContaCorrente servico = Ativador.Get<IServicoContaCorrente>();

                responseSaldo = servico.ObterSaldoContaCorrente(requestSaldoContaCorrente);

                saldo.valorSaldo = ((
                    responseSaldo.Objeto.SaldoD0 +
                    responseSaldo.Objeto.SaldoD1 +
                    responseSaldo.Objeto.SaldoD2 +
                    responseSaldo.Objeto.SaldoD3) +
                    (decimal.Parse(responseSaldo.Objeto.SaldoContaMargem.ToString())) +
                    (decimal.Parse(responseSaldo.Objeto.SaldoBloqueado.ToString())));

                saldo.descricaoSaldo =
                    responseSaldo.Objeto.SaldoD0 + " + " +
                    responseSaldo.Objeto.SaldoD1 + " + " +
                    responseSaldo.Objeto.SaldoD2 + " + " +
                    responseSaldo.Objeto.SaldoD3 + " + " +
                    responseSaldo.Objeto.SaldoContaMargem.ToString() + " + " +
                    responseSaldo.Objeto.SaldoBloqueado.ToString();

                return saldo;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private SaldoInfo ObterSaldoCustodia(ClienteProdutoInfo request)
        {
            SaldoInfo saldo = new SaldoInfo();
            saldo.valorSaldo = 0;
            saldo.descricaoSaldo = "";

            // Obtendo saldo da custódia do cliente
            try
            {
                SaldoCustodiaRequest requestCustodia = new SaldoCustodiaRequest();
                requestCustodia.IdCliente = request.IdCliente;

                IServicoCustodia servico = Ativador.Get<IServicoCustodia>();
                SaldoCustodiaResponse<CustodiaClienteInfo> responseCustodia = servico.ObterSaldoCustodiaClienteIntranet(requestCustodia);

                foreach (CustodiaClienteInfo custodia in responseCustodia.ColecaoObjeto)
                {
                    decimal ultimaCotacao = ObterUltimaCotacao(custodia.CodigoInstrumento);
                    if (ultimaCotacao != 0)
                    {
                        saldo.valorSaldo += (custodia.QtdeAtual * ultimaCotacao);
                        saldo.descricaoSaldo += custodia.CodigoInstrumento + "(" + custodia.QtdeAtual + "*" + ultimaCotacao + ") ";
                    }
                    else
                    {
                        saldo.valorSaldo += Convert.ToDecimal(custodia.ValorPosicao);
                        saldo.descricaoSaldo += custodia.CodigoInstrumento + "(" + custodia.ValorPosicao + ") ";
                    }
                }

                return saldo;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private SaldoInfo ObterValorCarteiraRecomendada(ClienteProdutoInfo request)
        {
            SaldoInfo saldo = new SaldoInfo();
            saldo.valorSaldo = 0;
            saldo.descricaoSaldo = "";

            // Obtendo valor total da composicao da carteira recomendada
            try
            {
                ListarComposicaoRequest requestComposicao = new ListarComposicaoRequest();
                requestComposicao.idProduto = request.IdProduto;

                ListarComposicaoResponse responseComposicao = SolicitarListaComposicao(requestComposicao);

                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicao)
                {
                    decimal ultimaCotacao = ObterUltimaCotacao(composicao.IdAtivo);
                    saldo.valorSaldo += (composicao.Quantidade * ultimaCotacao);
                    saldo.descricaoSaldo += composicao.IdAtivo + "(" + composicao.Quantidade + "*" + ultimaCotacao + ") ";
                }

                return saldo;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private SaldoInfo ObterValorCarteiraRecomendadaCompra(ClienteProdutoInfo request)
        {
            SaldoInfo saldo = new SaldoInfo();
            saldo.valorSaldo = 0;
            saldo.descricaoSaldo = "";

            RenovarClienteResponse response = new RenovarClienteResponse();
            try
            {
                ListarComposicaoClienteRequest requestComposicao = new ListarComposicaoClienteRequest();
                requestComposicao.idCliente = request.IdCliente;
                requestComposicao.idProduto = request.IdProduto;

                ListarComposicaoClienteResponse responseComposicao = SolicitarListaComposicaoCliente(requestComposicao);

                Hashtable listaAtual = new Hashtable();
                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicaoAtual)
                    listaAtual.Add(composicao.IdAtivo, composicao);

                Hashtable listaNova = new Hashtable();
                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicaoNova)
                    listaNova.Add(composicao.IdAtivo, composicao);

                // Se o ativo da composicao nova não existir na composição atual, contabiliza ativo para compra
                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicaoNova)
                {
                    if (!listaAtual.ContainsKey(composicao.IdAtivo))
                    {
                        decimal ultimaCotacao = ObterUltimaCotacao(composicao.IdAtivo);
                        saldo.valorSaldo += (composicao.Quantidade * ultimaCotacao);
                        saldo.descricaoSaldo += composicao.IdAtivo + "(" + composicao.Quantidade + "*" + ultimaCotacao + ") ";
                    }
                }

                return saldo;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private SaldoInfo ObterValorCarteiraRecomendadaVenda(ClienteProdutoInfo request)
        {
            SaldoInfo saldo = new SaldoInfo();
            saldo.valorSaldo = 0;
            saldo.descricaoSaldo = "";

            RenovarClienteResponse response = new RenovarClienteResponse();
            try
            {
                ListarComposicaoClienteRequest requestComposicao = new ListarComposicaoClienteRequest();
                requestComposicao.idCliente = request.IdCliente;
                requestComposicao.idProduto = request.IdProduto;

                ListarComposicaoClienteResponse responseComposicao = SolicitarListaComposicaoCliente(requestComposicao);

                Hashtable listaAtual = new Hashtable();
                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicaoAtual)
                    listaAtual.Add(composicao.IdAtivo, composicao);

                Hashtable listaNova = new Hashtable();
                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicaoNova)
                    listaNova.Add(composicao.IdAtivo, composicao);

                // Se o ativo da composicao atual não existir na composição nova, contabiliza ativo para venda
                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicaoAtual)
                {
                    if (!listaNova.ContainsKey(composicao.IdAtivo))
                    {
                        decimal ultimaCotacao = ObterUltimaCotacao(composicao.IdAtivo);
                        saldo.valorSaldo += (composicao.Quantidade * ultimaCotacao);
                        saldo.descricaoSaldo += composicao.IdAtivo + "(" + composicao.Quantidade + "*" + ultimaCotacao + ") ";
                    }
                }

                return saldo;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private AdesaoClienteResponse CalcularRiscoAdesaoCliente(AdesaoClienteRequest request)
        {
            AdesaoClienteResponse response = new AdesaoClienteResponse();
            SaldoInfo saldoProjetado = null;
            SaldoInfo saldoCustodia = null;
            SaldoInfo valorCarteira = null;

            try
            {
                ClienteProdutoInfo clienteProdutoInfo = new ClienteProdutoInfo();
                clienteProdutoInfo.IdCliente = request.IdCliente;
                clienteProdutoInfo.IdProduto = request.IdProduto;

                saldoProjetado = ObterSaldoProjetado(clienteProdutoInfo);
                saldoCustodia = ObterSaldoCustodia(clienteProdutoInfo);
                valorCarteira = ObterValorCarteiraRecomendada(clienteProdutoInfo);

                logger.Debug("Saldo Projetado: " + saldoProjetado.valorSaldo + " (" + saldoProjetado.descricaoSaldo + ")");
                logger.Debug("Saldo Custodia: " + saldoCustodia.valorSaldo + " (" + saldoCustodia.descricaoSaldo + ")");
                logger.Debug("Valor Carteira: " + valorCarteira.valorSaldo + " (" + valorCarteira.descricaoSaldo + ")");
            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.bSucesso = false;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.Exception = ex;
                response.DescricaoResposta = ex.Message;
                return response;
            }

            // Verifica o risco do cliente
            if (saldoCustodia.valorSaldo + saldoProjetado.valorSaldo < valorCarteira.valorSaldo)
            {
                response.DataResposta = DateTime.Now;
                response.bSucesso = false;
                response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                response.DescricaoResposta = "Cliente não possui saldo suficiente para adesão à Carteira Recomendada solicitada.";
                logger.Debug("Cliente não possui saldo suficiente para adesão à Carteira Recomendada solicitada");
            }
            else
            {
                response.DataResposta = DateTime.Now;
                response.bSucesso = true;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;
                response.DescricaoResposta = SUCESSO;
            }

            return response;
        }

        private RenovarClienteResponse CalcularRiscoRenovacaoCliente(RenovarClienteRequest request)
        {
            RenovarClienteResponse response = new RenovarClienteResponse();
            SaldoInfo saldoProjetado = null;
            SaldoInfo saldoCustodia = null;
            SaldoInfo valorCarteiraCompra = null;
            SaldoInfo valorCarteiraVenda = null;

            try
            {
                ClienteProdutoInfo clienteProdutoInfo = new ClienteProdutoInfo();
                clienteProdutoInfo.IdCliente = request.IdCliente;
                clienteProdutoInfo.IdProduto = request.IdProduto;

                saldoProjetado = ObterSaldoProjetado(clienteProdutoInfo);
                saldoCustodia = ObterSaldoCustodia(clienteProdutoInfo);
                valorCarteiraCompra = ObterValorCarteiraRecomendadaCompra(clienteProdutoInfo);
                valorCarteiraVenda = ObterValorCarteiraRecomendadaVenda(clienteProdutoInfo);

                logger.Debug("Saldo Projetado: " + saldoProjetado.valorSaldo + " (" + saldoProjetado.descricaoSaldo + ")");
                logger.Debug("Saldo Custodia: " + saldoCustodia.valorSaldo + " (" + saldoCustodia.descricaoSaldo + ")");
                logger.Debug("Valor Carteira Compra: " + valorCarteiraCompra.valorSaldo + " (" + valorCarteiraCompra.descricaoSaldo + ")");
                logger.Debug("Valor Carteira Venda: " + valorCarteiraVenda.valorSaldo + " (" + valorCarteiraVenda.descricaoSaldo + ")");
            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.bSucesso = false;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.Exception = ex;
                response.DescricaoResposta = ex.Message;
                return response;
            }

            // Verifica o risco do cliente
            if (saldoCustodia.valorSaldo + saldoProjetado.valorSaldo + valorCarteiraVenda.valorSaldo < valorCarteiraCompra.valorSaldo)
            {
                response.DataResposta = DateTime.Now;
                response.bSucesso = false;
                response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                response.DescricaoResposta = "Cliente não possui saldo suficiente para renovação da Carteira Recomendada solicitada.";
                logger.Debug("Cliente não possui saldo suficiente para renovação Carteira Recomendada solicitada");
            }
            else
            {
                response.DataResposta = DateTime.Now;
                response.bSucesso = true;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;
                response.DescricaoResposta = SUCESSO;
            }

            return response;
        }

        #endregion

        #region ContaCorrente

        public ValidarContaCorrenteResponse ObterSaldoContaCorrente(ValidarContaCorrenteRequest request)
        {

            SaldoContaCorrenteRequest requestSaldoContaCorrente = new SaldoContaCorrenteRequest();
            ValidarContaCorrenteResponse response = new ValidarContaCorrenteResponse();

            requestSaldoContaCorrente.IdCliente = request.IdCliente;


            try
            {
                IServicoContaCorrente servico = Ativador.Get<IServicoContaCorrente>();

                SaldoContaCorrenteResponse<ContaCorrenteInfo> responseSaldoContaCorrente = new SaldoContaCorrenteResponse<ContaCorrenteInfo>();
                responseSaldoContaCorrente = servico.ObterSaldoContaCorrente(requestSaldoContaCorrente);

                response.SaldoD0 = responseSaldoContaCorrente.Objeto.SaldoD0;
                response.SaldoD1 = responseSaldoContaCorrente.Objeto.SaldoD1;
                response.SaldoD2 = responseSaldoContaCorrente.Objeto.SaldoD2;
                response.SaldoD3 = responseSaldoContaCorrente.Objeto.SaldoD3;
                response.SaldoContaMargem = responseSaldoContaCorrente.Objeto.SaldoContaMargem;
                response.SaldoBloqueado = responseSaldoContaCorrente.Objeto.SaldoBloqueado;

                response.SaldoProjetado = ((
                    response.SaldoD0 +
                    response.SaldoD1 +
                    response.SaldoD2 +
                    response.SaldoD3) +
                    (decimal.Parse(response.SaldoContaMargem.ToString())) +
                    (decimal.Parse(response.SaldoBloqueado.ToString())));


                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;
                response.DescricaoResposta = "Saldos carregados com sucesso.";

            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.Exception = ex;
                response.DescricaoResposta = ex.Message;

            }
            return response;
        }

        #endregion

        #region Cotacao

        private decimal ObterUltimaCotacao(string ativo)
        {
            decimal ultimaCotacao = 0;
            try
            {
                IServicoCotacao servico = Ativador.Get<IServicoCotacao>();
                string tickerResumido = servico.ReceberTickerResumido(ativo);
                ultimaCotacao = Convert.ToDecimal(tickerResumido.Substring(0, 12), CultureInfo.CreateSpecificCulture("pt-BR"));
            }
            catch (Exception ex)
            {
                logger.Error("Não há cotação para o ativo[" + ativo + "]: " + ex.Message);
            }
            return ultimaCotacao;
        }

        #endregion

        #region Custodia

        public ValidarCustodiaResponse ObterCustodiaCliente(ValidarCustodiaRequest request)
        {
            ValidarCustodiaResponse response = new ValidarCustodiaResponse();

            try
            {
                SaldoCustodiaRequest requestCustodia = new SaldoCustodiaRequest();
                requestCustodia.IdCliente = request.IdCliente;

                IServicoCustodia servico = Ativador.Get<IServicoCustodia>();
                SaldoCustodiaResponse<CustodiaClienteInfo> responseCustodia = servico.ObterCustodiaCliente(requestCustodia);
                response.ColecaoObjeto = responseCustodia.ColecaoObjeto;

                foreach (CustodiaClienteInfo custodia in response.ColecaoObjeto)
                {
                    decimal ultimaCotacao = ObterUltimaCotacao(custodia.CodigoInstrumento);
                    logger.Info(
                        "Papel[" + custodia.CodigoInstrumento +
                        "] Cotacao[" + ultimaCotacao +
                        "] Qtd.Atual[" + custodia.QtdeAtual +
                        "] Qtd.Disponivel[" + custodia.QtdeDisponivel +
                        "] Posicao[" + custodia.ValorPosicao + "]");
                }

                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;
                response.DescricaoResposta = "Custodia carregada com sucesso.";

            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.Exception = ex;
                response.DescricaoResposta = ex.Message;

            }

            return response;

        }

        #endregion

        #region ServicoCarteiraRecomendada Members

        public EnviarNotificacaoResponse EnviarNotificacaoRenovacao(EnviarNotificacaoRequest request)
        {
            EnviarNotificacaoResponse response = new EnviarNotificacaoResponse();
            ListarClienteRenovacaoRequest requestListaCliente = new ListarClienteRenovacaoRequest();

            try
            {
                logger.Info("Avisar todos os clientes da Renovacao[" + request.IdRenovacao + "] da Carteira Recomendada[" + request.IdCarteiraRecomendada + "]");

                PersistenciaCarteiraRecomendada persistencia = new PersistenciaCarteiraRecomendada();

                requestListaCliente.IdCarteiraRecomendada = request.IdCarteiraRecomendada;
                requestListaCliente.IdRenovacao = request.IdRenovacao;
                ListarClienteRenovacaoResponse responseListaCliente = persistencia.ListaClientesRenovacao(requestListaCliente);

                CarteiraRecomendadaInfo carteiraRecomendadaInfo = persistencia.ObtemCarteiraRecomendada(request.IdCarteiraRecomendada);

                foreach (ClienteInfo cliente in responseListaCliente.lista)
                {
                    if (_emailsClientes.ContainsKey(cliente.IdCliente))
                    {
                        ClienteInfo dadosCliente = (ClienteInfo)_emailsClientes[cliente.IdCliente];

                        string corpoEmail = File.ReadAllText(
                            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                            @_arquivoEmailRenovacao);
                        corpoEmail = corpoEmail.
                            Replace("@NomeCliente", dadosCliente.DsCliente).
                            Replace("@CarteiraRecomendada", carteiraRecomendadaInfo.DsCarteira);

                        string assuntoEmail = "Sua Carteira Recomendada '" + carteiraRecomendadaInfo.DsCarteira + "' foi renovada!";

                        EnviarEmail(dadosCliente, corpoEmail, assuntoEmail);
                    }
                    else
                    {
                        logger.Error("Cliente id[" + cliente.IdCliente + "] não possui email válido!");
                    }
                }

                response.DataResposta = DateTime.Now;
                response.bSucesso = true;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;
                response.DescricaoResposta = SUCESSO;
            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna resposta");
            return response;
        }

        public EnviarNotificacaoResponse EnviarNotificacaoClienteAderido(EnviarNotificacaoRequest request)
        {
            EnviarNotificacaoResponse response = new EnviarNotificacaoResponse();

            try
            {
                logger.Info("Adesao do cliente[" + request.IdCliente + "] para Carteira Recomendada[" + request.IdCarteiraRecomendada + "]");

                PersistenciaCarteiraRecomendada persistencia = new PersistenciaCarteiraRecomendada();

                CarteiraRecomendadaInfo carteiraRecomendadaInfo = persistencia.ObtemCarteiraRecomendada(request.IdCarteiraRecomendada);

                if (_emailsClientes.ContainsKey(request.IdCliente))
                {
                    ClienteInfo dadosCliente = (ClienteInfo)_emailsClientes[request.IdCliente];

                    string corpoEmail = File.ReadAllText(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                        _arquivoEmailClienteAderido);
                    corpoEmail = corpoEmail.
                        Replace("@NomeCliente", dadosCliente.DsCliente).
                        Replace("@CarteiraRecomendada", carteiraRecomendadaInfo.DsCarteira);

                    string assuntoEmail = "Adesão à Carteira Recomendada '" + carteiraRecomendadaInfo.DsCarteira + "'";

                    EnviarEmail(dadosCliente, corpoEmail, assuntoEmail);
                }
                else
                {
                    logger.Error("Cliente id[" + request.IdCliente + "] não possui email válido!");
                }

                response.DataResposta = DateTime.Now;
                response.bSucesso = true;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;
                response.DescricaoResposta = SUCESSO;
            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna resposta");
            return response;
        }

        public EnviarNotificacaoResponse EnviarNotificacaoClienteRenovado(EnviarNotificacaoRequest request)
        {
            EnviarNotificacaoResponse response = new EnviarNotificacaoResponse();

            try
            {
                logger.Info("Renovacao do cliente[" + request.IdCliente + "] para Carteira Recomendada[" + request.IdCarteiraRecomendada + "]");

                PersistenciaCarteiraRecomendada persistencia = new PersistenciaCarteiraRecomendada();

                CarteiraRecomendadaInfo carteiraRecomendadaInfo = persistencia.ObtemCarteiraRecomendada(request.IdCarteiraRecomendada);

                if (_emailsClientes.ContainsKey(request.IdCliente))
                {
                    ClienteInfo dadosCliente = (ClienteInfo)_emailsClientes[request.IdCliente];

                    string corpoEmail = File.ReadAllText(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                        _arquivoEmailClienteRenovado);
                    corpoEmail = corpoEmail.
                        Replace("@NomeCliente", dadosCliente.DsCliente).
                        Replace("@CarteiraRecomendada", carteiraRecomendadaInfo.DsCarteira);

                    string assuntoEmail = "Renovação da Carteira Recomendada '" + carteiraRecomendadaInfo.DsCarteira + "'";

                    EnviarEmail(dadosCliente, corpoEmail, assuntoEmail);
                }
                else
                {
                    logger.Error("Cliente id[" + request.IdCliente + "] não possui email válido!");
                }

                response.DataResposta = DateTime.Now;
                response.bSucesso = true;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;
                response.DescricaoResposta = SUCESSO;
            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna resposta");
            return response;
        }

        public EnviarNotificacaoResponse EnviarNotificacaoClienteCancelado(EnviarNotificacaoRequest request)
        {
            EnviarNotificacaoResponse response = new EnviarNotificacaoResponse();

            try
            {
                logger.Info("Cancelamento do cliente[" + request.IdCliente + "] para Carteira Recomendada[" + request.IdCarteiraRecomendada + "]");

                PersistenciaCarteiraRecomendada persistencia = new PersistenciaCarteiraRecomendada();

                CarteiraRecomendadaInfo carteiraRecomendadaInfo = persistencia.ObtemCarteiraRecomendada(request.IdCarteiraRecomendada);

                if (_emailsClientes.ContainsKey(request.IdCliente))
                {
                    ClienteInfo dadosCliente = (ClienteInfo)_emailsClientes[request.IdCliente];

                    string corpoEmail = File.ReadAllText(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                        _arquivoEmailClienteCancelado);
                    corpoEmail = corpoEmail.
                        Replace("@NomeCliente", dadosCliente.DsCliente).
                        Replace("@CarteiraRecomendada", carteiraRecomendadaInfo.DsCarteira);

                    string assuntoEmail = "Cancelamento da Carteira Recomendada '" + carteiraRecomendadaInfo.DsCarteira + "'";

                    EnviarEmail(dadosCliente, corpoEmail, assuntoEmail);
                }
                else
                {
                    logger.Error("Cliente id[" + request.IdCliente + "] não possui email válido!");
                }

                response.DataResposta = DateTime.Now;
                response.bSucesso = true;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;
                response.DescricaoResposta = SUCESSO;
            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna resposta");
            return response;
        }

        private bool EnviarEmail(ClienteInfo dadosCliente, string corpoEmail, string assuntoEmail)
        {
            try
            {
                IServicoEmail servicoEmail = Ativador.Get<IServicoEmail>();
                EnviarEmailRequest requestEmail = new EnviarEmailRequest();
                requestEmail.Objeto = new OMS.Email.Lib.EmailInfo();
                requestEmail.Objeto.Assunto = assuntoEmail;
                requestEmail.Objeto.Destinatarios = new List<string>();
                if (_mockEmail == null)
                    requestEmail.Objeto.Destinatarios.Add(dadosCliente.DsEmail);
                else
                    requestEmail.Objeto.Destinatarios.Add(_mockEmail);
                requestEmail.Objeto.Remetente = _emailRemetenteNotificacao;
                requestEmail.Objeto.CorpoMensagem = corpoEmail;

                EnviarEmailResponse responseEmail = servicoEmail.Enviar(requestEmail);

                if (responseEmail.StatusResposta.Equals(MensagemResponseStatusEnum.OK))
                    logger.Info("E-mail para cliente [" + 
                        dadosCliente.IdCliente + "] [" + 
                        dadosCliente.DsCliente + "] [" + 
                        dadosCliente.DsEmail + "] disparado com sucesso");
                else
                    logger.Error("Não foi disparado e-mail para cliente [" + 
                        dadosCliente.IdCliente + "] [" +
                        dadosCliente.DsCliente + "] [" +
                        dadosCliente.DsEmail + "]");
            }
            catch (Exception ex)
            {
                logger.Error("Falha no disparo do e-mail: " + ex.Message);
            }
            return true;
        }

        private void OnTimer(Object source, ElapsedEventArgs e)
        {
            try
            {
                AtualizarEmailsClientes();

                _proximaAtualizacaoEmails = DateTime.Parse(
                    DateTime.Now.AddDays(1).ToString("yyyy/MM/dd") + " " + _horaAtualizacaoEmails,
                    new CultureInfo("pt-BR", false));

                TimeSpan intervaloAtualizacao = _proximaAtualizacaoEmails - DateTime.Now;
                _timer.Interval = intervaloAtualizacao.TotalMilliseconds;

                logger.Info("Proxima atualizacao dos emails: " + _proximaAtualizacaoEmails.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("ServicoCarteiraRecomendada > OnTimer()", ex);
            }
        }

        private void AtualizarEmailsClientes()
        {
            logger.Info("Iniciando atualização de emails dos clientes!");

            PersistenciaCarteiraRecomendada persistencia = new PersistenciaCarteiraRecomendada();
            ListarEmailResponse response = persistencia.ListaEmails();

            lock (_emailsClientes)
            {
                _emailsClientes.Clear();

                foreach (ClienteInfo clienteEmail in response.Lista)
                    if (!_emailsClientes.ContainsKey(clienteEmail.IdCliente))
                        _emailsClientes.Add(clienteEmail.IdCliente, clienteEmail);

                logger.Info("Emails atualizados! Foram obtidos " + response.Lista.Count +
                    " emails do Cadastro e atualizados " + _emailsClientes.Count + " emails no cache do serviço");
            }
        }

        #endregion //ServicoCarteiraRecomendada Members
    }
}