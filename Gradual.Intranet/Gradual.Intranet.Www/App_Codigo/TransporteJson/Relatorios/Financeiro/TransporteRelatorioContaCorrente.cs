using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.ContaCorrente.Lib.Info;
using System.Globalization;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro
{
    public class TransporteExtratoContaCorrente : ITransporteJSON
    {
        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        public string Mov { get; set; }
        public string Liq { get; set; }
        public string Historico { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        public string Saldo { get; set; }

        public List<TransporteExtratoContaCorrente> TraduzirLista(List<ContaCorrenteMovimentoInfo> pParametros)
        {
            var lRetorno = new List<TransporteExtratoContaCorrente>();

            if (null != pParametros)
                pParametros.ForEach(delegate(ContaCorrenteMovimentoInfo ccm)
                {
                    lRetorno.Add(
                        new TransporteExtratoContaCorrente()
                        {
                            Credito = ccm.ValorCredito.ToString("N2", gCultureInfo),
                            Debito = ccm.ValorDebito.ToString("N2", gCultureInfo),
                            Historico = ccm.Historico,
                            Liq = ccm.DataLiquidacao.ToString("dd/MM/yyyy", gCultureInfo),
                            Mov = ccm.DataMovimento.ToString("dd/MM/yyyy", gCultureInfo),
                            Saldo = ccm.ValorSaldo.ToString("N2", gCultureInfo),
                        });
                });

            return lRetorno;
        }

        public string TipoDeItem
        {
            get { return "ContaCorrente"; }
        }

        public int? Id
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public int ParentId
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }
    }
}