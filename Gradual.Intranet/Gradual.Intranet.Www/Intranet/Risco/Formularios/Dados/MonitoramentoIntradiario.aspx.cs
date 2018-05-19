using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Monitores.Risco.Lib;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Monitores.Risco.Info;
using Gradual.OMS.Library.Servicos;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco;
using Gradual.Intranet.Contratos.Dados.Risco;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class MonitoramentoIntradiario : PaginaBase
    {
        #region Propriedades
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

        public EnumEXPxPosicao GetEXPxPosicao
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["EXPxPosicao"]))
                    return EnumEXPxPosicao.TODOS;

                return (EnumEXPxPosicao)Enum.Parse(typeof(EnumEXPxPosicao), this.Request["EXPxPosicao"]);
            }
        }

        public EnumNet GetNETVOLUME
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["NETVOLUME"]))
                    return EnumNet.TODOS;

                return (EnumNet)Enum.Parse(typeof(EnumNet), this.Request["NETVOLUME"]);
            }
        }

        public EnumNETxSFP GetNETxSFP
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["NETxSFP"]))
                    return EnumNETxSFP.TODOS;

                return (EnumNETxSFP)Enum.Parse(typeof(EnumNETxSFP), this.Request["NETxSFP"]);
            }
        }

        public DateTime GetDataDe
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["DataDe"]))
                    return DateTime.Now.AddDays(-30);

                return DateTime.Parse( this.Request["DataDe"]);
            }
        }

        public DateTime GetDataAte
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["DataAte"]))
                    return DateTime.Now.AddDays(1);

                return DateTime.Parse(this.Request["DataAte"] + " 23:59:59");
            }
        }
        private static List<TransporteMonitoramentoIntradiario> gListMonitoramentoIntradiario;

        private List<TransporteMonitoramentoIntradiario> SessaoUltimaConsulta
        {
            get { return gListMonitoramentoIntradiario != null ? gListMonitoramentoIntradiario : new List<TransporteMonitoramentoIntradiario>(); }
            set { gListMonitoramentoIntradiario = value; }
        }
        #endregion

        #region Eventos
        
        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                     , "CarregarHtmlComDados"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderBuscarItensParaListagemSimples
                                                     });
        }
        #endregion
        #region Métodos serviço risco

        private string ResponderBuscarItensParaListagemSimples()
        {
            MonitoramentoIntradiarioDbLib lServico = new MonitoramentoIntradiarioDbLib();

            string lRetorno = string.Empty;

            string lColunas = string.Empty;

            MonitoramentoIntradiarioInfo lRequest = new MonitoramentoIntradiarioInfo();

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
                lRequest.CodigoLogin    = this.UsuarioLogado.Id;
            }

            lRequest.enumEXPxPosicao = this.GetEXPxPosicao;
            
            lRequest.enumNET         = this.GetNETVOLUME;
            
            lRequest.enumNETxSFP     = this.GetNETxSFP;
            
            lRequest.DataDe          = this.GetDataDe;
            
            lRequest.DataAte         = this.GetDataAte;

            MonitoramentoIntradiarioInfo lRetornoConsulta = new MonitoramentoIntradiarioInfo();

            lRetornoConsulta = lServico.ObterMonitoramentoIntradiario(lRequest);

            if (lRetornoConsulta != null  && lRetornoConsulta.Resultado != null )
            {
                List<TransporteMonitoramentoIntradiario> lListaTransporte =  new TransporteMonitoramentoIntradiario().TraduzirLista(lRetornoConsulta.Resultado);

                SessaoUltimaConsulta = lListaTransporte;

                this.ResponderFiltrarPorColuna();

                lRetornoLista = new TransporteDeListaPaginada(SessaoUltimaConsulta);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = lRetornoConsulta.Resultado.Count;// this.SessaoUltimaConsulta.Count;

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

                case "SFP":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SFP), decimal.Parse(lp2.SFP)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SFP), decimal.Parse(lp1.SFP)));
                    }
                    break;

                case "Net":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.Net), decimal.Parse(lp2.Net)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.Net), decimal.Parse(lp1.Net)));
                    }
                    break;

                case "Exposicao":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.Exposicao), decimal.Parse(lp2.Exposicao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.Exposicao), decimal.Parse(lp1.Exposicao)));
                    }
                    break;

                case "PercentualFINANxExposicao":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PercentualFINANxExposicao), decimal.Parse(lp2.PercentualFINANxExposicao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PercentualFINANxExposicao), decimal.Parse(lp1.PercentualFINANxExposicao)));
                    }
                    break;

                case "PercentualSFPxNET":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PercentualSFPxNET), decimal.Parse(lp2.PercentualSFPxNET)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PercentualSFPxNET), decimal.Parse(lp1.PercentualSFPxNET)));
                    }
                    break;

                case "Posicao":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.Posicao), decimal.Parse(lp2.Posicao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.Posicao), decimal.Parse(lp1.Posicao)));
                    }
                    break;

                }

                return base.RetornarSucessoAjax(this.SessaoUltimaConsulta, "sucesso");
            }
        #endregion
    }
}