using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Orbite.RV.Contratos.MarketData.Dados
{
    /// <summary>
    /// Contem informações sobre uma série.
    /// </summary>
    public class SerieDescricaoInfo
    {
        /// <summary>
        /// Informa o canal que fornece as informações da série
        /// </summary>
        public CanalInfo CanalInfo { get; set; }

        /// <summary>
        /// Código da série
        /// </summary>
        public string CodigoSerie { get; set; }

        /// <summary>
        /// Nome da série
        /// </summary>
        public string NomeSerie { get; set; }

        /// <summary>
        /// Descrição da série
        /// </summary>
        public string DescricaoSerie { get; set; }

        /// <summary>
        /// Indica o tipo da mensagem de request da série.
        /// </summary>
        [XmlIgnore]
        public Type TipoMensagemRequest 
        {
            get { return Type.GetType(this.TipoMensagemRequestString); }
            set { this.TipoMensagemRequestString = value.FullName + ", " + value.Assembly.FullName; }
        }

        /// <summary>
        /// Tipo da mensagem de request.
        /// Utilizado para serialização.
        /// </summary>
        [XmlElement("TipoMensagemRequest")]
        public string TipoMensagemRequestString { get; set; }

        /// <summary>
        /// Tipo da mensagem de response
        /// </summary>
        [XmlIgnore]
        public Type TipoMensagemResponse 
        {
            get { return Type.GetType(this.TipoMensagemResponseString); }
            set { this.TipoMensagemResponseString = value.FullName + ", " + value.Assembly.FullName; }
        }

        /// <summary>
        /// Tipo da mensagem de response
        /// Utilizado para serialização
        /// </summary>
        [XmlElement("TipoMensagemResponse")]
        public string TipoMensagemResponseString { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public SerieDescricaoInfo()
        {
        }
    }
}
