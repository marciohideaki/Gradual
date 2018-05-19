using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco
{
    public class TransporteRelatorio_003 : ICodigoEntidade
    {
        public string DescricaoParametro { get; set; }

        public string Bolsa { get; set; }

        public List<TransporteRelatorio_003> TraduzirLista(List<RiscoParametrosRelInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_003>();

            if (pParametro != null)
                pParametro.ForEach(delegate(RiscoParametrosRelInfo rpi)
                {
                    lRetorno.Add(new TransporteRelatorio_003()
                    {
                        DescricaoParametro = rpi.DsParametro,
                        Bolsa = rpi.DsBolsa, 
                    });
                });

            return lRetorno;
        }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}