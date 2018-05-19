using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Gradual.Intranet.Contratos;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Email.Lib;



namespace Gradual.Intranet.Servicos.RegrasDeNegocio
{
    public class ServicoEmailRnLib
    {
        #region | Propriedades

        public EmailInfo ObjetoEmail
        {
            get;
            set;
        }

        #endregion

        #region | Construtores

        /// <summary>
        /// Instancia um objeto ServicoEmailRnLib vazio.
        /// </summary>
        public ServicoEmailRnLib(EmailInfo pEmailInfo)
        {
            this.ObjetoEmail = pEmailInfo;
        }

        #endregion

        #region | Métodos

        /// <summary>
        /// Envia o email com base nas propriedades do objeto.
        /// </summary>
        public void Enviar()
        {
            if (0.Equals(this.ObjetoEmail.Destinatarios.Count) || string.IsNullOrEmpty(this.ObjetoEmail.Destinatarios[0]))
                throw new NullReferenceException("Informe ao menos um destinatário para mensagem.");

            var lMensagem = new MailMessage(ObjetoEmail.Remetente, this.ObjetoEmail.Destinatarios[0]);

            ObjetoEmail.Destinatarios.ForEach(delegate(string mail)
            {
                if (!ObjetoEmail.Destinatarios.Contains<string>(mail))
                    lMensagem.To.Add(mail);
            });

            ObjetoEmail.DestinatariosCC.ForEach(delegate(string mailCc)
            {
                lMensagem.CC.Add(mailCc);
            });

            ObjetoEmail.DestinatariosCO.ForEach(delegate(string mailCo)
            {
                lMensagem.Bcc.Add(mailCo);
            });

            ObjetoEmail.Anexos.ForEach(delegate(Gradual.OMS.Email.Lib.EmailAnexoInfo fileInfo)
            {
                lMensagem.Attachments.Add(new Attachment(new MemoryStream(fileInfo.Arquivo), fileInfo.Nome));
            });

            {
                lMensagem.Body = ObjetoEmail.CorpoMensagem;
                lMensagem.IsBodyHtml = ObjetoEmail.MensagemHTML;
                lMensagem.Priority = ObjetoEmail.Prioridade;
                lMensagem.Subject = ObjetoEmail.Assunto;
            }

            new SmtpClient("EXCHANGE01.gradual.intra").Send(lMensagem);
        }

        #endregion
    }
}
