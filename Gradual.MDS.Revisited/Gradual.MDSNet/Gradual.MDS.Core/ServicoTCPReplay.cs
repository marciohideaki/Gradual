using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.ServiceModel;
using System.Net.Sockets;
using Gradual.MDS.Core.Lib;
using Gradual.MDS.Adm.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;
using QuickFix;

namespace Gradual.MDS.Core
{
    // Estrutura das sessoes FIX configuradas
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoTCPReplay : IServicoControlavel, IServicoTCPReplayAdm
    {
        public const string QUICKFIX_CONNECTION_TYPE_INITIATOR = "initiator";
        public const string QUICKFIX_CONNECTION_TYPE_ACCEPTOR = "acceptor";

        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ServicoStatus _serviceStatus = ServicoStatus.Parado;
        private FixServerAcceptor fixServerAcceptor;
        private Dictionary<string, FixServerInitiator> _dctFixServerInitiator = new Dictionary<string, FixServerInitiator>();
        private Dictionary<string, SessionID> _dctSessionsFixClients = new Dictionary<string, SessionID>();
        private Dictionary<string, SessionID> _dctSessionsFixChannels = new Dictionary<string, SessionID>();
        private UMDFConfig _umdfconfig = null;
        private DateTime _dataHoraStartServico;
        private MonitorConfig _monitorConfig;
        //private ServidorConexaoTCPReplay servidorTcpReplay = null;

        public void IniciarServico()
        {
            try
            {
                _monitorConfig = new MonitorConfig();

                logger.Info("*** Iniciando Servico TCPReplay");

                logger.Info("Carregando configuracao dos canais");
                _umdfconfig = GerenciadorConfig.ReceberConfig<UMDFConfig>();

                // Carrega configuracao e ativa as threads das sessoes FIX
                foreach (TCPReplayConfig tcpReplayConfig in _umdfconfig.TCPReplay)
                {
                    if (tcpReplayConfig.ConnectionType.Equals(QUICKFIX_CONNECTION_TYPE_ACCEPTOR))
                    {
                        logger.Info("Iniciando ACCEPTOR SenderCompID[" + tcpReplayConfig.SenderCompID +
                            "] TargetCompID[" + tcpReplayConfig.TargetCompID +
                            "] Aguardando na porta [" + tcpReplayConfig.SocketAcceptPort + "]");

                        tcpReplayConfig.ChannelID = QUICKFIX_CONNECTION_TYPE_ACCEPTOR.ToUpper();
                        fixServerAcceptor = new FixServerAcceptor(
                            tcpReplayConfig, _dctSessionsFixClients, _dctSessionsFixChannels);
                    }
                    else
                    {
                        string channelsID = tcpReplayConfig.ChannelID;

                        if (tcpReplayConfig.IsPuma)
                            tcpReplayConfig.ChannelID = ConstantesUMDF.UMDF_CHANNEL_ID_PUMA_1_6;
                        else if (tcpReplayConfig.IsPuma20)
                            tcpReplayConfig.ChannelID = ConstantesUMDF.UMDF_CHANNEL_ID_PUMA_2_0;

                        logger.Info("Iniciando thread ChannelID[" + tcpReplayConfig.ChannelID +
                            "]: SenderCompID[" + tcpReplayConfig.SenderCompID +
                            "] TargetCompID[" + tcpReplayConfig.TargetCompID +
                            "] Host[" + tcpReplayConfig.SocketConnectHost +
                            "] Port[" + tcpReplayConfig.SocketConnectPort +
                            "] TemplateFile[" + tcpReplayConfig.TemplateFile + "]");

                        FixServerInitiator fixServerInitiator = new FixServerInitiator(
                            channelsID, tcpReplayConfig, _dctSessionsFixClients, _dctSessionsFixChannels);

                        _dctFixServerInitiator.Add(tcpReplayConfig.ChannelID, fixServerInitiator);
                    }
                }

                _serviceStatus = ServicoStatus.EmExecucao;

                logger.Info("Servico TcpReplay iniciado");
            }
            catch (Exception ex)
            {
                logger.Error("IniciarServico(): " + ex.Message, ex);
            }
        }

        public void PararServico()
        {
            try
            {
                logger.Info("Finalizando threads TcpReplay");
                Dictionary<string, FixServerInitiator>.Enumerator itens = _dctFixServerInitiator.GetEnumerator();
                while (itens.MoveNext())
                {
                    KeyValuePair<string, FixServerInitiator> item = itens.Current;
                    item.Value.Stop();
                    logger.Info("Thread FixInitiator ChannelID[" + item.Key + "] finalizado");
                }
                fixServerAcceptor.Stop();
                logger.Info("Thread FixAcceptor finalizado");

                _serviceStatus = ServicoStatus.Parado;

                logger.Info("Threads TCPReplay finalizados");
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
        public UMDFConfig ObterUmdfConfig()
        {
            logger.Info("Enviando UMDFConfig para o Monitor");
            return _umdfconfig;
        }

        public MonitorConfig ObterDadosMonitor()
        {
            /*
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
            */
            return _monitorConfig;
        }

        public bool AtivarChannel(string channelID)
        {
            /*
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
            */
            return true;
        }

        public bool DesativarChannel(string channelID)
        {
            /*
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
            */
            return true;
        }
        #endregion
    }
}
