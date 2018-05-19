using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickFix;

namespace Gradual.OMS.Library.Fix
{
    public class HostFixEventoAplicacaoEventArgs : EventArgs
    {
        public HostFixEventoAplicacaoEventArgs()
        {
        }

        public HostFixEventoAplicacaoEventArgs(QuickFixApplicationEvent eventType, SessionID session, Message message)
        {
            this.EventType = eventType;
            this.Session = session;
            this.Message = message;
        }

        public QuickFixApplicationEvent EventType { get; set; }
        public SessionID Session { get; set; }
        public Message Message { get; set; }
    }
}
