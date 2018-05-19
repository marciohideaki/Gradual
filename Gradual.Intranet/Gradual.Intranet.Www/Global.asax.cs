using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet.Www
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
             log4net.Config.XmlConfigurator.Configure();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (Session["CodigoSessao"] != null && Session["CodigoSessao"].ToString() != string.Empty)
            {
                IServicoSeguranca seguranca = (IServicoSeguranca)Application["ServicoSeguranca"]; //Ativador.Get<IServicoSeguranca>();
                string codigoSessao = Session["CodigoSessao"].ToString();
                try
                {
                    seguranca.EfetuarLogOut(new OMS.Library.MensagemRequestBase()
                    {
                        CodigoSessao = codigoSessao
                    });
                }
                catch (Exception ex)
                {
                    string x = ex.Message;
                    Ativador.AbortChannel(seguranca);
                    Application["ServicoSeguranca"] = null;
                }
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            
        }
    }
}