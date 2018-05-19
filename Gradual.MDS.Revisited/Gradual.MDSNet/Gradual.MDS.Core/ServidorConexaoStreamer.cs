using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.MDS.Core.Lib;
using System.Net.Sockets;
using System.Threading;
using Gradual.MDS.Eventos.Lib;
using Newtonsoft.Json;
using Gradual.MDS.Core.StreamerHandlers;

namespace Gradual.MDS.Core
{
    public class ClientHandlerState
    {
        public int ClientNumber { get; set; }
        public Socket ClientSocket { get; set; }
        public StreamerClientHandlerLivroOfertas HandlerLivroOfertas { get; set; }
        public StreamerClientHandlerNegocios HandlerNegocios { get; set; }
        public StreamerClientHandlerLivroNegocios HandlerLivroNegocios { get; set; }
        public StreamerClientHandlerLivroOfertasAgregado HandlerLivroOfertasAgregado { get; set; }
        public int TentativasSonda { get; set; }

    }

    public class ServidorConexaoStreamer
    {
        private ILog logger;

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


        #region ctor
        public ServidorConexaoStreamer()
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("ServidorConexaoStreamer-", logger.Logger);
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
                foreach (ClientHandlerState state in dctClientes.Values)
                {
                    try
                    {
                        state.HandlerLivroNegocios.Stop();
                        state.HandlerLivroOfertas.Stop();
                        state.HandlerNegocios.Stop();
                        state.HandlerLivroOfertasAgregado.Stop();
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
                int acao = Int32.Parse(dados.acao);
                string tipo = dados.tipo;
                string instrumento = dados.instrumento;
                string sessionID = dados.sessionID;

                logger.Info("SessionID[" + sessionID + "]: Requisicao acao[" + acao + "] tipo[" + tipo + "] instrumento[" + instrumento + "]");

                ClientHandlerState state = dctClientes[args.ClientNumber];
                
                switch (tipo)
                {
                    case ConstantesMDS.TIPO_REQUISICAO_DESTAQUES:
                    case ConstantesMDS.TIPO_REQUISICAO_RANKING:
                    case ConstantesMDS.TIPO_REQUISICAO_NOTICIA:
                        logger.Error("TIPO DE SINAL NAO IMPLEMENTADO [" + tipo + "]");
                        break;
                    case ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS:
                        state.HandlerNegocios.TrataRequisicao(acao, tipo, instrumento, sessionID);
                        break;
                    case ConstantesMDS.TIPO_REQUISICAO_LIVRO_NEGOCIOS:
                        state.HandlerLivroNegocios.TrataRequisicao(acao, tipo, instrumento, sessionID);
                        break;
                    case ConstantesMDS.TIPO_REQUISICAO_LIVRO_OFERTAS:
                        state.HandlerLivroOfertas.TrataRequisicao(acao, tipo, instrumento, sessionID);
                        break;
                    case ConstantesMDS.TIPO_REQUISICAO_LIVRO_OFERTAS_AGREGADO:
                        state.HandlerLivroOfertasAgregado.TrataRequisicao(acao, tipo, instrumento, sessionID);
                        break;
                    default:
                        logger.Error("SessionID[" + sessionID + "]: Tipo de Mensagem invalida: [" + tipo + "]");
                        break;
                }
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
            state.HandlerLivroOfertas = new StreamerClientHandlerLivroOfertas(args.ClientNumber, args.ClientSocket);
            state.HandlerLivroOfertas.Start();
            state.HandlerNegocios = new StreamerClientHandlerNegocios(args.ClientNumber, args.ClientSocket);
            state.HandlerNegocios.Start();
            state.HandlerLivroNegocios = new StreamerClientHandlerLivroNegocios(args.ClientNumber, args.ClientSocket);
            state.HandlerLivroNegocios.Start();
            state.HandlerLivroOfertasAgregado = new StreamerClientHandlerLivroOfertasAgregado(args.ClientNumber, args.ClientSocket);
            state.HandlerLivroOfertasAgregado.Start();
            state.TentativasSonda = 0;

            dctClientes.Add(args.ClientNumber, state);
        }


        private void sockServer_OnClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            logger.Info("Cliente [" + args.ClientNumber + "] desconectou");

            if (dctClientes.ContainsKey(args.ClientNumber))
            {
                ClientHandlerState state = dctClientes[args.ClientNumber];

                state.HandlerLivroNegocios.Stop();
                state.HandlerLivroOfertas.Stop();
                state.HandlerNegocios.Stop();
                state.HandlerLivroOfertasAgregado.Stop();

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
                        foreach (ClientHandlerState state in dctClientes.Values)
                        {
                            try
                            {
                                logger.Info("Enviando sonda para cliente " + state.ClientNumber + "[" + state.ClientSocket.RemoteEndPoint.ToString() + "]");
                                SocketPackage.SendData(sonda, state.ClientSocket);
                                state.TentativasSonda = 0;
                            }
                            catch (Exception ex)
                            {
                                logger.Error("Erro ao enviar sonda para [" + state.ClientNumber + "]");
                                state.TentativasSonda++;

                                if (state.TentativasSonda > 5)
                                {
                                    logger.Error("Erro ao enviar sonda para [" + state.ClientNumber + "] removendo cliente");
                                    toDelete.Add(state.ClientNumber);
                                }
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
            sonda.cabecalho = MDSUtils.montaCabecalhoStreamer(ConstantesMDS.TIPO_REQUISICAO_HB_SONDA, null, 0, null, 2, null);
            sonda.sonda = new Dictionary<string, string>();
            sonda.sonda.Add(ConstantesMDS.HTTP_SONDA_DATA, DateTime.Now.ToString("yyyyMMdd"));
            sonda.sonda.Add(ConstantesMDS.HTTP_SONDA_HORA, DateTime.Now.ToString("HHmmss"));

            string mensagem = Newtonsoft.Json.JsonConvert.SerializeObject(sonda);

            return MDSUtils.montaMensagemHttp(ConstantesMDS.TIPO_REQUISICAO_HB_SONDA, null, null, mensagem);
        }
    }
}
