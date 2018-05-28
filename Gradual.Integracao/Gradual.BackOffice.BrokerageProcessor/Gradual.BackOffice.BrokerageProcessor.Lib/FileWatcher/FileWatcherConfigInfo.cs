using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.FileWatcher
{
    public class FileWatcherConfigItem
    {
        public int Type { get; set; }
        public string NameType { get; set; }
        public string Exchange { get; set; }
        public string PathWatcher { get; set; }
        public string PathBkp { get; set;}
        public string PathProcessed { get; set; }
        public string ExtensionFilter { get; set; }
        public int TimeToRefresh { get; set; }
        public string TemplateFile { get; set; }
        public string SubjectEmail { get; set; }
        public bool ClientIdCheck { get; set; }
        public int FileType { get; set; }

        public FileWatcherConfigItem()
        {
            this.Exchange = string.Empty;
            this.NameType = string.Empty;
            this.PathWatcher = string.Empty;
            this.PathBkp = string.Empty;
            this.PathProcessed = string.Empty;
            this.ExtensionFilter = string.Empty;
            this.TimeToRefresh = 0;
            this.TemplateFile = string.Empty;
            this.SubjectEmail = string.Empty;
            this.Type = TypeWatcher.UNDEFINED;
            this.ClientIdCheck = false;
            this.FileType = FileTypes.UNDEFINED;
        }

    }

    public class FileWatcherConfig
    {
        public List<FileWatcherConfigItem> Watchers { get; set; }
        public FileWatcherConfig()
        {
            this.Watchers = new List<FileWatcherConfigItem>();
        }
    }
}
