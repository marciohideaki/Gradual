using System;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados
{
    [ValidarSegurancaAttribute("9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F", "1", "1")]
    public partial class DadosCompletos_Usuario : PaginaBaseAutenticada
    {
        #region | Métodos

        public string ResponderCarregarHtmlComDados()
        {
            string Id = Request["Id"];
            this.lblSeguranca_DadosCompletos_Usuario_Senha.Visible = false;

            TransporteSegurancaUsuario lDadosUsuario;
            ReceberUsuarioRequest lRequest = new ReceberUsuarioRequest()
            {
                CodigoSessao = this.CodigoSessao,
                CodigoUsuario = Id
            };

            ReceberUsuarioResponse lResponse = this.ServicoSeguranca.ReceberUsuario(lRequest);
            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lDadosUsuario = new TransporteSegurancaUsuario(lResponse.Usuario);

                hidDadosCompletos_Seguranca_Usuario.Value = JsonConvert.SerializeObject(lDadosUsuario);
            }
            else
            {
                return RetornarErroAjax(lResponse.DescricaoResposta);
            }

            return string.Empty;
        }

        public string ResponderCadastrar()
        {
            string lretorno = string.Empty;
            string lObjetoJson = Request["ObjetoJson"];

            TransporteSegurancaUsuario lDadosUsuario = null;

            SalvarUsuarioResponse lResponse = null;
            SalvarUsuarioRequest lRequest = new SalvarUsuarioRequest();

            UsuarioInfo lUsuarioInfo = new UsuarioInfo();

            try
            {
                lDadosUsuario = JsonConvert.DeserializeObject<TransporteSegurancaUsuario>(lObjetoJson);

                lUsuarioInfo = lDadosUsuario.ToUsuarioInfo();

                lUsuarioInfo.CodigoUsuario = "0";

                lRequest.Usuario = lUsuarioInfo;

                lRequest.CodigoSessao = this.CodigoSessao;

                lResponse = ServicoSeguranca.SalvarUsuario(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    ReceberUsuarioResponse lREsponseUsuario = ServicoSeguranca.ReceberUsuario(
                    new ReceberUsuarioRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        Email = lUsuarioInfo.Email
                    });

                    if (lREsponseUsuario.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lretorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lREsponseUsuario.Usuario.CodigoUsuario), "Usuário cadastrado com sucesso");
                        base.RegistrarLogInclusao(string.Concat("Usuário incluído: ", lDadosUsuario.Email));
                    }
                    else
                    {
                        lretorno = RetornarErroAjax(lREsponseUsuario.DescricaoResposta);
                    }
                }
                else
                {
                    lretorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lretorno = RetornarErroAjax("Erro durante o envio do pedido para cadastrar usuário", ex);
            }
            return lretorno;
        }

        public string ResponderExcluir()
        {
            return string.Empty;
        }

        public string ResponderAtualizar()
        {
            string lretorno = string.Empty;
            string lObjetoJson = this.Request.Form["ObjetoJson"];

            TransporteSegurancaUsuario lDadosUsuario = null;

            SalvarUsuarioResponse lResponseSalvar = null;
            SalvarUsuarioRequest lRequestSalvar = new SalvarUsuarioRequest()
                {
                    CodigoSessao = this.CodigoSessao
                };

            UsuarioInfo lUsuarioInfo = null;

            try
            {
                lDadosUsuario = JsonConvert.DeserializeObject<TransporteSegurancaUsuario>(lObjetoJson);

                ReceberUsuarioResponse lResponseReceber = ServicoSeguranca.ReceberUsuario(
                    new ReceberUsuarioRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        CodigoUsuario = lDadosUsuario.Id
                    });

                if(lResponseReceber.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lUsuarioInfo = lResponseReceber.Usuario;
                    lUsuarioInfo.Nome = lDadosUsuario.Nome;
                    lUsuarioInfo.Email = lDadosUsuario.Email;
                    int lTipoAcesso = 0;
                    if(int.TryParse(lDadosUsuario.TipoAcesso, out lTipoAcesso))
                        lUsuarioInfo.CodigoTipoAcesso = lTipoAcesso;

                    if ((eTipoAcesso)lTipoAcesso == eTipoAcesso.Assessor)
                    {
                        int lCodAssessor = -1;
                        if (int.TryParse(lDadosUsuario.CodAssessor, out lCodAssessor))
                        {
                            lUsuarioInfo.CodigoAssessor = lCodAssessor;
                            lUsuarioInfo.CodigosFilhoAssessor = lDadosUsuario.CodAssessorAssociado;
                        }
                        else
                        {
                            throw new Exception("Codigo de assessor inválido.");
                        }
                    }

                    lRequestSalvar.Usuario = lUsuarioInfo;

                    lResponseSalvar = ServicoSeguranca.SalvarUsuario(lRequestSalvar);

                    if (lResponseSalvar.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lretorno = RetornarSucessoAjax("Usuário alterado com sucesso");
                        base.RegistrarLogAlteracao(string.Concat("Usuário alterado: ", lDadosUsuario.Email));
                    }
                    else
                    {
                        lretorno = RetornarErroAjax(lResponseSalvar.DescricaoResposta);
                    }
                }
                else
                {
                    lretorno = RetornarErroAjax(lResponseReceber.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lretorno = RetornarErroAjax("Erro durante o envio do request para cadastrar usuário", ex);
            }

            return lretorno;
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] 
                                            { "Cadastrar"
                                            , "Excluir"
                                            , "Atualizar"
                                            , "CarregarHtmlComDados"
                                            },
            new ResponderAcaoAjaxDelegate[] { this.ResponderCadastrar
                                            , this.ResponderExcluir 
                                            , this.ResponderAtualizar
                                            , this.ResponderCarregarHtmlComDados
                                            });
        }

        #endregion
    }
}
