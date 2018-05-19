using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteCustodia
    {
        public string CodigoAssessor        { get; set; }
        public string CodigoCliente         { get; set; }
        public string CodigoAtivo           { get; set; }
        public string CodigoMercado         { get; set; }
        public string CodigoCarteira        { get; set; }
        public string QuantidadeDisponivel  { get; set; }
        public string QuantidadeD1          { get; set; }
        public string QuantidadeD2          { get; set; }
        public string QuantidadeD3          { get; set; }
        public string QuantidadePendente    { get; set; }
        public string QuantidadeALiquidar   { get; set; }
        public string QuantidadeTotal       { get; set; }

        public TransporteCustodia() {}

        public TransporteCustodia(Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo pInfo)
        {
            this.CodigoAssessor             = pInfo.CodigoAssessor.ToString();
            this.CodigoCliente              = pInfo.CodigoCliente.ToString();
            this.CodigoAtivo                = pInfo.CodigoAtivo.ToString();
            this.CodigoMercado              = pInfo.CodigoMercado.ToString();
            this.CodigoCarteira             = pInfo.CodigoCarteira.ToString();
            this.QuantidadeDisponivel       = pInfo.QuantidadeDisponivel.ToString();
            this.QuantidadeD1               = pInfo.QuantidadeD1.ToString();
            this.QuantidadeD2               = pInfo.QuantidadeD2.ToString();
            this.QuantidadeD3               = pInfo.QuantidadeD3.ToString();
            this.QuantidadePendente         = pInfo.QuantidadePendente.ToString();
            this.QuantidadeALiquidar        = pInfo.QuantidadeALiquidar.ToString();
            this.QuantidadeTotal            = pInfo.QuantidadeTotal.ToString();
        }

        //public List<TransporteCustodia> TraduzirLista(List<Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo> info)
        public List<TransporteCustodia> TraduzirLista(List<Gradual.OMS.Monitor.Custodia.Lib.Info.MonitorCustodiaInfo.CustodiaPosicao> info)    
        {
            var lRetorno = new List<TransporteCustodia>();

            info.ForEach(cus =>
            {
                lRetorno.Add(new TransporteCustodia()
                {
                    //CodigoAssessor          = cus.CodigoAssessor.ToString(),
                    //CodigoCliente           = cus.CodigoCliente.ToString(),
                    //CodigoAtivo             = cus.CodigoAtivo.ToString(),
                    //CodigoCarteira          = cus.CodigoCarteira.ToString(),
                    //CodigoMercado           = cus.CodigoMercado.ToString(),
                    //QuantidadeDisponivel    = cus.QuantidadeDisponivel.ToString(),
                    //QuantidadeD1            = cus.QuantidadeD1.ToString(),
                    //QuantidadeD2            = cus.QuantidadeD2.ToString(),
                    //QuantidadeD3            = cus.QuantidadeD3.ToString(),
                    //QuantidadePendente      = cus.QuantidadePendente.ToString(),
                    //QuantidadeALiquidar     = cus.QuantidadeALiquidar.ToString(),
                    //QuantidadeTotal         = cus.QuantidadeTotal.ToString()

                    //CodigoAssessor = cus.CodigoAssessor.ToString(),
                    CodigoCliente = cus.IdCliente.ToString(),
                    CodigoAtivo = cus.CodigoInstrumento.ToString(),
                    CodigoCarteira = cus.CodigoCarteira.ToString(),
                    CodigoMercado = cus.TipoMercado.ToString(),
                    QuantidadeDisponivel = cus.QtdeDisponivel.ToString(),
                    QuantidadeD1 = cus.QtdeD1.ToString(),
                    QuantidadeD2 = cus.QtdeD2.ToString(),
                    QuantidadeD3 = cus.QtdeD3.ToString(),
                    //QuantidadePendente = cus.QuantidadePendente.ToString(),
                    QuantidadeALiquidar = cus.QtdeLiquidar.ToString(),
                    QuantidadeTotal = cus.QtdeDATotal.ToString()
                });
            });

            return lRetorno;
        }

        //public List<TransporteCustodia> TraduzirListaDescobertos(List<Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo> info)
        public List<TransporteCustodia> TraduzirListaDescobertos(List<Gradual.OMS.Monitor.Custodia.Lib.Info.MonitorCustodiaInfo.CustodiaPosicao> info)
        {
            var lRetorno = new List<TransporteCustodia>();

            info.ForEach(cus =>
            {
                if (cus.QtdeDATotal < 0)
                {
                    lRetorno.Add(new TransporteCustodia()
                    {
                        //CodigoAssessor = cus.CodigoAssessor.ToString(),
                        //CodigoCliente = cus.CodigoCliente.ToString(),
                        //CodigoAtivo = cus.CodigoAtivo.ToString(),
                        //CodigoCarteira = cus.CodigoCarteira.ToString(),
                        //CodigoMercado = cus.CodigoMercado.ToString(),
                        //QuantidadeDisponivel = cus.QuantidadeDisponivel.ToString(),
                        //QuantidadeD1 = cus.QuantidadeD1.ToString(),
                        //QuantidadeD2 = cus.QuantidadeD2.ToString(),
                        //QuantidadeD3 = cus.QuantidadeD3.ToString(),
                        //QuantidadePendente = cus.QuantidadePendente.ToString(),
                        //QuantidadeALiquidar = cus.QuantidadeALiquidar.ToString(),
                        //QuantidadeTotal = cus.QuantidadeTotal.ToString()
                        
                        //CodigoAssessor = cus.CodigoAssessor.ToString(),
                        CodigoCliente = cus.IdCliente.ToString(),
                        CodigoAtivo = cus.CodigoInstrumento.ToString(),
                        CodigoCarteira = cus.CodigoCarteira.ToString(),
                        CodigoMercado = cus.TipoMercado.ToString(),
                        QuantidadeDisponivel = cus.QtdeDisponivel.ToString(),
                        QuantidadeD1 = cus.QtdeD1.ToString(),
                        QuantidadeD2 = cus.QtdeD2.ToString(),
                        QuantidadeD3 = cus.QtdeD3.ToString(),
                        //QuantidadePendente = cus.QuantidadePendente.ToString(),
                        QuantidadeALiquidar = cus.QtdeLiquidar.ToString(),
                        QuantidadeTotal = cus.QtdeDATotal.ToString()

                    });
                }
            });

            return lRetorno;
        }
    }
}