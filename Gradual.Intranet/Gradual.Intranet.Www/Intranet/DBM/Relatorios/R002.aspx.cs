using System;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM;
using Gradual.OMS.Library;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.Intranet.DBM.Relatorios
{
    public partial class R002 : PaginaBaseAutenticada
    {
        #region | Atributos

        public string gRelatorioDataInicial;

        public string gRelatorioDataFinal;

        public string gCadastroNomeAssessor;

        public string gCadastroQuantidadeTotal;

        public string gCadastroPercentualTotal;

        public string gCadastroQuantidadeAtivos;

        public string gCadastroPercentualAtivos;

        public string gCadastroQuantidadeInativos;

        public string gCadastroQuantidadeClientesNovos;

        public string gCadastroPercentualInativos;

        public string gCadastroQuantidadeVarejo;

        public string gCadastroQuantidadeInstitucional;

        public string gCadastroPercentualOperouNoMes;

        public string gCadastroPercentualComCustodia;

        public string gReceitaBovespaClientes;

        public string gReceitaBovespaValor;

        public string gReceitaBMFClientes;

        public string gReceitaBMFValor;

        public string gReceitaBTCClientes;

        public string gReceitaBTCValor;

        public string gReceitaTesouroClientes;

        public string gReceitaTesouroValor;

        public string gReceitaOutrasClientes;

        public string gReceitaOutrasValor;

        public string gReceitaTotalClientes;

        public string gReceitaTotalValor;

        public string gCanalHbValor;

        public string gCanalHbPercentual;

        public string gCanalRepassadorValor;

        public string gCanalRepassadorPercentual;

        public string gCanalMesaValor;

        public string gCanalTotalPercentual;

        public string gCanalTotalValor;

        public string gCanalMesaPercentual;

        public string gMetricasCorretagemNoDia;

        public string gMetricasCorretagemNoMes;

        public string gMetricasCorretagemNoMesAnterior;

        public string gMetricasCorretagemNoAno;

        public string gMetricasCadastrosNoDia;

        public string gMetricasCadastrosNoMes;

        public string gMetricasCadastrosNoMesAnterior;

        public string gMetricasCadastrosNoAno;

        private static ReceberEntidadeCadastroResponse<ResumoDoAssessorCadastroInfo> gResumoDoAssessorCadastro;

        private static ReceberEntidadeCadastroResponse<ResumoDoAssessorReceitaInfo> gResumoDoAssessorReceita;

        private static ReceberEntidadeCadastroResponse<ResumoDoAssessorCanalInfo> gResumoDoAssessorCanal;

        private static ReceberEntidadeCadastroResponse<ResumoDoAssessorMetricasInfo> gResumoDoAssessorMetricas;

        private static ConsultarEntidadeCadastroResponse<ResumoDoAssessorTop10Info> gResumoDoAssessorTop10;

        #endregion

        #region | Propriedades

        private int? GetCodigoAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 )//&& base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodAssessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private DateTime GetDataInicial
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataFinal
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataFinal"], out lRetorno);

                return lRetorno;
            }
        }

        private ReceberEntidadeCadastroResponse<ResumoDoAssessorCadastroInfo> GetDadosCadastrais
        {
            get
            {
                if (null == gResumoDoAssessorCadastro)
                {
                    gResumoDoAssessorCadastro = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ResumoDoAssessorCadastroInfo>(
                        new ReceberEntidadeCadastroRequest<ResumoDoAssessorCadastroInfo>()
                        {
                            EntidadeCadastro = new ResumoDoAssessorCadastroInfo()
                            {
                                ConsultaCodigoAssessor = this.GetCodigoAssessor
                            }
                        });
                }

                return gResumoDoAssessorCadastro;
            }
        }

        private ReceberEntidadeCadastroResponse<ResumoDoAssessorReceitaInfo> GetDadosReceita
        {
            get
            {
                if (null == gResumoDoAssessorReceita)
                {
                    gResumoDoAssessorReceita = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ResumoDoAssessorReceitaInfo>(
                        new ReceberEntidadeCadastroRequest<ResumoDoAssessorReceitaInfo>()
                        {
                            EntidadeCadastro = new ResumoDoAssessorReceitaInfo()
                            {
                                ConsultaDataFinal = this.GetDataFinal,
                                ConsultaDataInicial = this.GetDataInicial,
                                ConsultaCodigoAssessor = this.GetCodigoAssessor,
                            }
                        });
                }

                return gResumoDoAssessorReceita;
            }
        }

        private ReceberEntidadeCadastroResponse<ResumoDoAssessorCanalInfo> GetDadosCanal
        {
            get
            {
                if (null == gResumoDoAssessorCanal)
                {
                    gResumoDoAssessorCanal = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ResumoDoAssessorCanalInfo>(
                        new ReceberEntidadeCadastroRequest<ResumoDoAssessorCanalInfo>()
                        {
                            EntidadeCadastro = new ResumoDoAssessorCanalInfo()
                            {
                                ConsultaCodigoAssessor = this.GetCodigoAssessor,
                                ConsultaDataFinal = this.GetDataFinal,
                                ConsultaDataInicial = this.GetDataInicial,
                            }
                        });
                }

                return gResumoDoAssessorCanal;
            }
        }

        private ReceberEntidadeCadastroResponse<ResumoDoAssessorMetricasInfo> GetDadosMetricas
        {
            get
            {
                if (null == gResumoDoAssessorMetricas)
                {
                    gResumoDoAssessorMetricas = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ResumoDoAssessorMetricasInfo>(
                        new ReceberEntidadeCadastroRequest<ResumoDoAssessorMetricasInfo>()
                        {
                            EntidadeCadastro = new ResumoDoAssessorMetricasInfo()
                            {
                                ConsultaCdAssessor = this.GetCodigoAssessor,
                                ConsultaDataFim = this.GetDataFinal,
                                ConsultaDataInicio = this.GetDataInicial,
                            }
                        });
                }

                return gResumoDoAssessorMetricas;
            }
        }

        private ConsultarEntidadeCadastroResponse<ResumoDoAssessorTop10Info> GetDadosTop10Clientes
        {
            get
            {
                if (null == gResumoDoAssessorTop10)
                {
                    gResumoDoAssessorTop10 = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ResumoDoAssessorTop10Info>(
                        new ConsultarEntidadeCadastroRequest<ResumoDoAssessorTop10Info>()
                        {
                            EntidadeCadastro = new ResumoDoAssessorTop10Info()
                            {
                                ConsultaCodigoAssessor = this.GetCodigoAssessor,
                                ConsultaDataFinal = this.GetDataFinal,
                                ConsultaDataInicial = this.GetDataInicial
                            }
                        });
                }

                return gResumoDoAssessorTop10;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var lUsuarioPodeVisualizarResumoGerencial = base.UsuarioPode("Consultar", "6f94ac37-a66d-4199-a181-37b6b3b05bee");

                if (lUsuarioPodeVisualizarResumoGerencial)
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

                        string lResponse = base.RetornarSucessoAjax("Carregado com sucesso");

                        this.Response.Write(lResponse);

                        this.Response.End();
                    }
                }
                else
                {
                    this.Response.Clear();

                    string lResponse = base.RetornarSucessoAjax("Carregado com sucesso");

                    this.Response.Write(lResponse);
                    this.divRelatorioGerencial.Visible = false;
                    this.divMensagemDeErroDeAcesso.Visible = true;
                    //this.Response.Clear();
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
            gResumoDoAssessorCadastro = null;
            gResumoDoAssessorReceita = null;
            gResumoDoAssessorCanal = null;
            gResumoDoAssessorMetricas = null;
            gResumoDoAssessorTop10 = null;

            this.CarregarDadosCadastro();
            this.CarregarDadosReceita();
            this.CarregarDadosCanal();
            this.CarregarDadosMetricas();
            this.CarregarClientesTop10();
        }

        private void ResponderArquivoCSV()
        {
            var lConteudoArquivo = new StringBuilder("Cadastro\n");
            lConteudoArquivo.Append("\tQtde. Total\t% Total\tQtde. Ativos\t% Ativos\tQtde. Inativos\t% Inativos");
            lConteudoArquivo.Append("\tQtde. Novos Cliente Mês\tQtde. Varejo\tQtde. Institucional\t% Operou no Mês\t% com Custódia\t\n");

            if (this.GetDadosCadastrais.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_002_Cadastro(this.GetDadosCadastrais.EntidadeCadastro);

                lConteudoArquivo.AppendFormat("\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t"
                     , lTransporte.QuantidadeTotal, lTransporte.PercentualTotal, lTransporte.QuantidadeAtivos, lTransporte.PercentualAtivos, lTransporte.QuantidadeInativos, lTransporte.PercentualInativos);
                lConteudoArquivo.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}"
                    , lTransporte.QuantidadeClientesNovos, lTransporte.QuantidadeVarejo, lTransporte.QuantidadeInstitucional, lTransporte.PercentualOperouNoMes, lTransporte.PercentualComCustodia);
            }

            lConteudoArquivo.Append("\nReceita / Canal / Métricas\n");

            if (this.GetDadosReceita.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_002_Receita(this.GetDadosReceita.EntidadeCadastro, this.GetCodigoAssessor);

                lConteudoArquivo.Append("\tCorr. BVSP\t% BVSP\tCorr. BM&F\t% BM&F\tCorr. BTC\t% BTC\tCorr. Tesouro\t% Tesouro\tCorr. outras\t% Outras\t\n");

                lConteudoArquivo.AppendFormat("\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}"
                    , lTransporte.BovespaValor, lTransporte.BovespaClientes, lTransporte.BMFValor, lTransporte.BMFClientes, lTransporte.TBCValor, lTransporte.TBCClientes, lTransporte.TesouroValor, lTransporte.TesouroClientes, lTransporte.OutrasValor, lTransporte.OutrasClientes);
            }

            if (this.GetDadosCanal.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_002_Canal(this.GetDadosCanal.EntidadeCadastro, this.GetCodigoAssessor);

                lConteudoArquivo.Append("\r\tCorr. HB\t% HB\tCorr. Repassador\t% Repassador\tCorr. Mesa\t% Mesa\t");
                lConteudoArquivo.AppendFormat("\r\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}"
                    , lTransporte.HbValor, lTransporte.HbPercentual, lTransporte.RepassadorValor, lTransporte.RepassadorPercentual, lTransporte.MesaValor, lTransporte.MesaPercentual);
            }

            if (this.GetDadosMetricas.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorios_002_Metricas(this.GetDadosMetricas.EntidadeCadastro);

                lConteudoArquivo.Append("\r\tCorr. no Mês\t Cad. no Mês\tCorr. no Mês Anterior\t Cadastros no Mês Anterior\tCorr. Média no Período\t Cadastros Média no Período\t");
                lConteudoArquivo.AppendFormat("\r\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}"
                    , lTransporte.CorretagemNoMes, lTransporte.CadastrosNoMes, lTransporte.CorretagemNoMesAnterior, lTransporte.CadastrosNoMesAnterior, lTransporte.CorretagemNoAno, lTransporte.CadastrosNoAno);
            }

            lConteudoArquivo.Append("\nTop 10 Clientes\n");

            if (null != this.GetDadosTop10Clientes
            && (null != this.GetDadosTop10Clientes.Resultado)
            && (this.GetDadosTop10Clientes.StatusResposta == MensagemResponseStatusEnum.OK))
            {
                var lTransporte = new TransporteRelatorios_002_Top10().TraduzirLista(this.GetDadosTop10Clientes.Resultado);

                lConteudoArquivo.Append("\tCliente\tR$\t% total\tDev. média %\tCustódia (R$)\r");
                lTransporte.ForEach(top =>
                {
                    lConteudoArquivo.AppendFormat("\t{0}\t{1}\t{2}\t{3}\t{4}\t\r\n"
                        , top.NomeCliente, top.Corretagem, top.PercentualTotal, top.PercentualDevMedia, top.Custodia);
                });
            }

            this.Response.Clear();

            this.Response.ContentType = "text/xls";

            this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            this.Response.Charset = "iso-8859-1";

            this.Response.AddHeader("content-disposition", "attachment;filename=ResumoGerencial.xls");

            this.Response.Write(lConteudoArquivo.ToString());

            this.Response.End();
        }

        #endregion

        #region | Métodos de apoio

        private void CarregarDadosCadastro()
        {
            if (this.GetDadosCadastrais.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_002_Cadastro(this.GetDadosCadastrais.EntidadeCadastro);

                if (this.GetCodigoAssessor == null)
                    this.gCadastroNomeAssessor = "TODOS os assessores.";
                else
                {
                    var lDadosAssessor = base.BuscarListaDoSinacor(new Contratos.Dados.SinacorListaInfo() { Informacao = eInformacao.AssessorPadronizado }).Find(ass => { return ass.Id == this.GetCodigoAssessor.ToString(); });

                    this.gCadastroNomeAssessor = lDadosAssessor.Value;
                }

                this.gRelatorioDataInicial = this.GetDataInicial.ToString("dd/MM/yyyy");
                this.gRelatorioDataFinal = this.GetDataFinal.ToString("dd/MM/yyyy");
                this.gCadastroQuantidadeTotal = lTransporte.QuantidadeTotal;
                this.gCadastroQuantidadeAtivos = lTransporte.QuantidadeAtivos;
                this.gCadastroQuantidadeInativos = lTransporte.QuantidadeInativos;
                this.gCadastroQuantidadeVarejo = lTransporte.QuantidadeVarejo;
                this.gCadastroQuantidadeClientesNovos = lTransporte.QuantidadeClientesNovos;
                this.gCadastroQuantidadeInstitucional = lTransporte.QuantidadeInstitucional;
                this.gCadastroPercentualOperouNoMes = lTransporte.PercentualOperouNoMes;
                this.gCadastroPercentualComCustodia = lTransporte.PercentualComCustodia;
                this.gCadastroPercentualTotal = lTransporte.PercentualTotal;
                this.gCadastroPercentualAtivos = lTransporte.PercentualAtivos;
                this.gCadastroPercentualInativos = lTransporte.PercentualInativos;
            }
        }

        private void CarregarDadosReceita()
        {
            if (this.GetDadosReceita.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_002_Receita(this.GetDadosReceita.EntidadeCadastro, this.GetCodigoAssessor);

                this.gReceitaBovespaClientes = lTransporte.BovespaClientes;
                this.gReceitaBovespaValor = lTransporte.BovespaValor;
                this.gReceitaBMFClientes = lTransporte.BMFClientes;
                this.gReceitaBMFValor = lTransporte.BMFValor;
                this.gReceitaBTCClientes = lTransporte.TBCClientes;
                this.gReceitaBTCValor = lTransporte.TBCValor;
                this.gReceitaTesouroClientes = lTransporte.TesouroClientes;
                this.gReceitaTesouroValor = lTransporte.TesouroValor;
                this.gReceitaOutrasClientes = lTransporte.OutrasClientes;
                this.gReceitaOutrasValor = lTransporte.OutrasValor;
                this.gReceitaTotalClientes = lTransporte.TotalClientes;
                this.gReceitaTotalValor = lTransporte.TotalValor;
            }
        }

        private void CarregarDadosCanal()
        {
            if (this.GetDadosCanal.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_002_Canal(this.GetDadosCanal.EntidadeCadastro, this.GetCodigoAssessor);

                this.gCanalHbValor = lTransporte.HbValor;
                this.gCanalHbPercentual = lTransporte.HbPercentual;
                this.gCanalRepassadorValor = lTransporte.RepassadorValor;
                this.gCanalRepassadorPercentual = lTransporte.RepassadorPercentual;
                this.gCanalMesaValor = lTransporte.MesaValor;
                this.gCanalMesaPercentual = lTransporte.MesaPercentual;
                this.gCanalTotalPercentual = lTransporte.TotalPercentual;
                this.gCanalTotalValor = lTransporte.TotalValor;
            }
        }

        private void CarregarDadosMetricas()
        {
            if (this.GetDadosMetricas.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorios_002_Metricas(this.GetDadosMetricas.EntidadeCadastro);

                this.gMetricasCorretagemNoDia = lTransporte.CorretagemNoDia;
                this.gMetricasCorretagemNoMes = lTransporte.CorretagemNoMes;
                this.gMetricasCorretagemNoMesAnterior = lTransporte.CorretagemNoMesAnterior;
                this.gMetricasCorretagemNoAno = lTransporte.CorretagemNoAno;
                this.gMetricasCadastrosNoDia = lTransporte.CadastrosNoDia;
                this.gMetricasCadastrosNoMes = lTransporte.CadastrosNoMes;
                this.gMetricasCadastrosNoMesAnterior = lTransporte.CadastrosNoMesAnterior;
                this.gMetricasCadastrosNoAno = lTransporte.CadastrosNoAno;
            }
        }

        private void CarregarClientesTop10()
        {
            if (this.GetDadosTop10Clientes.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.rptDBM_ResumoAssessor_Top10.DataSource = new TransporteRelatorios_002_Top10().TraduzirLista(this.GetDadosTop10Clientes.Resultado);
                this.rptDBM_ResumoAssessor_Top10.DataBind();
            }
        }

        #endregion
    }
}
