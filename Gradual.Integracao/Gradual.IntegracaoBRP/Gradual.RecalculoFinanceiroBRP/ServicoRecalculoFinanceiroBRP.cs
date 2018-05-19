using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.IntegracaoBRP.Lib;
using System.Configuration;
using Gradual.IntegracaoBRP.Lib.Dados;
using System.Threading;
using Gradual.OMS.Library;
using System.Net.Mail;

namespace Gradual.RecalculoFinanceiroBRP
{
    public class ServicoRecalculoFinanceiroBRP : IServicoControlavel
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServicoStatus _status = ServicoStatus.Parado;
        private static ServicoRecalculoFinanceiroBRP _me = null;
        CronStyleScheduler _scheduler = null;
        private static bool bRecalculando = false;
        private static DateTime lastRun = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8,0,0);
        private static DateTime lastMonitor = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
        private static DateTime initialRun = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
        private static Dictionary<int, int> dctRecalculado = new Dictionary<int, int>();

        public ServicoRecalculoFinanceiroBRP GetInstance()
        {
            if (_me == null)
            {
                _me = new ServicoRecalculoFinanceiroBRP();
            }

            return _me;
        }

        public void IniciarServico()
        {
            logger.Info("Iniciando ServicoRecalculoFinanceiroBRP");

            _status = ServicoStatus.EmExecucao;

            _scheduler = new CronStyleScheduler();

            _scheduler.Start();

            logger.Info("ServicoRecalculoFinanceiroBRP iniciado");
        }

        public void PararServico()
        {
            logger.Info("Finalizando ServicoRecalculoFinanceiroBRP");

            _scheduler.Stop();

            _status = ServicoStatus.Parado;

            logger.Info("ServicoRecalculoFinanceiroBRP finalizado");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }


        public void RecalcularFinanceiro(string param)
        {
            bool bIgnoreTimestamp = false;

            if (!String.IsNullOrEmpty(param))
            {
                if (param.ToLowerInvariant().Equals("ignoretimestamp"))
                {
                    bIgnoreTimestamp = true;
                    EnviarRelatorioRecalculoUltimaHora();
                    dctRecalculado.Clear();
                    initialRun = lastRun;
                    lastRun = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                    logger.Info("IGNORANDO TIMESTAMP DA ORDEM");
                }
                else
                {
                    if (bRecalculando)
                    {
                        logger.Warn("Loop de recalculo em andamento, deixando para proxima rodada");
                        return;
                    }
                }
            }

            bRecalculando = true;

            logger.Info("Recalculando ordens executadas desde [" + lastRun.ToString("dd/MM/yyyy HH:mm:ss.fff") + "]");

            ConfigurationManager.RefreshSection("appSettings");

            int faxProcedureInterval = 10 * 1000;
            if (ConfigurationManager.AppSettings["FaxProcedureInterval"] != null)
            {
                faxProcedureInterval = Convert.ToInt32(ConfigurationManager.AppSettings["FaxProcedureInterval"].ToString());
            }


            try
            {
                PersistenciaDB db = new PersistenciaDB(ConfigurationManager.ConnectionStrings["TRADE"].ConnectionString);

                Dictionary<int, ComitenteDiaInfo> dctComit = db.BuscarComitentesDia(lastRun.AddMinutes(-5));

                lastRun = DateTime.Now;

                logger.Info("Comitentes no dia com " + dctComit.Count + " itens");

                foreach (int account in dctComit.Keys)
                {
                    try
                    {
                        db = new PersistenciaDB(ConfigurationManager.ConnectionStrings["SINACOR"].ConnectionString);

                        if (db.EfetuarRecalculo(account, DateTime.Now))
                        {
                            logger.Info("Recalculo da conta [" + account + "] chamada com sucesso");
                            if (!dctRecalculado.ContainsKey(account) && !bIgnoreTimestamp) 
                                dctRecalculado.Add(account, account);
                        }
                        else
                            logger.Error("Erro no recalculo da conta [" + account + "]");

                        if (!bIgnoreTimestamp)
                        {
                            // Calcula o intervalo correto para efetuar todos os recalculos
                            // em 58s
                            faxProcedureInterval = 58000 / dctComit.Keys.Count;
                            Thread.Sleep(faxProcedureInterval);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Erro no recalculo da conta [" + account + "]: " + ex.Message, ex);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("RecalcularFinanceiro(): " + ex.Message, ex);
            }
            finally
            {
                bRecalculando = false;
            }

        }


        public void MonitorarRoboPlural()
        {
            try
            {
                PersistenciaDB db = new PersistenciaDB(ConfigurationManager.ConnectionStrings["TRADE"].ConnectionString);

                Dictionary<int, ComitenteDiaInfo> dctComit = db.BuscarComitentesDia(lastMonitor);

                logger.Info("Monitorou comitentes no dia com " + dctComit.Count + " itens");

                if (DateTime.Now.Hour >= 10 && DateTime.Now.Hour < 18)
                {
                    if (dctComit.Count == 0)
                    {
                        logger.Warn("NAO RETORNOU NOVOS REGISTROS DESDE [" + lastMonitor.ToString("dd/MM/yyyy HH:mm:ss") + "]");

                        string message = "NAO RETORNOU NOVOS REGISTROS DESDE [" + lastMonitor.ToString("dd/MM/yyyy HH:mm:ss") + "].";

                        message += Environment.NewLine + "Verificar robo da Brasil Plural.";

                        string subject = "Alerta Servico RecalculoFinanceiroBRP: " + DateTime.Now.ToString("HH:mm:ss");

                        Utilities.EnviarEmail(subject, message);

                        lastRun = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                    }
                    else
                        lastMonitor = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                logger.Error("MonitorarRoboPlural(): " + ex.Message, ex);
            }
        }

        private bool EnviarRelatorioRecalculoUltimaHora()
        {
            try
            {
                string[] emailsTo;

                if (ConfigurationManager.AppSettings["EmailAlertaRemetente"] == null)
                {
                    logger.Error("AppSetting 'EmailAlertaRemetente' nao definido. Nao eh possivel enviar relatorio de envios");
                    return false;
                }

                if (ConfigurationManager.AppSettings["EmailRelatorioDestinatarios"] == null)
                {
                    logger.Fatal("AppSetting 'EmailRelatorioDestinatarios' deve ser definido");
                    return false;
                }

                char[] seps = { ';', ',' };
                emailsTo = ConfigurationManager.AppSettings["EmailRelatorioDestinatarios"].ToString().Split(seps);

                var lMensagem = new MailMessage(ConfigurationManager.AppSettings["EmailAlertaRemetente"].ToString(), emailsTo[0]);

                for (int i = 1; i < emailsTo.Length; i++)
                {
                    if (!String.IsNullOrEmpty(emailsTo[i]))
                        lMensagem.To.Add(emailsTo[i]);
                }

                string relatorio = "Contas recalculadas no intervalo entre " + initialRun.ToString("HH:mm:ss") + " e " + lastRun.ToString("HH:mm:ss") + "<br><br><br>";

                int ii = 0;
                List<int> lista = dctRecalculado.Keys.ToList();
                lista.Sort();
                while ( ii < lista.Count )
                {
                    for( int j=0; j < 10 && ii < lista.Count; j++ )
                    {
                        if (lista[ii] != null )
                            relatorio += lista[ii].ToString().PadLeft(8);
                        ii++;
                    }
                    relatorio += "<br>";
                }

                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailAlertaReplyTo"].ToString()));
                lMensagem.Subject = "Relatorio Servico RecalculoFinanceiro BRP " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                lMensagem.IsBodyHtml = true;
                lMensagem.Body = "<html><body style=\"font-family:courier;\">" + relatorio.Replace(" ", "&nbsp;").Replace(Environment.NewLine, "<br>" + Environment.NewLine) + "</body></html>";

                new SmtpClient(ConfigurationManager.AppSettings["EmailAlertaHost"].ToString()).Send(lMensagem);

                logger.Info("Email de relatorio de envios submetido com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("_enviaRelatorio(): " + ex.Message, ex);
                return false;
            }

            return true;
        }
    }
}
