using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para salvar um perfil de risco
    /// </summary>
    public class SalvarPerfilRiscoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Perfil de risco a ser salvo
        /// </summary>
        public PerfilRiscoInfo PerfilRiscoInfo { get; set; }
    }
}
