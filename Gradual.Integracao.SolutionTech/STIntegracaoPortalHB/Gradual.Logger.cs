using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Integration.WebService.Messages;
using Gradual.Integration.WebService.Objects;
using Gradual.Utils;

namespace STIntegracaoPortalHB
{
    public static class GradualLogger
    {

        public static void Log(string type, string methodName, Exception ex)
        {
            //Gradual.Integration.WebService.IntegrationLogSoapClient integration = new Gradual.Integration.WebService.IntegrationLogSoapClient();
            Gradual.Integration.WebService.IntegrationLog integration = new Gradual.Integration.WebService.IntegrationLog();
            Gradual.Integration.WebService.LogRequest request = new Gradual.Integration.WebService.LogRequest();
            Error error = new Error();

            request.logName = "SolutionTechExterno";
            request.message = string.Format("Erro no método: {0}.{1}. Tipo: {2}. Erro: {3}. Origem: {4}.",
                type,
                methodName,
                ex.GetType().Name,
                ex.Message,
                ex.StackTrace);
            request.StackTrace = ex.StackTrace;

            //error.Message = ex.Message;
            //error.StackTrace = ex.StackTrace;
            //error.TimeStamp = DateTime.Now;

            //request.error = error;

            //byte[] lRequest = ObjectToByteArray(request);

            integration.Log(request);
        }
    }
}