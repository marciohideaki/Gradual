using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para salvar um sistema cliente
    /// </summary>
    [Serializable]
    public class SalvarSistemaClienteRequest : MensagemRequestBase
    {
        /// <summary>
        /// Sistema cliente a ser salvo
        /// </summary>
        public SistemaClienteInfo SistemaCliente { get; set; }
    }
}
