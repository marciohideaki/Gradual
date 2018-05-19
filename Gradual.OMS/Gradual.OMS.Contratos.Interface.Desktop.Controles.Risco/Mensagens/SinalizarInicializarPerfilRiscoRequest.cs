using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Interface.Desktop.Controles.Risco.Mensagens
{
    /// <summary>
    /// Mensagem para informar inicializacao de perfil de risco
    /// </summary>
    public class SinalizarInicializarPerfilRiscoRequest : MensagemInterfaceRequestBase
    {
        /// <summary>
        /// Perfil de risco que está sendo inicializado
        /// </summary>
        public PerfilRiscoInfo PerfilRisco { get; set; }
    }
}
