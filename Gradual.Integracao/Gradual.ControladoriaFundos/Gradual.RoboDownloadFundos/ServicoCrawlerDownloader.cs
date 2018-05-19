using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;
using log4net;
using Gradual.RoboDownloadFundos.Lib.Dados;
using System.Configuration;
using System.IO;
using Gradual.RoboDownloadFundos.Lib;
using System.Threading;
using OfficeOpenXml;
using Gradual.FINC.Lib.Dados;
using System.Security;
using Gradual.RoboDownloadFundos.Lib.SFTP;
using ICSharpCode.SharpZipLib.Zip;
using System.Data;
using Gradual.Cetip.Lib;
using System.Globalization;
using System.Net.Mail;
using System.Security.Cryptography;

namespace Gradual.RoboDownloadFundos
{
    public class ServicoCrawlerDownloader : IServicoControlavel
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private const string DisableCachingName = @"TestSwitch.LocalAppContext.DisableCaching";
        private const string DontEnableSchUseStrongCryptoName = @"Switch.System.Net.DontEnableSchUseStrongCrypto";

        private ServicoStatus _status = ServicoStatus.Parado;
        private CronStyleScheduler _scheduler = new CronStyleScheduler();
        private static ServicoCrawlerDownloader _me = null;

        private string sftpRemoteDirList = ConfigurationManager.AppSettings["SFTPRemoteDir"].ToString();
        private string sftpLocalDir = ConfigurationManager.AppSettings["SFTPLocalDir"].ToString();
        private string sftpSiglasCerticadoras = ConfigurationManager.AppSettings["SFTPSiglasCertificadoras"].ToString();

        private string cetipRemoteDirList = ConfigurationManager.AppSettings["CETIPRemoteDir"].ToString();
        private string cetipLocalDir = ConfigurationManager.AppSettings["CETIPLocalDir"].ToString();

        private static Dictionary<string, FrontisFundoInfo> fundosFrontis = new Dictionary<string, FrontisFundoInfo>();

        private static Dictionary<int, PerformanceCETIPInfo> dctPerfCetip = new Dictionary<int, PerformanceCETIPInfo>();

        // Flags para nao atropelar os logins de usuario junto ao
        // banco plural, para evitar bloqueio de senha, etc
        private static bool bExecutandoFIDC = false;
        private static bool bExecutandoNetReport = false;
        private static bool bExecutandoNetCot = false;
        private static bool bExecutandoFINC = false;
        private static bool bBuscarFTP = false;
        private static bool bProcessarFrontis = false;
        private static bool bBuscarDanfe = false;

        private bool bGerandoPlanilhaPerfilMensal = false;
        private static bool bBuscarArquivosCetip = false;
        private static bool bProcessarCetipEscrituracao = false;
        CultureInfo ciBr = CultureInfo.CreateSpecificCulture("pt-Br");

        public ServicoCrawlerDownloader GetInstance()
        {
            if (_me == null)
            {
                _me = new ServicoCrawlerDownloader();
            }

            return _me;
        }

        public void IniciarServico()
        {
            try
            {
                AppContext.SetSwitch(DisableCachingName, true);
                AppContext.SetSwitch(DontEnableSchUseStrongCryptoName, true);
                Directory.CreateDirectory(ConfigurationManager.AppSettings["PathDownloadFIDC"].ToString());
                Directory.CreateDirectory(ConfigurationManager.AppSettings["PathSimpleBrowserLogs"].ToString());
                Directory.CreateDirectory(ConfigurationManager.AppSettings["PathDownloadCarteiraDiaria"].ToString());
                Directory.CreateDirectory(ConfigurationManager.AppSettings["PathDownloadMEC"].ToString());
                Directory.CreateDirectory(ConfigurationManager.AppSettings["PathDownloadExtratoCot"].ToString());

                _status = ServicoStatus.EmExecucao;
                _scheduler.Start();
            }
            catch (Exception ex)
            {
                logger.Error("IniciarServico: " + ex.Message, ex);
            }
        }

