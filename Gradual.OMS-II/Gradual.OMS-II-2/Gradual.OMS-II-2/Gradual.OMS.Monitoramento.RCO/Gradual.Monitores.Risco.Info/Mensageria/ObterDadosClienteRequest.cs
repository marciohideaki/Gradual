using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Monitores.Risco.Info
{
    /// <summary>
    /// Classe para Dados de request de métodos de Clientes
    /// </summary>
    [DataContract]
    [Serializable]
    public class ObterDadosClienteRequest
    {
        /// <summary>
        /// Codigo do cliente para buscar informações
        /// </summary>
        [DataMember]
        public int CodigoCliente { get; set; }
    }
}
