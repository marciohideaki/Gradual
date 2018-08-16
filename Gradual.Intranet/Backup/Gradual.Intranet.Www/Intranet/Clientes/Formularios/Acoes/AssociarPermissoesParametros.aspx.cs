using System;
using System.Collections.Generic;
using System.Web.UI;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class AssociarPermissoesParametros : PaginaBaseAutenticada
    {
        private string ResponderSalvar()
        {
            string lObjetoJson = Request.Params["ObjetoJson"];

            TransporteRiscoAssociacaoCliente lTransporte = JsonConvert.DeserializeObject<TransporteRiscoAssociacaoCliente>(lObjetoJson);

            if (lTransporte.EhExpirarLimite)
            {
                try
                {
                    SalvarParametroRiscoClienteRequest lSalvarParametroRiscoClienteRequest = new SalvarParametroRiscoClienteRequest()
                    {
                        ParametroRiscoCliente = new ParametroRiscoClienteInfo()
                        {
                            CodigoCliente = lTransporte.CodBovespa.DBToInt32(),
                            CodigoParametroCliente = lTransporte.CodigoParametro,
                        }
                    };

                    lSalvarParametroRiscoClienteRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
                    lSalvarParametroRiscoClienteRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                    var lSalvarClientePermissaoParametroResponse = this.ServicoRegrasRisco.SalvarExpirarLimite(lSalvarParametroRiscoClienteRequest);

                    if (lSalvarClientePermissaoParametroResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        base.RegistrarLogExclusao(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = lTransporte.CodBovespa.DBToInt32(), DsObservacao = "Expirar Limite" });  //--> Registrando o Log.

                        return RetornarSucessoAjax(new TransporteRetornoDeCadastro(lSalvarClientePermissaoParametroResponse.CodigoMensagem), "Permissão realizada com sucesso.");
                    }
                    else if (lSalvarClientePermissaoParametroResponse.StatusResposta == MensagemResponseStatusEnum.ErroNegocio)
                    {
                        return RetornarSucessoAjax(lSalvarClientePermissaoParametroResponse.DescricaoResposta);
                    }
                    else
                    {
                        return RetornarErroAjax(lSalvarClientePermissaoParametroResponse.DescricaoResposta);
                    }
                }
                catch (Exception ex)
                {
                    return RetornarErroAjax(ex.Message);
                }
            }
            else if (lTransporte.EhRenovacaoLimite || lTransporte.EhParametro)
            {
                SalvarClientePermissaoParametroRequest lReq = new SalvarClientePermissaoParametroRequest()
                {
                    Associacao = lTransporte.ToAssociacaoClienteRiscoInfo(),
                    DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                    IdUsuarioLogado = base.UsuarioLogado.Id
                };

                try
                {
                    SalvarClientePermissaoParametroResponse lREs = this.ServicoRegrasRisco.SalvarAssociacao(lReq);

                    if (lREs.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        base.RegistrarLogAlteracao(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = lTransporte.CodBovespa.DBToInt32(), DsObservacao = "Renovar Limite" });  //--> Registrando o Log.

                        return RetornarSucessoAjax(new TransporteRetornoDeCadastro(lREs.Associacao.CodigoAssociacao), "Permissão realizada com sucesso.");
                    }
                    else if (lREs.StatusResposta == MensagemResponseStatusEnum.ErroNegocio)
                    {
                        return RetornarSucessoAjax(lREs.DescricaoResposta);
                    }
                    else
                    {
                        return RetornarErroAjax(lREs.DescricaoResposta);
                    }
                }
                catch (Exception ex)
                {
                    return RetornarErroAjax(ex.Message);
                }
            }
            else
            {
                List<PermissaoRiscoAssociadaInfo> itensPermissoes = new List<PermissaoRiscoAssociadaInfo>();
                PermissaoRiscoAssociadaInfo prai;
                int codigo = 0;

                foreach (string item in lTransporte.Permissoes)
                {
                    prai = new PermissaoRiscoAssociadaInfo();
                    prai.PermissaoRisco = new PermissaoRiscoInfo()
                    {
                        CodigoPermissao = int.Parse(item)
                    };

                    if (lTransporte.CodigoGrupo != 0)
                        prai.Grupo = new GrupoInfo() { CodigoGrupo = lTransporte.CodigoGrupo };

                    codigo = 0;

                    if (int.TryParse(lTransporte.CodBovespa, out codigo))
                        prai.CodigoCliente = codigo;
                    else
                        throw new Exception("Cliente não possui CBLC.");

                    itensPermissoes.Add(prai);
                }

                SalvarPermissoesRiscoAssociadasRequest lreqSalvar = new SalvarPermissoesRiscoAssociadasRequest()
                {
                    PermissoesAssociadas = itensPermissoes,
                    DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                    IdUsuarioLogado = base.UsuarioLogado.Id
                };

                MensagemResponseBase lresPer = ServicoRegrasRisco.SalvarPermissoesRiscoAssociadas(lreqSalvar);

                if (lresPer.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogInclusao(new Contratos.Dados.Cadastro.LogIntranetInfo()
                    {
                        CdBovespaClienteAfetado = lTransporte.CodBovespa.DBToInt32(),
                        DsObservacao = string.Concat("Inclusão de limite para o cliente: id_cliente = ", lTransporte.CodigoClienteParametro.ToString())
                    });

                    return RetornarSucessoAjax("Permissões associadas com sucesso.");
                }
                else
                {
                    return RetornarErroAjax(lresPer.DescricaoResposta);
                }
            }
        }

        private string ResponderExcluir()
        {
            string lRetorno = "";

            string lID = Request["Id"];

            if (!string.IsNullOrEmpty(lID))
            {
                if (lID.Split('.')[0] == "1")
                {
                    lRetorno = RetornarErroAjax("Não é possível excluir um parâmetro devido às utilizações de seu valor, altere a data de Validade.");
                }
                else
                {
                    RemoverClientePermissaoParametroRequest lRequest;
                    RemoverClientePermissaoParametroResponse lResponse;

                    try
                    {
                        lRequest = new RemoverClientePermissaoParametroRequest();
                        lRequest.Associacao = new AssociacaoClienteRiscoInfo()
                        {
                            TipoAssociacao = (AssociacaoClienteRiscoInfo.eTipoAssociacao)int.Parse(lID.Split('.')[0]),
                            CodigoClienteParametro = int.Parse(lID.Split('.')[1]),
                            CodigoClientePermissao = int.Parse(lID.Split('.')[1])
                        };
                        lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                        lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
                        try
                        {
                            lResponse = ServicoRegrasRisco.RemoverAssociacao(lRequest);

                            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                            {
                                lRetorno = RetornarSucessoAjax("Dados Excluidos com sucesso", new object[] { });

                                base.RegistrarLogExclusao(); //--> Controle de log.
                            }
                            else
                            {
                                lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                            }
                        }
                        catch (Exception exEnvioRequest)
                        {
                            lRetorno = RetornarErroAjax("Erro durante o envio do request para excluir os dados", exEnvioRequest);
                        }
                    }
                    catch (Exception exConversao)
                    {
                        lRetorno = RetornarErroAjax("Erro ao converter os dados", exConversao);
                    }
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para excluir");
            }

            return lRetorno;
        }

        private string ResponderCarregarHtmlComDados()
        {
            int lId = int.Parse(Request["CodBovespa"]);

            ListarClientePermissaoParametroRequest lReq = new ListarClientePermissaoParametroRequest()
            {
                CodigoCliente = lId,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            try
            {
                ListarClientePermissaoParametroResponse lRes = this.ServicoRegrasRisco.ListarAssociacao(lReq);

                List<TransporteRiscoAssociacaoCliente> lList = new List<TransporteRiscoAssociacaoCliente>();

                if (lRes.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    foreach (AssociacaoClienteRiscoInfo p in lRes.Associacoes)
                    {
                        lList.Add(new TransporteRiscoAssociacaoCliente(p));
                    }
                    this.hidRisco_AssociarPermissoesParametros_ListaJson.Value = JsonConvert.SerializeObject(lList);

                    //this.hidRisco_AssociarPermissoesParametros_CBLC.Value = codigo.ToString();
                }
                else
                {
                    return RetornarErroAjax(lRes.DescricaoResposta);
                }

                ListarPermissoesRiscoClienteRequest lreqPer = new ListarPermissoesRiscoClienteRequest();
                lreqPer.CodigoCliente = lId;
                lreqPer.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                lreqPer.IdUsuarioLogado = base.UsuarioLogado.Id;

                ListarPermissoesRiscoClienteResponse lresPer = ServicoRegrasRisco.ListarPermissoesRiscoCliente(lreqPer);

                if (lresPer.StatusResposta != MensagemResponseStatusEnum.OK)
                    return RetornarErroAjax(lresPer.DescricaoResposta);
                else
                {
                    string itens = string.Empty;
                    foreach (PermissaoRiscoAssociadaInfo item in lresPer.PermissoesAssociadas)
                    {
                        itens += item.PermissaoRisco.CodigoPermissao.ToString() + ";";
                    }
                    itens += "-";
                    itens = itens.Replace(";-", "");
                    this.hidRisco_ListaPermissoesAssociadas.Value = itens;
                }

                RetornarSucessoAjax(String.Format("[{0}] Associações cadastradas.", lRes.Associacoes.Count));

            }
            catch (Exception ex)
            {
                return RetornarErroAjax(ex.Message);
            }

            return string.Empty;
        }

        protected new void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "Salvar"
                                                , "Excluir"
                                                , "CarregarHtmlComDados"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                                , ResponderExcluir
                                                , ResponderCarregarHtmlComDados 
                                                });
            if (!Page.IsPostBack)
            {
                ListarPermissoesRiscoResponse lResPermissoes = ServicoRegrasRisco.ListarPermissoesRisco(new ListarPermissoesRiscoRequest()
                {
                    //Bolsa = BolsaInfo.BOVESPA
                });
                if (lResPermissoes.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    throw new Exception(lResPermissoes.DescricaoResposta);// RetornarErroAjax(lResParametros.DescricaoResposta);
                }
                else
                {
                    this.rpt_Risco_AssociarPermissoesParametros_Permissoes_chk.DataSource = lResPermissoes.Permissoes;
                    this.rpt_Risco_AssociarPermissoesParametros_Permissoes_chk.DataBind();
                }

                ListarParametrosRiscoResponse lResParametros = ServicoRegrasRisco.ListarParametrosRisco(new ListarParametrosRiscoRequest()
                {
                    Bolsa = BolsaInfo.TODAS,
                    DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                    IdUsuarioLogado = base.UsuarioLogado.Id
                });

                if (lResParametros.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    throw new Exception(lResParametros.DescricaoResposta);// RetornarErroAjax(lResParametros.DescricaoResposta);
                }
                else
                {
                    this.rpt_Risco_AssociarPermissoesParametros_Parametros.DataSource = lResParametros.ParametrosRisco;
                    this.rpt_Risco_AssociarPermissoesParametros_Parametros.DataBind();
                }

                ListarGruposResponse lResGrupos = ServicoRegrasRisco.ListarGrupos(new ListarGruposRequest());

                if (lResGrupos.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    throw new Exception(lResGrupos.DescricaoResposta); // RetornarErroAjax(lResParametros.DescricaoResposta);
                }
                else
                {
                    this.rpt_Risco_AssociarPermissoesParametros_Grupo.DataSource = lResGrupos.Grupos;
                    this.rpt_Risco_AssociarPermissoesParametros_Grupo.DataBind();
                }
            }
        }
    }
}
