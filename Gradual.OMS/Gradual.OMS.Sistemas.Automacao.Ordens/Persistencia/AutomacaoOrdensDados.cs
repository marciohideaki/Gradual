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
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.OMS.Sistemas.Automacao.Ordens;
using Gradual.OMS.Library;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Sistemas.Automacao.Ordens.Persistencia;
using Gradual.OMS.Contratos.Ordens.Dados;


namespace Gradual.OMS.Sistemas.Automacao.Persistencia
{
    public class AutomacaoOrdensDados
    { 
        private ServicoAutomacaoOrdensConfig _config = GerenciadorConfig.ReceberConfig<ServicoAutomacaoOrdensConfig>();
        private string Conn, ConnMDS = string.Empty;
        
        public AutomacaoOrdensDados()
        {
            Conn = _config.ConnectionString;
            ConnMDS = _config.ConnectionStringMDS;
        }

        public void ExcluirUsuarioLogado(int idCliente, int idSistema)
        {
            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = ConnMDS;
            conn.Open();
            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_del_mds_authentication";
                command.Parameters.AddWithValue("@idCliente", idCliente);
                command.Parameters.AddWithValue("@idSistema", idSistema);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex);
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

        public string EnviarOrdemStop(AutomacaoOrdensInfo pOrdem)
        {
            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();
            Guid guid = Guid.NewGuid();
            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "PRC_INS_ORDEM_STOP";


                command.Parameters.AddWithValue("@id_startstop", guid);
                command.Parameters.AddWithValue("@id_stopstart_status", pOrdem.IdStopstartStatus);
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
                command.Parameters.AddWithValue("@stPrazoExecucao", pOrdem.PrazoValidade);
                command.ExecuteNonQuery();

                string id_stopstart = guid.ToString();

                return id_stopstart;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex);
                return null;
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

