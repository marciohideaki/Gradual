using System.Collections.Generic;
using System.ServiceModel;
using Cortex.OMS.ServidorFIXAdm.Lib.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using log4net;
using QuickFix;
using Cortex.OMS.ServidorFIXAdm.Lib;
using Cortex.OMS.ServidorFIXAdm.Lib.Mensagens;
using System;

namespace Cortex.OMS.ServidorFIX
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServidorFix : IServicoControlavel, IServidorFixAdmin
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _status = ServicoStatus.Parado;

        private QuickFix.SessionID _session;
        //private QuickFix.IAcceptor
        protected ThreadedSocketAcceptor _socketAcceptor;
        private ServidorFixConfig _config;
        private Executor _executorApp = null;
        private CronStyleScheduler _cron = null;


        public void IniciarServico()
        {
            logger.Info("IniciarServico(): iniciando Servidor FIX...");

            // Carrega configurações
            _config = GerenciadorConfig.ReceberConfig<ServidorFixConfig>();

            // Cria dicionario da configuracao 
            Dictionary mainDic = new Dictionary();

            if (_config.SocketAcceptPort > 0)
                mainDic.SetLong("SocketAcceptPort", _config.SocketAcceptPort);
            //mainDic.SetLong("SocketConnectPort", _config.SocketConnectPort);
            mainDic.SetLong("HeartBtInt", _config.HeartBtInt);
            mainDic.SetLong("ReconnectInterval", _config.ReconnectInterval);

            //mainDic.SetBool("ResetOnLogon", _config.ResetSeqNum);
            //mainDic.SetBool("PersistMessages", _config.PersistMessages);

            // Ver
            // ret.setString("ConnectionType", ConnectionType.ToLower());
            //mainDic.SetString("SocketConnectHost", _config.Host);
            mainDic.SetString("FileStorePath", _config.FileStorePath);

            logger.Debug("FileLogPath: " + _config.FileLogPath);
            mainDic.SetString("FileLogPath", _config.FileLogPath);

            logger.Debug("DebugFileLogPath: " + _config.FileLogPath);
            mainDic.SetString("DebugFileLogPath", _config.DebugFileLogPath);
            mainDic.SetString("StartTime", _config.StartTime);
            mainDic.SetString("EndTime", _config.EndTime);
            mainDic.SetString("ConnectionType", "acceptor");

            CamadaDeDados db = new CamadaDeDados();
            List<SessionFixInfo> sessions = db.BuscarSessoesFIX();
            
            // Configure the session settings
            SessionSettings settings = new SessionSettings();

            settings.Set(mainDic);


            foreach (SessionFixInfo info in sessions)
            {
                Dictionary sessDic = new Dictionary();

                //sessDic.SetString("BeginString", _config.BeginString);
                //sessDic.SetString("SenderCompID", _config.SenderCompID);
                //sessDic.SetString("TargetCompID", _config.TargetCompID);

                string dataDictionary;

                switch (info.FixVersion)
                {
                    case "5.0": dataDictionary = _config.DataDictionary50; break;
                    case "4.4": dataDictionary = _config.DataDictionary44; break;
                    case "4.2":
                    default:
                        dataDictionary = _config.DataDictionary42; break;
                }

                sessDic.SetString("DataDictionary", dataDictionary);
                sessDic.SetBool("UseDataDictionary", true);
                sessDic.SetBool("ResetOnLogon", info.ResetSeqNum!=0?true:false);
                sessDic.SetBool("PersistMessages", info.PersistMessages != 0 ? true : false);

                logger.InfoFormat("Criando sessao S:[{0}] T:[{1}] Ver:[{2}] Dic:[{3}] Rst:[{4}] Pers:[{5}] Begstr:[{6}]", 
                    info.SenderCompID,
                    info.TargetCompID,
                    info.FixVersion,
                    dataDictionary,
                    info.ResetSeqNum,
                    info.PersistMessages,
                    info.BeginString
                    );

                // Cria sessao que será usada para mandar as mensagens
                _session =
                    new SessionID(info.BeginString,
                        info.SenderCompID,
                        info.TargetCompID);

                settings.Set(_session, sessDic);
            }

            logger.Info("Iniciando gerenciador de limites...");
            LimiteManager.GetInstance().Start();

            logger.Info("Iniciando cron scheduler...");
            _cron = new CronStyleScheduler();
            _cron.Start();

            logger.Info("Iniciando Executor FIX...");
            _executorApp = Executor.GetInstance();
            _executorApp.Start();

            FileStoreFactory store = new FileStoreFactory(settings);

            LogFactory logs = new FileLogFactory(settings);
            IMessageFactory msgs = new DefaultMessageFactory();

            logger.Info("Iniciando ThreadedSocketAcceptor...");
            _socketAcceptor = new ThreadedSocketAcceptor(_executorApp, store, settings, logs, msgs);
            _socketAcceptor.Start();

            _status = ServicoStatus.EmExecucao;

            logger.Info("IniciarServico(): ServidorFIX em execucao....");
        }



        public void PararServico()
        {

            logger.Info("Parando ServidorFIX");

            logger.Info("Finalizando fix acceptor...");
            _socketAcceptor.Stop();

            logger.Info("Finalizando executor.....");
            _executorApp.Stop();

            logger.Info("Finalizando LimiteManager....");
            LimiteManager.GetInstance().Stop();

            logger.Info("*** ServidorFIX finalizado");
        }



        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        public ReloadConfigResponse ReloadConfiguration(ReloadConfigRequest request)
        {
            ReloadConfigResponse resp = new ReloadConfigResponse();

            try
            {
                resp.StatusResponse = "OK";
                LimiteManager.GetInstance().CarregarLimites(LimiteManager.GetInstance().AccountLimit);
            }
            catch (Exception ex)
            {
                resp.StatusResponse = "ERRO";
                resp.StackTrace = ex.StackTrace;
                resp.DescricaoErro = ex.Message;
            }

            return resp;
        }
    }
}
