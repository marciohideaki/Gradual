using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.CadastroPapeis.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Ordens.StartStop.Lib;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.Core.Ordens.Lib;
using Gradual.Core.Ordens;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;

namespace Gradual.Site.Www.MinhaConta.Operacoes
{
    public partial class Operacoes : PaginaBase
    {
        #region Propriedades

        CultureInfo _ciPtBR = new CultureInfo("pt-BR");
        CultureInfo _ciEn = new CultureInfo("en-US");

        private int CodigoBovespaCliente
        {
            get
            {
                return Convert.ToInt32(base.SessaoClienteLogado.CodigoPrincipal);
            }
        }

        private int CodigoBMFCliente
        {
            get
            {
                return base.SessaoClienteLogado.CodigoBMF;
            }
        }

        public string RequestAcao
        {
            get
            {
                if (Request.Form["Acao"] == null)
                    return "";
                else
                    return Request.Form["Acao"];
            }
        }

        public string RequestPapel
        {
            get
            {
                if (string.IsNullOrEmpty(Request["Ativo"]))
                {
                    throw new Exception("Ativo não especificado");
                }

                return Request["Ativo"].ToUpper();
            }
        }

        public string RequestBolsa
        {
            get
            {
                if (string.IsNullOrEmpty(Request["Bolsa"]))
                {
                    throw new Exception("Bolsa não especificada");
                }

                return Request["Bolsa"];
            }
        }

        public int RequestQuantidade
        {
            get
            {
                if (String.IsNullOrEmpty(Request["Quantidade"]))
                {
                    throw new Exception("Quantidade não especificada");
                }

                return Convert.ToInt32(Request["Quantidade"]);
            }
        }

        public int RequestQuantidadeAparente
        {
            get
            {
                if (String.IsNullOrEmpty(Request["QtdeAparente"]))
                {
                    return 0;
                }

                return Convert.ToInt32(Request["QtdeAparente"]);
            }
        }

        public int RequestQuantidadeMinima
        {
            get
            {
                if (String.IsNullOrEmpty(Request["QuantidadeMinima"]))
                {
                    return 0;
                }

                return Convert.ToInt32(Request["QuantidadeMinima"]);
            }
        }

        public OrdemDirecaoEnum RequestDirecao
        {
            get
            {
                if (String.IsNullOrEmpty(Request["Direcao"]))
                {
                    throw new Exception("Direção não especificada");
                }

                if (Request["Direcao"].ToUpper() == "COMPRA")
                {
                    return OrdemDirecaoEnum.Compra;
                }
                else if (Request["Direcao"].ToUpper() == "VENDA")
                {
                    return OrdemDirecaoEnum.Venda;
                }
                else
                {
                    return (OrdemDirecaoEnum)Convert.ToInt32(Request["Direcao"]);
                }
            }
        }

        public Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum RequestValidade
        {
            get
            {
                if (Request["Validade"] == "5" || Request["Validade"] == "2")
                {
                    return Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidoAteDeterminadaData;
                }

                if (Request["Validade"] == "6")
                {
                    return Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ExecutaIntegralParcialOuCancela;
                }

                return Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia;
            }
        }

        public double RequestPreco
        {
            get
            {
                if (String.IsNullOrEmpty(Request["Preco"]))
                {
                    throw new Exception("Preço não especificado");
                }

                return Convert.ToDouble(Request["Preco"], _ciEn);
            }
        }

        public double RequestStopStart
        {
            get
            {
                if (String.IsNullOrEmpty(Request["PrecoStopStart"]))
                {
                    return 0;
                }

                return Convert.ToDouble(Request["PrecoStopStart"], _ciEn);
            }
            
        }

        public string RequestAssinatura
        {
            get
            {
                return Request["Assinatura"];
            }
        }

        public string RequestConta
        {
            get
            {
                return Request["Conta"];
            }
        }

        public string RequestDataMovimento
        {
            get
            {
                return Request["DataMovimento"];
            }
        }

        public string RequestCodigoOrdem
        {
            get
            {
                return Request["Ordem"];
            }
        }

        public string RequestIdOrdem
        {
            get
            {
                return Request["IdOrdem"];
            }
        }

