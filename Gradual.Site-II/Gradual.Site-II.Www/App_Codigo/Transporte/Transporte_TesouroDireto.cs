using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.TesouroDireto.Lib.Dados;

namespace Gradual.Site.Www
{
    public class Transporte_TesouroDireto
    {
        #region Propriedades

        public string CodigoTitulo { get; set; }

        public string TipoNome { get; set; }

        public string NomeTitulo { get; set; }

        public string IndexadorNome { get; set; }

        public string ValorTaxaCompra { get; set; }

        public string ValorCompra { get; set; }

        public string DescricaoTitulo { get; set; }

        public string Vencimento { get; set; }

        #endregion

        public List<Transporte_TesouroDireto> TraduzirLista(List<TituloMercadoInfo> pParametros)
        {
            var lRetorno = new List<Transporte_TesouroDireto>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(lTituloMercado =>
                {
                    lRetorno.Add(new Transporte_TesouroDireto()
                    {
                        CodigoTitulo = lTituloMercado.CodigoTitulo.ToString(),
                        Vencimento = lTituloMercado.DataVencimento.ToString("dd/MM/yyyy"),
                        TipoNome = lTituloMercado.Tipo.Nome,
                        NomeTitulo = lTituloMercado.NomeTitulo,
                        IndexadorNome = lTituloMercado.Indexador.Nome,
                        ValorTaxaCompra = lTituloMercado.ValorTaxaCompra.ToString("N2"),
                        ValorCompra = lTituloMercado.ValorCompra.ToString("C2"),
                        DescricaoTitulo = lTituloMercado.DescricaoTitulo,
                    });
                });

            return lRetorno;
        }
    }
}