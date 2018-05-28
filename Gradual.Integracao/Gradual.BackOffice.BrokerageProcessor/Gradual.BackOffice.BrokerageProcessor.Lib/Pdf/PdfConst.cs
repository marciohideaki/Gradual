using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Pdf
{
    public class PdfConst
    {
        public static string BMF_CLIENT_ID_PATTERN = "Código do cliente";
        public static int BMF_CLIENT_ID_SIZE = 7;

        public static string BOVESPA_CLIENT_ID_PATTERN = "C.P.F./C.N.P.J./C.V.M./C.O.B. Cliente";
        public static int BOVESPA_CLIENT_ID_SIZE = 10;

        public static string POSICAO_BMF_CLIENT_ID_PATTERN = "Assessor: Cliente:";


    }
}
