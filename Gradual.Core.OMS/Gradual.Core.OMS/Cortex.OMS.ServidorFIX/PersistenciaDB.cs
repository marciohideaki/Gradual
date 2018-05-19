#region Includes
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Cortex.OMS.ServidorFIXAdm.Lib.Dados;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using System.Configuration;
using System;
using System.Collections;

#endregion

namespace Cortex.OMS.ServidorFIX
{
    public class CamadaDeDados
    {
        #region Globais

        private SqlConnection gConexao;

        private SqlCommand gComando;

        private bool bSaveFixMsgSeqNum = false;

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SqlConnection _connOMS;
        //private SqlCommand _commOMS;
        #endregion

        #region Propriedades

        /// <summary>
        /// (readonly) Retorna true se a conexão com o banco está instanciada
        /// </summary>
        public bool ConexaoIniciada
        {
            get
            {
                return !(gConexao == null);
            }
        }

        /// <summary>
        /// (readonly) Retorna true se a conexão com o banco está aberta
        /// </summary>
        public bool ConexaoAberta
        {
            get
            {
                return (gConexao != null && gConexao.State == System.Data.ConnectionState.Open);
            }
        }

        #endregion


        #region Constructor
        public CamadaDeDados()
        {
            AbrirConexaoOMS();
        }
        ~CamadaDeDados()
        {
            FecharConexaoOMS();
        }
        #endregion


        #region Métodos Private

        public void AbrirConexao()
        {
            if (!this.ConexaoAberta)
            {
                gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                                ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

                gConexao.Open();
            }
        }

        public void FecharConexao()
        {
            try
            {
                gConexao.Close();

                gConexao.Dispose();
            }
            catch { }
        }

        private void AbrirConexaoOMS()
        {
            try
            {
                _connOMS = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);
                _connOMS.Open();
            }
            catch { }
        }
        private void FecharConexaoOMS()
        {
            try
            {
                _connOMS.Close();
                _connOMS.Dispose();
            }
            catch{}
        }

        public void FecharConexao(SqlConnection conn)
        {
            try
            {
                conn.Close();

                conn.Dispose();
            }
            catch { }
        }

        

        private OrdemInfo _CloneOrdemInfo(OrdemInfo info )
        {
            OrdemInfo clone = new OrdemInfo();

            clone.Account = info.Account;
            clone.ChannelID = info.ChannelID;
            clone.ClOrdID = info.ClOrdID;
            clone.CumQty = info.CumQty;
            clone.Exchange  = info.Exchange;
            clone.ExchangeNumberID = info.ExchangeNumberID;
            clone.ExecBroker = info.ExecBroker;
            clone.ExpireDate = info.ExpireDate;
            clone.FixMsgSeqNum = info.FixMsgSeqNum;
            clone.IdOrdem = info.IdOrdem;
            clone.MaxFloor = info.MaxFloor;
            clone.MinQty = info.MinQty;
            clone.OrderQty = info.OrderQty;
            clone.OrderQtyRemmaining = info.OrderQtyRemmaining;
            clone.OrdStatus = info.OrdStatus;
            clone.OrdType = info.OrdType;
            clone.OrigClOrdID = info.OrigClOrdID;
            clone.Price = info.Price;
            clone.RegisterTime = info.RegisterTime;
            clone.SecurityExchangeID = info.SecurityExchangeID;
            clone.SecurityID = info.SecurityID;
            clone.SecurityIDSource = info.SecurityIDSource;
            clone.Side = info.Side;
            clone.StopPrice = info.StopPrice;
            clone.StopStartID = info.StopStartID;
            clone.Symbol = info.Symbol;
            clone.TimeInForce = info.TimeInForce;
            clone.TransactTime = info.TransactTime;

            clone.Acompanhamentos.AddRange(info.Acompanhamentos.ToArray());

            return clone;
        }

        #endregion

        #region Métodos Públicos

        public object InserirAcompanhamentoDeOrdem(AcompanhamentoOrdemInfo pInfo)
        {
            object lIdOrdemHistorico;

            AbrirConexao();

