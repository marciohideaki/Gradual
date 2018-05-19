using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    public class CorretoraInfo
    {
        /// <summary>
        /// Posicao da corretora no ranking
        /// </summary>
        [DataMember]
        public string r { get; set; }

        /// <summary>
        /// Codigo da Corretora
        /// </summary>
        [DataMember]
        public string c { get; set; }

        /// <summary>
        /// Volume de Compras
        /// </summary>
        [DataMember]
        public string vc { get; set; }

        /// <summary>
        /// Porcentagem do Volume de Compras
        /// </summary>
        [DataMember]
        public string pc { get; set; }

        /// <summary>
        /// Volume de Vendas
        /// </summary>
        [DataMember]
        public string vv { get; set; }

        /// <summary>
        /// Porcentagem do Volume de Vendas
        /// </summary>
        [DataMember]
        public string pv { get; set; }

        /// <summary>
        /// Volume Bruto
        /// </summary>
        [DataMember]
        public string vb { get; set; }

        /// <summary>
        /// Volume Liquido
        /// </summary>
        [DataMember]
        public string vl { get; set; }
    }
}
