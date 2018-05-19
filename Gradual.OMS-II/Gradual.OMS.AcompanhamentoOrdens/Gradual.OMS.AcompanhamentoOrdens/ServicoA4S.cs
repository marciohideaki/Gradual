using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.A4s.Lib;
using System.Threading;
using Gradual.OMS.Library;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;


namespace Gradual.OMS.ServicoA4S
{
    public class ServicoA4S : IServicoControlavel, IRoteadorOrdensCallback
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _servicoStatus = ServicoStatus.Parado;

        private Queue<OrdemInfoStreamer> queueOrdens = new Queue<OrdemInfoStreamer>();
        private Queue<StatusConexaoBolsaStreamer> queueStatus = new Queue<StatusConexaoBolsaStreamer>();
        private bool _bKeepRunning = false;
        private Thread _thOrdemProcessor = null;
        private Thread _thStatusProcessor = null;
        private DateTime _lastStatus = DateTime.MinValue;
        private IAssinaturasRoteadorOrdensCallback roteador = null;

        private SocketPackage sckServer = null;

        private A4SConfig _config = null;

        #region IServicoControlavel
        /// <summary>
        /// 
        /// </summary>
        public void IniciarServico()
        {
            logger.Info("Iniciando ServicoA4S");

            _config = GerenciadorConfig.ReceberConfig<A4SConfig>();

            _bKeepRunning = true;

            sckServer = new SocketPackage();

            sckServer.OnClientConnected += new ClientConnectedHandler(sckServer_OnClientConnected);
            sckServer.OnRequestReceived += new MessageReceivedHandler(sckServer_OnMessageReceived);


            _thOrdemProcessor = new Thread(new ThreadStart(OrdemProcessor));
            _thOrdemProcessor.Start();

            _thStatusProcessor = new Thread(new ThreadStart(StatusProcessor));
            _thStatusProcessor.Start();


            sckServer.StartListen(_config.ListenPort);

            _servicoStatus = ServicoStatus.EmExecucao;

            logger.Info("ServicoA4S Iniciado");
 

        }

        /// <summary>
        /// 
        /// </summary>
        public void PararServico()
        {
            logger.Info("Finalizando ServicoA4S");

            _bKeepRunning = false;

            while (_thOrdemProcessor != null && _thOrdemProcessor.IsAlive)
                Thread.Sleep(250);

            while (_thStatusProcessor != null && _thStatusProcessor.IsAlive)
                Thread.Sleep(250);

            _servicoStatus = ServicoStatus.Parado;

            logger.Info("ServicoA4S Finalizado");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServicoStatus ReceberStatusServico()
        {
            return _servicoStatus;
        }

        #endregion // IServicoControlavel

        #region IRoteadorOrdensCallback
        /// <summary>
        /// Event handler de acompanhamentos de ordens
        /// </summary>
        /// <param name="report"></param>
        public void OrdemAlterada(RoteadorOrdens.Lib.Dados.OrdemInfo report)
        {
            try
            {
                OrdemInfoStreamer ordem = new OrdemInfoStreamer(report);

                lock (queueOrdens)
                {
                    queueOrdens.Enqueue(ordem);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OrdemAlterada():" + ex.Message, ex);
            }
        }


        /// <summary>
        /// EventHandler de status de conexoes com as bolsas
        /// </summary>
        /// <param name="status"></param>
        public void StatusConexaoAlterada(RoteadorOrdens.Lib.Dados.StatusConexaoBolsaInfo status)
        {
            try
            {
                _lastStatus = DateTime.Now;

                StatusConexaoBolsaStreamer info = new StatusConexaoBolsaStreamer(status);

                lock (queueStatus)
                {
                    queueStatus.Enqueue(info);
                }
            }
            catch (Exception ex)
            {
                logger.Error("StatusConexaoAlterada():" + ex.Message, ex);
            }
        }
        #endregion //IRoteadorOrdensCallback


        /// <summary>
        /// Thread de processamento da fila de acompanhamento de ordens
        /// </summary>
        private void OrdemProcessor()
        {
            try
            {
                while (_bKeepRunning)
                {
                    lock (queueOrdens)
                    {
                        while (queueOrdens.Count > 0)
                        {
                            OrdemInfoStreamer ordem = queueOrdens.Dequeue();

                            logger.Debug("OrdemInfo: [" + ordem.ToMsg() + "]");

                            sckServer.SendToAll(ordem.ToMsg());
                        }
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OrdemProcessor: " + ex.Message, ex);
            }

        }


        /// <summary>
        /// Thread de processamento da fila de status de conexao com as bolsas
        /// </summary>
        private void StatusProcessor()
        {
            try
            {
                while (_bKeepRunning)
                {
                    TimeSpan lastInterval = new TimeSpan(DateTime.Now.Ticks - _lastStatus.Ticks);

                    if (lastInterval.TotalSeconds > 60)
                    {
                        UnsubscribeRoteador();

                        SubscribeRoteador();
                    }


                    lock (queueStatus)
                    {
                        while (queueStatus.Count > 0)
                        {
                            StatusConexaoBolsaStreamer status = queueStatus.Dequeue();

                            sckServer.SendToAll(status.ToMsg());
                        }
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                logger.Error("StatusProcessor: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Event Handler do evento de conexao de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void sckServer_OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            try
            {
                logger.Info( "Recebeu conexao do MDS: " + args.ClientNumber );

            }
            catch (Exception ex)
            {
                logger.Error("OnClientConnected: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Event Handler para evento de recebimento de mensagens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private  void sckServer_OnMessageReceived(object sender, MessageEventArgs args)
        {
            try
            {
            }
            catch (Exception ex)
            {
                logger.Error("OnMessageReceived: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Assina os eventos de acompanhamento de ordens e status das bolsas
        /// </summary>
        private void SubscribeRoteador()
        {
            try
            {
                logger.Info("SubscribeRoteador(): assinando eventos de acompanhamento e status");

                if (roteador == null)
                {
                    roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);

                    if (roteador != null)
                    {
                        AssinarExecucaoOrdemResponse resp = roteador.AssinarExecucaoOrdens(new AssinarExecucaoOrdemRequest());

                        AssinarStatusConexaoBolsaResponse resp1 = roteador.AssinarStatusConexaoBolsa(new AssinarStatusConexaoBolsaRequest());

                        _lastStatus = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("SubscribeRoteador: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Cancela a assinatura dos callbacks de ordem e status do roteador
        /// </summary>
        private void UnsubscribeRoteador()
        {
            try
            {
                if (roteador != null)
                {
                    logger.Info("UnsubscribeRoteador(): desassinando eventos de acompanhamento e status");

                    Ativador.AbortChannel(roteador);

                    roteador = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("UnsubscribeRoteador: " + ex.Message, ex);
            }

        }
    }
}
