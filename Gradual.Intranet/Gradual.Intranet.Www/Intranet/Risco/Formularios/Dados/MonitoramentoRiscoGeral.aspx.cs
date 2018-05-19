using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Monitores.Risco.Info;
using Gradual.Monitores.Risco.Lib;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using log4net;
using Newtonsoft.Json;
using ext = Gradual.Intranet.Www.App_Codigo.Extensions;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.OMS.Persistencia;


namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class MonitoramentoLucrosPrejuizos : PaginaBase
    {
        #region | Atributos
        private CultureInfo gCultura = new CultureInfo("pt-BR");
        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.LucrosPrejuizosInfo> gMonitoramentoLucrosPrejuizos;
        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesClienteInfo> gMonitoramentoOperacoesCliente;
        
        
        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo> gMonitoramentoOperacoesDetalhesCliente;
        private static List<TransporteCustodiaInfo> gMonitoramentoCustodia;
        private static TransporteRiscoMonitoramentoLucrosPrejuizos.MonitoramentoContaCorrenteLimiteGeral gMonitoramentoContaCorrenteLimite;
        
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #endregion

        #region | Propriedades

        private string GetColunas
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request["Colunas"]))
                    lRetorno = this.Request["Colunas"];

                return lRetorno;
            }
        }
        
        private int? GetIdJanela
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["idJanela"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private string GetConsulta
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request.Form.ToString()))
                    lRetorno = System.Web.HttpUtility.UrlDecode(this.Request.Form.ToString());

                return lRetorno;
            }
        }

        private string GetNomePagina
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request["NomePagina"]))
                    lRetorno = this.Request["NomePagina"];

                return lRetorno;
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
        
        private int? GetPaginacao
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodigoCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCdClienteBmf
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodigoClienteBmf"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCdGrupoAlavancagem
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["GrupoAlavancagem"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private string GetOrigem
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request["Origem"]))
                    lRetorno = this.Request["Origem"];

                return lRetorno;
            }
        }

        private string GetLucroPrejuizo
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request["LucroPrejuizo"]))
                    lRetorno = this.Request["LucroPrejuizo"];

                return lRetorno;
            }
        }

        private string GetPerda
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request["Perda"]))
                    lRetorno = this.Request["Perda"];

                return lRetorno;
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

        private string GetClienteSelecionado
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["ClienteSelecionado"]))
                    return this.Request["ClienteSelecionado"];

                return string.Empty;
            }
        }

        private string GetAssessorSelecionado
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["AssessorSelecionado"]))
                    return this.Request["AssessorSelecionado"];

                return string.Empty;
            }
        }

        private string GetPerdaSelecionada
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["PerdaSelecionada"]))
                    return this.Request["PerdaSelecionada"];

                return this.Request["PerdaSelecionada"];
            }
        }

        private string GetOrigemSelecionada
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["OrigemSelecionada"]))
                    return this.Request["OrigemSelecionada"];

                return this.Request["OrigemSelecionada"];
            }
        }

        private string GetLucroPrejuizoSelecionado
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["LucroPrejuizoSelecionado"]))
                    return this.Request["LucroPrejuizoSelecionado"];

                return this.Request["LucroPrejuizoSelecionado"];
            }
        }

        private string GetInstrumento
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["Instrumento"]))
                    return this.Request["Instrumento"];

                return this.Request["Instrumento"];
            }
        }

        public string GetMercado
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["Mercado"]))
                    return null;

                return this.Request["Mercado"];
            }
        }

        public Monitores.Risco.Enum.EnumProporcaoPrejuiso GetProporcaoPrejuizo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["ProporcaoPrejuizo"]) || int.Parse(this.Request["ProporcaoPrejuizo"]) == 0)
                    return Monitores.Risco.Enum.EnumProporcaoPrejuiso.SEMINFORMACAO;

                return (Monitores.Risco.Enum.EnumProporcaoPrejuiso)Enum.Parse(typeof(Monitores.Risco.Enum.EnumProporcaoPrejuiso), this.Request["ProporcaoPrejuizo"]);
            }
        }

        public Monitores.Risco.Enum.EnumSemaforo GetSemaforo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["Semaforo"]))
                    return Monitores.Risco.Enum.EnumSemaforo.SEMINFORMACAO;

                return (Monitores.Risco.Enum.EnumSemaforo)Enum.Parse(typeof(Monitores.Risco.Enum.EnumSemaforo), this.Request["Semaforo"]);
            }
        }

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.LucrosPrejuizosInfo> SessaoUltimaConsulta
        {
            get { return gMonitoramentoLucrosPrejuizos != null ? gMonitoramentoLucrosPrejuizos : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.LucrosPrejuizosInfo>(); }
            set { gMonitoramentoLucrosPrejuizos = value; }
        }

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesClienteInfo> SessaoUltimaConsultaOperacoesCliente
        {
            get { return gMonitoramentoOperacoesCliente != null ? gMonitoramentoOperacoesCliente : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesClienteInfo>(); }
            set { gMonitoramentoOperacoesCliente = value; }
        }

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo> SessaoUltimaConsultaOperacoesClienteDetalhe
        {
            get { return gMonitoramentoOperacoesDetalhesCliente != null ? gMonitoramentoOperacoesDetalhesCliente : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo>(); }
            set { gMonitoramentoOperacoesDetalhesCliente = value; }
        }

        private List<TransporteCustodiaInfo> SessaoUltimaConsultaCustodia
        {
            get { return gMonitoramentoCustodia != null ? gMonitoramentoCustodia : new List<TransporteCustodiaInfo>(); }
            set { gMonitoramentoCustodia = value; }
        }

        private TransporteRiscoMonitoramentoLucrosPrejuizos.MonitoramentoContaCorrenteLimiteGeral SessaoUltimaContaCorrenteLimite
        {
            get { return gMonitoramentoContaCorrenteLimite != null ? gMonitoramentoContaCorrenteLimite : new TransporteRiscoMonitoramentoLucrosPrejuizos.MonitoramentoContaCorrenteLimiteGeral(); }
            set { gMonitoramentoContaCorrenteLimite = value; }
        }

        private static string DsConsulta;
        
        private string SituacaoFinanceiraPatrimonial { get; set; }

        #endregion

        #region | Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                     , "CarregarHtmlComDados"
                                                     , "SalvarParametros"
                                                     , "ExluirJanela"
                                                     , "SelecionaColunasInvisiveis"
                                                     , "BuscarVolumeOperacoes"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderSalvarJanela
                                                     , this.ResponderExcluirJanela
                                                     , this.ResponderSelecionaColunasInvisiveis
                                                     , this.ResponderBuscarVolumeOperacoes
                                                     });
        }

        #endregion

        #region | Métodos Servico Risco
        private string ResponderBuscarVolumeOperacoes()
        {
            string lRetorno;

            List<ExposicaoClienteInfo> listMonitor = Session["Monitor_" + base.UsuarioLogado.Id] as List<ExposicaoClienteInfo>;

            decimal lVolumeBmf = 0.0M;

            decimal lVolumeBovespa = 0.0M;

            listMonitor.ForEach(monitor =>
            {
                lVolumeBmf += monitor.VolumeTotalFinanceiroBmf;

                lVolumeBovespa += monitor.VolumeTotalFinanceiroBov;
            });

            TransporteMonitoramentoRiscoVolumes lTransporte = new TransporteMonitoramentoRiscoVolumes(lVolumeBmf, lVolumeBovespa);

            lRetorno =  base.RetornarSucessoAjax(lTransporte, "Dados da página listados com sucesso!");

            return lRetorno;
        }
        private string ResponderSelecionaColunasInvisiveis()
        {
            string lRetorno = string.Empty;

            TransporteParametrosMonitoramentoRiscoColunas lTransporte = new TransporteParametrosMonitoramentoRiscoColunas();

            var lRequest = new ReceberEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo>();

            lRequest.Objeto = new MonitoramentoRiscoLucroPrejuizoParametrosInfo();

            lRequest.Objeto.IdJanela = GetIdJanela.Value;

            var lReponse = new PersistenciaDbIntranet().ReceberObjeto<MonitoramentoRiscoLucroPrejuizoParametrosInfo>(lRequest);

            lTransporte = new TransporteParametrosMonitoramentoRiscoColunas(lReponse.Objeto);

            lRetorno = base.RetornarSucessoAjax(lTransporte, "Dados da página listados com sucesso!");

            return lRetorno;
        }

        private string ResponderExcluirJanela()
        {
            string lRetorno = string.Empty;

            RemoverEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo> lRequest = new RemoverEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo>();

            lRequest.Objeto = new MonitoramentoRiscoLucroPrejuizoParametrosInfo();

            if (GetIdJanela.HasValue)
            {
                lRequest.Objeto.IdJanela = this.GetIdJanela.Value;
            }

            RemoverObjetoResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo> lResponse = new PersistenciaDbIntranet().RemoverObjeto<MonitoramentoRiscoLucroPrejuizoParametrosInfo>(lRequest);

            lRetorno = base.RetornarSucessoAjax(lRequest, "Sucesso");

            return lRetorno;
        }

        private string ResponderSalvarJanela()
        {
            string lRetorno = string.Empty;

            if (Session["Usuario"] == null) { return lRetorno; };

            SalvarObjetoRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo> lRequest = new SalvarObjetoRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo>();

            lRequest.Objeto = new MonitoramentoRiscoLucroPrejuizoParametrosInfo();

            lRequest.Objeto.Colunas = GetColunas;

            lRequest.Objeto.Consulta = DsConsulta;

            lRequest.Objeto.IdUsuario = base.UsuarioLogado.Id;

            lRequest.Objeto.NomeJanela = GetNomePagina;

            //if (GetIdJanela.HasValue)
            //{
            //    lRequest.Objeto.IdJanela = this.GetIdJanela.Value;
            //}

            SalvarObjetoResponse<MonitoramentoRiscoLucroPrejuizoParametrosInfo> lResponse =  new PersistenciaDbIntranet().SalvarObjeto<MonitoramentoRiscoLucroPrejuizoParametrosInfo>(lRequest);

            lRetorno = base.RetornarSucessoAjax(lResponse, "Sucesso");

            return lRetorno;

        }

        private void AplicarFiltrosDePesquisa()
        {
            //if (!string.IsNullOrWhiteSpace(this.GetLucroPrejuizo))
            //{
            //    if ("luc".EndsWith(this.GetLucroPrejuizo))
            //        this.SessaoUltimaConsulta = this.SessaoUltimaConsulta.FindAll(mlp => { return mlp.LucroPrejuizo.DBToDecimal() >= 0; });
            //    else
            //        this.SessaoUltimaConsulta = this.SessaoUltimaConsulta.FindAll(mlp => { return mlp.LucroPrejuizo.DBToDecimal() <= 0; });
            //}

            //if (!string.IsNullOrWhiteSpace(this.GetPerda))
            //    this.SessaoUltimaConsulta = this.SessaoUltimaConsulta.FindAll(mlp => { return this.GetPerda.Equals(mlp.Perda); });
        }
        
        private string ResponderFiltrarPorColuna()
        {
            switch (this.GetFiltrarPor)
            {
                case "Codigo":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(  int.Parse(lp1.Codigo), int.Parse(lp2.Codigo) ));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.Codigo),int.Parse(lp1.Codigo)));
                    }
                    break;

                case "NOM":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.Cliente, lp2.Cliente));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.Cliente, lp1.Cliente));
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
                case "Assessor":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.Assessor), int.Parse(lp2.Assessor)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.Assessor), int.Parse(lp1.Assessor)));
                    }
                    break;

                case "ContaCorrenteAbertura":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.ContaCorrenteAbertura), decimal.Parse(lp2.ContaCorrenteAbertura)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.ContaCorrenteAbertura), decimal.Parse(lp1.ContaCorrenteAbertura)));
                    }
                    break;

                case "CustodiaAbertura":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.CustodiaAbertura), decimal.Parse(lp2.CustodiaAbertura)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.CustodiaAbertura ), decimal.Parse(lp1.CustodiaAbertura )));
                    }
                    break;

                case "DtAtualizacao":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare( DateTime.Parse( lp1.DtAtualizacao), DateTime.Parse(lp2.DtAtualizacao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp2.DtAtualizacao), DateTime.Parse(lp1.DtAtualizacao)));
                    }
                    break;

                case "LucroPrejuizoBMF":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.LucroPrejuizoBMF ), decimal.Parse(lp2.LucroPrejuizoBMF )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.LucroPrejuizoBMF ), decimal.Parse(lp1.LucroPrejuizoBMF )));
                    }
                    break;

                case "LucroPrejuizoBOVESPA":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.LucroPrejuizoBOVESPA ), decimal.Parse(lp2.LucroPrejuizoBOVESPA )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.LucroPrejuizoBOVESPA ), decimal.Parse(lp1.LucroPrejuizoBOVESPA )));
                    }
                    break;

                case "LucroPrejuizoTOTAL":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.LucroPrejuizoTOTAL ), decimal.Parse(lp2.LucroPrejuizoTOTAL )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.LucroPrejuizoTOTAL ), decimal.Parse(lp1.LucroPrejuizoTOTAL )));
                    }
                    break;

                case "NetOperacoes":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.NetOperacoes ), decimal.Parse(lp2.NetOperacoes )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.NetOperacoes ), decimal.Parse(lp1.NetOperacoes )));
                    }
                    break;

                case "PatrimonioLiquidoTempoReal":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PatrimonioLiquidoTempoReal ), decimal.Parse(lp2.PatrimonioLiquidoTempoReal )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PatrimonioLiquidoTempoReal ), decimal.Parse(lp1.PatrimonioLiquidoTempoReal )));
                    }
                    break;

                case "PLAberturaBMF":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PLAberturaBMF ), decimal.Parse(lp2.PLAberturaBMF )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PLAberturaBMF ), decimal.Parse(lp1.PLAberturaBMF )));
                    }
                    break;

                case "PLAberturaBovespa":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PLAberturaBovespa ), decimal.Parse(lp2.PLAberturaBovespa )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PLAberturaBovespa ), decimal.Parse(lp1.PLAberturaBovespa )));
                    }
                    break;

                case "SaldoBMF":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SaldoBMF ), decimal.Parse(lp2.SaldoBMF )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SaldoBMF ), decimal.Parse(lp1.SaldoBMF )));
                    }
                    break;

                case "SaldoContaMargem":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SaldoContaMargem ), decimal.Parse(lp2.SaldoContaMargem )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SaldoContaMargem ), decimal.Parse(lp1.SaldoContaMargem )));
                    }
                    break;

                case "TotalContaCorrenteTempoReal":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.TotalContaCorrenteTempoReal ), decimal.Parse(lp2.TotalContaCorrenteTempoReal )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.TotalContaCorrenteTempoReal ), decimal.Parse(lp1.TotalContaCorrenteTempoReal )));
                    }
                    break;

                case "TotalGarantias":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.TotalGarantias ), decimal.Parse(lp2.TotalGarantias )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.TotalGarantias ), decimal.Parse(lp1.TotalGarantias )));
                    }
                    break;

                case "LimiteAVista":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.LimiteAVista ), decimal.Parse(lp2.LimiteAVista )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.LimiteAVista ), decimal.Parse(lp1.LimiteAVista )));
                    }
                    break;

                case "LimiteDisponivel":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.LimiteDisponivel ), decimal.Parse(lp2.LimiteDisponivel )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.LimiteDisponivel ),decimal.Parse( lp1.LimiteDisponivel )));
                    }
                    break;

                case "LimiteOpcoes":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.LimiteOpcoes ), decimal.Parse(lp2.LimiteOpcoes )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.LimiteOpcoes ), decimal.Parse(lp1.LimiteOpcoes )));
                    }
                    break;

                case "LimiteTotal":
                    if ( this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.LimiteTotal ), decimal.Parse(lp2.LimiteTotal )));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.LimiteTotal), decimal.Parse(lp1.LimiteTotal)));
                    }
                    break;
                case "VolumeTotalFinaceiroBmf":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.VolumeTotalFinaceiroBmf), decimal.Parse(lp2.VolumeTotalFinaceiroBmf)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.VolumeTotalFinaceiroBmf), decimal.Parse(lp1.VolumeTotalFinaceiroBmf)));
                    }
                    
                    break;
                case "VolumeTotalFinaceiroBov":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.VolumeTotalFinaceiroBov), decimal.Parse(lp2.VolumeTotalFinaceiroBov)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.VolumeTotalFinaceiroBov), decimal.Parse(lp1.VolumeTotalFinaceiroBov)));
                    }
                    break;
            }

            return base.RetornarSucessoAjax(this.SessaoUltimaConsulta, "sucesso");
        }

        private string ResponderBuscarItensParaListagemSimples()
        {
            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            string lRetorno = string.Empty;

            string lColunas = string.Empty;

            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            TransporteDeListaPaginadaMonitoramentoRisco lRetornoLista = new TransporteDeListaPaginadaMonitoramentoRisco();

            if (Session["Usuario"] == null) { return string.Empty; }

            if (null != this.GetCdCliente)
            {
                lRequest.Cliente= this.GetCdCliente.Value;
            }

            if (null != this.GetCdAssessor)
            {
                lRequest.Assessor = this.GetCdAssessor.Value;
            }

            if (base.CodigoAssessor != null)
            {
                lRequest.Assessor   = base.CodigoAssessor.Value;
                lRequest.CodigoLogin = this.UsuarioLogado.Id;
            }

            lRequest.Semaforo =  this.GetSemaforo;

            lRequest.ProporcaoPrejuiso = this.GetProporcaoPrejuizo;

            DsConsulta = this.GetConsulta;

            if (GetIdJanela.HasValue)
            {
                var lReponse = new PersistenciaDbIntranet().ReceberObjeto<MonitoramentoRiscoLucroPrejuizoParametrosInfo>
                    (new ReceberEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo>()
                {
                    Objeto = new MonitoramentoRiscoLucroPrejuizoParametrosInfo() 
                    {
                        IdJanela = GetIdJanela.Value
                    }
                });


                DsConsulta = lReponse.Objeto.Consulta;

                lColunas = lReponse.Objeto.Colunas;

                lRequest = this.SeparaConsultaGet(DsConsulta);
            }

            MonitorLucroPrejuizoResponse lRetornoConsulta = new MonitorLucroPrejuizoResponse();

            //lRequest.NovoRange = 1;

            lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

            if (null != lRetornoConsulta && null != lRetornoConsulta.Monitor)
            {
                List<ExposicaoClienteInfo> lListaMonitor = lRetornoConsulta.Monitor;

                for (int i = 50; i < lRetornoConsulta.TotalRegistros; i+=50)
                {
                    lRequest.NovoRange =  i/50;
                    lListaMonitor.AddRange(lServico.ObterMonitorLucroPrejuizo(lRequest).Monitor);
                }

                this.SessaoUltimaConsulta = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lListaMonitor);

                Session["Monitor_" + base.UsuarioLogado.Id] = lRetornoConsulta.Monitor;

                this.ResponderFiltrarPorColuna();

                this.AplicarFiltrosDePesquisa();

                //lRetorno = base.RetornarSucessoAjax(this.SessaoUltimaConsulta, "Sucesso");

                lRetornoLista = new TransporteDeListaPaginadaMonitoramentoRisco(this.SessaoUltimaConsulta);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                if (!string.IsNullOrEmpty(lColunas))
                {
                    lRetornoLista.ColunasDaGrid = lColunas;
                }

                lRetornoLista.TotalDeItens = this.SessaoUltimaConsulta.Count;

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

        private MonitorLucroPrejuizoRequest SeparaConsultaGet(string pConsulta)
        {
            MonitorLucroPrejuizoRequest lRetorno = new MonitorLucroPrejuizoRequest();

            string[] lConsultaSplit = pConsulta.Split('&');

            for (int i = 0; i < lConsultaSplit.Length; i++)
            {
                if (!lConsultaSplit[i].Split('=')[1].Equals(string.Empty))
                {
                    if (lConsultaSplit[i].Split('=')[0].Equals("CodigoCliente"))
                    {
                        lRetorno.Cliente = int.Parse(lConsultaSplit[i].Split('=')[1]);
                    }

                    if (lConsultaSplit[i].Split('=')[0].Equals("CodAssessor"))
                    {
                        lRetorno.Assessor = int.Parse(lConsultaSplit[i].Split('=')[1]);

                        if (base.CodigoAssessor != null)
                        {
                            lRetorno.Assessor = base.CodigoAssessor.Value;
                        }
                    }
                    
                    if (lConsultaSplit[i].Split('=')[0].Equals("ProporcaoPrejuizo"))
                    {
                        lRetorno.ProporcaoPrejuiso = (Monitores.Risco.Enum.EnumProporcaoPrejuiso)int.Parse(lConsultaSplit[i].Split('=')[1]);
                    }

                    if (lConsultaSplit[i].Split('=')[0].Equals("Semaforo"))
                    {
                        lRetorno.Semaforo = (Monitores.Risco.Enum.EnumSemaforo)int.Parse(lConsultaSplit[i].Split('=')[1]);
                    }
                }
            }

                return lRetorno;
        }
        #endregion
    }
}