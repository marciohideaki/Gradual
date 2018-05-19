using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;


using Gradual.OMS.Library.Servicos;
using Gradual.Core.Spider.Monitoring.Lib;
using log4net;
using Gradual.Core.Spider.Monitor.Monitoring;


namespace Gradual.Core.Spider.Monitor
{
    
    //[ServiceContract(Namespace = "http://gradual")]
    public class MonitorServer : IServicoControlavel
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        ServicoStatus _status;

        // MonitorProcessor _monitorProcessor;
        #endregion

        public MonitorServer()
        {
            _status = ServicoStatus.Parado;

            // _monitorProcessor = null;
        }


        #region IServicoControlavel Members

        /// <summary>
        /// 
        /// </summary>
        public void IniciarServico()
        {
            try
            {
                logger.Info("=============================================================================");
                logger.Info("Iniciando o servico Spider - Monitor...");
                logger.Info("Iniciando Monitor Processor");
                //_fixMonitor = new FixMonitor();
                MonitorProcessor.GetInstance().Start();
                
                _status = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao iniciar o servico: " + ex.Message, ex);
                _status = ServicoStatus.Erro;
            }
        }

        public void PararServico()
        {
            try
            {
                logger.Info("Parando o servico Spider - Monitor...");
                logger.Info("Parando Monitor Processor...");
                if (MonitorProcessor.GetInstance().IsStarted())
                    MonitorProcessor.GetInstance().Stop();
                _status = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao parar o servico: " + ex.Message, ex);
                _status = ServicoStatus.Erro;
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        #endregion
    }
}