            gComando = new SqlCommand("prc_update_order", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gComando.Parameters.Add(new SqlParameter("@ClOrdID", pInfo.NumeroControleOrdem));
            gComando.Parameters.Add(new SqlParameter("@OrdStatusId", pInfo.StatusOrdem));
            gComando.Parameters.Add(new SqlParameter("@ExchangeNumberID", pInfo.CodigoResposta));
            gComando.Parameters.Add(new SqlParameter("@OrderQtyRemaining", pInfo.QuantidadeRemanescente));
            gComando.Parameters.Add(new SqlParameter("@CumQty", pInfo.QuantidadeExecutada));
            gComando.Parameters.Add(new SqlParameter("@Price", pInfo.Preco));
            if ( bSaveFixMsgSeqNum )
                gComando.Parameters.Add(new SqlParameter("@FixMsgSeqNum", 0));


            gComando.ExecuteNonQuery();

            gComando.CommandText = "prc_ins_order_detail_ind";

            gComando.Parameters.Clear();

            gComando.Parameters.Add(new SqlParameter("@TransactId", pInfo.CodigoTransacao));
            gComando.Parameters.Add(new SqlParameter("@ClOrdID", pInfo.NumeroControleOrdem));
            gComando.Parameters.Add(new SqlParameter("@OrderQty", pInfo.QuantidadeSolicitada));
            gComando.Parameters.Add(new SqlParameter("@OrdQtyRemaining", pInfo.QuantidadeRemanescente));
            gComando.Parameters.Add(new SqlParameter("@CumQty", pInfo.QuantidadeExecutada));
            gComando.Parameters.Add(new SqlParameter("@TradeQty", pInfo.QuantidadeNegociada));
            gComando.Parameters.Add(new SqlParameter("@Price", pInfo.LastPx));
            gComando.Parameters.Add(new SqlParameter("@OrderStatusID", pInfo.StatusOrdem));
            gComando.Parameters.Add(new SqlParameter("@Description", pInfo.Descricao));
            gComando.Parameters.Add(new SqlParameter("@EventTime", pInfo.DataAtualizacao));

            if (bSaveFixMsgSeqNum)
                gComando.Parameters.Add(new SqlParameter("@FixMsgSeqNum", 0));

            lIdOrdemHistorico = gComando.ExecuteScalar();

            FecharConexao();


            return lIdOrdemHistorico;
        }

        public void AtualizarOrdem(OrdemInfo ordemInfo)
        {
            AbrirConexao();

            gComando = new SqlCommand("prc_update_order", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gComando.Parameters.Add(new SqlParameter("@ClOrdID", ordemInfo.ClOrdID));
            gComando.Parameters.Add(new SqlParameter("@OrdStatusId", ordemInfo.OrdStatus));
            gComando.Parameters.Add(new SqlParameter("@ExchangeNumberID", ordemInfo.ExchangeNumberID));
            gComando.Parameters.Add(new SqlParameter("@OrderQtyRemaining", ordemInfo.OrderQtyRemmaining));
            gComando.Parameters.Add(new SqlParameter("@CumQty", ordemInfo.CumQty));
            gComando.Parameters.Add(new SqlParameter("@Price", ordemInfo.Price));
            if (bSaveFixMsgSeqNum)
                gComando.Parameters.Add(new SqlParameter("@FixMsgSeqNum", 0));


            gComando.ExecuteNonQuery();

            FecharConexao();
        }

        /// <summary>
        /// </summary>
        /// <param name="pInfo"></param>
        public void TrocarNumeroControleOrdem(OrdemInfo pInfo)
        {

            AbrirConexao();

            gComando = new SqlCommand("prc_troca_numerocontrole", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gComando.Parameters.Add(new SqlParameter("@pClOrdID", pInfo.ClOrdID));

            gComando.ExecuteNonQuery();

            FecharConexao();
        }





        /// <summary>
        /// BuscarSessoesFIX - recupera toda lista de sessoes configuradas
        /// </summary>
        /// <param name="ordemalterada">OrdemInfo da ordem alterada</param>
        /// <returns>objeto tipo OrdemInfo</returns>
        public List<SessionFixInfo> BuscarSessoesFIX()
        {
            List<SessionFixInfo> ret = new List<SessionFixInfo>();

            SqlDataAdapter lAdapter;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);

            conn.Open();

            DataSet lDataSet = new DataSet();

            gComando = new SqlCommand("prc_fixsrv_sessaofix_lst", conn);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            foreach (DataRow lRow in lDataSet.Tables[0].Rows)
            {
                //DataRow lRow = lDataSet.Tables[0].Rows[0];

                SessionFixInfo session = new SessionFixInfo();

                session.idSessaoFix = lRow["idSessaoFix"].DBToInt32();
                session.SenderCompID = lRow["SenderCompID"].DBToString();
                session.TargetCompID = lRow["TargetCompID"].DBToString();
                session.LogonPassword = lRow["LogonPassword"].DBToString();
                session.PersistMessages = lRow["PersistMessages"].DBToInt32();
                session.ResetSeqNum = lRow["ResetSeqNum"].DBToInt32();
                session.HeartBeatInterval = lRow["HeartBeatInterval"].DBToInt32();
                session.FixVersion = lRow["FixVersion"].DBToDecimal().ToString(CultureInfo.InvariantCulture);
                session.BeginString = "FIX." + lRow["FixVersion"].DBToDecimal().ToString(CultureInfo.InvariantCulture);
                session.Bolsa = lRow["Bolsa"].DBToString();

                ret.Add(session);
            }

            FecharConexao(conn);

            return ret;
        }


        /// <summary>
        /// BuscarClienteOperador - recupera toda lista de contas x operador bolsa associados
        /// </summary>
        /// <param name="ordemalterada">OrdemInfo da ordem alterada</param>
        /// <returns>objeto tipo OrdemInfo</returns>
        public List<ClienteOperadorInfo> BuscarClienteOperador()
        {
            List<ClienteOperadorInfo> ret = new List<ClienteOperadorInfo>();

            SqlDataAdapter lAdapter;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);

            conn.Open();

            DataSet lDataSet = new DataSet();

            gComando = new SqlCommand("prc_fixsrv_sessaofix_lst", conn);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            foreach (DataRow lRow in lDataSet.Tables[0].Rows)
            {
                //DataRow lRow = lDataSet.Tables[0].Rows[0];

                ClienteOperadorInfo session = new ClienteOperadorInfo();

                session.Account = lRow["Account"].DBToInt32();
                session.Operador = lRow["Operador"].DBToInt32();

                ret.Add(session);
            }

            FecharConexao(conn);

            return ret;
        }


