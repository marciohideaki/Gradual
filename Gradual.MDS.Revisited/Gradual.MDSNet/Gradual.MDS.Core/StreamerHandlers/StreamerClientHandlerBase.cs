using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Gradual.MDS.Eventos.Lib;
using Newtonsoft.Json;
using Gradual.MDS.Core.Lib;
using log4net;
using System.Threading;
using Gradual.MDS.Core.Sinal;

namespace Gradual.MDS.Core.StreamerHandlers
{
    public abstract class StreamerClientHandlerBase
    {
        protected ILog logger;
        protected Socket ClientSocket;
        protected int ClientNumber;
        //protected Dictionary<string, EPStatement> dctEPStatements = new Dictionary<string, EPStatement>();
        protected Dictionary<string, List<string>> dctSessions = new Dictionary<string, List<string>>();
        protected bool bKeepRunning = true;
        protected Thread myThread = null;
        protected string myThreadName;

        public StreamerClientHandlerBase(int clientNumber, Socket clientSocket )
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

        protected abstract void trataAssinatura(string tipo, string instrumento, string sessionID);

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

        /// <summary>
        /// Handler de eventos do Nesper
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        //protected abstract void eventUpdateHandler(object sender, UpdateEventArgs args);

        protected abstract void listenEvents();
        protected abstract void unlistenEvents();
        protected abstract void queueProcessor();

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
    }
}
