using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// Classe de dados para informações do instrumento.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class InstrumentoInfo
    {
        public string CodigoBolsa { get; set; }
        public string Symbol { get; set; }
        public string SecurityID { get; set; }
        public string SecurityIDSource { get; set; }
        public string SecurityExchange { get; set; }
    }
}
