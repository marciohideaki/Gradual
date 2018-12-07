using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Lib.Widgets
{
    public class widTitulo : WidgetBase
    {
        #region Propriedades

        public string Texto { get; set; }

        public byte NivelDeTitulo { get; set; }

        #endregion

        #region Métodos Públicos

        public override string Renderizar()
        {
            if (this.NivelDeTitulo <= 0 || this.NivelDeTitulo >= 7)
                this.NivelDeTitulo = 1;

            string lRetorno;

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<h{0} id='widTitulo-{1}-{2}' style='{3}'>{4}<input type='hidden' class='TextoOriginal' value='{5}'/></h{0}>"
                                            , this.NivelDeTitulo
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoStyle
                                            , base.ProcessarTextoParaHTML(this.Texto)
                                            , this.Texto
                                        );
            }
            else
            {
                lRetorno = string.Format("<h{0} id='widTitulo-{1}-{2}' style='{3}'>{4}</h{0}>"
                                            , this.NivelDeTitulo
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoStyle
                                            , base.ProcessarTextoParaHTML(this.Texto)
                                        );
            }

            return lRetorno;
        }

        #endregion
    }
}