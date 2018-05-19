using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Net.Mail;
using Gradual.SaldoDevedor.Utils.Info;

namespace Gradual.SaldoDevedor.Utils
{
    public class Mail
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public static bool SendEmail(ConfigMailInfo cfg, MessageMailInfo msg)
        {
            try
            {
                char[] sep = { ';' };
                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;

                mail.From = new MailAddress(msg.From);

                // TO composition
                if (!string.IsNullOrEmpty(msg.To))
                {
                    string[] to = msg.To.Split(sep);
                    for (int i = 0; i < to.Length; i++)
                    {
                        mail.To.Add(new MailAddress(to[i]));
                    }
                }

                if (!string.IsNullOrEmpty(msg.Cc))
                {
                    string[] cc = msg.Cc.Split(sep);
                    for (int i = 0; i < cc.Length; i++)
                    {
                        mail.CC.Add(new MailAddress(cc[i]));
                    }
                }
                if (!string.IsNullOrEmpty(msg.Cco))
                {
                    string[] cco = msg.Cco.Split(sep);
                    for (int i = 0; i < cco.Length; i++)
                    {
                        mail.Bcc.Add(new MailAddress(cco[i]));
                    }
                }
                mail.Subject = msg.Subject;
                mail.Body = msg.Body;

                if (!string.IsNullOrEmpty(msg.FileAttach))
                {
                    string[] files = msg.FileAttach.Split(sep);
                    for (int i = 0; i < files.Length; i++)
                    {
                        mail.Attachments.Add(new Attachment(files[i]));
                    }
                }

                SmtpClient smtp = null;
                if (!string.IsNullOrEmpty(cfg.SmtpPort))
                    smtp = new SmtpClient(cfg.SmtpHost, Convert.ToInt32(cfg.SmtpPort));
                else
                    smtp = new SmtpClient(cfg.SmtpHost);

                smtp.Send(mail);

                mail.Dispose();
                mail = null;
                smtp.Dispose();
                smtp = null;
            }
            catch (Exception ex)
            {
                logger.Error("SendMail(): Exception " + ex.Message, ex);
                return false;
            }
            return true;

        }
    }
}
