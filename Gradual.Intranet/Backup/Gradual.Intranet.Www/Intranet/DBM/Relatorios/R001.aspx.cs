using System;
using System.Collections.Generic;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Cadastro;

namespace Gradual.Intranet.Www.Intranet.DBM.Relatorios
{
    public partial class R001 : PaginaBaseAutenticada
    {
        #region | Atributos

        //private int _gTamanhoDaParte = 50;
        private int _gTotalClientes = 0;

        private ListaAssessoresVinculadosInfo gListaAssessoresVinculados;

        #endregion

        #region | Propriedades

        private string GetParte
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Request.Form["Parte"]))
                    return string.Empty;

                return this.Request["Parte"];
            }
        }

        private string GetFilial
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.GetIdAssessorFilialLogado.DBToString();

                if (string.IsNullOrWhiteSpace(Request["Filial"]))
                    return "";

                return this.Request["Filial"];
            }
        }

        private string GetMercado
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Request["mercado"]))
                    return "";

                return this.Request["mercado"];
            }
        }

        private int gTotalClientes
        {
            get { return _gTotalClientes; }
            set { _gTotalClientes = value; }
        }

        private int? GetCodigoAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                return null;
            }
        }

        private DateTime GetDataFinal
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(Request["DataFinal"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataInicial
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(Request["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private ListaAssessoresVinculadosInfo GetListaAssessoresVinculados
        {
            get
            {
                if (null == this.gListaAssessoresVinculados)
                    this.gListaAssessoresVinculados = base.ConsultarCodigoAssessoresVinculadosLista(this.GetCodigoAssessor);

                return this.gListaAssessoresVinculados;
            }
        }

        public string CorretagemMes { get; set; }

        public string VolumeMes { get; set; }

        public string CadastroMes { get; set; }

        public string CorretagemMesAnt { get; set; }

        public string VolumeMesAnt { get; set; }

        public string CadastroMesAnt { get; set; }

        public string CorretagemPorPeriodo { get; set; }

        public string VolumePorPeriodo { get; set; }

        public string CadastroPorPeriodo { get; set; }

        public string PorcentOperou { get; set; }

        public string PorcentCustodia { get; set; }

        public string PorcentNaoOperou { get; set; }

        public string MediaCorretagem { get; set; }

        public string MediaCustodia { get; set; }

        public string PorcentagemTop5 { get; set; }

        public string PorcentagemTop10 { get; set; }

        public string PorcentagemTop20 { get; set; }

        private double TotalTop5 { get; set; }

        private double TotalTop10 { get; set; }

        private double TotalTop20 { get; set; }

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
                else if (this.Acao == "BuscarParte")
                {
                    this.Response.Clear();

                    string lResponse = this.ResponderBuscarMaisDados();

                    this.Response.Write(lResponse);

                    this.Response.End();
                }
                else if (this.Acao == "CarregarComoCSV")
                {
                    this.ResponderArquivoCSV();
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
            this.CarregarCamposTela();

            var lRetornoBreakdown = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ResumoGerenteinfo>(
                new ConsultarEntidadeCadastroRequest<ResumoGerenteinfo>()
                {
                    EntidadeCadastro = new ResumoGerenteinfo()
                    {
                        Mercado = this.RetornarTipoMercado(),
                        DataInicial = this.GetDataInicial,
                        DataFinal = this.GetDataFinal,
                        CodigoFilial = Convert.ToInt32(this.GetFilial),
                    }

                });

            var lListaBreakdown = new List<ResumoAssessorInfo>();

            this.FiltrarBreakdownPorAssessor(lRetornoBreakdown.Resultado[0].ListaRessumoAssessor, out lListaBreakdown);

            if (lRetornoBreakdown.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.rptBreakdownAssessor.DataSource = new TransporteRelatorio_001_BreackDownAssessor().TraduzirLista(lListaBreakdown);
                this.rptBreakdownAssessor.DataBind();
            }

            var lRetornoClientes = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<DBMClienteInfo>
                (
                    new ConsultarEntidadeCadastroRequest<DBMClienteInfo>()
                    {
                        EntidadeCadastro = new DBMClienteInfo()
                        {
                            Mercado = this.RetornarTipoMercadoDBM(),
                            CodigoFilial = Convert.ToInt32(this.GetFilial),
                        }
                    }
                );

            if (lRetornoClientes.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.rptClienteTop5.DataSource = new TransporteRelatorio_001_BreackDownAssessor().TraduzirLista(this.ListarTop5(lRetornoClientes.Resultado));
                this.rptClienteTop5.DataBind();

                this.rptClienteTop10.DataSource = new TransporteRelatorio_001_BreackDownAssessor().TraduzirLista(this.ListarTop10(lRetornoClientes.Resultado));
                this.rptClienteTop10.DataBind();

                this.rptClienteTop20.DataSource = new TransporteRelatorio_001_BreackDownAssessor().TraduzirLista(this.ListarTop20(lRetornoClientes.Resultado));
                this.rptClienteTop20.DataBind();

                this.CalcularPorcentagem(lRetornoClientes.Resultado);
            }
        }

        private void FiltrarBreakdownPorAssessor(List<ResumoAssessorInfo> pListaResumoEntrada, out List<ResumoAssessorInfo> pListaResumoSaida)
        {
            pListaResumoSaida = new List<ResumoAssessorInfo>();

            if (null != pListaResumoEntrada && pListaResumoEntrada.Count > 0)
            {
                if (null != this.GetListaAssessoresVinculados)
                {
                    foreach (ResumoAssessorInfo lResumoAssessor in pListaResumoEntrada)
                        if (this.GetListaAssessoresVinculados.ListaCodigoAssessoresVinculados.Contains(lResumoAssessor.CodigoAssessor.DBToInt32()))
                            pListaResumoSaida.Add(lResumoAssessor);
                }
                else
                {
                    pListaResumoSaida = pListaResumoEntrada;
                }
            }
        }

        private List<DBMClienteInfo> ListarTop5(List<DBMClienteInfo> pListaCliente)
        {
            var listaRetorno = new List<DBMClienteInfo>();

            if (null != pListaCliente && pListaCliente.Count > 0)
            {
                var lListaExclusaoDeJaApresentados = new List<int>();

                if (null != this.GetListaAssessoresVinculados)
                {
                    for (int i = 0; i < pListaCliente.Count; i++)
                    {
                        if (this.GetListaAssessoresVinculados.ListaCodigoAssessoresVinculados.Contains(pListaCliente[i].CodigoAssessor))
                        {
                            listaRetorno.Add(pListaCliente[i]);
                            TotalTop5 += pListaCliente[i].Corretagem.DBToDouble();
                            lListaExclusaoDeJaApresentados.Add(i);
                        }

                        if (listaRetorno.Count >= 5)
                        {
                            lListaExclusaoDeJaApresentados.ForEach(lIndice => { this.GetListaAssessoresVinculados.ListaCodigoAssessoresVinculados.RemoveAt(lIndice); });

                            break;
                        }
                    }

                    for (int i = lListaExclusaoDeJaApresentados.Count - 1; i > -1; i--)
                        pListaCliente.RemoveAt(lListaExclusaoDeJaApresentados[i]);
                }
                else
                {
                    int i = 0;
                    while (i < 5 && pListaCliente.Count > i)
                    {
                        listaRetorno.Add(pListaCliente[i]);
                        lListaExclusaoDeJaApresentados.Add(i);
                        TotalTop5 += pListaCliente[i].Corretagem.DBToDouble();
                        i++;
                    }

                    for (int j = lListaExclusaoDeJaApresentados.Count - 1; j > -1; j--)
                        pListaCliente.RemoveAt(lListaExclusaoDeJaApresentados[j]);
                }
            }

            return listaRetorno;
        }

        private List<DBMClienteInfo> ListarTop10(List<DBMClienteInfo> pListaCliente)
        {
            List<DBMClienteInfo> listaRetorno = new List<DBMClienteInfo>();

            if (null != pListaCliente && pListaCliente.Count > 0)
            {
                var lListaExclusaoDeJaApresentados = new List<int>();

                if (null != this.GetListaAssessoresVinculados)
                {

                    for (int i = 0; i < pListaCliente.Count; i++)
                    {
                        if (pListaCliente.Count > i && this.GetListaAssessoresVinculados.ListaCodigoAssessoresVinculados.Contains(pListaCliente[i].CodigoAssessor))
                        {
                            listaRetorno.Add(pListaCliente[i]);
                            TotalTop10 += pListaCliente[i].Corretagem.DBToDouble();
                            lListaExclusaoDeJaApresentados.Add(i);
                        }

                        if (listaRetorno.Count >= 10)
                        {
                            for (int j = lListaExclusaoDeJaApresentados.Count - 1; j > -1; j--)
                                pListaCliente.RemoveAt(lListaExclusaoDeJaApresentados[j]);

                            break;
                        }
                    }

                    for (int i = lListaExclusaoDeJaApresentados.Count - 1; i > -1; i--)
                        pListaCliente.RemoveAt(lListaExclusaoDeJaApresentados[i]);
                }
                else
                {
                    int i = 0;
                    while (i < 10 && pListaCliente.Count > i)
                    {
                        listaRetorno.Add(pListaCliente[i]);
                        lListaExclusaoDeJaApresentados.Add(i);
                        TotalTop10 += pListaCliente[i].Corretagem.DBToDouble();
                        i++;
                    }

                    for (int j = lListaExclusaoDeJaApresentados.Count - 1; j > -1; j--)
                        pListaCliente.RemoveAt(lListaExclusaoDeJaApresentados[j]);
                }
            }

            return listaRetorno;
        }

        private List<DBMClienteInfo> ListarTop20(List<DBMClienteInfo> pListaCliente)
        {
            List<DBMClienteInfo> listaRetorno = new List<DBMClienteInfo>();

            if (null != pListaCliente && pListaCliente.Count > 0)
            {
                if (null != this.GetListaAssessoresVinculados)
                {
                    var lListaExclusaoDeJaApresentados = new List<int>();

                    for (int i = 0; i < pListaCliente.Count; i++)
                    {
                        if (pListaCliente.Count > i && this.GetListaAssessoresVinculados.ListaCodigoAssessoresVinculados.Contains(pListaCliente[i].CodigoAssessor))
                        {
                            listaRetorno.Add(pListaCliente[i]);
                            TotalTop20 += pListaCliente[i].Corretagem.DBToDouble();
                            lListaExclusaoDeJaApresentados.Add(i);
                        }

                        if (listaRetorno.Count >= 20)
                        {
                            for (int j = lListaExclusaoDeJaApresentados.Count - 1; j > -1; j--)
                                pListaCliente.RemoveAt(lListaExclusaoDeJaApresentados[j]);

                            break;
                        }
                    }

                    for (int i = lListaExclusaoDeJaApresentados.Count - 1; i > -1; i--)
                        pListaCliente.RemoveAt(lListaExclusaoDeJaApresentados[i]);
                }
                else
                {
                    int i = 0;
                    while (i < 20 && pListaCliente.Count > i)
                    {
                        listaRetorno.Add(pListaCliente[i]);
                        TotalTop20 += pListaCliente[i].Corretagem.DBToDouble();
                        i++;
                    }
                }
            }

            return listaRetorno;
        }

        private List<DBMClienteInfo> Listar34Primeiros(List<DBMClienteInfo> pListaCliente)
        {
            List<DBMClienteInfo> listaRetorno = new List<DBMClienteInfo>();
            var i = default(int);

            if (null != pListaCliente && pListaCliente.Count > 0)
            {
                if (null != this.GetListaAssessoresVinculados)
                {
                    while (pListaCliente.Count > 0 && pListaCliente.Count > i && i < 35)
                    {
                        if (pListaCliente.Count > i && this.GetListaAssessoresVinculados.ListaCodigoAssessoresVinculados.Contains(pListaCliente[i].CodigoAssessor))
                            listaRetorno.Add(pListaCliente[i]);

                        i++;
                    }
                }
                else
                {
                    while (pListaCliente.Count > 0 && pListaCliente.Count > i && i < 35)
                    {
                        listaRetorno.Add(pListaCliente[i]);
                        i++;
                    }
                }
            }

            return listaRetorno;
        }

        private void CalcularPorcentagem(List<DBMClienteInfo> ListaCliente)
        {
            //decimal totalPorcentagem = 0;

            //foreach (DBMClienteInfo item in ListaCliente)
            //{
            //    totalPorcentagem += item.Corretagem;
            //}

            this.PorcentagemTop5 = this.FormatarDecimal(((TotalTop5 * 100) / CorretagemMes.DBToDouble()));
            this.PorcentagemTop10 = this.FormatarDecimal(((TotalTop10 * 100) / CorretagemMes.DBToDouble()));
            this.PorcentagemTop20 = this.FormatarDecimal(((TotalTop20 * 100) / CorretagemMes.DBToDouble()));
        }

        private string ResponderBuscarMaisDados()
        {
            return string.Empty;
        }

        private ResumoGerenteinfo.TipoMercado RetornarTipoMercado()
        {
            switch (this.GetMercado)
            {
                case "0":
                    return ResumoGerenteinfo.TipoMercado.BMF_BVSP;

                case "1":
                    return ResumoGerenteinfo.TipoMercado.BVSP;

                case "2":
                    return ResumoGerenteinfo.TipoMercado.BMF;

                default:
                    return ResumoGerenteinfo.TipoMercado.BVSP;
            }
        }

        private DBMClienteInfo.TipoMercado RetornarTipoMercadoDBM()
        {
            switch (this.GetMercado)
            {
                case "0":
                    return DBMClienteInfo.TipoMercado.BMF_BVSP;

                case "1":
                    return DBMClienteInfo.TipoMercado.BVSP;

                case "2":
                    return DBMClienteInfo.TipoMercado.BMF;

                default:
                    return DBMClienteInfo.TipoMercado.BVSP;
            }
        }

        private string FormatarDecimal(double numero)
        {
            return numero.ToString("N2");
        }

        private void ResponderArquivoCSV()
        {
            StringBuilder lBuilder = new StringBuilder();

            this.CarregarCamposTela();

            lBuilder.AppendFormat("\tCorretagem\tVolume\tCadastrados\r\n");
            lBuilder.AppendFormat("No Mês\t{0}\t{1}\t{2}\r\n", this.CorretagemMes, this.VolumeMes, this.CadastroMes);
            lBuilder.AppendFormat("Mês Anterior\t{0}\t{1}\t{2}\r\n", this.CorretagemMesAnt, this.VolumeMesAnt, this.CadastroMesAnt);
            lBuilder.AppendFormat("No Período\t{0}\t{1}\t{2}\r\n", this.CorretagemPorPeriodo, this.VolumePorPeriodo, this.CadastroPorPeriodo);

            lBuilder.AppendLine("\t\r\n");

            lBuilder.AppendFormat("Total Clientes\t\r\n");
            lBuilder.AppendFormat("% Operou\t{0}\r\n", this.PorcentOperou);
            lBuilder.AppendFormat("% Com Custódia\t{0}\r\n", this.PorcentCustodia);
            lBuilder.AppendFormat("% Não Opera a 90 dias\t{0}\r\n", this.PorcentNaoOperou);
            lBuilder.AppendFormat("% tCorretagem Média\t{0}\r\n", this.MediaCorretagem);
            lBuilder.AppendFormat("% tCustódia Média\t{0}\r\n", this.MediaCustodia);

            var lRetornoClientes = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<DBMClienteInfo>
               (
                   new ConsultarEntidadeCadastroRequest<DBMClienteInfo>()
                   {
                       EntidadeCadastro = new DBMClienteInfo()
                       {
                           Mercado = this.RetornarTipoMercadoDBM(),
                           CodigoFilial = Convert.ToInt32(this.GetFilial),
                       }
                   }
               );

            if (lRetornoClientes.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                List<TransporteRelatorio_001_BreackDownAssessor> listaCliente = new TransporteRelatorio_001_BreackDownAssessor().TraduzirLista(this.Listar34Primeiros(lRetornoClientes.Resultado));

                lBuilder.AppendLine("\t\r\n");
                lBuilder.AppendFormat("Top Clientes\t\r");
                lBuilder.AppendFormat("Nome\tCorretam\tVolume\t\r");

                foreach (TransporteRelatorio_001_BreackDownAssessor info in listaCliente)
                    lBuilder.AppendFormat("{0}\t{1}\t{2}\t\r\n", info.Nome, info.Corretagem, info.Volume);
            }

            //busca BreakDown
            var lRetornoBreakdown = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ResumoGerenteinfo>(
                new ConsultarEntidadeCadastroRequest<ResumoGerenteinfo>()
                {
                    EntidadeCadastro = new ResumoGerenteinfo()
                    {
                        Mercado = this.RetornarTipoMercado(),
                        DataInicial = this.GetDataInicial,
                        DataFinal = this.GetDataFinal,
                        CodigoFilial = this.GetFilial.DBToDecimal(),
                    }
                });

            if (lRetornoBreakdown.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                List<TransporteRelatorio_001_BreackDownAssessor> listaBreakDown = new TransporteRelatorio_001_BreackDownAssessor().TraduzirLista(lRetornoBreakdown.Resultado[0].ListaRessumoAssessor);
                lBuilder.AppendLine("\t\r\n");
                lBuilder.AppendFormat("Breakdown\t\r");
                lBuilder.AppendFormat("Tipo\tNome\tCodigo Assessor\tVolume\tCorretagem\tCustodia\t\r");

                foreach (TransporteRelatorio_001_BreackDownAssessor info in listaBreakDown)
                    lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\r\n", info.Tipo, info.Nome, info.CodigoAssessor, info.Volume, info.Corretagem, info.Custodia);
            }

            this.Response.Clear();

            this.Response.ContentType = "text/xls";

            this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            this.Response.Charset = "iso-8859-1";

            this.Response.AddHeader("content-disposition", "attachment;filename=RelatorioDBM001.xls");

            this.Response.Write(lBuilder.ToString());

            this.Response.End();
        }

        private void CarregarCamposTela()
        {
            var lRetornoMes = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ResumoGerenteinfo>(
              new ReceberEntidadeCadastroRequest<ResumoGerenteinfo>()
              {
                  EntidadeCadastro = new ResumoGerenteinfo()
                  {
                      Mercado = this.RetornarTipoMercado(),
                      CodigoFilial = Convert.ToInt32(this.GetFilial),
                  }
              });

            if (lRetornoMes.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.CorretagemMes = this.FormatarDecimal(lRetornoMes.EntidadeCadastro.CorretagemMes.DBToDouble());
                this.VolumeMes = this.FormatarDecimal(lRetornoMes.EntidadeCadastro.VolumeMes.DBToDouble());
                this.CadastroMes = this.FormatarDecimal(lRetornoMes.EntidadeCadastro.CadastradoMes.DBToDouble());
            }

            var lRetornoMesAnterior = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ResumoGerenteMesAnteriorInfo>(
                new ReceberEntidadeCadastroRequest<ResumoGerenteMesAnteriorInfo>()
                {
                    EntidadeCadastro = new ResumoGerenteMesAnteriorInfo()
                    {
                        Mercado = this.RetornarTipoMercado(),
                        CodigoFilial = Convert.ToInt32(this.GetFilial),
                    }
                });

            if (lRetornoMesAnterior.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.VolumeMesAnt = this.FormatarDecimal(lRetornoMesAnterior.EntidadeCadastro.VolumeMesAnterior.DBToDouble());
                this.CadastroMesAnt = this.FormatarDecimal(lRetornoMesAnterior.EntidadeCadastro.CadastradoMesAnterior.DBToDouble());
                this.CorretagemMesAnt = this.FormatarDecimal(lRetornoMesAnterior.EntidadeCadastro.CorretagemMesAnterior.DBToDouble());
            }

            var lRetornoPorPeriodo = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ResumoGerentePeriodoinfo>(
                new ReceberEntidadeCadastroRequest<ResumoGerentePeriodoinfo>()
                {
                    EntidadeCadastro = new ResumoGerentePeriodoinfo()
                    {
                        Mercado = this.RetornarTipoMercado(),
                        DataInicial = this.GetDataInicial,
                        DataFinal = this.GetDataFinal,
                        CodigoFilial = Convert.ToInt32(this.GetFilial),
                    }
                });

            if (lRetornoPorPeriodo.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.VolumePorPeriodo = this.FormatarDecimal(lRetornoPorPeriodo.EntidadeCadastro.VolumeIntervaloData.DBToDouble());
                this.CadastroPorPeriodo = this.FormatarDecimal(lRetornoPorPeriodo.EntidadeCadastro.CadastradoIntervaloData.DBToDouble());
                this.CorretagemPorPeriodo = this.FormatarDecimal(lRetornoPorPeriodo.EntidadeCadastro.CorretagemIntervaloData.DBToDouble());
            }

            //busca dados de clientes
            var lRetornoCliente = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ResumoGerenteClienteInfo>(
                new ReceberEntidadeCadastroRequest<ResumoGerenteClienteInfo>()
                {
                    EntidadeCadastro = new ResumoGerenteClienteInfo()
                    {
                        CodigoFilial = Convert.ToInt32(this.GetFilial),
                        Mercado = this.RetornarTipoMercado(),
                    }
                });

            if (lRetornoCliente.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.PorcentNaoOperou = this.FormatarDecimal(lRetornoCliente.EntidadeCadastro.ClienteNaoOperaramNoventaDia.DBToDouble());
                this.PorcentCustodia = this.FormatarDecimal(lRetornoCliente.EntidadeCadastro.PorcentagemClienteCustodia.DBToDouble());
                this.PorcentOperou = this.FormatarDecimal(lRetornoCliente.EntidadeCadastro.Porcentagemclientes.DBToDouble());
                this.MediaCorretagem = this.FormatarDecimal(lRetornoCliente.EntidadeCadastro.MediaCorretagem.DBToDouble());
                this.MediaCustodia = this.FormatarDecimal(lRetornoCliente.EntidadeCadastro.MediaCustodia.DBToDouble());
            }
        }

        #endregion
    }
}
