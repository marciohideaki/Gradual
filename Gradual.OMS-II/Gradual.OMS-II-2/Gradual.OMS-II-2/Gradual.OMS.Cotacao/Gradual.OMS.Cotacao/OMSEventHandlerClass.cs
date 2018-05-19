using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Cotacao
{
    public class OMSEventHandlerClass
    {
        public OMSEventHandlerClass(MDSEventFactory _eventOms)
        {
            _eventOms.OnMDSBussinesReceveive += new MDSEventFactory._OnMDSBussinesReceveive(_eventOms_OnMDSBussinesReceveive);
            _eventOms.OnMDSBookReceveive += new MDSEventFactory._OnMDSBookReceveive(_eventOms_OnMDSBookReceveive);
        }

        void _eventOms_OnMDSBussinesReceveive(object sender, MDSEventArgs fe)
        {
            fe.MDSReceive(sender);
        }

        void _eventOms_OnMDSBookReceveive(object sender, MDSEventArgs fe)
        {
            fe.MDSReceive(sender);
        }
    }
}
