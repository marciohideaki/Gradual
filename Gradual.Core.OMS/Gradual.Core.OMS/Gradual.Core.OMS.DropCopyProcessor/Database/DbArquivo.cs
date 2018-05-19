using System;
using System.Configuration;
using System.Linq;
using System.Text;
using Gradual.Core.OMS.DropCopy.Lib.Dados;
using log4net;
using log4net.Core;

namespace Gradual.Core.OMS.DropCopyProcessor.Database
{
    public class DbArquivo
    {

        protected ILog loggerCliente;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string _pathMvto;
        

        private StringBuilder _sb_prc_fix_inserir_ordem; 
        private StringBuilder _sb_prc_fix_inserir_ordem_detalhe;
        private StringBuilder _sb_prc_fix_inserir_ordem_update;
        private StringBuilder _sb_prc_fix_atualizar_ordem;

        public DbArquivo()
        {
            _sb_prc_fix_inserir_ordem = this.FormatInserirOrdem();
            _sb_prc_fix_inserir_ordem_detalhe = this.FormatInserirOrdemDetalhe();
            _sb_prc_fix_inserir_ordem_update = this.FormatInserirOrdemUpdate();
            _sb_prc_fix_atualizar_ordem = this.FormatAtualizarOrdem();

            string appender = "DropCopyProcessor_SQL";
            loggerCliente = LogManager.GetLogger(appender);
            this.AddAppender(appender, loggerCliente.Logger);

        }


        public void AddAppender(string appenderName, ILogger wLogger)
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains("BackupPath"))
                throw new Exception("Parameter 'BackupPath' is mandatory");

            string filename = ConfigurationManager.AppSettings["BackupPath"].ToString() + "\\" + appenderName + ".sql";

            log4net.Repository.Hierarchy.Logger l = (log4net.Repository.Hierarchy.Logger)wLogger;

