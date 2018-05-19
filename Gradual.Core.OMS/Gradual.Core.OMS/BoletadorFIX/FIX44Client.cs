using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Cortex.OMS.FixUtilities.Lib;
using QuickFix.Fields;
using Gradual.OMS.ServicoRoteador;
using System.Drawing;
using System.Configuration;
using QuickFix.FIX44;

namespace BoletadorFIX
{
    public class FIX44Client : QuickFix.MessageCracker, QuickFix.Application
    {
        private static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Session _session = null;
        private Form1 _mainform;

        #region IApplication interface overrides
        public FIX44Client(Form1 mainform)
        {
            _mainform = mainform;
        }

        public void OnCreate(SessionID sessionID)
        {
            _session = Session.LookupSession(sessionID);
        }

        public void OnLogon(SessionID sessionID)
        {
            _mainform.ButtonClient44.BackColor = Color.Green;
        }

        public void OnLogout(SessionID sessionID)
        {
            _mainform.ButtonClient44.BackColor = Color.Red;
        }

        public void FromAdmin(QuickFix.Message message, SessionID sessionID) { }

        public void ToAdmin(QuickFix.Message message, SessionID sessionID)
        {
            // Faz o processamento
            try
            {

                string msgType = message.Header.GetString(35);
                logger.Debug("ToAdmin: " + msgType);

                // Complementa a mensagem de logon com a senha 
                if (msgType.Equals(QuickFix.FIX44.Logon.MsgType))
                {
                    Logon message2 = (Logon)message;
                    if (ConfigurationManager.AppSettings["Passwd44"] != null)
                    {
                        message2.Set(new RawData(ConfigurationManager.AppSettings["Passwd44"].ToString()));
                        message2.Set(new RawDataLength(ConfigurationManager.AppSettings["Passwd44"].ToString().Length));
                    }

                    if (ConfigurationManager.AppSettings["CancelOnDisconnect"] != null &&
                        ConfigurationManager.AppSettings["CancelOnDisconnect"].ToString().ToUpper().Equals("N") &&
                        ConfigurationManager.AppSettings["CancelOnDisconnect"].ToString().ToUpper().Equals("FALSE"))
                    {
                        char codtype = ConfigurationManager.AppSettings["CancelOnDisconnect"].ToString()[0];
                        if (codtype >= '0' && codtype <= '3')
                        {
                            CharField field35002 = new CharField(35002, codtype);
                            message2.SetField(field35002);
                        }

                        IntField field35003 = new IntField(35003, 60000);
                        message2.SetField(field35003);
                    }
                    message2.Set(new HeartBtInt(30));
                    message2.Set(new EncryptMethod(0));
                    message2.Set(new ResetSeqNumFlag(ConfigurationManager.AppSettings["ResetSeqNum"].ToString().Equals("true")));
                }

                logger.Debug("toAdmin(). Session id : " + sessionID + " Msg: " + message.GetType().ToString());

                if (message.Header.GetString(35).Equals(QuickFix.FIX42.Heartbeat.MsgType) == false)
                    Crack(message, sessionID);
            }
            catch (Exception ex)
            {
                logger.Error("toAdmin() Erro: " + ex.Message, ex);
            }
        }

        public void FromApp(QuickFix.Message message, SessionID sessionID)
        {
            logger.Debug("IN:  " + message.ToString());
            try
            {
                Crack(message, sessionID);
            }
            catch (Exception ex)
            {
                logger.Error("==Cracker exception==");
                logger.Error(ex.ToString(),ex);
            }
        }


        public void ToApp(QuickFix.Message message, SessionID sessionID)
        {
            try
            {
                bool possDupFlag = false;
                if (message.Header.IsSetField(QuickFix.Fields.Tags.PossDupFlag))
                {
                    possDupFlag = QuickFix.Fields.Converters.BoolConverter.Convert(
                        message.Header.GetField(QuickFix.Fields.Tags.PossDupFlag)); /// FIXME
                }
                if (possDupFlag)
                    throw new DoNotSend();
            }
            catch (FieldNotFoundException fnfex)
            {
                logger.Error("Erro: " + fnfex.Message, fnfex);

            }
        }
        #endregion


