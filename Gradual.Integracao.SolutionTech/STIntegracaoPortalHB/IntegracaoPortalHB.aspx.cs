using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Reflection;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using Gradual.Integration.HomeBroker;

namespace STIntegracaoPortalHB
{
    public partial class IntegracaoPortalHB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ValidaRequest();

                ProcessaDadosRequest();
                
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                try
                {
                    GradualLogger.Log(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, ex);
                    byte[] byt = System.Text.Encoding.UTF8.GetBytes(ex.StackTrace);
                    string encoded = HttpUtility.UrlEncode(Convert.ToBase64String(byt));
                    Response.Redirect(String.Format("Error.aspx?Message={0}&StackTrace={1}", ex.Message, encoded));
                }
                catch (ThreadAbortException) { }
                catch (Exception exp)
                {
                    byte[] byt = System.Text.Encoding.UTF8.GetBytes(exp.StackTrace);
                    string encoded = HttpUtility.UrlEncode(Convert.ToBase64String(byt));
                    Response.Redirect(String.Format("Error.aspx?Message={0}&StackTrace={1}", exp.Message, encoded));
                }
            }
        }

        private void ValidaRequest()
        {
            if (string.IsNullOrEmpty(Request.Params["Token"]))
            {
                throw new Exception("Parâmetro obrigatório não encontrado [Token]");
            }

            if(string.IsNullOrEmpty(Request.Params["TokenType"]))
            {
                throw new Exception("Parâmetro obrigatório não encontrado [TokenType]");
            }

            if (string.IsNullOrEmpty(Request.Params["Host"]))
            {
                throw new Exception("Parâmetro obrigatório não encontrado [Host]");
            }
        }

        private void ProcessaDadosRequest()
        {
            try
            {
                // Carrega parametros
                string token            = Request.Params["Token"];
                string tokenType        = Request.Params["TokenType"];
                string host             = Request.Params["Host"];
                bool erro               = false;

                if (!string.IsNullOrEmpty(Request.Params["Erro"]))
                {
                    erro = true;
                }

                // Cria requisicao
                Request request         = new Request();
                request.Host            = host;
                request.Token           = token;
                request.TokenType       = tokenType;
                request.PublicKeyPath   = MapPath(String.Format("~/{0}", @ConfigurationManager.AppSettings.Get("PrivateKey")));

                // Efetua integracao
                Integration integration = new Integration();
                Response response       = integration.Integrate(request);

                // Valida resposta
                if (response.Valid)
                {
                    Session["atributoAutenticacaoAcessoDireto"] = response.User.Login;
                    string page = string.Format("{0}/ProcessaLoginAcessoDireto.aspx", ConfigurationManager.AppSettings.Get("EnderecoFerramenta"));

                    if(erro)
                    {
                        throw new Exception("Teste de erro");
                    }

                    Response.Redirect(page);
                }
                else
                {
                    GradualLogger.Log(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, response.Except);
                    byte[] byt = System.Text.Encoding.UTF8.GetBytes(response.Except.StackTrace);
                    string encoded = HttpUtility.UrlEncode(Convert.ToBase64String(byt));
                    Response.Redirect(String.Format("Error.aspx?Message={0}&StackTrace={1}", response.Except.Message, encoded));
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                try
                {
                    GradualLogger.Log(this.GetType().FullName, MethodInfo.GetCurrentMethod().Name, ex);
                    byte[] byt = System.Text.Encoding.UTF8.GetBytes(ex.StackTrace);
                    string encoded = HttpUtility.UrlEncode(Convert.ToBase64String(byt));
                    Response.Redirect(String.Format("Error.aspx?Message={0}&StackTrace={1}", ex.Message, encoded));
                }
                catch (ThreadAbortException) { }
                catch (Exception exp)
                {
                    byte[] byt = System.Text.Encoding.UTF8.GetBytes(exp.StackTrace);
                    string encoded = HttpUtility.UrlEncode(Convert.ToBase64String(byt));
                    Response.Redirect(String.Format("Error.aspx?Message={0}&StackTrace={1}", exp.Message, encoded));
                }
            }
        }
    }
}