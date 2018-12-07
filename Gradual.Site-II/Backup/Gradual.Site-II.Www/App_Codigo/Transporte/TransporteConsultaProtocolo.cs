using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.TesouroDireto.Lib.Dados;

namespace Gradual.Site.Www
{
    public class TransporteConsultaProtocolo
    {
        public struct ConsultaProtocoloInfo
        {

            public string CodigoCesta { get; set; }

            public string CodigoTitulo { get; set; }

            public string TipoCesta { get; set; }

            public string Mercado { get; set; }

            public string Situacao { get; set; }

            public string NomeTitulo { get; set; }

            public string QuantidadeCompra { get; set; }

            public string ValorTitulo { get; set; }

            public string ValorTaxaCBLC { get; set; }

            public string ValorTaxaAC { get; set; }

            public string ValorAposPgtoTaxas { get; set; }
        }

        public static List<ConsultaProtocoloInfo> TraduzirLista(List<TituloMercadoInfo> pParametros)
        {
            List<ConsultaProtocoloInfo> lRetorno = new List<ConsultaProtocoloInfo>();

            ConsultaProtocoloInfo lItem;

            if (pParametros != null)
            {
                foreach (TituloMercadoInfo lTitulo in pParametros)
                {
                    lItem = new ConsultaProtocoloInfo();

                    lItem.CodigoCesta        = lTitulo.CodigoCesta;
                    lItem.CodigoTitulo       = lTitulo.CodigoTitulo.DBToString();
                    lItem.Mercado            = lTitulo.Mercado.DBToString();
                    lItem.NomeTitulo         = lTitulo.NomeTitulo.ToUpper();
                    lItem.QuantidadeCompra   = lTitulo.QuantidadeCompra.ToString("N2");
                    lItem.Situacao           = TraduzirSituacao(lTitulo.Situacao);
                    lItem.TipoCesta          = lTitulo.TipoCesta == "1" ? "Compra" : "Venda";
                    lItem.ValorTaxaAC        = lTitulo.ValorTaxaAC.ToString("C2");
                    lItem.ValorTaxaCBLC      = lTitulo.ValorTaxaCBLC.ToString("C2");
                    lItem.ValorTitulo        = lTitulo.ValorTitulo.ToString("C2");
                    lItem.ValorAposPgtoTaxas = ((lTitulo.ValorTaxaCBLC.DBToDouble() + lTitulo.ValorTaxaAC.DBToDouble()) + lTitulo.ValorTitulo.DBToDouble() * lTitulo.QuantidadeCompra).ToString("C2");

                    lRetorno.Add(lItem);
                }
            }

            return lRetorno;
        }

        private static string TraduzirSituacao(string pParametro)
        {
            string lRetorno = "-";

            switch (pParametro)
            {
                case "1": lRetorno = "Em Liquidação";
                    break;
                case "2": lRetorno = "Liquidado";
                    break;
                case "3": lRetorno = "Cancelado";
                    break;
                case "4": lRetorno = "Pendente";
                    break;
            };
            return lRetorno;
        }
    }
}