        #region MessageCracker handlers
        public void OnMessage(QuickFix.FIX44.ExecutionReport m, SessionID s)
        {
            _mainform._addMsg("Received execution report (4.4)\r\n");

            string msg = "Order Dump Begin =======================\r\n";
            msg += "\r\n";

            msg += "OrdemInfo:\r\n";
            msg += "==========\r\n";
            msg += "Account .........: " + m.Account.getValue() + "\r\n";
            msg += "Numero da ordem .: " + m.ClOrdID.getValue() + "\r\n";
            msg += "Symbol ..........: " + m.Symbol.getValue() + "\r\n";
            msg += "ExchangeNumber ..: " + m.OrderID.getValue() + "\r\n";
            msg += "Status ..........: " + m.OrdStatus.getValue() + "\r\n";
            msg += "Status ..........: " + FixMessageUtilities.TraduzirOrdemStatus(m.OrdStatus.getValue()) + "\r\n";
            msg += "Quantidade ......: " + m.OrderQty.getValue() + "\r\n";
            msg += "Qtde restante ...: " + (m.OrderQty.getValue() - m.CumQty.getValue()) + "\r\n";
            if ( m.IsSetLastPx() )
                msg += "Preco ...........: " + m.LastPx.getValue() + "\r\n";

            if (m.IsSetText())
                msg += "Text ............: [" + m.Text.getValue() + "]\r\n";

            if (m.IsSetField(5149))
            {
                msg += "Memo5149 ........: " + m.GetString(5149) + "\r\n";
            }

            msg += "\r\n";

            msg += "\r\nOrder Dump End =========================\r\n";

            _mainform._addMsg(msg);

        }

        public void OnMessage(QuickFix.FIX44.OrderCancelReject m, SessionID s)
        {
            logger.Debug("Received order cancel reject");
        }
        #endregion

