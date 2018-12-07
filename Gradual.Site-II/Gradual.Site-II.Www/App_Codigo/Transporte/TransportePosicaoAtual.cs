using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Site.DbLib.Dados;
using Gradual.OMS.Monitor.Custodia.Lib.Info;

namespace Gradual.Site.Www
{
    public class TransportePosicaoAtual
    {
        #region Propriedades Bovespa

        public string CodigoAtivo { get; set; }

        public string NomeAtivo { get; set; }

        public string Mercado { get; set; }

        public string TipoCarteira { get; set; }

        public string QuantidadeAexecutarCompra { get; set; }

        public string QuantidadeAexecutarVenda { get; set; }

        public string SaldoD1 { get; set; }

        public string SaldoD2 { get; set; }

        public string SaldoD3 { get; set; }

        public string QuantidadeTotal { get; set; }

        public string ValorCotacao { get; set; }

        public string ValorFinanceiro { get; set; }

        public string DataVencimento { get; set; }

        public string CssSaldoD1 { get; set; }

        public string CssSaldoD2 { get; set; }

        public string CssSaldoD3 { get; set; }

        public string CssValorCotacao { get; set; }

        public string CssValorFinanceiro { get; set; }

        #endregion

        #region Propriedades Bmf
        public string TpMercado     { get; set; }     
        public string CodigoInstrumento   { get; set; } 
        public string CodigoSerie   { get; set; } 
        public string Empresa       { get; set; } 
        public string Carteira      { get; set; } 
        public string QtdeAbertura  { get; set; }
        public string QtdeCompra    { get; set; } 
        public string QtdeVenda     { get; set; } 
        public string QtdeAtual     { get; set; } 
        public string Ajuste        { get; set; } 
        public string Cotacao       { get; set; } 
        public string Variacao      { get; set; } 
        public string Resultado     { get; set; } 
        #endregion

        #region Métodos Públicos

