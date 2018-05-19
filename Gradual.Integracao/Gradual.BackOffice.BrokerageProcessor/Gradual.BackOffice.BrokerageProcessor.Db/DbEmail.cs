using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.BackOffice.BrokerageProcessor.Lib.Email;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Gradual.BackOffice.BrokerageProcessor.Db
{
    public class DbEmail
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        string _strConnectionStringDefault;
        public DbEmail()
        {
            _strConnectionStringDefault = ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString;
        }

        private SqlConnection _abrirConexaoSql(string strConnectionString)
        {
            SqlConnection sql = null;
            try
            {
                sql = new SqlConnection(strConnectionString);
                sql.Open();
                return sql;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na abertura da conexao: " + ex.Message, ex);
                throw;
            }
        }
        private void _fecharConexao(SqlConnection sql)
        {
            try
            {
                sql.Close();
                sql.Dispose();
                sql = null;
            }
            catch { }

        }


        public bool InserirEmailNotaCorretagem(EmailNotaCorretagemInfo info)
        {
            try
            {
                SqlConnection sql = _abrirConexaoSql(_strConnectionStringDefault);
                SqlCommand sqlCommand = new SqlCommand("prc_nota_corretagem_email_ins", sql);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@IdCliente", info.IdCliente));
                sqlCommand.Parameters.Add(new SqlParameter("@DtRegistroEmail", info.DtRegistroEmail));
                sqlCommand.Parameters.Add(new SqlParameter("@EmailOrigem", info.EmailOrigem));
                sqlCommand.Parameters.Add(new SqlParameter("@EmailDest", info.EmailDestinatario));
                sqlCommand.Parameters.Add(new SqlParameter("@EmailDestCc", info.EmailDestinatarioCc));
                sqlCommand.Parameters.Add(new SqlParameter("@EmailDestCco", info.EmailDestinatarioCco));
                sqlCommand.Parameters.Add(new SqlParameter("@Assunto", info.Assunto));
                sqlCommand.Parameters.Add(new SqlParameter("@Corpo", info.Body));
                sqlCommand.Parameters.Add(new SqlParameter("@ArquivoNC", info.ArquivoNota));
                sqlCommand.Parameters.Add(new SqlParameter("@Bolsa", info.Bolsa));
                sqlCommand.Parameters.Add(new SqlParameter("@Status", info.Status));
                sqlCommand.Parameters.Add(new SqlParameter("@DescStatus", info.DescStatus));

                int rows = sqlCommand.ExecuteNonQuery();
                _fecharConexao(sql);
                if (rows > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na insercao do Email Nota de Corretagem: " + ex.Message, ex);
                return false;
            }
        }

        public Dictionary<int, List<string>> BuscarPosicaoClienteEmail()
        {
            try
            {
                Dictionary<int, List<string>> ret = new Dictionary<int, List<string>>();
                SqlDataAdapter lAdapter;
                SqlConnection  sqlConn = _abrirConexaoSql(_strConnectionStringDefault);

                // TODO [FF]: Rever qual conn string utilizar
                DataSet lDataSet = new DataSet();
                SqlCommand  sqlCommand = new SqlCommand("prc_posicao_cliente_email_sel", sqlConn);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(sqlCommand);
                lAdapter.Fill(lDataSet);
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    List<string> lst = null;
                    if (!ret.TryGetValue(lRow["Account"].DBToInt32(), out lst))
                    {
                        lst = new List<string>();
                        lst.Add(lRow["Email"].DBToString());
                        ret.Add(lRow["Account"].DBToInt32(), lst);
                    }
                    else
                    {
                        string aux = lst.FirstOrDefault(x => x.Equals(lRow["Email"].DBToString(), StringComparison.CurrentCultureIgnoreCase));
                        if (string.IsNullOrEmpty(aux))
                            lst.Add(lRow["Email"].DBToString());
                    }
                }
                _fecharConexao(sqlConn);
                lAdapter.Dispose();
                lDataSet.Dispose();
                sqlCommand.Dispose();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na consulta de emails de Posicao Cliente: " + ex.Message, ex);
                return null;
            }


        }

    }
}
