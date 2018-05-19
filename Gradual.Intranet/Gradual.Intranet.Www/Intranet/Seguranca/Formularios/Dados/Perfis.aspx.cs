using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.MobileControls;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados
{
    [ValidarSegurancaAttribute("9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F", "1", "1")]
    public partial class Perfis : PaginaBaseAutenticada
    {
        public string ResponderSalvar() 
        {
            string lRetorno = string.Empty;
            string lObjetoJson = Request.Params["ObjetoJson"];
            string TipoDeObjeto = Request["TipoDeObjetoPai"]; 

            TransporteSegurancaItemFilho lDados;

            MensagemRequestBase lRequest;

            MensagemResponseBase lResponse;

            try
            {
                lDados = JsonConvert.DeserializeObject<TransporteSegurancaItemFilho>(lObjetoJson);
                switch (TipoDeObjeto)
                {
                    case "Usuario":
                        lRequest = new ReceberUsuarioRequest();
                        ((ReceberUsuarioRequest)lRequest).CodigoUsuario = lDados.ParentId;
                        lRequest.CodigoSessao = this.CodigoSessao;
                        lResponse = ServicoSeguranca.ReceberUsuario((ReceberUsuarioRequest)lRequest);
                        break;

                    case "Grupo":
                        lRequest = new ReceberUsuarioGrupoRequest();
                        ((ReceberUsuarioGrupoRequest)lRequest).CodigoUsuarioGrupo = lDados.ParentId;
                        lRequest.CodigoSessao = this.CodigoSessao;
                        lResponse = ServicoSeguranca.ReceberUsuarioGrupo((ReceberUsuarioGrupoRequest)lRequest);
                        break;

                    default:
                        return RetornarErroAjax("Não é possível associar um tipo de item que não seja Usuários ou Grupos.");
                }

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    MensagemResponseBase lSalvarResponse;
                    MensagemRequestBase lSalvarRequest = null;
                    if (lResponse is ReceberUsuarioResponse)
                    {
                        UsuarioInfo lUsuario = ((ReceberUsuarioResponse)lResponse).Usuario;
                        lUsuario.Perfis.Add(lDados.Item);
                        lSalvarRequest = new SalvarUsuarioRequest()
                        {
                            CodigoSessao = this.CodigoSessao,
                            Usuario = lUsuario
                        };
                        lSalvarResponse = ServicoSeguranca.SalvarUsuario((SalvarUsuarioRequest)lSalvarRequest);
                    }
                    else
                    {
                        UsuarioGrupoInfo lGrupo = ((ReceberUsuarioGrupoResponse)lResponse).UsuarioGrupo;

                        lGrupo.Perfis.Add(lDados.Item);
                        lSalvarRequest = new SalvarUsuarioGrupoRequest()
                        {
                            CodigoSessao = this.CodigoSessao,
                            UsuarioGrupo = lGrupo
                        };
                        lSalvarResponse = ServicoSeguranca.SalvarUsuarioGrupo((SalvarUsuarioGrupoRequest)lSalvarRequest);
                    }

                    if (lSalvarResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        base.RegistrarLogInclusao();
                        return RetornarSucessoAjax(new TransporteRetornoDeCadastro(lDados.ParentId + "|" + lDados.Item + "|" + TipoDeObjeto), "Dados associados com sucesso");
                    }
                    else
                    {
                        return RetornarErroAjax(lSalvarResponse.DescricaoResposta);
                    }
                }
                else
                {
                    return RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                return RetornarErroAjax(ex.Message);
            }
        }

        public string ResponderCarregarHtmlComDados()
        {
            string lCodigoItem = Request["Id"];
            string TipoDeObjeto = Request["TipoDeObjeto"];
            List<TransporteSegurancaItemFilho> lItensRetorno = new List<TransporteSegurancaItemFilho>();
            MensagemRequestBase lRequest;
            MensagemResponseBase lResponse;
            
            switch(TipoDeObjeto)
            {
                case "Usuario":
                    lRequest = new ReceberUsuarioRequest()
                        {
                            CodigoSessao = this.CodigoSessao,
                            CodigoUsuario = lCodigoItem
                        };
                    lResponse = this.ServicoSeguranca.ReceberUsuario((ReceberUsuarioRequest)lRequest);
                    break;
                case "Grupo":
                    lRequest = new ReceberUsuarioGrupoRequest()
                        {
                            CodigoSessao = this.CodigoSessao,
                            CodigoUsuarioGrupo = lCodigoItem
                        };
                    lResponse = this.ServicoSeguranca.ReceberUsuarioGrupo((ReceberUsuarioGrupoRequest)lRequest);
                    break;
                default:
                    return RetornarErroAjax("Só é permitido listar os perfis se o tipo de objeto for Usuário ou Grupo.");
            }

            

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                ListarPerfisRequest lPerfisRequest = new ListarPerfisRequest()
                {
                    CodigoSessao = this.CodigoSessao
                };

                ListarPerfisResponse lPerfisResponse = this.ServicoSeguranca.ListarPerfis(lPerfisRequest);

                if (lRequest is ReceberUsuarioRequest)
                {
                    UsuarioInfo lUsuario = ((ReceberUsuarioResponse)lResponse).Usuario;
                    lItensRetorno = RetornarItens(lUsuario.Perfis, lPerfisResponse.Perfis, lCodigoItem, TipoDeObjeto);
                }
                else
                {
                    UsuarioGrupoInfo lUsuarioGrupo = ((ReceberUsuarioGrupoResponse)lResponse).UsuarioGrupo;
                    lItensRetorno = RetornarItens(lUsuarioGrupo.Perfis, lPerfisResponse.Perfis, lCodigoItem, TipoDeObjeto);
                }

                hidSeguranca_Perfis_ListaJson.Value = JsonConvert.SerializeObject(lItensRetorno);
            }
            else
            {
                return RetornarErroAjax("Erro: " + lResponse.DescricaoResposta);
            }

            return string.Empty;
        }

        public string ResponderExcluir()
        {
            string lIdItemPermissao = Request["Id"];

            string lItemPaiID = lIdItemPermissao.Split('|')[0];
            string lPerfilID = lIdItemPermissao.Split('|')[1];
            string lTipoDeObjeto = lIdItemPermissao.Split('|')[2];

            MensagemRequestBase lRequest;
            MensagemResponseBase lResponse;

            switch(lTipoDeObjeto)
            {
                case "Usuario":
                    lRequest = new ReceberUsuarioRequest()
                    {
                        CodigoUsuario = lItemPaiID,
                        CodigoSessao = this.CodigoSessao
                    };
                    lResponse = this.ServicoSeguranca.ReceberUsuario((ReceberUsuarioRequest)lRequest);
                    break;
                case "Grupo":
                    lRequest = new ReceberUsuarioGrupoRequest()
                    {
                        CodigoUsuarioGrupo = lItemPaiID,
                        CodigoSessao = this.CodigoSessao
                    };
                    lResponse = this.ServicoSeguranca.ReceberUsuarioGrupo((ReceberUsuarioGrupoRequest)lRequest);
                    break;
                default:
                    return RetornarErroAjax("Só é permitido listar os perfis se o tipo de objeto for Usuário ou Grupo.");
            }

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                MensagemRequestBase lSalvarRequest;
                MensagemResponseBase lSalvarResponse;

                if (lResponse is ReceberUsuarioResponse)
                {
                    UsuarioInfo lUsuario = ((ReceberUsuarioResponse)lResponse).Usuario;
                    lUsuario.Perfis.Remove(lPerfilID);

                    lSalvarRequest = new SalvarUsuarioRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        Usuario = lUsuario
                    };
                    lSalvarResponse = ServicoSeguranca.SalvarUsuario((SalvarUsuarioRequest)lSalvarRequest);
                }
                else
                {
                    UsuarioGrupoInfo lUsuarioGrupo = ((ReceberUsuarioGrupoResponse)lResponse).UsuarioGrupo;
                    lUsuarioGrupo.Perfis.Remove(lPerfilID);

                    lSalvarRequest = new SalvarUsuarioGrupoRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        UsuarioGrupo = lUsuarioGrupo
                    };
                    lSalvarResponse = ServicoSeguranca.SalvarUsuarioGrupo((SalvarUsuarioGrupoRequest)lSalvarRequest);
                }
                
                if (lSalvarResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogExclusao();
                    return RetornarSucessoAjax("Perfil excluido com sucesso.");
                }
                else
                {
                    return RetornarErroAjax("Erro ao excluir o perfil");
                }
            }
            else
            {
                return RetornarErroAjax("Erro ao excluir o perfil");
            }
        }

        private List<TransporteSegurancaItemFilho> RetornarItens(List<string> lItens, List<PerfilInfo> lPerfis, string lCodigoItem, string lTipoDeObjeto)
        {
            List<TransporteSegurancaItemFilho> lItensRetorno = new List<TransporteSegurancaItemFilho>();
            foreach (string lItem in lItens)
            {
                lItensRetorno.Add(new TransporteSegurancaItemFilho()
                {
                    Id = lCodigoItem + "|" + lItem + "|" + lTipoDeObjeto,
                    ParentId = lCodigoItem,
                    ItemDesc = lPerfis.Find(delegate(PerfilInfo p) { return p.CodigoPerfil == lItem; }).NomePerfil,
                    TipoDeItem = "Perfis",
                    Item = lItem
                });
            }

            return lItensRetorno;
        }

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { 
                                                    "Salvar"
                                                  , "CarregarHtmlComDados"
                                                  , "Excluir"
                                                },
                new ResponderAcaoAjaxDelegate[] { 
                                                    ResponderSalvar
                                                  , ResponderCarregarHtmlComDados 
                                                  , ResponderExcluir });



            if (!Page.IsPostBack)
            {
                ListarPerfisRequest lRequest = new ListarPerfisRequest()
                {
                    CodigoSessao = this.CodigoSessao
                };

                ListarPerfisResponse lResponse = this.ServicoSeguranca.ListarPerfis(lRequest);
                rptSeguranca_Perfis_Perfil.DataSource = lResponse.Perfis;
                rptSeguranca_Perfis_Perfil.DataBind();
            }
        }
        #endregion
    }
}
