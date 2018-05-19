using System;
using System.Collections.Generic;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.Excessoes;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class DadosCompletos_PJ : PaginaBaseAutenticada
    {
        public string glIdAsessorLogado;

        #region Métodos Private

        private string ResponderAtualizarPJ()
        {
            string lRetorno = "";

            string lObjetoJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteDadosCompletosPJ lDadosCliente;

                SalvarEntidadeCadastroRequest<ClienteInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;
                try
                {
                    lDadosCliente = JsonConvert.DeserializeObject<TransporteDadosCompletosPJ>(lObjetoJson);
                    lRequest = new SalvarEntidadeCadastroRequest<ClienteInfo>();

                    //--> Recuperando do banco os dados persistidos para atualização.
                    var clienteInfoBanco = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(
                        new ReceberEntidadeCadastroRequest<ClienteInfo>()
                        {
                            EntidadeCadastro = new ClienteInfo(lDadosCliente.Id),
                            IdUsuarioLogado = base.UsuarioLogado.Id,
                            DescricaoUsuarioLogado = base.UsuarioLogado.Nome
                        });

                    lRequest.EntidadeCadastro = lDadosCliente.ToClienteInfo(clienteInfoBanco.EntidadeCadastro);
                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Cliente alterado com sucesso");

                            base.RegistrarLogAlteracao(new Gradual.Intranet.Contratos.Dados.Cadastro.LogIntranetInfo()
                            {   //--> Registrando o Log
                                IdClienteAfetado = lDadosCliente.Id.DBToInt32(),
                                DsObservacao = string.Concat("Cliente atualizado: id_cliente = ", lDadosCliente.Id)
                            });
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                        }
                    }
                    catch (Exception exEnvioRequest)
                    {
                        lRetorno = RetornarErroAjax("Erro durante o envio do request para alterar o ClienteInfo", exEnvioRequest);
                    }
                }
                catch (ExcessaoConverterParaClienteInfo exConversaoCliente)
                {
                    lRetorno = RetornarErroAjax(exConversaoCliente.Message, exConversaoCliente);
                }
                catch (Exception exDeserializacaoCliente)
                {
                    lRetorno = RetornarErroAjax("Erro durante a deserialização dos dados do cliente", exDeserializacaoCliente);
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para alterar");
            }

            return lRetorno;
        }

        private string ResponderCadastrarPJ()
        {
            string lRetorno = "";

            string lObjetoJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteDadosCompletosPJ lDadosCliente;

                SalvarEntidadeCadastroRequest<ClienteInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDadosCliente = JsonConvert.DeserializeObject<TransporteDadosCompletosPJ>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<ClienteInfo>();

                    try
                    {
                        lRequest.EntidadeCadastro = lDadosCliente.ToClienteInfo();
                        lRequest.EntidadeCadastro.StPasso = 1;
                        lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                        lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                        try
                        {
                            lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteInfo>(lRequest);

                            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                            {
                                lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Cliente cadastrado com sucesso");

                                this.ConfigurarEnvioEmailClienteSenha(lRequest);

                                base.RegistrarLogInclusao(new Gradual.Intranet.Contratos.Dados.Cadastro.LogIntranetInfo()
                                {   //--> Registrando o Log
                                    IdClienteAfetado = lDadosCliente.Id.DBToInt32(),
                                    DsObservacao = string.Concat("Cliente incluído: cd_cliente = ", lDadosCliente.Id)
                                });
                           }
                            else
                            {   //Verificação de cnpj já cadastrado e email já cadastrado
                                lRetorno = (lResponse.DescricaoResposta.ToLower().Contains("cpf/cnpj já cadastrado para outro cliente"))
                                    ? RetornarErroAjax("CNPJ já cadastrado para outro cliente")
                                    : ((lResponse.DescricaoResposta.ToLower().Contains("e-mail já cadastrado para outro cliente")) ? RetornarErroAjax("E-mail já cadastrado para outro cliente")
                                    : RetornarErroAjax(lResponse.DescricaoResposta));
                            }
                        }
                        catch (Exception exEnvioRequest)
                        {
                            lRetorno = RetornarErroAjax("Erro durante o envio do request para cadastrar o ClienteInfo", exEnvioRequest);
                        }
                    }
                    catch (Exception exConversaoCliente)
                    {
                        lRetorno = RetornarErroAjax("Erro durante a conversão dos dados do cliente para ClienteInfo", exConversaoCliente);
                    }
                }
                catch (Exception exDeserializacaoCliente)
                {
                    lRetorno = RetornarErroAjax("Erro durante a deserialização dos dados do cliente", exDeserializacaoCliente);
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para cadastrar");
            }

            return lRetorno;
        }

        private string ResponderCarregarHtmlComDados()
        {
            ClienteInfo lDadosDoCliente = new ClienteInfo(Request["Id"]);

            ReceberEntidadeCadastroRequest<ClienteInfo> lRequest = new ReceberEntidadeCadastroRequest<ClienteInfo>()
            {
                CodigoEntidadeCadastro = Request["Id"],
                EntidadeCadastro = lDadosDoCliente,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            lDadosDoCliente = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(lRequest).EntidadeCadastro;

            btnSalvar.Visible = UsuarioPode("Salvar", "8CBBCD25-D74D-4ef6-9646-28EB37679960");

            TransporteDadosCompletosPJ lClientePf = new TransporteDadosCompletosPJ(lDadosDoCliente);

            hidDadosCompletos_PJ.Value = JsonConvert.SerializeObject(lClientePf);

            return string.Empty;    //só para obedecer assinatura
        }

        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            RegistrarRespostasAjax(new string[] { "Cadastrar"
                                                , "Atualizar"
                                                , "CarregarHtmlComDados"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderCadastrarPJ
                                                , ResponderAtualizarPJ
                                                , ResponderCarregarHtmlComDados  });

            if (!Page.IsPostBack)
            {
                ReceberSessaoRequest lReq = new ReceberSessaoRequest();
                lReq.CodigoSessao = base.CodigoSessao;
                lReq.CodigoSessaoARetornar = base.CodigoSessao;
                ReceberSessaoResponse lRes = base.ServicoSeguranca.ReceberSessao(lReq);
                if (lRes.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    if (lRes.Usuario.Perfis.Contains("6"))
                        glIdAsessorLogado = lRes.Usuario.CodigoAssessor.ToString();
                }

                this.PopularControleComListaDoSinacor(eInformacao.AtividadePJ , rptClientes_DadosCompletos_PrincipalAtividade);

                base.PopularControleComListaDoSinacor(eInformacao.TipoInvestidorPJ, this.rptClientes_DadosCompletos_ObjetivoSocial);

                base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptClientes_DadosCompletos_Assessor);
                
                base.PopularControleComListaDoSinacor(eInformacao.Pais, this.rptClientes_DadosCompletos_PaisDeNascimento);
                
                base.PopularControleComTipoPessoa(eTipoPessoa.Juridica, this.rptClientes_DadosCompletos_Tipo);

            }
        }

        /// <summary>
        /// Captura as variáveis para enviar e-mail ao cliente
        /// </summary>
        private void ConfigurarEnvioEmailClienteSenha(SalvarEntidadeCadastroRequest<ClienteInfo> pRequest)
        {
            if (!string.Empty.Equals(pRequest.EntidadeCadastro.DsEmail))
            {
                Dictionary<string, string> dicVariaveis = new Dictionary<string, string>();

                dicVariaveis.Add("@NomeCliente", pRequest.EntidadeCadastro.DsNome);
                dicVariaveis.Add("@EmailCliente", pRequest.EntidadeCadastro.DsEmail);
                dicVariaveis.Add("@SenhaCliente", pRequest.EntidadeCadastro.DsSenhaGerada);
                dicVariaveis.Add("@AssinaturaEletronica", pRequest.EntidadeCadastro.DsSenhaGerada);
                dicVariaveis.Add("@DataCadastro", DateTime.Now.ToString("dd/MM/yyyy"));

                base.EnviarEmail(pRequest.EntidadeCadastro.DsEmail, "Você foi cadastrado no sistema da Gradual investimentos", "CadastroPasso1.htm", dicVariaveis, eTipoEmailDisparo.Assessor);
            }
        }

        #endregion
    }
}
