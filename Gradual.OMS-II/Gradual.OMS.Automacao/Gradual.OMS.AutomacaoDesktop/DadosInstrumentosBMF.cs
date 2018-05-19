using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.AutomacaoDesktop
{
    public class DadosInstrumentosBMF
    {
        public string SecurityID { get; set; }
        public string SecurityIDSource { get; set; }

        public DadosInstrumentosBMF(
                string securityid,
                string securityidsource)
        {
            this.SecurityID = securityid;
            this.SecurityIDSource = securityidsource;
        }
    }
}
