using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AS.Sockets;
using System.Configuration;

namespace AS.Hoster
{
    class Program
    {
        static void Main(string[] args)
        {
            int PortNumber = int.Parse(ConfigurationSettings.AppSettings["NumeroPorta"].ToString());

            SocketPackage SockPkg = new SocketPackage();
            SockPkg.StartListen(PortNumber);

            Console.Read();        
        }
    }
}
