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
        private string Conn = ConfigurationSettings.AppSettings["OMS"].ToString();

        public virtual int EnviarOrdemStop(AutomacaoOrdensInfo pOrdem)
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
                command.Parameters.AddWithValue("@id_mercado_tipo", pOrdem.IdMercadoTipo);
                command.Parameters.AddWithValue("@id_stopstart_status", pOrdem.IdStopStart);
                command.Parameters.AddWithValue("@id_bolsa", pOrdem.IdBolsa);
                command.Parameters.AddWithValue("@id_stopstart_tipo", pOrdem.IdStopStartTipo);
                command.Parameters.AddWithValue("@cd_cliente_cblc", pOrdem.IdCliente);
                command.Parameters.AddWithValue("@cd_instrumento", pOrdem.Instrumento);
                command.Parameters.AddWithValue("@dt_validade", pOrdem.DataValidade);
                command.Parameters.AddWithValue("@vl_quantidade", pOrdem.Quantidade);
                command.Parameters.AddWithValue("@vl_preco_gain", pOrdem.PrecoGain);
                command.Parameters.AddWithValue("@vl_preco_startgain", pOrdem.PrecoEnvioGain);
                command.Parameters.AddWithValue("@vl_preco_loss", pOrdem.PrecoLoss);
                command.Parameters.AddWithValue("@vl_preco_envioloss", pOrdem.PrecoEnvioLoss);
                command.Parameters.AddWithValue("@vl_inicio_movel", pOrdem.ValorInicioMovel);
                command.Parameters.AddWithValue("@vl_ajuste_movel", pOrdem.ValorAjusteMovel);
                command.Parameters.AddWithValue("@vl_preco_start", pOrdem.PrecoStart);
                command.Parameters.AddWithValue("@vl_preco_enviostart", pOrdem.PrecoEnvioStart);

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

        public virtual AutomacaoOrdensInfo ListarOrdemStopStartResumido(int id_startstop)
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

                command.Parameters.AddWithValue("@id_startstop", id_startstop);

                DataTable dtDados = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dtDados);

                if (dtDados.Rows.Count > 0)
                {
                    _TOrdem.IdCliente = int.Parse(dtDados.Rows[0]["cd_cliente_cblc"].ToString());
                    _TOrdem.IdStopStart = int.Parse(dtDados.Rows[0]["id_startstop"].ToString());
                    _TOrdem.Instrumento = dtDados.Rows[0]["cd_instrumento"].ToString();
                    _TOrdem.Ativo = char.Parse(dtDados.Rows[0]["cd_instrumento"].ToString());

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