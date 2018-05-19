using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Ordens.Mensagens;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// EventArgs utilizado no evento de retorno de Lista de Instrumentos.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SinalizarListaInstrumentosEventArgs : EventArgs
    {
        public SinalizarListaInstrumentosRequest SinalizarListaInstrumentosRequest { get; set; }
    }
}
