using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using System.Data;
using System.Configuration;
using log4net;
using Gradual.Core.OMS.DropCopy.Lib.Util;
using Gradual.Core.OMS.DropCopy.Lib.Dados;

namespace Gradual.OMS.ServicoA4S
{
    public class PersistenciaSpiderDB
    {

        private static readonly ILog gLog4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
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

                SqlCommand gComando = new SqlCommand("prc_buscar_ordens_fix_streamer", sqlConn);

                gComando.CommandType = System.Data.CommandType.StoredProcedure;

                gLog4Net.Debug("Chamando prc_buscar_ordens_fix_streamer()");

                lAdapter = new SqlDataAdapter(gComando);

                lAdapter.Fill(lDataSet);

                lRetorno = preencherOrdemInfo(lDataSet);
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
        public List<OrdemInfo> BuscarOrdemSpider(int orderID)
        {
            List<OrdemInfo> lRetorno = new List<OrdemInfo>();

            try
            {

                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString);

                sqlConn.Open();

                SqlDataAdapter lAdapter;

                DataSet lDataSet = new DataSet();

                SqlCommand gComando = new SqlCommand("prc_buscar_ordem_fix_streamer", sqlConn);

                gComando.CommandType = System.Data.CommandType.StoredProcedure;

                gComando.Parameters.Add(new SqlParameter("@OrderID", orderID));

                gLog4Net.Debug("Chamando prc_buscar_ordem_fix_streamer(" + orderID + ")");

                lAdapter = new SqlDataAdapter(gComando);

                lAdapter.Fill(lDataSet);

                lRetorno = preencherOrdemInfo(lDataSet);
            }
            catch (Exception ex)
            {
                gLog4Net.Error("BuscarOrdemSpider():" + ex.Message, ex);
            }

            return lRetorno;
        }


        private List<OrdemInfo> preencherOrdemInfo(DataSet pDataSetDeResultado)
        {
            string lUltimoId = "";

            List<OrdemInfo> lRetorno = new List<OrdemInfo>();

            OrdemInfo lOrdem;
            AcompanhamentoOrdemInfo lAcompanhamento;

            foreach (DataRow lRow in pDataSetDeResultado.Tables[0].Rows)
            {
                string idordem = lRow["ClOrdId"].DBToString() + lRow["OrderId"].DBToString();
                if (!lUltimoId.Equals(idordem))
                {
                    lOrdem = new OrdemInfo();

                    lOrdem.Acompanhamentos = new List<AcompanhamentoOrdemInfo>();

                    lOrdem.IdOrdem = Convert.ToInt32(lRow["OrderId"]);
                    lOrdem.Account = lRow["Account"].DBToInt32();
                    lOrdem.ChannelID = lRow["ChannelId"].DBToInt32();
                    lOrdem.ClOrdID = lRow["ClOrdId"].DBToString() + "-" + lRow["OrderId"].DBToString();
                    lOrdem.OrigClOrdID = lRow["OrigClOrdID"].DBToString() + "-" + lRow["OrderId"].DBToString();
                    lOrdem.ExchangeNumberID = lRow["ExchangeNumberId"].DBToString();
                    lOrdem.ExecBroker = lRow["ExecBroker"].DBToString();
                    lOrdem.ExpireDate = lRow["ExpireDate"].DBToDateTime();
                    lOrdem.MaxFloor = lRow["MaxFloor"].DBToDouble();
                    lOrdem.MinQty = lRow["MinQty"].DBToDouble();
                    lOrdem.OrderQty = lRow["OrderQty"].DBToInt32();
                    lOrdem.OrderQtyRemmaining = lRow["OrderQtyRemaining"].DBToInt32();
                    lOrdem.CumQty = lRow["CumQty"].DBToInt32();
                    lOrdem.OrdStatus = Conversions.FixOrderStatus2OrdemStatusEnum( (FixOrderStatus)lRow["OrdStatusId"].DBToInt32());
                    lOrdem.OrdType = Conversions.FixOrderType2OrdemTipoEnum(lRow["OrdTypeId"].DBToString());

                    lOrdem.RegisterTime = lRow["RegisterTime"].DBToDateTime();
                    lOrdem.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                    lOrdem.SecurityID = lRow["SecurityExchangeId"].DBToString();


                    lOrdem.Side = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                    lOrdem.StopStartID = lRow["StopStartID"].DBToInt32();
                    lOrdem.Symbol = lRow["Symbol"].DBToString();
                    lOrdem.TimeInForce = Conversions.FIXTimeInForce2OrdemValidadeEnum(lRow["TimeInForce"].DBToString()[0]);
                    lOrdem.TransactTime = lRow["TransactTime"].DBToDateTime();
                    lOrdem.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32();
                    lOrdem.CompIDOMS = lRow["SystemID"].DBToString();

                    if (lRow["Price"] != DBNull.Value)
                        lOrdem.Price = lRow["Price"].DBToDouble().Value;

                    lRetorno.Add(lOrdem);

                    lUltimoId = lRow["ClOrdId"].DBToString() + lRow["OrderId"].DBToString(); 
                }

                //Não é dentro de 'else' porque a primeira linha tem o primeiro acompanhamento:

                lAcompanhamento = new AcompanhamentoOrdemInfo();
                lAcompanhamento.CanalNegociacao = lRow["ChannelID"].DBToInt32();
                lAcompanhamento.CodigoDoCliente = lRow["Account"].DBToInt32();
                lAcompanhamento.CodigoResposta = lRow["ExchangeNumberId"].DBToString();
                lAcompanhamento.CodigoTransacao = lRow["OrderDetail_TransactId"].DBToString();
                lAcompanhamento.DataAtualizacao = lRow["TransactTime"].DBToDateTime();
                lAcompanhamento.DataOrdemEnvio = lRow["RegisterTime"].DBToDateTime();
                lAcompanhamento.DataValidade = lRow["ExpireDate"].DBToDateTime();
                lAcompanhamento.Direcao = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                lAcompanhamento.Instrumento = lRow["Symbol"].DBToString();
                lAcompanhamento.NumeroControleOrdem = lRow["ClOrdId"].DBToString() + "-" + lRow["OrderId"].DBToString();
                lAcompanhamento.Descricao = lRow["OrderDetail_Description"].DBToString();

                lAcompanhamento.QuantidadeRemanescente = lRow["OrderDetail_OrdQtyRemaining"].DBToInt32();
                lAcompanhamento.QuantidadeExecutada = lRow["OrderDetail_CumQty"].DBToInt32();
                lAcompanhamento.QuantidadeSolicitada = lRow["OrderDetail_OrderQty"].DBToInt32();
                lAcompanhamento.QuantidadeNegociada = lRow["OrderDetail_TradeQty"].DBToInt32();

                lAcompanhamento.StatusOrdem = Conversions.FixOrderStatus2OrdemStatusEnum( (FixOrderStatus)lRow["OrderDetail_OrderStatusId"].DBToInt32());
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



    }
}
