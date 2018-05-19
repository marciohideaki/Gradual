using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro
{
    public class TransporteFaxBmf
    {
        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        public string CodigoCliente { get; set; }

        public string DigitoCliente { get; set; }

        public string NomeCliente { get; set; }

        public string NomeAssessor { get; set; }

        public string Empresa { get; set; }

        public string Telefone { get; set; }

        public string Fax { get; set; }

        public string DataPregao { get; set; }

        public string DataLiquidacao { get; set; }

        public List<TransporteFaxBmfCabecalhoGrid> CabecalhosGrid { get; set; }

        public List<TransporteFaxBmfDetalhe> DetalhesGrid { get; set; }

        public TransporteFaxBmf TraduzirListaResumido(FaxBmfInfo pParametro, string lCulture = "")
        {
            var lRetorno = new TransporteFaxBmf();

            lRetorno.CodigoCliente  = pParametro.CodigoCliente.ToString();
            lRetorno.DataLiquidacao = lCulture == "EN" ? pParametro.DataLiquidacao.ToString("MM/dd/yyyy") : pParametro.DataLiquidacao.ToString("dd/MM/yyyy");
            lRetorno.DataPregao     = lCulture == "EN" ? pParametro.DataPregao.ToString("MM/dd/yyyy") : pParametro.DataPregao.ToString("dd/MM/yyyy");
            lRetorno.Fax            = pParametro.Fax;
            lRetorno.NomeCliente    = pParametro.NomeCliente;

            lRetorno.CabecalhosGrid = this.PreparaCabecalhoDetalhe(pParametro.CabecalhoGridBmf, pParametro.DataLiquidacao);

            return lRetorno;
        }
        
        private List<TransporteFaxBmfCabecalhoGrid> PreparaCabecalhoDetalhe(List<FaxBmfCabecalhoGridInfo> pList, DateTime dtLiquidacao)
        {
            var lRetorno = new List<TransporteFaxBmfCabecalhoGrid>();

            var lTrans = new TransporteFaxBmfCabecalhoGrid();

            var lListaTemp = from a in pList orderby a.CabecalhoCommod, a.CabecalhoSerie, a.CabecalhoSentido select a;

            var lLista = lListaTemp.ToList();

            foreach (var a in lLista)
            {
                lTrans = new TransporteFaxBmfCabecalhoGrid();

                lTrans.CabecalhoSentido     = a.CabecalhoSentido;
                lTrans.CabecalhoCommod      = a.CabecalhoCommod;
                lTrans.CabecalhoTipoMercado = a.CabecalhoTipoMercado;
                lTrans.CabecalhoSerie       = a.CabecalhoSerie;
                lTrans.SomaCodigoNegocio    = a.SomaCodigoNegocio;
                lTrans.SomaQuantidade       = a.SomaQuantidade.ToString("N0");
                lTrans.SomaPreco            = a.SomaPreco.ToString("N4");
                lTrans.NetCodigoNegocio     = a.NetCodigoNegocio;
                lTrans.NetQuantidade        = a.NetQuantidade.ToString("N");
                lTrans.NetPreco             = a.NetPreco.ToString("N4");
                lTrans.DataLiquidacao       = dtLiquidacao.ToString("dd/MM/yyyy");

                lTrans.DetalhesBmf = new List<TransporteFaxBmfDetalhe>();

                foreach (var b in a.DetalhesBmf)
                {
                    TransporteFaxBmfDetalhe lDetalhe = new TransporteFaxBmfDetalhe();

                    lDetalhe.PapelCodigoNegocio = a.SomaCodigoNegocio;// b.PapelCodigoNegocio;
                    lDetalhe.PapelQuantidade    = b.PapelQuantidade.ToString("N0");
                    lDetalhe.PapelPreco         = b.PapelPreco.ToString("N4");

                    lTrans.DetalhesBmf.Add(lDetalhe);
                }
                lRetorno.Add(lTrans);
            }
            return lRetorno;
        }
    }

    public class TransporteFaxBmfCabecalhoGrid
    {
        public string CabecalhoCommod                       { get; set; }

        public string CabecalhoTipoMercado                  { get; set; }

        public string CabecalhoSerie                        { get; set; }

        public List<TransporteFaxBmfDetalhe> DetalhesBmf    { get; set; }

        public string SomaCodigoNegocio                     { get; set; }

        public string SomaQuantidade                        { get; set; }

        public string SomaPreco                             { get; set; }

        public string NetCodigoNegocio                      { get; set; }

        public string NetQuantidade                         { get; set; }

        public string NetPreco                              { get; set; }

        public string CabecalhoSentido                      { get; set; }

        public string DataLiquidacao                        { get; set; }
    }

    public class TransporteFaxBmfDetalhe
    {
        public string PapelCodigoNegocio { get; set; }

        public string PapelQuantidade { get; set; }

        public string PapelPreco { get; set; }
    }
}