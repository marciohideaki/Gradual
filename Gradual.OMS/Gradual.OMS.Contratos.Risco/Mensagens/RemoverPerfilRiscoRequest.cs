using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de remoção de um Perfil de risco
    /// </summary>
    public class RemoverPerfilRiscoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do Perfil de risco a remover
        /// </summary>
        public string CodigoPerfilRisco { get; set; }
    }
}
