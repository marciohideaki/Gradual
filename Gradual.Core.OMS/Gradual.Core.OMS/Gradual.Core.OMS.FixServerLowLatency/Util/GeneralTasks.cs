using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using Gradual.Core.OMS.FixServerLowLatency.Rede;

namespace Gradual.Core.OMS.FixServerLowLatency.Util
{
    public class GeneralTasks
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        List<FixInitiator> _lstInitiators;
        #endregion


        public static GeneralTasks Instance { get { return GetInstance(); } }
        private static GeneralTasks _me = null;
        public static GeneralTasks GetInstance()
        {
            if (_me == null)
            {
                _me = new GeneralTasks();
            }
            return _me;
        }


        public GeneralTasks()
        {
            
        }

        ~GeneralTasks()
        {
            _lstInitiators = null;
        }


        #region Setup / Initializator Functions
        public void SetFixInitiators(List <FixInitiator> lst)
        {
            try
            {
                _lstInitiators = lst;
            }
            catch (Exception ex)
            {
                logger.Error("SetFixInitiators(): " + ex.Message, ex);
            }
        }

        #endregion

        #region Task Functions
        /// <summary>
        /// Routine to backup messages from dictionary to a file
        /// </summary>
        public void BackupOrderSessions()
        {
            try
            {
                
                if (null!=_lstInitiators)
                {
                    foreach (FixInitiator fix in _lstInitiators)
                    {
                        fix.SerializeMsgs();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("BackupOrderSessions(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Routine to clear expired registries from Message Dictionary
        /// CAUTION: Be really careful to frequency execution, because it will
        /// lock all dictionary until all operations are done
        /// </summary>
        public void PurgeOrderSessions()
        {
            try
            {
                foreach (FixInitiator fix in _lstInitiators)
                {
                    fix.ExpireOrderSessions();
                }
            }
            catch (Exception ex)
            {
                logger.Error("PurgeOrderSessions(): " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Routine to force memory cleaning, using in case of too much memory usage
        /// </summary>
        public void ReloadDataLimit()
        {
            try
            {
                logger.Info("ReloadDataLimit() - START ReloadLimits");
                LimiteManager.LimitControl.GetInstance().ReloadLimits(true);
                logger.Info("ReloadDataLimit() - END ReloadLimits");
            }
            catch (Exception ex)
            {
                logger.Error("ReloadDataLimit(): Erro: " + ex.Message, ex);
            }
        }

        #endregion
    }
}
