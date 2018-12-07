using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.DbLib.Widgets
{
    public class widTitulo : WidgetBase
    {
        #region Propriedades

        public string Texto { get; set; }

        public byte NivelDeTitulo { get; set; }

        #endregion

        #region Métodos Públicos

        public override string Renderizar(byte pRenderizandoNaAba)
        {
            OnMensagemDeWidget("widTextoHTML[{0}].Renderizar({1})", this.IdDoWidget, this.IdDaEstrutura);

            if (this.NivelDeTitulo <= 0 || this.NivelDeTitulo >= 7)
                this.NivelDeTitulo = 1;

            string lRetorno;

            string lPrefixo = (pRenderizandoNaAba == 0) ? "" : "_";

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<h{0} id='{7}widTitulo-{1}-{2}' style='{3}' class='{4}'>{5}<input type='hidden' class='TextoOriginal' value='{6}'/></h{0}>"
                                            , this.NivelDeTitulo
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoStyle
                                            , this.AtributoClass
                                            , base.ProcessarTextoParaHTML(this.Texto)
                                            , this.Texto
                                            , lPrefixo
                                        );
            }
            else
            {
                lRetorno = string.Format("<h{0} id='{6}widTitulo-{1}-{2}' style='{3}' class='{4}'>{5}</h{0}>"
                                            , this.NivelDeTitulo
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoStyle
                                            , this.AtributoClass
                                            , base.ProcessarTextoParaHTML(this.Texto)
                                            , lPrefixo
                                        );
            }

            OnMensagemDeWidget("widTitulo[{0}] Renderizado com [{1}] caracteres.", this.IdDoWidget, lRetorno.Length);

            return lRetorno;
        }

        #endregion
    }
}