using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

using Gradual.Core.OMS.SmartTrader.Lib.Mensagens;
using Gradual.Core.OMS.SmartTrader.Streamer;

namespace Gradual.Core.OMS.SmartTrader
{
    public class ProcessarOrdemSmart
    {

        #region log4net declaration

        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        SmartMonitor _smtMonitor;
        #endregion

        public ProcessarOrdemSmart()
        {
            try
            {
                _smtMonitor = new SmartMonitor();
                _smtMonitor.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Erro Constructor - Start SmartMonitor: " + ex.Message, ex);
            }

        }

        ~ProcessarOrdemSmart()
        {
            try
            {
                _smtMonitor.Stop();
                _smtMonitor = null;
            }
            catch (Exception ex)
            {
                logger.Error("Erro Destructor - Start SmartMonitor: " + ex.Message, ex);
            }
        }




        public EnviarOrdemSmartResponse EnviarOrdem(EnviarOrdemSmartRequest ordem)
        {
            try
            {
                EnviarOrdemSmartResponse resp = new EnviarOrdemSmartResponse();
                if (null != _smtMonitor)
                    _smtMonitor.AddInstrument(ordem.SmartOrder.Instrument, ordem.SmartOrder);
                return resp;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na Ordem Smart: " + ex.Message, ex);
                return null;
            }
        }

    }
}
