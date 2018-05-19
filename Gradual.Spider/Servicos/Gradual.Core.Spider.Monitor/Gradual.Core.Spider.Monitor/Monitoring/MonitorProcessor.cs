using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using log4net;
using Gradual.Core.Spider.Monitor.Db;
using Gradual.Core.Spider.Monitoring.Lib.Entities;
using Gradual.Core.Spider.Monitor.Teste;
using System.Collections.Concurrent;
using Gradual.Core.Spider.Monitoring.Lib;
using System.ServiceModel;
using Newtonsoft.Json;
using Gradual.Core.OMS.FixServerLowLatency.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Newtonsoft.Json.Converters;
using Gradual.Core.Spider.FixDCGateway.Lib;
using Gradual.Core.Spider.FixDCGateway.Lib.Mensageria;
using Gradual.Core.Spider.FixDCGateway.Lib.Dados;
using System.Reflection;
using Gradual.Core.Spider.Monitoring.Lib.Messages;

namespace Gradual.Core.Spider.Monitor.Monitoring
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MonitorProcessor : IMonitorSpider
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private variables
        DbMonitor _db = null;
        private static MonitorProcessor _me = null;
        private static object _objLock = new object();
        Dictionary<int, MonitorInfo> _dicMonitoring;
        Dictionary<string, ConfigInfo> _dicConfig;

        IFixServerLowLatencyAdm _itfFixSpiderAdm;
        IFixDCAdm _itfFixDCAdm;
        bool _isStarted;

        Thread _thMonitor;
        Thread _thDb;
        Thread _thActivator;

        ConcurrentQueue<MonitorInfo> _cqDb;
        #endregion
        public MonitorProcessor()
        {

        }

        public static MonitorProcessor GetInstance()
        {
            lock (_objLock)
            {
                if (_me == null)
                {
                    _me = new MonitorProcessor();
                }
            }
            return _me;
        }

        #region ServiceControls
        public void Start()
        {
            try
            {
                logger.Info("======> Iniciando Processador de monitoramento...");
                logger.Info("Alocando obj database");
                if (null == _db)
                    _db = new DbMonitor();
                // Alocando colecao de configuracoes
                _dicConfig = new Dictionary<string, ConfigInfo>();
                _dicConfig = _db.LoadMonitorConfig();
                if (null != _dicConfig)
                    logger.Info("No. Parametros de Configuracao: " + _dicConfig.Count);

                // Alocando colecao de sessoes fix
                _dicMonitoring = new Dictionary<int, MonitorInfo>();
                _dicMonitoring = _db.LoadMonitors();
                if (null != _dicMonitoring)
                    logger.Info("No. Monitoramentos: " + _dicMonitoring.Count);


                _isStarted = true; // Ativar a flag antes para iniciar as threads
                _cqDb = new ConcurrentQueue<MonitorInfo>();

                logger.Info("Iniciando thread de monitoramento...");
                _thMonitor = new Thread(new ThreadStart(_processMonitor));
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
                if (null != _dicMonitoring)
                {
                    _dicMonitoring.Clear();
                    _dicMonitoring = null;
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
        private void _processMonitor()
        {
            try
            {
                //int refresh = 30 * 1000;
                int i = 1;
                int times = 30;
                ConfigInfo item = this.GetConfig("REFRESH_INTERVAL");
                // int lenFix = _dicMonitoring.Count;
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

                        List<MonitorInfo> lst1 = null;
                        List<MonitorInfo> lst2 = null;
                        retorno = this._getMonitorInfo(_itfFixSpiderAdm, MonitorType.FIX_SERVER_LOW_LATENCY, out lst1);
                        retorno = this._getMonitorInfo(_itfFixDCAdm, MonitorType.FIX_DROP_COPY_GATEWAY, out lst2);

                        if (retorno)
                        {
                            if (lst1 != null)
                                this._processMonitorInfo(lst1);
                            if (lst2 != null)
                                this._processMonitorInfo(lst2);
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

        private void _addDbMessage(MonitorInfo item)
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
                    MonitorInfo item = null;
                    if (!_cqDb.TryDequeue(out item))
                    {
                        lock (_cqDb)
                            System.Threading.Monitor.Wait(_cqDb, 100);
                    }

                    if (null != item && null != _db)
                        _db.UpdateMonitorInfo(item);
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
            _itfFixSpiderAdm = (IFixServerLowLatencyAdm) this._activateInterface(_itfFixSpiderAdm, MonitorType.FIX_SERVER_LOW_LATENCY);
            _itfFixDCAdm = (IFixDCAdm) this._activateInterface(_itfFixDCAdm, MonitorType.FIX_DROP_COPY_GATEWAY);
            
            while (_isStarted)
            {
                try
                {
                    if (i >= 120)
                    {
                        _itfFixSpiderAdm = (IFixServerLowLatencyAdm)this._activateInterface(_itfFixSpiderAdm, 1);
                        _itfFixDCAdm = (IFixDCAdm)this._activateInterface(_itfFixDCAdm, 2);
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
        private object _activateInterface(object itf, int tipo)
        {
            try
            {
                MethodInfo mt = null;
                switch (tipo)
                {
                    // IFixServerLowLatencyAdm
                    case 1:
                        //if (itf ==null)
                        //    itf = Ativador.Get<IFixServerLowLatencyAdm>();
                        //mt = itf.GetType().GetMethod("Pong");
                        //mt.Invoke(itf, null);
                        break;
                    case 2:
                        if (itf == null)
                            itf = Ativador.Get<IFixDCAdm>();
                        mt = itf.GetType().GetMethod("DummyFunction");
                        mt.Invoke(itf, null);
                        break;
                }
            }
            catch
            {
                logger.ErrorFormat("Problemas na ativacao da interface: [{0}] ID: [{1}]", itf.GetType().ToString(), tipo);
                itf = null;
            }
            return itf;
        }

        private bool _getMonitorInfo(object itf, int tipo, out List<MonitorInfo> ret)
        {
            MonitorInfoResponse returnValue = null;
            ret = null;
            try
            {
                MethodInfo mt = null;
                switch (tipo)
                {
                    // IFixServerLowLatencyAdm
                    case 1:
                        //if (itf == null)
                        //    itf = Ativador.Get<IFixServerLowLatencyAdm>();
                        //mt = itf.GetType().GetMethod("GetMonitorInfo");
                        //returnValue = mt.Invoke(itf, null) as MonitorInfoResponse;
                        
                        break;
                    case 2:
                        if (itf == null)
                            itf = Ativador.Get<IFixDCAdm>();
                        mt = itf.GetType().GetMethod("GetMonitorInfo");
                        object[] parametersArray = new object[1];
                        parametersArray[0] = new MonitorInfoRequest();
                        returnValue =  mt.Invoke(itf, parametersArray) as MonitorInfoResponse;
                        break;
                }
            }
            catch (Exception ex)
            {
                // logger.Error(ex.Message, ex);
                logger.ErrorFormat("Problemas na consulta das informacoes de monitoramento: [{0}] ID: [{1}]", itf.GetType().ToString(), tipo);
                return false;
            }
            // Formatar retorno da funcao
            if (returnValue != null && returnValue.Infos!=null)
            {
                ret = new List<MonitorInfo>();
                ret.AddRange(returnValue.Infos);
            }
            return true;
        }

        private bool _processMonitorInfo(List<MonitorInfo> lst)
        {
            try
            {
                if (lst != null)
                {
                    int lenMonitor = _dicMonitoring.Count;

                    for (int j = 0; j < lenMonitor; j++)
                    {
                        KeyValuePair<int, MonitorInfo> aux = _dicMonitoring.ElementAt(j);
                        MonitorInfo monInfo = aux.Value;
                        MonitorInfo aux1 = lst.FirstOrDefault(x => x.KeyValue.Equals(monInfo.KeyValue));
                        if (null != aux1)
                        {
                            aux1.IdMonitor = monInfo.IdMonitor;
                            this._addDbMessage(aux1);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento das classes de Monitoramento: " + ex.Message, ex);
                return false;
            }
        }

        #endregion


        #region Generic Functions
        private ConfigInfo GetConfig(string chave)
        {

            // string aux = chave + "_" + MonitorType.FIX_SESSION;
            string aux = string.Empty;
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


        public string GetMonitorListJson()
        {
            string ret;
            lock (_me._dicMonitoring)
            {
                //List<FixSessionInfo> lst = _me._dicFixMonitoring.Values.ToList();
                List<MonitorInfo> lst = new List<MonitorInfo>();
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
