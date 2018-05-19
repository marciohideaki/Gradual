using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Core.Lib;

namespace Gradual.MDS.Core.Sinal
{
    public class LOFDadosOferta
    {
        public String Data { get; set; }
        public String Hora { get; set; }
        public Decimal Preco { get; set; }
        public long Quantidade { get; set; }
        public long Corretora { get; set; }
        public String IDOferta { get; set; }
        public int Posicao { get; set; }
        public bool Leilao { get; set; }

        public LOFDadosOferta() { }

        //public LOFDadosOferta(
        //        String data,
        //        String hora,
        //        Decimal preco,
        //        long quantidade,
        //        long compradora,
        //        long vendedora)
        //{
        //    this.Data = data;
        //    this.Hora = hora;
        //    this.Preco = preco;
        //    this.Quantidade = quantidade;
        //    this.Compradora = compradora;
        //    this.Vendedora = vendedora;
        //}

        public static Dictionary<String, String> montarRegistroOferta(int acao, LOFDadosOferta dadosOferta, int casasDecimais)
        {
            Dictionary<String, String> oferta = new Dictionary<String, String>();

            String preco = "";
            if (dadosOferta.Preco == ConstantesMDS.PRECO_LIVRO_OFERTAS_MIN_VALUE || dadosOferta.Preco == ConstantesMDS.PRECO_LIVRO_OFERTAS_MAX_VALUE)
            {
                preco = ConstantesMDS.DESCRICAO_OFERTA_ABERTURA;
            }
            else
            {
                if (dadosOferta.Preco != Decimal.Zero)
                {
                    preco = String.Format("{0:f" + casasDecimais + "}", dadosOferta.Preco).Replace('.', ',');
                }
            }

            oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_ACAO,
                    String.Format("{0:d}", acao));
            oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_POSICAO,
                    String.Format("{0:d}", dadosOferta.Posicao));
            oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_PRECO,
                    preco);
            oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_QUANTIDADE,
                    dadosOferta.Quantidade == 0 ? "" : String.Format("{0:d}", dadosOferta.Quantidade));
            oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_CORRETORA,
                    dadosOferta.Corretora == 0 ? "" : String.Format("{0:d}", dadosOferta.Corretora));
            oferta.Add(
                    ConstantesMDS.HTTP_OFERTAS_ID,
                    dadosOferta.IDOferta);

            return oferta;
        }
    }
}
