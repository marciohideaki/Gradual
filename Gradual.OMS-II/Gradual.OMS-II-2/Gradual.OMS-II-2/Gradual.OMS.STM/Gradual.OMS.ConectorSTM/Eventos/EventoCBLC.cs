using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.ConectorSTM.Eventos
{
    public class EventoCBLC
    {
        private string cabecalho;
        private object info;
        private string msgid;

        public EventoCBLC(string cabecalho, object info, string msgid)
        {
            this.cabecalho = cabecalho;
            this.info = info;
            this.msgid = msgid;
        }

        public string Cabecalho
        {
            get { return cabecalho; }
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
