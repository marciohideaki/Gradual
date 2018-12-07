using System;


namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class MapaDoSite : UserControlBase
    {
        #region Propriedades

        public string HostERaiz
        {
            get
            {
                return this.PaginaBase.HostERaiz;
            }
        }
        
        public string HostERaizHttps
        {
            get
            {
                return this.PaginaBase.HostERaizHttps;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ConfigurarTela();
        }

        #region Métodos Private

        private void ConfigurarTela()
        {
            if (this.PaginaBase.SessaoClienteLogado == null)
            {
                this.lblMapaSiteComoInvestirAbraSuaConta.Text =
                this.lblMapaSiteMinhaContaCadastroMeuCadastro.Text = "Abra sua Conta";
            }
            else if (this.PaginaBase.SessaoClienteLogado.Passo < 3)
            {
                this.lblMapaSiteComoInvestirAbraSuaConta.Text =
                this.lblMapaSiteMinhaContaCadastroMeuCadastro.Text = "Complete seu Cadastro";
            }
            else
            {
                this.lblMapaSiteComoInvestirAbraSuaConta.Text =
                this.lblMapaSiteMinhaContaCadastroMeuCadastro.Text = "Meu Cadastro";
            }
        }

        #endregion
    }
}