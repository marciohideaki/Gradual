using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Newtonsoft.Json;
using System.Data.Common;
using Gradual.OMS.Risco.Regra;
using Gradual.Generico.Dados;
using Gradual.OMS.Library;
using Gradual.Core.OMS.LimiteManager.Lib;
using Gradual.Core.OMS.LimiteManager.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class ManutencaoRestricoesSpider : PaginaBaseAutenticada
    {
        #region Propriedades
        
        public string GetTipoRegra
        {
            get
            {
                string lRetorno = string.Empty;

                lRetorno = this.Request.Form["TipoRegra"].ToString();

                return lRetorno;
            }
        }

        private int? GetIdGrupo
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["IdGrupo"], out lRetorno);

                return lRetorno;
            }
        }

        private bool _PermissaoExcluir { get; set; }

        private int? GetIdCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["CodigoCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private int? GetIdAcao
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["CodigoAcao"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetSentido
        {
            get
            {
                return Request.Form["Direcao"].ToString();
            }
        }

        private string GetAtivo
        {
            get
            {
                return Request.Form["Ativo"].ToString();
            }
        }

        private List<string> GetListaItemGrupoNovos
        {
            get
            {
                var lRetorno = new List<string>();

                if (!string.IsNullOrWhiteSpace(this.Request.Form["GrupoItensNovos"]))
                {
                    var lArrayItens = this.Request.Form["GrupoItensNovos"].Trim('|').Split('|');

                    lRetorno.AddRange(lArrayItens);
                }

                return lRetorno;
            }
        }

        private List<GrupoItemInfo> MontarListaGrupoItem()
        {
            var lRetorno = new List<GrupoItemInfo>();

            if (null != GetListaItemGrupoNovos && GetListaItemGrupoNovos.Count > 0)
            {
                GetListaItemGrupoNovos.ForEach(lItemGrupo =>
                {
                    lRetorno.Add(new GrupoItemInfo()
                    {
                        CodigoGrupo = this.GetIdGrupo,
                        NomeGrupoItem = lItemGrupo
                    });
                });
            }

            return lRetorno;
        }
        #endregion

        #region Eventos
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (base.Acao == "CarregarHtml")
            {
                this.ResponderCarregarHtmlComDados();
            }

            base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                     , "SalvarClienteBloqueioInstrumentoDirecao"
                                                     , "SalvarRegraGrupoItem"
                                                     , "SalvarRegraGrupoItemGlobal"
                                                     , "CarregarListaRegraGrupoItem"
                                                     , "CarregarListaClienteBloqueioInstrumento"
                                                     , "CarregarListaRegraGrupoItemGlobal"
                                                     , "ExcluirRegraGrupoItem"
                                                     , "ExcluirClienteBloqueioInstrumento"
                                                     , "ExcluirRegraGrupoItemGlobal"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderCarregarHtmlComDados
                                                     , this.ResponderSalvarClienteBloqueioInstrumentoDirecao
                                                     , this.ResponderSalvarRegraGrupoItem
                                                     , this.ResponderSalvarRegraGrupoItemGlobal
                                                     , this.ResponderCarregarListaRegraGrupoItem
                                                     , this.ResponderCarregarListaClienteBloqueioInstrumento
                                                     , this.ResponderCarregarListaRegraGrupoItemGlobal
                                                     , this.ResponderExcluirRegraGrupoItem
                                                     , this.ResponderExcluirClienteBloqueioInstrumento
                                                     , this.ResponderExcluirRegraGrupoItemGlobal
                                                     });
        }
        #endregion

        #region Métodos
        private string ResponderExcluirRegraGrupoItem()
        {
            string lRetorno = string.Empty;

            try
            {
                RemoverRegraGrupoItemRequest lRequest = new RemoverRegraGrupoItemRequest();

                lRequest.Objeto = new RegraGrupoItemInfo();

                lRequest.Objeto.CodigoCliente = this.GetIdCliente.Value;
                lRequest.Objeto.CodigoGrupo   = this.GetIdGrupo.Value;
                lRequest.Objeto.CodigoAcao    = this.GetIdAcao.Value;
                lRequest.Objeto.Sentido       = this.GetSentido;

                var lResponse = base.ServicoRegrasRisco.RemoverRegraGrupoItemSpider(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lResponse, "Regra de grupo excluída com sucesso");

                    string lLog = "Regra de grupo excluída com sucesso" + lRetorno;

                    base.RegistrarLogConsulta(lLog);

                    ILimiteManager lmtMng = Ativador.Get<ILimiteManager>();
                    ReloadClientLimitResponse resp = new ReloadClientLimitResponse();
                    ReloadClientLimitRequest req = new ReloadClientLimitRequest();
                    req.CodCliente = 0;
                    req.DeleteOnly = false;
                    lmtMng.ReloadLimitClientLimitStructures(req);
                }
                else
                {
                    lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
            }

            return lRetorno;
        }

        private string ResponderExcluirClienteBloqueioInstrumento()
        {
            string lRetorno = string.Empty;

            try
            {
                RemoverClienteBloqueioRequest lRequest = new RemoverClienteBloqueioRequest();

                lRequest.ClienteBloqueioRegra = new ClienteBloqueioRegraInfo();

                lRequest.ClienteBloqueioRegra.Ativo         = this.GetAtivo;
                lRequest.ClienteBloqueioRegra.Direcao       = this.GetSentido;
                lRequest.ClienteBloqueioRegra.CodigoCliente = this.GetIdCliente.Value;

                var lResponse = base.ServicoRegrasRisco.RemoverBloqueioClienteInstrumentoDirecaoSpider(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lResponse, "Regra de grupo excluída com sucesso");

                    string lLog = "Regra de grupo excluída com sucesso" + lRetorno;

                    base.RegistrarLogConsulta(lLog);

                    ILimiteManager lmtMng = Ativador.Get<ILimiteManager>();
                    ReloadClientLimitResponse resp = new ReloadClientLimitResponse();
                    ReloadClientLimitRequest req = new ReloadClientLimitRequest();
                    req.CodCliente = 0;
                    req.DeleteOnly = false;
                    lmtMng.ReloadLimitClientLimitStructures(req);
                }
                else
                {
                    lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
            }

            return lRetorno;
        }

        private string ResponderExcluirRegraGrupoItemGlobal()
        {
            string lRetorno = string.Empty;

            try
            {
                RemoverRegraGrupoItemRequest lRequest = new RemoverRegraGrupoItemRequest();

                lRequest.Objeto = new RegraGrupoItemInfo();

                lRequest.Objeto.CodigoGrupo = this.GetIdGrupo.Value;
                lRequest.Objeto.CodigoAcao  = this.GetIdAcao.Value;
                lRequest.Objeto.Sentido     = this.GetSentido;

                var lResponse = base.ServicoRegrasRisco.RemoverRegraGrupoItemGlobalSpider(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lResponse, "Regra de grupo (GLOBAL) excluída com sucesso");

                    string lLog = "Regra de grupo (GLOBAL) excluída com sucesso" + lRetorno;

                    base.RegistrarLogConsulta(lLog);

                    ILimiteManager lmtMng          = Ativador.Get<ILimiteManager>();
                    ReloadClientLimitResponse resp = new ReloadClientLimitResponse();
                    ReloadClientLimitRequest req   = new ReloadClientLimitRequest();
                    req.CodCliente                 = 0;
                    req.DeleteOnly                 = false;

                    ReloadClientLimitResponse lResponseLimit =  lmtMng.ReloadLimitClientLimitStructures(req);

                    
                }
                else
                {
                    lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
            }

            return lRetorno;
        }

        private string ResponderCarregarListaRegraGrupoItemGlobal()
        {
            var lRetorno = string.Empty;

            try
            {
                ListarRegraGrupoItemRequest lRequest = new ListarRegraGrupoItemRequest();

                lRequest.Objeto = new RegraGrupoItemInfo();

                lRequest.Objeto.CodigoGrupo = this.GetIdGrupo.Value;

                var lResponse = base.ServicoRegrasRisco.ListarRegraGrupoItemGlobalSpider(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lResponse, "Lista retornada com sucesso");

                    string lLog = "Lista (GLOBAL) retornada com sucesso";

                    base.RegistrarLogConsulta(lLog);
                }
                else
                {
                    lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
            }

            return lRetorno;
        }

        private string ResponderCarregarListaClienteBloqueioInstrumento()
        {
            var lRetorno = string.Empty;

            try
            {
                ListarBloqueiroInstrumentoRequest lRequest = new ListarBloqueiroInstrumentoRequest();

                var lReponse = base.ServicoRegrasRisco.ListarBloqueioClienteInstrumentoDirecaoSpider(lRequest);

                if (lReponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lReponse, "Lista retornada com sucesso");

                    string lLog = "Lista de clientes bloqueados retornada com sucesso";

                    base.RegistrarLogConsulta(lLog);
                }
                else
                {
                    lRetorno = RetornarErroAjax(lReponse.DescricaoResposta);
                }

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
            }

            return lRetorno;
        }

        private string ResponderCarregarListaRegraGrupoItem()
        {
            var lRetorno = string.Empty;

            try
            {
                ListarRegraGrupoItemRequest lRequest = new ListarRegraGrupoItemRequest();

                lRequest.Objeto = new RegraGrupoItemInfo();

                lRequest.Objeto.CodigoGrupo = this.GetIdGrupo.Value;

                var lReponse = base.ServicoRegrasRisco.ListarRegraGrupoItemSpider(lRequest);

                if (lReponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lReponse, "Lista retornada com sucesso");

                    string lLog = "Lista de clientes bloqueados retornada com sucesso";

                    base.RegistrarLogConsulta(lLog);
                }
                else
                {
                    lRetorno = RetornarErroAjax(lReponse.DescricaoResposta);
                }

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
            }

            return lRetorno;
        }

        private string ResponderSalvarClienteBloqueioInstrumentoDirecao()
        {
            var lRetorno = string.Empty;

            try
            {
                SalvarBloqueioInstrumentoRequest lRequestBloqueioCliente = new SalvarBloqueioInstrumentoRequest();

                lRequestBloqueioCliente.Objeto = new BloqueioInstrumentoInfo();

                lRequestBloqueioCliente.Objeto.CdAtivo   = this.GetAtivo.ToUpper();
                lRequestBloqueioCliente.Objeto.Direcao   = this.GetSentido;
                lRequestBloqueioCliente.Objeto.IdCliente = this.GetIdCliente.Value;

                var lRetornoInclusao = base.ServicoRegrasRisco.SalvarClienteBloqueioInstrumentoDirecaoSpider(lRequestBloqueioCliente);

                if (lRetornoInclusao.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lRetornoInclusao, "Cliente Bloqueio Instrumento Direção Item incluídos com sucesso");

                    string lLog = "Inclusão dos dados de Cliente Bloqueio Instrumento Direção : " + lRetornoInclusao;

                    base.RegistrarLogInclusao(lLog);

                    ILimiteManager lmtMng          = Ativador.Get<ILimiteManager>();
                    ReloadClientLimitResponse resp = new ReloadClientLimitResponse();
                    ReloadClientLimitRequest req   = new ReloadClientLimitRequest();
                    req.CodCliente                 = this.GetIdCliente.Value;
                    req.DeleteOnly                 = false;
                    lmtMng.ReloadLimitClientLimitStructures(req);

                }
                else
                {
                    lRetorno = RetornarErroAjax(lRetornoInclusao.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
            }

            return lRetorno;
        }

        private string ResponderSalvarRegraGrupoItemGlobal()
        {
            var lRetorno = string.Empty;

            
            try
            {
                SalvarRegraGrupoItemRequest lRequest = new SalvarRegraGrupoItemRequest();

                lRequest.RegraGrupoItem               = new RegraGrupoItemInfo();
                lRequest.RegraGrupoItem.CodigoAcao    = 2;
                lRequest.RegraGrupoItem.CodigoCliente = this.GetIdCliente.Value;
                lRequest.RegraGrupoItem.CodigoGrupo   = this.GetIdGrupo.Value;
                lRequest.RegraGrupoItem.CodigoUsuario = base.UsuarioLogado.Id;
                lRequest.RegraGrupoItem.Sentido       = this.GetSentido;

                var lRetornoInclusao = base.ServicoRegrasRisco.SalvarRegraGrupoItemGlobalSpider(lRequest);

                if (lRetornoInclusao.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    //lRetorno = RetornarSucessoAjax(lRetornoInclusao, "Grupo Item (GLOBAL) incluídos com sucesso");

                    ListarRegraGrupoItemRequest lRequestGrupo = new ListarRegraGrupoItemRequest();

                    lRequestGrupo.Objeto = new RegraGrupoItemInfo();

                    lRequestGrupo.Objeto.CodigoGrupo = this.GetIdGrupo.Value;

                    var lResponseGrupo = base.ServicoRegrasRisco.ListarRegraGrupoItemGlobalSpider(lRequestGrupo);

                    if (lResponseGrupo.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lResponseGrupo.Resultado.Sort((p1, p2) => Comparer<int>.Default.Compare(p2.CodigoGrupoRegra, p1.CodigoGrupoRegra));

                        lRetornoInclusao.RegraGrupoItem = lResponseGrupo.Resultado[0];

                        lRetorno = RetornarSucessoAjax(lRetornoInclusao, "Grupo Item (GLOBAL) incluídos com sucesso");
                    }

                    string lLog = "Grupo Item (GLOBAL) incluídos com sucesso : " + lRetorno ;

                    base.RegistrarLogInclusao(lLog);

                    ILimiteManager lmtMng = Ativador.Get<ILimiteManager>();
                    ReloadClientLimitResponse resp = new ReloadClientLimitResponse();
                    ReloadClientLimitRequest req = new ReloadClientLimitRequest();
                    req.CodCliente = 0;
                    req.DeleteOnly = false;
                    lmtMng.ReloadLimitClientLimitStructures(req);

                }
                else
                {
                    lRetorno = RetornarErroAjax(lRetornoInclusao.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
            }
            

            return lRetorno;
        }

        private string ResponderSalvarRegraGrupoItem()
        {
            var lRetorno = string.Empty;

            try
            {
                SalvarRegraGrupoItemRequest lRequest = new SalvarRegraGrupoItemRequest();

                lRequest.RegraGrupoItem               = new RegraGrupoItemInfo();
                lRequest.RegraGrupoItem.CodigoAcao    = 1;
                lRequest.RegraGrupoItem.CodigoCliente = this.GetIdCliente.Value;
                lRequest.RegraGrupoItem.CodigoGrupo   = this.GetIdGrupo.Value;
                lRequest.RegraGrupoItem.CodigoUsuario = base.UsuarioLogado.Id;
                lRequest.RegraGrupoItem.Sentido       = this.GetSentido;

                var lRetornoInclusao = base.ServicoRegrasRisco.SalvarRegraGrupoItemSpider(lRequest);

                if (lRetornoInclusao.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lRetornoInclusao, "Grupos Item incluídos com sucesso");

                    string lLog = "Inclusão dos dados de Grupo de ";

                    base.RegistrarLogInclusao(lLog);

                    ILimiteManager lmtMng          = Ativador.Get<ILimiteManager>();
                    ReloadClientLimitResponse resp = new ReloadClientLimitResponse();
                    ReloadClientLimitRequest req   = new ReloadClientLimitRequest();
                    req.CodCliente                 = this.GetIdCliente.Value;
                    req.DeleteOnly                 = false;
                    lmtMng.ReloadLimitClientLimitStructures(req);

                }
                else
                {
                    lRetorno = RetornarErroAjax(lRetornoInclusao.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
            }

            return lRetorno;
        }


        public bool PermissaoExcluir()
        {
            return _PermissaoExcluir;
        }

        private string ResponderCarregarHtmlComDados()
        {
            List<ItemSegurancaInfo> list           = new List<ItemSegurancaInfo>();
            ItemSegurancaInfo lItemSegurancaSalvar = new ItemSegurancaInfo();
            lItemSegurancaSalvar.Permissoes        = new List<string>() { "54f77b3b-ac85-42be-b5d9-92a4fa03b056" };
            lItemSegurancaSalvar.Tag               = "Salvar";
            lItemSegurancaSalvar.TipoAtivacao      = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;
            list.Add(lItemSegurancaSalvar);

            base.VerificaPermissoesPagina(list).ForEach(delegate(ItemSegurancaInfo item)
            {
                if ("Salvar".Equals(item.Tag))
                {
                    //btnClientes_Limites_Bovespa.Visible = item.Valido.Value;
                    //btnCliente_Restricoes.Visible = item.Valido.Value;
                    //btnGradIntra_Clientes_Restricoes_RestricaoPorAtivos_Add.Visible           = item.Valido.Value;
                    //btnGradIntra_Clientes_Restricoes_RestricaoPorAtivos_Remover_click.Visible = item.Valido.Value;
                    _PermissaoExcluir                                                         = item.Valido.Value;
                }
            });
            var lListaGrupoItem = new List<TransporteRiscoGrupo>();
            var lResponseGrupos = base.ServicoRegrasRisco.ListarGruposSpider(
                new ListarGruposRequest()
                {
                    FiltroTipoGrupo = EnumRiscoRegra.TipoGrupo.GrupoDeRisco
                });

            if (null != lResponseGrupos && null != lResponseGrupos.Grupos && lResponseGrupos.Grupos.Count > 0)
                lResponseGrupos.Grupos.ForEach(lGrupoInfo =>
                {
                    lListaGrupoItem.Add(new TransporteRiscoGrupo(lGrupoInfo));
                });

            base.PopularComboComListaGenerica<TransporteRiscoGrupo>(lListaGrupoItem, this.rptGradIntra_ComboBox_Digitavel);
            //this.CarregarDadosViaServico();

            //this.ConfigurarLimitesNaTela(new TransporteConfigurarLimites().TraduzirListaLimites(this.gRetornoLimitePorCliente.ParametrosRiscoCliente));

            //this.ConfigurarLimitesBloqueadosNaTela(this.gDetalhesDoLimite);

            //this.ConfigurarPermissoesNaTela(new TransporteConfigurarLimites().TraduzirListaPermissoes(this.gRetornoParametrosRisco.Permissoes));

            //this.ConfigurarRestricoesNaTela();

            this.Response.Clear();

            return string.Empty; // só para obedecer assinatura
        }

        private void SalvarRestricoesPorAtivo(TransporteBloqueioInstrumento pParametro)
        {
            if (null != pParametro)
            {
                DbTransaction lDbTransaction;
                {   //--> Criando a transação.
                    var lConexao = new Conexao();
                    lConexao._ConnectionStringName = "GradualSpider";
                    var lDbConnection = lConexao.CreateIConnection();
                    lDbConnection.Open();
                    lDbTransaction = lDbConnection.BeginTransaction();
                }

                var lRetornoSalvamentoBloqueio = new SalvarBloqueioInstrumentoResponse();

                int lIdCliente = this.Request.Form["Id"].DBToInt32();
                var lRetornoExclusaoBloqueio = new ServicoRegrasRisco().RemoverBloqueioPorCliente(lDbTransaction, new RemoverBloqueioInstrumentoRequest()
                {
                    Objeto = new BloqueioInstrumentoInfo()
                    {
                        IdCliente = lIdCliente
                    }
                });

                if (!MensagemResponseStatusEnum.OK.Equals(lRetornoExclusaoBloqueio.StatusResposta))
                {
                    lDbTransaction.Rollback();
                    base.RetornarErroAjax("Houve um erro durante o salvamento dos dados. Tente novamente por favor.");
                    return;
                }

                for (int i = 0; i < pParametro.Ativos.Length; i++)
                {
                    lRetornoSalvamentoBloqueio = new ServicoRegrasRisco().SalvarBloqueioInstrumento(
                        lDbTransaction,
                        new SalvarBloqueioInstrumentoRequest()
                        {
                            Objeto = new BloqueioInstrumentoInfo()
                            {
                                CdAtivo = pParametro.Ativos[i],
                                Direcao = pParametro.Direcoes[i].Substring(0, 1),
                                IdCliente = lIdCliente
                            }
                        });

                    if (!MensagemResponseStatusEnum.OK.Equals(lRetornoSalvamentoBloqueio.StatusResposta))
                    {
                        lDbTransaction.Rollback();
                        base.RetornarErroAjax("Houve um erro durante o salvamento dos dados. Tente novamente por favor.");
                        return;
                    }
                }

                if (MensagemResponseStatusEnum.OK.Equals(lRetornoSalvamentoBloqueio.StatusResposta))
                    lDbTransaction.Commit();
            }
        }
        #endregion
    }
}