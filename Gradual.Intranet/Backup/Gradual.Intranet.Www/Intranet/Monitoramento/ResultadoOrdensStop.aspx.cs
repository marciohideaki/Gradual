using System;
using System.Collections.Generic;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Info;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Ordens.StartStop.Lib;
using Gradual.OMS.Ordens.StartStop.Lib.Enum;
using Newtonsoft.Json;
using System.Linq;
namespace Gradual.Intranet.Www.Intranet.Monitoramento
{
    public partial class ResultadoOrdensStop : PaginaBaseAutenticada
    {
        #region | Propriedades

        private List<OrdemStopStartInfo> SessionUltimoResultadoDeBusca
        {
            get
            {
                return (List<OrdemStopStartInfo>)Session["UltimoResultadoDeBuscaDeOrdemStop"];
            }
            set
            {
                Session["UltimoResultadoDeBuscaDeOrdemStop"] = value;
            }
        }

        private DateTime? GetData
        {
            get
            {
                DateTime? lRetorno = null;

                DateTime lDtTemp = new DateTime();

                if (string.IsNullOrWhiteSpace(this.Request.Form["Data"]))
                    return null;
                else
                {
                    DateTime.TryParse(this.Request.Form["Data"], out lDtTemp);
                    lRetorno = lDtTemp;
                }
                return lRetorno;
            }
        }

