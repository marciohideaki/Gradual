using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.MDS.Core.Sinal
{
    public class LOAGrupoOfertas
    {
        public Decimal Preco;
        public long Quantidade;
        public int Indice;
        public int PosicaoInicialOfertas;
        public String PrecoFormatado;
        public String QtdeFormatada;
        public long QtdeOrdens;
        public String QtdeOfertasFormatada;

        public LOAGrupoOfertas() { }

        public LOAGrupoOfertas(Decimal preco2, LOFDadosOferta oferta, int indice)
        {
            this.Preco = preco2;
            this.Quantidade = oferta.Quantidade;
            this.Indice = indice;
            this.QtdeOrdens = 1;
        }

        public static void incluiOferta(LOAGrupoOfertas grupo, LOFDadosOferta oferta)
        {
            grupo.Quantidade = grupo.Quantidade + oferta.Quantidade;
            grupo.QtdeOrdens = grupo.QtdeOrdens + 1;
        }

        public static void alteraOferta(LOAGrupoOfertas grupo, LOFDadosOferta oldoferta, LOFDadosOferta newoferta)
        {
            grupo.Quantidade = grupo.Quantidade - oldoferta.Quantidade;
            grupo.Quantidade = grupo.Quantidade + newoferta.Quantidade;
        }

        public static bool excluiOferta(LOAGrupoOfertas grupo, LOFDadosOferta oferta, long qtdeOrdens)
        {
            grupo.Quantidade = grupo.Quantidade - oferta.Quantidade;
            grupo.QtdeOrdens = grupo.QtdeOrdens - qtdeOrdens;

            // Indica que deve remover esse preco do mapa
            if (grupo.Quantidade <= 0)
            {
                return true;
            }

            //if (grupo.QtdeOrdens <= 0)
            //    grupo.QtdeOrdens = 1;

            return false;
        }
    }
}
