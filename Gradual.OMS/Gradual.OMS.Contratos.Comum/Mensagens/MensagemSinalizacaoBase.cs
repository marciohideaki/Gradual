using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    [Serializable]
    public abstract class MensagemSinalizacaoBase : MensagemRequestBase
    {
        /// <summary>
        /// Indica se esta mensagem é algum tipo de retorno a outra mensagem.
        /// Nesses casos, este campo indica a mensagem a que esta se refere.
        /// </summary>
        [Category("MensagemBase")]
        public string CodigoMensagemReferencia { get; set; }
    }
}
