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
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString);

                sqlConn.Open();

                SqlDataAdapter lAdapter;

                DataSet lDataSet = new DataSet();

                SqlCommand gComando = new SqlCommand("prc_buscar_ordens_fix_streamer", sqlConn);

                gComando.CommandType = System.Data.CommandType.StoredProcedure;

                gLog4Net.Debug("Chamando prc_buscar_ordens_fix_streamer()");

                lAdapter = new SqlDataAdapter(gComando);

                lAdapter.Fill(lDataSet);

                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    AlgoStruct algo = new AlgoStruct();

                    algo.IDLogin = lRow["IDLogin"].DBToInt32().ToString();
                    algo.IDRegistro = lRow["IDAlgoritmo"].DBToInt32().ToString();
                    algo.Instrumento1 = lRow["Instrumento1"].DBToString();
                    algo.Instrumento2 = lRow["Instrumento2"].DBToString();
                    algo.SentidoAlgoritmo = (SentidoAlgoritmoEnum)lRow["SentidoAlgoritmo"].DBToInt32();
                    algo.TipoAlgorito = (AlgoritmoEnum)lRow["TipoAlgorito"].DBToInt32();

                    lRetorno.Add(algo);
                }
            }
            catch (Exception ex)
            {
                gLog4Net.Error("BuscarOrdensSpider():" + ex.Message, ex);
            }

            return lRetorno;
        }
    }
}
