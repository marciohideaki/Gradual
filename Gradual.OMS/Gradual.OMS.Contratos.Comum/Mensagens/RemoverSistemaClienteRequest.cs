using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de remoção de cliente
    /// </summary>
    [Serializable]
    public class RemoverSistemaClienteRequest : MensagemRequestBase
    {
        /// <summary>
        /// Codigo do sistema cliente a remover
        /// </summary>
        public string CodigoSistemaCliente { get; set; }
    }
}
