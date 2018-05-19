using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using System.Runtime.InteropServices;
using Gradual.BackOffice.BrokerageProcessor.Lib.IMBARQ;
using System.Configuration;
using Gradual.BackOffice.BrokerageProcessor.Db;
using Gradual.BackOffice.BrokerageProcessor.Lib.Cold;
using System.Net.Mail;
using ICSharpCode.SharpZipLib.Zip;
using System.Reflection;
using Gradual.BackOffice.BrokerageProcessor.Lib.Pdf;
using System.Security.Cryptography;
using System.Globalization;
using Gradual.BackOffice.BrokerageProcessor.Account;
using System.Collections.Concurrent;

namespace Gradual.BackOffice.BrokerageProcessor
{
    public class IMBARQProcessor
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private CultureInfo ciBr = CultureInfo.CreateSpecificCulture("pt-BR");
        private CultureInfo ciEn = CultureInfo.CreateSpecificCulture("en-US");

        private static IMBARQProcessor _me = null;

        private Dictionary<int, List<string>> dctRegs0001 = new Dictionary<int, List<string>>();
        private Dictionary<int, List<string>> dctRegs0002 = new Dictionary<int, List<string>>();
        private Dictionary<int, List<string>> dctBTC = new Dictionary<int, List<string>>();
        private Dictionary<int, List<string>> dctExigencia = new Dictionary<int, List<string>>();
        private Dictionary<int, List<string>> dctGarantias = new Dictionary<int, List<string>>();
        private Dictionary<int, List<string>> dctLiquidacoes = new Dictionary<int, List<string>>();
        private Dictionary<int, List<string>> dctOpcoes = new Dictionary<int, List<string>>();
        private Dictionary<int, List<string>> dctDividendos = new Dictionary<int, List<string>>();
        private Dictionary<int, List<string>> dctTermo = new Dictionary<int, List<string>>();
        private Dictionary<int, List<string>> dctCustodia = new Dictionary<int, List<string>>();
        private Dictionary<int, List<string>> dctPosVista = new Dictionary<int, List<string>>();

        private Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro28>> dctColdCustodia = new Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro28>>();
        private Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro17>> dctColdCustodiaDetLiq = new Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro17>>();
        private Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro26>> dctColdDividendos = new Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro26>>();
        private Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro18>> dctColdGarantias = new Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro18>>();
        private Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro01>> dctColdOpcoes = new Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro01>>();
        private Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro06>> dctColdBTC = new Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro06>>();
        private Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro14>> dctColdLiquidacoes = new Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro14>>();
        private Dictionary<int, RegistrosIMBARQ001.IMBARQ001_Registro19> dctColdExigencias = new Dictionary<int, RegistrosIMBARQ001.IMBARQ001_Registro19>();
        private Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro02>> dctColdPosVista = new Dictionary<int, List<RegistrosIMBARQ001.IMBARQ001_Registro02>>();
        private DateTime dataproc = DateTime.Now;

        private Dictionary<int, string> dctNome = new Dictionary<int, string>();

        public IMBARQProcessor Instance
        {
            get { return GetInstance(); }
        }

        public static IMBARQProcessor GetInstance()
        {
            if (_me == null)
            {
                _me = new IMBARQProcessor();
            }

            return _me;
        }

