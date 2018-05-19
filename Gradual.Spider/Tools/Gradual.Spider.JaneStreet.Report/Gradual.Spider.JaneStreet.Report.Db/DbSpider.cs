using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Gradual.Spider.JaneStreet.Report.Lib.Dados;
using log4net;

namespace Gradual.Spider.JaneStreet.Report.Db
{
    public class DbSpider
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        #region Private Variables
        protected SqlConnection _sqlConn;
        protected SqlCommand _sqlCommand;
        string _strConnectionStringDefault;
        #endregion

        #region Properties
        public bool ConexaoIniciada
        {
            get
            {
                return !(_sqlConn == null);
            }
        }
        public bool ConexaoAberta
        {
            get
            {
                return (_sqlConn != null && _sqlConn.State == System.Data.ConnectionState.Open);
            }
        }
        #endregion


        public DbSpider()
        {
            _sqlConn = null;
            _sqlCommand = null;
            _strConnectionStringDefault = ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString;
        }

        protected void AbrirConexao(string strConnectionString)
        {
            if (!this.ConexaoAberta)
            {
                _sqlConn = new SqlConnection(strConnectionString);
                _sqlConn.Open();
            }
        }

        protected void FecharConexao()
        {
            try
            {
                _sqlConn.Close();
                _sqlConn.Dispose();
            }
            catch { }
        }
        
        ~DbSpider()
        {
            if (null != _sqlCommand)
            {
                _sqlCommand.Dispose();
                _sqlCommand = null;
            }
            if (null!=_sqlConn)
            {
                FecharConexao();
            }

        }


        public List<ReportDCInfo> BuscarOrdensDropCopy()
        {
            try
            {
                List<ReportDCInfo> ret = new List<ReportDCInfo>();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_report_dropcopy", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    ReportDCInfo item = new ReportDCInfo();
                    item.DateOrder = lRow["DATE"].DBToString();
                    item.Client = lRow["CLIENT"].DBToInt32();
                    item.HourOrder = lRow["HOUR"].DBToString();
                    item.Side = lRow["SIDE"].DBToString();
                    item.Filled = lRow["FILLED"].DBToInt32();
                    item.Price = lRow["Price"].DBToDecimal();
                    item.RegisterTime = lRow["RegisterTime"].DBToDateTime();
                    item.OrderQty = lRow["ORDERQTY"].DBToInt32();
                    item.OrderPx = lRow["OrdPx"].DBToDecimal();
                    item.ContraBroker = lRow["ContraBroker"].DBToString();
                    ret.Add(item);
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na consulta: " + ex.Message, ex);
                return null;
            }


        }

        public DataSet BuscarOrdensDropCopyDataSet(int id)
        {
            try
            {
                List<ReportDCInfo> ret = new List<ReportDCInfo>();
                SqlDataAdapter lAdapter;
                AbrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_report_dropcopy", _sqlConn);
                _sqlCommand.Parameters.Add(new SqlParameter("@IdFix", id));
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                /*
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    ReportDCInfo item = new ReportDCInfo();
                    item.DateOrder = lRow["DATE"].DBToString();
                    item.Client = lRow["CLIENT"].DBToInt32();
                    item.HourOrder = lRow["HOUR"].DBToString();
                    item.Side = lRow["SIDE"].DBToString();
                    item.Filled = lRow["FILLED"].DBToInt32();
                    item.Price = lRow["Price"].DBToDecimal();
                    item.RegisterTime = lRow["RegisterTime"].DBToDateTime();
                    item.OrderQty = lRow["ORDERQTY"].DBToInt32();
                    item.OrderPx = lRow["OrdPx"].DBToDecimal();
                    item.ContraBroker = lRow["ContraBroker"].DBToString();
                    ret.Add(item);
                }*/
                return lDataSet;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na consulta: " + ex.Message, ex);
                return null;
            }
        }
    }
}
