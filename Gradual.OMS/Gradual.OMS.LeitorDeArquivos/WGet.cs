using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Gradual.OMS.LeitorDeArquivos
{
    public static class WGet
    {
        #region Métodos Públicos

        public static string Get(string pURL)
        {

            WebClient lClient = new WebClient();

            byte[] lResponse = lClient.DownloadData(pURL);

            string lRetorno = Encoding.ASCII.GetString(lResponse);

            return lRetorno;

        }

        #endregion
    }
}
