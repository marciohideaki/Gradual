using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Gradual.Pendencia.Email;
using Gradual.Pendencias.Dados;
using Gradual.Pendencias.Entidades;
using log4net;

namespace Gradual.Servico.PendenciaClientes
{
    public class NotificacaoPendencias
    {
        #region | Atributos

        private System.Timers.Timer timer1 = new System.Timers.Timer();

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int gContadorEmailsEnviados = default(int);

        private int gContadorEmailsComErro = default(int);

        #endregion

        #region | Construtores

        public NotificacaoPendencias()
        {
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
            gLogger.Info("Delegate do processo associado.");
        }

        #endregion

        #region | Propriedades

        private int GetHoraDeEnvio
        {
            get
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(ConfigurationManager.AppSettings["HorarioDeEnvioDeEmails"], out lRetorno))
                    return 10;

                return lRetorno.Hour;
            }
        }

        private long GetIntervaloExecucao
        {
            get
            {
                return 58    //--> 58 minutos
                     * 60    //--> convertendo segundo em minuto
                     * 1000; //--> convertendo milissegundos para segundos
            }
        }

        private DayOfWeek GetDiaDaSemandaParaEnvio
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(ConfigurationManager.AppSettings["DiaDaSemanaParaEnvio"], out lRetorno) || lRetorno > 6)
                    lRetorno = 1; //--> Segunda-feira

                return (DayOfWeek)lRetorno;
            }
        }

        private string GetTextoEmail
        {
            get
            {
                string lRetorno;

                using (StreamReader lArquivoEmail = File.OpenText(string.Concat(Application.StartupPath, "\\TextoEmail.htm")))
                {
                    lRetorno = lArquivoEmail.ReadToEnd();
                }

                return lRetorno;
            }
        }

        private string GetSmtp
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTP"].ToString();
            }
        }

        private string GetSenderDisplay
        {
            get
            {
                return ConfigurationManager.AppSettings["SenderDisplay"].ToString();
            }
        }

        private string GetSenderEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["Sender"].ToString();
            }
        }

        private string GetAssunto
        {
            get
            {
                return ConfigurationManager.AppSettings["Assunto"].ToString();
            }
        }

        private string GetDestinatario
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailDestino"].ToString();
            }
        }

        private bool IsProducao
        {
            get
            {
                if (null == ConfigurationManager.AppSettings["EmailDestino"]
                || (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailDestino"])))
                    return true;

                return false;
            }
        }

        #endregion

        #region | Eventos

        public void ServicoIniciar()
        {
            gLogger.Info(string.Format("Amarrando o período de envio para {0} minutos.", this.GetIntervaloExecucao.ToString()));

            timer1.Interval = this.GetIntervaloExecucao;
            timer1.Enabled = true;
            timer1.Start();
        }

        public void ServicoParar()
        {
            timer1.Enabled = false;
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            gLogger.Info("Verificando a necessidade de envio.");
            gLogger.Info(string.Format("Dia da semana correto? {0}.", (DateTime.Today.DayOfWeek.Equals(this.GetDiaDaSemandaParaEnvio).ToString())));
            gLogger.Info(string.Format("Agora são: {0}, hora de execucao: {1}", DateTime.Now.Hour.ToString(), this.GetHoraDeEnvio.ToString()));
            gLogger.Info(string.Format("Horário correto? {0}", DateTime.Now.Hour.Equals(this.GetHoraDeEnvio).ToString()));

            if (DateTime.Today.DayOfWeek.Equals(this.GetDiaDaSemandaParaEnvio)
            && (DateTime.Now.Hour.Equals(this.GetHoraDeEnvio)))
            {
                this.NotificarAssessor();
            }
        }

        #endregion

        #region | Métodos

        public void NotificarAssessor()
        {
            gLogger.Info("Coletando dados para enviar e-mail [START].");

            try
            {
                List<AssessorInfo> lListaDeAssessores = new PendenciaDbLib().GetPendencias();
                StringBuilder lClientesTexto;
                StringBuilder lEmailTexto;
                String lEmailDeDestino;

                gLogger.Info(string.Format("Dados para envio de e-mail coletados. Enviar para {0} assessores.", lListaDeAssessores.Count.ToString()));

                foreach (AssessorInfo itemAssessor in lListaDeAssessores)
                {
                    //if (itemAssessor.IdAssessor == 137)
                    {
                        gLogger.Info(string.Format("Iniciando - {2} - Enviar e-mail de notificação para assessor                 : {0} - {1}.", itemAssessor.IdAssessor.ToString(), itemAssessor.NomeAssessor, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));

                        lEmailTexto = new StringBuilder(this.GetTextoEmail);
                        lClientesTexto = new StringBuilder();

                        if (IsProducao)
                        {
                            lEmailTexto = lEmailTexto.Replace("@nome", itemAssessor.NomeAssessor);
                            lEmailDeDestino = itemAssessor.EmailAssessor;
                        }
                        else
                        {
                            lEmailTexto = lEmailTexto.Replace("@nome", string.Format("{0} - {1} - {2}", itemAssessor.NomeAssessor, itemAssessor.IdAssessor.ToString(), itemAssessor.EmailAssessor));
                            lEmailDeDestino = GetDestinatario;
                        }

                        foreach (ClienteInfo itemCliente in itemAssessor.Clientes)
                        {
                            lClientesTexto.AppendFormat("<b>Nome:</b> {0}<br />", itemCliente.NomeCliente);
                            lClientesTexto.AppendFormat("<b>CPF/CNPJ:</b> {0}<br />", itemCliente.CpfCnpjCliente);
                            lClientesTexto.AppendFormat("<b>Email:</b> {0}<br />", itemCliente.EmailCliente);
                            lClientesTexto.AppendFormat("<b>Código:</b> {0}<br />", itemCliente.CodigoBovespa);
                            lClientesTexto.Append("<b>Pendências: </b><ul style=\"margin-left: 45px;\">");

                            itemCliente.Pendencias.ForEach(
                                pen =>
                                {
                                    lClientesTexto.AppendFormat("<li><u>{0}</u> - <span style=\"font-size:smaller\">{1}</span></li><br />", pen.PendenciaTipo, pen.PendenciaDescricao);
                                });

                            lClientesTexto = lClientesTexto.Append("</ul><br />");
                        }

                        lEmailTexto = lEmailTexto.Replace("@clientes", lClientesTexto.ToString());

                        try
                        {
                            if (lEmailDeDestino.Contains("@"))
                            {
                                //new Email().EnviarEmail(GetSenderEmail, GetSenderDisplay, "brocha@gradualinvestimentos.com.br", GetAssunto, lEmailTexto.ToString(), GetSmtp);
                                new Email().EnviarEmail(GetSenderEmail, GetSenderDisplay, lEmailDeDestino, GetAssunto, lEmailTexto.ToString(), GetSmtp);
                                gLogger.Info(string.Format("Enviado   - {3} - Envido 1 e-mail com notificação de {0} clientes para assessor: {1} - {2}.", itemAssessor.Clientes.Count.ToString(), itemAssessor.IdAssessor.ToString(), itemAssessor.NomeAssessor, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                                gContadorEmailsEnviados++;
                            }
                            else
                            {
                                gLogger.Error(string.Format("Email Inválido - Não foi enviado email para o assessor {0} {1}, pois seu email não é válido: {2}."
                                            , itemAssessor.IdAssessor.ToString()
                                            , itemAssessor.NomeAssessor
                                            , itemAssessor.EmailAssessor));

                                gContadorEmailsComErro++;
                            }
                        }
                        catch (Exception ex)
                        {
                            gLogger.Error(string.Format("Erro ao Notificar Assessor: {1}{0}Assessor: {2}{0}IdAssessor: {3}{0}Email Assessor: {4}"
                                                        , Environment.NewLine
                                                        , ex.ToString()
                                                        , itemAssessor.NomeAssessor
                                                        , itemAssessor.IdAssessor.ToString()
                                                        , itemAssessor.EmailAssessor));

                            gContadorEmailsComErro++;
                        }
                    }
                }

                gLogger.Info(string.Format("{0} e-mails enviados para {1} assessores com {2} erros em {3}", gContadorEmailsEnviados.ToString(), lListaDeAssessores.Count - gContadorEmailsComErro, gContadorEmailsComErro, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                gContadorEmailsEnviados = default(int);
            }
            catch (Exception ex)
            {
                gLogger.Info("[Notificação Pendência] - Erro ao iniciar processo: ", ex);
            }
        }

        #endregion
    }
}
