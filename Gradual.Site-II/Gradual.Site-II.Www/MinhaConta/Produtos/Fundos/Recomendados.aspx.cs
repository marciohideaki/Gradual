using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;
using Gradual.Site.Www.Transporte;

namespace Gradual.Site.Www.MinhaConta.Produtos.Fundos
{
    public partial class Recomendados : PaginaFundos
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.ValidarSessao())
            {
                if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                {
                    base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                    return;
                }

                this.CarregarDados();
            }
        }
        #endregion

        #region Métodos
        public void CarregarDados()
        {
            List<Transporte_IntegracaoFundos> ListaFundos = base.PesquisarFundosSuitability(new PesquisarIntegracaoFundosRequest() 
            {
                IdPerfilSuitability = base.GetIdPerfilSuitability
            });

            this.rptListaDeRecomendados.DataSource = ListaFundos;
            this.rptListaDeRecomendados.DataBind();
        }
        #endregion

    }
}