        #endregion

        public  List<LimiteClienteInfo> CarregarLimiteClientes(int account = 0)
        {
            List<LimiteClienteInfo> ret = new List<LimiteClienteInfo>();

            SqlDataAdapter lAdapter;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);

            conn.Open();

            DataSet lDataSet = new DataSet();

            if (0 == account)
            {
                gComando = new SqlCommand("prc_obter_relacao_clientes_oms", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;
            }
            else
            {
                gComando = new SqlCommand("prc_obter_relacao_clientes_oms_account", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;
                gComando.Parameters.Add(new SqlParameter("@Account", account));
            }
            

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            foreach (DataRow lRow in lDataSet.Tables[0].Rows )
            {
                //DataRow lRow = lDataSet.Tables[0].Rows[0];

                LimiteClienteInfo info = new LimiteClienteInfo();

                info.idLimite = lRow["idLimite"].DBToInt32();
                info.CodigoBovespa = lRow["CodigoBovespa"].DBToInt32();
                info.CodigoBMF = lRow["CodigoBMF"].DBToInt32();
                info.flgClienteAdministrada = lRow["flgClienteRepasse"].DBToString().ToUpper().Equals("S");
                info.flgClienteRepasse = lRow["flgClienteRepasse"].DBToString().ToUpper().Equals("S");
                info.flgCOD = lRow["flgClienteRepasse"].DBToString().ToUpper().Equals("S");
                info.flgContaMasterBMF = lRow["flgClienteRepasse"].DBToString().ToUpper().Equals("S"); 
                info.flgContaMasterBovespa = lRow["flgClienteRepasse"].DBToString().ToUpper().Equals("S");
                info.OperadorBovespa = lRow["OperadorBovespa"].DBToInt32();
                info.OperadorBMF = lRow["OperadorBMF"].DBToInt32();
                info.valorConsumido = lRow["valorConsumido"].DBToDecimal();
                info.valorPreAlocado = 0.0M;
                info.valorRestante = lRow["valorRestante"].DBToDecimal();
                info.valorTotal = lRow["valorTotal"].DBToDecimal();

                info.dataValidade = lRow["dataValidade"].DBToDateTime();

                if (info.dataValidade < DateTime.Now)
                {
                    info.valorRestante = 0;
                    info.valorTotal = 0;
                }
                   
                ret.Add(info);
            }

            FecharConexao(conn);

            return ret;
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clordid"></param>
        /// <returns></returns>
        public OrdemInfo BuscarOrdemClordID(string clordid)
        {
            OrdemInfo ret = new OrdemInfo();

            SqlDataAdapter lAdapter;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);

            conn.Open();

            DataSet lDataSet = new DataSet();

            gComando = new SqlCommand("prc_fixsrv_buscar_ordem_clorid", conn);
            gComando.Parameters.Add(new SqlParameter("@CLOrdID", clordid));

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            if ( lDataSet.Tables[0].Rows.Count > 0 )
            {
                DataRow lRow = lDataSet.Tables[0].Rows[0];

                ret.IdOrdem = Convert.ToInt32(lRow["OrderId"]);
                ret.Account = lRow["Account"].DBToInt32();
                ret.ChannelID = lRow["ChannelId"].DBToInt32();
                ret.ClOrdID = lRow["ClOrdId"].DBToString();
                ret.ExchangeNumberID = lRow["ExchangeNumberId"].DBToString();
                ret.ExecBroker = lRow["ExecBroker"].DBToString();
                ret.ExpireDate = lRow["ExpireDate"].DBToDateTime();
                ret.MaxFloor = lRow["MaxFloor"].DBToDouble();
                ret.MinQty = lRow["MinQty"].DBToDouble();
                ret.OrderQty = lRow["OrderQty"].DBToInt32();
                ret.OrderQtyRemmaining = lRow["OrderQtyRemaining"].DBToInt32();
                ret.CumQty = lRow["CumQty"].DBToInt32();
                ret.OrdStatus = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();
                ret.OrdType = (OrdemTipoEnum)lRow["OrdTypeId"].DBToInt32();

                ret.RegisterTime = lRow["RegisterTime"].DBToDateTime();
                ret.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                ret.SecurityID = lRow["SecurityExchangeId"].DBToString();


                ret.Side = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                ret.StopStartID = lRow["StopStartID"].DBToInt32();
                ret.Symbol = lRow["Symbol"].DBToString();
                ret.TimeInForce = (OrdemValidadeEnum)lRow["TimeInForce"].DBToInt32();
                ret.TransactTime = lRow["TransactTime"].DBToDateTime();
                if (lRow.Table.Columns.Contains("FixMsgSeqNum"))
                    ret.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();

                if (lRow["Price"] != DBNull.Value)
                    ret.Price = lRow["Price"].DBToDouble().Value;
            }

            FecharConexao(conn);

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clordid"></param>
        /// <returns></returns>
        public OrdemInfo BuscarOrdemOrderID(int orderid)
        {
            OrdemInfo ret = new OrdemInfo();

            SqlDataAdapter lAdapter;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);

            conn.Open();

            DataSet lDataSet = new DataSet();

            gComando = new SqlCommand("prc_fixsrv_buscar_ordem_orderid", conn);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;
            gComando.Parameters.Add(new SqlParameter("@OrderID", orderid));

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            if (lDataSet.Tables[0].Rows.Count > 0)
            {
                DataRow lRow = lDataSet.Tables[0].Rows[0];

                ret.IdOrdem = Convert.ToInt32(lRow["OrderId"]);
                ret.Account = lRow["Account"].DBToInt32();
                ret.ChannelID = lRow["ChannelId"].DBToInt32();
                ret.ClOrdID = lRow["ClOrdId"].DBToString();
                ret.ExchangeNumberID = lRow["ExchangeNumberId"].DBToString();
                ret.ExecBroker = lRow["ExecBroker"].DBToString();
                ret.ExpireDate = lRow["ExpireDate"].DBToDateTime();
                ret.MaxFloor = lRow["MaxFloor"].DBToDouble();
                ret.MinQty = lRow["MinQty"].DBToDouble();
                ret.OrderQty = lRow["OrderQty"].DBToInt32();
                ret.OrderQtyRemmaining = lRow["OrderQtyRemaining"].DBToInt32();
                ret.CumQty = lRow["CumQty"].DBToInt32();
                ret.OrdStatus = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();
                ret.OrdType = (OrdemTipoEnum)lRow["OrdTypeId"].DBToInt32();

                ret.RegisterTime = lRow["RegisterTime"].DBToDateTime();
                ret.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                ret.SecurityID = lRow["SecurityExchangeId"].DBToString();


                ret.Side = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                ret.StopStartID = lRow["StopStartID"].DBToInt32();
                ret.Symbol = lRow["Symbol"].DBToString();
                ret.TimeInForce = (OrdemValidadeEnum)lRow["TimeInForce"].DBToInt32();
                ret.TransactTime = lRow["TransactTime"].DBToDateTime();
                if (lRow.Table.Columns.Contains("FixMsgSeqNum"))
                    ret.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();

                if (lRow["Price"] != DBNull.Value)
                    ret.Price = lRow["Price"].DBToDouble().Value;
            }

            FecharConexao(conn);

            return ret;
        }


        /// <summary>
        /// Método responsavel por guardar uma ordem original em caso de alteracao de ordens
        /// </summary>
        /// <param name="_ClienteOrdemInfo"></param>
        /// <returns></returns>
        public bool InserirOrdemBackup(OrdemInfo _ClienteOrdemInfo)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);
                    
