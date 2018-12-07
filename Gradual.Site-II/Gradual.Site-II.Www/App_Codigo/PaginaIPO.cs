using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Servicos.BancoDeDados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados.Vendas;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Vendas;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using Gradual.OMS.Monitor.Custodia.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using System.Web.UI.WebControls;
using System.Text;

namespace Gradual.Site.Www
{
    public class PaginaIPO : PaginaFundos
    {
        #region Propriedades
        
        #endregion
        /// <summary>
        /// Método que que chama a procedure de inclusão de IPO no banco de dados 
        /// </summary>
        /// <param name="pParametros">Objeto emcapsulado de dados de cadastro do IPO</param>
        /// <returns>Retorna um objeto de cadastro de IPO</returns>
        public IPOClienteInfo InserirIPOCliente(SalvarEntidadeRequest<IPOClienteInfo> pParametro)
        {
            var lResposta = ProdutoIPOClienteDbLib.Salvar(pParametro);

            return lResposta.Objeto;
        }

        /// <summary>
        /// Método que recebe os dados de filtro, ou não, e efetua a consulta no bancos de dados 
        /// </summary>
        /// <param name="pParametros"></param>
        /// <returns>Retorna uma lista de objeto de gerenciamento de IPO do banco de dados</returns>
        public List<IPOClienteInfo> ConsultarIPOCliente(ConsultarEntidadeRequest<IPOClienteInfo> pParametros)
        {
            var lResposta = ProdutoIPOClienteDbLib.ConsultarProdutosIPOCliente(pParametros);

            return lResposta.Resultado;
        }

        /// <summary>
        /// Seleciona os produtos de IPO que irão aaparece no site como compra para os
        /// clientes e na intranet para os assessores aderirem para o cliente
        /// </summary>
        /// <param name="pParametros">Parametros para filtro no banco de dados </param>
        /// <returns>Retorna o objeto de IPOINFO selecionado</returns>
        public IPOInfo SelecionarIPOSite(ReceberEntidadeRequest<IPOInfo> pParametros)
        {
            var lResposta = ProdutoIPODbLib.SelecionarProdutosIPOSite(pParametros);

            return lResposta.Objeto;
        }

        /// <summary>
        /// Seleciona os produtos de IPO que irão aaparece no site como compra para os
        /// clientes e na intranet para os assessores aderirem para o cliente
        /// </summary>
        /// <param name="pParametros">Parametros para filtro no banco de dados</param>
        /// <returns>Retorna o objeto de IPOINFO selecionado</returns>
        public List<IPOInfo> ConsultarIPOSite(ConsultarEntidadeRequest<IPOInfo> pParametros)
        {
            var lResposta = ProdutoIPODbLib.ConsultarProdutosIPO(pParametros);

            var ListaIPOAtivos = new List<IPOInfo>();

            foreach (IPOInfo info in lResposta.Resultado)
            {
                if (info.StAtivo)
                    ListaIPOAtivos.Add(info);
            }

            return ListaIPOAtivos;
        }

        /// <summary>
        /// Seleciona os itens de custódia do cliente logado ou buscado
        /// </summary>
        /// <returns>Retorna uma lista do tipo de Transporte de Custodia</returns>
        protected List<TransporteCustodiaInfo> BuscarCustodiasDoCliente(int CodigoCliente)
        {
            var lRequest = new MonitorCustodiaRequest();

            var lResponse = new MonitorCustodiaResponse();

            var gServicoCustodia = Ativador.Get<IServicoMonitorCustodia>();

            lRequest.CodigoCliente = CodigoCliente;

            lResponse = gServicoCustodia.ObterMonitorCustodiaMemoria(lRequest);

            List<TransporteCustodiaInfo> lRetorno = new List<TransporteCustodiaInfo>();

            IEnumerable<MonitorCustodiaInfo.CustodiaPosicao> Lista = from a in lResponse.MonitorCustodia.ListaCustodia orderby a.Resultado descending select a;

            lRetorno = TransporteCustodiaInfo.TraduzirCustodiaInfo(Lista.ToList());

            return lRetorno;
        }

        /// <summary>
        /// Exibe um "alert" no Page_Load via javascript
        /// </summary>
        /// <param name="pTipo_IAE">String de estilo da mensagem: 'A' para alerta, 'I' para informação e 'E' para erro.</param>
        /// <param name="pMensagem">Mensagem que será exibida</param>
        /// <param name="pRetornarAoEstadoNormalAposSegundos">Flag que indica se o painel deve voltar ao estado 'normal' depois de alguns segundos de exibição da mensagem.</param>
        /// <param name="pMensagemAdicional">(opcional) Mensagem longa, que aparece na caixa de texto no painel de erro.</param>
        //public void ExibirMensagemJsOnLoad(string pTipo_IAE, string pMensagem, bool pRetornarAoEstadoNormalAposSegundos = false, string pMensagemAdicional = "")
        //{
        //    string lMensagem, lMensagemExtendida;

        //    lMensagem = SanitarStringPraJavascript(pMensagem);

        //    //lMensagemExtendida = SanitarStringPraJavascript(pMensagemAdicional);
        //    /*
        //    if (lMensagem.Length > 180)
        //    {
        //        lMensagemExtendida = string.Format("{0}\\n\\n{1}", lMensagem, lMensagemExtendida);

        //        lMensagem = lMensagem.Substring(0, 180) + "(...)";
        //    }*/
        //    /*
        //    this.RodarJavascriptOnLoad(string.Format("GradSite_ExibirMensagem(\"{0}\", \"{1}\", {2}{3});"
        //                                            , pTipo_IAE
        //                                            , lMensagem
        //                                            , pRetornarAoEstadoNormalAposSegundos ? "true" : "false"
        //                                            , string.IsNullOrEmpty(lMensagemExtendida) ? "" : ", \"" + lMensagemExtendida + "\""));
        //     */
        //}


        #region Eventos
        /// <summary>
        /// Evento de Page load complete para 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Literal litJavascript = (Literal)this.FindControl("litJavascriptOnLoad");

            if (litJavascript == null && this.PaginaMaster != null)
                litJavascript = (Literal)this.PaginaMaster.FindControl("litJavascriptOnLoad");

            if (litJavascript != null)
                litJavascript.Text = this.JavascriptParaRodarOnLoad;

            if (this.PerfMonHabilitado && this.PaginaMaster != null)
            {
                this.PaginaMaster.RelatorioDePerformance = PerfMon.RenderizarRelatorio(this.PerfMonChave, true);
            }
        }
        #endregion
    }
}