            log4net.Appender.IAppender hasAppender = l.GetAppender(appenderName);
            if (hasAppender == null)
            {
                log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();
                appender.DatePattern = "yyyyMMdd";
                appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Date;
                appender.AppendToFile = true;
                appender.File = filename;
                appender.StaticLogFileName = true;
                appender.Name = appenderName;

                log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout();
                layout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
                layout.ActivateOptions();

                appender.Layout = layout;
                appender.ActivateOptions();

                l.AddAppender(appender);
            }
        }

        private StringBuilder FormatInserirOrdem()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append("EXECUTE prc_fix_inserir_ordem  ");
            ret.Append("{0} /*@OrderID     	       */,");
            ret.Append("'{1}' /*@ClOrdID           */,  ");
            ret.Append("{2} /*@StopStartID         */,  ");
            ret.Append("'{3}' /*@OrigClOrdID       */,  ");
            ret.Append("'{4}' /*@ExchangeNumberID  */,  ");
            ret.Append("{5} /*@Account             */,  ");
            ret.Append("'{6}' /*@Symbol            */,  ");
            ret.Append("'{7}' /*@SecurityExchangeID*/,  ");
            ret.Append("'{8}' /*@OrdTypeID         */,  ");
            ret.Append("{9} /*@OrdStatusID         */,  ");
            ret.Append("{10} /*@TransactTime     */,  ");
            ret.Append("{11} /*@ExpireDate       */,  ");
            ret.Append("{12} /*@TimeInForce        */,  ");
            ret.Append("{13} /*@ChannelID          */,  ");
            ret.Append("'{14}' /*@ExecBroker       */,  ");
            ret.Append("{15} /*@Side               */,  ");
            ret.Append("{16} /*@OrderQty           */,  ");
            ret.Append("{16} /*@OrderQtyMin        */,  ");
            ret.Append("{18} /*@OrderQtyApar       */,  ");
            ret.Append("{19} /*@OrderQtyRemaining  */,  ");
            ret.Append("{20} /*@Price              */,  ");
            ret.Append("{21} /*@CumQty		       */,");
            ret.Append("'{22}' /*@description      */,  ");
            ret.Append("{23} /*@FixMsgSeqNum       */,  ");
            ret.Append("'{24}' /*@systemID		   */,");
            ret.Append("'{25}' /*@Memo			   */,");
            ret.Append("'{26}' /*@SessionID	       */;");
            return ret;
        }

        private StringBuilder FormatInserirOrdemDetalhe()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append("EXECUTE prc_fix_inserir_ordem_detalhe  ");
            ret.Append("'{0}'  /*@TransactID       */,");
            ret.Append("'{1}'  /*@ClOrdID          */,");
            ret.Append("{2}  /*@OrderQty           */,");
            ret.Append("{3}  /*@OrdQtyRemaining    */,");
            ret.Append("{4}  /*@CumQty             */,");
            ret.Append("{5}  /*@TradeQty           */,");
            ret.Append("{6}  /*@Price              */,");
            ret.Append("{7}  /*@OrderStatusID      */,");
            ret.Append("'{8}'  /*@Description	   */,");
            ret.Append("{9}  /*@EventTime		   */,");
            ret.Append("'{10}' /*@CxlRejResponseTo */,");
            ret.Append("{11} /*@CxlRejReason	   */,");
            ret.Append("{12} /*@FixMsgSeqNum	   */;");
            return ret;
        }

        private StringBuilder FormatInserirOrdemUpdate()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append("EXECUTE prc_fix_inserir_ordem_update  ");
            ret.Append("{0}  /*@OrderID 			*/,");
            ret.Append("{1}  /*@account 			*/,");
            ret.Append("'{2}'  /*@instrumento 		*/,");
            ret.Append("'{3}'  /*@CLOrdID 			*/,");
            ret.Append("'{4}'  /*@OrdStatusID 		*/,");
            ret.Append("{5}  /*@price   			*/,");
            ret.Append("{6}  /*@quantidade 			*/,");
            ret.Append("{7}  /*@quantidade_exec		*/,");
            ret.Append("{8}  /*@quantidade_aparente */,");
            ret.Append("{9}  /*@quantidade_minima 	 */,");
            ret.Append("{10}  /*@dt_validade 		 */,");
            ret.Append("{11}  /*@TimeInForce 		 */;");		
            return ret;
        }

        private StringBuilder FormatAtualizarOrdem()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append("EXECUTE prc_fix_atualizar_ordem  ");
            ret.Append("{0} /*@OrderID           */,");
            ret.Append("'{1}' /*@ClOrdID           */,");
            ret.Append("'{2}' /*@OrigClOrdID       */,");
            ret.Append("'{3}' /*@ExchangeNumberID  */,");
            ret.Append("{4} /*@OrdStatusID       */,");
            ret.Append("{5} /*@TransactTime      */,");
            ret.Append("{6} /*@ExpireDate        */,");
            ret.Append("{7} /*@TimeInForce       */,");
            ret.Append("{8} /*@OrderQty          */,");
            ret.Append("{9} /*@OrderQtyMin       */,");
            ret.Append("{10} /*@OrderQtyApar      */,");
            ret.Append("{11} /*@OrderQtyRemaining */,");
            ret.Append("{12} /*@CumQty			   */,");
            ret.Append("{13} /*@Price             */,");
            ret.Append("{14} /*@FixMsgSeqNum	   */,");
            ret.Append("'{15}' /*@Memo			   */;");
   
            return ret;
        }

        public void InserirOrdem(OrderDbInfo order)
        {
            try
            {
                loggerCliente.InfoFormat(_sb_prc_fix_inserir_ordem.ToString(),
                                          order.OrderID, order.ClOrdID, order.StopStartID, order.OrigClOrdID, order.ExchangeNumberID,
                                          order.Account, order.Symbol, order.SecurityExchangeID, order.OrdTypeID, order.OrdStatus,
                                          DateTime.MinValue == order.TransactTime ? "NULL" : "'" + order.TransactTime.ToString("yyyy-MM-dd HH:mm:sss.fff") + "'",
                                          DateTime.MinValue == order.ExpireDate ? "NULL" : "'" + order.ExpireDate.ToString("yyyy-MM-dd HH:mm:sss.fff") + "'", 
                                          order.TimeInForce, order.ChannelID, order.ExecBroker,
                                          order.Side, order.OrderQty, order.OrderQtyMin, order.OrderQtyApar, order.OrderQtyRemaining,
                                          order.Price, order.CumQty, order.Description, order.FixMsgSeqNum, order.SystemID, order.Memo,
                                          order.SessionID);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na escrita do arquivo InserirOrdem: " + ex.Message, ex);
            }
        }

        public void InserirOrdemDetalhe(OrderDbDetalheInfo detail, string clordid)
        {
            try
            {
                loggerCliente.InfoFormat(_sb_prc_fix_inserir_ordem_detalhe.ToString(),
                                        detail.TransactID, clordid, detail.OrderQty, detail.OrderQtyRemaining, detail.CumQty, detail.TradeQty,
                                        detail.Price, detail.OrderStatusID, detail.Description,
                                        DateTime.MinValue == detail.TransactTime ? "NULL" : "'" + detail.TransactTime.ToString("yyyy-MM-dd HH:mm:sss.fff") + "'",
                                        detail.CxlRejResponseTo,
                                        detail.CxlRejReason, detail.FixMsgSeqNum);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na escrita do arquivo InserirOrdemDetalhe: " + ex.Message, ex);
            }
        }

        public void InserirOrdemUpdate(OrderDbUpdateInfo update)
        {
            try
            {
                loggerCliente.InfoFormat(_sb_prc_fix_inserir_ordem_update.ToString(),
                                        update.OrderID, update.Account, update.Instrumento, update.ClOrdID, update.OrdStatusID,
                                        update.Price, update.Price, update.Quantidade, update.Quantidade_Exec, update.Quantidade_Aparente,
                                        update.Quantidade_Minima,
                                        DateTime.MinValue == update.Dt_Validade ? "NULL" : "'" + update.Dt_Validade.ToString("yyyy-MM-dd HH:mm:sss.fff") + "'",
                                        update.TimeInForce);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na escrita do arquivo InserirOrdemUpdate: " + ex.Message, ex);
            }
        }

        public void AtualizarOrdem(OrderDbInfo order)
        {
            try
            {
                loggerCliente.InfoFormat(_sb_prc_fix_atualizar_ordem.ToString(),
                                         order.OrderID, order.ClOrdID, order.OrigClOrdID, order.ExchangeNumberID, order.OrdStatus,
                                         DateTime.MinValue == order.TransactTime ? "NULL" : "'" + order.TransactTime.ToString("yyyy-MM-dd HH:mm:sss.fff") + "'",
                                         DateTime.MinValue == order.ExpireDate ? "NULL" : "'" + order.ExpireDate.ToString("yyyy-MM-dd HH:mm:sss.fff") + "'", 
                                         order.TimeInForce, order.OrderQty, order.OrderQtyMin,
                                         order.OrderQtyApar, order.OrderQtyRemaining, order.CumQty, order.Price, order.FixMsgSeqNum,
                                         order.Memo);

            }
            catch (Exception ex)
            {
                logger.Error("Problemas na escrita do arquivo AtualizarOrdem: " + ex.Message, ex);
            }
        }
    }






}