        public void PararServico()
        {
            _scheduler.Stop();
            _status = ServicoStatus.Parado;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        public void DailyTasks(string parametros)
        {
            try
            {
                if (parametros.Equals("RotinaOperacionalFIDC", StringComparison.InvariantCultureIgnoreCase))
                {
                    RotinaOperacionalFIDC();
                }

                if (parametros.Equals("BuscarCarteiraDiaria", StringComparison.InvariantCultureIgnoreCase))
                {
                    BuscarCarteiraDiaria();
                }

                if (parametros.Equals("BuscarMEC", StringComparison.InvariantCultureIgnoreCase))
                {
                    BuscarMEC();
                }

                if (parametros.Equals("BuscarExtratoCotista", StringComparison.InvariantCultureIgnoreCase))
                {
                    BuscarExtratoCotista();
                }


            }
            catch (Exception ex)
            {
                logger.Error("DailyTasks: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void RotinaOperacionalFIDC()
        {
            if (bExecutandoFIDC)
            {
                logger.Warn("RotinaOperacionalFIDC ja em andamento, deixando para proxima rodada");
                return;
            }

            try
            {

                bExecutandoFIDC = true;

                DateTime dataDMenas1 = DateTime.Now.AddDays(-1);

                while (dataDMenas1.DayOfWeek == DayOfWeek.Saturday || dataDMenas1.DayOfWeek == DayOfWeek.Sunday)
                    dataDMenas1 = dataDMenas1.AddDays(-1);

                PersistenciaDB db = new PersistenciaDB();

                List<FundoInfo> fundosFIDC = db.ObterFundos(OrigemDownloadEnum.BCO_PAULISTA_FIDC);

                Dictionary<int, LogDownloadInfo> logDownload = db.ObterLogDownload(OrigemDownloadEnum.BCO_PAULISTA_FIDC, CategoriaDownloadEnum.TITULOS_BAIXADOS, DateTime.Now.Date);

                foreach (FundoInfo info in fundosFIDC)
                {
                    int idDownload = -1;
                    bool bShouldDownload = false;

                    if (!logDownload.ContainsKey(info.IDFundo))
                    {
                        idDownload = db.InserirLogDownload(OrigemDownloadEnum.BCO_PAULISTA_FIDC, CategoriaDownloadEnum.TITULOS_BAIXADOS,  null, info.IDFundo);
                        bShouldDownload = true;
                    }
                    else
                    {
                        idDownload = logDownload[info.IDFundo].IDDownloadTransacao;

                        bShouldDownload = (logDownload[info.IDFundo].stTransacao.Equals("N") &&
                                            logDownload[info.IDFundo].numTentativas < 5);
                    }

                    if (bShouldDownload)
                    {
                        Gradual.BancoPaulista.Lib.WebCrawler paulistaCrawler = new Gradual.BancoPaulista.Lib.WebCrawler();

                        logger.Info("Efetuando download do FIDC [" + info.IDFundo + "/" + info.CodFundo + "/" + info.DsFundo.Trim().Replace(' ', '+') + "]");

                        string pathCSV = String.Format(@"{0}\FIDC-TitBaixLiq-{1}-{2}-{3}.csv",
                            ConfigurationManager.AppSettings["PathDownloadFIDC"].ToString(),
                            info.IDFundo,
                            info.DsFundo.Trim().Replace(' ', '+'),
                            dataDMenas1.ToString("yyyy-MM-dd"));

                        string msgErro = "Download efetuado com sucesso";

                        bool bRet = paulistaCrawler.DownloadFIDC(info.CodFundo, dataDMenas1, dataDMenas1, pathCSV, ref msgErro);
                        if ( bRet )
                        {
                            logger.Info("Calculando total para o arquivo [" + pathCSV + "]");

                            Decimal totalBaixa = Gradual.BancoPaulista.Lib.Utilities.CalcularCobrancaComRelatorioBaixa(pathCSV);

                            logger.Info("Total para o arquivo [" + pathCSV + "] = " + totalBaixa);

                            db.InserirTotalTitulosBaixados(info.IDFundo, totalBaixa);
                        }

                        db.AtualizarLogDownload(idDownload, pathCSV, bRet, msgErro);

                        Thread.Sleep(2000);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("RotinaOperacionalFIDC: " + ex.Message, ex);
            }
            finally
            {
                bExecutandoFIDC = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuscarCarteiraDiaria()
        {
            if (bExecutandoNetReport)
            {
                logger.Warn("BuscarCarteiraDiaria ou BuscarMEC em andamento, deixando para proxima rodada");
                return;
            }

            try
            {

                bExecutandoNetReport = true;

                DateTime dataInicial = DateTime.Now;

                // Se for menor que o dia 15, pega o fechamento do mes anterior
                if (dataInicial.Day <= 15)
                {
                    DateTime mesAnterior = DateTime.Now.AddMonths(-1);

                    dataInicial = new DateTime(mesAnterior.Year, mesAnterior.Month, DateTime.DaysInMonth(mesAnterior.Year, mesAnterior.Month));
                }
                else
                {
                    // Se for maior, pega o dia 15
                    dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);
                }

                while (dataInicial.DayOfWeek == DayOfWeek.Saturday || dataInicial.DayOfWeek == DayOfWeek.Sunday)
                    dataInicial = dataInicial.AddDays(-1);

                logger.Info("Buscando Carteira Diaria para intervalo de " + dataInicial.ToString("dd/MM/yyyy") + " ate " + DateTime.Now.ToString("dd/MM/yyyy"));

                PersistenciaDB db = new PersistenciaDB();

                List<FundoInfo> fundos = db.ObterFundos(OrigemDownloadEnum.BCO_PAULISTA_NETREPORT);

                Dictionary<int, LogDownloadInfo> logDownload = db.ObterLogDownload(OrigemDownloadEnum.BCO_PAULISTA_NETREPORT, CategoriaDownloadEnum.CARTEIRA_DIARIA, DateTime.Now.Date);

                foreach (FundoInfo info in fundos)
                {
                    int idDownload = -1;
                    bool bShouldDownload = false;

                    if (!logDownload.ContainsKey(info.IDFundo))
                    {
                        idDownload = db.InserirLogDownload(OrigemDownloadEnum.BCO_PAULISTA_NETREPORT, CategoriaDownloadEnum.CARTEIRA_DIARIA,  null, info.IDFundo);
                        bShouldDownload = true;
                    }
                    else
                    {
                        idDownload = logDownload[info.IDFundo].IDDownloadTransacao;

                        bShouldDownload = (logDownload[info.IDFundo].stTransacao.Equals("N") &&
                                            logDownload[info.IDFundo].numTentativas < 5);
                    }

                    if (bShouldDownload)
                    {

                        Gradual.BancoPaulista.Lib.WebCrawler paulistaCrawler = new Gradual.BancoPaulista.Lib.WebCrawler();

                        logger.Info("Efetuando download da carteira do [" + info.IDFundo + "/" + info.CodFundo + "/" + info.DsFundo.Trim().Replace(' ', '+') + "]");

                        string pathPDF = String.Format(@"{0}\Carteira-Diaria-{1}-{2}-{3}.pdf",
                            ConfigurationManager.AppSettings["PathDownloadCarteiraDiaria"].ToString(),
                            info.IDFundo,
                            info.DsFundo.Trim().Replace(' ', '+'),
                            dataInicial.ToString("yyyy-MM-dd"));

                        string msgErro = "Download efetuado com sucesso";

                        bool bRet = paulistaCrawler.DownloadCarteiraDiaria(info, dataInicial, DateTime.Now, pathPDF, ref msgErro);

                        db.AtualizarLogDownload(idDownload, pathPDF, bRet, msgErro);

                        Thread.Sleep(2000);
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error("BuscarCarteiraDiaria: " + ex.Message, ex);
            }
            finally
            {
                bExecutandoNetReport = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void BuscarMEC()
        {
            if (bExecutandoNetReport)
            {
                logger.Warn("BuscarMEC ou BuscarCarteiraDiaria em andamento, deixando para proxima rodada");
                return;
            }

            try
            {

                bExecutandoNetReport = true;

                DateTime dataFinal = DateTime.Now;
                DateTime dataInicial = DateTime.Now;

                // Pega o fechamento do mes anterior
                DateTime mesAnterior = DateTime.Now.AddMonths(-1);

                dataFinal = new DateTime(mesAnterior.Year, mesAnterior.Month, DateTime.DaysInMonth(mesAnterior.Year, mesAnterior.Month));

                while (dataFinal.DayOfWeek == DayOfWeek.Saturday || dataFinal.DayOfWeek == DayOfWeek.Sunday)
                    dataFinal = dataFinal.AddDays(-1);

                DateTime mesRetrasado = dataFinal.AddMonths(-1);
                dataInicial = new DateTime(mesRetrasado.Year, mesRetrasado.Month, DateTime.DaysInMonth(mesRetrasado.Year, mesRetrasado.Month));
                while (dataInicial.DayOfWeek == DayOfWeek.Saturday || dataInicial.DayOfWeek == DayOfWeek.Sunday)
                    dataInicial = dataInicial.AddDays(-1);


                logger.Info("Buscando MEC para intervalo de " + dataInicial.ToString("dd/MM/yyyy") + " ate " + dataFinal.ToString("dd/MM/yyyy"));

                PersistenciaDB db = new PersistenciaDB();

                List<FundoInfo> fundos = db.ObterFundos(OrigemDownloadEnum.BCO_PAULISTA_NETREPORT);

                Dictionary<int, LogDownloadInfo> logDownload = db.ObterLogDownload(OrigemDownloadEnum.BCO_PAULISTA_NETREPORT, CategoriaDownloadEnum.MEC, DateTime.Now.Date);

                foreach (FundoInfo info in fundos)
                {
                    int idDownload = -1;
                    bool bShouldDownload = false;

                    if (!logDownload.ContainsKey(info.IDFundo))
                    {
                        idDownload = db.InserirLogDownload(OrigemDownloadEnum.BCO_PAULISTA_NETREPORT, CategoriaDownloadEnum.MEC,  null, info.IDFundo);
                        bShouldDownload = true;
                    }
                    else
                    {
                        idDownload = logDownload[info.IDFundo].IDDownloadTransacao;

                        bShouldDownload = (logDownload[info.IDFundo].stTransacao.Equals("N") &&
                                            logDownload[info.IDFundo].numTentativas < 5);
                    }

                    if (bShouldDownload)
                    {
                        Gradual.BancoPaulista.Lib.WebCrawler paulistaCrawler = new Gradual.BancoPaulista.Lib.WebCrawler();

                        logger.Info("Efetuando download do MEC do [" + info.IDFundo + "/" + info.CodFundo + "/" + info.DsFundo.Trim().Replace(' ', '+') + "]");

                        string pathPDF = String.Format(@"{0}\MEC-{1}-{2}-{3}.xls",
                            ConfigurationManager.AppSettings["PathDownloadMEC"].ToString(),
                            info.IDFundo,
                            info.DsFundo.Trim().Replace(' ', '+'),
                            dataFinal.ToString("yyyy-MM-dd"));

                        string msgErro = "Download efetuado com sucesso";

                        bool bRet = paulistaCrawler.DownloadMEC(info, dataFinal, DateTime.Now, pathPDF, ref msgErro);

                        db.AtualizarLogDownload(idDownload, pathPDF, bRet, msgErro);

                        Thread.Sleep(2000);
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error("BuscarMEC: " + ex.Message, ex);
            }
            finally
            {
                bExecutandoNetReport = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuscarExtratoCotista()
        {
            if (bExecutandoNetCot)
            {
                logger.Warn("BuscarExtratoCotista em andamento, deixando para proxima rodada");
                return;
            }

            try
            {

                bExecutandoNetCot = true;

                DateTime dataFinal = DateTime.Now;
                DateTime dataInicial = DateTime.Now;

                // Pega o fechamento do mes anterior
                DateTime mesAnterior = DateTime.Now.AddMonths(-1);

                dataFinal = new DateTime(mesAnterior.Year, mesAnterior.Month, DateTime.DaysInMonth(mesAnterior.Year, mesAnterior.Month));
                while (dataFinal.DayOfWeek == DayOfWeek.Saturday || dataFinal.DayOfWeek == DayOfWeek.Sunday)
                    dataFinal = dataFinal.AddDays(-1);

                DateTime mesRetrasado = dataFinal.AddMonths(-1);
                dataInicial = new DateTime(mesRetrasado.Year, mesRetrasado.Month, DateTime.DaysInMonth(mesRetrasado.Year, mesRetrasado.Month));
                while (dataInicial.DayOfWeek == DayOfWeek.Saturday || dataInicial.DayOfWeek == DayOfWeek.Sunday)
                    dataInicial = dataInicial.AddDays(-1);


                logger.Info("Buscando BuscarExtratoCotista para intervalo de " + dataInicial.ToString("dd/MM/yyyy") + " ate " + dataFinal.ToString("dd/MM/yyyy"));

                PersistenciaDB db = new PersistenciaDB();

                List<CotistaInfo> fundos = db.ObterCotistas();

                Dictionary<int, LogDownloadInfo> logDownload = db.ObterLogDownload(OrigemDownloadEnum.BCO_PAULISTA_NETCOT, CategoriaDownloadEnum.EXTRATO_COTISTA, DateTime.Now.Date);

                foreach (CotistaInfo cotista in fundos)
                {
                    int idDownload = -1;
                    bool bShouldDownload = false;

                    if (!logDownload.ContainsKey(cotista.IDCotista))
                    {
                        idDownload = db.InserirLogDownload(OrigemDownloadEnum.BCO_PAULISTA_NETCOT, CategoriaDownloadEnum.EXTRATO_COTISTA, null, cotista.IDCotista, cotista.CpfCnpj);
                        bShouldDownload = true;
                    }
                    else
                    {
                        idDownload = logDownload[cotista.IDCotista].IDDownloadTransacao;

                        bShouldDownload = (logDownload[cotista.IDCotista].stTransacao.Equals("N") &&
                                            logDownload[cotista.IDCotista].numTentativas < 5);
                    }

                    if (bShouldDownload)
                    {
                        Gradual.BancoPaulista.Lib.WebCrawler paulistaCrawler = new Gradual.BancoPaulista.Lib.WebCrawler();

                        logger.Info("Efetuando download do Extrato do [" + cotista.IDCotista + "/" + cotista.CodCotista + "/" + cotista.NomeCotista.Trim().Replace(' ', '+') + "]");

                        string pathPDF = String.Format(@"{0}\Extrato-{1}-{2}-{3}.pdf",
                            ConfigurationManager.AppSettings["PathDownloadExtratoCot"].ToString(),
                            cotista.IDCotista,
                            cotista.CodCotista.Trim().Replace(' ', '+'),
                            dataFinal.ToString("yyyy-MM-dd"));

                        string msgErro = "Download efetuado com sucesso";

                        bool bRet = paulistaCrawler.DownloadExtratoCotista(dataInicial, dataFinal, pathPDF, cotista.CodCotista, cotista.NomeCotista, ref msgErro);

                        db.AtualizarLogDownload(idDownload, pathPDF, bRet, msgErro);

                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("BuscarExtratoCotista: " + ex.Message, ex);
            }
            finally
            {
                bExecutandoNetCot = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ConvertePlanilhasFINC()
        {
            try
            {
                if (bExecutandoFINC)
                {
                    logger.Warn("RotinaOperacionalFIDC ja em andamento, deixando para proxima rodada");
                    return;
                }

                if ( ConfigurationManager.AppSettings["PathPlanilhasFINC"] ==  null )
                {
                    logger.Fatal("Erro: PathPlanilhasFINC nao definida !!!!");
                    return;
                }

                bExecutandoFINC = true;
                string pathFINC = ConfigurationManager.AppSettings["PathPlanilhasFINC"].ToString();

                List<string> files = Directory.GetFiles(pathFINC, "*.*")
                    .Where(file => file.ToLower().EndsWith(".xls") || file.ToLower().EndsWith(".xlsx"))
                    .ToList();

                foreach (string filename in files)
                {
                    ConverteXmlFINC(filename);
                }

                bExecutandoFINC = false;
            }
            catch (Exception ex)
            {

                logger.Error("BuscarExtratoCotista: " + ex.Message, ex);
                bExecutandoFINC = false;
            }
        }

        private void ConverteXmlFINC(string filename)
        {
            FileInfo finfo = new FileInfo(filename);

            string xmlFile = finfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(finfo.Name) + ".xml";

            if (File.Exists(xmlFile))
            {
                logger.WarnFormat("Arquivo [{0}] ja foi gerado, ignorando", xmlFile);
                return;
            }

            List<InformeFundo> lstInformeFundo = new List<InformeFundo>();
            string dataCompetencia = DateTime.Now.ToString("dd/MM/yyyy");
            string dataGeracao = DateTime.Now.ToString("dd/MM/yyyy");

            using (ExcelPackage xlPackage = new ExcelPackage(finfo))
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                //int iCol = 1;
                int iRow = 2;

                InformeFundo informeFundo = null;
                FormaDivulgacao formaDivulg = null;
                FormaDivulgacaoCotista formaDivulgCot = null;
                PrestadorServico prestador = null;

                string celLabel = "";
                while (worksheet.GetValue(iRow, 1) != null) 
                {
                    celLabel = worksheet.GetValue(iRow, 1).ToString().TrimEnd();
                    
                    string celvalue = "";

                    if ( worksheet.GetValue(iRow, 2) != null )
                        celvalue = SecurityElement.Escape( worksheet.GetValue(iRow, 2).ToString().TrimEnd());

                    logger.DebugFormat("Cell({0}, 1){1}=[{2}]", iRow, celLabel, celvalue);

                    if (!String.IsNullOrEmpty(celvalue))
                    {
                        switch (celLabel)
                        {
                            case "COD DOC": break;
                            case "VERSÃO": break;
                            case "DT COMPT":
                                DateTime aux;
                                if (DateTime.TryParse(celvalue, out aux))
                                    dataCompetencia = aux.ToString("dd/MM/yyyy");

                                break;
                            case "DT GER":
                                DateTime aux1;
                                if (DateTime.TryParse(celvalue, out aux1))
                                    dataGeracao  = aux1.ToString("dd/MM/yyyy");

                                break;
                            case "CNPJ FDO":
                                if (informeFundo != null)
                                    lstInformeFundo.Add(informeFundo);
                                informeFundo = new InformeFundo();
                                informeFundo.Cnpj = celvalue;
                                break;
                            case "PERIOD.":
                                if (celvalue.Length >250)
                                {
                                    logger.Warn("Campo 'PERIOD.' com mais de 250 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 250);
                                }
                                informeFundo.Periodicidade = celvalue;
                                break;
                            case "LOCAL DIV.":
                                formaDivulg = new FormaDivulgacao();
                                if (celvalue.Length > 300)
                                {
                                    logger.Warn("Campo 'LOCAL DIV.' com mais de 300 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 300);
                                }
                                formaDivulg.LocalDivulgacao = celvalue;
                                break;
                            case "MEIO DIV.":
                                formaDivulg.CodigoMeioDivulg = celvalue;
                                break;
                            case "FORMA DIV.":
                                if (celvalue.Length > 100)
                                {
                                    logger.Warn("Campo 'FORMA DIV.' com mais de 100 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 100);
                                }
                                formaDivulg.DescricaoForma = celvalue;
                                informeFundo.FormasDivulgacao.Add(formaDivulg);
                                break;
                            case "LOCAL DIV. COT.":
                                formaDivulgCot = new FormaDivulgacaoCotista();
                                if (celvalue.Length > 100)
                                {
                                    logger.Warn("Campo 'LOCAL DIV. COT' com mais de 100 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 100);
                                }
                                formaDivulgCot.LocalDivulgacao = celvalue;
                                break;
                            case "MEIO DIV. COT.":
                                formaDivulgCot.CodigoMeioDivulg = celvalue;
                                break;
                            case "FORMA DIV. COT.":
                                if (celvalue.Length > 100)
                                {
                                    logger.Warn("Campo 'FORMA DIV. COT.' com mais de 100 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 100);
                                }
                                formaDivulgCot.FormaSolicitacao = celvalue;
                                informeFundo.FormasDivulgacaoCotista.Add(formaDivulgCot);
                                break;
                            case "EXPOSICAO":
                                if (celvalue.Length > 2000)
                                {
                                    logger.Warn("Campo 'EXPOSICAO' com mais de 2000 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2000);
                                }
                                informeFundo.Exposicao = celvalue;
                                break;
                            case "VOTO (S ou N)":
                                if ( celvalue.ToUpperInvariant().Equals("N") )
                                    informeFundo.VotoGestor = false;
                                else
                                    informeFundo.VotoGestor = true;
                                break;
                            case "DESC.VOTO":
                                if (celvalue.Length > 1000)
                                {
                                    logger.Warn("Campo 'DESC.VOTO' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 1000);
                                }
                                informeFundo.PoliticaExercicioVoto = celvalue;
                                break;
                            case "TRIBUTACAO":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'TRIBUTACAO' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.PoliticaTributacao = celvalue;
                                break;
                            case "ADM.DE RISCO":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'ADM.DE RISCO' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.PoliticaAdmRisco = celvalue;
                                break;
                            case "AG. RISCO (Sim ou Não)":
                                break;
                            case "CNPJ AG.RISCO": break;
                            case "PRESTADOR": break;
                            case "CLASSIF.AG.RISCO": break;
                            case "DISCL_ADV": break;
                            case "AP.ADM.":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'AP.ADM.' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.ApresAdministrador = celvalue;
                                break;
                            case "AP.GESTOR":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'AP.GESTOR' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.ApresGestor = celvalue;
                                break;
                            case "DS. SERV.PREST.":
                                prestador = new PrestadorServico();
                                if (celvalue.Length > 100)
                                {
                                    logger.Warn("Campo 'DS. SERV.PREST' com mais de 100 caracteres, truncando");
                                    prestador.Descricao = celvalue.Substring(0,100);
                                }
                                else
                                    prestador.Descricao = celvalue;
                                break;
                            case "NOME PREST.":
                                if (celvalue.Length > 100)
                                {
                                    logger.Warn("Campo 'NOME PREST.' com mais de 100 caracteres, truncando");
                                    prestador.Nome = celvalue.Substring(0, 100);
                                }
                                else
                                    prestador.Nome = celvalue;
                                informeFundo.PrestadoresServico.Add(prestador);
                                break;
                            case "COD.DIST.OF.":
                                informeFundo.CodigoDistribuicaoOferta = celvalue;
                                break;
                            case "DETALHE":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'DETALHE' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.PoliticaDistribuicaoOferta = celvalue;
                                break;
                            case "INFORM ANBIMA":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'INFORM ANBIMA' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.InformeAmbima = celvalue;
                                break;
                            case "INFORM RELEV":
                                if (celvalue.Length > 4000)
                                {
                                    logger.Warn("Campo 'INFORM RELEV' com mais de 4000 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 4000);
                                }
                                informeFundo.InformeRelevante = celvalue;
                                break;
                        }

                    }
                    iRow++;
                }

                if (informeFundo != null)
                    lstInformeFundo.Add(informeFundo);

            } // the using statement calls Dispose() which closes the package.


            

            StringBuilder xmlContent = new StringBuilder();

            xmlContent.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlContent.AppendLine("<DOC_ARQ xmlns=\"urn:infocomp\">");
            xmlContent.AppendLine("<CAB_INFORM>");
            xmlContent.AppendLine("<COD_DOC>71</COD_DOC>");
            xmlContent.AppendLine("<VERSAO>1</VERSAO>");
            xmlContent.AppendFormat("<DT_COMPT>{0}</DT_COMPT>", dataCompetencia);
            xmlContent.AppendLine();
            xmlContent.AppendFormat("<DT_GERAC_ARQ>{0}</DT_GERAC_ARQ>", dataGeracao);
            xmlContent.AppendLine();
            xmlContent.AppendLine("</CAB_INFORM>");
            xmlContent.AppendLine("<LISTA_INFORM>");

            foreach(InformeFundo informe in lstInformeFundo)
            {
                xmlContent.AppendLine("<INFORM>");
                xmlContent.AppendFormat("<CNPJ_FDO>{0}</CNPJ_FDO>", informe.Cnpj);
                xmlContent.AppendLine();
                xmlContent.AppendFormat("<PERIODIC_MIN_DIVULG_COMPOS_CART>{0}</PERIODIC_MIN_DIVULG_COMPOS_CART>", informe.Periodicidade);
                xmlContent.AppendLine();
                xmlContent.AppendLine("<LISTA_LOCAL_MEIO_FORMA_DIVULG_INF>");
		        foreach(FormaDivulgacao formadiv in informe.FormasDivulgacao )
                {
                    xmlContent.AppendLine("<LOCAL_MEIO_FORMA_DIVULG_INF>");
                    xmlContent.AppendFormat("<DS_LOCAL_DIVULG>{0}</DS_LOCAL_DIVULG>", formadiv.LocalDivulgacao);
                    xmlContent.AppendLine();
                    xmlContent.AppendFormat("<COD_MEIO_DIVULG>{0}</COD_MEIO_DIVULG>", formadiv.CodigoMeioDivulg);
                    xmlContent.AppendLine();
                    xmlContent.AppendFormat("<DS_FORMA_DIVULG>{0}</DS_FORMA_DIVULG>", formadiv.DescricaoForma);
                    xmlContent.AppendLine();
                    xmlContent.AppendLine("</LOCAL_MEIO_FORMA_DIVULG_INF>");
                }
                xmlContent.AppendLine("</LISTA_LOCAL_MEIO_FORMA_DIVULG_INF>");
                xmlContent.AppendLine("<LISTA_LOCAL_MEIO_FORMA_DIVULG_INF_COTISTA>");
                foreach(FormaDivulgacaoCotista formdivcot in informe.FormasDivulgacaoCotista )
                {
                    xmlContent.AppendLine("<LOCAL_MEIO_FORMA_DIVULG_INF_COTISTA>");
                    xmlContent.AppendFormat("<DS_RESP>{0}</DS_RESP>", formdivcot.Responsavel);
                    xmlContent.AppendLine();
                    xmlContent.AppendFormat("<DS_LOCAL>{0}</DS_LOCAL>", formdivcot.LocalDivulgacao);
                    xmlContent.AppendLine();
                    xmlContent.AppendFormat("<COD_MEIO>{0}</COD_MEIO>", formdivcot.CodigoMeioDivulg);
                    xmlContent.AppendLine();
                    xmlContent.AppendFormat("<DS_FORMA_SOLIC>{0}</DS_FORMA_SOLIC>", formdivcot.FormaSolicitacao);
                    xmlContent.AppendLine();
                    xmlContent.AppendLine("</LOCAL_MEIO_FORMA_DIVULG_INF_COTISTA>");
                }
                xmlContent.AppendLine("</LISTA_LOCAL_MEIO_FORMA_DIVULG_INF_COTISTA>");
                xmlContent.AppendFormat("<EXPOSIC_RELEV_FAT_RISC>{0}</EXPOSIC_RELEV_FAT_RISC>", informe.Exposicao);
                xmlContent.AppendLine();
                xmlContent.AppendLine("<DS_POLIT_EXERC_VOTO_ATIV_FINANC>");

                if ( informe.VotoGestor )
                    xmlContent.AppendLine("<COD_VOTO_GEST_ASSEMB>1</COD_VOTO_GEST_ASSEMB>");
                else
                    xmlContent.AppendLine("<COD_VOTO_GEST_ASSEMB>2</COD_VOTO_GEST_ASSEMB>");

                xmlContent.AppendFormat("<DS_POLIT_EXERC_VOTO>{0}</DS_POLIT_EXERC_VOTO>", informe.PoliticaExercicioVoto);
                xmlContent.AppendLine();
                xmlContent.AppendLine("</DS_POLIT_EXERC_VOTO_ATIV_FINANC>");
                xmlContent.AppendFormat("<DS_TRIBUT_POLIT_TRIBUT>{0}</DS_TRIBUT_POLIT_TRIBUT>", informe.PoliticaTributacao);
                xmlContent.AppendLine();
                xmlContent.AppendFormat("<DS_POLIT_ADM_RISC>{0}</DS_POLIT_ADM_RISC>", informe.PoliticaAdmRisco);
                xmlContent.AppendLine();
                xmlContent.AppendLine("<IDENT_CLASSIF_RISCO>");
                xmlContent.AppendLine("<AGENC_CLASSIF_RATIN>2</AGENC_CLASSIF_RATIN>");
                xmlContent.AppendLine("<NR_CNPJ>000000000000</NR_CNPJ>");
                xmlContent.AppendLine("<NM_PREST></NM_PREST>");
                xmlContent.AppendLine("<CLASSIF_AGENC_RISCO></CLASSIF_AGENC_RISCO>");
                xmlContent.AppendLine("<DISCL_ADVERT></DISCL_ADVERT>");
                xmlContent.AppendLine("</IDENT_CLASSIF_RISCO>");
                xmlContent.AppendLine("<APRES_ADM_GEST>");
                xmlContent.AppendFormat("<APRES_DETALHE_ADM>{0}</APRES_DETALHE_ADM>",informe.ApresAdministrador);
                xmlContent.AppendLine();
                xmlContent.AppendFormat("<APRES_DETALHE_GEST>{0}</APRES_DETALHE_GEST>", informe.ApresGestor);
                xmlContent.AppendLine();
                xmlContent.AppendLine("</APRES_ADM_GEST>");
                xmlContent.AppendLine("<LISTA_PREST_SERVIC>");
                foreach(PrestadorServico prestad in informe.PrestadoresServico )
                {
                    xmlContent.AppendLine("<PREST_SERVIC>");
                    xmlContent.AppendFormat("<DS_SERVICO_PRESTADO>{0}</DS_SERVICO_PRESTADO>", prestad.Descricao);
                    xmlContent.AppendLine();
                    xmlContent.AppendFormat("<NM_PREST>{0}</NM_PREST>", prestad.Nome);
                    xmlContent.AppendLine();
                    xmlContent.AppendLine("</PREST_SERVIC>");
                }
                xmlContent.AppendLine("</LISTA_PREST_SERVIC>");
                xmlContent.AppendLine("<POLIT_DISTR_COTA>");
                xmlContent.AppendFormat("<COD_DISTR_OFERTA_PUB>{0}</COD_DISTR_OFERTA_PUB>", informe.CodigoDistribuicaoOferta);
                xmlContent.AppendLine();
                xmlContent.AppendFormat("<DS_DETALHE_POLIT_DISTR_COTA>{0}</DS_DETALHE_POLIT_DISTR_COTA>", informe.PoliticaDistribuicaoOferta);
                xmlContent.AppendLine();
                xmlContent.AppendLine("</POLIT_DISTR_COTA>");
                xmlContent.AppendLine("<OUTR_INFORM_RELEV>");
                xmlContent.AppendFormat("<INFORM_AUTOREGUL_ANBIMA>{0}</INFORM_AUTOREGUL_ANBIMA>", informe.InformeAmbima);
                xmlContent.AppendLine();
                xmlContent.AppendFormat("<INFORM_RELEV>{0}</INFORM_RELEV>", informe.InformeRelevante);
                xmlContent.AppendLine();
                xmlContent.AppendLine("</OUTR_INFORM_RELEV>");
                xmlContent.AppendLine("</INFORM>");
            }
            xmlContent.AppendLine("</LISTA_INFORM>");
            xmlContent.AppendLine("</DOC_ARQ>");


            File.WriteAllText(xmlFile, xmlContent.ToString(), Encoding.UTF8);
        }


        public void BuscarPKCS7()
        {
            try
            {
                if (bBuscarFTP)
                {
                    logger.Info("Outra operacao de download em FTP em andamento...");
                    return;
                }

                if (bProcessarFrontis)
                {
                    logger.Info("Operacao de processamento dos arquivos Frontis em andamento, adiando SFTP...");
                    return;
                }

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

                List<string> searchPatern = new List<string>();
                List<string> exceptionList = new List<string>();

                PersistenciaDB db = new PersistenciaDB();

                Dictionary<string, FrontisFundoInfo> fundos = db.ObterFundosFrontis();
                foreach (FrontisFundoInfo info in fundos.Values)
                {
                    if (!fundosFrontis.ContainsKey(info.CNPJ))
                    {
                        fundosFrontis.Add(info.CNPJ, info);
                    }
                }

                // Carrega os arquivos ja baixados historicamente
                List<FrontisDownloadInfo> downloads = db.ObterControleDownloadsFrontis();
                foreach (FrontisDownloadInfo download in downloads)
                {
                    if (fundosFrontis.ContainsKey(download.Cnpj))
                    {
                        FrontisFundoInfo info = fundosFrontis[download.Cnpj];

                        if (!info.Arquivos.Any(download.NomeArquivo.Contains))
                        {
                            info.Arquivos.Add(download.NomeArquivo);
                        }
                    }
                }

                string[] siglasCertificadoras = sftpSiglasCerticadoras.Split(';');
                foreach (string cnpj in fundos.Keys)
                {
                    foreach( string sigla in siglasCertificadoras )
                        searchPatern.Add( sigla + cnpj);

                    if (fundosFrontis.ContainsKey(cnpj))
                    {
                        exceptionList.AddRange(fundosFrontis[cnpj].Arquivos);
                    }
                }


                if (searchPatern.Count > 0)
                {
                    string[] sftpRemoteDirArr = sftpRemoteDirList.Split(';');

                    foreach (string sftpRemoteDir in sftpRemoteDirArr)
                    {
                        ftpClient.TransferirArquivos(sftpRemoteDir, sftpLocalDir, searchPatern.ToArray(), exceptionList.ToArray());
                    }
                }
                else
                {
                    logger.Warn("Nao ha mais arquivos para baixar hoje");
                }

                bBuscarFTP = false;
            }
            catch (Exception ex)
            {
                logger.Error("BuscarFTP: " + ex.Message, ex);
                bBuscarFTP = false;
            }
        }

        public void ProcessarArquivosFrontis()
        {
            try
            {
                if (bProcessarFrontis)
                {
                    logger.Info("Outra operacao de processamento dos arquivos Frontis em andamento...");
                    return;
                }

                bProcessarFrontis = true;

                logger.Info("Processando arquivos Frontis");

                PersistenciaDB db = new PersistenciaDB();

                string bkpPath = ConfigurationManager.AppSettings["FrontisBkp"].ToString();

                if (!Directory.Exists(bkpPath))
                {
                    Directory.CreateDirectory(bkpPath);
                }

                string pkcsPath = db.ObterPathFrontis();
                if ( ConfigurationManager.AppSettings["EmTeste"].ToString().ToLowerInvariant().Equals("true") )
                    pkcsPath = @"c:\temp\AdmFundos\FrontisMoved";

                // Popula o dicionario de fundos, se ja nao foi feito
                Dictionary<string, FrontisFundoInfo> fundos = db.ObterFundosFrontis();
                foreach(FrontisFundoInfo info in fundos.Values)
                {
                    if (!fundosFrontis.ContainsKey(info.CNPJ))
                    {
                        fundosFrontis.Add(info.CNPJ, info);
                    }
                }

                // Carrega os arquivos ja baixados historicamente
                List<FrontisDownloadInfo> downloads = db.ObterControleDownloadsFrontis();
                foreach (FrontisDownloadInfo download in downloads)
                {
                    if (fundosFrontis.ContainsKey(download.Cnpj))
                    {
                        FrontisFundoInfo info = fundosFrontis[download.Cnpj];

                        if (!info.Arquivos.Any(download.NomeArquivo.Contains))
                        {
                            info.Arquivos.Add(download.NomeArquivo);
                        }
                    }
                }

                string[] siglasCertificadoras = sftpSiglasCerticadoras.Split(';');

                foreach (FrontisFundoInfo info in fundosFrontis.Values)
                {
                    List<string> searchPatterns = new List<string>();

                    foreach (string sigla in siglasCertificadoras)
                        searchPatterns.Add( sigla + info.CNPJ + "*");

                    foreach (string searchPattern in searchPatterns)
                    {
                        logger.Info("Buscando por arquivos [" + searchPattern + "]");

                        DirectoryInfo dirInfo = new DirectoryInfo(sftpLocalDir);

                        List<FileInfo> filesPKCS7 = dirInfo.GetFiles(searchPattern).ToList();

                        logger.Info("Encontrou " + filesPKCS7.Count + " arquivos [" + searchPattern + "]");

                        foreach (FileInfo pkcs7zip in filesPKCS7)
                        {
                            try
                            {
                                // Movimenta somente se nao foi enviado ainda
                                // Caso tenha baixado novamente, ignora
                                if (!info.Arquivos.Any(pkcs7zip.Name.Contains))
                                {
                                    string destPath = String.Format(@"{0}\{1}\CERTIFICADORAS", pkcsPath, info.ISIN);

                                    if (!Directory.Exists(destPath))
                                        Directory.CreateDirectory(destPath);

                                    string bkpName = String.Format(@"{0}\{1}", bkpPath, pkcs7zip.Name);

                                    logger.InfoFormat("Descompactando {0} em {1}", pkcs7zip.FullName, destPath);

                                    UnzipFiles(pkcs7zip.FullName, destPath);

                                    logger.InfoFormat("Movendo {0} para {1}", pkcs7zip.FullName, bkpName);

                                    if (File.Exists(bkpName))
                                        File.Delete(bkpName);

                                    File.Move(pkcs7zip.FullName, bkpName);

                                    info.Arquivos.Add(pkcs7zip.Name);

                                    db.InserirControleFrontis(info.CNPJ, pkcs7zip.Name);
                                }
                                else
                                {
                                    logger.InfoFormat("Removendo {0}, ja obtido e manipulado", pkcs7zip.FullName);
                                    File.Delete(pkcs7zip.FullName);
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.Error("Arquivo [" + pkcs7zip.Name + "]: " + ex.Message, ex);
                            }
                        }
                    }
                }

                bProcessarFrontis = false;
            }
            catch (Exception ex)
            {
                logger.Error("ProcessarArquivosFrontis: " + ex.Message, ex);
                bProcessarFrontis = false;
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


        private void ConverteXmlPerfilMensal(string filename)
        {
            FileInfo finfo = new FileInfo(filename);

            string xmlFile = finfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(finfo.Name) + ".xml";

            if (File.Exists(xmlFile))
            {
                logger.WarnFormat("Arquivo [{0}] ja foi gerado, ignorando", xmlFile);
                return;
            }

            List<InformeFundo> lstInformeFundo = new List<InformeFundo>();
            string dataCompetencia = DateTime.Now.ToString("dd/MM/yyyy");
            string dataGeracao = DateTime.Now.ToString("dd/MM/yyyy");

            using (ExcelPackage xlPackage = new ExcelPackage(finfo))
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                //int iCol = 1;
                int iRow = 2;

                InformeFundo informeFundo = null;
                FormaDivulgacao formaDivulg = null;
                FormaDivulgacaoCotista formaDivulgCot = null;
                PrestadorServico prestador = null;

                string celLabel = "";
                while (worksheet.GetValue(iRow, 1) != null)
                {
                    celLabel = worksheet.GetValue(iRow, 1).ToString().TrimEnd();

                    string celvalue = "";

                    if (worksheet.GetValue(iRow, 2) != null)
                        celvalue = SecurityElement.Escape(worksheet.GetValue(iRow, 2).ToString().TrimEnd());

                    logger.DebugFormat("Cell({0}, 1){1}=[{2}]", iRow, celLabel, celvalue);

                    if (!String.IsNullOrEmpty(celvalue))
                    {
                        switch (celLabel)
                        {
                            case "COD DOC": break;
                            case "VERSÃO": break;
                            case "DT COMPT":
                                DateTime aux;
                                if (DateTime.TryParse(celvalue, out aux))
                                    dataCompetencia = aux.ToString("dd/MM/yyyy");

                                break;
                            case "DT GER":
                                DateTime aux1;
                                if (DateTime.TryParse(celvalue, out aux1))
                                    dataGeracao = aux1.ToString("dd/MM/yyyy");

                                break;
                            case "CNPJ FDO":
                                if (informeFundo != null)
                                    lstInformeFundo.Add(informeFundo);
                                informeFundo = new InformeFundo();
                                informeFundo.Cnpj = celvalue;
                                break;
                            case "PERIOD.":
                                if (celvalue.Length > 250)
                                {
                                    logger.Warn("Campo 'PERIOD.' com mais de 250 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 250);
                                }
                                informeFundo.Periodicidade = celvalue;
                                break;
                            case "LOCAL DIV.":
                                formaDivulg = new FormaDivulgacao();
                                if (celvalue.Length > 300)
                                {
                                    logger.Warn("Campo 'LOCAL DIV.' com mais de 300 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 300);
                                }
                                formaDivulg.LocalDivulgacao = celvalue;
                                break;
                            case "MEIO DIV.":
                                formaDivulg.CodigoMeioDivulg = celvalue;
                                break;
                            case "FORMA DIV.":
                                if (celvalue.Length > 100)
                                {
                                    logger.Warn("Campo 'FORMA DIV.' com mais de 100 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 100);
                                }
                                formaDivulg.DescricaoForma = celvalue;
                                informeFundo.FormasDivulgacao.Add(formaDivulg);
                                break;
                            case "LOCAL DIV. COT.":
                                formaDivulgCot = new FormaDivulgacaoCotista();
                                if (celvalue.Length > 100)
                                {
                                    logger.Warn("Campo 'LOCAL DIV. COT' com mais de 100 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 100);
                                }
                                formaDivulgCot.LocalDivulgacao = celvalue;
                                break;
                            case "MEIO DIV. COT.":
                                formaDivulgCot.CodigoMeioDivulg = celvalue;
                                break;
                            case "FORMA DIV. COT.":
                                if (celvalue.Length > 100)
                                {
                                    logger.Warn("Campo 'FORMA DIV. COT.' com mais de 100 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 100);
                                }
                                formaDivulgCot.FormaSolicitacao = celvalue;
                                informeFundo.FormasDivulgacaoCotista.Add(formaDivulgCot);
                                break;
                            case "EXPOSICAO":
                                if (celvalue.Length > 2000)
                                {
                                    logger.Warn("Campo 'EXPOSICAO' com mais de 2000 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2000);
                                }
                                informeFundo.Exposicao = celvalue;
                                break;
                            case "VOTO (S ou N)":
                                if (celvalue.ToUpperInvariant().Equals("N"))
                                    informeFundo.VotoGestor = false;
                                else
                                    informeFundo.VotoGestor = true;
                                break;
                            case "DESC.VOTO":
                                if (celvalue.Length > 1000)
                                {
                                    logger.Warn("Campo 'DESC.VOTO' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 1000);
                                }
                                informeFundo.PoliticaExercicioVoto = celvalue;
                                break;
                            case "TRIBUTACAO":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'TRIBUTACAO' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.PoliticaTributacao = celvalue;
                                break;
                            case "ADM.DE RISCO":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'ADM.DE RISCO' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.PoliticaAdmRisco = celvalue;
                                break;
                            case "AG. RISCO (Sim ou Não)":
                                break;
                            case "CNPJ AG.RISCO": break;
                            case "PRESTADOR": break;
                            case "CLASSIF.AG.RISCO": break;
                            case "DISCL_ADV": break;
                            case "AP.ADM.":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'AP.ADM.' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.ApresAdministrador = celvalue;
                                break;
                            case "AP.GESTOR":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'AP.GESTOR' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.ApresGestor = celvalue;
                                break;
                            case "DS. SERV.PREST.":
                                prestador = new PrestadorServico();
                                if (celvalue.Length > 100)
                                {
                                    logger.Warn("Campo 'DS. SERV.PREST' com mais de 100 caracteres, truncando");
                                    prestador.Descricao = celvalue.Substring(0, 100);
                                }
                                else
                                    prestador.Descricao = celvalue;
                                break;
                            case "NOME PREST.":
                                if (celvalue.Length > 100)
                                {
                                    logger.Warn("Campo 'NOME PREST.' com mais de 100 caracteres, truncando");
                                    prestador.Nome = celvalue.Substring(0, 100);
                                }
                                else
                                    prestador.Nome = celvalue;
                                informeFundo.PrestadoresServico.Add(prestador);
                                break;
                            case "COD.DIST.OF.":
                                informeFundo.CodigoDistribuicaoOferta = celvalue;
                                break;
                            case "DETALHE":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'DETALHE' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.PoliticaDistribuicaoOferta = celvalue;
                                break;
                            case "INFORM ANBIMA":
                                if (celvalue.Length > 2500)
                                {
                                    logger.Warn("Campo 'INFORM ANBIMA' com mais de 2500 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 2500);
                                }
                                informeFundo.InformeAmbima = celvalue;
                                break;
                            case "INFORM RELEV":
                                if (celvalue.Length > 4000)
                                {
                                    logger.Warn("Campo 'INFORM RELEV' com mais de 4000 caracteres, truncando");
                                    celvalue = celvalue.Substring(0, 4000);
                                }
                                informeFundo.InformeRelevante = celvalue;
                                break;
                        }

                    }
                    iRow++;
                }

                if (informeFundo != null)
                    lstInformeFundo.Add(informeFundo);

            } // the using statement calls Dispose() which closes the package.




            StringBuilder xmlContent = new StringBuilder();

            xmlContent.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlContent.AppendLine("<DOC_ARQ xmlns=\"urn:infocomp\">");
            xmlContent.AppendLine("<CAB_INFORM>");
            xmlContent.AppendLine("<COD_DOC>40</COD_DOC>");
            xmlContent.AppendLine("<VERSAO>3.0</VERSAO>");
            xmlContent.AppendFormat("<DT_COMPT>{0}</DT_COMPT>", dataCompetencia);
            xmlContent.AppendLine();
            xmlContent.AppendFormat("<DT_GERAC_ARQ>{0}</DT_GERAC_ARQ>", dataGeracao);
            xmlContent.AppendLine();
            xmlContent.AppendLine("</CAB_INFORM>");
            xmlContent.AppendLine("<PERFIL_MENSAL>");

            foreach (InformeFundo informe in lstInformeFundo)
            {
                xmlContent.AppendLine("<ROW_PERFIL>");

                xmlContent.AppendFormat("<CNPJ_FDO>SUBSTITUA ESTA FRASE PELO CNPJ DO FUNDO</CNPJ_FDO>");

 	 	 	    xmlContent.AppendLine("<NR_CLIENT>");
 	 	 	 	xmlContent.AppendFormat("<NR_PF_PRIV_BANK>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES PESSOAS FÍSICAS PRIVATE BANKING (Obrigátorio).</NR_PF_PRIV_BANK>");
 	 	 	 	xmlContent.AppendFormat("<NR_PF_VARJ>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES PESSOAS FÍSICAS VAREJO (Obrigátorio).</NR_PF_VARJ>");
 	 	 	 	xmlContent.AppendFormat("<NR_PJ_N_FINANC_PRIV_BANK>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES PESSOAS JURÍDICAS NÃO-FINANCEIRAS PRIVATE BANKING (Obrigátorio).</NR_PJ_N_FINANC_PRIV_BANK>");
 	 	 	 	xmlContent.AppendFormat("<NR_PJ_N_FINANC_VARJ>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES PESSOAS JURÍDICAS NÃO-FINANCEIRAS VAREJO (Obrigátorio).</NR_PJ_N_FINANC_VARJ>");
 	 	 	 	xmlContent.AppendFormat("<NR_BNC_COMERC>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES BANCOS COMERCIAIS(Obrigátorio).</NR_BNC_COMERC>");
 	 	 	 	xmlContent.AppendFormat("<NR_PJ_CORR_DIST>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES CORRETORAS OU DISTRIBUIDORAS(Obrigátorio).</NR_PJ_CORR_DIST>");
 	 	 	 	xmlContent.AppendFormat("<NR_PJ_OUTR_FINANC>SUBSTITUA ESTA FRASE PELO NÚMERO DE OUTROS CLIENTES PESSOAS JURÍDICAS FINANCEIRAS(Obrigátorio).</NR_PJ_OUTR_FINANC>");
 	 	 	 	xmlContent.AppendFormat("<NR_INV_N_RES>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES INVESTIDORES NÃO RESIDENTES (Obrigátorio).</NR_INV_N_RES>");
 	 	 	 	xmlContent.AppendFormat("<NR_ENT_AB_PREV_COMPL>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES ENTIDADES ABERTAS DE PREVIDÊNCIA COMPLEMENTAR (Obrigátorio).</NR_ENT_AB_PREV_COMPL>");
 	 	 	 	xmlContent.AppendFormat("<NR_ENT_FC_PREV_COMPL>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES ENTIDADES FECHADAS DE PREVIDÊNCIA COMPLEMENTAR (Obrigátorio).</NR_ENT_FC_PREV_COMPL>");
 	 	 	 	xmlContent.AppendFormat("<NR_REG_PREV_SERV_PUB>SUBSTITUA ESTA FRASE PELO NÚMERO DE REGIME PRÓPRIO DE PREVIDÊNCIA DOS SERVIDORES PÚBLICOS (Obrigátorio).</NR_REG_PREV_SERV_PUB>");
 	 	 	 	xmlContent.AppendFormat("<NR_SOC_SEG_RESEG>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES SOCIEDADES SEGURADORAS OU RESSEGURADORAS (Obrigátorio).</NR_SOC_SEG_RESEG>");
 	 	 	 	xmlContent.AppendFormat("<NR_SOC_CAPTLZ_ARRENDM_MERC>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES SOCIEDADES DE CAPITALIZAÇÃO E DE ARRENDAMENTOS MERCANTIS (Obrigátorio).</NR_SOC_CAPTLZ_ARRENDM_MERC>");
 	 	 	 	xmlContent.AppendFormat("<NR_FDOS_CLUB_INV>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES FUNDOS E CLUBES DE INVESTIMENTO (Obrigátorio).</NR_FDOS_CLUB_INV>");
 	 	 	 	xmlContent.AppendFormat("<NR_COTST_DISTR_FDO>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES COTISTAS DE DISTRIBUIDORES DO FUNDO - DISTRIBUIÇÃO POR CONTA E ORDEM (Obrigátorio).</NR_COTST_DISTR_FDO>");
 	 	 	 	xmlContent.AppendFormat("<NR_OUTROS_N_RELAC>SUBSTITUA ESTA FRASE PELO NÚMERO DE CLIENTES COTISTAS NÃO RELACIONADOS (Obrigátorio).</NR_OUTROS_N_RELAC>");
 	 	 	    xmlContent.AppendLine("</NR_CLIENT>");

 	 	 	    xmlContent.AppendLine("<DISTR_PATRIM>");
 	 	 	 	xmlContent.AppendFormat("<PR_PF_PRIV_BANK>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES PESSOAS FÍSICAS PRIVATE BANKING (Obrigátorio).</PR_PF_PRIV_BANK>");
 	 	 	 	xmlContent.AppendFormat("<PR_PF_VARJ>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES PESSOAS FÍSICAS VAREJO (Obrigátorio).</PR_PF_VARJ>");
 	 	 	 	xmlContent.AppendFormat("<PR_PJ_N_FINANC_PRIV_BANK>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES PESSOAS JURÍDICAS NÃO-FINANCEIRAS PRIVATE BANKING (Obrigátorio).</PR_PJ_N_FINANC_PRIV_BANK>");
 	 	 	 	xmlContent.AppendFormat("<PR_PJ_N_FINANC_VARJ>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES PESSOAS JURÍDICAS NÃO-FINANCEIRAS VAREJO (Obrigátorio).</PR_PJ_N_FINANC_VARJ>");
 	 	 	 	xmlContent.AppendFormat("<PR_BNC_COMERC>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES BANCOS COMERCIAIS (Obrigátorio).</PR_BNC_COMERC>");
 	 	 	 	xmlContent.AppendFormat("<PR_PJ_CORR_DIST>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES CORRETORAS OU DISTRIBUIDORAS (Obrigátorio).</PR_PJ_CORR_DIST>");
 	 	 	 	xmlContent.AppendFormat("<PR_PJ_OUTR_FINANC>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR OUTROS CLIENTES PESSOAS JURÍDICAS FINANCEIRAS (Obrigátorio).</PR_PJ_OUTR_FINANC>");
 	 	 	 	xmlContent.AppendFormat("<PR_INV_N_RES>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES INVESTIDORES NÃO RESIDENTES (Obrigátorio).</PR_INV_N_RES>");
 	 	 	 	xmlContent.AppendFormat("<PR_ENT_AB_PREV_COMPL>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES ENTIDADES ABERTAS DE PREVIDÊNCIA COMPLEMENTAR (Obrigátorio).</PR_ENT_AB_PREV_COMPL>");
 	 	 	 	xmlContent.AppendFormat("<PR_ENT_FC_PREV_COMPL>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES ENTIDADES FECHADAS DE PREVIDÊNCIA COMPLEMENTAR (Obrigátorio).</PR_ENT_FC_PREV_COMPL>");
 	 	 	 	xmlContent.AppendFormat("<PR_REG_PREV_SERV_PUB>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO PELO REGIME PRÓPRIO DE PREVIDÊNCIA DOS SERVIDORES PÚBLICOS (Obrigátorio).</PR_REG_PREV_SERV_PUB>");
 	 	 	 	xmlContent.AppendFormat("<PR_SOC_SEG_RESEG>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES SOCIEDADES SEGURADORAS OU RESSEGURADORAS (Obrigátorio).</PR_SOC_SEG_RESEG>");
 	 	 	 	xmlContent.AppendFormat("<PR_SOC_CAPTLZ_ARRENDM_MERC>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES SOCIEDADES DE CAPITALIZAÇÃO E DE ARRENDAMENTOS MERCANTIS (Obrigátorio).</PR_SOC_CAPTLZ_ARRENDM_MERC>");
 	 	 	 	xmlContent.AppendFormat("<PR_FDOS_CLUB_INV>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES FUNDOS E CLUBES DE INVESTIMENTO (Obrigátorio).</PR_FDOS_CLUB_INV>");
 	 	 	 	xmlContent.AppendFormat("<PR_COTST_DISTR_FDO>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES COTISTAS DE DISTRIBUIDORES DO FUNDO - DISTRIBUIÇÃO POR CONTA E ORDEM (Obrigátorio).</PR_COTST_DISTR_FDO>");
 	 	 	 	xmlContent.AppendFormat("<PR_OUTROS_N_RELAC>SUBSTITUA ESTA FRASE PELO PERCENTUAL DO PL DETIDO POR CLIENTES NÃO RELACIONADOS (Obrigátorio).</PR_OUTROS_N_RELAC>");

                xmlContent.AppendLine("</DISTR_PATRIM>");

                xmlContent.AppendFormat("<PRAZ_MED_CART_TIT>SUBSTITUA ESTA FRASE PELO PRAZO MÉDIO EM MESES DA CARTEIRA DE TÍTULOS DO FUNDO NO ÚLTIMO DIA ÚTIL DO MÊS (obrigatório)</PRAZ_MED_CART_TIT>");
                xmlContent.AppendFormat("<TOTAL_RECURS_EXTER>SUBSTITUA ESTA FRASE PELO VALOR TOTAL DOS CONTRATOS DE COMPRA DE US$ LIQUIDADOS NO MÊS (obrigatório).</TOTAL_RECURS_EXTER>");
                xmlContent.AppendFormat("<TOTAL_RECURS_BR>SUBSTITUA ESTA FRASE PELO VALOR TOTAL DE CONTRATOS DE VENDA DE US$ LIQUIDADOS NO MÊS (obrigatório)</TOTAL_RECURS_BR>");
                xmlContent.AppendFormat("<TOT_ATIVOS_P_RELAC>SUBSTITUA ESSA FRASE PELO VALOR TOTAL DOS ATIVOS (EM % DO PL) EM ESTOQUE DE EMISSÃO DE PARTES RELACIONADAS (Obrigatório).</TOT_ATIVOS_P_RELAC>");
                xmlContent.AppendFormat("<TOT_ATIVOS_CRED_PRIV>SUBSTITUA ESSA FRASE PELO VALOR DO TOTAL DE ATIVOS DE CRÉDITO PRIVADO (EM % DO PL) EM ESTOQUE.(Obrigatório) </TOT_ATIVOS_CRED_PRIV>");

                xmlContent.AppendLine("</ROW_PERFIL>");
            }
            xmlContent.AppendLine("</PERFIL_MENSAL>");
            xmlContent.AppendLine("</DOC_ARQ>");

            File.WriteAllText(xmlFile, xmlContent.ToString(), Encoding.UTF8);
        }

        public void GerarPlanilhaPerfilMensal()
        {

            if (bGerandoPlanilhaPerfilMensal)
            {
                logger.Warn("Gerando planilhas, retornando");
                return;
            }

            bGerandoPlanilhaPerfilMensal = true;

            logger.Info("Inicio GerarPlanilhaPerfilMensal");

            PersistenciaDB db = new PersistenciaDB();

            List<FundoLaminaPerfil> listaFundo = db.ObterFundosLaminaPefil();

            logger.Info("Serao geradas " + listaFundo.Count + " planilhas de fundos");

            // Carrega a tabela com perfis de cliente Sinacor x Financial
            Dictionary<int, TipoPerfilClienteInfo> perfis = db.ObterTiposPerfilCliente();

            logger.Info("Carregado tabela de perfis de cliente Financial x Sinacor ");

            string dirLaminaPerfil = ConfigurationManager.AppSettings["DirGeracaoLaminaPerfil"].ToString();

            foreach (FundoLaminaPerfil fundo in listaFundo)
            {
                string fileName = String.Format("{0}\\{1}-{2}.xlsx", dirLaminaPerfil, fundo.DsFundo, DateTime.Now.ToString("yyyy-MM-dd"));

                if (File.Exists(fileName))
                {
                    logger.WarnFormat("Arquivo [{0}] ja foi gerado, ignorando", fileName);
                    return;
                }

                logger.Info("Gerando arquivo [" + fileName + "]");

                // Obtem os cotistas do fundo
                Dictionary<int, CotistaInfo> cotistas = new Dictionary<int, CotistaInfo>();
                    //db.ObterCotistasFundo(fundo.IdCarteiraFinancial, DateTime.Now.ToString("yyyy-MM-dd"));

                logger.InfoFormat("Fundo [{0}] tem {1} cotistas", fundo.DsFundo, cotistas.Count);

                // Busca no Sinacor o perfil do cliente
                db.PopulateCotistaInfoSinacor(ref cotistas);

                foreach (CotistaInfo cotista in cotistas.Values)
                {
                    if (perfis.ContainsKey(cotista.TipoClienteSinacor))
                    {
                        cotista.TipoClienteFinancial = perfis[cotista.TipoClienteSinacor].IDPerfil;
                    }
                }

                IEnumerable<IGrouping<int, int>> query = 
                    cotistas.Values.GroupBy(o => o.TipoClienteFinancial, o => o.IDCotista);

                foreach (IGrouping<int, int> group in query)
                {
                    logger.Debug("TipoClienteFinancial: " + group.Key);

                    foreach (int idcotista in group)
                    {
                        logger.Debug("     Cotista: " + idcotista);
                    }
                }

                FileInfo finfo = new FileInfo(fileName);

                using (ExcelPackage xlPackage = new ExcelPackage(finfo))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Perfil");

                    //worksheet.DefaultRowHeight = 15;

                    //int iCol = 1;
                    int iRow = 2;

                    worksheet.Column(1).Style.WrapText = true;
                    worksheet.Column(1).Width = 50.0;
                    worksheet.Column(2).AutoFit(20);

                    worksheet.SetValue(iRow, 1, "CNPJ:");
                    worksheet.SetValue(iRow, 2, fundo.CpfCpnj);


                    iRow ++;
                    worksheet.SetValue(iRow, 1, "Nome:");
                    worksheet.SetValue(iRow, 2, fundo.DsFundo);


                    iRow++;
                    worksheet.SetValue(iRow, 1, "Mes de referencia:");
                    worksheet.SetValue(iRow, 2, DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy"));

                    iRow++;
                    worksheet.SetValue(iRow++, 1, "1) Número de clientes do Fundo no último dia útil do mês de referência, por tipo de cliente:");


                    List<int> lstPerfis = perfis.Values.Select(prf => prf.IDPerfil).Distinct().OrderBy(o => o).ToList();

                    foreach (int idPerfil in lstPerfis)
                    {
                        string desc = perfis.Values.Where(o => o.IDPerfil == idPerfil).Select(prf => prf.DescPerfil).First();

                        worksheet.SetValue(iRow, 1, desc.ToLower() + ":" );
                        worksheet.SetValue(iRow, 2, cotistas.Values.Count(o => o.TipoClienteFinancial == idPerfil));
                        iRow += 1;
                    }

                    iRow++;
                    worksheet.SetValue(iRow++, 1, "2) Distribuição percentual do patrimônio do Fundo no último dia útil do mês de referência, por tipo de cliente:");

                    worksheet.SetValue(iRow++, 1, "7) No último dia útil do mês de referência, qual o prazo médio da carteira de títulos do fundo? (em meses (30 dias) e calculado de acordo com a metodologia regulamentada pela RFB)");

                    worksheet.SetValue(iRow++, 1, "9) Total de recursos (em US$) enviados para o exterior para aquisição de ativos - Valor total dos contratos de compra de US$ liquidados no mês.");

                    worksheet.SetValue(iRow++, 1, "10) Total de recursos (em US$) ingressados no Brasil referente à venda de ativos - Total de contratos de venda de US$ liquidados no mês.");

                    worksheet.SetValue(iRow++, 1, "19) Total dos ativos (em % do PL) em estoque de emissão de partes relacionadas. O termo parte relacionada é aquele do artigo 86, § 1º, incs. II e III, da Instrução CVM nº 409, de 2004.");

                    worksheet.SetValue(iRow++, 1, "21) Total dos ativos de crédito privado (em % do PL) em estoque.");

                    xlPackage.Save();
                }
            }

            bGerandoPlanilhaPerfilMensal = false;
        }


        public void BuscarArquivosCetip()
        {
            try
            {
                if (bBuscarArquivosCetip)
                {
                    logger.Info("Outra operacao de download em SFTP em andamento...");
                    return;
                }
                bBuscarArquivosCetip = true;


                string bkpPath = ConfigurationManager.AppSettings["CETIPEscritBkp"].ToString();


                if (!Directory.Exists(sftpLocalDir))
                    Directory.CreateDirectory(sftpLocalDir);

                SFtpPubKeyClient sftpClient = new SFtpPubKeyClient();

                sftpClient.SFTPHost = ConfigurationManager.AppSettings["CETIPHost"].ToString();
                sftpClient.SFTPUser = ConfigurationManager.AppSettings["CETIPUser"].ToString();
                sftpClient.PrivateKeyFile = ConfigurationManager.AppSettings["CETIP_PK_FILE"].ToString();
                sftpClient.PrivateKeyPasswd = ConfigurationManager.AppSettings["CETIP_PK_PASSWD"].ToString();

                if (ConfigurationManager.AppSettings["CETIPPort"] != null)
                {
                    sftpClient.SFTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["CETIPPort"].ToString());
                }

                List<string> searchPattern = new List<string>();
                List<string> exceptionList = new List<string>();

                PersistenciaDB db = new PersistenciaDB();

                searchPattern.Add("ArqsBatch.zip");
                searchPattern.Add("RelsBatch.zip");

                DirectoryInfo dirBkpInfo = new DirectoryInfo(bkpPath);

                List<FileInfo> arqsBatchZipProcessados = dirBkpInfo.GetFiles("*_ArqsBatch.zip").ToList();
                foreach (FileInfo zipProcessado in arqsBatchZipProcessados)
                {
                    exceptionList.Add(zipProcessado.Name);
                }


                if (searchPattern.Count > 0)
                {
                    string[] cetipRemoteDirArr = cetipRemoteDirList.Split(';');

                    foreach (string cetipRemoteDir in cetipRemoteDirArr)
                    {
                        logger.Info("Verificando arquivos no diretorio remoto [" + cetipRemoteDir + "]");
                        sftpClient.TransferirArquivos(cetipRemoteDir, cetipLocalDir, searchPattern.ToArray(), exceptionList.ToArray());

                        Thread.Sleep(5000);
                    }
                }
                else
                {
                    logger.Warn("Nao ha mais arquivos para baixar hoje");
                }
            }
            catch (Exception ex)
            {
                logger.Error("BuscarFTP: " + ex.Message, ex);
            }
            finally
            {
                bBuscarArquivosCetip = false;
            }
        }

        private class ResultLine
        {
            public ResultLine() { }

            public string DataMovto { get; set; }
            public string CodigoIF { get; set; }
            public string CpfCnpj { get; set; }
            public string RazaoSocial { get; set; }

            public double QtdeCotas { get; set; }
        }

        public bool ConciliarCETIP( string filename)
        {
            try
            {

                FileInfo fileInfo = new FileInfo(filename);

                List<RelatEscrituracaoItemInfo> lstRelatorio = new List<RelatEscrituracaoItemInfo>();

                DataTable tbDSTotais = ParserArquivosCetip.CarregarArquivoTotaisPrivadosEscriturador(filename);

                IEnumerable<ResultLine> linhasQuery = (
                    from linha in tbDSTotais.AsEnumerable()
                    group linha by new { CpfCnpj = linha.Field<string>("CNPJ_INVESTIDOR"), CodigoIF = linha.Field<string>("CODIGO_IF") }
                        into grp
                        select new ResultLine {
                            CodigoIF = grp.Key.CodigoIF, 
                            CpfCnpj = grp.Key.CpfCnpj,
                            QtdeCotas = grp.Sum(linha => Convert.ToDouble(linha.Field<string>("QTD_DISTRIBUICAO").TruncaDecimais(4), ciBr)),
                            RazaoSocial = grp.First().Field<string>("RAZAO_SOCIAL_INVESTIDOR"),
                            DataMovto = grp.First().Field<string>("DATA_MOVIMENTO")  }
                        ).ToList();

                IEnumerable<ResultLine> linhasValidas = 
                    linhasQuery.OrderBy(p => p.CodigoIF);

                PersistenciaDB db = new PersistenciaDB();

                Dictionary<string, CorrelacaoCarteiraCetipInfo> dctCetipCarteira = db.CarregarCorrelacaoCarteirasCetip();
                Dictionary<int, string> dctDescCarteira = db.CarregarDescricaoCarteirasFinancial( dctCetipCarteira.Values.Select(o=> o.IDCarteira).ToList() );

                double total = 0.0;
                double totalDistribuido = 0.0;
                int iQtdeCotistas = 0;
                string codigoIF = string.Empty;
                int idCarteiraFinancial = 0;
                List<string> lstMsgRelatorio = new List<string>();
                List<string> lstDetalheRelatorio = new List<string>();
                List<string> lstDetalheInconformidades = new List<string>();
                DateTime dataMovto = DateTime.Now;
                Dictionary<string, CotistaCarteiraInfo> dctCotistaInfo = new Dictionary<string,CotistaCarteiraInfo>();
                foreach (ResultLine linha in linhasValidas)
                {
                    logger.DebugFormat("Linha: [{0}] [{1}] [{2}] [{3}] [{4}]",
                        linha.DataMovto,
                        linha.CodigoIF,
                        linha.RazaoSocial,
                        linha.CpfCnpj,
                        linha.QtdeCotas);

                    string auxCodigoIF = linha.CodigoIF.Trim();
                    dataMovto = DateTime.ParseExact(linha.DataMovto, "yyyyMMdd", CultureInfo.InvariantCulture);
                    DateTime dataAbertura = dataMovto.AddDays(1);

                    while (!IsWorkDay(dataAbertura))
                    {
                        dataAbertura = dataAbertura.AddDays(1);
                    }

                    if (String.IsNullOrEmpty(codigoIF))
                    {
                        codigoIF = auxCodigoIF;
                    }

                    // Ao trocar de codigo, armazena as mensagens e zera totais
                    if (!codigoIF.Equals(auxCodigoIF))
                    {
                        if (dctCetipCarteira.ContainsKey(codigoIF))
                        {
                            double qtdeBritech = dctCotistaInfo.Values.Sum(o => o.QtdeCotas);
                            RelatEscrituracaoItemInfo relat = new RelatEscrituracaoItemInfo(codigoIF, idCarteiraFinancial, dctDescCarteira[idCarteiraFinancial]);
                            relat.DataMovto = dataMovto;
                            relat.QtdeCotistas = iQtdeCotistas;
                            relat.QtdeCotasCetip = total;
                            relat.QtdeCotasDistribuidas = totalDistribuido;
                            relat.QtdeLivro = qtdeBritech - totalDistribuido;
                            relat.QtdeVM = qtdeBritech;
                            relat.ListaMsgs.AddRange(lstMsgRelatorio);
                            relat.Detalhes.AddRange(lstDetalheRelatorio);
                            relat.Inconformidades.AddRange(lstDetalheInconformidades);

                            lstRelatorio.Add(relat);

                            if (!dctPerfCetip.ContainsKey(idCarteiraFinancial))
                                dctPerfCetip.Add(idCarteiraFinancial, new PerformanceCETIPInfo());

                            dctPerfCetip[idCarteiraFinancial].QtdeBritech = qtdeBritech;
                            dctPerfCetip[idCarteiraFinancial].QtdeCetip = total;
                            dctPerfCetip[idCarteiraFinancial].TipoEscrituracao = dctCetipCarteira[codigoIF].TipoEscrituracao;
                        }

                        iQtdeCotistas = 0;
                        total = 0.0;
                        totalDistribuido = 0.0;
                        lstMsgRelatorio.Clear();
                        lstDetalheRelatorio.Clear();
                        lstDetalheInconformidades.Clear();
                        codigoIF = auxCodigoIF;
                    }

                    if (!dctCetipCarteira.ContainsKey(auxCodigoIF))
                    {
                        logger.Info("CodigoIF [" + auxCodigoIF + "] nao encontrado na tabela de correlacao, ignorando");
                        continue;
                    }

                    idCarteiraFinancial = dctCetipCarteira[auxCodigoIF].IDCarteira;
                    string cpfCnpj = linha.CpfCnpj.Replace("-", "").Replace(".", "").Replace("/", "");

                    // Carrega cotistas da carteira, se ja nao houver
                    dctCotistaInfo = db.ObterCotistaFundo(idCarteiraFinancial, dataMovto);
                    double qtdeCotas = linha.QtdeCotas;

                    if (!dctCotistaInfo.ContainsKey(cpfCnpj))
                    {
                        logger.ErrorFormat("Nao ha cotista [{0}]  para carteira [{1}]", cpfCnpj, idCarteiraFinancial);

                        StringBuilder linhaInconformidade = new StringBuilder();


                        linhaInconformidade.AppendFormat("<td valign=bottom style='border-style: solid; border-width: 1px; border-color: #000000;'>{0}</td>", dataMovto.ToString("dd/MM/yyyy"));
                        linhaInconformidade.AppendFormat("<td valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0}</td>", auxCodigoIF);
                        linhaInconformidade.AppendFormat("<td valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0}</td>", idCarteiraFinancial);
                        linhaInconformidade.AppendFormat("<td valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0}</td>", cpfCnpj);
                        linhaInconformidade.AppendFormat("<td valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0}</td>", linha.RazaoSocial);
                        linhaInconformidade.AppendFormat("<td align=right valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0:0.0000}</td>", qtdeCotas);
                        linhaInconformidade.AppendFormat("<td align=right valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Não encontrado na BRITECH</td>", qtdeCotas);

                        lstDetalheInconformidades.Add(linhaInconformidade.ToString());

                        total += qtdeCotas;
                        iQtdeCotistas++;

                        continue;
                    }

                    CotistaCarteiraInfo info = dctCotistaInfo[cpfCnpj];


                    string observ = "Correta";
                    if (info.QtdeCotas != qtdeCotas)
                    {
                        lstMsgRelatorio.Add(string.Format("Quantidade de cotas divergente para IF [{0}] Carteira [{1}] Investidor [{2}] Qtde [{3}] x Qtde Cetip [{4}]",
                            auxCodigoIF,
                            idCarteiraFinancial,
                            info.IDCotista,
                            info.QtdeCotas,
                            qtdeCotas)
                        );

                        observ = "<h2 style='font-family: Helvetica, sans-serif; font-size: 14px; font-weight: bold; color: #FF0000;'>DIVERGENTE</h2>";
                    }
                    else
                    {
                        lstMsgRelatorio.Add(string.Format("Quantidade VM IF [{0}] Carteira [{1}] Investidor [{2}] Qtde [{3}] x Qtde Cetip [{4}] esta correta",
                            auxCodigoIF,
                            idCarteiraFinancial,
                            info.IDCotista,
                            info.QtdeCotas,
                            qtdeCotas)
                        );

                    }

                    StringBuilder detalhe = new StringBuilder();


                    detalhe.AppendFormat("<td valign=bottom style='border-style: solid; border-width: 1px; border-color: #000000;'>{0}</td>", dataMovto.ToString("dd/MM/yyyy"));
                    detalhe.AppendFormat("<td valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0}</td>", auxCodigoIF);
                    detalhe.AppendFormat("<td valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0}</td>", idCarteiraFinancial);
                    detalhe.AppendFormat("<td valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0}</td>", info.IDCotista);
                    detalhe.AppendFormat("<td valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0}</td>", linha.RazaoSocial);
                    detalhe.AppendFormat("<td align=right valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0:0.0000}</td>", info.QtdeCotas);
                    detalhe.AppendFormat("<td align=right valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0:0.0000}</td>", qtdeCotas);
                    detalhe.AppendFormat("<td valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0}</td>", observ);
                    
                    lstDetalheRelatorio.Add(detalhe.ToString());

                    total += qtdeCotas;
                    totalDistribuido += info.QtdeCotas;
                    iQtdeCotistas++;

                    logger.DebugFormat("Valores: {0} {1}", qtdeCotas, total);
                }

                if (dctCetipCarteira.ContainsKey(codigoIF))
                {
                    double qtdeBritech = dctCotistaInfo.Values.Sum(o => o.QtdeCotas);

                    RelatEscrituracaoItemInfo relat = new RelatEscrituracaoItemInfo(codigoIF, idCarteiraFinancial, dctDescCarteira[idCarteiraFinancial]);
                    relat.DataMovto = dataMovto;
                    relat.QtdeCotistas = iQtdeCotistas;
                    relat.QtdeCotasCetip = total;
                    relat.QtdeCotasDistribuidas = totalDistribuido;
                    relat.QtdeLivro = qtdeBritech - totalDistribuido;
                    relat.QtdeVM = qtdeBritech;
                    relat.ListaMsgs.AddRange(lstMsgRelatorio);
                    relat.Detalhes.AddRange(lstDetalheRelatorio);
                    relat.Inconformidades.AddRange(lstDetalheInconformidades);

                    lstRelatorio.Add(relat);

                    if (!dctPerfCetip.ContainsKey(idCarteiraFinancial))
                        dctPerfCetip.Add(idCarteiraFinancial, new PerformanceCETIPInfo());

                    dctPerfCetip[idCarteiraFinancial].QtdeBritech = qtdeBritech;
                    dctPerfCetip[idCarteiraFinancial].QtdeCetip = total;
                    dctPerfCetip[idCarteiraFinancial].TipoEscrituracao = dctCetipCarteira[codigoIF].TipoEscrituracao;
                }


                StringBuilder msgBody = new StringBuilder();
                if (lstRelatorio.Count > 0)
                {
                    logger.Info("Relatorio de processamento CETIP [" + filename + "]");

                    foreach (RelatEscrituracaoItemInfo itemRelat in lstRelatorio)
                    {
                        msgBody.Clear();

                        string mainMsg = String.Format("Carteira [{0}] IF[{1}] Qtde CETIP [{2}] Qtde Britech/Financial [{3}] Qtde Investidores [{4}]",
                            itemRelat.IDCarteira,
                            itemRelat.CodigoIF,
                            itemRelat.QtdeCotasCetip,
                            itemRelat.QtdeCotasDistribuidas,
                            itemRelat.QtdeCotistas);

                        logger.Info(mainMsg);

                        msgBody.AppendLine("<tr>");
						msgBody.AppendLine("<td>");
                        msgBody.AppendFormat("<h2 style='font-family: Helvetica, sans-serif; font-size: 14px; font-weight: bold; line-height: 20px; color: #4e443c;'>{0} - Carteira {1} - {2}</h2>", itemRelat.CodigoIF, itemRelat.IDCarteira, itemRelat.DescCarteira);
						msgBody.AppendLine("</td>");
					    msgBody.AppendLine("</tr>");
                     
                        msgBody.AppendLine("<tr>");
						msgBody.AppendLine("<td>");
                        msgBody.AppendFormat("<p style='font-family: Helvetica, sans-serif; font-size: 14px; line-height: 20px; color: #808285;'>Quantidade Distribuida CETIP: {0:0.0000}</p>", itemRelat.QtdeCotasCetip);
						msgBody.AppendLine("</td>");
					    msgBody.AppendLine("</tr>");

                        msgBody.AppendLine("<tr>");
                        msgBody.AppendLine("<td>");
                        msgBody.AppendFormat("<p style='font-family: Helvetica, sans-serif; font-size: 14px; line-height: 20px; color: #808285;'>Quantidade Distribuida Britech: {0:0.0000}</p>", itemRelat.QtdeCotasDistribuidas);
                        msgBody.AppendLine("</td>");
                        msgBody.AppendLine("</tr>");

                        if (dctCetipCarteira[itemRelat.CodigoIF].TipoEscrituracao.Equals(TipoEscritCetip.TIPO_ESCRITURACAO_DEBENTURES))
                        {
                            if (itemRelat.QtdeLivro > 0.0 ) 
                            {
                                msgBody.AppendLine("<tr>");
                                msgBody.AppendLine("<td>");
                                msgBody.AppendFormat("<p style='font-family: Helvetica, sans-serif; font-size: 14px; line-height: 20px; color: #808285;'>Quantidade em Livro: {0:0.0000}</p>", itemRelat.QtdeLivro);
                                msgBody.AppendLine("</td>");
                                msgBody.AppendLine("</tr>");
                            }

                            msgBody.AppendLine("<tr>");
                            msgBody.AppendLine("<td>");
                            msgBody.AppendFormat("<p style='font-family: Helvetica, sans-serif; font-size: 14px; line-height: 20px; color: #808285;'>Quantidade Total VM: {0:0.0000}</p>", itemRelat.QtdeVM);
                            msgBody.AppendLine("</td>");
                            msgBody.AppendLine("</tr>");

                            msgBody.AppendLine("<tr>");
                            msgBody.AppendLine("<td>");
                            msgBody.AppendFormat("<p style='font-family: Helvetica, sans-serif; font-size: 14px; line-height: 20px; color: #808285;'>Quantidade de Detentores: {0}</p>", itemRelat.QtdeCotistas);
                            msgBody.AppendLine("</td>");
                            msgBody.AppendLine("</tr>");
                        }
                        else
                        {
                            msgBody.AppendLine("<tr>");
                            msgBody.AppendLine("<td>");
                            msgBody.AppendFormat("<p style='font-family: Helvetica, sans-serif; font-size: 14px; line-height: 20px; color: #808285;'>Quantidade Total: {0:0.0000}</p>", itemRelat.QtdeVM);
                            msgBody.AppendLine("</td>");
                            msgBody.AppendLine("</tr>");

                            msgBody.AppendLine("<tr>");
                            msgBody.AppendLine("<td>");
                            msgBody.AppendFormat("<p style='font-family: Helvetica, sans-serif; font-size: 14px; line-height: 20px; color: #808285;'>Quantidade de Cotistas: {0}</p>", itemRelat.QtdeCotistas);
                            msgBody.AppendLine("</td>");
                            msgBody.AppendLine("</tr>");
                        }

                        if (itemRelat.Detalhes.Count > 0)
                        {
                            if (dctCetipCarteira[itemRelat.CodigoIF].TipoEscrituracao.Equals(TipoEscritCetip.TIPO_ESCRITURACAO_DEBENTURES))
                            {
                                msgBody.AppendLine("<tr><td><table border=0 bgcolor='f6f6f6' cellspacing=0 cellpadding=0 width=\"100%\" style='width:100%; font-family: Helvetica, sans-serif; font-size: 12px;'>");
                                msgBody.AppendLine("<tr>");
                                msgBody.Append("<th width='10%' style='border-style: solid; border-width: 1px; border-color: #000000;'>Data Movto</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Codigo IF</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>ID Carteira</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>ID Detentor VM</th>");
                                msgBody.Append("<th style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Razao Social</th>");
                                msgBody.Append("<th width='5%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Quantidade VM</th>");
                                msgBody.Append("<th width='5%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Qtde Cetip</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Conciliação</th>");
                                msgBody.AppendLine("</tr>");
                            }
                            else
                            {
                                msgBody.AppendLine("<tr><td><table border=0 bgcolor='f6f6f6' cellspacing=0 cellpadding=0 width=\"100%\" style='width:100%; font-family: Helvetica, sans-serif; font-size: 12px;'>");
                                msgBody.AppendLine("<tr>");
                                msgBody.Append("<th width='10%' style='border-style: solid; border-width: 1px; border-color: #000000;'>Data Movto</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Codigo IF</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>ID Carteira</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>ID Cotista</th>");
                                msgBody.Append("<th style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Razao Social</th>");
                                msgBody.Append("<th width='5%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Quantidade Cotas</th>");
                                msgBody.Append("<th width='5%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Qtde Cotas Cetip</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Conciliação</th>");
                                msgBody.AppendLine("</tr>");
                            }
                            foreach (string msg in itemRelat.ListaMsgs)
                            {
                                logger.InfoFormat("\t[{0}]", msg);
                            }

                            foreach (string detHtml in itemRelat.Detalhes)
                            {
                                msgBody.AppendLine("<tr>");
                                msgBody.AppendLine(detHtml);
                                msgBody.AppendLine("</tr>");
                            }

                            msgBody.AppendLine("</table></td></tr>");

                            if (itemRelat.Inconformidades.Count > 0)
                            {
                                msgBody.AppendLine("<tr>");
                                msgBody.AppendLine("<td>");
                                msgBody.AppendFormat("<p style='font-family: Helvetica, sans-serif; font-size: 14px; line-height: 20px; color: #808285;'><h2 style='font-family: Helvetica, sans-serif; font-size: 14px; font-weight: bold; color: #FF0000;'>Inconformidades encontradas:</h2></p>");
                                msgBody.AppendLine("</td>");
                                msgBody.AppendLine("</tr>");

                                msgBody.AppendLine("<tr><td><table border=0 bgcolor='f6f6f6' cellspacing=0 cellpadding=0 width=\"100%\" style='width:100%; font-family: Helvetica, sans-serif; font-size: 12px;'>");
                                msgBody.AppendLine("<tr>");
                                msgBody.Append("<th width='10%' style='border-style: solid; border-width: 1px; border-color: #000000;'>Data Movto</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Codigo IF</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>ID Carteira</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>CPF/CNPJ</th>");
                                msgBody.Append("<th style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Razao Social</th>");
                                msgBody.Append("<th width='5%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Qtde Cetip</th>");
                                msgBody.Append("<th width='10%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Observação</th>");
                                msgBody.AppendLine("</tr>");

                                foreach (string detHtml in itemRelat.Inconformidades)
                                {
                                    msgBody.AppendLine("<tr>");
                                    msgBody.AppendLine(detHtml);
                                    msgBody.AppendLine("</tr>");
                                }
                                msgBody.AppendLine("</table></td></tr>");
                            }
                        }

                        msgBody.AppendLine("<tr><td height='45'></td></tr>");

                        logger.Info("*****");

                        string subject = "Relatorio " + itemRelat.CodigoIF + " - " + itemRelat.DescCarteira + " - Arquivo CETIP [" + fileInfo.Name + "]";
                        string corpoHtml = File.ReadAllText(ConfigurationManager.AppSettings["CETIPTemplateRelat"].ToString());
                        corpoHtml = corpoHtml.Replace("[TABLE_BODY]", msgBody.ToString());
                        corpoHtml = corpoHtml.Replace("[TITLE]", subject);

                        string relatPdf = string.Format("{0}\\{1}-{2}.pdf",
                            ConfigurationManager.AppSettings["CETIPTempPdf"].ToString(),
                            itemRelat.CodigoIF,
                            DateTime.Now.ToString("yyyy-MM-dd"));

                        geraRelatorioPDF(corpoHtml, relatPdf);

                        EnviarRelatorioConciliacaoCETIP(subject, corpoHtml, relatPdf);

                        //File.Delete(relatPdf);
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error("ConciliarCETIP: " + ex.Message, ex);
                return false;
            }

            return true;
        }

        private bool EnviarRelatorioConciliacaoCETIP(string subject, string corpoHTML, string relatPDF)
        {
            try
            {
                string[] emailsTo;

                if (ConfigurationManager.AppSettings["EmailCetipRemetente"] == null)
                {
                    logger.Error("AppSetting 'EmailCetipRemetente' nao definido. Nao eh possivel enviar relatorio de envios");
                    return false;
                }

                if (ConfigurationManager.AppSettings["EmailRelatorioCetipDestinatarios"] == null)
                {
                    logger.Fatal("AppSetting 'EmailRelatorioCetipDestinatarios' deve ser definido");
                    return false;
                }

                char[] seps = { ';', ',' };
                emailsTo = ConfigurationManager.AppSettings["EmailRelatorioCetipDestinatarios"].ToString().Split(seps);

                var lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailCetipRemetente"].ToString(), emailsTo[0]);

                for (int i = 1; i < emailsTo.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsTo[i]))
                        lMensagem.To.Add(emailsTo[i]);
                }


                if ( ConfigurationManager.AppSettings["EmailCetipRemetente"] != null )
                    lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailCetipRemetente"].ToString()));


                lMensagem.Subject = subject;
                lMensagem.IsBodyHtml = true;
                lMensagem.Body = corpoHTML;
                if (!String.IsNullOrEmpty(relatPDF))
                    lMensagem.Attachments.Add(new Attachment(relatPDF));


                new SmtpClient(ConfigurationManager.AppSettings["EmailAlertaHost"].ToString()).Send(lMensagem);

                logger.Info("Email de relatorio submetido com sucesso");

                lMensagem.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error("EnviarRelatorioConciliacaoCETIP(): " + ex.Message, ex);
                return false;
            }

            return true;
        }


        public void ProcessarArquivosCetip()
        {
            try
            {
                if (bBuscarArquivosCetip)
                {
                    logger.Info("SFTP em andamento...aguardando");
                    return;
                }

                if (bProcessarCetipEscrituracao)
                {
                    logger.Info("Outra operacao de processamento dos arquivos CETIP de Escrituracao em andamento...");
                    return;
                }


                if (ConfigurationManager.AppSettings["CETIPEscritBkp"] == null ||
                    ConfigurationManager.AppSettings["CETIPEscritPath"] == null ||
                     ConfigurationManager.AppSettings["CETIPEscritPath"] == null )
                {
                    logger.Fatal("Definir as chaves 'CETIPEscritPath', 'CETIPEscritBkp' e 'CETIPBackupBackoffice'no app.config, finalizando");
                    return;
                }

                bProcessarCetipEscrituracao = true;
                logger.Info("Processando arquivos CETIP");

                PersistenciaDB db = new PersistenciaDB();

                string bkpPath = ConfigurationManager.AppSettings["CETIPEscritBkp"].ToString();
                string escritPath = ConfigurationManager.AppSettings["CETIPEscritPath"].ToString();
                string bkpBackoffice = ConfigurationManager.AppSettings["CETIPBackupBackoffice"].ToString();

                if (!Directory.Exists(bkpPath))
                {
                    Directory.CreateDirectory(bkpPath);
                }

                if (!Directory.Exists(cetipLocalDir))
                {
                    Directory.CreateDirectory(cetipLocalDir);
                }

                if (!Directory.Exists(escritPath))
                {
                    Directory.CreateDirectory(escritPath);
                }

                if (!Directory.Exists(bkpBackoffice))
                {
                    Directory.CreateDirectory(bkpBackoffice);
                }

                List<string> lstHashProcessados = new List<string>();

                List<string> searchPatterns = new List<string>();

                searchPatterns.Add("*ArqsBatch.zip");
                searchPatterns.Add("*RelsBatch.zip");

                foreach (string searchPattern in searchPatterns)
                {
                    logger.Info("Verificando arquivos ja processados");

                    DirectoryInfo dirBkpInfo = new DirectoryInfo(bkpPath);

                    List<FileInfo> arqsBatchZipProcessados = dirBkpInfo.GetFiles(searchPattern).ToList();
                    foreach (FileInfo zipProcessados in arqsBatchZipProcessados)
                    {
                        string hash = MD5HashFile(zipProcessados.FullName);

                        lstHashProcessados.Add(hash);
                    }

                    logger.Info("Buscando por arquivos [" + searchPattern + "]");

                    DirectoryInfo dirInfo = new DirectoryInfo(cetipLocalDir);

                    List<FileInfo> arqsBatchZip = dirInfo.GetFiles(searchPattern).ToList();

                    logger.Info("Encontrou " + arqsBatchZip.Count + " arquivos [" + searchPattern + "]");

                    foreach (FileInfo arqbatchzip in arqsBatchZip)
                    {
                        try
                        {
                            string hshZip = MD5HashFile(arqbatchzip.FullName);

                            if (lstHashProcessados.Any(hshZip.Contains))
                            {
                                logger.InfoFormat("Arquivo [{0}] ja processado, ignorando", arqbatchzip.FullName);
                                File.Delete(arqbatchzip.FullName);
                                continue;
                            }

                            string bkpName = String.Format(@"{0}\{1}", bkpPath, arqbatchzip.Name);
                            string backoffice = String.Format(@"{0}\{1}", bkpBackoffice, arqbatchzip.Name);

                            logger.InfoFormat("Descompactando {0} em {1}", arqbatchzip.FullName, escritPath);

                            UnzipFiles(arqbatchzip.FullName, escritPath);

                            logger.InfoFormat("Copiando ZIP para backoffice {0}", backoffice);

                            try
                            {
                                if (File.Exists(backoffice))
                                    File.Delete(backoffice);

                                File.Copy(arqbatchzip.FullName, backoffice);
                            }
                            catch (Exception ex)
                            {
                                logger.Error("Erro ao fazer backup para o backoffice : " + ex.Message, ex);
                            }

                            logger.InfoFormat("Movendo {0} para {1}", arqbatchzip.FullName, bkpName);

                            if (File.Exists(bkpName))
                                File.Delete(bkpName);

                            File.Move(arqbatchzip.FullName, bkpName);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("ProcessarArquivosCetip: " + ex.Message, ex);
                        }
                    }
                }


                searchPatterns.Clear();

                searchPatterns.Add("*DSTOTAIS_ESCRITURADOR.CETIP21-DEP");
                searchPatterns.Add("*DSTOTAISATIVOSPRIVADOS_ESCRITURADOR.CETIP21-DEP");

                searchPatterns.Add("CETIP21-DEP_*_SP_ESCRITURADOR_17472009_DSTOTAIS_ESCRITURADOR.txt");
                searchPatterns.Add("CETIP21-DEP_*_SP_ESCRITURADOR_23206001_DSTOTAIS_ESCRITURADOR.txt");
                searchPatterns.Add("CETIP21-DEP_*_SP_ESCRITURADOR_17472009_DSTOTAISATIVOSPRIVADOS_ESCRITURADOR.txt");
                searchPatterns.Add("CETIP21-DEP_*_SP_ESCRITURADOR_23206001_DSTOTAISATIVOSPRIVADOS_ESCRITURADOR.txt");

                foreach (string searchPattern in searchPatterns)
                {
                    logger.Info("Buscando por arquivos [" + searchPattern + "] em [" + escritPath + "]");

                    DirectoryInfo dirInfo = new DirectoryInfo(escritPath);

                    List<FileInfo> filesCetip = dirInfo.GetFiles(searchPattern).ToList();

                    logger.Info("Encontrou " + filesCetip.Count + " arquivos [" + searchPattern + "]");

                    foreach (FileInfo fileCetip in filesCetip)
                    {
                        try
                        {
                            // Movimenta somente se nao foi enviado ainda
                            // Caso tenha baixado novamente, ignora
                            //if (!info.Arquivos.Any(fileCetip.Name.Contains))
                            if (true)
                            {
                                string bkpName = String.Format(@"{0}\{1}", bkpPath, fileCetip.Name);

                                if (File.Exists(bkpName))
                                    File.Delete(bkpName);

                                logger.Info("Processando arquivo CETIP [" + fileCetip.FullName + "]");

                                if (ConciliarCETIP(fileCetip.FullName))
                                {
                                    File.Move(fileCetip.FullName, bkpName);

                                    //TODO: inserir controle do arquivo
                                    //info.Arquivos.Add(fileCetip.Name);

                                    //db.InserirControleFrontis(info.CNPJ, fileCetip.Name);
                                }
                            }
                            else
                            {
                                logger.InfoFormat("Removendo {0}, ja obtido e manipulado", fileCetip.FullName);
                                File.Delete(fileCetip.FullName);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Arquivo [" + fileCetip.Name + "]: " + ex.Message, ex);
                        }
                    }
                }

                bProcessarCetipEscrituracao = false;
            }
            catch (Exception ex)
            {
                logger.Error("ProcessarArquivosCetip: " + ex.Message, ex);
                bProcessarCetipEscrituracao = false;
            }

        }


        private void geraRelatorioPDF(string html, string pdfFileName)
        {
            string htmlFile = Path.GetDirectoryName(pdfFileName) + "\\" + Path.GetFileNameWithoutExtension(pdfFileName) + ".html";

            File.WriteAllText(htmlFile, html);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = ConfigurationManager.AppSettings["WKHtmlToPdf"].ToString();
            startInfo.Arguments = htmlFile + " " + pdfFileName;
            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();

            File.Delete(htmlFile);
        }

        public void GerarRelatorioPerformanceCetip()
        {
            try
            {
                StringBuilder msgBody = new StringBuilder();

                double totBritech = 0.0;
                double totCetip = 0.0;

                msgBody.AppendLine("<tr><td><table border=0 bgcolor='f6f6f6' cellspacing=0 cellpadding=0 width=\"100%\" style='width:100%; font-family: Helvetica, sans-serif; font-size: 12px;'>");
                msgBody.AppendLine("<tr>");
                msgBody.Append("<th width='25%' style='border-style: solid; border-width: 1px; border-color: #000000;'>ID Carteira</th>");
                msgBody.Append("<th width='25%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Codigo IF</th>");
                msgBody.Append("<th width='25%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Quantidade VM</th>");
                msgBody.Append("<th width='25%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>Qtde Cetip</th>");
                msgBody.AppendLine("</tr>");

                foreach (PerformanceCETIPInfo info in dctPerfCetip.Values)
                {
                    msgBody.AppendLine("<tr>");
                    msgBody.AppendFormat("<td valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0}</td>", info.IDCarteira);
                    msgBody.AppendFormat("<td valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0}</td>", info.CodigoIF);
                    msgBody.AppendFormat("<td align=right valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0:0.0000}</td>", info.QtdeBritech);
                    msgBody.AppendFormat("<td align=right valign=bottom style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0:0.0000}</td>", info.QtdeCetip);
                    msgBody.AppendLine("</tr>");

                    totBritech += info.QtdeBritech;
                    totCetip += info.QtdeCetip;
                }

                msgBody.AppendLine("<tr>");
                msgBody.Append("<th width='50%' style='border-style: solid; border-width: 1px; border-color: #000000;' colspan=2>Total</th>");
                msgBody.AppendFormat("<th width='25%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0:0.0000}</th>", totBritech);
                msgBody.AppendFormat("<th width='25%' style='border-top:solid  windowtext 1.0pt; border-left:solid black 1.0pt;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;'>{0:0.0000}</th>", totCetip);
                msgBody.AppendLine("</tr>");
                msgBody.AppendLine("</table>");

                string subject = "Escrituration Performance Report - " + DateTime.Now.ToString("dd/MM/yyyy");

                string corpoHtml = File.ReadAllText(ConfigurationManager.AppSettings["CETIPTemplateRelat"].ToString());
                corpoHtml = corpoHtml.Replace("[TABLE_BODY]", msgBody.ToString());
                corpoHtml = corpoHtml.Replace("[TITLE]", subject);


                EnviarRelatorioConciliacaoCETIP(subject, corpoHtml, null);
            }
            catch (Exception ex)
            {
                logger.Error("GerarRelatorioPerformanceCetip: " + ex.Message, ex);
            }

        }

        private bool IsWorkDay(DateTime data)
        {
            string feriados = "01/01;21/04;09/07;07/09;12/10;02/11;15/11;25/12;31/12";

            if (ConfigurationManager.AppSettings["Feriados"] != null)
            {
                feriados = ConfigurationManager.AppSettings["Feriados"].ToString();
            }

            PersistenciaDB db = new PersistenciaDB();

            List<string> feriadosSinacor = db.ObterFeriadosSinacor();

            if (feriados.Contains(data.ToString("dd/MM")) ||
                feriadosSinacor.Contains(data.ToString("yyyy/MM/dd")) ||
                data.DayOfWeek == DayOfWeek.Saturday ||
                data.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            return true;
        }

        public void BuscarDanfe()
        {
            if (bBuscarDanfe)
            {
                logger.Warn("RotinaOperacionalFIDC ja em andamento, deixando para proxima rodada");
                return;
            }

            try
            {
                bBuscarDanfe = true;

                List<string> chavesNfe = new List<string>();

                chavesNfe.AddRange(File.ReadAllLines(@"c:\temp\chavesnfe.txt"));

                foreach (string chaveNfe in chavesNfe)
                {
                    string pdf = string.Format(@"c:\temp\danfes\{0}.pdf", chaveNfe);

                    if (!File.Exists(pdf))
                    {
                        Gradual.BancoPaulista.Lib.WebCrawler paulistaCrawler = new Gradual.BancoPaulista.Lib.WebCrawler();

                        paulistaCrawler.GerarDanfe(chaveNfe, pdf);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("BuscarDanfe: " + ex.Message, ex);
            }

            bBuscarDanfe = false;
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



        //private void geraRelatorioPDF1(string html, string pdfFileName)
        //{
        //    //Create a byte array that will eventually hold our final PDF
        //    Byte[] bytes;
        //    //Boilerplate iTextSharp setup here
        //    //Create a stream that we can write to, in this case a MemoryStream
        //    using (var ms = new MemoryStream())
        //    {
        //        //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
        //        using (var doc = new Document())
        //        {

        //            //Create a writer that's bound to our PDF abstraction and our stream
        //            using (var writer = PdfWriter.GetInstance(doc, ms))
        //            {
        //                //Open the document for writing
        //                doc.Open();
        //                doc.SetPageSize(PageSize.A4);


        //                /**************************************************
        //                 * Example #1                                     *
        //                 *                                                *
        //                 * Use the built-in HTMLWorker to parse the HTML. *
        //                 * Only inline CSS is supported.                  *
        //                 * ************************************************/

        //                //Create a new HTMLWorker bound to our document
        //                using (var htmlWorker = new HTMLWorker(doc))
        //                {
        //                    htmlWorker.StartDocument();
        //                    htmlWorker.SetPageSize(PageSize.A4);
        //                    htmlWorker.SetMargins(0, 0, 0, 0);

        //                    //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
        //                    using (var sr = new StringReader(html))
        //                    {

        //                        //Parse the HTML
        //                        htmlWorker.Parse(sr);
        //                    }

        //                    htmlWorker.EndDocument();

        //                    htmlWorker.Close();
        //                }

        //                /**************************************************
        //                 * Example #2                                     *
        //                 *                                                *
        //                 * Use the XMLWorker to parse the HTML.           *
        //                 * Only inline CSS and absolutely linked          *
        //                 * CSS is supported                               *
        //                 * ************************************************/

        //                //XMLWorker also reads from a TextReader and not directly from a string
        //                //using (var srHtml = new StringReader(html))
        //                //{

        //                //    //Parse the HTML
        //                //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
        //                //}

        //                /**************************************************
        //                 * Example #3                                     *
        //                 *                                                *
        //                 * Use the XMLWorker to parse HTML and CSS        *
        //                 * ************************************************/

        //                //In order to read CSS as a string we need to switch to a different constructor
        //                //that takes Streams instead of TextReaders.
        //                //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
        //                //using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
        //                //{
        //                //    using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
        //                //    {

        //                //        //Parse the HTML
        //                //        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
        //                //    }
        //                //}


        //                doc.Close();
        //            }
        //        }

        //        //After all of the PDF "stuff" above is done and closed but **before** we
        //        //close the MemoryStream, grab all of the active bytes from the stream
        //        bytes = ms.ToArray();
        //    }

        //    //Now we just need to do something with those bytes.
        //    //Here I'm writing them to disk but if you were in ASP.Net you might Response.BinaryWrite() them.
        //    //You could also write the bytes to a database in a varbinary() column (but please don't) or you
        //    //could pass them to another function for further PDF processing.
        //    System.IO.File.WriteAllBytes(pdfFileName, bytes);
        //}
    }
}

