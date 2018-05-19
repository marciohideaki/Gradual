using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos
{
    public class OMSEventHandlerClass
    {
        public OMSEventHandlerClass(MDSEventFactory _eventOms)
        {
            _eventOms.OnMDSStopStartEvent += new MDSEventFactory._OnMDSStopStartEvent(_eventOms_OnMDSStopStartEvent);
        }

        void _eventOms_OnMDSStopStartEvent(object sender, MDSEventArgs e)
        {
           
        }

    }    
}
