using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using log4net.Core;
using System.Configuration;
using Cortex.OMS.ServidorFIXAdm.Lib.Dados;


namespace Cortex.OMS.ServidorFIX
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
                /*
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
                */
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
                /*
                gComando.Parameters.Add(new SqlParameter("@idClienteParametroInstrumento", info.idClienteParametroInstrumento));
                gComando.Parameters.Add(new SqlParameter("@idClienteParametroBMF", info.idClienteParametroBMF));
                gComando.Parameters.Add(new SqlParameter("@instrumento", info.instrumento));
                gComando.Parameters.Add(new SqlParameter("@qtTotalContratoPai", info.qtTotalContratoPai));
                gComando.Parameters.Add(new SqlParameter("@qtTotalInstrumento", info.qtTotalInstrumento));
                gComando.Parameters.Add(new SqlParameter("@qtDisponivel", info.qtDispInstrumento));
                */
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
                /*
                gComando.Parameters.Add(new SqlParameter("@idClienteParametroBMF", info.idClienteParametroBMF));
                gComando.Parameters.Add(new SqlParameter("@idClientePermissao", info.idClientePermissao));
                gComando.Parameters.Add(new SqlParameter("@account", info.account));
                gComando.Parameters.Add(new SqlParameter("@contrato", info.contrato));
                gComando.Parameters.Add(new SqlParameter("@sentido", info.sentido));
                gComando.Parameters.Add(new SqlParameter("@qtTotal", info.qtTotal));
                gComando.Parameters.Add(new SqlParameter("@qtDisponivel", info.qtDisponivel));
                gComando.Parameters.Add(new SqlParameter("@stRenovacaoAutomatica", info.stRenovacaoAutomatica));
                gComando.Parameters.Add(new SqlParameter("@dtValidade", info.dtValidade));
                */
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
