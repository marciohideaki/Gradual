using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;

using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;
using System.Globalization;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos
{
    public partial class OperacoesExtrato : System.Web.UI.UserControl
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var lBase = (PaginaBase)this.Page;

                if (string.IsNullOrEmpty(lBase.SessaoClienteLogado.CodigoPrincipal))
                {
                    lBase.ExibirMensagemJsOnLoad("I", "Você ainda não possui código de conta Gradual, para acessar essa opção finalize seu cadastro");
                    return;
                }

                this.CarregarDados();
               
            }
        }
        #endregion

        #region Métodos
        private void CarregarDados()
        {
            var lBase = new PaginaFundos();

            //var lRequest = new PosicaoConsolidadaIntregacaoFundosRequest();

            //lRequest.BuscarPor = OpcaoBuscaClienteIntegracaoFundosEnum.CodigoCBLC;

            //lRequest.TermoDeBusca = "204";// lBase.SessaoClienteLogado.CodigoPrincipal;

            //var ListaFundos = lBase.BuscarPosicaoConsolidada(lRequest);

            var lListaFundos = lBase.PosicaoFundosSumarizada();

            this.rptListaDeExtratoPosicaoConsolidado.DataSource = lListaFundos;
            this.rptListaDeExtratoPosicaoConsolidado.DataBind();

            this.rptListaDeExtratoMensal.DataSource = lListaFundos;
            this.rptListaDeExtratoMensal.DataBind();

            decimal lSaldoTotal = 0.0M;
            decimal lTotalBruto = 0.0M;
            decimal lTotalIR    = 0.0M;
            decimal lTotalIOF   = 0.0M;
            var lListaProdutos  = new List<int>();

            foreach (var fundo in lListaFundos)
            {
                lSaldoTotal += Convert.ToDecimal(fundo.ValorLiquido);
                lTotalBruto += Convert.ToDecimal(fundo.ValorBruto);
                lTotalIR    += Convert.ToDecimal(fundo.IR);
                lTotalIOF   += Convert.ToDecimal(fundo.IOF);

                lListaProdutos.Add(fundo.IdFundo.DBToInt32());
            }

            this.lblOperacoesExtratoTotalBruto.Text = lTotalBruto.ToString("N2");
            this.lblOperacoesExtratoTotalIOF.Text   = lTotalIOF.ToString("N2");
            this.lblOperacoesExtratoTotalIR.Text    = lTotalIR.ToString("N2");
            this.lblOperacoesExtratoSaldoTotal.Text = lSaldoTotal.ToString("N2");

            this.trNenhumExtratoConsolidada.Visible = lListaFundos.Count == 0;

            this.BuscarRentabilidadeFundo(lListaProdutos);
            this.BuscarRentabilidadeIndices();
            this.BuscarDadosRentabilidadeGrafico(lListaProdutos);
        }

        private void BuscarRentabilidadeFundo(List<int> pListaProdutos)
        {
            var lBase = new PaginaFundos();

            var lRequest = new RentabilidadeIntegracaoFundosRequest();

            lRequest.Produtos = pListaProdutos;

            var lResponse = lBase.PesquisarRentabilidadeFundo(lRequest);

            this.rptListaDeRentabilidade.DataSource = lResponse;
            this.rptListaDeRentabilidade.DataBind();

            this.trNenhumRentabilidade.Visible = pListaProdutos.Count == 0;
        }

        private void BuscarRentabilidadeIndices()
        {
            var lBase = new PaginaFundos();

            var ListaIndices = new List<IntegracaoFundosIndexadorInfo>();

            IndicePeriodoIntegracaoFundosRequest lRequest = new IndicePeriodoIntegracaoFundosRequest();
            
            ListaIndices = lBase.RetornoDoIndicePorPeriodo(lRequest);

            this.rptListaDeRentabilidadeIndices.DataSource = ListaIndices;
            this.rptListaDeRentabilidadeIndices.DataBind();

            trNenhumRentabilidadeIndices.Visible = ListaIndices.Count == 0;

        }

        private void BuscarDadosRentabilidadeGrafico(List<int> pListaProdutos)
        {
            var lBase = new PaginaFundos();

            var lResponse = new CompararRentabilidadeIntegracaoFundosResponse();

            var lRequest         = new CompararRentabilidadeIntegracaoFundosRequest();
            
            lRequest.Indexadores = new List<int>();
            
            lRequest.Periodo     = 1;
            
            lRequest.Produtos = pListaProdutos;

            lResponse = lBase.CompararRentabilidade(lRequest);

            var ListaGrafico = new List<GraficoExtratoMensalAux>();

            foreach (var fundos in lResponse.FundosSimulados)
            {
                var lInfo = new GraficoExtratoMensalAux();
                
                string lValor = string.Empty;

                int lCount = 0;

                foreach (var simulado in fundos)
                {
                    TimeSpan lSpan = new TimeSpan(new DateTime(1970, 1, 1).Ticks);

                    DateTime lDiferenca = simulado.Data.Subtract(lSpan);

                    long Data = (long)(lDiferenca.Ticks / 10000);

                    lInfo.NomeFundo = simulado.Produto.NomeProduto;

                    lValor += string.Concat("[", Data, ",", simulado.Valor.ToString("N2", new CultureInfo("en-US")), "]");

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

        public struct GraficoExtratoMensalAux
        {
            public string NomeFundo  { get; set; }
            public string Valor     { get; set; }
        }
        #endregion
    }
}