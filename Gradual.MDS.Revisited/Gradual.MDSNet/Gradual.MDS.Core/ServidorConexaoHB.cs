using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Gradual.MDS.Core.HomeBrokerHandlers;
using log4net;
using System.Threading;
using Gradual.MDS.Core.Lib;
using Gradual.MDS.Eventos.Lib;
using Newtonsoft.Json;

namespace Gradual.MDS.Core
{
    public class HomeBrokerHandlerState
    {
        public int ClientNumber { get; set; }
        public Socket ClientSocket { get; set; }
        public HomeBrokerHandlerLivroOfertas HandlerLivroOfertas { get; set; }
        public HomeBrokerHandlerNegocios HandlerNegocios { get; set; }
        public HomeBrokerHandlerLivroOfertaAgregado HandlerLivroAgregado { get; set; }
    }

    public class ServidorConexaoHB
    {
        private ILog logger;

        public int ListenPortNumber { get; set; }
        private Dictionary<int, HomeBrokerHandlerState> dctClientes = new Dictionary<int, HomeBrokerHandlerState>();
        private bool _bKeepRunning = false;
        private Thread myThread = null;
        private string myThreadName = "";
        private SocketPackage sockServer = null;

        private static ServidorConexaoHB _me = null;

        public static ServidorConexaoHB Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new ServidorConexaoHB();
                }

                return _me;
            }
        }


        #region ctor
        public ServidorConexaoHB()
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("ServidorConexaoHB-", logger.Logger);
        }
        #endregion ctor

        public void Start()
        {
            _bKeepRunning = true;
            sockServer = new SocketPackage();

            sockServer.OnClientConnected += new ClientConnectedHandler(sockServer_OnClientConnected);
            sockServer.OnRequestReceived += new MessageReceivedHandler(sockServer_OnRequestReceived);
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
                foreach (HomeBrokerHandlerState state in dctClientes.Values)
                {
                    try
                    {
                        state.HandlerLivroOfertas.Stop();
                        state.HandlerNegocios.Stop();
                        state.HandlerLivroAgregado.Stop();
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
                logger.Info("Aguardando finalizacao ServidorConexaoHB");
                Thread.Sleep(250);
            }
            logger.Info("ServidorConexaoHB Finalizado");
        }

        private void sockServer_OnRequestReceived(object sender, MessageEventArgs args)
        {
            try
            {
                string message = args.Message;
                string serverName = message.Substring(21).Trim();

                logger.Info("Recebeu login [" + message + "]");

                lock (dctClientes)
                {
                    if (dctClientes.ContainsKey(args.ClientNumber))
                    {
                        HomeBrokerHandlerState state = dctClientes[args.ClientNumber];

                        state.HandlerNegocios.Start();
                        state.HandlerLivroOfertas.Start();
                        state.HandlerLivroAgregado.Start();

                        state.HandlerNegocios.TratarServicoCotacaoLogin(serverName);
                        state.HandlerLivroOfertas.TratarServicoCotacaoLogin(serverName);
                        state.HandlerLivroAgregado.TratarServicoCotacaoLogin(serverName);
                    }
                    else
                    {
                        logger.Error("ClientNumber nao encontrado [" + args.ClientNumber + "]");
                    }
                }


            }
            catch (Exception ex)
            {
                logger.Error("sockServer_OnRequestReceived(): " + ex.Message, ex);
            }
        }

        private void sockServer_OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            lock (dctClientes)
            {
                logger.Info("Cliente [" + args.ClientNumber + "] [" + args.ClientSocket.RemoteEndPoint.ToString() + "]");

                HomeBrokerHandlerState state = new HomeBrokerHandlerState();

                state.ClientNumber = args.ClientNumber;
                state.ClientSocket = args.ClientSocket;
                state.HandlerLivroOfertas = new HomeBrokerHandlerLivroOfertas(args.ClientNumber, args.ClientSocket);
                state.HandlerNegocios = new HomeBrokerHandlerNegocios(args.ClientNumber, args.ClientSocket);
                state.HandlerLivroAgregado = new HomeBrokerHandlerLivroOfertaAgregado(args.ClientNumber, args.ClientSocket);

                dctClientes.Add(args.ClientNumber, state);
            }
        }


        private void sockServer_OnClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            if (dctClientes.ContainsKey(args.ClientNumber))
            {
                logger.Info("Cliente [" + args.ClientNumber + "] desconectou");
                
                HomeBrokerHandlerState state = dctClientes[args.ClientNumber];

                state.HandlerLivroOfertas.Stop();
                state.HandlerNegocios.Stop();
                state.HandlerLivroAgregado.Stop();

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
                        foreach (HomeBrokerHandlerState state in dctClientes.Values)
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
