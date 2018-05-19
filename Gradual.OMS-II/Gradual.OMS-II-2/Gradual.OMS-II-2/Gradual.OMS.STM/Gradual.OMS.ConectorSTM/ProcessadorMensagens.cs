using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.espertech.esper.client;
using Gradual.OMS.ConectorSTM.Eventos;
using log4net;
using System.Threading;

namespace Gradual.OMS.ConectorSTM
{
    public class ProcessadorMensagens: StatementAwareUpdateListener
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Queue<EventoSTM> queue;
        private Semaphore queueSem;
        private bool _bKeepRunning = false;
        private ParserCBLCMessage parserCBLC = new ParserCBLCMessage();
        private ParserMegaMessage parserMega = new ParserMegaMessage();
        private Thread _me;


        public ProcessadorMensagens()
        {
            queue = new Queue<EventoSTM>();
            queueSem = new Semaphore(1, short.MaxValue);
        }


        public void Start()
        {
            _bKeepRunning = true;
            _me = new Thread(new ThreadStart(Run));
            _me.Start();
        }

        public void Stop()
        {
            _bKeepRunning = false;
            while (_me != null && _me.IsAlive)
            {
                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// Thread de processamento das mensagen
        /// </summary>
        public void Run()
        {
            logger.Info("Iniciando thread de tratamento das mensagens do STM");

            prepareNesper();

            int qtdeFila = 0;
            while (_bKeepRunning)
            {
                try
                {
                    queueSem.WaitOne(250);

                    lock (queue)
                    {
                        qtdeFila = queue.Count;
                    }

                    for (int i = 0; i < qtdeFila; i++)
                    {
                        EventoSTM evento;
                        lock (queue)
                        {
                            evento = queue.Dequeue();
                        }

                        switch (evento.Tipo)
                        {
                            case EventoSTM.TIPO_MSG_CBLC:
                                parserCBLC.Parse(evento);
                                break;
                            case EventoSTM.TIPO_MSG_MEGA:
                                parserMega.Parse(evento);
                                break;
                            default:
                                logger.Error("Tipo de mensagem nao esperado [" + evento.Tipo + "]");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Run() Error: " + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Handler dos eventos do Nesper
        /// </summary>
        /// <param name="newEvents"></param>
        /// <param name="oldEvents"></param>
        /// <param name="statement"></param>
        /// <param name="epServiceProvider"></param>
        public void Update(EventBean[] newEvents, EventBean[] oldEvents, EPStatement statement, EPServiceProvider epServiceProvider)
        {
            foreach (EventBean evento in newEvents)
            {
                lock (queue)
                {
                    queue.Enqueue((EventoSTM)evento.Underlying);
                }
            }

            queueSem.Release();

            logger.Info("Update(): mensagens na fila: " + queue.Count);
        }



        /// <summary>
        /// prepara esta classe para receber os eventos do Nesper
        /// </summary>
        private void prepareNesper()
        {
            String consultaEsper = "select * from EventoSTM";

            EPStatement comandoEsper = ServicoConectorSTM.epService.EPAdministrator.CreateEPL(consultaEsper);
            
            comandoEsper.AddListener(this);
        }
    }
}
