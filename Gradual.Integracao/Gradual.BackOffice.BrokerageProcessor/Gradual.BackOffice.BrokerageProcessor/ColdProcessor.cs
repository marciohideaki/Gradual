using System;
using System.Collections.Generic;
using log4net;
using System.Linq;
using Gradual.BackOffice.BrokerageProcessor.Db;
using Gradual.BackOffice.BrokerageProcessor.Lib.Cold;
using System.Configuration;
using System.IO;
using Gradual.BackOffice.BrokerageProcessor.Lib.Pdf;
using ICSharpCode.SharpZipLib.Zip;
using System.Net.Mail;
using System.Text;
using Gradual.BackOffice.BrokerageProcessor.Account;
using System.Globalization;
using Gradual.BackOffice.BrokerageProcessor.Processor;

namespace Gradual.BackOffice.BrokerageProcessor
{
    public class ColdProcessor
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private string dirSplitted = @"C:\Temp\ColdSplitted";
        private static ColdProcessor _me = null;
        private static DateTime dataproc = DateTime.Now;
        private static bool bMerging = false;

        private Dictionary<int, List<STRelatMergeCustodia>> dctRelatorioMerge = new Dictionary<int, List<STRelatMergeCustodia>>();

        private SortedDictionary<int, List<STCustodiaCliente>> dctCustodia = new SortedDictionary<int, List<STCustodiaCliente>>();