        #region IBoletadorMembers
        public bool EnviarOrdem(OrdemInfo ordem)
        {
            //Cria a mensagem FIX de NewOrderSingle
            QuickFix.FIX44.NewOrderSingle ordersingle = new QuickFix.FIX44.NewOrderSingle();

            if (ordem.Account > 0)
                ordersingle.Set(new Account(ordem.Account.ToString()));

            // Instrument Identification Block
            ordersingle.Set(new Symbol(ordem.Symbol));
            ordersingle.Set(new SecurityID(ordem.SecurityID));
            ordersingle.Set(new SecurityIDSource(SecurityIDSource.EXCHANGE_SYMBOL));

            ordersingle.Set(new ClOrdID(ordem.ClOrdID));

            if (ordem.Side == OrdemDirecaoEnum.Venda)
                ordersingle.Set(new Side(Side.SELL));
            else
                ordersingle.Set(new Side(Side.BUY));


            TimeInForce tif = FixMessageUtilities.deOrdemValidadeParaTimeInForce(ordem.TimeInForce);
            if (tif != null)
                ordersingle.Set(tif);

            ordersingle.Set(new OrderQty(ordem.OrderQty));

            if (ordem.TimeInForce == OrdemValidadeEnum.ValidoAteDeterminadaData)
            {
                DateTime expiredate = Convert.ToDateTime(ordem.ExpireDate);
                ordersingle.Set(new ExpireDate(expiredate.ToString("yyyyMMdd")));
            }

            OrdType ordType = FixMessageUtilities.deOrdemTipoParaOrdType(ordem.OrdType);
            if (ordType != null)
                ordersingle.Set(ordType);

            // Verifica envio do preco
            switch (ordem.OrdType)
            {
                case OrdemTipoEnum.Limitada:
                case OrdemTipoEnum.MarketWithLeftOverLimit:
                    ordersingle.Set(new Price(Convert.ToDecimal(ordem.Price)));
                    break;
                case OrdemTipoEnum.StopLimitada:
                case OrdemTipoEnum.StopStart:
                    ordersingle.Set(new Price(Convert.ToDecimal(ordem.Price)));
                    ordersingle.Set(new StopPx(Convert.ToDecimal(ordem.StopPrice)));
                    break;
                case OrdemTipoEnum.Mercado:
                case OrdemTipoEnum.OnClose:
                    break;
                default:
                    ordersingle.Set(new Price(Convert.ToDecimal(ordem.Price)));
                    break;
            }

            ordersingle.Set(new TransactTime(DateTime.Now));
            ordersingle.Set(new HandlInst('1'));

            if (ordem.MaxFloor > 0)
                ordersingle.Set(new MaxFloor(Convert.ToDecimal(ordem.MaxFloor)));

            if (ordem.MinQty > 0)
                ordersingle.Set(new MinQty(Convert.ToDecimal(ordem.MinQty)));

            // Cliente
            QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroup1 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
            //PartyIDGroup1.set(new PartyID(ordem.Account.ToString()));
            PartyIDGroup1.Set(new PartyID(ordem.ExecBroker));
            PartyIDGroup1.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup1.Set(new PartyRole(PartyRole.ENTERING_TRADER));

            // Corretora
            QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroup2 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
            PartyIDGroup2.Set(new PartyID("227"));
            PartyIDGroup2.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup2.Set(new PartyRole(PartyRole.ENTERING_FIRM));

            // Location ID
            QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroup3 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
            if (ordem.SenderLocation != null && ordem.SenderLocation.Length > 0)
                PartyIDGroup3.Set(new PartyID(ordem.SenderLocation));
            else
                PartyIDGroup3.Set(new PartyID("GRA"));
            PartyIDGroup3.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup3.Set(new PartyRole(PartyRole.SENDER_LOCATION));

            ordersingle.AddGroup(PartyIDGroup1);
            ordersingle.AddGroup(PartyIDGroup2);
            ordersingle.AddGroup(PartyIDGroup3);

            //BEI - 2012-Nov-13
            if (ordem.ForeignFirm != null && ordem.ForeignFirm.Length > 0)
            {
                QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroup5 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                PartyIDGroup5.Set(new PartyID(ordem.ForeignFirm));
                PartyIDGroup5.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup5.Set(new PartyRole(PartyRole.FOREIGN_FIRM));

                ordersingle.AddGroup(PartyIDGroup5);
            }

            //SelfTradeProtection - 2012-Nov-27
            if (ordem.InvestorID != null && ordem.InvestorID.Length > 0)
            {
                QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroup6 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                PartyIDGroup6.Set(new PartyID(ordem.InvestorID));
                PartyIDGroup6.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup6.Set(new PartyRole(PartyRole.INVESTOR_ID));

                ordersingle.AddGroup(PartyIDGroup6);
            }

            if (ordem.ExecutingTrader != null && ordem.ExecutingTrader.Length > 0)
            {
                QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroup7 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                PartyIDGroup7.Set(new PartyID(ordem.ExecutingTrader));
                PartyIDGroup7.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup7.Set(new PartyRole(PartyRole.EXECUTING_TRADER));

                ordersingle.AddGroup(PartyIDGroup7);
            }

            //if (ordem.Account > 0)
            //{
            //    QuickFix.FIX44.NewOrderSingle.NoAllocsGroup noAllocsGrp = new QuickFix.FIX44.NewOrderSingle.NoAllocsGroup();
            //    noAllocsGrp.Set(new AllocAccount(ordem.Account.ToString()));
            //    noAllocsGrp.Set(new AllocAcctIDSource(99));
            //    ordersingle.AddGroup(noAllocsGrp);
            //}

            if (ordem.PositionEffect != null && ordem.PositionEffect.Equals("C"))
                ordersingle.Set(new PositionEffect('C'));

            // Memo Field
            if (ordem.Memo5149 != null && ordem.Memo5149.Length > 0)
            {
                if (ordem.Memo5149.Length > 50)
                    ordem.Memo5149 = ordem.Memo5149.Substring(0, 50);

                StringField memo5149 = new StringField(5149, ordem.Memo5149);

                ordersingle.SetField(memo5149);
            }

            // AccountType
            if (ordem.AcountType == ContaTipoEnum.REMOVE_ACCOUNT_INFORMATION)
            {
                IntField int581 = new IntField(581, 38);
                ordersingle.SetField(int581);
            }
            else if (ordem.AcountType == ContaTipoEnum.GIVE_UP_LINK_IDENTIFIER)
            {
                IntField int581 = new IntField(581, 40);
                ordersingle.SetField(int581);
            }

            bool bRet = Session.SendToTarget(ordersingle, _session.SessionID);

            return bRet;
        }

