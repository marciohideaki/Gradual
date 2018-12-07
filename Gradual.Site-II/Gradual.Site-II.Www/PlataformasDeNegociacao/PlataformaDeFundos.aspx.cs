using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;
using Gradual.Site.Www.Transporte;

namespace Gradual.Site.Www.PlataformasDeNegociacao
{
    public partial class PlataformaDeFundos : PaginaFundos
    {
        #region Métodos private

        private void CarregarDadosComPerfil()
        {
            List<Transporte_IntegracaoFundos> ListaFundos = base.PesquisarFundosSuitability(new PesquisarIntegracaoFundosRequest() 
            {
                IdPerfilSuitability = base.GetIdPerfilSuitability
            });

            this.rptListaDeRecomendados.DataSource = (from i in ListaFundos orderby i.Fundo select i);
            this.rptListaDeRecomendados.DataBind();

            tdNenhumRecomendado.Visible = (ListaFundos.Count == 0);
        }

        private void CarregarDados()
        {
            // renda fixa:
            
            List<Transporte_IntegracaoFundos> ListaFundos = base.PesquisarFundos(new PesquisarIntegracaoFundosRequest()
            {
                IdCategoria = 1
            });

            this.rptListaDeRendaFixa.DataSource = (from i in ListaFundos orderby i.Fundo select i);
            this.rptListaDeRendaFixa.DataBind();

            tdNenhumRendaFixa.Visible = (ListaFundos.Count == 0);

            // multi mercado:
            
            ListaFundos = base.PesquisarFundos(new PesquisarIntegracaoFundosRequest()
            {
                IdCategoria = 2
            });

            this.rptListaDeMultimercado.DataSource = (from i in ListaFundos orderby i.Fundo select i);
            this.rptListaDeMultimercado.DataBind();
            
            tdNenhumMultimercado.Visible = (ListaFundos.Count == 0);

            // ações:
            
            ListaFundos = base.PesquisarFundos(new PesquisarIntegracaoFundosRequest()
            {
                IdCategoria = 3
            });

            this.rptListaDeAcoes.DataSource = (from i in ListaFundos orderby i.Fundo select i);
            this.rptListaDeAcoes.DataBind();

            tdNenhumAcoes.Visible = (ListaFundos.Count == 0);

            // referenciado:

            ListaFundos = base.PesquisarFundos(new PesquisarIntegracaoFundosRequest()
            {
                IdCategoria = 12
            });

            this.rptListaDeReferenciado.DataSource = (from i in ListaFundos orderby i.Fundo select i);
            this.rptListaDeReferenciado.DataBind();
            
            tdNenhumReferenciado.Visible = (ListaFundos.Count == 0);

            // cambial:

            ListaFundos = base.PesquisarFundos(new PesquisarIntegracaoFundosRequest()
            {
                IdCategoria = 13
            });

            this.rptListaDeCambial.DataSource = (from i in ListaFundos orderby i.Fundo select i);
            this.rptListaDeCambial.DataBind();

            tdNenhumCambial.Visible = (ListaFundos.Count == 0);

            // exterior:

            ListaFundos = base.PesquisarFundos(new PesquisarIntegracaoFundosRequest()
            {
                IdCategoria = 14
            });

            this.rptListaDeExterior.DataSource = (from i in ListaFundos orderby i.Fundo select i);
            this.rptListaDeExterior.DataBind();
            
            tdNenhumExterior.Visible = (ListaFundos.Count == 0);
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (SessaoClienteLogado != null)
            if(base.ValidarSessao())
            {
                CarregarDadosComPerfil();
            }
            else
            {
                ulAbas.Attributes["class"] = "menu-tabs menu-tabs-full menu-tabs-7";
                
                liRecomendados.Visible = false;
                pnlRecomendados.Visible = false;

                liRendaFixa.Attributes["class"] = "ativo";
                pnlTabelaRendafixa.Attributes["style"] = "";
            }

            CarregarDados();
        }
    }
}