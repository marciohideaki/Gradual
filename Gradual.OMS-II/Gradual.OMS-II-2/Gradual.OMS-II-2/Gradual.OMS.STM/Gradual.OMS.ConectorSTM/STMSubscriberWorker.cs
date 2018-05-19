using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.ConectorSTM.Lib;
using System.Threading;
using com.espertech.esper.client;
using log4net;
using Gradual.OMS.ConectorSTM.Eventos;
using Gradual.OMS.ConectorSTM.Lib.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.ConectorSTM
{
    public class STMSubscriberWorker : StatementAwareUpdateListener
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Queue<object> queue;
        private Semaphore queueSem;
        private IServicoSTMCallback subscriberChannel;
        private Thread _me = null;
        private bool _bKeepRunning = false;
        private string eplName1;
        private string eplName2;
        private Guid guid;

        public STMSubscriberWorker(IServicoSTMCallback subscriber)
        {
            subscriberChannel = subscriber;
            queue = new Queue<object>();
            queueSem = new Semaphore(1, short.MaxValue);

            guid = Guid.NewGuid();

            eplName1 = guid.ToString() + "-epl1";
            eplName2 = guid.ToString() + "-epl2";
        }

        public void Start()
        {
            logger.Info("Inicializando Worker [" + guid.ToString() + "]");

            _bKeepRunning = true;
            _me = new Thread(new ThreadStart(Run));
            _me.Start();

            logger.Info("Worker inicializado [" + guid.ToString() + "]");
        }


        public void Stop()
        {
            logger.Info("Finalizando Worker [" + guid.ToString() + "]");

            _bKeepRunning = false;
            while (_me != null && _me.IsAlive)
            {
                Thread.Sleep(250);
            }

            logger.Info("Worker finalizado [" + guid.ToString() + "]");
        }

        public void Run()
        {
            try
            {
                logger.Info("Iniciando thread de repasse de mensagens ao subscriber");

                prepareNesper();

                int qtdeFila = 0;
                long lastHeartbeat = 0;
                while (_bKeepRunning)
                {
                    queueSem.WaitOne(250);

                    lock (queue)
                    {
                        qtdeFila = queue.Count;
                    }

                    for (int i = 0; i < qtdeFila; i++)
                    {
                        object evento;
                        string msgid="";
                        lock (queue)
                        {
                            evento = queue.Dequeue();
                        }

                        if (evento.GetType() == typeof(EventoMega))
                        {
                            EventoMega mega = (EventoMega)evento;

                            msgid = mega.MsgID;

                            repassaEventoMega(mega);
                        }

                        if (evento.GetType() == typeof(EventoCBLC))
                        {
                            EventoCBLC cblc = (EventoCBLC)evento;
                            msgid = cblc.MsgID;
                            repassaEventoCBLC(cblc);
                        }

                        logger.Debug("Encaminhou mensagem [" + msgid + "]");
                    }

                    TimeSpan tspan = new TimeSpan(DateTime.Now.Ticks - lastHeartbeat);

                    if (tspan.TotalSeconds > 30)
                    {
                        lastHeartbeat = DateTime.Now.Ticks;
                        subscriberChannel.OnHeartBeat();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Run() Error: " + ex.Message);

                if (Ativador.IsValidChannel(subscriberChannel))
                {
                    Ativador.AbortChannel(subscriberChannel);
                }
            }

            ServicoConectorSTM.epService.EPAdministrator.GetStatement(eplName1).RemoveListener(this);
            ServicoConectorSTM.epService.EPAdministrator.GetStatement(eplName2).RemoveListener(this);

            logger.Info("Thread finalizada [" + guid.ToString() + "]");
        }

        /// <summary>
        /// prepara esta classe para receber os eventos do Nesper
        /// </summary>
        private void prepareNesper()
        {
            String consultaEsper = "select * from EventoCBLC";

            EPStatement comandoEsper = ServicoConectorSTM.epService.EPAdministrator.CreateEPL(consultaEsper, eplName1);

            comandoEsper.AddListener(this);

            consultaEsper = "select * from EventoMega";

            comandoEsper = ServicoConectorSTM.epService.EPAdministrator.CreateEPL(consultaEsper, eplName2);

            comandoEsper.AddListener(this);
        }


        /// <summary>
        /// Executa a chamada do callback para mensagens CBLC
        /// </summary>
        /// <param name="cblc"></param>
        private void repassaEventoCBLC(EventoCBLC cblc)
        {
            switch (cblc.Cabecalho)
            {
                case ParserCBLCMessage.TIPO_MSG_AN:
                    {
                        CBLCConfirmacaoNegocioMegaBolsaInfo info = (CBLCConfirmacaoNegocioMegaBolsaInfo) cblc.Info;
                        subscriberChannel.OnCBLC_ConfirmacaoNegocioMegabolsa(info);
                    }
                    break;
                case ParserCBLCMessage.TIPO_MSG_ANF:
                    {
                        CBLCConfirmacaoNegocioBovespaFixInfo fixinfo = (CBLCConfirmacaoNegocioBovespaFixInfo) cblc.Info;
                        subscriberChannel.OnCBLC_ConfirmacaoNegocioBovespaFIX(fixinfo);
                    }
                    break;
                default:
                    throw new Exception("repassaEventoCBLC() Error: Tipo de mensagem invalida: " + cblc.Cabecalho);
            }
        }


        /// <summary>
        /// Executa a chamada do callback para as mensagems MegaBolsa
        /// </summary>
        /// <param name="mega"></param>
        private void repassaEventoMega(EventoMega mega)
        {
            switch (mega.Function)
            {
                case ParserMegaMessage.TIPO_MSG_0100:
                    {
                        MEGA0100NotificacaoCancelamentoNegocioInfo info = (MEGA0100NotificacaoCancelamentoNegocioInfo )mega.Info;
                        subscriberChannel.OnMega_CancelamentoNegocio(info);
                    }
                    break;
                case ParserMegaMessage.TIPO_MSG_0103:
                    {
                        MEGA0103CriacaoNegocioInfo info = (MEGA0103CriacaoNegocioInfo)mega.Info;
                        subscriberChannel.OnMega_CriacaoNegocio(info);
                    }
                    break;
                case ParserMegaMessage.TIPO_MSG_0105:
                    {
                        MEGA0105NotificacaoExecucaoInfo info = (MEGA0105NotificacaoExecucaoInfo)mega.Info;
                        subscriberChannel.OnMega_NotificacaoExecucao(info);
                    }
                    break;
                case ParserMegaMessage.TIPO_MSG_0138:
                    {
                        MEGA0138OrdemEliminadaInfo info = (MEGA0138OrdemEliminadaInfo)mega.Info;
                        subscriberChannel.OnMega_OrdemEliminada(info);
                    }
                    break;
                case ParserMegaMessage.TIPO_MSG_0172:
                    {
                        MEGA0172ConfirmacaoOrdemInfo info = (MEGA0172ConfirmacaoOrdemInfo)mega.Info;
                        subscriberChannel.OnMega_ConfirmacaoOrdem(info);
                    }
                    break;
                default:
                    throw new Exception("repassaEventoMega() Tipo de mensagem invalida: " + mega.Function);
            }
        }



        /// <summary>
        /// Nesper - handler
        /// </summary>
        /// <param name="newEvents"></param>
        /// <param name="oldEvents"></param>
        /// <param name="statement"></param>
        /// <param name="epServiceProvider"></param>
        public void Update(EventBean[] newEvents, EventBean[] oldEvents, EPStatement statement, EPServiceProvider epServiceProvider)
        {
            try
            {
                if (_bKeepRunning)
                {
                    foreach (EventBean evento in newEvents)
                    {
                        lock (queue)
                        {
                            queue.Enqueue(evento.Underlying);
                        }
                    }
                }

                queueSem.Release();

                logger.Info("Update(): mensagens na fila: " + queue.Count);
            }
            catch (Exception ex)
            {
                logger.Error("Update() Error: " + ex.Message, ex);
            }
        }
    }
}
