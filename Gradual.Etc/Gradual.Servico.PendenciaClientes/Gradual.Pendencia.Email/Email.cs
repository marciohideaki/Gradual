using System;
using System.Net.Mail;

namespace Gradual.Pendencia.Email
{
    public class Email
    {
        public bool EnviarEmail(string pRemetenteEmail,string pRemetenteDisplay, string pDestinatario, string pAssunto, string pCorpo, string pServerSMTP)
        {
            try
            {
                MailMessage email = new MailMessage();
                
                email.From = new MailAddress(pRemetenteEmail,pRemetenteDisplay);
                email.To.Add(new MailAddress(pDestinatario));
                email.Subject = pAssunto;
                email.Priority =  MailPriority.Normal;
                email.IsBodyHtml = true;
                email.Body = pCorpo;
                SmtpClient SmtpMail = new SmtpClient(pServerSMTP);
                SmtpMail.Send(email);
                return true;
            }
            catch (Exception)
            {
                throw new MethodAccessException("Um erro ocorreu durante o envio do E-mail");
            }
        }
    }
}
