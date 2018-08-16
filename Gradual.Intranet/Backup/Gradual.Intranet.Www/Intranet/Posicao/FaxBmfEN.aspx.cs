using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;
using Gradual.OMS.Library;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;

namespace Gradual.Intranet.Www.Intranet.Posicao
{
    public partial class FaxBmfEN : PaginaBase
    {
        #region | Atributos
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

        #region | Eventos
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

        #region | Métodos
        private void CarregarHtml()
        {
            var lRelatorio = this.CarregarRelatorio();

            //this.lblCabecalho_Provisorio.Visible = DateTime.Today.Equals(this.GetDataInicial);
            this.lblCabecalho_DataEmissao.Text = this.GetDataInicial.ToString("dd/MM/yyyy");

            TransporteFaxBmf lTrans = new TransporteFaxBmf().TraduzirListaResumido(lRelatorio);

            this.lblCodigoCliente.Text = (this.GetCodCliente.ToString() + lTrans.DigitoCliente).PadLeft(10, '0');

            this.lblNomeCliente.Text = lTrans.NomeCliente;
            this.lblNomeAssessor.Text = lTrans.NomeAssessor;

            this.rptLinhasDoRelatorio.DataSource = lTrans.CabecalhosGrid;
            this.rptLinhasDoRelatorio.DataBind();

        }

        private FaxBmfInfo CarregarRelatorio()
        {
            try
            {
                var lServicoAtivador = Ativador.Get<IServicoRelatoriosFinanceiros>();

                var lResponse = lServicoAtivador.ObterFaxBmf(new FaxRequest()
                {
                    ConsultaCodigoClienteBmf = this.GetCodigoBMF,
                    ConsultaDataMovimento = this.GetDataInicial,
                });

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = this.GetCodCliente, DsObservacao = string.Concat("Consulta realizada para o cliente: cd_codigo = ", this.GetCodCliente) });

                    return lResponse.RelatorioBmf;
                }
                else
                {
                    throw new Exception(string.Format("{0}-{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Houve um erro ao tentar gerar a Bmf", ex);
                return new FaxBmfInfo();
            }
        }

        protected void rptLinhasDoRelatorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater lRepeaterDetalhesDoFax = this.BuscarRepeater(e.Item.Controls);

            lRepeaterDetalhesDoFax.DataSource = ((TransporteFaxBmfCabecalhoGrid)(e.Item.DataItem)).DetalhesBmf;
            lRepeaterDetalhesDoFax.DataBind();
        }

        private Repeater BuscarRepeater(ControlCollection pParametro)
        {
            if (null != pParametro && pParametro.Count > 0)
                for (int i = 0; i < pParametro.Count; i++)
                    if (pParametro[i] is Repeater && ((Repeater)pParametro[i]).ID == "rptLinhasDoRelatorioDetalhes")
                        return (Repeater)pParametro[i];

            return new Repeater();
        }
        #endregion
    }
}