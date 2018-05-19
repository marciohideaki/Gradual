using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using Gradual.BackOffice.BrokerageProcessor.Lib.MTA;
using Gradual.BackOffice.BrokerageProcessor.Lib.SFTP;
using ICSharpCode.SharpZipLib.Zip;
using Gradual.BackOffice.BrokerageProcessor.Db;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;
using System.Data;
using System.Runtime.InteropServices;
using Gradual.OMS.Library;
using System.Globalization;

namespace Gradual.BackOffice.BrokerageProcessor
{
    public class MTAFilesProcessor
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private static MTAFilesProcessor _me = null;
        private static bool bCLCO = false;
        private static bool bCMDF = false;
        private static bool bCSGD = false;
        private static bool bPFEN = false;
        private static bool bPENR = false;
        private static bool bBuscarFTP = false;
        private static bool bFtpcs = false;


        private string pathMTAGradual = ConfigurationManager.AppSettings["PathMTAGradual"].ToString();
        private string pathMTABrasilPlural = ConfigurationManager.AppSettings["PathMTABrasilPlural"].ToString();
        private string pathMTAMerged = ConfigurationManager.AppSettings["PathMTAMerged"].ToString();
        private string pathMTABackup = ConfigurationManager.AppSettings["PathMTABackup"].ToString();
        private string sftpRemoteDir = ConfigurationManager.AppSettings["SFTPRemoteDir"].ToString();
        private string sftpLocalDir = ConfigurationManager.AppSettings["SFTPLocalDir"].ToString();

        private string pathJava = String.Empty;
        private string ftpcsSite = String.Empty;
        private string ftpcsWorkDir = String.Empty;
        private string ftpcsLocalDir = String.Empty;
        private string ftpcsScriptDir = String.Empty;
        private string ftpcsTempDir = String.Empty;
        private string ftpcsOutputDir = String.Empty;
        private string ftpcsPwd = String.Empty;
        private string ftpcsUsr = String.Empty;

        private static Dictionary<string, DownloadMTAInfo> dcControle = new Dictionary<string, DownloadMTAInfo>();

        private static List<string> lstGenericos = new List<string>();

        private Process process = null;

        private enum XfbError
        {
            OK=0,               //All commands ended without error
            SITE_UNKNOWN=1,     //Connecting site is unknown
            INVALID_COMAND=2,
            SCRIPT_ERROR=3,     //Syntax error or script execution error
            FTP_ERROR =4,        //FTP error
            ALREADY_RUNNING=6   // Another instance is running
        }


