using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.DbLib.Widgets
{
    public class widTexto : WidgetBase
    {
        #region Propriedades

        public string Texto { get; set; }

        #endregion

        #region Métodos Públicos

        public override string Renderizar(byte pRenderizandoNaAba)
        {
            OnMensagemDeWidget("widTexto[{0}].Renderizar({1})", this.IdDoWidget, this.IdDaEstrutura);

            //é melhor deixar os atributos class e style sempre visíveis mesmo sem nada dentro pro js não pegar como "undefined', e sim como vazio, pra não estragar a comparação de json que verifica se o objeto teve alterações

            string lTextoRenderizado = base.ProcessarTextoParaHTML(this.Texto);

            string lRetorno;

            string lPrefixo = (pRenderizandoNaAba == 0) ? "" : "_";

            lTextoRenderizado = lTextoRenderizado.Replace(Environment.NewLine, "<br />");

            lTextoRenderizado = lTextoRenderizado.Replace("\n", "<br />");

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<p id='{5}widTexto-{0}-{1}' style='{2}'>{3}<input type='hidden' class='TextoOriginal' value='{4}'/></p>"
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoStyle
                                            , lTextoRenderizado
                                            , this.Texto
                                            , lPrefixo
                                        );
            }
            else
            {
                lRetorno = string.Format("<p id='{4}widTexto-{0}-{1}' style='{2}'>{3}</p>"
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoStyle
                                            , lTextoRenderizado
                                            , lPrefixo
                                        );
            }

            OnMensagemDeWidget("widTexto[{0}] Renderizado com [{1}] caracteres.", this.IdDoWidget, lRetorno.Length);

            return lRetorno;
        }

        #endregion
    }
}