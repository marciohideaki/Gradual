using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using log4net;
using Newtonsoft.Json;
using Gradual.OMS.SpreadMonitor.Lib.Dados;

namespace Gradual.OMS.SpreadMonitor
{
    public class ClientHandlerState
    {
        public int ClientNumber { get; set; }
        public Socket ClientSocket { get; set; }
        public StreamerClientHandler ClientHandler { get; set; }
    }

    public class ServidorConexaoStreamer
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int ListenPortNumber { get; set; }
        private Dictionary<int, ClientHandlerState> dctClientes = new Dictionary<int, ClientHandlerState>();
        private bool _bKeepRunning = false;
        private Thread myThread = null;
        private string myThreadName = "";
        private SocketPackage sockServer = null;

        private static ServidorConexaoStreamer _me = null;

        public static ServidorConexaoStreamer Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new ServidorConexaoStreamer();
                }

                return _me;
            }
        }


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
                foreach (ClientHandlerState state in dctClientes.Values)
                {
                    try
                    {
                        state.ClientHandler.Stop();
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
                logger.Info("Aguardando finalizacao ServidorConexaoStreamer");
                Thread.Sleep(250);
            }
            logger.Info("ServidorConexaoStreamer Finalizado");
        }

        private void sockServer_OnRequestReceived(object sender, MessageEventArgs args)
        {
            try
            {
                string message = args.Message;

                DadosRequisicaoStreamer dados = JsonConvert.DeserializeObject<DadosRequisicaoStreamer>(message);
                //int acao = Int32.Parse(dados.acao);
                //string tipo = dados.tipo;
                //string instrumento = dados.instrumento;
                //string sessionID = dados.sessionID;

                //logger.Info("SessionID[" + sessionID + "]: Requisicao acao[" + acao + "] tipo[" + tipo + "] instrumento[" + instrumento + "]");

            }
            catch (Exception ex)
            {
                logger.Error("sockServer_OnRequestReceived(): " + ex.Message, ex);
            }
        }

        private void sockServer_OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            logger.Info("Cliente [" + args.ClientNumber + "] [" + args.ClientSocket.RemoteEndPoint.ToString() + "] conectou");

            ClientHandlerState state = new ClientHandlerState();

            state.ClientNumber = args.ClientNumber;
            state.ClientSocket = args.ClientSocket;

            state.ClientHandler = new StreamerClientHandler(state.ClientNumber, state.ClientSocket);
            state.ClientHandler.Start();

            dctClientes.Add(args.ClientNumber, state);
        }


        private void sockServer_OnClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            logger.Info("Cliente [" + args.ClientNumber + "] desconectou");

            if (dctClientes.ContainsKey(args.ClientNumber))
            {
                ClientHandlerState state = dctClientes[args.ClientNumber];

                state.ClientHandler.Stop();

                dctClientes.Remove(args.ClientNumber);
            }
        }

        private void MonitorClient()
        {
            long lastRun = DateTime.Now.Ticks;
            while (_bKeepRunning)
            {
                try
                {
                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - lastRun);
                    if (ts.TotalMilliseconds > 30000)
                    {
                        lastRun = DateTime.Now.Ticks;

                        string sonda = GerarSonda();

                        List<int> toDelete = new List<int>();
                        foreach (ClientHandlerState state in dctClientes.Values)
                        {
                            logger.Info("Enviando sonda para cliente " + state.ClientNumber + "[" + state.ClientSocket.RemoteEndPoint.ToString() + "]");

                            try
                            {
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
            EventoHttpSonda sonda = new EventoHttpSonda();
            sonda.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_SONDA, null, 0, null, null);
            sonda.sonda = new Dictionary<string, string>();
            sonda.sonda.Add(ConstantesMDS.HTTP_SONDA_DATA, DateTime.Now.ToString("yyyyMMdd"));
            sonda.sonda.Add(ConstantesMDS.HTTP_SONDA_HORA, DateTime.Now.ToString("HHmmss"));

            string mensagem = Newtonsoft.Json.JsonConvert.SerializeObject(sonda);

            return MDSUtils.montaMensagemHttp(ConstantesMDS.TIPO_REQUISICAO_SONDA, null, null, mensagem);
        }
    }
}
