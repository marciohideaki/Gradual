using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel.Description;
using System.Net;
using AppTesteDownloads.br.com.itaucustodia.www.DownloadArquivos;
using System.IO;
using log4net;
using System.Xml;
using System.Xml.Linq;
using Ionic.Zip;

namespace AppTesteDownloads
{
    public partial class Form1 : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const int DEFAULT_BUFSIZE = 1024;
        public static CookieContainer cookies = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                br.com.itaucustodia.www.DownloadArquivos.DownloadArquivoServiceService cliente = new br.com.itaucustodia.www.DownloadArquivos.DownloadArquivoServiceService();

                //cliente.Credentials = new NetworkCredential("gradual.op53", "1s22s22p6");

                string resp = cliente.saldosCotaAberturaD0XML("gradual.op53", "1s22s22p", "990686");

                //string resp = cliente.saldosCotaAberturaD0XMLNoZIP("gradual.op53", "1s22s22p", "990686");

                //resp = resp.Replace("<![CDATA[ ", "");
                //resp = resp.Replace(" ]]>", "");

                byte[] zipbytes = Convert.FromBase64String(resp);

                ZipFile zip = ZipFile.Read(zipbytes);
                zip.ExtractAll("c:\\temp\\itaufj\\recebidos", ExtractExistingFileAction.OverwriteSilently);


                //ParserXMLSaldosAberturaD0 pser = new ParserXMLSaldosAberturaD0();

                //pser.Parse(resp);



                //br.com.itaucustodia.www.PosicaoGerencial.PosicaoGerencialServiceService geren = new br.com.itaucustodia.www.PosicaoGerencial.PosicaoGerencialServiceService();


