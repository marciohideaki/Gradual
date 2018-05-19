using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.Dados;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de cblc´s de cliente
    /// </summary>
    public class ListarCBLCsClienteSinacorResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista dos cblc´s de cliente
        /// </summary>
        public List<ClienteCBLCInfo> ClientesCBLC { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListarCBLCsClienteSinacorResponse()
        {
            this.ClientesCBLC = new List<ClienteCBLCInfo>();
        }
    }
}
