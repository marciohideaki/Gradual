using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM
{
    public class TransporteRelatorio_007
    {
        public string Cliente { get; set; }

        public string DataLanc { get; set; }

        public string DataRef { get; set; }

        public string DataLiq { get; set; }

        public string Lancamento { get; set; }

        public string Valor { get; set; }

        public List<TransporteRelatorio_007> TraduzirLista(List<MovimentoDeContaCorrenteInfo> pParametro)
        {
            var lRetorno = new List<TransporteRelatorio_007>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(mcc =>
                {
                    lRetorno.Add(new TransporteRelatorio_007()
                    {
                        Cliente = string.Concat(mcc.CdCliente.ToCodigoClienteFormatado(), " ", mcc.NmCliente.ToStringFormatoNome()),
                        DataLanc = mcc.DtLancamento.ToString("dd/MM/yyyy"),
                        DataLiq = mcc.DtLiquidacao.ToString("dd/MM/yyyy"),
                        DataRef = mcc.DtReferencia.ToString("dd/MM/yyyy"),
                        Lancamento = mcc.DsLancamento,
                        Valor = mcc.VlLancamento.ToString("N2"),
                    });
                });

            return lRetorno;
        }
    }
}