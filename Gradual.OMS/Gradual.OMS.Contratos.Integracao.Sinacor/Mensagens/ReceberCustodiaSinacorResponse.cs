using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.Dados;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de custodia do sinacor
    /// </summary>
    public class ReceberCustodiaSinacorResponse : MensagemResponseBase
    {
        /// <summary>
        /// Resultado das posições de custodia encontradas
        /// </summary>
        public List<CustodiaSinacorPosicaoInfo> Resultado { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberCustodiaSinacorResponse()
        {
            this.Resultado = new List<CustodiaSinacorPosicaoInfo>();
        }
    }
}
