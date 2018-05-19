using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.SaldoDevedor.Utils.Info
{
    [Serializable]
    public class MessageMailInfo
    {

        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Cco { get; set; }
        public string From { get; set; }
        public string FileAttach { get; set; }
        public MessageMailInfo()
        {
            this.Subject = string.Empty;
            this.Body = string.Empty;
            this.To = string.Empty;
            this.Cc = string.Empty;
            this.Cco = string.Empty;
            this.From = string.Empty;
            this.FileAttach = string.Empty;
        }

    }
}