                //br.com.itaucustodia.www.PosicaoGerencial.SituacaoCotistaBean[] cotistas = geren.consultarSituacaoCotistas("gradual.op53", "1s22s22p", "990686");
                //string  cotistas = geren.consultarSituacaoCotistasXML("gradual.op53", "1s22s22p", "990686");






            }
            catch (Exception ex)
            {

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string resposta;
            FormData formData = new FormData();


            formData.Add("siteEscolhido", "passivo");
            formData.Add("res", "1280x1024");
            formData.Add("cli", "Microsoft+Internet+Explorer");
            formData.Add("site", "PFUNDOS");
            formData.Add("login", "gradual.op53");
            formData.Add("ebusiness", "gradual.op53");
            formData.Add("selectSite", "passivo");
            formData.Add("ususario", "gradual.op53");
            formData.Add("senha", "1s22s22p");
            formData.Add("x", "4");
            formData.Add("y", "4");

            string url = "https://www.itaucustodia.com.br/Passivo/login.do";

            resposta = _doRequest(url, formData.GetAll());

            formData.Clear();

            formData.Add("pageExecutionId", "03582178544379744");

            url = "https://www.itaucustodia.com.br/Passivo/abreFiltroDownloadArquivos.do";

            resposta = _doRequest(url, formData.GetAll());


            formData.Clear();
            formData.Add("nomeGestor", "GRADUAL+CCTVM+LTDA");
            formData.Add("idGestor", "0686");
            formData.Add("codigoGestor", "990686");
            formData.Add("tipoArquivo", "T");
            formData.Add("data", "16102012");

            url = "https://www.itaucustodia.com.br/Passivo/listarOpcoesArquivosDownloadArquivos.do";

            resposta = _doRequest(url, formData.GetAll());

            formData.Clear();
            formData.Add("numeroArquivo", "003");
            formData.Add("codigoGestor", "0686");
            formData.Add("nomeGestor", "GRADUAL CCTVM LTDA");
            formData.Add("tipoArquivo", "T");
            formData.Add("userId", "00038214");
            formData.Add("checkArquivos", "001");
            formData.Add("checkArquivos", "002");
            formData.Add("checkArquivos", "003");
            formData.Add("checkArquivos", "004");
            formData.Add("checkArquivos", "005");
            formData.Add("checkArquivos", "006");
            formData.Add("checkArquivos", "007");
            formData.Add("checkArquivos", "008");
            formData.Add("checkArquivos", "009");
            formData.Add("checkArquivos", "010");
            formData.Add("checkArquivos", "011");
            formData.Add("checkArquivos", "012");
            formData.Add("checkArquivos", "013");
            formData.Add("checkArquivos", "014");
            formData.Add("checkArquivos", "015");
            formData.Add("checkArquivos", "016");
            formData.Add("checkArquivos", "017");
            formData.Add("checkArquivos", "018");
            formData.Add("checkArquivos", "019");
            formData.Add("checkArquivos", "021");
            formData.Add("checkArquivos", "022");
            formData.Add("checkArquivos", "023");
            formData.Add("checkArquivos", "024");
            formData.Add("checkArquivos", "025");
            formData.Add("checkArquivos", "026");
            formData.Add("checkArquivos", "027");
            formData.Add("checkArquivos", "028");
            formData.Add("checkArquivos", "029");
            formData.Add("checkArquivos", "030");
            formData.Add("checkArquivos", "031");
            formData.Add("checkArquivos", "032");
            formData.Add("checkArquivos", "033");
            formData.Add("checkArquivos", "034");
            formData.Add("checkArquivos", "035");
            formData.Add("checkArquivos", "036");
            formData.Add("checkArquivos", "037");
            formData.Add("checkArquivos", "038");
            formData.Add("checkArquivos", "039");
            formData.Add("checkArquivos", "041");
            formData.Add("checkArquivos", "050");
            formData.Add("checkArquivos", "051");
            formData.Add("checkArquivos", "053");
            formData.Add("checkArquivos", "056");
            formData.Add("checkArquivos", "057");
            formData.Add("checkArquivos", "058");
            formData.Add("checkArquivos", "062");
            formData.Add("checkArquivos", "063");
            formData.Add("checkArquivos", "064");
            formData.Add("checkArquivos", "065");
            formData.Add("checkArquivos", "066");
            formData.Add("checkArquivos", "067");
            formData.Add("checkArquivos", "068");
            formData.Add("checkArquivos", "069");
            formData.Add("checkArquivos", "073");
            formData.Add("checkArquivos", "074");
            formData.Add("checkArquivos", "075");
            formData.Add("checkArquivos", "076");
            formData.Add("checkArquivos", "077");
            formData.Add("checkArquivos", "078");
            formData.Add("checkArquivos", "079");
            formData.Add("checkArquivos", "080");
            formData.Add("checkArquivos", "081");
            formData.Add("checkArquivos", "082");
            formData.Add("checkArquivos", "080");
            formData.Add("todosArquivos", "on");
            formData.Add("inputProcessandoArquivo", "");
            formData.Add("inputQtdeArquivosSelecionados", "");


            url = "https://www.itaucustodia.com.br/Passivo/processarDownloadArquivosAjax.do";

            resposta = _doRequest(url, formData.GetAll());

            formData.Clear();
            formData.Add("numeroArquivo", "029");
            formData.Add("codigoGestor", "0686");
            formData.Add("nomeGestor", "GRADUAL CCTVM LTDA");
            formData.Add("tipoArquivo", "T");
            formData.Add("userId", "00038214");
            formData.Add("checkArquivos", "001");
            formData.Add("checkArquivos", "002");
            formData.Add("checkArquivos", "003");
            formData.Add("checkArquivos", "004");
            formData.Add("checkArquivos", "005");
            formData.Add("checkArquivos", "006");
            formData.Add("checkArquivos", "007");
            formData.Add("checkArquivos", "008");
            formData.Add("checkArquivos", "009");
            formData.Add("checkArquivos", "010");
            formData.Add("checkArquivos", "011");
            formData.Add("checkArquivos", "012");
            formData.Add("checkArquivos", "013");
            formData.Add("checkArquivos", "014");
            formData.Add("checkArquivos", "015");
            formData.Add("checkArquivos", "016");
            formData.Add("checkArquivos", "017");
            formData.Add("checkArquivos", "018");
            formData.Add("checkArquivos", "019");
            formData.Add("checkArquivos", "021");
            formData.Add("checkArquivos", "022");
            formData.Add("checkArquivos", "023");
            formData.Add("checkArquivos", "024");
            formData.Add("checkArquivos", "025");
            formData.Add("checkArquivos", "026");
            formData.Add("checkArquivos", "027");
            formData.Add("checkArquivos", "028");
            formData.Add("checkArquivos", "029");
            formData.Add("checkArquivos", "030");
            formData.Add("checkArquivos", "031");
            formData.Add("checkArquivos", "032");
            formData.Add("checkArquivos", "033");
            formData.Add("checkArquivos", "034");
            formData.Add("checkArquivos", "035");
            formData.Add("checkArquivos", "036");
            formData.Add("checkArquivos", "037");
            formData.Add("checkArquivos", "038");
            formData.Add("checkArquivos", "039");
            formData.Add("checkArquivos", "041");
            formData.Add("checkArquivos", "050");
            formData.Add("checkArquivos", "051");
            formData.Add("checkArquivos", "053");
            formData.Add("checkArquivos", "056");
            formData.Add("checkArquivos", "057");
            formData.Add("checkArquivos", "058");
            formData.Add("checkArquivos", "062");
            formData.Add("checkArquivos", "063");
            formData.Add("checkArquivos", "064");
            formData.Add("checkArquivos", "065");
            formData.Add("checkArquivos", "066");
            formData.Add("checkArquivos", "067");
            formData.Add("checkArquivos", "068");
            formData.Add("checkArquivos", "069");
            formData.Add("checkArquivos", "073");
            formData.Add("checkArquivos", "074");
            formData.Add("checkArquivos", "075");
            formData.Add("checkArquivos", "076");
            formData.Add("checkArquivos", "077");
            formData.Add("checkArquivos", "078");
            formData.Add("checkArquivos", "079");
            formData.Add("checkArquivos", "080");
            formData.Add("checkArquivos", "081");
            formData.Add("checkArquivos", "082");
            formData.Add("checkArquivos", "080");
            formData.Add("todosArquivos", "on");
            formData.Add("inputProcessandoArquivo", "1");
            formData.Add("inputQtdeArquivosSelecionados", "3");


            url = "https://www.itaucustodia.com.br/Passivo/processarDownloadArquivosAjax.do";

            resposta = _doRequest(url, formData.GetAll());



            formData.Clear();
            formData.Add("numeroArquivo", "035");
            formData.Add("codigoGestor", "0686");
            formData.Add("nomeGestor", "GRADUAL CCTVM LTDA");
            formData.Add("tipoArquivo", "T");
            formData.Add("userId", "00038214");
            formData.Add("checkArquivos", "001");
            formData.Add("checkArquivos", "002");
            formData.Add("checkArquivos", "003");
            formData.Add("checkArquivos", "004");
            formData.Add("checkArquivos", "005");
            formData.Add("checkArquivos", "006");
            formData.Add("checkArquivos", "007");
            formData.Add("checkArquivos", "008");
            formData.Add("checkArquivos", "009");
            formData.Add("checkArquivos", "010");
            formData.Add("checkArquivos", "011");
            formData.Add("checkArquivos", "012");
            formData.Add("checkArquivos", "013");
            formData.Add("checkArquivos", "014");
            formData.Add("checkArquivos", "015");
            formData.Add("checkArquivos", "016");
            formData.Add("checkArquivos", "017");
            formData.Add("checkArquivos", "018");
            formData.Add("checkArquivos", "019");
            formData.Add("checkArquivos", "021");
            formData.Add("checkArquivos", "022");
            formData.Add("checkArquivos", "023");
            formData.Add("checkArquivos", "024");
            formData.Add("checkArquivos", "025");
            formData.Add("checkArquivos", "026");
            formData.Add("checkArquivos", "027");
            formData.Add("checkArquivos", "028");
            formData.Add("checkArquivos", "029");
            formData.Add("checkArquivos", "030");
            formData.Add("checkArquivos", "031");
            formData.Add("checkArquivos", "032");
            formData.Add("checkArquivos", "033");
            formData.Add("checkArquivos", "034");
            formData.Add("checkArquivos", "035");
            formData.Add("checkArquivos", "036");
            formData.Add("checkArquivos", "037");
            formData.Add("checkArquivos", "038");
            formData.Add("checkArquivos", "039");
            formData.Add("checkArquivos", "041");
            formData.Add("checkArquivos", "050");
            formData.Add("checkArquivos", "051");
            formData.Add("checkArquivos", "053");
            formData.Add("checkArquivos", "056");
            formData.Add("checkArquivos", "057");
            formData.Add("checkArquivos", "058");
            formData.Add("checkArquivos", "062");
            formData.Add("checkArquivos", "063");
            formData.Add("checkArquivos", "064");
            formData.Add("checkArquivos", "065");
            formData.Add("checkArquivos", "066");
            formData.Add("checkArquivos", "067");
            formData.Add("checkArquivos", "068");
            formData.Add("checkArquivos", "069");
            formData.Add("checkArquivos", "073");
            formData.Add("checkArquivos", "074");
            formData.Add("checkArquivos", "075");
            formData.Add("checkArquivos", "076");
            formData.Add("checkArquivos", "077");
            formData.Add("checkArquivos", "078");
            formData.Add("checkArquivos", "079");
            formData.Add("checkArquivos", "080");
            formData.Add("checkArquivos", "081");
            formData.Add("checkArquivos", "082");
            formData.Add("checkArquivos", "080");
            formData.Add("todosArquivos", "on");
            formData.Add("inputProcessandoArquivo", "2");
            formData.Add("inputQtdeArquivosSelecionados", "3");


            url = "https://www.itaucustodia.com.br/Passivo/processarDownloadArquivosAjax.do";

            resposta = _doRequest(url, formData.GetAll());

            formData.Clear();
            formData.Add("numeroArquivo", "062");
            formData.Add("codigoGestor", "0686");
            formData.Add("nomeGestor", "GRADUAL CCTVM LTDA");
            formData.Add("tipoArquivo", "T");
            formData.Add("userId", "00038214");
            formData.Add("checkArquivos", "001");
            formData.Add("checkArquivos", "002");
            formData.Add("checkArquivos", "003");
            formData.Add("checkArquivos", "004");
            formData.Add("checkArquivos", "005");
            formData.Add("checkArquivos", "006");
            formData.Add("checkArquivos", "007");
            formData.Add("checkArquivos", "008");
            formData.Add("checkArquivos", "009");
            formData.Add("checkArquivos", "010");
            formData.Add("checkArquivos", "011");
            formData.Add("checkArquivos", "012");
            formData.Add("checkArquivos", "013");
            formData.Add("checkArquivos", "014");
            formData.Add("checkArquivos", "015");
            formData.Add("checkArquivos", "016");
            formData.Add("checkArquivos", "017");
            formData.Add("checkArquivos", "018");
            formData.Add("checkArquivos", "019");
            formData.Add("checkArquivos", "021");
            formData.Add("checkArquivos", "022");
            formData.Add("checkArquivos", "023");
            formData.Add("checkArquivos", "024");
            formData.Add("checkArquivos", "025");
            formData.Add("checkArquivos", "026");
            formData.Add("checkArquivos", "027");
            formData.Add("checkArquivos", "028");
            formData.Add("checkArquivos", "029");
            formData.Add("checkArquivos", "030");
            formData.Add("checkArquivos", "031");
            formData.Add("checkArquivos", "032");
            formData.Add("checkArquivos", "033");
            formData.Add("checkArquivos", "034");
            formData.Add("checkArquivos", "035");
            formData.Add("checkArquivos", "036");
            formData.Add("checkArquivos", "037");
            formData.Add("checkArquivos", "038");
            formData.Add("checkArquivos", "039");
            formData.Add("checkArquivos", "041");
            formData.Add("checkArquivos", "050");
            formData.Add("checkArquivos", "051");
            formData.Add("checkArquivos", "053");
            formData.Add("checkArquivos", "056");
            formData.Add("checkArquivos", "057");
            formData.Add("checkArquivos", "058");
            formData.Add("checkArquivos", "062");
            formData.Add("checkArquivos", "063");
            formData.Add("checkArquivos", "064");
            formData.Add("checkArquivos", "065");
            formData.Add("checkArquivos", "066");
            formData.Add("checkArquivos", "067");
            formData.Add("checkArquivos", "068");
            formData.Add("checkArquivos", "069");
            formData.Add("checkArquivos", "073");
            formData.Add("checkArquivos", "074");
            formData.Add("checkArquivos", "075");
            formData.Add("checkArquivos", "076");
            formData.Add("checkArquivos", "077");
            formData.Add("checkArquivos", "078");
            formData.Add("checkArquivos", "079");
            formData.Add("checkArquivos", "080");
            formData.Add("checkArquivos", "081");
            formData.Add("checkArquivos", "082");
            formData.Add("checkArquivos", "080");
            formData.Add("todosArquivos", "on");
            formData.Add("inputProcessandoArquivo", "2");
            formData.Add("inputQtdeArquivosSelecionados", "3");


            url = "https://www.itaucustodia.com.br/Passivo/processarDownloadArquivosAjax.do";

            resposta = _doRequest(url, formData.GetAll());

            formData.Clear();
            formData.Add("numeroArquivo", "003;029;035;062");
            formData.Add("codigoGestor", "0686");
            formData.Add("nomeGestor", "GRADUAL+CCTVM+LTDA");
            formData.Add("tipoArquivo", "T");
            formData.Add("userId", "00038214");
            formData.Add("inputProcessandoArquivo", "4");
            formData.Add("inputQtdeArquivosSelecionados", "4");


            url = "https://www.itaucustodia.com.br/Passivo/downloadArquivosLista.do";

            resposta = _doRequest(url, formData.GetAll());

            formData.Clear();
            formData.Add("numerosArquivosSelecionados", "003;029;062");
            formData.Add("checkArquivos", "003");
            formData.Add("checkArquivos", "029");
            formData.Add("checkArquivos", "062");


            url = "https://www.itaucustodia.com.br/Passivo/EfetuarDownloadArquivosListaServlet";

            resposta = _doRequest(url, formData.GetAll());

        }


        public static CookieContainer GetCookieContainer()
        {
            if (cookies == null)
            {
                cookies = new CookieContainer();
            }

            return cookies;
        }

        private string _doRequest(string siteURL, Dictionary<string, string> formData)
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
            request.CookieContainer = Form1.GetCookieContainer();

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

        private string _doRequest(string siteURL, List<KeyValuePair<string, string>> formData)
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
            request.CookieContainer = Form1.GetCookieContainer();

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

    }
}
