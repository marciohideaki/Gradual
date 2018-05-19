using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using log4net;
using System.Net;
using System.IO;
using Ionic.Zip;
using System.Configuration;
using System.Threading;

namespace Gradual.Integracao.Itau.Downloader
{
    public class ServicoItauDownloader : IServicoControlavel
    {
        private ServicoStatus _serviceStatus = ServicoStatus.Parado;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static CookieContainer cookies = null;
        private const int DEFAULT_BUFSIZE = 1024;
        private string _dirArquivosRecebidos;
        private string _dirArquivosProcessadosBase;
        private string _dirArquivosErroBase;
        private string _dirArquivoExtracao;
        private string _dirArquivosErro;
        private int _intervaloPoll = 300;
        private bool _bKeepRunning = false;
        CronStyleScheduler cron = null;

        public void IniciarServico()
        {
            _bKeepRunning = true;

            logger.Info("Iniciando ServicoItauDownloader");

            if (ConfigurationManager.AppSettings["DiretorioArquivosRecebidos"] != null)
            {
                _dirArquivosRecebidos = ConfigurationManager.AppSettings["DiretorioArquivosRecebidos"].ToString();
            }

            if (ConfigurationManager.AppSettings["DiretorioArquivosExtracao"] != null)
            {
                _dirArquivosProcessadosBase = ConfigurationManager.AppSettings["DiretorioArquivosExtracao"].ToString();
            }

            if (ConfigurationManager.AppSettings["DiretorioArquivosErro"] != null)
            {
                _dirArquivosErroBase = ConfigurationManager.AppSettings["DiretorioArquivosErro"].ToString();
            }

            if (ConfigurationManager.AppSettings["IntervaloMonitoracao"] != null)
            {
                _intervaloPoll = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloMonitoracao"].ToString());
            }

            logger.Info("Parametros de inicializacao:");
            logger.Info("Diretorio de arquivos recebidos ....: " + _dirArquivosRecebidos);
            logger.Info("Diretorio de arquivos rejeitados ...: " + _dirArquivosErroBase);
            logger.Info("Diretorio temporario extracao ......: " + _dirArquivoExtracao);
            logger.Info("Intervalo de monitoracao ...........: " + _intervaloPoll + "s.");

            cron = new CronStyleScheduler();

            // Sleep para fins de debug
            //Thread.Sleep(20000);

            cron.Start();

            logger.Info("ServicoItauDownloader iniciado");

        }

        public void PararServico()
        {
            cron.Stop();

            logger.Info("ServicoItauDownloader finalizado");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _serviceStatus;
        }

        public static CookieContainer GetCookieContainer()
        {
            if (cookies == null)
            {
                cookies = new CookieContainer();
            }

            return cookies;
        }

