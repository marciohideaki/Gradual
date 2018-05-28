using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.BackOffice.BrokerageProcessor.Lib.FileWatcher;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Pdf
{
    public class TOArqPdf
    {
        public string FileName { get; set; }
        public FileWatcherConfigItem Config { get; set; }


        public TOArqPdf()
        {
            this.FileName = string.Empty;
            this.Config = null;
        }

    }
}
