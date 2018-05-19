using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    [Serializable]
    public class ConversorLayoutBVMFParametroTabela
    {
        public int RegistroTipo { get; set; }
        public ConversorLayoutBVMFParametroTabelaTipoEnum TabelaTipo { get; set; }
    }
}
