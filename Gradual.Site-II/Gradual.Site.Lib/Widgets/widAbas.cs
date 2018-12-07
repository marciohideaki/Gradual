using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Gradual.Site.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Site.Lib.Mensagens;
using Gradual.Site.Lib.Dados;
using Newtonsoft.Json;

namespace Gradual.Site.Lib.Widgets
{
    public class widAbas : WidgetBase
    {
        #region Propriedades

        public string ListaDeAbas { get; set; }

        [JsonIgnore]
        private string AbasRenderizadas { get; set; }

        [JsonIgnore]
        private string ConteudosRenderizados { get; set; }

        #endregion

        #region Métodos Private

        private string RenderizarAbas()
        {
            StringBuilder lBuilder = new StringBuilder();

            string[] lAbas = this.ListaDeAbas.Split("+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            string[] lDados;

            string lClasse = "class='ativo'";

            for (int a = 0; a < lAbas.Length; a++)
            {
                lDados = lAbas[a].Split("@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (lDados.Length == 2 && !string.IsNullOrEmpty(lDados[0]) && !string.IsNullOrEmpty(lDados[1]))
                {
                    lBuilder.AppendFormat("<li onclick='return lnkAba_Conteudo_Click(this)' data-IdConteudo='{0}' {2}> <a href='#'>{1}</a> </li>", lDados[0], lDados[1], lClasse);
                }

                lClasse = "";
            }

            return lBuilder.ToString();
        }
        
        private string RenderizarConteudos()
        {
            StringBuilder lBuilder = new StringBuilder();

            //IServicoPersistenciaSite 

            string[] lAbas = this.ListaDeAbas.Split("+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            string[] lDados;

            string lStyle = "";

            for (int a = 0; a < lAbas.Length; a++)
            {
                lDados = lAbas[a].Split("@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (lDados.Length == 2 && !string.IsNullOrEmpty(lDados[0]) && !string.IsNullOrEmpty(lDados[1]))
                {
                    lBuilder.AppendFormat("<div data-IdConteudo='{0}' {2}> Conteudo [{1}] </div>", lDados[0], lDados[1], lStyle);
                }

                lStyle = " style='display:none' ";
            }

            return lBuilder.ToString();
        }
        #endregion

        #region Métodos Públicos

        public override string Renderizar()
        {
            string lRetorno;

            this.AbasRenderizadas = RenderizarAbas();

            this.ConteudosRenderizados = RenderizarConteudos();

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<ul id='widAbas-{0}-{1}' class='menu-tabs {2}' style='{3}' data-ListaDeAbas='{6}'>\r\n{4}\r\n</ul>{5}"
                                          , this.IdDaEstrutura
                                          , this.IdDoWidget
                                          , this.AtributoClass
                                          , this.AtributoStyle
                                          , this.AbasRenderizadas
                                          , this.ConteudosRenderizados
                                          , this.ListaDeAbas
                                        );
            }
            else
            {
                lRetorno = string.Format("<ul id='widAbas-{0}-{1}' class='menu-tabs {2}' style='{3}'>\r\n{4}\r\n</ul>{5}"
                                          , this.IdDaEstrutura
                                          , this.IdDoWidget
                                          , this.AtributoClass
                                          , this.AtributoStyle
                                          , this.AbasRenderizadas
                                          , this.ConteudosRenderizados
                                        );
            }

            return lRetorno;
        }

        #endregion
    }
}