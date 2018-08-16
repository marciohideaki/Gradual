using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro
{
    public class TransporteFaxBovespa
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

        public string DataLiquidacaoVista { get; set; }

        public string DataLiquidacaoOpcao { get; set; }

        public List<TransporteFaxBovespaCabecalhoGrid> CabecalhosGridOpcao {get; set;}

        public List<TransporteFaxBovespaCabecalhoGrid> CabecalhosGridVista { get; set; }

        public List<TransporteFaxBovespaDetalhe> DetalhesGridResumidoOpcao { get; set; }

        public List<TransporteFaxBovespaDetalhe> DetalhesGridResumidoVista { get; set; }

        public string RodapeTotalComprasOpcao { get; set; }
               
        public string RodapeTotalVendasOpcao { get; set; }
               
        public string RodapeTotalComprasVista { get; set; }
               
        public string RodapeTotalVendasVista { get; set; }

        public string RodapeTotalTermoVista { get; set; }

        public string RodapeTotalAjusteVista { get; set; }

        public string RodapeTotalNegociosVista { get; set; }

        public string RodapeTotalCorretagemVista { get; set; }

        public string RodapeTaxaCblcVista { get; set; }

        public string RodapeTaxaBovespaVista { get; set; }

        public string RodapeTaxaOperacionaisVista { get; set; }

        public string RodapeOutrasDepesasVista { get; set; }

        public string RodapeIRDayTradeVista { get; set; }

        public string RodapeIrOperacoesVista { get; set; }

        public string RodapeTotalLiquidoVista { get; set; }

        public string RodapeTotalTermoOpcao { get; set; }

        public string RodapeTotalAjusteOpcao { get; set; }

        public string RodapeTotalNegociosOpcao { get; set; }

        public string RodapeTotalCorretagemOpcao { get; set; }

        public string RodapeTaxaCblcOpcao { get; set; }

        public string RodapeTaxaBovespaOpcao { get; set; }

        public string RodapeTaxaOperacionaisOpcao { get; set; }

        public string RodapeOutrasDepesasOpcao { get; set; }

        public string RodapeIRDayTradeOpcao { get; set; }

        public string RodapeIrOperacoesOpcao { get; set; }

        public string RodapeTotalLiquidoOpcao { get; set; }

        public string RodapeBaseIRDTOpcao { get; set; }

        public string RodapeBaseIROperacoesOpcao { get; set; }

        public string RodapeEmolumentoBolsaOpcao { get; set; }

        public string RodapeEmolumentoTotalOpcao { get; set; }

        public string RodapeTaxaLiquidacaoOpcao { get; set; }

        public string RodapeTaxaRegistroBolsaOpcao { get; set; }

        public string RodapeTaxaRegistroTotalOpcao { get; set; }

        public string RodapeBaseIRDTVista { get; set; }

        public string RodapeBaseIROperacoesVista { get; set; }

        public string RodapeEmolumentoBolsaVista { get; set; }

        public string RodapeEmolumentoTotalVista { get; set; }

        public string RodapeTaxaLiquidacaoVista { get; set; }

        public string RodapeTaxaRegistroBolsaVista { get; set; }

        public string RodapeTaxaRegistroTotalVista { get; set; }

        public string RodapeTaxaRegistroVista { get; set; }

        public string RodapeTaxaRegistroOpcao { get; set; }

        public TransporteFaxBovespa TraduzirLista(FaxBovespaInfo pParametro, string lCulture = "")
        {
            var lRetorno = new TransporteFaxBovespa();

            lRetorno.CodigoCliente           = pParametro.CodigoCliente.ToString();
            lRetorno.DataLiquidacaoOpcao     = lCulture == "EN" ? pParametro.DataLiquidacaoOpcao.ToString("MM/dd/yyyy"): pParametro.DataLiquidacaoOpcao.ToString("dd/MM/yyyy");
            lRetorno.DataLiquidacaoVista     = lCulture == "EN" ? pParametro.DataLiquidacaoVista.ToString("MM/dd/yyyy"):pParametro.DataLiquidacaoVista.ToString("dd/MM/yyyy");
            lRetorno.DataPregao              = lCulture == "EN" ? pParametro.DataPregao.ToString("MM/dd/yyyy") : pParametro.DataPregao.ToString("dd/MM/yyyy");
            lRetorno.DigitoCliente           = pParametro.DigitoCliente.ToString();
            lRetorno.Empresa                 = pParametro.Empresa;
            lRetorno.Fax                     = pParametro.Fax;
            lRetorno.NomeCliente             = pParametro.NomeCliente;

            //Rodapé opção
            lRetorno.RodapeTotalComprasOpcao      = pParametro.RodapeTotalComprasOpcao.ToString("N2");
            lRetorno.RodapeTotalVendasOpcao       = pParametro.RodapeTotalVendasOpcao.ToString("N2");
            lRetorno.RodapeIRDayTradeOpcao        = pParametro.RodapeIRDayTradeOpcao.ToString("N2");
            lRetorno.RodapeIrOperacoesOpcao       = pParametro.RodapeIrOperacoesOpcao.ToString("N2");
            lRetorno.RodapeOutrasDepesasOpcao     = pParametro.RodapeOutrasDepesasOpcao.ToString("N2");
            lRetorno.RodapeTaxaBovespaOpcao       = pParametro.RodapeTaxaBovespaOpcao.ToString("N2");
            lRetorno.RodapeTaxaCblcOpcao          = pParametro.RodapeTaxaCblcOpcao.ToString("N2");
            lRetorno.RodapeTaxaOperacionaisOpcao  = pParametro.RodapeTaxaOperacionaisOpcao.ToString("N2");
            lRetorno.RodapeTotalAjusteOpcao       = pParametro.RodapeTotalAjusteOpcao.ToString("N2");
            lRetorno.RodapeTotalCorretagemOpcao   = pParametro.RodapeTotalCorretagemOpcao.ToString("N2");
            lRetorno.RodapeTotalLiquidoOpcao      = pParametro.RodapeTotalLiquidoOpcao.ToString("N2");
            lRetorno.RodapeTotalNegociosOpcao     = pParametro.RodapeTotalNegociosOpcao.ToString("N2");
            lRetorno.RodapeTotalTermoOpcao        = pParametro.RodapeTotalTermoOpcao.ToString("N2");
            lRetorno.RodapeBaseIRDTOpcao          = pParametro.RodapeBaseIRDTOpcao.ToString("N2");
            lRetorno.RodapeBaseIROperacoesOpcao   = pParametro.RodapeBaseIROperacoesOpcao.ToString("N2");
            lRetorno.RodapeEmolumentoBolsaOpcao   = pParametro.RodapeEmolumentoBolsaOpcao.ToString("N2");
            lRetorno.RodapeEmolumentoTotalOpcao   = pParametro.RodapeEmolumentoTotalOpcao.ToString("N2");
            lRetorno.RodapeTaxaLiquidacaoOpcao    = pParametro.RodapeTaxaLiquidacaoOpcao.ToString("N2");
            lRetorno.RodapeTaxaRegistroBolsaOpcao = pParametro.RodapeTaxaRegistroBolsaOpcao.ToString("N2");
            lRetorno.RodapeTaxaRegistroTotalOpcao = pParametro.RodapeTaxaRegistroTotalOpcao.ToString("N2");
            lRetorno.RodapeTaxaRegistroOpcao      = pParametro.RodapeTaxaRegistroOpcao.ToString("N2");

            //Rodapé a vista
            lRetorno.RodapeTotalVendasVista       = pParametro.RodapeTotalVendasVista.ToString("N2");
            lRetorno.RodapeTotalComprasVista      = pParametro.RodapeTotalComprasVista.ToString("N2");
            lRetorno.RodapeIRDayTradeVista        = pParametro.RodapeIRDayTradeVista.ToString("N2");
            lRetorno.RodapeIrOperacoesVista       = pParametro.RodapeIrOperacoesVista.ToString("N2");
            lRetorno.RodapeOutrasDepesasVista     = pParametro.RodapeOutrasDepesasVista.ToString("N2");
            lRetorno.RodapeTaxaBovespaVista       = pParametro.RodapeTaxaBovespaVista.ToString("N2");
            lRetorno.RodapeTaxaCblcVista          = pParametro.RodapeTaxaCblcVista.ToString("N2");
            lRetorno.RodapeTaxaOperacionaisVista  = pParametro.RodapeTaxaOperacionaisVista.ToString("N2");
            lRetorno.RodapeTotalAjusteVista       = pParametro.RodapeTotalAjusteVista.ToString("N2");
            lRetorno.RodapeTotalCorretagemVista   = pParametro.RodapeTotalCorretagemVista.ToString("N2");
            lRetorno.RodapeTotalLiquidoVista      = pParametro.RodapeTotalLiquidoVista.ToString("N2");
            lRetorno.RodapeTotalNegociosVista     = pParametro.RodapeTotalNegociosVista.ToString("N2");
            lRetorno.RodapeTotalTermoVista        = pParametro.RodapeTotalTermoVista.ToString("N2");
            lRetorno.RodapeBaseIRDTVista          = pParametro.RodapeBaseIRDTVista.ToString("N2")  ;
            lRetorno.RodapeBaseIROperacoesVista   = pParametro.RodapeBaseIROperacoesVista.ToString("N2");
            lRetorno.RodapeEmolumentoBolsaVista   = pParametro.RodapeEmolumentoBolsaVista.ToString("N2");
            lRetorno.RodapeEmolumentoTotalVista   = pParametro.RodapeEmolumentoTotalVista.ToString("N2");
            lRetorno.RodapeTaxaLiquidacaoVista    = pParametro.RodapeTaxaLiquidacaoVista.ToString("N2");  
            lRetorno.RodapeTaxaRegistroBolsaVista = pParametro.RodapeTaxaRegistroBolsaVista.ToString("N2");
            lRetorno.RodapeTaxaRegistroTotalVista = pParametro.RodapeTaxaRegistroTotalVista.ToString("N2");
            lRetorno.RodapeTaxaRegistroVista      = pParametro.RodapeTaxaRegistroVista.ToString("N2");

            var lDetalhe = new TransporteFaxBovespaCabecalhoGrid();
            lRetorno.CabecalhosGridOpcao = new List<TransporteFaxBovespaCabecalhoGrid>();
            lRetorno.CabecalhosGridVista = new List<TransporteFaxBovespaCabecalhoGrid>();

            lRetorno.CabecalhosGridOpcao = PreparaCabecalhoDetalheOpcao(pParametro.DetalhesBovespa, pParametro.DataLiquidacaoOpcao);
            lRetorno.CabecalhosGridVista = PreparaCabecalhoDetalheVista(pParametro.DetalhesBovespa, pParametro.DataLiquidacaoVista);

            return lRetorno;
        }

        public TransporteFaxBovespa TraduzirListaResumido(FaxBovespaInfo pParametro, string lCulture = "")
        {
            var lRetorno = new TransporteFaxBovespa();

            lRetorno.CodigoCliente       = pParametro.CodigoCliente.ToString();
            lRetorno.DataLiquidacaoOpcao = lCulture == "EN" ? pParametro.DataLiquidacaoOpcao.ToString("MM/dd/yyyy") : pParametro.DataLiquidacaoOpcao.ToString("dd/MM/yyyy");
            lRetorno.DataLiquidacaoVista = lCulture == "EN" ? pParametro.DataLiquidacaoVista.ToString("MM/dd/yyyy") : pParametro.DataLiquidacaoVista.ToString("dd/MM/yyyy");
            lRetorno.DataPregao          = lCulture == "EN" ? pParametro.DataPregao.ToString("MM/dd/yyyy") : pParametro.DataPregao.ToString("dd/MM/yyyy");
            lRetorno.DigitoCliente       = pParametro.DigitoCliente.ToString();
            lRetorno.Empresa             = pParametro.Empresa;
            lRetorno.Fax                 = pParametro.Fax;
            lRetorno.NomeCliente         = pParametro.NomeCliente;

            //Rodapé opção
            lRetorno.RodapeTotalComprasOpcao      = pParametro.RodapeTotalComprasOpcao.ToString("N2");
            lRetorno.RodapeTotalVendasOpcao       = pParametro.RodapeTotalVendasOpcao.ToString("N2");
            lRetorno.RodapeIRDayTradeOpcao        = pParametro.RodapeIRDayTradeOpcao.ToString("N2");
            lRetorno.RodapeIrOperacoesOpcao       = pParametro.RodapeIrOperacoesOpcao.ToString("N2");
            lRetorno.RodapeOutrasDepesasOpcao     = pParametro.RodapeOutrasDepesasOpcao.ToString("N2");
            lRetorno.RodapeTaxaBovespaOpcao       = pParametro.RodapeTaxaBovespaOpcao.ToString("N2");
            lRetorno.RodapeTaxaCblcOpcao          = pParametro.RodapeTaxaCblcOpcao.ToString("N2");
            lRetorno.RodapeTaxaOperacionaisOpcao  = pParametro.RodapeTaxaOperacionaisOpcao.ToString("N2");
            lRetorno.RodapeTotalAjusteOpcao       = pParametro.RodapeTotalAjusteOpcao.ToString("N2");
            lRetorno.RodapeTotalCorretagemOpcao   = pParametro.RodapeTotalCorretagemOpcao.ToString("N2");
            lRetorno.RodapeTotalLiquidoOpcao      = pParametro.RodapeTotalLiquidoOpcao.ToString("N2");
            lRetorno.RodapeTotalNegociosOpcao     = pParametro.RodapeTotalNegociosOpcao.ToString("N2");
            lRetorno.RodapeTotalTermoOpcao        = pParametro.RodapeTotalTermoOpcao.ToString("N2");
            lRetorno.RodapeBaseIRDTOpcao          = pParametro.RodapeBaseIRDTOpcao.ToString("N2");
            lRetorno.RodapeBaseIROperacoesOpcao   = pParametro.RodapeBaseIROperacoesOpcao.ToString("N2");
            lRetorno.RodapeEmolumentoBolsaOpcao   = pParametro.RodapeEmolumentoBolsaOpcao.ToString("N2");
            lRetorno.RodapeEmolumentoTotalOpcao   = pParametro.RodapeEmolumentoTotalOpcao.ToString("N2");
            lRetorno.RodapeTaxaLiquidacaoOpcao    = pParametro.RodapeTaxaLiquidacaoOpcao.ToString("N2");
            lRetorno.RodapeTaxaRegistroBolsaOpcao = pParametro.RodapeTaxaRegistroBolsaOpcao.ToString("N2");
            lRetorno.RodapeTaxaRegistroTotalOpcao = pParametro.RodapeTaxaRegistroTotalOpcao.ToString("N2");
            lRetorno.RodapeTaxaRegistroOpcao      = pParametro.RodapeTaxaRegistroOpcao.ToString("N2");

            //Rodapé a vista
            lRetorno.RodapeTotalVendasVista       = pParametro.RodapeTotalVendasVista.ToString("N2");
            lRetorno.RodapeTotalComprasVista      = pParametro.RodapeTotalComprasVista.ToString("N2");
            lRetorno.RodapeIRDayTradeVista        = pParametro.RodapeIRDayTradeVista.ToString("N2");
            lRetorno.RodapeIrOperacoesVista       = pParametro.RodapeIrOperacoesVista.ToString("N2");
            lRetorno.RodapeOutrasDepesasVista     = pParametro.RodapeOutrasDepesasVista.ToString("N2");
            lRetorno.RodapeTaxaBovespaVista       = pParametro.RodapeTaxaBovespaVista.ToString("N2");
            lRetorno.RodapeTaxaCblcVista          = pParametro.RodapeTaxaCblcVista.ToString("N2");
            lRetorno.RodapeTaxaOperacionaisVista  = pParametro.RodapeTaxaOperacionaisVista.ToString("N2");
            lRetorno.RodapeTotalAjusteVista       = pParametro.RodapeTotalAjusteVista.ToString("N2");
            lRetorno.RodapeTotalCorretagemVista   = pParametro.RodapeTotalCorretagemVista.ToString("N2");
            lRetorno.RodapeTotalLiquidoVista      = pParametro.RodapeTotalLiquidoVista.ToString("N2");
            lRetorno.RodapeTotalNegociosVista     = pParametro.RodapeTotalNegociosVista.ToString("N2");
            lRetorno.RodapeTotalTermoVista        = pParametro.RodapeTotalTermoVista.ToString("N2");
            lRetorno.RodapeBaseIRDTVista          = pParametro.RodapeBaseIRDTVista.ToString("N2");
            lRetorno.RodapeBaseIROperacoesVista   = pParametro.RodapeBaseIROperacoesVista.ToString("N2");
            lRetorno.RodapeEmolumentoBolsaVista   = pParametro.RodapeEmolumentoBolsaVista.ToString("N2");
            lRetorno.RodapeEmolumentoTotalVista   = pParametro.RodapeEmolumentoTotalVista.ToString("N2");
            lRetorno.RodapeTaxaLiquidacaoVista    = pParametro.RodapeTaxaLiquidacaoVista.ToString("N2");
            lRetorno.RodapeTaxaRegistroBolsaVista = pParametro.RodapeTaxaRegistroBolsaVista.ToString("N2");
            lRetorno.RodapeTaxaRegistroTotalVista = pParametro.RodapeTaxaRegistroTotalVista.ToString("N2");
            lRetorno.RodapeTaxaRegistroVista      = pParametro.RodapeTaxaRegistroVista.ToString("N2");

            lRetorno.CabecalhosGridOpcao= this.PreparaDetalheResumidoOpcao(pParametro.DetalhesBovespa, pParametro.DataLiquidacaoOpcao);
            lRetorno.CabecalhosGridVista = this.PreparaDetalheResumidoVista(pParametro.DetalhesBovespa, pParametro.DataLiquidacaoVista);

            return lRetorno;
        }

        private List<TransporteFaxBovespaCabecalhoGrid> PreparaCabecalhoDetalheOpcao(List<FaxBovespaDetalheInfo> pList, DateTime dtLiquidacaoOpcao)
        {
            var lRetorno = new List<TransporteFaxBovespaCabecalhoGrid>();

            var lLista = from a in pList
                         group a by new
                         {
                             a.PapelCodigoNegocio,
                             a.PapelTipoMercado,
                             a.PapelSentido,
                             a.SomaMedio,
                             a.SomaQtdeTotal,
                             a.SomaTotal,
                             a.SomaTotalCorretagem,
                             a.TotalNet,
                             a.TotalPrecoNet

                         } into g
                         select new  FaxBovespaCabecalhoGridInfo
                         {
                             CodigoNegocio   = g.Key.PapelCodigoNegocio,
                             TipoMercado     = g.Key.PapelTipoMercado,
                             Sentido         = g.Key.PapelSentido,
                             SomaPreco       = g.Key.SomaMedio,
                             SomaQuantidade  = int.Parse(g.Key.SomaQtdeTotal.ToString()),
                             SomaCorretagem  = g.Key.SomaTotalCorretagem,
                             SomaTotal       = g.Key.SomaTotal,
                             TotalNet        = g.Key.TotalNet,
                             TotalPrecoNet   = g.Key.TotalPrecoNet,
                             DetalhesBovespa = g.ToList()
                         };

            var lTrans = new TransporteFaxBovespaCabecalhoGrid();

            foreach (var a in lLista)
            {
                if (a.TipoMercado == "VIS" ||
                    a.TipoMercado == "TER" ||
                    a.TipoMercado == "FRA" ||
                    a.TipoMercado == "EOC" ||
                    a.TipoMercado == "EOV")
                    continue;

                lTrans = new TransporteFaxBovespaCabecalhoGrid();


                lTrans.NomeRes        = a.NomeRes;
                lTrans.CodigoIsin     = a.CodigoIsin;
                lTrans.CodigoNegocio  = a.CodigoNegocio;
                lTrans.SomaCorretagem = a.SomaCorretagem.ToString("N2");
                lTrans.SomaPreco      = a.SomaPreco.ToString("N4");
                lTrans.SomaQuantidade = a.SomaQuantidade.ToString("N0");
                lTrans.SomaTotal      = a.SomaTotal.ToString("N2");
                lTrans.TipoMercado    = a.TipoMercado.ToString();
                lTrans.Sentido        = a.Sentido;
                lTrans.TotalNet       = a.TotalNet.ToString("N2");
                lTrans.TotalPrecoNet  = a.TotalPrecoNet.ToString("N4");
                lTrans.DataLiquidacao = dtLiquidacaoOpcao.ToString("dd/MM/yyyy");

                lTrans.DetalhesBovespa = new List<TransporteFaxBovespaDetalhe>();

                foreach (var b in a.DetalhesBovespa)
                {
                    if (b.PapelTipoMercado == "VIS"|| 
                        b.PapelTipoMercado == "FRA"||
                        b.PapelTipoMercado == "TER"||
                        b.PapelTipoMercado == "EOC" ||
                        b.PapelTipoMercado == "EOV")
                        continue;
                    TransporteFaxBovespaDetalhe lDetalhe = new TransporteFaxBovespaDetalhe();
                    lTrans.CodigoIsin           = b.PapelCodigoIsin;
                    lTrans.NomeRes              = b.PapelNomeRes;
                    lDetalhe.PapelCodigoNegocio = b.PapelCodigoNegocio;
                    lDetalhe.PapelCorretagem    = b.PapelCorretagem.ToString("N4");
                    lDetalhe.PapelQuantidade    = b.PapelQuantidade.ToString("N0");
                    lDetalhe.PapelPreco         = b.PapelPreco.ToString("N2");
                    lDetalhe.PapelSentido       = b.PapelSentido.ToString();
                    lDetalhe.PapelTotal         = b.PapelTotal.ToString("N2");
                    lDetalhe.PapelTipoMercado   = b.PapelTipoMercado;
                    lDetalhe.PapelNomeRes       = b.PapelNomeRes;
                    lDetalhe.SomaTotal          = b.SomaTotal.ToString("N2");

                    lTrans.DetalhesBovespa.Add(lDetalhe);    
                }

                lRetorno.Add(lTrans);
            }

            return lRetorno;
        }

        private List<TransporteFaxBovespaCabecalhoGrid> PreparaCabecalhoDetalheVista(List<FaxBovespaDetalheInfo> pList, DateTime dtLiquidacaoVista)
        {
            var lRetorno = new List<TransporteFaxBovespaCabecalhoGrid>();

            var lLista = from a in pList
                         group a by new
                         {
                             a.PapelCodigoNegocio,
                             a.PapelTipoMercado,
                             a.PapelSentido,
                             a.SomaMedio,
                             a.SomaQtdeTotal,
                             a.SomaTotal,
                             a.SomaTotalCorretagem,
                             a.TotalNet,
                             a.TotalPrecoNet

                         } into g
                         select new FaxBovespaCabecalhoGridInfo
                         {
                             CodigoNegocio   = g.Key.PapelCodigoNegocio,
                             TipoMercado     = g.Key.PapelTipoMercado,
                             Sentido         = g.Key.PapelSentido,
                             SomaPreco       = g.Key.SomaMedio,
                             SomaQuantidade  = int.Parse(g.Key.SomaQtdeTotal.ToString()),
                             SomaCorretagem  = g.Key.SomaTotalCorretagem,
                             SomaTotal       = g.Key.SomaTotal,
                             TotalNet        = g.Key.TotalNet,
                             TotalPrecoNet   = g.Key.TotalPrecoNet,
                             DetalhesBovespa = g.ToList()
                         };

            var lTrans = new TransporteFaxBovespaCabecalhoGrid();

            foreach (var a in lLista)
            {
                if (a.TipoMercado != "VIS" &&
                    a.TipoMercado != "TER" &&
                    a.TipoMercado != "FRA" &&
                    a.TipoMercado != "EOC" &&
                    a.TipoMercado != "EOV")
                    continue;

                lTrans = new TransporteFaxBovespaCabecalhoGrid();

                lTrans.NomeRes        = a.NomeRes;
                lTrans.CodigoIsin     = a.CodigoIsin;
                lTrans.CodigoNegocio  = a.CodigoNegocio;
                lTrans.SomaCorretagem = a.SomaCorretagem.ToString("N2");
                lTrans.SomaPreco      = a.SomaPreco.ToString("N4");
                lTrans.SomaQuantidade = a.SomaQuantidade.ToString("N0");
                lTrans.SomaTotal      = a.SomaTotal.ToString("N2");
                lTrans.TipoMercado    = a.TipoMercado.ToString();
                lTrans.Sentido        = a.Sentido;
                lTrans.TotalNet       = a.TotalNet.ToString("N2");
                lTrans.TotalPrecoNet  = a.TotalPrecoNet.ToString("N4");
                lTrans.DataLiquidacao = dtLiquidacaoVista.ToString("dd/MM/yyyy");

                lTrans.DetalhesBovespa = new List<TransporteFaxBovespaDetalhe>();

                foreach (var b in a.DetalhesBovespa)
                {
                    if (b.PapelTipoMercado != "VIS" && 
                        b.PapelTipoMercado != "TER" &&
                        b.PapelTipoMercado != "FRA" &&
                        b.PapelTipoMercado != "EOC" &&
                        b.PapelTipoMercado != "EOV")
                        continue;

                    TransporteFaxBovespaDetalhe lDetalhe = new TransporteFaxBovespaDetalhe();
                    lTrans.CodigoIsin           = b.PapelCodigoIsin;
                    lTrans.NomeRes              = b.PapelNomeRes;
                    lDetalhe.PapelCodigoNegocio = b.PapelCodigoNegocio;
                    lDetalhe.PapelCorretagem    = b.PapelCorretagem.ToString("N4");
                    lDetalhe.PapelQuantidade    = b.PapelQuantidade.ToString("N0");
                    lDetalhe.PapelPreco         = b.PapelPreco.ToString("N2");
                    lDetalhe.PapelSentido       = b.PapelSentido.ToString();
                    lDetalhe.PapelTotal         = b.PapelTotal.ToString("N2");
                    lDetalhe.PapelTipoMercado   = b.PapelTipoMercado;
                    lDetalhe.PapelNomeRes       = b.PapelNomeRes;
                    lDetalhe.SomaTotal          = b.SomaTotal.ToString("N2");

                    lTrans.DetalhesBovespa.Add(lDetalhe);
                }

                lRetorno.Add(lTrans);
            }

            return lRetorno;
        }

        private List<TransporteFaxBovespaCabecalhoGrid> PreparaDetalheResumidoOpcao(List<FaxBovespaDetalheInfo> pList, DateTime dtLiquidacaoOpcao)
        {
            var lRetorno = new List<TransporteFaxBovespaCabecalhoGrid>();

            var lLista = from a in pList
                         group a by new
                         {
                             a.PapelSentido,
                             a.PapelCodigoNegocio
                         } 
                         into g
                         select new FaxBovespaCabecalhoGridInfo
                         {
                             Sentido = g.Key.PapelSentido,
                             CodigoNegocio = g.Key.PapelCodigoNegocio,
                             DetalhesBovespa = g.ToList(),
                             TipoMercado     = g.ToList()[0].PapelTipoMercado
                         };

            foreach (var a in lLista)
            {
                if (a.TipoMercado == "VIS"||
                    a.TipoMercado == "TER"||
                    a.TipoMercado == "FRA"||
                    a.TipoMercado == "EOC"||
                    a.TipoMercado == "EOV") continue;

                var lTrans = new TransporteFaxBovespaCabecalhoGrid();

                lTrans.DetalhesBovespa = new List<TransporteFaxBovespaDetalhe>();

                foreach (var b in a.DetalhesBovespa)
                {
                    if (b.PapelTipoMercado == "VIS" ||
                        b.PapelTipoMercado == "TER" ||
                        b.PapelTipoMercado == "FRA" ||
                        b.PapelTipoMercado == "EOC" ||
                        b.PapelTipoMercado == "EOV") continue;

                    var lDetalhe = new TransporteFaxBovespaDetalhe();

                    lTrans.SomaPreco      = b.SomaMedio.ToString("N4");
                    lTrans.SomaQuantidade = b.SomaQtdeTotal.ToString();
                    lTrans.SomaTotal      = b.SomaTotal.ToString("N2");

                    lDetalhe.PapelCodigoNegocio = b.PapelCodigoNegocio;
                    lDetalhe.PapelSentido       = b.PapelSentido;
                    lDetalhe.PapelTipoMercado   = b.PapelTipoMercado;
                    lDetalhe.PapelNomeRes       = b.PapelNomeRes;
                    lDetalhe.PapelQuantidade    = b.PapelQuantidade.ToString("N0");
                    lDetalhe.PapelPreco         = b.PapelPreco.ToString("N2");
                    lDetalhe.PapelVolume        = b.PapelTotal.ToString("N2");
                    

                    lTrans.DetalhesBovespa.Add(lDetalhe);
                }

                lRetorno.Add(lTrans);
            }

            return lRetorno;
        }

        private List<TransporteFaxBovespaCabecalhoGrid> PreparaDetalheResumidoVista(List<FaxBovespaDetalheInfo> pList, DateTime dtLiquidacaoVista)
        {
            var lRetorno = new List<TransporteFaxBovespaCabecalhoGrid>();

            var lLista = from a in pList
                         group a by new
                         {
                             a.PapelSentido,
                             a.PapelCodigoNegocio
                         }
                         into g
                         select new FaxBovespaCabecalhoGridInfo
                         {
                            Sentido         = g.Key.PapelSentido,
                            CodigoNegocio   = g.Key.PapelCodigoNegocio,
                            DetalhesBovespa = g.ToList(),
                            TipoMercado     = g.ToList()[0].PapelTipoMercado
                         };

            foreach (var a in lLista)
            {
                if (a.TipoMercado != "VIS" &&
                    a.TipoMercado != "TER" &&
                    a.TipoMercado != "FRA" &&
                    a.TipoMercado != "EOC" &&
                    a.TipoMercado != "EOV") continue;

                var lTrans = new TransporteFaxBovespaCabecalhoGrid();

                lTrans.DetalhesBovespa = new List<TransporteFaxBovespaDetalhe>();

                foreach (var b in a.DetalhesBovespa)
                {
                    if (b.PapelTipoMercado != "VIS" &&
                        b.PapelTipoMercado != "TER" &&
                        b.PapelTipoMercado != "FRA" &&
                        b.PapelTipoMercado != "EOC" &&
                        b.PapelTipoMercado != "EOV") continue;

                    var lDetalhe = new TransporteFaxBovespaDetalhe();

                    lTrans.SomaPreco      = b.SomaMedio.ToString("N4");
                    lTrans.SomaQuantidade = b.SomaQtdeTotal.ToString();
                    lTrans.SomaTotal      = b.SomaTotal.ToString("N2");

                    lDetalhe.PapelCodigoNegocio = b.PapelCodigoNegocio;
                    lDetalhe.PapelSentido       = b.PapelSentido;
                    lDetalhe.PapelTipoMercado   = b.PapelTipoMercado;
                    lDetalhe.PapelNomeRes       = b.PapelNomeRes;
                    lDetalhe.PapelQuantidade    = b.PapelQuantidade.ToString("N0");
                    lDetalhe.PapelPreco         = b.PapelPreco.ToString("N2");
                    lDetalhe.PapelVolume        = b.PapelTotal.ToString("N2");

                    lTrans.DetalhesBovespa.Add(lDetalhe);
                }

                lRetorno.Add(lTrans);
            }

            return lRetorno;
        }
    }
    public class TransporteFaxBovespaCabecalhoGrid
    {
        public List<TransporteFaxBovespaDetalhe> DetalhesBovespa { get; set; }
        
        public string CodigoIsin { get; set; }
        
        public string CodigoNegocio { get; set; }
        
        public string NomeRes { get; set; }
        
        public string TipoMercado { get; set; }
        
        public string SomaQuantidade { get; set; }
        
        public string SomaPreco { get; set; }
        
        public string SomaTotal { get; set; }
        
        public string SomaCorretagem { get; set; }
        
        public string TotalNet { get; set; }

        public string TotalPrecoNet { get; set; }

        public string Sentido { get; set; }

        public string DataLiquidacao { get; set; }
    }

    public class TransporteFaxBovespaDetalhe
    {
        
        public string PapelSentido { get; set; }

        public string PapelTipoMercado { get; set; }

        public string PapelNomeRes { get; set; }

        public string PapelCodigoIsin { get; set; }

        public string PapelCodigoNegocio { get; set; }

        public string PapelQuantidade { get; set; }

        public string PapelPreco { get; set; }

        public string PapelTotal { get; set; }

        public string PapelCorretagem { get; set; }

        public string SomaCodigoNegocio { get; set; }

        public string SomaQtdeTotal { get; set; }

        public string SomaMedio { get; set; }

        public string SomaTotal { get; set; }

        public string SomaTotalCorretagem { get; set; }

        public string TotalCodigoNegocio { get; set; }

        public string TotalSentido { get; set; }

        public string TotalNet { get; set; }

        public string PapelVolume { get; set; }
    }
}