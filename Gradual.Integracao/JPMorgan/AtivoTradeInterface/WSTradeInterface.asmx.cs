using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using OpenBlotterLib;
using log4net;
using System.Web.Services.Protocols;
using Gradual.OMS.Library;
using System.Xml.Serialization;

namespace AtivoTradeInterface
{
    //[Serializable]
    //[XmlRootAttribute("QueryTradesStrResponse")]
    //public struct QueryTradesStr1
    //{
    //    [XmlElement(IsNullable = true)]
    //    public string QueryTradesStrResult { get; set; }
        
    //    [XmlElement(IsNullable = true)]
    //    public string pTradeID { get; set; }
    //}

    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/AttivoTradeInterface/WSTradeInterface")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSTradeInterface : System.Web.Services.WebService
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private OpenBlotterConfig _config;

        public WSTradeInterface()
            : base()
        {
            log4net.Config.XmlConfigurator.Configure();
            _config = GerenciadorConfig.ReceberConfig<OpenBlotterConfig>();
        }

        [System.Web.Services.WebMethod(MessageName = "QueryTradesStr")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/AttivoTradeInterface/WSTradeInterface/QueryTradesStr",
        RequestNamespace = "http://tempuri.org/AttivoTradeInterface/WSTradeInterface",
        ResponseNamespace = "http://tempuri.org/AttivoTradeInterface/WSTradeInterface",
        Use = System.Web.Services.Description.SoapBindingUse.Literal,
        ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string QueryTradesStr(string pUser,
            string pPassword,
            int pInstitutionID,
            DateTime pInitialDate,
            DateTime pFinalDate,
            ref DateTime pTradeID,
            string pTradeStatus,
            string pProduct,
            string pAfterHour,
            string pTraderID )
        {
            try
            {
                TradeProcessor trProc = new TradeProcessor();

                string pwd64 = base64Encode(pPassword);

                if (pUser.Equals(_config.OpenBlotterUsr) == false || pwd64.Equals(_config.OpenBlotterPwd) == false)
                {
                    return "pUser or pPassword is invalid!";

                    //QueryTradesStr1 error = new QueryTradesStr1();
                    //error.pTradeID = pTradeID.ToString("yyyy/MM/dd HH:mm:ss");
                    //error.QueryTradesStrResult = "pUser or pPassword is invalid!";

                    //return error;
                }

                logger.Info("QueryTradesStr(" + pTradeID.ToString("yyyy/MM/dd HH:mm:ss") + ") Inicio");

                string xmlTrades = trProc.GetTrades(pTradeID);

                //string xmlRet = "<QueryTradesStrResponse>";

                //string xmlRet = "<QueryTradesStrResult>";
                //xmlRet += xmlTrades;
                //xmlRet += "</QueryTradesStrResult>";
                //xmlRet += "<pTradeID>";
                //xmlRet += pTradeID.ToString("yyyy/MM/dd HH:mm:ss");
                //xmlRet += "</pTradeID>";
                //xmlRet += "</QueryTradesStrResponse>";

                logger.Debug("XML Trades [" + xmlTrades + "]");

                logger.Info("QueryTradesStr(" + pTradeID.ToString("yyyy/MM/dd HH:mm:ss") + ") FIM");

                return xmlTrades;

                //QueryTradesStr1 resp = new QueryTradesStr1();
                //resp.pTradeID = pTradeID.ToString("yyyy/MM/dd HH:mm:ss");
                //resp.QueryTradesStrResult = xmlTrades;

                //return resp;
            }
            catch (Exception ex)
            {
                logger.Error("QueryTradesStr: " + ex.Message, ex);
            }

            return "<trades/>";
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