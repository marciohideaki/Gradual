using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.Spider.CommSocket;
using System.Configuration;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Gradual.Spider.Communications.Lib.Mensagens;
using System.Net;

namespace Gradual.Spider.CommServer
{
    public struct stCliente
    {
        public Socket ClientSocket { get; set; }
        public int ClientNumber { get; set; }
        public long LastSonda { get; set; }
        public string IPAddress { get; set; }
        public int IPPort { get; set; }
        public string SessionID { get; set; }
    }


    public class ServidorComunicacoes : IServicoControlavel
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _status = ServicoStatus.Parado;
        private SpiderSocket sockServer;
        private Dictionary<int, stCliente> dctClientes = new Dictionary<int, stCliente>();
        private Thread thMonitorClientes = null;
        private int listenPort = 23231;
        private bool _bKeepRunning = false;

        #region IServicoControlavel
        public void IniciarServico()
        {
            _bKeepRunning = true;

            ProvidersManager.Instance.Start();
            SubscriptionManager.Instance.Start();

            if (ConfigurationManager.AppSettings["ListenPort"] != null)
            {
                listenPort = Convert.ToInt32(ConfigurationManager.AppSettings["ListenPort"]);
            }

            thMonitorClientes = new Thread(new ThreadStart(monitorClientes));
            thMonitorClientes.Start();

            sockServer = new SpiderSocket();

            sockServer.OnClientConnected += new ClientConnectedHandler(sockServer_OnClientConnected);
            sockServer.OnClientDisconnected += new ClientDisconnectedHandler(sockServer_OnClientDisconnected);
            sockServer.OnMessageReceived += new MessageReceivedHandler(sockServer_OnMessageReceived);

            sockServer.AddHandler<LoginCommServerRequest>(new ProtoObjectReceivedHandler<LoginCommServerRequest>(sockServer_OnClientLoginRequest));
            sockServer.AddHandler<AssinaturaCommServerRequest>(new ProtoObjectReceivedHandler<AssinaturaCommServerRequest>(sockServer_OnClientAssinaturaRequest));
            sockServer.AddHandler<CancelAssinaturaCommServerRequest>(new ProtoObjectReceivedHandler<CancelAssinaturaCommServerRequest>(sockServer_OnClientCancelamentoAssinaturaRequest));
            sockServer.AddHandler<SondaCommServer>(new ProtoObjectReceivedHandler<SondaCommServer>(sockServer_OnClientSonda));
            sockServer.OnUnmappedObjectReceived += new UnmappedObjectReceivedHandler(sockServer_OnUnmappedObjectReceived);

            sockServer.StartListen(listenPort);

            _status = ServicoStatus.EmExecucao;
        }



        public void PararServico()
        {
            _bKeepRunning = false;

            while (thMonitorClientes.IsAlive)
            {
                Thread.Sleep(250);
            }

            _status = ServicoStatus.Parado;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }
        #endregion //IServicoControlavel

        void sockServer_OnMessageReceived(object sender, MessageEventArgs args)
        {
            logger.Debug("Recebeu: [" + System.Text.Encoding.ASCII.GetString(args.Data) + "]");
        }

        void sockServer_OnClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            logger.Info("Cliente desconectado: [" + args.ClientNumber + "]");

