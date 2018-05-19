using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.CarteiraRecomendada.lib;
using Gradual.OMS.CarteiraRecomendada.lib.Mensageria;
using Gradual.OMS.CarteiraRecomendadaPersistencia;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.ContaCorrente;
using Gradual.OMS.Custodia;
using Gradual.OMS.Custodia.Lib;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Risco.Custodia;
using Gradual.OMS.Custodia.Lib.Mensageria;
using Gradual.OMS.Custodia.Lib.Info;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Seguranca.Lib;
using System.Globalization;
using log4net;

namespace Gradual.OMS.CarteiraRecomendada
{
    public class ServicoCarteiraRecomendada
    {      
        #region Constantes

        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string SUCESSO      = "SOLICITACAO ENVIADA COM SUCESSO.";
        public const string ERROPROGRAMA = "OCORREU UM ERRO AO ENVIAR A SOLICITACAO.";

        #endregion

        public ServicoCarteiraRecomendada()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

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
                    response.DescricaoResposta = "Nenhuma Carteira Recomendada encontrada";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.DescricaoResposta = response.Lista.Count.ToString() + " carteira(s) recomendada(s) encontrada(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
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
                    response.DescricaoResposta = "Nenhuma ativo encontrado na composição da Carteira Recomendada";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.DescricaoResposta = response.listaComposicao.Count.ToString() + " ativo(s) encontrado(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
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
                logger.Info("Id da Carteira: " + request.idCarteiraRecomendada.ToString());
                logger.Info("Tipo de solicitação: Lista");

                logger.Info("Chamando o método ListaComposicaoCliente() para efetuar a transacao no banco de dados");
                response = persistenciaCarteiraRecomendada.ListaComposicaoCliente(request);
                logger.Info("Transacao efetuada com sucesso");

