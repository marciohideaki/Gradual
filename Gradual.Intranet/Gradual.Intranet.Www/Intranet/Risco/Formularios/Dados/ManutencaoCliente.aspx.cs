using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class ManutencaoCliente : PaginaBaseAutenticada
    {
        #region | Propriedades

        private int? GetIdGrupo
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["IdGrupo"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCdCliente
        {
            get
            {

                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CdCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCdAssessor
        {
            get
            {

                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CdAssessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private DateTime GetDataInicial
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DtInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataFinal
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DtFinal"], out lRetorno);

                return lRetorno;
            }
        }

        private List<TransporteManutencaoCliente> SessionUltimoResultadoDeBusca
        {
            get
            {
                if (null != this.Session["UltimoResultadoDeBuscaDeManutencaoCliente"] && this.Session["UltimoResultadoDeBuscaDeManutencaoCliente"] is List<TransporteManutencaoCliente>)
                    return (List<TransporteManutencaoCliente>)this.Session["UltimoResultadoDeBuscaDeManutencaoCliente"];

                return new List<TransporteManutencaoCliente>();
            }
            set
            {
                this.Session["UltimoResultadoDeBuscaDeManutencaoCliente"] = value;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if ("CarregarHtml".Equals(base.Acao))
                this.ResponderCarregarHtml();

            base.RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                     , "Paginar"
                                                     , "Excluir"
                                                     , "CarregarHtmlComDados"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderPaginar
                                                     , this.ResponderExcluir
                                                     , this.ResponderCarregarHtml});
        }

        #endregion

        #region | Métodos

        public string ResponderPaginar()
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

        private string ResponderCarregarHtml()
        {
            this.SessionUltimoResultadoDeBusca = null;

            base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptGradIntra_Risco_ManutencaoCliente_Assessor);

            var lGrupos = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoGrupoInfo>(
                new ConsultarEntidadeCadastroRequest<RiscoGrupoInfo>()
                {
                    EntidadeCadastro = new RiscoGrupoInfo
                    {
                        TipoGrupo = EnumRiscoRegra.TipoGrupo.GrupoAlavancagem
                    }
                });

            if (lGrupos.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.rptGradIntra_Risco_ManutencaoCliente_Grupo.DataSource = lGrupos.Resultado;
                this.rptGradIntra_Risco_ManutencaoCliente_Grupo.DataBind();

                return base.RetornarSucessoAjax("");
            }
            else
            {
                return base.RetornarErroAjax(lGrupos.DescricaoResposta);
            }
        }

        private string ResponderExcluir()
        {
            var lRetorno = string.Empty;

            var lResponse = base.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ParametroAlavancagemConsultaInfo>(
                new RemoverEntidadeCadastroRequest<ParametroAlavancagemConsultaInfo>()
                {
                    EntidadeCadastro = new ParametroAlavancagemConsultaInfo() { ConsultaCdCliente = this.GetCdCliente }
                });

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lRetorno = base.RetornarSucessoAjax(this.GetCdCliente, "Item com sucesso");
            }
            else
            {
                lRetorno = base.RetornarErroAjax(lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        private string ResponderBuscarItensParaListagemSimples()
        {
            string lRetorno = "Erro...";

            var lResponse = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ParametroAlavancagemConsultaInfo>(
                new ConsultarEntidadeCadastroRequest<ParametroAlavancagemConsultaInfo>()
                {
                    EntidadeCadastro = new ParametroAlavancagemConsultaInfo()
                    {
                        ConsultaCdAssessor = this.GetCdAssessor,
                        ConsultaCdCliente = this.GetCdCliente,
                        ConsultaIdGrupo = this.GetIdGrupo,
                    }
                });

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.SessionUltimoResultadoDeBusca = new TransporteManutencaoCliente().TraduzirLista(lResponse.Resultado);

                TransporteDeListaPaginada lListaPaginada = this.BuscarPaginaDeResultados(1);

                lRetorno = base.RetornarSucessoAjax(lListaPaginada, "Encontrados [{0}] clientes", lResponse.Resultado.Count);
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao tentar carregar os grupos", lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina)
        {
            var lRetorno = new TransporteDeListaPaginada();

            var lLista = new List<TransporteManutencaoCliente>();

            int lIndiceInicial, lIndiceFinal;

            TransporteDeListaPaginada.ItensPorPagina = 30;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPagina);
            lIndiceFinal = (pPagina) * TransporteDeListaPaginada.ItensPorPagina;

            //--> Ordenando a consulta por cliente.
            this.SessionUltimoResultadoDeBusca.Sort((lGru1, lGru2) => { return lGru1.Cliente.DBToInt32().CompareTo(lGru2.Cliente.DBToInt32()); });

            for (int i = lIndiceInicial; i < lIndiceFinal; i++)
            {
                if (i < this.SessionUltimoResultadoDeBusca.Count)
                {
                    lLista.Add(this.SessionUltimoResultadoDeBusca[i]);
                }
            }

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double)TransporteDeListaPaginada.ItensPorPagina));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }

        #endregion
    }
}