        public static ColdProcessor Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new ColdProcessor();
                }

                return _me;
            }
        }

        public ColdProcessor()
        {
            if (ConfigurationManager.AppSettings["DirColdSplitted"] != null)
            {
                dirSplitted = ConfigurationManager.AppSettings["DirColdSplitted"].ToString();
            }
        }

        public void CronWatchDog()
        {
            logger.Info("CronWatchDog");
        }


        public void SendColdFiles()
        {
            bool bShouldSendReport = false;

            StringBuilder relatorioEnvio = new StringBuilder();

            relatorioEnvio.AppendLine("Relatorio de Envio Arquivos CBLC " + dataproc.ToString("dd/MM/yyyy HH:mm:ss"));

            try
            {
                string diretorioSaida;
                string diretorioBkp;


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
                    
                    string [] emails = grupoEmail.EmailsTO.Split();

                    if (emails != null && emails.Length > 0)
                    {
                        for( int i=0; i < emails.Length; i++ )
                        {
                            if ( i==0 )
                                relatorioGrupo.AppendLine(Environment.NewLine + " To: " + emails[i]);
                            else
                                relatorioGrupo.AppendLine("     " + emails[i]);
                        }
                    }

                    emails = grupoEmail.EmailsCC.Split();

                    if (emails!= null && emails.Length > 0)
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

                        if (cliente.FlagBTC)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\BTC-{2}.txt", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\BTC-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\BTC-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (grupoEmail.FlagFolhaUnica)
                            {
                                pdfSaida = String.Format(@"{0}\BTC-{1}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"));
                                arqTxtSaida = String.Format(@"{0}\BTC-{1}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"));
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " BTC";
                            }
                        }

                        if (cliente.FlagExigencia)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\EXIGENCIA-{2}.txt", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\EXIGENCIA-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\EXIGENCIA-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (grupoEmail.FlagFolhaUnica)
                            {
                                pdfSaida = String.Format(@"{0}\EXIGENCIA-{1}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"));
                                arqTxtSaida = String.Format(@"{0}\EXIGENCIA-{1}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"));
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " EXIG";
                            }
                            //if (_generateExigencia(cliente.Account, diretGrupo, grupoEmail.FlagPdf))
                            //    bSendEmail = true;
                        }

                        if (cliente.FlagGarantia)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\GARANTIAS-{2}.txt", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\GARANTIAS-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\GARANTIAS-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (grupoEmail.FlagFolhaUnica)
                            {
                                pdfSaida = String.Format(@"{0}\GARANTIAS-{1}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"));
                                arqTxtSaida = String.Format(@"{0}\GARANTIAS-{1}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"));
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " GARANT";
                            }
                        }

                        if (cliente.FlagLiqInvest)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\LIQUIDACOES-{2}.txt", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\LIQUIDACOES-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\LIQUIDACOES-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (grupoEmail.FlagFolhaUnica)
                            {
                                pdfSaida = String.Format(@"{0}\LIQUIDACOES-{1}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"));
                                arqTxtSaida = String.Format(@"{0}\LIQUIDACOES-{1}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"));
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " LIQ";
                            }
                        }

                        if (cliente.FlagPosCliente)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\POSCLIENTE-{2}.txt", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\POSCLIENTE-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\POSCLIENTE-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (grupoEmail.FlagFolhaUnica)
                            {
                                pdfSaida = String.Format(@"{0}\POSCLIENTE-{1}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"));
                                arqTxtSaida = String.Format(@"{0}\POSCLIENTE-{1}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"));
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " POSCLI";
                            }
                        }

                        if (cliente.FlagPosDivBtc)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\DIVIDENDOS-{2}.txt", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\DIVIDENDOS-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\DIVIDENDOS-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (grupoEmail.FlagFolhaUnica)
                            {
                                pdfSaida = String.Format(@"{0}\DIVIDENDOS-{1}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"));
                                arqTxtSaida = String.Format(@"{0}\DIVIDENDOS-{1}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"));
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " DIVID";
                            }
                        }

                        if (cliente.FlagTermo)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\TERMO-{2}.txt", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\TERMO-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\TERMO-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (grupoEmail.FlagFolhaUnica)
                            {
                                pdfSaida = String.Format(@"{0}\TERMO-{1}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"));
                                arqTxtSaida = String.Format(@"{0}\TERMO-{1}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"));
                            }

                            if (_geraArquivoSaida(arqTxtEntrada, arqTxtSaida, pdfSaida, grupoEmail.FlagPdf, grupoEmail.FlagFolhaUnica))
                            {
                                bSendEmail = true;
                                reportItemCliente += " TERMO";
                            }
                        }

                        if (cliente.FlagCustodia)
                        {
                            string arqTxtEntrada = String.Format(@"{0}\{1}\CUSTODIA-{2}.txt", dirSplitted, cliente.Account, cliente.Account);

                            string pdfSaida = String.Format(@"{0}\CUSTODIA-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);
                            string arqTxtSaida = String.Format(@"{0}\CUSTODIA-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), cliente.Account);

                            if (grupoEmail.FlagFolhaUnica)
                            {
                                pdfSaida = String.Format(@"{0}\CUSTODIA-{1}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"));
                                arqTxtSaida = String.Format(@"{0}\CUSTODIA-{1}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"));
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


            }
            catch(Exception ex)
            {
                logger.Error("SendColdFiles: " + ex.Message, ex);
            }

            if (bShouldSendReport)
            {
                logger.Info(Environment.NewLine + relatorioEnvio.ToString() + Environment.NewLine);

                _enviaRelatorio(relatorioEnvio.ToString());
            }
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

                char[] seps = { ';',',' };
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

                DirectoryInfo dirInfo = new DirectoryInfo(diretGrupo);

                List<FileInfo> files = new List<FileInfo>();

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
                logger.Error("Cannot find directory ["+ diretGrupo + "]");
			    return;
		    }

		    try
		    {
			    // Depending on the directory this could be very large and would require more attention
			    // in a commercial package.
                List<string> filenames = new List<string>();

                filenames.AddRange(Directory.GetFiles(diretGrupo, "*.txt"));
                filenames.AddRange(Directory.GetFiles(diretGrupo, "*.pdf"));
			
			    // 'using' statements guarantee the stream is closed properly which is a big source
			    // of problems otherwise.  Its exception safe as well which is great.
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipSaida)))
                {
                    s.UseZip64 = UseZip64.Off;

                    s.SetLevel(9); // 0 - store only to 9 - means best compression
		
				    byte[] buffer = new byte[4096];
				
				    foreach (string file in filenames) {
					
					    // Using GetFileName makes the result compatible with XP
					    // as the resulting path is not absolute.
					    ZipEntry entry = new ZipEntry(Path.GetFileName(file));
					
					    // Setup the entry data as required.
					
					    // Crc and size are handled by the library for seakable streams
					    // so no need to do them here.

					    // Could also use the last write time or similar for the file.
					    entry.DateTime = DateTime.Now;
					    s.PutNextEntry(entry);
					
					    using ( FileStream fs = File.OpenRead(file) ) {
		
						    // Using a fixed size buffer here makes no noticeable difference for output
						    // but keeps a lid on memory usage.
						    int sourceBytes;
						    do {
							    sourceBytes = fs.Read(buffer, 0, buffer.Length);
							    s.Write(buffer, 0, sourceBytes);
						    } while ( sourceBytes > 0 );
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
		    catch(Exception ex)
		    {
                logger.Error("_generateZIP: " + ex.Message, ex);
			
			    // No need to rethrow the exception as for our purposes its handled.
		    }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="diretGrupo"></param>
        /// <param name="flagPDF"></param>
        /// <returns></returns>
        //private bool _generateCustodia(int account, string diretGrupo, bool flagPDF)
        //{
        //    try
        //    {
        //        string arqTXT = String.Format(@"{0}\{1}\CUSTODIA-{2}.txt", dirSplitted, account, account);

        //        if (!File.Exists(arqTXT))
        //        {
        //            logger.Info("_generateCustodia: in [" + arqTXT + "] nao existe");
        //            return false;
        //        }

        //        if (flagPDF)
        //        {
        //            string pdfSaida = String.Format(@"{0}\CUSTODIA-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

        //            logger.Info("_generateCustodia: in [" + arqTXT + "] out [" + pdfSaida + "]");

        //            Txt2Pdf.ConvertTxt2Pdf(arqTXT, pdfSaida);
        //            File.Delete(arqTXT);
        //        }
        //        else
        //        {
        //            string arqTXTOut = String.Format(@"{0}\CUSTODIA-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

        //            logger.Info("_generateCustodia: in [" + arqTXT + "] move to [" + arqTXTOut + "]");

        //            File.Move(arqTXT, arqTXTOut);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("_generateCustodia: erro ao processar account " + account + ": " + ex.Message, ex);
        //        return false;
        //    }

        //    logger.Info("_generateCustodia: account [" + account + "] processado com sucesso");

        //    return true;
        //}

        //private bool _generatePosDivBtc(int p, string diretGrupo, bool p_2)
        //{
        //    return false;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="diretGrupo"></param>
        /// <param name="flagPDF"></param>
        /// <returns></returns>
        //private bool _generatePosCliente(int account, string diretGrupo, bool flagPDF)
        //{
        //    try
        //    {
        //        string arqTXT = String.Format(@"{0}\{1}\POSCLIENTE-{2}.txt", dirSplitted, account, account);

        //        if (!File.Exists(arqTXT))
        //        {
        //            logger.Info("_generatePosCliente: in [" + arqTXT + "] nao existe");
        //            return false;
        //        }

        //        if (flagPDF)
        //        {
        //            string pdfSaida = String.Format(@"{0}\POSCLIENTE-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

        //            logger.Info("_generatePosCliente: in [" + arqTXT + "] out [" + pdfSaida + "]");

        //            Txt2Pdf.ConvertTxt2Pdf(arqTXT, pdfSaida);
        //            File.Delete(arqTXT);
        //        }
        //        else
        //        {
        //            string arqTXTOut = String.Format(@"{0}\POSCLIENTE-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

        //            logger.Info("_generatePosCliente: in [" + arqTXT + "] move to [" + arqTXTOut + "]");

        //            File.Move(arqTXT, arqTXTOut);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("_generatePosCliente: erro ao processar account " + account + ": " + ex.Message, ex);
        //        return false;
        //    }

        //    logger.Info("_generatePosCliente: account [" + account + "] processado com sucesso");

        //    return true;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="diretGrupo"></param>
        /// <param name="flagPDF"></param>
        /// <returns></returns>
        //private bool _generateLiqInvest(int account, string diretGrupo, bool flagPDF)
        //{
        //    try
        //    {
        //        string arqTXT = String.Format(@"{0}\{1}\LIQUIDACOES-{2}.txt", dirSplitted, account, account);

        //        if (!File.Exists(arqTXT))
        //        {
        //            logger.Info("_generateLiqInvest: in [" + arqTXT + "] nao existe");
        //            return false;
        //        }

        //        if (flagPDF)
        //        {
        //            string pdfSaida = String.Format(@"{0}\LIQUIDACOES-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

        //            logger.Info("_generateLiqInvest: in [" + arqTXT + "] out [" + pdfSaida + "]");

        //            Txt2Pdf.ConvertTxt2Pdf(arqTXT, pdfSaida);
        //            File.Delete(arqTXT);
        //        }
        //        else
        //        {
        //            string arqTXTOut = String.Format(@"{0}\LIQUIDACOES-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

        //            logger.Info("_generateLiqInvest: in [" + arqTXT + "] move to [" + arqTXTOut + "]");

        //            File.Move(arqTXT, arqTXTOut);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("_generateLiqInvest: erro ao processar account " + account + ": " + ex.Message, ex);
        //        return false;
        //    }

        //    logger.Info("_generateLiqInvest: account [" + account + "] processado com sucesso");

        //    return true;
        //}


        //private bool _generateGarantia(int account, string diretGrupo, bool flagPDF)
        //{
        //    try
        //    {
        //        string arqTXT = String.Format(@"{0}\{1}\GARANTIAS-{2}.txt", dirSplitted, account, account);

        //        if (!File.Exists(arqTXT))
        //        {
        //            logger.Info("_generateGarantia: in [" + arqTXT + "] nao existe");
        //            return false;
        //        }

        //        if (flagPDF)
        //        {
        //            string pdfSaida = String.Format(@"{0}\GARANTIAS-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

        //            logger.Info("_generateGarantia: in [" + arqTXT + "] out [" + pdfSaida + "]");

        //            Txt2Pdf.ConvertTxt2Pdf(arqTXT, pdfSaida);
        //            File.Delete(arqTXT);
        //        }
        //        else
        //        {
        //            string arqTXTOut = String.Format(@"{0}\GARANTIAS-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

        //            logger.Info("_generateGarantia: in [" + arqTXT + "] move to [" + arqTXTOut + "]");

        //            File.Move(arqTXT, arqTXTOut);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("_generateGarantia: erro ao processar account " + account + ": " + ex.Message, ex);
        //        return false;
        //    }

        //    logger.Info("_generateGarantia: account [" + account + "] processado com sucesso");

        //    return true;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="diretGrupo"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        //private bool _generateExigencia(int account, string diretGrupo, bool flagPDF)
        //{
        //    try
        //    {
        //        string arqTXT = String.Format(@"{0}\{1}\MARGEM-{2}.txt", dirSplitted, account, account);

        //        if (!File.Exists(arqTXT))
        //        {
        //            logger.Info("_generateExigencia: in [" + arqTXT + "] nao existe");
        //            return false;
        //        }

        //        if (flagPDF)
        //        {
        //            string pdfSaida = String.Format(@"{0}\MARGEM-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

        //            logger.Info("_generateExigencia: in [" + arqTXT + "] out [" + pdfSaida + "]");

        //            Txt2Pdf.ConvertTxt2Pdf(arqTXT, pdfSaida);
        //            File.Delete(arqTXT);
        //        }
        //        else
        //        {
        //            string arqTXTOut = String.Format(@"{0}\MARGEM-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

        //            logger.Info("_generateExigencia: in [" + arqTXT + "] move to [" + arqTXTOut + "]");

        //            File.Move(arqTXT, arqTXTOut);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("_generateExigencia: erro ao processar account " + account + ": " + ex.Message, ex);
        //        return false;
        //    }

        //    logger.Info("_generateExigencia: account [" + account + "] processado com sucesso");

        //    return true;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="diretGrupo"></param>
        /// <param name="flagPDF"></param>
        /// <returns></returns>
        /*private bool _generateBTC(int account, string diretGrupo, bool flagPDF)
        {
            try
            {
                string arqTXT = String.Format(@"{0}\{1}\BTC-{2}.txt", dirSplitted, account, account);

                if (!File.Exists(arqTXT))
                {
                    logger.Info("_generateBTC: in [" + arqTXT + "] nao existe");
                    return false;
                }

                if (flagPDF)
                {
                    string pdfSaida = String.Format(@"{0}\BTC-{1}-{2}.pdf", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

                    logger.Info("_generateBTC: in [" + arqTXT + "] out [" + pdfSaida + "]");

                    Txt2Pdf.ConvertTxt2Pdf(arqTXT, pdfSaida);
                    File.Delete(arqTXT);
                }
                else
                {
                    string arqTXTOut = String.Format(@"{0}\BTC-{1}-{2}.txt", diretGrupo, dataproc.ToString("yyyyMMdd"), account);

                    logger.Info("_generateBTC: in [" + arqTXT + "] move to [" + arqTXTOut + "]");

                    File.Move(arqTXT, arqTXTOut);
                }
            }
            catch (Exception ex)
            {
                logger.Error("_generateBTC: erro ao processar account " + account + ": " + ex.Message, ex);
                return false;
            }

            logger.Info("_generateBTC: account [" + account + "] processado com sucesso");

            return true;
        }*/


        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="diretGrupo"></param>
        /// <param name="flagPDF"></param>
        /// <returns></returns>
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
                lMensagem.Subject = "Relatorio de Envio Arquivos CBLC/COLD " + dataproc.ToString("dd/MM/yyyy");
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

        /// <summary>
        /// 
        /// </summary>
        public void MergeCustodiaCold()
        {
            if (bMerging)
            {
                logger.Warn("Outra operacao de merge de arquivos de custodia em execucao");
                return;
            }

            bMerging = true;
            dctCustodia.Clear();
            dctRelatorioMerge.Clear();

            try
            {
                string dirCold = ConfigurationManager.AppSettings["DirArqCustodiaToMerge"].ToString();
                string dirBkp = ConfigurationManager.AppSettings["DirArqCustodiaBackup"].ToString();

                string arq1 = String.Format(@"{0}\CUSTODIA_120_Gradual.txt", dirCold);

                if (File.Exists(arq1) && ParseCustodia(arq1))
                {
                    string arq2 = String.Format(@"{0}\227__COLD.txt", dirCold);

                    if (File.Exists(arq2) && ParseCustodia(arq2, true))
                    {
                        string arqMerge = String.Format(@"{0}\CUSTODIA_120_227_Merged.txt", dirCold);

                        string tmpMerge = Path.GetTempPath() + "CUSTODIA_120_227_Merged.txt";
                        string tmpMerge1 = Path.GetTempPath() + "CUSTODIA_Gradual_BRP_Merged.txt";

                        if (File.Exists(tmpMerge))
                            File.Delete(tmpMerge);

                        if (File.Exists(tmpMerge1))
                            File.Delete(tmpMerge1);

                        logger.Info("Gerando arquivo temporario [" + tmpMerge + "]");

                        // Tenta gerar o arquivo combinado,
                        if (this.GeraCustodiaMergeCold(tmpMerge, tmpMerge1))
                        {
                            logger.Info("Movendo temporario [" + tmpMerge + "] para [" + arqMerge + "]");

                            if (File.Exists(arqMerge))
                                File.Delete(arqMerge);
                            File.Move(tmpMerge, arqMerge);

                            string dirBkpHj = String.Format(@"{0}\{1}", dirBkp, DateTime.Now.ToString("yyyy-MM-dd"));
                            if (!Directory.Exists(dirBkpHj))
                                Directory.CreateDirectory(dirBkpHj);

                            string bkp1 = String.Format(@"{0}\CUSTODIA_120_Gradual.txt", dirBkpHj);

                            logger.Info("Movendo [" + arq1 + "] para [" + bkp1 + "]");

                            if (File.Exists(bkp1))
                                File.Delete(bkp1);

                            File.Move(arq1, bkp1);

                            string bkp2 = String.Format(@"{0}\CustodiaGRADUAL227.txt", dirBkpHj);

                            logger.Info("Movendo [" + arq2 + "] para [" + bkp2 + "]");

                            if (File.Exists(bkp2))
                                File.Delete(bkp2);
                            File.Move(arq2, bkp2);

                            _enviaCustodia(tmpMerge1);

                            string bkp3 = String.Format(@"{0}\CUSTODIA_Gradual_BRP_Merged.txt", dirBkpHj);

                            logger.Info("Copiando [" + tmpMerge1 + "] para [" + bkp3 + "]");

                            if (File.Exists(bkp3))
                                File.Delete(bkp3);
                            File.Move(tmpMerge1, bkp3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("MergeCustodiaCold: " + ex.Message, ex);
            }

            bMerging = false;
        }


        /// <summary>
        /// processa o arquivo da brasilplural
        /// o layout eh semelhante ao gradual, com menores diferencas no inicio das linhas
        /// </summary>
        public bool ParseCustodia(string arqentrada, bool converteconta = false)
        {
            try
            {
                ColdFilesSplitter splitter = new ColdFilesSplitter();

                logger.Info("Parsing [" + arqentrada + "]");

                string[] allLines = File.ReadAllLines(arqentrada);
                bool cabecalholido = false;

                string cabecalho = "";
                string clienteBRP = "";
                string clienteGradual = "";
                string cliente = "";
                string clienteOrig = "";
                string oldcliente = "";
                string oldpapel = "";
                string carteira = "";

                char ffChar = Convert.ToChar(0xff);
                char zerocChar = Convert.ToChar(0x0c);

                DBClientesCOLD db = new DBClientesCOLD();
                Dictionary<int, STClienteRelatCold> listaClientesCold = db.ObterListaClientesCOLD();
                Dictionary<int, bool> cabecalhoGrupo = new Dictionary<int, bool>();
                STCustodiaCliente custodia = null;

                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i].Replace('\r', ' ').Replace('\n', ' ').Replace(ffChar, ' ').Replace(zerocChar, ' ').TrimEnd();

                    int xxx = line.IndexOf("***  C B L C  -  COMPANHIA BRASILEIRA DE LIQUIDACAO E CUSTODIA  ***");
                    if (xxx > 0 && !cabecalholido)
                    {
                        string data = line.Substring(3, 8).Trim();

                        //TODO: bater as datas
                    }

                    // Ignorar todas as linhas referentes a cabecalho
                    if (cabecalholido)
                    {
                        if (line.IndexOf("C B L C") > 0 ||
                            line.IndexOf("S I S T E M A") > 0 ||
                            line.IndexOf("POSICAO CONSOLIDADA") > 0 ||
                            line.IndexOf("SOCIEDADE CORRETORA") > 0 ||
                            line.IndexOf("SALDOS  NA  CUSTODIA") > 0 ||
                            line.IndexOf("BLOQUEIOS P/") > 0 ||
                            line.IndexOf("CUSTODIA  ----II----    DEPOSITO") > 0 ||
                            line.IndexOf("DIREITOS  DE") > 0 ||
                            line.IndexOf("SALDOS REGISTRADO") > 0 ||
                            line.IndexOf("QUANTIDADE  DE  CLIENTES") > 0 ||
                            line.IndexOf("T O T A L") > 0)
                        {
                            continue;
                        }
                    }


                    logger.Debug("Line[" + line + "]");

                    // Do the fuc*** job
                    if (!String.IsNullOrEmpty(line))
                    {
                        int idxClient = line.IndexOf("CLIENTE:");
                        if (idxClient >= 0)
                        {
                            cabecalholido = true;
                            //int idxFimCliente = line.IndexOf("XXXXXXXXXXXXXXXXX");

                            clienteOrig = line.Substring(idxClient + 9, 11);
                            cliente = splitter.StripDigitAndThousand(clienteOrig.Trim());

                            if ( converteconta )
                                logger.Info("Processando cliente Gradual [" + cliente + "]");
                            else
                                logger.Info("Processando cliente BRP [" + cliente + "]");

                            if (String.IsNullOrEmpty(oldcliente) ||
                                (!String.IsNullOrEmpty(oldcliente) && !cliente.Equals(oldcliente)) )
                            {

                                if (converteconta)
                                {
                                    clienteBRP = splitter.BuscarClienteBRP(cliente);
                                    clienteGradual = cliente;
                                }
                                else
                                {
                                    clienteBRP = cliente;
                                    clienteGradual = splitter.BuscarClienteGradual(cliente);
                                }
                            }

                            continue;
                        }

                        if (!cabecalholido)
                        {
                            cabecalho += line + "\r\n";
                            continue;
                        }


                        int icarteira = line.IndexOf("CARTEIRA...:");
                        if (icarteira > 0)
                        {
                            carteira = line.Substring(icarteira + 13).Trim();
                            continue;
                        }

                        if (clienteBRP.Equals("42528"))
                        {
                            logger.Debug("Criente pobrema");
                        }



                        // Aqui é a linha do papel/ativo
                        line = line.TrimEnd().PadRight(150);

                        // Se a linha comecar com espaco, remove
                        if (line[0] == ' ')
                            line.Remove(0, 1);

                        string papel = line.Substring(0, 12).Trim();
                        string isin = line.Substring(13, 12).Trim();
                        string dis = line.Substring(26, 3).Trim();
                        string esp1 = line.Substring(30, 3).Trim();
                        string esp2 = line.Substring(34, 3).Trim();
                        string esp3 = line.Substring(38, 3).Trim();
                        string sit = line.Substring(41, 11).Trim();

                        string cust = line.Substring(54, 23).Trim().Replace(".", "");
                        string depst = line.Substring(79, 23).Trim().Replace(".", "");
                        string credito = line.Substring(105, 22).Trim().Replace(".", "");
                        string debito = line.Substring(128, 22).Trim().Replace(".", "");

                        if (!String.IsNullOrEmpty(papel))
                        {
                            if ( (!String.IsNullOrEmpty(oldpapel) && !papel.Equals(oldpapel)) || 
                                (!String.IsNullOrEmpty(oldcliente) && !clienteBRP.Equals(oldcliente)) )
                            {
                                // Busca as entradas ja existentes
                                // para esse cliente, para fazer a soma
                                STCustodiaCliente itemEncontrado = null;
                                foreach (STCustodiaCliente item in dctCustodia[custodia.CodBolsa])
                                {
                                    if (item.Carteira.Equals(custodia.Carteira) &&
                                        item.Papel.Equals(custodia.Papel))
                                    {
                                        int iContaGradual = Convert.ToInt32(clienteGradual);
                                        if (!dctRelatorioMerge.ContainsKey(iContaGradual))
                                        {
                                            dctRelatorioMerge.Add(iContaGradual, new List<STRelatMergeCustodia>());
                                        }

                                        List<STRelatMergeCustodia> relatorio = dctRelatorioMerge[iContaGradual];
                                        STRelatMergeCustodia itemRelat = relatorio.Find(x => (x.Carteira.Equals(custodia.Carteira) && x.Papel.Equals(custodia.Papel)));

                                        if (itemRelat == null)
                                        {
                                            itemRelat = new STRelatMergeCustodia();
                                            itemRelat.Papel = custodia.Papel;
                                            itemRelat.Carteira = custodia.Carteira;
                                            itemRelat.CodConta = iContaGradual;
                                            itemRelat.CodContaBRP = Convert.ToInt32(clienteBRP);

                                            relatorio.Add(itemRelat);
                                        }

                                        item.SaldoCustodia  += custodia.SaldoCustodia;
                                        item.BloqueioDeposito += custodia.BloqueioDeposito;

                                        foreach(KeyValuePair<string,STLancamentoPrevisto> custoPrev in custodia.Lancamentos)
                                        {
                                            if (item.Lancamentos.ContainsKey(custoPrev.Key) )
                                            {
                                                item.Lancamentos[custoPrev.Key].LctoPrevCredito += custoPrev.Value.LctoPrevCredito;
                                                item.Lancamentos[custoPrev.Key].LctoPrevDebito += custoPrev.Value.LctoPrevDebito;
                                            }
                                            else
                                            {
                                                item.Lancamentos.Add( custoPrev.Key, custoPrev.Value);
                                            }
                                        }


                                        //if (converteconta)
                                        //{
                                        //    itemRelat.BloqueioDepositoGRD = custodia.BloqueioDeposito;
                                        //    itemRelat.LctoPrevCreditoGRD = custodia.LctoPrevCredito;
                                        //    itemRelat.LctoPrevDebitoGRD = custodia.LctoPrevDebito;
                                        //    itemRelat.SaldoCustodiaGRD = custodia.SaldoCustodia;
                                        //}
                                        //else
                                        //{
                                        //    itemRelat.BloqueioDepositoBRP = custodia.BloqueioDeposito;
                                        //    itemRelat.LctoPrevCreditoBRP = custodia.LctoPrevCredito;
                                        //    itemRelat.LctoPrevDebitoBRP = custodia.LctoPrevDebito;
                                        //    itemRelat.SaldoCustodiaBRP = custodia.SaldoCustodia;
                                        //}


                                        itemEncontrado = item;

                                        break;
                                    }
                                }

                                if (itemEncontrado == null)
                                {
                                    // Se nao achou carteira + papel, acrescenta
                                    dctCustodia[custodia.CodBolsa].Add(custodia);
                                }
                            }

                            logger.DebugFormat("Parsing Account [{0}] Carteira [{1}] Papel [{2}] Cred [{3}] Deb [{4}]",
                                clienteBRP,
                                carteira,
                                papel,
                                credito,
                                debito);

                            // Inicializa nova entrada de custodia
                            custodia = new STCustodiaCliente();
                            custodia.Papel = papel;
                            custodia.Carteira = carteira;
                            custodia.CodBolsa = Convert.ToInt32(clienteBRP);
                            custodia.Situacao = sit;
                            custodia.ISIN = string.Format("{0} {1} {2} {3} {4}",
                                                        isin.PadRight(12),
                                                        dis.PadRight(3),
                                                        esp1.PadRight(3),
                                                        esp2.PadRight(3),
                                                        esp3.PadRight(3));

                            if (!String.IsNullOrEmpty(cust))
                                custodia.SaldoCustodia = Convert.ToInt64(cust);
                            if (!String.IsNullOrEmpty(depst))
                                custodia.BloqueioDeposito = Convert.ToInt64(depst);

                            if (!dctCustodia.ContainsKey(custodia.CodBolsa))
                            {
                                dctCustodia.Add(custodia.CodBolsa, new List<STCustodiaCliente>());
                            }

                            oldpapel = papel;
                        }
                        else
                        {
                            string dataprevisao = sit;

                            logger.DebugFormat("Parsing Account [{0}] Carteira [{1}] Papel [{2}] Previsao [{3}] Cred [{4}] Deb [{5}]",
                                clienteBRP,
                                carteira,
                                papel,
                                dataprevisao,
                                credito,
                                debito);

                            STLancamentoPrevisto lcto = new STLancamentoPrevisto();
                            lcto.DataPrevisao = dataprevisao;

                            if (!String.IsNullOrEmpty(credito))
                                lcto.LctoPrevCredito = Convert.ToInt64(credito);
                            if (!String.IsNullOrEmpty(debito))
                                lcto.LctoPrevDebito = Convert.ToInt64(debito);

                            custodia.Lancamentos.Add(dataprevisao, lcto);
                        }

                        oldcliente = cliente;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("ParseCustodia: " + ex.Message, ex);
                return false;
            }
            return true;
        }


        public bool GeraCustodiaMergeCold(string arqMergeBRP, string arqMergeGradual)
        {
            try
            {
                ColdFilesSplitter splitter = new ColdFilesSplitter();
                AccountDigit acDig = new AccountDigit();
                string account = "";
                string accountGradual = "";

                string tplcabec = File.ReadAllText(ConfigurationManager.AppSettings["TemplateCabecalhoCustodia"].ToString());

                string cabecalho = string.Format(tplcabec, DateTime.Now.ToString("dd/MM/yyyy"));

                File.WriteAllText(arqMergeBRP, cabecalho);
                File.WriteAllText(arqMergeGradual, cabecalho);

                foreach (KeyValuePair<int, List<STCustodiaCliente>> item in dctCustodia)
                {
                    int codBolsa = item.Key;

                    if (codBolsa <= 0)
                        continue;

                    List<STCustodiaCliente> custodias = item.Value;

                    account = acDig.CalculaDV(120, codBolsa).ToString();

                    account = account.Insert(account.Length - 1, "-");

                    int codGradual = AccountParser.Instance.GetAccountParsed(codBolsa);

                    if (codGradual <= 0)
                        continue;

                    string linha = string.Format("CLIENTE: {0} - {1}\r\n", account.PadLeft(11), "GRADUAL CCTVM S/A");
                    File.AppendAllText(arqMergeBRP, linha);

                    accountGradual = acDig.CalculaDV(227, codGradual).ToString();
                    accountGradual = accountGradual.Insert(accountGradual.Length - 1, "-");
                    string nomeCliente = splitter.BuscarNomeCliente(codGradual.ToString());


                    linha = string.Format("CLIENTE: {0} - {1}\r\n", accountGradual.PadLeft(11), nomeCliente);
                    File.AppendAllText(arqMergeGradual, linha);

                    // custodias.Sort((x, y) => x.Carteira.CompareTo(y.Carteira));
                    custodias = item.Value.OrderBy(i => i.Carteira).ThenBy(i => i.Papel).ToList();

                    string carteira = "";
                    foreach (STCustodiaCliente custodia in custodias)
                    {
                        if (!custodia.Carteira.Equals(carteira))
                        {
                            linha = string.Format("      CARTEIRA...:  {0}", custodia.Carteira);
                            File.AppendAllText(arqMergeBRP, linha.PadLeft(21));
                            File.AppendAllText(arqMergeBRP, "\r\n");

                            File.AppendAllText(arqMergeGradual, linha.PadLeft(21));
                            File.AppendAllText(arqMergeGradual, "\r\n");
                        }
                        carteira = custodia.Carteira;

                        linha = string.Format("{0} {1}",
                            custodia.Papel.PadRight(12),
                            custodia.ISIN);

                        File.AppendAllText(arqMergeBRP, linha);
                        File.AppendAllText(arqMergeGradual, linha);

                        if (!String.IsNullOrEmpty(custodia.Situacao))
                            linha = string.Format("{0}", custodia.Situacao.PadRight(12));
                        else
                            linha = string.Format("{0}", " ".PadRight(12));

                        File.AppendAllText(arqMergeBRP, linha);
                        File.AppendAllText(arqMergeGradual, linha);

                        linha = String.Format(CultureInfo.CreateSpecificCulture("pt-Br"), "{0:#,0}", custodia.SaldoCustodia);
                        File.AppendAllText(arqMergeBRP, linha.PadLeft(23));
                        File.AppendAllText(arqMergeGradual, linha.PadLeft(23));

                        File.AppendAllText(arqMergeBRP, " ".PadRight(2));
                        File.AppendAllText(arqMergeGradual, " ".PadRight(2));

                        linha = String.Format(CultureInfo.CreateSpecificCulture("pt-Br"), "{0:#,0}", custodia.BloqueioDeposito);
                        File.AppendAllText(arqMergeBRP, linha.PadLeft(23));
                        File.AppendAllText(arqMergeGradual, linha.PadLeft(23));

                        File.AppendAllText(arqMergeBRP, "\r\n");
                        File.AppendAllText(arqMergeGradual, "\r\n");


                        foreach (STLancamentoPrevisto lcto in custodia.Lancamentos.Values )
                        {
                            linha = string.Format("{0} {1} {2}",
                                " ".PadRight(42),
                                lcto.DataPrevisao,
                                " ".PadRight(50));

                            File.AppendAllText(arqMergeBRP, linha);
                            File.AppendAllText(arqMergeGradual, linha);

                            linha = String.Format(CultureInfo.CreateSpecificCulture("pt-Br"), "{0:#,0}", lcto.LctoPrevCredito);
                            File.AppendAllText(arqMergeBRP, linha.PadLeft(20));
                            File.AppendAllText(arqMergeGradual, linha.PadLeft(20));

                            File.AppendAllText(arqMergeBRP, " ".PadRight(3));
                            File.AppendAllText(arqMergeGradual, " ".PadRight(3));

                            linha = String.Format(CultureInfo.CreateSpecificCulture("pt-Br"), "{0:#,0}", lcto.LctoPrevDebito);
                            File.AppendAllText(arqMergeBRP, linha.PadLeft(20));
                            File.AppendAllText(arqMergeGradual, linha.PadLeft(20));

                            File.AppendAllText(arqMergeBRP, "\r\n");
                            File.AppendAllText(arqMergeGradual, "\r\n");
                        }
                    }

                    File.AppendAllText(arqMergeBRP, "\r\n");
                    File.AppendAllText(arqMergeGradual, "\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Error("GeraCustodiaCold: " + ex.Message, ex);
                return false;
            }

            return true;
        }


        private bool _enviaCustodia(string arquivoMerged)
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
                emailsTo = ConfigurationManager.AppSettings["EmailDestCustodia"].ToString().Split(seps);

                var lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailColdRemetente"].ToString(), emailsTo[0]);

                for (int i = 1; i < emailsTo.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsTo[i]))
                        lMensagem.To.Add(emailsTo[i]);
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
                lMensagem.Subject = "RELATORIO DE CUSTODIA CBLC MERGED " + dataproc.ToString("dd/MM/yyyy");

                lMensagem.Attachments.Add(new Attachment(arquivoMerged));

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
    }
}
