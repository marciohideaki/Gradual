using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM
{
    public class TransporteRelatorio_006
    {
        public string CodigoCliente { get; set; }

        public string NomeCliente { get; set; }

        public string Total { get; set; }

        public string ALiquidar { get; set; }

        public string Disponivel { get; set; }

        public string D1 { get; set; }

        public string D2 { get; set; }

        public List<TransporteRelatorio_006> TraduzirLista(List<SaldoProjecoesEmContaCorrenteInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_006>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(spc =>
                {
                    lRetorno.Add(new TransporteRelatorio_006()
                    {
                        ALiquidar = spc.VlALiquidar.ToString("N2"),
                        CodigoCliente = spc.CdCliente.ToString(),
                        D1 = spc.VlProjetado1.ToString("N2"),
                        D2 = spc.VlProjetado2.ToString("N2"),
                        Disponivel = spc.VlDisponivel.ToString("N2"),
                        NomeCliente = spc.NmCliente.ToStringFormatoNome(),
                        Total = spc.VlTotal.ToString("N2"),
                    });
                });

            return lRetorno;
        }
    }
}