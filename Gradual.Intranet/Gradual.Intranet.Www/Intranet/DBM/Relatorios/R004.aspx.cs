using System;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.DBM.Relatorios
{
    public partial class R004 : PaginaBaseAutenticada
    {
        #region | Atributos

        public string gNomeCliente;

        #endregion

        #region | Propriedades

        private DateTime GetDataInicial
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataFinal
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request["DataFinal"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodigoCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CodigoCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private int? GetCdAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                return null;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                if (this.Acao == "BuscarItensParaListagemSimples")
                {
                    this.ResponderBuscarItensParaListagemSimples();
                }
                else if (this.Acao == "CarregarComoCSV")
                {
                    this.ResponderArquivoCSV();
                }
                else if (this.Acao == "BuscarParte")
                {
                    this.Response.Clear();

                    string lResponse = base.RetornarSucessoAjax("Carregado com sucesso!");

                    this.Response.Write(lResponse);

                    this.Response.End();
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax(ex.ToString());
            }
        }

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lListaAssessores = base.ConsultarCodigoAssessoresVinculadosString(this.GetCdAssessor);
            var lConsultaDBM = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<LTVDoClienteInfo>(
                new ConsultarEntidadeCadastroRequest<LTVDoClienteInfo>()
                {
                    EntidadeCadastro = new LTVDoClienteInfo()
                    {
                        ConsultaCodigoAssessor = lListaAssessores,
                        ConsultaCodigoCliente = this.GetCodigoCliente,
                        ConsultaDataAte = this.GetDataFinal,
                        ConsultaDataDe = this.GetDataInicial,
                    }
                });

            if (lConsultaDBM.StatusResposta == MensagemResponseStatusEnum.OK
            && (lConsultaDBM.Resultado != null)
            && (lConsultaDBM.Resultado.Count > 0))
            {
                var lTransporte = new TransporteRelatorio_004().TraduzirLista(lConsultaDBM.Resultado);

                this.gNomeCliente = lTransporte.Count > 0 ? lTransporte[0].CodigoNomeCliente : "Não encontrado";

                this.rptLTVDoCliente_Detalhes.DataSource = lTransporte;
                this.rptLTVDoCliente_Detalhes.DataBind();

                this.divLTVDoCliente_Detalhes.Visible = true;
                this.lblLTVDoCliente_Detalhes.Visible = false;
            }
            else
            {
                this.divLTVDoCliente_Detalhes.Visible = false;
                this.lblLTVDoCliente_Detalhes.Visible = true;
            }
        }

        private void ResponderArquivoCSV()
        {
            var lConteudoArquivo = new StringBuilder();
            var lListaAssessores = base.ConsultarCodigoAssessoresVinculadosString(this.GetCdAssessor);

            var lResponse = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<LTVDoClienteInfo>(
                new ConsultarEntidadeCadastroRequest<LTVDoClienteInfo>()
                {
                    EntidadeCadastro = new LTVDoClienteInfo()
                    {
                        ConsultaCodigoAssessor = lListaAssessores,
                        ConsultaCodigoCliente = this.GetCodigoCliente,
                        ConsultaDataAte = this.GetDataFinal,
                        ConsultaDataDe = this.GetDataInicial,
                    }
                });

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lLista = new TransporteRelatorio_004().TraduzirLista(lResponse.Resultado);

                lConteudoArquivo.AppendFormat("LTV do Cliente\r\n\r\n");

                if (lLista.Count > 0)
                    lConteudoArquivo.AppendFormat("Cliente: {0}\r\n\r\n", lLista[0].CodigoNomeCliente);

                lConteudoArquivo.AppendLine("Mês/Ano de ocorrência\tCorretagem no período (R$)\tVolume no período (R$)\t\r\n");

                foreach (TransporteRelatorio_004 info in lLista)
                    lConteudoArquivo.AppendFormat("{0}\t{1}\t{2}\t\r\n", info.Mes, info.Corretagem, info.Volume);

                this.Response.Clear();

                this.Response.ContentType = "text/xls";

                this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                this.Response.Charset = "iso-8859-1";

                this.Response.AddHeader("content-disposition", "attachment;filename=LTVDoCliente.xls");

                this.Response.Write(lConteudoArquivo.ToString());

                this.Response.End();
            }
        }

        #endregion
    }
}