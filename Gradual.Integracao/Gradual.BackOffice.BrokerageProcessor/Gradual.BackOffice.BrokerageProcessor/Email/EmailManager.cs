using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.BackOffice.BrokerageProcessor.Lib.Email;
using System.Collections.Concurrent;
using System.Threading;
using System.Configuration;
using Gradual.Spider.Utils.Email;
using Gradual.Spider.Utils.Email.Entities;
using Gradual.BackOffice.BrokerageProcessor.Db;
using Gradual.BackOffice.BrokerageProcessor.Lib.FileWatcher;
using System.IO;


namespace Gradual.BackOffice.BrokerageProcessor.Email
{
    public class EmailManager
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Static Objects
        private static EmailManager _me = null;
        public static EmailManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new EmailManager();
                }

                return _me;
            }
        }
        #endregion


        #region private variables
        bool _isRunning = false;

        object _syncEmail = new object();
        ConcurrentQueue<TOEmail> _cqEmail;
        Thread _thEmail;

        string _server = string.Empty;
        int _port = 0;
        ConfigMailInfo _cfg;
        DbEmail _db;

        string _emailAlert;
        string _emailCc;
        string _emailCco;
        string _emailFrom;
        string _emailTo;

        string _templateContent;
        #endregion


        public EmailManager()
        {
            
        }

        public void Start()
        {
            try
            {
                logger.Info("Iniciando Email Manager...");
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SmtpServer"))
                    _server = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                else
                    throw new Exception("Parametro SmtpServer nao encontrado!!!");

                if (ConfigurationManager.AppSettings.AllKeys.Contains("SmtpPort"))
                    _port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                else
                    throw new Exception("Parametro SmtpPort nao encontrado!!!");

                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmailFrom"))
                    _emailFrom = ConfigurationManager.AppSettings["EmailFrom"].ToString();
                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmailAlert"))
                    _emailAlert = ConfigurationManager.AppSettings["EmailAlert"].ToString();
                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmailTo"))
                    _emailTo = ConfigurationManager.AppSettings["EmailTo"].ToString();
                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmailCc"))
                    _emailCc = ConfigurationManager.AppSettings["EmailCc"].ToString();
                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmailCco"))
                    _emailCco = ConfigurationManager.AppSettings["EmailCco"].ToString();


                _db = new DbEmail();
                _cfg = new ConfigMailInfo();
                _cfg.SmtpHost = _server;
                _cfg.SmtpPort = _port.ToString();
                _cqEmail = new ConcurrentQueue<TOEmail>();
                _thEmail = new Thread(new ThreadStart(this._queueEmail));
                _thEmail.Start();
                
                _isRunning = true;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do Email Manager: " + ex.Message, ex);
            }
        }


        public void Stop()
        {
            try
            {
                logger.Info("Parando Email Manager... ");
                _isRunning = false;

                if (_thEmail != null && _thEmail.IsAlive)
                {
                    try
                    {
                        _thEmail.Join(200);
                        if (_thEmail.IsAlive)
                            _thEmail.Abort();

                        _thEmail = null;
                    }
                    catch
                    {
                        logger.Error("Thread _thEmail aborted");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop do Email Manager: " + ex.Message, ex);
            }
        }

        public void AddEmail(TOEmail email)
        {
            _cqEmail.Enqueue(email);
            lock (_syncEmail)
                Monitor.Pulse(_syncEmail);
        }

        private void _queueEmail()
        {
            try
            {
                while (_isRunning)
                {
                    TOEmail to = null;
                    if (_cqEmail.TryDequeue(out to))
                    {
                        if (to != null)
                            this._processEmail(to);
                    }
                    else
                    {
                        lock (_syncEmail)
                        {
                            Monitor.Wait(_syncEmail, 200);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro no envio do email: " + ex.Message, ex);
            }
        }

        private void _processEmail(TOEmail email)
        {
            EmailNotaCorretagemInfo info = new EmailNotaCorretagemInfo();
            try
            {
                // Inserir no BD
                info.ArquivoNota = email.Msg.FileAttach;
                info.Bolsa = email.Exchange;
                info.DtRegistroEmail = DateTime.Now;
                info.EmailDestinatario = email.Msg.To;
                if (!string.IsNullOrEmpty(email.Msg.Cc))
                    info.EmailDestinatarioCc = email.Msg.Cc;
                if (!string.IsNullOrEmpty(email.Msg.Cco))
                    info.EmailDestinatarioCco = email.Msg.Cco;
                info.Assunto = email.Msg.Subject;
                info.Body = email.Msg.Body;
                info.EmailOrigem = email.Msg.From;
                info.Status = email.Status;
                info.DescStatus = email.DescStatus;

                if (!string.IsNullOrEmpty(email.IdCliente)) 
                    info.IdCliente = Convert.ToInt32(email.IdCliente);

                if (!email.Status.Equals(StatusInfo.ERRO) && !string.IsNullOrEmpty(email.Msg.To))
                {
                    logger.DebugFormat("===> SendEmail: IDCliente[{0}] Emails:[{1}] Subject:[{2}]", info.IdCliente, email.Msg.To, email.Msg.Subject);
                    SpiderMail.SendEmail(_cfg, email.Msg);
                }
                else
                {
                    logger.DebugFormat("===> SendEmail: Email com ERRO ou destinatario vazio. IDCliente[{0}], Emails:[{1}] Subject:[{2}]", info.IdCliente, email.Msg.To, email.Msg.Subject);
                }
                if (_db != null)
                {
                    _db.InserirEmailNotaCorretagem(info);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro no envio dos emails: " + ex.Message, ex);
                info.Status = StatusInfo.ERRO;
                info.DescStatus = ex.Message;
                if (_db !=null)
                    _db.InserirEmailNotaCorretagem(info);

            }
        }

        public void ProcessEmailPath(string path, FileWatcherConfigItem cfg)
        {
            try
            {
                StringBuilder sbAlert = new StringBuilder();
                _composeMessageAlert(ref sbAlert, "Diretorio a processar: " + path, 1);

                // Buscando os emails
                Dictionary<int, List<string>> dicEmails = new Dictionary<int, List<string>>();
                logger.Info("Buscando Emails: " + cfg.Exchange);
                switch (cfg.Type)
                {
                    case TypeWatcher.BMF:
                        dicEmails = new DbEmailOracle().BuscarClienteBmf();
                        break;
                    case TypeWatcher.BOVESPA:
                        dicEmails = new DbEmailOracle().BuscarClienteBovespa();
                        break;
                    case TypeWatcher.POSICAO_BMF:
                        dicEmails = new DbEmail().BuscarPosicaoClienteEmail();
                        break;
                }
                if (dicEmails == null || dicEmails.Count == 0)
                {
                    logger.Error("Problemas na consulta dos emails dos clientes...");
                    return;
                }
                logger.InfoFormat("Fim busca Emails. Tipo [{0}]. Accounts: [{1}] ", cfg.NameType, dicEmails.Count);

                

                // Ler o diretorio com os arquivos
                if (Directory.Exists(path))
                {
                    string[] arqNames = Directory.GetFiles(path);
                    logger.Info("Numero de Arquivos: " + arqNames.Length);
                    foreach(string arqName in arqNames)
                    {
                        int idClient = this._extractIdCliente(arqName);
                        if (idClient == 0)
                        {
                            _composeMessageAlert(ref sbAlert, "ERRO. Não foi possivel extrair  IdCliente", 2);
                            logger.Error("ERRO. Impossivel buscar cliente");
                            continue;
                        }
                        
                        string emailTo = string.Empty;
                        if (!this._getEmail(idClient, dicEmails, ref emailTo))
                        {
                            _composeMessageAlert(ref sbAlert, "Email inexistente para cliente :" + idClient, 2);
                        }
                        else
                        {
                            string msg = string.Format("FileName: [{0}] IdCliente: [{1}] Email: [{2}]", arqName, idClient.ToString("D8"), emailTo);
                            _composeMessageAlert(ref sbAlert, msg, 1);
                            TOEmail toMsg = _composeEmailMsg(_emailFrom, emailTo, arqName, cfg.Exchange, idClient.ToString(), cfg, dicEmails);
                            this.AddEmail(toMsg);
                        }
                    }
                }
                else
                {
                    _composeMessageAlert(ref sbAlert, "Diretorio nao existe!!", 2);
                }

                // Gerando mensagem de processamento
                TOEmail toEmailAlert = this._composeEmailAlert(_emailFrom, _emailAlert, _emailCc, 
                                      _emailCco, string.Format("{0} - Processamento de Arquivo", cfg.NameType));
                toEmailAlert.Msg.Body = sbAlert.ToString();
                this.AddEmail(toEmailAlert);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento do diretorio de arquivos: " + path + " " + ex.Message, ex);
            }
        }

        private int _extractIdCliente(string fileName)
        {
            int ret = 0;
            try
            {
                string strid = fileName.Substring(fileName.LastIndexOf("\\")+1);
                string []strAux = strid.Split( new char [] {'-', '.'}, StringSplitOptions.RemoveEmptyEntries);
                if (strAux.Length > 0)
                {
                    ret = Convert.ToInt32(strAux[0]);
                }
                return ret;
            }
            catch
            {
                return 0;
            }
        }
        public void _composeMessageAlert(ref StringBuilder sb, string msg, int logtype =0)
        {
            if (sb == null)
                sb = new StringBuilder();
            if (!msg.EndsWith("."))
                msg = msg + ".";
                sb.AppendLine(msg);
            switch (logtype)
            {
                case 1: logger.Info(msg);
                    break;
                case 2: logger.Error(msg);
                    break;
            }
        }

        private TOEmail _composeEmailAlert(string emailFrom, string emailTo, string emailCc, string emailCco, string subject)
        {
            TOEmail ret = new TOEmail();
            ret.Msg.From = emailFrom;
            if (!string.IsNullOrEmpty(emailTo))
                ret.Msg.To = emailTo;
            if (!string.IsNullOrEmpty(emailCc))
                ret.Msg.Cc = emailCc;
            if (!string.IsNullOrEmpty(emailCco))
                ret.Msg.Cco = emailCco;
            if (!string.IsNullOrEmpty(subject))
                ret.Msg.Subject = subject;

            ret.Msg.IsBodyHtml = false;
            return ret;
        }


        private TOEmail _composeEmailMsg(string emailFrom, string emailTo, string file, string exchange, string idCliente, FileWatcherConfigItem cfg, Dictionary<int, List<string>> dicEmails = null)
        {
            TOEmail ret = new TOEmail();
            try
            {
                string subjectBrokerage;
                if (!string.IsNullOrEmpty(cfg.TemplateFile) && string.IsNullOrEmpty(_templateContent))
                {
                    _templateContent = File.ReadAllText(cfg.TemplateFile);
                }
                subjectBrokerage = cfg.SubjectEmail.Replace("#clientid#", idCliente);
                ret.IdCliente = idCliente;
                ret.Exchange = exchange;
                ret.Msg.From = emailFrom;
                ret.Msg.To = emailTo;
                ret.Msg.FileAttach = file;
                ret.Msg.Subject = subjectBrokerage;
                if (!string.IsNullOrEmpty(_templateContent))
                {
                    ret.Msg.Body = _templateContent;
                    ret.Msg.IsBodyHtml = true;
                }

                logger.DebugFormat("===> Email Message Compose: From:[{0}] To:[{1}] Subject:[{2}]", ret.Msg.From, ret.Msg.To, ret.Msg.Subject);

                // Verificacao do ClientID com Assunto, anexo e emails gerados.
                if (cfg.ClientIdCheck)
                {
                    int aux;
                    if (int.TryParse(ret.IdCliente, out aux))
                    {
                        if (aux != 0)
                        {
                            // Verificar se IdCliente esta presente no assunto
                            if (ret.Msg.Subject.IndexOf(ret.IdCliente) < 0)
                                throw new Exception("Client ID is not present at email subject");

                            // Verificar se IdCliente esta no anexo
                            string fileAux = ret.Msg.FileAttach.Substring(ret.Msg.FileAttach.LastIndexOf("\\"));
                            if (fileAux.IndexOf(ret.IdCliente) < 0)
                                throw new Exception("Client ID is not present at file name");

                            // Verificar se os emails da mensagem correspondem ao cliente
                            if (dicEmails == null)
                                throw new Exception("Email collection is null");

                            List<string> lst = null;
                            if (dicEmails.TryGetValue(aux, out lst))
                            {
                                foreach (string email in lst)
                                {
                                    if (ret.Msg.To.IndexOf(email, StringComparison.CurrentCultureIgnoreCase) < 0)
                                        throw new Exception("Email differs from collection");
                                }
                            }
                        }
                    }
                    else
                        throw new Exception("Unable to parse ClientId to int to proceed check");
                }


                
                // Verificacao de testes: se os parametros destinatarios de emails estao definidos no config do servico, irao 
                // sobrescrever os sets vindo dos parametros
                if (!string.IsNullOrEmpty(_emailTo))
                {
                    ret.Msg.Subject = string.Format("{0} - [{1}]", ret.Msg.Subject, ret.Msg.To);
                    ret.Msg.To = _emailTo;
                }
                if (!string.IsNullOrEmpty(_emailCc))
                    ret.Msg.Cc = _emailCc;
                if (!string.IsNullOrEmpty(_emailCco))
                    ret.Msg.Cco = _emailCco;
                
                ret.Status = StatusInfo.OK;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na composicao da mensagem de Nota de Corretagem: " + ex.Message, ex);
                ret = null;
            }
            return ret;
        }

        private bool _getEmail(int account, Dictionary<int, List<string>> emails, ref string strEmail)
        {
            bool ret = false;
            strEmail = string.Empty;
            try
            {
                List<string> lst = null;
                if (emails.TryGetValue(account, out lst))
                {
                    foreach (string aux in lst)
                    {
                        strEmail += aux + ";";
                    }
                    if (string.IsNullOrEmpty(strEmail) || strEmail.IndexOf("@") < 0)
                        ret = false;
                    else
                        ret = true;
                }
                else
                    ret = false;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na busca dos emails: " + ex.Message, ex);
            }
            return ret;
        }
    }
}
