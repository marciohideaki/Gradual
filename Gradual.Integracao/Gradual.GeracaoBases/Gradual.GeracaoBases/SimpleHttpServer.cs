using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using Gradual.GeracaoBasesDB.Lib.Dados;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;

// offered to the public domain for any use with no restriction
// and also with no warranty of any kind, please enjoy. - David Jeske. 

// simple HTTP explanation
// http://www.jmarshall.com/easy/http/

namespace Gradual.GeracaoBases
{
    public class HttpProcessor
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public TcpClient socket;        
        public HttpServer srv;

        private Stream inputStream;
        public StreamWriter outputStream;

        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();


        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        public HttpProcessor(TcpClient s, HttpServer srv)
        {
            this.socket = s;
            this.srv = srv;                   
        }
        

        private string streamReadLine(Stream inputStream) {
            int next_char;
            string data = "";
            while (true) {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }            
            return data;
        }
        public void process() {                        
            // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
            // "processed" view of the world, and we want the data raw after the headers
            inputStream = new BufferedStream(socket.GetStream());

            // we probably shouldn't be using a streamwriter for all output from handlers either
            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try {
                parseRequest();
                readHeaders();
                if (http_method.Equals("GET")) {
                    handleGETRequest();
                } else if (http_method.Equals("POST")) {
                    handlePOSTRequest();
                }
            } catch (Exception e) {
                logger.DebugFormat("Exception: " + e.ToString());
                writeFailure();
            }
            outputStream.Flush();
            // bs.Flush(); // flush any remaining output
            inputStream = null; outputStream = null; // bs = null;            
            socket.Close();             
        }

        public void parseRequest() {
            String request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3) {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            logger.DebugFormat("starting: " + request);
        }

