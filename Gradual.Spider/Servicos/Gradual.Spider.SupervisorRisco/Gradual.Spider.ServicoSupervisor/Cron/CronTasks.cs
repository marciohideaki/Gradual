using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Net.Sockets;
using Gradual.Spider.ServicoSupervisor.Memory;

namespace Gradual.Spider.ServicoSupervisor.Cron
{
    public class CronTasks
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        private static CronTasks _me;
        public static CronTasks GetInstance()
        {
            if (_me == null)
            {
                _me = new CronTasks();
            }
            return _me;
        }



        public void DummyFunction()
        {
            logger.Info("==================================================================");
            logger.Info("Dummy Function executed...");
        }


        public void CarregarInfoAberturas()
        {
            logger.Info("==================================================================");
            logger.Info("Executando CarregarInfoAberturas");
            ConsolidatedRiskManager.Instance.LoadConsolidatedRisk();
        }
    }
}
