using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Interface.Mensagens;

namespace Gradual.Intranet.Www.Intranet.Risco
{
    public partial class DefaultRestricoesSpider : PaginaBaseAutenticada
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            ReceberArvoreComandosInterfaceResponse lResponseDadosRestricoes =
                    ServicoInterface.ReceberArvoreComandosInterface(
                        new ReceberArvoreComandosInterfaceRequest()
                        {
                            CodigoSessao = this.CodigoSessao,
                            CodigoGrupoComandoInterface = "Menu_Dados_Risco_Restricoes_Spider"
                        });

            rpt_Menu_RiscoDados_Restricoes.DataSource = lResponseDadosRestricoes.ComandosInterfaceRaiz;
            rpt_Menu_RiscoDados_Restricoes.DataBind();
        }
    }
}