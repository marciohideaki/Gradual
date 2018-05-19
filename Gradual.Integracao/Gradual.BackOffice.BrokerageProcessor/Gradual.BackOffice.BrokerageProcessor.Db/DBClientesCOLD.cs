using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using log4net;
using System.Data.SqlClient;
using Gradual.BackOffice.BrokerageProcessor.Lib.Cold;
using System.Data;

namespace Gradual.BackOffice.BrokerageProcessor.Db
{
    public class DBClientesCOLD
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        string _strConnectionStringDefault;
        public DBClientesCOLD()
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


        public Dictionary<int, STGrupoRelatCold> ObterListaGruposCOLD()
        {
            Dictionary<int, STGrupoRelatCold> retorno = new Dictionary<int, STGrupoRelatCold>();

            try
            {
                SqlDataAdapter lAdapter;
                SqlConnection sqlConn = _abrirConexaoSql(_strConnectionStringDefault);

                // TODO [FF]: Rever qual conn string utilizar
                DataSet lDataSet = new DataSet();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConn;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "SELECT * FROM tb_grupos_relcold";

                lAdapter = new SqlDataAdapter(sqlCommand);
                lAdapter.Fill(lDataSet);

                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    STGrupoRelatCold st = new STGrupoRelatCold();

                    st.IDGrupo = lRow["id_grupo"].DBToInt32();
                    st.EmailsTO = lRow["email_to"].DBToString();
                    st.EmailsCC = lRow["email_cc"].DBToString();
                    st.EmailsBCC = lRow["email_bcc"].DBToString();
                    st.FlagAnexo = lRow["flg_send_attached"].DBToInt32() == 1 ? true : false;
                    st.FlagFolhaUnica = lRow["flg_send_unified"].DBToInt32() == 1 ? true : false;
                    st.FlagPdf = lRow["flg_send_pdf"].DBToInt32() == 1 ? true : false;
                    st.FlagZip = lRow["flg_send_zipped"].DBToInt32() == 1 ? true : false;

                    retorno.Add(st.IDGrupo, st);
                }

                _fecharConexao(sqlConn);

                lAdapter.Dispose();
                lDataSet.Dispose();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na consulta da lista de grupos COLD: " + ex.Message, ex);
            }

            return retorno;
        }

        public Dictionary<int, STClienteRelatCold> ObterListaClientesCOLD(int idgrupo=-1)
        {
            Dictionary<int, STClienteRelatCold> retorno = new Dictionary<int, STClienteRelatCold>();

            try
            {
                SqlDataAdapter lAdapter;
                SqlConnection sqlConn = _abrirConexaoSql(_strConnectionStringDefault);

                DataSet lDataSet = new DataSet();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConn;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                if ( idgrupo==-1)
                    sqlCommand.CommandText = "SELECT * FROM tb_clientes_relcold";
                else
                    sqlCommand.CommandText = "SELECT * FROM tb_clientes_relcold WHERE id_grupo=" + idgrupo;

                lAdapter = new SqlDataAdapter(sqlCommand);
                lAdapter.Fill(lDataSet);


                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    STClienteRelatCold st = new STClienteRelatCold();

                    st.Account = Convert.ToInt32(lRow["account"].DBToString());
                    st.IDGrupo = lRow["id_grupo"].DBToInt32();

                    st.FlagAnexo          = lRow["flg_send_attached"].DBToInt32() == 1?true:false;
                    st.FlagBTC            = lRow["flg_rel_btc"].DBToInt32() == 1 ? true : false;
                    st.FlagCustodia       = lRow["flg_rel_custodia"].DBToInt32() == 1 ? true : false;
                    st.FlagExigencia      = lRow["flg_rel_exigencia"].DBToInt32() == 1 ? true : false;
                    st.FlagFolhaUnica     = lRow["flg_send_unified"].DBToInt32() == 1 ? true : false;
                    st.FlagGarantia       = lRow["flg_rel_garantia"].DBToInt32() == 1 ? true : false;
                    st.FlagLiqInvest      = lRow["flg_rel_liqinvest"].DBToInt32() == 1 ? true : false;
                    st.FlagPdf            = lRow["flg_send_pdf"].DBToInt32() == 1 ? true : false;
                    st.FlagPosCliente     = lRow["flg_rel_posclient"].DBToInt32() == 1 ? true : false;
                    st.FlagPosDivBtc      = lRow["flg_rel_posdivbtc"].DBToInt32() == 1 ? true : false;
                    st.FlagTermo          = lRow["flg_rel_termo"].DBToInt32() == 1 ? true : false;
                    st.FlagZip            = lRow["flg_send_zipped"].DBToInt32() == 1 ? true : false;
                    st.FlagConvertCold    = lRow["flg_convert_cold"].DBToInt32() == 1 ? true : false;
                    st.FlagSendFlatImbarq = lRow["flg_send_flat_imbarq"].DBToInt32() == 1 ? true : false;

                    if (retorno.ContainsKey(st.Account))
                    {
                        logger.Error("CONTA [" + st.Account + "] JA EXISTE, VERIFIQUE TABELA tb_clientes_relcold POR DUPLICATAS");
                        continue;
                    }

                    retorno.Add(st.Account,st);
                }

                _fecharConexao(sqlConn);

                lAdapter.Dispose();
                lDataSet.Dispose();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na consulta da lista de clientes COLD: " + ex.Message, ex);
            }

            return retorno;
        }
    }
}
