using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using log4net;
using System.Net.Mail;

namespace QGCadastro1
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
                log4net.Config.XmlConfigurator.Configure();

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DirectTradeCadastro"].ConnectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PRC_VIRTUALTARGET_LISTA";

                SqlDataAdapter lAdapter;
                DataSet lDataSet = new DataSet();

                lAdapter = new SqlDataAdapter(cmd);
                lAdapter.Fill(lDataSet);

                StringBuilder sb = new StringBuilder();

                logger.Debug("Obteve " + lDataSet.Tables[0].Rows.Count + " registros");

                foreach (DataRow row in lDataSet.Tables[0].Rows )
                {
                    sb.AppendLine(row[0].ToString());
                }

                string dir = ConfigurationManager.AppSettings["DiretorioCSV"].ToString();

                string csv = String.Format(@"{0}\LISTAPROSPECT.CSV", dir);

                File.WriteAllText(csv, sb.ToString(), Encoding.UTF8);

                logger.Info("Arquivo gerado com sucesso!");

                string emails = ConfigurationManager.AppSettings["EmailDestProspects"].ToString();

                string subject = "Arquivo LISTAPROSPECT.CSV disponibilizado em " + dir;

                List<string> lista = new List<string>();
                lista.Add(csv);
                _enviaAviso(emails, subject, lista);


                conn.Close();
            }
            catch (Exception ex)
            {
                logger.Error("Main: " + ex.Message, ex);
            }
        }


        private static bool _enviaAviso(string emailsDest, string subject, List<string> arquivosMerged)
        {
            try
            {
                string[] emailsTo;
                string[] emailsCC;
                string[] emailsBCC;


                if (ConfigurationManager.AppSettings["EmailRemetente"] == null)
                {
                    logger.Error("AppSetting 'EmailMTARemetente' nao definido. Nao eh possivel enviar alerta");
                    return false;
                }

                char[] seps = { ';', ',' };
                emailsTo = emailsDest.ToString().Split(seps);

                MailMessage lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailRemetente"].ToString(), emailsTo[0]);

                for (int i = 1; i < emailsTo.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsTo[i]))
                        lMensagem.To.Add(emailsTo[i]);
                }

                if (ConfigurationManager.AppSettings["EmailBCC"] != null)
                {
                    emailsBCC = ConfigurationManager.AppSettings["EmailBCC"].ToString().Split(seps);

                    for (int i = 0; i < emailsBCC.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(emailsBCC[i]))
                            lMensagem.Bcc.Add(emailsBCC[i]);
                    }
                }

                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailReplyTo"].ToString()));
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

                foreach (string arquivoMerged in arquivosMerged)
                    lMensagem.Attachments.Add(new Attachment(arquivoMerged));

                new SmtpClient(ConfigurationManager.AppSettings["EmailHost"].ToString()).Send(lMensagem);

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
    }
}
