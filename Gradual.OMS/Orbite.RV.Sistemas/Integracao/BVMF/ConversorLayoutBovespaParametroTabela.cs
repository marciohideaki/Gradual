using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    [Serializable]
    public class ConversorLayoutBovespaParametroTabela
    {
        public int RegistroTipo { get; set; }
        public ConversorLayoutBovespaParametroTabelaTipoEnum TabelaTipo { get; set; }
    }
}
