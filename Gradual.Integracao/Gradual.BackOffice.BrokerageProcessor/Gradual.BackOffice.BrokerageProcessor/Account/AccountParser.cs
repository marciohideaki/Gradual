using System;
using System.Collections.Concurrent;
using System.Threading;
using log4net;
using Gradual.BackOffice.BrokerageProcessor.Db;
using Gradual.BackOffice.BrokerageProcessor.Lib.Account;
using System.Configuration;

namespace Gradual.BackOffice.BrokerageProcessor.Account
{
    public class AccountParser
    {
        #region log4net declaration
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region private variables
        static AccountParser _me = null;
        ConcurrentDictionary<int, AccountParserInfo> _cdAccount;
        bool _isRunning = false;
        Thread _thAccount;
        DbAccOracle _dbOracle;
        object lck = new object();
        bool _accStripDigit;
        #endregion

        public static AccountParser Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new AccountParser();
                    _me._cdAccount = _me._dbOracle.CarregarListaContas(_me._accStripDigit);
                    logger.Info("Registros AccountParser atualizados: " + _me._cdAccount.Count);
                }
                return _me;
            }
        }
        public bool IsStarted 
        { 
            get
            {
                return _isRunning;
            }  
            internal set{}
        }


        public AccountParser()
        {
            _dbOracle = new DbAccOracle();
            lock(lck) this.IsStarted = false;

            if (ConfigurationManager.AppSettings["AccountStripDigit"] != null )
            {
                _accStripDigit = Convert.ToBoolean(ConfigurationManager.AppSettings["AccountStripDigit"].ToString());
            }
            else
                _accStripDigit = true;
        }

        public void Start()
        {
            try
            {
                if (!_isRunning)
                {
                    lock (lck)
                        _isRunning = true;
                }
                else
                    return;
                logger.Info("Iniciando Manager de Account Parser...");
                
                _thAccount = new Thread(new ThreadStart(_processAccountUpdate));
                _thAccount.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na inicializacao do Account Parser: " + ex.Message);
            }
        }


        public void Stop()
        {
            try
            {
                _isRunning = false;
                logger.Info("Parando Account Parser Manager...");
                if (_thAccount != null && _thAccount.IsAlive)
                {
                    _thAccount.Join(200);
                    if (_thAccount.IsAlive)
                    {
                        try
                        {
                            _thAccount.Abort();
                        }
                        catch { }
                    }
                    _thAccount = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no encerramento do Account Parser: " + ex.Message);
            }
        }

        private void _processAccountUpdate()
        {
            try
            {
                int refresh = 15;
                if (ConfigurationManager.AppSettings["AccountParserRefresh"] != null )
                {
                    refresh = Convert.ToInt32(ConfigurationManager.AppSettings["AccountParserRefresh"].ToString());
                }

                int vezes = (refresh * 1000) / 200;
                int i = 0;
                while (_isRunning)
                {
                    if (i >= vezes)
                    {
                        // Efetuar a atualizacao da conta
                        _cdAccount = _dbOracle.CarregarListaContas(_accStripDigit);
                        logger.Info("Registros AccountParser atualizados: " + _cdAccount.Count);
                        i = 0;
                    }
                    i++;
                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao das informacoes de conta: " +ex.Message, ex);
            }
        }

        public int GetAccountParsed(int account2Parse)
        {
            int ret = -1;
            try
            {
                AccountParserInfo item = null;
                if (_cdAccount.TryGetValue(account2Parse, out item))
                {
                    ret = item.CdCliente;
                }
            }
            catch (Exception eX)
            {
                logger.Error("Problemas na busca da conta: " + eX.Message, eX);
            }
            return ret;
        }


        public int GetAccountParsedWithDv(int account2Parse, string exchange)
        {
            int ret = 0;
            try
            {
                AccountParserInfo item = null;
                if (_cdAccount.TryGetValue(account2Parse, out item))
                {
                    ret = AccountDigit.Instance.GetAccountWithDigit(item.CdCliente, exchange); 

                }
                // Caso nao encontre, retorna o mesmo valor do parametro
                else
                {
                    ret = account2Parse;
                }
            }
            catch (Exception eX)
            {
                logger.Error("Problemas na busca da conta: " + eX.Message, eX);
                ret = -1;
            }
            return ret;
        }

        /// <summary>
        /// GetAccountBRP - retorna a conta BrasilPlural
        /// </summary>
        /// <param name="accountGradual">cod bolsa na Gradual</param>
        /// <returns>codigo de bolsa na Brasil Plural</returns>
        public int GetAccountBRP(int accountGradual)
        {
            int ret = -1;
            try
            {
                foreach(AccountParserInfo item in _cdAccount.Values)
                {
                    if ( item.CdCliente==accountGradual )
                        return Convert.ToInt32(item.CdClieOutrBolsa);
                }
            }
            catch (Exception eX)
            {
                logger.Error("Problemas na busca da conta BRP: " + eX.Message, eX);
                ret = -1;
            }
            return ret;
        }


    }
}
