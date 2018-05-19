using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Gradual.OMS.Library.Servicos;
using log4net;
using System.Configuration;
using Gradual.BackOffice.BrokerageProcessor.Watcher;

using Gradual.BackOffice.BrokerageProcessor.Lib.FileWatcher;
using Gradual.OMS.Library;
using Gradual.BackOffice.BrokerageProcessor.Email;
using System.Threading;
using Gradual.BackOffice.BrokerageProcessor.FileManager;

namespace Gradual.BackOffice.BrokerageProcessor
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BrokerageProcessorSrv: IServicoControlavel
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private variables
        string _pathBrokerage = string.Empty;
        string _pathBackup = string.Empty;
        string _pathProcessed = string.Empty;
        string _filterWatch = string.Empty;
        #endregion

        ServicoStatus _status;

        FileWatcherConfig _config;
        List<FileWatcherManager> _lstFileManager;
        CronStyleScheduler scheduler = new CronStyleScheduler();


        public void IniciarServico()
        {
            try
            {
                logger.Info("**********************************************************************");
                logger.Info("**********************************************************************");
                logger.Info("BrokerageProcessor v." + typeof(BrokerageProcessorSrv).Assembly.GetName().Version);
                logger.Info("*** IniciarServico(): BrokerageProcessor....");
                _config = GerenciadorConfig.ReceberConfig<FileWatcherConfig>();
                logger.Info("Config Count: " + _config.Watchers.Count);

                _lstFileManager = new List<FileWatcherManager>();
                foreach (FileWatcherConfigItem aux in _config.Watchers)
                {
                    FileWatcherManager item = new FileWatcherManager();
                    item.Config = aux;
                    item.CreatePaths();
                    item.Start();
                    _lstFileManager.Add(item);
                }
                
                // Iniciando PdfManager
                PdfManager.Instance.Start();

                // Iniciando TxtManager
                TxtManager.Instance.Start();

                // Iniciando EmailManager
                EmailManager.Instance.Start();

                //Iniciando cron scheduler
                scheduler.Start();

                _status = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do servico: " + ex.Message, ex);
                _status = ServicoStatus.Erro;
                throw ex;
            }
        }

        public void PararServico()
        {
            try
            {
                logger.Info("**********************************************************************");
                logger.Info("**********************************************************************");
                logger.Info("*** PararServico(): BrokerageProcessor....");

                //Finalizando cron scheduler
                scheduler.Stop();


                for (int i =0; i< _lstFileManager.Count; i++)
                {
                    _lstFileManager[i].Stop();
                    _lstFileManager[i] = null;
                }
                _lstFileManager.Clear();
                _lstFileManager = null;
                
                // PdfManager Stop 
                PdfManager.Instance.Stop();

                // TxtManager Stop
                TxtManager.Instance.Stop();

                // EmailManager Stop
                EmailManager.Instance.Stop();

                _status = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop do servico: " + ex.Message, ex);
                _status = ServicoStatus.Erro;
            }
        }


        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }


    }
}
