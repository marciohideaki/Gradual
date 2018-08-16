using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Monitores.Compliance.Lib;
using Gradual.OMS.Library.Servicos;
using Newtonsoft.Json;
using System.Text;

namespace Gradual.Intranet.Www.Intranet.Compliance.Formularios
{
    public partial class OrdensAlteradasDayTrade : PaginaBase
    {
        #region Propriedades
        private DateTime? GetDataDe
        {
            get
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(this.Request["DataDe"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private DateTime? GetDataAte
        {
            get
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(this.Request["DataAte"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }
        private int? GetNumeroSeqOrdem
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["NumeroSeqOrdem"], out lRetorno))
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
        private static List<TransporteCompliance.OrdensAlteradasInfo> gComplianceOrdensAlteradasDayTrade;

        private static List<TransporteCompliance.OrdensAlteradasDayTradeCabecalhoInfo> gComplianceOrdensAlteradasDayTradeCabecalho;

        private List<TransporteCompliance.OrdensAlteradasInfo> SessaoUltimaConsulta
        {
            get { return gComplianceOrdensAlteradasDayTrade != null ? gComplianceOrdensAlteradasDayTrade : new List<TransporteCompliance.OrdensAlteradasInfo>(); }
            set { gComplianceOrdensAlteradasDayTrade = value; }
        }

        private List<TransporteCompliance.OrdensAlteradasDayTradeCabecalhoInfo> SessaoUltimaConsultaCabecalho
        {
            get { return gComplianceOrdensAlteradasDayTradeCabecalho != null ? gComplianceOrdensAlteradasDayTradeCabecalho : new List<TransporteCompliance.OrdensAlteradasDayTradeCabecalhoInfo>(); }
            set { gComplianceOrdensAlteradasDayTradeCabecalho = value; }
        }

        #endregion

        #region Eventos
        
        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                      ,"BuscarOrdensAlteradasDayTrade"
                                                      ,"BuscarOrdensAlteradas"
                                                      , "CarregarComoCSV"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderBuscarOrdensAlteradas
                                                     , this.ResponderArquivoCSV
                                                     });
        }

        private string ResponderBuscarOrdensAlteradas()
        {
            string lRetorno = string.Empty;

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            List<TransporteCompliance.OrdensAlteradasInfo> lLista = this.SessaoUltimaConsulta.FindAll(Ord => Ord.NumeroSeqOrdem == GetNumeroSeqOrdem.Value.ToString());

            if (lLista != null)
            {
                lRetornoLista = new TransporteDeListaPaginada(lLista);

                lRetorno      = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = this.SessaoUltimaConsulta.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0;
            }

            return lRetorno;
        }
        private string ResponderFiltrarPorColuna()
        {
            switch (this.GetFiltrarPor)
            {
                case "DataHoraOrdem":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaCabecalho.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(Convert.ToDateTime(lp1.DataHoraOrdem), Convert.ToDateTime(lp2.DataHoraOrdem)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaCabecalho.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(Convert.ToDateTime(lp2.DataHoraOrdem), Convert.ToDateTime(lp1.DataHoraOrdem)));
                    }
                    break;
                
            }

            return base.RetornarSucessoAjax(this.SessaoUltimaConsulta, "sucesso");
        }

        private string ResponderBuscarItensParaListagemSimples()
        {

            OrdensAlteradasDayTradeRequest lRequest = new OrdensAlteradasDayTradeRequest();

            IServicoMonitorCompliance lServico = Ativador.Get<IServicoMonitorCompliance>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            string lRetorno = string.Empty;

            if (this.GetDataDe != null)
            {
                lRequest.DataDe = this.GetDataDe.Value;
            }

            if (this.GetDataAte != null)
            {
                lRequest.DataAte = this.GetDataAte.Value;
            }

            OrdensAlteradasDayTradeResponse lResponse = new OrdensAlteradasDayTradeResponse();

            lResponse = lServico.ObterAlteracaoDayTrade(lRequest);

            if (lResponse != null && lResponse.lstAlteracaoDayTrade != null)
            {
                this.SessaoUltimaConsulta = new TransporteCompliance().TraduzirLista(lResponse.lstAlteracaoDayTrade, "ordens");

                this.SessaoUltimaConsultaCabecalho = new TransporteCompliance().TraduzirLista(lResponse.lstAlteracaoDayTrade);

                this.ResponderFiltrarPorColuna();

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsultaCabecalho);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens   = this.SessaoUltimaConsulta.Count;
                
                lRetornoLista.PaginaAtual    = 1;
                
                lRetornoLista.TotalDePaginas = 0;

                return lRetorno;
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requisição");
            }

            return lRetorno;
        }

        private string ResponderArquivoCSV()
        {
            StringBuilder lContent = new StringBuilder();

            if (this.SessaoUltimaConsulta != null)
            {
                lContent.Append("Número Ordem" + "\t");
                lContent.Append("Data/Hora" + "\t");
                lContent.Append("DayTrade" + "\t");
                lContent.Append("Justificativa" + "\t");
                lContent.Append("Tipo Mercado" + "\r");

                this.SessaoUltimaConsultaCabecalho.ForEach(cab =>
                {
                    
                    lContent.AppendFormat(cab.NumeroSeqOrdem + "\t");
                    lContent.AppendFormat(cab.DataHoraOrdem + "\t");
                    lContent.AppendFormat(cab.DayTrade + "\t");
                    lContent.AppendFormat(cab.Justificativa + "\t");
                    lContent.AppendFormat(cab.TipoMercado + "\r");

                    var lDetalhes = SessaoUltimaConsulta.FindAll(ordem=>ordem.NumeroSeqOrdem == cab.NumeroSeqOrdem);

                    if (lDetalhes.Count > 0)
                    {
                        lContent.Append("Assessor" + "\t");
                        lContent.Append("CodigoCliente" + "\t");
                        lContent.Append("ContaErro" + "\t");
                        lContent.Append("DataAlteracao" + "\t");
                        lContent.Append("DescontoCorretagem" + "\t");
                        lContent.Append("Instrumento" + "\t");
                        lContent.Append("NumeroSeqOrdem" + "\t");
                        lContent.Append("Quantidade" + "\t");
                        lContent.Append("Sentido" + "\t");
                        lContent.Append("TipoPessoa" + "\t");
                        lContent.Append("Usuario" + "\t");
                        lContent.Append("Vinculado" + "\t");
                        lContent.Append("UsuarioAlteracao" + "\r");

                        lDetalhes.ForEach(ordens =>
                            {
                                lContent.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\r",
                                ordens.Assessor,
                                ordens.CodigoCliente,
                                ordens.ContaErro,
                                ordens.DataAlteracao,
                                ordens.DescontoCorretagem,
                                ordens.Instrumento,
                                ordens.NumeroSeqOrdem,
                                ordens.Quantidade,
                                ordens.Sentido,
                                ordens.TipoPessoa,
                                ordens.Usuario,
                                ordens.UsuarioAlteracao,
                                ordens.Vinculado
                                );
                            });
                    }
                    
                });

                this.Response.Clear();

                this.Response.ContentType = "text/xls";

                this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                this.Response.Charset = "iso-8859-1";

                this.Response.AddHeader("content-disposition", "attachment;filename=OrdensAlteradas.xls");

                this.Response.Write(lContent.ToString());

                this.Response.End();
            }

            return string.Empty;
        }

        #endregion
    }
}