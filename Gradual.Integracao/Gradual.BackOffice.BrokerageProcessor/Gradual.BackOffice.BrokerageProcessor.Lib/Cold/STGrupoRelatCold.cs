using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Cold
{
    public struct STGrupoRelatCold
    {
        public int IDGrupo { get; set; }
        public string EmailsTO { get; set; }
        public string EmailsCC { get; set; }
        public string EmailsBCC { get; set; }
        public bool FlagFolhaUnica { get; set; }
        public bool FlagAnexo { get; set; }
        public bool FlagPdf { get; set; }
        public bool FlagZip { get; set; }
    }
}
