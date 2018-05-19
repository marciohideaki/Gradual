using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

using QuickFix.Fields;
using QuickFix.FIX44;
using QuickFix;

using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Gradual.Core.OMS.FixServerLowLatency.Memory;

namespace Gradual.Core.OMS.FixServerLowLatency.Regras
{
    public class OrdensTransicao
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public OrdensTransicao()
        {

        }
        
        /// <summary>
        /// Validar os estados da ordem e executar as respectivas operacoes no dicionario de mensagens
        /// (Mensagens de Execution Report - ER)
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="eType"></param>
        /// <param name="oStatus"></param>
        /// <param name="dic"></param>
        /// Retorna o TOORderSession referente ao Execution Report
        /// OBS: strChave - no momento esta considerando sempre clOrdID
        public void VerifyOrderSituationER(ExecutionReport msg, char e, char o, SessionID ss, 
                                           OrderSessionManager dic, string strChave, out TOOrderSession ord)
        
        {
            ord = null;
            try
            {
                string strOrigClChave = string.Empty;
                string strExchNumberChave = msg.OrderID.ToString() + "-" + msg.Account.ToString() + "-" + msg.Symbol.ToString();
                if (null != ss)// Registry found
                {
                    // Atualiza o TO com o ExchangeNumber (Tag 37, order ID)
                    TOOrderSession aux = null;
                    dic.GetOrder(strChave, out aux);
                    if (null != aux)
                        aux.ExchangeNumberID = strExchNumberChave;
                                        
                    // New Order, Exec: New, OrdStatus: New - nao faz nada, somente retorna o TOOrderSession correspondente
                    if (e == ExecType.NEW && o == OrdStatus.NEW) 
                    {
                        // Por ser new order, nao se aplica fazer busca pelo exchange number id 
                        // pois ainda não foi atualizado
                        dic.GetOrder(strChave, out ord); 
                        //ord.ExchangeNumberID = msg.OrderID.getValue() + "-" + msg.Account.getValue() + "-" + msg.Symbol.getValue();
                    }
                    // Retornar o TOOrderSession correspondente para 
                    if (o == OrdStatus.FILLED || o == OrdStatus.PARTIALLY_FILLED)
                    {
                        dic.GetOrder(strChave, out ord, strExchNumberChave);
                        //ord.ExchangeNumberID = msg.OrderID.getValue() + "-" + msg.Account.getValue() + "-" + msg.Symbol.getValue();
                    }

                    // Stop Order Entry, Exec: New, OrdStatus: New - nao faz nada
                    // No request stop order trigger, Exec: New, OrdStatus: New - nao faz nada
                    // Order with on close, Exec: New, OrdStatus: New
                    // Order with on close attribute is activated when the closing auction starts, Exec: New, Order: New
                    // MinQty order entry, not enough quantity, Exec: new, Order: new

                    // New Order, Exec: Rejected, OrdStatus: Rejected - excluir a chave
                    // New Order, Exec: Suspended, OrdStatus: Suspended
                    // No request, Exec: Trade, OrdStatus: Filled
                    if ((e == ExecType.REJECTED && o == OrdStatus.REJECTED) ||
                         (e == ExecType.SUSPENDED && o == OrdStatus.SUSPENDED) ||
                         (e == ExecType.TRADE && o == OrdStatus.FILLED) ||
                         (e == ExecType.EXPIRED && o == OrdStatus.EXPIRED))
                    {
                        lock (dic)
                        {
                            TOOrderSession toOS = null;
                            int ret = dic.GetOrder(strChave, out toOS, strExchNumberChave);
                            ord = toOS;
                            toOS = null;
                            if (ret == FindType.EXCHANGE_NUMBER)
                                dic.RemoveOrder(ord.ChaveDicionario);
                            else
                                dic.RemoveOrder(strChave);
                        }
                    }
                    // Order Modify, Exec: Replace, OrdStatus: Replaced
                    if (e == ExecType.REPLACE && o == OrdStatus.REPLACED)
                    {
                        strOrigClChave = msg.OrigClOrdID.ToString() + "-" + msg.Account.ToString()+ "-" + msg.Symbol.ToString();
                        lock (dic)
                        {
                            //if (dic.ExistOrder(strOrigClChave))
                            //{
                                TOOrderSession toOS = null;
                                int ret = dic.GetOrder(strOrigClChave, out toOS, strExchNumberChave, KeyType.ORIGCLORDID);
                                ord = toOS;
                                toOS = null;
                                if (ret == FindType.EXCHANGE_NUMBER)
                                    dic.RemoveOrder(ord.ChaveDicionario);
                                else
                                    dic.RemoveOrder(strOrigClChave);
                            //}
                        }
                    }
                    // Cancelation, Exec: Cancelled, OrdStatus: Cancelled
                    // No request FAK Partially Filled, Exec: Canceled, OrdStatus: Canceled
                    // No request FOK Partially Filled, Exec: Canceled, OrdStatus: Canceled
                    // MinQty order entry, not enough quantity  rejected, Exec: Cancelled, Order: Cancelled
                    if (e == ExecType.CANCELED && o == OrdStatus.CANCELED)
                    {
                        bool processOrig = true;
                        // Tratamento de outros tipos de timeinforce (para execucao e cancelamento, o orig clord id não é fornecido)
                        if (msg.IsSetField(Tags.TimeInForce))
                        {
                            switch (msg.TimeInForce.getValue())
                            {
                                case TimeInForce.IMMEDIATE_OR_CANCEL:
                                case TimeInForce.FILL_OR_KILL:
                                    processOrig = false;
                                    strOrigClChave = msg.ClOrdID.ToString() + "-" + msg.Account.ToString() + "-" + msg.Symbol.ToString();
                                    break;
                                default:
                                    {
                                        // Caso o cancelamento tenha partido da bolsa, tambem nao eh fornecido o OrigClOrdID
                                        // entao tenta-se utilizar o ClOrdID vindo do ExecutionReport
                                        string aux1 = msg.IsSetOrigClOrdID() ? msg.OrigClOrdID.ToString() : msg.ClOrdID.ToString();
                                        strOrigClChave = aux1 + "-" + msg.Account.ToString() + "-" + msg.Symbol.ToString();
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            // TimeInForce = 0 (DAY) - Default
                            // Caso o Orig nao esteja na mensagem, tenta-se buscar pelo ClOrdID 
                            string aux1 = msg.IsSetOrigClOrdID() ? msg.OrigClOrdID.ToString() : msg.ClOrdID.ToString();
                            strOrigClChave = aux1 + "-" + msg.Account.ToString() + "-" + msg.Symbol.ToString();
                        }
                        lock (dic)
                        {
                            TOOrderSession toOS = null;
                            int ret = 0;
                            ret = dic.GetOrder(strOrigClChave, out toOS, strExchNumberChave, KeyType.ORIGCLORDID);
                            if (null!=toOS)
                            {
                                ord = toOS;
                                toOS = null;
                                if (ret ==FindType.EXCHANGE_NUMBER)
                                    dic.RemoveOrder(ord.ChaveDicionario);
                                else
                                    dic.RemoveOrder(strOrigClChave);
                            }
                            if (processOrig)
                            {
                                ret = dic.GetOrder(strChave, out toOS, strExchNumberChave);
                                if (ret == FindType.EXCHANGE_NUMBER)
                                    dic.RemoveOrder(toOS.ChaveDicionario);
                                else
                                    dic.RemoveOrder(strChave);
                                toOS = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("VerifyOrderSituationER(): " + ex.Message, ex);
            }
        }
        
        /// <summary>
        /// Manipulacao do dicionario em situacoes 
        /// (Mensagens de Order Cancel Reject - OCR)
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="dic"></param>
        /// <param name="strChave"></param>
        //public void VerifyOrderSituationOCR(SessionID ss, Dictionary<string, TOOrderSession> dic, string strChave)
        // OBS :strChave sempre ClOrdID até momento
        public void VerifyOrderSituationOCR(OrderCancelReject msg,  SessionID ss, OrderSessionManager dic, string strChave, string strChaveExch)
        {
            try
            {
                if (null != ss) // Registry found
                {

                    TOOrderSession aux = null;
                    dic.GetOrder(strChave, out aux);
                    if (null!=aux && msg.IsSetField(Tags.OrderID) && msg.OrderID.getValue()!="NONE")
                    {
                        if (msg.IsSetField(Tags.Account))
                            aux.ExchangeNumberID = msg.OrderID.getValue() + "-" + msg.Account.getValue() + "-" + msg.Symbol.getValue();
                    }
                    // Se houve rejeicao de cancelamento, entao somente excluir do dicionario
                    lock (dic)
                    {
                        TOOrderSession toOS = null;
                        int ret = dic.GetOrder(strChave, out toOS, strChaveExch);
                        if (null!= toOS)
                        {
                            if (ret == FindType.EXCHANGE_NUMBER)
                                dic.RemoveOrder(toOS.ChaveDicionario);
                            else
                                dic.RemoveOrder(strChave);
                        }
                        toOS = null;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("VerifyOrderSituationOCR(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Manipulacao de dicionario
        /// Mensagem BusinessMessageReject (BMR and RReject Messages)
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="dic"></param>
        /// <param name="strChave"></param>
        //public void VerifyOrderSituationBMRandR(SessionID ss, Dictionary<string, TOOrderSession> dic, string strChave)
        public void VerifyOrderSituationBMRandR(SessionID ss, OrderSessionManager dic, string strChave)
        {
            try
            {
                if (null != ss) // Registry found
                {
                    // Se houve rejeicao de cancelamento, entao somente excluir do dicionario
                    lock (dic)
                    {
                        TOOrderSession toOS = null;
                        dic.GetOrder(strChave, out toOS);
                        toOS = null;
                        dic.RemoveOrder(strChave);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("VerifyOrderSituationMBR(): " + ex.Message, ex);
            }
        }
    }
}
