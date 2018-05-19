using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Interface.Desktop.Controles.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de sinalização de salvar o perfil de risco
    /// </summary>
    public class SinalizarSalvarPerfilRiscoRequest : MensagemInterfaceRequestBase
    {
        /// <summary>
        /// Perfil de risco que está sendo salvo
        /// </summary>
        public PerfilRiscoInfo PerfilRisco { get; set; }
    }
}
