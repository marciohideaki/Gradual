using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using log4net;

namespace Gradual.OMS.Cotacao
{
    public class DCotacoes
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Dictionary<string, string> ObterHashDadosCotacao()
        {
            Dictionary<string, string> retorno = new Dictionary<string, string>();

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["OMS"]);

            try
            {
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_TB_ATIVO_sel";

                DataTable dtPapel = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter(Command);

                adapter.Fill(dtPapel);

                if (dtPapel.Rows.Count > 0)
                {

                    for (int i = 0; i <= dtPapel.Rows.Count - 1; i++)
                    {
                        string Instrumento = dtPapel.Rows[i]["cd_negociacao"].ToString();
                        string Mensagem = dtPapel.Rows[i]["Mensagem"].ToString();
                        //Mensagem += dtPapel.Rows[i]["id_ativo_tipo"].ToString();

                        if ( !retorno.ContainsKey(Instrumento) )
                            retorno.Add(Instrumento, Mensagem);
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.Error("ObterHashDadosCotacao(): " + ex.Message + "-" + ex.ErrorCode, ex);
            }
            catch (Exception ex)
            {
                logger.Error("ObterHashDadosCotacao(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return retorno;

        }

        /// <summary>
        /// ObterDadosOpcoes - busca dados do exercicio para opcoes a vencer
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ObterHashDadosOpcoes()
        {
            Dictionary<string, string> retorno = new Dictionary<string, string>();

            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.AppSettings["MDS"]);

                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_TBCADASTROPAPEL_opcoes_sel";

                SqlDataReader dtOpcoes = Command.ExecuteReader();

                if (dtOpcoes.HasRows)
                {
                    while (dtOpcoes.Read())
                    {
                        string Instrumento = dtOpcoes.GetSqlString(0).Value;
                        string Indicador = dtOpcoes.GetSqlString(1).Value;
                        string PrecoExerc = dtOpcoes.GetSqlString(2).Value;
                        DateTime DataVencimento = dtOpcoes.GetSqlDateTime(3).Value;

                        string Mensagem = Indicador + PrecoExerc + DataVencimento.ToString("yyyyMMdd");

                        retorno.Add(Instrumento, Mensagem);
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.Error("ObterHashOpcoes(): " + ex.Message + "-" + ex.ErrorCode, ex);
            }
            catch (Exception ex)
            {
                logger.Error("ObterHashOpcoes(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return retorno;
        }

        /// <summary>
        /// ObterDadosIndice - busca dados de índice
        /// </summary>
        /// <returns></returns>
        public List<ComposicaoIndice.ItemIndice> ObterDadosIndice()
        {
            List<ComposicaoIndice.ItemIndice> listaIndice = new List<ComposicaoIndice.ItemIndice>();

            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.AppSettings["OMS"]);

                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_TB_ATIVO_indices_sel";

                SqlDataReader dtDados = Command.ExecuteReader();

                if (dtDados.HasRows)
                {
                    while (dtDados.Read())
                    {
                        ComposicaoIndice.ItemIndice item = new ComposicaoIndice.ItemIndice();
                        item.indice = dtDados.GetSqlString(0).Value;
                        item.codigoIndice = dtDados.IsDBNull(1) ? 0 : dtDados.GetInt32(1);
                        item.fechamento = dtDados.IsDBNull(2) ? 0 : dtDados.GetSqlDecimal(2).ToDouble();
                        item.oscilacao = dtDados.IsDBNull(3) ? 0 : dtDados.GetSqlDecimal(3).ToDouble();
                        item.dataCotacao = dtDados.IsDBNull(4) ? new DateTime(1970, 1, 1) : dtDados.GetDateTime(4);
                        listaIndice.Add(item);
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.Error("ObterDadosIndice(): " + ex.Message + "-" + ex.ErrorCode, ex);
            }
            catch (Exception ex)
            {
                logger.Error("ObterDadosIndice(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return listaIndice;
        }

        /// <summary>
        /// ObterDadosComposicaoIndice - busca dados de composição de índice
        /// </summary>
        /// <returns></returns>
        public List<ComposicaoIndice.ItemComposicaoIndice> ObterDadosComposicaoIndice()
        {
            List<ComposicaoIndice.ItemComposicaoIndice> listaComposicaoIndice = new List<ComposicaoIndice.ItemComposicaoIndice>();

            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConfigurationManager.AppSettings["OMS"]);
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_TB_COMPOSICAO_INDICE_sel";

                SqlDataReader dtDados = Command.ExecuteReader();

                if (dtDados.HasRows)
                {
                    while (dtDados.Read())
                    {
                        ComposicaoIndice.ItemComposicaoIndice item = new ComposicaoIndice.ItemComposicaoIndice();
                        item.indice = dtDados.GetSqlString(0).Value;
                        item.papel = dtDados.GetSqlString(1).Value;
                        item.qtdeTeorica = dtDados.GetSqlDecimal(2).ToDouble();
                        item.fechamento = dtDados.GetSqlDecimal(3).ToDouble();
                        ObterCotacaoAtual(item);
                        listaComposicaoIndice.Add(item);
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.Error("ObterDadosComposicaoIndice(): " + ex.Message + "-" + ex.ErrorCode, ex);
            }
            catch (Exception ex)
            {
                logger.Error("ObterDadosComposicaoIndice(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return listaComposicaoIndice;
        }

        /// <summary>
        /// ObterCotacaoAtual - busca cotação atual de um papel.
        /// </summary>
        /// <returns></returns>
        public void ObterCotacaoAtual(ComposicaoIndice.ItemComposicaoIndice item)
        {
            SqlConnection conn = null;
            string cmd = "";

            try
            {
                cmd = "SELECT vl_ultima, dt_negocio FROM tb_ativo_cotacao ";
                cmd += " WHERE id_ativo ='" + item.papel + "'";

                conn = new SqlConnection(ConfigurationManager.AppSettings["DirectTradeRisco"]);
                conn.Open();

                SqlCommand command = new SqlCommand(cmd, conn);
                SqlDataReader dtDados = command.ExecuteReader();
                if (dtDados.HasRows)
                {
                    if (dtDados.Read())
                    {
                        item.valor = dtDados.IsDBNull(0) ? 0 : dtDados.GetSqlDecimal(0).ToDouble();
                        item.dataCotacao = dtDados.IsDBNull(1) ? DateTime.Now : dtDados.GetDateTime(1);
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.Error("ObterCotacaoAtual(): " + ex.Message + "-" + ex.ErrorCode, ex);
            }
            catch (Exception ex)
            {
                logger.Error("ObterCotacaoAtual(): " + ex.Message, ex);
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

        /// <summary>
        /// AtualizarTbAtivo - atualiza item na tabela TB_ATIVO.
        /// </summary>
        /// <returns></returns>
        public void AtualizarTbAtivo(ComposicaoIndice.ItemIndice item)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConfigurationManager.AppSettings["OMS"]);
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_TB_ATIVO_upd";

                Command.Parameters.AddWithValue("@id_Ativo", item.codigoIndice);
                Command.Parameters.AddWithValue("@vl_fechamento", item.valor);

                Command.ExecuteNonQuery();
                Command.Dispose();
            }

            catch (SqlException ex)
            {
                logger.Error("AtualizarTbAtivo(): " + ex.Message + "-" + ex.ErrorCode, ex);
            }
            catch (Exception ex)
            {
                logger.Error("AtualizarTbAtivo(): " + ex.Message, ex);
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

        /// <summary>
        /// AtualizarTbAtivo - atualiza item na tabela TB_ATIVO.
        /// </summary>
        /// <returns></returns>
        public void InserirTbHistoricoAtivo(ComposicaoIndice.ItemIndice item)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConfigurationManager.AppSettings["OMS"]);
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_TB_HISTORICO_INDICE_ins";

                Command.Parameters.AddWithValue("@id_indice", item.codigoIndice);
                Command.Parameters.AddWithValue("@dt_cotacao", item.dataCotacao.Date);
                Command.Parameters.AddWithValue("@vl_fechamento", item.valor);
                Command.Parameters.AddWithValue("@vl_oscilacao", item.oscilacao);

                Command.ExecuteNonQuery();
                Command.Dispose();
            }

            catch (SqlException ex)
            {
                logger.Error("InserirTbHistoricoAtivo(): " + ex.Message + "-" + ex.ErrorCode, ex);
            }
            catch (Exception ex)
            {
                logger.Error("InserirTbHistoricoAtivo(): " + ex.Message, ex);
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

        /// <summary>
        /// ObterListaIndicesGradual - busca lista de índices Gradual
        /// </summary>
        /// <returns></returns>
        public List<IndiceGradual.ItemIndice> ObterListaIndicesGradual()
        {
            List<IndiceGradual.ItemIndice> listaIndice = new List<IndiceGradual.ItemIndice>();
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConfigurationManager.AppSettings["OMSProducao"]);
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_tbCotacaoIndice_lst";

                SqlDataReader dtDados = Command.ExecuteReader();

                if (dtDados.HasRows)
                {
                    while (dtDados.Read())
                    {
                        IndiceGradual.ItemIndice item = new IndiceGradual.ItemIndice();
                        item.indice = dtDados.GetSqlString(1).Value;
                        item.codigoIndice = dtDados.IsDBNull(0) ? 0 : dtDados.GetInt32(0);
                        item.cotacaoAtual = dtDados.IsDBNull(2) ? 0 : dtDados.GetSqlDecimal(2).ToDouble();
                        item.fechamentoAnterior = dtDados.IsDBNull(3) ? 0 : dtDados.GetSqlDecimal(3).ToDouble();
                        item.variacao = dtDados.IsDBNull(4) ? 0 : dtDados.GetSqlDecimal(4).ToDouble();
                        item.dataCotacao = dtDados.IsDBNull(5) ? new DateTime(1970, 1, 1) : dtDados.GetDateTime(5);
                        listaIndice.Add(item);
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.Error("ObterListaIndicesGradual(): " + ex.Message + "-" + ex.ErrorCode, ex);
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaIndicesGradual(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return listaIndice;
        }

        /// <summary>
        /// ObterListaIndicesGradual - busca lista de índices Gradual
        /// </summary>
        /// <returns></returns>
        public List<IndiceGradual.ItemComposicaoIndice> ObterListaComposicaoIndiceGradual(int idIndice)
        {
            List<IndiceGradual.ItemComposicaoIndice> listaComposicaoIndice = new List<IndiceGradual.ItemComposicaoIndice>();
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConfigurationManager.AppSettings["OMSProducao"]);
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_tbIndiceGradual_lst";

                Command.Parameters.AddWithValue("@idIndice", idIndice);

                SqlDataReader dtDados = Command.ExecuteReader();

                if (dtDados.HasRows)
                {
                    while (dtDados.Read())
                    {
                        IndiceGradual.ItemComposicaoIndice item = new IndiceGradual.ItemComposicaoIndice();
                        item.ativo = dtDados.GetSqlString(0).Value;
                        item.qtdeTeorica = dtDados.IsDBNull(1) ? 0 : dtDados.GetSqlDecimal(1).ToDouble();
                        item.dataCotacao = dtDados.IsDBNull(2) ? new DateTime(1970, 1, 1) : dtDados.GetDateTime(2);
                        listaComposicaoIndice.Add(item);
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.Error("ObterListaComposicaoIndiceGradual(): " + ex.Message + "-" + ex.ErrorCode, ex);
            }
            catch (Exception ex)
            {
                logger.Error("ObterListaComposicaoIndiceGradual(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return listaComposicaoIndice;
        }

        /// <summary>
        /// AtualizarComposicaoIndiceGradual - atualiza item na tabela tbIndiceGradual.
        /// </summary>
        /// <returns></returns>
        public void AtualizarComposicaoIndiceGradual(string indice, IndiceGradual.ItemComposicaoIndice item, bool atualizarQtdTeorica)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConfigurationManager.AppSettings["OMSProducao"]);
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_tbIndiceGradual_upd";

                Command.Parameters.AddWithValue("@dsIndice", indice);
                Command.Parameters.AddWithValue("@idAtivo", item.ativo);
                Command.Parameters.AddWithValue("@Cotacao", item.cotacao);
                Command.Parameters.AddWithValue("@Variacao", item.variacao);
                Command.Parameters.AddWithValue("@QtdeAjustada", item.qtdeAjustada);
                if (atualizarQtdTeorica)
                    Command.Parameters.AddWithValue("@QtdeTeorica", item.qtdeTeorica);
                else
                    Command.Parameters.AddWithValue("@QtdeTeorica", null);
                Command.Parameters.AddWithValue("@dtCotacao", item.dataCotacao);

                Command.ExecuteNonQuery();
                Command.Dispose();
            }

            catch (SqlException ex)
            {
                logger.Error("AtualizarComposicaoIndiceGradual(): " + ex.Message + "-" + ex.ErrorCode, ex);
            }
            catch (Exception ex)
            {
                logger.Error("AtualizarComposicaoIndiceGradual(): " + ex.Message, ex);
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

        /// <summary>
        /// AtualizarComposicaoIndiceGradual - atualiza item na tabela tbIndiceGradual.
        /// </summary>
        /// <returns></returns>
        public void AtualizarCotacaoIndice(IndiceGradual.ItemIndice item)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConfigurationManager.AppSettings["OMSProducao"]);
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_tbCotacaoIndice_ins";

                Command.Parameters.AddWithValue("@dsIndice", item.indice);
                Command.Parameters.AddWithValue("@CotacaoAtual", item.cotacaoAtual);
                Command.Parameters.AddWithValue("@FechAnterior", item.fechamentoAnterior);
                Command.Parameters.AddWithValue("@Variacao", item.variacao);
                Command.Parameters.AddWithValue("@dtCotacao", item.dataCotacao);

                Command.ExecuteNonQuery();
                Command.Dispose();
            }

            catch (SqlException ex)
            {
                logger.Error("AtualizarCotacaoIndice(): " + ex.Message + "-" + ex.ErrorCode, ex);
            }
            catch (Exception ex)
            {
                logger.Error("AtualizarCotacaoIndice(): " + ex.Message, ex);
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
