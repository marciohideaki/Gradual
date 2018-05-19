using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Compliance;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Intranet.Contratos.Dados.Compliance;
using Newtonsoft.Json;
using System.Text;

namespace Gradual.Intranet.Www.Intranet.Compliance.Formularios
{
    public partial class Churning : PaginaBase
    {
        #region Propriedades
        private List<TransporteChurningIntraday> SessaoUltimaConsulta
        {
            get
            {
                return Session["SessaoUltimaConsulta"] as List<TransporteChurningIntraday>;
            }
            set
            {
                Session["SessaoUltimaConsulta"] = value;
            }
        }

        private string GetFiltrarPor
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["sidx"]))
                    return this.Request["sidx"];

                return null;
            }
        }

        private string GetOrdenacao
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["sord"]))
                    return this.Request["sord"];

                return null;
            }
        }

        public string PeriodoFiltro { get; set; }

        public DateTime GetDataDe
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["DataDe"]))
                    return DateTime.Now.AddDays(-30);

                return DateTime.Parse(this.Request["DataDe"]).Date;
            }
        }

        public DateTime GetDataAte
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["DataAte"]))
                    return DateTime.Now.AddDays(1);

                return DateTime.Parse(this.Request["DataAte"] ).Date;
            }
        }

        private int? GetCdAssessor
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodAssessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCdCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodigoCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        public enumPercentualCE GetPercentualCE
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["PercentualCE"]))
                    return enumPercentualCE.TODOS;

                return (enumPercentualCE)Enum.Parse(typeof(enumPercentualCE), this.Request["PercentualCE"]);
            }
        }
        public enumPercentualTR GetPercentualTR
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["PercentualTR"]))
                    return enumPercentualTR.TODOS;

                return (enumPercentualTR)Enum.Parse(typeof(enumPercentualTR), this.Request["PercentualTR"]);
            }
        }

        public enumCarteiraMedia GetCarteiraMedia
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["CarteiraMedia"]))
                    return enumCarteiraMedia.TODOS;

                return (enumCarteiraMedia)Enum.Parse(typeof(enumCarteiraMedia), this.Request["CarteiraMedia"]);
            }
        }

        public enumTotalCompras GetTotalCompras
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["TotalCompras"]))
                    return enumTotalCompras.TODOS;

                return (enumTotalCompras)Enum.Parse(typeof(enumTotalCompras), this.Request["TotalCompras"]);
            }
        }

        public enumTotalVendas GetTotalVendas
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["TotalVendas"]))
                    return enumTotalVendas.TODOS;

                return (enumTotalVendas)Enum.Parse(typeof(enumTotalVendas), this.Request["TotalVendas"]);
            }
        }
        public int? GetPorta
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["Porta"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                     , "BuscarChurning"
                                                     , "ExportarExcell"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderExportarExcell
                                                     });
        }
        #endregion

        #region Métodos
        private string ResponderExportarExcell()
        {
            ChurningIntradayInfo lRequest = Session["Sessao_Filtro"] as ChurningIntradayInfo;

            ChurningIntradayDbLib lServico = new ChurningIntradayDbLib();

            TransporteDeListaPaginada lRetornoLista = null;

            var lListaTransporte = new List<TransporteChurningIntraday>();

            var lRetornoConsulta = lServico.ObterMonitoramentoIntradiario(lRequest);

            if (lRetornoConsulta != null && lRetornoConsulta.Resultado != null)
            {
                lListaTransporte = new TransporteChurningIntraday().TraduzirLista(lRetornoConsulta.Resultado);

                lRetornoLista = new TransporteDeListaPaginada(lListaTransporte);
            }

            StringBuilder lContent = new StringBuilder();

            lContent.AppendFormat("Data de consulta: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")+  "\t \t \t \t \t \t \t \t \t \t Período De: {0} até: {1} \t\r", this.GetDataDe, this.GetDataAte);

            lContent.Append("Cliente" + "\t");
            lContent.Append("Nome Cliente" + "\t");
            lContent.Append("Assessor" + "\t");
            lContent.Append("Nome Assessor" + "\t");

            lContent.Append("% TR do dia" + "\t");
            lContent.Append("% TR no Período" + "\t");
            lContent.Append("Total de Compras" + "\t");

            lContent.Append("Corretagem" + "\t");

            lContent.Append("% CE no dia" + "\t");
            lContent.Append("% CE no Período" + "\t");
            lContent.Append("Total de Vendas" + "\t");

            lContent.Append("Carteira Média" + "\t");
            lContent.Append("Carteira Dia" + "\t");
            lContent.Append("L1" + "\t");
            lContent.Append("Data" + "\t");
            lContent.Append("Portas" + "\t");
            lContent.Append("Tipo Pessoa\r");

            lListaTransporte.ForEach(chu =>
                {
                    lContent.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\r",
                        chu.CodigoCliente,
                        chu.NomeCliente,
                        chu.CodigoAssessor,
                        chu.NomeAssessor,
                        chu.PercentualTRnoDia,
                        chu.PercentualTRnoPeriodo,
                        chu.ValorCompras,
                        chu.Corretagem,
                        chu.PercentualCEnoDia,
                        chu.PercentualCEnoPeriodo,
                        chu.ValorVendas,
                        chu.CarteiraMedia,
                        chu.ValorCarteiraDia,
                        chu.ValorL1,
                        chu.Data,
                        chu.Portas,
                        chu.TipoPessoa
                        );
                });

            this.Response.Clear();

            this.Response.ContentType = "text/xls";

            this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            this.Response.Charset = "iso-8859-1";

            this.Response.AddHeader("content-disposition", "attachment;filename=Churning.xls");

            this.Response.Write(lContent.ToString());

            this.Response.End();

            return string.Empty;
        }

        private string ResponderBuscarItensParaListagemSimples()
        {
            ChurningIntradayDbLib lServico = new ChurningIntradayDbLib();

            string lRetorno = string.Empty;

            string lColunas = string.Empty;

            ChurningIntradayInfo lRequest = new ChurningIntradayInfo();

            TransporteDeListaPaginada lRetornoLista = null;

            if (Session["Usuario"] == null) { return string.Empty; }

            if (null != this.GetCdCliente)
            {
                lRequest.CodigoCliente = this.GetCdCliente.Value;
            }

            if (null != this.GetCdAssessor)
            {
                lRequest.CodigoAssessor = this.GetCdAssessor.Value;
            }

            if (base.CodigoAssessor != null)
            {
                lRequest.CodigoAssessor = base.CodigoAssessor.Value;
                lRequest.CodigoLogin = this.UsuarioLogado.Id;
            }

            if (this.GetPorta != null)
            {
                lRequest.Porta = this.GetPorta.Value.ToString();
            }

            lRequest.enumPercentualCE   = this.GetPercentualCE;

            lRequest.enumPercentualTR   = this.GetPercentualTR;

            lRequest.enumTotalCompras   = this.GetTotalCompras;

            lRequest.enumTotalVendas    = this.GetTotalVendas;

            lRequest.enumCarteiraMedia  = this.GetCarteiraMedia;

            lRequest.ListaFeriados = base.ListaFeriados;

            var lFeriados = base.ListaFeriados;

            var lDataInicial = this.GetDataDe;

            var lDataFinal = this.GetDataAte;

            bool lEFeriado = lFeriados.Contains(lDataInicial);

            bool lEhDiaUtilIntervaloValido = false;

            int lDiasUteis = 0;

            if (ValidaDataDiaUtil(lDataInicial))
            {
                lRetorno = base.RetornarErroAjax("A data inicial deve ser um dia útil");

                return lRetorno;
            }

            if (ValidaDataDiaUtil(lDataFinal))
            {
                lRetorno = base.RetornarErroAjax("A data final deve ser um dia útil");

                return lRetorno;
            }

            //Primeiro elimina sabados domigos e feriados do intervalo 
            while (lDiasUteis < 1)
            {
                lDataInicial = lDataInicial.AddDays(1);

                lEhDiaUtilIntervaloValido = lDataInicial.DayOfWeek == DayOfWeek.Saturday ||
                    lDataInicial.DayOfWeek == DayOfWeek.Sunday || lFeriados.Contains(lDataInicial);

                if (!lEhDiaUtilIntervaloValido)
                    lDiasUteis++;
            }

            lDiasUteis = 0;

            lEhDiaUtilIntervaloValido = false;

            //Primeiro elimina sabados domigos e feriados do intervalo 
            while (lDiasUteis < 1)
            {
                lDataFinal = lDataFinal.AddDays(1);

                lEhDiaUtilIntervaloValido = lDataFinal.DayOfWeek == DayOfWeek.Saturday ||
                    lDataFinal.DayOfWeek == DayOfWeek.Sunday || lFeriados.Contains(lDataFinal);

                if (!lEhDiaUtilIntervaloValido)
                    lDiasUteis++;
            }

            lRequest.DataDe = lDataInicial;

            lRequest.DataAte = lDataFinal;

            this.PeriodoFiltro = "De : " + lRequest.DataDe + " Até : " + lRequest.DataAte;

            ChurningIntradayInfo lRetornoConsulta = new ChurningIntradayInfo();

            Session["Sessao_Filtro"] = lRequest;

            lRetornoConsulta = lServico.ObterMonitoramentoIntradiario(lRequest);

            if (lRetornoConsulta != null && lRetornoConsulta.Resultado != null)
            {
                List<TransporteChurningIntraday> lListaTransporte = new TransporteChurningIntraday().TraduzirLista(lRetornoConsulta.Resultado);

                this.SessaoUltimaConsulta = lListaTransporte;

                lRetornoLista = new TransporteDeListaPaginada(lListaTransporte);

                this.ResponderFiltrarPorColuna();

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = lRetornoConsulta.Resultado.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0;

                return lRetorno;
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requisição");
            }

            return lRetorno;
        }

        private string ResponderFiltrarPorColuna()
        {
            switch (this.GetFiltrarPor)
            {

                case "CodigoCliente":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.CodigoCliente), int.Parse(lp2.CodigoCliente)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.CodigoCliente), int.Parse(lp1.CodigoCliente)));
                    }
                    break;

                case "NomeCliente":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.NomeCliente, lp2.NomeCliente));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.NomeCliente, lp1.NomeCliente));
                    }
                    break;
                
                case "Tipo":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.TipoPessoa, lp2.TipoPessoa));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.TipoPessoa, lp1.TipoPessoa));
                    }
                    break;
                case "CodigoAssessor":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.CodigoAssessor), int.Parse(lp2.CodigoAssessor)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.CodigoAssessor), int.Parse(lp1.CodigoAssessor)));
                    }
                    break;

                case "NomeAssessor":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.NomeAssessor, lp2.NomeAssessor));
                        
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.NomeAssessor, lp1.NomeAssessor));
                        
                    }
                    break;

                case "PercentualTRnoDia":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PercentualTRnoDia), decimal.Parse(lp2.PercentualTRnoDia)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PercentualTRnoDia), decimal.Parse(lp1.PercentualTRnoDia)));
                    }
                    break;

                case "PercentualTRnoPeriodo":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PercentualTRnoPeriodo), decimal.Parse(lp2.PercentualTRnoPeriodo)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PercentualTRnoPeriodo), decimal.Parse(lp1.PercentualTRnoPeriodo)));
                    }
                    break;

                case "ValorCompras":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.ValorCompras), decimal.Parse(lp2.ValorCompras)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.ValorCompras), decimal.Parse(lp1.ValorCompras)));
                    }
                    break;

                case "PercentualCEnoDia":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PercentualCEnoDia), decimal.Parse(lp2.PercentualCEnoDia)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PercentualCEnoDia), decimal.Parse(lp1.PercentualCEnoDia)));
                    }
                    break;

                case "PercentualCEnoPeriodo":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PercentualCEnoPeriodo), decimal.Parse(lp2.PercentualCEnoPeriodo)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PercentualCEnoPeriodo), decimal.Parse(lp1.PercentualCEnoPeriodo)));
                    }
                    break;
                case "ValorVendas":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.ValorVendas), decimal.Parse(lp2.ValorVendas)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.ValorVendas), decimal.Parse(lp1.ValorVendas)));
                    }
                    break;

                case "ValorCarteira":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.ValorCarteira), decimal.Parse(lp2.ValorCarteira)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.ValorCarteira), decimal.Parse(lp1.ValorCarteira)));
                    }
                    break;

                    
                case "ValorCarteiraDia":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.ValorCarteiraDia), decimal.Parse(lp2.ValorCarteiraDia)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.ValorCarteiraDia), decimal.Parse(lp1.ValorCarteiraDia)));
                    }
                    break;
                case "L1":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.ValorL1), decimal.Parse(lp2.ValorL1)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.ValorL1), decimal.Parse(lp1.ValorL1)));
                    }
                    break;

                case "Data":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp1.Data), DateTime.Parse(lp2.Data)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp2.Data), DateTime.Parse(lp1.Data)));
                    }
                    break;

                case "Portas":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.Portas, lp2.Portas));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.Portas, lp1.Portas));
                    }
                    break;

               
            }

            return base.RetornarSucessoAjax(this.SessaoUltimaConsulta, "sucesso");
        }

        private bool ValidaDataDiaUtil(DateTime Dia)
        {
            bool lEhDiaUtilIntervaloValido = Dia.DayOfWeek == DayOfWeek.Saturday ||
                    Dia.DayOfWeek == DayOfWeek.Sunday || base.ListaFeriados.Contains(Dia);
            
            return lEhDiaUtilIntervaloValido;
        }

        #endregion
    }


}