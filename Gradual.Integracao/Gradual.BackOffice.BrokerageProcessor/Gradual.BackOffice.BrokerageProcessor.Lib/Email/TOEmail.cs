using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Spider.Utils.Email.Entities;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Email
{
    public class TOEmail
    {
        public MessageMailInfo Msg { get;set; }
        public string Exchange { get; set; }
        public string IdCliente { get; set; }
        public string Status { get; set; }
        public string DescStatus { get; set; }
        public TOEmail()
        {
            this.Msg = new MessageMailInfo();
            this.Exchange = string.Empty;
            this.IdCliente = string.Empty;
            this.Status = StatusInfo.OK;
            this.DescStatus = string.Empty;
        }

    }
}
