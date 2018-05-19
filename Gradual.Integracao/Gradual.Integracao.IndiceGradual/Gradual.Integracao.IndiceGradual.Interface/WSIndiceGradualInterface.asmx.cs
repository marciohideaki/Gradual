using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using log4net;
using Gradual.OMS.Library;
using Gradual.Integracao.IndiceGradual.lib;

namespace Gradual.Integracao.IndiceGradual.Interface
{
    /// <summary>
    /// Summary description for WSIndiceGradualInterface
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/Gradual.Integracao.IndiceGradual.Interface/WSIndiceGradualInterface")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSIndiceGradualInterface : System.Web.Services.WebService
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IndiceGradualConfig _config;

        public WSIndiceGradualInterface()
            : base()
        {
            log4net.Config.XmlConfigurator.Configure();
            _config = GerenciadorConfig.ReceberConfig<IndiceGradualConfig>();
        }

        [WebMethod(MessageName = "QueryIndiceGradualString")]
        [SoapDocumentMethodAttribute("http://tempuri.org/Gradual.Integracao.IndiceGradual.Interface/WSIndiceGradualInterface/QueryIndiceGradualString",
        RequestNamespace = "http://tempuri.org/Gradual.Integracao.IndiceGradual.Interface/WSIndiceGradualInterface",
        ResponseNamespace = "http://tempuri.org/Gradual.Integracao.IndiceGradual.Interface/WSIndiceGradualInterface",
        Use = SoapBindingUse.Literal,
        ParameterStyle = SoapParameterStyle.Wrapped)]
        public string QueryIndiceGradualString(
            string pUser,
            string pPassword,
            int pInstitutionID,
            int pIndiceID)
        {
            string xmlConsultaIndice;
            try
            {
                IndiceGradualProcessor processor = new IndiceGradualProcessor();

                string pwd64 = base64Encode(pPassword);

                logger.Debug("Usuario informado[" + pUser + "]");
                if (pUser.Equals(_config.IndiceGradualUsuario) == false || pwd64.Equals(_config.IndiceGradualSenha) == false)
                {
                    xmlConsultaIndice = "<erro>Usuario ou senha invalidos</erro>";
                }
                else
                {
                    DateTime agora = DateTime.Now;
                    logger.Info("QueryIndiceGradualString(" + agora.ToString("yyyy/MM/dd HH:mm:ss") + ") Inicio");

                    xmlConsultaIndice = processor.GetIndiceGradual(pIndiceID);

                    logger.Debug("XML ConsultaIndice [" + xmlConsultaIndice + "]");

                    logger.Info("QueryIndiceGradualString(" + agora.ToString("yyyy/MM/dd HH:mm:ss") + ") Fim");
                }
            }
            catch (Exception ex)
            {
                logger.Error("QueryIndiceGradualString(): " + ex.Message, ex);
                xmlConsultaIndice = "<erro>" + ex.Message + ex.StackTrace + "</erro>";
            }

            string xmlRetorno = "<indices>";
            xmlRetorno += xmlConsultaIndice;
            xmlRetorno += "</indices>";

            return xmlRetorno;
        }

        private string base64Encode(string data)
        {
            try
            {
                byte[] bt = new byte[data.Length];
                bt = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(bt);

                return encodedData;
            }
            catch (Exception ex)
            {
                logger.Error("base64Encode():" + ex.Message, ex);
                throw ex;
            }
        }
    }
}
