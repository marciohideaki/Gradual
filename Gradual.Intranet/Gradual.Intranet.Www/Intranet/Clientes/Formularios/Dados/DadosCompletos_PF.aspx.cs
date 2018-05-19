using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
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
    public partial class DadosCompletos_PF : PaginaBaseAutenticada
    {
        #region | Atributos

        public string glIdAsessorLogado;

        #endregion

        #region | Métodos

        private string ResponderAtualizarPF()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = this.Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteDadosCompletosPF lDadosCliente;

                SalvarEntidadeCadastroRequest<ClienteInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDadosCliente = JsonConvert.DeserializeObject<TransporteDadosCompletosPF>(lObjetoJson);
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

                        if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
                        {
                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Cliente alterado com sucesso");
                            base.RegistrarLogAlteracao(new LogIntranetInfo()
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

        private string ResponderCadastrarPF()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = this.Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteDadosCompletosPF lDadosCliente;

                SalvarEntidadeCadastroRequest<ClienteInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDadosCliente = JsonConvert.DeserializeObject<TransporteDadosCompletosPF>(lObjetoJson);

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

                            if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
                            {
                                lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Cliente cadastrado com sucesso");

                                try
                                {
                                    this.ConfigurarEnvioEmail(lRequest);

                                    this.ConfigurarEnvioEmailClienteSenha(lRequest);
                                }
                                catch (Exception exx)
                                {
                                    Logger.ErrorFormat("Erro ao enviar email pro cliente: {0}\r\n{1}", exx.Message, exx.StackTrace);
                                }

                                base.RegistrarLogInclusao(new LogIntranetInfo()
                                {   //--> Registrando o Log
                                    IdClienteAfetado = lDadosCliente.Id.DBToInt32(),
                                    DsObservacao = string.Concat("Cliente incluído: cd_cliente = ", lDadosCliente.Id)
                                });
                            }
                            else
                            {
                                //Verificação de cnpj já cadastrado e email já cadastrado
                                lRetorno = (lResponse.DescricaoResposta.ToLower().Contains("cpf/cnpj já cadastrado para outro cliente"))
                                         ? RetornarErroAjax("CPF já cadastrado para outro cliente")
                                         : (lResponse.DescricaoResposta.ToLower().Contains("e-mail já cadastrado para outro cliente"))
                                         ? RetornarErroAjax("E-mail já cadastrado para outro cliente")
                                         : RetornarErroAjax(lResponse.DescricaoResposta);
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

        /// <summary>
        /// Configura o envio de alerta ao compliance caso o cliente seja menor de 18 ou maior de 65 anos.
        /// </summary>
        private void ConfigurarEnvioEmail(SalvarEntidadeCadastroRequest<ClienteInfo> pRequest)
        {
            if (null != pRequest.EntidadeCadastro.DtNascimentoFundacao)
            {
                var lIDadeEmDias = DateTime.Now.Subtract(pRequest.EntidadeCadastro.DtNascimentoFundacao.Value).Days;

                var lVariaveis = new System.Collections.Generic.Dictionary<string, string>();
                lVariaveis.Add("@NomeCliente", pRequest.EntidadeCadastro.DsNome);
                lVariaveis.Add("@CPF", pRequest.EntidadeCadastro.DsCpfCnpj);

                if (lIDadeEmDias >= 23725)    //--> maior 65 anos
                    base.EnviarEmail(ConfigurationManager.AppSettings["EmailCompliance"], "Sistema de cadastro - Clientes para analise", "CadastroClienteMaior65.htm", lVariaveis, eTipoEmailDisparo.Compliance);

                else if (lIDadeEmDias <= 6574) //--> menor 18 anos
                    base.EnviarEmail(ConfigurationManager.AppSettings["EmailCompliance"], "Sistema de cadastro - Clientes para analise", "CadastroClienteMenor18.htm", lVariaveis, eTipoEmailDisparo.Compliance);
            }
        }

        private string ResponderCarregarHtmlComDados()
        {
            ClienteInfo lDadosDoCliente = new ClienteInfo(Request["Id"]);

            TransporteDadosCompletosPF lClientePf;

            ReceberEntidadeCadastroRequest<ClienteInfo> req = new ReceberEntidadeCadastroRequest<ClienteInfo>()
            {
                CodigoEntidadeCadastro = Request["Id"],
                EntidadeCadastro = lDadosDoCliente,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            lDadosDoCliente = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(req).EntidadeCadastro;

            lDadosDoCliente.DadosClienteNaoOperaPorContaPropria = this.RecuperarDadosDeClienteNaoOperaPorContaPropria(lDadosDoCliente.IdCliente.Value);

            btnSalvar.Visible = UsuarioPode("Salvar", "8CBBCD25-D74D-4ef6-9646-28EB37679960");

            lClientePf = new TransporteDadosCompletosPF(lDadosDoCliente);

            hidDadosCompletos_PF.Value = JsonConvert.SerializeObject(lClientePf);

            return string.Empty;    //só para obedecer assinatura
        }

        private ClienteNaoOperaPorContaPropriaInfo RecuperarDadosDeClienteNaoOperaPorContaPropria(int pIdCliente)
        {
            return this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteNaoOperaPorContaPropriaInfo>(
                new ReceberEntidadeCadastroRequest<ClienteNaoOperaPorContaPropriaInfo>() 
                {
                    EntidadeCadastro = new ClienteNaoOperaPorContaPropriaInfo()
                    {
                        IdCliente = pIdCliente,
                    }
                }).EntidadeCadastro;
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "Cadastrar"
                                                , "Atualizar"
                                                , "CarregarHtmlComDados"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderCadastrarPF
                                                , ResponderAtualizarPF
                                                , ResponderCarregarHtmlComDados });

            if (!Page.IsPostBack)
            {
                ReceberSessaoResponse lRes = base.ServicoSeguranca.ReceberSessao(new ReceberSessaoRequest()
                {
                    CodigoSessao = base.CodigoSessao,
                    CodigoSessaoARetornar = base.CodigoSessao,
                });

                if (lRes.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    if(lRes.Usuario.Perfis.Contains("6"))
                        glIdAsessorLogado = lRes.Usuario.CodigoAssessor.ToString();
                }

                base.PopularControleComListaDoSinacor(eInformacao.ProfissaoPF, this.rptClientes_DadosCompletos_Profissao);

                base.PopularControleComListaDoSinacor(eInformacao.EstadoCivil, this.rptClientes_DadosCompletos_EstadoCivil);

                base.PopularControleComListaDoSinacor(eInformacao.Nacionalidade, this.rptClientes_DadosCompletos_Nacionalidade);

                base.PopularControleComListaDoSinacor(eInformacao.OrgaoEmissor, this.rptClientes_DadosCompletos_OrgaoEmissor);

                base.PopularControleComListaDoSinacor(eInformacao.TipoDocumento, this.rptClientes_DadosCompletos_TipoDeDocumento);

                base.PopularControleComListaDoSinacor(eInformacao.Pais, this.rptClientes_DadosCompletos_PaisDeNascimento);

                base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptClientes_DadosCompletos_Assessor);

                base.PopularControleComListaDoSinacor(eInformacao.Estado, this.rptClientes_DadosCompletos_EstadoDeNascimento, this.rptClientes_DadosCompletos_EstadoEmissao);

                base.PopularControleComTipoPessoa(eTipoPessoa.Fisica, this.rptClientes_DadosCompletos_Tipo);
            }
        }

        #endregion
    }
}
