using System;
using System.Collections.Generic;
using System.Web.UI;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados
{
    [ValidarSegurancaAttribute("9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F", "1", "1")]
    public partial class Usuarios : PaginaBaseAutenticada
    {
        #region Métodos

        public string ResponderSalvar()
        {
            string lObjetoJson = Request["ObjetoJson"];
            string lTipoDeObjetoPai = Request["TipoDeObjetoPai"];
            TransporteSegurancaItemFilho lDados = null;

            ReceberUsuarioRequest lRequest = new ReceberUsuarioRequest()
            {
                CodigoSessao = this.CodigoSessao
            };

            ReceberUsuarioResponse lResponse = null;

            try
            {
                lDados = JsonConvert.DeserializeObject<TransporteSegurancaItemFilho>(lObjetoJson);

                lRequest.CodigoUsuario = lDados.Item;

                lResponse = this.ServicoSeguranca.ReceberUsuario(lRequest);

                               
                UsuarioInfo lUsuario = lResponse.Usuario;

                if (lTipoDeObjetoPai == "Grupo")
                    lUsuario.Grupos.Add(lDados.ParentId);
                else if (lTipoDeObjetoPai == "Perfil")
                    lUsuario.Perfis.Add(lDados.ParentId);

                SalvarUsuarioRequest lSalvarUsuarioReq = new SalvarUsuarioRequest()
                {
                    CodigoSessao = this.CodigoSessao,
                    Usuario = lUsuario
                };

                SalvarUsuarioResponse lSalvarUsuarioRes = ServicoSeguranca.SalvarUsuario(lSalvarUsuarioReq);

                if (lSalvarUsuarioRes.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogInclusao(string.Concat("Incluído usuário: ", lResponse.Usuario.Email));
                    return RetornarSucessoAjax(new TransporteRetornoDeCadastro(lDados.ParentId + "|" + lDados.Item + "|" + lTipoDeObjetoPai), "Usuario associado com sucesso.");
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
            ListarUsuariosRequest lRequest = new ListarUsuariosRequest()
                {
                    CodigoSessao = this.CodigoSessao
                };

            switch (TipoDeObjeto)
            {
                case "Grupo":
                    lRequest.FiltroCodigoUsuarioGrupo = lCodigoItem;
                    break;
                case "Perfil":
                    lRequest.FiltroCodigoPerfil = lCodigoItem;
                    break;
            }
            try
            {
                ListarUsuariosResponse lResponse = ServicoSeguranca.ListarUsuarios(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    foreach (UsuarioInfo lUsuario in lResponse.Usuarios)
                    {
                        lItensRetorno.Add(
                            new TransporteSegurancaItemFilho()
                            {
                                Id = lCodigoItem + "|" + lUsuario.CodigoUsuario + "|" + TipoDeObjeto,
                                Item = lUsuario.CodigoUsuario,
                                ItemDesc = lUsuario.CodigoUsuario + " - " + lUsuario.Nome,
                                ParentId = lCodigoItem,
                                TipoDeItem = "Usuarios"
                            });
                    }

                    hidSeguranca_Usuarios_ListaJson.Value = JsonConvert.SerializeObject(lItensRetorno);
                }
                else
                {
                    return RetornarErroAjax("Erro:" + lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                return RetornarErroAjax("Erro:" + ex.Message);
            }
            return string.Empty;

        }

        public string ResponderExcluir()
        {
            string lIdItem = Request["Id"];
            string lItemPaiID = lIdItem.Split('|')[0];
            string lUsuarioID = lIdItem.Split('|')[1];
            string lTipoDeObjeto = lIdItem.Split('|')[2];

            ReceberUsuarioRequest lRequest = new ReceberUsuarioRequest()
            {
                CodigoSessao = this.CodigoSessao,
                CodigoUsuario = lUsuarioID
            };

            try
            {
                ReceberUsuarioResponse lResponse = ServicoSeguranca.ReceberUsuario(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    UsuarioInfo lUsuario = lResponse.Usuario;

                    if (lTipoDeObjeto == "Perfil")
                        lUsuario.Perfis.Remove(lItemPaiID);
                    else if (lTipoDeObjeto == "Grupo")
                        lUsuario.Grupos.Remove(lItemPaiID);

                    var lSalvarUsuarioReq = new SalvarUsuarioRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        Usuario = lUsuario
                    };

                    SalvarUsuarioResponse lSalvarUsuarioRes = ServicoSeguranca.SalvarUsuario(lSalvarUsuarioReq);

                    if (lSalvarUsuarioRes.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        base.RegistrarLogExclusao(string.Concat("Excluido usuário: ", lResponse.Usuario.Email));
                        return RetornarSucessoAjax("Usuario excluido com sucesso.");
                    }
                    else
                    {
                        return RetornarErroAjax("Erro: " + lSalvarUsuarioRes.DescricaoResposta);
                    }
                }
                else
                {
                    return RetornarErroAjax("Erro:" + lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                return RetornarErroAjax("Erro:" + ex.Message);
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] { "Salvar"
                                                     , "CarregarHtmlComDados"
                                                     , "Excluir"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                                     , ResponderCarregarHtmlComDados 
                                                     , ResponderExcluir });
            if (!Page.IsPostBack)
            {
                ListarUsuariosResponse lResponse = this.ServicoSeguranca.ListarUsuarios(new ListarUsuariosRequest() { CodigoSessao = this.CodigoSessao });
                rptSeguranca_Usuarios_Usuario.DataSource = lResponse.Usuarios;
                rptSeguranca_Usuarios_Usuario.DataBind();
            }
        }

        #endregion
    }
}
