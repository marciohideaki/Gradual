using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Representa um sistema cliente.
    /// Quando o usuário realizar a autenticação, ele deverá informar de qual 
    /// sistema cliente ele está se conectando.
    /// Exemplos de sistemas cliente: hb, plataforma, robo, link externo, etc.
    /// </summary>
    [Serializable]
    public class SistemaClienteInfo : ICodigoEntidade
    {
        /// <summary>
        /// Chave primária do sistema cliente.
        /// </summary>
        public string CodigoSistemaCliente { get; set; }

        /// <summary>
        /// Nome do sistema cliente.
        /// </summary>
        public string NomeSistemaCliente { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public SistemaClienteInfo()
        {
            this.CodigoSistemaCliente = Guid.NewGuid().ToString();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoSistemaCliente;
        }

        #endregion
    }
}
