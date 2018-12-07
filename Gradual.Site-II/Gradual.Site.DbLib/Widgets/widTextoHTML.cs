using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gradual.Site.DbLib.Widgets
{
    public class widTextoHTML : WidgetBase
    {
        #region Propriedades

        public string ConteudoHTML { get; set; }

        #endregion

        #region Métodos Private

        #endregion

        #region Métodos Públicos

        public override string Renderizar(byte pRenderizandoNaAba)
        {
            OnMensagemDeWidget("widTextoHTML[{0}].Renderizar({1})", this.IdDoWidget, this.IdDaEstrutura);

            string lRetorno;

            //é melhor deixar os atributos class e style sempre visíveis mesmo sem nada dentro pro js não pegar como "undefined', e sim como vazio, pra não estragar a comparação de json que verifica se o objeto teve alterações

            string lPrefixo = (pRenderizandoNaAba != 0) ? "_" : "";

            string lConteduoAjustado = base.AjustarReferenciasDeLinksNoHTML(this.ConteudoHTML);

            lRetorno = string.Format("<div id='{5}widTextoHTML-{0}-{1}' class='{2}' style='{3}' />{4}</div>"
                                      , this.IdDaEstrutura
                                      , this.IdDoWidget
                                      , this.AtributoClass
                                      , this.AtributoStyle
                                      , lConteduoAjustado
                                      , lPrefixo
                                      );

            OnMensagemDeWidget("widTextoHTML[{0}] Renderizado com [{1}] caracteres.", this.IdDoWidget, lRetorno.Length);

            return lRetorno;
        }

        #endregion
    }
}
