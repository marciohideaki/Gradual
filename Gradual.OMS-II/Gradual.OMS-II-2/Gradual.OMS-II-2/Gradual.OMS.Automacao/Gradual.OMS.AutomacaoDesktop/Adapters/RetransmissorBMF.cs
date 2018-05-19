using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using QuickFix44;
using com.espertech.esper.client;
using com.espertech.esper.compat.collections;
using System.IO;
using QuickFix;
using Gradual.OMS.AutomacaoDesktop.Adapters;
using System.Threading;

namespace Gradual.OMS.AutomacaoDesktop.Adapters
{
    public class RetransmissorBMF
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DadosGlobais dadosGlobais;
	    private AutomacaoConfig parametros;
	    private EPServiceProvider epService = null;
	    private LinkedBlockingQueue<string> filaFeeder;
	    private LinkedBlockingQueue<string> filaMensagensRetransmissorBMF;
	    private LinkedBlockingQueue<MarketDataSnapshotFullRefresh> filaMensagensFIXInstantaneo;
	    private LinkedBlockingQueue<MarketDataIncrementalRefresh> filaMensagensFIXIncremental;
        private FixConfig _config;
        private Thread _me = null;
        private GeradorEventosBMFRetransmissor geradorBMF;
        private SessaoFIXInstantaneo fixInstantaneo;
        private SessaoFIXIncremental fixIncremental;

	    public RetransmissorBMF(DadosGlobais dadosGlobais)
	    {
		    this.dadosGlobais = dadosGlobais;
		    this.epService = dadosGlobais.EpService;
		    this.parametros = dadosGlobais.Parametros;
		    this.filaFeeder = dadosGlobais.getFilaFeeder();
            this._config = parametros.BMFMarketDataConfig;

		    filaMensagensRetransmissorBMF = new LinkedBlockingQueue<string>();
		    filaMensagensFIXInstantaneo = new LinkedBlockingQueue<MarketDataSnapshotFullRefresh>();
		    filaMensagensFIXIncremental = new LinkedBlockingQueue<MarketDataIncrementalRefresh>();
		    return;
	    }


        public void Start()
        {
            geradorBMF = new GeradorEventosBMFRetransmissor(epService, filaMensagensRetransmissorBMF);
            geradorBMF.Start();

            fixInstantaneo = new SessaoFIXInstantaneo(filaMensagensFIXInstantaneo, filaMensagensRetransmissorBMF);
            fixInstantaneo.Start();

            fixIncremental = new SessaoFIXIncremental(filaMensagensFIXIncremental, filaMensagensRetransmissorBMF);
            fixIncremental.Start();

            _me = new Thread(new ThreadStart(Run));
            _me.Start();
        }

        public void Stop()
        {
            fixIncremental.Stop();
            fixInstantaneo.Stop();

            geradorBMF.Stop();
            if (_me != null)
            {
                while (_me.IsAlive)
                    Thread.Sleep(250);
            }
        }
	
	    public void Run()
	    {
            try
            {
                logger.Info("*** Iniciado Retransmissor BMF!");



                // Cria sessao que será usada para mandar as mensagens
                SessionID _session =
                    new SessionID(
                        new BeginString(_config.BeginString),
                        new SenderCompID(_config.SenderCompID),
                        new TargetCompID(_config.TargetCompID));

                // Cria dicionario da configuracao 
                Dictionary mainDic = new Dictionary();

                mainDic.setLong("SocketConnectPort", _config.SocketConnectPort);
                mainDic.setLong("HeartBtInt", _config.HeartBtInt);
                mainDic.setLong("ReconnectInterval", _config.ReconnectInterval);

                mainDic.setBool("ResetOnLogon", _config.ResetSeqNum);
                mainDic.setBool("PersistMessages", _config.PersistMessages);

                // Ver
                // ret.setString("ConnectionType", ConnectionType.ToLower());
                mainDic.setString("SocketConnectHost", _config.Host);
                mainDic.setString("FileStorePath", _config.FileStorePath);

                logger.Debug("FileLogPath: " + _config.FileLogPath);

                mainDic.setString("FileLogPath", _config.FileLogPath);
                mainDic.setString("StartTime", _config.StartTime);
                mainDic.setString("EndTime", _config.EndTime);
                mainDic.setString("ConnectionType", "initiator");

                Dictionary sessDic = new Dictionary();

                sessDic.setString("BeginString", _config.BeginString);
                sessDic.setString("SenderCompID", _config.SenderCompID);
                sessDic.setString("TargetCompID", _config.TargetCompID);
                sessDic.setString("DataDictionary", _config.DataDictionary);
                sessDic.setBool("UseDataDictionary", true);

                if (_config.RawData != null && _config.RawData.Length > 0)
                    sessDic.setString(SessaoFIX.FIX_RAWDATA, _config.RawData);

                if (_config.NewPassword != null && _config.NewPassword.Length > 0)
                    sessDic.setString(SessaoFIX.FIX_NEWPASSWORD, _config.NewPassword);

                if (_config.FiltroListaInstrumentos != null && _config.FiltroListaInstrumentos.Length > 0)
                    sessDic.setString(SessaoFIX.FIX_FILTRO_LISTA_INSTRUMENTOS, _config.FiltroListaInstrumentos);

                if (_config.MdReqID != null && _config.MdReqID.Length > 0)
                    sessDic.setString(SessaoFIX.FIX_MDREQID_PADRAO, _config.MdReqID);

                // Configure the session settings
                SessionSettings sessionSettings = new SessionSettings();

                sessionSettings.set(mainDic);
                sessionSettings.set(_session, sessDic);

			    //MemoryStoreFactory fileStore = new MemoryStoreFactory();
                FileStoreFactory fileStore = new FileStoreFactory(sessionSettings);
                FileLogFactory fileLog = new FileLogFactory(sessionSettings);
                QuickFix44.MessageFactory message = new QuickFix44.MessageFactory();


			    SessaoFIX sessaoFIX = new SessaoFIX(
					    sessionSettings, 
					    dadosGlobais,
					    filaMensagensFIXInstantaneo,
					    filaMensagensFIXIncremental,
					    filaMensagensRetransmissorBMF);

			    while ( this.dadosGlobais.KeepRunning )
			    {
				    Initiator sessao = 
					    new ThreadedSocketInitiator(
							    sessaoFIX, fileStore, sessionSettings, fileLog, message);

				    try 
				    {
					    logger.Info("Iniciando Sessao FIX...");

					    sessao.start();

					    string mensagem = filaFeeder.Pop();
					    if ( mensagem.Contains(ConstantesMDS.TIPOMSG_FIM_CONEXAO) )
                            break ;

					    logger.Info("Finalizando Sessao FIX...");
					    sessao.stop();
				    } 

				    catch (Exception ex) 
				    {
					    logger.Error("Falha ao enviar mensagem na fila do Retransmissor BMF: " +
							    ex.Message,ex);
					    continue;
				    }
			    }
		    }
		    catch (ConfigError e) 
		    {
			    logger.Error("Falha durante configuracao do QuickFix:" + e.Message);
			    return;
		    }
            catch (Exception ex)
            {
                logger.Error("Run(): " + ex.Message, ex);
                return;
            }
        }
    }
}
