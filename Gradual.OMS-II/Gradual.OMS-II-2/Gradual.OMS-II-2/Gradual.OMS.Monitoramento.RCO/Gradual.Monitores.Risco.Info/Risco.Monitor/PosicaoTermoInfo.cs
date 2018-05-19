using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Classe que armazena a posição de termo
    /// </summary>
    public class PosicaoTermoInfo
    {
        /// <summary>
        /// Codigo do Cliente
        /// </summary>
        public int IDCliente { set; get; }

        /// <summary>
        /// Instrumento do termo
        /// </summary>
        public string Instrumento { set; get; }

        /// <summary>
        /// Data de Execução 
        /// </summary>
        public DateTime DataExecucao { set; get; }

        /// <summary>
        /// Data de vencimento do termo
        /// </summary>
        public DateTime DataVencimento { set; get; }

        /// <summary>
        /// Quantidade 
        /// </summary>
        public int Quantidade { set; get; }

        /// <summary>
        /// Preço de execução do termo
        /// </summary>
        public decimal PrecoExecucao { set; get; }

        /// <summary>
        /// Preco de mercado do termo
        /// </summary>
        public decimal PrecoMercado { set; get; }

        /// <summary>
        /// Lucro Prejuízo do termo
        /// </summary>
        public decimal LucroPrejuizo { set; get; }

    }
}
