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
    /// Mensagem de resposta a uma solicitação de consulta da árvore de regras
    /// </summary>
    public class ConsultarRegrasDoAgrupamentoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de regras a serem validadas para o grupo informado
        /// </summary>
        public List<RegraBase> Regras { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ConsultarRegrasDoAgrupamentoResponse()
        {
            this.Regras = new List<RegraBase>();
        }
    }
}
