using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;
using Gradual.MDS.Core.Lib;
using System.Threading;
using Gradual.MDS.Core.Sinal;
using Gradual.MDS.Adm.Lib;
using System.Configuration;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Gradual.MDS.Core
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoMDS : IServicoControlavel, IServicoMdsAdm
    {
        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ServicoStatus _serviceStatus = ServicoStatus.Parado;
        private ServidorConexaoStreamer servidorStreamer = null;
        private ServidorConexaoHB servidorCotacao = null;
        private ServidorConexaoANG servidorANG = null;
        private Dictionary<string, ChannelUDMF> _dctCanais = new Dictionary<string, ChannelUDMF>();
        private Dictionary<string, ChannelTcpConflated> _dctConflated = new Dictionary<string, ChannelTcpConflated>();
        private UMDFConfig _umdfconfig = null;
        private MonitorConfig _monitorConfig;
        private TCPConflatedConfig _conflatedConfig;


        /// <summary>
        /// 
        /// </summary>
        public void IniciarServico()
        {
            try
            {
                _monitorConfig = new MonitorConfig();

                logger.Info("Iniciando servidor MDS");

                logger.Info("Inicializando NEsper");

                //NesperManager.Instance.Configure();

                logger.Info("Inicializando gerenciador de filas de eventos");
                EventQueueManager.Instance.Start();

                logger.Info("Carregando configuracao dos canais UMDF");
                _umdfconfig = GerenciadorConfig.ReceberConfig<UMDFConfig>();

                logger.Info("Criando instancias dos canais UMDF");

                foreach (ChannelUMDFConfig cfgcanal in _umdfconfig.CanaisUMDF)
                {
                    cfgcanal.Segment = cfgcanal.Segment.ToUpperInvariant();
                    cfgcanal.TCPConfig = _umdfconfig.TCPConfig;

                    ChannelUDMF canal = new ChannelUDMF(cfgcanal, _monitorConfig);
                    canal.startup = cfgcanal.Startup;

                    logger.Info("Configurado ChannelID[" + cfgcanal.ChannelID +
                        "]: SecurityList[" + cfgcanal.SecurityListHost + ":" + cfgcanal.SecurityListPorta +
                        "] Snapshot[" + cfgcanal.MDRecoveryHost + ":" + cfgcanal.MDRecoveryPorta +
                        "] Incremental[" + cfgcanal.MDIncrementalHost + ":" + cfgcanal.MDIncrementalPorta + "]");

                    _dctCanais.Add(cfgcanal.ChannelID, canal);
                    _monitorConfig.AddChannel(cfgcanal.ChannelID, cfgcanal.Startup);
                }


                logger.Info("Iniciando canais TCPConflated");
                _conflatedConfig = GerenciadorConfig.ReceberConfig<TCPConflatedConfig>();

                if (_conflatedConfig != null)
                {
                    ChannelTcpConflated tcpConflated = new ChannelTcpConflated(_conflatedConfig);

                    _dctConflated.Add(_conflatedConfig.SenderCompID, tcpConflated);
                }

                if (ConfigurationManager.AppSettings["AnaliseGraficaListenPort"] != null)
                {
                    string portaANG = ConfigurationManager.AppSettings["AnaliseGraficaListenPort"].ToString();

                    logger.Info("Iniciando servidor sinal AnaliseGrafica na porta [" + portaANG + "]");

                    servidorANG = new ServidorConexaoANG();
                    servidorANG.ListenPortNumber = Int32.Parse(portaANG);
                    servidorANG.Start();
                }

                logger.Info("Iniciando threads de tratamento dos sinais");

                ContainerManager.Instance.Start(_dctCanais);

                logger.Info("Iniciando threads dos Canais UMDF");

                foreach (ChannelUDMF canal in _dctCanais.Values)
                {
                    if (canal.startup)
                    {
                        canal.Start();
                        logger.Info("Ativado ChannelID[" + canal.channelConfig.ChannelID + "]");
                    }
                }


                Parallel.Invoke( ()=> {
                        Thread.Sleep(15000);
                        if (ConfigurationManager.AppSettings["StreamerListenPort"] != null)
                        {
                            string portaStreamer = ConfigurationManager.AppSettings["StreamerListenPort"].ToString();

                            logger.Info("Iniciando servidor Streaming na porta [" + portaStreamer + "]");

                            servidorStreamer = new ServidorConexaoStreamer();
                            servidorStreamer.ListenPortNumber = Int32.Parse(portaStreamer);
                            servidorStreamer.Start();
                        }
                    },
                    () => {
                        Thread.Sleep(15000);
                        if (ConfigurationManager.AppSettings["CotacaoListenPort"] != null)
                        {
                            string portaCotacao = ConfigurationManager.AppSettings["CotacaoListenPort"].ToString();

                            logger.Info("Iniciando servidor cotacoes HB na porta [" + portaCotacao + "]");

                            servidorCotacao = new ServidorConexaoHB();
                            servidorCotacao.ListenPortNumber = Int32.Parse(portaCotacao);
                            servidorCotacao.Start();
                        }
                    }
                );

                _serviceStatus = ServicoStatus.EmExecucao;

                logger.Info("Servidor MDS iniciado");
            }
            catch (Exception ex)
            {
                logger.Error("IniciarServico(): "+ ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PararServico()
        {
            try
            {

                logger.Info("Finalizando Servidor MDS");
                if (servidorCotacao != null)
                {
                    servidorCotacao.Stop();
                }

                if (servidorStreamer != null )
                {
                    servidorStreamer.Stop();
                }

                if (servidorANG != null)
                {
                    servidorANG.Stop();
                }

                foreach (ChannelUDMF canal in _dctCanais.Values)
                {
                    canal.Stop();
                }

                foreach( ChannelTcpConflated tcpConflated in _dctConflated.Values)
                {
                    tcpConflated.Stop();
                }


                ContainerManager.Instance.Stop();

                //NesperManager.Instance.epService.Dispose();

                _serviceStatus = ServicoStatus.Parado;

                logger.Info("Servidor MDS finalizado");
            }
            catch (Exception ex)
            {
                logger.Error("PararServico(): " + ex.Message, ex);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _serviceStatus;
        }


        #region IServicoMdsAdm Members
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UMDFConfig ObterUmdfConfig()
        {
            logger.Info("Enviando UMDFConfig para o Monitor");
            return _umdfconfig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MonitorConfig ObterDadosMonitor()
        {
            logger.Debug("Enviando MonitorConfig para o Monitor");
            logger.Debug("dateTimeStarted[" + _monitorConfig.dateTimeStarted + "]");
            Dictionary<string, MonitorConfigChannel>.Enumerator channels = _monitorConfig.channels.GetEnumerator();
            while (channels.MoveNext())
            {
                KeyValuePair<string, MonitorConfigChannel> channel = channels.Current;
                logger.Debug("ChannelID[" + channel.Key + "]");
                Dictionary<string, MonitorConfigDetails>.Enumerator details = channel.Value.configDetails.GetEnumerator();
                while (details.MoveNext())
                {
                    KeyValuePair<string, MonitorConfigDetails> detail = details.Current;
                    logger.Debug("Detail[" + detail.Key + "] Value[" + detail.Value.value + "] Detail[" + detail.Value.details + "]");
                }
            }
            return _monitorConfig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public bool AtivarChannel(string channelID)
        {
            if (!_dctCanais.ContainsKey(channelID))
            {
                logger.Error("ChannelID[" + channelID + "] não existente na configuração!");
                return false;
            }

            ChannelUDMF canal = _dctCanais[channelID];
            if (canal.isRunning)
            {
                logger.Error("ChannelID[" + channelID + "] já ativo!");
                return false;
            }

            canal.Start();
            logger.Info("Ativado ChannelID[" + channelID + "]");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public bool DesativarChannel(string channelID)
        {
            if (!_dctCanais.ContainsKey(channelID))
            {
                logger.Error("ChannelID[" + channelID + "] não existente na configuração!");
                return false;
            }

            ChannelUDMF canal = _dctCanais[channelID];
            if (!canal.isRunning)
            {
                logger.Error("ChannelID[" + channelID + "] já desativado!");
                return false;
            }

            canal.Stop();
            logger.Info("Desativado ChannelID[" + channelID + "]");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public bool ResumeChannelIncremental(string channelID)
        {
            if (!_dctCanais.ContainsKey(channelID))
            {
                logger.Error("ChannelID[" + channelID + "] não existente na configuração!");
                return false;
            }

            ChannelUDMF canal = _dctCanais[channelID];
            if (!canal.isPaused)
            {
                logger.Error("ChannelID[" + channelID + "] já ativo!");
                return false;
            }

            canal.ResumeIncremental();
            logger.Info("Retomando ChannelID[" + channelID + "]");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public bool PauseChannelIncremental(string channelID)
        {
            if (!_dctCanais.ContainsKey(channelID))
            {
                logger.Error("ChannelID[" + channelID + "] não existente na configuração!");
                return false;
            }

            ChannelUDMF canal = _dctCanais[channelID];
            if (canal.isPaused)
            {
                logger.Error("ChannelID[" + channelID + "] já pausado!");
                return false;
            }

            canal.PauseIncremental();
            logger.Info("Pausado ChannelID[" + channelID + "]");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public bool DoD330(string channelID)
        {
            /**
             * Todo o codigo abaixo eh apenas para atender ao roteiro de certificacao do puma 2.0
             * Item D3.30
             */
            if (!_dctCanais.ContainsKey(channelID))
            {
                logger.Error("ChannelID[" + channelID + "] não existente na configuração!");
                return false;
            }

            ChannelUDMF canal = _dctCanais[channelID];
            if (canal.isPaused)
            {
                logger.Error("ChannelID[" + channelID + "] já pausado!");
                return false;
            }

            canal.DoD330();
            logger.Info("Pausado ChannelID[" + channelID + "]");
            return true;
        }

        public bool RecoveryInterval(string channelID, int seqNumIni, int seqNumFin)
        {
            /**
            * Todo o codigo abaixo eh apenas para atender ao roteiro de certificacao do puma 2.0
            * Item D3.30
            */
            if (!_dctCanais.ContainsKey(channelID))
            {
                logger.Error("ChannelID[" + channelID + "] não existente na configuração!");
                return false;
            }

            ChannelUDMF canal = _dctCanais[channelID];
            if (canal.isPaused)
            {
                logger.Error("ChannelID[" + channelID + "] já pausado!");
                return false;
            }

            canal.RecoveryInterval( seqNumIni, seqNumFin);

            logger.Info("Pausado ChannelID[" + channelID + "]");
            return true;
        }


        // TCPConflatedOperations
        public bool AtivarChannelConflated(string senderCompID)
        {
            if ( _dctConflated.ContainsKey(senderCompID) )
            {
                _dctConflated[senderCompID].Start();
                return true;
            }

            return false;
        }

        public bool DesativarChannelConflated(string senderCompID)
        {
            if (_dctConflated.ContainsKey(senderCompID))
            {
                _dctConflated[senderCompID].Stop();
                return true;
            }

            return false;
        }

        public string EnviarAssinaturaSecurityListConflated(string senderCompID, string securityType, string product, string cfiCode)
        {

            if (_dctConflated.ContainsKey(senderCompID))
            {
                return _dctConflated[senderCompID].EnviarAssinaturaSecurityListConflated( securityType, product, cfiCode);
            }

            return "";
        }

        public bool CancelarAssinaturaSecurityListConflated(string senderCompID, string securityType, string product, string cfiCode, string securityReqID)
        {
            if (_dctConflated.ContainsKey(senderCompID))
            {
                _dctConflated[senderCompID].CancelarAssinaturaSecurityListConflated( securityType,  product, cfiCode, securityReqID);
                return true;
            }

            return false;
        }

        public string EnviarAssinaturaMarketDataConflated(string senderCompID, string instrumento, string securityType, string product, string cfiCode)
        {
            if (_dctConflated.ContainsKey(senderCompID))
            {
                return _dctConflated[senderCompID].EnviarAssinaturaMarketDataConflated( instrumento, securityType, product, cfiCode);
            }

            return "";
        }

        public bool CancelarAssinaturaMarketDataConflated(string senderCompID, string instrumento, string securityType, string product, string cfiCode, string mdReqID)
        {
            if (_dctConflated.ContainsKey(senderCompID))
            {
                _dctConflated[senderCompID].CancelarAssinaturaMarketDataConflated(instrumento, securityType, product, cfiCode, mdReqID);
                return true;
            }

            return false;
        }

        public bool EnviarResendRequestConflated(string senderCompID, int seqNoIni, int seqNoFim)
        {
            if (_dctConflated.ContainsKey(senderCompID))
            {
                return _dctConflated[senderCompID].EnviarResendRequestConflated( seqNoIni, seqNoFim);
            }

            return false;
        }

        public bool EnviarSequenceResetConflated(string senderCompID, int newSeqNo)
        {
            if (_dctConflated.ContainsKey(senderCompID))
            {
                return _dctConflated[senderCompID].EnviarSequenceResetConflated(newSeqNo);
            }

            return false;
        }

        #endregion
    }
}