        public static MTAFilesProcessor Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new MTAFilesProcessor();
                }

                return _me;
            }
        }

        public MTAFilesProcessor()
        {
            try
            {
                Directory.CreateDirectory(pathMTAGradual);
                Directory.CreateDirectory(pathMTABrasilPlural);
                Directory.CreateDirectory(pathMTAMerged);
                Directory.CreateDirectory(pathMTABackup);

                syncBD();

                configFtpcs();
            }
            catch (Exception ex)
            {
                logger.Fatal("MTAFilesProcessor:ctor(): " + ex.Message, ex);
            }
        }

        private void configFtpcs()
        {
            try
            {
                pathJava = ConfigurationManager.AppSettings["PathJava"].ToString();
                ftpcsSite = ConfigurationManager.AppSettings["FtpcsSite"].ToString();
                ftpcsWorkDir = ConfigurationManager.AppSettings["FtpcsWorkDir"].ToString();
                ftpcsLocalDir = ConfigurationManager.AppSettings["FtpcsLocalDir"].ToString();
                ftpcsScriptDir = ConfigurationManager.AppSettings["FtpcsScriptDir"].ToString();
                ftpcsTempDir = ConfigurationManager.AppSettings["FtpcsTmpDir"].ToString();
                ftpcsOutputDir = ConfigurationManager.AppSettings["FtpcsOutputDir"].ToString();
                ftpcsPwd = ConfigurationManager.AppSettings["FtpcsPwd"].ToString();
                ftpcsUsr = ConfigurationManager.AppSettings["FtpcsUsr"].ToString();


                if (!Directory.Exists(ftpcsLocalDir))
                    Directory.CreateDirectory(ftpcsLocalDir);

                if (!Directory.Exists(ftpcsScriptDir))
                    Directory.CreateDirectory(ftpcsScriptDir);

                if (!Directory.Exists(ftpcsTempDir))
                    Directory.CreateDirectory(ftpcsTempDir);

                if (!Directory.Exists(ftpcsOutputDir))
                    Directory.CreateDirectory(ftpcsOutputDir);
            }
            catch (Exception ex)
            {
                logger.Error("configFtpcs: " + ex.Message, ex);
            }
        }

        //public void CopiaArquivos()
        //{
        //    try
        //    {

        //        // Get the subdirectories for the specified directory.
        //        DirectoryInfo dir = new DirectoryInfo(sourceDir);

        //        if (!dir.Exists)
        //        {
        //            throw new DirectoryNotFoundException(
        //                "Source directory does not exist or could not be found: "
        //                + sourceDir);
        //        }

        //        // Get the files in the directory and copy them to the new location.
        //        FileInfo[] files = dir.GetFiles();
        //        foreach (FileInfo file in files)
        //        {
        //            string temppath = Path.Combine(destDir, file.Name);
        //            file.CopyTo(temppath, false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("CopiaArquivos: " + ex.Message, ex);
        //    }
        //}

        public void BuscarFTP()
        {
            try
            {
                if (IsFeriado(DateTime.Now))
                {
                    logger.Info("Uhuhu Feriadao, dia de curticao.....");
                    return;
                }

                if (bBuscarFTP)
                {
                    logger.Info("Outra operacao de download em FTP em andamento...");
                    return;
                }

                if (bCLCO || bCMDF || bCSGD || bPFEN || bPENR )
                {
                    logger.Info("Arquivos em processamento, adiando FTP...");
                    return;
                }

                syncBD();

                bBuscarFTP = true;

                if (!Directory.Exists(sftpLocalDir))
                    Directory.CreateDirectory(sftpLocalDir);

                SFtpClient ftpClient = new SFtpClient();

                ftpClient.SFTPHost = ConfigurationManager.AppSettings["SFTPHost"].ToString();
                ftpClient.SFTPUser = ConfigurationManager.AppSettings["SFTPUser"].ToString();
                ftpClient.SFTPPassword = ConfigurationManager.AppSettings["SFTPPassword"].ToString();

                if (ConfigurationManager.AppSettings["SFTPPort"] != null)
                {
                    ftpClient.SFTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SFTPPort"].ToString());
                }

                List<string> searchPattern = new List<string>();

                lock (dcControle)
                {
                    if (!dcControle.ContainsKey(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        DBControleMTA.InserirControleDownloadMTA();
                        dcControle.Add(DateTime.Now.ToString("yyyyMMdd"), new DownloadMTAInfo());
                    }
                }

                // Se for uma segunda feira, pegar o arquivo de sabado
                DateTime dataCMDF = DateTime.Now;
                if (dataCMDF.DayOfWeek == DayOfWeek.Monday)
                    dataCMDF = dataCMDF.AddDays(-2);

                while (IsFeriado(dataCMDF))
                    dataCMDF = dataCMDF.AddDays(-1);

                //if ( !dcControle[DateTime.Now.ToString("yyyyMMdd")].HasCMDF )
                //    searchPatern.Add("CMDF" + dataCMDF.ToString("yyyyMMdd"));

                //if (!dcControle[DateTime.Now.ToString("yyyyMMdd")].HasCSGD)
                //    searchPatern.Add("CSGD" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                if (!dcControle[DateTime.Now.ToString("yyyyMMdd")].HasCMDF)
                {
                    searchPattern.Add("CMDF" + DateTime.Now.ToString("yyyyMMdd"));
                    searchPattern.Add("CMDF" + dataCMDF.ToString("yyyyMMdd"));
                    searchPattern.Add("CMDF" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));
                }

                if (!dcControle[DateTime.Now.ToString("yyyyMMdd")].HasCSGD)
                {
                    searchPattern.Add("CSGD" + DateTime.Now.ToString("yyyyMMdd"));
                    searchPattern.Add("CSGD" + dataCMDF.ToString("yyyyMMdd"));
                    searchPattern.Add("CSGD" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));
                }

                if (!dcControle[DateTime.Now.ToString("yyyyMMdd")].HasCLCO)
                    searchPattern.Add("CLCO" + DateTime.Now.ToString("yyyyMMdd"));

                if (!dcControle[DateTime.Now.ToString("yyyyMMdd")].HasPENR)
                    searchPattern.Add("PENR" + DateTime.Now.ToString("yyyyMMdd"));

                if (!dcControle[DateTime.Now.ToString("yyyyMMdd")].HasPFEN)
                    searchPattern.Add("PFEN" + DateTime.Now.ToString("yyyyMMdd"));

                if (ConfigurationManager.AppSettings["BVBGPrefixList"] != null)
                {
                    string[] bvbgPrefixList = ConfigurationManager.AppSettings["BVBGPrefixList"].ToString().Split(new char[] { ';', ',' });

                    foreach (string bvbgPrefix in bvbgPrefixList)
                    {
                        searchPattern.Add(bvbgPrefix + DateTime.Now.ToString("yyyyMMdd"));
                        searchPattern.Add(bvbgPrefix + dataCMDF.ToString("yyyyMMdd"));
                        searchPattern.Add(bvbgPrefix + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));
                    }
                }

                searchPattern.Add("CLF_" + DateTime.Now.ToString("yyyyMMdd"));

                //ATP: 2016-08-31
                searchPattern.Add("CTLP" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("CTLP" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("CTLP" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                searchPattern.Add("CTRP" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("CTLP" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("CTRP" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                searchPattern.Add("DBTC" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("DBTC" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("DBTC" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                searchPattern.Add("DBTP" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("DBTP" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("DBTP" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                searchPattern.Add("DBTL" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("DBTL" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("DBTL" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                searchPattern.Add("DGAR" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("DGAR" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("DGAR" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                searchPattern.Add("TGAR" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("TGAR" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("TGAR" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                searchPattern.Add("POSR" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("POSR" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("POSR" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                searchPattern.Add("RELATORIO-OPERACOES_" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("RELATORIO-OPERACOES_" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("RELATORIO-OPERACOES_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                searchPattern.Add("PS_RT_G015_0199_120_" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("PS_RT_G015_0199_120_" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("PS_RT_G015_0199_120_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                searchPattern.Add("PSTAC0200299_NGA" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("PSTAC0200299_NGA" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("PSTAC0200299_NGA" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));
                
                searchPattern.Add("PSTAC0200199_NGA" + DateTime.Now.ToString("yyyyMMdd"));
                searchPattern.Add("PSTAC0200199_NGA" + dataCMDF.ToString("yyyyMMdd"));
                searchPattern.Add("PSTAC0200199_NGA" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd"));

                searchPattern.Add("00120.mdb");

                foreach (string pattern in searchPattern)
                {
                    logger.Info("Buscando por [" + pattern + "] ao fazer download");
                }


                if (searchPattern.Count > 0)
                {
                    ftpClient.TransferirArquivos(sftpRemoteDir, sftpLocalDir, searchPattern.ToArray());
                }
                else
                {
                    logger.Warn("Nao ha mais arquivos para baixar hoje, todos ja processados");
                }

                bBuscarFTP = false;
            }
            catch (Exception ex)
            {
                logger.Error("BuscarFTP: " + ex.Message, ex);
                bBuscarFTP = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void ProcessaCLCO()
        {
            try
            {
                if (IsFeriado(DateTime.Now))
                {
                    logger.Info("Uhuhu Feriadao, dia de curticao.....");
                    return;
                }

                if (bCLCO)
                {
                    logger.Warn("Outra operacao de processamento de CLCO esta em execucao, aguardando");
                    return;
                }

                bCLCO = true;

                logger.Info("Processando arquivo CLCO...");

                lock (dcControle)
                {
                    if (!dcControle.ContainsKey(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        DBControleMTA.InserirControleDownloadMTA();
                        dcControle.Add(DateTime.Now.ToString("yyyyMMdd"), new DownloadMTAInfo());
                    }
                }

                if (dcControle[DateTime.Now.ToString("yyyyMMdd")].HasCLCO)
                {
                    logger.Warn("Ja foi processado CLCO hoje: [" + dcControle[DateTime.Now.ToString("yyyyMMdd")].PathCLCO + "]");
                    bCLCO = false;
                    return;
                }

                string dirBkp = pathMTABackup + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                if (!Directory.Exists(dirBkp))
                    Directory.CreateDirectory(dirBkp);

                string searchPattern = String.Format("CLCO{0}*", DateTime.Now.ToString("yyyyMMdd"));

                logger.InfoFormat("Buscando por arquivos [{0}] em [{1}]", searchPattern, pathMTAGradual);

                List<string> filesGRD = Directory.GetFiles(pathMTAGradual, searchPattern).ToList();
                filesGRD.Sort();

                logger.Debug("Encontrou " + filesGRD.Count + " arquivos");

                logger.InfoFormat("Buscando por arquivos [{0}] em [{1}]", searchPattern, sftpLocalDir);

                List<string> filesBRP = Directory.GetFiles(sftpLocalDir, searchPattern).ToList();
                filesBRP.Sort();

                logger.Debug("Encontrou " + filesBRP.Count + " arquivos");

                // Achou os 2 arquivos para merge
                List<string> linesMerged = new List<string>();

                string header = "";
                string footer = "";

                if (filesBRP.Count > 0  && filesGRD.Count > 0 )
                {
                    FileInfo infoBRP = new FileInfo(filesBRP[0]);
                    FileInfo infoGRD = new FileInfo(filesGRD[0]);

                    logger.Info("Merging " + infoGRD.Name + " com " + infoBRP.Name);

                    string bkpBRP = String.Format(@"{0}\{1}.{2}.120", dirBkp, infoBRP.Name, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    string bkpGRD = String.Format(@"{0}\{1}.{2}.227", dirBkp, infoGRD.Name, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                    File.Copy(infoBRP.FullName, bkpBRP, true);
                    File.Copy(infoGRD.FullName, bkpGRD, true);

                    string pathCLCO = String.Format(@"{0}\{1}", pathMTAMerged, infoGRD.Name);

                    string[] allLinesBRP = File.ReadAllLines(filesBRP[0]);
                    string[] allLinesGRD = File.ReadAllLines(filesGRD[0]);

                    header = allLinesGRD[0];
                    footer = "99" + header.TrimEnd().Substring(2);

                    StringBuilder content = new StringBuilder();

                    //
                    content.AppendLine(header);
                    for (int i = 1; i < allLinesGRD.Length-1; i++)
                    {
                        content.AppendLine(allLinesGRD[i]);
                    }

                    // 
                    for (int i = 1; i < allLinesBRP.Length-1; i++)
                    {
                        content.AppendLine(allLinesBRP[i]);
                    }

                    footer += String.Format("{0:000000000}", (allLinesGRD.Length + allLinesBRP.Length - 2));

                    content.AppendLine(footer.PadRight(450));
                    File.WriteAllText(pathCLCO, content.ToString(), Encoding.ASCII);

                    logger.Info("Finalizou merging " + infoGRD.Name + " com " + infoBRP.Name);

                    dcControle[DateTime.Now.ToString("yyyyMMdd")].HasCLCO = true;
                    dcControle[DateTime.Now.ToString("yyyyMMdd")].PathCLCO = pathCLCO;
                    DBControleMTA.AtualizarControleDownloadMTA(DateTime.Now, dcControle[DateTime.Now.ToString("yyyyMMdd")]);
                }

                bCLCO = false;
            }
            catch (Exception ex)
            {
                logger.Error("ProcessaCLCO: " + ex.Message, ex);
                bCLCO = false;
            }
        }


        public void ProcessaCMDF()
        {
            try
            {
                if (IsFeriado(DateTime.Now))
                {
                    logger.Info("Uhuhu Feriadao, dia de curticao.....");
                    return;
                }

                if (bCMDF)
                {
                    logger.Warn("Outra operacao de processamento de CMDF esta em execucao, aguardando");
                    return;
                }

                bCMDF = true;

                logger.Info("Processando arquivo CMDF...");

                lock (dcControle)
                {
                    if (!dcControle.ContainsKey(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        DBControleMTA.InserirControleDownloadMTA();
                        dcControle.Add(DateTime.Now.ToString("yyyyMMdd"), new DownloadMTAInfo());
                    }
                }

                if (dcControle[DateTime.Now.ToString("yyyyMMdd")].HasCMDF)
                {
                    logger.Warn("Ja foi processado CMDF hoje: [" + dcControle[DateTime.Now.ToString("yyyyMMdd")].PathCMDF + "]");
                    bCMDF = false;
                    return;
                }

                string dirBkp = pathMTABackup + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                if ( !Directory.Exists(dirBkp))
                    Directory.CreateDirectory(dirBkp);


                DateTime ultimoDiaUtil = UltimoDiaUtil(DateTime.Now);
                List<string> searchPatternLst = new List<string>();
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    string searchPattern = String.Format("CMDF{0}*", DateTime.Now.AddDays(-2).ToString("yyyyMMdd"));

                    logger.Debug("Acrescentando [" + searchPattern + "] na busca por arquivos");

                    searchPatternLst.Add(searchPattern);
                }

                for (int i = 0; i < 5; i++)
                {
                    string searchPattern = String.Format("CMDF{0}*", ultimoDiaUtil.AddDays(i * -1).ToString("yyyyMMdd"));

                    logger.Debug("Acrescentando [" + searchPattern + "] na busca por arquivos");

                    searchPatternLst.Add(searchPattern);
                }

                // Procura os arquivos e indexa do mais novo pro mais antigo
                // Vai usar sempre o primeiro da lista, o mais novo
                List<string> filesGRD = new List<string>();
                foreach (string searchPattern in searchPatternLst)
                {
                    filesGRD.AddRange(Directory.GetFiles(pathMTAGradual, searchPattern).ToList());
                }
                //filesGRD.Sort();
                //filesGRD.Sort((a, b) => a.CompareTo(b)); // ascending sort
                filesGRD.Sort((a, b) => -1 * a.CompareTo(b)); // descending sort

                logger.Debug("Encontrou " + filesGRD.Count + " arquivos GRD");

                logger.InfoFormat("Buscando por arquivos [{0}] em [{1}]", searchPatternLst, sftpLocalDir);


                // Procura os arquivos e indexa do mais novo pro mais antigo
                // Vai usar sempre o primeiro da lista, o mais novo
                List<string> filesBRP = new List<string>();
                foreach (string searchPattern in searchPatternLst)
                {
                    filesBRP.AddRange(Directory.GetFiles(sftpLocalDir, searchPattern).ToList());
                }
                //filesBRP.Sort();
                //filesBRP.Sort((a, b) => a.CompareTo(b)); // ascending sort
                filesBRP.Sort((a, b) => -1 * a.CompareTo(b)); // descending sort

                logger.Debug("Encontrou " + filesBRP.Count + " arquivos BRP");

                // Achou os 2 arquivos para merge

                List<string> linesMerged = new List<string>();

                string header = "";
                string footer = "";

                if (filesBRP.Count > 0  && filesGRD.Count > 0 )
                {
                    // Pega o primeiro da lista, o mais novo
                    FileInfo infoBRP = new FileInfo(filesBRP[0]);
                    FileInfo infoGRD = new FileInfo(filesGRD[0]);

                    logger.Info("Merging " + infoGRD.Name + " com " + infoBRP.Name);

                    string bkpBRP = String.Format(@"{0}\{1}.{2}.120", dirBkp, infoBRP.Name, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    string bkpGRD = String.Format(@"{0}\{1}.{2}.227", dirBkp, infoGRD.Name, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                    File.Copy(infoBRP.FullName, bkpBRP, true);
                    File.Copy(infoGRD.FullName, bkpGRD, true);

                    string pathCMDF = String.Format(@"{0}\{1}", pathMTAMerged, infoGRD.Name);

                    string[] allLinesBRP = File.ReadAllLines(infoBRP.FullName);
                    string[] allLinesGRD = File.ReadAllLines(infoGRD.FullName);

                    header = allLinesGRD[0];
                    footer = "99" + header.TrimEnd().Substring(2);

                    StringBuilder content = new StringBuilder();

                    //
                    content.AppendLine(header);
                    for (int i = 1; i < allLinesGRD.Length - 1; i++)
                    {
                        content.AppendLine(allLinesGRD[i]);
                    }

                    // 
                    for (int i = 1; i < allLinesBRP.Length - 1; i++)
                    {
                        content.AppendLine(allLinesBRP[i]);
                    }

                    footer += String.Format("{0:000000000}", (allLinesGRD.Length + allLinesBRP.Length - 2));

                    content.AppendLine(footer.PadRight(450));
                    File.WriteAllText(pathCMDF, content.ToString(), Encoding.ASCII);

                    logger.Info("Finalizou merging " + infoGRD.Name + " com " + infoBRP.Name);

                    dcControle[DateTime.Now.ToString("yyyyMMdd")].HasCMDF = true;
                    dcControle[DateTime.Now.ToString("yyyyMMdd")].PathCMDF = pathCMDF;
                    DBControleMTA.AtualizarControleDownloadMTA(DateTime.Now, dcControle[DateTime.Now.ToString("yyyyMMdd")]);
                }

                bCMDF = false;
            }
            catch (Exception ex)
            {
                logger.Error("ProcessaCMDF: " + ex.Message, ex);
                bCMDF = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void ProcessaCSGD()
        {
            try
            {
                if (IsFeriado(DateTime.Now))
                {
                    logger.Info("Uhuhu Feriadao, dia de curticao.....");
                    return;
                }

                if (bCSGD)
                {
                    logger.Warn("Outra operacao de processamento de CSGD esta em execucao, aguardando");
                    return;
                }

                bCSGD = true;

                logger.Info("Processando arquivo CSGD...");

                lock (dcControle)
                {
                    if (!dcControle.ContainsKey(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        DBControleMTA.InserirControleDownloadMTA();
                        dcControle.Add(DateTime.Now.ToString("yyyyMMdd"), new DownloadMTAInfo());
                    }
                }

                if (dcControle[DateTime.Now.ToString("yyyyMMdd")].HasCSGD)
                {
                    logger.Warn("Ja foi processado CSGD hoje: [" + dcControle[DateTime.Now.ToString("yyyyMMdd")].PathCSGD + "]");
                    bCSGD = false;
                    return;
                }

                string dirBkp = pathMTABackup + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                if (!Directory.Exists(dirBkp))
                    Directory.CreateDirectory(dirBkp);

                DateTime ultimoDiaUtil = UltimoDiaUtil(DateTime.Now.AddDays(-1));
                //string searchPattern = String.Format("CSGD{0}*", ultimoDiaUtil.ToString("yyyyMMdd"));
                List<string> searchPatternLst = new List<string>();
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    string searchPattern = String.Format("CSGD{0}*", DateTime.Now.AddDays(-2).ToString("yyyyMMdd"));

                    logger.Debug("Acrescentando [" + searchPattern + "] na busca por arquivos");

                    searchPatternLst.Add(searchPattern);
                }


                // Busca arquivos de hoje ateh 5 dias passados
                for (int i = 0; i < 5; i++)
                {
                    string searchPattern = String.Format("CSGD{0}*", DateTime.Now.AddDays(i * -1).ToString("yyyyMMdd"));
                    
                    logger.Debug("Acrescentando [" + searchPattern + "] na busca por arquivos");

                    searchPatternLst.Add(searchPattern);
                }

                List<string> filesGRD = new List<string>();
                foreach( string searchPattern in searchPatternLst )
                {
                    filesGRD.AddRange(Directory.GetFiles(pathMTAGradual, searchPattern).ToList());
                }
                filesGRD.Sort();

                logger.Debug("Encontrou " + filesGRD.Count + " arquivos GRD");

                logger.InfoFormat("Buscando por arquivos [{0}] em [{1}]", searchPatternLst, sftpLocalDir);

                List<string> filesBRP = new List<string>();
                foreach( string searchPattern in searchPatternLst )
                {
                    filesBRP.AddRange(Directory.GetFiles(sftpLocalDir, searchPattern).ToList());
                }
                filesBRP.Sort();

                logger.Debug("Encontrou " + filesBRP.Count + " arquivos BRP");

                List<string> linesMerged = new List<string>();

                string header = "";
                string footer = "";

                if (filesBRP.Count > 0  && filesGRD.Count > 0 )
                {
                    int j=0;
                    FileInfo infoBRP = null;
                    // Busca na 1a linha do arquivo o adequado para processamento (ultimo dia util)
                    while (j < filesBRP.Count)
                    {
                        infoBRP = new FileInfo(filesBRP[j]);
                        if ( CSGDOkForMerge(infoBRP, ultimoDiaUtil) )
                            break;
                        j++;
                    }

                    if (j == filesBRP.Count)
                    {
                        logger.Error("Nao ha arquivo CSGD da BRP adequado para merging (data no arquivo incompativel)");
                        bCSGD = false;
                        return;
                    }
                    
                    j = 0;
                    FileInfo infoGRD = null;
                    // Busca na 1a linha do arquivo o adequado para processamento (ultimo dia util)
                    while ( j < filesGRD.Count)
                    {
                        infoGRD = new FileInfo(filesGRD[j]);
                        if (CSGDOkForMerge(infoGRD, ultimoDiaUtil))
                            break;
                        j++;
                    }

                    if (j == filesGRD.Count)
                    {
                        logger.Error("Nao ha arquivo CSGD da GRD adequado para merging (data no arquivo incompativel)");
                        bCSGD = false;
                        return;
                    }

                    string bkpBRP = String.Format(@"{0}\{1}.{2}.120", dirBkp, infoBRP.Name, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    string bkpGRD = String.Format(@"{0}\{1}.{2}.227", dirBkp, infoGRD.Name, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                    File.Copy(infoBRP.FullName, bkpBRP, true);
                    File.Copy(infoGRD.FullName, bkpGRD, true);

                    logger.Info("Merging " + infoGRD.Name + " com " + infoBRP.Name);

                    string pathCSGD = String.Format(@"{0}\{1}", pathMTAMerged, infoGRD.Name);

                    string[] allLinesBRP = File.ReadAllLines(infoBRP.FullName);
                    string[] allLinesGRD = File.ReadAllLines(infoGRD.FullName);

                    header = allLinesGRD[0];
                    footer = "99" + header.TrimEnd().Substring(2, 37);

                    StringBuilder content = new StringBuilder();

                    //
                    content.AppendLine( header );
                    for (int i = 1; i < allLinesGRD.Length - 1; i++)
                    {
                        content.AppendLine(allLinesGRD[i]);
                    }

                    // 
                    for (int i = 1; i < allLinesBRP.Length - 1; i++)
                    {
                        content.AppendLine(allLinesBRP[i]);
                    }

                    footer += String.Format("{0:000000000}", (allLinesGRD.Length + allLinesBRP.Length - 2));
                    footer += ultimoDiaUtil.ToString("yyyyMMdd");

                    content.AppendLine(footer.PadRight(200));

                    File.WriteAllText(pathCSGD, content.ToString(), Encoding.ASCII);

                    logger.Info("Finalizou merging " + infoGRD.Name + " com " + infoBRP.Name);

                    dcControle[DateTime.Now.ToString("yyyyMMdd")].HasCSGD = true;
                    dcControle[DateTime.Now.ToString("yyyyMMdd")].PathCSGD = pathCSGD;
                    DBControleMTA.AtualizarControleDownloadMTA(DateTime.Now, dcControle[DateTime.Now.ToString("yyyyMMdd")]);

                    // Gerar planilha Excel para batimento das garantias
                    GerarRelatorioCSGD(pathCSGD);
                }

                // Enviar email

                bCSGD = false;
            }
            catch (Exception ex)
            {
                logger.Error("ProcessaCSGD: " + ex.Message, ex);
                bCSGD = false;
            }
        }

        public  void GerarRelatorioCSGD(string pathCSGD)
        {
            try
            {
                DataTable tbCsgd01 = new DataTable("CSGD01");

                tbCsgd01.Columns.Add("CodigoAgenteCustodia", typeof(System.String));
                tbCsgd01.Columns.Add("DVCodigoAgenteCustodia", typeof(System.String));
                tbCsgd01.Columns.Add("CodigoInvestidor", typeof(System.String));
                tbCsgd01.Columns.Add("DVCodigoInvestidor", typeof(System.String));
                tbCsgd01.Columns.Add("CPFCNPJInvestidor", typeof(System.String));
                tbCsgd01.Columns.Add("DataNascFundacao", typeof(System.String));
                tbCsgd01.Columns.Add("CodigoDependencia", typeof(System.String));
                tbCsgd01.Columns.Add("DescricaoDependencia", typeof(System.String));
                tbCsgd01.Columns.Add("NomeInvestidor", typeof(System.String));
                tbCsgd01.Columns.Add("NomeAdministrador", typeof(System.String));
                tbCsgd01.Columns.Add("CodAgenteCustodiaFinal", typeof(System.String));
                tbCsgd01.Columns.Add("CodigoInvestidorFinal", typeof(System.String));

                tbCsgd01.AcceptChanges();

                DataTable tbCsgd02 = new DataTable("CSGD02");

                tbCsgd02.Columns.Add("CodAgenteCustodia", typeof(System.String));
                tbCsgd02.Columns.Add("CodigoInvestidor", typeof(System.String));
                tbCsgd02.Columns.Add("CodigoCarteira", typeof(System.String));
                tbCsgd02.Columns.Add("DescricaoCarteira", typeof(System.String));
                tbCsgd02.Columns.Add("CodigoISIN", typeof(System.String));
                tbCsgd02.Columns.Add("Distribuicao", typeof(System.String));
                tbCsgd02.Columns.Add("NomeEmissor", typeof(System.String));
                tbCsgd02.Columns.Add("Especificacao", typeof(System.String));
                tbCsgd02.Columns.Add("QtdeTotalAtivosCustodia", typeof(System.String));
                tbCsgd02.Columns.Add("QtdeTotalAtivosBloqueados", typeof(System.String));
                tbCsgd02.Columns.Add("CodigoNegociacao", typeof(System.String));
                tbCsgd02.Columns.Add("IndicadorSaldoAnalitico", typeof(System.String));
                tbCsgd02.Columns.Add("TipoAtivo", typeof(System.String));

                tbCsgd02.AcceptChanges();


                DataTable tbCsgd03 = new DataTable("CSGD03");

                tbCsgd03.Columns.Add("CodAgenteCustodia", typeof(System.String));
                tbCsgd03.Columns.Add("CodigoInvestidor", typeof(System.String));
                tbCsgd03.Columns.Add("CodigoCarteira", typeof(System.String));
                tbCsgd03.Columns.Add("CodigoISIN", typeof(System.String));
                tbCsgd03.Columns.Add("Distribuicao", typeof(System.String));
                tbCsgd03.Columns.Add("TipoBloqueio", typeof(System.String));
                tbCsgd03.Columns.Add("DescricaoBloqueio", typeof(System.String));
                tbCsgd03.Columns.Add("DataPregaoCompra", typeof(System.String));
                tbCsgd03.Columns.Add("DataInicioBloqueio", typeof(System.String));
                tbCsgd03.Columns.Add("DataFimBloqueio", typeof(System.String));
                tbCsgd03.Columns.Add("QtdeAtivosBloqueados", typeof(System.String));
                tbCsgd03.Columns.Add("CodAgenteCompensacao", typeof(System.String));
                tbCsgd03.Columns.Add("DataAquisicao", typeof(System.String));
                tbCsgd03.Columns.Add("PrecoAquisicao", typeof(System.String));

                tbCsgd03.AcceptChanges();

                DataTable tbCsgd04 = new DataTable("CSGD04");

                tbCsgd04.Columns.Add("CodAgenteCustodia", typeof(System.String));
                tbCsgd04.Columns.Add("CodInvestidor", typeof(System.String));
                tbCsgd04.Columns.Add("CodCarteira", typeof(System.String));
                tbCsgd04.Columns.Add("DescricaoCarteira", typeof(System.String));
                tbCsgd04.Columns.Add("CodigoISIN", typeof(System.String));
                tbCsgd04.Columns.Add("Distribuicao", typeof(System.String));
                tbCsgd04.Columns.Add("NomeEmissor", typeof(System.String));
                tbCsgd04.Columns.Add("Especificacao", typeof(System.String));
                tbCsgd04.Columns.Add("QtdeAtivosCustodia", typeof(System.String));
                tbCsgd04.Columns.Add("QtdeAtivosBloqueados", typeof(System.String));
                tbCsgd04.Columns.Add("CodigoNegociacao", typeof(System.String));
                tbCsgd04.Columns.Add("DataAquisicao", typeof(System.String));
                tbCsgd04.Columns.Add("PrecoAquisicao", typeof(System.String));

                tbCsgd04.AcceptChanges();

                //try
                //{
                logger.Info("Inicio processamento arquivo CSGD");

                string[] lines = File.ReadAllLines(pathCSGD);


                foreach (string line in lines)
                {
                    string tipo = line.Substring(0, 2);

                    if (tipo.Equals("01"))
                    {
                        DataRow row = tbCsgd01.NewRow();

                        CSGD_01_ID_INVESTIDOR strut = Utilities.MarshalFromStringBlock<CSGD_01_ID_INVESTIDOR>(line);

                        row["CodigoAgenteCustodia"] = Convert.ToInt32(strut.CodigoAgenteCustodia.ByteArrayToString()).ToString();
                        row["CodigoInvestidor"] = Convert.ToInt32(strut.CodigoInvestidor.ByteArrayToString()).ToString();
                        row["CPFCNPJInvestidor"] = strut.CPFCNPJInvestidor.ByteArrayToString();
                        row["DataNascFundacao"] = strut.DataNascFundacao.ByteArrayToString();
                        row["CodigoDependencia"] = strut.CodigoDependencia.ByteArrayToString();
                        row["DescricaoDependencia"] = strut.DescricaoDependencia.ByteArrayToString();
                        row["NomeInvestidor"] = strut.NomeInvestidor.ByteArrayToString();
                        row["NomeAdministrador"] = strut.NomeAdministrador.ByteArrayToString();
                        row["CodAgenteCustodiaFinal"] = strut.CodAgenteCustodiaFinal.ByteArrayToString();
                        row["CodigoInvestidorFinal"] = strut.CodigoInvestidorFinal.ByteArrayToString();

                        tbCsgd01.Rows.Add(row);
                    }


                    if (tipo.Equals("02"))
                    {
                        DataRow row = tbCsgd02.NewRow();

                        CSGD_02_ID_SALDO strut = Utilities.MarshalFromStringBlock<CSGD_02_ID_SALDO>(line);

                        row["CodAgenteCustodia"] = Convert.ToInt32(strut.CodAgenteCustodia.ByteArrayToString()).ToString();
                        row["CodigoInvestidor"] = Convert.ToInt32(strut.CodigoInvestidor.ByteArrayToString()).ToString();
                        row["CodigoCarteira"] =strut.CodigoCarteira.ByteArrayToString();
                        row["DescricaoCarteira"] = strut.DescricaoCarteira.ByteArrayToString();
                        row["CodigoISIN"] = strut.CodigoISIN.ByteArrayToString();
                        row["Distribuicao"] = strut.Distribuicao.ByteArrayToString();
                        row["NomeEmissor"] = strut.NomeEmissor.ByteArrayToString();
                        row["Especificacao"] = strut.Especificacao.ByteArrayToString();
                        row["QtdeTotalAtivosCustodia"] = strut.QtdeTotalAtivosCustodia.ByteArrayToDecimal(3).ToString();
                        row["QtdeTotalAtivosBloqueados"] = strut.QtdeTotalAtivosBloqueados.ByteArrayToDecimal(0).ToString();
                        row["CodigoNegociacao"] = strut.CodigoNegociacao.ByteArrayToString();
                        row["IndicadorSaldoAnalitico"] = strut.IndicadorSaldoAnalitico.ByteArrayToString();
                        row["TipoAtivo"] = strut.TipoAtivo.ByteArrayToString();

                        tbCsgd02.Rows.Add(row);
                    }

                    if (tipo.Equals("03"))
                    {
                        DataRow row = tbCsgd03.NewRow();

                        CSGD_03_ID_SALDOS_BLOQUEADOS strut = Utilities.MarshalFromStringBlock<CSGD_03_ID_SALDOS_BLOQUEADOS>(line);

                        row["CodigoAgenteCustodia"] = Convert.ToInt32(strut.CodAgenteCustodia.ByteArrayToString()).ToString();
                        row["CodigoInvestidor"] = Convert.ToInt32(strut.CodigoInvestidor.ByteArrayToString()).ToString();
                        row["CodigoCarteira"] = strut.CodigoCarteira.ByteArrayToString();
                        row["CodigoISIN"] = strut.CodigoISIN.ByteArrayToString();
                        row["Distribuicao"] = strut.Distribuicao.ByteArrayToString();
                        row["TipoBloqueio"] = strut.TipoBloqueio.ByteArrayToString();
                        row["DescricaoBloqueio"] = strut.DescricaoBloqueio.ByteArrayToString();
                        row["DataPregaoCompra"] = strut.DataPregaoCompra.ByteArrayToString();
                        row["DataInicioBloqueio"] = strut.DataInicioBloqueio.ByteArrayToString();
                        row["DataFimBloqueio"] = strut.DataFimBloqueio.ByteArrayToString();
                        row["QtdeAtivosBloqueados"] = strut.QtdeAtivosBloqueados.ByteArrayToDecimal(0).ToString();
                        row["CodAgenteCompensacao"] = strut.CodAgenteCompensacao.ByteArrayToString();
                        row["DataAquisicao"] = strut.DataAquisicao.ByteArrayToString();
                        row["PrecoAquisicao"] = strut.PrecoAquisicao.ByteArrayToDecimal(6).ToString();

                        tbCsgd03.Rows.Add(row);
                    }

                    if (tipo.Equals("04"))
                    {
                        DataRow row = tbCsgd04.NewRow();

                        CSGD_04_ID_SALDO_ANALITICO strut = Utilities.MarshalFromStringBlock<CSGD_04_ID_SALDO_ANALITICO>(line);

                        row["CodAgenteCustodia"] = Convert.ToInt32(strut.CodAgenteCustodia.ByteArrayToString()).ToString();
                        row["CodInvestidor"] = Convert.ToInt32(strut.CodInvestidor.ByteArrayToString()).ToString();
                        row["CodCarteira"] = strut.CodCarteira.ByteArrayToString();
                        row["DescricaoCarteira"] = strut.CodigoISIN.ByteArrayToString();
                        row["CodigoISIN"] = strut.Distribuicao.ByteArrayToString();
                        row["Distribuicao"] = strut.Distribuicao.ByteArrayToString();
                        row["NomeEmissor"] = strut.NomeEmissor.ByteArrayToString();
                        row["Especificacao"] = strut.Especificacao.ByteArrayToString();
                        row["QtdeAtivosCustodia"] = strut.QtdeAtivosCustodia.ByteArrayToDecimal(3).ToString();
                        row["QtdeAtivosBloqueados"] = strut.QtdeAtivosBloqueados.ByteArrayToDecimal(0);
                        row["CodigoNegociacao"] = strut.CodigoNegociacao.ByteArrayToString();
                        row["DataAquisicao"] = strut.DataAquisicao.ByteArrayToString();
                        row["PrecoAquisicao"] = strut.PrecoAquisicao.ByteArrayToDecimal(6).ToString();

                        tbCsgd04.Rows.Add(row);
                    }
                }

                DataSet ds1 = new DataSet("DS1");
                ds1.Tables.Add(tbCsgd01);
                ds1.Tables.Add(tbCsgd02);
                ds1.Tables.Add(tbCsgd03);
                ds1.Tables.Add(tbCsgd04);
                ds1.AcceptChanges();


                IEnumerable<DataRow> investidoresQuery =
                    from investidores in tbCsgd01.AsEnumerable()
                    select investidores;

                StringBuilder body = new StringBuilder();
                StringBuilder bodyCSV = new StringBuilder();

                body.AppendLine("RELATORIO DE BATIMENTO CSGD".CentralizeText(80) + "\r\n" );
                body.AppendLine(("GERADO AS " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")).CentralizeText(80) + "\r\n");

                bodyCSV.AppendLine( "'Nome';'Codigo';'Carteira';'DescricaoCarteira';'Ativo';'QtdeTotalAtivosCustodia';'QtdeTotalAtivosBloqueados'".Replace('\'','"') );

                bool suspDuplicacao = false;

                foreach (DataRow investidor in investidoresQuery)
                {
                    logger.InfoFormat("Gerando relatorio do cliente {0} - {1} - {2}",
                        investidor.Field<string>("NomeInvestidor").TrimEnd(),
                        investidor.Field<string>("CodigoInvestidor"),
                        investidor.Field<string>("CPFCNPJInvestidor"));

                    body.AppendFormat("\r\nCliente: {0} - {1} - {2}\r\n",
                        investidor.Field<string>("NomeInvestidor").TrimEnd(),
                        investidor.Field<string>("CodigoInvestidor"),
                        investidor.Field<string>("CPFCNPJInvestidor"));

                    IEnumerable<DataRow> carteiraQuery =
                        from carteiras in tbCsgd02.AsEnumerable()
                        where carteiras.Field<string>("CodigoInvestidor") == investidor.Field<string>("CodigoInvestidor")
                        orderby carteiras.Field<string>("CodigoNegociacao") ascending
                        select carteiras;

                    string idCarteira = "";
                    string idAtivo = "";

                    foreach (DataRow carteira in carteiraQuery)
                    {
                        body.AppendFormat("{0}   {1}   ATIVO: {2} Total em custodia: {3} Total Bloqueado: {4}",
                            carteira.Field<string>("CodigoCarteira"),
                            carteira.Field<string>("DescricaoCarteira"),
                            carteira.Field<string>("CodigoNegociacao"),
                            Convert.ToDecimal(carteira.Field<string>("QtdeTotalAtivosCustodia")).ToString("F0", CultureInfo.InvariantCulture).PadLeft(12),
                            Convert.ToDecimal(carteira.Field<string>("QtdeTotalAtivosBloqueados")).ToString("F0", CultureInfo.InvariantCulture).PadLeft(12));

                        if (idCarteira.Equals(carteira.Field<string>("CodigoCarteira")) &&
                            idAtivo.Equals(carteira.Field<string>("CodigoNegociacao")))
                        {
                            body.AppendLine("  <=== PROVAVEL DUPLICACAO");

                            logger.WarnFormat(" Provavel Duplicacao para cliente {0} Carteira: {1}-{2} ATIVO: {3} Total em custodia: {4}",
                                carteira.Field<string>("CodigoInvestidor"),
                                carteira.Field<string>("CodigoCarteira"),
                                carteira.Field<string>("DescricaoCarteira"),
                                carteira.Field<string>("CodigoNegociacao"),
                                Convert.ToDecimal(carteira.Field<string>("QtdeTotalAtivosCustodia")).ToString("F0", CultureInfo.InvariantCulture).PadLeft(12));

                            suspDuplicacao = true;
                        }
                        else
                            body.Append("\r\n");

                        idCarteira = carteira.Field<string>("CodigoCarteira");
                        idAtivo = carteira.Field<string>("CodigoNegociacao");

                        //bodyCSV.AppendLine("'Nome','Codigo','Carteira','DescricaoCarteira','Ativo','QtdeTotalAtivosCustodia','QtdeTotalAtivosBloqueados'".Replace('\'', '"'));
                        string linhaCSV = String.Format("{0};{1};{2};{3};{4};{5};{6}",
                                    investidor.Field<string>("NomeInvestidor").TrimEnd(),
                                    investidor.Field<string>("CodigoInvestidor"),
                                    carteira.Field<string>("CodigoCarteira"),
                                    carteira.Field<string>("DescricaoCarteira"),
                                    carteira.Field<string>("CodigoNegociacao"),
                                    Convert.ToDecimal(carteira.Field<string>("QtdeTotalAtivosCustodia")).ToString("F0", CultureInfo.InvariantCulture).PadLeft(12),
                                    Convert.ToDecimal(carteira.Field<string>("QtdeTotalAtivosBloqueados")).ToString("F0", CultureInfo.InvariantCulture).PadLeft(12));
                        
                        bodyCSV.AppendLine(linhaCSV.Replace('\'', '"'));


                        IEnumerable<DataRow> bloqueadosQuery =
                            from bloqueados in tbCsgd03.AsEnumerable()
                            where ( bloqueados.Field<string>("CodigoInvestidor") == carteira.Field<string>("CodigoInvestidor") &&
                                    bloqueados.Field<string>("CodigoCarteira") == carteira.Field<string>("CodigoCarteira") &&
                                    bloqueados.Field<string>("CodigoISIN") == carteira.Field<string>("CodigoISIN") &&
                                    bloqueados.Field<string>("Distribuicao") == carteira.Field<string>("Distribuicao"))
                            select bloqueados;

                        bool bHeader = true;

                        foreach (DataRow bloqueado in bloqueadosQuery)
                        {
                            if (bHeader)
                            {
                                bHeader = false;
                                body.AppendLine("ATIVOS BLOQUEADOS:");
                            }

                            body.AppendFormat("Tipo: {0}  Desc: {1}  Dt Compra: {2} Inicio: {3} Fim: {4} Aquisicao: {5} Preco: {6} Total Bloqueado: {7}\r\n",
                                bloqueado.Field<string>("TipoBloqueio"),
                                bloqueado.Field<string>("DescricaoBloqueio").TrimEnd(),
                                bloqueado.Field<string>("DataPregaoCompra"),
                                bloqueado.Field<string>("DataInicioBloqueio"),
                                bloqueado.Field<string>("DataFimBloqueio"),
                                Convert.ToDecimal(bloqueado.Field<string>("QtdeAtivosBloqueados")).ToString("F0", CultureInfo.InvariantCulture),
                                bloqueado.Field<string>("DataAquisicao"),
                                Convert.ToDecimal(bloqueado.Field<string>("PrecoAquisicao")).ToString("F2", CultureInfo.InvariantCulture));
                        }

                        IEnumerable<DataRow> analiticosQuery =
                            from analiticos in tbCsgd04.AsEnumerable()
                            where (analiticos.Field<string>("CodInvestidor") == carteira.Field<string>("CodigoInvestidor") &&
                                    analiticos.Field<string>("CodCarteira") == carteira.Field<string>("CodigoCarteira") &&
                                    analiticos.Field<string>("CodigoISIN") == carteira.Field<string>("CodigoISIN") &&
                                    analiticos.Field<string>("Distribuicao") == carteira.Field<string>("Distribuicao"))
                            select analiticos;

                        bHeader = true;

                        foreach (DataRow analitico in analiticosQuery)
                        {
                            if (bHeader)
                            {
                                bHeader = false;
                                body.AppendLine("SALDOS ANALITICOS:");
                            }

                            body.AppendFormat("Emissor: {0}  Desc: {1}  Dt Compra: {2} Inicio: {3} Fim: {4} Aquisicao: {5} Preco: {6} Total Bloqueado: {7}\r\n",
                                analitico.Field<string>("NomeEmissor").TrimEnd(),
                                analitico.Field<string>("Especificacao").TrimEnd(),
                                Convert.ToDecimal(analitico.Field<string>("QtdeAtivosCustodia")).ToString("F0", CultureInfo.InvariantCulture),
                                Convert.ToDecimal(analitico.Field<string>("QtdeAtivosBloqueados")).ToString("F0", CultureInfo.InvariantCulture),
                                analitico.Field<string>("DataAquisicao"),
                                Convert.ToDecimal(analitico.Field<string>("PrecoAquisicao")).ToString("F2", CultureInfo.InvariantCulture));
                        }
                    }
                }

                FileInfo info = new FileInfo(pathCSGD);

                string pathRelatorio = info.DirectoryName + "\\BATIMENTO-" + info.Name + ".TXT";
                string pathCSV = info.DirectoryName + "\\BATIMENTO-" + info.Name + ".CSV";

                File.WriteAllText(pathRelatorio, body.ToString());
                File.WriteAllText(pathCSV, bodyCSV.ToString());

                string emails = ConfigurationManager.AppSettings["EmailsRelatorioCSGD"].ToString();
                
                string subject = "CSGD - RELATORIO DE BATIMENTO " + pathRelatorio + " (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ")";

                string msgBody = "Relatorio gerado com sucesso";

                if (suspDuplicacao)
                    msgBody = "HA SUSPEITA DE REGISTROS DUPLICADOS!!!!!\r\nVERIFIQUE!!!!!!\r\n";
                    

                logger.Info("Relatorio " + pathRelatorio + " gerado, notificando [" + emails + "]");

                List<string> lstArquivos = new List<string>();
                lstArquivos.Add(pathRelatorio);
                lstArquivos.Add(pathCSV);

                _enviaAviso(emails, subject, msgBody, lstArquivos);
            }
            catch (Exception ex)
            {
                logger.Error("GerarRelatorioCSGD: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void ProcessaPFEN()
        {
            try
            {
                if (IsFeriado(DateTime.Now))
                {
                    logger.Info("Uhuhu Feriadao, dia de curticao.....");
                    return;
                }

                if (bPFEN)
                {
                    logger.Warn("Outra operacao de processamento de PFEN esta em execucao, aguardando");
                    return;
                }

                bPFEN = true;

                logger.Info("Processando arquivo PFEN...");

                lock (dcControle)
                {
                    if (!dcControle.ContainsKey(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        DBControleMTA.InserirControleDownloadMTA();
                        dcControle.Add(DateTime.Now.ToString("yyyyMMdd"), new DownloadMTAInfo());
                    }
                }

                if (dcControle[DateTime.Now.ToString("yyyyMMdd")].HasPFEN)
                {
                    logger.Warn("Ja foi processado PFEN hoje: [" + dcControle[DateTime.Now.ToString("yyyyMMdd")].PathPFEN + "]");
                    bPFEN = false;
                    return;
                }

                string dirBkp = pathMTABackup + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                if (!Directory.Exists(dirBkp))
                    Directory.CreateDirectory(dirBkp);

                string searchPattern = String.Format("PFEN{0}*", DateTime.Now.ToString("yyyyMMdd"));

                List<string> filesGRD = Directory.GetFiles(pathMTAGradual, searchPattern).ToList();
                filesGRD.Sort();

                logger.Debug("Encontrou " + filesGRD.Count + " arquivos");

                logger.InfoFormat("Buscando por arquivos [{0}] em [{1}]", searchPattern, sftpLocalDir);

                List<string> filesBRP = Directory.GetFiles(sftpLocalDir, searchPattern).ToList();
                filesBRP.Sort();

                logger.Debug("Encontrou " + filesBRP.Count + " arquivos");

                // Achou os 2 arquivos para merge
                List<string> linesMerged = new List<string>();

                string header = "";
                string footer = "";

                if (filesBRP.Count > 0 && filesGRD.Count > 0)
                {
                    FileInfo infoBRP = new FileInfo(filesBRP[0]);
                    FileInfo infoGRD = new FileInfo(filesGRD[0]);

                    logger.Info("Merging " + infoGRD.Name + " com " + infoBRP.Name);

                    string bkpBRP = String.Format(@"{0}\{1}.{2}.120", dirBkp, infoBRP.Name, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    string bkpGRD = String.Format(@"{0}\{1}.{2}.227", dirBkp, infoGRD.Name, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                    File.Copy(infoBRP.FullName, bkpBRP, true);
                    File.Copy(infoGRD.FullName, bkpGRD, true);

                    string pathPFEN = String.Format(@"{0}\{1}", pathMTAMerged, infoGRD.Name);

                    string[] allLinesBRP = File.ReadAllLines(filesBRP[0]);
                    string[] allLinesGRD = File.ReadAllLines(filesGRD[0]);

                    header = allLinesGRD[0];

                    // O footer do PFEN PENR eh diferente
                    // o numero de linhas eh inserido no meio
                    footer = "99" + header.TrimEnd().Substring(2,28);

                    StringBuilder content = new StringBuilder();
                    //
                    content.AppendLine(header);
                    for (int i = 1; i < allLinesGRD.Length - 1; i++)
                    {
                        content.AppendLine(allLinesGRD[i]);
                    }

                    // 
                    for (int i = 1; i < allLinesBRP.Length - 1; i++)
                    {
                        content.AppendLine(allLinesBRP[i]);
                    }

                    footer += String.Format("{0:000000000}", (allLinesGRD.Length + allLinesBRP.Length - 2));
                    footer += header.TrimEnd().Substring(30);

                    content.AppendLine(footer.PadRight(350));
                    File.WriteAllText(pathPFEN, content.ToString(), Encoding.ASCII);

                    logger.Info("Finalizou merging " + infoGRD.Name + " com " + infoBRP.Name);

                    dcControle[DateTime.Now.ToString("yyyyMMdd")].HasPFEN = true;
                    dcControle[DateTime.Now.ToString("yyyyMMdd")].PathPFEN = pathPFEN;
                    DBControleMTA.AtualizarControleDownloadMTA(DateTime.Now, dcControle[DateTime.Now.ToString("yyyyMMdd")]);
                }

                bPFEN = false;
            }
            catch (Exception ex)
            {
                logger.Error("ProcessaPFEN: " + ex.Message, ex);
                bPFEN = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessaPENR()
        {
            try
            {
                if (IsFeriado(DateTime.Now))
                {
                    logger.Info("Uhuhu Feriadao, dia de curticao.....");
                    return;
                }

                if (bPENR)
                {
                    logger.Warn("Outra operacao de processamento de PENR esta em execucao, aguardando");
                    return;
                }

                bPENR = true;

                logger.Info("Processando arquivo PENR...");

                lock (dcControle)
                {
                    if (!dcControle.ContainsKey(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        DBControleMTA.InserirControleDownloadMTA();
                        dcControle.Add(DateTime.Now.ToString("yyyyMMdd"), new DownloadMTAInfo());
                    }
                }

                if (dcControle[DateTime.Now.ToString("yyyyMMdd")].HasPENR)
                {
                    logger.Warn("Ja foi processado PENR hoje: [" + dcControle[DateTime.Now.ToString("yyyyMMdd")].PathPENR + "]");
                    bPENR = false;
                    return;
                }

                string dirBkp = pathMTABackup + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                if (!Directory.Exists(dirBkp))
                    Directory.CreateDirectory(dirBkp);

                string searchPattern = String.Format("PENR{0}*", DateTime.Now.ToString("yyyyMMdd"));

                List<string> filesGRD = Directory.GetFiles(pathMTAGradual, searchPattern).ToList();
                filesGRD.Sort();

                logger.Debug("Encontrou " + filesGRD.Count + " arquivos");

                logger.InfoFormat("Buscando por arquivos [{0}] em [{1}]", searchPattern, sftpLocalDir);

                List<string> filesBRP = Directory.GetFiles(sftpLocalDir, searchPattern).ToList();
                filesBRP.Sort();

                logger.Debug("Encontrou " + filesBRP.Count + " arquivos");

                // Achou os 2 arquivos para merge
                List<string> linesMerged = new List<string>();

                string header = "";
                string footer = "";

                if (filesBRP.Count > 0 && filesGRD.Count > 0 )
                {
                    FileInfo infoBRP = new FileInfo(filesBRP[0]);
                    FileInfo infoGRD = new FileInfo(filesGRD[0]);

                    logger.Info("Merging " + infoGRD.Name + " com " + infoBRP.Name);

                    string bkpBRP = String.Format(@"{0}\{1}.{2}.120", dirBkp, infoBRP.Name, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    string bkpGRD = String.Format(@"{0}\{1}.{2}.227", dirBkp, infoGRD.Name, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                    File.Copy(infoBRP.FullName, bkpBRP, true);
                    File.Copy(infoGRD.FullName, bkpGRD, true);

                    string pathPENR = String.Format(@"{0}\{1}", pathMTAMerged, infoGRD.Name);

                    string[] allLinesBRP = File.ReadAllLines(filesBRP[0]);
                    string[] allLinesGRD = File.ReadAllLines(filesGRD[0]);

                    header = allLinesGRD[0];

                    // O footer do PFEN PENR eh diferente
                    // o numero de linhas eh inserido no meio
                    footer = "99" + header.TrimEnd().Substring(2, 28);

                    StringBuilder content = new StringBuilder();

                    //
                    content.AppendLine(header);
                    for (int i = 1; i < allLinesGRD.Length - 1; i++)
                    {
                        content.AppendLine(allLinesGRD[i]);
                    }

                    // 
                    for (int i = 1; i < allLinesBRP.Length - 1; i++)
                    {
                        content.AppendLine(allLinesBRP[i]);
                    }

                    footer += String.Format("{0:000000000}", (allLinesGRD.Length + allLinesBRP.Length - 2));
                    footer += header.TrimEnd().Substring(30);

                    content.AppendLine(footer.PadRight(480));
                    File.WriteAllText(pathPENR, content.ToString(), Encoding.ASCII);

                    logger.Info("Finalizou merging " + infoGRD.Name + " com " + infoBRP.Name);

                    dcControle[DateTime.Now.ToString("yyyyMMdd")].HasPENR = true;
                    dcControle[DateTime.Now.ToString("yyyyMMdd")].PathPENR = pathPENR;
                    DBControleMTA.AtualizarControleDownloadMTA(DateTime.Now, dcControle[DateTime.Now.ToString("yyyyMMdd")]);
                }

                bPENR = false;
            }
            catch (Exception ex)
            {
                logger.Error("ProcessaPENR: " + ex.Message, ex);
                bPENR = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void NotificarInteressados()
        {
            try
            {
                logger.Info("Verificando se ha arquivos disponiveis...");
                lock (dcControle)
                {
                    if (dcControle.ContainsKey(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        DownloadMTAInfo info = dcControle[DateTime.Now.ToString("yyyyMMdd")];

                        if (info.HasCMDF && !info.NotificadoCMDF)
                        {
                            string emails = ConfigurationManager.AppSettings["EmailsAvisoCSGDCMDF"].ToString();
                            string subject = "ASSUNTO: ARQUIVOS CMDF disponível (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ")";

                            logger.Info("CMDF disponivel, notificando [" + emails + "]");

                            List<string> lstArquivos = new List<string>();
                            lstArquivos.Add(info.PathCMDF);

                            FileInfo infoCMDF = new FileInfo(info.PathCMDF);

                            _enviaAviso(emails, subject, lstArquivos, "CMDF.zip");

                            string mtaCMDF = String.Format(@"{0}\{1}", pathMTAGradual, infoCMDF.Name);

                            dcControle[DateTime.Now.ToString("yyyyMMdd")].NotificadoCMDF = true;

                            if (File.Exists(mtaCMDF))
                                File.Delete(mtaCMDF);
                            File.Copy(info.PathCMDF, mtaCMDF);

                            DBControleMTA.AtualizarControleDownloadMTA(DateTime.Now, dcControle[DateTime.Now.ToString("yyyyMMdd")]);
                        }


                        if (info.HasCSGD && !info.NotificadoCSGD )
                        {
                            string emails = ConfigurationManager.AppSettings["EmailsAvisoCSGDCMDF"].ToString();
                            string subject = "ASSUNTO: ARQUIVOS CSGD disponível (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ")";

                            logger.Info("CSGD disponivel, notificando [" + emails + "]");

                            List<string> lstArquivos = new List<string>();
                            lstArquivos.Add(info.PathCSGD);

                            FileInfo infoCSGD = new FileInfo(info.PathCSGD);

                            _enviaAviso(emails, subject, lstArquivos, "CSGD.zip");

                            string mtaCSGD = String.Format(@"{0}\{1}", pathMTAGradual, infoCSGD.Name);

                            dcControle[DateTime.Now.ToString("yyyyMMdd")].NotificadoCSGD = true;


                            if (File.Exists(mtaCSGD))
                                File.Delete(mtaCSGD);
                            File.Copy(info.PathCSGD, mtaCSGD);

                            DBControleMTA.AtualizarControleDownloadMTA(DateTime.Now, dcControle[DateTime.Now.ToString("yyyyMMdd")]);
                        }

                        if (info.HasCLCO && info.HasPENR && info.HasPFEN &&
                            !info.NotificadoCLCO && !info.NotificadoPENR && !info.NotificadoPFEN)
                        {
                            string emails = ConfigurationManager.AppSettings["EmailsAvisoCLCO"].ToString();
                            string subject = "ASSUNTO: ARQUIVOS CLCO, PFEN e PENR disponíveis (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ")";

                            logger.Info("CLCO, PENR e PFEN disponiveis, notificando [" + emails + "]");

                            List<string> lstArquivos = new List<string>();
                            lstArquivos.Add(info.PathCLCO);
                            lstArquivos.Add(info.PathPENR);
                            lstArquivos.Add(info.PathPFEN);

                            _enviaAviso(emails, subject, lstArquivos, "CLCO-PFEN-PENR.zip" );

                            dcControle[DateTime.Now.ToString("yyyyMMdd")].NotificadoCLCO = true;
                            dcControle[DateTime.Now.ToString("yyyyMMdd")].NotificadoPFEN = true;
                            dcControle[DateTime.Now.ToString("yyyyMMdd")].NotificadoPENR = true;
                            DBControleMTA.AtualizarControleDownloadMTA(DateTime.Now, dcControle[DateTime.Now.ToString("yyyyMMdd")]);
                        }
                    }
                    else
                    {
                        logger.Info("Nao ha arquivos disponibilizados ate o momento");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("NotificarInteressados: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void NotificarGenericos()
        {
            try
            {
                Thread.Sleep(15000);

                if (bBuscarFTP)
                {
                    logger.Warn("Aguardando finalizar FTP");
                    return;
                }

                logger.Info("Verificando se ha arquivos disponiveis...");

                // Se for uma segunda feira, pegar o arquivo de sabado
                DateTime dataCMDF = DateTime.Now;
                if (dataCMDF.DayOfWeek == DayOfWeek.Monday)
                    dataCMDF = dataCMDF.AddDays(-2);

                while (IsFeriado(dataCMDF))
                    dataCMDF = dataCMDF.AddDays(-1);

                //Procura pelos CLF_
                List<string> searchList = new List<string>();


                searchList.Add("CLF_" + DateTime.Now.ToString("yyyyMMdd") + "*");

                //ATP: 2016-08-31
                searchList.Add("CTLP" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("CTLP" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("CTLP" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                searchList.Add("CTRP" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("CTLP" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("CTRP" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                searchList.Add("DBTC" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("DBTC" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("DBTC" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                searchList.Add("DBTP" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("DBTP" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("DBTP" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                searchList.Add("DBTL" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("DBTL" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("DBTL" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                searchList.Add("DGAR" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("DGAR" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("DGAR" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                searchList.Add("TGAR" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("TGAR" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("TGAR" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                searchList.Add("POSR" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("POSR" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("POSR" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                searchList.Add("RELATORIO-OPERACOES_" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("RELATORIO-OPERACOES_" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("RELATORIO-OPERACOES_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                searchList.Add("PS_RT_G015_0199_120_" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("PS_RT_G015_0199_120_" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("PS_RT_G015_0199_120_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                searchList.Add("PSTAC0200299_NGA" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("PSTAC0200299_NGA" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("PSTAC0200299_NGA" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                searchList.Add("PSTAC0200199_NGA" + DateTime.Now.ToString("yyyyMMdd") + "*");
                searchList.Add("PSTAC0200199_NGA" + dataCMDF.ToString("yyyyMMdd") + "*");
                searchList.Add("PSTAC0200199_NGA" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");

                if (ConfigurationManager.AppSettings["BVBGPrefixList"] != null)
                {
                    string[] bvbgPrefixList = ConfigurationManager.AppSettings["BVBGPrefixList"].ToString().Split(new char[] { ';', ',' });

                    foreach (string bvbgPrefix in bvbgPrefixList)
                    {
                        searchList.Add(bvbgPrefix + DateTime.Now.ToString("yyyyMMdd") + "*");
                        searchList.Add(bvbgPrefix + dataCMDF.ToString("yyyyMMdd") + "*");
                        searchList.Add(bvbgPrefix + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("yyyyMMdd") + "*");
                    }
                }

                List<string> filesMTA = new List<string>();
                foreach (string searchPattern in searchList)
                {
                    logger.InfoFormat("Buscando por arquivos [{0}] em [{1}]", searchPattern, pathMTAGradual);
                    filesMTA.AddRange(Directory.GetFiles(sftpLocalDir, searchPattern).ToList());
                }
                filesMTA.Sort();

                logger.Debug("Encontrou " + filesMTA.Count + " arquivos");

                foreach (string arquivo in filesMTA)
                {
                    logger.InfoFormat("Arquivo baixado do ambiente remoto: [{0}] ", arquivo);

                    if (!lstGenericos.Any(arquivo.Contains))
                    {
                        FileInfo info = new FileInfo(arquivo);

                        string emails = ConfigurationManager.AppSettings["EmailsAvisoGenericos"].ToString();
                        string subject = "ARQUIVO " + info.Name + " disponível (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ")";

                        
                        string mtaPlural = String.Format(@"{0}\{1}", pathMTABrasilPlural, info.Name);

                        string hash1 = MD5HashFile(arquivo);

                        if (File.Exists(mtaPlural))
                        {
                            string hash2 = MD5HashFile(mtaPlural);

                            if (hash1.Equals(hash2))
                            {
                                lstGenericos.Add(arquivo);
                                logger.InfoFormat("Arquivo [{0}] igual ao [{1}], ignorando", arquivo, mtaPlural);
                                continue;
                            }
                        }

                        logger.Info("Arquivo " + arquivo + " disponivel, notificando [" + emails + "]");

                        List<string> lstArquivos = new List<string>();
                        lstArquivos.Add(arquivo);

                        _enviaAviso(emails, subject, lstArquivos, info.Name + ".zip" );

                        if (!File.Exists(mtaPlural))
                        {
                            logger.Info("Copiando [" + info.FullName + "] para [" + mtaPlural + "]");
                            File.Copy(info.FullName, mtaPlural);
                        }

                        lstGenericos.Add(arquivo);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("NotificarGenericos: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Notificar120MDB()
        {
            try
            {
                Thread.Sleep(15);

                if (bBuscarFTP)
                {
                    logger.Warn("Aguardando finalizar FTP");
                    return;
                }

                logger.Info("Verificando se ha arquivos disponiveis...");

                //Procura pelos CLF_
                List<string> searchList = new List<string>();

                searchList.Add("00120.mdb");

                List<string> filesMTA = new List<string>();
                foreach (string searchPattern in searchList)
                {
                    logger.InfoFormat("Buscando por arquivos [{0}] em [{1}]", searchPattern, pathMTAGradual);
                    filesMTA.AddRange(Directory.GetFiles(sftpLocalDir, searchPattern).ToList());
                }
                filesMTA.Sort();

                logger.Debug("Encontrou " + filesMTA.Count + " arquivos");

                foreach (string arquivo in filesMTA)
                {
                    logger.InfoFormat("Arquivo baixado do ambiente remoto: [{0}] ", arquivo);

                    FileInfo info = new FileInfo(arquivo);

                    string emails = ConfigurationManager.AppSettings["EmailsAvisoGenericos"].ToString();
                    string subject = "ARQUIVO " + info.Name + " disponível (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ")";

                    string mtaPlural = String.Format(@"{0}\{1}", pathMTABrasilPlural, info.Name);

                    string hash1 = MD5HashFile(arquivo);

                    if (File.Exists(mtaPlural) )
                    {
                        string hash2 = MD5HashFile(mtaPlural);

                        if (hash1.Equals(hash2))
                        {
                            logger.InfoFormat("Arquivos [{0}] e [{1}] sao iguais, ignorando", arquivo, mtaPlural);
                            return;
                        }
                    }

                    File.Delete(mtaPlural);

                    logger.Info("Arquivo " + arquivo + " disponivel, notificando [" + emails + "]");

                    List<string> lstArquivos = new List<string>();
                    lstArquivos.Add(arquivo);

                    _enviaAviso(emails, subject, lstArquivos, info.Name + ".zip");


                    //if (File.Exists(mtaPlural))
                    //    File.Delete(mtaPlural);

                    if (!File.Exists(mtaPlural))
                    {
                        logger.Info("Copiando [" + info.FullName + "] para [" + mtaPlural + "]");
                        File.Copy(info.FullName, mtaPlural);
                    }

                }

            }
            catch (Exception ex)
            {
                logger.Error("NotificarGenericos: " + ex.Message, ex);
            }
        }

        private bool _enviaAviso(string emailsDest, string subject, List<string> arquivosMerged, string zipName)
        {
            try
            {
                string[] emailsTo;
                string[] emailsCC;
                string[] emailsBCC;


                if (ConfigurationManager.AppSettings["EmailMTARemetente"] == null)
                {
                    logger.Error("AppSetting 'EmailMTARemetente' nao definido. Nao eh possivel enviar alerta");
                    return false;
                }

                char[] seps = { ';', ',' };
                emailsTo = emailsDest.ToString().Split(seps);

                MailMessage lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailMTARemetente"].ToString(), emailsTo[0]);

                for (int i = 1; i < emailsTo.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsTo[i]))
                        lMensagem.To.Add(emailsTo[i]);
                }

                if (ConfigurationManager.AppSettings["EmailMTABCC"] != null)
                {
                    emailsBCC = ConfigurationManager.AppSettings["EmailMTABCC"].ToString().Split(seps);

                    for (int i = 0; i < emailsBCC.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(emailsBCC[i]))
                            lMensagem.Bcc.Add(emailsBCC[i]);
                    }
                }

                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailMTAReplyTo"].ToString()));
                lMensagem.Subject = subject;


                if (ConfigurationManager.AppSettings["EmTeste"] == null ||
                    ConfigurationManager.AppSettings["EmTeste"].ToString().ToLowerInvariant().Equals("true"))
                {
                    string relatorio = "<b>*** ATENÇÃO !!!!!! ****</b>" + Environment.NewLine;
                    relatorio += "SERVICO EM TESTE, verificar os arquivos anexos antes de submeter a processamento em produção" + Environment.NewLine;
                    relatorio += "Não nos responsabilizamos por eventuais erros ou danos decorrentes do descumprimento do aviso acima" + Environment.NewLine;

                    lMensagem.IsBodyHtml = true;
                    lMensagem.Body = "<html><body style=\"font-family:courier;\">" + relatorio.Replace(" ", "&nbsp;").Replace(Environment.NewLine, "<br>" + Environment.NewLine) + "</body></html>";
                }

                //foreach (string arquivoMerged in arquivosMerged)
                //    lMensagem.Attachments.Add(new Attachment(arquivoMerged));

                string zipSaida = Path.GetTempPath() + zipName;

                _zipFiles(zipSaida, arquivosMerged);

                lMensagem.Attachments.Add(new Attachment(zipSaida));

                new SmtpClient(ConfigurationManager.AppSettings["EmailMTAHost"].ToString()).Send(lMensagem);

                // Se nao chamar o Dispose, o arquivo anexado nao pode ser removido do disco
                lMensagem.Dispose();

                File.Delete(zipSaida);

                logger.Info("Email enviado com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("_enviaAviso(): " + ex.Message, ex);
                return false;
            }

            return true;
        }


        private bool _enviaAviso(string emailsDest, string subject, string body, List<string> attachments = null)
        {
            try
            {
                string[] emailsTo;
                string[] emailsCC;
                string[] emailsBCC;


                if (ConfigurationManager.AppSettings["EmailMTARemetente"] == null)
                {
                    logger.Error("AppSetting 'EmailMTARemetente' nao definido. Nao eh possivel enviar alerta");
                    return false;
                }

                char[] seps = { ';', ',' };
                emailsTo = emailsDest.ToString().Split(seps);

                MailMessage lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailMTARemetente"].ToString(), emailsTo[0]);

                for (int i = 1; i < emailsTo.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsTo[i]))
                        lMensagem.To.Add(emailsTo[i]);
                }

                if (ConfigurationManager.AppSettings["EmailMTABCC"] != null)
                {
                    emailsBCC = ConfigurationManager.AppSettings["EmailMTABCC"].ToString().Split(seps);

                    for (int i = 0; i < emailsBCC.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(emailsBCC[i]))
                            lMensagem.Bcc.Add(emailsBCC[i]);
                    }
                }

                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailMTAReplyTo"].ToString()));
                lMensagem.Subject = subject;

                string relatorio = body + Environment.NewLine + Environment.NewLine;

                if (ConfigurationManager.AppSettings["EmTeste"] == null ||
                    ConfigurationManager.AppSettings["EmTeste"].ToString().ToLowerInvariant().Equals("true"))
                {
                    relatorio += "<b>*** ATENÇÃO !!!!!! ****</b>" + Environment.NewLine;
                    relatorio += "SERVICO EM TESTE, verificar os arquivos anexos antes de submeter a processamento em produção" + Environment.NewLine;
                    relatorio += "Não nos responsabilizamos por eventuais erros ou danos decorrentes do descumprimento do aviso acima" + Environment.NewLine;
                }

                lMensagem.IsBodyHtml = true;
                lMensagem.Body = "<html><body style=\"font-family:courier;\">" + relatorio.Replace(" ", "&nbsp;").Replace(Environment.NewLine, "<br>" + Environment.NewLine) + "</body></html>";

                if (attachments != null)
                {
                    foreach (string attachment in attachments)
                    {
                        lMensagem.Attachments.Add(new Attachment(attachment));
                    }
                }

                new SmtpClient(ConfigurationManager.AppSettings["EmailMTAHost"].ToString()).Send(lMensagem);

                // Se nao chamar o Dispose, o arquivo anexado nao pode ser removido do disco
                lMensagem.Dispose();

                logger.Info("Email enviado com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("_enviaAviso(): " + ex.Message, ex);
                return false;
            }

            return true;
        }

        private DateTime UltimoDiaUtil(DateTime dataInicial)
        {
            string feriados = "01/01;25/01;21/04;09/07;07/09;12/10;02/11;15/11;25/12;31/12";

            if (ConfigurationManager.AppSettings["Feriados"] != null)
            {
                feriados = ConfigurationManager.AppSettings["Feriados"].ToString();
            }

            while (dataInicial.DayOfWeek == DayOfWeek.Saturday || dataInicial.DayOfWeek == DayOfWeek.Sunday || feriados.Contains(dataInicial.ToString("dd/MM")))
            {
                dataInicial = dataInicial.AddDays(-1);
            }

            return dataInicial;
        }

        private bool IsFeriado(DateTime dataInicial)
        {
            string feriados = "01/01;21/04;09/07;07/09;12/10;02/11;15/11;25/12;31/12";

            if (ConfigurationManager.AppSettings["Feriados"] != null)
            {
                feriados = ConfigurationManager.AppSettings["Feriados"].ToString();
            }
            DBControleMTA db = new DBControleMTA();

            if ( feriados.Contains(dataInicial.ToString("dd/MM")) ||
                db.IsFeriadoSinacor() )
            {
                return true;
            }

            return false;
        }

        private bool CSGDOkForMerge(FileInfo infoBRP, DateTime ultimoDiaUtil)
        {
            try
            {
                string[] allLinesBRP = File.ReadAllLines(infoBRP.FullName);

                string data1 = allLinesBRP[0].Substring(31, 8);
                string data2 = allLinesBRP[0].Substring(39, 8);
                string dataref = ultimoDiaUtil.ToString("yyyyMMdd");

                if (dataref.Equals(data1) && dataref.Equals(data2))
                {
                    return true;
                }

                logger.ErrorFormat("Arquivo [{0}] com datas incompativeis pra processamento Ref[{1}] Dt1[{2}] Dt2[{3}]",
                    infoBRP.FullName,
                    dataref,
                    data1,
                    data2);
            }
            catch (Exception ex)
            {
                logger.Error("CSGDOkForMerge: " + ex.Message, ex);
            }

            return false;
        }

        private void _zipFiles(string zipSaida, List<string> fileNames)
        {
            try
            {
                // Depending on the directory this could be very large and would require more attention
                // in a commercial package.

                // 'using' statements guarantee the stream is closed properly which is a big source
                // of problems otherwise.  Its exception safe as well which is great.
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipSaida)))
                {
                    s.UseZip64 = UseZip64.Off;

                    s.SetLevel(9); // 0 - store only to 9 - means best compression

                    byte[] buffer = new byte[4096];

                    foreach (string file in fileNames)
                    {

                        // Using GetFileName makes the result compatible with XP
                        // as the resulting path is not absolute.
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                        FileInfo finfo = new FileInfo(file);

                        // Setup the entry data as required.

                        // Crc and size are handled by the library for seakable streams
                        // so no need to do them here.

                        // Could also use the last write time or similar for the file.
                        entry.DateTime = finfo.LastWriteTime;
                        s.PutNextEntry(entry);

                        using (FileStream fs = File.OpenRead(file))
                        {

                            // Using a fixed size buffer here makes no noticeable difference for output
                            // but keeps a lid on memory usage.
                            int sourceBytes;
                            while (true)
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                if (sourceBytes <= 0)
                                    break;
                                s.Write(buffer, 0, sourceBytes);
                            }
                        }
                        s.CloseEntry();

                    }

                    // Finish/Close arent needed strictly as the using statement does this automatically

                    // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                    // the created file would be invalid.
                    s.Finish();

                    // Close is important to wrap things up and unlock the file.
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error("_zipFiles: " + ex.Message, ex);

                // No need to rethrow the exception as for our purposes its handled.
            }
        }


        public void syncBD()
        {
            try
            {
                logger.Info("Sincronizando controle de download com DB");

                Dictionary<string, DownloadMTAInfo> fromDB = DBControleMTA.ObterControleDownloadsMTA();

                logger.Info("Trouxe " + fromDB.Count + " registros de controle de download");

                lock (dcControle)
                {
                    foreach (KeyValuePair<string, DownloadMTAInfo> entry in fromDB)
                    {
                        if (dcControle.ContainsKey(entry.Key))
                            dcControle[entry.Key] = entry.Value;
                        else
                            dcControle.Add(entry.Key, entry.Value);
                    }
                }

                logger.Info("Controle de downloads sincronizado com DB");
            }
            catch (Exception ex)
            {
                logger.Error("syncDB: " + ex.Message, ex);
            }
        }

        public static string MD5HashFile(string sPath)
        {
            StreamReader sr = new StreamReader(sPath);
            MD5CryptoServiceProvider md5h = new MD5CryptoServiceProvider();

            string sHash = "";

            sHash = BitConverter.ToString(md5h.ComputeHash(sr.BaseStream));

            sr.Close();

            return sHash.ToUpperInvariant();
        }



        /// <summary>
        /// 
        /// </summary>
        public void BuscarInformacoesPublicasMTA()
        {
            try
            {
                if (IsFeriado(DateTime.Now))
                {
                    logger.Info("Uhuhu Feriadao, dia de curticao.....");
                    return;
                }

                if (bFtpcs)
                {
                    logger.Warn("Aguardando finalizar FTPCS");
                    return;
                }

                bFtpcs = true;

                logger.Info("Verificando se ha arquivos disponiveis...");
                List<string> remoteNames = new List<string>();

                remoteNames.Add("BDOM_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy"));
                remoteNames.Add("BTOP_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy"));
                remoteNames.Add("PAPD_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy"));
                remoteNames.Add("PAPT_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy"));
                remoteNames.Add("PROD_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy"));
                remoteNames.Add("PROT_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy"));
                remoteNames.Add("TABD_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy"));
                remoteNames.Add("TABT_" + UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy"));

                foreach(string remoteName in remoteNames )
                {
                    string scriptTemplate = File.ReadAllText(ftpcsScriptDir + "\\download.ftpcs");
                    string outputFile = String.Format(@"{0}\public-output-{1}.log", ftpcsOutputDir, DateTime.Now.ToString("yyyMMdd-HHmmss-fff"));
                    string remoteFile = String.Format("/PUBLICO/{0}", remoteName);
                    string locFile = String.Format(@"{0}\{1}", ftpcsLocalDir, remoteName);
                    string scriptFile = String.Format(@"{0}\download-{1}.ftpcs", ftpcsTempDir, DateTime.Now.ToString("yyyMMdd-HHmmss-fff"));

                    logger.Debug("Ftpcs Template ..: " + scriptFile);
                    logger.Debug("Ftpcs Output ....: " + outputFile);
                    logger.Debug("Ftpcs Remote ....: " + remoteFile);
                    logger.Debug("Ftpcs Local .....: " + locFile);
                    logger.Debug("Ftpcs Script ....: " + scriptFile);

                    if (File.Exists(locFile))
                    {
                        FileInfo info = new FileInfo(locFile);

                        if (info.Length > 0 )
                        {
                            logger.WarnFormat("Arquivo [{0}] ja foi transferido", info.Name);
                            continue;
                        }
                    }

                    string scrContent = String.Format(scriptTemplate, ftpcsUsr, ftpcsPwd, outputFile, remoteFile, locFile, "", "");

                    logger.Info( "Gerando script [" + scriptFile + "]");

                    File.WriteAllText(scriptFile, scrContent);

                    logger.Info("Invocando FTPCS...");

                    if (InvocarFtpcs(ftpcsSite, scriptFile))
                    {
                        logger.Info("FTPCS Finalizado com sucesso, removendo script");

                    }
                    else
                    {
                        logger.Info("FTPCS Finalizado com erro, removendo script e arquivo");
                        if (File.Exists(locFile))
                            File.Delete(locFile);
                    }

                    File.Delete(scriptFile);
                }

                bFtpcs = false;
            }
            catch (Exception ex)
            {
                logger.Error("BuscarInformacoesPublicasMTA: " + ex.Message, ex);
                bFtpcs = false;
            }
        }


        public void NotificarArquivosPublicos()
        {
            try
            {
                Thread.Sleep(15);

                if (bFtpcs)
                {
                    logger.Warn("Aguardando finalizar FTPCS");
                    return;
                }

                logger.Info("Verificando se ha arquivos disponiveis...");
                List<string> locNames = new List<string>();

                locNames.Add(String.Format(@"{0}\BDOM_{1}", ftpcsLocalDir, UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy")));
                locNames.Add(String.Format(@"{0}\BTOP_{1}", ftpcsLocalDir, UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy")));
                locNames.Add(String.Format(@"{0}\PAPD_{1}", ftpcsLocalDir, UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy")));
                locNames.Add(String.Format(@"{0}\PAPT_{1}", ftpcsLocalDir, UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy")));
                locNames.Add(String.Format(@"{0}\PROD_{1}", ftpcsLocalDir, UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy")));
                locNames.Add(String.Format(@"{0}\PROT_{1}", ftpcsLocalDir, UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy")));
                locNames.Add(String.Format(@"{0}\TABD_{1}", ftpcsLocalDir, UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy")));
                locNames.Add(String.Format(@"{0}\TABT_{1}", ftpcsLocalDir, UltimoDiaUtil(DateTime.Now.AddDays(-1)).ToString("ddMMyy")));

                string emails = ConfigurationManager.AppSettings["EmailsAvisoArquivosPublicos"].ToString();
                string subject = "ARQUIVOS DISPONIVEIS (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ")";
                string arquivoZip = "PUBLICOS-" + DateTime.Now.ToString("yyyy-MM-dd") + ".zip";
                List<string> lstArquivos = new List<string>();

                foreach (string arquivo in locNames)
                {
                    if (File.Exists(arquivo))
                    {
                        FileInfo info = new FileInfo(arquivo);
                        if (info.Length > 0 && !lstGenericos.Any(arquivo.Contains))
                        {
                            logger.Info("Arquivo " + arquivo + " disponivel, notificando [" + emails + "]");

                            lstArquivos.Add(arquivo);

                            string mtaGradual = String.Format(@"{0}\{1}", pathMTAGradual, info.Name);

                            if (!File.Exists(mtaGradual))
                            {
                                logger.Info("Copiando [" + info.FullName + "] para [" + mtaGradual + "]");
                                File.Copy(info.FullName, mtaGradual);
                            }

                            lstGenericos.Add(arquivo);
                        }
                    }
                }

                if (lstArquivos.Count > 0)
                    _enviaAviso(emails, subject, lstArquivos, arquivoZip);

            }
            catch (Exception ex)
            {
                logger.Error("NotificarArquivosPublicos: " + ex.Message, ex);
            }
        }


        public bool InvocarFtpcs(string site, string script)
        {
            try
            {

                process = new System.Diagnostics.Process();
                
                ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = pathJava;
                startInfo.Arguments = "-jar ftpcs.jar " + site + " script " + script;
                startInfo.WorkingDirectory = ftpcsWorkDir;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;

                logger.Info("Comando Ftpcs [" + pathJava + " " + startInfo.Arguments);

                process.StartInfo = startInfo;
                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler(process_Exited);
                process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
                process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
                process.Start();
                process.WaitForExit();

                XfbError error = (XfbError)process.ExitCode;

                if (error != XfbError.OK)
                {
                    logger.Error("Transferencia finalizou com erroxitcode: " + process.ExitCode + ":" + error.ToString() + " - removendo arquivo");

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("InvocarFtpcs: " + ex.Message, ex);
            }

            return false;
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                logger.Error("FTPCS/JAVA: [" + e.Data + "]");
            }
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                logger.Debug("FTPCS/JAVA: [" + e.Data + "]");
            }
        }

        private void process_Exited(object sender, EventArgs e)
        {
            XfbError error = (XfbError) process.ExitCode;
            logger.Info("Finalizou FTPCS/JAVA com exitcode: " + process.ExitCode + ":" + error.ToString());
            bFtpcs = false;
        }

        public void EfetuarLimpezaMargem()
        {
            try
            {
                logger.Info("Efetuando limpeza de Margem");

                string emails = ConfigurationManager.AppSettings["EmailsAvisoLimpezaMargem"].ToString();
                string subject = "LIMPEZA DE MARGEM (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ")";
                string body = "Limpeza de margem efetuada com sucesso";

                DBControleMTA db = new DBControleMTA();

                bool bRet = db.MTALimpezaMargem();

                if ( !bRet )
                    body = "Houve um erro na Limpeza de margem";

                logger.Info( body + ", notificando [" + emails + "]");

                _enviaAviso(emails, subject, body);

                logger.Info("Final da execucao de Limpeza de margem");
            }
            catch (Exception ex)
            {
                logger.Error("EfetuarLimpezaMargem: " + ex.Message, ex);
            }
        }
    }
}
