using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.MinhaConta.Financeiro.Informe
{
    public partial class Custodia : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (base.ValidarSessao())
                {
                    if (!this.IsPostBack)
                    {
                        //this.txtDataInicial.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        //this.txtData.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                        //this.BuscarRelatorio();

                        this.CarregaCombo();
                    }
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
            }
        }
        
        private void CarregaCombo()
        {
            cboAnoDeExercicio.Items.Clear();

            ListItem lAno;

            for (int f = DateTime.Now.Year - 1; f > DateTime.Now.Year - 6; f--)
            {
                lAno = new ListItem(f.ToString(), f.ToString());

                cboAnoDeExercicio.Items.Add(lAno);
            }

            cboAnoDeExercicio.SelectedIndex = 0;
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            this.PaginaMaster.BreadCrumbVisible = true;

            this.PaginaMaster.Crumb1Text = "Minha Conta";
            this.PaginaMaster.Crumb2Text = "Financeiro";
            this.PaginaMaster.Crumb3Text = "Informe de Rendimentos";
        }
    }
}