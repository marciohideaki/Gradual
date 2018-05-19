using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Spider.JaneStreet.Report.Db;
using Gradual.Spider.JaneStreet.Report.Lib.Dados;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Data;
using System.IO;
using Gradual.Spider.Utils.Email;
using Gradual.Spider.Utils.Email.Entities;
using System.Configuration;

namespace Gradual.Spider.JaneStreet.Report
{
    class Program
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        static void Main(string[] args)
        {
            try
            {
                char[] sep = { ';' };
                char[] sep2 = { ':' };
                log4net.Config.XmlConfigurator.Configure();
                logger.Info("-------------------------------------------------------");
                logger.Info("Gradual.Spider.JaneStreet.Report v." + typeof(Program).Assembly.GetName().Version);

                string strIdFix = ConfigurationManager.AppSettings["IdFix"].ToString();
                if (string.IsNullOrEmpty(strIdFix))
                {
                    logger.Error("IdFix nulo ou invalido");
                    return;
                }
                logger.Info("IdFix para processar: " + strIdFix);
                

                // Buscar informacoes
                DbSpider db = new DbSpider();
                List<string> lstFileNames = new List<string>();
                string[] arr = strIdFix.Split(sep);
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] arraux = arr[i].Split(sep2);
                    int id = Convert.ToInt32(arraux[0]);
                    DataSet ds = db.BuscarOrdensDropCopyDataSet(id);
                    // Gerar o excel
                    string fileName = ExcelCreator.CreateJaneStreetExcel(ds, arraux[1]);
                    lstFileNames.Add(fileName);
                }
                Program._sendJaneStreetMail(lstFileNames);
                logger.Info("Processo Concluido");
            }
            catch (Exception ex)
            {
                logger.Error("Erro na execucao da geracao do Relatorio: " + ex.Message, ex);
            }
        }

        public static void _sendJaneStreetMail(List<string> arqs)
        {

            try
            {
                // Enviar Email
                SpiderMail sMail = new SpiderMail();
                ConfigMailInfo cfg = new ConfigMailInfo();
                MessageMailInfo msg = new MessageMailInfo();
                
                if (!ConfigurationManager.AppSettings.AllKeys.Contains("MailFrom"))
                {
                    logger.Error("Parametro MailFrom obrigatorio");
                    return;
                }
                if (!ConfigurationManager.AppSettings.AllKeys.Contains("MailTo"))
                {
                    logger.Error("Parametro MailTo obrigatorio");
                    return;
                }

                if (!ConfigurationManager.AppSettings.AllKeys.Contains("SmtpHost"))
                {
                    logger.Error("Parametro SmtpHost obrigatorio");
                    return;
                }
                cfg.SmtpHost = ConfigurationManager.AppSettings["SmtpHost"].ToString();
                msg.From = ConfigurationManager.AppSettings["MailFrom"].ToString();
                msg.To = ConfigurationManager.AppSettings["MailTo"].ToString();
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MailCc"))
                {
                    msg.Cc = ConfigurationManager.AppSettings["MailCc"].ToString();
                }
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MailCco"))
                {
                    msg.Cco = ConfigurationManager.AppSettings["MailCco"].ToString();
                }
                

                if (ConfigurationManager.AppSettings.AllKeys.Contains("Subject"))
                    msg.Subject = ConfigurationManager.AppSettings["Subject"].ToString() + " " + DateTime.Now.ToString("dd-MM-yyyy");
                else
                    msg.Subject = "Relatorio " + DateTime.Now.ToString("dd-MM-yyyy");

                msg.Body = "Relatorio de Execucoes JaneStreet: " + DateTime.Now.ToString("dd-MM-yyyy") + "\n\n";

                for (int i = 0; i < arqs.Count; i++)
                {
                    msg.FileAttach = msg.FileAttach + arqs[i] + ";";
                }
                msg.FileAttach = msg.FileAttach.Substring(0, msg.FileAttach.Length-1);

                // Logs dos Parametros
                logger.InfoFormat("From: [{0}]", msg.From);
                logger.InfoFormat("To: [{0}]", msg.To);
                logger.InfoFormat("Cc: [{0}]", msg.Cc);
                logger.InfoFormat("Cco: [{0}]", msg.Cco);
                logger.InfoFormat("Subject: [{0}]", msg.Subject);
                logger.InfoFormat("File: [{0}]", msg.FileAttach);
                if (!SpiderMail.SendEmail(cfg, msg))
                {
                    logger.Error("Nao foi possivel enviar o email!!!!");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio de email: " + ex.Message, ex);
            }

        }


        /*
        public static void _createJaneStreetExcel(DataSet ds)
        {
            try
            {
                string currentDirectorypath = Environment.CurrentDirectory;
                string fileName = string.Format("{0}{1}{2}-{3}.xlsx" , Environment.CurrentDirectory, Path.DirectorySeparatorChar, "JaneStreetDP", DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
                FileInfo newFile = new FileInfo(fileName);
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Executadas");
                    if (ds!=null)
                        ws.Cells["A1"].LoadFromDataTable(ds.Tables[0], true, TableStyles.None);
                    package.Save();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro na criacao do excel...: " + ex.Message, ex);
            }
            
        }
         */
    }
}
