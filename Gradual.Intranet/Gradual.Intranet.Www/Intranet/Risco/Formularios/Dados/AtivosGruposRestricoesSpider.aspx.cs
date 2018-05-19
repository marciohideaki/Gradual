using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Newtonsoft.Json;
using Gradual.Core.OMS.LimiteManager.Lib;
using Gradual.Core.OMS.LimiteManager.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class AtivosGruposRetricoesSpider : PaginaBaseAutenticada
    {
        #region | Propriedades

        private int? GetIdGrupo
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["IdGrupo"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetIdGrupoItem
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["IdItemGrupo"], out lRetorno);

                return lRetorno;

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

        #endregion

        #region | Métodos

        private string ResponderCarregarHtmlComDados()
        {
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

            return string.Empty;    //só para obedecer assinatura
        }

        private string ResponderCarregarListaGrupoItem()
        {
            var lRetorno = string.Empty;
            var lResponseGrupoItem = base.ServicoRegrasRisco.ListarGrupoItensSpider(
                new ListarGrupoItemRequest()
                {
                    FiltroIdGrupo = this.GetIdGrupo
                });

            if (lResponseGrupoItem.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lListaGrupoItem = new List<TransporteRiscoGrupoItem>();

                lResponseGrupoItem.GrupoItens.ForEach(lGrupoItem => { lListaGrupoItem.Add(new TransporteRiscoGrupoItem(lGrupoItem)); });

                var lObjetoJSonRetorno = JsonConvert.SerializeObject(lListaGrupoItem);

                lRetorno = base.RetornarSucessoAjax(lListaGrupoItem, string.Format("{0} registros encontrados", lResponseGrupoItem.GrupoItens.Count.ToString()));
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requsição", lResponseGrupoItem.DescricaoResposta);
            }

            return lRetorno;
        }

        private string ResponderSalvarGrupoItem()
        {
            var lRetorno = string.Empty;

            if (this.GetListaItemGrupoNovos.Count > 0)
            {
                try
                {
                    var lRetornoInclusao = base.ServicoRegrasRisco.SalvarGrupoItemSpider(
                        new SalvarGrupoItemRequest()
                        {
                            GrupoItemLista = this.MontarListaGrupoItem(),
                        });

                    if (lRetornoInclusao.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = RetornarSucessoAjax(lRetornoInclusao, "Grupos Item incluídos com sucesso");
                        base.RegistrarLogInclusao();
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax(lRetornoInclusao.DescricaoResposta);
                    }

                    ILimiteManager lmtMng = Ativador.Get<ILimiteManager>();
                    ReloadClientLimitResponse resp = new ReloadClientLimitResponse();
                    ReloadClientLimitRequest req = new ReloadClientLimitRequest();
                    req.CodCliente = 0;
                    req.DeleteOnly = false;
                    lmtMng.ReloadLimitClientLimitStructures(req);

                }
                catch (Exception ex)
                {
                    lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para alterar");
            }

            return lRetorno;
        }

        private string ResponderExcluirGrupoItem()
        {
            var lRetorno = string.Empty;

            if (GetIdGrupoItem > 0)
            {
                RemoverGrupoItemRequest lRequest;

                RemoverGrupoItemResponse lResponse;

                try
                {
                    lRequest = new RemoverGrupoItemRequest()
                    {
                        CodigoGrupoItem = GetIdGrupoItem
                    };

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
                    lResponse = this.ServicoRegrasRisco.RemoverGrupoItemSpider(lRequest);

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = RetornarSucessoAjax(GetIdGrupoItem, "Item excluído com sucesso");

                        ILimiteManager lmtMng          = Ativador.Get<ILimiteManager>();
                        ReloadClientLimitResponse resp = new ReloadClientLimitResponse();
                        ReloadClientLimitRequest req   = new ReloadClientLimitRequest();
                        req.CodCliente                 = 0;
                        req.DeleteOnly                 = false;
                        lmtMng.ReloadLimitClientLimitStructures(req);

                        base.RegistrarLogExclusao();
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax("Erro ao excluir o item");
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

        private List<GrupoItemInfo> MontarListaGrupoItem()
        {
            var lRetorno = new List<GrupoItemInfo>();

            if (null != GetListaItemGrupoNovos && GetListaItemGrupoNovos.Count > 0)
            {
                GetListaItemGrupoNovos.ForEach(lItemGrupo =>
                {
                    lRetorno.Add(new GrupoItemInfo()
                    {
                        CodigoGrupo   = this.GetIdGrupo,
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

            RegistrarRespostasAjax(new string[] { "SalvarGrupoItem"
                                                , "ExcluirGrupoItem"
                                                , "CarregarListaGrupoItem"
                                                , "CarregarHtmlComDados"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderSalvarGrupoItem
                                                , ResponderExcluirGrupoItem
                                                , ResponderCarregarListaGrupoItem 
                                                , ResponderCarregarListaGrupoItem 
                                                });
        }
        #endregion
    }
}