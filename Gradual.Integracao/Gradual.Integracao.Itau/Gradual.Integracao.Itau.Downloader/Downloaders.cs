using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.Configuration;
using log4net;

namespace Gradual.Integracao.Itau.Downloader
{
    public class Downloaders
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _dirArquivosRecebidos;
        private string _ebusiness;
        private string _senha;
        private string _codgestor;

        public Downloaders()
        {
            if (ConfigurationManager.AppSettings["DiretorioArquivosRecebidos"] != null)
            {
                _dirArquivosRecebidos = ConfigurationManager.AppSettings["DiretorioArquivosRecebidos"].ToString();
            }

            if (ConfigurationManager.AppSettings["EBusiness"] != null)
            {
                _ebusiness = ConfigurationManager.AppSettings["EBusiness"].ToString();
            }

            if (ConfigurationManager.AppSettings["Senha"] != null)
            {
                _senha = ConfigurationManager.AppSettings["Senha"].ToString();
            }

            if (ConfigurationManager.AppSettings["CodigoGestor"] != null)
            {
                _codgestor = ConfigurationManager.AppSettings["CodigoGestor"].ToString();
            }
        }

        public bool _download_SaldosCotistasD0()
        {
            try
            {
                br.com.itaucustodia.www.DownloadArquivos.DownloadArquivoServiceService cliente = new br.com.itaucustodia.www.DownloadArquivos.DownloadArquivoServiceService();

                //string resp = cliente.saldosCotaAberturaD0XML("gradual.op53", "1s22s22p", "990686");
                string resp = cliente.saldosCotaAberturaD0XML(_ebusiness, _senha, _codgestor);

                byte[] zipbytes = Convert.FromBase64String(resp);

                ZipFile zip = ZipFile.Read(zipbytes);
                zip.ExtractAll(_dirArquivosRecebidos, ExtractExistingFileAction.OverwriteSilently);

                zip.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("_download_SaldosCotistasD0(): " + ex.Message, ex);
            }

            return false;
        }

    }
}
