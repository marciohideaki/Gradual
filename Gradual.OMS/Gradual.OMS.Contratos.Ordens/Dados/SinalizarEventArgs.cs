using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// EventArgs para utilização no evento de sinalização do serviço de ordens
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SinalizarEventArgs : EventArgs
    {
        public MensagemSinalizacaoBase Mensagem { get; set; }
    }
}
