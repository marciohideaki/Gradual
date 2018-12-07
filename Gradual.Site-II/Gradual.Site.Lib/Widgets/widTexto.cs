using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Lib.Widgets
{
    public class widTexto : WidgetBase
    {
        #region Propriedades

        public string Texto { get; set; }

        #endregion

        #region Métodos Públicos

        public override string Renderizar()
        {
            //é melhor deixar os atributos class e style sempre visíveis mesmo sem nada dentro pro js não pegar como "undefined', e sim como vazio, pra não estragar a comparação de json que verifica se o objeto teve alterações

            string lTextoRenderizado = base.ProcessarTextoParaHTML(this.Texto);

            string lRetorno;

            lTextoRenderizado = lTextoRenderizado.Replace(Environment.NewLine, "<br />");

            lTextoRenderizado = lTextoRenderizado.Replace("\n", "<br />");

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<p id='widTexto-{0}-{1}' style='{2}'>{3}<input type='hidden' class='TextoOriginal' value='{4}'/></p>"
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoStyle
                                            , lTextoRenderizado
                                            , this.Texto
                                        );
            }
            else
            {
                lRetorno = string.Format("<p id='widTexto-{0}-{1}' style='{2}'>{3}</p>"
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoStyle
                                            , lTextoRenderizado
                                        );
            }

            return lRetorno;
        }

        #endregion
    }
}