using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Sistema
{
    public partial class PessoasVinculadas : PaginaBaseAutenticada
    {
        #region | Métodos

        private string ResponderSalvar()
        {
            string lRetorno = string.Empty,
                   lCodigoCliente = this.Request.Form["CodigoCliente"],
                   lCodigoPessoaVinculadaResponsavel = this.Request.Form["CodigoPessoaVinculadaResponsavel"],
                   lNome = this.Request.Form["Nome"],
                   lCpfCnpj = this.Request.Form["CPFCNPJ"],
                   lCodigoPessoaVinculada = this.Request.Form["Id"],
                   lAtivo = this.Request.Form["FlagAtivo"];

            if (!string.IsNullOrEmpty(lNome) && !string.IsNullOrEmpty(lCpfCnpj))
            {
                SalvarEntidadeCadastroRequest<ClientePessoaVinculadaInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lRequest = new SalvarEntidadeCadastroRequest<ClientePessoaVinculadaInfo>();

                    lRequest.EntidadeCadastro = new ClientePessoaVinculadaInfo()
                    {
                        IdCliente = lCodigoCliente.Equals(string.Empty) ? new Nullable<int>() : int.Parse(lCodigoCliente),
                        DsNome = lNome,
                        StAtivo = bool.Parse(lAtivo),
                        IdPessoaVinculadaResponsavel = lCodigoPessoaVinculadaResponsavel.Equals(string.Empty) ? new Nullable<int>() : int.Parse(lCodigoPessoaVinculadaResponsavel),
                        DsCpfCnpj = lCpfCnpj
                    };

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClientePessoaVinculadaInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRequest.EntidadeCadastro.IdPessoaVinculada = int.Parse(lResponse.DescricaoResposta);
                            lRetorno = RetornarSucessoAjax(new TransportePessoaVinculada(lRequest.EntidadeCadastro), "Dados salvos com sucesso", new object[] { });
                            base.RegistrarLogInclusao();
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

        /// <summary>
        /// Excluir pessoa vinculada
        /// </summary>
        /// <returns></returns>
        private string ResponderExcluirPessoaVinculada()
        {
            string lRetorno = "";

            string lID = Request["Id"];

            if (!string.IsNullOrEmpty(lID))
            {
                RemoverEntidadeCadastroRequest<ClientePessoaVinculadaInfo> lRequest;

                RemoverEntidadeCadastroResponse lResponse;

                ReceberEntidadeCadastroResponse<ClientePessoaVinculadaInfo> lResponsePessoa;

                try
                {
                    ClientePessoaVinculadaInfo lPessoa = new ClientePessoaVinculadaInfo();

                    lPessoa.IdPessoaVinculada = int.Parse(lID);

                    lRequest = new RemoverEntidadeCadastroRequest<ClientePessoaVinculadaInfo>()
                    {
                        EntidadeCadastro = lPessoa
                    };
                    try
                    {
                        lResponsePessoa = ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClientePessoaVinculadaInfo>(
                            new ReceberEntidadeCadastroRequest<ClientePessoaVinculadaInfo>()
                            {
                                EntidadeCadastro = new ClientePessoaVinculadaInfo()
                                {
                                    IdPessoaVinculada = int.Parse(lID),
                                    ReceberPessoaVinculada = eReceberPessoaVinculada.PorIdResponsavel
                                },
                                IdUsuarioLogado = base.UsuarioLogado.Id,
                                DescricaoUsuarioLogado = base.UsuarioLogado.Nome
                            }
                        );

                        if (!lResponsePessoa.EntidadeCadastro.IdPessoaVinculada.Equals(0))
                        {
                            object TemErro = true;
                            lRetorno = RetornarSucessoAjax(TemErro, string.Format("Exclusão não permitida. Há pessoa(s) vinculada(s) a essa pessoa!"), new object[] { });
                        }
                        else
                        {
                            lResponse = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClientePessoaVinculadaInfo>(lRequest);

                            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                            {
                                lRetorno = RetornarSucessoAjax("Dados Excluidos com sucesso", new object[] { });
                                base.RegistrarLogExclusao();
                            }
                            else
                            {
                                lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                            }
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
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para excluir");
            }

            return lRetorno;

        }
        /// <summary>
        /// Carrega os dados no formulário
        /// </summary>
        /// <returns></returns>
        private string ResponderCarregarHtmlComDados()
        {
            ConsultarEntidadeCadastroRequest<ClientePessoaVinculadaInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClientePessoaVinculadaInfo>();
            ConsultarEntidadeCadastroResponse<ClientePessoaVinculadaInfo> lResponse;

            ReceberEntidadeCadastroRequest<ClientePessoaVinculadaInfo> req = new ReceberEntidadeCadastroRequest<ClientePessoaVinculadaInfo>()
            {
                EntidadeCadastro = new ClientePessoaVinculadaInfo()
                ,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePessoaVinculadaInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                IEnumerable<TransportePessoaVinculada> lLista = from ClientePessoaVinculadaInfo t
                                                           in lResponse.Resultado
                                                                select new TransportePessoaVinculada(t);

                rptListaDePessoasVinculadas.DataSource = lLista;
                rptListaDePessoasVinculadas.DataBind();
                rowLinhaDeNenhumItem.Visible = (lLista.Count().Equals(0));

            }

            return string.Empty;    //só para obedecer assinatura
        }

        private string ResponderBuscaCliente()
        {
            string lResposta = string.Empty;

            string lCodigoCliente = this.Request.Form["CodigoCliente"];

            var lClienteContaInfo = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteContaInfo>(
                new ReceberEntidadeCadastroRequest<ClienteContaInfo>()
                {
                    EntidadeCadastro = new ClienteContaInfo() { CdCodigo = int.Parse(lCodigoCliente), StPrincipal = true }
                });

            var lClienteInfo = new ReceberEntidadeCadastroRequest<ClienteInfo>()
            {
                EntidadeCadastro = new ClienteInfo()
                {
                    IdCliente = lClienteContaInfo == null ? int.Parse(lCodigoCliente) : lClienteContaInfo.EntidadeCadastro.IdCliente,
                }
            };

            ClienteInfo lInfo = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(lClienteInfo).EntidadeCadastro;

            TransporteDadosCompletosClienteBase lTransporteCliente = null;

            if (lInfo != null)
            {
                lTransporteCliente = new TransporteDadosCompletosClienteBase(lInfo);
                lResposta = RetornarSucessoAjax(lTransporteCliente, "Cliente encontrado!", new object[] { });
            }
            else
                lResposta = RetornarSucessoAjax("Cliente não encontrado!", new object[] { });


            return lResposta;    //só para obedecer assinatura
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] { "SalvarPessoasVinculadas",
                                                       "Excluir",
                                                       "BuscaCliente"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { ResponderSalvar,
                                                       ResponderExcluirPessoaVinculada,
                                                       ResponderBuscaCliente
                                                     });

            if (!Page.IsPostBack)
                this.ResponderCarregarHtmlComDados();
        }

        #endregion
    }
}