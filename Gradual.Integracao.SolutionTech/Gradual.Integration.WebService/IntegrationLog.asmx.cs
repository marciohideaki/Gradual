using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Gradual.Integration.WebService
{
    /// <summary>
    /// Summary description for IntegrationLog
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class IntegrationLog : System.Web.Services.WebService
    {

        public IntegrationLog()
        {
            
        }

        [WebMethod]
        public Messages.LogResponse Log(Messages.LogRequest request)
        {
            Messages.LogResponse lResponse = new Messages.LogResponse();

            try
            {
                Objects.GradualException ex = new Objects.GradualException(request.message, request.StackTrace);

                Gradual.Utils.Logger.Log(request.logName, Utils.LoggingLevel.Error, request.message, new { User = "SolutionTech", Environment = "Producao" }, ex);
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("WebService", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return lResponse;
        }
    }
}
