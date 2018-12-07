using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;
using Gradual.Site.Www.Transporte;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos
{
    public partial class OperacoesAplicar : System.Web.UI.UserControl
    {
        #region Evento
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var lBase = (PaginaBase)this.Page;

                if (string.IsNullOrEmpty(lBase.SessaoClienteLogado.CodigoPrincipal))
                {
                    lBase.ExibirMensagemJsOnLoad("I", "Você ainda não possui código de conta Gradual, para acessar essa opção finalize seu cadastro");
                    return;
                }
                this.CarregarDados();
            }
        }
        #endregion

        #region Métodos
        public void CarregarDados()
        {
            PaginaFundos lServico = new PaginaFundos();
            List<Transporte_IntegracaoFundos> ListaFundos = lServico.PesquisarFundosAplicar(new PesquisarIntegracaoFundosRequest());

            this.rptListaDeOperacoesAplicar.DataSource = ListaFundos;
            this.rptListaDeOperacoesAplicar.DataBind();
        }
        #endregion
    }
}