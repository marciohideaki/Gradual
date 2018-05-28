using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.MTA
{
    [Serializable]
    public class DownloadMTAInfo
    {
        public bool HasCLCO { get; set; }
        public bool HasCMDF { get; set; }
        public bool HasCSGD { get; set; }
        public bool HasPFEN { get; set; }
        public bool HasPENR { get; set; }

        public bool NotificadoCLCO { get; set; }
        public bool NotificadoCMDF { get; set; }
        public bool NotificadoCSGD { get; set; }
        public bool NotificadoPFEN { get; set; }
        public bool NotificadoPENR { get; set; }

        public string PathCLCO { get; set; }
        public string PathCMDF { get; set; }
        public string PathCSGD { get; set; }
        public string PathPFEN { get; set; }
        public string PathPENR { get; set; }

        public string MD5CLCO { get; set; }
        public string MD5CMDF { get; set; }
        public string MD5CSGD { get; set; }
        public string MD5PFEN { get; set; }
        public string MD5PENR { get; set; }
    }
}
