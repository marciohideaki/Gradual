using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Risco.Regra.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteBloqueioInstrumento
    {
        public string DsInstrumento { get; set; }

        public string DsDirecao { get; set; }

        public string[] Ativos { get; set; }

        public string[] Direcoes { get; set; }

        public List<TransporteBloqueioInstrumento> TraduzirLista(List<BloqueioInstrumentoInfo> pParametro)
        {
            var lRetorno = new List<TransporteBloqueioInstrumento>();

            if (null != pParametro)
                pParametro.ForEach(delegate(BloqueioInstrumentoInfo bii)
                {
                    lRetorno.Add(new TransporteBloqueioInstrumento()
                    {
                        DsDirecao = "V".Equals(bii.Direcao) ? "Venda" : "C".Equals(bii.Direcao) ? "Compra" : "Ambos",
                        DsInstrumento = bii.CdAtivo.ToLower().ToUpper(),
                    });
                });

            return lRetorno;
        }
    }
}