        private string GetPapel
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.Form["Papel"]))
                    return null;

                return this.Request.Form["Papel"];
            }
        }

        private int? GetStatusOrdem
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.Form["Status"]))
                    return null;

                return Convert.ToInt32(this.Request.Form["Status"]);
            }
        }

        private int? GetCodBovespa
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.Form["TermoDeBusca"]))
                    return null;

                return Convert.ToInt32(this.Request.Form["TermoDeBusca"]);
            }
        }

        private int? GetIdSistemaOrigem
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["sistema"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        //private string GetTermoBusca
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(this.Request.Form["TermoDeBusca"]))
        //            return null;

        //        return this.Request.Form["TermoDeBusca"].ToUpper();
        //    }
        //}

        //private CancelarOrdemStartStopInfo GetListaIdsSelecionados
        //{
        //    get
        //    {
        //        var lRetorno = new CancelarOrdemStartStopInfo();
        //        var lArrayIds = (null != this.Request.Form["Ids"]) ? this.Request.Form["Ids"].Split(',') : new string[0];

        //        lRetorno.ListaCancelarStartStopOrdensRequest = new List<CancelarStartStopOrdensRequest>();

        //        for (int i = 0; i < lArrayIds.Length; i++)
        //            if (!string.IsNullOrWhiteSpace(lArrayIds.GetValue(i).DBToString()))
        //            {
        //                lRetorno.ListaCancelarStartStopOrdensRequest.Add(
        //                    new CancelarStartStopOrdensRequest()
        //                    {
        //                        IdStopStart = lArrayIds.GetValue(i).DBToString(),
        //                    });
        //            }

        //        return lRetorno;
        //    }
        //}

        #endregion

        #region | Métodos

        /// <summary>
        /// Busca as ordens do tipo Stopstart para listagem no grid
        /// </summary>
        /// <returns>Preenche o repeater dentro do grid para visualização</returns>
        private string ResponderBuscarItensParaListagemSimples()
        {
            BuscarOrdensStopStartRequest lRequest = new BuscarOrdensStopStartRequest()
            {
                Account = GetCodBovespa,
                Symbol = GetPapel,
                OrderStatusId = GetStatusOrdem,
                DataDe = GetData,
                CodigoAssessor = base.CodigoAssessor,
                IdSistema = this.GetIdSistemaOrigem,
            };

            string lRetorno = string.Empty;

            try
            {
                TransporteDeListaPaginada lLista = new TransporteDeListaPaginada();

                MonitoramentoOrdemStopDbLib lServico = new MonitoramentoOrdemStopDbLib();

                BuscarOrdensStopStartResponse lResponseStartStop = lServico.BuscarOrdensStopStart(lRequest);

                this.SessionUltimoResultadoDeBusca = lResponseStartStop.OrdensStartStop;

                lLista = BuscarPaginaDeResultados(1);

                if (lLista.TotalDeItens > 0)
                {
                    lRetorno = RetornarSucessoAjax(lLista, string.Format("Foram encontrados {0} registros", lLista.TotalDeItens.ToString())); //O grid espera o objeto direto, sem estar encapsulado
                    base.RegistrarLogConsulta();
                }
                else
                    lRetorno = RetornarSucessoAjax(lLista, "Nenhum registro encontrado!");

            }
            catch (Exception exBusca)
            {
                base.RetornarErroAjax("Erro ao buscar dados de ordem", exBusca);
            }

            return lRetorno;
        }

        /// <summary>
        /// Exclui as ordens no serviço de ordens stopstart
        /// </summary>
        /// <returns></returns>
        private string ResponderExcluirOrdens()
        {
            string lRetorno = string.Empty;

            try
            {
                string lIds = Request.Form["Ids"];
                string lPapeis = Request.Form["Papeis"];

                List<string> lstIdOrdem = new List<string>();

                int lCount = 0;

                IServicoOrdemStopStart lOrdem = Ativador.Get<IServicoOrdemStopStart>();

                char[] separator = { ',' };

                string[] lIdOrdem = lIds.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                string[] lPapel = lPapeis.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                List<string> lstOrdem = new List<string>(lIdOrdem.Length);
                lstOrdem.AddRange(lIdOrdem);

                List<string> lstPapel = new List<string>(lPapel.Length);
                lstPapel.AddRange(lPapel);

                foreach (string idOrdem in lstOrdem)
                {
                    lOrdem.CancelaOrdemStopStart(new CancelarStartStopOrdensRequest()
                    {
                        IdStopStart = Convert.ToInt32(idOrdem),
                        IdStopStartStatus = (int)OrdemStopStatus.CancelamentoEnviadoMDS,
                        Instrument = lstPapel[lCount].ToString()

                    });

                    lCount++;
                }

                lRetorno = RetornarSucessoAjax("Ordens Canceladas com sucesso");

                base.RegistrarLogExclusao(string.Format("Exclusão realizada para os ID's = {0} e para os PAPÉIS = {1}", lIds, lPapeis));
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax("Erro ao tentar cancelar ordem de Stop", ex);
            }
            return lRetorno;
        }

        #endregion

        #region | Eventos

        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina)
        {
            TransporteDeListaPaginada lRetorno = new TransporteDeListaPaginada();

            List<TransporteResultadoOrdensStop> lLista = new List<TransporteResultadoOrdensStop>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPagina);
            lIndiceFinal = (pPagina) * TransporteDeListaPaginada.ItensPorPagina;

            for (int a = lIndiceInicial; a < lIndiceFinal; a++)
            {
                if (a < this.SessionUltimoResultadoDeBusca.Count)
                {
                    lLista.Add(new TransporteResultadoOrdensStop(this.SessionUltimoResultadoDeBusca[a]));
                }
            }

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double)TransporteDeListaPaginada.ItensPorPagina));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            this.btnCancelarOrdensStop.Visible = base.UsuarioPode("Cancelar", "1ff4cc57-65ce-4181-ba8f-7cf979acf5c6");
            //}
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                this.ResponderBuscarItensParaListagemSimples();
            }
            else
            {
                base.RegistrarRespostasAjax(new string[] { "ExcluirOrdens"
                                                         , "BuscarItensParaSelecao"
                                                         , "Paginar"
                                                         , "CarregarDetalhes"
                                                         },
                         new ResponderAcaoAjaxDelegate[] { this.ResponderExcluirOrdens
                                                         , this.ResponderBuscarItensParaListagemSimples
                                                         , this.ResponderPaginar
                                                         , this.ResponderCarregarDetalhes
                                                         });
            }
        }

        private string ResponderPaginar()
        {
            string lRetorno = string.Empty;

            TransporteDeListaPaginada lLista = new TransporteDeListaPaginada();

            int lPagina;

            if (int.TryParse(Request["page"], out lPagina))
            {
                lLista = BuscarPaginaDeResultados(lPagina);

            }

            lRetorno = JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }

        #endregion

        #region Responder Carregar Detalhes

        /// <summary>
        /// Carrega os detalhes
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarDetalhes()
        {
            string lRetorno = string.Empty;

            var lstAcompanhamentos = from lista in SessionUltimoResultadoDeBusca where lista.StopStartID == Request["id"].DBToInt32() select lista; 

            //List<OrdemStopStartInfoDetalhe> lstAcompanhamentos = SessionUltimoResultadoDeBusca[Request["id"].DBToInt32() - 1].Details;

            List<OrdemStopStartInfo> lOrdens = lstAcompanhamentos.ToList<OrdemStopStartInfo>();

            OrdemStopStartInfo lOrdem = lOrdens[0];

            List<TransporteOrdemStopDetalhe> lDetalhes = new List<TransporteOrdemStopDetalhe>();

            if (null != lstAcompanhamentos)
                lOrdem.Details.ForEach(lAcompanhamento =>
                    {
                        lDetalhes.Add(new TransporteOrdemStopDetalhe(lAcompanhamento));
                    });

            TransporteDeListaPaginada lLista = new TransporteDeListaPaginada(lDetalhes);

            lRetorno = JsonConvert.SerializeObject(lLista);

            return lRetorno;
        }

        #endregion
    }
}
