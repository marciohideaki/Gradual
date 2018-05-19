using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM
{
    public class TransporteRelatorio_004
    {
        public string CodigoNomeCliente { get; set; }

        public string Mes { get; set; }

        public string Corretagem { get; set; }

        public string Volume { get; set; }

        public List<TransporteRelatorio_004> TraduzirLista(List<LTVDoClienteInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_004>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(ltv =>
                {
                    lRetorno.Add(new TransporteRelatorio_004()
                    {
                        CodigoNomeCliente = string.Format("{0} {1}", ltv.CodigoCliente.ToCodigoClienteFormatado(), ltv.NomeCliente.ToStringFormatoNome()),
                        Corretagem = ltv.ValorCorretagemPorPeriodo.ToString("N2"),
                        Mes = ltv.MesNegocio,
                        Volume = ltv.ValorVolumePorPeriodo.ToString("N2"),
                    });
                });

            return lRetorno;
        }
    }
}