using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de regras disponíveis para cadastro
    /// </summary>
    public class ListarRegrasDisponiveisResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de regras disponíveis
        /// </summary>
        public List<RegraInfo> Regras { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListarRegrasDisponiveisResponse()
        {
            this.Regras = new List<RegraInfo>();
        }
    }
}
