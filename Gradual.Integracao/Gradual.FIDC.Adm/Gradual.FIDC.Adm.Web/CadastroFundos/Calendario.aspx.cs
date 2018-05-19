using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.OMS.Library;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.UI;

namespace Gradual.FIDC.Adm.Web.CadastroFundos
{
    public partial class Calendario : PaginaBase
    {
        #region Propriedades

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
                        "GetEventos"
                    },
                    new ResponderAcaoAjaxDelegate[]
                    {
                        ResponderGetEventos
                    });

                CarregarDadosIniciais();
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de fundos na tela", ex);
            }
        }

        /// <summary>
        /// Carregar dados iniciais da página de eventos
        private void CarregarDadosIniciais()
        {
            if (Page.IsPostBack) return;
            TituloDaPagina = "Calendário";
            LinkPreSelecionado = "lnkTL_Calendario";
        }

        /// <summary>
        /// Carrega select de eventos
        /// </summary>
        /// <returns></returns>
        public string ResponderGetEventos()
        {
            var lRetorno = string.Empty;

            try
            {
                var lResponse = BuscarCalendarioEventos(new CalendarioEventoRequest { });
                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaEventos.OrderBy(p => p.DtEvento));
                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar lista de eventos na tela", ex);
                lRetorno = RetornarErroAjax("Erro no método ResponderGetEventos ", ex);
            }

            return lRetorno;
        }

        #endregion
    }
}