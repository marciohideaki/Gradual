using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente
{
    public class TransporteRelatorio_014
    {
        public string ClienteCodigo { get; set; }

        public string ClienteNome { get; set; }

        public string TipoCliente { get; set; }

        public string Assessor { get; set; }

        public string CodNeg { get; set; }

        public string Carteira { get; set; }

        public string Locador { get; set; }

        public string Total { get; set; }

        public string Disponivel { get; set; }

        public string D1 { get; set; }

        public string D2 { get; set; }

        public string D3 { get; set; }

        public List<TransporteRelatorio_014> TraduzirLista(List<PosicaoConsolidadaPorPapelRelInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_014>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(pcp =>
                {
                    lRetorno.Add(new TransporteRelatorio_014()
                    {
                        Assessor = string.Concat(pcp.AssessorCodigo.ToString(), " - ", pcp.AssessorNome.ToStringFormatoNome()),
                        Carteira = pcp.DescricaoCarteira.ToStringFormatoNome(),
                        ClienteCodigo = pcp.ClienteCodigo.ToCodigoClienteFormatado(),
                        ClienteNome = pcp.ClienteNome.ToStringFormatoNome(),
                        CodNeg = pcp.CodigoNegocio.ToUpper(),
                        D1 = pcp.QuantidadeD1.ToString("N0"),
                        D2 = pcp.QuantidadeD2.ToString("N0"),
                        D3 = pcp.QuantidadeD3.ToString("N0"),
                        Disponivel = pcp.QuantidadeDisponivel.ToString("N2"),
                        Locador = pcp.Locador.DBToString(),
                        TipoCliente = pcp.ClienteTipo.ToStringFormatoNome(),
                        Total = pcp.QuantidadeTotal.ToString("N2"),
                    });
                });

            return lRetorno;
        }
    }
}