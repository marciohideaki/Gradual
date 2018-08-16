using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Monitores.Compliance.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Newtonsoft.Json;
using System.Text;

namespace Gradual.Intranet.Www.Intranet.Compliance.Formularios
{
    public partial class EstatisticaDayTrade : PaginaBase
    {
        #region Propriedades
        
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

        private string GetTipoBolsa
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request["TipoBolsa"]))
                    lRetorno =  this.Request["TipoBolsa"];

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

        private static List<TransporteCompliance.EstatisticasDayTradeinfo> gComplianceEstatisticaDayTrade;

        private List<TransporteCompliance.EstatisticasDayTradeinfo> SessaoUltimaConsulta
        {
            get { return gComplianceEstatisticaDayTrade != null ? gComplianceEstatisticaDayTrade : new List<TransporteCompliance.EstatisticasDayTradeinfo>(); }
            set { gComplianceEstatisticaDayTrade = value; }
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                      ,"BuscarEstatisticaDayTrade"
                                                      ,"CarregarComoCSV"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderBuscarItensParaListagemSimples
                                                     ,  this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderArquivoCSV
                                                     });

            if (string.IsNullOrWhiteSpace(base.Acao))
                this.ResponderCarregarHtmlComDados();
        }

        private string ResponderCarregarHtmlComDados()
        {
            //base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptBM_FiltroRelatorio_CodAssessor);

            return base.RetornarSucessoAjax("Funcionou 3");
        }

        #region Métodos 
        
        private string ResponderBuscarItensParaListagemSimples()
        {

            EstatisticaDayTradeRequest lRequest = new EstatisticaDayTradeRequest();

            IServicoMonitorCompliance lServico = Ativador.Get<IServicoMonitorCompliance>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            string lRetorno = string.Empty;

            if (this.GetCdAssessor != null)
            {
                lRequest.Assessor = this.GetCdAssessor.Value;
            }

            if (this.GetCdCliente != null)
            {
                lRequest.CodigoCliente = this.GetCdCliente.Value;
            }

            if (!string.IsNullOrEmpty(this.GetTipoBolsa))
            {
                if (this.GetTipoBolsa.Equals("BMF"))
                {
                    lRequest.TipoBolsa = EnumBolsaDayTrade.BMF;
                }
                else if (this.GetTipoBolsa.Equals("BOVESPA"))
                {
                    lRequest.TipoBolsa = EnumBolsaDayTrade.BOVESPA;
                }
            }
            else
            {
                lRequest.TipoBolsa = EnumBolsaDayTrade.TODOS;
            }

            EstatisticaDayTradeResponse lResponse = new EstatisticaDayTradeResponse();

            lResponse = lServico.ObterEstatisticaDayTradeBovespa(lRequest);

            if (lResponse != null && lResponse.ListaEstatisticaBovespa != null)
            {
                this.SessaoUltimaConsulta = new TransporteCompliance().TraduzirLista(lResponse.ListaEstatisticaBovespa);

                this.ResponderFiltrarPorColunaDetalhes();

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

        private string ResponderFiltrarPorColunaDetalhes()
        {
            switch (this.GetFiltrarPor)
            {
                case "NET":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare( Convert.ToDecimal( lp1.NET), Convert.ToDecimal(lp2.NET)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(Convert.ToDecimal(lp2.NET), Convert.ToDecimal(lp1.NET)));
                    }
                    break;
                case "ValorNegativo":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(Decimal.Parse(lp1.ValorNegativo), Decimal.Parse(lp2.ValorNegativo)));
                    }
                    else
                    {
                        this.SessaoUltimaConsulta.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(Decimal.Parse(lp2.ValorNegativo), Decimal.Parse(lp1.ValorNegativo)));
                    }
                    break;
            }

            return base.RetornarSucessoAjax(this.SessaoUltimaConsulta, "sucesso");
        }

        private string ResponderArquivoCSV()
        {
            StringBuilder lContent = new StringBuilder();

            if (this.SessaoUltimaConsulta != null)
            {
                lContent.Append("Assessor"       + "\t");
                lContent.Append("Nome Assessor"  + "\t");
                lContent.Append("Cliente"        + "\t");
                lContent.Append("Nome Cliente"   + "\t");
                lContent.Append("Idade"          + "\t");
                lContent.Append("Data Negócio"   + "\t");
                lContent.Append("NET"            + "\t");
                lContent.Append("% Positivo"     + "\t");
                lContent.Append("Pessoa Vinculadaprador" + "\t");
                lContent.Append("Qtde DayTrade"  + "\t");
                lContent.Append("Qtde DayTrade"  + "\t");
                lContent.Append("Tipo Bolsa"     + "\t");
                lContent.Append("Valor Negativo" + "\t");
                lContent.Append("Valor Positivo" + "\r");

                this.SessaoUltimaConsulta.ForEach(est =>
                {
                    lContent.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\r",
                        est.CodigoAssessor            ,
                        est.NomeAssessor              ,
                        est.CodigoCliente             ,
                        est.NomeCliente               ,
                        est.Idade                     ,
                        est.DataNegocio               ,
                        est.NET                       ,
                        est.PercentualPositivo        ,
                        est.PessoaVinculada           ,
                        est.QuantidadeDayTrade        ,
                        est.QuantidadeDayTradePositivo,
                        est.TipoBolsa                 ,
                        est.ValorNegativo             ,
                        est.ValorPositivo             
                        );
                });

                this.Response.Clear();

                this.Response.ContentType = "text/xls";

                this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                this.Response.Charset = "iso-8859-1";

                this.Response.AddHeader("content-disposition", "attachment;filename=EstatisticaDayTrade.xls");

                this.Response.Write(lContent.ToString());

                this.Response.End();
            }

            return string.Empty;
        }

        #endregion
    }
}