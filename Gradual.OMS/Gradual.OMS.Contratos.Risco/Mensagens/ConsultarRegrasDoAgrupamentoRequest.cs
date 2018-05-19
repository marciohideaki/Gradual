using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de consulta da árvore de regras do risco
    /// </summary>
    public class ConsultarRegrasDoAgrupamentoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Agrupamento cujas regras serão consultadas
        /// </summary>
        public RiscoGrupoInfo Agrupamento { get; set; }
    }
}
