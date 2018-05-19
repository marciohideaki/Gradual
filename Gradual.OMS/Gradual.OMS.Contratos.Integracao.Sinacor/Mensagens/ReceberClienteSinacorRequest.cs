using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de cliente para o sinacor
    /// </summary>
    [Serializable]
    public class ReceberClienteSinacorRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código CBLC do cliente a ser recuperado
        /// </summary>
        public string CodigoCBLC { get; set; }

        /// <summary>
        /// Código do assessor do cliente
        /// </summary>
        public string CodigoAssessor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Nome { get; set; }
    }
}
