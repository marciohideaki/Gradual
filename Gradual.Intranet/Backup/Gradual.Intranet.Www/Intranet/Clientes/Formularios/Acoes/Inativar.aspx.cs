using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Contratos.Dados.Portal;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class Inativar : PaginaBaseAutenticada
    {
        #region | Metodos

        private string ResponderSalvar()
        {
            //$("#pnlClientes_Formularios_Acoes_Inativar table.GridIntranet tbody input[type='checkbox']:checked")
            //$("#chkAcoes_AtivarInativar_Login_Confirma").is(":checked")

            string lRetorno = string.Empty;

            string lIdCliente = Request.Form["IdCliente"];
            string lAtivoCliGer = Request.Form["StAtivoCliGer"];
            string lAtivoPortal = Request.Form["StAtivoLogin"];
            string lAtivoHb = Request.Form["StAtivoHb"];
            string lCkbChecked = Request.Form["DsCkbTrue"];

            if (!string.IsNullOrEmpty(lIdCliente))
            {
                SalvarEntidadeCadastroRequest<ClienteAtivarInativarInfo> lRequest;
                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lRequest = new SalvarEntidadeCadastroRequest<ClienteAtivarInativarInfo>();

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    //Pega Antiga
                    lRequest.EntidadeCadastro = GetContas(int.Parse(lIdCliente));

                    //Atualizando de acordo com a tela
                    lRequest.EntidadeCadastro.IdCliente = int.Parse(lIdCliente);
                    lRequest.EntidadeCadastro.StLoginAtivo = Boolean.Parse(lAtivoPortal);
                    lRequest.EntidadeCadastro.StClienteGeralAtivo = Boolean.Parse(lAtivoCliGer);
                    lRequest.EntidadeCadastro.StHbAtivo = Boolean.Parse(lAtivoHb);
                    foreach (ClienteAtivarInativarContasInfo item in lRequest.EntidadeCadastro.Contas)
                    {
                        if (item.Bovespa != null)
                        {
                            item.Bovespa.StAtiva = lCkbChecked.IndexOf(string.Concat("ckbBov", item.CdCodigo.ToString(), ",")) > -1 ? true : false;
                        }
                        
                        if (item.Bmf != null)
                        {
                            item.Bmf.StAtiva = lCkbChecked.IndexOf(string.Concat("ckbBmf", item.CdCodigo.ToString(), ",")) > -1 ? true : false;
                        }

                        if (item.CC != null)
                        {
                            item.CC.StAtiva = lCkbChecked.IndexOf(string.Concat("ckbCc", item.CdCodigo.ToString(), ",")) > -1 ? true : false;
                        }

                        if (item.Custodia != null)
                        {
                            item.Custodia.StAtiva = lCkbChecked.IndexOf(string.Concat("ckbCus", item.CdCodigo.ToString(), ",")) > -1 ? true : false;
                        }
                    }

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteAtivarInativarInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax(new TransporteAtivarInativarCliente(lRequest.EntidadeCadastro), "Dados alterados com sucesso", new object[] { });

                            base.RegistrarLogAlteracao(new Contratos.Dados.Cadastro.LogIntranetInfo()
                            {   //--> Registrando o Log
                                IdClienteAfetado = lRequest.EntidadeCadastro.IdCliente,
                                DsObservacao = string.Concat("Cliente inativado: id_cliente = ", lRequest.EntidadeCadastro.IdCliente)
                            });
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                        }
                    }
                    catch (Exception exEnvioRequest)
                    {
                        lRetorno = RetornarErroAjax("Erro durante o envio do request para salvar os dados", exEnvioRequest);
                    }
                }
                catch (Exception ex)
                {
                    lRetorno = RetornarErroAjax("Erro ao Capturar Informações da Tela", ex);
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para alterar");
            }
            return lRetorno;
        }

        private string ResponderCarregarHtmlComDados()
        {
            ClienteAtivarInativarInfo lContas = GetContas(int.Parse(Request["Id"]));
            List<ItemSegurancaInfo> list = new List<ItemSegurancaInfo>();
            TransporteAtivarInativarCliente lTransporte = new TransporteAtivarInativarCliente(lContas);

            hdAcoes_Inativar_Id_Cliente.Value = lTransporte.IdCliente.ToString();

            rptClientes_AtivarInativar.DataSource = lTransporte.Contas;
            rptClientes_AtivarInativar.DataBind();

            if (null != lContas.DtUltimaAtualizacao && DateTime.MinValue != lContas.DtUltimaAtualizacao)
            {
                pnlDataAtivacaoInativacao.Visible = true;

                pnlDataAtivacaoInativacao.InnerHtml = string.Format("<br/>Cliente Ativado/Inativado pela última vez em: <em>{0}</em>", lContas.DtUltimaAtualizacao);
            }
            else
            {
                pnlDataAtivacaoInativacao.Visible = false;
            }

            hdAcoes_Inativar_Transporte.Value = JsonConvert.SerializeObject(lTransporte);

            ItemSegurancaInfo lItemSegurancaSalvar = new ItemSegurancaInfo();
            lItemSegurancaSalvar.Permissoes = new List<string>() { "79175a20-6129-4d2b-bd6e-bb36f3c31f04" };
            lItemSegurancaSalvar.Tag = "Salvar";
            lItemSegurancaSalvar.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;
            list.Add(lItemSegurancaSalvar);

            base.VerificaPermissoesPagina(list).ForEach(delegate(ItemSegurancaInfo item)
            {
                if ("Salvar".Equals(item.Tag))
                    btnSalvar.Visible = item.Valido.Value;
            });

            return string.Empty;    //só para obedecer assinatura
        }

        private string ResponderZerarTentativasInvalidasDeLogin()
        {
            var lRetorno = string.Empty;

            var lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LoginLiberarAcesoTentativasErradasInfo>(
                new SalvarEntidadeCadastroRequest<LoginLiberarAcesoTentativasErradasInfo>()
                {
                    EntidadeCadastro = new LoginLiberarAcesoTentativasErradasInfo()
                    {
                        CdCodigo = base.Request.Form["CdCodigo"].DBToInt32(),
                        DsEmail =  base.Request.Form["email"].DBToString().ToLower()
                    }
                });

            if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
                lRetorno = base.RetornarSucessoAjax(string.Format("Acesso liberado para o cliente {0}", base.Request.Form["Nome"].ToStringFormatoNome()));
            else
                lRetorno = base.RetornarErroAjax("Não foi possível liberar o acesso para este cliente", lResponse.DescricaoResposta);

            return lRetorno;
        }

        private ClienteAtivarInativarInfo GetContas(int pIdCliente)
        {
            ReceberEntidadeCadastroRequest<ClienteAtivarInativarInfo> lRequest;
            ReceberEntidadeCadastroResponse<ClienteAtivarInativarInfo> lResponse;

            ClienteAtivarInativarInfo lDados = new ClienteAtivarInativarInfo()
            {
                IdCliente = pIdCliente
            };

            lRequest = new ReceberEntidadeCadastroRequest<ClienteAtivarInativarInfo>()
            {
                EntidadeCadastro = lDados,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            lResponse = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteAtivarInativarInfo>(lRequest);
            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                return lResponse.EntidadeCadastro;
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "Salvar"
                                                , "CarregarHtmlComDados"
                                                , "ZerarTentativasInvalidasDeLogin"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                                , ResponderCarregarHtmlComDados
                                                , ResponderZerarTentativasInvalidasDeLogin
                                                });
            //if (!this.Page.IsPostBack)
            //{
            //    this.ResponderCarregarHtmlComDados();
            //}
        }

        #endregion
    }
}
