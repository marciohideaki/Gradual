using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class DocumentacaoEntregue : PaginaBaseAutenticada
    {
        #region | Propriedades

        private int GetIdItemSelecionadoParaExclusao
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["Id"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodigoCliente
        {
            get
            {
                var lRetorno = default(int);

                if (int.TryParse(this.Request.Form["Id"], out lRetorno))
                    return lRetorno;

                int.TryParse(this.Request.Form["CdCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetObjetoJson
        {
            get
            {
                return this.Request.Form["ObjetoJson"];
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] { "Salvar"
                                                     , "CarregarHtmlComDados"
                                                     , "Excluir"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                                     , ResponderCarregarHtmlComDados 
                                                     , ResponderExcluir});
        }

        #endregion

        #region | Métodos

        private string ResponderCarregarHtmlComDados()
        {
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

            //bool lExcluir = true;
            //base.VerificaPermissoesPagina(lListaItemSeguranca).ForEach(delegate(ItemSegurancaInfo item)
            //{
            //    if ("Salvar".Equals(item.Tag))
            //        NovaPendencia.Visible = btnSalvar.Visible = item.Valido.Value;

            //    if ("Excluir".Equals(item.Tag))
            //        lExcluir = item.Valido.Value;
            //});

            var lResponse = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteDocumentacaoEntregueInfo>(
                new ConsultarEntidadeCadastroRequest<ClienteDocumentacaoEntregueInfo>()
                {
                    EntidadeCadastro = new ClienteDocumentacaoEntregueInfo()
                    {
                        IdCliente = this.GetCodigoCliente
                    }
                });

            if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
            {
                var lLista = new TransporteDocumentacaoEntregue().TraduzirLista(lResponse.Resultado);

                this.hidAcoes_DocumentacaoEntregue_ListaJson.Value = JsonConvert.SerializeObject(lLista);
            }
            else
            {
                throw new Exception(base.RetornarErroAjax(lResponse.DescricaoResposta));
            }

            return string.Empty;
        }

        private string ResponderSalvar()
        {
            var lRetorno = string.Empty;

            if (!string.IsNullOrWhiteSpace(this.GetObjetoJson))
            {
                try
                {
                    var lDados = JsonConvert.DeserializeObject<TransporteDocumentacaoEntregue>(this.GetObjetoJson);

                    var lRequest = new SalvarEntidadeCadastroRequest<ClienteDocumentacaoEntregueInfo>() { EntidadeCadastro = lDados.ToClienteDocumentacaoEntregueInfo() };

                    lRequest.EntidadeCadastro.IdLoginUsuarioLogado = base.UsuarioLogado.Id;
                    lRequest.EntidadeCadastro.IdCliente = this.GetCodigoCliente;

                    var lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteDocumentacaoEntregueInfo>(lRequest);

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = RetornarSucessoAjax(new TransporteDocumentacaoEntregue().TraduzirLista((ClienteDocumentacaoEntregueInfo)lResponse.Objeto), "Dados alterados com sucesso");

                        if (lRequest.EntidadeCadastro.IdDocumentacaoEntregue > 0)
                            base.RegistrarLogAlteracao(new Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lRequest.EntidadeCadastro.IdCliente });
                        else
                            base.RegistrarLogInclusao(new Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lRequest.EntidadeCadastro.IdCliente });
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                    }
                }
                catch (Exception ex)
                {
                    lRetorno = RetornarErroAjax("Erro durante o envio do request para salvar os dados", ex);
                }
            }

            return lRetorno;
        }

        private string ResponderExcluir()
        {
            var lRetorno = string.Empty;

            if (this.GetIdItemSelecionadoParaExclusao > 0)
            {
                try
                {
                    var lResponse = base.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteDocumentacaoEntregueInfo>(
                        new RemoverEntidadeCadastroRequest<ClienteDocumentacaoEntregueInfo>()
                        {
                            EntidadeCadastro = new ClienteDocumentacaoEntregueInfo()
                            {
                                IdDocumentacaoEntregue = this.GetIdItemSelecionadoParaExclusao
                            }
                        });

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = RetornarSucessoAjax("Dados Excluidos com sucesso", new object[] { });

                        base.RegistrarLogExclusao(new Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = this.GetCodigoCliente, });
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                    }
                }
                catch (Exception ex)
                {
                    lRetorno = RetornarErroAjax("Erro durante o envio do request para excluir os dados", ex);
                }
            }

            return lRetorno;
        }

        #endregion
    }
}