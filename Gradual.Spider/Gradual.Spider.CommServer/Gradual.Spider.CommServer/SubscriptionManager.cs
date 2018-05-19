using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Net.Sockets;
using Gradual.Spider.Communications.Lib.Mensagens;
using log4net;
using System.Threading;
using System.Configuration;
using Gradual.Spider.CommSocket;

namespace Gradual.Spider.CommServer
{
    public class SubscriptionManager
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static SubscriptionManager _me = null;
        private bool bKeepRunning = false;

        private ConcurrentDictionary<Type, ConcurrentDictionary<string, List<string>>> subcribers = new ConcurrentDictionary<Type, ConcurrentDictionary<string, List<string>>>();
        private ConcurrentDictionary<string, Socket> clientSessions = new ConcurrentDictionary<string, Socket>();
        private ConcurrentDictionary<Type, ConcurrentDictionary<string, List<string>>> snapshotWait = new ConcurrentDictionary<Type, ConcurrentDictionary<string, List<string>>>();

        private Thread[] publisherThreads;
        private int numPublisher = System.Environment.ProcessorCount;

        private ConcurrentQueue<PublishCommServerResponse> publishQueue = new ConcurrentQueue<PublishCommServerResponse>();

        public static SubscriptionManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new SubscriptionManager();
                }
                return _me;
            }
        }

        public SubscriptionManager()
        {
            
            if (ConfigurationManager.AppSettings["NumPublisherThreads"] != null)
            {
                numPublisher = Convert.ToInt32(ConfigurationManager.AppSettings["NumPublisherThreads"].ToString());
            }

            publisherThreads = new Thread[numPublisher];
        }

        public void Start()
        {
            bKeepRunning = true;

            logger.Info("Iniciando as threads de difusao");
            for (int i = 0; i < numPublisher; i++)
            {
                publisherThreads[i] = new Thread(new ParameterizedThreadStart(publishProc));
                publisherThreads[i].Start(i);
            }
        }

        public void Subscribe(Socket socket, string sessionID, Type tipo, string instrumento, object objeto)
        {
            clientSessions.TryAdd(sessionID, socket);

            // Coloca a sessionID para 
            ConcurrentDictionary<string, List<string>> dctInstrumentos = 
                snapshotWait.GetOrAdd(tipo, new ConcurrentDictionary<string, List<string>>());

            List<string> lstSessions = dctInstrumentos.GetOrAdd(instrumento, new List<string>());

            if (!lstSessions.Contains(sessionID))
                lstSessions.Add(sessionID);

            AssinaturaCommServerRequest req = new AssinaturaCommServerRequest();
            req.SessionID = sessionID;
            req.TiposAssinados.Add(tipo);
            AssinaturaCommServerRequest.AddObject(req, objeto);

            ProvidersManager.Instance.SendRequest(tipo, req);
        }


        public void UnSubscribe(Socket socket, string sessionID, Type tipo, string instrumento)
        {
            bool bUltimoAssinante = false;

            if (clientSessions.ContainsKey(sessionID))
            {
                // Coloca a sessionID para 

                ConcurrentDictionary<string, List<string>> dctInstrumentos = null;

                if (subcribers.TryGetValue(tipo, out dctInstrumentos))
                {

                    List<string> lstSessions = dctInstrumentos.GetOrAdd(instrumento, new List<string>());

                    if (!lstSessions.Contains(sessionID))
                    {
                        logger.Warn("SessionID nao encontrado pra instrumento [" + instrumento + "] Type [" + tipo.ToString() + "]");
                        return;
                    }
                    lstSessions.Remove(sessionID);

                    if (lstSessions.Count == 0)
                    {
                        logger.Info("Cancelada ultima sessao do instrumento [" + instrumento + "] Type [" + tipo.ToString() + "] removendo do dicionario de instrumento x sessoes");
                        dctInstrumentos.TryRemove(instrumento, out lstSessions);
                        bUltimoAssinante = true;
                    }

                    if (dctInstrumentos.Count == 0)
                    {
                        logger.Info("Nao ha mais assinaturas de intrumentos para o Type [" + tipo.ToString() + "] removendo do dicionario de tipos");
                        this.subcribers.TryRemove(tipo, out dctInstrumentos);
                        bUltimoAssinante = true;
                    }

                    CancelAssinaturaCommServerRequest req = new CancelAssinaturaCommServerRequest();

                    req.SessionID = sessionID;
                    req.Instrumento = instrumento;
                    req.TiposAssinados.Add(tipo);
                    req.UltimoAssinante = bUltimoAssinante;

                    ProvidersManager.Instance.SendRequest(tipo, req);

                    return;
                }

                logger.Warn("Nao ha assinantes para o Type [" + tipo.ToString() + "]");
                return;
            }

            logger.Warn("SessionID nao esta na lista de clientes x socket");
        }


        
        public void Publish(PublishCommServerResponse args)
        {
            publishQueue.Enqueue(args);
        }


        private void publishProc(object param)
        {
            logger.Info("Iniciando thread de distribuicao de sinal");

            while (bKeepRunning)
            {
                try
                {
                    PublishCommServerResponse resp = null;
                    if (publishQueue.TryDequeue(out resp))
                    {
                        if (resp.IsSnapshot)
                        {
                            List<string> lstSessions = null;

                            List<Object> objetos = PublishCommServerResponse.GetObjects(resp);
                            foreach (Object objeto in objetos)
                            {
                                ConcurrentDictionary<string, List<string>> dctInstrumentos = null;
                                if (snapshotWait.TryGetValue(objeto.GetType(), out dctInstrumentos))
                                {
                                    if ( dctInstrumentos.TryGetValue(resp.Instrumento, out lstSessions))
                                    {
                                        if (lstSessions.Contains(resp.SessionID))
                                        {
                                            Socket socket = clientSessions[resp.SessionID];

                                            logger.Debug("Enviando snapshot de [" + resp.Instrumento + "] Type [" + objeto.ToString().ToString() + "] SessionID [" + resp.SessionID + "]");

                                            SpiderSocket.SendObject(objeto, socket);

                                            //Remove a sessao da lista snapshotWait
                                            lstSessions.Remove(resp.SessionID);

                                            dctInstrumentos = subcribers.GetOrAdd(objeto.GetType(), new ConcurrentDictionary<string, List<string>>());
                                            lstSessions = dctInstrumentos.GetOrAdd(resp.Instrumento, new List<string>());
                                            if ( !lstSessions.Contains(resp.SessionID))
                                                lstSessions.Add(resp.SessionID);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            List<string> lstSessions = null;
                            ConcurrentDictionary<string, List<string>> dctInstrumentos  = null;

                            List<Object> objetos = PublishCommServerResponse.GetObjects(resp);
                            foreach (Object objeto in objetos)
                            {
                                if (subcribers.TryGetValue(objeto.GetType(), out dctInstrumentos))
                                {
                                    if (dctInstrumentos.TryGetValue(resp.Instrumento, out lstSessions))
                                    {
                                        foreach (string session in lstSessions)
                                        {
                                            logger.Debug("Difundindo [" + resp.Instrumento + "] Type [" + objeto.GetType().ToString() + "]");

                                            Socket socket = clientSessions[session];

                                            SpiderSocket.SendObject(objeto, socket);
                                        }
                                    }
                                }
                            }
                        }
                        continue;
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Error("publishProc(): " + ex.Message, ex);
                }
            }

            logger.Info("Iniciando thread de distribuicao de sinal");

        }

        public void UnSubscribeAll(string sessionID)
        {
            bool bUltimoAssinante = false;
            if ( clientSessions.ContainsKey(sessionID) )
            {
                Socket socket = clientSessions[sessionID];
                List<Type> tipos = subcribers.Keys.ToList();

                foreach (Type tipo in tipos)
                {
                    ConcurrentDictionary<string, List<string>> dctInstrumento = null;

                    if (subcribers.TryGetValue(tipo, out dctInstrumento))
                    {
                        List<string> instrumentos = dctInstrumento.Keys.ToList();

                        foreach (string instrumento in instrumentos)
                        {
                            List<string> sessions = null;
                            if ( dctInstrumento.TryGetValue(instrumento, out sessions) )
                            {
                                if (sessions.Contains(sessionID))
                                {
                                    sessions.Remove(sessionID);
                                    dctInstrumento.AddOrUpdate(instrumento, sessions, (key, oldValue) => sessions);
                                    if (sessions.Count == 0)
                                    {
                                        logger.Info("Nao ha mais assinaturas de intrumentos para o Type [" + tipo.ToString() + "] removendo do dicionario de tipos");
                                        bUltimoAssinante = true;
                                        dctInstrumento.TryRemove(instrumento, out sessions);
                                    }


                                    CancelAssinaturaCommServerRequest req = new CancelAssinaturaCommServerRequest();

                                    req.SessionID = sessionID;
                                    req.Instrumento = instrumento;
                                    req.TiposAssinados.Add(tipo);
                                    req.UltimoAssinante = bUltimoAssinante;

                                    ProvidersManager.Instance.SendRequest(tipo, req);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
