using System.Collections.Generic;
using Gradual.OMS.TesouroDireto.Lib.Dados;

namespace Gradual.Site.Www
{
    public class Transporte_TesouroDireto_Extrato
    {
        #region Estruturas

        public struct Transporte_TesouroDireto_Extrato_Detalhes
        {
            public string NomeTitulo { get; set; }
            public string DataCompra { get; set; }
            public string ValorTitulo { get; set; }
            public string ValorTaxaCBLC { get; set; }
            public string ValorTaxaAC { get; set; }
            public string QuantidadeBloqueada { get; set; }
            public string QuantidadeCompra { get; set; }
            public string QuantidadeCredito { get; set; }
            public string QuantidadeDebito { get; set; }
        }

        #endregion

        #region Propriedades

        public string TituloNome { get; set; }

        public string CodigoTitulo { get; set; }

        public string DataVencimento { get; set; }

        public string SaldoAnterior { get; set; }

        public string QuantidadeCredito { get; set; }

        public string QuantidadeDebito { get; set; }

        public string QuantidadeBloqueada { get; set; }

        public string ValorBase { get; set; }

        public string ValorTaxaDevida { get; set; }

        public string SaldoAtual { get; set; }

        public string ValorAtual { get; set; }

        public string TituloValor { get; set; }

        #endregion

        #region Métodos

        public List<Transporte_TesouroDireto_Extrato> TraduzirLista(List<TituloMercadoInfo> pParametros)
        {
            var lRetorno = new List<Transporte_TesouroDireto_Extrato>();

            if (null != pParametros && pParametros.Count > 0)
            {
                for (int i = 0; i < pParametros.Count; i++ )
                {
                    var lTrans = new Transporte_TesouroDireto_Extrato();
                        
                    var lTituloMercado=  pParametros[i];

                    lTrans.TituloNome          = lTituloMercado.NomeTitulo;
                    lTrans.CodigoTitulo        = lTituloMercado.CodigoTitulo.ToString();
                    lTrans.DataVencimento      = lTituloMercado.DataVencimento.ToString("dd/MM/yyyy");
                    lTrans.SaldoAnterior       = lTituloMercado.SaldoAnterior.ToString("N2");
                    lTrans.QuantidadeCredito   = lTituloMercado.QuantidadeCredito.ToString("N2");
                    lTrans.QuantidadeDebito    = lTituloMercado.QuantidadeDebito.ToString("N2");
                    lTrans.QuantidadeBloqueada = lTituloMercado.QuantidadeBloqueada.ToString("N2");
                    lTrans.ValorBase           = lTituloMercado.ValorBase.ToString("N2");
                    lTrans.ValorTaxaDevida     = lTituloMercado.ValorTaxaDevida.ToString("N2");
                    lTrans.TituloValor         = lTituloMercado.ValorBase.DBToDouble().ToString("N2"); //((lTituloMercado.ValorBase.DBToDouble() - lTituloMercado.SaldoAnterior.DBToDouble()) * (lTituloMercado.SaldoAnterior.DBToDouble() - lTituloMercado.QuantidadeDebito.DBToDouble() + lTituloMercado.QuantidadeCredito.DBToDouble())).ToString("N2");
                    lTrans.SaldoAtual          = (lTituloMercado.SaldoAnterior.DBToDouble() - lTituloMercado.QuantidadeDebito.DBToDouble() + lTituloMercado.QuantidadeCredito).ToString("N2");
                    lTrans.ValorAtual          = (decimal.Parse(lTrans.TituloValor) * decimal.Parse(lTrans.SaldoAtual)).ToString("N2");

                    if (lTrans.ValorAtual == "0,00") continue;

                    lRetorno.Add(lTrans);
                }
            }
            return lRetorno;
        }

        public List<Transporte_TesouroDireto_Extrato_Detalhes> TraduzirListaDetalhes(List<TituloMercadoInfo> pParametros)
        {
            var lRetorno = new List<Transporte_TesouroDireto_Extrato_Detalhes>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(tmi =>
                {
                    lRetorno.Add(new Transporte_TesouroDireto_Extrato_Detalhes()
                    {
                        DataCompra = tmi.DataCompra.ToString("dd/MM/yyyy HH:mm"),
                        NomeTitulo = tmi.NomeTitulo.ToUpper(),
                        QuantidadeBloqueada = tmi.QuantidadeBloqueada.ToString("N2"),
                        QuantidadeCompra = tmi.QuantidadeCompra.ToString("N2"),
                        QuantidadeCredito = tmi.QuantidadeCredito.ToString("N2"),
                        QuantidadeDebito = tmi.QuantidadeDebito.ToString("N2"),
                        ValorTaxaAC = tmi.ValorTaxaAC.ToString("N2"),
                        ValorTaxaCBLC = tmi.ValorTaxaCBLC.ToString("N2"),
                        ValorTitulo = tmi.ValorTitulo.ToString("C2"),
                    });
                });

            return lRetorno;
        }

        #endregion
    }
}