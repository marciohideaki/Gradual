using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using Gradual.OMS.Library;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;

namespace Gradual.Intranet.Www.Intranet.Posicao
{
    public partial class FaxBovespaBR : PaginaBase
    {
        #region Atributos
        private DateTime GetDataInicial
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CdBovespaCliente"], out lRetorno);

                if (lRetorno == default(int))
                    int.TryParse(this.Request["CdBmfCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodigoBovespa
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CdBovespaCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodigoBMF
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CdBmfCliente"], out lRetorno);

                return lRetorno;
            }
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Request["Acao"] == "CarregarComoCSV")
                {
                    //this.ConvertURLToPDF(Request.Url.AbsoluteUri, "NotaDeCorretagem", false);
                }
                else if (this.Request["Acao"] == "CarregarComPaginacao")
                {

                    this.CarregarHtml();
                }
                else
                {

                    this.CarregarHtml();
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Houve um erro ao processar a Nota de Corretagem", ex);
            }
        }
        #endregion
        
        #region Métodos
        private void CarregarHtml()
        {
            var lRelatorio = this.CarregarRelatorio();

            //this.lblCabecalho_Provisorio.Visible = DateTime.Today.Equals(this.GetDataInicial);
            this.lblCabecalho_DataEmissao.Text   = this.GetDataInicial.ToString("dd/MM/yyyy");
            
            TransporteFaxBovespa lTrans = new TransporteFaxBovespa().TraduzirLista(lRelatorio);

            this.lblCodigoCliente.Text = (this.GetCodCliente.ToString() + lTrans.DigitoCliente).PadLeft(10, '0');

            this.lblNomeCliente.Text = lTrans.NomeCliente;
            this.lblNomeAssessor.Text = lTrans.NomeAssessor;

            this.rptLinhasDoRelatorioVista.DataSource = lTrans.CabecalhosGridVista;
            this.rptLinhasDoRelatorioVista.DataBind();

            if (lTrans.CabecalhosGridOpcao.Count == 0) this.divRelatorioOpcao.Visible = false;
            if (lTrans.CabecalhosGridVista.Count == 0) this.divRelatorioVista.Visible = false;

            //Rodapé A vista
            this.lblRodape_TotalComprasVista.Text      = lTrans.RodapeTotalComprasVista;
            this.lblRodape_TotalVendasVista .Text      = lTrans.RodapeTotalVendasVista;
            this.lblRodape_TotalTermoVista.Text        = lTrans.RodapeTotalTermoVista;
            this.lblRodape_TotalAjusteDiarioVista.Text = lTrans.RodapeTotalAjusteVista;
            this.lblRodape_TotalNegociosVista.Text     = lTrans.RodapeTotalNegociosVista;
            this.lblRodape_TotalCorretagemVista.Text   = lTrans.RodapeTotalCorretagemVista;
            this.lblRodape_TaxasCBLCVista.Text         = lTrans.RodapeTaxaCblcVista;
            this.lblRodape_TaxaBovespaVista.Text       = lTrans.RodapeTaxaBovespaVista;
            this.lblRodape_TaxasOperacionaisVista.Text = lTrans.RodapeTaxaOperacionaisVista;
            this.lblRodape_IRDayTradeVista.Text        = lTrans.RodapeIRDayTradeVista;
            this.lblRodape_AjustePosicaoVista.Text     = lTrans.RodapeTotalAjusteVista;
            this.lblRodape_TotalLiquidoVista.Text      = lTrans.RodapeTotalLiquidoVista;
            this.lblRodape_DebitoCreditoVista.Text = lTrans.RodapeTotalLiquidoVista.Contains("-") ? "Débito" : "Crédito";
            this.lblRodape_DataLiquidacaoVista.Text    = lTrans.DataLiquidacaoVista;

            this.rptLinhasDoRelatorioOpcao.DataSource = lTrans.CabecalhosGridOpcao;
            this.rptLinhasDoRelatorioOpcao.DataBind();

            //Rodapé opção
            this.lblRodape_TotalComprasOpcao.Text       = lTrans.RodapeTotalComprasOpcao;
            this.lblRodape_TotalVendasOpcao .Text       = lTrans.RodapeTotalVendasOpcao;
            this.lblRodape_TotalTermoOpcao.Text         = lTrans.RodapeTotalTermoOpcao;
            this.lblRodape_TotalAjusteDiarioOpcao.Text  = lTrans.RodapeTotalAjusteOpcao;
            this.lblRodape_TotalNegociosOpcao    .Text  = lTrans.RodapeTotalNegociosOpcao;
            this.lblRodape_TotalCorretagemOpcao  .Text  = lTrans.RodapeTotalCorretagemOpcao;
            this.lblRodape_TaxasCBLCOpcao        .Text  = lTrans.RodapeTaxaCblcOpcao;
            this.lblRodape_TaxaBovespaOpcao      .Text  = lTrans.RodapeTaxaBovespaOpcao;
            this.lblRodape_TaxasOperacionaisOpcao.Text  = lTrans.RodapeTaxaOperacionaisOpcao;
            this.lblRodape_IRDayTradeOpcao       .Text  = lTrans.RodapeIRDayTradeOpcao;
            this.lblRodape_AjustePosicaoOpcao    .Text  = lTrans.RodapeTotalAjusteOpcao;
            this.lblRodape_TotalLiquidoOpcao     .Text  = lTrans.RodapeTotalLiquidoOpcao;
            this.lblRodape_DebitoCreditoOpcao.Text = lTrans.RodapeTotalLiquidoOpcao.Contains("-") ? "Débito" : "Crédito";
            this.lblRodape_DataLiquidacaoOpcao.Text     =  lTrans.DataLiquidacaoOpcao;

            
        }

