using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos
{

    public class MDSEventArgs : EventArgs
    {
        public object StopStartEventObject { get; set; }

        public MDSEventArgs(object eventobject)
        {
            this.StopStartEventObject = eventobject;
        }
    }
}
