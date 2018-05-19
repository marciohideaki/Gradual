using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    [Serializable]
    public class ConversorCampoDataParametro
    {
        public string FormatoData { get; set; }

        public ConversorCampoDataParametro()
        {
            this.FormatoData = "yyyymmdd";
        }
    }
}
