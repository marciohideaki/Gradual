using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de tradução do código CBLC
    /// </summary>
    public class TraduzirCodigoCBLCResponse : MensagemResponseBase
    {
        /// <summary>
        /// Códigos de usuário correspondentes ao código CBLC informado
        /// </summary>
        public List<UsuarioInfo> Usuarios { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public TraduzirCodigoCBLCResponse()
        {
            this.Usuarios = new List<UsuarioInfo>();
        }
    }
}
