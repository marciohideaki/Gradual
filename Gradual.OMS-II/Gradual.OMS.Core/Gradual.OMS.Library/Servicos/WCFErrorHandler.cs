using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using log4net;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Handler para logar erros do WCF
    /// </summary>
    public class WCFErrorHandler : IErrorHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool HandleError(Exception error)
        {
            logger.Error(error);
            return false;
        }

        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
        }
    }
}
