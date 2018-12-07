using System.Collections.Generic;
using Gradual.OMS.TesouroDireto.Lib.Dados;

namespace Gradual.Site.Www
{
    public class Transporte_TesouroDireto_Compra
    {
        #region Estruturas

        public struct TesouroDireto_Compra
        {
            public string CodigoTitulo { get; set; }

            public string TipoNome { get; set; }

            public string TituloNome { get; set; }

            public string DataVencimento { get; set; }

            public string IndiceNome { get; set; }

            public string ValorTaxaCompra { get; set; }

            public string ValorCompra { get; set; }
        }

        public struct TesouroDireto_Cesta
        {
            public string CodigoTitulo { get; set; }

            public string TipoTitulo { get; set; }

            public string NomeTitulo { get; set; }

            public string DataVencimento { get; set; }

            public string QuantidadeCompra { get; set; }

            public string ValorCBLC { get; set; }

            public string ValorAC { get; set; }

            public string ValorAposPgtoTaxas { get; set; }

            public string ValorCompra { get; set; }

            public string ValorTitulo { get; set; }
        }

        #endregion

        #region Métodos

        public List<TesouroDireto_Compra> TraduzirLista(List<TituloMercadoInfo> pParametro)
        {
            var lRetorno = new List<TesouroDireto_Compra>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(lTitulo =>
                {
                    lRetorno.Add(new TesouroDireto_Compra()
                    {
                        CodigoTitulo = lTitulo.CodigoTitulo.ToString(),
                        TipoNome = lTitulo.Tipo.Nome,
                        TituloNome = lTitulo.NomeTitulo,
                        DataVencimento = lTitulo.DataVencimento.ToString("dd/MM/yyyy"),
                        IndiceNome = lTitulo.Indexador.Nome,
                        ValorTaxaCompra = lTitulo.ValorTaxaCompra.ToString("N2"),
                        ValorCompra = lTitulo.ValorCompra.ToString("C2"),
                    });
                });

            return lRetorno;
        }

        public List<TesouroDireto_Cesta> TraduzirLista(List<CompraConsultaCestaItemInfo> pParametro)
        {
            var lRetorno = new List<TesouroDireto_Cesta>();

            if (null != pParametro && pParametro.Count > 0)
                pParametro.ForEach(ccc =>
                {
                    lRetorno.Add(new TesouroDireto_Cesta()
                    {
                        CodigoTitulo = ccc.CodigoTitulo.ToString(),
                        DataVencimento = ccc.DataVencimento.ToString("dd/MM/yyyy"),
                        NomeTitulo = ccc.NomeTitulo,
                        QuantidadeCompra = ccc.QuantidadeCompra.ToString("N2"),
                        TipoTitulo = ccc.TipoTitulo.ToString(),
                        ValorAC = ccc.ValorAC.ToString("C2"),
                        ValorAposPgtoTaxas = (ccc.ValorCBLC + ccc.ValorAC + (ccc.ValorTitulo * ccc.QuantidadeCompra)).ToString("C2"),
                        ValorCBLC = ccc.ValorCBLC.ToString("C2"),
                        ValorTitulo = ccc.ValorTitulo.ToString("C2"),
                        ValorCompra = (ccc.ValorTitulo * ccc.QuantidadeCompra).ToString("C2"),
                    });
                });

            return lRetorno;
        }

        #endregion
    }
}