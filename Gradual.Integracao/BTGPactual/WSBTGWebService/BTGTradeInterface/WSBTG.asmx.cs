using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using log4net;
using BTGTradeProcessor;
using System.Configuration;
using System.Data;

namespace BTGTradeInterface
{

    /// <summary>
    /// Summary description for WSBTG
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/BTGTradeInterface/WSBTG")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSBTG : System.Web.Services.WebService
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WSBTG() : base()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [System.Web.Services.WebMethod(MessageName = "GetBMFIntraday")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/BTGTradeInterface/WSBTG/GetBMFIntraday",
        RequestNamespace = "http://tempuri.org/BTGTradeInterface/WSBTG",
        ResponseNamespace = "http://tempuri.org/BTGTradeInterface/WSBTG",
        Use = System.Web.Services.Description.SoapBindingUse.Literal,
        ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public DataSet GetBMFIntraday(string Cd_Usuario, string Ds_Senha, long lastSequence)
        {
            TradeProcessor trProc = new TradeProcessor();

            string user = ConfigurationManager.AppSettings["username"].ToString();
            string pwd = ConfigurationManager.AppSettings["password"].ToString();

            if (user.Equals(Cd_Usuario) == false ||
                pwd.Equals(Ds_Senha) == false)
            {
                DataSet ds = new DataSet("NewDataSet");
                DataTable table = new DataTable("Table1");
                DataColumn col = new DataColumn("ERRO");
                table.Columns.Add(col);
                ds.Tables.Add(table);

                DataRow dr = ds.Tables["Table1"].NewRow();

                dr["Erro"] = "Usuario / senha invalidos";

                ds.Tables["Table1"].Rows.Add(dr);

                logger.Fatal("Usuario / senha invalidos");
                return ds;
            }

            logger.Info("GetBMFIntraday(" + lastSequence + ") Inicio");

            //string xmlTrades = trProc.GetTrades(lastSequence);

            //string xmlRet = "<QueryTradesStrResponse>";

            //string xmlRet = "<QueryTradesStrResult>";
            //xmlRet += xmlTrades;
            //xmlRet += "</QueryTradesStrResult>";
            //xmlRet += "<pTradeID>";
            //xmlRet += pTradeID.ToString("yyyy/MM/dd HH:mm:ss");
            //xmlRet += "</pTradeID>";
            //xmlRet += "</QueryTradesStrResponse>";

            //logger.Debug("XML Trades [" + xmlTrades + "]");

            // logger.Info("QueryTradesStr(" + lastSequence + ") FIM");

            return trProc.GetTradesDataset(lastSequence);

            //QueryTradesStr1 resp = new QueryTradesStr1();
            //resp.pTradeID = pTradeID.ToString("yyyy/MM/dd HH:mm:ss");
            //resp.QueryTradesStrResult = xmlTrades;

            //return resp;
        }
    }
}
