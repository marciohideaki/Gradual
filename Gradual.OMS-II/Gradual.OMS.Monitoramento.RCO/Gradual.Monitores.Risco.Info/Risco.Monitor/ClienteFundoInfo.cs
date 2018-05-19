using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Classe que armazena os dados de fundos do cliente
    /// </summary>
    [Serializable]
    public class ClienteFundoInfo
    {
        /// <summary>
        /// Codigo do Cliente 
        /// </summary>
        [DataMember]
        public int IdCliente { set; get; }

        /// <summary>
        /// Nome do fundo
        /// </summary>
        [DataMember]
        public string NomeFundo { set; get; }

        /// <summary>
        /// Saldo do fundo
        /// </summary>
        [DataMember]
        public decimal Saldo { set; get; }
    }

    /// <summary>
    /// Classe que armazena os dados de clubes do cliente
    /// </summary>
    [Serializable]
    public class ClienteClubeInfo
    {
        /// <summary>
        /// Código do cliente
        /// </summary>
        [DataMember]
        public int IdCliente { set; get; }

        /// <summary>
        /// Nome do Clube
        /// </summary>
        [DataMember]
        public string NomeClube { set; get; }

        /// <summary>
        /// Saldo do clube
        /// </summary>
        [DataMember]
        public decimal Saldo { set; get; }
    }
}
