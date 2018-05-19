using System;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class SolicitacaoAlteracaoCadastral : PaginaBaseAutenticada
    {
        private string ResponderSalvar()
        {

            string lRetorno = string.Empty;

            string lObjetoJson = this.Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteSolicitacaoAlteracaoCadastral lDados;

                SalvarEntidadeCadastroRequest<ClienteAlteracaoInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDados = JsonConvert.DeserializeObject<TransporteSolicitacaoAlteracaoCadastral>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<ClienteAlteracaoInfo>();

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                    lRequest.EntidadeCadastro = lDados.ToClienteAlteracaoCadastralInfo();

                    if (null == lRequest.EntidadeCadastro.IdAlteracao)
                    {//Novo 
                        lRequest.EntidadeCadastro.IdLoginSolicitante = lRequest.IdUsuarioLogado;
                        lRequest.EntidadeCadastro.DtSolicitacao = DateTime.Now;
                        if (lDados.StResolvido)
                        {
                            //Se resolvido
                            lRequest.EntidadeCadastro.IdLoginRealizacao = lRequest.IdUsuarioLogado;
                            lRequest.EntidadeCadastro.DtRealizacao = DateTime.Now;
                        }
                    }
                    else
                    {//Alteração 
                        if (!lDados.StResolvido)
                        {
                            return RetornarSucessoAjax("Não foi dada baixa na solicitação, pois para isto é necessário clicar em Resolvido!");
                        }
                        else
                        {
                            lRequest.EntidadeCadastro.IdLoginRealizacao = lRequest.IdUsuarioLogado;
                            lRequest.EntidadeCadastro.DtRealizacao = DateTime.Now;
                        }
                    }

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteAlteracaoInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Dados alterados com sucesso");

                            if (lRequest.EntidadeCadastro.IdAlteracao > 0)
                                base.RegistrarLogAlteracao(new Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lRequest.EntidadeCadastro.IdCliente, DsObservacao = string.Concat("Solicitação realizada para o cliente: id_cliente  = ", lRequest.EntidadeCadastro.IdCliente) });
                            else
                                base.RegistrarLogInclusao(new Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lRequest.EntidadeCadastro.IdCliente, DsObservacao = string.Concat("Solicitação realizada para o cliente: id_cliente  = ", lRequest.EntidadeCadastro.IdCliente) });
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
                catch (Exception exConversao)
                {
                    lRetorno = RetornarErroAjax("Erro ao converter os dados", exConversao);
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

            ConsultarEntidadeCadastroRequest<ClienteAlteracaoInfo> lRequest;
            ConsultarEntidadeCadastroResponse<ClienteAlteracaoInfo> lResponse;
            List<ItemSegurancaInfo> list = new List<ItemSegurancaInfo>();


            ClienteAlteracaoInfo lDados = new ClienteAlteracaoInfo()
            {
                IdCliente = int.Parse(Request["Id"])
            };

            lRequest = new ConsultarEntidadeCadastroRequest<ClienteAlteracaoInfo>()
            {
                EntidadeCadastro = lDados,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteAlteracaoInfo>(lRequest);

            //ItemSegurancaInfo lItemSegurancaSalvar = new ItemSegurancaInfo();
            //lItemSegurancaSalvar.Permissoes = new List<string>() { "08faf3a5-06ad-4556-896e-b503f7f5f94d" };
            //lItemSegurancaSalvar.Tag = "Salvar";
            //lItemSegurancaSalvar.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;
            //list.Add(lItemSegurancaSalvar);

            //base.VerificaPermissoesPagina(list).ForEach(delegate(ItemSegurancaInfo item)
            //{
            //    if ("Salvar".Equals(item.Tag))
            //        btnSalvar.Visible = item.Valido.Value;

            //});

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                try
                {
                    IEnumerable<TransporteSolicitacaoAlteracaoCadastral> lLista = from ClienteAlteracaoInfo t
                                                               in lResponse.Resultado
                                                                                  select new TransporteSolicitacaoAlteracaoCadastral(t);//, GetLoginName(t.IdLoginRealizacao));

                    hidAcoes_AlteracaoCadastral_ListaJson.Value = JsonConvert.SerializeObject(lLista);
                }
                catch (Exception ex)
                {
                    RetornarErroAjax("Erro ao consultar as solicitações de alteração do cliente", ex.Message);
                }
            }
            else
            {
                RetornarErroAjax("Erro ao consultar as solicitações de alteração do cliente", lResponse.DescricaoResposta);
            }

            return string.Empty;    //só para obedecer assinatura
        }

        private string GetLoginName(int? pIdLogin)
        {
            if (pIdLogin == null || pIdLogin == 0)
            {
                return "";
            }
            else
            {
                ReceberEntidadeCadastroRequest<LoginInfo> lLoginEntrada = new ReceberEntidadeCadastroRequest<LoginInfo>();
                ReceberEntidadeCadastroResponse<LoginInfo> lLoginRetorno;
                lLoginEntrada.EntidadeCadastro = new LoginInfo();
                lLoginEntrada.EntidadeCadastro.IdLogin = pIdLogin;
                lLoginRetorno = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<LoginInfo>(lLoginEntrada);
                if (lLoginRetorno.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lLoginRetorno.EntidadeCadastro.TpAcesso == Contratos.Dados.Enumeradores.eTipoAcesso.Assessor)
                    {
                        //Pegar do Sinacor
                        var lLoginAssessor = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaComboInfo>(new ConsultarEntidadeCadastroRequest<SinacorListaComboInfo>() //new ConsultarEntidadeRequest<SinacorListaComboInfo>()
                        {
                            DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                            IdUsuarioLogado = base.UsuarioLogado.Id,
                            EntidadeCadastro = new SinacorListaComboInfo()
                           {
                               Informacao = Contratos.Dados.Enumeradores.eInformacao.Assessor,
                               Filtro = lLoginRetorno.EntidadeCadastro.CdAssessor.ToString()
                           }
                        });
                        if (lLoginAssessor.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            return lLoginAssessor.Resultado[0].Value;
                        }
                        else
                        {
                            throw new Exception(lLoginAssessor.DescricaoResposta);
                        }
                    }
                    else
                    {
                        return lLoginRetorno.EntidadeCadastro.DsNome;
                    }
                }
                else
                {
                    throw new Exception(lLoginRetorno.DescricaoResposta);
                }
            }
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            this.NovaSolicitacaoAlteracao.Visible = btnSalvar.Visible = base.UsuarioPode("Salvar", "08faf3a5-06ad-4556-896e-b503f7f5f94d");

            base.RegistrarRespostasAjax(new string[] { "Salvar"
                                                     , "CarregarHtmlComDados"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                                     , ResponderCarregarHtmlComDados 
                                                     });
        }
    }
}