        public AtualizaOrdemStartStopResponse AtualizaOrdemStop(AtualizaOrdemStartStopRequest req)
        {
            AtualizaOrdemStartStopResponse res = new AtualizaOrdemStartStopResponse();
            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_atualiza_ordem_stop";

                command.Parameters.AddWithValue("@id_startstop", req.IdStartStop);
                command.Parameters.AddWithValue("@id_stopstart_status", req.IdStopStartStatus);
                if (req.PrecoReferencia != 0)
                {
                    command.Parameters.AddWithValue("@preco_referencia", req.PrecoReferencia);
                }
                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.Read())
                {
                    res.CodigoBolsa = dr["ds_bolsa"].ToString();
                    res.CodigoCliente = dr["cd_cliente_cblc"].ToString();
                    res.Quantidade = (decimal)dr["vl_quantidade"];
                    if (!Convert.IsDBNull(dr["vl_preco_enviostart"]))
                        res.PrecoEnvio = (decimal)dr["vl_preco_enviostart"];
                    if (!Convert.IsDBNull(dr["vl_preco_enviogain"]))
                        res.PrecoEnvio = (decimal)dr["vl_preco_enviogain"];
                    if (!Convert.IsDBNull(dr["vl_preco_envioloss"]))
                        res.PrecoEnvio = (decimal)dr["vl_preco_envioloss"];
                    res.ItemAutomacaoTipo = (ItemAutomacaoTipoEnum)((int)dr["id_stopstart_tipo"]);
                }
                dr.Dispose();
                dr = null;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex);
            }
            finally
            {
                conn.Close();
                conn.Dispose();

                command.Connection.Close();
                command.Dispose();
                command = null;
            }
            return res;
        }

        public void CancelaOrdemStopStart(string id_startstop, int id_stopstart_status)
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
            catch (Exception ex)
            {
                Log.EfetuarLog(ex);
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

        public void ExcluirOrdem(string idStartStop)
        {
            string procName = "prc_TB_STOPSTART_del";

            SqlCommand command = new SqlCommand();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();
            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = procName;
                command.Parameters.AddWithValue("@id_startstop", idStartStop);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex);
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

        public AutomacaoOrdensInfo ListarOrdemStopStartResumido(string id_startstop)
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
                    _TOrdem.IdStopStart = dtDados.Rows[0]["id_startstop"].ToString();
                    _TOrdem.Instrumento = dtDados.Rows[0]["cd_instrumento"].ToString();

                }

                return _TOrdem;

            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex);
                return null;
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

        public ListarItensAutomacaoOrdemResponse ListarOrdensStartStop(ListarItensAutomacaoOrdemRequest req)
        {

            ListarItensAutomacaoOrdemResponse res = new ListarItensAutomacaoOrdemResponse();

            SqlCommand command = new SqlCommand();

            AutomacaoOrdensInfo _TOrdem = new AutomacaoOrdensInfo();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_TB_STOPSTART_lst";

                if (req.CodigoItemAutomacaoOrdem != null)
                    command.Parameters.AddWithValue("@id_startstop", req.CodigoItemAutomacaoOrdem);
                if (req.StatusAutomacaoOrdem != null)
                    command.Parameters.AddWithValue("@id_StopStart_status", (int)req.StatusAutomacaoOrdem);
                if (req.TipoAutomacaoOrdem != null)
                    command.Parameters.AddWithValue("@id_StopStart_tipo", (int)req.TipoAutomacaoOrdem);
                if (req.CodigoCliente != null)
                    command.Parameters.AddWithValue("@cd_Cliente_cblc", req.CodigoCliente);
                if (req.DataDeOrdemEnvio != DateTime.MinValue)
                    command.Parameters.AddWithValue("@DataOrdemEnvio", req.DataDeOrdemEnvio);
                if (req.CodigoNegocio != null)
                    command.Parameters.AddWithValue("@cd_instrumento", req.CodigoNegocio);

                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                res.ListaDeAutomacaoOrdens = new List<ItemAutomacaoOrdemInfo>();
                while (dr.Read())
                {
                    ItemAutomacaoOrdemInfo itemAutomacao = new ItemAutomacaoOrdemInfo();

                    itemAutomacao.CodigoItemAutomacaoOrdem = dr["id_startstop"].ToString();

                    if (!Convert.IsDBNull(dr["dt_ordemdisparo"]))
                        itemAutomacao.DataDisparoOrdem = Convert.ToDateTime(dr["dt_ordemdisparo"]);
                    if (!Convert.IsDBNull(dr["dt_execucao"]))
                        itemAutomacao.DataExecucao = Convert.ToDateTime(dr["dt_execucao"]);
                    if (!Convert.IsDBNull(dr["dt_ordemenvio"]))
                        itemAutomacao.DataOrdemEnvio = Convert.ToDateTime(dr["dt_ordemenvio"]);
                    if (!Convert.IsDBNull(dr["dt_validade"]))
                        itemAutomacao.DataValidade = Convert.ToDateTime(dr["dt_validade"]);
                    //itemAutomacao.ItemAtivo = dr[""]
                    itemAutomacao.AutomacaoInfo.AutomacaoStatus = (ItemAutomacaoStatusEnum)dr["id_stopstart_status"];
                    itemAutomacao.AutomacaoInfo.AutomacaoTipo = (ItemAutomacaoTipoEnum)dr["id_stopstart_tipo"];
                    itemAutomacao.AutomacaoInfo.CodigoBolsa = dr["ds_bolsa"].ToString();
                    itemAutomacao.AutomacaoInfo.CodigoCliente = dr["cd_cliente_cblc"].ToString();
                    itemAutomacao.AutomacaoInfo.Instrumento = dr["cd_instrumento"].ToString();
                    itemAutomacao.PrazoExecucao = Convert.IsDBNull(dr["stPrazoExecucao"]) ? ItemPrazoExecucaoEnum.Para30Dias : ((ItemPrazoExecucaoEnum)(int)dr["stPrazoExecucao"]);
                    itemAutomacao.AutomacaoInfo.OrdemDirecao =
                        itemAutomacao.AutomacaoInfo.AutomacaoTipo == ItemAutomacaoTipoEnum.StartCompra ? OrdemDirecaoEnum.Compra : OrdemDirecaoEnum.Venda;
                    switch (itemAutomacao.AutomacaoInfo.AutomacaoTipo)
                    {
                        case ItemAutomacaoTipoEnum.StartCompra:
                            itemAutomacao.AutomacaoInfo.PrecosTaxas.Add(
                            new AutomacaoPrecosTaxasInfo()
                            {
                                AutomacaoTipo = ItemAutomacaoTipoEnum.StartCompra,
                                PrecoEnvio = (decimal)dr["vl_preco_enviostart"],
                                PrecoGatilho = (decimal)dr["vl_preco_start"]
                            });
                            break;
                        case ItemAutomacaoTipoEnum.StopGain:
                            itemAutomacao.AutomacaoInfo.PrecosTaxas.Add(
                                new AutomacaoPrecosTaxasInfo()
                                {
                                    AutomacaoTipo = ItemAutomacaoTipoEnum.StopGain,
                                    PrecoEnvio = (decimal)dr["vl_preco_startgain"],
                                    PrecoGatilho = (decimal)dr["vl_preco_gain"]
                                });
                            break;
                        case ItemAutomacaoTipoEnum.StopLoss:
                            itemAutomacao.AutomacaoInfo.PrecosTaxas.Add(
                                new AutomacaoPrecosTaxasInfo()
                                {
                                    AutomacaoTipo = ItemAutomacaoTipoEnum.StopLoss,
                                    PrecoEnvio = (decimal)dr["vl_preco_envioloss"],
                                    PrecoGatilho = (decimal)dr["vl_preco_loss"]
                                });
                            break;
                        case ItemAutomacaoTipoEnum.StopMovel:
                            itemAutomacao.AutomacaoInfo.PrecosTaxas.Add(
                                new AutomacaoPrecosTaxasInfo()
                                {
                                    AutomacaoTipo = ItemAutomacaoTipoEnum.StopMovel,
                                    TaxaAjusteInicio = (decimal)dr["vl_inicio_movel"],
                                    TaxaAjusteMovel = (decimal)dr["vl_ajuste_movel"]
                                });
                            break;
                        case ItemAutomacaoTipoEnum.StopSimultaneo:
                            itemAutomacao.AutomacaoInfo.PrecosTaxas.Add(
                                new AutomacaoPrecosTaxasInfo()
                                {
                                    AutomacaoTipo = ItemAutomacaoTipoEnum.StopGain,
                                    PrecoEnvio = (decimal)dr["vl_preco_startgain"],
                                    PrecoGatilho = (decimal)dr["vl_preco_gain"]
                                });
                            itemAutomacao.AutomacaoInfo.PrecosTaxas.Add(
                                new AutomacaoPrecosTaxasInfo()
                                {
                                    AutomacaoTipo = ItemAutomacaoTipoEnum.StopLoss,
                                    PrecoEnvio = (decimal)dr["vl_preco_envioloss"],
                                    PrecoGatilho = (decimal)dr["vl_preco_loss"]
                                });
                            break;
                    }
                    itemAutomacao.AutomacaoInfo.Quantidade = (decimal)dr["vl_quantidade"];
                    res.ListaDeAutomacaoOrdens.Add(itemAutomacao);
                }
                dr.Dispose();
                dr = null;
                return res;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex);
                return null;
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

        public List<ItemAutomacaoOrdemHistoricoInfo> ListarHistorico(string idstartstop)
        {
            List<ItemAutomacaoOrdemHistoricoInfo> lstHist = new List<ItemAutomacaoOrdemHistoricoInfo>();
            SqlCommand command = new SqlCommand();

            AutomacaoOrdensInfo _TOrdem = new AutomacaoOrdensInfo();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = Conn;
            conn.Open();

            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_TB_STOPSTART_HISTORICO_lst";

                command.Parameters.AddWithValue("@id_startstop", idstartstop);

                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    lstHist.Add(new ItemAutomacaoOrdemHistoricoInfo()
                    {
                        DataDoEvento = Convert.ToDateTime(dr["dt_status_operacao"]),
                        DescricaoDoStatus = dr["ds_stopstart_status"].ToString()
                    });
                }
                dr.Dispose();
                dr = null;
                return lstHist;

            }
            catch(Exception ex)
            {
                Log.EfetuarLog(ex);
                return null;
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
