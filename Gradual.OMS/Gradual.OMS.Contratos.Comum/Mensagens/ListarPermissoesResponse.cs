using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Permissoes;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de permissoes
    /// </summary>
    public class ListarPermissoesResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de permissoes
        /// </summary>
        public List<PermissaoBase> Permissoes { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListarPermissoesResponse()
        {
            this.Permissoes = new List<PermissaoBase>();
        }
    }
}
