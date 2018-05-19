using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using Gradual.OMS.Ordens.StartStop.Lib.Enum;
using Gradual.OMS.Ordens.StartStop.Geral;
using log4net;

namespace Gradual.OMS.Ordens.StartStop.Lib
{
    public class AutomacaoOrdensDados
    {
        #region Propriedades
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                command.CommandText = "PRC_INS_ORDEM_STOP_OMS";

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
                command.Parameters.AddWithValue("@SendStopGainPrice",      pOrdem.SendStopGainPrice);
                command.Parameters.AddWithValue("@StopLossValuePrice",     pOrdem.StopLossValuePrice);
                command.Parameters.AddWithValue("@SendStopLossValuePrice", pOrdem.SendStopLossValuePrice);
                command.Parameters.AddWithValue("@InitialMovelPrice",      pOrdem.InitialMovelPrice);
                command.Parameters.AddWithValue("@AdjustmentMovelPrice",   pOrdem.AdjustmentMovelPrice);
                command.Parameters.AddWithValue("@StopStartTipoEnum",   (int) pOrdem.IdStopStartTipo);
                command.Parameters.AddWithValue("@PortaControle", pOrdem.ControlePorta);

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
                logger.Error("AtualizaOrdemStop(): " + ex.Message, ex);
                throw new Exception(string.Format("{0}{1}", "AtualizaOrdemStop: ", ex.Message),ex);
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
        public virtual void AtualizaOrdemStop(int id_startstop, int id_stopstart_status, string ds_critica = null)
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

                command.Parameters.AddWithValue("@StopStartID",       id_startstop);
                command.Parameters.AddWithValue("@StopStartStatusID", id_stopstart_status);
                command.Parameters.AddWithValue("@Critica",           ds_critica);

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
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}{1}", "CancelaOrdemStopStart: ", ex.Message));
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
        #region VerificaOrdemEmDataExpiracao
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<int, string> VerificaOrdemEmDataExpiracao()
        {
            SqlCommand command = new SqlCommand();

            Dictionary<int, string> lRetorno = new Dictionary<int, string>();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection  = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_sel_stopstart_para_expirar";

                DataTable dtDados = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dtDados);

                foreach(DataRow row in dtDados.Rows)
                    lRetorno.Add(Convert.ToInt32(row["StopStartID"]), row["Symbol"].ToString());

                return lRetorno;
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
                    _TOrdem.ControlePorta = dtDados.Rows[0]["PortaControle"].ToString();

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

        #region ListarOrdemStopStartNaoEnviadasMDS
        public virtual List<AutomacaoOrdensInfo> ListarOrdemStopStartNaoEnviadasMDS()
        {
            SqlCommand command = new SqlCommand();

            AutomacaoOrdensInfo _TOrdem = new AutomacaoOrdensInfo();

            List<AutomacaoOrdensInfo> _ListOrdem = new List<AutomacaoOrdensInfo>();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_sel_stopstart_nao_enviadas_mds";

                //command.Parameters.AddWithValue("@StopStartID", id_startstop);

                DataTable dtDados = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dtDados);