        public bool BuscarProcessarIMBARQ()
        {
            try
            {
                _CarregarListaNomes();
                string dirIMBARQReceb = null;
                string dirBackup = null;

                if (ConfigurationManager.AppSettings["DirIMBARQBackup"] != null)
                {
                    dirBackup = ConfigurationManager.AppSettings["DirIMBARQBackup"].ToString();

                    if (!Directory.Exists(dirBackup))
                    {
                        Directory.CreateDirectory(dirBackup);
                    }
                }
                else
                {
                    logger.Fatal("AppSetting 'DirIMBARQBackup' nao definido, abortando");
                    return false;
                }

                if (ConfigurationManager.AppSettings["DirIMBARQRecebido"] != null)
                {
                    dirIMBARQReceb = ConfigurationManager.AppSettings["DirIMBARQRecebido"].ToString();

                    if (Directory.Exists(dirIMBARQReceb))
                    {
                        string searchPattern = String.Format("IMBARQ00*.zip", DateTime.Now.ToString("yyyyMMdd"));

                        logger.Info("Verificando arquivos ja processados");

                        List<string> lstHashProcessados = new List<string>();
                        DirectoryInfo dirBkpInfo = new DirectoryInfo(dirBackup);

                        List<FileInfo> arqsBatchZipProcessados = dirBkpInfo.GetFiles(searchPattern).ToList();
                        foreach (FileInfo zipProcessados in arqsBatchZipProcessados)
                        {
                            string hash = MD5HashFile(zipProcessados.FullName);

                            lstHashProcessados.Add(hash);
                        }

                        // Verifica e descompacta os arquivos ZIP recebidos
                        DirectoryInfo dirZips = new DirectoryInfo(dirIMBARQReceb);
                        logger.InfoFormat("Buscando por arquivos [{0}] em [{1}]", searchPattern, dirIMBARQReceb);

                        List<FileInfo> arqsZipRecebidos = dirZips.GetFiles(searchPattern).ToList();

                        foreach (FileInfo finfo in arqsZipRecebidos)
                        {
                            try
                            {
                                string hshZip = MD5HashFile(finfo.FullName);

                                if (lstHashProcessados.Any(hshZip.Contains))
                                {
                                    logger.InfoFormat("Arquivo [{0}] ja processado, ignorando", finfo.FullName);
                                    File.Delete(finfo.FullName);
                                    continue;
                                }

                                string bkpName = String.Format(@"{0}\{1}", dirBackup, finfo);

                                logger.InfoFormat("Descompactando {0} em {1}", finfo, dirIMBARQReceb);

                                UnzipFiles(finfo.FullName, dirIMBARQReceb);

                                logger.InfoFormat("Movendo {0} para {1}", finfo, bkpName);

                                if (File.Exists(bkpName))
                                    File.Delete(bkpName);

                                File.Move(finfo.FullName, bkpName);
                            }
                            catch (Exception ex)
                            {
                                logger.Error("BuscarProcessarIMBARQ: " + ex.Message, ex);
                            }
                        }

                        // Tratar o IMBARQ001
                        List<string> lstArqsIMBARQ = new List<string>();
                        lstArqsIMBARQ.Clear();
                        searchPattern = String.Format("IMBARQ001*.txt", DateTime.Now.ToString("yyyyMMdd"));

                        logger.InfoFormat("Buscando por arquivos [{0}] em [{1}]", searchPattern, dirIMBARQReceb);

                        lstArqsIMBARQ.AddRange(Directory.GetFiles(dirIMBARQReceb, searchPattern).ToList());

                        foreach (string filename in lstArqsIMBARQ)
                        {
                            ProcessIMBARQ001(filename);
                        }

                        // Tratar o IMBARQ002
                        lstArqsIMBARQ.Clear();
                        searchPattern = String.Format("IMBARQ002*.txt", DateTime.Now.ToString("yyyyMMdd"));

                        logger.InfoFormat("Buscando por arquivos [{0}] em [{1}]", searchPattern, dirIMBARQReceb);

                        lstArqsIMBARQ.AddRange(Directory.GetFiles(dirIMBARQReceb, searchPattern).ToList());

                        foreach (string filename in lstArqsIMBARQ)
                        {
                            ProcessIMBARQ002(filename);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("BuscarProcessarIMBARQ(): " + ex.Message, ex);
            }

            return true;
        }

        public bool ProcessIMBARQ001(string filename)
        {
            try
            {

                string dirSplitted = null;
                string dirBackup = null;

                if (ConfigurationManager.AppSettings["DirIMBARQSplitted"] != null)
                {
                    dirSplitted = ConfigurationManager.AppSettings["DirIMBARQSplitted"].ToString();

                    if (!Directory.Exists(dirSplitted))
                    {
                        Directory.CreateDirectory(dirSplitted);
                    }
                }
                else
                {
                    logger.Fatal("AppSetting 'DirIMBARQSplitted' nao definido, abortando");
                    return false;
                }


                if (ConfigurationManager.AppSettings["DirIMBARQBackup"] != null)
                {
                    dirBackup = ConfigurationManager.AppSettings["DirIMBARQBackup"].ToString();

                    if (!Directory.Exists(dirBackup))
                    {
                        Directory.CreateDirectory(dirBackup);
                    }
                }
                else
                {
                    logger.Fatal("AppSetting 'DirIMBARQBackup' nao definido, abortando");
                    return false;
                }


                //filename = @"C:\Users\apiza\Documents\Manuais\BMF-Bovespa\IPN\IMBARQ001_BV000272201709110000001000227063318.txt";

                string[] allLines = File.ReadAllLines(filename);
                bool cabecalholido = false;

                string cabecalho = "";
                string body = "";
                string clienteGradual = "";
                string cliente = "";
                string clienteOrig = "";
                string clienteTmp = "";

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);



                //DBClientesCOLD db = new DBClientesCOLD();
                //Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();

                RegistrosIMBARQ001.IMBARQ001_Trailler trailler = new RegistrosIMBARQ001.IMBARQ001_Trailler();
                RegistrosIMBARQ001.IMBARQ001_Header header = new RegistrosIMBARQ001.IMBARQ001_Header();
                dctRegs0001.Clear();
                dctBTC.Clear();
                dctExigencia.Clear();
                dctGarantias.Clear();
                dctOpcoes.Clear();
                dctDividendos.Clear();
                dctTermo.Clear();
                dctPosVista.Clear();
                dctCustodia.Clear();
                dctColdCustodia.Clear();
                dctColdCustodiaDetLiq.Clear();
                dctColdDividendos.Clear();
                dctColdGarantias.Clear();
                dctColdOpcoes.Clear();
                dctColdBTC.Clear();
                dctColdLiquidacoes.Clear();
                dctColdExigencias.Clear();
                dctColdPosVista.Clear();

                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ');

                    if (!String.IsNullOrEmpty(line))
                    {
                        string tipoRegistro = line.Substring(0, 2);
                        int CodCliente = -1;

                        string linhasBtc = null;
                        string linhasExigencia = null;
                        string linhasGarantias = null;
                        string linhasLiquidacoes = null;
                        string linhasOpcoes = null;
                        string linhasDividendos = null;
                        string linhasTermo = null;
                        string linhasCustodia = null;
                        string linhasPosVista = null;

                        switch (tipoRegistro)
                        {
                            case "00":
                                {
                                    header = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Header>(line);

                                    //dataproc = header.DataGeracaoArquivo.DataMovimento.ByteArrayToDate("yyyyMMdd");
                                    dataproc = header.DataGeracaoArquivo.ByteArrayToDate("yyyyMMdd");
                                }
                                break; // Header
                            case "01":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro01 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro01>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro01>(reg);
                                    linhasOpcoes = GetCSV<RegistrosIMBARQ001.IMBARQ001_Registro01>(reg);
                                    adicionaOpcoes(CodCliente, reg);
                                }
                                break; // Posicao
                            case "02":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro02 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro02>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro02>(reg);
                                    linhasPosVista = GetCSV<RegistrosIMBARQ001.IMBARQ001_Registro02>(reg);
                                    adicionaPosicaoVista(CodCliente, reg);
                                }
                                break; // Posicao
                            case "03":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro03 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro03>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro03>(reg);
                                }
                                break;
                            case "04":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro04 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro04>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro04>(reg);
                                }
                                break;
                            case "05":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro05 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro05>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro05>(reg);
                                }
                                break;//  Posicao
                            case "06":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro06 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro06>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro06>(reg);
                                    linhasBtc = GetCSV<RegistrosIMBARQ001.IMBARQ001_Registro06>(reg);
                                    adicionaBTC(CodCliente, reg);
                                }
                                break;//  Posicao
                            case "07":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro07 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro07>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro07>(reg);
                                    linhasTermo = GetCSV<RegistrosIMBARQ001.IMBARQ001_Registro07>(reg);
                                }
                                break;
                            case "08":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro08 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro08>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro08>(reg);
                                }
                                break;
                            case "09":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro09 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro09>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro09>(reg);
                                }
                                break;
                            case "10":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro10 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro10>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro10>(reg);
                                }
                                break;
                            case "11":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro11 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro11>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro11>(reg);
                                }
                                break;
                            case "12":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro12 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro12>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro12>(reg);
                                }
                                break;// Liquidacao
                            case "13":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro13 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro13>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro13>(reg);
                                }
                                break;// Liquidacao
                            case "14":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro14 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro14>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro14>(reg);
                                    linhasLiquidacoes = GetCSV<RegistrosIMBARQ001.IMBARQ001_Registro14>(reg);
                                    adicionaLiquidacoes(CodCliente, reg);
                                }
                                break;// LIquidacao
                            case "15":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro15 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro15>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro15>(reg);
                                }
                                break;// Liquidacao
                            case "16":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro16 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro16>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro16>(reg);
                                }
                                break;// Liquidacao
                            case "17":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro17 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro17>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro17>(reg);
                                    adicionaCustodiaDetLiqCliente(CodCliente, reg);
                                }
                                break;// Liquidacao
                            case "18":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro18 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro18>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro18>(reg);
                                    linhasGarantias = GetCSV<RegistrosIMBARQ001.IMBARQ001_Registro18>(reg);
                                    adicionaGarantiasCliente(CodCliente, reg);
                                }
                                break;// Garantias
                            case "31":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro31 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro31>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro31>(reg);
                                }
                                break;// Margem/garantias
                            case "19":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro19 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro19>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro19>(reg);
                                    adicionaExigencias(CodCliente, reg);
                                }
                                break;// Margem/garantias
                            case "20":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro20 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro20>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro20>(reg);
                                }
                                break;
                            case "21":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro21 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro21>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro21>(reg);
                                }
                                break;
                            case "22":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro22 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro22>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro22>(reg);
                                }
                                break;
                            case "23":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro23 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro23>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro23>(reg);
                                }
                                break;// Eventos aluguel ativos (BTC)
                            case "24":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro24 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro24>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro24>(reg);
                                }
                                break;
                            case "25":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro25 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro25>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro25>(reg);
                                }
                                break;
                            case "26":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro26 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro26>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro26>(reg);
                                    linhasDividendos = GetCSV<RegistrosIMBARQ001.IMBARQ001_Registro26>(reg);
                                    adicionaDividendosCliente(CodCliente, reg);
                                }
                                break;// Dividendos
                            case "27":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro27 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro27>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro27>(reg);
                                }
                                break;
                            case "28":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro28 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro28>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro28>(reg);
                                    linhasCustodia = GetCSV < RegistrosIMBARQ001.IMBARQ001_Registro28>(reg);
                                    adicionaCustodiaCliente(CodCliente, reg);
                                }
                                break;// Saldo custodia
                            case "29":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro29 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro29>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro29>(reg);
                                    //linhasCustodia = GetCSV<RegistrosIMBARQ001.IMBARQ001_Registro29>(reg);
                                }
                                break;
                            case "30":
                                {
                                    RegistrosIMBARQ001.IMBARQ001_Registro30 reg = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Registro30>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ001.IMBARQ001_Registro30>(reg);
                                }
                                break;
                            case "99":
                                {
                                    trailler = FromStringBlock<RegistrosIMBARQ001.IMBARQ001_Trailler>(line);
                                }
                                break; // Trailler
                        }

                        if (CodCliente != -1)
                        {
                            if (!dctRegs0001.ContainsKey(CodCliente))
                            {
                                dctRegs0001.Add(CodCliente, new List<string>());
                            }

                            if (!dctBTC.ContainsKey(CodCliente))
                            {
                                dctBTC.Add(CodCliente, new List<string>());
                            }
                            if (!dctExigencia.ContainsKey(CodCliente))
                            {
                                dctExigencia.Add(CodCliente, new List<string>());
                            }
                            if (!dctGarantias.ContainsKey(CodCliente))
                            {
                                dctGarantias.Add(CodCliente, new List<string>());
                            }
                            if (!dctLiquidacoes.ContainsKey(CodCliente))
                            {
                                dctLiquidacoes.Add(CodCliente, new List<string>());
                            }
                            if (!dctOpcoes.ContainsKey(CodCliente))
                            {
                                dctOpcoes.Add(CodCliente, new List<string>());
                            }
                            if (!dctDividendos.ContainsKey(CodCliente))
                            {
                                dctDividendos.Add(CodCliente, new List<string>());
                            }
                            if (!dctTermo.ContainsKey(CodCliente))
                            {
                                dctTermo.Add(CodCliente, new List<string>());
                            }
                            if (!dctCustodia.ContainsKey(CodCliente))
                            {
                                dctCustodia.Add(CodCliente, new List<string>());
                            }
                            if (!dctPosVista.ContainsKey(CodCliente))
                            {
                                dctPosVista.Add(CodCliente, new List<string>());
                            }

                            dctRegs0001[CodCliente].Add(line);

                            if (!String.IsNullOrEmpty(linhasBtc))
                                dctBTC[CodCliente].Add(linhasBtc);

                            if (!String.IsNullOrEmpty(linhasExigencia))
                                dctExigencia[CodCliente].Add(linhasExigencia);

                            if (!String.IsNullOrEmpty(linhasGarantias))
                                dctGarantias[CodCliente].Add(linhasGarantias);

                            if (!String.IsNullOrEmpty(linhasLiquidacoes))
                                dctLiquidacoes[CodCliente].Add(linhasLiquidacoes);

                            if (!String.IsNullOrEmpty(linhasOpcoes))
                                dctOpcoes[CodCliente].Add(linhasOpcoes);

                            if (!String.IsNullOrEmpty(linhasDividendos))
                                dctDividendos[CodCliente].Add(linhasDividendos);

                            if (!String.IsNullOrEmpty(linhasTermo))
                                dctTermo[CodCliente].Add(linhasTermo);

                            if (!String.IsNullOrEmpty(linhasCustodia))
                                dctCustodia[CodCliente].Add(linhasCustodia);

                            if (!String.IsNullOrEmpty(linhasPosVista))
                                dctPosVista[CodCliente].Add(linhasPosVista);
                        }
                    }
                }

                StringBuilder contents = new StringBuilder();

                foreach (KeyValuePair<int, List<string>> item in dctRegs0001)
                {
                    List<string> lines = item.Value;
                    int CodCliente = item.Key;

                    contents.Clear();
                    contents.AppendLine(BlockToString<RegistrosIMBARQ001.IMBARQ001_Header>(header));

                    for (int i = 0; i < lines.Count; i++)
                        contents.AppendLine(lines[i]);

                    trailler.TotalRegistros = ASCIIEncoding.ASCII.GetBytes(string.Format("{0:000000000}", lines.Count));
                    contents.AppendLine(BlockToString<RegistrosIMBARQ001.IMBARQ001_Trailler>(trailler));

                    if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                    {
                        Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                    }
                    string imbarqCliente = string.Format(@"{0}\{1}\IMBARQ001-{2}.txt", dirSplitted, CodCliente, CodCliente);

                    File.WriteAllText(imbarqCliente, contents.ToString());
                }

                foreach (KeyValuePair<int, List<string>> item in dctPosVista)
                {
                    List<string> lines = item.Value;
                    int CodCliente = item.Key;

                    if (lines.Count > 0)
                    {
                        contents.Clear();
                        contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro02>(new RegistrosIMBARQ001.IMBARQ001_Registro02()));

                        for (int i = 0; i < lines.Count; i++)
                            contents.AppendLine(lines[i]);

                        if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                        {
                            Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                        }
                        string imbarqCliente = string.Format(@"{0}\{1}\POSVISTA-{2}.csv", dirSplitted, CodCliente, CodCliente);

                        File.WriteAllText(imbarqCliente, contents.ToString());
                    }
                }

                foreach (KeyValuePair<int, List<string>> item in dctOpcoes)
                {
                    List<string> lines = item.Value;
                    int CodCliente = item.Key;

                    if (lines.Count > 0)
                    {
                        contents.Clear();
                        contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro01>(new RegistrosIMBARQ001.IMBARQ001_Registro01()));

                        for (int i = 0; i < lines.Count; i++)
                            contents.AppendLine(lines[i]);

                        if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                        {
                            Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                        }
                        string imbarqCliente = string.Format(@"{0}\{1}\OPCOES-{2}.csv", dirSplitted, CodCliente, CodCliente);

                        File.WriteAllText(imbarqCliente, contents.ToString());
                    }
                }


                foreach (KeyValuePair<int, List<string>> item in dctBTC)
                {
                    List<string> lines = item.Value;
                    int CodCliente = item.Key;

                    if (lines.Count > 0)
                    {
                        contents.Clear();
                        contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro06>(new RegistrosIMBARQ001.IMBARQ001_Registro06()));

                        for (int i = 0; i < lines.Count; i++)
                            contents.AppendLine(lines[i]);

                        if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                        {
                            Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                        }
                        string imbarqCliente = string.Format(@"{0}\{1}\BTC-{2}.csv", dirSplitted, CodCliente, CodCliente);

                        File.WriteAllText(imbarqCliente, contents.ToString());
                    }
                }

                convertColdBTC();

                foreach (KeyValuePair<int, List<string>> item in dctTermo)
                {
                    List<string> lines = item.Value;
                    int CodCliente = item.Key;

                    if (lines.Count > 0)
                    {
                        contents.Clear();
                        contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro07>(new RegistrosIMBARQ001.IMBARQ001_Registro07()));

                        for (int i = 0; i < lines.Count; i++)
                            contents.AppendLine(lines[i]);

                        if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                        {
                            Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                        }
                        string imbarqCliente = string.Format(@"{0}\{1}\Termo-{2}.csv", dirSplitted, CodCliente, CodCliente);

                        File.WriteAllText(imbarqCliente, contents.ToString());
                    }
                }

                foreach (KeyValuePair<int, List<string>> item in dctGarantias)
                {
                    List<string> lines = item.Value;
                    int CodCliente = item.Key;

                    if (lines.Count > 0)
                    {
                        contents.Clear();
                        contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro18>(new RegistrosIMBARQ001.IMBARQ001_Registro18()));

                        for (int i = 0; i < lines.Count; i++)
                            contents.AppendLine(lines[i]);

                        if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                        {
                            Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                        }
                        string imbarqCliente = string.Format(@"{0}\{1}\Garantias-{2}.csv", dirSplitted, CodCliente, CodCliente);

                        File.WriteAllText(imbarqCliente, contents.ToString());
                    }
                }


                foreach (KeyValuePair<int, List<string>> item in dctCustodia)
                {
                    List<string> lines = item.Value;
                    int CodCliente = item.Key;

                    if (lines.Count > 0)
                    {
                        contents.Clear();
                        contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro28>(new RegistrosIMBARQ001.IMBARQ001_Registro28()));

                        for (int i = 0; i < lines.Count; i++)
                            contents.AppendLine(lines[i]);

                        if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                        {
                            Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                        }
                        string imbarqCliente = string.Format(@"{0}\{1}\Custodia-{2}.csv", dirSplitted, CodCliente, CodCliente);

                        File.WriteAllText(imbarqCliente, contents.ToString());
                    }
                }


                foreach (KeyValuePair<int, List<string>> item in dctDividendos)
                {
                    List<string> lines = item.Value;
                    int CodCliente = item.Key;

                    if (lines.Count > 0)
                    {
                        contents.Clear();
                        contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro26>(new RegistrosIMBARQ001.IMBARQ001_Registro26()));

                        for (int i = 0; i < lines.Count; i++)
                            contents.AppendLine(lines[i]);

                        if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                        {
                            Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                        }
                        string imbarqCliente = string.Format(@"{0}\{1}\Dividendos-{2}.csv", dirSplitted, CodCliente, CodCliente);

                        File.WriteAllText(imbarqCliente, contents.ToString());
                    }
                }

                convertColdOpcoes();
                convertColdGarantias();
                convertColdCustodia();
                convertColdDividendos();
                convertColdLiquidacoes();
                convertColdExigencias();
                convertColdPosicaoVista();

                FileInfo finfo = new FileInfo(filename);

                string imbarqBkp = string.Format(@"{0}\{1}", dirBackup, finfo.Name);

                if (File.Exists(imbarqBkp))
                    File.Delete(imbarqBkp);

                File.Move(filename, imbarqBkp);

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("ProcessIMBARQ001: " + ex.Message, ex);
            }

            return false;
        }

        private void adicionaPosicaoVista(int CodCliente, RegistrosIMBARQ001.IMBARQ001_Registro02 registro)
        {
            if (CodCliente != -1)
            {
                if (!dctColdPosVista.ContainsKey(CodCliente))
                {
                    dctColdPosVista.Add(CodCliente, new List<RegistrosIMBARQ001.IMBARQ001_Registro02>());
                }

                dctColdPosVista[CodCliente].Add(registro);
            }
        }

        private void adicionaExigencias(int CodCliente, RegistrosIMBARQ001.IMBARQ001_Registro19 registro)
        {
            if (CodCliente != -1)
            {
                if (!dctColdExigencias.ContainsKey(CodCliente))
                    dctColdExigencias.Add(CodCliente, registro);
                else
                    logger.Error("JA EXISTE REGISTRO DE MARGEM REQUERIDA PARA CLIENTE [" + CodCliente + "]");
            }
        }


        private void adicionaLiquidacoes(int CodCliente, RegistrosIMBARQ001.IMBARQ001_Registro14 registro)
        {
            if (CodCliente != -1)
            {
                if (!dctColdLiquidacoes.ContainsKey(CodCliente))
                {
                    dctColdLiquidacoes.Add(CodCliente, new List<RegistrosIMBARQ001.IMBARQ001_Registro14>());
                }

                dctColdLiquidacoes[CodCliente].Add(registro);
            }
        }


        private void adicionaBTC(int CodCliente, RegistrosIMBARQ001.IMBARQ001_Registro06 registro)
        {
            if (CodCliente != -1)
            {
                if (!dctColdBTC.ContainsKey(CodCliente))
                {
                    dctColdBTC.Add(CodCliente, new List<RegistrosIMBARQ001.IMBARQ001_Registro06>());
                }

                dctColdBTC[CodCliente].Add(registro);
            }
        }

        private void adicionaOpcoes(int CodCliente, RegistrosIMBARQ001.IMBARQ001_Registro01 registro)
        {
            if (CodCliente != -1)
            {
                if (!dctColdOpcoes.ContainsKey(CodCliente))
                {
                    dctColdOpcoes.Add(CodCliente, new List<RegistrosIMBARQ001.IMBARQ001_Registro01>());
                }

                dctColdOpcoes[CodCliente].Add(registro);
            }
        }

        private void adicionaGarantiasCliente(int CodCliente, RegistrosIMBARQ001.IMBARQ001_Registro18 registro)
        {
            if (CodCliente != -1)
            {
                if (!dctColdGarantias.ContainsKey(CodCliente))
                {
                    dctColdGarantias.Add(CodCliente, new List<RegistrosIMBARQ001.IMBARQ001_Registro18>());
                }

                dctColdGarantias[CodCliente].Add(registro);
            }
        }

        private void adicionaDividendosCliente(int CodCliente, RegistrosIMBARQ001.IMBARQ001_Registro26 registro)
        {
            if (CodCliente != -1)
            {
                if (!dctColdDividendos.ContainsKey(CodCliente))
                {
                    dctColdDividendos.Add(CodCliente, new List<RegistrosIMBARQ001.IMBARQ001_Registro26>());
                }

                dctColdDividendos[CodCliente].Add(registro);
            }
        }


        private void adicionaCustodiaCliente(int CodCliente, RegistrosIMBARQ001.IMBARQ001_Registro28 registro)
        {
            if (CodCliente != -1)
            {
                if (!dctColdCustodia.ContainsKey(CodCliente))
                {
                    dctColdCustodia.Add(CodCliente, new List<RegistrosIMBARQ001.IMBARQ001_Registro28>());
                }

                dctColdCustodia[CodCliente].Add(registro);
            }
        }

        private void adicionaCustodiaDetLiqCliente(int CodCliente, RegistrosIMBARQ001.IMBARQ001_Registro17 registro)
        {
            if (CodCliente != -1)
            {
                if (!dctColdCustodiaDetLiq.ContainsKey(CodCliente))
                {
                    dctColdCustodiaDetLiq.Add(CodCliente, new List<RegistrosIMBARQ001.IMBARQ001_Registro17>());
                }

                dctColdCustodiaDetLiq[CodCliente].Add(registro);
            }
        }

        private class ResultLine
        {
            public ResultLine() { }

            public int Carteira { get; set; }
            public string ISIN { get; set; }
        }




        private bool convertColdCustodia()
        {
            string dirSplitted;

            if (ConfigurationManager.AppSettings["DirIMBARQSplitted"] != null)
            {
                dirSplitted = ConfigurationManager.AppSettings["DirIMBARQSplitted"].ToString();

                if (!Directory.Exists(dirSplitted))
                {
                    Directory.CreateDirectory(dirSplitted);
                }
            }
            else
            {
                logger.Fatal("AppSetting 'DirIMBARQSplitted' nao definido, abortando");
                return false;
            }

            string tplcabec = File.ReadAllText(ConfigurationManager.AppSettings["TemplateCabecalhoCustodia"].ToString());

            StringBuilder contents = new StringBuilder();
            StringBuilder contBackoffice = new StringBuilder();


            foreach (KeyValuePair<int, List<RegistrosIMBARQ001.IMBARQ001_Registro28>> item in dctColdCustodia)
            {
                int CodCliente = item.Key;

                Dictionary<int, List<string>> dctAtivos = new Dictionary<int, List<string>>(); 


                List<RegistrosIMBARQ001.IMBARQ001_Registro28> registros = item.Value
                    .OrderBy( o => o.Carteira.ByteArrayToString() )
                    .ThenBy( o=>o.CodigoNegociacao.ByteArrayToString())
                    .ToList();

                foreach (RegistrosIMBARQ001.IMBARQ001_Registro28 registro in registros)
                {
                    if (!dctAtivos.ContainsKey(registro.Carteira.ByteArrayToInt32()))
                    {
                        dctAtivos.Add(registro.Carteira.ByteArrayToInt32(), new List<string>());
                    }

                    if (!dctAtivos[registro.Carteira.ByteArrayToInt32()].Contains(registro.CodigoIsinBase.ByteArrayToString().Trim()))
                    {
                        dctAtivos[registro.Carteira.ByteArrayToInt32()].Add(registro.CodigoIsinBase.ByteArrayToString().Trim());
                    }
                }

                if (dctColdCustodiaDetLiq.ContainsKey(CodCliente))
                {
                    List<RegistrosIMBARQ001.IMBARQ001_Registro17> detalhes = dctColdCustodiaDetLiq[CodCliente];

                    foreach (RegistrosIMBARQ001.IMBARQ001_Registro17 detalhe in detalhes)
                    {
                        if (!dctAtivos.ContainsKey(detalhe.Carteira.ByteArrayToInt32()))
                        {
                            dctAtivos.Add(detalhe.Carteira.ByteArrayToInt32(), new List<string>());
                        }

                        if (!dctAtivos[detalhe.Carteira.ByteArrayToInt32()].Contains(detalhe.CodigoISIN.ByteArrayToString().Trim()))
                        {
                            dctAtivos[detalhe.Carteira.ByteArrayToInt32()].Add(detalhe.CodigoISIN.ByteArrayToString().Trim());
                        }
                    }
                }


                if (dctAtivos.Count > 0)
                {
                    contents.Clear();

                    contents.AppendLine(tplcabec.Replace("[DATAPROC]", DateTime.Now.ToString("dd/MM/yyyy")));

                    contents.AppendLine("       CLIENTE : " + CodCliente + " - " + BuscarNomeCliente(CodCliente).ToUpperInvariant());

                    //contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro28>(new RegistrosIMBARQ001.IMBARQ001_Registro28()));

                    string carteiraAnterior = "";
                    foreach (KeyValuePair<int, List<string>> itemCustodia in dctAtivos)
                    {
                        string carteira = itemCustodia.Key.ToString();
                        string descricaoCarteira = ((IMBARQ_SubAccountCode) itemCustodia.Key).ToString().Replace("_", " ");

                        if (!carteiraAnterior.Equals(carteira))
                        {
                            contents.AppendLine("       CARTEIRA ..: " + carteira + " - " + descricaoCarteira);
                            carteiraAnterior = carteira;
                        }

                        foreach (string isin in itemCustodia.Value) 
                        {
                            RegistrosIMBARQ001.IMBARQ001_Registro28 registro28 = registros
                                .FirstOrDefault(o => o.Carteira.ByteArrayToInt32() == itemCustodia.Key && o.CodigoIsinBase.ByteArrayToString().Trim().Equals(isin));

                            bool bHasRegistro28 = false;

                            if (!registro28.Equals(default(RegistrosIMBARQ001.IMBARQ001_Registro28)))
                            {

                                decimal qtde = registros
                                    .Where(o => o.Carteira.ByteArrayToInt32() == itemCustodia.Key && o.CodigoIsinBase.ByteArrayToString().Trim().Equals(isin))
                                    .Sum(o => o.QtdeAcoesCustodia.ByteArrayToDecimal(3));

                                decimal qtdeBloqueada = registros
                                    .Where(o => o.Carteira.ByteArrayToInt32() == itemCustodia.Key && o.CodigoIsinBase.ByteArrayToString().Trim().Equals(isin))
                                    .Sum(o => o.QtdeTotalAcoesBloqueadas.ByteArrayToDecimal(0));

                                contents.AppendFormat("{0} {1} {2} {3} {4} {5} \r\n",
                                    registro28.CodigoNegociacao.ByteArrayToString()
                                    , registro28.CodigoIsinBase.ByteArrayToString()
                                    , registro28.DistribuicaoBase.ByteArrayToString()
                                    , registro28.EspecificacaoAcaoBase.ByteArrayToString()
                                    , qtde.ToString("#,##0", ciBr).PadLeft(35)
                                    , qtdeBloqueada.ToString("#,##0", ciBr).PadLeft(24));

                                bHasRegistro28 = true;
                            }


                            if (dctColdCustodiaDetLiq.ContainsKey(CodCliente))
                            {
                                List<RegistrosIMBARQ001.IMBARQ001_Registro17> detalhes17 = dctColdCustodiaDetLiq[CodCliente]
                                    .Where(o => o.CodigoISIN.ByteArrayToString() == isin &&
                                        o.Carteira.ByteArrayToInt32() == itemCustodia.Key)
                                    .OrderBy(o => o.DataLiquidacao.ByteArrayToDate("yyyy-MM-dd"))
                                    .ToList();

                                foreach (RegistrosIMBARQ001.IMBARQ001_Registro17 detalhe in detalhes17)
                                {
                                    decimal qtdeLiquidacao = detalhe.QtdeTotalInstrucaoLiquidacao.ByteArrayToDecimal(6);

                                    if (qtdeLiquidacao == 0)
                                        continue;

                                    if (!bHasRegistro28)
                                    {
                                        string cdNego = DBCadastroPapeis.ObterCodigoNegociacao(detalhe.CodigoISIN.ByteArrayToString().Trim());

                                        contents.AppendFormat("{0} {1} {2} {3} {4} {5} \r\n",
                                            cdNego.PadRight(12)
                                            , detalhe.CodigoISIN.ByteArrayToString()
                                            , detalhe.DistribuicaoISIN.ByteArrayToInt32()
                                            , String.Empty.PadLeft(10)
                                            , Decimal.Zero.ToString("#,##0", ciBr).PadLeft(35)
                                            , Decimal.Zero.ToString("#,##0", ciBr).PadLeft(24));

                                        bHasRegistro28 = true;
                                    }

                                    string side = detalhe.NaturezaOperacao.ByteArrayToString();

                                    if (side.Equals("D"))
                                    {
                                        contents.AppendFormat("{0} {1} {2} {3}\r\n",
                                            " ".PadLeft(40),
                                            detalhe.DataLiquidacao.ByteArrayToDate("yyyy-MM-dd").ToString("dd/MM/yyyy"),
                                            qtdeLiquidacao.ToString("#,##0", ciBr).PadLeft(73),
                                            "0".PadLeft(23));
                                    }
                                    else
                                    {
                                        contents.AppendFormat("{0} {1} {2} {3}\r\n",
                                            " ".PadLeft(40),
                                            detalhe.DataLiquidacao.ByteArrayToDate("yyyy-MM-dd").ToString("dd/MM/yyyy"),
                                            "0".PadLeft(73),
                                            qtdeLiquidacao.ToString("#,##0", ciBr).PadLeft(23));
                                    }
                                }
                            }

                        }
                    }

                    if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                    {
                        Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                    }
                    string imbarqCliente = string.Format(@"{0}\{1}\Custodia-{2}.txt", dirSplitted, CodCliente, CodCliente);

                    File.WriteAllText(imbarqCliente, contents.ToString());

                    contBackoffice.Append(contents.ToString());
                    contBackoffice.AppendLine("-".PadLeft(200, '-'));
                    contBackoffice.AppendLine("\r\n");
                }
            }

            if (!Directory.Exists(dirSplitted + "\\Backoffice"))
            {
                Directory.CreateDirectory(dirSplitted + "\\Backoffice");
            }
            string custBackoffice = string.Format(@"{0}\Backoffice\Custodia-Backoffice.txt", dirSplitted);

            File.WriteAllText(custBackoffice, contBackoffice.ToString());

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool convertColdDividendos()
        {
            string dirSplitted;

            if (ConfigurationManager.AppSettings["DirIMBARQSplitted"] != null)
            {
                dirSplitted = ConfigurationManager.AppSettings["DirIMBARQSplitted"].ToString();

                if (!Directory.Exists(dirSplitted))
                {
                    Directory.CreateDirectory(dirSplitted);
                }
            }
            else
            {
                logger.Fatal("AppSetting 'DirIMBARQSplitted' nao definido, abortando");
                return false;
            }

            string tplcabec = File.ReadAllText(ConfigurationManager.AppSettings["TemplateCabecalhoDividendos"].ToString());

            StringBuilder contents = new StringBuilder();


            foreach (KeyValuePair<int, List<RegistrosIMBARQ001.IMBARQ001_Registro26>> item in dctColdDividendos)
            {
                int CodCliente = item.Key;

                List<RegistrosIMBARQ001.IMBARQ001_Registro26> registros = item.Value
                    .OrderBy(o => o.CodigoIsinBase.ByteArrayToString())
                    .ToList();

                if (registros.Count > 0)
                {
                    contents.Clear();

                    contents.AppendLine(tplcabec.Replace("[DATAPROC]", DateTime.Now.ToString("dd/MM/yyyy")));

                    contents.AppendLine("       CLIENTE : " + CodCliente + " - " + BuscarNomeCliente(CodCliente).ToUpperInvariant());

                    contents.AppendLine("         Cód. Isin      Distr.          Pagamento         Contrato     Qtde. Original    Valor Provento                Valor Tipo                 Situação");

                    //contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro28>(new RegistrosIMBARQ001.IMBARQ001_Registro28()));

                    string carteiraAnterior = "";
                    foreach (RegistrosIMBARQ001.IMBARQ001_Registro26 itemDividendo in registros)
                    {
                        decimal qtde = itemDividendo.QtdeAcoesBase.ByteArrayToDecimal(3);
                        decimal valorBruto = itemDividendo.ValorBrutoDividendo.ByteArrayToDecimal(2);
                        decimal percIR = itemDividendo.PercentualImpostoRenda.ByteArrayToDecimal(2);

                        decimal valorProvento = valorBruto * ((100-percIR)/100) / qtde;
                        decimal valorPago = valorProvento * qtde;

                        contents.AppendFormat("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}\r\n"
                            , "T".PadRight(6)
                            , itemDividendo.CodigoIsinBase.ByteArrayToString()
                            , itemDividendo.DistribuicaoBase.ByteArrayToInt32().ToString().PadLeft(8)
                            , itemDividendo.DataPrevistaPagamentoDividendo.ByteArrayToDate("yyyyMMdd").ToString("dd/MM/yyyy").PadLeft(18)
                            , itemDividendo.NumeroProcesso.ByteArrayToString().PadLeft(17)
                            , itemDividendo.QtdeAcoesBase.ByteArrayToDecimal(3).ToString("#,##0", ciBr).PadLeft(18)
                            , valorProvento.ToString("0.00000000000", ciBr).PadLeft(18)
                            , valorPago.ToString("#,##0.00", ciBr).PadLeft(20)
                            , itemDividendo.DescricaoProvento.ByteArrayToString().PadRight(20)
                            , "DEFINITIVO");
                    }

                    if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                    {
                        Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                    }
                    string imbarqCliente = string.Format(@"{0}\{1}\DIVIDENDOS-{2}.txt", dirSplitted, CodCliente, CodCliente);

                    File.WriteAllText(imbarqCliente, contents.ToString());
                }
            }

            return false;
        }

        private bool convertColdGarantias()
        {
            string dirSplitted;

            if (ConfigurationManager.AppSettings["DirIMBARQSplitted"] != null)
            {
                dirSplitted = ConfigurationManager.AppSettings["DirIMBARQSplitted"].ToString();

                if (!Directory.Exists(dirSplitted))
                {
                    Directory.CreateDirectory(dirSplitted);
                }
            }
            else
            {
                logger.Fatal("AppSetting 'DirIMBARQSplitted' nao definido, abortando");
                return false;
            }

            string tplcabec = File.ReadAllText(ConfigurationManager.AppSettings["TemplateCabecalhoGarantias"].ToString());

            StringBuilder contents = new StringBuilder();


            foreach (KeyValuePair<int, List<RegistrosIMBARQ001.IMBARQ001_Registro18>> item in dctColdGarantias)
            {
                int CodCliente = item.Key;

                List<RegistrosIMBARQ001.IMBARQ001_Registro18> registros = item.Value
                    .OrderBy(o => o.CodigoISIN.ByteArrayToString())
                    .ToList();

                if (registros.Count > 0)
                {
                    contents.Clear();

                    contents.AppendLine(tplcabec.Replace("[DATAPROC]", DateTime.Now.ToString("dd/MM/yyyy")));

                    contents.AppendLine("       CLIENTE : " + CodCliente + " - " + BuscarNomeCliente(CodCliente).ToUpperInvariant());

                    contents.AppendLine("TP ATIVO     COD.ISIN        COD ATIVO           VALOR/QTDE ATIVO    VLR.VALORIZ.ATIVO       VAL BLOQUEADO    DT.VENCTO   CUSTOD FINALIDADE\r\n");

                    //contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro28>(new RegistrosIMBARQ001.IMBARQ001_Registro28()));

                    string carteiraAnterior = "";
                    decimal total = 0;
                    foreach (RegistrosIMBARQ001.IMBARQ001_Registro18 itemGarantia in registros)
                    {

                        string vcto = String.Empty.PadLeft(10);
                        decimal valorGarantia = itemGarantia.ValorGarantias.ByteArrayToDecimal(2);
                        total += valorGarantia;

                        if (itemGarantia.DataVencimento.ByteArrayToDate("yyyy-MM-dd") != DateTime.MinValue)
                            vcto = itemGarantia.DataVencimento.ByteArrayToDate("yyyy-MM-dd").ToString("dd/MM/yyyy");

                        contents.AppendFormat("{0} {1} {2} {3} {4} {5} {6} {7} {8}\r\n"
                            , ((IMBARQ_CollateralType)itemGarantia.CodigoTipoGarantia.ByteArrayToInt32()).ToString().PadRight(20).Substring(0,10).Replace("_", " ")
                            , itemGarantia.CodigoISIN.ByteArrayToString()
                            , itemGarantia.IdentificadorGarantias.ByteArrayToString().PadRight(20)
                            , itemGarantia.QtdeAceita.ByteArrayToDecimal(10).ToString("#,##0", ciBr).PadLeft(20)
                            , valorGarantia.ToString("#,##0.00", ciBr).PadLeft(20)
                            , itemGarantia.ValorBloqueadoRetirada.ByteArrayToDecimal(10).ToString("#,##0.00", ciBr).PadLeft(19)
                            , vcto.PadLeft(12)
                            , itemGarantia.CodigoCustodiante.ByteArrayToString().Trim().PadLeft(8)
                            , ((IMBARQ_CollateralCoverageType)itemGarantia.FinalidadeCobertura.ByteArrayToInt32()).ToString().PadRight(20).Substring(0, 10).Replace("_", " ")
                            );
                    }

                    contents.AppendLine("                                                                     -----------------                                                        ");
                    contents.AppendFormat("                             TOTAIS:                               {0}", total.ToString("#,##0.00", ciBr).PadLeft(19));

                    if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                    {
                        Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                    }
                    string imbarqCliente = string.Format(@"{0}\{1}\GARANTIAS-{2}.txt", dirSplitted, CodCliente, CodCliente);

                    File.WriteAllText(imbarqCliente, contents.ToString());
                }
            }

            return false;
        }

        private bool convertColdPosicaoVista()
        {
            string dirSplitted;

            if (ConfigurationManager.AppSettings["DirIMBARQSplitted"] != null)
            {
                dirSplitted = ConfigurationManager.AppSettings["DirIMBARQSplitted"].ToString();

                if (!Directory.Exists(dirSplitted))
                {
                    Directory.CreateDirectory(dirSplitted);
                }
            }
            else
            {
                logger.Fatal("AppSetting 'DirIMBARQSplitted' nao definido, abortando");
                return false;
            }

            string tplcabec = File.ReadAllText(ConfigurationManager.AppSettings["TemplateCabecalhoPosicaoVista"].ToString());

            StringBuilder contents = new StringBuilder();


            foreach (KeyValuePair<int, List<RegistrosIMBARQ001.IMBARQ001_Registro02>> item in dctColdPosVista)
            {
                int CodCliente = item.Key;

                List<RegistrosIMBARQ001.IMBARQ001_Registro02> registros = item.Value
                    .OrderBy(o => o.CodigoISIN.ByteArrayToString())
                    .ToList();

                if (registros.Count > 0)
                {
                    contents.Clear();

                    contents.AppendLine(tplcabec.Replace("[DATAPROC]", DateTime.Now.ToString("dd/MM/yyyy")));

                    contents.AppendLine("       CLIENTE : " + CodCliente + " - " + BuscarNomeCliente(CodCliente).ToUpperInvariant());

                    contents.AppendLine("                                                                                                                                                                                    ----------- GARANTIAS ---------");
                    contents.AppendLine("PAPEL       ISIN        DIST.    DT.PREGAO DT.LIQUID. CARTEIRA      COMPRAS  PRC.MED.COMPRA          VL. COMPRADO          VENDAS   PRC.MED.VENDA           VOL.VENDIDO             COBERTURA                MARGEM");

                    foreach (RegistrosIMBARQ001.IMBARQ001_Registro02 itemPosicao in registros)
                    {

                        string liquid = String.Empty.PadLeft(10);
                        string dtpregao = String.Empty.PadLeft(10);

                        if (itemPosicao.DataLiquidacao.ByteArrayToDate("yyyy-MM-dd") != DateTime.MinValue)
                            liquid = itemPosicao.DataLiquidacao.ByteArrayToDate("yyyy-MM-dd").ToString("dd/MM/yyyy");

                        if (itemPosicao.DataPregao.ByteArrayToDate("yyyy-MM-dd") != DateTime.MinValue)
                            dtpregao = itemPosicao.DataPregao.ByteArrayToDate("yyyy-MM-dd").ToString("dd/MM/yyyy");

                        contents.AppendFormat("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14}\r\n"
                            , itemPosicao.CodigoNegociacao.ByteArrayToString().Trim().PadRight(10)
                            , itemPosicao.CodigoISIN.ByteArrayToString()
                            , itemPosicao.Distribuicao.ByteArrayToInt32().ToString("000")
                            , itemPosicao.Mercado.ByteArrayToInt32().ToString("000")
                            , dtpregao
                            , liquid
                            , itemPosicao.CodigoCarteira.ByteArrayToString().PadRight(10).Substring(0,5)
                            , itemPosicao.QtdeComprada.ByteArrayToDecimal(0).ToString("#,##0", ciBr).PadLeft(15)
                            , itemPosicao.PrecoMedioCompra.ByteArrayToDecimal(7).ToString("#,##0.00", ciBr).PadLeft(15)
                            , itemPosicao.VolumeFinanceiroComprado.ByteArrayToDecimal(2).ToString("#,##0.00", ciBr).PadLeft(21)
                            , itemPosicao.QtdeVendida.ByteArrayToDecimal(0).ToString("#,##0", ciBr).PadLeft(15)
                            , itemPosicao.PrecoMedioVenda.ByteArrayToDecimal(7).ToString("#,##0.00", ciBr).PadLeft(15)
                            , itemPosicao.VolumeFinanceiroVendido.ByteArrayToDecimal(2).ToString("#,##0.00", ciBr).PadLeft(21)
                            , itemPosicao.PosicaoCobertaVendida.ByteArrayToDecimal(7).ToString("#,##0", ciBr).PadLeft(21)
                            , itemPosicao.PosicaoDescobertaVendida.ByteArrayToDecimal(7).ToString("#,##0", ciBr).PadLeft(21)
                            );
                    }

                    if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                    {
                        Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                    }
                    string imbarqCliente = string.Format(@"{0}\{1}\POSVISTA-{2}.txt", dirSplitted, CodCliente, CodCliente);

                    File.WriteAllText(imbarqCliente, contents.ToString());
                }
            }

            return false;
        }

        private bool convertColdOpcoes()
        {
            string dirSplitted;

            if (ConfigurationManager.AppSettings["DirIMBARQSplitted"] != null)
            {
                dirSplitted = ConfigurationManager.AppSettings["DirIMBARQSplitted"].ToString();

                if (!Directory.Exists(dirSplitted))
                {
                    Directory.CreateDirectory(dirSplitted);
                }
            }
            else
            {
                logger.Fatal("AppSetting 'DirIMBARQSplitted' nao definido, abortando");
                return false;
            }

            string tplcabec = File.ReadAllText(ConfigurationManager.AppSettings["TemplateCabecalhoOpcoes"].ToString());

            StringBuilder contents = new StringBuilder();


            foreach (KeyValuePair<int, List<RegistrosIMBARQ001.IMBARQ001_Registro01>> item in dctColdOpcoes)
            {
                int CodCliente = item.Key;

                List<RegistrosIMBARQ001.IMBARQ001_Registro01> registros = item.Value
                    .OrderBy(o => o.CodigoISIN.ByteArrayToString())
                    .ToList();

                if (registros.Count > 0)
                {
                    contents.Clear();

                    contents.AppendLine(tplcabec.Replace("[DATAPROC]", DateTime.Now.ToString("dd/MM/yyyy")));

                    contents.AppendLine("       CLIENTE : " + CodCliente + " - " + BuscarNomeCliente(CodCliente).ToUpperInvariant());

                    contents.AppendLine("--------------- S E R I E ----------------                                                                                                     ");
                    contents.AppendLine("PAPEL        CUP   PR.EXER        DATA.VENC. DIS COD.NEGOC.                                             -------GARANTIAS-------              ");   
                    contents.AppendLine("POS.INICIAL             COMPRAS          VENDAS      EXERCICIOS    TRANSF/ALTER       POS.ATUAL       COBERTURA          MARGEM             POP");

                    //contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro28>(new RegistrosIMBARQ001.IMBARQ001_Registro28()));

                    string carteiraAnterior = "";
                    foreach (RegistrosIMBARQ001.IMBARQ001_Registro01 itemOpcao in registros)
                    {

                        string vcto = String.Empty.PadLeft(10);

                        if (itemOpcao.DataVencimento.ByteArrayToDate("yyyy-MM-dd") != DateTime.MinValue)
                            vcto = itemOpcao.DataVencimento.ByteArrayToDate("yyyy-MM-dd").ToString("dd/MM/yyyy");

                        contents.AppendFormat("{0} {1} {2} {3} {4} {5} {6}\r\n"
                            , itemOpcao.CodigoISIN.ByteArrayToString()
                            , itemOpcao.PrecoExercicio.ByteArrayToDecimal(7).ToString("#,##0.00", ciBr).PadLeft(20)
                            , vcto
                            , itemOpcao.DistribuicaoOpcao.ByteArrayToInt32().ToString("000")
                            , itemOpcao.CodigoNegociacao.ByteArrayToString().Trim().PadRight(10)
                            , itemOpcao.Mercado.ByteArrayToInt32().ToString("000")
                            , itemOpcao.CodBolsaValor.ByteArrayToString().Trim()
                            );

                        decimal sinal = 1;
                        if (((IMBARQ_Side)itemOpcao.NaturezaPosicaoInicial.ByteArrayToString()) == IMBARQ_Side.VENDA)
                            sinal = -1;
                        decimal posInicial = itemOpcao.ValorPosicaoInicial.ByteArrayToDecimal(0) * sinal;

                        sinal = 1;
                        if (((IMBARQ_Side)itemOpcao.NaturezaPosicaoAtual.ByteArrayToString()) == IMBARQ_Side.VENDA)
                            sinal = -1;

                        decimal posAtual = itemOpcao.ValorPosicaoAtual.ByteArrayToDecimal(0) * sinal;

                        contents.AppendFormat("{0} {1} {2} {3} {4} {5} {6} {7} {8}\r\n"
                            , posInicial.ToString("#,##0", ciBr).PadLeft(15)
                            , itemOpcao.QuantidadeCompradaDia.ByteArrayToDecimal(0).ToString("#,##0", ciBr).PadLeft(15)
                            , itemOpcao.QuantidadeVendidaDia.ByteArrayToDecimal(0).ToString("#,##0", ciBr).PadLeft(15)
                            , itemOpcao.ValorPosicaoEncerradaExerc.ByteArrayToDecimal(0).ToString("#,##0", ciBr).PadLeft(15)
                            , itemOpcao.ValorPosicaoEnviadaTransfer.ByteArrayToDecimal(0).ToString("#,##0", ciBr).PadLeft(15)
                            , posAtual.ToString("#,##0", ciBr).PadLeft(15)
                            , itemOpcao.ValorPosicaoCobertaVendida.ByteArrayToDecimal(0).ToString("#,##0", ciBr).PadLeft(15)
                            , itemOpcao.ValorPosicaoDescobertaVendida.ByteArrayToDecimal(0).ToString("#,##0", ciBr).PadLeft(15)
                            , itemOpcao.ValorPremioOpcao.ByteArrayToDecimal(2).ToString("#,##0", ciBr).PadLeft(15)
                            );
                    }

                    if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                    {
                        Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                    }
                    string imbarqCliente = string.Format(@"{0}\{1}\OPCOES-{2}.txt", dirSplitted, CodCliente, CodCliente);

                    File.WriteAllText(imbarqCliente, contents.ToString());
                }
            }

            return false;
        }


        private bool convertColdBTC()
        {
            string dirSplitted;

            if (ConfigurationManager.AppSettings["DirIMBARQSplitted"] != null)
            {
                dirSplitted = ConfigurationManager.AppSettings["DirIMBARQSplitted"].ToString();

                if (!Directory.Exists(dirSplitted))
                {
                    Directory.CreateDirectory(dirSplitted);
                }
            }
            else
            {
                logger.Fatal("AppSetting 'DirIMBARQSplitted' nao definido, abortando");
                return false;
            }

            string tplcabec = File.ReadAllText(ConfigurationManager.AppSettings["TemplateCabecalhoBTC"].ToString());

            StringBuilder contents = new StringBuilder();
            StringBuilder contBackoffice = new StringBuilder();


            foreach (KeyValuePair<int, List<RegistrosIMBARQ001.IMBARQ001_Registro06>> item in dctColdBTC)
            {
                int CodCliente = item.Key;

                List<RegistrosIMBARQ001.IMBARQ001_Registro06> registros = item.Value
                    .OrderBy(o => o.IsinAtivoObjeto.ByteArrayToString())
                    .ToList();

                if (registros.Count > 0)
                {
                    contents.Clear();

                    contents.AppendLine(tplcabec.Replace("[DATAPROC]", DateTime.Now.ToString("dd/MM/yyyy")));

                    contents.AppendLine("       CLIENTE : " + CodCliente + " - " + BuscarNomeCliente(CodCliente).ToUpperInvariant());

                    contents.AppendLine("=================================================================================================================================================================");
                    contents.AppendLine("D/T Cod.Neg. Dist. Contrato            Dt.Regist. Liq.Ant. Liq.Ant. Dt.Vcto    Dt.Carência Renov.    Quantidade    Cot.Refer   Taxa (%)  Contraparte");

                    //contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ001.IMBARQ001_Registro28>(new RegistrosIMBARQ001.IMBARQ001_Registro28()));

                    string carteiraAnterior = "";
                    foreach (RegistrosIMBARQ001.IMBARQ001_Registro06 itemBTC in registros)
                    {

                        string dout = ((IMBARQ_Side)itemBTC.NaturezaDoadorVendedor.ByteArrayToString()).ToString().Substring(0, 1).PadRight(3);
                        string regto = String.Empty.PadLeft(10);
                        string carencia = String.Empty.PadLeft(10);
                        string vcto = String.Empty.PadLeft(10);

                        if (itemBTC.DataNegociacao.ByteArrayToDate("yyyy-MM-dd") != DateTime.MinValue)
                            regto = itemBTC.DataNegociacao.ByteArrayToDate("yyyy-MM-dd").ToString("dd/MM/yyyy");

                        if (itemBTC.DataCarencia.ByteArrayToDate("yyyy-MM-dd") != DateTime.MinValue)
                            carencia = itemBTC.DataCarencia.ByteArrayToDate("yyyy-MM-dd").ToString("dd/MM/yyyy");

                        if (itemBTC.DataVencimento.ByteArrayToDate("yyyy-MM-dd") != DateTime.MinValue)
                            vcto = itemBTC.DataVencimento.ByteArrayToDate("yyyy-MM-dd").ToString("dd/MM/yyyy");

                        string renov = " SIM ";
                        if (String.IsNullOrEmpty(itemBTC.NumeroContratoAnterior.ByteArrayToString().Trim()))
                            renov = " NAO ";

                        decimal taxa = itemBTC.TaxaDoadoraTomadora.ByteArrayToDecimal(7) * 100;


                        contents.AppendFormat("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} \r\n"
                            , dout
                            , itemBTC.CodigoNegociacao.ByteArrayToString().Trim().PadRight(8)
                            , itemBTC.DistribuicaoAtivoObjeto.ByteArrayToInt32().ToString("000")
                            , itemBTC.NumeroContrato.ByteArrayToString().Trim().PadRight(21)
                            , regto
                            , (itemBTC.IndicadorLiquidacaoAntecipada.ByteArrayToString().Trim().Equals("1")?"SIM":"NAO").PadRight(8)
                            , (itemBTC.IndicadorLiquidacaoAntecipadaOPA.ByteArrayToString().Trim().Equals("1") ? "SIM" : "NAO").PadRight(8)
                            , vcto
                            , carencia
                            , renov
                            , itemBTC.QtdeAtual.ByteArrayToDecimal(0).ToString("#,##0", ciBr).PadLeft(15)
                            , itemBTC.PrecoReferenciaAtivoObjeto.ByteArrayToDecimal(7).ToString("#,##0.00", ciBr).PadLeft(10)
                            , taxa.ToString("##0.00000", ciBr).PadLeft(10)
                            );
                    }

                    if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                    {
                        Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                    }
                    string imbarqCliente = string.Format(@"{0}\{1}\BTC-{2}.txt", dirSplitted, CodCliente, CodCliente);

                    File.WriteAllText(imbarqCliente, contents.ToString());

                    contBackoffice.Append(contents.ToString());
                    contBackoffice.AppendLine("-".PadLeft(200, '-'));
                    contBackoffice.AppendLine("\r\n");
                }
            }

            if (!Directory.Exists(dirSplitted + "\\Backoffice"))
            {
                Directory.CreateDirectory(dirSplitted + "\\Backoffice");
            }
            string custBackoffice = string.Format(@"{0}\Backoffice\BTC-Backoffice.txt", dirSplitted);

            File.WriteAllText(custBackoffice, contBackoffice.ToString());

            return false;
        }

        private bool convertColdLiquidacoes()
        {
            string dirSplitted;

            if (ConfigurationManager.AppSettings["DirIMBARQSplitted"] != null)
            {
                dirSplitted = ConfigurationManager.AppSettings["DirIMBARQSplitted"].ToString();

                if (!Directory.Exists(dirSplitted))
                {
                    Directory.CreateDirectory(dirSplitted);
                }
            }
            else
            {
                logger.Fatal("AppSetting 'DirIMBARQSplitted' nao definido, abortando");
                return false;
            }

            string tplcabec = File.ReadAllText(ConfigurationManager.AppSettings["TemplateCabecalhoLiquidacoes"].ToString());

            StringBuilder contents = new StringBuilder();


            foreach (KeyValuePair<int, List<RegistrosIMBARQ001.IMBARQ001_Registro14>> item in dctColdLiquidacoes)
            {
                int CodCliente = item.Key;

                List<RegistrosIMBARQ001.IMBARQ001_Registro14> registros = item.Value
                    .OrderByDescending(o => o.DiasUteisAbertContratoLiquidacao.ByteArrayToInt32())
                    .ToList();

                if (registros.Count > 0)
                {
                    contents.Clear();

                    contents.AppendLine(tplcabec.Replace("[DATAPROC]", DateTime.Now.ToString("dd/MM/yyyy")));

                    contents.AppendLine("       CLIENTE : " + CodCliente + " - " + BuscarNomeCliente(CodCliente).ToUpperInvariant());

                    contents.AppendLine("D/T Contrato             Ação            Quantidade     Preço   Dias Taxa %AA       Débito(taxa)   Taxa CBLC        Bruto           IRRF           Crédito ");

                    string carteiraAnterior = "";
                    decimal totalDebito = 0;
                    decimal totalTaxaCBLC = 0;
                    decimal totalBruto = 0;
                    decimal totalImposto = 0;
                    decimal totalCredito = 0;

                    foreach (RegistrosIMBARQ001.IMBARQ001_Registro14 itemLiq in registros)
                    {
                        string dout = ((IMBARQ_Side) itemLiq.NaturezaPosicao.ByteArrayToString()).ToString().Substring(0, 1).PadRight(3);

                        string vcto = String.Empty.PadLeft(10);

                        if (itemLiq.DataVencimento.ByteArrayToDate("yyyy-MM-dd") != DateTime.MinValue)
                            vcto = itemLiq.DataVencimento.ByteArrayToDate("yyyy-MM-dd").ToString("dd/MM/yyyy");

                        decimal debito = 0;
                        decimal credito = 0;
                        if (dout.Substring(0, 1).Equals("T"))
                            debito = itemLiq.ValorPagoPeloParaInvestidor.ByteArrayToDecimal(7);
                        else
                            credito = itemLiq.ValorPagoPeloParaInvestidor.ByteArrayToDecimal(7);

                        decimal bruto = itemLiq.ValorBrutoDoador.ByteArrayToDecimal(7);
                        decimal imposto =itemLiq.ImpostoSobreValorBruto.ByteArrayToDecimal(7);
                        decimal taxaCBLC =itemLiq.TaxaDoador.ByteArrayToDecimal(7) * 100;
                        decimal taxa = itemLiq.Taxa.ByteArrayToDecimal(7) * 100;

                        totalDebito += debito;
                        totalCredito += credito;
                        totalTaxaCBLC += taxaCBLC;
                        totalBruto += bruto;
                        totalImposto += imposto;

                        contents.AppendFormat("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}\r\n"
                            , dout
                            , itemLiq.NumeroContrato.ByteArrayToString().Trim().PadRight(20)
                            , itemLiq.CodigoNegociacaoAtivoObjeto.ByteArrayToString().Trim().PadRight(12)
                            , itemLiq.QuantidadeOriginal.ByteArrayToDecimal(6).ToString("#,##0", ciBr).PadLeft(15)
                            , itemLiq.PrecoReferenciaAtivoObjeto.ByteArrayToDecimal(7).ToString("#,##0.00", ciBr).PadLeft(7)
                            , itemLiq.DiasUteisAbertContratoLiquidacao.ByteArrayToInt32().ToString().PadLeft(6)
                            , taxa.ToString("#,##0.00", ciBr)
                            , (debito != Decimal.Zero) ? debito.ToString("#,##0.00", ciBr).PadLeft(15) : " ".PadLeft(15)
                            , (taxaCBLC != Decimal.Zero) ? taxaCBLC.ToString("#,##0.00", ciBr).PadLeft(15) : " ".PadLeft(15)
                            , (bruto != Decimal.Zero) ? bruto.ToString("#,##0.00", ciBr).PadLeft(15) : " ".PadLeft(15)
                            , (imposto != Decimal.Zero) ? imposto.ToString("#,##0.00", ciBr).PadLeft(15) : " ".PadLeft(15)
                            , (credito != Decimal.Zero) ? credito.ToString("#,##0.00", ciBr).PadLeft(15) : " ".PadLeft(15)
                            );
                    }

                    contents.AppendLine(" ".PadLeft(80));
                    contents.AppendFormat("             Totais do Investidor                                         {0}  {1} {2} {3} {4}\r\n",
                        totalDebito.ToString("#,##0.00", ciBr).PadLeft(15),
                        totalTaxaCBLC.ToString("#,##0.00", ciBr).PadLeft(15),
                        totalBruto.ToString("#,##0.00", ciBr).PadLeft(15),
                        totalImposto.ToString("#,##0.00", ciBr).PadLeft(15),
                        totalCredito.ToString("#,##0.00", ciBr).PadLeft(15)
                        );

                    contents.AppendFormat("             Saldo                                                                                                                         {0}",
                        (totalCredito - totalDebito - totalTaxaCBLC - totalImposto).ToString("#,##0.00", ciBr).PadLeft(15)
                        );

                    if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                    {
                        Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                    }
                    string imbarqCliente = string.Format(@"{0}\{1}\LIQUIDACOES-{2}.txt", dirSplitted, CodCliente, CodCliente);

                    File.WriteAllText(imbarqCliente, contents.ToString());
                }
            }

            return false;
        }


        private bool convertColdExigencias()
        {
            string dirSplitted;

            if (ConfigurationManager.AppSettings["DirIMBARQSplitted"] != null)
            {
                dirSplitted = ConfigurationManager.AppSettings["DirIMBARQSplitted"].ToString();

                if (!Directory.Exists(dirSplitted))
                {
                    Directory.CreateDirectory(dirSplitted);
                }
            }
            else
            {
                logger.Fatal("AppSetting 'DirIMBARQSplitted' nao definido, abortando");
                return false;
            }

            string tplcabec = File.ReadAllText(ConfigurationManager.AppSettings["TemplateCabecalhoExigencias"].ToString());

            StringBuilder contents = new StringBuilder();


            foreach (KeyValuePair<int, RegistrosIMBARQ001.IMBARQ001_Registro19> item in dctColdExigencias)
            {
                int CodCliente = item.Key;

                RegistrosIMBARQ001.IMBARQ001_Registro19 registro = item.Value;

                contents.Clear();

                contents.AppendLine(tplcabec.Replace("[DATAPROC]", DateTime.Now.ToString("dd/MM/yyyy")));

                contents.AppendLine("       CLIENTE : " + CodCliente + " - " + BuscarNomeCliente(CodCliente).ToUpperInvariant() + "\r\n");


                decimal sinal = registro.SinalRiscoSemGarantias.ByteArrayToString().Equals("-")?-1M:1M;

                decimal riscoSemGarantias = registro.RiscoSemGarantias.ByteArrayToDecimal(2) * sinal;
                decimal chamadaMargemInicial = registro.ChamadaMargemInicial.ByteArrayToDecimal(2);
                decimal valorMargemAdicional = registro.ValorMargemAdicional.ByteArrayToDecimal(7);
                decimal valorTotalGarantias = registro.ValorTotalGarantias.ByteArrayToDecimal(2);
                decimal perdasPermanentes = registro.PerdasPermanentes.ByteArrayToDecimal(7);
                decimal perdasTransitorias = registro.PerdasTransitorias.ByteArrayToDecimal(7);

                sinal = registro.SinalSaldoGarantias.ByteArrayToString().Equals("-")?-1M:1M;
                decimal saldoGarantias = registro.SaldoGarantias.ByteArrayToDecimal(2) * sinal;

                contents.AppendFormat("                   RISCO SEM GARANTIAS ........: {0}\r\n", riscoSemGarantias.ToString("#,##0.00", ciBr).PadLeft(19));
                contents.AppendFormat("                   CHAMADA DE MARGEM INICIAL ..: {0}\r\n", chamadaMargemInicial.ToString("#,##0.00", ciBr).PadLeft(19));
                contents.AppendFormat("                   VALOR DA MARGEM ADICIONAL ..: {0}\r\n", valorMargemAdicional.ToString("#,##0.00", ciBr).PadLeft(19));
                contents.AppendFormat("                   VALOR TOTAL DAS GARANTIAS ..: {0}\r\n", valorTotalGarantias.ToString("#,##0.00", ciBr).PadLeft(19));
                contents.AppendFormat("                   PERDAS PERMANENTES .........: {0}\r\n", perdasPermanentes.ToString("#,##0.00", ciBr).PadLeft(19));
                contents.AppendFormat("                   PERDAS TRANSITÓRIAS ........: {0}\r\n", perdasTransitorias.ToString("#,##0.00", ciBr).PadLeft(19));
                contents.AppendFormat("                   SALDO DE GARANTIAS .........: {0}\r\n", saldoGarantias.ToString("#,##0.00", ciBr).PadLeft(19));


                if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                {
                    Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                }

                string imbarqCliente = string.Format(@"{0}\{1}\EXIGENCIA-{2}.txt", dirSplitted, CodCliente, CodCliente);

                File.WriteAllText(imbarqCliente, contents.ToString());
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool ProcessIMBARQ002(string filename)
        {
            try
            {
                string dirSplitted = null;
                string dirBackup = null;

                if (ConfigurationManager.AppSettings["DirIMBARQSplitted"] != null)
                {
                    dirSplitted = ConfigurationManager.AppSettings["DirIMBARQSplitted"].ToString();

                    if (!Directory.Exists(dirSplitted))
                    {
                        Directory.CreateDirectory(dirSplitted);
                    }
                }
                else
                {
                    logger.Fatal("AppSetting 'DirIMBARQSplitted' nao definido, abortando");
                    return false;
                }


                if (ConfigurationManager.AppSettings["DirIMBARQBackup"] != null)
                {
                    dirBackup = ConfigurationManager.AppSettings["DirIMBARQBackup"].ToString();

                    if (!Directory.Exists(dirBackup))
                    {
                        Directory.CreateDirectory(dirBackup);
                    }
                }
                else
                {
                    logger.Fatal("AppSetting 'DirIMBARQBackup' nao definido, abortando");
                    return false;
                }

                //filename = @"C:\Users\apiza\Documents\Manuais\BMF-Bovespa\IPN\IMBARQ002_BV000272201709120000001000227130658.txt";

                string[] allLines = File.ReadAllLines(filename);
                bool cabecalholido = false;

                string cabecalho = "";
                string body = "";
                string clienteGradual = "";
                string cliente = "";
                string clienteOrig = "";
                string clienteTmp = "";

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);

                //DBClientesCOLD db = new DBClientesCOLD();
                //Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();

                RegistrosIMBARQ002.IMBARQ002_Trailler trailler = new RegistrosIMBARQ002.IMBARQ002_Trailler();
                RegistrosIMBARQ002.IMBARQ002_Header header = new RegistrosIMBARQ002.IMBARQ002_Header();

                dctRegs0002.Clear();
                dctLiquidacoes.Clear();

                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ');

                    if (!String.IsNullOrEmpty(line))
                    {
                        string tipoRegistro = line.Substring(0, 2);
                        int CodCliente = -1;
                        string linhasLiquidacoes = null;

                        switch (tipoRegistro)
                        {
                            case "00":
                                {
                                    header = FromStringBlock<RegistrosIMBARQ002.IMBARQ002_Header>(line);
                                }
                                break; // Header
                            case "50":
                                {
                                    RegistrosIMBARQ002.IMBARQ002_Registro50 reg = FromStringBlock<RegistrosIMBARQ002.IMBARQ002_Registro50>(line);
                                    CodCliente = reg.CodInvestidorSolicitante.ByteArrayToInt32();
                                    reg.CodParticipanteSolicitado.FillWithByte(0x30);
                                    reg.CodInvestidorSolicitado.FillWithByte(0x30);
                                    line = BlockToString<RegistrosIMBARQ002.IMBARQ002_Registro50>(reg);
                                }
                                break; // Posicao
                            case "99":
                                {
                                    trailler = FromStringBlock<RegistrosIMBARQ002.IMBARQ002_Trailler>(line);
                                }
                                break; // Trailler
                        }

                        if (CodCliente != -1)
                        {
                            if (!dctRegs0002.ContainsKey(CodCliente))
                            {
                                dctRegs0002.Add(CodCliente, new List<string>());
                            }

                            if (!dctLiquidacoes.ContainsKey(CodCliente))
                            {
                                dctLiquidacoes.Add(CodCliente, new List<string>());
                            }

                            dctRegs0002[CodCliente].Add(line);

                            if (!String.IsNullOrEmpty(linhasLiquidacoes))
                                dctLiquidacoes[CodCliente].Add(linhasLiquidacoes);
                        }

                    }
                }

                StringBuilder contents = new StringBuilder();

                foreach (KeyValuePair<int, List<string>> item in dctRegs0002)
                {
                    List<string> lines = item.Value;
                    int CodCliente = item.Key;

                    contents.Clear();
                    contents.AppendLine(BlockToString<RegistrosIMBARQ002.IMBARQ002_Header>(header));

                    for (int i = 0; i < lines.Count; i++)
                        contents.AppendLine(lines[i]);

                    trailler.TotalRegistros = ASCIIEncoding.ASCII.GetBytes(string.Format("{0:000000000}", lines.Count));
                    contents.AppendLine(BlockToString<RegistrosIMBARQ002.IMBARQ002_Trailler>(trailler));

                    if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                    {
                        Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                    }

                    string imbarqCliente = string.Format(@"{0}\{1}\IMBARQ002-{2}.txt", dirSplitted, CodCliente, CodCliente);

                    File.WriteAllText(imbarqCliente, contents.ToString());
                }

                foreach (KeyValuePair<int, List<string>> item in dctLiquidacoes)
                {
                    List<string> lines = item.Value;
                    int CodCliente = item.Key;

                    if (lines.Count > 0)
                    {
                        contents.Clear();
                        contents.AppendLine(GetHeaderLabels<RegistrosIMBARQ002.IMBARQ002_Registro50>(new RegistrosIMBARQ002.IMBARQ002_Registro50()));

                        for (int i = 0; i < lines.Count; i++)
                            contents.AppendLine(lines[i]);

                        if (!Directory.Exists(dirSplitted + "\\" + CodCliente))
                        {
                            Directory.CreateDirectory(dirSplitted + "\\" + CodCliente);
                        }
                        string imbarqCliente = string.Format(@"{0}\{1}\Liquidacoes-{2}.csv", dirSplitted, CodCliente, CodCliente);

                        File.WriteAllText(imbarqCliente, contents.ToString());
                    }
                }

                FileInfo finfo = new FileInfo(filename);

                string imbarqBkp = string.Format(@"{0}\{1}", dirBackup, finfo.Name);

                if (File.Exists(imbarqBkp))
                    File.Delete(imbarqBkp);
                File.Move(filename, imbarqBkp);

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("ProcessIMBARQ002: " + ex.Message, ex);
            }

            return false;
        }

        #region BlockReaders

        #region Distribuidor
        public void SendIMBARQFiles()
        {
            bool bShouldSendReport = false;

            StringBuilder relatorioEnvio = new StringBuilder();

            relatorioEnvio.AppendLine("Relatorio de Envio Arquivos IMBARQ " + dataproc.ToString("dd/MM/yyyy HH:mm:ss"));

            try
            {
                string diretorioSaida;
                string diretorioBkp;


                string dirSplitted = null;

                if (ConfigurationManager.AppSettings["DirIMBARQSplitted"] != null)
                {
                    dirSplitted = ConfigurationManager.AppSettings["DirIMBARQSplitted"].ToString();
                }

                if (!Directory.Exists(dirSplitted))
                {
                    Directory.CreateDirectory(dirSplitted);
                }

                List<int> lstBkpGrupo = new List<int>();

                if (ConfigurationManager.AppSettings["DirArquivosCold"] == null)
                {
                    logger.Fatal("AppSetting DirArquivosCold deve ser definido");
                    return;
                }

                diretorioSaida = ConfigurationManager.AppSettings["DirArquivosCold"].ToString();

                if (ConfigurationManager.AppSettings["DirArquivosBackupCold"] == null)
                {
                    logger.Fatal("AppSetting DirArquivosBackupCold deve ser definido");
                    return;
                }

                diretorioBkp = ConfigurationManager.AppSettings["DirArquivosBackupCold"].ToString();

                if (!Directory.Exists(diretorioSaida))
                {
                    Directory.CreateDirectory(diretorioSaida);
                }

                if (!Directory.Exists(diretorioBkp))
                {
                    Directory.CreateDirectory(diretorioBkp);
                }

                DBClientesCOLD db = new DBClientesCOLD();

                Dictionary<int, STGrupoRelatCold> dicGruposCold = db.ObterListaGruposCOLD();

                foreach (STGrupoRelatCold grupoEmail in dicGruposCold.Values)
                {
                    //if (grupoEmail.IDGrupo < 176)
                    //  continue;

                    // So tenta mandar email se algum arquivo foi gerado para o grupo
                    bool bSendEmail = false;
                    StringBuilder relatorioGrupo = new StringBuilder();

                    if (!grupoEmail.FlagAnexo && (grupoEmail.FlagPdf || grupoEmail.FlagZip))
                    {
                        logger.ErrorFormat("Erro na configuracao do grupo Anexo: {0} PDF {1} Zip {2} conflitantes",
                            grupoEmail.FlagAnexo, grupoEmail.FlagPdf, grupoEmail.FlagZip);
                        continue;
                    }

                    // Criar diretorio para geracao dos arquivos e/ou limpar
                    // 
                    string diretGrupo = String.Format(@"{0}\Grupo-{1}", diretorioSaida, grupoEmail.IDGrupo);

                    if (Directory.Exists(diretGrupo))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(diretGrupo);
                        foreach (FileInfo file in dirInfo.GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    else
                        Directory.CreateDirectory(diretGrupo);

                    // obtem as contas referentes ao grupo
                    Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD(grupoEmail.IDGrupo);

                    relatorioGrupo.AppendLine(Environment.NewLine + Environment.NewLine + "*** Grupo [" + grupoEmail.IDGrupo + "]");

                    string[] emails = grupoEmail.EmailsTO.Split();

                    if (emails != null && emails.Length > 0)
                    {
                        for (int i = 0; i < emails.Length; i++)
                        {
                            if (i == 0)
                                relatorioGrupo.AppendLine(Environment.NewLine + " To: " + emails[i]);
                            else
                                relatorioGrupo.AppendLine("     " + emails[i]);
                        }
                    }

                    emails = grupoEmail.EmailsCC.Split();

                    if (emails != null && emails.Length > 0)
                    {
                        for (int i = 0; i < emails.Length; i++)
                        {
                            if (i == 0)
                                relatorioGrupo.AppendLine(" CC: " + emails[i]);
                            else
                                relatorioGrupo.AppendLine("     " + emails[i]);
                        }
                    }

                    emails = grupoEmail.EmailsBCC.Split();

                    if (emails != null && emails.Length > 0)
                    {
                        for (int i = 0; i < emails.Length; i++)
                        {
                            if (i == 0)
                                relatorioGrupo.AppendLine(" BCC: " + emails[i]);
                            else
                                relatorioGrupo.AppendLine("      " + emails[i]);
                        }
                    }


                    relatorioGrupo.AppendLine(Environment.NewLine + "  ** Contas e relatorios enviados **" + Environment.NewLine);

                    dataproc = DateTime.Now;
                    //dataproc = new DateTime(2016, 01, 21, 8, 0, 0);

                    foreach (STClienteRelatCold cliente in listaClientesCold.Values)
                    {
                        string reportItemCliente = "  Relat. [" + cliente.Account + "]: ";


                        // Gerar IMBARQ001
                        if ( cliente.FlagSendFlatImbarq )
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\IMBARQ001-{2}.txt", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\IMBARQ001-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\IMBARQ001-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, false, false))
                            {
                                bSendEmail = true;
                                reportItemCliente += " IMBARQ001";
                            }
                        }

                        // Gerar IMBARQ002
                        if ( cliente.FlagSendFlatImbarq )
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\IMBARQ002-{2}.txt", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\IMBARQ002-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\IMBARQ002-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, false, false))
                            {
                                bSendEmail = true;
                                reportItemCliente += " IMBARQ002";
                            }
                        }

                        if (cliente.FlagBTC)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\BTC-{2}.csv", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\BTC-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\BTC-{1}-{2}.csv", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (cliente.FlagConvertCold)
                            {
                                arqTxtEntrada = String.Format(@"{0}\{1}\BTC-{2}.txt", dirSplitted, cliente.Account, cliente.Account);
                                pdfSaida = String.Format(@"{0}\BTC-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                                arqTxtSaida = String.Format(@"{0}\BTC-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, false))
                            {
                                bSendEmail = true;
                                reportItemCliente += " BTC";
                            }
                        }

                        if (cliente.FlagExigencia)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\EXIGENCIA-{2}.csv", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\EXIGENCIA-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\EXIGENCIA-{1}-{2}.csv", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (cliente.FlagConvertCold)
                            {
                                arqTxtEntrada = String.Format(@"{0}\{1}\EXIGENCIA-{2}.txt", dirSplitted, cliente.Account, cliente.Account);
                                pdfSaida = String.Format(@"{0}\EXIGENCIA-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                                arqTxtSaida = String.Format(@"{0}\EXIGENCIA-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, false))
                            {
                                bSendEmail = true;
                                reportItemCliente += " EXIG";
                            }
                        }

                        if (cliente.FlagGarantia)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\GARANTIAS-{2}.csv", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\GARANTIAS-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\GARANTIAS-{1}-{2}.csv", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (cliente.FlagConvertCold)
                            {
                                arqTxtEntrada = String.Format(@"{0}\{1}\GARANTIAS-{2}.txt", dirSplitted, cliente.Account, cliente.Account);
                                pdfSaida = String.Format(@"{0}\GARANTIAS-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                                arqTxtSaida = String.Format(@"{0}\GARANTIAS-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, false))
                            {
                                bSendEmail = true;
                                reportItemCliente += " GARANT";
                            }
                        }

                        if (cliente.FlagLiqInvest)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\LIQUIDACOES-{2}.csv", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\LIQUIDACOES-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\LIQUIDACOES-{1}-{2}.csv", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (cliente.FlagConvertCold)
                            {
                                arqTxtEntrada = String.Format(@"{0}\{1}\LIQUIDACOES-{2}.txt", dirSplitted, cliente.Account, cliente.Account);
                                pdfSaida = String.Format(@"{0}\LIQUIDACOES-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                                arqTxtSaida = String.Format(@"{0}\LIQUIDACOES-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, false))
                            {
                                bSendEmail = true;
                                reportItemCliente += " LIQ";
                            }
                        }

                        if (cliente.FlagPosCliente)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\OPCOES-{2}.csv", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\OPCOES-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\OPCOES-{1}-{2}.csv", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (cliente.FlagConvertCold)
                            {
                                arqTxtEntrada = String.Format(@"{0}\{1}\OPCOES-{2}.txt", dirSplitted, cliente.Account, cliente.Account);
                                pdfSaida = String.Format(@"{0}\OPCOES-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                                arqTxtSaida = String.Format(@"{0}\OPCOES-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            }


                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " OPCOES";
                            }

                            arqTxtEntrada = String.Format(@"{0}\{1}\POSVISTA-{2}.csv", dirSplitted, cliente.Account, cliente.Account);
                            pdfSaida = String.Format(@"{0}\POSVISTA-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            arqTxtSaida = String.Format(@"{0}\POSVISTA-{1}-{2}.csv", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (cliente.FlagConvertCold)
                            {
                                arqTxtEntrada = String.Format(@"{0}\{1}\POSVISTA-{2}.txt", dirSplitted, cliente.Account, cliente.Account);
                                pdfSaida = String.Format(@"{0}\POSVISTA-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                                arqTxtSaida = String.Format(@"{0}\POSVISTA-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            }


                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " POSICAOVISTA";
                            }
                        }

                        if (cliente.FlagPosDivBtc)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\DIVIDENDOS-{2}.csv", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\DIVIDENDOS-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\DIVIDENDOS-{1}-{2}.csv", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (cliente.FlagConvertCold)
                            {
                                arqTxtEntrada = String.Format(@"{0}\{1}\DIVIDENDOS-{2}.txt", dirSplitted, cliente.Account, cliente.Account);
                                pdfSaida = String.Format(@"{0}\DIVIDENDOS-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                                arqTxtSaida = String.Format(@"{0}\DIVIDENDOS-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " DIVID";
                            }
                        }

                        if (cliente.FlagTermo)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\TERMO-{2}.csv", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\TERMO-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\TERMO-{1}-{2}.csv", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (cliente.FlagConvertCold)
                            {
                                arqTxtEntrada = String.Format(@"{0}\{1}\TERMO-{2}.txt", dirSplitted, cliente.Account, cliente.Account);
                                pdfSaida = String.Format(@"{0}\TERMO-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                                arqTxtSaida = String.Format(@"{0}\TERMO-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " TERMO";
                            }
                        }

                        if (cliente.FlagCustodia)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\CUSTODIA-{2}.csv", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\CUSTODIA-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\CUSTODIA-{1}-{2}.csv", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (cliente.FlagConvertCold)
                            {
                                arqTxtEntrada = String.Format(@"{0}\{1}\CUSTODIA-{2}.txt", dirSplitted, cliente.Account, cliente.Account);
                                pdfSaida = String.Format(@"{0}\CUSTODIA-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                                arqTxtSaida = String.Format(@"{0}\CUSTODIA-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " CUSTODIA";
                            }
                        }

                        relatorioGrupo.AppendLine(reportItemCliente);
                    } // foreach (STClienteRelatCold cliente in listaClientesCold.Values)

                    bool bEmailSent = false;

                    // Se gerou alguma informacao
                    if (bSendEmail)
                    {
                        if (grupoEmail.FlagAnexo && grupoEmail.FlagZip)
                        {
                            relatorioGrupo.AppendLine(Environment.NewLine + "--- Enviado em anexo ZIP ---");
                            _generateZIP(diretGrupo);
                        }

                        if (grupoEmail.FlagAnexo)
                        {
                            relatorioGrupo.AppendLine(Environment.NewLine + "--- Enviado como arquivos anexos  ---");
                            if (_enviaEmail(grupoEmail, diretGrupo))
                                bEmailSent = true;
                        }

                        if (!grupoEmail.FlagAnexo)
                        {
                            relatorioGrupo.AppendLine(Environment.NewLine + "--- Enviado como texto no corpo do email  ---");

                            if (_embeddedEmail(grupoEmail, diretGrupo))
                                bEmailSent = true;
                        }
                    }

                    // Move o processamento para um diretorio de backup
                    if (bEmailSent)
                    {
                        relatorioEnvio.Append(relatorioGrupo);
                        bShouldSendReport = true;
                        lstBkpGrupo.Add(grupoEmail.IDGrupo);
                    }
                }

                foreach (int grupoID in lstBkpGrupo)
                {
                    string diretGrupo = String.Format(@"{0}\Grupo-{1}", diretorioSaida, grupoID);
                    string diretBkpGrupo = String.Format(@"{0}\Grupo-{1}-{2}",
                        diretorioBkp,
                        grupoID,
                        dataproc.ToString("yyyyMMdd-HHmm"));

                    Directory.Move(diretGrupo, diretBkpGrupo);
                }

                if (bShouldSendReport)
                {
                    logger.Info(Environment.NewLine + relatorioEnvio.ToString() + Environment.NewLine);

                    _enviaRelatorio(relatorioEnvio.ToString());

                    _enviaEmailBackoffice(dirSplitted + "\\Backoffice");
                }

            }
            catch (Exception ex)
            {
                logger.Error("SendColdFiles: " + ex.Message, ex);
            }


        }

        private bool _geraArquivoSaida(string txtEntrada, string txtSaida, string pdfSaida, bool flagPDF, bool flagAppend)
        {
            try
            {
                if (!File.Exists(txtEntrada))
                {
                    logger.Info("_geraArquivoSaida: in [" + txtEntrada + "] nao existe");
                    return false;
                }

                if (flagPDF && !flagAppend)
                {
                    logger.Info("_geraArquivoSaida: in [" + txtEntrada + "] out [" + pdfSaida + "]");

                    Txt2Pdf.ConvertTxt2Pdf(txtEntrada, pdfSaida);
                    File.Delete(txtEntrada);
                }
                else
                {

                    if (flagAppend)
                    {
                        logger.Info("_geraArquivoSaida: in [" + txtEntrada + "] appending to [" + txtSaida + "]");
                        File.AppendAllText(txtSaida, File.ReadAllText(txtEntrada));

                        File.AppendAllText(txtSaida, Environment.NewLine);

                        File.Delete(txtEntrada);

                        if (flagPDF)
                        {
                            if (File.Exists(pdfSaida))
                                File.Delete(pdfSaida);

                            Txt2Pdf.ConvertTxt2Pdf(txtSaida, pdfSaida);
                        }
                    }
                    else
                    {
                        logger.Info("_geraArquivoSaida: in [" + txtEntrada + "] move to [" + txtSaida + "]");
                        File.Move(txtEntrada, txtSaida);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("_geraArquivoSaida: erro ao processar: " + ex.Message, ex);
                return false;
            }

            logger.Info("_geraArquivoSaida: arquivo [" + txtEntrada + "] processado com sucesso");

            return true;
        }


        private bool _embeddedEmail(STGrupoRelatCold grupoEmail, string diretGrupo)
        {
            try
            {
                string[] emailsTo;
                string[] emailsCC;
                string[] emailsBCC;


                if (ConfigurationManager.AppSettings["EmailColdRemetente"] == null)
                {
                    logger.Error("AppSetting 'EmailAlertaRemetente' nao definido. Nao eh possivel enviar alerta");
                    return false;
                }

                char[] seps = { ';', ',' };
                emailsTo = grupoEmail.EmailsTO.Split(seps);
                emailsCC = grupoEmail.EmailsCC.Split(seps);
                emailsBCC = grupoEmail.EmailsBCC.Split(seps);

                var lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailColdRemetente"].ToString(), emailsTo[0]);

                for (int i = 1; i < emailsTo.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsTo[i]))
                        lMensagem.To.Add(emailsTo[i]);
                }

                for (int i = 0; i < emailsCC.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsCC[i]))
                        lMensagem.CC.Add(emailsCC[i]);
                }

                for (int i = 0; i < emailsBCC.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsBCC[i]))
                        lMensagem.Bcc.Add(emailsBCC[i]);
                }

                if (ConfigurationManager.AppSettings["EmailColdBCC"] != null)
                {
                    emailsBCC = ConfigurationManager.AppSettings["EmailColdBCC"].ToString().Split(seps);

                    for (int i = 0; i < emailsBCC.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(emailsBCC[i]))
                            lMensagem.Bcc.Add(emailsBCC[i]);
                    }
                }

                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailColdReplyTo"].ToString()));
                lMensagem.Subject = "RELATORIOS CBLC " + dataproc.ToString("dd/MM/yyyy");
                lMensagem.IsBodyHtml = false;

                DirectoryInfo dirInfo = new DirectoryInfo(diretGrupo);

                List<FileInfo> files = new List<FileInfo>();

                files.AddRange(dirInfo.GetFiles("*.txt"));

                StringBuilder strbuild = new StringBuilder();

                foreach (FileInfo arquivo in files)
                {
                    strbuild.Append(File.ReadAllText(arquivo.FullName));
                    strbuild.AppendLine("." + Environment.NewLine);
                    lMensagem.Body += strbuild.ToString();
                }

                new SmtpClient(ConfigurationManager.AppSettings["EmailColdHost"].ToString()).Send(lMensagem);

                logger.Info("Email enviado com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("EnviarEmail(): " + ex.Message, ex);
                return false;
            }

            return true;
        }

        private bool _enviaEmail(STGrupoRelatCold grupoEmail, string diretGrupo)
        {
            try
            {
                string[] emailsTo;
                string[] emailsCC;
                string[] emailsBCC;


                if (ConfigurationManager.AppSettings["EmailColdRemetente"] == null)
                {
                    logger.Error("AppSetting 'EmailAlertaRemetente' nao definido. Nao eh possivel enviar alerta");
                    return false;
                }

                char[] seps = { ';', ',' };
                emailsTo = grupoEmail.EmailsTO.Split(seps);
                emailsCC = grupoEmail.EmailsCC.Split(seps);
                emailsBCC = grupoEmail.EmailsBCC.Split(seps);

                var lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailColdRemetente"].ToString(), emailsTo[0]);

                for (int i = 1; i < emailsTo.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsTo[i]))
                        lMensagem.To.Add(emailsTo[i]);
                }

                for (int i = 0; i < emailsCC.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsCC[i]))
                        lMensagem.CC.Add(emailsCC[i]);
                }

                for (int i = 0; i < emailsBCC.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsBCC[i]))
                        lMensagem.Bcc.Add(emailsBCC[i]);
                }

                if (ConfigurationManager.AppSettings["EmailColdBCC"] != null)
                {
                    emailsBCC = ConfigurationManager.AppSettings["EmailColdBCC"].ToString().Split(seps);

                    for (int i = 0; i < emailsBCC.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(emailsBCC[i]))
                            lMensagem.Bcc.Add(emailsBCC[i]);
                    }
                }

                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailColdReplyTo"].ToString()));
                lMensagem.Subject = "RELATORIOS IMBARQ " + dataproc.ToString("dd/MM/yyyy");

                DirectoryInfo dirInfo = new DirectoryInfo(diretGrupo);

                List<FileInfo> files = new List<FileInfo>();

                files.AddRange(dirInfo.GetFiles("*.csv"));
                files.AddRange(dirInfo.GetFiles("*.txt"));
                files.AddRange(dirInfo.GetFiles("*.pdf"));
                files.AddRange(dirInfo.GetFiles("*.zip"));

                foreach (FileInfo arquivo in files)
                {
                    lMensagem.Attachments.Add(new Attachment(arquivo.FullName));
                }

                new SmtpClient(ConfigurationManager.AppSettings["EmailColdHost"].ToString()).Send(lMensagem);

                lMensagem.Dispose();

                logger.Info("Email enviado com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("EnviarEmail(): " + ex.Message, ex);
                return false;
            }

            return true;
        }

        private void _generateZIP(string diretGrupo)
        {
            string zipSaida = String.Format(@"{0}\Arquivos.zip", diretGrupo);

            if (!Directory.Exists(diretGrupo))
            {
                logger.Error("Cannot find directory [" + diretGrupo + "]");
                return;
            }

            try
            {
                // Depending on the directory this could be very large and would require more attention
                // in a commercial package.
                List<string> filenames = new List<string>();

                filenames.AddRange(Directory.GetFiles(diretGrupo, "*.csv"));
                filenames.AddRange(Directory.GetFiles(diretGrupo, "*.txt"));
                filenames.AddRange(Directory.GetFiles(diretGrupo, "*.pdf"));

                // 'using' statements guarantee the stream is closed properly which is a big source
                // of problems otherwise.  Its exception safe as well which is great.
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipSaida)))
                {
                    s.UseZip64 = UseZip64.Off;

                    s.SetLevel(9); // 0 - store only to 9 - means best compression

                    byte[] buffer = new byte[4096];

                    foreach (string file in filenames)
                    {

                        // Using GetFileName makes the result compatible with XP
                        // as the resulting path is not absolute.
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));

                        // Setup the entry data as required.

                        // Crc and size are handled by the library for seakable streams
                        // so no need to do them here.

                        // Could also use the last write time or similar for the file.
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);

                        using (FileStream fs = File.OpenRead(file))
                        {

                            // Using a fixed size buffer here makes no noticeable difference for output
                            // but keeps a lid on memory usage.
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
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
                logger.Error("_generateZIP: " + ex.Message, ex);

                // No need to rethrow the exception as for our purposes its handled.
            }
        }

        private bool _enviaRelatorio(string relatorio)
        {
            try
            {
                string[] emailsTo;

                if (ConfigurationManager.AppSettings["EmailColdRemetente"] == null)
                {
                    logger.Error("AppSetting 'EmailAlertaRemetente' nao definido. Nao eh possivel enviar relatorio de envios");
                    return false;
                }

                if (ConfigurationManager.AppSettings["EmailRelatorioEnviosCold"] == null)
                {
                    logger.Fatal("AppSetting 'EmailRelatorioEnviosCold' deve ser definido");
                    return false;
                }

                char[] seps = { ';', ',' };
                emailsTo = ConfigurationManager.AppSettings["EmailRelatorioEnviosCold"].ToString().Split(seps);

                var lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailColdRemetente"].ToString(), emailsTo[0]);

                for (int i = 1; i < emailsTo.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsTo[i]))
                        lMensagem.To.Add(emailsTo[i]);
                }

                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailColdReplyTo"].ToString()));
                lMensagem.Subject = "Relatorio de Envio Arquivos IMBARQ " + dataproc.ToString("dd/MM/yyyy");
                lMensagem.IsBodyHtml = true;
                lMensagem.Body = "<html><body style=\"font-family:courier;\">" + relatorio.Replace(" ", "&nbsp;").Replace(Environment.NewLine, "<br>" + Environment.NewLine) + "</body></html>";

                new SmtpClient(ConfigurationManager.AppSettings["EmailColdHost"].ToString()).Send(lMensagem);

                lMensagem.Dispose();

                logger.Info("Email de relatorio de envios submetido com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("_enviaRelatorio(): " + ex.Message, ex);
                return false;
            }

            return true;
        }

        private bool _enviaEmailBackoffice(string diretGrupo)
        {
            try
            {
                string[] emailsTo;
                string[] emailsCC;
                string[] emailsBCC;


                if (ConfigurationManager.AppSettings["EmailColdRemetente"] == null)
                {
                    logger.Error("AppSetting 'EmailAlertaRemetente' nao definido. Nao eh possivel enviar alerta");
                    return false;
                }

                if (ConfigurationManager.AppSettings["EmailIMBARQBackoffice"] == null)
                {
                    logger.Error("AppSetting 'EmailIMBARQBackoffice' nao definido. Nao eh possivel enviar arquivos consolidado");
                    return false;
                }

                string emailsBackoffice = ConfigurationManager.AppSettings["EmailIMBARQBackoffice"].ToString();

                char[] seps = { ';', ',' };
                emailsTo = emailsBackoffice.Split(seps);

                var lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailColdRemetente"].ToString(), emailsTo[0]);

                for (int i = 1; i < emailsTo.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsTo[i]))
                        lMensagem.To.Add(emailsTo[i]);
                }

                //for (int i = 0; i < emailsCC.Length; i++)
                //{
                //    if (!String.IsNullOrEmpty(emailsCC[i]))
                //        lMensagem.CC.Add(emailsCC[i]);
                //}

                //for (int i = 0; i < emailsBCC.Length; i++)
                //{
                //    if (!String.IsNullOrEmpty(emailsBCC[i]))
                //        lMensagem.Bcc.Add(emailsBCC[i]);
                //}

                if (ConfigurationManager.AppSettings["EmailColdBCC"] != null)
                {
                    emailsBCC = ConfigurationManager.AppSettings["EmailColdBCC"].ToString().Split(seps);

                    for (int i = 0; i < emailsBCC.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(emailsBCC[i]))
                            lMensagem.Bcc.Add(emailsBCC[i]);
                    }
                }

                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailColdReplyTo"].ToString()));
                lMensagem.Subject = "RELATORIOS BACKOFFICE COLD " + dataproc.ToString("dd/MM/yyyy");

                DirectoryInfo dirInfo = new DirectoryInfo(diretGrupo);

                List<FileInfo> files = new List<FileInfo>();

                files.AddRange(dirInfo.GetFiles("*.csv"));
                files.AddRange(dirInfo.GetFiles("*.txt"));
                files.AddRange(dirInfo.GetFiles("*.pdf"));
                files.AddRange(dirInfo.GetFiles("*.zip"));

                if (files.Count > 0)
                {
                    foreach (FileInfo arquivo in files)
                    {
                        lMensagem.Attachments.Add(new Attachment(arquivo.FullName));
                    }

                    new SmtpClient(ConfigurationManager.AppSettings["EmailColdHost"].ToString()).Send(lMensagem);

                    lMensagem.Dispose();

                    foreach (FileInfo arquivo in files)
                    {
                        arquivo.Delete();
                    }

                    logger.Info("Email enviado com sucesso");
                }
                else
                {
                    logger.Info("Nao ha arquivos consolidados para enviar");
                }

            }
            catch (Exception ex)
            {
                logger.Error("EnviarEmail(): " + ex.Message, ex);
                return false;
            }

            return true;
        }

        #endregion // Distribuidor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        private T FromStringBlock<T>(string line)
        {
            int tamstrut = Marshal.SizeOf(typeof(T));
            byte[] buff = new byte[tamstrut];

            ASCIIEncoding.ASCII.GetBytes(line, 0, tamstrut, buff, 0 );

            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            T s = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string BlockToString<T>(T obj)
        {
            int tamstrut = Marshal.SizeOf(typeof(T));
            byte[] buff = new byte[tamstrut];

            IntPtr ptr = Marshal.AllocHGlobal(tamstrut);
            Marshal.StructureToPtr(obj, ptr, true);

            Marshal.Copy(ptr, buff, 0, tamstrut);
            Marshal.FreeHGlobal(ptr);

            return ASCIIEncoding.ASCII.GetString(buff);
        }


        private string GetCSV<T>(T obj)
        {
            if (obj == null)
                return String.Empty;

            StringBuilder linha = new StringBuilder();

            string[] campos = new string[obj.GetType().GetFields().Count()];

            foreach (FieldInfo prop in obj.GetType().GetFields())
            {
                IMBARQFieldType tipo = IMBARQFieldType.FieldTypeString;
                int numDecimals = 0;
                string valueFormat = "";

                try
                {
                    byte[] valor = (byte [] ) obj.GetType().GetField(prop.Name).GetValue(obj);

                    object[] attribs = obj.GetType().GetField(prop.Name).GetCustomAttributes(typeof(IMBARQExport), false);
                    object[] formats = obj.GetType().GetField(prop.Name).GetCustomAttributes(typeof(IMBARQFieldDescription), false);

                    if (attribs != null && attribs.Length > 0 && attribs[0].GetType() == typeof(IMBARQExport))
                    {
                        IMBARQExport attrib = (IMBARQExport)attribs[0];

                        logger.Debug(" Parsing: [" + valor.ByteArrayToString() + "]");

                        if (formats != null && formats.Length > 0 && formats[0].GetType() == typeof(IMBARQFieldDescription))
                        {
                            tipo = ((IMBARQFieldDescription)formats[0]).FieldType;
                            numDecimals = ((IMBARQFieldDescription)formats[0]).NumDecimalPlaces;
                            valueFormat = ((IMBARQFieldDescription)formats[0]).Format;
                        }

                        if (valor != null && attrib.ExportField )
                        {
                            if ( tipo == IMBARQFieldType.FieldTypeString)
                                campos[attrib.FieldOrder] = valor.ByteArrayToString().Trim();

                            if (tipo == IMBARQFieldType.FieldTypeDateTime)
                                campos[attrib.FieldOrder] = valor.ByteArrayToDate(valueFormat).ToString("dd/MM/yyyy");

                            if (tipo == IMBARQFieldType.FieldTypeDecimal)
                            {
                                if ( numDecimals == 0 )
                                    campos[attrib.FieldOrder] = valor.ByteArrayToInt64().ToString();

                                if (numDecimals > 0)
                                    campos[attrib.FieldOrder] = valor.ByteArrayToDecimal(numDecimals).ToString(CultureInfo.InvariantCulture);
                            }

                            if (tipo == IMBARQFieldType.FieldTypeYesOrNo)
                            {
                                campos[attrib.FieldOrder] = valor.ByteArrayToString().Trim().Equals("1")?"SIM":"NAO";
                            }

                            if (tipo == IMBARQFieldType.FieldTypeContractType)
                            {
                                IMBARQ_ContractType contractType = (IMBARQ_ContractType) Enum.Parse(typeof(IMBARQ_ContractType), valor.ByteArrayToString().Trim());
                                campos[attrib.FieldOrder] = contractType.ToString();
                            }

                            if (tipo == IMBARQFieldType.FieldTypeSettlementType)
                            {
                                IMBARQ_SettlementType settlType = (IMBARQ_SettlementType)Enum.Parse(typeof(IMBARQ_SettlementType), valor.ByteArrayToString().Trim());
                                campos[attrib.FieldOrder] = settlType.ToString();
                            }

                            if (tipo == IMBARQFieldType.FieldTypeSide)
                            {
                                IMBARQ_Side side = (IMBARQ_Side) valor.ByteArrayToString();
                                campos[attrib.FieldOrder] = side.ToString();
                            }

                            if (tipo == IMBARQFieldType.FieldTypeLaunch)
                            {
                                IMBARQ_Launch launch = (IMBARQ_Launch )valor.ByteArrayToString();
                                campos[attrib.FieldOrder] = launch.ToString();
                            }

                            if (tipo == IMBARQFieldType.FieldTypeSubAccountCode)
                            {
                                IMBARQ_SubAccountCode carteira = (IMBARQ_SubAccountCode)Enum.Parse(typeof(IMBARQ_SubAccountCode), valor.ByteArrayToString().Trim());
                                campos[attrib.FieldOrder] = carteira.ToString();
                            }

                            if (tipo == IMBARQFieldType.FieldTypePayingInstitution)
                            {
                                IMBARQ_PayingInstitution instit = (IMBARQ_PayingInstitution)Enum.Parse(typeof(IMBARQ_PayingInstitution), valor.ByteArrayToString().Trim());
                                campos[attrib.FieldOrder] = instit.ToString();
                            }

                            if (tipo == IMBARQFieldType.FieldTypeCollateralType)
                            {
                                IMBARQ_CollateralType tipoGarantia = (IMBARQ_CollateralType)Enum.Parse(typeof(IMBARQ_CollateralType), valor.ByteArrayToString().Trim());
                                campos[attrib.FieldOrder] = tipoGarantia.ToString();
                            }

                            if (tipo == IMBARQFieldType.FieldTypeCollateralCoverage)
                            {
                                IMBARQ_CollateralCoverageType finalidadeGarantia = (IMBARQ_CollateralCoverageType)Enum.Parse(typeof(IMBARQ_CollateralCoverageType), valor.ByteArrayToString().Trim());
                                campos[attrib.FieldOrder] = finalidadeGarantia.ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("GetCSV(): " + ex.Message, ex);
                    logger.Error("Class: [" + obj.GetType().Name + "] Tipo: " + tipo.ToString() + " Format: [" + valueFormat + "] Decimais: [" + numDecimals + "]");
                }
            }

            foreach (string campo in campos)
            {
                linha.AppendFormat("{0};", campo);
            }
            
            string retorno = linha.ToString();

            if ( !String.IsNullOrEmpty(retorno) && retorno.LastIndexOf(';') == (retorno.Length - 1))
                retorno = retorno.Remove(retorno.Length - 1);

            return retorno;
        }

        private string GetHeaderLabels<T>(T obj)
        {
            if (obj == null)
                return String.Empty;

            StringBuilder linha = new StringBuilder();

            string[] colunas = new string[obj.GetType().GetFields().Count()];

            foreach (FieldInfo prop in obj.GetType().GetFields())
            {
                try
                {
                    object[] attribs = obj.GetType().GetField(prop.Name).GetCustomAttributes(typeof(IMBARQExport), false);

                    if (attribs != null && attribs[0].GetType() == typeof(IMBARQExport))
                    {
                        IMBARQExport attrib = (IMBARQExport)attribs[0];

                        if (attrib.ExportField)
                        {
                            colunas [attrib.FieldOrder] = prop.Name;
                        }
                    }
                }
                catch (Exception ex)
                { }
            }

            foreach (string coluna in colunas)
            {
                linha.AppendFormat("{0};", coluna);
            }

            string retorno = linha.ToString();

            if (!String.IsNullOrEmpty(retorno) && retorno.LastIndexOf(';') == (retorno.Length - 1))
                retorno = retorno.Remove(retorno.Length - 1);

            return retorno.ToString();
        }

        /// <summary>
        /// Copy properties values from source to destination if they have the 
        /// same properties names and types
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopyPropertiesAsPossible(object source, object destination)
        {
            if (source == null || destination == null)
                return;

            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                try
                {
                    foreach (PropertyInfo prop1 in destination.GetType().GetProperties())
                    {
                        if (prop1.Name.Equals(prop.Name) && prop1.PropertyType.Equals(prop.PropertyType))
                        {
                            object xxx = source.GetType().GetProperty(prop1.Name).GetValue(source, null);
                            if (xxx != null)
                                prop1.SetValue(destination, xxx, null);
                        }
                    }

                }
                catch (Exception ex)
                { }
            }
        }

        private static void UnzipFiles(string file, string destPath)
        {
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(file)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    logger.Debug("Arquivo [" + theEntry.Name + "]");

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        string destName = String.Format(@"{0}\{1}", destPath, fileName);

                        logger.Info("Uncompressing to [" + destName + "]");

                        using (FileStream streamWriter = File.Create(destName))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
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

        public void _CarregarListaNomes()
        {
            try
            {
                DbAccOracle db = new DbAccOracle();

                ConcurrentDictionary<int, string> dct = db.CarregarNomeClientes();

                foreach (KeyValuePair<int, string> item in dct)
                {
                    if (!dctNome.ContainsKey(item.Key))
                        dctNome.Add(item.Key, item.Value);
                }

                logger.Info("Carregou lista de nomes com " + dctNome.Count + " itens.");
            }
            catch (Exception ex)
            {
                logger.Error("_CarregarListaNomes: " + ex.Message, ex);
            }
        }

        public string BuscarNomeCliente(int clienteGradual)
        {
            string ret = String.Empty;

            if (dctNome.ContainsKey(clienteGradual))
                ret = dctNome[clienteGradual];

            return ret;
        }

        #endregion //BlockReaders
    }
}
