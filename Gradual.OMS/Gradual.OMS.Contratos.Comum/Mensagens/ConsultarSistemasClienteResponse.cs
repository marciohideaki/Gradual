using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de consulta de sistemas cliente
    /// </summary>
    [Serializable]
    public class ConsultarSistemasClienteResponse : MensagemResponseBase
    {
        /// <summary>
        /// Sistemas cliente encontrados
        /// </summary>
        public List<SistemaClienteInfo> SistemasCliente { get; set; }
    }
}