        public bool AlterarOrdem(OrdemInfo ordem)
        {
            //Cria a mensagem FIX de NewOrderSingle
            QuickFix.FIX44.OrderCancelReplaceRequest orderCancelReplaceReq = new QuickFix.FIX44.OrderCancelReplaceRequest();

            orderCancelReplaceReq.Set(new OrigClOrdID(ordem.OrigClOrdID));

            if (ordem.Account > 0)
                orderCancelReplaceReq.Set(new Account(ordem.Account.ToString()));

            // Instrument Identification Block
            orderCancelReplaceReq.Set(new Symbol(ordem.Symbol));
            orderCancelReplaceReq.Set(new SecurityID(ordem.SecurityID));
            orderCancelReplaceReq.Set(new SecurityIDSource(SecurityIDSource.EXCHANGE_SYMBOL));

            if (ordem.ExchangeNumberID != null && ordem.ExchangeNumberID.Length > 0)
            {
                orderCancelReplaceReq.Set(new OrderID(ordem.ExchangeNumberID));
            }

            orderCancelReplaceReq.Set(new ClOrdID(ordem.ClOrdID));
            //ordersingle.set(new IDSource());
            if (ordem.Side == OrdemDirecaoEnum.Venda)
                orderCancelReplaceReq.Set(new Side(Side.SELL));
            else
                orderCancelReplaceReq.Set(new Side(Side.BUY));
            orderCancelReplaceReq.Set(new Price(Convert.ToDecimal(ordem.Price)));

            TimeInForce tif = FixMessageUtilities.deOrdemValidadeParaTimeInForce(ordem.TimeInForce);
            if (tif != null)
                orderCancelReplaceReq.Set(tif);

            orderCancelReplaceReq.Set(new OrderQty(ordem.OrderQty));


            if (ordem.TimeInForce == OrdemValidadeEnum.ValidoAteDeterminadaData)
            {
                DateTime expiredate = Convert.ToDateTime(ordem.ExpireDate);
                orderCancelReplaceReq.Set(new ExpireDate(expiredate.ToString("yyyyMMdd")));
            }

            OrdType ordType = FixMessageUtilities.deOrdemTipoParaOrdType(ordem.OrdType);
            if (ordType != null)
                orderCancelReplaceReq.Set(ordType);

            //if (ordem.OrdType == OrdemTipoEnum.StopLimitada )
            //{
            //    ordersingle.set(new StopPx(ordem.StopPx));
            //}

            orderCancelReplaceReq.Set(new TransactTime(DateTime.Now));
            // Cliente
            QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup PartyIDGroup1 = new QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup();
            PartyIDGroup1.Set(new PartyID(ordem.ExecBroker));
            PartyIDGroup1.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup1.Set(new PartyRole(PartyRole.ENTERING_TRADER));

            // Corretora
            QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup PartyIDGroup2 = new QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup();
            PartyIDGroup2.Set(new PartyID("227"));
            PartyIDGroup2.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup2.Set(new PartyRole(PartyRole.ENTERING_FIRM));

            // Location ID
            QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup PartyIDGroup3 = new QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup();
            if (ordem.SenderLocation != null && ordem.SenderLocation.Length > 0)
                PartyIDGroup3.Set(new PartyID(ordem.SenderLocation));
            else
                PartyIDGroup3.Set(new PartyID("GRA"));
            PartyIDGroup3.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup3.Set(new PartyRole(54));

            orderCancelReplaceReq.AddGroup(PartyIDGroup1);
            orderCancelReplaceReq.AddGroup(PartyIDGroup2);
            orderCancelReplaceReq.AddGroup(PartyIDGroup3);

            //BEI - 2012-Nov-14
            if (ordem.ForeignFirm != null && ordem.ForeignFirm.Length > 0)
            {
                QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup PartyIDGroup4 = new QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup();
                PartyIDGroup4.Set(new PartyID(ordem.ForeignFirm));
                PartyIDGroup4.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup4.Set(new PartyRole(PartyRole.FOREIGN_FIRM));

                orderCancelReplaceReq.AddGroup(PartyIDGroup4);
            }

            if (ordem.Account > 0)
            {
                QuickFix.FIX44.OrderCancelReplaceRequest.NoAllocsGroup noAllocsGrp = new QuickFix.FIX44.OrderCancelReplaceRequest.NoAllocsGroup();
                noAllocsGrp.Set(new AllocAccount(ordem.Account.ToString()));
                noAllocsGrp.Set(new AllocAcctIDSource(99));
                orderCancelReplaceReq.AddGroup(noAllocsGrp);
            }

            //SelfTradeProtection - 2012-Nov-27
            if (ordem.InvestorID != null && ordem.InvestorID.Length > 0)
            {
                QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup PartyIDGroup5 = new QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup();
                PartyIDGroup5.Set(new PartyID(ordem.InvestorID));
                PartyIDGroup5.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup5.Set(new PartyRole(PartyRole.INVESTOR_ID));

                orderCancelReplaceReq.AddGroup(PartyIDGroup5);
            }

            if (ordem.ExecutingTrader != null && ordem.ExecutingTrader.Length > 0)
            {
                QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup PartyIDGroup7 = new QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup();
                PartyIDGroup7.Set(new PartyID(ordem.ExecutingTrader));
                PartyIDGroup7.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup7.Set(new PartyRole(PartyRole.EXECUTING_TRADER));

                orderCancelReplaceReq.AddGroup(PartyIDGroup7);
            }

            // Memo Field
            if (ordem.Memo5149 != null && ordem.Memo5149.Length > 0)
            {
                if (ordem.Memo5149.Length > 50)
                    ordem.Memo5149 = ordem.Memo5149.Substring(0, 50);

                StringField memo5149 = new StringField(5149, ordem.Memo5149);

                orderCancelReplaceReq.SetField(memo5149);
            }

            bool bRet = Session.SendToTarget(orderCancelReplaceReq, _session.SessionID);

            return bRet;
        }

