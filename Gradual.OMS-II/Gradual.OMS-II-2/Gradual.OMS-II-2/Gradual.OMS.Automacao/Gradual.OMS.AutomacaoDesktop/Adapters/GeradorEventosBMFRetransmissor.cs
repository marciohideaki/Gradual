using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.espertech.esper.client;
using com.espertech.esper.compat.collections;
using log4net;
using Gradual.OMS.AutomacaoDesktop.Events;
using System.Threading;

namespace Gradual.OMS.AutomacaoDesktop.Adapters
{
    public class GeradorEventosBMFRetransmissor
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EPServiceProvider epService = null;
        private LinkedBlockingQueue<string> filaMensagensRetransmissorBMF;
        private bool _bKeepRunning = false;
        private Thread _me = null;

        public GeradorEventosBMFRetransmissor(
            EPServiceProvider epService,
            LinkedBlockingQueue<string> filaMensagensRetransmissorBMF)
        {
            this.epService = epService;
            this.filaMensagensRetransmissorBMF = filaMensagensRetransmissorBMF;

            return;
        }

        public void Start()
        {
            logger.Info("Iniciando GeradorEventosBMFRetransmissor");
            _bKeepRunning = true;
            _me = new Thread(new ThreadStart(Run));
            _me.Start();
            logger.Info("Thread GeradorEventosBMFRetransmissor iniciada");
        }

        public void Stop()
        {
            logger.Info("Finalizando GeradorEventosBMFRetransmissor");
            _bKeepRunning = false;
            if (_me != null)
            {
                while (_me.IsAlive)
                    Thread.Sleep(250);
            }
            logger.Info("GeradorEventosBMFRetransmissor finalizada");
        }

        private void Run()
        {
            string linha;
            EventoBMF evento;
            long antes;
            long depois;

            while (_bKeepRunning)
            {
                linha = null;
                try
                {
                    linha = filaMensagensRetransmissorBMF.Pop();
                }
                catch (Exception intExcept)
                {
                    logger.Error("InterruptedException na leitura da fila de mensagens:");
                    logger.Debug(intExcept.Message);
                    return;
                }
                antes = DateTime.Now.Ticks;

                string sequencia = linha.Substring(
                        EventoBMF.SEQNUM_INI,
                        EventoBMF.SEQNUM_FIM - EventoBMF.SEQNUM_INI).Trim();
                string tipo = linha.Substring(
                        EventoBMF.TYPE_INI,
                        EventoBMF.TYPE_FIM - EventoBMF.TYPE_INI).Trim();
                string instrumento = linha.Substring(
                        EventoBMF.SYMBOL_INI,
                        EventoBMF.SYMBOL_FIM - EventoBMF.SYMBOL_INI).Trim();
                string codigoInstrumento = linha.Substring(
                        EventoBMF.SECURITYID_INI,
                        EventoBMF.SECURITYID_FIM - EventoBMF.SECURITYID_INI).Trim();
                string mensagem = linha.Substring(
                        EventoBMF.BODY_INI);

                if (tipo.Equals(ConstantesMDS.TIPO_REQUISICAO_BMF_HEARTBEAT))
                    continue;

                evento = new EventoBMF(sequencia, tipo, instrumento, codigoInstrumento, mensagem);
                epService.EPRuntime.SendEvent(evento);

                depois = DateTime.Now.Ticks;
                TimeSpan elapsed = new TimeSpan(depois - antes);
                logger.Debug("Mensagem[" + linha + "] Duracao: " + elapsed.TotalMilliseconds + "ms");
            }
        }

    }
}
