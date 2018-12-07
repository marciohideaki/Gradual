using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.DbLib.Widgets
{
    public class widEmbed : WidgetBase
    {
        #region Propriedades

        public string Codigo { get; set; }

        #endregion

        #region Métodos Públicos

        public override string Renderizar(byte pRenderizandoNaAba)
        {
            OnMensagemDeWidget("widEmbed[{0}].Renderizar({1})", this.IdDoWidget, this.IdDaEstrutura);

            string lRetorno;

            //é melhor deixar os atributos class e style sempre visíveis mesmo sem nada dentro pro js não pegar como "undefined', e sim como vazio, pra não estragar a comparação de json que verifica se o objeto teve alterações
            
            string lPrefixo = (pRenderizandoNaAba == 0) ? "" : "_";

            lRetorno = string.Format("<div id='{5}widEmbed-{0}-{1}' class='{2}' style='{3}' />{4}</div>"
                                      , this.IdDaEstrutura
                                      , this.IdDoWidget
                                      , this.AtributoClass
                                      , this.AtributoStyle
                                      , this.Codigo
                                      , lPrefixo
                                      );

            OnMensagemDeWidget("widEmbed[{0}] Renderizado com [{1}] caracteres.", this.IdDoWidget, lRetorno.Length);

            return lRetorno;
        }

        #endregion
    }
}