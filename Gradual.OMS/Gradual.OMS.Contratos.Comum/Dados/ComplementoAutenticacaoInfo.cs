using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Classe contendo informações de um complemento de autenticação
    /// </summary>
    public class ComplementoAutenticacaoInfo
    {
        /// <summary>
        /// Indica se o complemento está habilitado
        /// </summary>
        public bool Habilitado { get; set; }

        /// <summary>   
        /// Indica o tipo do complemento
        /// </summary>
        [XmlIgnore]
        public Type TipoComplemento 
        {
            get { return Type.GetType(this.TipoComplementoString); }
            set { this.TipoComplementoString = value.FullName + ", " + value.Assembly.FullName; } 
        }

        /// <summary>
        /// Propriedade auxiliar para serialização
        /// </summary>
        [XmlElement("TipoComplemento")]
        public string TipoComplementoString { get; set; }
    }
}
