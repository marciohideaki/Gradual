using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class VerPendencias : PaginaBaseAutenticada
    {
        #region | Propriedades

        private int? GetCodigoCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CodCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int GetCodigoAssessor
        {
            get 
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CodAssessor"], out lRetorno))
                    return 956000;

                return lRetorno;
            }
        }

        #endregion

        #region | Metodos

        private string ResponderSalvar()
        {
            string lRetorno = "";

            string lObjetoJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransportePendenciaCadastral lDados;

                SalvarEntidadeCadastroRequest<ClientePendenciaCadastralInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDados = JsonConvert.DeserializeObject<TransportePendenciaCadastral>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<ClientePendenciaCadastralInfo>();

                    if (lDados.FlagResolvido)
                        lRequest.EntidadeCadastro = lDados.ToClientePendenciaCadastralInfo(lRequest.IdUsuarioLogado);
                    else
                        lRequest.EntidadeCadastro = lDados.ToClientePendenciaCadastralInfo();

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    lRequest.EntidadeCadastro.IdLoginRealizacao = lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                    try
                    {
                        lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClientePendenciaCadastralInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Dados alterados com sucesso");

                            if (lRequest.EntidadeCadastro.IdPendenciaCadastral > 0)
                                base.RegistrarLogAlteracao(new Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lRequest.EntidadeCadastro.IdCliente, DsObservacao = string.Concat("Tipo de pendência: ", lDados.TipoDesc) });
                            else
                                base.RegistrarLogInclusao(new Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lRequest.EntidadeCadastro.IdCliente, DsObservacao = string.Concat("Tipo de pendência: ", lDados.TipoDesc) });
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

        private string ResponderExcluir()
        {
            string lRetorno = "";

            string lID = Request["Id"];

            if (!string.IsNullOrEmpty(lID))
            {
                RemoverEntidadeCadastroRequest<ClientePendenciaCadastralInfo> lRequest;

                RemoverEntidadeCadastroResponse lResponse;

                try
                {
                    ClientePendenciaCadastralInfo lPendencia = new ClientePendenciaCadastralInfo();

                    lPendencia.IdPendenciaCadastral = int.Parse(lID);

                    lRequest = new RemoverEntidadeCadastroRequest<ClientePendenciaCadastralInfo>()
                    {
                        EntidadeCadastro = lPendencia,
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
                    };

                    try
                    {
                        lResponse = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClientePendenciaCadastralInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax("Dados Excluidos com sucesso", new object[] { });

                            base.RegistrarLogExclusao(new Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lID.DBToInt32(), });
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
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para excluir");
            }

            return lRetorno;

        }

        private string ResponderNotificarAssessor()
        {
            var lPendenciasDoCliente = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<PendenciaClienteAssessorInfo>(
                new ConsultarEntidadeCadastroRequest<PendenciaClienteAssessorInfo>()
                {
                    EntidadeCadastro = new PendenciaClienteAssessorInfo()
                    {
                        IdCliente = this.GetCodigoCliente,
                    },
                });

            if (MensagemResponseStatusEnum.OK.Equals(lPendenciasDoCliente.StatusResposta))
            {
                var lClientesTexto = new StringBuilder();
                var lObjetoAssessor = base.BuscarListaDoSinacor(new SinacorListaInfo(eInformacao.Assessor)).Find(ass => { return ass.Id == this.GetCodigoAssessor.ToString(); });
                var lDestinatarioNome = (lObjetoAssessor != null) ? lObjetoAssessor.Value : string.Empty;

                var lListaAssessores = base.ConsultarListaEmailAssessor(this.GetCodigoAssessor);

                if (null != lListaAssessores && null != lListaAssessores.ListaEmailAssessor && lListaAssessores.ListaEmailAssessor.Count > 0)
                {
                    if (null != lPendenciasDoCliente.Resultado
                    && (lPendenciasDoCliente.Resultado.Count > 0))
                    {
                        lClientesTexto.AppendFormat("<b>Nome:</b> {0}<br />", lPendenciasDoCliente.Resultado[0].NomeCliente);
                        lClientesTexto.AppendFormat("<b>CPF/CNPJ:</b> {0}<br />", lPendenciasDoCliente.Resultado[0].CpfCnpjCliente.ToCpfCnpjString());
                        lClientesTexto.AppendFormat("<b>Email:</b> {0}<br />", lPendenciasDoCliente.Resultado[0].EmailCliente);
                        lClientesTexto.AppendFormat("<b>Código:</b> {0}<br />", (null != lPendenciasDoCliente.Resultado[0].CdBmfBovespa) ? lPendenciasDoCliente.Resultado[0].CdBmfBovespa.ToCodigoClienteFormatado() : string.Empty);
                        lClientesTexto.Append("<b>Pendências: </b><ul style=\"margin-left: 45px;\">");

                        lPendenciasDoCliente.Resultado.ForEach(
                            pen =>
                            {
                                lClientesTexto.AppendFormat("<li><u>{0}</u> - <span style=\"font-size:smaller\">{1}</span></li><br />", pen.TpPendenciaDescricao, pen.DescricaoPendencia);
                            });

                        lClientesTexto = lClientesTexto.Remove(lClientesTexto.Length - 2, 2).Append("</ul><br />");

                        var lVariaveisEmail = new Dictionary<string, string>();

                        lVariaveisEmail.Add("@clientes", lClientesTexto.ToString());
                        lVariaveisEmail.Add("@nome", lDestinatarioNome);

                        var lRetornoEnvioEmail = MensagemResponseStatusEnum.OK;
                        var lMensagemDeErro = new StringBuilder();

                        lListaAssessores.ListaEmailAssessor.ForEach(
                            lDestinatarioEmail =>
                            {
                                lRetornoEnvioEmail = base.EnviarEmail(lDestinatarioEmail, "Clientes com Pendências", "CadastroPendenciaClienteAssessor.htm", lVariaveisEmail, Contratos.Dados.Enumeradores.eTipoEmailDisparo.Assessor);

                                if (!MensagemResponseStatusEnum.OK.Equals(lRetornoEnvioEmail))
                                    lMensagemDeErro.AppendFormat("<br />{0}", lDestinatarioEmail);
                            });

                        if (lMensagemDeErro.ToString().Length > 0)
                        {
                            return base.RetornarErroAjax("Ocorreu um erro ao processar o envio de email para o assessor", string.Concat("Os e-mails abaixo não receberam a notificação:", lMensagemDeErro.ToString()));
                        }
                    }
                    else
                    {
                        return base.RetornarSucessoAjax("O cliente não possui pendências no momento.");
                    }
                }
                else
                {
                    return base.RetornarErroAjax("Ocorreu um erro ao processar o envio de email para o assessor", lPendenciasDoCliente.DescricaoResposta);
                }
            }
            else
            {
                return base.RetornarErroAjax("Ocorreu um erro ao processar o envio de email para o assessor", lPendenciasDoCliente.DescricaoResposta);
            }

            return base.RetornarSucessoAjax("E-mail enviado com sucesso.");
        }

        private string ResponderCarregarHtmlComDados()
        {
            ConsultarEntidadeCadastroRequest<ClientePendenciaCadastralInfo> lRequest;
            ConsultarEntidadeCadastroResponse<ClientePendenciaCadastralInfo> lResponse;
            bool lExcluir = true;

            var lDados = new ClientePendenciaCadastralInfo()
            {
                IdCliente = int.Parse(this.Request["Id"])
            };

            lRequest = new ConsultarEntidadeCadastroRequest<ClientePendenciaCadastralInfo>()
            {
                EntidadeCadastro = lDados,
                IdUsuarioLogado = base.UsuarioLogado.Id,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome
            };

            var lListaItemSeguranca = new List<ItemSegurancaInfo>();
            var lItemSegurancaSalvar = new ItemSegurancaInfo();
            lItemSegurancaSalvar.Permissoes = new List<string>() { "C33D260E-0050-45a2-BBD3-2EFBF96E7C4F" };
            lItemSegurancaSalvar.Tag = "Salvar";
            lItemSegurancaSalvar.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;
            lListaItemSeguranca.Add(lItemSegurancaSalvar);

            var lItemSegurancaExcluir = new ItemSegurancaInfo();
            lItemSegurancaExcluir.Permissoes = new List<string>() { "E390C1DD-7CB1-4a9f-8E92-24F8E1F6F4A1" };
            lItemSegurancaExcluir.Tag = "Excluir";
            lItemSegurancaExcluir.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;
            lListaItemSeguranca.Add(lItemSegurancaExcluir);

            base.VerificaPermissoesPagina(lListaItemSeguranca).ForEach(delegate(ItemSegurancaInfo item)
            {
                if ("Salvar".Equals(item.Tag))
                    NovaPendencia.Visible = btnSalvar.Visible = item.Valido.Value;

                if ("Excluir".Equals(item.Tag))
                    lExcluir = item.Valido.Value;
            });

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePendenciaCadastralInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                IEnumerable<TransportePendenciaCadastral> lLista = from ClientePendenciaCadastralInfo t
                                                                   in lResponse.Resultado
                                                                   select new TransportePendenciaCadastral(t, lExcluir);

                hidAcoes_PendenciaCadastral_ListaJson.Value = JsonConvert.SerializeObject(lLista);
            }
            else
            {
                throw new Exception(RetornarErroAjax(lResponse.DescricaoResposta));
            }

            return string.Empty;
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "Salvar"
                                                , "CarregarHtmlComDados"
                                                , "NotificarAssessor"
                                                , "Excluir"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                                , ResponderCarregarHtmlComDados 
                                                , ResponderNotificarAssessor   
                                                , ResponderExcluir});

            if (!Page.IsPostBack)
            {
                this.PopularControleComListaDoCadastro<TipoDePendenciaCadastralInfo>(new TipoDePendenciaCadastralInfo(), rptAcoes_PendenciaCadastral_Tipo);
            }
        }

        #endregion
    }
}