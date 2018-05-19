#region Includes
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using log4net;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Linq;
using System.Globalization;
#endregion

namespace Gradual.OMS.ServicoA4S
{
    public class CamadaDeDados
    {
        #region Globais

        private SqlConnection gConexao;

        private SqlCommand gComando;

        private static readonly ILog gLog4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _connSinacor = "TRADE";

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

        #region Métodos Private

        public void AbrirConexao()
        {
            if (!this.ConexaoAberta)
            {
                gConexao = new SqlConnection(ConfigurationManager.ConnectionStrings["Acompanhamento"].ConnectionString);

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
                    lOrdem.OrigClOrdID        = lRow["OrigClOrdID"].DBToString();
                    lOrdem.ExchangeNumberID   = lRow["ExchangeNumberId"].DBToString();
                    lOrdem.ExecBroker         = lRow["ExecBroker"].DBToString();
                    lOrdem.ExpireDate         = lRow["ExpireDate"].DBToDateTime();
                    lOrdem.MaxFloor           = lRow["MaxFloor"].DBToDouble();
                    lOrdem.MinQty             = lRow["MinQty"].DBToDouble();
                    lOrdem.OrderQty           = lRow["OrderQty"].DBToInt32();
                    lOrdem.OrderQtyRemmaining = lRow["OrderQtyRemaining"].DBToInt32();
                    lOrdem.CumQty             = lRow["CumQty"].DBToInt32();
                    lOrdem.OrdStatus          = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();
                    lOrdem.OrdType            = (OrdemTipoEnum)lRow
                        ["OrdTypeId"].DBToInt32();

                    lOrdem.RegisterTime       = lRow["RegisterTime"].DBToDateTime();
                    lOrdem.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                    lOrdem.SecurityID         = lRow["SecurityExchangeId"].DBToString();


                    lOrdem.Side         = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                    lOrdem.StopStartID  = lRow["StopStartID"].DBToInt32();
                    lOrdem.Symbol       = lRow["Symbol"].DBToString();
                    lOrdem.TimeInForce  = (OrdemValidadeEnum)lRow["TimeInForce"].DBToInt32();
                    lOrdem.TransactTime = lRow["TransactTime"].DBToDateTime();
                    lOrdem.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                    lOrdem.CompIDOMS    = lRow["SystemID"].DBToString();

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
                lAcompanhamento.FixMsgSeqNum = lRow["OrderDetail_FixMsgSeqNum"].DBToInt32();

                if (lRow["OrderDetail_Price"] != DBNull.Value)
                {
                    lAcompanhamento.Preco = lRow["OrderDetail_Price"].DBToDecimal();
                    lAcompanhamento.LastPx = lRow["OrderDetail_Price"].DBToDecimal(); ;
                }

                lRetorno[lRetorno.Count - 1].Acompanhamentos.Add(lAcompanhamento);

            }

            return lRetorno;
        }

        private OrdemInfo _CloneOrdemInfo(OrdemInfo info)
        {
            OrdemInfo clone = new OrdemInfo();

            clone.Account = info.Account;
            clone.ChannelID = info.ChannelID;
            clone.ClOrdID = info.ClOrdID;
            clone.CumQty = info.CumQty;
            clone.Exchange = info.Exchange;
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
            clone.CompIDOMS = info.CompIDOMS;

            clone.Acompanhamentos.AddRange(info.Acompanhamentos.ToArray());

            return clone;
        }

        #endregion

        #region Métodos Públicos

        public object InserirAcompanhamentoDeOrdem(AcompanhamentoOrdemInfo pInfo)
        {
            object lIdOrdemHistorico;

