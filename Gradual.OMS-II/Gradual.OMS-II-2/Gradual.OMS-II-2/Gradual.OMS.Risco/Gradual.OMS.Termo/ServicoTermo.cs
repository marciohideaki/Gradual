using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Risco.Custodia;
using Gradual.OMS.Custodia.Lib.Mensageria;
using Gradual.OMS.Custodia.Lib;
using Gradual.OMS.Custodia.Lib.Info;
using Gradual.OMS.Cotacao.Lib.Mensageria;
using Gradual.OMS.Cotacao;
using Gradual.OMS.Termo.Lib.Mensageria;
using Gradual.OMS.Termo.Persistencia.Lib;
using Gradual.OMS.ContaCorrente;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.ContaCorrente.Lib;
using log4net;
using System.Collections;
using Gradual.OMS.Termo.Lib.Info;
using Gradual.OMS.Termo.Lib;
using System.ServiceModel;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Termo
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ServicoTermo : IServicoTermo, IServicoControlavel
    {
        private Hashtable htIntervaloMargem;

        private const string SALDOINSUFICIENTE = "Saldo insuficiente para efetuar esta operação.";
        private const string SOLICITACAOTERMOINCLUSAO = "Solicitação de inclusão de ordem de termo enviada com sucesso.";
        private const string SOLICITACAOTERMOALTERACAO = "Solicitação de alteração de termo de enviada com sucesso.";

        public ServicoTermo()
        {
            log4net.Config.XmlConfigurator.Configure();
            this.htIntervaloMargem = new PersistenciaTermo().ObterIntervaloMargem();

        }

        public readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// Cotações em tempo real
        /// </summary>
        /// <param name="Instrumento"></param>
        /// <returns></returns>
        private EnviarCotacaoResponse ObterCotacao(string Instrumento)
        {
            EnviarCotacaoRequest  _request = new EnviarCotacaoRequest();
            EnviarCotacaoResponse _response = new EnviarCotacaoResponse();

            try
            {
                logger.Info("SOLICITA CONSULTA DE COTACOES.");

                _request.CotacaoInfo.Ativo = Instrumento.Trim();
                ServicoCotacaoOMS _ServicoCotacaoOMS = new ServicoCotacaoOMS();

                _response = _ServicoCotacaoOMS.ObterCotacaoInstrumento(_request);
            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO OBTER COTACOES.", ex);
            }

            return _response;
        }

        /// <summary>
        /// Método responsável por retornar a taxa de termo do dia ( novo e rolagem )
        /// </summary>
        /// <returns></returns>
        public ConsultarTaxaTermoResponse ConsultarTaxaTermoDia()
        {
            ConsultarTaxaTermoResponse lResponse = new ConsultarTaxaTermoResponse();
            try
            {

                logger.Info("SOLICITA CONSULTA DE TAXA DE TERMO.");
                lResponse = new PersistenciaTermo().ConsultarTaxaTermoDia();
                lResponse.CriticaResposta = StatusRespostaEnum.Sucesso;
                lResponse.DataResposta = DateTime.Now;

                return lResponse;

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO OBTER A CONSULTA DE TAXA DE TERMOS PARA O DIA", ex);
                lResponse.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                lResponse.Exception = ex;
                lResponse.DescricaoResposta = ex.Message;
                lResponse.DataResposta = DateTime.Now;
            }

            return lResponse;
        }

        /// <summary>
        /// Método responsavel por calcular a margemrequerida de uma operação a termo.
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <param name="InstrumentoTermo"></param>
        /// <param name="Quantidade"></param>
        /// <returns></returns>
        public CalcularMargemRequeridaResponse CalcularMargemRequerida(CalcularMargemRequeridaRequest request)
        {
            CalcularMargemRequeridaResponse response = new CalcularMargemRequeridaResponse();

            try
            {
                decimal MargemRequerida = 0;

                string PapelAVista = request.InstrumentoTermo.Remove(request.InstrumentoTermo.Length - 1, 1);
                string PapelTermo = request.InstrumentoTermo;

                EnviarCotacaoResponse CotacaoAVista = this.ObterCotacao(PapelAVista);
                CotacaoTermoResponse CotacaoTermo = new PersistenciaTermo().ObterCotacaoTermo(PapelTermo);

                decimal IntervaloMargem = decimal.Parse(htIntervaloMargem[PapelAVista].ToString());
                IntervaloMargem = (IntervaloMargem / 100);

                MargemRequerida = (CotacaoAVista.Objeto.Ultima - (CotacaoAVista.Objeto.Ultima * IntervaloMargem) - (CotacaoTermo.ObjetoCotacao.Abertura));
                MargemRequerida = (MargemRequerida * request.Quantidade);

                response.ValorMargemRequerida = MargemRequerida;
                logger.Info("MARGEM REQUERIDA CALCULADA COM SUCESSO, CLIENTE: " + request.IdCliente + " .");

                response.DataResposta = DateTime.Now;
                response.CriticaResposta = StatusRespostaEnum.Sucesso;

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO CALCULAR A MARGEM REQUERIDA DO CLIENTE :" + request.IdCliente.ToString() + " .", ex);
                response.DataResposta = DateTime.Now;
                response.DescricaoResposta = "OCORREU UM ERRO AO CALCULAR A MARGEM REQUERIDA DO CLIENTE :" + request.IdCliente.ToString() +  " - " + ex.Message;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
            }


            return response;
        }

        public ClienteGarantiaResponse AlocarGarantiasTermo(ClienteGarantiaRequest request)
        {
            ClienteGarantiaResponse response = new ClienteGarantiaResponse();

            try 
            {
                logger.Info("Prepara envio de e-mail para custódia");
                logger.Info("Grava informações no banco de dados.");
                Email Email = new Email();

                Email.EnviarEmail(request);
                logger.Info("E-mail enviado com sucesso");

                response.CriticaResposta = StatusRespostaEnum.Sucesso;
                response.DataResposta = DateTime.Now;
                response.DescricaoResposta = "Solicitação enviada com sucesso";
            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO ALOCAR AS GARANTIAS DO CLIENTE: " + request.ClienteGarantiaInfo.IdCliente.ToString());
                response.DataResposta = DateTime.Now;
                response.DescricaoResposta = "OCORREU UM ERRO AO CALCULAR A MARGEM REQUERIDA DO CLIENTE :" + request.ClienteGarantiaInfo.IdCliente.ToString() + " - " + ex.Message;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
            }

            return response;
        }

        /// <summary>
        /// Método responsavel por inserir uma nova taxa de termo ( novo e rolagem )
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TaxaTermoResponse InserirTaxaTermo(TaxaTermoRequest request)
        {
            TaxaTermoResponse _resposta = new TaxaTermoResponse();

            try
            {
                logger.Info("INICIA ROTINA PARA INSERIR UMA NOVA TAXA DE TERMO");
                _resposta = new PersistenciaTermo().InserirTaxaTermo(request);

                _resposta.CriticaResposta = StatusRespostaEnum.Sucesso;
                _resposta.DescricaoResposta = "Taxa atualizada com sucesso.";
                _resposta.DataResposta = DateTime.Now;

                logger.Info("TAXA ATUALIZADA COM SUCESSO.");
            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO INSERIR UMA NOVA TAXA DE TERMO.");

                _resposta.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                _resposta.DescricaoResposta = ex.Message;
                _resposta.Exception = ex;
                _resposta.DataResposta = DateTime.Now;

            }

            return _resposta;
        }

        /// <summary>
        /// ´Método responsavel por inserir uma nova solicitação de operação a termo.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OrdemTermoResponse InserirOrdemTermo(OrdemTermoRequest request)
        {
            OrdemTermoResponse response = new OrdemTermoResponse();

            try
            {
                logger.Info("****************** INCLUSAO ********************");
                logger.Info("SOLICITACAO DE INCLUSAO DE ORDEM A TERMO ENVIADA");

                logger.Info("Cliente......................................." + request.OrdemTermoInfo.IdCliente.ToString());
                logger.Info("IdOrdemTermo.................................." + request.OrdemTermoInfo.IdOrdemTermo.ToString());
                logger.Info("IdTaxa........................................" + request.OrdemTermoInfo.IdTaxa.ToString());
                logger.Info("Intrumento...................................." + request.OrdemTermoInfo.Instrumento.ToString());
                logger.Info("precoDireto..................................." + request.OrdemTermoInfo.precoDireto.ToString());
                logger.Info("PrecoLimite..................................." + request.OrdemTermoInfo.PrecoLimite.ToString());
                logger.Info("precoMaximo..................................." + request.OrdemTermoInfo.precoMaximo.ToString());
                logger.Info("precoMinimo..................................." + request.OrdemTermoInfo.precoMinimo.ToString());
                logger.Info("precoNegocio.................................." + request.OrdemTermoInfo.precoNegocio.ToString());
                logger.Info("Quantidade...................................." + request.OrdemTermoInfo.Quantidade.ToString());
                logger.Info("StatusOrdemTermo.............................." + request.OrdemTermoInfo.StatusOrdemTermo.ToString());
                logger.Info("TipoSolicitacao..............................." + request.OrdemTermoInfo.TipoSolicitacao.ToString());

                // Obtem Cotacoes  
                string PapelAVista = request.OrdemTermoInfo.Instrumento.Remove(request.OrdemTermoInfo.Instrumento.Length - 1, 1);
                string PapelTermo = request.OrdemTermoInfo.Instrumento;

                EnviarCotacaoResponse CotacaoAVista = this.ObterCotacao(PapelAVista);
                CotacaoTermoResponse CotacaoTermo = new PersistenciaTermo().ObterCotacaoTermo(PapelTermo);

                decimal IntervaloMargem = decimal.Parse(htIntervaloMargem[PapelAVista].ToString());
                IntervaloMargem = (IntervaloMargem / 100);

                decimal MargemRequerida = (CotacaoAVista.Objeto.Ultima - (CotacaoAVista.Objeto.Ultima * IntervaloMargem) - (CotacaoTermo.ObjetoCotacao.Abertura));
                MargemRequerida = (MargemRequerida * request.OrdemTermoInfo.Quantidade);

                logger.Info("MargemRequerida..............................." + MargemRequerida.ToString());
                request.OrdemTermoInfo.MargemRequerida = Math.Abs(MargemRequerida);

                logger.Info("Verifica a posição atual do cliente:");
                ClientePosicaoResponse PosicaoCliente = this.ObterSaldoDisponivelTermo(request.OrdemTermoInfo.IdCliente);

                logger.Info("Total em conta corrente..............................." + PosicaoCliente.TotalContaCorrente.ToString());
                logger.Info("Total em garantias a prazo............................" + PosicaoCliente.TotalCustodiaGarantiaPrazo.ToString());
                logger.Info("Total em garantias no intraday........................" + PosicaoCliente.TotalCustodiaGarantiaIntraday.ToString());
                logger.Info("Total em garantias...................................." + PosicaoCliente.TotalGarantias.ToString());
                logger.Info("Total Negociavel em termo ............................" + PosicaoCliente.SaldoTotalTermo.ToString());

                if (PosicaoCliente.SaldoTotalTermo > 0)
                {
                    if (PosicaoCliente.SaldoTotalTermo >= MargemRequerida)
                    {
                        logger.Info("Envia a ordem de termo");
                        logger.Info("Saldo remanescente = " + (PosicaoCliente.SaldoTotalTermo - MargemRequerida).ToString());

                        response = new PersistenciaTermo().InserirOrdemTermo(request);
                        logger.Info(SOLICITACAOTERMOINCLUSAO + " cliente: " + request.OrdemTermoInfo.IdCliente.ToString());

                        response.DescricaoResposta = SOLICITACAOTERMOINCLUSAO;
                        response.CriticaResposta = StatusRespostaEnum.Sucesso;
                        response.DataResposta = DateTime.Now;
                        response.bSucesso = true;

                    }
                    else
                    {
                        logger.Info(SALDOINSUFICIENTE);
                        response.DescricaoResposta = SALDOINSUFICIENTE;
                        response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                        response.DataResposta = DateTime.Now;
                        response.bSucesso = false;
                    }
                }
                else
                {
                    logger.Info(SALDOINSUFICIENTE);
                    response.DescricaoResposta = SALDOINSUFICIENTE;
                    response.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                    response.DataResposta = DateTime.Now;
                    response.bSucesso = false;
                }

            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO INSERIR UMA NOVA SOLICITAÇÃO DE TERMO.", ex);
                response.DescricaoResposta = ex.Message;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;
                response.DataResposta = DateTime.Now;
                response.bSucesso = false;
                response.Exception = ex;
            }

            return response;

        }

        /// <summary>
        /// Método responsavel por atualizar uma solicitação de termo.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OrdemTermoResponse AtualizarOrdemTermo(OrdemTermoRequest request)
        {
            OrdemTermoResponse response = new OrdemTermoResponse();

            try
            {
                logger.Info("****************** ALTERACAO *********************");
                logger.Info("SOLICITACAO DE ALTERACAO DE ORDEM  A TERMO ENVIADA");

                logger.Info("Cliente......................................." + request.OrdemTermoInfo.IdCliente.ToString());
                logger.Info("IdOrdemTermo.................................." + request.OrdemTermoInfo.IdOrdemTermo.ToString());
                logger.Info("IdTaxa........................................" + request.OrdemTermoInfo.IdTaxa.ToString());
                logger.Info("precoDireto..................................." + request.OrdemTermoInfo.precoDireto.ToString());
                logger.Info("PrecoLimite..................................." + request.OrdemTermoInfo.PrecoLimite.ToString());
                logger.Info("precoMaximo..................................." + request.OrdemTermoInfo.precoMaximo.ToString());
                logger.Info("precoMinimo..................................." + request.OrdemTermoInfo.precoMinimo.ToString());
                logger.Info("precoNegocio.................................." + request.OrdemTermoInfo.precoNegocio.ToString());
                logger.Info("Quantidade...................................." + request.OrdemTermoInfo.Quantidade.ToString());
                logger.Info("StatusOrdemTermo.............................." + request.OrdemTermoInfo.StatusOrdemTermo.ToString());
                logger.Info("TipoSolicitacao..............................." + request.OrdemTermoInfo.TipoSolicitacao.ToString());

                ConsultarOrdemTermoRequest ConsultarStatusOrdemTermoRequest = new ConsultarOrdemTermoRequest();
                this.htIntervaloMargem = new PersistenciaTermo().ObterIntervaloMargem();


                ConsultarStatusOrdemTermoRequest.ConsultaTermoInfo.IdCliente = request.OrdemTermoInfo.IdCliente;
                ConsultarStatusOrdemTermoRequest.ConsultaTermoInfo.IdOrdemTermo = request.OrdemTermoInfo.IdOrdemTermo;

                ConsultarOrdemTermoResponse ConsultarStatusOrdemTermo = new PersistenciaTermo().ConsultarStatusOrdemTermo(ConsultarStatusOrdemTermoRequest);

                if (ConsultarStatusOrdemTermo.ListaOrdemTermo.Count > 0)
                {
                    request.OrdemTermoInfo.Instrumento = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].Instrumento;
                    request.OrdemTermoInfo.IdCliente = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].IdCliente;
                    request.OrdemTermoInfo.IdTaxa = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].IdTaxa;
                    request.OrdemTermoInfo.MargemRequerida = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].MargemRequerida;
                    request.OrdemTermoInfo.precoDireto = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].precoDireto;
                    request.OrdemTermoInfo.PrecoLimite = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].PrecoLimite;
                    request.OrdemTermoInfo.precoMaximo = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].precoMaximo;
                    request.OrdemTermoInfo.precoMinimo = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].precoMinimo;
                    request.OrdemTermoInfo.precoNegocio = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].precoNegocio;
                    request.OrdemTermoInfo.Quantidade = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].Quantidade;

                    // Obtem Cotacoes  
                    string PapelAVista = request.OrdemTermoInfo.Instrumento.Remove(request.OrdemTermoInfo.Instrumento.Length - 1, 1);
                    string PapelTermo = request.OrdemTermoInfo.Instrumento;

                    // INDICA QUE SOMENTE É POSSIVEL ALTERAR UMA SOLICITACAO DE TERMO QUANDO ESTA ESTIVER COM STATUS NOVO
                    if (ConsultarStatusOrdemTermo.ListaOrdemTermo[0].StatusOrdemTermo == Lib.EnumStatusTermo.SolicitacaoEnviada)
                    {
                        EnviarCotacaoResponse CotacaoAVista = this.ObterCotacao(PapelAVista);

                        CotacaoTermoResponse CotacaoTermo
                            = new PersistenciaTermo().ObterCotacaoTermo(PapelTermo);

                        decimal IntervaloMargem = decimal.Parse(htIntervaloMargem[PapelAVista].ToString());
                        IntervaloMargem = (IntervaloMargem / 100);

                        decimal MargemRequerida = (CotacaoAVista.Objeto.Ultima - (CotacaoAVista.Objeto.Ultima * IntervaloMargem) - (CotacaoTermo.ObjetoCotacao.Abertura));
                        MargemRequerida = (MargemRequerida * request.OrdemTermoInfo.Quantidade);

                        logger.Info("MargemRequerida..............................." + MargemRequerida.ToString());
                        request.OrdemTermoInfo.MargemRequerida = Math.Abs(MargemRequerida);

                        logger.Info("Verifica a posição atual do cliente:");
                        ClientePosicaoResponse PosicaoCliente = this.ObterSaldoDisponivelTermo(request.OrdemTermoInfo.IdCliente);

                        logger.Info("Total em conta corrente..............................." + PosicaoCliente.TotalContaCorrente.ToString());
                        logger.Info("Total em garantias a prazo............................" + PosicaoCliente.TotalCustodiaGarantiaPrazo.ToString());
                        logger.Info("Total em garantias no intraday........................" + PosicaoCliente.TotalCustodiaGarantiaIntraday.ToString());
                        logger.Info("Total em garantias...................................." + PosicaoCliente.TotalGarantias.ToString());
                        logger.Info("Total Negociavel em termo ............................" + PosicaoCliente.SaldoTotalTermo.ToString());

                        if (PosicaoCliente.SaldoTotalTermo > 0)
                        {
                            if (PosicaoCliente.SaldoTotalTermo >= MargemRequerida)
                            {
                                response = new PersistenciaTermo().AtualizarOrdemTermo(request);
                                logger.Info(SOLICITACAOTERMOINCLUSAO + " cliente: " + request.OrdemTermoInfo.IdCliente.ToString());

                                response.DescricaoResposta = SOLICITACAOTERMOALTERACAO;
                                response.CriticaResposta = Lib.Info.StatusRespostaEnum.Sucesso;
                                response.DataResposta = DateTime.Now;


                                logger.Info("Solicitacao de alteracao enviada com sucesso. Cliente: " + request.OrdemTermoInfo.IdCliente.ToString());

                            }
                            else
                            {
                                logger.Info(SALDOINSUFICIENTE);
                                response.DescricaoResposta = SALDOINSUFICIENTE;
                                response.CriticaResposta = Lib.Info.StatusRespostaEnum.ErroNegocio;
                                response.DataResposta = DateTime.Now;
                            }
                        }
                        else
                        {
                            logger.Info(SALDOINSUFICIENTE);
                            response.DescricaoResposta = SALDOINSUFICIENTE;
                            response.CriticaResposta = Lib.Info.StatusRespostaEnum.ErroNegocio;
                            response.DataResposta = DateTime.Now;
                        }
                    }
                    else
                    {
                        logger.Info("NÃO É POSSIVEL ALTERAR A SOLICITAÇÃO EM ABERTO, POIS ESTA SE ENCONTRA EM STATUS DE PROCESSAMENTO.");
                        response.DescricaoResposta = "NÃO É POSSIVEL ALTERAR A SOLICITAÇÃO EM ABERTO, POIS ESTA SE ENCONTRA EM STATUS DE PROCESSAMENTO.";
                        response.CriticaResposta = Lib.Info.StatusRespostaEnum.ErroNegocio;
                        response.DataResposta = DateTime.Now;
                    }
                }
                else
                {
                    logger.Info("DADOS NÃO ENCONTRADOS");
                    response.DescricaoResposta = "DADOS NAO ENCONTRADOS";
                    response.CriticaResposta = Lib.Info.StatusRespostaEnum.ErroNegocio;
                    response.DataResposta = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                response.CriticaResposta = Lib.Info.StatusRespostaEnum.ErroPrograma;
                response.DataResposta = DateTime.Now;
                response.DescricaoResposta = "OCORREU UM ERRO AO ATUALIZAR A SOLICITACAO DO CLIENTE.";
                logger.Error("OCORREU UM ERRO AO ATUALIZAR A SOLICITACAO DO CLIENTE.", ex);
            }


            return response;

        }

        /// <summary>
        /// Método responsavel por atualizar o status de uma solicitação de termo.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OrdemTermoResponse AlterarStatusSolicitacaoTermo(OrdemTermoRequest request)
        {
            OrdemTermoResponse response = new OrdemTermoResponse();
            try
            {
                logger.Info("INICIA ROTINA DE ATUALIZAÇÃO DE STATUS DO TERMO.");
                ConsultarOrdemTermoRequest ConsultarStatusOrdemTermoRequest = new ConsultarOrdemTermoRequest();

                ConsultarStatusOrdemTermoRequest.ConsultaTermoInfo.IdCliente = request.OrdemTermoInfo.IdCliente;
                ConsultarStatusOrdemTermoRequest.ConsultaTermoInfo.IdOrdemTermo = request.OrdemTermoInfo.IdOrdemTermo;

                ConsultarOrdemTermoResponse ConsultarStatusOrdemTermo = new PersistenciaTermo().ConsultarStatusOrdemTermo(ConsultarStatusOrdemTermoRequest);

                if (ConsultarStatusOrdemTermo.ListaOrdemTermo.Count > 0)
                {
                    request.OrdemTermoInfo.Instrumento = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].Instrumento;
                    request.OrdemTermoInfo.IdCliente = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].IdCliente;
                    request.OrdemTermoInfo.IdTaxa = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].IdTaxa;
                    request.OrdemTermoInfo.MargemRequerida = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].MargemRequerida;
                    request.OrdemTermoInfo.precoDireto = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].precoDireto;
                    request.OrdemTermoInfo.PrecoLimite = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].PrecoLimite;
                    request.OrdemTermoInfo.precoMaximo = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].precoMaximo;
                    request.OrdemTermoInfo.precoMinimo = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].precoMinimo;
                    request.OrdemTermoInfo.precoNegocio = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].precoNegocio;
                    request.OrdemTermoInfo.Quantidade = ConsultarStatusOrdemTermo.ListaOrdemTermo[0].Quantidade;

                    response = new PersistenciaTermo().InserirOrdemTermo(request);
                    response.DataResposta = DateTime.Now;
                    response.bSucesso = true;
                    response.CriticaResposta = StatusRespostaEnum.Sucesso;

                    logger.Info("DADOS ATUALIZADOS COM SUCESSO.");
                }
            }
            catch (Exception ex)
            {
                response.DescricaoResposta = ex.Message;
                response.DataResposta = DateTime.Now;
                response.Exception = ex;
                response.CriticaResposta = StatusRespostaEnum.ErroPrograma;

                logger.Error("OCORREU UM ERRO AO ATUALIZAR O STATUS DO TERMO. ", ex);
            }

            return response;

        }
        
        /// <summary>
        /// Metodo responsável por obter a custódia do cliente em todas as carteiras
        /// </summary>
        /// <param name="_request"></param>
        /// <returns></returns>
        public  SaldoCustodiaResponse<CustodiaClienteInfo> ObterCustodiaCliente ( SaldoCustodiaRequest request)
        {
            ServicoCustodia _servicoCustodia =
                new ServicoCustodia();

            logger.Info("SOLICITA POSICAO DE CUSTODIA DO CLIENTE");
            SaldoCustodiaResponse<CustodiaClienteInfo> lstCustodia =
                new SaldoCustodiaResponse<CustodiaClienteInfo>();

            try{
                lstCustodia = _servicoCustodia.ObterCustodiaCliente(request);

                 logger.Info("POSICAO CARREGADA COM SUCESSO.");
            }
            catch (Exception ex){
                logger.Error("Ocorreu um erro ao acessar o servico de custódia",ex);

            }

            return lstCustodia;
        }
          
        /// <summary>
        /// Retorna o saldo disponivel para operações a termo ( Projetado + garantias )
        /// </summary>
        /// <param name="IdCliente"></param>
        public ClientePosicaoResponse ObterSaldoDisponivelTermo(int IdCliente)
        {
            #region DECLARACOES

            decimal SaldoGarantiasMercadoPrazo = 0;
            decimal SaldoGarantiaIntraday = 0;
            decimal SaldoContaCorrente = 0;

            // Instancia do objeto de saldos de termo.
            ClientePosicaoResponse _ClientePosicaoResponse = new ClientePosicaoResponse();

            // Instancia do servico de custódia.
            ServicoCustodia _servicoCustodia = new ServicoCustodia();

            // Instancia do servico de conta corrente
            ServicoContaCorrente _ServicoContaCorrente = new ServicoContaCorrente();


            #endregion


            SaldoContaCorrenteResponse<ContaCorrenteInfo> _contaCorrenteResponse = new SaldoContaCorrenteResponse<ContaCorrenteInfo>();

            logger.Info("INICIA CONSULTA PARA OBTER O SALDO DE CC DO CLIENTE.");
            try
            {
                logger.Info("Inicia rotina de calculo de posição a termo. Cliente: " + IdCliente.ToString());

                #region TOTAL DE CUSTODIA DISPONIVEL ( CARTEIRA 23 + GARANTIAS ALOCADAS PARA O DIA )

                #region Posicao na carteira de garantia de mercado a prazo.

                SaldoCustodiaRequest _request = new SaldoCustodiaRequest();
                _request.IdCliente = IdCliente;

                SaldoCustodiaResponse<CustodiaClienteInfo> lstCustodia
                    = _servicoCustodia.ObterCustodiaCliente(_request);

                if (lstCustodia.ColecaoObjeto.Count > 0)
                {

                    var PosicaoCustodia = from p in lstCustodia.ColecaoObjeto
                                          where p.CodigoCarteira == 23019 // Garantia de opções.
                                          select p;

                    if (PosicaoCustodia.Count() > 0)
                    {
                        foreach (var item in PosicaoCustodia)
                        {

                            EnviarCotacaoResponse ResponseCotacao
                                = this.ObterCotacao(item.CodigoInstrumento);

                            SaldoGarantiasMercadoPrazo += (item.QtdeAtual * ResponseCotacao.Objeto.Ultima);

                        }
                    }
                }

                #endregion

                #region Posicao de garantias no Intraday

                ClienteCustodiaTermoRequest _GarantiasIntradayRequest = new ClienteCustodiaTermoRequest();

                _GarantiasIntradayRequest.IdCliente = IdCliente;
                _GarantiasIntradayRequest.dataReferencia = DateTime.Now; // DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));

                ClienteCustodiaTermoResponse _GarantiasIntradayResponse =
                    new PersistenciaTermo().ObterPosicaoTermoIntraday(_GarantiasIntradayRequest);

                if (_GarantiasIntradayResponse.ListaGarantias.Count > 0)
                {
                    foreach (var item in _GarantiasIntradayResponse.ListaGarantias)
                    {

                        EnviarCotacaoResponse ResponseCotacao
                            = this.ObterCotacao(item.Instrumento);

                        item.Cotacao = ResponseCotacao.Objeto.Ultima;
                        SaldoGarantiaIntraday += (item.Quantidade * item.Cotacao);

                    }
                }


                #endregion

                _ClientePosicaoResponse.IdCliente = IdCliente;
                _ClientePosicaoResponse.TotalCustodiaGarantiaPrazo = SaldoGarantiasMercadoPrazo;
                _ClientePosicaoResponse.TotalCustodiaGarantiaIntraday = SaldoGarantiaIntraday;

                _ClientePosicaoResponse.TotalGarantias = (_ClientePosicaoResponse.TotalCustodiaGarantiaPrazo + _ClientePosicaoResponse.TotalCustodiaGarantiaIntraday);


                #endregion

                #region TOTAL CONTA CORRENTE

                SaldoContaCorrenteRequest _contaCorrenteRequest = new SaldoContaCorrenteRequest();
                _contaCorrenteRequest.IdCliente = IdCliente;

                _contaCorrenteResponse = _ServicoContaCorrente.ObterSaldoContaCorrente(_contaCorrenteRequest);

                SaldoContaCorrente = _contaCorrenteResponse.Objeto.SaldoD0 +
                                     _contaCorrenteResponse.Objeto.SaldoD1 +
                                     _contaCorrenteResponse.Objeto.SaldoD2 +
                                     _contaCorrenteResponse.Objeto.SaldoD3;

                SaldoContaCorrente += (decimal.Parse(_contaCorrenteResponse.Objeto.SaldoBloqueado.ToString()) +
                                       decimal.Parse(_contaCorrenteResponse.Objeto.SaldoContaMargem.ToString())
                                      );

                _ClientePosicaoResponse.TotalContaCorrente = SaldoContaCorrente;

                #endregion

                #region SALDO TOTAL TERMO

                _ClientePosicaoResponse.SaldoTotalTermo = (_ClientePosicaoResponse.TotalGarantias + _ClientePosicaoResponse.TotalContaCorrente);

                #endregion

                _ClientePosicaoResponse.DataResposta   = DateTime.Now;             
                _ClientePosicaoResponse.CriticaResposta = StatusRespostaEnum.Sucesso;    


                logger.Info("Posicao calculada com sucesso.Cliente: " + IdCliente.ToString() + " Posicao total R$" + _ClientePosicaoResponse.SaldoTotalTermo.ToString());
            }
            catch (Exception ex)
            {

                logger.Error("Ocorreu um erro ao calcular o saldo do cliente [ObterSaldoDisponivelTermo]", ex);

                _ClientePosicaoResponse.DataResposta = DateTime.Now;
                _ClientePosicaoResponse.DescricaoResposta = ex.Message;
                _ClientePosicaoResponse.CriticaResposta = StatusRespostaEnum.ErroPrograma;    
            }

            return _ClientePosicaoResponse;

        }

        /// <summary>
        /// Metodo responsável por retornar todas as ordens executadas no mercado de termo dentro da corretora;
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AcompanhamentoOrdemTermoResponse ObterAcompanhamentoOrdemTermoSinacor(AcompanhamentoOrdemTermoRequest request)
        {

            logger.Info("INICIA CONSULTA PARA OBER O ACOMPANHAMENTO DE ORDENS DE TERMO (L/P) BO SINACOR.");
            AcompanhamentoOrdemTermoResponse AcompanhamentoOrdemTermoResponse = new AcompanhamentoOrdemTermoResponse();
            try
            {
                AcompanhamentoOrdemTermoResponse = new PersistenciaTermo().ObterAcompanhamentoOrdemTermoSinacor(request);

                if (AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo.Count > 0)
                {

                    for (int i = 0; i <= AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo.Count - 1; i++)
                    {

                        decimal LucroPrejuizo = 0;
                        decimal TotalGasto = 0;
                        decimal TotalValorizado = 0;

                        string Instrumento = AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].Instrumento;
                        int Quantidade = AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].Quantidade;
                        decimal PrecoTermo = AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].PrecoTermo;

                        string InstrumentoAVista = Instrumento.Remove(Instrumento.Length - 1, 1);
                        string InstrumentoATermo = Instrumento;

                        EnviarCotacaoResponse CotacaoAVista = this.ObterCotacao(InstrumentoAVista);
                        CotacaoTermoResponse CotacaoTermo = new PersistenciaTermo().ObterCotacaoTermo(InstrumentoATermo);

                        TotalGasto = (Quantidade * PrecoTermo);
                        TotalValorizado = (Quantidade * CotacaoTermo.ObjetoCotacao.Ultima);

                        LucroPrejuizo = (TotalValorizado - TotalGasto);

                        AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].PrecoMercado = CotacaoAVista.Objeto.Ultima;
                        AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].PrecoTermo = CotacaoTermo.ObjetoCotacao.Ultima;
                        AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].LucroPrejuizo = LucroPrejuizo;
                        AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].TotalMercado = (Quantidade * CotacaoTermo.ObjetoCotacao.Ultima);
                        AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].TotalTermo = (Quantidade * PrecoTermo);

                        AcompanhamentoOrdemTermoResponse.CriticaResposta = StatusRespostaEnum.Sucesso;
                        AcompanhamentoOrdemTermoResponse.DataResposta = DateTime.Now;

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO OBTER AS POSICOES CALCULADAS NO SINACOR.",ex);
                AcompanhamentoOrdemTermoResponse.Exception = ex;
                AcompanhamentoOrdemTermoResponse.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                AcompanhamentoOrdemTermoResponse.DataResposta = DateTime.Now;

            }

            return AcompanhamentoOrdemTermoResponse;
        }

        /// <summary>
        /// Método responsável por retornar todas as solicitações feitas pelo cliente.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private AcompanhamentoOrdemTermoResponse ObterAcompanhamentoSolicitacoes(AcompanhamentoOrdemTermoRequest request)
        {
            AcompanhamentoOrdemTermoResponse AcompanhamentoOrdemTermoResponse = new AcompanhamentoOrdemTermoResponse();

            logger.Info("INICIA CONSULTA PARA OBER O ACOMPANHAMENTO DE ORDENS DE SOLICITACOES DE TERMO ENVIADA PELO CLIENTE: ");

            try
            {
                AcompanhamentoOrdemTermoResponse = new PersistenciaTermo().ObterAcompanhamentoOrdemTermo(request.AcompanhamentoOrdemTermoInfo);

                if (AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo.Count > 0)
                {

                    for (int i = 0; i <= AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo.Count - 1; i++)
                    {
                        string Instrumento = AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].Instrumento;
                        int Quantidade = AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].Quantidade;
                        decimal PrecoTermo = AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].PrecoTermo;

                        string InstrumentoAVista = Instrumento.Remove(Instrumento.Length - 1, 1);
                        string InstrumentoATermo = Instrumento;

                        EnviarCotacaoResponse CotacaoAVista = this.ObterCotacao(InstrumentoAVista);
                        CotacaoTermoResponse CotacaoTermo = new PersistenciaTermo().ObterCotacaoTermo(InstrumentoATermo);

                        AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].PrecoMercado = CotacaoAVista.Objeto.Ultima;
                        AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].PrecoTermo = CotacaoTermo.ObjetoCotacao.Ultima;
                        AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].TotalMercado = (Quantidade * CotacaoAVista.Objeto.Ultima);
                        AcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].TotalTermo = (Quantidade * CotacaoTermo.ObjetoCotacao.Ultima);

                        logger.Info("INFORMAÇOES RETORNADAS COM SUCESSO.");
                        AcompanhamentoOrdemTermoResponse.CriticaResposta = StatusRespostaEnum.Sucesso;
                        AcompanhamentoOrdemTermoResponse.DataResposta = DateTime.Now;

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO CARREGAR AS SOLICITACOES DE TERMO DO SISTEMA SINACOR.");
                AcompanhamentoOrdemTermoResponse.CriticaResposta = StatusRespostaEnum.ErroNegocio;
                AcompanhamentoOrdemTermoResponse.DataResposta    = DateTime.Now;
                AcompanhamentoOrdemTermoResponse.Exception       = ex;
            }

            return AcompanhamentoOrdemTermoResponse;
        }

        /// <summary>
        /// Método responsável por retornar o acompanhamento de solicitações de ordens a termo.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AcompanhamentoConsolidadoOrdemTermoResponse AcompanhamentoOrdensConsolidadoSolicitacoes(AcompanhamentoConsolidadoOrdemTermoRequest request)
        {
            AcompanhamentoOrdemTermoConsolidadoInfo lAcompanhamentoOrdemTermoConsolidadoInfo = new AcompanhamentoOrdemTermoConsolidadoInfo();
            AcompanhamentoConsolidadoOrdemTermoResponse lAcompanhamentoConsolidadoOrdemTermoResponse = new AcompanhamentoConsolidadoOrdemTermoResponse();
            AcompanhamentoOrdemTermoResponse lAcompanhamentoOrdemTermoResponse = new AcompanhamentoOrdemTermoResponse();

            List<string> lstAtivos = new List<string>();
            List<DateTime> lstVencimentos = new List<DateTime>();

            try
            {
                int CodigoCliente = request.CodigoCliente;

                lAcompanhamentoOrdemTermoResponse =
                    new PersistenciaTermo().ObterAcompanhamentoOrdemTermo(request.AcompanhamentoOrdemTermoInfo);


                if (lAcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo.Count > 0)
                {

                    for (int i = 0; i <= lAcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo.Count - 1; i++)
                    {

                        string Ativo = lAcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].Instrumento;
                        DateTime dtVencimento = lAcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].DataVencimento;

                        if (!lstAtivos.Contains(Ativo))
                            lstAtivos.Add(Ativo);

                        if (!lstVencimentos.Contains(dtVencimento))
                            lstVencimentos.Add(dtVencimento);
                    }

                    foreach (string Ativo in lstAtivos)
                    {
                        foreach (DateTime Vencimento in lstVencimentos)
                        {

                            decimal LucroPrejuizoAcumulado = 0;

                            var lstTrades = from p in lAcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo
                                            where p.Instrumento == Ativo && p.DataVencimento == Vencimento
                                            select p;

                            if (lstTrades.Count() > 0)
                            {

                                AcompanhamentoOrdemTermoResponse lAcompanhamentoOrdemTermoRef = new AcompanhamentoOrdemTermoResponse();
                                lAcompanhamentoOrdemTermoConsolidadoInfo = new AcompanhamentoOrdemTermoConsolidadoInfo();

                                foreach (AcompanhamentoOrdemTermoInfo info in lstTrades)
                                {
                                    decimal LucroPrejuizo = 0;
                                    decimal TotalGasto = 0;
                                    decimal TotalValorizado = 0;

                                    string InstrumentoAVista = info.Instrumento.Remove(info.Instrumento.Length - 1, 1);
                                    string InstrumentoATermo = info.Instrumento;

                                    EnviarCotacaoResponse CotacaoAVista = this.ObterCotacao(InstrumentoAVista);
                                    CotacaoTermoResponse CotacaoTermo = new PersistenciaTermo().ObterCotacaoTermo(InstrumentoATermo);

                                    TotalGasto = (info.Quantidade * info.PrecoTermo);
                                    TotalValorizado = (info.Quantidade * CotacaoTermo.ObjetoCotacao.Ultima);
                                    LucroPrejuizo = (TotalValorizado - TotalGasto);
                                    info.PrecoMercado = CotacaoAVista.Objeto.Ultima;
                                    info.PrecoTermo = CotacaoTermo.ObjetoCotacao.Ultima;
                                    // info.LucroPrejuizo = LucroPrejuizo;
                                    // info.TotalMercado = (info.Quantidade * CotacaoTermo.Cotacao.Ultima);
                                    // info.totalTermo = (info.Quantidade * info.PrecoTermo);       
                         

                                    lAcompanhamentoOrdemTermoRef.ListaAcompanhamentoOrdemTermo.Add(info);
                                }

                                lAcompanhamentoOrdemTermoConsolidadoInfo.lstAcompanhamentoOrdemTermo = lAcompanhamentoOrdemTermoRef.ListaAcompanhamentoOrdemTermo;
                                lAcompanhamentoOrdemTermoConsolidadoInfo.Instrumento = Ativo;
                                lAcompanhamentoOrdemTermoConsolidadoInfo.LucroPrejuizo = LucroPrejuizoAcumulado;
                                lAcompanhamentoOrdemTermoConsolidadoInfo.DataVencimento = Vencimento;

                                lAcompanhamentoConsolidadoOrdemTermoResponse.ListaAcompanhamentoConsolidado.Add(lAcompanhamentoOrdemTermoConsolidadoInfo);

                                
                                lAcompanhamentoConsolidadoOrdemTermoResponse.DataResposta = DateTime.Now;
                                lAcompanhamentoConsolidadoOrdemTermoResponse.CriticaResposta = StatusRespostaEnum.Sucesso;
                                lAcompanhamentoConsolidadoOrdemTermoResponse.DescricaoResposta = "Dados retornados com sucesso";

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao chamar o método AcompanhamentoOrdensConsolidadoSolicitacoes. ", ex);
                lAcompanhamentoConsolidadoOrdemTermoResponse.DescricaoResposta = ex.Message;
                lAcompanhamentoConsolidadoOrdemTermoResponse.Exception = ex;
                lAcompanhamentoConsolidadoOrdemTermoResponse.DataResposta = DateTime.Now;
                lAcompanhamentoConsolidadoOrdemTermoResponse.CriticaResposta = StatusRespostaEnum.ErroPrograma;


            }

            return lAcompanhamentoConsolidadoOrdemTermoResponse;
        }


      

        /// <summary>
        /// Método responsável por retornar todas as ordens executadas no mercado de termo, consolidando por vencimento de termo.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>    
        public AcompanhamentoConsolidadoOrdemTermoResponse AcompanhamentoOrdensConsolidadoSinacor(AcompanhamentoConsolidadoOrdemTermoRequest request)
        {
            AcompanhamentoOrdemTermoConsolidadoInfo lAcompanhamentoOrdemTermoConsolidadoInfo = new AcompanhamentoOrdemTermoConsolidadoInfo();
            AcompanhamentoConsolidadoOrdemTermoResponse lAcompanhamentoConsolidadoOrdemTermoResponse = new AcompanhamentoConsolidadoOrdemTermoResponse();
            AcompanhamentoOrdemTermoResponse lAcompanhamentoOrdemTermoResponse = new AcompanhamentoOrdemTermoResponse();

            List<string> lstAtivos = new List<string>();
            List<DateTime> lstVencimentos = new List<DateTime>();

            int CodigoCliente = 0;

            try
            {
                CodigoCliente = request.CodigoCliente;

                lAcompanhamentoOrdemTermoResponse =
                    new PersistenciaTermo().ObterAcompanhamentoOrdemTermoSinacor(CodigoCliente);

                if (lAcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo.Count > 0)
                {

                    for (int i = 0; i <= lAcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo.Count - 1; i++)
                    {

                        string Ativo = lAcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].Instrumento;
                        DateTime dtVencimento = lAcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo[i].DataVencimento;

                        if (!lstAtivos.Contains(Ativo))
                            lstAtivos.Add(Ativo);

                        if (!lstVencimentos.Contains(dtVencimento))
                            lstVencimentos.Add(dtVencimento);
                    }

                    foreach (string Ativo in lstAtivos)
                    {
                        foreach (DateTime Vencimento in lstVencimentos)
                        {

                            decimal LucroPrejuizoAcumulado = 0;

                            var lstTrades = from p in lAcompanhamentoOrdemTermoResponse.ListaAcompanhamentoOrdemTermo
                                            where p.Instrumento == Ativo && p.DataVencimento == Vencimento
                                            select p;

                            if (lstTrades.Count() > 0)
                            {

                                AcompanhamentoOrdemTermoResponse lAcompanhamentoOrdemTermoRef = new AcompanhamentoOrdemTermoResponse();
                                lAcompanhamentoOrdemTermoConsolidadoInfo = new AcompanhamentoOrdemTermoConsolidadoInfo();

                                foreach (AcompanhamentoOrdemTermoInfo info in lstTrades)
                                {
                                    decimal LucroPrejuizo = 0;
                                    decimal TotalGasto = 0;
                                    decimal TotalValorizado = 0;

                                    string InstrumentoAVista = info.Instrumento.Remove(info.Instrumento.Length - 1, 1);
                                    string InstrumentoATermo = info.Instrumento;

                                    EnviarCotacaoResponse CotacaoAVista = this.ObterCotacao(InstrumentoAVista);
                                    CotacaoTermoResponse CotacaoTermo = new PersistenciaTermo().ObterCotacaoTermo(InstrumentoATermo);

                                    TotalGasto = (info.Quantidade * info.PrecoTermo);
                                    TotalValorizado = (info.Quantidade * CotacaoTermo.ObjetoCotacao.Ultima);
                                    LucroPrejuizo = (TotalValorizado - TotalGasto);
                                    info.PrecoMercado = CotacaoAVista.Objeto.Ultima;
                                    info.PrecoTermo = CotacaoTermo.ObjetoCotacao.Ultima;
                                    info.LucroPrejuizo = LucroPrejuizo;
                                    info.TotalMercado = (info.Quantidade * CotacaoTermo.ObjetoCotacao.Ultima);
                                    info.TotalTermo = (info.Quantidade * info.PrecoTermo);

                                    info.IdStatusOrdemTermo = EnumStatusTermo.SolicitacaoExecutada;
                                    info.IdTipoSolicitacao = EnumTipoSolicitacao.NovoTermo;

                                    LucroPrejuizoAcumulado += info.LucroPrejuizo;

                                    lAcompanhamentoOrdemTermoRef.ListaAcompanhamentoOrdemTermo.Add(info);
                                }

                                lAcompanhamentoOrdemTermoConsolidadoInfo.lstAcompanhamentoOrdemTermo = lAcompanhamentoOrdemTermoRef.ListaAcompanhamentoOrdemTermo;
                                lAcompanhamentoOrdemTermoConsolidadoInfo.Instrumento = Ativo;
                                lAcompanhamentoOrdemTermoConsolidadoInfo.LucroPrejuizo = LucroPrejuizoAcumulado;
                                lAcompanhamentoOrdemTermoConsolidadoInfo.DataVencimento = Vencimento;
                                lAcompanhamentoOrdemTermoConsolidadoInfo.CodigoCliente = CodigoCliente;

                                lAcompanhamentoConsolidadoOrdemTermoResponse.ListaAcompanhamentoConsolidado.Add(lAcompanhamentoOrdemTermoConsolidadoInfo);

                                lAcompanhamentoConsolidadoOrdemTermoResponse.DataResposta = DateTime.Now;
                                lAcompanhamentoConsolidadoOrdemTermoResponse.CriticaResposta = StatusRespostaEnum.Sucesso;
                                lAcompanhamentoConsolidadoOrdemTermoResponse.DescricaoResposta = "Dados retornados com sucesso";

                                logger.Info("INFORMACOES CARREGADAS COM SUCESSO.");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("OCORREU UM ERRO AO OBTER O SALDO CONSOLIDADO DO CLIENTE. " + request.CodigoCliente.ToString());
                lAcompanhamentoConsolidadoOrdemTermoResponse.DescricaoResposta = ex.Message;
                lAcompanhamentoConsolidadoOrdemTermoResponse.Exception = ex;
                lAcompanhamentoConsolidadoOrdemTermoResponse.DataResposta = DateTime.Now;
                lAcompanhamentoConsolidadoOrdemTermoResponse.CriticaResposta = StatusRespostaEnum.ErroPrograma;

            }

            return lAcompanhamentoConsolidadoOrdemTermoResponse;
        }


        #region IServicoControlavel Members

        ServicoStatus _ServicoStatus { set; get; }

        public void IniciarServico()
        {
            _ServicoStatus = ServicoStatus.EmExecucao;
            logger.Info("Serviço iniciado com sucesso.");
            logger.Info("Aguardando solicitações.");
        }

        public void PararServico()
        {
            _ServicoStatus = ServicoStatus.Parado;
            logger.Info("Serviço parado com sucesso.");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion
    }


    

}

