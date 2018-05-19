using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Clientes_Assessores : PaginaBaseAutenticada
    {
        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            /*
            RegistrarRespostasAjax(new string[] { 
                                                    "BuscarItensParaSelecao"
                                                  , "Paginar"
                                                },
                new ResponderAcaoAjaxDelegate[] { 
                                                    ResponderBuscarItensParaSelecao
                                                  , ResponderPaginar
                                                });
             */

            this.PopularControleComListaDoSinacor(eInformacao.Assessor, rptAssessorDe);
        }

        #endregion
    }
}
