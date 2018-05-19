using System;
using System.IO;
using System.Configuration;
using System.Web;

namespace STIntegracaoPortalHB
{
    public class LogErro
    {
        /// <summary>
        /// Método responsável por armazenar msg's no arquivo de log;
        /// </summary>
        /// <param name="msg"></param>
        public static void GravaLog(string msg)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.CurrentExecutionFilePath);

                string caminhoArquivoLog = string.Format(@"{0}\..\Integracao-trace-{1}.log", path, System.DateTime.Today.ToString("yyyyMMdd"));

                FileStream outputFile = new FileStream(caminhoArquivoLog, FileMode.Append);
                StreamWriter sw = new StreamWriter(outputFile);
                sw.WriteLine(System.DateTime.Now.ToString() + " - " + msg);
                sw.Close();
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }
    }
}
