using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// EventArgs do evento de echo
    /// </summary>
    [Serializable]
    public class EchoEventArgs : EventArgs
    {
        /// <summary>
        /// Mensagem do echo
        /// </summary>
        public string Mensagem { get; set; }
    }
}