        public void readHeaders() {
            logger.DebugFormat("readHeaders()");
            String line;
            while ((line = streamReadLine(inputStream)) != null) {
                if (line.Equals("")) {
                    logger.DebugFormat("got headers");
                    return;
                }
                
                int separator = line.IndexOf(':');
                if (separator == -1) {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' ')) {
                    pos++; // strip any spaces
                }
                    
                string value = line.Substring(pos, line.Length - pos);
                logger.DebugFormat("header: {0}:{1}",name,value);
                httpHeaders[name] = value;
            }
        }

        public void handleGETRequest() {
            srv.handleGETRequest(this);
        }

        private const int BUF_SIZE = 4096;
        public void handlePOSTRequest() {
            // this post data processing just reads everything into a memory stream.
            // this is fine for smallish things, but for large stuff we should really
            // hand an input stream to the request processor. However, the input stream 
            // we hand him needs to let him see the "end of the stream" at this content 
            // length, because otherwise he won't know when he's seen it all! 

            logger.DebugFormat("get post data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length")) {
                 content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                 if (content_len > MAX_POST_SIZE) {
                     throw new Exception(
                         String.Format("POST Content-Length({0}) too big for this simple server",
                           content_len));
                 }
                 byte[] buf = new byte[BUF_SIZE];              
                 int to_read = content_len;
                 while (to_read > 0) {  
                     logger.DebugFormat("starting Read, to_read={0}",to_read);

                     int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                     logger.DebugFormat("read finished, numread={0}", numread);
                     if (numread == 0) {
                         if (to_read == 0) {
                             break;
                         } else {
                             throw new Exception("client disconnected during post");
                         }
                     }
                     to_read -= numread;
                     ms.Write(buf, 0, numread);
                 }
                 ms.Seek(0, SeekOrigin.Begin);
            }
            logger.DebugFormat("get post data end");
            srv.handlePOSTRequest(this, new StreamReader(ms));

        }

        public void writeSuccess(string content_type="text/html") {
            outputStream.WriteLine("HTTP/1.0 200 OK");            
            outputStream.WriteLine("Content-Type: " + content_type);
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void writeFailure() {
            outputStream.WriteLine("HTTP/1.0 404 File not found");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class HttpServer
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private extern static bool CloseHandle(IntPtr handle);

        private static IntPtr tokenHandle = new IntPtr(0);
        private static WindowsImpersonationContext impersonatedUser;

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        private static bool Impersonate(string domainName, string userName, string password)
        {
            try
            {
                const int LOGON32_PROVIDER_DEFAULT = 0;
                const int LOGON32_LOGON_INTERACTIVE = 2;
                tokenHandle = IntPtr.Zero;

                bool returnValue = LogonUser(userName, domainName, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref tokenHandle);

                if (!returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    logger.Error("LogonUser call failed with error code : " + ret);
                    throw new System.ComponentModel.Win32Exception(ret);
                }

                WindowsIdentity newId = new WindowsIdentity(tokenHandle);
                impersonatedUser = newId.Impersonate();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Exception occurred. " + ex.Message);
                return false;
            }
        }


        private static void UndoImpersonate()
        {
            impersonatedUser.Undo();
            // Free the tokens.
            if (tokenHandle != IntPtr.Zero)
                CloseHandle(tokenHandle);
        }

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        protected int port;
        TcpListener listener;
        bool is_active = true;
       
        public HttpServer(int port) {
            this.port = port;
        }

        public void listen() {
            //Impersonate("GRADUAL", "svc.directtrade", "S&rvdir20");

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (is_active) {                
                TcpClient s = listener.AcceptTcpClient();
                
                
                HttpProcessor processor = new HttpProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }

            //UndoImpersonate();
        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyHttpServer : HttpServer
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        private GeracaoBaseConfig _config = null;
        private List<Type> macroClasses = new List<Type>();

        public MyHttpServer(int port, GeracaoBaseConfig config)
            : base(port)
        {
            _config = config;

            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                macroClasses.AddRange(assembly.GetTypes());
            }
        }

        public override void handleGETRequest (HttpProcessor p)
		{

			if (p.http_url.Equals ("/Test.png")) {
				Stream fs = File.Open("../../Test.png",FileMode.Open);

				p.writeSuccess("image/png");
				fs.CopyTo (p.outputStream.BaseStream);
				p.outputStream.BaseStream.Flush ();
			}

            logger.DebugFormat("request: {0}", p.http_url);
            p.writeSuccess();
            p.outputStream.WriteLine("<html><body><h1>Gerador Relatorios</h1>");
            p.outputStream.WriteLine("<br>Current Time: " + DateTime.Now.ToString());
            //p.outputStream.WriteLine("url : {0}", p.http_url);

            p.outputStream.WriteLine("<p><br><form method=post action=/form>");
            foreach(BaseParam parametro in _config.Parametros)
            {
                if ( !String.IsNullOrEmpty(parametro.OnDemand) )
                {
                    p.outputStream.WriteLine("<br>&nbsp;<br><input type=submit name=\"" + parametro.FunctionName + "\" value=\"" + parametro.Subject + "\">");
                }
            }
            p.outputStream.WriteLine("</form></p>");
            p.outputStream.WriteLine("</body></html>");
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData) {
            logger.DebugFormat("POST request: {0}", p.http_url);
            string data = inputData.ReadToEnd();

            p.writeSuccess();
            p.outputStream.WriteLine("<html><body><h1>Gerador Relatorios</h1>");
            p.outputStream.WriteLine("<p>");

            char[] sep = { '=' };
            string[] dados = data.Split( sep, StringSplitOptions.RemoveEmptyEntries);

            foreach (string dado in dados)
            {
                string xxxx = WebUtility.HtmlDecode(dado);
                foreach(BaseParam parametro in _config.Parametros)
                {
                    if (parametro.FunctionName.Equals(xxxx))
                    {
                        InvocarMetodo(parametro.OnDemand, parametro.FunctionName);
                        p.outputStream.WriteLine("<br>relatorio gerado: <pre>{0}</pre>", dado);
                    }
                }
            }
            p.outputStream.WriteLine("<a href=/test>Voltar</a></p>");
            p.outputStream.WriteLine("</body></html>");
        }


        public void InvocarMetodo(string classmethod, params object[] parameters)
        {
            if (classmethod.IndexOf('.') <= 0)
            {
                logger.Error("Invalid argument classmethod: [" + classmethod + "]. First parameter must be <classname.methodname>");
                throw new ArgumentException("First parameter must be <classname.methodname>");
            }

            string[] strarr = classmethod.Split('.');

            string classname = string.Empty;
            for (int i = 0; i < strarr.Length - 1; i++)
            {
                classname = classname + strarr[i] + ".";
            }
            classname = classname.Substring(0, classname.Length - 1);
            string methodname = strarr[strarr.Length - 1];

            logger.Debug("Encontrou " + macroClasses.Count + " classes");

            foreach (Type tempClass in macroClasses)
            {
                try
                {
                    if (tempClass.Name.Equals(classname))
                    {
                        object curInstance = null;
                        MethodInfo getinstance = tempClass.GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static);

                        if (getinstance != null)
                        {
                            curInstance = getinstance.Invoke(null, null);
                        }
                        else
                        {
                            curInstance = Activator.CreateInstance(tempClass);
                        }

                        logger.Info("Calling Invoke(" + methodname + ")");
                        tempClass.GetMethod(methodname).Invoke(curInstance, parameters);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("InvocarMetodo:" + ex.Message, ex);
                }
            }
        }

    }

    /*public class TestMain {
        public static int Main(String[] args) {
            HttpServer httpServer;
            if (args.GetLength(0) > 0) {
                httpServer = new MyHttpServer(Convert.ToInt16(args[0]));
            } else {
                httpServer = new MyHttpServer(8080);
            }
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
            return 0;
        }

    }*/

}