                if (response.listaComposicaoNova.Count == 0)
                {
                    response.DescricaoResposta = "Nenhuma ativo encontrado na composição da Carteira Recomendada Atual";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.DescricaoResposta = (response.listaComposicaoNova.Count + response.listaComposicaoAtual.Count).ToString() +" ativo(s) encontrado(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
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

                response.DataResposta = DateTime.Now;
                response.bSucesso = bTransacao;
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

                response.DataResposta = DateTime.Now;
                response.bSucesso = bTransacao;
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
                bool bTransacao = persistenciaCarteiraRecomendada.Renovacao(request);
                logger.Info("Transacao efetuada com sucesso");

                response.DataResposta = DateTime.Now;
                response.bSucesso = bTransacao;
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

        /// <summary>
        /// Adesao do cliente a uma Carteira Recomendada (associada ao produto informado)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AdesaoClienteResponse SolicitarAdesaoCliente(AdesaoClienteRequest request)
        {
            AdesaoClienteResponse response = null;
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de adesão de cliente a carteira recomendada");
                logger.Info("Código do Cliente: " + request.IdCliente.ToString());
                logger.Info("Código do Produto: " + request.IdProduto.ToString());
                logger.Info("Tipo de solicitação: Inclusão");

                logger.Info("Avaliando Risco do Cliente");
                response = CalcularRiscoCliente(request);
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
                bool bTransacao = true; // persistenciaCarteiraRecomendada.AdesaoCliente(clienteProdutoInfo);
                logger.Info("Transacao efetuada com sucesso");

                response.DataResposta = DateTime.Now;
                response.bSucesso = bTransacao;
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
        
        /// <summary>
        /// Renovacao do cliente a uma Carteira Recomendada que foi atualizada (associada ao produto informado)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RenovarClienteResponse SolicitarRenovacaoCliente(RenovarClienteRequest request)
        {
            RenovarClienteResponse response = new RenovarClienteResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de renovação de cliente a carteira recomendada");
                logger.Info("Código do Cliente: " + request.IdCliente.ToString());
                logger.Info("Código do Produto: " + request.IdProduto.ToString());
                logger.Info("Tipo de solicitação: Renovação");

                logger.Info("Preenchendo a classe ClienteProdutoInfo");

                ClienteProdutoInfo clienteProdutoInfo = new ClienteProdutoInfo();

                clienteProdutoInfo.IdCliente = request.IdCliente;
                clienteProdutoInfo.IdProduto = request.IdProduto;
                clienteProdutoInfo.StAtivo = 'S';
                clienteProdutoInfo.IP = request.IP;

                logger.Info("Chamando o método RenovacaoCliente() para efetuar a transacao no banco de dados");
                bool bTransacao = persistenciaCarteiraRecomendada.RenovacaoCliente(clienteProdutoInfo);
                logger.Info("Transacao efetuada com sucesso");

                response.DataResposta = DateTime.Now;
                response.bSucesso = bTransacao;
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

        /// <summary>
        /// Cancelamento de carteira recomendada
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CancelarResponse SolicitarCancelamento(CancelarRequest request)
        {
            CancelarResponse response = new CancelarResponse();
            PersistenciaCarteiraRecomendada persistenciaCarteiraRecomendada = new PersistenciaCarteiraRecomendada();

            try
            {
                logger.Info("Preparando solicitação de cancelamento de cliente de carteira recomendada");
                logger.Info("Código do cliente: " + request.IdCliente.ToString());
                logger.Info("Código do produto: " + request.IdProduto.ToString());
                logger.Info("Tipo de solicitação: Cancelamento");

                logger.Info("Preenchendo a classe ClienteProdutoInfo");

                ClienteProdutoInfo clienteProdutoInfo = new ClienteProdutoInfo();

                clienteProdutoInfo.IdCliente = request.IdCliente;
                clienteProdutoInfo.IdProduto = request.IdProduto;
                clienteProdutoInfo.StAtivo = 'N';
                clienteProdutoInfo.IP = request.IP;
                clienteProdutoInfo.Descricao = "Cliente[" + request.IdCliente.ToString() + "] teve cancelado adesão ao Produto[" + request.IdProduto.ToString() + "]";

                logger.Info("Chamando o método Cancelamento() para efetuar a transacao no banco de dados");
                bool bTransacao = persistenciaCarteiraRecomendada.Cancelamento(clienteProdutoInfo);
                logger.Info("Transacao efetuada com sucesso");

                response.DataResposta = DateTime.Now;
                response.bSucesso = bTransacao;
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
                    response.DescricaoResposta = "Nenhuma Carteira Recomendada encontrada";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.DescricaoResposta = response.lista.Count.ToString() + " Carteira(s) Recomendada(s) encontrada(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
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
                    response.DescricaoResposta = "Nenhum Assessor encontrado";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.DescricaoResposta = response.Lista.Count.ToString() + " assessor(es) encontrado(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
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
                    response.DescricaoResposta = "Nenhum Acompanhamento encontrado";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                }
                else
                {
                    response.DescricaoResposta = response.Lista.Count.ToString() + " Acompanhamento(s) encontrado(s)";
                    response.DataResposta = DateTime.Now;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;
                }
            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DescricaoResposta = ERROPROGRAMA;
                response.Exception = ex;
            }

            logger.Info("Retorna a resposta");
            return response;
        }

        #region Risco

        private AdesaoClienteResponse CalcularRiscoCliente(AdesaoClienteRequest request)
        {
            AdesaoClienteResponse response = new AdesaoClienteResponse();
            decimal saldoProjetado = 0;
            decimal saldoCustodia = 0;
            decimal valorCarteira = 0;

            // Obtendo saldo projetado do cliente
            SaldoContaCorrenteResponse<ContaCorrenteInfo> responseSaldo = new SaldoContaCorrenteResponse<ContaCorrenteInfo>();
            try
            {
                SaldoContaCorrenteRequest requestSaldoContaCorrente = new SaldoContaCorrenteRequest();
                requestSaldoContaCorrente.IdCliente = request.IdCliente;

                IServicoContaCorrente servico = Ativador.Get<IServicoContaCorrente>();

                responseSaldo = servico.ObterSaldoContaCorrente(requestSaldoContaCorrente);

                saldoProjetado = ((
                    responseSaldo.Objeto.SaldoD0 +
                    responseSaldo.Objeto.SaldoD1 +
                    responseSaldo.Objeto.SaldoD2 +
                    responseSaldo.Objeto.SaldoD3) +
                    (decimal.Parse(responseSaldo.Objeto.SaldoContaMargem.ToString())) +
                    (decimal.Parse(responseSaldo.Objeto.SaldoBloqueado.ToString())));
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

            // Obtendo saldo da custódia do cliente
            string listaCustodia = "";
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
                        saldoCustodia += (custodia.QtdeDisponivel * ultimaCotacao);
                        listaCustodia += custodia.CodigoInstrumento + "(" + custodia.QtdeDisponivel + "*" + ultimaCotacao + ") ";
                    }
                    else
                    {
                        saldoCustodia += Convert.ToDecimal(custodia.ValorPosicao);
                        listaCustodia += custodia.CodigoInstrumento + "(" + custodia.ValorPosicao + ") ";
                    }
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

            // Obtendo valor total da composicao da carteira recomendada
            string listaCarteira = "";
            try
            {
                ListarComposicaoRequest requestComposicao = new ListarComposicaoRequest();
                requestComposicao.idProduto = request.IdProduto;

                ListarComposicaoResponse responseComposicao = SolicitarListaComposicao(requestComposicao);

                foreach (CarteiraRecomendadaComposicaoInfo composicao in responseComposicao.listaComposicao)
                {
                    decimal ultimaCotacao = ObterUltimaCotacao(composicao.IdAtivo);
                    valorCarteira += (composicao.Quantidade * ultimaCotacao);

                    listaCarteira += composicao.IdAtivo + "(" + composicao.Quantidade + "*" + ultimaCotacao + ") ";
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


            logger.Debug("Saldo Projetado: " + saldoProjetado + " (" +
                responseSaldo.Objeto.SaldoD0 + " + " +
                responseSaldo.Objeto.SaldoD1 + " + " +
                responseSaldo.Objeto.SaldoD2 + " + " +
                responseSaldo.Objeto.SaldoD3 + " + " +
                responseSaldo.Objeto.SaldoContaMargem.ToString() + " + " +
                responseSaldo.Objeto.SaldoBloqueado.ToString() + ")");
            logger.Debug("Saldo Custodia: " + saldoCustodia + " (" + listaCustodia + ")");
            logger.Debug("Valor Carteira: " + valorCarteira + " (" + listaCarteira + ")");

            // Verifica o risco do cliente
            if (saldoCustodia + saldoProjetado < valorCarteira)
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

                response.SaldoD0 =          responseSaldoContaCorrente.Objeto.SaldoD0;
                response.SaldoD1 =          responseSaldoContaCorrente.Objeto.SaldoD1;
                response.SaldoD2 =          responseSaldoContaCorrente.Objeto.SaldoD2;
                response.SaldoD3 =          responseSaldoContaCorrente.Objeto.SaldoD3;
                response.SaldoContaMargem = responseSaldoContaCorrente.Objeto.SaldoContaMargem;
                response.SaldoBloqueado =   responseSaldoContaCorrente.Objeto.SaldoBloqueado;

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

    }
}

