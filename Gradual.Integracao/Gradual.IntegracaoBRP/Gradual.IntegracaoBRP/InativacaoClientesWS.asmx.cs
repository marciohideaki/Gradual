using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;
using log4net;
using Gradual.IntegracaoBRP.Lib.Dados;
using Gradual.IntegracaoBRP.Lib;

namespace Gradual.IntegracaoBRP
{
    /// <summary>
    /// Summary description for InativacaoClientes
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class InativacaoClientesWS : System.Web.Services.WebService
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public InativacaoClientesWS() : base()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [System.Web.Services.WebMethod(MessageName = "GetInactiveAccounts")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Gradual.IntegracaoBRP/InativacaoClientes/GetInactiveAccounts",
        RequestNamespace = "http://tempuri.org/Gradual.IntegracaoBRP/InativacaoClientes",
        ResponseNamespace = "http://tempuri.org/Gradual.IntegracaoBRP/InativacaoClientes",
        Use = System.Web.Services.Description.SoapBindingUse.Literal,
        ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public DataSet GetInactiveAccounts(string Cd_Usuario, string Ds_Senha)
        {
            DataSet retorno = new DataSet("NewDataSet");

            logger.Info( "Inicio GetInactiveAccounts() Remote IP [" + this.Context.Request.UserHostAddress+ "]");

            try
            {

                string user = ConfigurationManager.AppSettings["username"].ToString();
                string pwd = ConfigurationManager.AppSettings["password"].ToString();

                if (user.Equals(Cd_Usuario) == false ||
                    pwd.Equals(Ds_Senha) == false)
                {
                    logger.Fatal("Usuario / senha invalidos");
                    return RetornaErro("Usuario / senha invalidos");
                }

                logger.Debug("Buscando a lista de clientes ativos" );
                PersistenciaDB db = new PersistenciaDB(ConfigurationManager.ConnectionStrings["TRADE"].ConnectionString);

                List<InativacaoClienteInfo> ret = db.BuscarClientesAtivos();

                logger.Debug("Lista de clientes ativos com " + ret.Count + " itens." );

                DataTable table = new DataTable("Table1");
                DataColumn col = new DataColumn("CD_CLIENTE");
                table.Columns.Add(col);
                col = new DataColumn("CLIENTE_PLURAL");
                table.Columns.Add(col);
                col = new DataColumn("STATUS");
                table.Columns.Add(col);

                retorno.Tables.Add(table);

                foreach (InativacaoClienteInfo info in ret)
                {
                    DataRow dr = retorno.Tables["Table1"].NewRow();

                    dr["CD_CLIENTE"] = info.CDClienteGradual;
                    dr["CLIENTE_PLURAL"] = info.CDClientePlural;
                    dr["STATUS"] = info.StatusCliente;

                    retorno.Tables["Table1"].Rows.Add(dr);
                }

                logger.Info("Final GetInactiveAccounts() Remote IP [" + this.Context.Request.UserHostAddress + "]");
            }
            catch (Exception ex)
            {
                logger.Error("Erro GetInactiveAccounts():" + ex.Message, ex);

                return RetornaErro("Erro ao processar consulta");
            }

            return retorno;
        }

        private DataSet RetornaErro(string erro)
        {
            DataSet ds = new DataSet("NewDataSet");
            DataTable table = new DataTable("Table1");
            DataColumn col = new DataColumn("ERRO");
            table.Columns.Add(col);
            ds.Tables.Add(table);

            DataRow dr = ds.Tables["Table1"].NewRow();

            dr["Erro"] = erro;

            ds.Tables["Table1"].Rows.Add(dr);

            return ds;
        }
            
    }
}
