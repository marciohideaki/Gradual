using System;
using System.Collections.Generic;
using System.Globalization;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro
{
    public class TransporteExtratoNotaDeCorretagem
    {
        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        public string NomeBolsa { get; set; }

        public string TipoOperacao { get; set; }

        public string TipoMercado { get; set; }

        public string EspecificacaoTitulo { get; set; }

        public string Observacao { get; set; }

        public string Quantidade { get; set; }

        public string ValorNegocioPosNeg { get; set; }

        public string ValorNegocio { get; set; }

        public string ValorTotalPosNeg { get; set; }

        public string ValorTotal { get; set; }

        public string DC { get; set; }

        public List<TransporteExtratoNotaDeCorretagem> TraduzirLista(List<NotaDeCorretagemExtratoInfo> pParametros)
        {
            var lRetorno = new List<TransporteExtratoNotaDeCorretagem>();

            if (null != pParametros)
                pParametros.ForEach(delegate(NotaDeCorretagemExtratoInfo nce)
                {
                    lRetorno.Add(
                        new TransporteExtratoNotaDeCorretagem()
                        {
                            NomeBolsa = nce.NomeBolsa,
                            TipoOperacao = nce.TipoOperacao,
                            DC = nce.DC,
                            EspecificacaoTitulo = string.Concat(nce.CodigoNegocio, " ", nce.EspecificacaoTitulo),
                            Quantidade = nce.Quantidade.ToString("N0", gCultureInfo),
                            Observacao = string.IsNullOrWhiteSpace(nce.Observacao) ? string.Empty : nce.Observacao.Replace("N", string.Empty),
                            TipoMercado = nce.TipoMercado,
                            ValorNegocio = nce.ValorNegocio.ToString("N2", gCultureInfo),
                            ValorNegocioPosNeg = nce.ValorNegocio >= 0 ? "ValorPositivo" : "ValorNegativo",
                            ValorTotal = Math.Abs(nce.ValorTotal).ToString("N2", gCultureInfo),
                            ValorTotalPosNeg = nce.ValorTotal >= 0 ? "ValorPositivo" : "ValorNegativo",
                        });
                });

            return lRetorno;
        }
    }
}