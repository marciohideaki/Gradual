using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Contratos.Comum
{
    /// <summary>
    /// Interface para o serviço de validação.
    /// </summary>
    public interface IServicoValidacao : IServicoControlavel
    {
        /// <summary>
        /// Solicita que uma mensagem passe pelo pipeline de validação.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ValidarMensagemResponse ValidarMensagem(ValidarMensagemRequest parametros);
    }
}
