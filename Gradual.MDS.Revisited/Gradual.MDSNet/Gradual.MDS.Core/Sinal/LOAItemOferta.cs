using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Core.Lib;

namespace Gradual.MDS.Core.Sinal
{
    public class LOAItemOferta
    {
        public int Acao  { get; set; }
        public int Indice { get; set; }
        public Decimal Preco { get; set; }
        public long Quantidade { get; set; }
        public long QtdeOrdens {get;set;}

        public static Dictionary<String, String> montarRegistroAgregado(LOAItemOferta itemOferta, int casasDecimais)
        {
            Dictionary<String, String> oferta = new Dictionary<String, String>();

            String preco = "";
            if (itemOferta.Preco == ConstantesMDS.PRECO_LIVRO_OFERTAS_MIN_VALUE ||
                    itemOferta.Preco == ConstantesMDS.PRECO_LIVRO_OFERTAS_MAX_VALUE)
            {
                preco = ConstantesMDS.DESCRICAO_OFERTA_ABERTURA;
            }
            else
            {
                if (itemOferta.Preco != Decimal.Zero)
                {
                    preco = String.Format("{0:f" + casasDecimais + "}", itemOferta.Preco).Replace('.', ',');
                }
            }

            oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_ACAO,
                    String.Format("{0:d}", itemOferta.Acao));
            oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_POSICAO,
                    String.Format("{0:d}", itemOferta.Indice));
            oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_PRECO,
                    preco);
            oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_QUANTIDADE,
                    itemOferta.Quantidade == 0 ? "" : String.Format("{0:d}", itemOferta.Quantidade));

            if (itemOferta.Acao == ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR && itemOferta.QtdeOrdens <= 0)
                oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_QTDE_ORDENS, "N/D");
            else
                oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_QTDE_ORDENS,
                    itemOferta.QtdeOrdens == 0 ? "" : String.Format("{0:d}", itemOferta.QtdeOrdens));

            return oferta;
        }
    }
}
