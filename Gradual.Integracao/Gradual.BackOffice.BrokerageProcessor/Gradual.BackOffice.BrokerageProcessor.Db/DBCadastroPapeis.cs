using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using log4net;
using System.Configuration;

namespace Gradual.BackOffice.BrokerageProcessor.Db
{
    public class DBCadastroPapeis
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public static string ObterCodigoNegociacao(string isin)
        {
            string retorno = String.Empty;

            try
            {
                SqlDataAdapter lAdapter;
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["OMS"].ConnectionString);
                sqlConn.Open();

                DataSet lDataSet = new DataSet();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConn;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                sqlCommand.CommandText = "SELECT  * FROM TB_ATIVO ";
                sqlCommand.CommandText += " where cd_negociacao_isin ='" + isin + "'";
                sqlCommand.CommandText += " and LotePadrao = 100";
                sqlCommand.CommandText += "  and id_ativo_tipo = 1";


                lAdapter = new SqlDataAdapter(sqlCommand);
                lAdapter.Fill(lDataSet);

                if (lDataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow lRow = lDataSet.Tables[0].Rows[0];

                    retorno = lRow["cd_negociacao"].DBToString();
                }

                lAdapter.Dispose();
                lDataSet.Dispose();
                sqlCommand.Dispose();
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                logger.Error("ObterCodigoNegociacao: " + ex.Message, ex);
            }

            return retorno;
        }

    }
}
