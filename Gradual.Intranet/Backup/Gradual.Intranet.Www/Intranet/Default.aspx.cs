using System;
using System.Web.UI;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Interface.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet
{
    public partial class Default : PaginaBaseAutenticada
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!Page.IsPostBack)
            {
                //this.pnlFerramentas.Visible = base.UsuarioPode("Consultar", "63db2334-9351-4ca6-8b39-bd3ce4d9778b");

                ReceberArvoreComandosInterfaceRequest lRequest;
                ReceberArvoreComandosInterfaceResponse lResponse;

                lRequest = new ReceberArvoreComandosInterfaceRequest();

                lRequest.CodigoSessao = this.CodigoSessao;
                lRequest.CodigoGrupoComandoInterface = "default";

                lResponse = this.ServicoInterface.ReceberArvoreComandosInterface(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    this.rptMenuPrincipal.DataSource = lResponse.ComandosInterfaceRaiz;
                    this.rptMenuPrincipal.DataBind();
                }
                else
                {
                    //TODO: Erro!
                    throw new Exception(string.Format("Erro de resposta do serviço: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }

            base.RegistrarRespostasAjax(new string[] { "BuscarPapelCotacaoRapida"
                                                     , "ManterSessaoDoUsuario" },
                     new ResponderAcaoAjaxDelegate[] { BuscarPapelCotacaoRapida
                                                     , ResponderManterSessaoDoUsuario});
        }

        protected string BuscarPapelCotacaoRapida()
        {
            string lPapel = Request.Form["Papel"].ToUpper();
            string[] lMensagens;
            string lRetornoMensagem;

            lRetornoMensagem = Gradual.OMS.Library.Servicos.Ativador.Get<IServicoCotacao>().ReceberTickerCotacao(lPapel);

            if (lRetornoMensagem.Length > 0)
            {
                lMensagens = lRetornoMensagem.Split('|');
                foreach (string lMensagem in lMensagens)
                {
                    lRetornoMensagem = RetornarSucessoAjax(new Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteMensagemDeNegocio(lMensagem), "Ok", new object[] { });
                }
            }
            else
            {
                lRetornoMensagem = RetornarErroAjax("Papel não encontrado!");
            }
            return lRetornoMensagem;
        }

        protected string ResponderManterSessaoDoUsuario()
        {
            return base.RetornarSucessoAjax("Sucesso");
        }
    }
}