        public decimal RequestPrecoDisparo
        {
            get
            {
                if (string.IsNullOrEmpty(Request.Form["PrecoDisparo"]))
                {
                    return 0;       //O zero como padrão se não vier nada foi implementado pelo Kubo, verificar depois se é isso mesmo ou tem que dar erro
                }
                else
                {
                    return Convert.ToDecimal(Request.Form["PrecoDisparo"], _ciPtBR);
                }
            }
        }

        public decimal RequestPrecoLimite
        {
            get
            {
                if (string.IsNullOrEmpty(Request.Form["PrecoLimite"]))
                {
                    return 0;       //O zero como padrão se não vier nada foi implementado pelo Kubo, verificar depois se é isso mesmo ou tem que dar erro
                }
                else
                {
                    return Convert.ToDecimal(Request.Form["PrecoLimite"], _ciPtBR);
                }
            }
        }

        public decimal RequestTaxaAjusteInicio
        {
            get
            {
                if (string.IsNullOrEmpty(Request.Form["Inicio"]))
                {
                    return 0;       //O zero como padrão se não vier nada foi implementado pelo Kubo, verificar depois se é isso mesmo ou tem que dar erro
                }
                else
                {
                    return Convert.ToDecimal(Request.Form["Inicio"], _ciPtBR);
                }
            }
        }

        public decimal RequestTaxaAjusteMovel
        {
            get
            {
                if (string.IsNullOrEmpty(Request.Form["Ajuste"]))
                {
                    return 0;       //O zero como padrão se não vier nada foi implementado pelo Kubo, verificar depois se é isso mesmo ou tem que dar erro
                }
                else
                {
                    return Convert.ToDecimal(Request.Form["Ajuste"], _ciPtBR);
                }
            }
        }

        public string RequestTipoOrdem
        {
            get
            {
                return Request["TipoOrdem"];
            }
        }

        public decimal RequestPrecoDisparoGain
        {
            get
            {
                if (string.IsNullOrEmpty(Request.Form["PrecoDisparo_Gain"]))
                {
                    return 0;       //O zero como padrão se não vier nada foi implementado pelo Kubo, verificar depois se é isso mesmo ou tem que dar erro
                }
                else
                {
                    return Convert.ToDecimal(Request.Form["PrecoDisparo_Gain"], _ciPtBR);
                }
            }
        }

        public decimal RequestPrecoLimiteGain
        {
            get
            {
                if (string.IsNullOrEmpty(Request.Form["PrecoLimite_Gain"]))
                {
                    return 0;       //O zero como padrão se não vier nada foi implementado pelo Kubo, verificar depois se é isso mesmo ou tem que dar erro
                }
                else
                {
                    return Convert.ToDecimal(Request.Form["PrecoLimite_Gain"], _ciPtBR);
                }
            }
        }

        public decimal RequestPrecoDisparoLoss
        {
            get
            {
                if (string.IsNullOrEmpty(Request.Form["PrecoDisparo_Loss"]))
                {
                    return 0;       //O zero como padrão se não vier nada foi implementado pelo Kubo, verificar depois se é isso mesmo ou tem que dar erro
                }
                else
                {
                    return Convert.ToDecimal(Request.Form["PrecoDisparo_Loss"], _ciPtBR);
                }
            }
        }

        public decimal RequestPrecoLimiteLoss
        {
            get
            {
                if (string.IsNullOrEmpty(Request.Form["PrecoLimite_Loss"]))
                {
                    return 0;       //O zero como padrão se não vier nada foi implementado pelo Kubo, verificar depois se é isso mesmo ou tem que dar erro
                }
                else
                {
                    return Convert.ToDecimal(Request.Form["PrecoLimite_Loss"], _ciPtBR);
                }
            }
        }
        private string ClOrdId
        {
            get { return ViewState["ClOrdId"].ToString(); }
            set { ViewState["ClOrdId"] = value;           }
        }
        #endregion

        #region Métodos Private

