using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Gradual.Site.DbLib.Widgets;

namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class RenderizadorDeWidgets : UserControlBase
    {
        #region Propriedades

        public int IdDaPagina { get; set; }

        public List<WidgetBase> Widgets { get; set; }

        public string ConteudoHTML { get; set; }

        public string HostERaiz { get; set; }

        public string VersaoDaEstrutura { get; set; }

        public bool RenderizarHabilitandoCMS { get; set; }

        public bool ExisteUsuarioLogado { get; set; }

        #endregion

        #region Métodos Públicos

        public override void DataBind()
        {
            if (this.Widgets == null)
            {
                lblWidgets.Text = this.ConteudoHTML;
            }
            else
            {
                StringBuilder lBuilder = new StringBuilder();

                foreach (WidgetBase lWidget in this.Widgets)
                {
                    if (lWidget != null)
                    {
                        lWidget.MensagemDeWidget += new MensagemDeWidgetEventHandler(lWidget_MensagemDeWidget);

                        lWidget.RenderizarHabilitandoCMS = this.RenderizarHabilitandoCMS;

                        lWidget.ExisteUsuarioLogado = this.ExisteUsuarioLogado;

                        lWidget.HostERaiz = this.HostERaiz;

                        lWidget.VersaoDaEstrutura = this.VersaoDaEstrutura;

                        lBuilder.AppendLine(lWidget.Renderizar(0));
                    }
                }

                lblWidgets.Text = lBuilder.ToString();

                this.ConteudoHTML = lblWidgets.Text;    //pra refletir na propriedade e poder ser guardado pelo cache de página
            }
        }

        void lWidget_MensagemDeWidget(object pSender, DateTime pEventDate, string pMensagem)
        {
            base.PaginaBase.MarcarPerformanceMonitor(pMensagem);
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion
    }
}