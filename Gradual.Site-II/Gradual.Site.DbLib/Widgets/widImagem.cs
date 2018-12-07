using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.DbLib.Widgets
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

        public override string Renderizar(byte pRenderizandoNaAba)
        {
            OnMensagemDeWidget("widImagem[{0}].Renderizar({1})", this.IdDoWidget, this.IdDaEstrutura);

            string lRetorno = "";

            //this.FlagHabilitarZoom = true;

            //this.AtributoWidth = "150";
            //this.AtributoHeight = "100";

            string lPrefixo = (pRenderizandoNaAba == 0) ? "" : "_";

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

            lRetorno += string.Format("<img id='{8}widImagem-{0}-{1}' src='{2}' alt='{3}'{4}{5} class='{6}' style='{7}' />"
                                      , this.IdDaEstrutura
                                      , this.IdDoWidget
                                      , this.AtributoSrc
                                      , this.AtributoAlt
                                      , string.IsNullOrEmpty(this.AtributoWidth)  ? "" : " width='"  + this.AtributoWidth  + "'"
                                      , string.IsNullOrEmpty(this.AtributoHeight) ? "" : " height='" + this.AtributoHeight + "'"
                                      , this.AtributoClass
                                      , this.AtributoStyle
                                      , lPrefixo
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
            
            OnMensagemDeWidget("widImagem[{0}] Renderizado com [{1}] caracteres.", this.IdDoWidget, lRetorno.Length);

            return lRetorno;
        }

        #endregion
    }
}