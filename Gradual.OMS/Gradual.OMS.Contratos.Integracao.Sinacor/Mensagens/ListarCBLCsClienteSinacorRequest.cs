using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de lista de cblcs de cliente
    /// </summary>
    public class ListarCBLCsClienteSinacorRequest : MensagemRequestBase
    {
        /// <summary>
        /// CPF/CNPJ do cliente
        /// </summary>
        public string CodigoCPFCNPJ { get; set; }

        /// <summary>
        /// Código CBLC do cliente
        /// </summary>
        public string CodigoCBLC { get; set; }
    }
}
