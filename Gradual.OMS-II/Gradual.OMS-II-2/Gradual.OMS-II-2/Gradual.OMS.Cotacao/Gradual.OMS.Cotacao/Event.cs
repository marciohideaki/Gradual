using System;
using System.Net.Sockets;

namespace Gradual.OMS.Cotacao
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
