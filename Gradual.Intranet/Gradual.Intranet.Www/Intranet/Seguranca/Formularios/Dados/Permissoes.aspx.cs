using System;
using System.Collections.Generic;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Cadastro;

namespace Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados
{
    [ValidarSegurancaAttribute("9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F", "1", "1")]
    public partial class Permissoes : PaginaBaseAutenticada
    {
        #region Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            base.RegistrarRespostasAjax(new string[] { "Salvar"
                                                     , "CarregarHtmlComDados"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderSalvar
                                                     , this.ResponderCarregarHtmlComDados 
                     });

            if (!this.Page.IsPostBack)
            {
                this.rptSeguranca_Permissoes_Permissao.DataSource = ReceberListaDePermissoes();
                this.rptSeguranca_Permissoes_Permissao.DataBind();
            }
        }

        #endregion

        #region Metodos de Apoio

        private void EnviarEmailDePermissaoGTI(int pIdClienteHabilitado, string pFerramenta)
        {
            Gradual.Intranet.Contratos.Dados.ClienteInfo lDadosDoCliente = new Gradual.Intranet.Contratos.Dados.ClienteInfo();

            lDadosDoCliente.IdLogin = pIdClienteHabilitado;

            ReceberEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteInfo> lRequest = new ReceberEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteInfo>()
            {
                CodigoEntidadeCadastro = pIdClienteHabilitado.ToString(),
                EntidadeCadastro = lDadosDoCliente
            };

            ReceberEntidadeCadastroResponse<Gradual.Intranet.Contratos.Dados.ClienteInfo> lResponse;

            lResponse = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<Gradual.Intranet.Contratos.Dados.ClienteInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (!string.IsNullOrEmpty(lResponse.EntidadeCadastro.DsCpfCnpj))
                {
                    Dictionary<string, string> lVariaveis = new Dictionary<string, string>();

                    lVariaveis.Add("###NOME###", lResponse.EntidadeCadastro.DsNome);
                    lVariaveis.Add("###CODIGO###", lResponse.EntidadeCadastro.IdCliente.Value.ToString());
                    lVariaveis.Add("###CPF###", lResponse.EntidadeCadastro.DsCpfCnpj);
                    lVariaveis.Add("###FERRAMENTA###", pFerramenta);

                    base.EnviarEmail(ConfiguracoesValidadas.EmailNotificacaoLiberacaoFerramenta
                                    , "Notificação de Liberação de Ferramenta"
                                    , "EmailPermissaoConcedida.htm"
                                    , lVariaveis, Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos);
                }
                else
                {
                    Logger.InfoFormat("Segurança/Permissões.aspx: Usuário [{0}] sem dados de cliente, email do GTI não enviado.", pIdClienteHabilitado);
                }
            }

        }


        public string ResponderSalvar()
        {
            string lRetorno = string.Empty;
            string lObjetoJson = Request.Params["ObjetoJson"];
            string TipoDeObjeto = Request["TipoDeObjetoPai"];

            MensagemRequestBase lRequest;
            MensagemResponseBase lResponse;

            bool lTinhaGTI = false;
            bool lTinhaStock = false;

            try
            {
                TransporteSegurancaPermissao lDados = JsonConvert.DeserializeObject<TransporteSegurancaPermissao>(lObjetoJson);

                switch (TipoDeObjeto)
                {
                    case "Usuario":
                        lRequest = new ReceberUsuarioRequest()
                            {
                                CodigoSessao = this.CodigoSessao,
                                CodigoUsuario = lDados.ParentId
                            };
                        lResponse = this.ServicoSeguranca.ReceberUsuario((ReceberUsuarioRequest)lRequest);
                        break;
                    case "Grupo":
                        lRequest = new ReceberUsuarioGrupoRequest()
                            {
                                CodigoSessao = this.CodigoSessao,
                                CodigoUsuarioGrupo = lDados.ParentId
                            };
                        lResponse = this.ServicoSeguranca.ReceberUsuarioGrupo((ReceberUsuarioGrupoRequest)lRequest);
                        break;
                    case "Perfil":
                        lRequest = new ReceberPerfilRequest()
                        {
                            CodigoSessao = this.CodigoSessao,
                            CodigoPerfil = lDados.ParentId
                        };
                        lResponse = this.ServicoSeguranca.ReceberPerfil((ReceberPerfilRequest)lRequest);
                        break;
                    default:
                        return RetornarErroAjax("Só é possível salvar permissões para grupos, usuários e perfis.");
                }

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    MensagemRequestBase lRequestBase;
                    MensagemResponseBase lResponseBase = new MensagemResponseBase();

                    if (lResponse is ReceberUsuarioResponse)
                    {
                        PermissaoAssociadaInfo lPermissao;
                        UsuarioInfo lUsuario = ((ReceberUsuarioResponse)lResponse).Usuario;

                        //precisa verificar todas que tinha antes pra não re-enviar o email quando uma outra permissão que não a do GTI ou Stock forem adicionadas
                        foreach (Gradual.OMS.Seguranca.Lib.PermissaoAssociadaInfo lPerm in lUsuario.Permissoes)
                        {
                            if (lPerm.CodigoPermissao.ToUpper() == ConfiguracoesValidadas.PermissaoDeAcesso_GTI.ToUpper())
                            {
                                lTinhaGTI = true;
                            }
                            
                            if (lPerm.CodigoPermissao.ToUpper() == ConfiguracoesValidadas.PermissaoDeAcesso_Stock.ToUpper())
                            {
                                lTinhaStock = true;
                            }
                        }

                        lUsuario.Permissoes.Clear();

                        var lCount = default(int);

                        do
                        {
                            lPermissao = new PermissaoAssociadaInfo()
                            {
                                CodigoPermissao = lDados.Permissoes.Count > 0 ? lDados.Permissoes[lCount] : string.Empty,
                                Status = PermissaoAssociadaStatusEnum.Permitido
                            };

                            lUsuario.Permissoes.Add(lPermissao);

                            lRequestBase = new SalvarUsuarioRequest()
                            {
                                CodigoSessao = this.CodigoSessao,
                                Usuario = lUsuario
                            };

                            lResponseBase = ServicoSeguranca.SalvarUsuario((SalvarUsuarioRequest)lRequestBase);

                            if (lResponseBase.StatusResposta == MensagemResponseStatusEnum.OK)
                            {
                                base.RegistrarLogInclusao(new LogIntranetInfo()
                                {
                                    CdBovespaClienteAfetado = lUsuario.CodigoUsuario.DBToInt32(),

                                    DsObservacao = string.Format("Cód. Usuário logado: {0}; Nome do cliente: {1}; e-Mail:", base.UsuarioLogado.Id, lUsuario.Nome, lUsuario.Email),
                                });

                                if (TipoDeObjeto == "Usuario")
                                {
                                    if (!lTinhaGTI && lPermissao.CodigoPermissao.ToUpper() == ConfiguracoesValidadas.PermissaoDeAcesso_GTI.ToUpper())
                                    {
                                        EnviarEmailDePermissaoGTI(lUsuario.CodigoUsuario.DBToInt32(), "Gradual Trader Interface (GTI)");
                                    }

                                    if (!lTinhaStock && lPermissao.CodigoPermissao.ToUpper() == ConfiguracoesValidadas.PermissaoDeAcesso_Stock.ToUpper())
                                    {
                                        EnviarEmailDePermissaoGTI(lUsuario.CodigoUsuario.DBToInt32(), "Stock Market");
                                    }
                                }
                            }

                            lCount++;

                        } while (lDados.Permissoes.Count > lCount);
                    }
                    else if (lResponse is ReceberUsuarioGrupoResponse)
                    {
                        PermissaoAssociadaInfo lPermissao;
                        UsuarioGrupoInfo lUsuarioGrupo = ((ReceberUsuarioGrupoResponse)lResponse).UsuarioGrupo;
                        foreach (string itemPermissao in lDados.Permissoes)
                        {
                            lPermissao = new PermissaoAssociadaInfo()
                            {
                                CodigoPermissao = itemPermissao,
                                Status = PermissaoAssociadaStatusEnum.Permitido
                            };
                            lUsuarioGrupo.Permissoes.Add(lPermissao);

                            lRequestBase = new SalvarUsuarioGrupoRequest()
                            {
                                CodigoSessao = this.CodigoSessao,
                                UsuarioGrupo = lUsuarioGrupo
                            };
                            lResponseBase = ServicoSeguranca.SalvarUsuarioGrupo((SalvarUsuarioGrupoRequest)lRequestBase);
                        }
                    }
                    else
                    {
                        PermissaoAssociadaInfo lPermissao;
                        PerfilInfo lPerfil = ((ReceberPerfilResponse)lResponse).Perfil;
                        lPerfil.Permissoes.Clear();

                        foreach (string itemPermissao in lDados.Permissoes)
                        {
                            lPermissao = new PermissaoAssociadaInfo()
                            {
                                CodigoPermissao = itemPermissao,
                                Status = PermissaoAssociadaStatusEnum.Permitido
                            };
                            lPerfil.Permissoes.Add(lPermissao);

                            lRequestBase = new SalvarPerfilRequest()
                            {
                                CodigoSessao = this.CodigoSessao,
                                Perfil = lPerfil
                            };
                            lResponseBase = ServicoSeguranca.SalvarPerfil((SalvarPerfilRequest)lRequestBase);
                        }
                    }

                    if (lResponseBase.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        return RetornarSucessoAjax("Permissão associada com sucesso.");
                    }
                    else
                    {
                        return RetornarErroAjax(lResponseBase.DescricaoResposta);
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
            string Id = Request["Id"];
            string TipoDeObjeto = Request["TipoDeObjeto"];

            MensagemRequestBase lRequest;
            MensagemResponseBase lResponse;

            switch (TipoDeObjeto)
            {
                case "Usuario":
                    lRequest = new ReceberUsuarioRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        CodigoUsuario = Id
                    };

                    var lRequestUsuario = new UsuarioPermissaoInfo();

                    lRequestUsuario.CodigoUsuario = Convert.ToInt32(Id);

                    lResponse = new UsuarioPermissoesDbLib().ListarIntranetPermissoesUsuario(lRequestUsuario);
                    
                    break;
                case "Grupo":
                    lRequest = new ReceberUsuarioGrupoRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        CodigoUsuarioGrupo = Id
                    };
                    lResponse = this.ServicoSeguranca.ReceberUsuarioGrupo((ReceberUsuarioGrupoRequest)lRequest);
                    break;
                case "Perfil":
                    lRequest = new ReceberPerfilRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        CodigoPerfil = Id
                    };
                    lResponse = this.ServicoSeguranca.ReceberPerfil((ReceberPerfilRequest)lRequest);
                    break;

                default:
                    return RetornarErroAjax("Só é possível mostrar permissões para Grupos, usuários e perfis.");
            }



            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                TransporteSegurancaPermissao lLista = new TransporteSegurancaPermissao();
                if (lResponse is ReceberUsuarioResponse)
                {
                    UsuarioInfo lUsuario = ((ReceberUsuarioResponse)lResponse).Usuario;
                    lLista = ReceberLista(lUsuario.Permissoes, Id, TipoDeObjeto);
                }
                else if (lResponse is ReceberUsuarioGrupoResponse)
                {
                    UsuarioGrupoInfo lUsuarioGrupo = ((ReceberUsuarioGrupoResponse)lResponse).UsuarioGrupo;
                    lLista = ReceberLista(lUsuarioGrupo.Permissoes, Id, TipoDeObjeto);
                }
                else
                {
                    PerfilInfo lPerfil = ((ReceberPerfilResponse)lResponse).Perfil;
                    lLista = ReceberLista(lPerfil.Permissoes, Id, TipoDeObjeto);
                }

                this.hidSeguranca_Permissoes_ListaJson.Value = JsonConvert.SerializeObject(lLista.Permissoes.ToArray());
            }
            else
            {
                return RetornarErroAjax(lResponse.DescricaoResposta);
            }

            return string.Empty;
        }

        private List<PermissaoInfo> ReceberListaDePermissoes()
        {
            ListarPermissoesRequest lRequest = new ListarPermissoesRequest();
            lRequest.CodigoSessao = this.CodigoSessao;
            ListarPermissoesResponse lResponse = this.ServicoSeguranca.ListarPermissoes(lRequest);
            List<PermissaoInfo> lLista = new List<PermissaoInfo>();
            foreach (PermissaoInfo pBase in lResponse.Permissoes)
            {
                lLista.Add(pBase);
            }

            return lLista;
        }

        private TransporteSegurancaPermissao ReceberLista(List<PermissaoAssociadaInfo> pPermissoes, string pId, string pTipoDeObjeto)
        {
            TransporteSegurancaPermissao lDados = new TransporteSegurancaPermissao();
            foreach (PermissaoAssociadaInfo lpai in pPermissoes)
            {
                lDados.Permissoes.Add(lpai.CodigoPermissao);
            }
            return lDados;
        }

        #endregion
    }
}
