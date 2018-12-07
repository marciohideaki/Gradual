using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.DbLib.Dados;
using Gradual.Site.DbLib.Widgets;

namespace Gradual.Site.Www
{
    public partial class Busca : PaginaBase
    {
        #region Métodos Private
        
        private void BuscarConteudo()
        {
            string lMensagem;

            string lTermo = Request["termo"].ToLower();

            try
            {
                List<int> lIDsDasPaginas = new List<int>();

                int lLimite = 20;
                
                List<TransportePaginaConteudoInfo_Busca> lResultados = new List<TransportePaginaConteudoInfo_Busca>();

                TransportePaginaConteudoInfo_Busca lInfo;

                foreach (string lChaveCache in this.CacheDePaginas.Keys)
                {
                    if (this.CacheDePaginas[lChaveCache].ToLower().Contains(lTermo))
                    {
                        lIDsDasPaginas.Add(Convert.ToInt32(lChaveCache.Substring(0, lChaveCache.IndexOf('-'))));

                        if (lIDsDasPaginas.Count >= lLimite)
                            break;
                    }
                }

                foreach (PaginaInfo lPagina in this.ListaDePaginas)
                {
                    foreach (int lIdPagina in lIDsDasPaginas)
                    {
                        if(lPagina.CodigoPagina == lIdPagina)
                        {
                            lInfo = new TransportePaginaConteudoInfo_Busca();

                            lInfo.NomePagina = lPagina.NomePagina;
                            lInfo.DescURL = lPagina.DescURL;

                            lResultados.Add(lInfo);

                            break;
                        }
                    }
                }

                if(lResultados.Count > 0)
                {
                    this.rptConteudo.DataSource = lResultados;
                    this.rptConteudo.DataBind();
                    
                    lblNenhumItem.Visible = false;
                }
                else
                {
                    lblNenhumItem.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lMensagem = string.Format("Erro em Busca.aspx > BuscarConteudo(pTermo [{0}]) : [{1}]\r\n{2}"
                                          , lTermo
                                          , ex.Message
                                          , ex.StackTrace);
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            BuscarConteudo();
        }

        #endregion
    }
}