        #region private methods
        private void thMonitorDiretorio()
        {
            string[] arqRecebidos;
            DateTime lastCheck = DateTime.Now;

            while (_bKeepRunning)
            {
                try
                {
                    TimeSpan lastInterval = new TimeSpan(DateTime.Now.Ticks - lastCheck.Ticks);

                    if (lastInterval.TotalMilliseconds > _intervaloPoll)
                    {
                        lastCheck = DateTime.Now;

                        // Criando diretorios
                        _dirArquivosErro = _dirArquivosErroBase + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                        //_dirArquivosProcessados = _dirArquivosProcessadosBase + "\\" + DateTime.Now.ToString("yyyy-MM-dd");

                        if (!Directory.Exists(_dirArquivosErro))
                        {
                            logger.Info("Criando diretorio [" + _dirArquivosErro + "]");
                            Directory.CreateDirectory(_dirArquivosErro);
                        }

                        //if (!Directory.Exists(_dirArquivosProcessados))
                        //{
                        //    logger.Info("Criando diretorio [" + _dirArquivosProcessados + "]");
                        //    Directory.CreateDirectory(_dirArquivosProcessados);
                        //}
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("thMonitorDiretorio(): " + ex.Message, ex);
                }

                Thread.Sleep(250);

            }
        }



        private string  _doRequest(string siteURL, Dictionary<string,string> formData )
        {
            DateTime antes = DateTime.Now;
            DateTime depois = DateTime.Now;
            string resposta = String.Empty;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(siteURL);

            logger.Debug("Populando post");

            // Set the 'Method' property of the 'Webrequest' to 'POST'.
            request.Method = "POST";
            request.ContentType = "application/json";
            request.KeepAlive = true;
            request.ServicePoint.Expect100Continue = false;
            request.Pipelined = true;
            //request.ServicePoint.ConnectionLimit = 1; <- Nao descomentar 
            request.CookieContainer = ServicoItauDownloader.GetCookieContainer();

            logger.Debug("Pegando mensagem json");

            StringBuilder postData = new StringBuilder();

            int totItems = formData.Count;
            int i = 0;
            foreach (KeyValuePair<string, string> item in formData)
            {
                i++;
                postData.Append(item.Key + "=" + item.Value);
                if (i < formData.Count)
                    postData.Append("&");
            }

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postdata = encoding.GetBytes(postData.ToString());

            // Set the content type of the data being posted.
            request.ContentType = "application/x-www-form-urlencoded";

            // Set the content length of the string being posted.
            request.ContentLength = postdata.Length;

            logger.Debug("Abrindo request stream");

            Stream newStream = request.GetRequestStream();

            logger.Debug("Gravando no stream de request");

            newStream.Write(postdata, 0, postdata.Length);

            depois = DateTime.Now;

            logger.Debug("HttpConversation() fim do POST: " + (new TimeSpan(depois.Ticks - antes.Ticks)).TotalMilliseconds + "ms.");

            antes = DateTime.Now;

            WebResponse response = request.GetResponse();

            long respSize = response.ContentLength;
            Stream respStream = response.GetResponseStream();
            depois = DateTime.Now;

            logger.Debug("HttpConversation() abertura do response stream: " + (new TimeSpan(depois.Ticks - antes.Ticks)).TotalMilliseconds + "ms.");

            if (respSize > 0)
            {
                byte[] respBuf = new byte[respSize];

                long totalRead = 0;
                long toRead = respSize;
                long bytesRead;

                antes = DateTime.Now;

                while (true)
                {
                    // read bytes with msg length
                    bytesRead = respStream.Read(respBuf, (int)totalRead, (int)toRead);

                    totalRead += bytesRead;
                    if (totalRead < respSize)
                    {
                        toRead -= bytesRead;
                        continue;
                    }
                    else
                        break;
                }

                depois = DateTime.Now;

                logger.Debug("HttpConversation() leitura do buffer com ContentLength: " + (new TimeSpan(depois.Ticks - antes.Ticks)).TotalMilliseconds + "ms.");

                respStream.Close();

                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                resposta = enc.GetString(respBuf);
            }
            else
            {
                byte[] respBlock = new byte[DEFAULT_BUFSIZE];
                MemoryStream respMemStream = new MemoryStream();

                antes = DateTime.Now;

                while (true)
                {
                    // read bytes with msg length
                    long bytesRead = respStream.Read(respBlock, 0, DEFAULT_BUFSIZE);

                    if (bytesRead <= 0)
                        break;
                    else
                        respMemStream.Write(respBlock, 0, (int)bytesRead);
                }

                depois = DateTime.Now;

                logger.Debug("HttpConversation() leitura do buffer sem ContentLength: " + (new TimeSpan(depois.Ticks - antes.Ticks)).TotalMilliseconds + "ms.");

                if (respMemStream.Length > 0)
                {
                    byte[] respBuf = respMemStream.ToArray();

                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                    resposta = enc.GetString(respBuf);
                }
            }

            return resposta;
        }
        #endregion // private methods
    }
}