        private FaxBovespaInfo CarregarRelatorio()
        {
            try
            {
                var lServicoAtivador = Ativador.Get<IServicoRelatoriosFinanceiros>();

                var lResponse = lServicoAtivador.ObterFaxBovespa(new FaxRequest()
                    {
                        ConsultaCodigoCliente = this.GetCodCliente,
                        ConsultaDataMovimento = this.GetDataInicial,
                    });

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = this.GetCodCliente, DsObservacao = string.Concat("Consulta realizada para o cliente: cd_codigo = ", this.GetCodCliente) });

                    return lResponse.Relatorio;
                }
                else
                {
                    throw new Exception(string.Format("{0}-{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Houve um erro ao tentar gerar a nota de corretagem", ex);
                return new FaxBovespaInfo();
            }
        }

        protected void rptLinhasDoRelatorioOpcao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater lRepeaterDetalhesDaNota = this.BuscarRepeaterOpcao(e.Item.Controls);

            lRepeaterDetalhesDaNota.DataSource = ((TransporteFaxBovespaCabecalhoGrid)(e.Item.DataItem)).DetalhesBovespa;
            lRepeaterDetalhesDaNota.DataBind();
        }

        protected void rptLinhasDoRelatorioVista_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater lRepeaterDetalhesDaNota = this.BuscarRepeaterVista(e.Item.Controls);

            lRepeaterDetalhesDaNota.DataSource = ((TransporteFaxBovespaCabecalhoGrid)(e.Item.DataItem)).DetalhesBovespa;
            lRepeaterDetalhesDaNota.DataBind();
        }

        private Repeater BuscarRepeaterOpcao(ControlCollection pParametro)
        {
            if (null != pParametro && pParametro.Count > 0)
                for (int i = 0; i < pParametro.Count; i++)
                    if (pParametro[i] is Repeater && ((Repeater)pParametro[i]).ID == "rptLinhasDoRelatorioDetalhesOpcao")
                        return (Repeater)pParametro[i];

            return new Repeater();
        }

        private Repeater BuscarRepeaterVista(ControlCollection pParametro)
        {
            if (null != pParametro && pParametro.Count > 0)
                for (int i = 0; i < pParametro.Count; i++)
                    if (pParametro[i] is Repeater && ((Repeater)pParametro[i]).ID == "rptLinhasDoRelatorioDetalhesVista")
                        return (Repeater)pParametro[i];

            return new Repeater();
        }
        #endregion

        
    }
}