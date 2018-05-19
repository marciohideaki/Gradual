using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using System.Threading;
using System.Configuration;
using Gradual.BackOffice.BrokerageProcessor.Lib.FileWatcher;
using Gradual.BackOffice.BrokerageProcessor.Lib.Pdf;
using Gradual.BackOffice.BrokerageProcessor.FileManager;
using Gradual.BackOffice.BrokerageProcessor.Lib.Txt;

namespace Gradual.BackOffice.BrokerageProcessor.Watcher
{
    public class FileWatcherManager
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Static Objects

        //private static FileWatcherManager _me = null;
        //public static FileWatcherManager Instance
        //{
        //    get
        //    {
        //        if (_me == null)
        //        {
        //            _me = new FileWatcherManager();
        //        }

        //        return _me;
        //    }
        //}
        #endregion

        #region Properties
        public FileWatcherConfigItem Config { get; set; }
        #endregion



        #region private variables
        // FileSystemWatcher _watcher;
        Thread _thWatcher;
        bool _isRunning;


        #endregion


        public void Start()
        {
            try
            {
                logger.Info("Iniciando File Watcher Manager...");

                if (string.IsNullOrEmpty(this.Config.PathWatcher))
                {
                    logger.Error("Path to Watch is Empty!!!!!");
                    return;
                }

                if (string.IsNullOrEmpty(this.Config.PathBkp))
                {
                    logger.Error("Path Bkp is Empty!!!!!");
                    return;
                }

                logger.Info("PathWatcher: " + this.Config.PathWatcher);
                logger.Info("PathBkp: " + this.Config.PathBkp);
                logger.Info("PathProcessed: " + this.Config.PathProcessed);

                // Formatando padrao dos diretorios
                int len = this.Config.PathBkp.Length;
                if (!this.Config.PathBkp[len-1].Equals(Path.DirectorySeparatorChar))
                    this.Config.PathBkp += Path.DirectorySeparatorChar;
                len = this.Config.PathWatcher.Length;
                if (!this.Config.PathWatcher[len - 1].Equals(Path.DirectorySeparatorChar))
                    this.Config.PathWatcher += Path.DirectorySeparatorChar;


                logger.Info("Criando watcher do diretorio");
                _isRunning = true;

                _thWatcher = new Thread(new ThreadStart(this._directoryWatcher));
                _thWatcher.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao iniciar o file watcher: " + ex.Message, ex);
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("Parando File Watcher Manager...");
                _isRunning = false;
                
                // Parando Thread Watcher
                if (_thWatcher != null && _thWatcher.IsAlive)
                {
                    _thWatcher.Join(200);
                    try
                    {
                        if (_thWatcher.IsAlive)
                            _thWatcher.Abort();
                    }
                    catch 
                    {
                        logger.Error("Thread aborted!");
                    }
                    _thWatcher = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao efetuar a parada do file watcher: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Criar os diretorios caso nao existam
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public bool CreatePaths()
        {
            bool ret = true;
            List<String> lst = new List<string>();
            lst.Add(this.Config.PathWatcher);
            lst.Add(this.Config.PathBkp);
            lst.Add(this.Config.PathProcessed);
            foreach (string path in lst)
            {
                try
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Problemas na criacao do diretorio: [{0}] Msg:[{1}]",path, ex.StackTrace + " " + ex.Message);
                    ret = false;
                }
            }
            return ret;            
        }

        #region private functions
        private bool _isFileReady(String sFilename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (inputStream.Length > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        private void _directoryWatcher()
        {
            try
            {
                int timeRefresh = 5;
                
                timeRefresh = this.Config.TimeToRefresh;
                int times = Convert.ToInt32(timeRefresh / 0.250);
                int i = 0;
                bool isProcessing = false;
                while (_isRunning)
                {
                    if (i>=times && !isProcessing)
                    {
                        string [] filePaths;
                        isProcessing = true;
                        i = 0;
                        if (string.IsNullOrEmpty(this.Config.ExtensionFilter))
                            filePaths = Directory.GetFiles(this.Config.PathWatcher);
                        else
                            filePaths = Directory.GetFiles(this.Config.PathWatcher, this.Config.ExtensionFilter);
                        logger.Info("Arquivos encontrados: " + filePaths.Length);
                        foreach (string item in filePaths)
                        {
                            logger.Debug("Movendo Arquivo: " + item);
                            this._moveFile(item);
                        }
                        isProcessing = false;
                    }
                    i++;
                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na varredura do diretorio: " + ex.Message, ex);
            }
        }

        private void _moveFile(string fileName)
        {
            try
            {
                if (_isFileReady(fileName))
                {
                    string pathBkp = this.Config.PathBkp + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd");

                    if (!Directory.Exists(pathBkp))
                    {
                        Directory.CreateDirectory(pathBkp);
                    }

                    string onlyFileName = fileName.Substring(fileName.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                    string fileBkp = pathBkp + Path.DirectorySeparatorChar + onlyFileName;

                    if (!File.Exists(fileBkp))
                    {
                        File.Move(fileName, fileBkp);
                        switch (this.Config.FileType)
                        {
                            case FileTypes.PDF:
                                TOArqPdf toPdf = new TOArqPdf();
                                toPdf.FileName = fileBkp;
                                toPdf.Config = this.Config;
                                PdfManager.Instance.AddPdfFile(toPdf);
                                break;
                            case FileTypes.TXT:
                                TOArqTxt toTxt = new TOArqTxt();
                                toTxt.FileName = fileBkp;
                                toTxt.Config = this.Config;
                                TxtManager.Instance.AddTxtFile(toTxt);
                                break;
                        }
                    }
                    else
                    {
                        string ext = fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
                        string newext = DateTime.Now.ToString(".yyyy-MM-dd-HH-mm-ss-fff") + ext;
                        string newFileName = fileBkp.Substring(0, fileBkp.LastIndexOf('.')) + newext;
                        File.Move(fileName, newFileName);

                        switch (this.Config.FileType)
                        {
                            case FileTypes.PDF:
                                TOArqPdf toPdf = new TOArqPdf();
                                toPdf.FileName = newFileName;
                                toPdf.Config = this.Config;
                                PdfManager.Instance.AddPdfFile(toPdf);
                                break;
                            case FileTypes.TXT:
                                TOArqTxt toTxt = new TOArqTxt();
                                toTxt.FileName = newFileName;
                                toTxt.Config = this.Config;
                                TxtManager.Instance.AddTxtFile(toTxt);
                                break;
                        }
                        
                    }
                }
                else
                {
                    logger.Debug("File not ready: " + fileName);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao mover o arquivo: " + ex.Message, ex);
            }
        }

        #endregion
    }
}
