using System;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.DBM.Relatorios
{
    public partial class R005 : PaginaBaseAutenticada
    {
        #region | Atributos

        public string gTotalClientesOperaram;

        public string gDataConsultaInicio;

        public string gDataConsultaFim;

        public string gDescricaoOperador;

        public string gTotalCB = "0,00";

        public string gTotalDV = "0,00";

        public string gTotalCL = "0,00";

        public string gTotalFG = "0,00";

        public string gTotalPC = "0,00";

        public string gTotalVC = "0,00";

        #endregion

        #region | Propriedades

        private int? GetCdAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodAssessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private DateTime GetDataNegocioInicio
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataOperacaoInicio"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataNegocioFim
        {
            get
            {
                var lRetorno = DateTime.Today.AddDays(1);

                if (!DateTime.TryParse(this.Request["DataOperacaoFim"], out lRetorno))
                    return DateTime.Today.AddDays(1);

                return lRetorno;
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

            var lResponse = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<TotalClientePorAssessorInfo>(new ConsultarEntidadeCadastroRequest<TotalClientePorAssessorInfo>()
            {
                EntidadeCadastro = new TotalClientePorAssessorInfo()
                {
                    ConsultaCdAssessor = lListaAssessores,
                    ConsultaDtNegocioInicio = this.GetDataNegocioInicio,
                    ConsultaDtNegocioFim = this.GetDataNegocioFim,
                }
            });

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_005().TraduzirLista(lResponse.Resultado);

                if (lTransporte.Count > 0)
                {
                    this.gDescricaoOperador = lTransporte[0].Operador;
                    this.gTotalClientesOperaram = lTransporte.Count.ToString();
                    this.gDataConsultaInicio = this.GetDataNegocioInicio.ToString("dd/MM/yyyy");
                    this.gDataConsultaFim = this.GetDataNegocioFim.ToString("dd/MM/yyyy");
                    this.gTotalCB = lTransporte[0].TotalCB;
                    this.gTotalDV = lTransporte[0].TotalDV;
                    this.gTotalCL = lTransporte[0].TotalCL;
                    this.gTotalFG = lTransporte[0].TotalFG;
                    this.gTotalPC = lTransporte[0].TotalPC;
                    this.gTotalVC = lTransporte[0].TotalVC;
                }

                this.rptTotalDeAssessorPorCliente.DataSource = lTransporte;
                this.rptTotalDeAssessorPorCliente.DataBind();
            }
        }

        private void ResponderArquivoCSV()
        {
            var lConteudoArquivo = new StringBuilder();
            var lListaAssessores = base.ConsultarCodigoAssessoresVinculadosString(this.GetCdAssessor);

            var lResponse = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<TotalClientePorAssessorInfo>(new ConsultarEntidadeCadastroRequest<TotalClientePorAssessorInfo>()
            {
                EntidadeCadastro = new TotalClientePorAssessorInfo()
                {
                    ConsultaCdAssessor = lListaAssessores,
                    ConsultaDtNegocioInicio = this.GetDataNegocioInicio,
                    ConsultaDtNegocioFim = this.GetDataNegocioFim
                }
            });

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_005().TraduzirLista(lResponse.Resultado);

                lConteudoArquivo.AppendLine("Total de cliente por assessor\t\r\n\r\n");

                lConteudoArquivo.AppendFormat("{0}Clientes operaram no dia: {1}\n\r\n", lTransporte.Count.ToString(), this.GetDataNegocioInicio.ToString("dd/MM/yyyy"));

                if (lTransporte.Count > 0)
                {
                    lConteudoArquivo.AppendFormat("Assessor: {0}\tBolsa\tC.B.\tDV\t\tC.L.\tF.G\tP.C.\tV.C.\r\n", base.UsuarioLogado.Nome);

                    lTransporte.ForEach(toc =>
                    {
                        lConteudoArquivo.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\r\n", toc.Cliente, toc.Bolsa, toc.CB, toc.DVDesconto, toc.DVPercentual, toc.CL, toc.FG, toc.PC, toc.VC);
                    });

                    lConteudoArquivo.AppendFormat("Totais:\t\t{0}\t{1}\t-\t{2}\t{3}\t{4}\t{5}", lTransporte[0].TotalCB, lTransporte[0].TotalDV, lTransporte[0].TotalCL, lTransporte[0].TotalFG, lTransporte[0].TotalPC, lTransporte[0].TotalVC);
                }

                this.Response.Clear();

                this.Response.ContentType = "text/xls";

                this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                this.Response.Charset = "iso-8859-1";

                this.Response.AddHeader("content-disposition", "attachment;filename=TotalAssessorPorCliente.xls");

                this.Response.Write(lConteudoArquivo.ToString());

                this.Response.End();
            }
        }

        #endregion
    }
}