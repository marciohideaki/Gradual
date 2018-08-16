using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Gradual.OMS.Library;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class DadosGrupoRestricaoSpider : PaginaBaseAutenticada
    {

        #region | Propriedades

        private List<GrupoInfo> SessionUltimoResultadoDeBusca
        {
            get
            {
                if (null != this.Session["UltimoResultadoDeBuscaDeGruposDeRiscoRestricoes"] && this.Session["UltimoResultadoDeBuscaDeGruposDeRiscoRestricoes"] is List<GrupoInfo>)
                    return (List<GrupoInfo>)this.Session["UltimoResultadoDeBuscaDeGruposDeRiscoRestricoes"];

                return new List<GrupoInfo>();
            }
            set
            {
                this.Session["UltimoResultadoDeBuscaDeGruposDeRiscoRestricoes"] = value;
            }
        }

        private bool GetExcluiComFilhos
        {
            get
            {
                var lRetorno = default(bool);

                bool.TryParse(this.Request.Form["ExcluiComfilhos"], out lRetorno);

                return lRetorno;
            }
        }

        #endregion

        #region | Métodos
        private string ResponderBuscarItensParaListagemSimples()
        {
            string lRetorno = "Erro...";

            var lResponse = base.ServicoRegrasRisco.ListarGruposSpider(new ListarGruposRequest() { FiltroTipoGrupo = EnumRiscoRegra.TipoGrupo.GrupoDeRisco });

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.SessionUltimoResultadoDeBusca = lResponse.Grupos;

                TransporteDeListaPaginada lListaPaginada = this.BuscarPaginaDeResultados(1);

                lRetorno = RetornarSucessoAjax(lListaPaginada, "Encontrados [{0}] clientes", lResponse.Grupos.Count);
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao tentar carregar os grupos", lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        private string ResponderPaginar()
        {
            string lRetorno = string.Empty;

            var lLista = new TransporteDeListaPaginada();

            if (this.SessionUltimoResultadoDeBusca != null)
            {
                int lPagina;

                if (int.TryParse(Request["page"], out lPagina))
                {
                    lLista = BuscarPaginaDeResultados(lPagina);
                }
            }

            lRetorno = JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }

        private string ResponderExcluir()
        {
            string lretorno = string.Empty;
            string lId = this.Request.Form["Id"];

            if (!this.GetExcluiComFilhos)
            {
                var lGrupoSelecionado = base.ServicoRegrasRisco.ListarGrupoItensSpider(new ListarGrupoItemRequest() { FiltroIdGrupo = lId.DBToInt32() });

                if (null != lGrupoSelecionado.GrupoItens && lGrupoSelecionado.GrupoItens.Count > 0)
                    return base.RetornarSucessoAjax((object)lId, "ComFilhos");
            }

            RemoverGrupoRiscoRequest lRequest = new RemoverGrupoRiscoRequest() { CodigoGrupo = int.Parse(lId), DescricaoUsuarioLogado = base.UsuarioLogado.Nome, IdUsuarioLogado = base.UsuarioLogado.Id };
            RemoverGrupoRiscoResponse lResponse = null;
            try
            {
                lResponse = this.ServicoRegrasRisco.RemoverGrupoRiscoSpider(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.BusinessException)
                    {
                        lretorno = RetornarErroAjax(lResponse.MessageException);
                    }
                    else
                    {
                        {   //--> Removendo o item excluído da sessão
                            var lItemASerExcluido = this.SessionUltimoResultadoDeBusca.Find(lGrupo => { return lGrupo.CodigoGrupo == lId.DBToInt32(); });
                            this.SessionUltimoResultadoDeBusca.Remove(lItemASerExcluido);
                        }

                        lretorno = base.RetornarSucessoAjax((object)lId, "Grupo Removido com sucesso");
                        base.RegistrarLogExclusao();
                    }
                }
                else
                {
                    lretorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lretorno = RetornarErroAjax(ex.Message);
            }
            return lretorno;
        }

        private string ResponderAtualizar()
        {
            string lRetorno = "";
            string lObjetoJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteRiscoGrupo lDadosGrupo;

                SalvarGrupoRequest lRequest;

                SalvarGrupoResponse lResponse;
                try
                {
                    lDadosGrupo               = JsonConvert.DeserializeObject<TransporteRiscoGrupo>(lObjetoJson);
                    lRequest                  = new SalvarGrupoRequest();
                    lRequest.Grupo            = lDadosGrupo.ToGrupoInfo();
                    lRequest.Grupo.GrupoItens = new List<GrupoItemInfo>();
                    lRequest.Grupo.TipoGrupo  = EnumRiscoRegra.TipoGrupo.GrupoDeRisco;

                    try
                    {
                        lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                        lRequest.IdUsuarioLogado        = base.UsuarioLogado.Id;

                        lResponse = this.ServicoRegrasRisco.SalvarGrupoSpider(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            this.SessionUltimoResultadoDeBusca.Add(new GrupoInfo()
                            {   //--> Incluindo item na sessão
                                NomeDoGrupo = lRequest.Grupo.NomeDoGrupo,
                                CodigoGrupo = lResponse.Grupo.CodigoGrupo,
                                TipoGrupo   = EnumRiscoRegra.TipoGrupo.GrupoAlavancagem,
                            });

                            var lMensagem = string.Format("Grupo {0} foi incluído com sucesso", lRequest.Grupo.NomeDoGrupo);

                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.Grupo.CodigoGrupo), lMensagem);

                            base.RegistrarLogInclusao(lMensagem);
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                        }
                    }
                    catch (Exception exEnvioRequest)
                    {
                        lRetorno = RetornarErroAjax("Erro durante o envio do request para alterar o GrupoInfo", exEnvioRequest);
                    }
                }
                catch (Exception exDeserializacaoCliente)
                {
                    lRetorno = RetornarErroAjax("Erro durante a deserialização dos dados do grupo", exDeserializacaoCliente);
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para alterar");
            }
            return lRetorno;
        }

        private string ResponderCadastrar()
        {
            return this.ResponderAtualizar();
        }

        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina)
        {
            var lRetorno = new TransporteDeListaPaginada();

            var lLista = new List<GrupoInfo>();

            int lIndiceInicial, lIndiceFinal;

            TransporteDeListaPaginada.ItensPorPagina = 10;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPagina);
            lIndiceFinal = (pPagina) * TransporteDeListaPaginada.ItensPorPagina;

            this.SessionUltimoResultadoDeBusca.Sort((lGru1, lGru2) => { return lGru1.NomeDoGrupo.CompareTo(lGru2.NomeDoGrupo); });

            for (int i = lIndiceInicial; i < lIndiceFinal; i++) if (i < this.SessionUltimoResultadoDeBusca.Count)
                    lLista.Add(this.SessionUltimoResultadoDeBusca[i]);

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double)TransporteDeListaPaginada.ItensPorPagina));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }
        #endregion

        #region | Eventos
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (base.Acao.Equals("CarregarHtmlComDados"))
                this.ResponderBuscarItensParaListagemSimples();
            else
            {
                base.RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                         , "CarregarHtmlComDados"
                                                         , "Paginar"
                                                         , "Cadastrar"
                                                         , "Atualizar"
                                                         , "Excluir"
                                                         },
                             new ResponderAcaoAjaxDelegate[] 
                                                         { this.ResponderBuscarItensParaListagemSimples
                                                         , this.ResponderBuscarItensParaListagemSimples
                                                         , this.ResponderPaginar
                                                         , this.ResponderCadastrar
                                                         , this.ResponderAtualizar
                                                         , this.ResponderExcluir});
            }
        }
        #endregion


    }
}