        private TransporteOrdemCriticas ValidaCodigoBVMF(string pBolsa)
        {
            TransporteOrdemCriticas lRetorno = new TransporteOrdemCriticas();

            if (pBolsa == "BMF")
            {
                if (this.CodigoBMFCliente == 0)
                {
                    lRetorno.MensagemCritica = string.Format("Participante desconhecido no mercado de BMF. Por favor entre em contato com o atendimento.");
                }
            }

            return lRetorno;
        }

        private string VerificaPapelBVMF(string pPapel)
        {
            string lRetorno = "BOV";

            IServicoCadastroPapeis lServico = InstanciarServicoDoAtivador<IServicoCadastroPapeis>();

            ConsultarPapelNegociadoRequest lRequest = new ConsultarPapelNegociadoRequest();

            lRequest.LstAtivos = new List<string>() { pPapel };

            ConsultarPapelNegociadoResponse lResponse = lServico.ConsultarPapelNegociado(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.LstPapelBmfInfo.Count > 0)

                    lRetorno = "BMF";

                else if (lResponse.LstPapelBovespaInfo.Count > 0)

                    lRetorno = "BOV";
            }

            return lRetorno;
        }

        private bool ValidarSePapelExiste(string pPapel)
        {
            if (string.IsNullOrEmpty(pPapel))
                return true;

            try
            {
                ConsultarSePapelExisteRequest lRequest = new ConsultarSePapelExisteRequest();

                lRequest.Ativo = pPapel;

                IServicoCadastroPapeis lServicoPapeis = InstanciarServicoDoAtivador<IServicoCadastroPapeis>();

                ConsultarSePapelExisteResponse lResponse = lServicoPapeis.ConsultarSePapelExiste(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    return lResponse.PapelExiste;
                }
                else
                {
                    gLogger.InfoFormat("Erro ao consultar se papel existe, retornando 'true': [{0}] [{1}]"
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);

                    return true;
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Exception ao consultar se papel existe, retornando 'true': [{0}] [{1}]"
                                    , ex.Message
                                    , ex.StackTrace);

                return true;
            }
        }