        public static List<TransportePosicaoAtual> TraduzirLista(List<CustodiaInfo> pParametro)
        {
            List<TransportePosicaoAtual> lRetorno = new List<TransportePosicaoAtual>();
            List<CustodiaInfo> lTemp = new List<CustodiaInfo>();
            TransportePosicaoAtual lItem;

            foreach (CustodiaInfo lInfoAtual in pParametro)
            {
                var lInfoAnterior = lTemp.Find(negocio => negocio.CodigoAtivo == lInfoAtual.CodigoAtivo && negocio.TipoCarteira == lInfoAtual.TipoCarteira);

                if (lInfoAnterior != null)
                {
                    lTemp.Remove(lInfoAnterior);

                    lInfoAnterior.CodigoAtivo                = lInfoAtual.CodigoAtivo;
                    lInfoAnterior.NomeAtivo                  = lInfoAtual.NomeAtivo;
                    lInfoAnterior.Mercado                    = lInfoAtual.Mercado;
                    lInfoAnterior.TipoCarteira               = lInfoAtual.TipoCarteira;
                    lInfoAnterior.TipoGrupo                  = lInfoAtual.TipoGrupo;
                    lInfoAnterior.QuantidadeTotal           += lInfoAtual.QuantidadeTotal;
                    lInfoAnterior.SaldoD1                   += lInfoAtual.SaldoD1;
                    lInfoAnterior.SaldoD2                   += lInfoAtual.SaldoD2;
                    lInfoAnterior.SaldoD3                   += lInfoAtual.SaldoD3;
                    lInfoAnterior.QuantidadeAexecutarCompra += lInfoAtual.QuantidadeAexecutarCompra;
                    lInfoAnterior.QuantidadeAexecutarVenda  += lInfoAtual.QuantidadeAexecutarVenda;
                    lInfoAnterior.DataVencimento             = lInfoAtual.DataVencimento;
                    lInfoAnterior.ValorAtual                += lInfoAtual.ValorAtual;
                    lInfoAnterior.ValorFinanceiro           += lInfoAtual.ValorFinanceiro;

                    lTemp.Add(lInfoAnterior);
                }
                else
                {
                    lTemp.Add(lInfoAtual);
                }
            }

            //foreach (CustodiaInfo lInfo in pParametro)
            foreach (CustodiaInfo lInfo in lTemp)
            {
                if (lInfo.TipoGrupo.Equals("TEDI"))
                    continue;

                lItem = new TransportePosicaoAtual();

                lItem.CodigoAtivo = lInfo.CodigoAtivo;

                lItem.CssSaldoD1 = DefinirCorPosNeg(lInfo.SaldoD1);
                lItem.CssSaldoD2 = DefinirCorPosNeg(lInfo.SaldoD2);
                lItem.CssSaldoD3 = DefinirCorPosNeg(lInfo.SaldoD3);

                lItem.CssValorCotacao    = DefinirCorPosNeg(lInfo.ValorCotacao);
                lItem.CssValorFinanceiro = DefinirCorPosNeg(lInfo.ValorFinanceiro);

                lItem.NomeAtivo = lInfo.NomeAtivo;
                lItem.DataVencimento = lInfo.DataVencimento != null && lInfo.DataVencimento != DateTime.MinValue ? lInfo.DataVencimento.Value.ToString("dd/MM/yyyy") : "n/d";

                lItem.Mercado = lInfo.Mercado;

                lItem.QuantidadeAexecutarCompra = lInfo.QuantidadeAexecutarCompra != null ? lInfo.QuantidadeAexecutarCompra.Value.ToString("N2") : "n/c";
                lItem.QuantidadeAexecutarVenda  = lInfo.QuantidadeAexecutarVenda  != null ? lInfo.QuantidadeAexecutarVenda.Value.ToString("N2") : "n/c";

                lItem.QuantidadeTotal = lInfo.QuantidadeTotal != null ? lInfo.QuantidadeTotal.Value.ToString("N0") : "n/c";

                lItem.SaldoD1 = lInfo.SaldoD1 != null ? lInfo.SaldoD1.Value.ToString("N2") : "0,00";
                lItem.SaldoD2 = lInfo.SaldoD2 != null ? lInfo.SaldoD2.Value.ToString("N2") : "0,00";
                lItem.SaldoD3 = lInfo.SaldoD3 != null ? lInfo.SaldoD3.Value.ToString("N2") : "0,00";

                lItem.TipoCarteira = lInfo.TipoCarteira;
                lItem.ValorCotacao = lInfo.ValorCotacao != null ? lInfo.ValorCotacao.Value.ToString("N2") : "0,00";
                lItem.ValorFinanceiro = lInfo.ValorFinanceiro != null ? lInfo.ValorFinanceiro.Value.ToString("N2") : "0,00";

                lRetorno.Add(lItem);
            }

            return lRetorno;
        }

        public static List<TransportePosicaoAtual> TraduzirListaBmf(List<MonitorCustodiaInfo.CustodiaPosicao> pParametro)
        {
            var lRetorno = new List<TransportePosicaoAtual>();

            pParametro.ForEach(custodia =>
            {
                var item = new TransportePosicaoAtual();

                if (custodia.TipoMercado.ToLower().Equals("fut") || custodia.TipoMercado.ToLower().Equals("dis"))
                {

                    item.TpMercado         = custodia.TipoMercado;
                    item.CodigoInstrumento = custodia.CodigoInstrumento;
                    item.CodigoSerie       = custodia.CodigoSerie;
                    item.Empresa           = custodia.NomeEmpresa;
                    item.Carteira          = custodia.CodigoCarteira + " " + custodia.DescricaoCarteira;
                    item.QtdeAbertura      = "";
                    item.QtdeCompra        = custodia.QtdeAExecCompra.ToString("N2");
                    item.QtdeVenda         = custodia.QtdeAExecVenda.ToString("N2");
                    item.QtdeAtual         = custodia.QtdeAtual.ToString();
                    item.Ajuste            = custodia.ValorFechamento.ToString("N2");
                    item.Cotacao           = custodia.Cotacao.ToString("N2");
                    item.Variacao          = custodia.Variacao.ToString("N2");
                    item.Resultado         = custodia.Resultado.ToString("N2");

                    lRetorno.Add(item);
                }
            });

            return lRetorno;
        }

        private static string DefinirCorPosNeg(decimal? pParametro)
        {
            if (pParametro > 0L)
            {
                return "ValorPositivo_Azul";
            }
            else if (pParametro < 0L)
            {
                return "ValorNegativo_Vermelho";
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }
}