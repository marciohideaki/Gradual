using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using log4net;
using Gradual.OMS.Alertas.Lib.Dados;

namespace Gradual.OMS.Alertas
{
    class GdbAlertas
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string chaveAlerta = "00000000000000000000";

        public String CadastrarAlerta(
            string idCliente, 
            string instrumento, 
            Operando tipoOperando, 
            Operador tipoOperador, 
            Decimal valor,
            DateTime dataHoraCadastro)
        {
            IncrementarChaveAlerta();

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Alertas"].ConnectionString);

            String result = null;

            try
            {
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_TB_ALERTAS_ins";
                Command.Parameters.Add(new SqlParameter("@IdAlerta", chaveAlerta));
                Command.Parameters.Add(new SqlParameter("@IdCliente", idCliente));
                Command.Parameters.Add(new SqlParameter("@Instrumento", instrumento));
                Command.Parameters.Add(new SqlParameter("@TipoOperando", (int)tipoOperando));
                Command.Parameters.Add(new SqlParameter("@TipoOperador", (int)tipoOperador));
                Command.Parameters.Add(new SqlParameter("@Valor", valor));
                //Command.Parameters.Add(new SqlParameter("@Atingido", "0"));
                //Command.Parameters.Add(new SqlParameter("@Exibido", "0"));
                Command.Parameters.Add(new SqlParameter("@DataCadastro", dataHoraCadastro));
                //Command.Parameters.Add(new SqlParameter("@DataAtingimento", null));

                int retornoSp = Command.ExecuteNonQuery();

                result = chaveAlerta;
            }
            catch (SqlException ex)
            {
                logger.Error("CadastrarAlertas(): " + ex.Message);
                logger.Error(ex.StackTrace);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return result;
        }

        private void IncrementarChaveAlerta()
        {
            lock (chaveAlerta)
            {

                String dataHoraAtual = DateTime.Now.ToString(string.Format("{0}", "yyyyMMddHHmmss"));
                String sequenciaAtualizada;

                if (chaveAlerta == null)
                    sequenciaAtualizada = String.Format("{0, 6:000000}", 1);
                else
                {
                    String dataHoraChave = chaveAlerta.Substring(0, 14);
                    if (dataHoraChave.Equals(dataHoraAtual))
                        sequenciaAtualizada =
                            String.Format("{0, 6:000000}",
                                (Convert.ToInt32(chaveAlerta.Substring(14)) + 1));
                    else
                        sequenciaAtualizada = String.Format("{0, 6:000000}", 1);

                }
                chaveAlerta = dataHoraAtual + sequenciaAtualizada;
            }
        }

        public Dictionary<String, DadosAlerta> ListarAlertas()
        {
            Dictionary<String, DadosAlerta> dadosRetorno = new Dictionary<String, DadosAlerta>();

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Alertas"].ConnectionString);
            SqlDataReader rdr = null;

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conn;
                command.CommandText = "prc_TB_ALERTAS_lst";

                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    String idAlertaDb = (String)rdr["IdAlerta"];

                    DadosAlerta registro = new DadosAlerta();
                    registro.IdAlerta = idAlertaDb;
                    registro.IdCliente = (String)rdr["IdCliente"];
                    registro.Instrumento = (String)rdr["Instrumento"];
                    registro.TipoOperando = (Operando)rdr["TipoOperando"];
                    registro.TipoOperador = (Operador)rdr["TipoOPerador"];
                    registro.Valor = (Decimal)rdr["Valor"];
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Atingido")))
                        registro.Atingido = ((((String)rdr["Atingido"]).ElementAt(0) == '0') ? false : true);
                    else
                        registro.Atingido = false;
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Exibido")))
                        registro.Exibido = ((((String)rdr["Exibido"]).ElementAt(0) == '0') ? false : true);
                    else
                        registro.Exibido = false;
                    if (!rdr.IsDBNull(rdr.GetOrdinal("DataCadastro")))
                        registro.DataCadastro = (DateTime)rdr["DataCadastro"];
                    if (!rdr.IsDBNull(rdr.GetOrdinal("DataAtingimento")))
                        registro.DataAtingimento = (DateTime)rdr["DataAtingimento"];
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Cotacao")))
                        registro.Cotacao = (Decimal)rdr["Cotacao"];

                    dadosRetorno.Add(idAlertaDb, registro);
                }
            }
            catch (SqlException ex)
            {
                logger.Error("CadastrarAlertas(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return dadosRetorno;
        }

        public void ExcluirAlerta(String idAlerta)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Alertas"].ConnectionString);

            try
            {
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_TB_ALERTAS_del";
                Command.Parameters.Add(new SqlParameter("@id_alerta", idAlerta));

                int retornoSp = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                logger.Error("ExcluirAlerta(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public void AtualizarAlertaAtingido(String idAlerta, DateTime dataAtingimento, Decimal cotacao)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Alertas"].ConnectionString);

            try
            {
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_TB_ALERTAS_upd";
                Command.Parameters.Add(new SqlParameter("@IdAlerta", idAlerta));
                Command.Parameters.Add(new SqlParameter("@Atingido", '1'));
                Command.Parameters.Add(new SqlParameter("@DataAtingimento", dataAtingimento));
                Command.Parameters.Add(new SqlParameter("@Cotacao", cotacao));

                int retornoSp = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                logger.Error("AtualizarAlertaAtingido(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public void AtualizarAlertaExibido(String idAlerta)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Alertas"].ConnectionString);

            try
            {
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_TB_ALERTAS_upd";
                Command.Parameters.Add(new SqlParameter("@IdAlerta", idAlerta));
                Command.Parameters.Add(new SqlParameter("@Exibido", '1'));

                int retornoSp = Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                logger.Error("AtualizarAlertaExibido(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

    }
}
