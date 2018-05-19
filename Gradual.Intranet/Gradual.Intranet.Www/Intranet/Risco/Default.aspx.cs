using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Interface.Mensagens;

namespace Gradual.Intranet.Www.Intranet.Risco
{
    public partial class Default : PaginaBaseAutenticada
    {
        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            ReceberArvoreComandosInterfaceResponse lResponseDados =
                    ServicoInterface.ReceberArvoreComandosInterface(
                        new ReceberArvoreComandosInterfaceRequest()
                        {
                            CodigoSessao = this.CodigoSessao,
                            CodigoGrupoComandoInterface = "Menu_Dados_Risco"
                        });

            rpt_Menu_RiscoDados.DataSource = lResponseDados.ComandosInterfaceRaiz;
            rpt_Menu_RiscoDados.DataBind();

            //ReceberArvoreComandosInterfaceResponse lResponseDadosRestricoes =
            //        ServicoInterface.ReceberArvoreComandosInterface(
            //            new ReceberArvoreComandosInterfaceRequest()
            //            {
            //                CodigoSessao = this.CodigoSessao,
            //                CodigoGrupoComandoInterface = "Menu_Dados_Risco_Restricoes"
            //            });

            //rpt_Menu_RiscoDados_Restricoes.DataSource = lResponseDadosRestricoes.ComandosInterfaceRaiz;
            //rpt_Menu_RiscoDados_Restricoes.DataBind();
        }

        #endregion
    }
}
