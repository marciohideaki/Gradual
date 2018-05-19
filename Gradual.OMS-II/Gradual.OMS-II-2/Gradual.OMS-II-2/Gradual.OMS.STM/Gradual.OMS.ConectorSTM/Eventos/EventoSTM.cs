using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.ConectorSTM.Eventos
{
    public class EventoSTM
    {
        public const string TIPO_MSG_CBLC = "C";
        public const string TIPO_MSG_MEGA = "M";

        private string cabecalho;
        private string corpo;
        private string tipo;
        private string msgid;

        public EventoSTM(string tipo, string cabecalho, string corpo, string msgid)
        {
            this.cabecalho = cabecalho;
            this.corpo = corpo;
            this.tipo = tipo;
            this.msgid = msgid;
        }

        public string Tipo
        {
            get { return tipo; }
        }

        public string Cabecalho
        {
            get { return cabecalho; }
        }

        public string Corpo
        {
            get { return corpo; }
        }

        public string MsgID
        {
            get { return msgid; }
        }

    }
}
