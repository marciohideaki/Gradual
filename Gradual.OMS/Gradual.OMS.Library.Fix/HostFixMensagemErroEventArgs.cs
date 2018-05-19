using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickFix;

namespace Gradual.OMS.Library.Fix
{
    public class HostFixMensagemErroEventArgs : EventArgs
    {
        public HostFixMensagemErroEventArgs()
        {
        }

        public HostFixMensagemErroEventArgs(SessionID session, Message message, MessageCracker fixApplication, Exception exception)
        {
            this.Session = session;
            this.Message = message;
            this.FixApplication = fixApplication;
            this.Exception = exception;
        }

        public SessionID Session { get; set; }
        public Message Message { get; set; }
        public MessageCracker FixApplication { get; set; }
        public Exception Exception { get; set; }
    }
}
