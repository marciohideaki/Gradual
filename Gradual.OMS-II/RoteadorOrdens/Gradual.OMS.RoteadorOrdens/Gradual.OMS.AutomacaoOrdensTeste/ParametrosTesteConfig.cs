using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.AutomacaoOrdensTeste
{
    [Serializable]
    public class ParametrosTesteConfig
    {
        public List<ParametroTesteConfig> Parametros { get; set; }
    }

    [Serializable]
    public class ParametroTesteConfig
    {
        public string Bolsa { get; set; }
        public string Porta { get; set; }
        public string Account { get; set; }

        [XmlElement("EnteringTrader", IsNullable = true)]
        public string EnteringTrader { get; set; }

        public string Papeis { get; set; }
        public int Qtde { get; set; }
    }
}
