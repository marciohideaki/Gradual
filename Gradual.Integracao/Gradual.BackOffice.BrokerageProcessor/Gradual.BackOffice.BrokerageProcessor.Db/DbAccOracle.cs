using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using log4net;
using Gradual.BackOffice.BrokerageProcessor.Lib.Account;
using System.Data.OracleClient;
using System.Configuration;

namespace Gradual.BackOffice.BrokerageProcessor.Db

{
    public class DbAccOracle
    {
        #region log4net declaration
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        public DbAccOracle()
        {

        }

        public ConcurrentDictionary<int, AccountParserInfo> CarregarListaContas(bool accStripDigit = true)
        {
            ConcurrentDictionary<int, AccountParserInfo> ret = new ConcurrentDictionary<int, AccountParserInfo>();
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["TRADE"].ConnectionString);
                // Buscar Posicao de cliente bovespa

                //using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_OBTER_CLIENTE_ACC_PARSER"))
                string sqlQuery = "SELECT * FROM tsccboutp WHERE cd_empresa=227 ";

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.CommandType = CommandType.Text;

                using (cmd)
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    DataTable table = dataSet.Tables[0];
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow dataRow = table.Rows[i];
                        AccountParserInfo ap = new AccountParserInfo();
                        ap.CdBolsa = dataRow["CD_BOLSA"].DBToString();
                        ap.CdMembro = dataRow["CD_MEMBRO"].DBToString();
                        ap.CdCliente = dataRow["CD_CLIENTE"].DBToInt32();
                        ap.CdClieOutrBolsa = dataRow["CD_CLIE_OUTR_BOLSA"].DBToString();
                        ap.DvClieOutrBolsa = dataRow["DV_CLIE_OUTR_BOLSA"].DBToString();
                        ap.InUtilCorresp = dataRow["IN_UTIL_CORRESP"].DBToString();
                        ap.InUtilArbitragem = dataRow["IN_UTIL_ARBITRAGEM"].DBToString();
                        ap.CdEmpresa = dataRow["CD_EMPRESA"].DBToInt32();
                        ap.CdUsuario = dataRow["CD_USUARIO"].DBToInt32();
                        ap.TpOcorrencia = dataRow["TP_OCORRENCIA"].DBToString();
                        
