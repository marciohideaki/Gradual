using System;
using Gradual.OMS.Interface.Mensagens;
using System.Configuration;

namespace Gradual.Intranet.Www.Intranet.Clientes
{
    public partial class Default : PaginaBaseAutenticada
    {
        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!Page.IsPostBack)
            {
                ReceberArvoreComandosInterfaceResponse lResponseDados =
                    ServicoInterface.ReceberArvoreComandosInterface(
                        new ReceberArvoreComandosInterfaceRequest()
                        {
                            CodigoSessao = this.CodigoSessao,
                            CodigoGrupoComandoInterface = "Menu_Dados_Clientes"
                        });

                ReceberArvoreComandosInterfaceResponse lResponseAcoes =
                    ServicoInterface.ReceberArvoreComandosInterface(
                        new ReceberArvoreComandosInterfaceRequest()
                        {
                            CodigoSessao = this.CodigoSessao,
                            CodigoGrupoComandoInterface = "Menu_Acoes_Clientes"
                        });

                this.rpt_Menu_ClienteAcoes.DataSource = lResponseAcoes.ComandosInterfaceRaiz;
                this.rpt_Menu_ClienteAcoes.DataBind();

                this.rpt_Menu_ClienteDados.DataSource = lResponseDados.ComandosInterfaceRaiz;
                this.rpt_Menu_ClienteDados.DataBind();

                ValidaPermissaoDeEdicaoDeClientesPassos3e4();

            }
        }

        #endregion


        private void ValidaPermissaoDeEdicaoDeClientesPassos3e4(){
           string[] lPerfiisComPermissao = ConfigurationManager.AppSettings["PerfiisComPermissaoDeAlterarClientesPasso3e4"].ToString().Split(',');

                //var lPerfiisDoUsuario = ServicoSeguranca.ListarPerfis(
                //    new OMS.Seguranca.Lib.ListarPerfisRequest(){ 
                //        CodigoSessao = base.CodigoSessao, 
                //        IdUsuarioLogado=base.UsuarioLogado.Id, 
                //        DescricaoUsuarioLogado = base.UsuarioLogado.Nome
                //    }
                //    );

           var Sessao = ServicoSeguranca.ReceberSessao(new OMS.Seguranca.Lib.ReceberSessaoRequest(){ 
               CodigoSessao = base.CodigoSessao, 
               CodigoSessaoARetornar = base.CodigoSessao, 
               IdUsuarioLogado = base.UsuarioLogado.Id, 
               DescricaoUsuarioLogado= base.UsuarioLogado.Nome});

                Boolean lPermissaoParaEditarClientesPasso3e3 = false;
                foreach (var itemUsuario in Sessao.Usuario.Perfis)
	            {
                    if (lPermissaoParaEditarClientesPasso3e3) 
                        break;
                    foreach (string itemPermisao in lPerfiisComPermissao)
                    {
                        if (itemUsuario == itemPermisao)
                        {
                            lPermissaoParaEditarClientesPasso3e3 = true;
                            break;
                        }
                    }
	            }

                hdMenuClientesPermissaoAlterarPasso_3_4.Value = lPermissaoParaEditarClientesPasso3e3.ToString();
        }


       
    }
}
