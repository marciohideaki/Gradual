using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;

using System.Globalization;
using Gradual.Site.Www.Transporte;

namespace Gradual.Site.Www.MinhaConta.Produtos.Fundos
{
    public partial class Simular : PaginaFundos
    {
        #region Propriedade
        private const int DOLAR = 1;
        private const int IGPM = 2;
        private const int IBOVESPA = 9;
        private const int CDI = 37;
        private const int IBX = 14;
        private const int IBA = 15;
        private const int SELIC = 38;
        private const int EURO = 36;

        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.ValidarSessao())
            {
                if (!this.IsPostBack)
                {
                    var lBase = (PaginaBase)this.Page;

                    //if (string.IsNullOrEmpty(lBase.SessaoClienteLogado.CodigoPrincipal))
                    //{
                    //    if (!lBase.JavascriptParaRodarOnLoad.Contains("Você ainda não possui código de conta Gradual"))
                    //    {
                    //        lBase.ExibirMensagemJsOnLoad("I", "Você ainda não possui código de conta Gradual, para acessar essa opção finalize seu cadastro");
                    //    }

                    //    return;
                    //}

                    this.CarregarDados();
                }
            }
        }

        protected void tbnFitroConsultaSimulacao_Click(object sender, EventArgs e)
        {
            if (!this.ValidaDados())
                return;

            var lBase = new PaginaFundos();

            var lRequest = new SimularAplicacaoIntegracaoFundosRequest();

            var ListaProdutos = ListarProdutos();

            lRequest.Produtos = ListaProdutos;

            lRequest.Periodo = this.PeriodoFiltroConsulta.SelectedValue.DBToInt32();

            lRequest.Indexadores = ListarIndexadores();

            decimal lValorRequested = default(decimal);

            decimal.TryParse(ValorFiltroConsulta.Text, out lValorRequested);

            if (!lValorRequested.Equals(0))
            {
                lRequest.Valor = lValorRequested;
            }

            var lResponse = base.ListarSimularAplicacaoGrid(lRequest);

            this.rptListaDeSimulacao.DataSource = lResponse.ListarProdutosSimulados;
            this.rptListaDeSimulacao.DataBind();

            this.trNenhumSimularResultado.Visible = (lResponse.ListarProdutosSimulados.Count == 0);

            pnlSimulacaoResultados.Attributes["style"] = "";

            if (lValorRequested.Equals(0))
            {
                this.BuscarDadosCompararRentabilidadeGrafico(ListaProdutos);
            }
            else
            {
                this.BuscarDadosSimularRentabilidadeGrafico(ListaProdutos, lValorRequested);
            }

            base.RodarJavascriptOnLoad("MinhaConta_Fundos_Simular_RecarregaGridEscolhidos();MinhaConta_GerarGrafico_Fundos_Simular();");
        }
        #endregion

        #region Métodos
        private List<int> ListarIndexadores()
        {
            var lRetorno = new List<int>();

            if (this.chkCDI.Checked) lRetorno.Add(CDI);
            if (this.chkDOLAR.Checked) lRetorno.Add(DOLAR);
            if (this.chkIBOVESPA.Checked) lRetorno.Add(IBOVESPA);
            if (this.chkIBX.Checked) lRetorno.Add(IBX);
            if (this.chkIBA.Checked) lRetorno.Add(IBA);
            if (this.chkIGPM.Checked) lRetorno.Add(IGPM);
            if (this.chkSELIC.Checked) lRetorno.Add(SELIC);
            if (this.chkEURO.Checked) lRetorno.Add(EURO);

            return lRetorno;
        }

        private List<int> ListarProdutos()
        {
            var lRetorno = new List<int>();

            string lProdutos = this.hddSelect_produtos_adicionados.Value;

            string[] lSeparador = new string[] { "[", "]" };

            string[] lProd = lProdutos.Split(lSeparador, StringSplitOptions.RemoveEmptyEntries);

            foreach (string fundo in lProd)
            {
                lRetorno.Add(int.Parse(fundo));
            }

            return lRetorno;
        }

        protected bool ValidaDados()
        {
            var lBase = (PaginaBase)this.Page;

            bool lRetorno = true;

            if (this.hddSelect_produtos_adicionados.Value == string.Empty)
            {
                lRetorno = false;
                lBase.ExibirMensagemJsOnLoad("I", "É necessário adicionar fundos para a simulação!");
            }

            //if (this.ValorFiltroConsulta.Text.Equals("0,00") || this.ValorFiltroConsulta.Text.Equals(string.Empty))
            //{
            //    lRetorno = false;
            //    lBase.ExibirMensagemJsOnLoad("I", "É necessátio inserir um valor para a simulação!");
            //}

            return lRetorno;
        }

        protected void CarregarDados()
        {
            var lBase = new PaginaFundos();

            var lRequest = new PesquisarIntegracaoFundosRequest();

            List<Transporte_IntegracaoFundos> lResposta = lBase.PesquisarFundosAplicar(lRequest);

            this.rptListaParaSimulacao.DataSource = lResposta;
            this.rptListaParaSimulacao.DataBind();
        }

        private void BuscarDadosCompararRentabilidadeGrafico(List<int> pListaProdutos)
        {
            var lBase = new PaginaFundos();

            var lResponse = new CompararRentabilidadeIntegracaoFundosResponse();

            var lRequest = new CompararRentabilidadeIntegracaoFundosRequest();

            lRequest.Indexadores = new List<int>();

            lRequest.Periodo = 1;

            lRequest.Produtos = pListaProdutos;

            lRequest.Indexadores = ListarIndexadores();

            lResponse = lBase.CompararRentabilidade(lRequest);

            var ListaGrafico = new List<GraficoSimularAux>();

            foreach (var fundos in lResponse.FundosSimulados)
            {
                var lInfo = new GraficoSimularAux();

                string lValor = string.Empty;

                int lCount = 0;

                foreach (var simulado in fundos)
                {
                    TimeSpan lSpan = new TimeSpan(new DateTime(1970, 1, 1).Ticks);

                    DateTime lDiferenca = simulado.Data.Subtract(lSpan);

                    long Data = (long)(lDiferenca.Ticks / 10000);

                    lInfo.NomeFundo = simulado.Produto.NomeProduto;

                    lValor += string.Concat("[", Data, ",", simulado.Valor.ToString(new CultureInfo("en-US")), "]");

                    lCount++;

                    if (lCount != fundos.Count)
                    {
                        lValor += ",";
                    }

                }

                lInfo.Valor = string.Concat("[", lValor, "]");

                ListaGrafico.Add(lInfo);
            }

            this.rptListaDeValoresGrafico.DataSource = ListaGrafico;
            this.rptListaDeValoresGrafico.DataBind();
        }

        private void BuscarDadosSimularRentabilidadeGrafico(List<int> pListaProdutos, decimal pValor)
        {
            var lBase = new PaginaFundos();

            var lResponse = new SimularAplicacaoIntegracaoFundosResponse();

            var lRequest = new SimularAplicacaoIntegracaoFundosRequest();

            lRequest.Indexadores = new List<int>();

            lRequest.Periodo = 1;

            lRequest.Produtos = pListaProdutos;

            lRequest.Indexadores = ListarIndexadores();

            lRequest.Valor = pValor;

            lResponse = lBase.SimularRentabilidade(lRequest);

            var ListaGrafico = new List<GraficoSimularAux>();

            foreach (var fundos in lResponse.ProdutosSimulados)
            {
                var lInfo = new GraficoSimularAux();

                string lValor = string.Empty;

                int lCount = 0;

                foreach (var simulado in fundos)
                {
                    TimeSpan lSpan = new TimeSpan(new DateTime(1970, 1, 1).Ticks);

                    DateTime lDiferenca = simulado.Data.Subtract(lSpan);

                    long Data = (long)(lDiferenca.Ticks / 10000);

                    lInfo.NomeFundo = simulado.Produto.NomeProduto;

                    lValor += string.Concat("[", Data, ",", simulado.Valor.ToString(new CultureInfo("en-US")), "]");

                    lCount++;

                    if (lCount != fundos.Count)
                    {
                        lValor += ",";
                    }

                }

                lInfo.Valor = string.Concat("[", lValor, "]");

                ListaGrafico.Add(lInfo);
            }

            this.rptListaDeValoresGrafico.DataSource = ListaGrafico;
            this.rptListaDeValoresGrafico.DataBind();
        }

        public struct GraficoSimularAux
        {
            public string NomeFundo { get; set; }
            public string Valor { get; set; }
        }
        #endregion
    }
}