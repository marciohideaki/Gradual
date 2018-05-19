#region Includes
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using System.ServiceModel;
using log4net;
using Gradual.OMS.Email.Lib;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Concurrent;
using System.Threading;
#endregion

namespace Gradual.OMS.Email
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoEmail : IServicoEmail, IServicoControlavel
    {
        #region | Propriedades

        private ConcurrentQueue<MailMessage> queueMessages = new ConcurrentQueue<MailMessage>();
        private object syncQueueMessages = new object();
        private ServicoStatus status = ServicoStatus.Parado;
        private bool bKeepRunning = false;

        private Thread thQueueProc = null;

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string GetSMTPHost
        {
            get
            {
                var lConfig = GerenciadorConfig.ReceberConfig<ServicoEmailConfig>();

                return lConfig.SMTPHost;
            }
        }

        public EmailInfo ObjetoEmail
        {
            get;
            set;
        }

        private ServicoStatus GetServicoStatus
        {
            get;
            set;
        }

        #endregion

        #region | Métodos

        /// <summary>
        /// Envia o email com base nas propriedades do objeto.
        /// </summary>
        public EnviarEmailResponse Enviar(EnviarEmailRequest pParametro)
        {
            try
            {
                string lDestinatarios = "";

                foreach (string lEmail in pParametro.Objeto.Destinatarios)
                {
                    lDestinatarios += string.Format("{0},", lEmail);
                }

                lDestinatarios = lDestinatarios.TrimEnd(",".ToCharArray());

                if (pParametro.Objeto != null)
                    logger.InfoFormat("> Chamada para envio de email assunto [{0}], Remetente: {1}, Destinatário: {2}", pParametro.Objeto.Assunto, pParametro.Objeto.Remetente, lDestinatarios);
                else
                    logger.InfoFormat("> Chamada para envio de email com pParametro.Objeto nulo!");

                this.ObjetoEmail = pParametro.Objeto;
                var lResposta = new EnviarEmailResponse()
                {
                    StatusResposta = MensagemResponseStatusEnum.OK,
                    Resposta = pParametro.Objeto,
                };

                if (0.Equals(this.ObjetoEmail.Destinatarios.Count) || string.IsNullOrEmpty(this.ObjetoEmail.Destinatarios[0]))
                    throw new NullReferenceException("Informe ao menos um destinatário para mensagem.");

                var lMensagem = new MailMessage(ObjetoEmail.Remetente, this.ObjetoEmail.Destinatarios[0]);

                for (int a = 1; a < ObjetoEmail.Destinatarios.Count; a++)
                {
                    lMensagem.To.Add(ObjetoEmail.Destinatarios[a]); //o destinatário número 0 já foi incluido na linha acima: this.ObjetoEmail.Destinatarios[0]
                }

                ObjetoEmail.DestinatariosCC.ForEach(delegate(string mailCc)
                {
                    lMensagem.CC.Add(mailCc);
                });

                ObjetoEmail.DestinatariosCO.ForEach(delegate(string mailCo)
                {
                    lMensagem.Bcc.Add(mailCo);
                });

                if (ObjetoEmail.Anexos != null && ObjetoEmail.Anexos.Count > 0)
                {
                    foreach (var item in ObjetoEmail.Anexos)
                    {
                        lMensagem.Attachments.Add(new Attachment(new MemoryStream(item.Arquivo), item.Nome));
                    }
                }

                {
                    lMensagem.Body = ObjetoEmail.CorpoMensagem;
                    lMensagem.IsBodyHtml = ObjetoEmail.MensagemHTML;
                    lMensagem.Priority = ObjetoEmail.Prioridade;
                    lMensagem.Subject = ObjetoEmail.Assunto;
                }

                //new SmtpClient("ironport.gradualcorretora.com.br").Send(lMensagem);
                //new SmtpClient(this.GetSMTPHost).Send(lMensagem);
                bool sinaliza = queueMessages.IsEmpty;
                queueMessages.Enqueue(lMensagem);
                if (sinaliza)
                {
                    lock (syncQueueMessages)
                    {
                        Monitor.Pulse(syncQueueMessages);
                    }
                }

                logger.InfoFormat("< Retorno de envio de email; Status [{0}]", lResposta.StatusResposta);

                return lResposta;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Enviar  - Erro {0} - Stack Trace {1}", ex.Message, ex.StackTrace);

                this.GetServicoStatus = ServicoStatus.Erro;

                return new EnviarEmailResponse()
                {
                    StatusResposta = MensagemResponseStatusEnum.ErroPrograma,
                    DescricaoResposta = ex.ToString(),
                };
            }
        }

        /// <summary>
        /// Envia o email com base nas propriedades do objeto.
        /// </summary>
        public EnviarEmailResponse EnviarComTemplate(EnviarEmailRequest pParametro)
        {
            try
            {
                string lCorpoEmail = string.Empty;

                if (pParametro.Objeto != null)
                    logger.InfoFormat("> Chamada para envio de email assunto [{0}]", pParametro.Objeto.Assunto);
                else
                    logger.InfoFormat("> Chamada para envio de email com pParametro.Objeto nulo!");

                this.ObjetoEmail = pParametro.Objeto;

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

                if (ObjetoEmail.Anexos != null && ObjetoEmail.Anexos.Count > 0)
                {
                    foreach (var item in ObjetoEmail.Anexos)
                    {
                        lMensagem.Attachments.Add(new Attachment(new MemoryStream(item.Arquivo), item.Nome));
                    }
                }

                switch (pParametro.Objeto.ETipoTemplate)
                {
                    case ETipoTemplate.NotificacaoRetirada:
                        {
                            lCorpoEmail = File.ReadAllText(ConfigurationManager.AppSettings["pathEmailNotificacaoRetirada"].ToString());
                            foreach (KeyValuePair<string, string> dados in pParametro.Objeto.ListaVariavelTemplate)
                                lCorpoEmail = lCorpoEmail.Replace(dados.Key, dados.Value);
                        }
                        break;

                    case ETipoTemplate.NotificacaoDeposito:
                        {
                            lCorpoEmail = File.ReadAllText(ConfigurationManager.AppSettings["pathEmailNotificacaoDeposito"].ToString());
                            foreach (KeyValuePair<string, string> dados in pParametro.Objeto.ListaVariavelTemplate)
                                lCorpoEmail = lCorpoEmail.Replace(dados.Key, dados.Value);
                        }
                        break;
                }

                {
                    lMensagem.Body       = lCorpoEmail;
                    lMensagem.IsBodyHtml = ObjetoEmail.MensagemHTML;
                    lMensagem.Priority   = ObjetoEmail.Prioridade;
                    lMensagem.Subject    = ObjetoEmail.Assunto;
                }

                //new SmtpClient("ironport.gradualcorretora.com.br").Send(lMensagem);
                //new SmtpClient(this.GetSMTPHost).Send(lMensagem);
                bool sinaliza = queueMessages.IsEmpty;
                queueMessages.Enqueue(lMensagem);
                if (sinaliza)
                {
                    lock (syncQueueMessages)
                    {
                        Monitor.Pulse(syncQueueMessages);
                    }
                }
                
                var lResposta = new EnviarEmailResponse()
                {
                    StatusResposta = MensagemResponseStatusEnum.OK,
                    Resposta = pParametro.Objeto,
                };


                logger.InfoFormat("< Retorno de envio de email; Status [{0}]", lResposta.StatusResposta);

                return lResposta;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("EnviarComTemplate  - Erro {0} - Stack Trace {1}", ex.Message, ex.StackTrace);

                this.GetServicoStatus = ServicoStatus.Erro;

                return new EnviarEmailResponse()
                {
                    StatusResposta = MensagemResponseStatusEnum.ErroPrograma,
                    DescricaoResposta = ex.ToString(),
                };
            }
        }

        #endregion

        #region | Implementacoes interface
        private void EnviarEmailTeste()
        {
            logger.Info("Gradual.OMS.Sistemas.Email.IniciarServico() - Arquivo carregado.");

            var lDestinatarios = new System.Collections.Generic.List<string>();
            lDestinatarios.Add("arosario@gradualinvestimentos.com.br");

            var lEmail = new EmailInfo()
            {
                Assunto = "Teste de e-mail",
                CorpoMensagem = string.Format("<html><body><p>TESTE DO SERVICO DE EMAIL</p><p>Realizado em: {0}</p></body></html>", DateTime.Today.ToString("dd/MM/yyyy HH:mm:ss")),
                Destinatarios = lDestinatarios,
                Remetente = "arosario@gradualinvestimentos.com.br",
            };


            this.Enviar(new EnviarEmailRequest() { Objeto = lEmail });
        }

        #endregion

        public void IniciarServico()
        {
            logger.Info("Iniciando ServicoEmail");

            bKeepRunning = true;

            thQueueProc = new Thread(new ThreadStart(queueProc));
            thQueueProc.Start();

            logger.Info("ServicoEmail iniciado");

        }

        public void PararServico()
        {
            logger.Info("Finalizando ServicoEmail");

            bKeepRunning = false;

            while (thQueueProc.IsAlive)
            {
                logger.Info("Aguardando finalizacao da thread de envio de mensagens");
            }

            logger.Info("ServicoEmail finalizado");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return status;
        }

        private void queueProc()
        {
            logger.Info("Iniciando thread de processamento da fila de mensagens");
            
            while (bKeepRunning || !queueMessages.IsEmpty)
            {
                try
                {
                    MailMessage message;
                    if (queueMessages.TryPeek(out message))
                    {
                        new SmtpClient(this.GetSMTPHost).Send(message);

                        // Remove a mensagem definitivamente
                        while (!queueMessages.TryDequeue(out message));
                        logger.Info("Mensagens na fila: " + queueMessages.Count);

                        continue;
                    }

                    lock (syncQueueMessages)
                    {
                        Monitor.Wait(syncQueueMessages, 250);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("queueProc(): " + ex.Message, ex);

                    Thread.Sleep(10000);
                }
            }

            logger.Info("Finalizando thread de processamento da fila de mensagens");
        }
    }
}