            gComando = new SqlCommand("prc_update_order", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gComando.Parameters.Add(new SqlParameter("@ClOrdID", pInfo.NumeroControleOrdem));
            gComando.Parameters.Add(new SqlParameter("@OrdStatusId", pInfo.StatusOrdem));
            gComando.Parameters.Add(new SqlParameter("@ExchangeNumberID", pInfo.CodigoResposta));
            gComando.Parameters.Add(new SqlParameter("@OrderQtyRemaining", pInfo.QuantidadeRemanescente));
            gComando.Parameters.Add(new SqlParameter("@CumQty", pInfo.QuantidadeExecutada));
            gComando.Parameters.Add(new SqlParameter("@FixMsgSeqNum", pInfo.FixMsgSeqNum));


            gComando.ExecuteNonQuery();

            gComando.CommandText = "prc_ins_order_detail";

            gComando.Parameters.Clear();

            gComando.Parameters.Add(new SqlParameter("@TransactId", pInfo.CodigoTransacao));
            gComando.Parameters.Add(new SqlParameter("@ClOrdID", pInfo.NumeroControleOrdem));
            gComando.Parameters.Add(new SqlParameter("@OrderQty", pInfo.QuantidadeSolicitada));
            gComando.Parameters.Add(new SqlParameter("@OrdQtyRemaining", pInfo.QuantidadeRemanescente));
            gComando.Parameters.Add(new SqlParameter("@CumQty", pInfo.QuantidadeExecutada));
            gComando.Parameters.Add(new SqlParameter("@TradeQty", pInfo.QuantidadeNegociada));
            gComando.Parameters.Add(new SqlParameter("@Price", pInfo.Preco));
            gComando.Parameters.Add(new SqlParameter("@OrderStatusID", pInfo.StatusOrdem));
            gComando.Parameters.Add(new SqlParameter("@Description", pInfo.Descricao));
            gComando.Parameters.Add(new SqlParameter("@FixMsgSeqNum", pInfo.FixMsgSeqNum));

            lIdOrdemHistorico = gComando.ExecuteScalar();


            return lIdOrdemHistorico;
        }


        public List<string> BuscarOrdensValidasParaoDia()
        {

            List<string> lRetorno = new List<string>();

            SqlDataAdapter lAdapter;

            DataTable table = new DataTable();

            gComando = new SqlCommand("prc_sel_ordens_abertas_dia", gConexao);

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

            return lRetorno;

        }

