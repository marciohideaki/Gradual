using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using log4net.Core;
using System.Configuration;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;
//using Cortex.OMS.ServidorFIXAdm.Lib.Dados;


namespace Gradual.Core.OMS.LimiteManager.Database
{
    public class PersistenciaArquivo
    {
        protected ILog loggerCliente;
        //private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string _pathMvto;

        //int _codCliente;
        public int CodCliente
        {
            get;
            internal set;
        }

        public string Exchange
        {
            get;
            internal set;
        }

        public PersistenciaArquivo(string exchange, int codCliente)
        {
            _pathMvto = string.Empty;
            this.CodCliente = codCliente;
            this.Exchange = exchange;

            string appender = exchange + "." + this.CodCliente.ToString();
            loggerCliente = LogManager.GetLogger(appender);
            this.AddAppender(appender, loggerCliente.Logger);
        }

        public void AddAppender(string appenderName, ILogger wLogger)
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains("FinancialMovementPath"))
                throw new Exception("Parameter 'FinancialMovementPath' is mandatory");

            string filename = ConfigurationManager.AppSettings["FinancialMovementPath"].ToString() + "\\" + appenderName + ".log";

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

        public bool InserirMvtoBovespa(OperatingLimitInfo item)
        {
            try
            {
                loggerCliente.InfoFormat("CodigoCliente: [{0}]   TipoLimite: [{1}]   PrecoBase: [{2}]   ValorAlocado: [{3}]   ValorDisponivel: [{4}]   ValorTotal: [{5}]",
                    item.CodigoCliente, item.TipoLimite, item.PrecoBase, item.ValorAlocado, item.ValorDisponivel, item.ValotTotal);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public bool InserirMvtoBmf(ClientLimitBMFInfo item)
        {
            try
            {
                loggerCliente.InfoFormat("CodigoCliente: [{0}]   C.[{1}]  C.MaxOferta: [{2}]  C.Disp: [{3}] C.Total: [{4}] C.Sentido:[{5}] C.DataMovimento:[{6}]",
                                         item.Account, item.ContractLimit[0].Contrato, item.ContractLimit[0].QuantidadeMaximaOferta,
                                         item.ContractLimit[0].QuantidadeDisponivel, item.ContractLimit[0].QuantidadeTotal, item.ContractLimit[0].Sentido,
                                         item.ContractLimit[0].DataMovimento.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                loggerCliente.InfoFormat("CodigoCliente: [{0}]   I.[{1}]  I.MaxOferta: [{2}]  I.Disp: [{3}] I.Total: [{4}] I.ContratoPai:[{5}] I.Sentido:[{6}] I.DataMovimento:[{7}]",
                                         item.Account, item.InstrumentLimit[0].Instrumento, item.InstrumentLimit[0].QuantidadeMaximaOferta,
                                         item.InstrumentLimit[0].QtDisponivel, item.InstrumentLimit[0].QtTotalInstrumento, item.InstrumentLimit[0].QtTotalContratoPai,
                                         item.InstrumentLimit[0].Sentido, item.InstrumentLimit[0].dtMovimento.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                
                return true;
            }
            catch 
            {
                return false;
            }
        }


        
        /*
        /// <summary>
        /// Insercao de movimentacao Bovespa
        /// </summary>
        /// <param name="movto"></param>
        /// <returns></returns>
        public bool InserirMovimentoLimite(LimiteMovimentoInfo movto)
        {
            try
            {

                loggerCliente.InfoFormat("Account[{0}] IdLimite[{1}] IdLancamento[{2}] Instrumento[{3}] " +
                                         "Preco[{4}], Quantidade[{5}] ValorTotal[{6}] ValorConsumido[{7}] " +
                                         "ValorRestante[{8}] DataMovimento[{9}] DataReferencia[{10}]",
                                         movto.Account, movto.idLimite, movto.idLancamento, movto.instrumento,
                                         movto.Preco, movto.quantidade, movto.valorTotal, movto.valorConsumido,
                                         movto.valorRestante, movto.dataMovimento, DateTime.Now.ToString());
                
                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// Instrumento BMF
        /// </summary>
        /// <param name="movto"></param>
        /// <returns></returns>
        public bool InserirMovimentoLimiteInstrumentoBMF(LimiteClienteInstrumentoBMF info)
        {
            try
            {
                loggerCliente.InfoFormat("IdClienteParametroInstrumento[{0}] IdClienteParametroBMF[{1}] Instrumento[{2}] " +
                                         "QtTotalContratoPai[{3}] QtTotalInstrumento[{4}] QtDisponivel[{5}]",
                                         info.idClienteParametroInstrumento, info.idClienteParametroInstrumento, info.instrumento,
                                         info.qtTotalContratoPai, info.qtTotalInstrumento, info.qtDispInstrumento);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Inserir Movimentacao contrato BMF
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool InserirMovimentoLimiteContratoBMF(LimiteClienteContratoBMF info)
        {
            try
            {

                loggerCliente.InfoFormat("IdClienteParametroBMF[{0}] IdClientePermissao[{1}] Account[{2}] Contrato[{3}] " + 
                                         "Sentido[{4}] QtTotal[{5}] QtDisponivel[{6}] StRenovacaoAutomatica[{7}] " +
                                         "DtValidade[{8}]",
                                         info.idClienteParametroBMF, info.idClientePermissao, info.account, info.contrato,
                                         info.sentido, info.qtTotal, info.qtDisponivel, info.stRenovacaoAutomatica, 
                                         info.dtValidade);
                
                return true;
            }
            catch
            {
                return false;
            }
        }*/
    }
}
