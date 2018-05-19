using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.ConectorSTM.Eventos
{
    public class EventoMega
    {
        private string function;
        private object info;
        private string msgid;

        public EventoMega(string function, object info, string msgid)
        {
            this.function = function;
            this.info = info;
            this.msgid = msgid;
        }

        public string Function
        {
            get { return function; }
        }

        public object Info
        {
            get { return info; }
        }

        public string MsgID
        {
            get { return msgid; }
        }

    }
}
