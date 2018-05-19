using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Monitores.Risco.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Monitores.Risco.Info;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Newtonsoft.Json;
using System.Globalization;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class PLD : PaginaBase
    {
        #region Propriedades
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

        private string GetInstrumento
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request["Instrumento"]))
                    lRetorno = this.Request["Instrumento"];

                return lRetorno;
            }
        }

        private Monitores.Risco.Enum.EnumStatusPLD GetCriticidade
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["Criticidade"]))
                    return Monitores.Risco.Enum.EnumStatusPLD.COMPLETO;

                return (Monitores.Risco.Enum.EnumStatusPLD)Enum.Parse(typeof(Monitores.Risco.Enum.EnumStatusPLD), this.Request["Criticidade"]);
            }
        }

        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.PDLOperacaoGridInfo> gCompliancePLDOperacao;

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.PDLOperacaoGridInfo> SessaoUltimaConsulta
        {
            get { return gCompliancePLDOperacao != null ? gCompliancePLDOperacao : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.PDLOperacaoGridInfo>(); }
            set { gCompliancePLDOperacao = value; }
        }

        #endregion

        #region Metodos
        
        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                     },
                    new ResponderAcaoAjaxDelegate[] { this.ResponderBuscarItensParaListagemSimples
                                                     });
        }

        private string ResponderBuscarItensParaListagemSimples()
        {
            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            string lRetorno = string.Empty;

            MonitorPLDRequest lRequest = new MonitorPLDRequest();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            //if (null != this.GetCdCliente)
            //{
            //    lRequest.CodigoCliente = this.GetCdCliente.Value;
            //}
            
            if (!string.IsNullOrEmpty(this.GetInstrumento))
            {
                lRequest.Instrumento = this.GetInstrumento;
            }

            lRequest.EnumStatusPLD = this.GetCriticidade;

            MonitorPLDResponse lRetornoConsulta = lServico.ObterMonitorPLD(lRequest);

            if (null != lRetornoConsulta && null != lRetornoConsulta.lstPLD)
            {
                this.SessaoUltimaConsulta = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.lstPLD);

                this.OrdenarMinutosRestantes();

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsulta);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

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
        private void OrdenarMinutosRestantes()
        {
            this.SessaoUltimaConsulta.Sort(delegate ( TransporteRiscoMonitoramentoLucrosPrejuizos.PDLOperacaoGridInfo lp1, TransporteRiscoMonitoramentoLucrosPrejuizos.PDLOperacaoGridInfo lp2) 
            {
                TimeSpan lSpan2;
                TimeSpan lSpan1;

                TimeSpan.TryParse(lp1.MinutosRestantesPLD, new CultureInfo("pt-BR"), out lSpan1);
                TimeSpan.TryParse(lp2.MinutosRestantesPLD, new CultureInfo("pt-BR"), out lSpan2);

                return lSpan1.CompareTo(lSpan2);
            });
        }
        #endregion
    }
}