using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Gradual.MDS.Core.AnaliseGrafica;
using log4net;
using System.Threading;
using Gradual.MDS.Core.Lib;

namespace Gradual.MDS.Core
{
    public class AnaliseGraficaHandlerState
    {
        public int ClientNumber { get; set; }
        public Socket ClientSocket { get; set; }
        public AnaliseGraficaClientHandler HandlerAnaliseGrafica { get; set; }
    }

    public class ServidorConexaoANG
    {

        private ILog logger;

        public int ListenPortNumber { get; set; }
        private Dictionary<int, AnaliseGraficaHandlerState> dctClientes = new Dictionary<int, AnaliseGraficaHandlerState>();
        private bool _bKeepRunning = false;
        private Thread myThread = null;
        private string myThreadName = "";
        private SocketPackage sockServer = null;

        private static ServidorConexaoANG _me = null;

        public static ServidorConexaoANG Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new ServidorConexaoANG();
                }

                return _me;
            }
        }


        #region ctor
        public ServidorConexaoANG()
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("ServidorConexaoANG", logger.Logger);
        }
        #endregion ctor

        public void Start()
        {
            _bKeepRunning = true;
            sockServer = new SocketPackage();

            sockServer.OnClientConnected += new ClientConnectedHandler(sockServer_OnClientConnected);
            sockServer.OnClientDisconnected += new ClientDisconnectedHandler(sockServer_OnClientDisconnected);
            sockServer.StartListen(ListenPortNumber);

            myThread = new Thread(new ThreadStart(MonitorClient));
            myThread.Name = myThreadName;
            myThread.Start();
        }


        public void Stop()
        {
            _bKeepRunning = false;
            sockServer.CloseSocket();

            lock (dctClientes)
            {
                foreach (AnaliseGraficaHandlerState state in dctClientes.Values)
                {
                    try
                    {
                        state.HandlerAnaliseGrafica.Stop();

                        state.ClientSocket.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Erro(): " + ex.Message, ex);
                    }
                }

            }

            while (myThread != null && myThread.IsAlive)
            {
                logger.Info("Aguardando finalizacao ServidorConexaoANG");
                Thread.Sleep(250);
            }
            logger.Info("ServidorConexaoANG Finalizado");
        }


        private void sockServer_OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            lock (dctClientes)
            {
                logger.Info("Cliente [" + args.ClientNumber + "] [" + args.ClientSocket.RemoteEndPoint.ToString() + "]");

                AnaliseGraficaHandlerState state = new AnaliseGraficaHandlerState();

                state.ClientNumber = args.ClientNumber;
                state.ClientSocket = args.ClientSocket;
                state.HandlerAnaliseGrafica = new AnaliseGraficaClientHandler(args.ClientNumber, args.ClientSocket);
                state.HandlerAnaliseGrafica.Start();
                state.HandlerAnaliseGrafica.TratarConexaoANG(args.ClientSocket.RemoteEndPoint.ToString());

                dctClientes.Add(args.ClientNumber, state);
            }
        }


        private void sockServer_OnClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            if (dctClientes.ContainsKey(args.ClientNumber))
            {
                logger.Info("Cliente [" + args.ClientNumber + "] desconectou");

                AnaliseGraficaHandlerState state = dctClientes[args.ClientNumber];

                state.HandlerAnaliseGrafica.Stop();

                dctClientes.Remove(args.ClientNumber);
            }
        }

        private void MonitorClient()
        {
            long lastRun = DateTime.UtcNow.Ticks;
            while (_bKeepRunning)
            {
                try
                {
                    TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - lastRun);
                    if (ts.TotalMilliseconds > 30000)
                    {
                        lastRun = DateTime.UtcNow.Ticks;

                        string sonda = GerarSonda();

                        List<int> toDelete = new List<int>();

                        foreach (AnaliseGraficaHandlerState state in dctClientes.Values)
                        {
                            try
                            {
                                logger.Info("Enviando sonda para cliente " + state.ClientNumber + "[" + state.ClientSocket.RemoteEndPoint.ToString() + "]");

                                SocketPackage.SendData(sonda, state.ClientSocket);
                            }
                            catch (Exception ex)
                            {
                                logger.Error("Erro ao enviar sonda para [" + state.ClientNumber + "] removendo cliente");
                                toDelete.Add(state.ClientNumber);
                            }
                        }

                        if (toDelete.Count > 0)
                        {
                            foreach (int clientNumber in toDelete)
                            {
                                dctClientes.Remove(clientNumber);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("MonitorClient(): " + ex.Message, ex);
                }

                Thread.Sleep(100);
            }
        }

        private string GerarSonda()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ConstantesMDS.DESCRICAO_SONDA);
            builder.Append(ConstantesMDS.TIPO_REQUISICAO_HB_SONDA);
            builder.Append(ConstantesMDS.DESCRICAO_DE_BOLSA_BOVESPA);
            builder.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            builder.Append(ConstantesMDS.DESCRICAO_SONDA);

            return builder.ToString();
        }
    }
}
