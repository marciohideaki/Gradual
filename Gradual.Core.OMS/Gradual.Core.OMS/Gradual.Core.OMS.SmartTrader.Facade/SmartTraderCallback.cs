using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using System.Threading;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace Gradual.Core.OMS.SmartTrader.Facade
{
    public class SmartTraderCallback: IRoteadorOrdensCallback
    {

        #region IRoteadorOrdensCallback Members

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IAssinaturasRoteadorOrdensCallback roteador;
        private bool _bKeepRunning = true;

        private const string SMARTTRADELABEL = "SMART";


        #endregion

        #region Events

        public event EventHandler SmartOrderFilled;   

        #endregion

        public SmartTraderCallback()
        {
            roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);
        }     

        public void StartRouterCallBack()
        {
            IAssinaturasRoteadorOrdensCallback roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);

            logger.Info("Callback ativado com sucesso");
            AssinarExecucaoOrdemResponse resp = roteador.AssinarExecucaoOrdens(new AssinarExecucaoOrdemRequest());
            logger.Info("Assinatura de execução realizada com sucesso");
            AssinarStatusConexaoBolsaResponse cnxresp = roteador.AssinarStatusConexaoBolsa(new AssinarStatusConexaoBolsaRequest());
            logger.Info("Assinatura de execução realizada com sucesso");

            Thread thrMonitorRoteador = new Thread(new ThreadStart(RunMonitor));
            thrMonitorRoteador.Start();


        }


        #region Monitoramento

        /// <summary>
        ///MONITOR DE CONEXOES DO ROTEADOR
        /// </summary>
        private void RunMonitor()
        {
            try
            {
                logger.Info("Iniciando thread de monitoracao do roteador de ordens");
                int _iMonitorConexoes = 0;

                if (roteador == null)
                    roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);

                while (_bKeepRunning)
                {
                    // 4 * 250 = 1 segundo 
                    if (_iMonitorConexoes == 30 * 4)
                    {
                        lock (roteador)
                        {
                            try
                            {
                                if (roteador == null)
                                    roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);
                            }
                            catch (Exception ex)
                            {
                                Ativador.AbortChannel(roteador);
                                roteador = null;
                            }
                        }
                        _iMonitorConexoes = 0;
                    }
                    else
                    {
                        _iMonitorConexoes++;
                    }

                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                logger.Error("RunMonitor(): " + ex.Message, ex);
            }

            logger.Info("THREAD DE MONITORAMENTO DO ROTEADOR FINALIZADA");
        }

        #endregion


        #region IRoteadorOrdensCallback Members

        public void OrdemAlterada(Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemInfo report)
        {
            try
            {
                // - COLOCAR EM ARQUIVO DE CONFIG [ HARD CODED -> TESTES ]
                if ((report.ChannelID == 362) && (report.Account == 319400))
                {
                    if ((report.OrdStatus != OrdemStatusEnum.REJEITADA) &&
                        (report.OrdStatus != OrdemStatusEnum.EXPIRADA) &&
                        (report.OrdStatus != OrdemStatusEnum.SUSPENSA) &&
                        (report.OrdStatus != OrdemStatusEnum.ENVIADAPARAOCANAL) &&
                        (report.OrdStatus != OrdemStatusEnum.ENVIADAPARAABOLSA) &&
                        (report.OrdStatus != OrdemStatusEnum.ENVIADAPARAOROTEADORDEORDENS))
                    {


                        logger.Info("CALLBACK INICIALIZADO COM SUCESSO.");
                        logger.Info("INVOCA THREAD TratarNotificacaoExecucao");

                        if (report.Memo5149 == SMARTTRADELABEL)
                        {
                            ThreadPool.QueueUserWorkItem(
                                new WaitCallback(
                                    TratarNotificacaoExecucao), report);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao receber o callback do acompanhamento de ordens...", ex);
                logger.Error("Descrição do erro..." + ex.Message);
            }
        }

        private void TratarNotificacaoExecucao(object report)
        {
            EventHandler handler = SmartOrderFilled;
            if (handler != null)
            {
                handler(report, EventArgs.Empty);
            }
        }

        public void StatusConexaoAlterada(Gradual.OMS.RoteadorOrdens.Lib.Dados.StatusConexaoBolsaInfo status)
        {
           
        }

        #endregion
    }
}
