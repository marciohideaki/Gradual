using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Cotacao
{
    public class MDSEventFactory
    {
        MDSEventArgs eventArgs = new MDSEventArgs();

        public delegate void _OnMDSBussinesReceveive(object sender, MDSEventArgs e);
        public event _OnMDSBussinesReceveive OnMDSBussinesReceveive;

        public void MDSBusinessReceive(object sender){
            if(OnMDSBussinesReceveive != null)
                OnMDSBussinesReceveive(sender, eventArgs);
        }

        public delegate void _OnMDSBookReceveive(object sender, MDSEventArgs e);
        public event _OnMDSBookReceveive OnMDSBookReceveive;

        public void MDSBookReceveive(object sender)
        {
            if(OnMDSBookReceveive != null)
                OnMDSBookReceveive(sender, eventArgs);
        }


    }
}
