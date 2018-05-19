using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de geração de metadados de segurança
    /// </summary>
    public class GerarMetadadoSegurancaResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de permissoes do sistema
        /// </summary>
        public List<PermissaoInfo> Permissoes { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public GerarMetadadoSegurancaResponse()
        {
            this.Permissoes = new List<PermissaoInfo>();
        }
    }
}
