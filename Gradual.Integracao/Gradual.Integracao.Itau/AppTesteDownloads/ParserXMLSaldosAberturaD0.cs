using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace AppTesteDownloads
{
    public class ParserXMLSaldosAberturaD0 :XmlParser
    {
        public void Characters(string param1, int param2, int param3)
        {
            string cdata = param1;
        }
    }
}
