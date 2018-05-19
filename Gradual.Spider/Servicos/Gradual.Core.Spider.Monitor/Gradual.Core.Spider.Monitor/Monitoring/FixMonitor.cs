using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using log4net;
using Gradual.Core.Spider.Monitor.Db;
using Gradual.Core.Spider.Monitor.Lib.Entities;
using Gradual.Core.Spider.Monitor.Teste;
using System.Collections.Concurrent;
using Gradual.Core.Spider.Monitor.Lib;
using System.ServiceModel;
using Newtonsoft.Json;
using Gradual.Core.OMS.FixServerLowLatency.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Newtonsoft.Json.Converters;

namespace Gradual.Core.Spider.Monitor.Monitoring
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FixMonitor: IMonitorFix
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private variables
        DbMonitor _db = null;
        private static FixMonitor _me = null;
        private static object _objLock = new object();
        Dictionary<int, FixSessionInfo> _dicFixMonitoring;
        Dictionary<string, ConfigInfo> _dicConfig;

        IFixServerLowLatencyAdm _itfFixSpiderAdm;
        bool _isStarted;

        Thread _thMonitor;
        Thread _thDb;
        Thread _thActivator;

        ConcurrentQueue<FixSessionInfo> _cqDb;
        #endregion
        public FixMonitor()
        {

        }

        public static FixMonitor GetInstance()
        {
            lock (_objLock)
            {
                if (_me == null)
                {
                    _me = new FixMonitor();
                }
            }
            return _me;
        }

        #region ServiceControls
        public void Start()
        {
            try
            {
                logger.Info("======> Iniciando Monitor das sessoes fix... ");
                logger.Info("Alocando obj database");
                if (null == _db)
                    _db = new DbMonitor();
                // Alocando colecao de configuracoes
                _dicConfig = new Dictionary<string, ConfigInfo>();
                _dicConfig = _db.LoadMonitorConfig(MonitorType.FIX_SESSION);
                if (null!=_dicConfig)
                    logger.Info("No. Parametros de Configuracao: " + _dicConfig.Count);

                // Alocando colecao de sessoes fix
                _dicFixMonitoring = new Dictionary<int, FixSessionInfo>();
                _dicFixMonitoring = _db.LoadMonitorSessionFix(MonitorType.FIX_SESSION);
                if (null != _dicFixMonitoring)
                    logger.Info("No. Sessoes Fix Monitoramento: " + _dicFixMonitoring.Count);

                
                _isStarted = true; // Ativar a flag antes para iniciar as threads
                _cqDb = new ConcurrentQueue<FixSessionInfo>();

                logger.Info("Iniciando thread de monitoramento...");
                _thMonitor = new Thread(new ThreadStart(_processFixMonitor));
                _thMonitor.Start();

                logger.Info("Iniciando thread de database...");
                _thDb = new Thread(new ThreadStart(_processDbMessage));
                _thDb.Start();

                logger.Info("Iniciando thread Ativador...");
                _thActivator = new Thread(new ThreadStart(_activatorMonitor));
                _thActivator.Start();



            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do FixMonitor: " + ex.Message, ex);
                throw;
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("======> Parando Monitor das sessoes fix... ");
                _isStarted = false;

                if (_thMonitor.IsAlive)
                {
                    _thMonitor.Join(2000);
                    _thMonitor.Abort();
                    _thMonitor = null;
                }
                
                if (_thDb.IsAlive)
                {
                    _thDb.Join(2000);
                    _thDb.Abort();
                    _thDb = null;
                }

                if (_thActivator.IsAlive)
                {
                    _thActivator.Join(2000);
                    _thActivator.Abort();
                    _thActivator = null;
                }

                if (null != _db)
                    _db = null;

                _cqDb = null;

                logger.Info("Limpando configs...");
                if (null != _dicConfig)
                {
                    _dicConfig.Clear();
                    _dicConfig = null;
                }
                logger.Info("Limpando sessoes de monitoramento...");
                if (null != _dicFixMonitoring)
                {
                    _dicFixMonitoring.Clear();
                    _dicFixMonitoring = null;
                }
                logger.Info("Servico Parado!!");
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop do FixMonitor: " + ex.Message, ex);
                throw;
            }
        }

        public bool IsStarted()
        {
            return _isStarted;
        }
        #endregion



        #region ThreadControls / Functions
        private void _processFixMonitor()
        {
            try
            {
                //int refresh = 30 * 1000;
                int i = 1;
                int times = 30;
                ConfigInfo item = this.GetConfig("REFRESH_INTERVAL");
                int lenFix = _dicFixMonitoring.Count;
                if (null != item)
                {
                    times = Convert.ToInt32(item.Valor) * 4;
                    //refresh = times * 250 *4;
                }

                while (_isStarted)
                {
                    if (i >= times)
                    {
                        bool retorno = false;
                        FixMonitoringResponse resp = null;
                        try
                        {
                            resp = _itfFixSpiderAdm.GetFixMonitors(new FixMonitoringRequest());
                            retorno = true;
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Ativador indisponivel ou desconectado: " + ex.Message, ex);
                        }
                        if (retorno)
                        {
                            for (int j = 0; j < lenFix; j++)
                            {
                                KeyValuePair<int, FixSessionInfo> aux = _dicFixMonitoring.ElementAt(j);
                                FixSessionInfo fixInfo = aux.Value;
                                string ssid = fixInfo.ComposeSessionID();
                                FixMonitorInfo xpto = resp.Monitors.FirstOrDefault(x => x.SessionID == ssid);
                                if (null != xpto)
                                {
                                    fixInfo.ActiveUsers = xpto.ActiveUsers;
                                    fixInfo.LastMessage = xpto.LastMsg;
                                    fixInfo.OrderCount = xpto.OrderCount;
                                    fixInfo.TransactionCount = xpto.TransactionCount;
                                }
                                this._addDbMessage(fixInfo);
                                fixInfo = null;
                            }
                        }
                        i = 1;
                    }
                    else
                        i++;
                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento de busca do monitoramento Fix: " + ex.Message, ex);
            }
        }

        private void _addDbMessage(FixSessionInfo item)
        {
            try
            {
                _cqDb.Enqueue(item);
                lock (_cqDb)
                    System.Threading.Monitor.Pulse(_cqDb);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na insercao da mensagem na fila do DB: " + ex.Message, ex);
            }

        }

        private void _processDbMessage()
        {
            while (_isStarted)
            {
                try
                {
                    FixSessionInfo item = null;
                    if (!_cqDb.TryDequeue(out item))
                    {
                        lock (_cqDb)
                            System.Threading.Monitor.Wait(_cqDb, 100);
                    }

                    if (null != item && null != _db)
                        _db.UpdateMonitorFix(item);
                }
                catch (Exception ex)
                {
                    logger.Error("Problema ao adicionar ao atualizar o registro no banco de dados: " + ex.Message, ex);
                }
            }
        }

        private void _activatorMonitor()
        {
            int i = 1;
            logger.Info("Efetuando instanciacao do Ativador... ");
            try
            {
                _itfFixSpiderAdm = Ativador.Get<IFixServerLowLatencyAdm>();
                PongResponse resp = _itfFixSpiderAdm.Pong(new PongRequest());
            }
            catch (Exception ex)
            {
                logger.Error("Não foi possivel efetuar a instanciacao do ativador... " + ex.Message, ex);
                _itfFixSpiderAdm = null;
            }

            
            while (_isStarted)
            {
                try
                {
                    if (i >=120)
                    {
                        if (null ==_itfFixSpiderAdm)
                            _itfFixSpiderAdm = Ativador.Get<IFixServerLowLatencyAdm>();
                        PongResponse resp = _itfFixSpiderAdm.Pong(new PongRequest());
                        logger.Info("Pong Response: " +resp.DateResponse.ToString("dd/MM/yyyy HH:mm:ss,fff"));
                        i = 1;
                    }
                    else
                        i++;
                    
                    Thread.Sleep(250);
                }
                catch (Exception ex)
                {
                    logger.Error("Erro no ping do Ativador, teoricamente conexao perdida: " + ex.Message, ex);
                    _itfFixSpiderAdm = null;
                    i = 1;
                }
            }
        }

        #endregion


        #region Generic Functions
        private ConfigInfo GetConfig(string chave)
        {

            string aux = chave + "_" + MonitorType.FIX_SESSION;
            ConfigInfo ret = null;

            if (_dicConfig.TryGetValue(aux, out ret))
                return ret;
            return null;
        }

        #endregion


        #region IMonitorFix Members

        public void DummyFunction()
        {
            logger.Info("Chamando IMonitorFix. DummyFunction");
        }


        public string GetFixMonitorListJson()
        {
            string ret;
            lock (_me._dicFixMonitoring)
            {
                List<FixSessionInfo> lst = _me._dicFixMonitoring.Values.ToList();
                IsoDateTimeConverter dtConvert = new IsoDateTimeConverter();
                dtConvert.DateTimeFormat = "dd/MM/yyyy HH:mm:ss.fff";
                ret = JsonConvert.SerializeObject(lst, dtConvert);
                dtConvert = null;
            }
            return ret;
        }
        #endregion
    }
}
