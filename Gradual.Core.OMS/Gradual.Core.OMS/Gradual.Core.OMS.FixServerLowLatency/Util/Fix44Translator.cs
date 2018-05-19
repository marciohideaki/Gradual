using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

using QuickFix;

using QuickFix.Fields;

using Gradual.OMS.RoteadorOrdens.Lib.Dados;

using Cortex.OMS.FixUtilities.Lib;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using System.Globalization;
using Gradual.Core.OMS.FixServerLowLatency.Rede;

namespace Gradual.Core.OMS.FixServerLowLatency.Util
{
    public class Fix44Translator
    {

        private static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Fix44Translator()
        {

        }

        public static OrdemInfo Fix44ExecutionReport2OrdemInfo(QuickFix.FIX44.ExecutionReport er, FixSessionItem cfg)
        {
            try
            {
                OrdemInfo order = new OrdemInfo();
                string descricao = string.Empty;

                // order = new OrdemInfo();
                order.Account = Convert.ToInt32(er.IsSetAccount() ? er.Account.getValue() : "0");
                order.Exchange = cfg.Bolsa;
                order.ChannelID = cfg.Operador;
                order.ClOrdID = er.ClOrdID.ToString();//chaveClOrdId;
                order.OrigClOrdID = er.IsSetOrigClOrdID() ? er.OrigClOrdID.getValue() : null;
                order.ExecBroker = "227";
                if (er.IsSetExpireDate())
                {
                    string expdate = er.ExpireDate.getValue() + "235959";
                    order.ExpireDate = DateTime.ParseExact(expdate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }
                else
                {
                    order.ExpireDate = DateTime.MinValue;
                }
                order.MaxFloor = er.IsSetMaxFloor() ? Convert.ToDouble(er.MaxFloor.getValue()) : 0;
                order.MinQty = er.IsSetMinQty() ? Convert.ToDouble(er.MinQty.getValue()) : 0;
                order.OrderQty = Convert.ToInt32(er.IsSetOrderQty() ? er.OrderQty.getValue() : 0);
                order.OrdType = FixMessageUtilities.TraduzirOrdemTipo(er.OrdType.getValue());
                order.Price = er.IsSetPrice() ? Convert.ToDouble(er.Price.getValue()) : 0;
                order.SecurityID = er.IsSetSecurityID() ? er.SecurityID.ToString() : null;
                order.Side = er.IsSetSide() ? (OrdemDirecaoEnum)Convert.ToInt32(er.Side.ToString()) : OrdemDirecaoEnum.NaoInformado;
                order.Symbol = er.IsSetSymbol() ? er.Symbol.ToString() : null;
                order.TimeInForce = er.IsSetTimeInForce() ? FixMessageUtilities.deTimeInForceParaOrdemValidade(er.TimeInForce) : OrdemValidadeEnum.NaoInformado;
                //}

                order.ExchangeNumberID = er.IsSetOrderID() ? er.OrderID.getValue() : null;
                order.OrderQtyRemmaining = Convert.ToInt32(er.IsSetLeavesQty() ? er.LeavesQty.getValue() : 0);
                order.CumQty = Convert.ToInt32(er.IsSetCumQty() ? er.CumQty.getValue() : 0);
                order.OrdStatus = FixMessageUtilities.TraduzirOrdemStatus(er.OrdStatus.getValue());
                order.Memo5149 = er.IsSetField(5149) ? er.GetField(5149) : String.Empty;
                order.PossDupFlag = er.Header.IsSetField(Tags.PossDupFlag) ? er.Header.GetBoolean(Tags.PossDupFlag) : false;
                order.PossResend = er.Header.IsSetField(Tags.PossResend) ? er.Header.GetBoolean(Tags.PossResend) : false;
                order.CompIDBolsa = er.Header.IsSetField(Tags.SenderCompID) ? er.Header.GetString(Tags.SenderCompID) : cfg.SenderCompID;
                order.CompIDOMS = er.Header.IsSetField(Tags.TargetCompID) ? er.Header.GetString(Tags.TargetCompID) : cfg.TargetCompID;

                switch (order.OrdStatus)
                {
                    case OrdemStatusEnum.NOVA:
                        descricao = "Ordem aberta";
                        order.RegisterTime = DateTime.Now;
                        break;
                    case OrdemStatusEnum.CANCELADA:
                        descricao = "Ordem cancelada";
                        break;
                    case OrdemStatusEnum.PARCIALMENTEEXECUTADA:
                        descricao = "Ordem com execucao parcial";
                        break;
                    case OrdemStatusEnum.SUSPENSA:
                        descricao = "Ordem suspensa";
                        break;
                    case OrdemStatusEnum.EXECUTADA:
                        descricao = "Ordem executada";
                        break;
                    case OrdemStatusEnum.SUBSTITUIDA:
                        descricao = "Ordem substituida";
                        break;
                    case OrdemStatusEnum.REJEITADA:
                        if (er.IsSetText())
                            descricao = er.Text.getValue();
                        break;
                }
                order.TransactTime = DateTime.Now;
                // Try get the msg seq number
                order.FixMsgSeqNum = er.Header.IsSetField(Tags.MsgSeqNum) ? er.Header.GetInt(Tags.MsgSeqNum) : 0;
                order.ProtectionPrice = Convert.ToDecimal(er.IsSetField(35001) ? er.GetString(35001) : "0");


                // Setando informações de acompanhamento
                AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();
                acompanhamento.NumeroControleOrdem = order.ClOrdID;
                acompanhamento.CodigoDoCliente = order.Account;
                acompanhamento.CodigoResposta = order.ExchangeNumberID;
                acompanhamento.CodigoTransacao = er.IsSetExecID() ? er.ExecID.getValue() : null;
                acompanhamento.Instrumento = order.Symbol;
                acompanhamento.SecurityID = order.SecurityID;
                acompanhamento.CanalNegociacao = order.ChannelID;
                acompanhamento.Direcao = order.Side;
                acompanhamento.QuantidadeSolicitada = order.OrderQty;
                acompanhamento.QuantidadeExecutada = (int)er.CumQty.getValue();
                acompanhamento.QuantidadeRemanescente = (int)er.LeavesQty.getValue();
                acompanhamento.QuantidadeNegociada = er.IsSetLastQty() ? (int)er.LastQty.getValue() : 0;
                acompanhamento.Preco = new Decimal(order.Price);
                acompanhamento.StatusOrdem = FixMessageUtilities.TraduzirOrdemStatus(er.OrdStatus.getValue());
                acompanhamento.DataOrdemEnvio = order.TransactTime;
                acompanhamento.DataAtualizacao = DateTime.Now;
                acompanhamento.CodigoRejeicao = er.IsSetOrdRejReason() ? er.OrdRejReason.ToString() : "0";
                acompanhamento.Descricao = descricao;
                // Try get the msg seq number
                acompanhamento.FixMsgSeqNum = er.Header.IsSetField(Tags.MsgSeqNum) ? er.Header.GetInt(Tags.MsgSeqNum) : 0;
                acompanhamento.LastPx = er.IsSetLastPx() ? (Decimal)er.LastPx.getValue() : new Decimal(order.Price);
                acompanhamento.TradeDate = er.IsSetTradeDate() ? er.TradeDate.getValue() : DateTime.Now.ToString("yyyyMMdd");

                //BEI Fields
                //Added in 2012-Nov-13 by ATP
                acompanhamento.ExchangeOrderID = er.IsSetField(35022) ? er.GetString(35022) : String.Empty;
                acompanhamento.ExchangeExecID = er.IsSetField(35023) ? er.GetString(35023) : String.Empty;
                acompanhamento.LastPxInIssuedCurrency = Convert.ToDecimal(er.IsSetField(35024) ? er.GetString(35024) : "0");
                acompanhamento.PriceInIssuedCurrency = Convert.ToDecimal(er.IsSetField(35025) ? er.GetString(35025) : "0");
                acompanhamento.ExchangeSecondaryOrderID = er.IsSetField(35026) ? er.GetString(35026) : String.Empty;
                acompanhamento.TradeLinkID = er.IsSetField(820) ? er.GetString(820) : String.Empty;
                acompanhamento.OrderLinkID = er.IsSetField(5975) ? er.GetString(5975) : String.Empty;
                acompanhamento.ExchangeQuoteID = er.IsSetField(5001) ? er.GetString(5001) : String.Empty;
                acompanhamento.PossDupFlag = er.Header.IsSetField(Tags.PossDupFlag) ? er.Header.GetBoolean(Tags.PossDupFlag) : false;
                acompanhamento.PossResend = er.Header.IsSetField(Tags.PossResend) ? er.Header.GetBoolean(Tags.PossResend) : false;
                acompanhamento.CompIDBolsa = er.Header.IsSetField(Tags.SenderCompID) ? er.Header.GetString(Tags.SenderCompID) : cfg.SenderCompID;
                acompanhamento.CompIDOMS = er.Header.IsSetField(Tags.TargetCompID) ? er.Header.GetString(Tags.TargetCompID) : cfg.TargetCompID;

                if (er.IsSetNoMiscFees())
                {
                    int ocorr = er.GetInt(Tags.NoMiscFees);// noMiscFees.getValue();

                    for (uint i = 0; i < ocorr; i++)
                    {
                        EmolumentoInfo emol = new EmolumentoInfo();
                        Group feeGroup = er.GetGroup((int)i, Tags.NoMiscFees);
                        emol.Valor = Convert.ToDecimal(feeGroup.IsSetField(Tags.MiscFeeAmt) ? feeGroup.GetString(Tags.MiscFeeAmt) : "0");
                        emol.BaseEmolumento = Convert.ToInt32(feeGroup.IsSetField(Tags.MiscFeeBasis) ? feeGroup.GetString(Tags.MiscFeeBasis) : "0");
                        emol.Currency = feeGroup.GetString(Tags.MiscFeeCurr);
                        emol.Tipo = (EmolumentoTipoEnum)feeGroup.GetInt(Tags.MiscFeeType);
                    }
                }

                // Adicionando informações de acompanhamento ao OrdemInfo
                order.Acompanhamentos.Clear();
                order.Acompanhamentos.Add(acompanhamento);

                return order;
            }
            catch (Exception ex)
            {
                logger.Error("Fix44ExecutionReport2OrdemInfo(): " + ex.Message, ex);
            }
            return null;
        }

        public static QuickFix.FIX44.ExecutionReport Fix44Rejection2ExecutionReport(QuickFix.Message msgReject, QuickFix.Message msgOri)
        {
            try
            {
                QuickFix.FIX44.ExecutionReport er = new QuickFix.FIX44.ExecutionReport();
                if (msgOri.IsSetField(Tags.OrderID))
                    er.Set(new OrderID(msgOri.GetString(Tags.OrderID)));
                else
                    er.Set(new OrderID("NONE"));

                if (msgOri.IsSetField(Tags.ClOrdID)) er.Set(new ClOrdID(msgOri.GetField(Tags.ClOrdID)));
                if (msgOri.IsSetField(Tags.OrigClOrdID)) er.Set(new OrigClOrdID(msgOri.GetField(Tags.OrigClOrdID)));
                // Fazer atribuicao dos PartyIDs da mensagem original
                int len = msgOri.GetInt(Tags.NoPartyIDs);
                for (int i = 0; i < len; i++)
                {
                    Group grp = msgOri.GetGroup(i + 1, Tags.NoPartyIDs);
                    er.AddGroup(grp);
                }
                er.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                er.Set(new ExecType(ExecType.REJECTED));
                er.Set(new OrdStatus(OrdStatus.REJECTED));
                if (msgOri.IsSetField(Tags.Account)) er.Set(new Account(msgOri.GetField(Tags.Account)));
                er.Set(new Symbol(msgOri.GetField(Tags.Symbol)));
                if (msgOri.IsSetField(Tags.SecurityID)) er.Set(new SecurityID(msgOri.GetField(Tags.SecurityID)));
                if (msgOri.IsSetField(Tags.SecurityIDSource)) er.Set(new SecurityIDSource(msgOri.GetField(Tags.SecurityIDSource)));
                er.Set(new Side(msgOri.GetChar(Tags.Side)));
                er.Set(new OrderQty(msgOri.GetDecimal(Tags.OrderQty)));
                if (msgOri.IsSetField(Tags.OrdType)) er.Set(new OrdType(msgOri.GetChar(Tags.OrdType)));
                if (msgOri.IsSetField(Tags.Price)) er.Set(new Price(msgOri.GetDecimal(Tags.Price)));
                if (msgOri.IsSetField(Tags.StopPx)) er.Set(new StopPx(msgOri.GetDecimal(Tags.StopPx)));
                if (msgOri.IsSetField(Tags.TimeInForce)) er.Set(new TimeInForce(msgOri.GetChar(Tags.TimeInForce)));
                if (msgOri.IsSetField(Tags.ExpireDate)) er.Set(new ExpireDate(msgOri.GetField(Tags.ExpireDate)));
                er.Set(new LeavesQty(0));
                er.Set(new CumQty(0));
                er.Set(new AvgPx(Decimal.Zero));
                DateTime transact = DateTime.UtcNow;
                er.Set(new TransactTime(transact));
                if (msgOri.IsSetField(Tags.MaxFloor)) er.Set(new MaxFloor(msgOri.GetDecimal(Tags.MaxFloor)));
                if (msgOri.IsSetField(Tags.Memo))
                    er.SetField(new Memo(msgOri.GetString(Tags.Memo)));

                if (msgReject.IsSetField(Tags.Text))
                    er.Set(new Text(msgReject.GetField(Tags.Text)));
                if (msgReject.IsSetField(Tags.OrderID))
                    er.Set(new OrderID(msgReject.GetField(Tags.OrderID)));

                return er;
            }
            catch (Exception ex)
            {
                logger.Error("Fix44Rejection2ExecutionReport(): " + ex.Message, ex);
                return null;
            }
        }

        public static QuickFix.FIX44.OrderCancelReject Fix44Reject2OrderCancelReject(QuickFix.Message msgReject, QuickFix.Message msgOri)
        {
            try
            {
                QuickFix.FIX44.OrderCancelReject ocr = new QuickFix.FIX44.OrderCancelReject();
                if (msgOri.IsSetField(Tags.OrderID))
                    ocr.Set(new OrderID(msgOri.GetString(Tags.OrderID)));
                else
                    ocr.Set(new OrderID("NONE"));
                ocr.Set(new ClOrdID(msgOri.GetString(Tags.ClOrdID)));           // mandatory
                ocr.Set(new OrigClOrdID(msgOri.GetString(Tags.OrigClOrdID)));   // mandatory
                ocr.Set(new OrdStatus(OrdStatus.REJECTED));                     // mandatory
                if (msgOri.IsSetField(Tags.Account))
                    ocr.SetField(new Account(msgOri.GetString(Tags.Account)));
                if (msgOri.Header.GetString(Tags.MsgType).Equals(MsgType.ORDER_CANCEL_REQUEST)) // mandatory
                    ocr.Set(new CxlRejResponseTo('1'));
                else
                    ocr.Set(new CxlRejResponseTo('2'));
                if (msgReject.IsSetField(Tags.Text))
                    ocr.Set(new Text(msgReject.GetString(Tags.Text)));
                // ocr.Set(new CxlRejReason(2137));

                // Tratamento tag SecondaryOrderID
                // Se nao vier na mensagem busca pelo TO
                //if (msgOri.IsSetField(Tags.SecondaryOrderID))
                //{
                //    ocr.Set(new SecondaryOrderID(msgOri.GetString(Tags.SecondaryOrderID)));
                //}
                //else
                //{
                //    if (null != to)
                //    {
                //        if (!string.IsNullOrEmpty(to.SecondaryOrderID))
                //            ocr.Set(new SecondaryOrderID(to.SecondaryOrderID));
                //            
                //        if (!string.IsNullOrEmpty(to.TradeDate))
                //            ocr.Set(new TradeDate(to.TradeDate));
                //    }
                //}
                int len = msgOri.GetInt(Tags.NoPartyIDs);
                for (int i = 0; i < len; i++)
                {
                    Group grp = msgOri.GetGroup(i + 1, Tags.NoPartyIDs);
                    ocr.AddGroup(grp);
                }
                ocr.Set(new Symbol(msgOri.GetString(Tags.Symbol)));
                if (msgOri.IsSetField(Tags.SecurityID)) ocr.Set(new SecurityID(msgOri.GetField(Tags.SecurityID)));
                if (msgOri.IsSetField(Tags.SecurityIDSource)) ocr.Set(new SecurityIDSource(msgOri.GetField(Tags.SecurityIDSource)));
                ocr.Set(new Side(msgOri.GetChar(Tags.Side)));
                if (msgOri.IsSetField(Tags.Memo))
                    ocr.Set(new Memo(msgOri.GetString(Tags.Memo)));
                return ocr;
            }
            catch (Exception ex)
            {
                logger.Error("Fix44Reject2OrderCancelReject(): " + ex.Message, ex);
                return null;
            }

        }

        public static List<QuickFix.FIX44.ExecutionReport> Fix44Rejection2ExecutionReportNOC(QuickFix.Message msgReject, QuickFix.FIX44.NewOrderCross noc)
        {
            try
            {
                QuickFix.FIX44.ExecutionReport er1 = new QuickFix.FIX44.ExecutionReport();
                QuickFix.FIX44.ExecutionReport er2 = new QuickFix.FIX44.ExecutionReport();
                er1.Set(new OrderID("NONE"));
                er2.Set(new OrderID("NONE"));
                if (noc.IsSetNoSides() && noc.NoSides.getValue() == 2)
                {
                    QuickFix.Group grpNoSides = noc.GetGroup(1, Tags.NoSides);
                    if (grpNoSides.IsSetField(Tags.ClOrdID)) er1.Set(new ClOrdID(grpNoSides.GetField(Tags.ClOrdID)));
                    er1.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                    er1.Set(new ExecType(ExecType.REJECTED));
                    er1.Set(new OrdStatus(OrdStatus.REJECTED));
                    if (grpNoSides.IsSetField(Tags.Account)) er1.Set(new Account(grpNoSides.GetField(Tags.Account)));
                    er1.Set(new Symbol(noc.GetField(Tags.Symbol)));
                    if (noc.IsSetField(Tags.SecurityID)) er1.Set(new SecurityID(noc.GetField(Tags.SecurityID)));
                    if (noc.IsSetField(Tags.SecurityIDSource)) er1.Set(new SecurityIDSource(noc.GetField(Tags.SecurityIDSource)));
                    er1.Set(new Side(grpNoSides.GetChar(Tags.Side)));
                    er1.Set(new OrderQty(grpNoSides.GetDecimal(Tags.OrderQty)));
                    if (noc.IsSetField(Tags.OrdType)) er1.Set(new OrdType(noc.GetChar(Tags.OrdType)));
                    if (noc.IsSetField(Tags.Price)) er1.Set(new Price(noc.GetDecimal(Tags.Price)));
                    er1.Set(new LeavesQty(0));
                    er1.Set(new CumQty(0));
                    er1.Set(new AvgPx(Decimal.Zero));
                    DateTime transact1 = DateTime.UtcNow;
                    er1.Set(new TransactTime(transact1));
                    if (msgReject.IsSetField(Tags.Text))
                        er1.Set(new Text(msgReject.GetField(Tags.Text)));

                    grpNoSides = noc.GetGroup(2, Tags.NoSides);
                    if (grpNoSides.IsSetField(Tags.ClOrdID)) er2.Set(new ClOrdID(grpNoSides.GetField(Tags.ClOrdID)));
                    er2.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                    er2.Set(new ExecType(ExecType.REJECTED));
                    er2.Set(new OrdStatus(OrdStatus.REJECTED));
                    if (grpNoSides.IsSetField(Tags.Account)) er2.Set(new Account(grpNoSides.GetField(Tags.Account)));
                    er2.Set(new Symbol(noc.GetField(Tags.Symbol)));
                    if (noc.IsSetField(Tags.SecurityID)) er2.Set(new SecurityID(noc.GetField(Tags.SecurityID)));
                    if (noc.IsSetField(Tags.SecurityIDSource)) er2.Set(new SecurityIDSource(noc.GetField(Tags.SecurityIDSource)));
                    er2.Set(new Side(grpNoSides.GetChar(Tags.Side)));
                    er2.Set(new OrderQty(grpNoSides.GetDecimal(Tags.OrderQty)));
                    if (noc.IsSetField(Tags.OrdType)) er2.Set(new OrdType(noc.GetChar(Tags.OrdType)));
                    if (noc.IsSetField(Tags.Price)) er2.Set(new Price(noc.GetDecimal(Tags.Price)));
                    er2.Set(new LeavesQty(0));
                    er2.Set(new CumQty(0));
                    er2.Set(new AvgPx(Decimal.Zero));
                    DateTime transact2 = DateTime.UtcNow;
                    er2.Set(new TransactTime(transact2));
                    if (msgReject.IsSetField(Tags.Text))
                        er2.Set(new Text(msgReject.GetField(Tags.Text)));
                }

                List<QuickFix.FIX44.ExecutionReport> ret = new List<QuickFix.FIX44.ExecutionReport>();
                ret.Add(er1);
                ret.Add(er2);
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Fix44Rejection2ExecutionReportNOC(): " + ex.Message, ex);
                return null;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////
        #region "FIX42 Conversions to Fix44"

        



        

        

        /*
        public static QuickFix.FIX44.SequenceReset Fix42SR_2_Fix44SR(QuickFix.FIX42.SequenceReset sr42)
        {
            try
            {
                QuickFix.FIX44.SequenceReset sr44 = new QuickFix.FIX44.SequenceReset();
                _fix42Header_2_Fix44Header(sr42.Header, sr44.Header);

                if (sr42.IsSetNewSeqNo()) sr44.Set(new NewSeqNo(sr42.NewSeqNo.getValue()));
                if (sr42.IsSetGapFillFlag()) sr44.Set(new GapFillFlag(sr42.GapFillFlag.getValue()));
                return sr44;
            }
            catch (Exception ex)
            {
                logger.Error("Fix42SR_2_Fix44SR(): Problemas na conversao da mensagem SR: " + ex.Message, ex);
                return null;
            }
        }
        */
        public static void Generate44RejectMessage(QuickFix.Message msg, SessionID s, string msgType, Exception ex, SessionAcceptor ssAcceptor, string msgText)
        {
            try
            {
                if (null != ex)
                {
                    string aux = string.Format("QuickFix44 MsgType: [{0}], Message: [{1}]", msgType, ex.Message);
                    logger.Error(aux, ex);
                }

                QuickFix.FIX44.Reject rej = new QuickFix.FIX44.Reject();
                rej.Set(new RefMsgType(msgType));
                rej.Set(new RefSeqNum(msg.Header.GetInt(Tags.MsgSeqNum)));
                if (string.IsNullOrEmpty(msgText))
                    rej.Set(new Text("System unavaliable")); // Mensagem generica para nao expor possiveis erros de aplicacao
                else
                    rej.Set(new Text(msgText));

                if (msgType.Equals(MsgType.ORDER_CANCEL_REQUEST) || msgType.Equals(MsgType.ORDER_CANCEL_REPLACE_REQUEST))
                {
                    QuickFix.FIX44.OrderCancelReject ocr = Fix44Translator.Fix44Reject2OrderCancelReject(rej, msg);
                    Session.SendToTarget(ocr, s);
                    if (null != ssAcceptor) ssAcceptor.Send2DropCopy(ocr);
                }
                else
                {
                    QuickFix.FIX44.ExecutionReport er = Fix44Translator.Fix44Rejection2ExecutionReport(rej, msg);
                    Session.SendToTarget(er, s);
                    if (null != ssAcceptor) ssAcceptor.Send2DropCopy(er);
                }
            }
            catch (Exception exC)
            {
                logger.Error("Problemas na geracao de mensagem de reject (tratamento de excecoes) de mensagem fix 4.2: " + exC.Message, exC);
            }
        }
        #endregion
    }

    
}
