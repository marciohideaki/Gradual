using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using log4net;
using System.Configuration;
using System.Globalization;

namespace WSZarathustra
{
    /// <summary>
    /// Summary description for WSTraderBean
    /// </summary>
    [WebService(Namespace = "http://wszarathustra.gradualinvestimentos.com.br/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSTraderBean : System.Web.Services.WebService
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public WSTraderBean() : base()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

	
        [System.Web.Services.WebMethod(MessageName = "GetBeans")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://wszarathustra.gradualinvestimentos.com.br/WSZarathustra/WSTraderBean/GetBeans",
        RequestNamespace = "http://wszarathustra.gradualinvestimentos.com.br/WSZarathustra/WSTraderBean",
        ResponseNamespace = "http://wszarathustra.gradualinvestimentos.com.br/WSZarathustra/WSTraderBean",
        Use = System.Web.Services.Description.SoapBindingUse.Literal,
        ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public List<traderBean> GetBeans(string pUser, string pPassword, string pTradeID)
        {
            TradeProcessor trProc = new TradeProcessor();
            List<traderBean> beans = new List<traderBean>();

            bool bInTeste = false;

            if (ConfigurationManager.AppSettings["EmTeste"] != null &&
                ConfigurationManager.AppSettings["EmTeste"].ToString().ToLowerInvariant().Equals("true") )
            {
                bInTeste = true;
            }

            DateTime lastTimestamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            if ( bInTeste ) lastTimestamp = DateTime.MinValue;

            if (!String.IsNullOrEmpty(pTradeID))
            {
                try
                {
                    lastTimestamp = DateTime.ParseExact(pTradeID, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    logger.Error("Erro ao tentar efetuar parse do parametro pTradeID [" + pTradeID + "]");
                    logger.Error("Assumindo hora default [" + lastTimestamp.ToString("yyyy/MM/dd HH:mm:ss") + "]");
                }
            }

            //string pwd64 = base64Encode(pPassword);
            string user = ConfigurationManager.AppSettings["username"].ToString();
            string passwd = ConfigurationManager.AppSettings["password"].ToString();

            if (pUser.Equals(user) == false || pPassword.Equals(passwd) == false)
            {
                traderBean bean = new traderBean();
                bean.ID = "pUser or pPassword is invalid!";

                beans.Add(bean);

                return beans;
            }

            logger.Info("GetBeans(" + lastTimestamp.ToString("yyyy/MM/dd HH:mm:ss") + ") Inicio");

            beans.AddRange(trProc.GetTrades(lastTimestamp));

            

            //string xmlRet = "<QueryTradesStrResponse>";

            //string xmlRet = "<QueryTradesStrResult>";
            //xmlRet += xmlTrades;
            //xmlRet += "</QueryTradesStrResult>";
            //xmlRet += "<pTradeID>";
            //xmlRet += pTradeID.ToString("yyyy/MM/dd HH:mm:ss");
            //xmlRet += "</pTradeID>";
            //xmlRet += "</QueryTradesStrResponse>";

            //logger.Debug("XML Trades [" + xmlTrades + "]");

            logger.Info("GetBeans(" + lastTimestamp.ToString("yyyy/MM/dd HH:mm:ss") + ") FIM");

            return beans;

            //QueryTradesStr1 resp = new QueryTradesStr1();
            //resp.pTradeID = pTradeID.ToString("yyyy/MM/dd HH:mm:ss");
            //resp.QueryTradesStrResult = xmlTrades;

            //return resp;
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
