using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.Monitores.Risco.Lib;

namespace Gradual.Monitores.Risco.Info
{
    /// <summary>
    /// Classe de dados de clientes para retornar informações de clientes no SINACOR
    /// </summary>
    [DataContract]
    [Serializable]
    public class ObterDadosClienteResponse
    {
        /// <summary>
        /// Parametro de dados do cliente com informações do SINACOR
        /// </summary>
        [DataMember]
        public ClienteInfo ClienteInfos { get; set; }
    }
}
