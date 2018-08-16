using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Monitores.Risco.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Monitores.Risco.Info;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Newtonsoft.Json;
using System.Globalization;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class MonitoramentoCustodia : PaginaBaseAutenticada
    {
        #region Propriedades
        private int lTamanhoDaParte = 50;
        private int lItensPorPagina = 50;
        private bool lNova = false;
        private bool lDescobertos = false;

        private static List<TransporteCustodia> gListCustodia;
        
        private List<TransporteCustodia> SessionUltimoResultadoDeBusca
        {
            get { return gListCustodia != null ? gListCustodia : null; }
            set { gListCustodia = value; }
        }

        private int? GetCodigoAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                var lRetorno = default(int);

                int.TryParse(this.Request.Form["Assessor"], out lRetorno);

                return lRetorno;
            }
        }

        private int? GetCodigoCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodigoCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private string GetCodigoAtivo
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request["CodigoAtivo"]))
                    lRetorno = this.Request["CodigoAtivo"];

                return lRetorno;
            }
        }

        private string GetCodigoMercado
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request["CodigoMercado"]))
                    lRetorno = this.Request["CodigoMercado"];

                return lRetorno;
            }
        }

        private IEnumerable<TransporteCustodia> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteCustodia>)Session["ListaDeResultados_MonitoramentoCustodia"];
            }

            set
            {
                Session["ListaDeResultados_MonitoramentoCustodia"] = value;
            }
        }

        #endregion

        #region Metodos
        
        protected new void Page_Load(object sender, EventArgs e)
        {
            RegistrarRespostasAjax(new string[] { 
                                                    "BuscarItensParaSelecao"
                                                  , "Paginar"
                                                },
                new ResponderAcaoAjaxDelegate[] { 
                                                    ResponderBuscarItensParaSelecao
                                                  , ResponderPaginar
                                                });
        }

        private string ResponderBuscarItensParaSelecao()
        {
            string lRetorno = string.Empty;
            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            //var lRequest = new Gradual.Intranet.Contratos.Mensagens.ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo>();// { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            //var lResponse = new Gradual.Intranet.Contratos.Mensagens.ConsultarEntidadeCadastroResponse<Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo>();
            Gradual.OMS.Monitor.Custodia.Lib.Mensageria.MonitorCustodiaRequest lRequest = new Gradual.OMS.Monitor.Custodia.Lib.Mensageria.MonitorCustodiaRequest();
            Gradual.OMS.Monitor.Custodia.Lib.Mensageria.MonitorCustodiaResponse lResponse = new Gradual.OMS.Monitor.Custodia.Lib.Mensageria.MonitorCustodiaResponse();

            try
            {
                //var lInfo = new Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo()
                //{
                //    CodigoAssessor = this.GetCodigoAssessor,
                //    CodigoCliente = this.GetCodigoCliente,
                //    CodigoAtivo= this.GetCodigoAtivo,
                //    CodigoMercado = this.GetCodigoMercado
                //};
                //lRequest.EntidadeCadastro = lInfo;
                //lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo>(lRequest);

                if (this.GetCodigoCliente != null)
                {
                    lRequest.CodigoCliente = this.GetCodigoCliente.Value;
                }

                if (this.GetCodigoAssessor.HasValue)
                {
                    lRequest.CodAssessor = this.GetCodigoAssessor;
                }

                Gradual.OMS.Monitor.Custodia.Lib.IServicoMonitorCustodia gServicoCustodia = Ativador.Get<Gradual.OMS.Monitor.Custodia.Lib.IServicoMonitorCustodia>();

                lResponse = gServicoCustodia.ObterMonitorCustodiaMemoria(lRequest);

                if (lResponse.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    if (lResponse != null && lResponse.MonitorCustodia != null )
                    {
                        if (lResponse.MonitorCustodia.ListaCustodia != null && lResponse.MonitorCustodia.ListaCustodia.Count > 0)
                        {
                            List<TransporteCustodia> lListaTransporte;
                            
                            if (lDescobertos)
                            {
                                lListaTransporte = new TransporteCustodia().TraduzirListaDescobertos(lResponse.MonitorCustodia.ListaCustodia);
                            }
                            else
                            {
                                lListaTransporte = new TransporteCustodia().TraduzirLista(lResponse.MonitorCustodia.ListaCustodia);
                            }

                            this.SessionUltimoResultadoDeBusca = lListaTransporte;

                            lRetornoLista = new TransporteDeListaPaginada(this.SessionUltimoResultadoDeBusca);

                            lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                            //lRetornoLista.TotalDeItens = lResponse.Resultado.Count;
                            lRetornoLista.TotalDeItens = lListaTransporte.Count;

                            lRetornoLista.PaginaAtual = 1;

                            lRetornoLista.TotalDePaginas = (int)Math.Ceiling((double)(lListaTransporte.Count() / lTamanhoDaParte));

                            TransporteDeListaPaginada lListaPaginada = BuscarPaginaDeResultados(1);

                            lRetorno = JsonConvert.SerializeObject(lListaPaginada);
                        }
                        else
                        {
                            //List<TransporteCustodia> lListaTransporte = new TransporteCustodia().TraduzirLista(lResponse.Resultado);
                            List<TransporteCustodia> lListaTransporte = new TransporteCustodia().TraduzirLista(lResponse.MonitorCustodia.ListaCustodia);

                            this.SessionUltimoResultadoDeBusca = lListaTransporte;

                            lRetornoLista = new TransporteDeListaPaginada(this.SessionUltimoResultadoDeBusca);

                            lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                            lRetornoLista.TotalDeItens = lResponse.MonitorCustodia.ListaCustodia.Count;

                            lRetornoLista.PaginaAtual = 1;

                            lRetornoLista.TotalDePaginas = 0;

                            TransporteDeListaPaginada lListaPaginada = new TransporteDeListaPaginada(lListaTransporte);

                            lRetorno = JsonConvert.SerializeObject(lListaPaginada);
                        }
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax("Erro durante a busca.", string.Format("{0}\r\n\r\n{1}\r\n\r\n{2}", lResponse.StatusResposta, lResponse.StatusResposta, lResponse.DescricaoResposta));
                    }
                }
            }
            catch (Exception exBusca)
            {

                throw exBusca;
            }

            return lRetorno;

        }

        private string ResponderPaginar()
        {
            string lRetorno = string.Empty;

            TransporteDeListaPaginada lLista = new TransporteDeListaPaginada();

            bool.TryParse(Request["Nova"], out lNova);
            bool.TryParse(Request["Descobertos"], out lDescobertos);

            if (this.SessionUltimoResultadoDeBusca != null && !lNova)
            {
                int lPagina;

                if (int.TryParse(Request["page"], out lPagina))
                {
                    lLista = BuscarPaginaDeResultados(lPagina);
                }
            }
            else
            {
                return ResponderBuscarItensParaSelecao();   
            }

            lRetorno = JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }

        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina)
        {
            var lRetorno = new TransporteDeListaPaginada();

            var lLista = new List<TransporteCustodia>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * lItensPorPagina);
            lIndiceFinal = (pPagina) * lItensPorPagina;

            for (int a = lIndiceInicial; a < lIndiceFinal; a++)
            {
                if (a < this.SessionUltimoResultadoDeBusca.Count)
                {
                    lLista.Add(this.SessionUltimoResultadoDeBusca[a]);
                }
            }

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double) lItensPorPagina));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }

        #endregion
    }
}