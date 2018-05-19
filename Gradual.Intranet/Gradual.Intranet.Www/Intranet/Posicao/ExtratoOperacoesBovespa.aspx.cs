using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;

namespace Gradual.Intranet.Www.Intranet.Posicao
{
    public partial class ExtratoOperacoesBovespa : PaginaBase
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
                base.RetornarErroAjax("Houve um erro ao processar a Fax bmf BR", ex);
            }
        }
        #endregion

        #region Métodos
        private void CarregarHtml()
        {
            var lRelatorio = this.CarregarRelatorio();

            List<TransporteExtratoOperacoesBovespa> lTrans = new TransporteExtratoOperacoesBovespa().TraduzirLista( lRelatorio );

            this.rptLinhasDoRelatorio.DataSource = lTrans;
            this.rptLinhasDoRelatorio.DataBind();

        }

        private ExtratoOrdemInfo CarregarRelatorio()
        {
            try
            {
                var lServicoAtivador = Ativador.Get<IServicoRelatoriosFinanceiros>();

                ExtratoOperacaoResponse lResponse = lServicoAtivador.ObterExtratoOperacoesBovespa(new ExtratoOperacaoRequest()
                {
                    ConsultaCodigoCliente = this.GetCodigoBovespa,
                    ConsultaDataMovimento = this.GetDataInicial,
                });

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = this.GetCodCliente, DsObservacao = string.Concat("Consulta realizada para o cliente: cd_codigo = ", this.GetCodCliente) });

                    return lResponse.RelatorioBovespa;
                }
                else
                {
                    throw new Exception(string.Format("{0}-{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Houve um erro ao tentar gerar Relatorio de Extrato de Operações", ex);
                return new ExtratoOrdemInfo();
            }
        }

        protected void rptLinhasDoRelatorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater lRepeaterDetalhesDoFax = this.BuscarRepeater(e.Item.Controls);

            lRepeaterDetalhesDoFax.DataSource = ((TransporteExtratoOperacoesBovespa)(e.Item.DataItem)).Detalhes;
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