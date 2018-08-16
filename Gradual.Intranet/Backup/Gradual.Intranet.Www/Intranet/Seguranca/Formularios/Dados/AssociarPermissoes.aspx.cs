using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Interface.Dados;
using Gradual.OMS.Interface.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using log4net;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados
{
    public partial class AssociarPermissoes : PaginaBaseAutenticada
    {
        #region | Atributos

        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Propriedades

        private List<ComandoInterfaceInfo> SubSistemas
        {
            get
            {
                if (Session["SubSistemas"] != null)
                    return Session["SubSistemas"] as List<ComandoInterfaceInfo>;
                else
                {
                    return new List<ComandoInterfaceInfo>();
                }
            }
            set
            {
                Session["SubSistemas"] = value;
            }
        }

        private List<PermissaoInfo> ListaDePermissoes
        {
            get
            {
                if (Session["ListaDePermissoes"] != null)
                    return Session["ListaDePermissoes"] as List<PermissaoInfo>;
                else
                {
                    return new List<PermissaoInfo>();
                }
            }
            set
            {
                Session["ListaDePermissoes"] = value;
            }
        }

        #endregion

        #region | Métodos

        public string Salvar()
        {
            string lObjetoJson = Request.Params["ObjetoJson"];

            try
            {
                TransporteSegurancaDadosAssociados lDados = JsonConvert.DeserializeObject<TransporteSegurancaDadosAssociados>(lObjetoJson);
                MensagemRequestBase lRequestItem;
                MensagemRequestBase lRequestSalvarItem;
                MensagemResponseBase lResponseItem, lResponseSalvar;
                UsuarioGrupoInfo lUsuarioGrupo = null;
                UsuarioInfo lUsuario = null;

                if (lDados.EhGrupo)
                {
                    lRequestItem = new ReceberUsuarioGrupoRequest();
                    lRequestSalvarItem = new SalvarUsuarioGrupoRequest();
                    lRequestSalvarItem.CodigoSessao = this.CodigoSessao;
                    lRequestItem.CodigoSessao = this.CodigoSessao;

                    ((ReceberUsuarioGrupoRequest)lRequestItem).CodigoUsuarioGrupo = lDados.Grupo;
                    lResponseItem = ServicoSeguranca.ReceberUsuarioGrupo((ReceberUsuarioGrupoRequest)lRequestItem);
                }
                else if (lDados.EhUsuario)
                {
                    lRequestItem = new ReceberUsuarioRequest();
                    lRequestSalvarItem = new SalvarUsuarioRequest();
                    lRequestSalvarItem.CodigoSessao = this.CodigoSessao;
                    lRequestItem.CodigoSessao = this.CodigoSessao;

                    ((ReceberUsuarioRequest)lRequestItem).CodigoUsuario = lDados.Usuario;
                    lResponseItem = ServicoSeguranca.ReceberUsuario((ReceberUsuarioRequest)lRequestItem);
                }
                else
                    return RetornarErroAjax("Selecione um grupo ou usuário para associar as permissões");



                if (lResponseItem.StatusResposta != MensagemResponseStatusEnum.OK)
                    return RetornarErroAjax(lResponseItem.DescricaoResposta);

                if (lResponseItem is ReceberUsuarioGrupoResponse)
                    lUsuarioGrupo = ((ReceberUsuarioGrupoResponse)lResponseItem).UsuarioGrupo;
                else
                    lUsuario = ((ReceberUsuarioResponse)lResponseItem).Usuario;

                string nomePermissao = string.Empty;

                nomePermissao = lDados.Interface.Trim() + "Consultar";
                this.AssociarPermissaoAoObjeto(lResponseItem, nomePermissao, lDados.Consultar);

                nomePermissao = lDados.Interface.Trim() + "Salvar";
                this.AssociarPermissaoAoObjeto(lResponseItem, nomePermissao, lDados.Salvar);

                nomePermissao = lDados.Interface.Trim() + "Excluir";
                this.AssociarPermissaoAoObjeto(lResponseItem, nomePermissao, lDados.Excluir);

                nomePermissao = lDados.Interface.Trim() + "Executar";
                this.AssociarPermissaoAoObjeto(lResponseItem, nomePermissao, lDados.Executar);

                if (lDados.EhGrupo)
                {
                    ((SalvarUsuarioGrupoRequest)lRequestSalvarItem).UsuarioGrupo = lUsuarioGrupo;
                    lResponseSalvar = ServicoSeguranca.SalvarUsuarioGrupo((SalvarUsuarioGrupoRequest)lRequestSalvarItem);
                }
                else
                {
                    ((SalvarUsuarioRequest)lRequestSalvarItem).Usuario = lUsuario;
                    lResponseSalvar = ServicoSeguranca.SalvarUsuario((SalvarUsuarioRequest)lRequestSalvarItem);
                }

                if (lResponseSalvar.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogInclusao();
                    return RetornarSucessoAjax("Itens Associados com sucesso");
                }
                else
                {
                    return RetornarErroAjax(lResponseSalvar.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                return RetornarErroAjax(ex.Message, ex);
            }
        }

        public string CarregarInterfaces()
        {
            return RetornarSucessoAjax(ObterInterfaces(), "OK");
        }

        public string CarregarUsuariosOuGrupos()
        {
            return string.Empty;
        }

        public string ReceberPermissoes()
        {
            string lObjetoJson = Request.Params["ObjetoJson"];

            try
            {
                TransporteSegurancaDadosAssociados lDados = JsonConvert.DeserializeObject<TransporteSegurancaDadosAssociados>(lObjetoJson);
                lDados.Consultar =
                    lDados.Excluir =
                        lDados.Salvar =
                            lDados.Executar = false;
                if (lDados.EhGrupo)
                {
                    ReceberUsuarioGrupoRequest lReuqest = new ReceberUsuarioGrupoRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        CodigoUsuarioGrupo = lDados.Grupo,
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
                    };

                    ReceberUsuarioGrupoResponse lRes = ServicoSeguranca.ReceberUsuarioGrupo(lReuqest);

                    List<PermissaoInfo> permissoes = ListaDePermissoes.Where(p => p.GetType().Name.Contains(lDados.Interface)).ToList();

                    foreach (PermissaoAssociadaInfo lPI in lRes.UsuarioGrupo.Permissoes)
                    {
                        PermissaoInfo lPermissao = permissoes.Find(p => p.CodigoPermissao == lPI.CodigoPermissao);
                        if (lPermissao != null)
                        {
                            if (lPermissao.GetType().Name.Contains("Excluir"))
                                lDados.Excluir = true;
                            if (lPermissao.GetType().Name.Contains("Executar"))
                                lDados.Executar = true;
                            if (lPermissao.GetType().Name.Contains("Consultar"))
                                lDados.Consultar = true;
                            if (lPermissao.GetType().Name.Contains("Salvar"))
                                lDados.Salvar = true;
                        }
                    }
                }
                else
                {
                    ReceberUsuarioRequest lReuqest = new ReceberUsuarioRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        CodigoUsuario = lDados.Usuario,
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
                    };

                    ReceberUsuarioResponse lRes = ServicoSeguranca.ReceberUsuario(lReuqest);

                    List<PermissaoInfo> permissoes = ListaDePermissoes.Where(p => p.GetType().Name.Contains(lDados.Interface)).ToList();

                    foreach (PermissaoAssociadaInfo lPI in lRes.Usuario.Permissoes)
                    {
                        PermissaoInfo lPermissao = permissoes.Find(p => p.CodigoPermissao == lPI.CodigoPermissao);
                        if (lPermissao != null)
                        {
                            if (lPermissao.GetType().Name.Contains("Excluir"))
                                lDados.Excluir = true;
                            if (lPermissao.GetType().Name.Contains("Executar"))
                                lDados.Executar = true;
                            if (lPermissao.GetType().Name.Contains("Consultar"))
                                lDados.Consultar = true;
                            if (lPermissao.GetType().Name.Contains("Salvar"))
                                lDados.Salvar = true;
                        }
                    }
                }
                return RetornarSucessoAjax(lDados, "Ok");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                return RetornarErroAjax(ex.Message);
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] { "Salvar"
                                                     , "CarregarInterfaces"
                                                     , "CarregarUsuariosOuGrupos"
                                                     , "ReceberPermissoes"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { Salvar
                                                     , CarregarInterfaces 
                                                     , CarregarUsuariosOuGrupos
                                                     , ReceberPermissoes
                                                     });
            if (!Page.IsPostBack)
            {
                this.CarregarListaDePermissoes();
                this.PopularComboGrupos();
                this.PopularComboUsuarios();
                this.ObterSubSistemas();
                this.PupularComboSubSistemas();
            }
        }

        #endregion

        #region | Métodos de Apoio

        private void ObterSubSistemas()
        {
            ReceberArvoreComandosInterfaceRequest ReceberSubSistemasReq = new ReceberArvoreComandosInterfaceRequest();
            ReceberSubSistemasReq.CodigoSessao = this.CodigoSessao;
            ReceberSubSistemasReq.CodigoGrupoComandoInterface = "default";

            ReceberArvoreComandosInterfaceResponse ReceberSubSistemasRes = this.ServicoInterface.ReceberArvoreComandosInterface(ReceberSubSistemasReq);

            this.SubSistemas = ReceberSubSistemasRes.ComandosInterfaceRaiz;
        }

        private List<TransporteSegurancaInterfaces> ObterInterfaces()
        {
            string lSubSistema = Request["SubSistema"];
            string lInterfaceAcoes = "Menu_Acoes_" + lSubSistema;
            string lInterfaceDados = "Menu_Dados_" + lSubSistema;
            //string lInterface = "Menu_" + lSubSistema;

            ReceberArvoreComandosInterfaceRequest ReceberInterfacesAcoesReq = new ReceberArvoreComandosInterfaceRequest();
            ReceberInterfacesAcoesReq.CodigoSessao = this.CodigoSessao;
            ReceberInterfacesAcoesReq.CodigoGrupoComandoInterface = lInterfaceAcoes;

            ReceberArvoreComandosInterfaceRequest ReceberInterfacesDadosReq = new ReceberArvoreComandosInterfaceRequest();
            ReceberInterfacesDadosReq.CodigoSessao = this.CodigoSessao;
            ReceberInterfacesDadosReq.CodigoGrupoComandoInterface = lInterfaceDados;

            List<TransporteSegurancaInterfaces> lRetorno = new List<TransporteSegurancaInterfaces>();

            try
            {
                ReceberInterfacesDadosReq.DescricaoUsuarioLogado = base.UsuarioLogado.Nome; ReceberInterfacesDadosReq.IdUsuarioLogado = base.UsuarioLogado.Id;
                ReceberArvoreComandosInterfaceResponse ReceberInterfacesDadosRes = this.ServicoInterface.ReceberArvoreComandosInterface(ReceberInterfacesDadosReq);

                foreach (ComandoInterfaceInfo ci in ReceberInterfacesDadosRes.ComandosInterfaceRaiz)
                {
                    TransporteSegurancaInterfaces iTsi = new TransporteSegurancaInterfaces();
                    iTsi.Nome = ci.Nome;
                    iTsi.NomePermissao = ci.CodigoComandoInterface.Split('_')[1];
                    iTsi.SubSistema = ci.CodigoComandoInterface.Split('_')[0];

                    lRetorno.Add(iTsi);
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, Ex);
            }

            try
            {
                ReceberInterfacesAcoesReq.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                ReceberInterfacesAcoesReq.IdUsuarioLogado = base.UsuarioLogado.Id;
                ReceberArvoreComandosInterfaceResponse ReceberInterfacesAcoesRes = this.ServicoInterface.ReceberArvoreComandosInterface(ReceberInterfacesAcoesReq);

                foreach (ComandoInterfaceInfo ci in ReceberInterfacesAcoesRes.ComandosInterfaceRaiz)
                {
                    TransporteSegurancaInterfaces iTsi = new TransporteSegurancaInterfaces();
                    iTsi.Nome = ci.Nome;
                    iTsi.NomePermissao = ci.CodigoComandoInterface.Split('_')[1];
                    iTsi.SubSistema = ci.CodigoComandoInterface.Split('_')[0];

                    lRetorno.Add(iTsi);
                }

            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, Ex);
            }

            return lRetorno;
        }

        private List<String> RetornarInterfacesSubSistema(string SubSistema)
        {
            List<String> lRetorno = new List<string>();

            ComandoInterfaceInfo lComando = SubSistemas.Find(delegate(ComandoInterfaceInfo p) { return p.Nome.Trim().ToLower() == SubSistema.Trim().ToLower(); });

            foreach (ComandoInterfaceInfo ci in lComando.Filhos)
                lRetorno.Add(ci.Nome);

            return lRetorno;
        }

        private void PopularComboUsuarios()
        {
            ListarUsuariosRequest lListarUsuariosReq = new ListarUsuariosRequest()
            {
                CodigoSessao = this.CodigoSessao
                ,
                IdUsuarioLogado = base.UsuarioLogado.Id,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome
            };

            ListarUsuariosResponse lListarUsuariosRes = this.ServicoSeguranca.ListarUsuarios(lListarUsuariosReq);
            this.rptSeguranca_Associacoes_Usuario.DataSource = lListarUsuariosRes.Usuarios;
            this.rptSeguranca_Associacoes_Usuario.DataBind();
        }

        private void PopularComboGrupos()
        {
            ListarUsuarioGruposRequest lListarUsuarioGruposReq = new ListarUsuarioGruposRequest()
            {
                CodigoSessao = this.CodigoSessao,
                IdUsuarioLogado = base.UsuarioLogado.Id,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome
            };

            ListarUsuarioGruposResponse lListarUsuarioGruposRes = this.ServicoSeguranca.ListarUsuarioGrupos(lListarUsuarioGruposReq);
            this.rptSeguranca_Associacoes_Grupo.DataSource = lListarUsuarioGruposRes.UsuarioGrupos;
            this.rptSeguranca_Associacoes_Grupo.DataBind();
        }

        private void PupularComboSubSistemas()
        {
            this.rptSeguranca_Associacoes_Subsistemas.DataSource = this.SubSistemas;
            this.rptSeguranca_Associacoes_Subsistemas.DataBind();
        }

        private void CarregarListaDePermissoes()
        {
            ListarPermissoesRequest lreq = new ListarPermissoesRequest();
            lreq.CodigoSessao = this.CodigoSessao;
            lreq.IdUsuarioLogado = base.UsuarioLogado.Id; lreq.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
            ListarPermissoesResponse lRes = ServicoSeguranca.ListarPermissoes(lreq);
            this.ListaDePermissoes = lRes.Permissoes;
        }

        private void AssociarPermissaoAoObjeto(MensagemResponseBase lItem, string nomePermissao, bool associar)
        {
            //nomePermissao = lDados.Interface.Trim() + "Executar";
            PermissaoInfo lPermissao = this.ListaDePermissoes.Find(p => p.GetType().Name == nomePermissao);

            if ((lPermissao != null)
            && ((lItem != null))
            && ((lItem is ReceberUsuarioGrupoResponse)))
            {
                PermissaoAssociadaInfo lPermAssoc = ((ReceberUsuarioGrupoResponse)lItem).UsuarioGrupo.Permissoes.Find(p => p.CodigoPermissao == lPermissao.CodigoPermissao);
                if (lPermAssoc == null)
                {
                    if (associar)
                        ((ReceberUsuarioGrupoResponse)lItem).UsuarioGrupo.Permissoes.Add(new PermissaoAssociadaInfo()
                        {
                            CodigoPermissao = lPermissao.CodigoPermissao,
                            Status = PermissaoAssociadaStatusEnum.Permitido
                        });
                }
                else
                {
                    if (!associar)
                        ((ReceberUsuarioGrupoResponse)lItem).UsuarioGrupo.Permissoes.Remove(lPermAssoc);
                }
            }
            else
            {
                PermissaoAssociadaInfo lPermAssoc = ((ReceberUsuarioGrupoResponse)lItem).UsuarioGrupo.Permissoes.Find(p => p.CodigoPermissao == lPermissao.CodigoPermissao);

                if (lPermAssoc == null)
                {
                    if (associar)
                        ((ReceberUsuarioResponse)lItem).Usuario.Permissoes.Add(new PermissaoAssociadaInfo()
                        {
                            CodigoPermissao = lPermissao.CodigoPermissao,
                            Status = PermissaoAssociadaStatusEnum.Permitido
                        });
                }
                else
                {
                    if (!associar)
                        ((ReceberUsuarioResponse)lItem).Usuario.Permissoes.Remove(lPermAssoc);
                }
            }
        }

        #endregion
    }
}