using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos
{
    public class MDSEventFactory
    {
        MDSEventArgs eventArgs = new MDSEventArgs();

        public delegate void _OnMDSStopStartEvent (object sender, MDSEventArgs e);
        public event _OnMDSStopStartEvent OnMDSStopStartEvent;

        public void MDSStopStartEvent(object sender)
        {
            if (OnMDSStopStartEvent != null)
                OnMDSStopStartEvent(sender, eventArgs);
        }


        public delegate void _OnMDSSRespostaAutenticacaoEvent(object sender, MDSEventArgs e);
        public event _OnMDSSRespostaAutenticacaoEvent OnMDSSRespostaAutenticacaoEvent;

        public void MDSSRespostaAutenticacaoEvent(object sender)
        {
            if (OnMDSSRespostaAutenticacaoEvent != null)
                OnMDSSRespostaAutenticacaoEvent(sender, eventArgs);
        }


        public delegate void _OnMDSSRespostaCancelamentoEvent(object sender, MDSEventArgs e);
        public event _OnMDSSRespostaCancelamentoEvent OnMDSSRespostaCancelamentoEvent;

        public void MDSSRespostaCancelamentoEvent(object sender)
        {
            if (OnMDSSRespostaCancelamentoEvent != null)
                OnMDSSRespostaCancelamentoEvent(sender, eventArgs);
        }


    }
}
