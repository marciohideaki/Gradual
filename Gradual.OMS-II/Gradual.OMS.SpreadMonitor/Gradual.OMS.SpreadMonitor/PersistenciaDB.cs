using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.OMS.SpreadMonitor.Lib.Dados;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Gradual.OMS.SpreadMonitor
{
    public class PersistenciaDB
    {
        private static readonly ILog gLog4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portasoms"></param>
        /// <param name="contas"></param>
        /// <returns></returns>
        public List<AlgoStruct> CarregarAlgoritmos()
        {
            List<AlgoStruct> lRetorno = new List<AlgoStruct>();

            try
            {
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DirectTradeCadastro"].ConnectionString);

                sqlConn.Open();

                SqlDataAdapter lAdapter;

                DataSet lDataSet = new DataSet();

                SqlCommand gComando = new SqlCommand("prc_buscar_algoritmos", sqlConn);

                gComando.CommandType = System.Data.CommandType.StoredProcedure;

                gLog4Net.Debug("Chamando prc_buscar_algoritmos()");

                lAdapter = new SqlDataAdapter(gComando);

                lAdapter.Fill(lDataSet);

                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    AlgoStruct algo = new AlgoStruct();

                    algo.IDLogin = lRow["IDLogin"].DBToInt32().ToString();
                    algo.IDRegistro = lRow["IDAlgoritmo"].DBToInt32().ToString();
                    algo.Instrumento1 = lRow["Instrumento1"].DBToString();
                    algo.Instrumento2 = lRow["Instrumento2"].DBToString();
                    algo.Qtde1 = lRow["Quantidade1"].DBToInt32();
                    algo.Qtde2 = lRow["Quantidade2"].DBToInt32();
                    algo.SentidoAlgoritmo = (SentidoAlgoritmoEnum)lRow["SentidoAlgoritmo"].DBToInt32();
                    algo.TipoAlgoritmo = (AlgoritmoEnum)lRow["TipoAlgorito"].DBToInt32();

                    lRetorno.Add(algo);
                }

                sqlConn.Close();

                sqlConn.Dispose();
            }
            catch (Exception ex)
            {
                gLog4Net.Error("BuscarOrdensSpider():" + ex.Message, ex);
            }

            return lRetorno;
        }


        public string SalvarAlgoritmo (AlgoStruct algo)
        {
            string idalgoritmo = "";

            try
            {
                if (!String.IsNullOrEmpty(algo.IDRegistro))
                {
                    idalgoritmo = algo.IDRegistro;
                }

                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DirectTradeCadastro"].ConnectionString);

                sqlConn.Open();

                //SqlDataAdapter lAdapter;

                //DataSet lDataSet = new DataSet();

                SqlCommand gComando = new SqlCommand("prc_salvar_algoritmos", sqlConn);

                gComando.CommandType = System.Data.CommandType.StoredProcedure;

                gComando.Parameters.Add(new SqlParameter("@IDAlgoritmo", algo.IDRegistro));
                gComando.Parameters.Add(new SqlParameter("@IDLogin", algo.IDLogin));
                gComando.Parameters.Add(new SqlParameter("@Instrumento1", algo.Instrumento1));
                gComando.Parameters.Add(new SqlParameter("@Instrumento2", algo.Instrumento2));
                gComando.Parameters.Add(new SqlParameter("@Qtde1", algo.Qtde1));
                gComando.Parameters.Add(new SqlParameter("@Qtde2", algo.Qtde2));

                gComando.Parameters.Add(new SqlParameter("@LastCalc", algo.LastCalc));
                gComando.Parameters.Add(new SqlParameter("@SentidoAlgo", algo.SentidoAlgoritmo));
                gComando.Parameters.Add(new SqlParameter("@TipoAlgo", algo.TipoAlgoritmo));

                int id = (Int32) gComando.ExecuteScalar();

                idalgoritmo = id.ToString();

                sqlConn.Close();

                sqlConn.Dispose();
            }
            catch (Exception ex)
            {
                gLog4Net.Error("SalvarAlgoritmo():" + ex.Message, ex);
            }

            return idalgoritmo;
        }
    }
}
