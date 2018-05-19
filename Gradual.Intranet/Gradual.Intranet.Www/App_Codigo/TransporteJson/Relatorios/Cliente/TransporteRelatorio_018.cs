using System.Collections.Generic;
using System.Globalization;
using Gradual.OMS.ContaCorrente.Lib.Info;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente
{
    public class TransporteRelatorio_018
    {
        #region | Propriedade

        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        public string codigo { get; set; }

        public string nome { get; set; }

        public CabecalhoDoExtrato CabecalhoExtrato { get; set; }

        public List<DetalhesDoExtrato> ListaDetalhesExtrato { get; set; }

        #endregion

        #region | Estrutura

        public struct CabecalhoDoExtrato
        {
            public string NomeCliente { get; set; }

            public string SaldoAnterior { get; set; }
        }

        public struct DetalhesDoExtrato
        {
            public string Mov { get; set; }

            public string Liq { get; set; }

            public string Historico { get; set; }

            public string Debito { get; set; }

            public string Credito { get; set; }

            public string Saldo { get; set; }
        }

        #endregion

        #region | Construtores

        public TransporteRelatorio_018()
        {
            this.CabecalhoExtrato = new CabecalhoDoExtrato();

            this.ListaDetalhesExtrato = new List<DetalhesDoExtrato>();
        }

        #endregion

        public List<TransporteRelatorio_018> TraduzirLista(List<ContaCorrenteExtratoInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_018>();
            var lTransporteRelatorio = new TransporteRelatorio_018();

            if (null != pParametros && pParametros.Count > 0)
                foreach (ContaCorrenteExtratoInfo cce in pParametros)
                {
                    lTransporteRelatorio = new TransporteRelatorio_018();

                    lTransporteRelatorio.CabecalhoExtrato = new CabecalhoDoExtrato()
                    {
                        NomeCliente = string.Format("{0} {1}", cce.CodigoCliente, cce.NomeCliente.ToStringFormatoNome()),
                        SaldoAnterior = cce.SaldoAnterior.ToString("N2", this.gCultureInfo),
                    };

                    if (null != cce.ListaContaCorrenteMovimento && cce.ListaContaCorrenteMovimento.Count > 0)
                        cce.ListaContaCorrenteMovimento.ForEach(ccm =>
                        {
                            lTransporteRelatorio.ListaDetalhesExtrato.Add(new DetalhesDoExtrato()
                            {
                                Credito = ccm.ValorCredito.ToString("N2", this.gCultureInfo),
                                Debito = ccm.ValorDebito.ToString("N2", this.gCultureInfo),
                                Historico = ccm.Historico.DBToString().Trim(),
                                Liq = ccm.DataLiquidacao.ToString("dd/MM/yyyy"),
                                Mov = ccm.DataMovimento.ToString("dd/MM/yyyy"),
                                Saldo = ccm.ValorSaldo.ToString("N2", this.gCultureInfo),
                            });
                        });

                    lRetorno.Add(lTransporteRelatorio);
                }

            return lRetorno;
        }

        public List<TransporteRelatorio_018> TraduzirListaConsulta(List<Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClientePorAssessorInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_018>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(lcpi =>
                {
                    lRetorno.Add(new TransporteRelatorio_018()
                    {
                        codigo = lcpi.CdCodigoBovespa.DBToString(),
                        nome = string.Format("{0} {1}", lcpi.CdCodigoBovespa.ToCodigoClienteFormatado(), lcpi.DsNome.ToStringFormatoNome()),
                    });
                });

            return lRetorno;
        }
    }
}