        public List<OrdemInfo> BuscarOrdensOnline(int pAccount, Nullable<int> pCodBmf)
        {
            List<OrdemInfo> lRetorno;

            SqlDataAdapter lAdapter;

            DataSet lDataSet = new DataSet();

            gComando = new SqlCommand("prc_buscar_ordens_online", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;
            
            gComando.Parameters.Add(new SqlParameter("@Account", pAccount));
            gComando.Parameters.Add(new SqlParameter("@CodBmf", pCodBmf));

            gLog4Net.Debug(string.Format("Chamando prc_buscar_ordens_online({0}, {1})", pAccount, pCodBmf));

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            lRetorno = PreencherOrdensInfoDaVwOrderDetails(lDataSet);

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

            DataSet lDataSet = new DataSet();

            gComando = new SqlCommand("prc_buscar_ordens", gConexao);

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


        public List<OrdemInfo> BuscarOrdensStreamer()
        {
            List<OrdemInfo> lRetorno;

            SqlDataAdapter lAdapter;

            DataSet lDataSet = new DataSet();

            gComando = new SqlCommand("prc_buscar_ordens_streamer", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gLog4Net.Debug("Chamando prc_buscar_ordens_streamer()");

            lAdapter = new SqlDataAdapter(gComando);

            lAdapter.Fill(lDataSet);

            lRetorno = PreencherOrdensInfoDaVwOrderDetails(lDataSet);

            return lRetorno;
        }

        /// <summary>
        /// BuscarOrdemOriginal - resgata uma ordem gravada na tabela TBOrderUpdated
        /// </summary>
        /// <param name="ordemalterada">OrdemInfo da ordem alterada</param>
        /// <returns>objeto tipo OrdemInfo</returns>
        public OrdemInfo BuscarOrdemOriginal(OrdemInfo ordemalterada)
        {
            OrdemInfo lOrdem = this._CloneOrdemInfo(ordemalterada);

            // Clona as informacoes da ordem
            lOrdem.ClOrdID = ordemalterada.OrigClOrdID;
            //lOrdem.OrigClOrdID = String.Empty;

            SqlDataAdapter lAdapter;

            AbrirConexao();

            DataSet lDataSet = new DataSet();

            gComando = new SqlCommand("prc_buscar_ordem_original", gConexao);

            gComando.CommandType = System.Data.CommandType.StoredProcedure;

            gComando.Parameters.Add(new SqlParameter("@ClOrdIdOriginal", ordemalterada.OrigClOrdID));

            gLog4Net.DebugFormat("Chamando prc_buscar_ordem_original({0})", ordemalterada.OrigClOrdID);

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

            FecharConexao();

            return lOrdem;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="portasoms"></param>
        /// <param name="contas"></param>
        /// <returns></returns>
        public List<OrdemInfo> BuscarOrdensOutrasPlataformas(string portasoms, string contas, DateTime ultimaconsulta)
        {
            List<OrdemInfo> lResposta = new List<OrdemInfo>();

            try
            {
                AcessaDados acesso = new AcessaDados("Retorno");

                //DbConnection dbConn = null;
                //dbConn = DbProviderFactories.GetFactory("System.Data.OracleClient").CreateConnection();
                //dbConn.ConnectionString = ConfigurationManager.ConnectionStrings["TRADE"].ConnectionString;
                //dbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                //dbConn.Open();

                char [] trimchar = {','};

                string dbcontas = contas.TrimEnd(trimchar);

                acesso.ConnectionStringName = this._connSinacor;

                gLog4Net.Debug("Buscando ordens Bovespa no Sinacor [" + dbcontas + "]");

                // Buscar ordens Bovespa
                using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_ACOMP_BOV_ONLINE"))
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "PRC_ACOMP_BOV_ONLINE";

                    acesso.AddInParameter(cmd, "pDtInicio", DbType.Date, ultimaconsulta);
                    acesso.AddInParameter(cmd, "pCodClientes", DbType.AnsiString, dbcontas);
                    acesso.AddInParameter(cmd, "pDtFinal", DbType.Date, DateTime.Now.AddMinutes(1));

                    DataTable table = acesso.ExecuteOracleDataTable(cmd);

                    for (int i = 0; i < table.Rows.Count; i++)
                        lResposta.Add(_PreencherOrdemInfo(table.Rows[i]));
                }

                gLog4Net.Debug("Buscando ordens BMF no Sinacor");

                // Buscar ordens BMF
                using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_ACOMP_BMF_ONLINE"))
                {
                    acesso.AddInParameter(cmd, "pDtInicio", DbType.Date, ultimaconsulta);
                    acesso.AddInParameter(cmd, "pCodClientes", DbType.AnsiString, contas);
                    acesso.AddInParameter(cmd, "pDtFinal", DbType.Date, DateTime.Now.AddMinutes(1));

                    DataTable table = acesso.ExecuteOracleDataTable(cmd);

                    for (int i = 0; i < table.Rows.Count; i++)
                        lResposta.Add(_PreencherOrdemInfo(table.Rows[i]));
                }

                gLog4Net.Debug("Fim da busca por ordens no Sinacor");

                return lResposta;
            }
            catch (Exception ex)
            {
                gLog4Net.Error(ex.Message, ex);
                throw (ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="portasoms"></param>
        /// <param name="contas"></param>
        /// <returns></returns>
        public List<OrdemInfo> BuscarOrdensSpider()
        {
            List<OrdemInfo> lRetorno = new List<OrdemInfo>();

            try
            {

                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString);

                sqlConn.Open();

                SqlDataAdapter lAdapter;

                DataSet lDataSet = new DataSet();

                gComando = new SqlCommand("prc_buscar_ordens_fix_streamer", gConexao);

                gComando.CommandType = System.Data.CommandType.StoredProcedure;

                gLog4Net.Debug("Chamando prc_buscar_ordens_fix_streamer()");

                lAdapter = new SqlDataAdapter(gComando);

                lAdapter.Fill(lDataSet);

                lRetorno = PreencherOrdensInfoDaVwOrderDetails(lDataSet);
            }
            catch (Exception ex)
            {
                gLog4Net.Error("BuscarOrdensSpider():" + ex.Message, ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portasoms"></param>
        /// <param name="contas"></param>
        /// <returns></returns>
        public List<OrdemInfo> BuscarOrdemSpider(string clOrdID)
        {
            List<OrdemInfo> lRetorno = new List<OrdemInfo>();

            try
            {

                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString);

                sqlConn.Open();

                SqlDataAdapter lAdapter;

                DataSet lDataSet = new DataSet();

                gComando = new SqlCommand("prc_buscar_ordem_fix_streamer", gConexao);

                gComando.CommandType = System.Data.CommandType.StoredProcedure;

                gComando.Parameters.Add(new SqlParameter("@ClOrdId", clOrdID));

                gLog4Net.Debug("Chamando prc_buscar_ordem_fix_streamer(" + clOrdID + ")");

                lAdapter = new SqlDataAdapter(gComando);

                lAdapter.Fill(lDataSet);

                lRetorno = PreencherOrdensInfoDaVwOrderDetails(lDataSet);
            }
            catch (Exception ex)
            {
                gLog4Net.Error("BuscarOrdensSpider():" + ex.Message, ex);
            }

            return lRetorno;
        }



        public List<string> BuscarClientesAtivosHoje()
        {
            List<string> lResposta = new List<string>();

            //string sqlQuery = "SELECT DISTINCT(TORMOVD.CD_CLIENTE) AS CLIENTE ";
            //sqlQuery += " FROM  TORMOVD ";
            //sqlQuery += " INNER JOIN  TSCCLIBOL ";
            //sqlQuery += " ON(TORMOVD.CD_CLIENTE = TSCCLIBOL.CD_CLIENTE) ";
            //sqlQuery += " WHERE TSCCLIBOL.IN_SITUAC = 'A' ";
            //sqlQuery += " AND TORMOVD.DT_DATORD >= trunc(sysdate) ";

            try
            {
                AcessaDados acesso = new AcessaDados("Retorno");

                acesso.ConnectionStringName = this._connSinacor;

                gLog4Net.Debug("Buscando clientes que operaram hoje" );

                // Buscar ordens Bovespa
                using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_RELACAO_CLIENTE_ENVIADAS"))
                {
                    DataTable table = acesso.ExecuteOracleDataTable(cmd);

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        lResposta.Add(table.Rows[i]["CD_CLIENTE"].DBToInt32().ToString());
                    }

                }

                gLog4Net.Debug("Fim da busca por clientes ativos hoje: " + lResposta.Count + " clientes");

                return lResposta;
            }
            catch (Exception ex)
            {
                gLog4Net.Error("BuscarClientesAtivosHoje():" + ex.Message, ex);
                throw (ex);
            }

        }

        private OrdemInfo _PreencherOrdemInfo(DataRow dataRow)
        {
            OrdemInfo ret = new OrdemInfo();

            ret.Account = dataRow["CD_CLIENTE"].DBToInt32();
            ret.ChannelID = dataRow["CHANNELID"].DBToInt32();
            ret.ClOrdID = dataRow["ORDERID"].DBToString();
            ret.OrderQty = dataRow["ORDERQTY"].DBToInt32();
            ret.OrderQtyRemmaining = dataRow["ORDERQTYREMAINING"].DBToInt32();
            ret.CumQty = dataRow["QT_ORDEXEC"].DBToInt32();


            int orderstatus = dataRow["ORDSTATUSID"].DBToInt32();
            switch(orderstatus)
            {
                case 0: ret.OrdStatus = OrdemStatusEnum.NOVA; break;
                case 1: ret.OrdStatus = OrdemStatusEnum.PARCIALMENTEEXECUTADA; break;
                case 2: ret.OrdStatus = OrdemStatusEnum.EXECUTADA; break;
                case 4: ret.OrdStatus = OrdemStatusEnum.CANCELADA; break;
                default:
                    ret.OrdStatus = OrdemStatusEnum.EXPIRADA; break;
            }

            int orderside = dataRow["SIDE"].DBToInt32();
            if (orderside == 2 )
                ret.Side = OrdemDirecaoEnum.Venda;
            else
                ret.Side = OrdemDirecaoEnum.Compra;

            ret.RegisterTime = DateTime.ParseExact(dataRow["REGISTERTIME"].DBToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            ret.ExpireDate = dataRow["EXPIREDATE"].DBToDateTime();
            ret.Price = dataRow["PRICE"].DBToDouble().Value;
            ret.Symbol = dataRow["SYMBOL"].DBToString();

            //TODO: preencher os campos
            gLog4Net.DebugFormat("ORDID[{0}] CLI[{1}] CH[{2}] SIDE[{3}] QTY[{4}] ST[{5}] PR[{6}] SYMB[{7}]",
                ret.ClOrdID,
                ret.Account,
                ret.ChannelID,
                orderside,
                ret.OrderQty,
                orderstatus,
                ret.Price,
                ret.Symbol);

            return ret;
        }


        #endregion
    }
}
