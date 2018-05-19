using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para salvar uma sessao
    /// </summary>
    [Serializable]
    public class SalvarSessaoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Sessao a ser salva
        /// </summary>
        public SessaoInfo Sessao { get; set; }
    }
}
