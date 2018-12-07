using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Lib.Widgets
{
    public class widEmbed : WidgetBase
    {
        #region Propriedades

        public string Codigo { get; set; }

        #endregion

        #region Métodos Públicos

        public override string Renderizar()
        {
            string lRetorno;

            //é melhor deixar os atributos class e style sempre visíveis mesmo sem nada dentro pro js não pegar como "undefined', e sim como vazio, pra não estragar a comparação de json que verifica se o objeto teve alterações

            lRetorno = string.Format("<div id='widEmbed-{0}-{1}' class='{2}' style='{3}' />{4}</div>"
                                      , this.IdDaEstrutura
                                      , this.IdDoWidget
                                      , this.AtributoClass
                                      , this.AtributoStyle
                                      , this.Codigo
                                      );

            return lRetorno;
        }

        #endregion
    }
}