        private string ResponderBuscarDadosDoAtivo()
        {
            string lResposta;

            try
            {
                if (!string.IsNullOrEmpty(this.RequestPapel))
                {
                    if (ValidarSePapelExiste(this.RequestPapel))
                    {
                        lResposta = RetornarSucessoAjax(BuscarDadosDoAtivo(this.RequestPapel), "Dados carregados com sucesso.");

                        //lResposta.ObjetoDeRetorno = ;
                    }
                    else
                    {
                        lResposta = RetornarSucessoAjax(string.Format("Ativo [{0}] inexistente.", this.RequestPapel));
                    }
                }
                else
                {
                    lResposta = RetornarSucessoAjax("Favor especificar um ativo.");
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Operacoes/EnvioDeOrdens.aspx > ResponderBuscarDadosDoAtivo() [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                lResposta = RetornarErroAjax("Erro ao buscar dados do ativo", ex);
            }

            return lResposta;
        }

        private TransporteDadosDoAtivo BuscarDadosDoAtivo(string pAtivo)
        {
            TransporteDadosDoAtivo lRetorno = new TransporteDadosDoAtivo();

            lRetorno.DadosDeCotacao = BuscarDadosDeCotacaoDoAtivo(pAtivo);

            lRetorno.DadosCadastrais = new TransporteDadosCadastraisDoAtivo(pAtivo);

            lRetorno.LivroDeOferta = BuscarLivroDeOfertaDoAtivo(pAtivo);

            return lRetorno;
        }

        private TransporteMensagemDeNegocio BuscarDadosDeCotacaoDoAtivo(string pAtivo)
        {
            TransporteMensagemDeNegocio lTransporteMensagem = null;

            if (!string.IsNullOrEmpty(pAtivo))
            {
                string lMensagem;

                IServicoCotacao lServico = InstanciarServicoDoAtivador<IServicoCotacao>();

                lMensagem = lServico.ReceberTickerCotacao(pAtivo);

                lTransporteMensagem = new TransporteMensagemDeNegocio(lMensagem);
            }

            return lTransporteMensagem;
        }

        private TransporteLivroDeOferta BuscarLivroDeOfertaDoAtivo(string pAtivo)
        {
            TransporteLivroDeOferta lTransporteLivro = null;

            if (!string.IsNullOrEmpty(pAtivo))
            {
                string lMensagem;

                IServicoCotacao lServico = InstanciarServicoDoAtivador<IServicoCotacao>();

                lMensagem = lServico.ReceberLivroOferta(pAtivo);

                lTransporteLivro = new TransporteLivroDeOferta(lMensagem);

                //retira as ofertas além da décima

                for (int a = lTransporteLivro.OfertasDeCompra.Count - 1; a >= 10; a--)
                {
                    lTransporteLivro.OfertasDeCompra.RemoveAt(a);
                }

                for (int a = lTransporteLivro.OfertasDeVenda.Count - 1; a >= 10; a--)
                {
                    lTransporteLivro.OfertasDeVenda.RemoveAt(a);
                }
            }

            return lTransporteLivro;
        }

        private bool UsuarioAceitouContrato(int pIdContrato)
        {
            if (this.SessaoClienteLogado.Contratos == null)
            {
                WsCadastro.CadastroSoapClient lWsCadastro = new WsCadastro.CadastroSoapClient();

                WsCadastro.BuscarListaDeContratosRequest lRequestLista = new WsCadastro.BuscarListaDeContratosRequest();
                WsCadastro.BuscarListaDeContratosResponse lResponseLista;

                lRequestLista.CodigoBovespaDoCliente = this.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

                lResponseLista = lWsCadastro.BuscarListaDeContratos(lRequestLista);

                if (lResponseLista.StatusResposta == "OK")
                {
                    this.SessaoClienteLogado.Contratos = new List<WsCadastro.ClienteContratoInfo>(lResponseLista.Contratos);

                    string lContratos = "";

                    foreach (WsCadastro.ClienteContratoInfo lContrato in SessaoClienteLogado.Contratos)
                    {
                        lContratos += string.Format("{0}, ", lContrato.IdContrato);
                    }

                    lContratos = lContratos.Trim().TrimEnd(',');

                    gLogger.InfoFormat("Recebidos [{4}] contratos para o Cliente [CBLC: {0}] [IdCLiente: {1}] [IdLogin: {2}] > [{3}]"
                                        , SessaoClienteLogado.CodigoPrincipal
                                        , SessaoClienteLogado.IdCliente
                                        , SessaoClienteLogado.IdLogin
                                        , lContratos
                                        , SessaoClienteLogado.Contratos.Count);
                }
                else
                {
                    gLogger.ErrorFormat("Erro de resposta do WsCadastro: [{0}] [{1}]", lResponseLista.StatusResposta, lResponseLista.DescricaoResposta);

                    return false;
                }
            }

            foreach (WsCadastro.ClienteContratoInfo lContrato in this.SessaoClienteLogado.Contratos)
            {
                if (lContrato.IdContrato == pIdContrato) //ConfiguracoesValidadas.IdContrato_TermoParaRealizacaoOrdemStop)
                    return true;
            }

            return false;
        }

        private string TirarFFracionario(string pPapel)
        {
            string lRetorno = string.Empty;

            if (pPapel.Substring(pPapel.Length - 1, 1).ToLower() == "f")
            {
                lRetorno = pPapel.Substring(0, pPapel.Length - 1);
            }
            else
            {
                lRetorno = pPapel;
            }

            return lRetorno;
        }

        private string ResponderRegistrarOrdem()
        {
            string lRetorno;

            string lBolsa = this.VerificaPapelBVMF(this.RequestPapel);

            TransporteOrdemCriticas lCriticasPreValidacao = ValidaCodigoBVMF(lBolsa);

            if (!string.IsNullOrEmpty(lCriticasPreValidacao.MensagemCritica))
                return RetornarErroAjax("Há Críticas", lCriticasPreValidacao);

            if (!ValidarAssinaturaEletronica(RequestAssinatura))
                return RetornarErroAjax("Assinatura não confere");

            if (!UsuarioAceitouContrato(ConfiguracoesValidadas.IdContrato_TermoAlavancagemFinanceira))
                return RetornarErroAjax("Alavancagem não Assinada");

            // Pega referencia ao servidor de ordens
            IServicoOrdens lServicoOrdens = InstanciarServicoDoAtivador<Gradual.Core.Ordens.Lib.IServicoOrdens>();

            // Pede execução de uma ordem
            EnviarOrdemRequest lRequest;
            EnviarOrdemResponse lResponse;

            lRequest = new EnviarOrdemRequest();

            try
            {
                Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemInfo lOrdemInfo = new Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemInfo();

                if (!string.IsNullOrEmpty(RequestIdOrdem)) //--> Verifica se é alteração de Ordem
                {
                    lOrdemInfo.OrigClOrdID = RequestIdOrdem;
                }

                lOrdemInfo.Account            = this.CodigoBovespaCliente;
                lOrdemInfo.OrderQty           = this.RequestQuantidade;
                lOrdemInfo.Price              = this.RequestPreco;
                lOrdemInfo.Side               = this.RequestDirecao;
                lOrdemInfo.Symbol             = this.TirarFFracionario(this.RequestPapel);
                lOrdemInfo.ChannelID          = ConfiguracoesValidadas.Ordens_PortaDeControle.DBToInt32();
                lOrdemInfo.CompIDOMS          = "HB";   //modificado por requisição, Task 3056
                lOrdemInfo.StopPrice          = this.RequestStopStart;
                // Mercado = '1',
                // Limitada = '2',
                // StopLimitada = '4',
                // OnClose = 'A',
                // MarketWithLeftOverLimit = 'K',
                // StopStart = 'S',
                switch (this.RequestTipoOrdem)
                {
                    case "2":
                        lOrdemInfo.OrdType = OrdemTipoEnum.Limitada;
                        break;
                    default:
                        lOrdemInfo.OrdType = OrdemTipoEnum.StopLimitada;
                        break;
                }

                DateTime lFimDoDia = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                switch (this.RequestValidade)
                {
                    case OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidoAteDeterminadaData:

                        lOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidoAteDeterminadaData;
                        lOrdemInfo.ExpireDate = lFimDoDia.AddDays(30);

                        break;
                    
                    case OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaAteSerCancelada:
                        lOrdemInfo.TimeInForce = OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaAteSerCancelada;
                        //lOrdem.ExpireDate = pRequest.DtValidade;

                        break;

                    case OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ExecutaIntegralParcialOuCancela:

                        lOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ExecutaIntegralParcialOuCancela;
                        lOrdemInfo.ExpireDate = lFimDoDia;

                        break;
                    case OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ExecutaIntegralOuCancela:

                        lOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ExecutaIntegralParcialOuCancela;
                        lOrdemInfo.ExpireDate = lFimDoDia;

                        break;
                    
                    case OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.BoaParaLeilao:

                        lOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.BoaParaLeilao;
                        lOrdemInfo.ExpireDate = lFimDoDia;

                        break;

                    default:

                        lOrdemInfo.TimeInForce = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia;
                        lOrdemInfo.ExpireDate = lFimDoDia;

                        break;
                }


                lRequest.ClienteOrdemInfo = lOrdemInfo;

                lResponse = lServicoOrdens.EnviarOrdem(lRequest);

                if (lResponse.StatusResposta == Gradual.Core.Ordens.Lib.CriticaRiscoEnum.Sucesso)
                {
                    lRetorno = RetornarSucessoAjax(lResponse, "Ordem registrada com sucesso.");

                    this.ClOrdId = null;
                }
                else
                {
                    TransporteOrdemCriticas lCriticas = new TransporteOrdemCriticas();

                    lCriticas.MensagemCritica = lResponse.DescricaoResposta;

                    if (lResponse.CriticaInfo != null)
                    {
                        foreach (Gradual.Core.Ordens.Lib.PipeLineCriticaInfo critica in lResponse.CriticaInfo)
                        {
                            lCriticas.DataHoras.Add(critica.DataHoraCritica.ToString("dd/MM/yyyy HH:mm:ss"));

                            lCriticas.Criticas.Add(critica.Critica);
                        }
                    }

                    lRetorno = RetornarErroAjax("Há Críticas", lCriticas);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Operacoes/EnvioDeOrdens.aspx > ResponderRegistrarOrdem() [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                lRetorno = RetornarErroAjax("Erro ao enviar ordem", ex);
            }

            return lRetorno;
        }

        private string ResponderCancelarOrdem()
        {
            string lRetorno = string.Empty;

            //if (!base.ValidarAssinaturaEletronica(RequestAssinatura))
            //    return RetornarErroAjax(CONST_ERRO_ASSINATURA_NAO_CONFERE);

            try
            {
                IServicoOrdens lOrdem = InstanciarServicoDoAtivador<Gradual.Core.Ordens.Lib.IServicoOrdens>();

                EnviarCancelamentoOrdemRequest lRequest = new EnviarCancelamentoOrdemRequest();

                ExecutarCancelamentoOrdemResponse lResponse = new ExecutarCancelamentoOrdemResponse();

                OrdemCancelamentoInfo lInfo = new OrdemCancelamentoInfo();

                //lInfo.OrderID     = this.RequestIdOrdem;
                lInfo.OrigClOrdID = this.RequestIdOrdem;
                lInfo.ChannelID   = ConfiguracoesValidadas.Ordens_PortaDeControle.DBToInt32();
                lInfo.Account     = SessaoClienteLogado.CodigoPrincipal.DBToInt32();
                lInfo.Symbol      = this.RequestPapel;
                
                lRequest.ClienteCancelamentoInfo = lInfo;

                //logger.Debug("Solicitando cancelamento IDOrdem=" + this.RequestIdOrdem);

                lResponse = lOrdem.CancelarOrdem(lRequest);

                if (lResponse.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                {
                    lRetorno = RetornarSucessoAjax("Ordem cancelada com sucesso.");
                }
                else
                {
                    lRetorno = RetornarErroAjax("Erro no cancelamento da ordem");
                }

                try
                {
                    Ativador.AbortChannel(lOrdem);
                }
                catch (Exception ax)
                {
                    gLogger.ErrorFormat("Erro do Ativador.AbortChannel em Boleta.aspx - ResponderCancelarOrdem() [{0}]", ax.Message);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Operacoes/EnvioDeOrdens.aspx > ResponderCancelarOrdem() [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                lRetorno = RetornarErroAjax("Erro ao tentar cancelar ordem.", ex.Message + "-" + ex.StackTrace);
            }

            return lRetorno;
        }

       

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.ValidarSessao())
            {
                if (!this.IsPostBack)
                {
                    if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                    {
                        base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                    }

                    AcompanhamentoOrdens lAcompanhamento = Context.Handler as AcompanhamentoOrdens;

                    if (lAcompanhamento != null &&  !string.IsNullOrEmpty(lAcompanhamento.ClOrdId))
                    {
                        List<OrdemInfo> lOrdem =  base.GetOrdem(lAcompanhamento.ClOrdId);

                        if (lOrdem != null && lOrdem.Count > 0)
                        {
                            this.ClOrdId = lOrdem[0].ClOrdID;
                            string pOrdem = RetornarSucessoAjax(lOrdem[0],"TemOrdem");
                            base.RodarJavascriptOnLoad("Ordem_PreencheBoletaAlteracao(" + pOrdem + ");");
                        }
                    }

                    RegistrarRespostasAjax(new string[]
                                                {
                                                    "BuscarDadosDoAtivo",
                                                    "RegistrarOrdem",
                                                    "CancelarOrdem",
                                                },
                            new ResponderAcaoAjaxDelegate[]
                                                {
                                                    ResponderBuscarDadosDoAtivo,
                                                    ResponderRegistrarOrdem,
                                                    ResponderCancelarOrdem,
                                                });
                }
            }
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            //this.PaginaMaster.Secao = new SecaoDoSite("MinhaConta", "MinhaConta");

            //this.PaginaMaster.SubSecao = new SecaoDoSite("Operacoes", HostERaizFormat("MinhaConta/Operacoes/EnvioDeOrdens.aspx"));
        }

        #endregion
    }
}