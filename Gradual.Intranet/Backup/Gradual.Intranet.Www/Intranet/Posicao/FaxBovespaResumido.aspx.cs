using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;

namespace Gradual.Intranet.Www.Intranet.Posicao
{
    public partial class FaxBovespaResumido : PaginaBase
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
                base.RetornarErroAjax("Houve um erro ao processar a Fax (Resumido)", ex);
            }
        }
        #endregion

        #region Métodos
        private void CarregarHtml()
        {
            var lRelatorio = this.CarregarRelatorio();

            this.lblCabecalho_DataEmissao.Text = this.GetDataInicial.ToString("MM/dd/yyyy");
            this.lblCodigoCliente.Text = this.GetCodCliente.ToString().PadLeft(10, '0');

            TransporteFaxBovespa lTrans = new TransporteFaxBovespa().TraduzirListaResumido(lRelatorio);

            this.lblNomeCliente.Text = lTrans.NomeCliente;

            if (lTrans.CabecalhosGridOpcao.Count == 0) this.divRelatorioOpcao.Visible = false;
            if (lTrans.CabecalhosGridVista.Count == 0) this.divRelatorioVista.Visible = false;

            this.rptLinhasDoRelatorioOpcao.DataSource = lTrans.CabecalhosGridOpcao;
            this.rptLinhasDoRelatorioOpcao.DataBind();

            this.rptLinhasDoRelatorioVista.DataSource = lTrans.CabecalhosGridVista;
            this.rptLinhasDoRelatorioVista.DataBind();

            this.lblDataLiquidacaoOpcao.Text = lTrans.DataLiquidacaoOpcao;
            this.lblDataLiquidacaoVista.Text = lTrans.DataLiquidacaoVista;

            //Rodapé a vista
            this.lblRodape_TotalComprasVista        .Text = lTrans.RodapeTotalComprasVista;
            this.lblRodape_TotalVendasVista         .Text = lTrans.RodapeTotalVendasVista;
            this.lblRodape_TotalTermoVista          .Text = lTrans.RodapeTotalTermoVista;
            this.lblRodape_TaxaLiquidacaoVista      .Text = lTrans.RodapeTaxaLiquidacaoVista;
            this.lblRodape_EmolumentoBolsa          .Text = lTrans.RodapeEmolumentoBolsaVista;
            this.lblRodape_TotalCorretagemVista     .Text = lTrans.RodapeTotalCorretagemVista;
            this.lblRodape_EmolumentoTotalVista     .Text = lTrans.RodapeEmolumentoTotalVista;
            this.lblRodape_TaxaRegistroBolsaVista   .Text = lTrans.RodapeTaxaRegistroBolsaVista;
            this.lblRodape_TaxaRegistroVista        .Text = lTrans.RodapeTaxaRegistroVista;
            this.lblRodape_TaxaRegitroTotalVista    .Text = lTrans.RodapeTaxaRegistroTotalVista;
            this.lblRodape_BaseDayTradeVista        .Text = lTrans.RodapeBaseIRDTVista;
            this.lblRodape_BaseSemOperacoesVista    .Text = lTrans.RodapeBaseIROperacoesVista;
            this.lblRodape_ImpostoSemDayTradeVista  .Text = lTrans.RodapeIRDayTradeVista;
            this.lblRodape_ImpostoSemOperacoesVista .Text = lTrans.RodapeIrOperacoesVista;
            this.lblRodape_TotalLiquidoVista        .Text = lTrans.RodapeTotalLiquidoVista;
            this.lblRodape_DataLiquidacaoVista      .Text = lTrans.DataLiquidacaoVista;

            //Rodapé opção
            this.lblRodape_TotalComprasOpcao        .Text = lTrans.RodapeTotalComprasOpcao;
            this.lblRodape_TotalVendasOpcao         .Text = lTrans.RodapeTotalVendasOpcao;
            this.lblRodape_TotalTermoOpcao          .Text = lTrans.RodapeTotalTermoOpcao;
            this.lblRodape_TaxaLiquidacaoOpcao      .Text = lTrans.RodapeTaxaLiquidacaoOpcao;
            this.lblRodape_EmolumentoBolsaOpcao     .Text = lTrans.RodapeEmolumentoBolsaOpcao;
            this.lblRodape_TotalCorretagemOpcao     .Text = lTrans.RodapeTotalCorretagemOpcao;
            this.lblRodape_EmolumentoTotalOpcao     .Text = lTrans.RodapeEmolumentoTotalOpcao;
            this.lblRodape_TaxaRegistroBolsaOpcao   .Text = lTrans.RodapeTaxaRegistroBolsaOpcao;
            this.lblRodape_TaxaRegistroOpcao        .Text = lTrans.RodapeTaxaRegistroOpcao;
            this.lblRodape_TaxaRegitroTotalOpcao    .Text = lTrans.RodapeTaxaRegistroTotalOpcao;
            this.lblRodape_BaseDayTradeOpcao        .Text = lTrans.RodapeBaseIRDTOpcao;
            this.lblRodape_BaseSemOperacoesOpcao    .Text = lTrans.RodapeBaseIROperacoesOpcao;
            this.lblRodape_ImpostoSemDayTradeOpcao  .Text = lTrans.RodapeIRDayTradeOpcao;
            this.lblRodape_ImpostoSemOperacoesOpcao .Text = lTrans.RodapeIrOperacoesVista;
            this.lblRodape_TotalLiquidoOpcao        .Text = lTrans.RodapeTotalLiquidoOpcao;
            this.lblRodape_DataLiquidacaoOpcao.Text       = lTrans.DataLiquidacaoOpcao;
            
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
                base.RetornarErroAjax("Houve um erro ao tentar gerar o Fax Resumido", ex);
                return new FaxBovespaInfo();
            }
        }

        protected void rptLinhasDoRelatorioVista_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater lRepeaterDetalhesDaNota = this.BuscarRepeaterVista(e.Item.Controls);

            lRepeaterDetalhesDaNota.DataSource = ((TransporteFaxBovespaCabecalhoGrid)(e.Item.DataItem)).DetalhesBovespa;
            lRepeaterDetalhesDaNota.DataBind();
        }

        private Repeater BuscarRepeaterVista(ControlCollection pParametro)
        {
            if (null != pParametro && pParametro.Count > 0)
                for (int i = 0; i < pParametro.Count; i++)
                    if (pParametro[i] is Repeater && ((Repeater)pParametro[i]).ID == "rptLinhasDoRelatorioVistaDetalhes")
                        return (Repeater)pParametro[i];

            return new Repeater();
        }
        protected void rptLinhasDoRelatorioOpcao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater lRepeaterDetalhesDaNota = this.BuscarRepeaterOpcao(e.Item.Controls);

            lRepeaterDetalhesDaNota.DataSource = ((TransporteFaxBovespaCabecalhoGrid)(e.Item.DataItem)).DetalhesBovespa;
            lRepeaterDetalhesDaNota.DataBind();
        }

        private Repeater BuscarRepeaterOpcao(ControlCollection pParametro)
        {
            if (null != pParametro && pParametro.Count > 0)
                for (int i = 0; i < pParametro.Count; i++)
                    if (pParametro[i] is Repeater && ((Repeater)pParametro[i]).ID == "rptLinhasDoRelatorioOpcaoDetalhes")
                        return (Repeater)pParametro[i];

            return new Repeater();
        }
        #endregion
    }
}