        public bool CancelarOrdem(OrdemCancelamentoInfo orderCancelInfo)
        {
            //Cria a mensagem FIX de OrderCancelRequest
            QuickFix.FIX44.OrderCancelRequest orderCancel = new QuickFix.FIX44.OrderCancelRequest();

            if (orderCancelInfo.Account > 0)
                orderCancel.Set(new Account(orderCancelInfo.Account.ToString()));

            orderCancel.Set(new OrigClOrdID(orderCancelInfo.OrigClOrdID));
            orderCancel.Set(new ClOrdID(orderCancelInfo.ClOrdID));
            if (orderCancelInfo.OrderID != null && orderCancelInfo.OrderID.Length > 0)
            {
                orderCancel.Set(new OrderID(orderCancelInfo.OrderID));
            }


            // Instrument Identification Block
            orderCancel.Set(new Symbol(orderCancelInfo.Symbol));
            orderCancel.Set(new SecurityID(orderCancelInfo.SecurityID));
            orderCancel.Set(new SecurityIDSource(SecurityIDSource.EXCHANGE_SYMBOL));

            if (orderCancelInfo.Side == OrdemDirecaoEnum.Venda)
                orderCancel.Set(new Side(Side.SELL));
            else
                orderCancel.Set(new Side(Side.BUY));
            orderCancel.Set(new TransactTime(DateTime.Now));

            //ATP - 2012-10-29
            //Qtde de contratos/papeis a serem cancelados
            orderCancel.Set(new OrderQty(orderCancelInfo.OrderQty));

            // Cliente
            QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup PartyIDGroup1 = new QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup();
            PartyIDGroup1.Set(new PartyID(orderCancelInfo.ExecBroker));
            PartyIDGroup1.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup1.Set(new PartyRole(PartyRole.ENTERING_TRADER));

            // Corretora
            QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup PartyIDGroup2 = new QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup();
            PartyIDGroup2.Set(new PartyID("227"));
            PartyIDGroup2.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup2.Set(new PartyRole(PartyRole.ENTERING_FIRM));

            QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup PartyIDGroup3 = new QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup();
            if (orderCancelInfo.SenderLocation != null && orderCancelInfo.SenderLocation.Length > 0)
                PartyIDGroup3.Set(new PartyID(orderCancelInfo.SenderLocation));
            else
                PartyIDGroup3.Set(new PartyID("GRA"));
            PartyIDGroup3.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup3.Set(new PartyRole(54));

            orderCancel.AddGroup(PartyIDGroup1);
            orderCancel.AddGroup(PartyIDGroup2);
            orderCancel.AddGroup(PartyIDGroup3);

            //BEI - 2012-Nov-14
            if (orderCancelInfo.ForeignFirm != null && orderCancelInfo.ForeignFirm.Length > 0)
            {
                QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup PartyIDGroup4 = new QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup();
                PartyIDGroup4.Set(new PartyID(orderCancelInfo.ForeignFirm));
                PartyIDGroup4.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup4.Set(new PartyRole(PartyRole.FOREIGN_FIRM));

                orderCancel.AddGroup(PartyIDGroup4);
            }

            if (orderCancelInfo.ExecutingTrader != null && orderCancelInfo.ExecutingTrader.Length > 0)
            {
                QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup PartyIDGroup7 = new QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup();
                PartyIDGroup7.Set(new PartyID(orderCancelInfo.ExecutingTrader));
                PartyIDGroup7.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup7.Set(new PartyRole(PartyRole.EXECUTING_TRADER));

                orderCancel.AddGroup(PartyIDGroup7);
            }

            // Memo Field
            if (orderCancelInfo.Memo5149 != null && orderCancelInfo.Memo5149.Length > 0)
            {
                if (orderCancelInfo.Memo5149.Length > 50)
                    orderCancelInfo.Memo5149 = orderCancelInfo.Memo5149.Substring(0, 50);

                StringField memo5149 = new StringField(5149, orderCancelInfo.Memo5149);

                orderCancel.SetField(memo5149);
            }


            bool bRet = Session.SendToTarget(orderCancel, _session.SessionID);

            return bRet;

        }

