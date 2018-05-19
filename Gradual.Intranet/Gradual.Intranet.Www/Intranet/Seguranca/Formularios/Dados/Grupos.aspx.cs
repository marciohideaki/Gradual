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
    public partial class Grupos : PaginaBaseAutenticada
    {
        public string ResponderSalvar()
        {
            string lRetorno = string.Empty;
            string lObjetoJson = Request["ObjetoJson"];

            TransporteSegurancaItemFilho lDados = null;

            ReceberUsuarioRequest lRequestUsuario = new ReceberUsuarioRequest()
            {
                CodigoSessao = this.CodigoSessao
            };

            ReceberUsuarioResponse lResponseUsuario = null;

            try
            {
                lDados = JsonConvert.DeserializeObject<TransporteSegurancaItemFilho>(lObjetoJson);

                lRequestUsuario.CodigoUsuario = lDados.ParentId;

                lResponseUsuario = this.ServicoSeguranca.ReceberUsuario(lRequestUsuario) as ReceberUsuarioResponse;

                UsuarioInfo lUsuario = lResponseUsuario.Usuario;

                lUsuario.Grupos.Add(lDados.Item);

                SalvarUsuarioRequest lRequest = new SalvarUsuarioRequest()
                {
                    CodigoSessao = this.CodigoSessao,
                    Usuario = lUsuario
                };

                SalvarUsuarioResponse lResponse = this.ServicoSeguranca.SalvarUsuario(lRequest) as SalvarUsuarioResponse;

                lRetorno = base.RetornarSucessoAjax(new TransporteRetornoDeCadastro(lDados.ParentId + lDados.Item), "Dados alterados com sucesso");

                base.RegistrarLogInclusao(string.Concat("Inclusão do grupo: ", lDados.Item));
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax(ex.Message);
            }

            return lRetorno;
        }

        public string ResponderCarregarHtmlComDados()
        {
            ReceberUsuarioRequest lRequest;
            ReceberUsuarioResponse lResponse;

            lRequest = new ReceberUsuarioRequest()
            {
                CodigoUsuario = Request["Id"],
                CodigoSessao = this.CodigoSessao

            };

            lResponse = this.ServicoSeguranca.ReceberUsuario(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                ListarUsuarioGruposRequest lRequestGrupos = new ListarUsuarioGruposRequest()
                {
                    CodigoSessao = this.CodigoSessao
                };

                ListarUsuarioGruposResponse lResponseGrupos = this.ServicoSeguranca.ListarUsuarioGrupos(lRequestGrupos);

                List<TransporteSegurancaItemFilho> lLista = new List<TransporteSegurancaItemFilho>();

                foreach(string lGrupo in lResponse.Usuario.Grupos)
                {
                    
                    TransporteSegurancaItemFilho lTransporte = new TransporteSegurancaItemFilho()
                    {
                        Id = lResponse.Usuario.CodigoUsuario + "|" + lGrupo,
                        Item = lGrupo,
                        ItemDesc = lResponseGrupos.UsuarioGrupos.Find(delegate(UsuarioGrupoInfo p) { return p.CodigoUsuarioGrupo == lGrupo; }).NomeUsuarioGrupo,
                        ParentId = lResponse.Usuario.CodigoUsuario,
                        TipoDeItem = "Grupos"
                    };

                    lLista.Add(lTransporte);
                }

                hidSeguranca_Grupos_ListaJson.Value = JsonConvert.SerializeObject(lLista);
            }
            else
            {
                RetornarErroAjax("Erro ao consultar os Grupos do usuario", lResponse.DescricaoResposta);
            }

            return string.Empty;    //só para obedecer assinatura
        }

        public string ResponderExcluir()
        {
            string lIdItem = Request["Id"];

            try
            {
                string lCodigoUsuario = lIdItem.Split('|')[0];
                string lCodigoGrupo = lIdItem.Split('|')[1];

                ReceberUsuarioRequest lRequest = new ReceberUsuarioRequest()
                {
                    CodigoSessao = this.CodigoSessao,
                    CodigoUsuario = lCodigoUsuario
                };

                ReceberUsuarioResponse lResponse = ServicoSeguranca.ReceberUsuario(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lResponse.Usuario.Grupos.Remove(lCodigoGrupo);

                    MensagemResponseBase lREsponseAUx = ServicoSeguranca.SalvarUsuario(new SalvarUsuarioRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        Usuario = lResponse.Usuario
                    });

                    if (lREsponseAUx.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        base.RegistrarLogExclusao();
                        return RetornarSucessoAjax("Grupo removido com sucesso.");
                    }
                    else
                    {
                        return RetornarErroAjax(lREsponseAUx.DescricaoResposta);
                    }
                }
            }
            catch (Exception ex)
            {
                return RetornarErroAjax(ex.Message);
            }
            return string.Empty;
        }

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] { "Salvar"
                                                     , "CarregarHtmlComDados"
                                                     , "Excluir"
                                                     },
                   new ResponderAcaoAjaxDelegate[]   { ResponderSalvar
                                                     , ResponderCarregarHtmlComDados 
                                                     , ResponderExcluir });
            if (!Page.IsPostBack)
            {
                ListarUsuarioGruposRequest lRequest = new ListarUsuarioGruposRequest() { CodigoSessao = this.CodigoSessao };
                ListarUsuarioGruposResponse lResponse = this.ServicoSeguranca.ListarUsuarioGrupos(lRequest);
                rptSeguranca_Grupos_Grupo.DataSource = lResponse.UsuarioGrupos;
                rptSeguranca_Grupos_Grupo.DataBind();
            }
        }

        #endregion
    }
}
