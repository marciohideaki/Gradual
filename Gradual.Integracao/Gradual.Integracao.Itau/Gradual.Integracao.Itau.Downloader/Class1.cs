using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Integracao.Itau.Downloader
{
    public class Class1
    {

        public void teste ()
        {
            PassivoWebServices.DownloadArquivoServiceClient cliente = new PassivoWebServices.DownloadArquivoServiceClient();

            cliente.aplicacoesEmAbertoCotasAberturaXML();


        }
    }
}