                conn.Open();

                gComando = new SqlCommand("prc_ins_order_updated", conn);

                gComando.CommandType = System.Data.CommandType.StoredProcedure;

                gComando.Parameters.Add(new SqlParameter("@OrderID", _ClienteOrdemInfo.IdOrdem));
                gComando.Parameters.Add(new SqlParameter("@account", _ClienteOrdemInfo.Account));
                gComando.Parameters.Add(new SqlParameter("@instrumento", _ClienteOrdemInfo.Symbol));
                gComando.Parameters.Add(new SqlParameter("@CLOrdID", _ClienteOrdemInfo.ClOrdID));
                gComando.Parameters.Add(new SqlParameter("@OrdStatusID", (int)_ClienteOrdemInfo.OrdStatus));
                gComando.Parameters.Add(new SqlParameter("@price",  _ClienteOrdemInfo.Price));
                gComando.Parameters.Add(new SqlParameter("@quantidade",  _ClienteOrdemInfo.OrderQty));
                gComando.Parameters.Add(new SqlParameter("@quantidade_exec",  _ClienteOrdemInfo.CumQty));
                gComando.Parameters.Add(new SqlParameter("@quantidade_aparente", _ClienteOrdemInfo.MaxFloor));
                gComando.Parameters.Add(new SqlParameter("@quantidade_minima", _ClienteOrdemInfo.MinQty));
                gComando.Parameters.Add(new SqlParameter("@dt_validade", _ClienteOrdemInfo.ExpireDate));

