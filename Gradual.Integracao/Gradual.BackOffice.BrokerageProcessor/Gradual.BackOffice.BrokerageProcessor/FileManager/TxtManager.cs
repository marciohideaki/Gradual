using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Collections.Concurrent;
using System.Threading;
using Gradual.BackOffice.BrokerageProcessor.Lib.Txt;
using Gradual.BackOffice.BrokerageProcessor.Lib.FileWatcher;
using Gradual.BackOffice.BrokerageProcessor.Processor;

namespace Gradual.BackOffice.BrokerageProcessor.FileManager
{
    public class TxtManager
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Static Objects
        private static TxtManager _me = null;
        public static TxtManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new TxtManager();
                }

                return _me;
            }
        }
        #endregion


        #region private_variables
        ConcurrentQueue<TOArqTxt> _cqFilesTxt;
        bool _isRunning;
        Thread _thTxt;
        object _syncTxt = new object();
        #endregion 


        public void Start()
        {
            try
            {
                logger.Info("Iniciando TxtManager...");

                _cqFilesTxt = new ConcurrentQueue<TOArqTxt>();
                _isRunning = true;
                _thTxt = new Thread(new ThreadStart(this._queueTxt));
                _thTxt.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao iniciar Txt Manager: " + ex.Message, ex);
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("Parando TxtManager...");
                _isRunning = false;

                if (_thTxt != null && _thTxt.IsAlive)
                {
                    _thTxt.Join(200);
                    if (_thTxt.IsAlive)
                    {
                        try
                        {
                            _thTxt.Abort();
                        }
                        catch { }
                    }
                    _thTxt = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao parar o TxtManager: " + ex.Message, ex);
            }
        }

        public void AddTxtFile(TOArqTxt file)
        {
            _cqFilesTxt.Enqueue(file);
            lock (_syncTxt)
                Monitor.Pulse(_syncTxt);

        }

        private void _queueTxt()
        {
            while (_isRunning)
            {
                try
                {
                    TOArqTxt fileName = null;
                    if (_cqFilesTxt.TryDequeue(out fileName))
                    {
                        this._processTxt(fileName);
                    }
                    else
                    {
                        lock (_syncTxt)
                            Monitor.Wait(_syncTxt, 300);
                    }
                }
                catch { }
            }
        }

        private void _processTxt(TOArqTxt toTxt)
        {
            try
            {
                ColdFilesSplitter splitter = new ColdFilesSplitter();
                bool bSplit = false;

                switch (toTxt.Config.Type)
                {
                    case TypeWatcher.COLD_BTC:
                        bSplit = splitter.SplitArquivoBTC(toTxt);
                        break;
                    case TypeWatcher.COLD_LIQ:
                        bSplit = splitter.SplitArquivoLiquidacoes(toTxt);
                        break;
                    case TypeWatcher.COLD_POS_CLI:
                        bSplit = splitter.SplitArquivoPosicaoCliente(toTxt);
                        break;
                    case TypeWatcher.COLD_CUST:
                        bSplit = splitter.SplitArquivoCustodia(toTxt);
                        break;
                    case TypeWatcher.COLD_MARG:
                        bSplit = splitter.SplitArquivoMargem(toTxt);
                        break;
                    case TypeWatcher.COLD_GART:
                        bSplit = splitter.SplitArquivoGarantias(toTxt);
                        break;
                    case TypeWatcher.COLD_TERMO:
                        bSplit = splitter.SplitArquivoTermo(toTxt);
                        break;
                    case TypeWatcher.COLD_DIVIDENDO:
                        bSplit = splitter.SplitArquivoDividendo(toTxt);
                        break;
                    default: break;
                }

                if (!bSplit)
                {
                    logger.Warn("*** Erro ao fragmentar arquivo: " + toTxt.FileName);
                    logger.Warn("Config.ClientIdCheck ...: " + toTxt.Config.ClientIdCheck);
                    logger.Warn("Config.Exchange ........: " + toTxt.Config.Exchange);
                    logger.Warn("Config.ExtensionFilter..: " + toTxt.Config.ExtensionFilter);
                    logger.Warn("Config.FileType.........: " + toTxt.Config.FileType);
                    logger.Warn("Config.NameType ........: " + toTxt.Config.NameType);
                    logger.Warn("Config.PathBkp .........: " + toTxt.Config.PathBkp);
                    logger.Warn("Config.PathProcessed ...: " + toTxt.Config.PathProcessed);
                    logger.Warn("Config.PathWatcher .....: " + toTxt.Config.PathWatcher);
                    logger.Warn("Config.SubjectEmail ....: " + toTxt.Config.SubjectEmail);
                    logger.Warn("Config.TemplateFile ....: " + toTxt.Config.TemplateFile);
                    logger.Warn("Config.TimeToRefresh ...: " + toTxt.Config.TimeToRefresh);
                    logger.Warn("Config.Type ............: " + toTxt.Config.Type);
                    logger.Warn("*** End of report *** ");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento do arquivo txt: " + ex.Message, ex);
            }
        }
    }
}
