using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Monitores.Risco.Lib;

namespace Gradual.Site.Www.Transporte
{
    public class Transporte_BTC
    {
        public string Carteira { get; set; }

        public string CodigoCliente { get; set; }

        public string DataAbertura { get; set; }

        public string DataVencimento { get; set; }

        public string Instrumento { get; set; }

        public string PrecoMedio { get; set; }

        public string PrecoMercado { get; set; }

        public string Quantidade { get; set; }

        public string Remuneracao { get; set; }

        public string Taxa { get; set; }

        public string TipoContrato { get; set; }

        public string SubtotalQuantidade { get; set; }

        public string SubtotalValor { get; set; }

        public List<Transporte_BTC> TraduzirLista(List<BTCInfo> pParametros)
        {
            var lRetorno = new List<Transporte_BTC>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(btc =>
                {
                    var lTemp = lRetorno.Find(ordem => ordem.Instrumento == btc.Instrumento);

                    if (lTemp != null && !string.IsNullOrEmpty(lTemp.Instrumento))
                    {
                        lRetorno.Remove(lTemp);

                        decimal lTempDecimal = Convert.ToDecimal(lTemp.Remuneracao) + btc.Remuneracao;

                        int lTempInt = Convert.ToInt32(lTemp.Quantidade.Replace(".", string.Empty)) + btc.Quantidade;

                        lTemp.Carteira       = btc.Carteira.ToString();
                        lTemp.DataVencimento = btc.DataVencimento.ToString("dd/MM/yyyy");
                        lTemp.DataAbertura   = btc.DataAbertura.ToString("dd/MM/yyyy");
                        lTemp.CodigoCliente  = btc.CodigoCliente.ToString();
                        lTemp.Instrumento    = btc.Instrumento;
                        lTemp.Remuneracao    = lTempDecimal.ToString("N2");
                        lTemp.PrecoMedio     = btc.PrecoMedio.ToString("N2");
                        lTemp.PrecoMercado   = btc.PrecoMercado.ToString();
                        lTemp.Quantidade     = string.Format("{0:#,0}", lTempInt);
                        lTemp.Taxa           = btc.Taxa.ToString("N2");
                        lTemp.TipoContrato   = btc.TipoContrato.ToString();

                        lTempDecimal = 0.0M;

                        lTempDecimal = decimal.Parse(lTemp.SubtotalValor) + (btc.PrecoMercado * btc.Quantidade);

                        lTemp.SubtotalValor = lTempDecimal.ToString("N2");

                        lTemp.SubtotalQuantidade = string.Format("{0:#,0}", lTempInt);

                        lRetorno.Add(lTemp);
                    }
                    else
                    {
                        Transporte_BTC lInfo = new Transporte_BTC();

                        lInfo.Carteira           = btc.Carteira.ToString();
                        lInfo.DataVencimento     = btc.DataVencimento.ToString("dd/MM/yyyy");
                        lInfo.DataAbertura       = btc.DataAbertura.ToString("dd/MM/yyyy");
                        lInfo.CodigoCliente      = btc.CodigoCliente.ToString();
                        lInfo.Instrumento        = btc.Instrumento;
                        lInfo.Remuneracao        = btc.Remuneracao.ToString("N2");
                        lInfo.PrecoMedio         = btc.PrecoMedio.ToString("N2");
                        lInfo.PrecoMercado       = btc.PrecoMercado.ToString();
                        lInfo.Quantidade         = string.Format("{0:#,0}", btc.Quantidade);
                        lInfo.Taxa               = btc.Taxa.ToString("N2");
                        lInfo.TipoContrato       = btc.TipoContrato.ToString();
                        lInfo.SubtotalValor      = (btc.PrecoMercado * btc.Quantidade).ToString("N2");
                        lInfo.SubtotalQuantidade = string.Format("{0:#,0}", btc.Quantidade);

                        lRetorno.Add(lInfo);
                    }
                });

            return lRetorno;
        }

    }

}