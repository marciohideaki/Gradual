using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class InstituicoesInfo
    {
        public string CodInst { get; set; }

        public string NomeFantasia { get; set; }

        public string CodMae { get; set; }

        public char InstMae { get; set; }

        public char InstFin { get; set; }

        public char EmpAberta { get; set; }

        public string CodCVM { get; set; }

        public char InstAdministradora { get; set; }

        public DateTime DataHora { get; set; }
    }
}