                        ret.AddOrUpdate(Convert.ToInt32(ap.CdClieOutrBolsa), ap, (key, oldvalue) => ap);
                    }
                }

                AccountParserInfo ap1 = new AccountParserInfo();
                ap1.CdBolsa = "SP";
                ap1.CdMembro = "227120";
                ap1.CdCliente = 43101;
                ap1.CdClieOutrBolsa = "1187";
                ap1.DvClieOutrBolsa = "1";
                ap1.InUtilCorresp = "S";
                ap1.InUtilArbitragem = "N";
                ap1.CdEmpresa = 227;
                ap1.CdUsuario = 1;
                ap1.TpOcorrencia = "D";

                AccountParserInfo ap2= new AccountParserInfo();
                ap2.CdBolsa = "SP";
                ap2.CdMembro = "227120";
                ap2.CdCliente = 43103;
                ap2.CdClieOutrBolsa = "3766";
                ap2.DvClieOutrBolsa = "1";
                ap2.InUtilCorresp = "S";
                ap2.InUtilArbitragem = "N";
                ap2.CdEmpresa = 227;
                ap2.CdUsuario = 1;
                ap2.TpOcorrencia = "D";
                
                AccountParserInfo ap3 = new AccountParserInfo();
                ap3.CdBolsa = "SP";
                ap3.CdMembro = "227120";
                ap3.CdCliente = 52875;
                ap3.CdClieOutrBolsa = "55555";
                ap3.DvClieOutrBolsa = "1";
                ap3.InUtilCorresp = "S";
                ap3.InUtilArbitragem = "N";
                ap3.CdEmpresa = 227;
                ap3.CdUsuario = 1;
                ap3.TpOcorrencia = "D";

                AccountParserInfo ap4 = new AccountParserInfo();
                ap4.CdBolsa = "SP";
                ap4.CdMembro = "227120";
                ap4.CdCliente = 36952;
                ap4.CdClieOutrBolsa = "7586";
                ap4.DvClieOutrBolsa = "1";
                ap4.InUtilCorresp = "S";
                ap4.InUtilArbitragem = "N";
                ap4.CdEmpresa = 227;
                ap4.CdUsuario = 1;
                ap4.TpOcorrencia = "D";

                AccountParserInfo ap5 = new AccountParserInfo();
                ap5.CdBolsa = "SP";
                ap5.CdMembro = "227120";
                ap5.CdCliente = 49725;
                ap5.CdClieOutrBolsa = "40004";
                ap5.DvClieOutrBolsa = "1";
                ap5.InUtilCorresp = "S";
                ap5.InUtilArbitragem = "N";
                ap5.CdEmpresa = 227;
                ap5.CdUsuario = 1;
                ap5.TpOcorrencia = "D";

                AccountParserInfo ap6 = new AccountParserInfo();
                ap6.CdBolsa = "SP";
                ap6.CdMembro = "227120";
                ap6.CdCliente = 55244;
                ap6.CdClieOutrBolsa = "6880";
                ap6.DvClieOutrBolsa = "1";
                ap6.InUtilCorresp = "S";
                ap6.InUtilArbitragem = "N";
                ap6.CdEmpresa = 227;
                ap6.CdUsuario = 1;
                ap6.TpOcorrencia = "D";

                //ret.AddOrUpdate(Convert.ToInt32(ap1.CdClieOutrBolsa), ap1, (key, oldvalue) => ap1);
                //ret.AddOrUpdate(Convert.ToInt32(ap2.CdClieOutrBolsa), ap2, (key, oldvalue) => ap2);
                //ret.AddOrUpdate(Convert.ToInt32(ap3.CdClieOutrBolsa), ap3, (key, oldvalue) => ap3);
                //ret.AddOrUpdate(Convert.ToInt32(ap4.CdClieOutrBolsa), ap4, (key, oldvalue) => ap4);
                //ret.AddOrUpdate(Convert.ToInt32(ap5.CdClieOutrBolsa), ap5, (key, oldvalue) => ap5);
                //ret.AddOrUpdate(Convert.ToInt32(ap6.CdClieOutrBolsa), ap6, (key, oldvalue) => ap6);

                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na consulta das Contas para AccountParser: " + ex.Message, ex);
                return null;
            }
        }


        public ConcurrentDictionary<int, string> CarregarNomeClientes(bool accStripDigit = true)
        {
            ConcurrentDictionary<int, string> ret = new ConcurrentDictionary<int, string>();
            try
            {
                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["TRADE"].ConnectionString);

                string sqlQuery = "";
                sqlQuery += " SELECT NM_CLIENTE, CD_CLIENTE";
                sqlQuery += " FROM (";
                sqlQuery += " SELECT NM_CLIENTE, CD_CLIENTE";
                sqlQuery += " FROM TSCCLIGER A ";
                sqlQuery += " INNER JOIN TSCCLIBOL B ON (A.CD_CPFCGC = B.CD_CPFCGC AND A.DT_NASC_FUND = B.DT_NASC_FUND) ";
                sqlQuery += " UNION ALL ";
                sqlQuery += " SELECT NM_CLIENTE, CODCLI AS CD_CLIENTE ";
                sqlQuery += " FROM TSCCLIGER A ";
                sqlQuery += " INNER JOIN TSCCLIBMF B ON (A.CD_CPFCGC = B.CD_CPFCGC AND A.DT_NASC_FUND = B.DT_NASC_FUND) ";
                sqlQuery += " )";

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.CommandType = CommandType.Text;

                using (cmd)
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    DataTable table = dataSet.Tables[0];
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow dataRow = table.Rows[i];

                        string account = dataRow["CD_CLIENTE"].DBToString();
                        string nome  = dataRow["NM_CLIENTE"].DBToString();

                        ret.AddOrUpdate(Convert.ToInt32(account), nome, (key, oldvalue) => nome);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("CarregarNomeClientes" + ex.Message, ex);
            }

            return ret;
        }
    }
}