                gComando.ExecuteNonQuery();

                FecharConexao(conn);

                logger.Info("Backup da ordem a ser alterada inserida no banco de dados com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao enviar alteracao ordem para o banco de dados");
                logger.Error("Descrição do erro:    " + ex.Message, ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Método responsável por inserir uma nova ordem no banco de dados
        /// </summary>
        /// <param name="_ClienteOrdemInfo">Atributos da ordem do cliente</param>
        /// <returns>bool</returns>
        public bool InserirOrdem(OrdemInfo _ClienteOrdemInfo, string description=null)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);

                conn.Open();

                gComando = new SqlCommand("prc_ins_order_router_oms_v2", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;

                // Adiciona os parametros de ordem
                if (_ClienteOrdemInfo.IdOrdem == 0)
                {
                    gComando.Parameters.Add(new SqlParameter("@OrderID", Convert.ToInt32(0)));
                }
                else
                {
                    gComando.Parameters.Add(new SqlParameter("@OrderID", _ClienteOrdemInfo.IdOrdem));
                }

                gComando.Parameters.Add(new SqlParameter("@ClOrdID", _ClienteOrdemInfo.ClOrdID));

                if (_ClienteOrdemInfo.StopStartID == 0)
                {
                    gComando.Parameters.Add(new SqlParameter("@StopStartID", DBNull.Value));
                }
                else
                {
                    gComando.Parameters.Add(new SqlParameter("@StopStartID", _ClienteOrdemInfo.StopStartID));
                }

                gComando.Parameters.Add(new SqlParameter("@OrigClOrdID", _ClienteOrdemInfo.OrigClOrdID));
                gComando.Parameters.Add(new SqlParameter("@Account", _ClienteOrdemInfo.Account));
                gComando.Parameters.Add(new SqlParameter("@Symbol", _ClienteOrdemInfo.Symbol.Trim().ToUpper()));
                
                SqlParameter param = new SqlParameter("@OrdTypeID", ((int)_ClienteOrdemInfo.OrdType));
                param.DbType = DbType.AnsiString;
                gComando.Parameters.Add(param);

                gComando.Parameters.Add(new SqlParameter("@OrdStatusID", (int)_ClienteOrdemInfo.OrdStatus));
                gComando.Parameters.Add(new SqlParameter("@ExpireDate", _ClienteOrdemInfo.ExpireDate.Value));
                gComando.Parameters.Add(new SqlParameter("@TimeInForce", (int)_ClienteOrdemInfo.TimeInForce));
                gComando.Parameters.Add(new SqlParameter("@ChannelID", _ClienteOrdemInfo.ChannelID));
                gComando.Parameters.Add(new SqlParameter("@ExecBroker", _ClienteOrdemInfo.ExecBroker));
                gComando.Parameters.Add(new SqlParameter("@Side", (int)_ClienteOrdemInfo.Side));
                gComando.Parameters.Add(new SqlParameter("@OrderQty", _ClienteOrdemInfo.OrderQty));

                gComando.Parameters.Add(new SqlParameter("@OrderQtyMin", _ClienteOrdemInfo.MinQty.Value));
                gComando.Parameters.Add(new SqlParameter("@OrderQtyApar", _ClienteOrdemInfo.MaxFloor.Value));

                gComando.Parameters.Add(new SqlParameter("@OrderQtyRemaining", _ClienteOrdemInfo.OrderQtyRemmaining));
                gComando.Parameters.Add(new SqlParameter("@Price", _ClienteOrdemInfo.Price));

                if ( description==null )
                    gComando.Parameters.Add(new SqlParameter("@description", DBNull.Value));
                else
                    gComando.Parameters.Add(new SqlParameter("@description", description));

                gComando.ExecuteNonQuery();

                logger.InfoFormat("Nova ordem [{0}] inserida no banco de dados ", _ClienteOrdemInfo.ClOrdID);
            }
            catch (SqlException sqlex)
            {
                PrintSqlException(sqlex);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao enviar a ordem para o banco de dados");
                logger.Error("Descrição do erro:    " + ex.Message, ex);

                return false;
            }

