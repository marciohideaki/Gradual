using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.BackOffice.BrokerageProcessor.Lib.FileWatcher;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Txt
{
    public class TOArqTxt
    {
        public string FileName { get; set; }
        public FileWatcherConfigItem Config { get; set; }


        public TOArqTxt()
        {
            this.FileName = string.Empty;
            this.Config = null;
        }

    }
}
