using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;

namespace Gradual.OMS.Contratos.Interface.Desktop.Controles.Seguranca.Mensagens
{
    /// <summary>
    /// Mensagem de sinalização de inicialização de usuário
    /// </summary>
    public class SinalizarInicializarUsuarioRequest : MensagemInterfaceRequestBase
    {
        /// <summary>
        /// Usuário que está sendo inicializado
        /// </summary>
        public UsuarioInfo Usuario { get; set; }
    }
}
