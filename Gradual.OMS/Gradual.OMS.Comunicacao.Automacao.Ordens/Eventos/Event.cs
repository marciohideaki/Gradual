using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Gradual.OMS.Comunicacao.Automacao.Ordens;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos
{
    public sealed class Event
    {
        public delegate void _onMDSAuthenticationResponse(object Response, Socket _ClientSocket);
        public static event _onMDSAuthenticationResponse _MDSAuthenticationResponse;

        public static void MDSAuthenticationResponse(object Response, Socket _ClientSocket)
        {
            _MDSAuthenticationResponse(Response, _ClientSocket);
        }

        public delegate void _onMDSBusinessResponse(object Response);
        public static event _onMDSBusinessResponse _MDSBusinessResponse;

        public static void MDSBusinessResponse(object Response)
        {
            _MDSBusinessResponse(Response);
        }


    }
}
