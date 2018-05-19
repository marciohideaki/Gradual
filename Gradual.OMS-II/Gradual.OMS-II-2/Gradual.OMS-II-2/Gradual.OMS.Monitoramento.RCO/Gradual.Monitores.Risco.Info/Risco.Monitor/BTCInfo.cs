using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Classes que armazenam operações BTC - Aluguel de Ações
    /// </summary>
    [Serializable]
    public class BTCInfo
    {
        /// <summary>
        /// Código de Cliente
        /// </summary>
        [DataMember]
        public int CodigoCliente { set; get; }

        /// <summary>
        /// Instrumento
        /// </summary>
        [DataMember]
        public string Instrumento { set; get; }

        /// <summary>
        /// Tipo de Contrato
        /// </summary>
        [DataMember]
        public string TipoContrato { set; get; }

        /// <summary>
        /// Data de abertura da operação btc
        /// </summary>
        [DataMember]
        public DateTime DataAbertura { set; get; }

        /// <summary>
        /// Data do vencimento do btc
        /// </summary>
        [DataMember]
        public DateTime DataVencimento { set; get; }

        /// <summary>
        /// Código da Carteira
        /// </summary>
        [DataMember]
        public int Carteira { set; get; }

        /// <summary>
        /// Quantidade 
        /// </summary>
        [DataMember]
        public int Quantidade { set; get; }

        /// <summary>
        /// Preço médio da operação BTC
        /// </summary>
        [DataMember]
        public decimal PrecoMedio { set; get; }

        /// <summary>
        /// Taxa da Operação BTC
        /// </summary>
        [DataMember]
        public decimal Taxa { set; get; }

        /// <summary>
        /// Preço de Mercado da Operação
        /// </summary>
        [DataMember]
        public decimal PrecoMercado { set; get; }

        /// <summary>
        /// Remuneração da operação BTC
        /// </summary>
        [DataMember]
        public decimal Remuneracao { set; get; }
    }

}
