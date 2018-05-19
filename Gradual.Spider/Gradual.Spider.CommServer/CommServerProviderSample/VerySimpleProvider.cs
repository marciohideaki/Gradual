using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using Gradual.Spider.CommSocket;
using Gradual.Spider.Communications.Lib.Mensagens;
using System.Net.Sockets;
using System.Threading;
using log4net;

namespace CommServerProviderSample
{
    public class VerySimpleProvider : IServicoControlavel
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _status = ServicoStatus.Parado;
        private SpiderSocket sock;
        private Dictionary<string, long> dctInstrumentos = new Dictionary<string, long>();
        private bool bKeepRunning = false;
        private Thread thDifusao = null;

        public void IniciarServico()
        {
            bKeepRunning = true;

            thDifusao = new Thread(new ThreadStart(difusaoProc));
            thDifusao.Start();

            sock = new SpiderSocket();
            sock.AddHandler<SondaCommServer>(new ProtoObjectReceivedHandler<SondaCommServer>(OnSonda));
            sock.AddHandler<AssinaturaCommServerRequest>(new ProtoObjectReceivedHandler<AssinaturaCommServerRequest>(OnChegadaAssinatura));
            sock.AddHandler<CancelAssinaturaCommServerRequest>(new ProtoObjectReceivedHandler<CancelAssinaturaCommServerRequest>(OnCancelaAssinatura));

            sock.StartListen(21004);
        }

        private void OnSonda(object sender, int clientNumber, Socket clientSock, SondaCommServer args)
        {
            SpiderSocket.SendObject(args, clientSock);
        }

        private void OnChegadaAssinatura(object sender, int clientNumber, Socket clientSock, AssinaturaCommServerRequest args)
        {
            string SessionID = args.SessionID;

            // Obtem o objeto com detalhes da assinatura 
            List<Object> objetos = AssinaturaCommServerRequest.GetObjects(args);
            SimpleProviderSubscriptionRequest assinatura = objetos[0] as SimpleProviderSubscriptionRequest;

            if (assinatura != null)
            {
                // Gera o "snapshot" para essa assinatura
                SimpleProviderPublishing pub = new SimpleProviderPublishing();

                pub.Instrumento = assinatura.Instrumento;
                pub.TimeStampSinal = DateTime.Now;
                pub.SinalDescription = "Snapshoshots";

                // Envelopa o snapshot, informando os detalhes da assinatura
                PublishCommServerResponse response = new PublishCommServerResponse();

                response.IsSnapshot = true;
                response.SessionID = SessionID;
                response.Instrumento = assinatura.Instrumento;
                PublishCommServerResponse.AddObject(response, pub);

                logger.Debug("Enviando snapshot de [" + assinatura.Instrumento + "] para id [" + SessionID + "]");
                // Envia o snapshot
                SpiderSocket.SendObject(response, clientSock);

                // Guarda o instrumento para a distribuicao fake
                lock (dctInstrumentos)
                {
                    if (!dctInstrumentos.ContainsKey(assinatura.Instrumento))
                    {
                        dctInstrumentos.Add(assinatura.Instrumento, DateTime.Now.Ticks);
                    }
                }
            }
        }

        private void OnCancelaAssinatura(object sender, int clientNumber, Socket clientSock, CancelAssinaturaCommServerRequest args)
        {
            string SessionID = args.SessionID;

            logger.Info("OnCancelaAssinatura client[" + clientNumber + "] SessionID[" + args.SessionID + "] Inst[" + args.Instrumento + "] UltAss[" + args.UltimoAssinante + "]");

            // Guarda o instrumento para a distribuicao fake
            if (args.UltimoAssinante)
            {
                lock (dctInstrumentos)
                {
                    if (dctInstrumentos.ContainsKey(args.Instrumento))
                    {
                        dctInstrumentos.Remove(args.Instrumento);
                    }
                }
            }
        }

        public void PararServico()
        {
            bKeepRunning = false;

            Thread.Sleep(3000);
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        public void difusaoProc()
        {
            logger.Info("Inicio difusaoProc");

            while (bKeepRunning)
            {
                try
                {
                    List<string> enviados = new List<string>();
                    lock (dctInstrumentos)
                    {
                        foreach (KeyValuePair<string, long> item in dctInstrumentos)
                        {
                            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - item.Value);

                            if (ts.TotalMilliseconds > 10000)
                            {
                                // Gera o "sinal" para esse instrumento
                                SimpleProviderPublishing pub = new SimpleProviderPublishing();

                                pub.Instrumento = item.Key;
                                pub.TimeStampSinal = DateTime.Now;
                                pub.SinalDescription = "Incremental "  + DateTime.Now.Ticks.ToString();

                                // Envelopa o snapshot, informando os detalhes da assinatura
                                PublishCommServerResponse response = new PublishCommServerResponse();

                                response.Instrumento = item.Key;
                                response.IsSnapshot = false;
                                PublishCommServerResponse.AddObject(response, pub);

                                // Envia o "sinal"
                                logger.Debug("Enviando sinal de [" + item.Key + "]");
                                sock.SendToAll(response);

                                enviados.Add(item.Key);
                            }
                        }

                        foreach( string instrumento in enviados)
                            dctInstrumentos[instrumento] = DateTime.Now.Ticks;
                    }

                    Thread.Sleep(250);
                }
                catch (Exception ex)
                {
                    logger.Error("difusaoProc() : " + ex.Message, ex);
                }
            }

            logger.Info("Fianlizando difusaoProc");
        }
    }
}