        public bool EnviarOrdemCross(OrdemCrossInfo crossinfo)
        {
            OrdemInfo ordemCompra = crossinfo.OrdemInfoCompra;
            OrdemInfo ordemVenda = crossinfo.OrdemInfoVenda;

            // Cria mensagem de new order cross -
            // Essa mensagem nao é padrao do 4.2 - some weird fucking things can
            // fucking happen with this fucking shit code
            QuickFix.FIX44.NewOrderCross ordercross = new QuickFix.FIX44.NewOrderCross();

            ordercross.Set(new CrossID(crossinfo.CrossID));
            // Unico valor aceito 1 - Negocio executado total ou parcial
            ordercross.Set(new CrossType(1));
            // Prioridade de uma das pontas. Unico valor aceito: 0 - nenhuma
            ordercross.Set(new CrossPrioritization(0));


            // Ordem cross, sempre 2 pernas
            // Informacoes da perna de compra
            QuickFix.FIX44.NewOrderCross.NoSidesGroup sideGroupC = new QuickFix.FIX44.NewOrderCross.NoSidesGroup();

            sideGroupC.Set(new Side(Side.BUY));
            sideGroupC.Set(new ClOrdID(ordemCompra.ClOrdID));
            if (ordemCompra.Account > 0)
                sideGroupC.Set(new Account(ordemCompra.Account.ToString()));
            sideGroupC.Set(new OrderQty(ordemCompra.OrderQty));
            //sideGroupC.Set(new PositionEffect('C'));
            if (ordemCompra.PositionEffect != null && ordemCompra.PositionEffect.Equals("C"))
                sideGroupC.Set(new PositionEffect('C'));

            // PartIDs Compra
            // Cliente
            QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroupC1 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
            PartyIDGroupC1.Set(new PartyID(ordemCompra.ExecBroker));
            PartyIDGroupC1.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroupC1.Set(new PartyRole(PartyRole.ENTERING_TRADER));

            // Corretora
            QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroupC2 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
            PartyIDGroupC2.Set(new PartyID("227"));
            PartyIDGroupC2.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroupC2.Set(new PartyRole(PartyRole.ENTERING_FIRM));

            // Sender Location
            QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroupC3 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
            PartyIDGroupC3.Set(new PartyID("GRA"));
            PartyIDGroupC3.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroupC3.Set(new PartyRole(54));

            // Adiciono os partIDs ao Side (perna de Compra)
            sideGroupC.AddGroup(PartyIDGroupC1);
            sideGroupC.AddGroup(PartyIDGroupC2);
            sideGroupC.AddGroup(PartyIDGroupC3);

            //BEI - 2012-Nov-14
            if (ordemCompra.ForeignFirm != null && ordemCompra.ForeignFirm.Length > 0)
            {
                QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroupC4 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                PartyIDGroupC4.Set(new PartyID(ordemCompra.ForeignFirm));
                PartyIDGroupC4.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroupC4.Set(new PartyRole(PartyRole.FOREIGN_FIRM));

                sideGroupC.AddGroup(PartyIDGroupC4);
            }

            //SelfTradeProtection - 2012-Nov-27
            if (ordemCompra.InvestorID != null && ordemCompra.InvestorID.Length > 0)
            {
                QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroup4 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                PartyIDGroup4.Set(new PartyID(ordemCompra.InvestorID));
                PartyIDGroup4.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup4.Set(new PartyRole(PartyRole.INVESTOR_ID));

                sideGroupC.AddGroup(PartyIDGroup4);
            }

            if (ordemCompra.Account > 0)
            {
                QuickFix.FIX44.NewOrderSingle.NoAllocsGroup noAllocsGrpC = new QuickFix.FIX44.NewOrderSingle.NoAllocsGroup();
                noAllocsGrpC.Set(new AllocAccount(ordemCompra.Account.ToString()));
                noAllocsGrpC.Set(new AllocAcctIDSource(99));
                sideGroupC.AddGroup(noAllocsGrpC);
            }



            // Insere informacoes da perna de venda
            QuickFix.FIX44.NewOrderCross.NoSidesGroup sideGroupV = new QuickFix.FIX44.NewOrderCross.NoSidesGroup();

            sideGroupV.Set(new Side(Side.SELL));
            sideGroupV.Set(new ClOrdID(ordemVenda.ClOrdID));
            if (ordemVenda.Account > 0)
                sideGroupV.Set(new Account(ordemVenda.Account.ToString()));
            sideGroupV.Set(new OrderQty(ordemVenda.OrderQty)); ;
            sideGroupV.Set(new PositionEffect('C'));
            if (ordemVenda.PositionEffect != null && ordemVenda.PositionEffect.Equals("C"))
                sideGroupV.Set(new PositionEffect('C'));

            // Cliente
            QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroup1 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
            PartyIDGroup1.Set(new PartyID(ordemVenda.ExecBroker));
            PartyIDGroup1.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup1.Set(new PartyRole(PartyRole.ENTERING_TRADER));

            // Corretora
            QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroup2 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
            PartyIDGroup2.Set(new PartyID("227"));
            PartyIDGroup2.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup2.Set(new PartyRole(PartyRole.ENTERING_FIRM));

            // Sender Location ID
            QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroup3 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
            PartyIDGroup3.Set(new PartyID("GRA"));
            PartyIDGroup3.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup3.Set(new PartyRole(54));

            // Adiciono os partIDs ao Side (perna de venda)
            sideGroupV.AddGroup(PartyIDGroup1);
            sideGroupV.AddGroup(PartyIDGroup2);
            sideGroupV.AddGroup(PartyIDGroup3);

            //BEI - 2012-Nov-14
            if (ordemVenda.ForeignFirm != null && ordemVenda.ForeignFirm.Length > 0)
            {
                QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroupV4 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                PartyIDGroupV4.Set(new PartyID(ordemVenda.ForeignFirm));
                PartyIDGroupV4.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroupV4.Set(new PartyRole(PartyRole.FOREIGN_FIRM));

                sideGroupV.AddGroup(PartyIDGroupV4);
            }

            //SelfTradeProtection - 2012-Nov-27
            if (ordemVenda.InvestorID != null && ordemVenda.InvestorID.Length > 0)
            {
                QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup PartyIDGroup4 = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                PartyIDGroup4.Set(new PartyID(ordemVenda.InvestorID));
                PartyIDGroup4.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup4.Set(new PartyRole(PartyRole.INVESTOR_ID));

                sideGroupV.AddGroup(PartyIDGroup4);
            }

            if (ordemVenda.Account > 0)
            {
                QuickFix.FIX44.NewOrderSingle.NoAllocsGroup noAllocsGrpV = new QuickFix.FIX44.NewOrderSingle.NoAllocsGroup();
                noAllocsGrpV.Set(new AllocAccount(ordemVenda.Account.ToString()));
                noAllocsGrpV.Set(new AllocAcctIDSource(99));
                sideGroupV.AddGroup(noAllocsGrpV);
            }

            // Insere os grupos das 2 pernas
            ordercross.AddGroup(sideGroupC);
            ordercross.AddGroup(sideGroupV);

            ordercross.Set(new Symbol(crossinfo.Symbol));
            if (crossinfo.SecurityID != null &&
                crossinfo.SecurityID.Length > 0)
            {
                ordercross.Set(new SecurityID(crossinfo.SecurityID));
                ordercross.Set(new SecurityIDSource("8"));
            }
            OrdType ordType = FixMessageUtilities.deOrdemTipoParaOrdType(crossinfo.OrdType);
            if (ordType != null)
                ordercross.Set(ordType);

            ordercross.Set(new Price(Convert.ToDecimal(crossinfo.Price)));
            ordercross.Set(new TransactTime(DateTime.Now));

            // Memo Field
            if (crossinfo.Memo5149 != null && crossinfo.Memo5149.Length > 0)
            {
                if (crossinfo.Memo5149.Length > 50)
                    crossinfo.Memo5149 = crossinfo.Memo5149.Substring(0, 50);

                StringField memo5149 = new StringField(5149, crossinfo.Memo5149);
                ordercross.SetField(memo5149);
            }


            bool bRet = Session.SendToTarget(ordercross, _session.SessionID);

            return bRet;
        }
        #endregion
    }
}
