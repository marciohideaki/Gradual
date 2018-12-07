using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Lib.Widgets
{
    public class widImagem : WidgetBase
    {
        #region Propriedades

        public string AtributoSrc { get; set; }

        public string AtributoAlt { get; set; }

        public string LinkPara { get; set; }

        public bool FlagTamanhoAutomatico { get; set; }

        public string AtributoWidth { get; set; }

        public string AtributoHeight { get; set; }

        public bool FlagHabilitarZoom { get; set; }

        #endregion

        #region Métodos Públicos

        public override string Renderizar()
        {
            string lRetorno = "";

            //this.FlagHabilitarZoom = true;

            //this.AtributoWidth = "150";
            //this.AtributoHeight = "100";

            if (this.FlagHabilitarZoom)
            {
                lRetorno += "<a href='#' class='ContainerDeZoom'>";
            }
            else
            {
                if (!string.IsNullOrEmpty(this.LinkPara))
                {
                    lRetorno += string.Format("<a href='{0}' class='LinkDeImagem'>", this.LinkPara);
                }
            }
            
            //é melhor deixar os atributos class e style sempre visíveis mesmo sem nada dentro pro js não pegar como "undefined', e sim como vazio, pra não estragar a comparação de json que verifica se o objeto teve alterações

            lRetorno += string.Format("<img id='widImagem-{0}-{1}' src='{2}' alt='{3}'{4}{5} class='{6}' style='{7}' />"
                                      , this.IdDaEstrutura
                                      , this.IdDoWidget
                                      , this.AtributoSrc
                                      , this.AtributoAlt
                                      , string.IsNullOrEmpty(this.AtributoWidth)  ? "" : " width='"  + this.AtributoWidth  + "'"
                                      , string.IsNullOrEmpty(this.AtributoHeight) ? "" : " height='" + this.AtributoHeight + "'"
                                      , this.AtributoClass
                                      , this.AtributoStyle
                                      );
            
            if (this.FlagHabilitarZoom)
            {
                lRetorno += "<span class='IconeDeZoom'></span></a>";
            }
            else
            {
                if (!string.IsNullOrEmpty(this.LinkPara))
                {
                    lRetorno += "</a>";
                }
            }

            return lRetorno;
        }

        #endregion
    }
}