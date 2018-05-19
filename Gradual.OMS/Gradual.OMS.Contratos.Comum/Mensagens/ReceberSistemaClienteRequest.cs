using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de detalhe de sistema cliente
    /// </summary>
    [Serializable]
    public class ReceberSistemaClienteRequest : MensagemRequestBase
    {
        /// <summary>
        /// Codigo do sistema cliente desejado
        /// </summary>
        public string CodigoSistemaCliente { get; set; }
    }
}
