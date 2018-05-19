using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de lista de regras de risco
    /// </summary>
    public class ListarRegraRiscoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Informações de filtro de agrupamento
        /// </summary>
        public RiscoGrupoInfo FiltroAgrupamento { get; set; }
    }
}