                if (dtDados.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDados.Rows.Count-1; i++)
                    {
                        _TOrdem                        = new AutomacaoOrdensInfo();
                        _TOrdem.Account                = int.Parse(dtDados.Rows[i]["Account"].ToString());
                        _TOrdem.StopStartID            = int.Parse(dtDados.Rows[i]["StopStartID"].ToString());
                        _TOrdem.Symbol                 = dtDados.Rows[i]["Symbol"].ToString();
                        _TOrdem.IdStopStartTipo        = (StopStartTipoEnum) dtDados.Rows[i]["StopStartTipoEnum"];
                        _TOrdem.OrdTypeID              = int.Parse(dtDados.Rows[i]["OrdTypeID"].ToString());
                        _TOrdem.IdBolsa                = Conversao.ToInt(dtDados.Rows[i]["id_Bolsa"].ToString());
                        _TOrdem.InitialMovelPrice      = Conversao.ToDecimal(dtDados.Rows[i]["InitialMovelPrice"].ToString());
                        _TOrdem.OrderQty               = int.Parse(dtDados.Rows[i]["OrderQty"].ToString());
                        _TOrdem.ReferencePrice         = Conversao.ToDecimal(dtDados.Rows[i]["ReferencePrice"]);
                        _TOrdem.SendStartPrice         = Conversao.ToDecimal(dtDados.Rows[i]["SendStartPrice"]);
                        _TOrdem.SendStopGainPrice      = Conversao.ToDecimal(dtDados.Rows[i]["SendStopGainPrice"]);
                        _TOrdem.SendStopLossValuePrice = Conversao.ToDecimal(dtDados.Rows[i]["SendStopLossValuePrice"]);
                        _TOrdem.StartPriceValue        = Conversao.ToDecimal(dtDados.Rows[i]["StartPriceValue"]);
                        _TOrdem.StopGainValuePrice     = Conversao.ToDecimal(dtDados.Rows[i]["StopGainValuePrice"]);
                        _TOrdem.StopLossValuePrice     = Conversao.ToDecimal(dtDados.Rows[i]["StopLossValuePrice"]);
                        _TOrdem.StopStartStatusID      = int.Parse(dtDados.Rows[i]["StopStartStatusID"].ToString());
                        _TOrdem.RegisterTime           = Conversao.ToDateTime(dtDados.Rows[i]["RegisterTime"]);
                        _TOrdem.ExecutionTime          = Conversao.ToDateTime(dtDados.Rows[i]["ExecutionTime"]);
                        _TOrdem.ExpireDate             = Conversao.ToDateTime(dtDados.Rows[i]["ExpireDate"]); 

                        _ListOrdem.Add(_TOrdem);
                    }
                }

                return _ListOrdem;

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

        #region SelecionaOrdemStopStartResumido
        public virtual AutomacaoOrdensInfo SelecionaOrdemStopStart(int pIdStartStop)
        {
            SqlCommand command = new SqlCommand();

            AutomacaoOrdensInfo _TOrdem = new AutomacaoOrdensInfo();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_sel_stopstart";

                command.Parameters.AddWithValue("@StopStartID", pIdStartStop);

                DataTable dtDados = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dtDados);

                if (dtDados.Rows.Count > 0)
                {
                    _TOrdem                        = new AutomacaoOrdensInfo();
                    _TOrdem.Account                = int.Parse(dtDados.Rows[0]["Account"].ToString());
                    _TOrdem.StopStartID            = int.Parse(dtDados.Rows[0]["StopStartID"].ToString());
                    _TOrdem.Symbol                 = dtDados.Rows[0]["Symbol"].ToString();
                    _TOrdem.IdStopStartTipo        = (StopStartTipoEnum)dtDados.Rows[0]["StopStartTipoEnum"];
                    _TOrdem.OrdTypeID              = int.Parse(dtDados.Rows[0]["OrdTypeID"].ToString());
                    _TOrdem.IdBolsa                = Conversao.ToInt(dtDados.Rows[0]["id_Bolsa"].ToString());
                    _TOrdem.InitialMovelPrice      = Conversao.ToDecimal(dtDados.Rows[0]["InitialMovelPrice"]);
                    _TOrdem.OrderQty               = int.Parse(dtDados.Rows[0]["OrderQty"].ToString());
                    _TOrdem.ReferencePrice         = Conversao.ToDecimal(dtDados.Rows[0]["ReferencePrice"]);
                    _TOrdem.SendStartPrice         = Conversao.ToDecimal(dtDados.Rows[0]["SendStartPrice"]);
                    _TOrdem.SendStopGainPrice      = Conversao.ToDecimal(dtDados.Rows[0]["SendStopGainPrice"]);
                    _TOrdem.SendStopLossValuePrice = Conversao.ToDecimal(dtDados.Rows[0]["SendStopLossValuePrice"]);
                    _TOrdem.StartPriceValue        = Conversao.ToDecimal(dtDados.Rows[0]["StartPriceValue"]);
                    _TOrdem.StopGainValuePrice     = Conversao.ToDecimal(dtDados.Rows[0]["StopGainValuePrice"]);
                    _TOrdem.StopLossValuePrice     = Conversao.ToDecimal(dtDados.Rows[0]["StopLossValuePrice"]);
                    _TOrdem.StopStartStatusID      = int.Parse(dtDados.Rows[0]["StopStartStatusID"].ToString());
                    _TOrdem.RegisterTime           = Conversao.ToDateTime(dtDados.Rows[0]["RegisterTime"]);
                    _TOrdem.ExecutionTime          = Conversao.ToDateTime(dtDados.Rows[0]["ExecutionTime"]);
                    _TOrdem.ExpireDate             = Conversao.ToDateTime(dtDados.Rows[0]["ExpireDate"]);
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