#region Includes
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Info;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Dados.Enum;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using log4net;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Linq;
using System.Globalization;
#endregion

namespace Gradual.OMS.AcompanhamentoOrdens
{
    public class CamadaDeDados
    {
        #region Globais

        //private SqlConnection gConexao;

        //private SqlCommand gComando;

        //private string gConexaoEmUso = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString;

        private bool bSaveFixMsgSeqNum = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveFixMsgSeqNum"].ToString());

        private static readonly ILog gLog4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Propriedades

        /// <summary>
        /// (readonly) Retorna true se a conexão com o banco está instanciada
        /// </summary>
        //public bool ConexaoIniciada
        //{
        //    get
        //    {
        //        return !(gConexao == null);
        //    }
        //}

        ///// <summary>
        ///// (readonly) Retorna true se a conexão com o banco está aberta
        ///// </summary>
        //public bool ConexaoAberta
        //{
        //    get
        //    {
        //        return (gConexao != null && gConexao.State == System.Data.ConnectionState.Open);
        //    }
        //}

        #endregion

        #region Métodos Private


        private List<OrdemInfo> PreencherOrdensInfoDaVwOrderDetails(DataSet pDataSetDeResultado)
        {
            string lUltimoId = "";

            List<OrdemInfo> lRetorno = new List<OrdemInfo>();

            OrdemInfo lOrdem;
            AcompanhamentoOrdemInfo lAcompanhamento;

            foreach (DataRow lRow in pDataSetDeResultado.Tables[0].Rows)
            {
                if (lUltimoId != lRow["ClOrdId"].ToString())
                {
                    lOrdem = new OrdemInfo();

                    lOrdem.Acompanhamentos = new List<AcompanhamentoOrdemInfo>();

                    lOrdem.IdOrdem            = Convert.ToInt32(lRow["OrderId"]);
                    lOrdem.Account            = lRow["Account"].DBToInt32();
                    lOrdem.ChannelID          = lRow["ChannelId"].DBToInt32();
                    lOrdem.ClOrdID            = lRow["ClOrdId"].DBToString();
                    lOrdem.ExchangeNumberID   = lRow["ExchangeNumberId"].DBToString();
                    lOrdem.ExecBroker         = lRow["ExecBroker"].DBToString();
                    lOrdem.ExpireDate         = lRow["ExpireDate"].DBToDateTime();
                    lOrdem.MaxFloor           = lRow["MaxFloor"].DBToDouble();
                    lOrdem.MinQty             = lRow["MinQty"].DBToDouble();
                    lOrdem.OrderQty           = lRow["OrderQty"].DBToInt32();
                    lOrdem.OrderQtyRemmaining = lRow["OrderQtyRemaining"].DBToInt32();
                    lOrdem.CumQty             = lRow["CumQty"].DBToInt32();
                    lOrdem.OrdStatus          = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();
                    lOrdem.OrdType            = (OrdemTipoEnum)lRow["OrdTypeId"].DBToInt32();

                    lOrdem.RegisterTime       = lRow["RegisterTime"].DBToDateTime();
                    lOrdem.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                    lOrdem.SecurityID         = lRow["SecurityExchangeId"].DBToString();


                    lOrdem.Side         = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                    lOrdem.StopStartID  = lRow["StopStartID"].DBToInt32();
                    lOrdem.Symbol       = lRow["Symbol"].DBToString();
                    lOrdem.TimeInForce  = (OrdemValidadeEnum)lRow["TimeInForce"].DBToInt32();
                    lOrdem.TransactTime = lRow["TransactTime"].DBToDateTime();
                    if (lRow.Table.Columns.Contains("FixMsgSeqNum"))
                        lOrdem.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();

                    if (lRow["Price"] != DBNull.Value)
                        lOrdem.Price = lRow["Price"].DBToDouble().Value;

                    lRetorno.Add(lOrdem);

                    lUltimoId = lOrdem.ClOrdID;
                }

                //Não é dentro de 'else' porque a primeira linha tem o primeiro acompanhamento:

                lAcompanhamento = new AcompanhamentoOrdemInfo();
                lAcompanhamento.CanalNegociacao     = lRow["ChannelID"].DBToInt32();
                lAcompanhamento.CodigoDoCliente     = lRow["Account"].DBToInt32();
                lAcompanhamento.CodigoResposta      = lRow["ExchangeNumberId"].DBToString();
                lAcompanhamento.CodigoTransacao     = lRow["OrderDetail_TransactId"].DBToString();
                lAcompanhamento.DataAtualizacao     = lRow["TransactTime"].DBToDateTime();
                lAcompanhamento.DataOrdemEnvio      = lRow["RegisterTime"].DBToDateTime();
                lAcompanhamento.DataValidade        = lRow["ExpireDate"].DBToDateTime();
                lAcompanhamento.Direcao             = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                lAcompanhamento.Instrumento         = lRow["Symbol"].DBToString();
                lAcompanhamento.NumeroControleOrdem = lRow["ClOrdId"].DBToString();
                lAcompanhamento.Descricao           = lRow["OrderDetail_Description"].DBToString();

                lAcompanhamento.QuantidadeRemanescente = lRow["OrderDetail_OrdQtyRemaining"].DBToInt32();
                lAcompanhamento.QuantidadeExecutada    = lRow["OrderDetail_CumQty"].DBToInt32();
                lAcompanhamento.QuantidadeSolicitada   = lRow["OrderDetail_OrderQty"].DBToInt32();
                lAcompanhamento.QuantidadeNegociada    = lRow["OrderDetail_TradeQty"].DBToInt32();
                lAcompanhamento.StatusOrdem = (OrdemStatusEnum)lRow["OrderDetail_OrderStatusId"].DBToInt32();
                
                if (lRow.Table.Columns.Contains("OrderDetail_FixMsgSeqNum"))
                    lAcompanhamento.FixMsgSeqNum = lRow["OrderDetail_FixMsgSeqNum"].DBToInt32();

                if (lRow["OrderDetail_Price"] != DBNull.Value)
                {
                    lAcompanhamento.Preco = lRow["OrderDetail_Price"].DBToDecimal();
                    lAcompanhamento.LastPx = lRow["OrderDetail_Price"].DBToDecimal();
                }

                lRetorno[lRetorno.Count - 1].Acompanhamentos.Add(lAcompanhamento);

            }

            return lRetorno;
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

        private OrdemInfo _fillOrder(DataRow lRow )
        {
            OrdemInfo lOrdem = new OrdemInfo();

            lOrdem.Acompanhamentos = new List<AcompanhamentoOrdemInfo>();

            lOrdem.IdOrdem            = Convert.ToInt32(lRow["OrderId"]);
            lOrdem.Account            = lRow["Account"].DBToInt32();
            lOrdem.ChannelID          = lRow["ChannelId"].DBToInt32();
            lOrdem.ClOrdID            = lRow["ClOrdId"].DBToString();
            lOrdem.ExchangeNumberID   = lRow["ExchangeNumberId"].DBToString();
            lOrdem.ExecBroker         = lRow["ExecBroker"].DBToString();
            lOrdem.ExpireDate         = lRow["ExpireDate"].DBToDateTime();
            lOrdem.MaxFloor           = lRow["MaxFloor"].DBToDouble();
            lOrdem.MinQty             = lRow["MinQty"].DBToDouble();
            lOrdem.OrderQty           = lRow["OrderQty"].DBToInt32();
            lOrdem.OrderQtyRemmaining = lRow["OrderQtyRemaining"].DBToInt32();
            lOrdem.CumQty             = lRow["CumQty"].DBToInt32();
            lOrdem.OrdStatus          = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();
            lOrdem.OrdType            = (OrdemTipoEnum)lRow["OrdTypeId"].DBToInt32();
            lOrdem.RegisterTime       = lRow["RegisterTime"].DBToDateTime();
            lOrdem.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
            lOrdem.SecurityID         = lRow["SecurityExchangeId"].DBToString();


            lOrdem.Side         = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
            lOrdem.StopStartID  = lRow["StopStartID"].DBToInt32();
            lOrdem.Symbol       = lRow["Symbol"].DBToString();
            lOrdem.TimeInForce  = (OrdemValidadeEnum)lRow["TimeInForce"].DBToInt32();
            lOrdem.TransactTime = lRow["TransactTime"].DBToDateTime();
            if (lRow.Table.Columns.Contains("FixMsgSeqNum"))
                lOrdem.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();

            if (lRow["Price"] != DBNull.Value)
                lOrdem.Price = lRow["Price"].DBToDouble().Value;

            //Não é dentro de 'else' porque a primeira linha tem o primeiro acompanhamento:

            AcompanhamentoOrdemInfo lAcompanhamento = new AcompanhamentoOrdemInfo();
            lAcompanhamento.CanalNegociacao     = lRow["ChannelID"].DBToInt32();
            lAcompanhamento.CodigoDoCliente     = lRow["Account"].DBToInt32();
            lAcompanhamento.CodigoResposta      = lRow["OrderDetail_TransactId"].DBToString();
            lAcompanhamento.CodigoTransacao     = lRow["OrderDetail_TransactId"].DBToString();
            lAcompanhamento.DataAtualizacao     = lRow["TransactTime"].DBToDateTime();
            lAcompanhamento.DataOrdemEnvio      = lRow["RegisterTime"].DBToDateTime();
            lAcompanhamento.DataValidade        = lRow["ExpireDate"].DBToDateTime();
            lAcompanhamento.Direcao             = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
            lAcompanhamento.Instrumento         = lRow["Symbol"].DBToString();
            lAcompanhamento.NumeroControleOrdem = lRow["ClOrdId"].DBToString();
            lAcompanhamento.Descricao           = lRow["OrderDetail_Description"].DBToString();

            lAcompanhamento.QuantidadeRemanescente = lRow["OrderDetail_OrdQtyRemaining"].DBToInt32();
            lAcompanhamento.QuantidadeExecutada    = lRow["OrderDetail_CumQty"].DBToInt32();
            lAcompanhamento.QuantidadeSolicitada   = lRow["OrderDetail_OrderQty"].DBToInt32();
            lAcompanhamento.QuantidadeNegociada    = lRow["OrderDetail_TradeQty"].DBToInt32();

            lAcompanhamento.StatusOrdem = (OrdemStatusEnum)lRow["OrderDetail_OrderStatusId"].DBToInt32();
                
            if (lRow.Table.Columns.Contains("OrderDetail_FixMsgSeqNum"))
                lAcompanhamento.FixMsgSeqNum = lRow["OrderDetail_FixMsgSeqNum"].DBToInt32();

            if (lRow["OrderDetail_Price"] != DBNull.Value)
                lAcompanhamento.Preco = lRow["OrderDetail_Price"].DBToDecimal();

            lOrdem.Acompanhamentos.Add(lAcompanhamento);

            return lOrdem;
        }

        #endregion

        #region Métodos Públicos

        public object InserirAcompanhamentoDeOrdem(AcompanhamentoOrdemInfo pInfo)
        {
            object lIdOrdemHistorico;

            SqlConnection gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

            gConexao.Open();

            SqlCommand gComando = new SqlCommand("prc_update_order_ponso", gConexao);

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

            gConexao.Close();

            gConexao.Dispose();

            return lIdOrdemHistorico;
        }

        public void AtualizarOrdem(OrdemInfo ordemInfo)
        {
            SqlConnection gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

            gConexao.Open();

            SqlCommand gComando = new SqlCommand("prc_update_order_acomp", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gComando.Parameters.Add(new SqlParameter("@ClOrdID", ordemInfo.ClOrdID));
            gComando.Parameters.Add(new SqlParameter("@OrdStatusId", ordemInfo.OrdStatus));
            gComando.Parameters.Add(new SqlParameter("@ExchangeNumberID", ordemInfo.ExchangeNumberID));
            gComando.Parameters.Add(new SqlParameter("@OrderQtyRemaining", ordemInfo.OrderQtyRemmaining));
            gComando.Parameters.Add(new SqlParameter("@CumQty", ordemInfo.CumQty));
            gComando.Parameters.Add(new SqlParameter("@Price", ordemInfo.Price));
            gComando.Parameters.Add(new SqlParameter("@ExpireDate", ordemInfo.ExpireDate));
            gComando.Parameters.Add(new SqlParameter("@TimeInForce", (int) ordemInfo.TimeInForce));
            if (bSaveFixMsgSeqNum)
                gComando.Parameters.Add(new SqlParameter("@FixMsgSeqNum", 0));


            gComando.ExecuteNonQuery();

            gConexao.Close();

            gConexao.Dispose();
        }

        /// <summary>
        /// </summary>
        /// <param name="pInfo"></param>
        public void TrocarNumeroControleOrdem(OrdemInfo pInfo)
        {

            SqlConnection gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

            gConexao.Open();

            SqlCommand gComando = new SqlCommand("prc_troca_numerocontrole", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gComando.Parameters.Add(new SqlParameter("@pClOrdID", pInfo.ClOrdID));

            gComando.ExecuteNonQuery();

            gConexao.Close();

            gConexao.Dispose();
        }

        public List<string> BuscarOrdensValidasParaoDia()
        {

            List<string> lRetorno = new List<string>();

            SqlConnection gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

            gConexao.Open();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            SqlCommand gComando = new SqlCommand("prc_sel_ordens_abertas_dia", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.SelectCommand.Connection = gConexao;

            lAdapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i <= table.Rows.Count - 1; i++)
                {
                    lRetorno.Add(table.Rows[i]["ClOrdID"].ToString());

                }
            }

            gConexao.Close();

            gConexao.Dispose();

            return lRetorno;

        }

        public List<OrdemInfo> BuscarOrdensOnline(int pAccount, Nullable<int> pCodBmf)
        {
            List<OrdemInfo> lRetorno;

            SqlDataAdapter lAdapter;

            SqlConnection gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

            gConexao.Open();

            DataSet lDataSet = new DataSet();

            SqlCommand gComando = new SqlCommand("prc_buscar_ordens_online", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;
            
            gComando.Parameters.Add(new SqlParameter("@Account", pAccount));
            gComando.Parameters.Add(new SqlParameter("@CodBmf", pCodBmf));

            gLog4Net.Debug(string.Format("Chamando prc_buscar_ordens_online({0}, {1})", pAccount, pCodBmf));

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            lRetorno = PreencherOrdensInfoDaVwOrderDetails(lDataSet);

            gConexao.Close();

            gConexao.Dispose();

            return lRetorno;
        }

        public List<OrdemInfo> BuscarOrdens(  Nullable<int> pAccount
                                            , Nullable<int> pCodBmf
                                            , Nullable<DateTime> pDataDe
                                            , Nullable<DateTime> pDataAte
                                            , Nullable<int> pChannelId
                                            , string pSymbol
                                            , Nullable<int> pOrderStatusId
                                            , Nullable<int> pCodigoAssessor
                                            )
        {

            List<OrdemInfo> lRetorno = new List<OrdemInfo>();

            SqlDataAdapter lAdapter;

            SqlConnection gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

            gConexao.Open();

            DataSet lDataSet = new DataSet();

            SqlCommand gComando = new SqlCommand("prc_buscar_ordens", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;
            
            gComando.Parameters.Add(new SqlParameter("@Account", pAccount));
            gComando.Parameters.Add(new SqlParameter("@CodBmf", pCodBmf));
            gComando.Parameters.Add(new SqlParameter("@DataDe", pDataDe));
            gComando.Parameters.Add(new SqlParameter("@DataAte", pDataAte));
            gComando.Parameters.Add(new SqlParameter("@ChannelId", pChannelId));
            gComando.Parameters.Add(new SqlParameter("@Symbol", pSymbol));
            gComando.Parameters.Add(new SqlParameter("@OrdStatusId", pOrderStatusId));
            gComando.Parameters.Add(new SqlParameter("@CodigoAssessor", pCodigoAssessor));

            gLog4Net.Debug(string.Format("Chamando prc_buscar_ordens({0},{1},{2},{3},{4},{5},{6},{7})",
                pAccount,
                pCodBmf,
                pDataDe,
                pDataAte,
                pChannelId,
                pSymbol,
                pOrderStatusId,
                pCodigoAssessor));

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            lRetorno = PreencherOrdensInfoDaVwOrderDetails(lDataSet);

            this.BuscarAssessoresFiltro(pCodigoAssessor, ref lRetorno);

            gConexao.Close();

            gConexao.Dispose();

            return lRetorno;
        }

        public BuscarOrdensResponse BuscarOrdensHistorico(Nullable<int> pAccount
                                            , Nullable<int> pCodBmf
                                            , Nullable<DateTime> pDataDe
                                            , Nullable<DateTime> pDataAte
                                            , Nullable<int> pChannelId
                                            , string pSymbol
                                            , Nullable<int> pOrderStatusId
                                            , Nullable<int> pCodigoAssessor
                                            , Nullable<int> pTotalRegistros
                                            , int pPaginaCorrente
                                            )
        {

            BuscarOrdensResponse lRetorno = new BuscarOrdensResponse();

            SqlDataAdapter lAdapter;

            SqlConnection gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

            gConexao.Open();

            DataSet lDataSet = new DataSet();

            SqlCommand gComando = new SqlCommand("prc_buscar_ordens_historico", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gComando.Parameters.Add(new SqlParameter("@Account",         pAccount));
            gComando.Parameters.Add(new SqlParameter("@CodBmf",          pCodBmf));
            gComando.Parameters.Add(new SqlParameter("@DataDe",          pDataDe));
            gComando.Parameters.Add(new SqlParameter("@DataAte",         pDataAte));
            gComando.Parameters.Add(new SqlParameter("@ChannelId",       pChannelId));
            gComando.Parameters.Add(new SqlParameter("@Symbol",          pSymbol));
            gComando.Parameters.Add(new SqlParameter("@OrdStatusId",     pOrderStatusId));
            gComando.Parameters.Add(new SqlParameter("@CodigoAssessor",  pCodigoAssessor));
            gComando.Parameters.Add(new SqlParameter("@pPaginaCorrente", pPaginaCorrente));

            //gComando.Parameters.Add(new SqlParameter("@pTotalRegistros", pTotalRegistros));
            SqlParameter sqlParameter  = new SqlParameter();
            sqlParameter.Direction     = ParameterDirection.Output;
            sqlParameter.DbType        = DbType.Int32;
            sqlParameter.ParameterName = "@pTotalRegistros";

            gComando.Parameters.Add(sqlParameter);

            gLog4Net.Debug(string.Format("Chamando prc_buscar_ordens_historico({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})",
                pAccount,
                pCodBmf,
                pDataDe,
                pDataAte,
                pChannelId,
                pSymbol,
                pOrderStatusId,
                pCodigoAssessor,
                pTotalRegistros,
                pPaginaCorrente));

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            lRetorno.TotalItens = int.Parse(gComando.Parameters["@pTotalRegistros"].Value.ToString());

            List<OrdemInfo> lstOrdens;

            gLog4Net.Debug("Passou do Fill no SqlAdapter ....");

            try
            {
                lstOrdens = PreencherOrdensInfoDaVwOrderDetails(lDataSet);
                
                this.BuscarAssessoresFiltro(pCodigoAssessor, ref lstOrdens);
                
                lRetorno.Ordens = lstOrdens;

                gLog4Net.Debug("Passou do BuscarAssessoresFiltro ....");
            }
            catch (Exception ex)
            {
                gLog4Net.ErrorFormat("Erro em BuscarOrdensHistorico Erro - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }

            gConexao.Close();

            gConexao.Dispose();

            return lRetorno;
        }

        /// <summary>
        /// Busca assessores para filtro 
        /// </summary>
        /// <param name="cd_assessor">Código do assessor</param>
        public void BuscarAssessoresFiltro(int? cd_assessor, ref List<OrdemInfo> list)
        {
            if (list.Count().Equals(0) || cd_assessor == null)
                return;

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "Risco";

            List<int> CodBovespaBmf = new List<int>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_assessor_monitoramento_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, cd_assessor);

                DataTable lDados = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                foreach (DataRow lRow in lDados.Rows)
                {
                    if (!CodBovespaBmf.Contains(lRow["cd_bovespa"].DBToInt32()))
                    {
                        if (!lRow["cd_bovespa"].DBToInt32().Equals(0))
                            CodBovespaBmf.Add(lRow["cd_bovespa"].DBToInt32());

                        if (!lRow["cd_bmf"].DBToInt32().Equals(0))
                            CodBovespaBmf.Add(lRow["cd_bmf"].DBToInt32());
                    }
                }

                IEnumerable<OrdemInfo> listTemp = from a in list where CodBovespaBmf.Contains(a.Account) select a;

                list = listTemp.ToList<OrdemInfo>();
            }
        }

        /// <summary>
        /// Busca ordens inseridas no sinacor
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public BuscarOrdensResponse BuscarOrdensSinacor(BuscarOrdensRequest pRequest)
        {
            BuscarOrdensResponse lReturn = new BuscarOrdensResponse();

            OrdemInfo lOrdem = new OrdemInfo();

            AcessaDados lAcessaDados = new AcessaDados("Retorno");

            lAcessaDados.ConnectionStringName = "SINACOR";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ACOMPANHAMENTO_ORDENS_LST"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pDtDe", DbType.Date, pRequest.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "pDtAte", DbType.Date, pRequest.DataAte);
                lAcessaDados.AddInParameter(lDbCommand, "pPapel", DbType.AnsiString, pRequest.Instrumento);
                lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, pRequest.ContaDoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pPaginaCorrente", DbType.Int32, pRequest.PaginaCorrente);
                lAcessaDados.AddInParameter(lDbCommand, "pQtdRegs", DbType.Int32, pRequest.QtdeLimiteRegistros);
                lAcessaDados.AddInParameter(lDbCommand, "pCodAssessor", DbType.Int32, pRequest.CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "pStatusOrdem", DbType.Int32, pRequest.Status == null ? new Nullable<int>() : (int)pRequest.Status);

                lAcessaDados.AddOutParameter(lDbCommand, "pTotalRegistros", DbType.Int32, 12);

                DataTable lDados = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                lReturn.Ordens = new List<OrdemInfo>();

                lReturn.TotalItens = pRequest.Status == null ? int.Parse(lDbCommand.Parameters["pTotalRegistros"].Value.ToString()) : lDados.Rows.Count;

                foreach (DataRow lRow in lDados.Rows)
                {
                    lOrdem = new OrdemInfo();

                    lOrdem.IdOrdem            = Convert.ToInt32(lRow["OrderId"]);
                    lOrdem.Account            = lRow["CD_CLIENTE"].DBToInt32();
                    lOrdem.ChannelID          = lRow["ChannelId"].DBToInt32();
                    lOrdem.ExpireDate         = lRow["ExpireDate"].DBToDateTime();
                    lOrdem.OrderQty           = lRow["OrderQty"].DBToInt32();
                    lOrdem.OrderQtyRemmaining = lRow["OrderQtyRemaining"].DBToInt32();
                    lOrdem.OrdStatus          = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();
                    lOrdem.ClOrdID            = lRow["OrderId"].ToString();
                    lOrdem.OrdType            = (OrdemTipoEnum)50;
                    lOrdem.RegisterTime       = DateTime.ParseExact(lRow["RegisterTime"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture );
                    lOrdem.Side               = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                    lOrdem.Symbol             = lRow["SYMBOL"].DBToString();

                    if (lRow["Price"] != DBNull.Value)
                        lOrdem.Price = lRow["Price"].DBToDouble().Value;

                    lOrdem.Acompanhamentos = new List<AcompanhamentoOrdemInfo>();

                    lReturn.Ordens.Add(lOrdem);
                }
            }

            return lReturn;
        }

        public BuscarOrdensResponse BuscarOrdensSinacorBmf(BuscarOrdensRequest pRequest)
        {
            BuscarOrdensResponse lReturn = new BuscarOrdensResponse();

            OrdemInfo lOrdem = new OrdemInfo();

            AcessaDados lAcessaDados = new AcessaDados("Retorno");

            lAcessaDados.ConnectionStringName = "SINACOR";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ACOMPANHAMENTO_BMF_LST"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pDtDe", DbType.Date, pRequest.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "pDtAte", DbType.Date, pRequest.DataAte);
                lAcessaDados.AddInParameter(lDbCommand, "pPapel", DbType.AnsiString, pRequest.Instrumento);
                lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, pRequest.ContaDoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pPaginaCorrente", DbType.Int32, pRequest.PaginaCorrente);
                lAcessaDados.AddInParameter(lDbCommand, "pQtdRegs", DbType.Int32, pRequest.QtdeLimiteRegistros);
                lAcessaDados.AddInParameter(lDbCommand, "pCodAssessor", DbType.Int32, pRequest.CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "pStatusOrdem", DbType.Int32, pRequest.Status == null ? new Nullable<int>() : (int)pRequest.Status);

                lAcessaDados.AddOutParameter(lDbCommand, "pTotalRegistros", DbType.Int32, 12);

                DataTable lDados = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                lReturn.Ordens = new List<OrdemInfo>();

                lReturn.TotalItens = pRequest.Status == null ? int.Parse(lDbCommand.Parameters["pTotalRegistros"].Value.ToString()) : lDados.Rows.Count;

                foreach (DataRow lRow in lDados.Rows)
                {
                    lOrdem = new OrdemInfo();

                    lOrdem.IdOrdem            = Convert.ToInt32(lRow["OrderId"]);
                    lOrdem.Account            = lRow["CD_CLIENTE"].DBToInt32();
                    lOrdem.ChannelID          = lRow["ChannelId"].DBToInt32();
                    lOrdem.ExpireDate         = lRow["ExpireDate"].DBToDateTime();
                    lOrdem.OrderQty           = lRow["OrderQty"].DBToInt32();
                    lOrdem.OrderQtyRemmaining = lRow["OrderQtyRemaining"].DBToInt32();
                    lOrdem.OrdStatus          = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();
                    lOrdem.ClOrdID            = lRow["OrderId"].ToString();
                    lOrdem.OrdType            = (OrdemTipoEnum)50;
                    lOrdem.RegisterTime       = DateTime.ParseExact(lRow["RegisterTime"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture);
                    lOrdem.Side               = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                    lOrdem.Symbol             = lRow["SYMBOL"].DBToString();

                    if (lRow["Price"] != DBNull.Value)
                        lOrdem.Price = lRow["Price"].DBToDouble().Value;

                    lOrdem.Acompanhamentos = new List<AcompanhamentoOrdemInfo>();

                    lReturn.Ordens.Add(lOrdem);
                }
            }

            return lReturn;
        }


        /// <summary>
        /// Busca as ordens stop start 
        /// </summary>
        /// <param name="pInfo">Entidade do tipo BuscarOrdensStopStartRequest</param>
        /// <returns>Retorna um List do tipo OrdemStopStartInfo</returns>
        public List<OrdemStopStartInfo> BuscarOrdensStopStart(BuscarOrdensStopStartRequest pInfo)
        {
            SqlCommand command = new SqlCommand();

            OrdemStopStartInfo _TOrdem = new OrdemStopStartInfo();

            List<OrdemStopStartInfo> _ListOrdem = new List<OrdemStopStartInfo>();

            OrdemStopStartInfoDetalhe lDetalhe;

            string lUltimoId = "";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString;
            conn.Open();

            try
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "prc_buscar_ordens_stop_start";
                
                command.Parameters.AddWithValue("@Account", pInfo.Account);
                command.Parameters.AddWithValue("@CodBmf", pInfo.CodBmf);
                command.Parameters.AddWithValue("@DataDe", pInfo.DataDe);
                command.Parameters.AddWithValue("@DataAte", pInfo.DataAte);
                command.Parameters.AddWithValue("@Symbol", pInfo.Symbol);
                command.Parameters.AddWithValue("@StopStartStatusID", pInfo.OrderStatusId);
                command.Parameters.AddWithValue("@CodigoAssessor", pInfo.CodigoAssessor);

                DataTable dtDados = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dtDados);

                if (dtDados.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDados.Rows.Count - 1; i++)
                    {
                        if (lUltimoId != dtDados.Rows[i]["StopStartID"].ToString())
                        {
                            _TOrdem = new OrdemStopStartInfo();
                            _TOrdem.Details = new List<OrdemStopStartInfoDetalhe>();

                            _TOrdem.Account                = int.Parse(dtDados.Rows[i]["Account"].ToString());
                            _TOrdem.StopStartID            = int.Parse(dtDados.Rows[i]["StopStartID"].ToString());
                            _TOrdem.Symbol                 = dtDados.Rows[i]["Symbol"].ToString();
                            _TOrdem.IdStopStartTipo        = (StopStartTipoEnum)dtDados.Rows[i]["StopStartTipoEnum"];
                            _TOrdem.OrdTypeID              = int.Parse(dtDados.Rows[i]["OrdTypeID"].ToString());
                            _TOrdem.IdBolsa                = dtDados.Rows[i]["id_Bolsa"].ToString().DBToInt32();
                            _TOrdem.InitialMovelPrice      = dtDados.Rows[i]["InitialMovelPrice"].DBToDecimal();
                            _TOrdem.OrderQty               = int.Parse(dtDados.Rows[i]["OrderQty"].ToString());
                            _TOrdem.ReferencePrice         = dtDados.Rows[i]["ReferencePrice"].DBToDecimal();
                            _TOrdem.SendStartPrice         = dtDados.Rows[i]["SendStartPrice"].DBToDecimal();
                            _TOrdem.SendStopGainPrice      = dtDados.Rows[i]["SendStopGainPrice"].DBToDecimal();
                            _TOrdem.SendStopLossValuePrice = dtDados.Rows[i]["SendStopLossValuePrice"].DBToDecimal();
                            _TOrdem.StartPriceValue        = dtDados.Rows[i]["StartPriceValue"].DBToDecimal();
                            _TOrdem.StopGainValuePrice     = dtDados.Rows[i]["StopGainValuePrice"].DBToDecimal();
                            _TOrdem.StopLossValuePrice     = dtDados.Rows[i]["StopLossValuePrice"].DBToDecimal();
                            _TOrdem.StopStartStatusID      = int.Parse(dtDados.Rows[i]["StopStartStatusID"].ToString());
                            _TOrdem.RegisterTime           = dtDados.Rows[i]["RegisterTime"].DBToDateTimeNullable();
                            _TOrdem.ExecutionTime          = dtDados.Rows[i]["ExecutionTime"].DBToDateTimeNullable();
                            _TOrdem.ExpireDate             = dtDados.Rows[i]["ExpireDate"].DBToDateTimeNullable();
                            _TOrdem.AdjustmentMovelPrice   = dtDados.Rows[i]["AdjustmentMovelPrice"].DBToDecimal();

                            _ListOrdem.Add(_TOrdem);

                            lUltimoId = _TOrdem.StopStartID.ToString();
                        }

                        lDetalhe = new OrdemStopStartInfoDetalhe();

                        lDetalhe.OrderStatusDescription = dtDados.Rows[i]["OrderStatusDescription"].DBToString();
                        lDetalhe.RegisterTime           = dtDados.Rows[i]["RegisterTimeDetail"].DBToDateTimeNullable();
                        lDetalhe.StopStartStatusID      = dtDados.Rows[i]["OrderStatusID"].DBToInt32();


                        _ListOrdem[_ListOrdem.Count - 1].Details.Add(lDetalhe);
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

        /// <summary>
        /// BuscarOrdemOriginal - resgata uma ordem gravada na tabela TBOrderUpdated
        /// </summary>
        /// <param name="ordemalterada">OrdemInfo da ordem alterada</param>
        /// <returns>objeto tipo OrdemInfo</returns>
        public OrdemInfo BuscarOrdemOriginal( OrdemInfo ordemalterada )
        {
            OrdemInfo lOrdem = this._CloneOrdemInfo(ordemalterada);

            // Clona as informacoes da ordem
            lOrdem.ClOrdID = ordemalterada.OrigClOrdID;

            SqlDataAdapter lAdapter;

            SqlConnection gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

            gConexao.Open();

            DataSet lDataSet = new DataSet();

            SqlCommand gComando = new SqlCommand("prc_buscar_ordem_original", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gComando.Parameters.Add(new SqlParameter("@ClOrdIdOriginal", ordemalterada.OrigClOrdID));

            gLog4Net.DebugFormat("Chamando prc_buscar_ordem_original({0})", ordemalterada.OrigClOrdID);

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            if ( lDataSet.Tables[0].Rows.Count > 0 )
            {
                DataRow lRow = lDataSet.Tables[0].Rows[0];

                lOrdem.IdOrdem = Convert.ToInt32(lRow["OrderId"]);
                lOrdem.OrdStatus = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();

                if (lRow["Price"] != DBNull.Value)
                    lOrdem.Price = lRow["Price"].DBToDouble().Value;

                lOrdem.OrderQty = lRow["quantidade"].DBToInt32();
                lOrdem.MaxFloor = lRow["quantidade_aparente"].DBToDouble();
                lOrdem.MinQty = lRow["quantidade_minima"].DBToDouble();
                lOrdem.CumQty = lRow["quantidade_exec"].DBToInt32();
                lOrdem.ExpireDate = lRow["dt_validade"].DBToDateTime();
                lOrdem.TransactTime = lRow["dt_atualizacao"].DBToDateTime();
                lOrdem.RegisterTime = lRow["dt_atualizacao"].DBToDateTime();
                lOrdem.OrderQtyRemmaining = lOrdem.OrderQty - lOrdem.CumQty;

                gLog4Net.DebugFormat("Restaurando: id:{0} qtde:{1} exec:{2} min:{3} reman:{4} prc:{5} st:{6}",
                     lOrdem.ClOrdID,
                     lOrdem.OrderQty,
                     lOrdem.CumQty,
                     lOrdem.MinQty,
                     lOrdem.OrderQtyRemmaining,
                     lOrdem.Price,
                     lOrdem.OrdStatus);
            }
            else
                gLog4Net.ErrorFormat("NAO ENCONTROU BACKUP DA ORDEM: id:{0} qtde:{1} exec:{2} min:{3} reman:{4} prc:{5} st:{6}",
                     lOrdem.ClOrdID,
                     lOrdem.OrderQty,
                     lOrdem.CumQty,
                     lOrdem.MinQty,
                     lOrdem.OrderQtyRemmaining,
                     lOrdem.Price,
                     lOrdem.OrdStatus);

            gConexao.Close();

            gConexao.Dispose();

            return lOrdem;
        }


        /// <summary>
        /// BuscarOrdemOriginal - resgata uma ordem gravada na tabela TBOrderUpdated
        /// </summary>
        /// <param name="ordemalterada">OrdemInfo da ordem alterada</param>
        /// <returns>objeto tipo OrdemInfo</returns>
        public OrdemInfo BuscarOrdemTBOrder(string ClOrdId)
        {
            OrdemInfo lOrdem = new OrdemInfo();

            SqlDataAdapter lAdapter;

            SqlConnection gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

            gConexao.Open();

            DataSet lDataSet = new DataSet();
            SqlCommand gComando = new SqlCommand("prc_buscar_ordem", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gComando.Parameters.Add(new SqlParameter("@ClOrdId", ClOrdId));

            gLog4Net.DebugFormat("Chamando prc_buscar_ordem({0})", ClOrdId);

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            if (lDataSet.Tables[0].Rows.Count > 0)
            {
                DataRow lRow = lDataSet.Tables[0].Rows[0];

                lOrdem.IdOrdem = Convert.ToInt32(lRow["OrderId"]);
                lOrdem.OrdStatus = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();

                if (lRow["Price"] != DBNull.Value)
                    lOrdem.Price = lRow["Price"].DBToDouble().Value;

                lOrdem.OrderQty = lRow["quantidade"].DBToInt32();
                lOrdem.MaxFloor = lRow["quantidade_aparente"].DBToDouble();
                lOrdem.MinQty = lRow["quantidade_minima"].DBToDouble();
                lOrdem.CumQty = lRow["quantidade_exec"].DBToInt32();
                lOrdem.ExpireDate = lRow["dt_validade"].DBToDateTime();
                lOrdem.TransactTime = lRow["dt_atualizacao"].DBToDateTime();
                lOrdem.RegisterTime = lRow["dt_atualizacao"].DBToDateTime();
                lOrdem.OrderQtyRemmaining = lOrdem.OrderQty - lOrdem.CumQty;

                gLog4Net.DebugFormat("Restaurando: id:{0} qtde:{1} exec:{2} min:{3} reman:{4} prc:{5} st:{6}",
                     lOrdem.ClOrdID,
                     lOrdem.OrderQty,
                     lOrdem.CumQty,
                     lOrdem.MinQty,
                     lOrdem.OrderQtyRemmaining,
                     lOrdem.Price,
                     lOrdem.OrdStatus);
            }

            gConexao.Close();

            gConexao.Dispose();

            return lOrdem;
        }

        /// <summary>
        /// BuscarOrdem - retorna os dados de uma ordem gravada na TbOrder
        /// </summary>
        /// <returns>objeto tipo OrdemInfo</returns>
        public BuscarOrdemResponse BuscarOrdem(String ClOrdId)
        {
            BuscarOrdemResponse lReturn = new BuscarOrdemResponse();

            SqlDataAdapter lAdapter;

            SqlConnection gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

            gConexao.Open();

            DataSet lDataSet = new DataSet();

            SqlCommand gComando = new SqlCommand("prc_buscar_ordem", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gComando.Parameters.Add(new SqlParameter("@ClOrdId", ClOrdId));

            gLog4Net.DebugFormat("Chamando prc_buscar_ordem({0})", ClOrdId);

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            if (lDataSet.Tables[0].Rows.Count > 0)
            {
                DataRow lRow = lDataSet.Tables[0].Rows[0];

                lReturn.IdOrdem                 = Convert.ToInt32(lRow["OrderId"]);
                lReturn.OrdStatus               = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();
                lReturn.Account                 = int.Parse(lRow["Account"].ToString());
                lReturn.Symbol                  = lRow["Symbol"].ToString();
                lReturn.OrdType                 = (OrdemTipoEnum)lRow["OrdTypeId"].DBToInt32();
                lReturn.Side                    = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                lReturn.OrderQty                = lRow["OrderQty"].DBToInt32();
                lReturn.OrderQtyRemmaining      = lRow["OrderQtyRemaining"].DBToInt32();

                if (lRow["Price"] != DBNull.Value)
                    lReturn.Price = lRow["Price"].DBToDouble().Value;
            }

            gConexao.Close();

            gConexao.Dispose();

            return lReturn;
        }

        /// <summary>
        /// Retorna um dicionario com as contas BMF que tem codigo diferente do Bovespa
        /// para correto mapeamento das ordens em memoria
        /// </summary>
        public Dictionary<int,int> BuscarCodigoBovespaBMFDiferentes()
        {
            Dictionary<int, int> retorno = new Dictionary<int, int>();

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "DirectTradeCadastro";

            List<int> CodBovespaBmf = new List<int>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_bov_bmf_diferentes"))
            {
                DataTable lDados = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                foreach (DataRow lRow in lDados.Rows)
                {
                    int cd_bov = lRow["cd_bov"].DBToInt32();
                    int cd_bmf = lRow["cd_bmf"].DBToInt32();

                    if ( !retorno.ContainsKey(cd_bov) )
                        retorno.Add(cd_bov, cd_bmf);
                }
            }

            return retorno;
        }

        public List<OrdemInfo> BuscarOrdensAtivas()
        {
            List<OrdemInfo> lRetorno;

            SqlDataAdapter lAdapter;

            SqlConnection gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings[
                                            ConfigurationManager.AppSettings["ConexaoEmUso"]].ConnectionString);

            gConexao.Open();

            DataSet lDataSet = new DataSet();

            SqlCommand gComando = new SqlCommand("prc_buscar_ordens_ativas", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gLog4Net.Debug(string.Format("Chamando prc_buscar_ordens_ativas"));

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            lRetorno = PreencherOrdensInfoDaVwOrderDetails(lDataSet);

            gConexao.Close();

            gConexao.Dispose();

            return lRetorno;
        }

        #endregion
    }
}