            if (dctClientes.ContainsKey(args.ClientNumber))
            {
                stCliente strut = dctClientes[args.ClientNumber];

                logger.Info("Removendo cliente [" + args.ClientNumber + "] [" + strut.IPAddress + ":" + strut.IPPort + " da lista de monitoracao");
                dctClientes.Remove(args.ClientNumber);

                SubscriptionManager.Instance.UnSubscribeAll(strut.SessionID);
            }
        }

        void sockServer_OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            logger.Info("Cliente conectado: [" + args.ClientNumber + "]");

            stCliente strut = new stCliente();
            strut.ClientNumber = args.ClientNumber;
            strut.ClientSocket = args.ClientSocket;
            strut.LastSonda = 0;
            strut.IPAddress = IPAddress.Parse(((IPEndPoint)args.ClientSocket.RemoteEndPoint).Address.ToString()).ToString();
            strut.IPPort = ((IPEndPoint)args.ClientSocket.RemoteEndPoint).Port;


            dctClientes.Add(args.ClientNumber, strut);
        }

        private void monitorClientes()
        {
            logger.Info("Iniciando a monitoracao dos clientes conectados");
            long lastSonda = 0;
            List<int> toDelete = new List<int>();

            while (_bKeepRunning)
            {
                TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - lastSonda);
                toDelete.Clear();

                if (ts.TotalMilliseconds > 30000 && dctClientes.Count > 0)
                {
                    lastSonda = DateTime.Now.Ticks;
                    logger.Info("Enviando sonda para " + dctClientes.Count + " clientes");

                    foreach (stCliente strut in dctClientes.Values)
                    {
                        try
                        {
                            SondaCommServer sonda = new SondaCommServer();

                            if (strut.ClientSocket.Connected)
                            {
                                SpiderSocket.SendObject(sonda, strut.ClientSocket);

                                logger.Info("Sonda enviada pra cliente [" + strut.ClientNumber + "] [" + strut.IPAddress + ":" + strut.IPPort + "]");
                            }
                            else
                            {
                                logger.Error("Cliente [" + strut.ClientNumber + "] desconectado, removendo");
                                toDelete.Add(strut.ClientNumber);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Erro ao enviar sonda para cliente [" + strut.ClientNumber + "], removendo");
                            toDelete.Add(strut.ClientNumber);
                        }
                    }

                    foreach (int clientNumber in toDelete)
                    {
                        try
                        {
                            dctClientes[clientNumber].ClientSocket.Close();
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Erro ao fechar socket");
                        }

                        dctClientes.Remove(clientNumber);
                    }
                }
                Thread.Sleep(100);
            }

            logger.Info("Finalizando a monitoracao dos clientes conectados");
        }


        void sockServer_OnClientLoginRequest(object sender, int clientNumber, Socket clientSocket, LoginCommServerRequest args)
        {
            LoginCommServerResponse response = new LoginCommServerResponse();

            response.SessionID = Guid.NewGuid().ToString();

            if (dctClientes.ContainsKey(clientNumber))
            {
                stCliente strut = dctClientes[clientNumber];
                strut.SessionID = response.SessionID;
                dctClientes[clientNumber] = strut;
            }

            SpiderSocket.SendObject(response, clientSocket);
        }

        void sockServer_OnClientAssinaturaRequest(object sender, int clientNumber, Socket clientSocket, AssinaturaCommServerRequest args)
        {
            string sessionID = args.SessionID;

            List<Object> objetos = AssinaturaCommServerRequest.GetObjects(args);
            if (args.TiposAssinados.Count != objetos.Count)
            {
                logger.Error("Lista de parametros nao corresponde a lista de tipos assinados");
                return;
            }

            for (int i = 0; i < args.TiposAssinados.Count; i++)
            {
                logger.Debug("SessionID [" + args.SessionID + "] efetuando assinatura de [" + args.Instrumento + "] Type [" + args.TiposAssinados[i].ToString() + "]");
                SubscriptionManager.Instance.Subscribe(clientSocket, args.SessionID, args.TiposAssinados[i], args.Instrumento, objetos[i]);
            }
        }

        void sockServer_OnClientCancelamentoAssinaturaRequest(object sender, int clientNumber, Socket clientSocket, CancelAssinaturaCommServerRequest args)
        {
            string sessionID = args.SessionID;

            for (int i = 0; i < args.TiposAssinados.Count; i++)
            {
                logger.Debug("SessionID [" + args.SessionID + "] cancelando assinatura de [" + args.Instrumento + "] Type [" + args.TiposAssinados[i].ToString() + "]");
                SubscriptionManager.Instance.UnSubscribe(clientSocket, args.SessionID, args.TiposAssinados[i], args.Instrumento);
            }
        }

        void sockServer_OnClientSonda(object sender, int clientNumber, Socket clientSocket, SondaCommServer args)
        {
            logger.Debug("Recebeu sonda do cliente [" + clientNumber + "]");
        }

        void sockServer_OnUnmappedObjectReceived(object sender, int clientNumber, Socket clientSock, Type objectType, object objeto)
        {
            logger.Error("Ai cacete, recebeu um objeto [" + objectType.ToString() + "]");
        }
    }
}
