using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    [Serializable]
    public class ConversorCampoNumeroParametro
    {
        public int NumeroDecimais { get; set; }
        public string Separador { get; set; }

        public ConversorCampoNumeroParametro()
        {
            this.NumeroDecimais = 2;
            this.Separador = "";
        }
    }
}
