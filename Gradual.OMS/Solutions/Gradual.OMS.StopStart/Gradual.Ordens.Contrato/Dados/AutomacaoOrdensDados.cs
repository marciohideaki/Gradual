using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Dados
{
    public class AutomacaoOrdensDados
    {
        #region Propriedades
        private string Conn = ConfigurationManager.AppSettings["Risco"].ToString();
        #endregion

        #region EnviarOrdemStop
        /// <summary>
        /// Inserir stop start da ordem na tabela tbStopStartOrder
        /// </summary>
        /// <param name="pOrdem"></param>
        /// <returns></returns>
        public virtual int EnviarOrdemStop(AutomacaoOrdensInfo pOrdem)
        {
            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection  = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "PRC_INS_ORDEM_STOP";

                SqlParameter OutParameter  = new SqlParameter();
                OutParameter.Direction     = ParameterDirection.Output;
                OutParameter.ParameterName = "@StopStartID";
                OutParameter.Size          = 32;

                command.Parameters.Add(OutParameter);
                command.Parameters.AddWithValue("@OrdTypeID",              pOrdem.OrdTypeID);
                command.Parameters.AddWithValue("@StopStartStatusID",      pOrdem.StopStartStatusID);
                command.Parameters.AddWithValue("@Symbol",                 pOrdem.Symbol);
                command.Parameters.AddWithValue("@OrderQty",               pOrdem.OrderQty);
                command.Parameters.AddWithValue("@Account",                pOrdem.Account);
                //command.Parameters.AddWithValue("@RegisterTime",           pOrdem.RegisterTime);
                command.Parameters.AddWithValue("@ExpireDate",             pOrdem.ExpireDate);
                command.Parameters.AddWithValue("@ExecutionTime",          pOrdem.ExecutionTime);
                command.Parameters.AddWithValue("@ReferencePrice",         pOrdem.ReferencePrice);
                command.Parameters.AddWithValue("@StartPriceValue",        pOrdem.StartPriceValue);
                command.Parameters.AddWithValue("@SendStartPrice",         pOrdem.SendStartPrice);
                command.Parameters.AddWithValue("@StopGainValuePrice",     pOrdem.StopGainValuePrice);
                command.Parameters.AddWithValue("@SendStopGainPrice",     pOrdem.SendStopGainPrice);
                command.Parameters.AddWithValue("@StopLossValuePrice",     pOrdem.StopLossValuePrice);
                command.Parameters.AddWithValue("@SendStopLossValuePrice", pOrdem.SendStopLossValuePrice);
                command.Parameters.AddWithValue("@InitialMovelPrice",      pOrdem.InitialMovelPrice);
                command.Parameters.AddWithValue("@AdjustmentMovelPrice",   pOrdem.AdjustmentMovelPrice);

                command.ExecuteNonQuery();

                int id_stopstart = int.Parse(command.Parameters["@StopStartID"].Value.ToString());

                return id_stopstart;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}{1}", "EnviarOrdemStop: ", ex.Message));
            }
            finally
            {
                conn.Close();
                conn.Dispose();

                command.Connection.Close();
                command.Dispose();
                command = null;
            }
        }
        #endregion

        #region AtualizaOrdemStop
        /// <summary>
        /// Atualiza stop start da ordem na tabela tbStopStartOrder
        /// </summary>
        /// <param name="id_startstop">Id do Stop/Start </param>
        /// <param name="id_stopstart_status">Id do status Stop/Start</param>
        /// <param name="preco_referencia">Preço referencia</param>
        public virtual void AtualizaOrdemStop(int id_startstop, int id_stopstart_status, decimal? preco_referencia)
        {
            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString                    = Conn;
            conn.Open();

            try
            {
                command.Connection  = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_atualiza_ordem_stop";

                command.Parameters.AddWithValue("@StopStartID", id_startstop);
                command.Parameters.AddWithValue("@StopStartStatusID", id_stopstart_status);
                command.Parameters.AddWithValue("@ReferencePrice", preco_referencia);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}{1}", "AtualizaOrdemStop: ", ex.Message));
            }
            finally
            {
                conn.Close();
                conn.Dispose();

                command.Connection.Close();
                command.Dispose();
                command = null;
            }
        }
        #endregion

        #region AtualizaOrdemStop
        /// <summary>
        /// Atualiza stop start da ordem na tabela tbStopStartOrder
        /// </summary>
        /// <param name="id_startstop">Id do Stop/Start</param>
        /// <param name="id_stopstart_status">Id do status Stop/Start</param>
        public virtual void AtualizaOrdemStop(int id_startstop, int id_stopstart_status)
        {
            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString                    = Conn;
            conn.Open();

            try
            {
                command.Connection  = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_atualiza_ordem_stop";

                command.Parameters.AddWithValue("@StopStartID", id_startstop);
                command.Parameters.AddWithValue("@StopStartStatusID", id_stopstart_status);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}{1}", "AtualizaOrdemStop: ", ex.Message));
            }
            finally
            {
                conn.Close();
                conn.Dispose();

                command.Connection.Close();
                command.Dispose();
                command = null;
            }
        }
        #endregion 

        #region CancelaOrdemStopStart
        /// <summary>
        /// Cancela Stop/Start da ordem
        /// </summary>
        /// <param name="id_startstop">Id do Stop/Start</param>
        /// <param name="id_stopstart_status">Id do Status do stop</param>
        public virtual void CancelaOrdemStopStart(int id_startstop, int id_stopstart_status)
        {
            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString                    = Conn;
            conn.Open();

            try
            {
                command.Connection  = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_cancela_ordem_stop";

                command.Parameters.AddWithValue("@StopStartID", id_startstop);
                command.Parameters.AddWithValue("@StopStartStatusID", id_stopstart_status);

                command.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
                conn.Dispose();

                command.Connection.Close();
                command.Dispose();
                command = null;
            }

        }
        #endregion

        #region ListarOrdemStopStartResumido
        /// <summary>
        /// Seleciona o Stop/Start
        /// </summary>
        /// <param name="id_startstop">Id do Stop/Start</param>
        /// <returns>Retorna uma entidade do tipo AutomacaoOrdensInfo</returns>
        public virtual AutomacaoOrdensInfo ListarOrdemStopStartResumido(int id_startstop)
        {
            SqlCommand command = new SqlCommand();

            AutomacaoOrdensInfo _TOrdem = new AutomacaoOrdensInfo();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString                    = Conn;
            conn.Open();

            try
            {
                command.Connection  = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_sel_stopstart";

                command.Parameters.AddWithValue("@StopStartID", id_startstop);

                DataTable dtDados = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dtDados);

                if (dtDados.Rows.Count > 0)
                {
                    _TOrdem.Account = int.Parse(dtDados.Rows[0]["Account"].ToString());
                    _TOrdem.StopStartID = int.Parse(dtDados.Rows[0]["StopStartID"].ToString());
                    _TOrdem.Symbol = dtDados.Rows[0]["Symbol"].ToString();
                    //_TOrdem.Ativo       = char.Parse(dtDados.Rows[0]["cd_instrumento"].ToString());

                }

                return _TOrdem;

            }
            finally
            {
                conn.Close();
                conn.Dispose();

                command.Connection.Close();
                command.Dispose();
                command = null;
            }

        }
        #endregion

    }
}