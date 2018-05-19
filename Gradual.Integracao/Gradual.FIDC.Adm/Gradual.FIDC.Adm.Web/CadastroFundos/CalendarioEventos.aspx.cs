using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.Web.App_Codigo.Transporte;
using Gradual.OMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.FIDC.Adm.Web.CadastroFundos
{
    public partial class CalendarioEventos : PaginaBase
    {
        #region Propriedades

        private int GetIdCalendarioEvento
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["IdCalendarioEvento"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private int GetIdFundoCadastro
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["IdFundoCadastro"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private string GetDescEvento
        {
            get { return !string.IsNullOrEmpty(Request["DescEvento"]) ? Request["DescEvento"] : string.Empty; }
        }

        private DateTime GetDtEvento
        {
            get { return !string.IsNullOrEmpty(Request["DtEvento"]) ? Convert.ToDateTime(Request["DtEvento"]) : DateTime.Now; }
        }

        private DateTime GetDtEventoEnd
        {
            get { return !string.IsNullOrEmpty(Request["DtEventoEnd"]) ? Convert.ToDateTime(Request["DtEventoEnd"]) : DateTime.MinValue; }
        }

        private string GetEmailEvento
        {
            get { return !string.IsNullOrEmpty(Request["EmailEvento"]) ? Request["EmailEvento"] : string.Empty; }
        }
        
        private bool GetEnviarNotificacaoDia
        {
            get { return !string.IsNullOrEmpty(Request["EnviarNotificacaoDia"]) ? Convert.ToBoolean(Request["EnviarNotificacaoDia"]) : false; }
        }

        private bool GetMostrarHome
        {
            get { return !string.IsNullOrEmpty(Request["MostrarHome"]) ? Convert.ToBoolean(Request["MostrarHome"]) : false; }
        }

        #endregion

        #region Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            try
            {
                base.Page_Load(sender, e);

                RegistrarRespostasAjax(new[]
                    {
                        "CarregarHtmlComDados",
                        "GetFundos",
                        "AddEvento",
                        "RemoverEvento"
                    },
                    new ResponderAcaoAjaxDelegate[]
                    {
                        ResponderCarregarHtmlComDados,
                        ResponderGetFundos,
                        ResponderAddEvento,
                        ResponderRemoverEvento
                    });

                CarregarDadosIniciais();
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de fundos na tela", ex);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carregar dados iniciais da página de eventos
        private void CarregarDadosIniciais()
        {
            if (Page.IsPostBack) return;
            TituloDaPagina = "Calendário de Eventos";
            LinkPreSelecionado = "lnkTL_CalendarioEventos";
        }

        /// <summary>
        /// Carrega dados do grid de eventos
        /// </summary>
        /// <returns>JSON do grid</returns>
        public string ResponderCarregarHtmlComDados()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new CalendarioEventoRequest()
                {
                    DtEvento = GetDtEvento,
                    DtEventoEnd = GetDtEventoEnd,
                    IdFundoCadastro = GetIdFundoCadastro
                };

                var lResponse = BuscarCalendarioEventos(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteCalendarioEventos().TraduzirLista(lResponse.ListaEventos);

                    var lRetornoLista = new TransporteDeListaPaginada(lListaTransporte)
                    {
                        TotalDeItens = lResponse.ListaEventos.Count,
                        PaginaAtual = 1,
                        TotalDePaginas = 0
                    };

                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de eventos na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarHtmlComDados ", ex);
            }

            return lRetorno;
        }

        public string ResponderAddEvento()
        {
            try
            {
                var lRequest = new CalendarioEventoRequest
                {
                    IdFundoCadastro = GetIdFundoCadastro,
                    DtEvento = GetDtEvento,
                    DescEvento = GetDescEvento,
                    EmailEvento = GetEmailEvento,
                    EnviarNotificacaoDia = GetEnviarNotificacaoDia,
                    MostrarHome = GetMostrarHome,
                    DescricaoUsuarioLogado = UsuarioLogado.Nome
                };

                #region Gravação Log4Net

                var mensagemLog = string.Empty;

                mensagemLog += "IdFundoCadastro:" + lRequest.IdFundoCadastro + ";";
                mensagemLog += "DtEvento:" + lRequest.DtEvento + ";";
                mensagemLog += "DescEvento:" + lRequest.DescEvento + ";";
                mensagemLog += "EmailEvento:" + lRequest.EmailEvento + ";";
                mensagemLog += "EnviarNotificacaoDia:" + lRequest.EnviarNotificacaoDia + ";";
                mensagemLog += "MostrarHome:" + lRequest.MostrarHome + ";";
                mensagemLog += "TipoTransacao:INSERT;";
                mensagemLog += "UsuarioTransacao:" + lRequest.DescricaoUsuarioLogado + ";";

                Logger.Info(mensagemLog);

                #endregion

                var lResponse = IncluirCalendarioEvento(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    return RetornarSucessoAjax(lResponse.StatusResposta.ToString());
                }
                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.ErroNegocio)
                {
                    //erro de negócio
                    return RetornarErroAjax(lResponse.DescricaoResposta);
                }

                return RetornarErroAjax("Erro ao cadastrar evento do calendário.");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao cadastrar evento do calendário", ex);

                return RetornarErroAjax("Erro no método ResponderAddEvento", ex);
            }
        }

        public string ResponderRemoverEvento()
        {
            try
            {
                var lRequest = new CalendarioEventoRequest
                {
                    IdCalendarioEvento = GetIdCalendarioEvento
                };

                #region Gravação Log4Net

                var mensagemLog = string.Empty;

                mensagemLog += "IdCalendarioEvento:" + lRequest.IdFundoCadastro + ";";
                mensagemLog += "TipoTransacao:DELETE;";
                mensagemLog += "UsuarioTransacao:" + lRequest.DescricaoUsuarioLogado + ";";

                Logger.Info(mensagemLog);

                #endregion

                var lResponse = RemoverCalendarioEvento(lRequest); //RemoverCalendarioEvento

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    return RetornarSucessoAjax(lResponse.StatusResposta.ToString());
                }
                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.ErroNegocio)
                {
                    //erro de negócio
                    return RetornarErroAjax(lResponse.DescricaoResposta);
                }

                return RetornarErroAjax("Erro ao cadastrar evento do calendário.");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao cadastrar evento do calendário", ex);

                return RetornarErroAjax("Erro no método ResponderAddEvento", ex);
            }
        }

        /// <summary>
        /// Carrega select de fundos
        /// </summary>
        /// <returns></returns>
        public string ResponderGetFundos()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new CadastroFundoRequest { };

                var lResponse = BuscarFundosCadastrados(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaFundos.OrderBy(p => p.NomeFundo));

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar lista de fundos na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarSelectFundos ", ex);
            }

            return lRetorno;
        }

        #endregion
    }
}