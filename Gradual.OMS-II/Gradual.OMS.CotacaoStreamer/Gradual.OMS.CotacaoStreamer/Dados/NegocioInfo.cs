using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    public class NegocioInfo
    {
        /// <summary>
        /// Codigo do Instrumento
        /// </summary>
        public string Instrumento { get; set; }

        /// <summary>
        /// Tipo de Bolsa ('BOV':Bovespa, 'BMF':BM&F)
        /// </summary>
        public string TipoBolsa { get; set; }

        /// <summary>
        /// Data e hora do negocio
        /// </summary>
        public DateTime DataHora { get; set; }

        /// <summary>
        /// Corretora Compradora
        /// </summary>
        public int Compradora { get; set; }

        /// <summary>
        /// Corretora Vendedora
        /// </summary>
        public int Vendedora { get; set; }

        /// <summary>
        /// Preco do Negocio
        /// </summary>
        public double Preco { get; set; }

        /// <summary>
        /// Quantidade do Negocio
        /// </summary>
        public double Quantidade { get; set; }

        /// <summary>
        /// Valor maximo no dia
        /// </summary>
        public double Maxima { get; set; }

        /// <summary>
        /// Valor minimo no dia / intervalo
        /// </summary>
        public double Minima { get; set; }

        /// <summary>
        /// Volume financeiro acumulado no dia
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// Quantidade de negocios no dia
        /// </summary>
        public int NumeroNegocio { get; set; }

        /// <summary>
        /// Variação do Instrumento
        /// </summary>
        public double Variacao { get; set; }

        /// <summary>
        /// Estado do Instrumento
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Quantidade total no topo do livro compra
        /// </summary>
        public double MelhorQtdeCompra { get; set; }

        /// <summary>
        /// Quantidade total no topo do livro venda
        /// </summary>
        public double MelhorQtdeVenda { get; set; }

        /// <summary>
        /// Quantidade de papeis negociados no dia
        /// </summary>
        public double QuantidadeNegociadaDia { get; set; }


        /// <summary>
        /// Preco medio intraday
        /// </summary>
        public double PrecoMedio { get; set; }


        /// <summary>
        /// Preco teorico de abertura (leilao)
        /// </summary>
        public double PrecoTeoricoAbertura { get; set; }

        /// <summary>
        /// Variacao do preco teorico de abertura sobre o fechamento do dia anterior
        /// </summary>
        public double VariacaoTeorica { get; set; }

        /// <summary>
        /// Horario para termino da prorrogacao do leilao
        /// </summary>
        public DateTime HorarioTeorico { get; set; }
    }
}
