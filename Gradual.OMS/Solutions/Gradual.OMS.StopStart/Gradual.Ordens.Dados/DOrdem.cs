using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Ordens.Template;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;

namespace Gradual.OMS.Contratos.Ordens.Automacao.Dados
{
    public class AutomacaoOrdemInfo
    { 
        private string Conn = ConfigurationSettings.AppSettings["OMS"].ToString();

        public virtual int EnviarOrdemStop(TOrdem pOrdem)
        {
            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "PRC_INS_ORDEM_STOP";

                SqlParameter OutParameter = new SqlParameter();
                OutParameter.Direction = ParameterDirection.Output;
                OutParameter.ParameterName = "@id_startstop";
                OutParameter.Size = 32;

                command.Parameters.Add(OutParameter);
                command.Parameters.AddWithValue("@id_mercado_tipo", pOrdem.id_mercado_tipo);
                command.Parameters.AddWithValue("@id_stopstart_status", pOrdem.id_stopstart_status);
                command.Parameters.AddWithValue("@id_bolsa", pOrdem.id_bolsa);
                command.Parameters.AddWithValue("@id_stopstart_tipo", pOrdem.id_stopstart_tipo);
                command.Parameters.AddWithValue("@cd_cliente_cblc", pOrdem.id_cliente);
                command.Parameters.AddWithValue("@cd_instrumento", pOrdem.instrumento);
                command.Parameters.AddWithValue("@dt_validade", pOrdem.data_validade);
                command.Parameters.AddWithValue("@vl_quantidade", pOrdem.quantidade);
                command.Parameters.AddWithValue("@vl_preco_gain", pOrdem.preco_gain);
                command.Parameters.AddWithValue("@vl_preco_startgain", pOrdem.preco_envio_gain);
                command.Parameters.AddWithValue("@vl_preco_loss", pOrdem.preco_loss);
                command.Parameters.AddWithValue("@vl_preco_envioloss", pOrdem.preco_envio_loss);
                command.Parameters.AddWithValue("@vl_inicio_movel", pOrdem.valor_inicio_movel);
                command.Parameters.AddWithValue("@vl_ajuste_movel", pOrdem.valor_ajuste_movel);
                command.Parameters.AddWithValue("@vl_preco_start", pOrdem.preco_start);
                command.Parameters.AddWithValue("@vl_preco_enviostart", pOrdem.preco_envio_start);

                command.ExecuteNonQuery();

                int id_stopstart = int.Parse(command.Parameters["@id_startstop"].Value.ToString());

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

        public virtual void AtualizaOrdemStop(int id_startstop, int id_stopstart_status, decimal? preco_referencia)
        {
            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_atualiza_ordem_stop";

                command.Parameters.AddWithValue("@id_startstop", id_startstop);
                command.Parameters.AddWithValue("@id_stopstart_status", id_stopstart_status);
                command.Parameters.AddWithValue("@preco_referencia", preco_referencia);

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

        public virtual void AtualizaOrdemStop(int id_startstop, int id_stopstart_status)
        {
            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_atualiza_ordem_stop";

                command.Parameters.AddWithValue("@id_startstop", id_startstop);
                command.Parameters.AddWithValue("@id_stopstart_status", id_stopstart_status);
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

        public virtual void CancelaOrdemStopStart(int id_startstop, int id_stopstart_status)
        {
            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_cancela_ordem_stop";

                command.Parameters.AddWithValue("@id_startstop", id_startstop);
                command.Parameters.AddWithValue("@id_stopstart_status", id_stopstart_status);

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

        public virtual TOrdem ListarOrdemStopStartResumido(int id_startstop)
        {
            SqlCommand command = new SqlCommand();

            TOrdem _TOrdem = new TOrdem();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_sel_stopstart";

                command.Parameters.AddWithValue("@id_startstop", id_startstop);

                DataTable dtDados = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dtDados);

                if (dtDados.Rows.Count > 0)
                {
                    _TOrdem.id_cliente = int.Parse(dtDados.Rows[0]["cd_cliente_cblc"].ToString());
                    _TOrdem.id_stopstart = int.Parse(dtDados.Rows[0]["id_startstop"].ToString());
                    _TOrdem.instrumento = dtDados.Rows[0]["cd_instrumento"].ToString();
                    _TOrdem.st_ativo = char.Parse(dtDados.Rows[0]["cd_instrumento"].ToString());

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

    }


}