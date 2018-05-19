using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Gradual.OMS.Library;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Classe que armazena a Posição do cliente intraday 
    /// </summary>
    [Serializable]
    public class PosicaoClienteIntradayInfo
    {
        /// <summary>
        /// Código do cliente
        /// </summary>
        [DataMember]
        public int CodigoCliente { set; get; }

        /// <summary>
        /// Código do instrumento
        /// </summary>
        [DataMember]
        public string Instrumento { set; get; }

        /// <summary>
        /// Código do instrumento Opção
        /// </summary>
        [DataMember]
        public string InstrumentoOpcao { set; get; }

        /// <summary>
        /// Código do assessor
        /// </summary>
        [DataMember]
        public string TipoMercado { set; get; }

        /// <summary>
        /// Código do assessor
        /// </summary>
        public string Assessor { set; get; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        [DataMember]
        public string NomeCliente { set; get; }

        /// <summary>
        /// Quantidade 
        /// </summary>
        [DataMember]
        public int Quantidade { set; get; }
        
        /// <summary>
        /// Preço Negociação Opção
        /// </summary>
        [DataMember]
        public decimal PrecoNegocioOpcao { get; set; }

        /// <summary>
        /// Preço médio
        /// </summary>
        [DataMember]
        public decimal PrecoMedioNegocio { set; get; }

        /// <summary>
        /// Valor da operação ( Preco x Quantidade )
        /// </summary>

        [DataMember]
        public decimal VolumeOperacao { set; get; }

        /// <summary>
        /// Sentido da operação ( Compra / Venda )
        /// </summary>
        [DataMember]
        public char SentidoOperacao { set; get; }

        /// <summary>
        /// Volume do negocio valorizado a mercado
        /// </summary>
        [DataMember]
        public decimal PrecoNegocioAtual { set; get; }

        /// <summary>
        /// Lucro ou Prejuiso da operação
        /// </summary>
        [DataMember]
        public decimal LucroPrejuiso { set; get; }

        /// <summary>
        /// Porta da operação
        /// </summary>
        [DataMember]
        public string Porta { set; get; }
    }
}
