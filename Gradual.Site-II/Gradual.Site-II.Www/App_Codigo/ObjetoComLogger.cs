using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace Gradual.Site.Www
{
    public class ObjetoComLogger
    {
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}