using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Net.Sockets;
using System.Threading;
using Gradual.OMS.SpreadMonitor.Lib.Eventos;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using Gradual.OMS.SpreadMonitor.Lib.Dados;

namespace Gradual.OMS.SpreadMonitor
{
    public class StreamerClientHandler
    {
        protected ILog logger;
        protected Socket ClientSocket;
        protected int ClientNumber;
        //protected Dictionary<string, EPStatement> dctEPStatements = new Dictionary<string, EPStatement>();
        protected Dictionary<string, List<string>> dctSessions = new Dictionary<string, List<string>>();
        protected bool bKeepRunning = true;
        protected Thread myThread = null;
        protected string myThreadName;
        private HttpAlgoritmoEventHander eventHandler = null;
        private Object objLockSnapshot = new Object();
        protected ConcurrentQueue<string> queueToStreamer = new ConcurrentQueue<string>();


        public StreamerClientHandler(int clientNumber, Socket clientSocket)
        {
            ClientNumber = clientNumber;
            ClientSocket = clientSocket;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mensagem"></param>
        public void TrataRequisicao(int acao, string tipo, string instrumento, string sessionID)
        {

            logger.Debug("SessionID[" + sessionID + "]: Requisicao acao[" + acao + "] tipo[" + tipo + "] instrumento[" + instrumento + "]");


            // Cadastra o statment do Nesper, se ja nao assinou o evento
            if (acao == ConstantesMDS.ACAO_REQUISICAO_ASSINAR)
            {
                assinarSinal(tipo, instrumento);
            }
            else
            {
                if (removerSessionID(tipo, instrumento, sessionID))
                    cancelarSinal(tipo, instrumento);
                return;
            }

            ThreadPool.QueueUserWorkItem(
                new WaitCallback(
                    delegate(object required)
                    {
                        try
                        {
                            trataAssinatura(tipo, instrumento, sessionID);
                            cadastrarSessionID(tipo, instrumento, sessionID);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("trataAssinatura(): " + ex.Message, ex);
                        }
                    }
                )
            );

            return;
        }


        protected void trataAssinatura(string tipo, string idlogin, string sessionID)
        {
            string mensagem = "";
            ThreadPoolManager poolManager = ThreadPoolManager.Instance;
            Dictionary<string, string> cabecalho = MDSUtils.montaCabecalhoStreamer(tipo, null, ConstantesMDS.HTTP_ALGORITMOS_TIPO_ACAO_COMPLETO, idlogin, sessionID);

            logger.DebugFormat("{0} assinatura de {1} de {2}", sessionID, tipo, idlogin);

            // Aqui tem o pulo do gato
            // interrompe o processamento dos eventos ate a chegada do snapshot
            // para nao quebrar a sequencia do sinal do livro
            lock (objLockSnapshot)
            {
                EventoHttpAlgoritmo httpAlgo = new EventoHttpAlgoritmo();
                httpAlgo.idlogin = idlogin;
                httpAlgo.cabecalho = cabecalho;
                httpAlgo.algoritmos = poolManager.SnapshotStreamer(idlogin);

                logger.Debug("Snapshot algoritmos de " + idlogin + ": " + httpAlgo.algoritmos.Count + " items");

                mensagem = JsonConvert.SerializeObject(httpAlgo);
                mensagem = MDSUtils.montaMensagemHttp(ConstantesMDS.TIPO_REQUISICAO_ALGORITMO, idlogin, null, mensagem);

                queueToStreamer.Enqueue(mensagem);
            }
        }

        /// <summary>
        /// Remove o listener do evento associado ao tipo e instrumento
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="instrumento"></param>
        protected void cancelarSinal(string tipo, string instrumento)
        {
            //lock (dctEPStatements)
            //{
            //    logger.Info( "Cancelando Statement para [" + instrumento + "]");
            //    if (!dctEPStatements.ContainsKey(instrumento))
            //    {
            //        logger.Error("Tentativa de cancelar assinatura de sinal sem EPL [" + tipo + "][" + instrumento + "]");
            //        return;
            //    }

            //    EPStatement epl = dctEPStatements[instrumento];
            //    epl.RemoveAllEventHandlers();
            //    dctEPStatements.Remove(instrumento);
            //}
        }

        /// <summary>
        /// Cadastra um listener para o tipo de sinal e instrumento
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="instrumento"></param>
        protected void assinarSinal(string tipo, string instrumento)
        {
            //lock (dctEPStatements)
            //{
            //    if (!dctEPStatements.ContainsKey(instrumento))
            //    {
            //        EPStatement epl = createStatement(tipo, instrumento);

            //        if (epl != null)
            //        {
            //            dctEPStatements.Add(instrumento, epl);
            //        }
            //    }
            //}
        }

        //protected abstract EPStatement createStatement(string tipo, string instrumento);

        protected void listenEvents()
        {
            eventHandler = new HttpAlgoritmoEventHander(despacharEventos);
            ThreadPoolManager.Instance.OnAlgoritmoEvent += eventHandler;
        }

        protected void unlistenEvents()
        {
            if (eventHandler != null)
                ThreadPoolManager.Instance.OnAlgoritmoEvent -= eventHandler;
        }

        private void despacharEventos(object sender, HttpAlgoritmoEventArgs args)
        {
            try
            {
                string mensagem = null;

                EventoHttpAlgoritmo httpAlgo = args.Evento;

                if (dctSessions.ContainsKey(httpAlgo.idlogin))
                {
                    mensagem = JsonConvert.SerializeObject(httpAlgo);
                    mensagem = MDSUtils.montaMensagemHttp(ConstantesMDS.TIPO_REQUISICAO_ALGORITMO, httpAlgo.idlogin, null, mensagem);

                    if (!String.IsNullOrEmpty(mensagem))
                    {
                        queueToStreamer.Enqueue(mensagem);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("despacharEventos(): " + ex.Message, ex);
            }

        }

        protected void cadastrarSessionID(string tipo, string instrumento, string sessionID)
        {
            lock (dctSessions)
            {
                if (!dctSessions.ContainsKey(instrumento))
                {
                    dctSessions.Add(instrumento, new List<string>());
                }

                List<string> sessoes = dctSessions[instrumento];

                if (!sessoes.Contains(sessionID))
                    sessoes.Add(sessionID);

                dctSessions[instrumento] = sessoes;
            }
        }

        /// <summary>
        /// Remover SessionID do controle de assinaturas.
        /// Se for a ultima sessao daquele tipo+instrumento, retorna true
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="instrumento"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        protected bool removerSessionID(string tipo, string instrumento, string sessionID)
        {
            lock (dctSessions)
            {
                if (dctSessions.ContainsKey(instrumento))
                {
                    List<string> sessoes = dctSessions[instrumento];

                    if (sessoes.Contains(sessionID))
                    {
                        logger.Info("Removendo sessao [" + sessionID + "] da lista de [" + instrumento + "]");
                        sessoes.Remove(sessionID);
                    }

                    if (sessoes.Count > 0)
                    {
                        logger.Info("Ainda restam " + sessoes.Count + " assinaturas de [" + instrumento + "]");
                        return false;
                    }

                    logger.Info("Removendo [" + instrumento + "] do dicionario de sessoes");
                    dctSessions.Remove(instrumento);
                }
            }

            return true;
        }

        public void Start()
        {
            myThread = new Thread(new ThreadStart(queueProcessor));
            myThread.Name = myThreadName;
            myThread.Start();

            listenEvents();
        }

        public void Stop()
        {
            unlistenEvents();

            bKeepRunning = false;
            while (myThread != null && myThread.IsAlive)
            {
                Thread.Sleep(250);
            }

            logger.Info(myThreadName + " Finalizada");
        }

        protected void queueProcessor()
        {
            logger.Info("Iniciando processamento da fila de envio streamer de Livro de Negocios");
            long lastLogTicks = 0;

            while (bKeepRunning)
            {
                try
                {
                    string msgLNG = null;
                    lock (objLockSnapshot)
                    {
                        if (queueToStreamer.TryDequeue(out msgLNG))
                        {
                            if (!String.IsNullOrEmpty(msgLNG))
                            {
                                SocketPackage.SendData(msgLNG, ClientSocket);
                            }

                            //if (MDSUtils.shouldLog(lastLogTicks))
                            //{
                            //    lastLogTicks = DateTime.Now.Ticks;
                            //    logger.Info("Mensagens na fila: " + queueToStreamer.Count);
                            //}

                            continue;
                        }
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Error("queueProcessor(): " + ex.Message, ex);
                }

            }

            logger.Info("Finalizando processamento da fila de envio streamer de algoritmos");
        }

    }
}