            return true;
        }

        public bool InserirMovimentoLimite(LimiteMovimentoInfo movto)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);
            try
            {
                conn.Open();

                gComando = new SqlCommand("prc_fixsrv_ins_movimento_limite", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;


                gComando.Parameters.Add(new SqlParameter("@Account", movto.Account));
                gComando.Parameters.Add(new SqlParameter("@Symbol", movto.instrumento));
                gComando.Parameters.Add(new SqlParameter("@DataMovimento", movto.dataMovimento));
                gComando.Parameters.Add(new SqlParameter("@IdLimite", movto.idLimite));
                gComando.Parameters.Add(new SqlParameter("@IdLancamento", movto.idLancamento));
                gComando.Parameters.Add(new SqlParameter("@Quantidade", movto.quantidade));
                gComando.Parameters.Add(new SqlParameter("@Preco", movto.Preco));
                gComando.Parameters.Add(new SqlParameter("@ValorConsumido", movto.valorConsumido));
                gComando.Parameters.Add(new SqlParameter("@ValorRestante", movto.valorRestante));
                gComando.Parameters.Add(new SqlParameter("@ValorTotal", movto.valorTotal));

                gComando.ExecuteNonQuery();


                logger.Info("Movimento de limite inserido no banco de dados ");
            }
            catch (SqlException sqlex)
            {
                PrintSqlException(sqlex);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("InserirMovimentoLimite() Erro:    " + ex.Message, ex);
                return false;
            }
            finally
            {
                // Liberar a conexao e objetos
                gComando.Dispose();
                gComando = null;
                FecharConexao(conn);
            }
            
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ZerarLimiteDiario()
        {
            try
            {

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);

                conn.Open();

                DataSet lDataSet = new DataSet();

                gComando = new SqlCommand("prc_fixsrv_zerar_limite_diario", conn);

                gComando.CommandType = System.Data.CommandType.StoredProcedure;

                int rows = gComando.ExecuteNonQuery();

                logger.Info("Zerou consumo diario para [" + rows + "] clientes");

                FecharConexao(conn);
            }
            catch (SqlException sqlex)
            {
                PrintSqlException(sqlex);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("ZerarLimiteDiario() Erro: " + ex.Message, ex);
                return false;
            }

            return true;
        }

        public bool ZerarLimiteDiarioContratoBMF()
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);
                conn.Open();
                DataSet lDataSet = new DataSet();
                gComando = new SqlCommand("prc_fixsrv_zerar_limite_bmf_contrato", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;
                int rows = gComando.ExecuteNonQuery();
                logger.Info("ZerarLimiteDiarioContratoBMF(): Zerou consumo diario para [" + rows + "] clientes");
                FecharConexao(conn);
            }
            catch (SqlException sqlex)
            {
                PrintSqlException(sqlex);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("ZerarLimiteDiarioContratoBMF(): Erro: " + ex.Message, ex);
                return false;
            }
            return true;
        }

        public bool ZerarLimiteDiarioInstrumentoBMF()
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);
                conn.Open();
                DataSet lDataSet = new DataSet();
                gComando = new SqlCommand("prc_fixsrv_zerar_limite_bmf_instrumento", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;
                int rows = gComando.ExecuteNonQuery();
                logger.Info("ZerarLimiteDiarioInstrumentoBMF(): Zerou consumo diario para [" + rows + "] clientes");
                FecharConexao(conn);
            }
            catch (SqlException sqlex)
            {
                PrintSqlException(sqlex);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("ZerarLimiteDiarioInstrumentoBMF(): Erro: " + ex.Message, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AtualizarClienteLimite(int iAccount, Decimal vlrTotal, Decimal vlrConsumido, Decimal vlrRestante)
        {
            try
            {
                //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);
                //conn.Open();
                // DataSet lDataSet = new DataSet();

                SqlCommand sqlCommand = new SqlCommand("prc_fixsrv_atualizar_limite_cliente", _connOMS);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@Account", iAccount));
                sqlCommand.Parameters.Add(new SqlParameter("@ValorTotal", vlrTotal));
                sqlCommand.Parameters.Add(new SqlParameter("@ValorConsumido", vlrConsumido));
                sqlCommand.Parameters.Add(new SqlParameter("@ValorRestante", vlrRestante));
                int rows = sqlCommand.ExecuteNonQuery();
                if (rows < 0)
                    logger.InfoFormat("Cliente[{0}] ValorTotal[{1}] nao encontrado, inativo ou expirado", iAccount, vlrTotal);
                sqlCommand.Dispose();
                sqlCommand = null;
                //FecharConexao(conn);
            }
            catch (SqlException sqlex)
            {
                PrintSqlException(sqlex);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("ZerarLimiteDiario() Erro: " + ex.Message, ex);
                return false;
            }

            return true;
        }

        public static void PrintSqlException(SqlException sqlex)
        {
            foreach (DictionaryEntry de in sqlex.Data)
            {
                logger.ErrorFormat("Data: [{0}]=[{1}]", de.Key, de.Value);
            }

            logger.Error("SqlErrorCode : " + sqlex.ErrorCode);
            foreach(SqlError error in sqlex.Errors )
            {
                logger.ErrorFormat( "Line {0}", error.LineNumber);
            }

            logger.Error("Line ........: " + sqlex.LineNumber);
            logger.Error("Msg .........: " +  sqlex.Message);
            logger.Error("Number ......: " + sqlex.Number);
            logger.Error("Proc ........: " + sqlex.Procedure);
            logger.Error("Server ......: " + sqlex.Server);
            logger.Error("Source ......: " + sqlex.Source);
            logger.Error("StackTrace ..: " + sqlex.StackTrace);
            logger.Error("State .......: " + sqlex.State);
        }


        public List<LimiteClienteContratoBMF> CarregarLimiteContratosBMF(int account = 0)
        {
            List<LimiteClienteContratoBMF> ret = new List<ServidorFIXAdm.Lib.Dados.LimiteClienteContratoBMF>();
            SqlDataAdapter lAdapter;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);

            conn.Open();

            DataSet lDataSet = new DataSet();

            if (0 == account)
            {
                gComando = new SqlCommand("prc_fixsrv_carregar_limite_clientes_bmf", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;
            }
            else
            {
                gComando = new SqlCommand("prc_fixsrv_carregar_limite_clientes_bmf_account", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;
                gComando.Parameters.Add(new SqlParameter("@Account", account));
            }
            

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            foreach (DataRow lRow in lDataSet.Tables[0].Rows)
            {
                //DataRow lRow = lDataSet.Tables[0].Rows[0];
                LimiteClienteContratoBMF info = new LimiteClienteContratoBMF();

                info.idClienteParametroBMF = lRow["idClienteParametroBMF"].DBToInt32();
                info.idClientePermissao = lRow["idClientePermissao"].DBToInt32();
                info.account = lRow["account"].DBToInt32();
                info.contrato = lRow["contrato"].DBToString();
                info.sentido = lRow["sentido"].DBToString().ToUpper();
                info.qtDisponivel = lRow["qtDisponivel"].DBToInt32();
                //info.qtPrealocado = lRow["qtPrealocado"].DBToInt32();
                info.qtTotal = lRow["qtTotal"].DBToInt32();
                info.stAtivo = lRow["stAtivo"].DBToString().ToUpper().Equals("S");
                info.stRenovacaoAutomatica = lRow["stRenovacaoAutomatica"].DBToString().ToUpper().Equals("S");
                info.dtMovimento = lRow["dtMovimento"].DBToDateTime();
                info.dtValidade = lRow["dtValidade"].DBToDateTime();

                info.qtPrealocado = 0;
                if (info.dtValidade < DateTime.Now)
                {
                    info.qtTotal = 0;
                    info.qtDisponivel = 0;
                }

                ret.Add(info);
            }

            FecharConexao(conn);

            return ret;
        }

        public List<LimiteClienteInstrumentoBMF> CarregarLimiteInstrumentosBMF(int account = 0)
        {
            List<LimiteClienteInstrumentoBMF> ret = new List<LimiteClienteInstrumentoBMF>();
            SqlDataAdapter lAdapter;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);

            conn.Open();

            DataSet lDataSet = new DataSet();

            if (0 == account)
            {
                gComando = new SqlCommand("prc_fixsrv_carregar_limite_clientes_bmf_instrumento", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;
            }
            else
            {
                gComando = new SqlCommand("prc_fixsrv_carregar_limite_clientes_bmf_instrumento_account", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;
                gComando.Parameters.Add(new SqlParameter("@Account", account));
            }
            
            

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            foreach (DataRow lRow in lDataSet.Tables[0].Rows)
            {
                //DataRow lRow = lDataSet.Tables[0].Rows[0];

                LimiteClienteInstrumentoBMF info = new LimiteClienteInstrumentoBMF();

                info.idClienteParametroInstrumento = lRow["idClienteParametroInstrumento"].DBToInt32();
                info.idClienteParametroBMF = lRow["idClienteParametroBMF"].DBToInt32();
                info.account = lRow["account"].DBToInt32();
                info.contrato = lRow["contrato"].DBToString();
                info.instrumento = lRow["instrumento"].DBToString();
                info.sentido = lRow["sentido"].DBToString().ToUpper();
                info.qtDispInstrumento = lRow["qtDispInstrumento"].DBToInt32();
                //info.qtPrealocado = lRow["qtPrealocado"].DBToInt32();
                info.qtTotalContratoPai = lRow["qtTotalContratoPai"].DBToInt32();
                info.qtTotalInstrumento = lRow["qtTotalInstrumento"].DBToInt32();
                info.stAtivo = lRow["stAtivo"].DBToString().ToUpper().Equals("S");
                info.stRenovacaoAutomatica = lRow["stRenovacaoAutomatica"].DBToString().ToUpper().Equals("S");
                info.dtMovimento = lRow["dtMovimento"].DBToDateTime();
                info.dtValidade = lRow["dtValidade"].DBToDateTime();

                info.qtPrealocado = 0;
                if (info.dtValidade < DateTime.Now)
                {
                    info.qtTotalContratoPai = 0;
                    info.qtTotalInstrumento = 0;
                    info.qtDispContrato = 0;
                    info.qtDispInstrumento = 0;
                }

                ret.Add(info);
            }

            FecharConexao(conn);

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool InserirMovimentoLimiteInstrumentoBMF(LimiteClienteInstrumentoBMF info)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);
            try
            {
                conn.Open();

                gComando = new SqlCommand("prc_ins_limite_bmf_instrumento", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;


                gComando.Parameters.Add(new SqlParameter("@idClienteParametroInstrumento", info.idClienteParametroInstrumento));
                gComando.Parameters.Add(new SqlParameter("@idClienteParametroBMF", info.idClienteParametroBMF));
                gComando.Parameters.Add(new SqlParameter("@instrumento", info.instrumento));
                gComando.Parameters.Add(new SqlParameter("@qtTotalContratoPai", info.qtTotalContratoPai));
                gComando.Parameters.Add(new SqlParameter("@qtTotalInstrumento", info.qtTotalInstrumento));
                gComando.Parameters.Add(new SqlParameter("@qtDisponivel", info.qtDispInstrumento));

                gComando.ExecuteNonQuery();

                logger.Info("Movimento de limite de instrumento BMF inserido no banco de dados ");
            }
            catch (SqlException sqlex)
            {
                PrintSqlException(sqlex);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("InserirMovimentoLimite() Erro:    " + ex.Message, ex);
                return false;
            }
            finally
            {
                gComando.Dispose();
                gComando = null;
                FecharConexao(conn);
            }
            
            return true;
        }



        public  bool InserirMovimentoLimiteContratoBMF(LimiteClienteContratoBMF info)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString);
            try
            {
                conn.Open();

                gComando = new SqlCommand("prc_ins_limite_bmf", conn);
                gComando.CommandType = System.Data.CommandType.StoredProcedure;

                gComando.Parameters.Add(new SqlParameter("@idClienteParametroBMF", info.idClienteParametroBMF));
                gComando.Parameters.Add(new SqlParameter("@idClientePermissao", info.idClientePermissao));
                gComando.Parameters.Add(new SqlParameter("@account", info.account));
                gComando.Parameters.Add(new SqlParameter("@contrato", info.contrato));
                gComando.Parameters.Add(new SqlParameter("@sentido", info.sentido));
                gComando.Parameters.Add(new SqlParameter("@qtTotal", info.qtTotal));
                gComando.Parameters.Add(new SqlParameter("@qtDisponivel", info.qtDisponivel));
                gComando.Parameters.Add(new SqlParameter("@stRenovacaoAutomatica", info.stRenovacaoAutomatica));
                gComando.Parameters.Add(new SqlParameter("@dtValidade", info.dtValidade));

                gComando.ExecuteNonQuery();

                logger.Info("Movimento de limite de contrato BMF inserido no banco de dados ");
            }
            catch (SqlException sqlex)
            {
                PrintSqlException(sqlex);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("InserirMovimentoLimite() Erro:    " + ex.Message, ex);
                return false;
            }
            finally
            {
                // Desalocar objetos e fechar conexao
                gComando.Dispose();
                gComando = null;
                FecharConexao(conn);
            }
            
            return true;
        }
    }
}
