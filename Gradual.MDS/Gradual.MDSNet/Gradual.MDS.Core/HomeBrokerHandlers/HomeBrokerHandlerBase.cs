using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Net.Sockets;
using System.Threading;

namespace Gradual.MDS.Core.HomeBrokerHandlers
{
    public abstract class HomeBrokerHandlerBase
    {
        protected ILog logger;
        protected Socket ClientSocket;
        protected int ClientNumber;

        public HomeBrokerHandlerBase(int clientNumber, Socket clientSocket)
        {
            ClientNumber = clientNumber;
            ClientSocket = clientSocket;
        }


        /// <summary>
        /// Assinar os eventos do HomeBroker gerados pelao
        /// </summary>
        protected abstract void listenEvents();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void enviarSnapshotHB();

        public virtual void TratarServicoCotacaoLogin(string serverName)
        {
            logger.Debug("Tratando login de  [" + serverName + "]");

            ThreadPool.QueueUserWorkItem(
                new WaitCallback(
                    delegate(object required)
                    {
                        try
                        {
                            enviarSnapshotHB();
                            listenEvents();
                        }
                        catch (Exception ex)
                        {
                            logger.Error("TratarServicoCotacaoLogin(): " + ex.Message, ex);
                        }
                    }
                )
            );
        }
    }
}
