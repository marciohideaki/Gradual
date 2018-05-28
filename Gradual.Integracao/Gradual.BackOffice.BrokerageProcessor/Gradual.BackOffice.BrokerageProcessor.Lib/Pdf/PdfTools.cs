using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Pdf
{
    public class PdfTools
    {
        public static int ExtractBMFClientID(string content)
        {
            try
            {
                string[] lines = content.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                int idx = -1;
                for (int j = 0; j < lines.Length; j++)
                {
                    if (lines[j].IndexOf(PdfConst.BMF_CLIENT_ID_PATTERN) >= 0)
                    {
                        idx = j+1;
                        break;
                    }
                }
                int ret;
                if (lines[idx].Length >= PdfConst.BMF_CLIENT_ID_SIZE)
                {
                    // Regra BMF
                    // AV DA CAVALHADA, 5205 - C 92 Código do cliente
                    // 0005158 CAVALHADA    PORTO ALEGRE - RS 91751-830
                    if (!int.TryParse(lines[idx].Substring(0, PdfConst.BMF_CLIENT_ID_SIZE), out ret))
                        ret = - 1;
                }
                else
                    ret = -1;
                
                return ret;
            }
            catch
            {
                return -1;
            }
        }



        public static int ExtractBovespaClientID(string content)
        {
            try
            {
                string[] lines = content.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                int idx = -1;
                for (int j = 0; j < lines.Length; j++)
                {
                    if (lines[j].IndexOf(PdfConst.BOVESPA_CLIENT_ID_PATTERN) >= 0)
                    {
                        idx = j + 1;
                        break;
                    }
                }
                int ret;
                if (lines[idx].Length >= PdfConst.BOVESPA_CLIENT_ID_SIZE)
                {
                    // Regra Bovespa
                    // C.P.F./C.N.P.J./C.V.M./C.O.B. Cliente
                    // 910.983.437-00 MARCIA ANDREIA SOARES PEREIRA COELHO 9 0032720-
                    string aux = lines[idx].Substring(lines[idx].Length - PdfConst.BOVESPA_CLIENT_ID_SIZE);
                    aux = aux.Replace("-", "");
                    aux = aux.Replace(" ", "");
                    string aux2 = aux.Substring(1);
                    if (!int.TryParse(aux2, out ret))
                        ret = -1;
                }
                else
                    ret = -1;

                return ret;
            }
            catch
            {
                return -1;
            }
        }


        

        public static int ExtractPosicaoBmfClientID(string content)
        {
            try
            {
                string[] lines = content.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                int idx = -1;
                for (int j = 0; j < lines.Length; j++)
                {
                    if (lines[j].IndexOf(PdfConst.POSICAO_BMF_CLIENT_ID_PATTERN) >= 0)
                    {
                        idx = j;
                        break;
                    }
                }
                int ret;
                
                if (!string.IsNullOrEmpty(lines[idx]))
                //if (lines[idx].Length >= PdfConst.BOVESPA_CLIENT_ID_SIZE)
                {
                    // Regra Posicao BMF 
                    // Assessor: Cliente: 11 MATRIZ-SP-HNETTO AAI-HUMIHIR RAFAEL JOSE HASSON 1055
                    string aux = lines[idx].Substring(lines[idx].LastIndexOf(" "));
                    aux = aux.Trim();
                    if (!int.TryParse(aux, out ret))
                        ret = -1;
                }
                else
                    ret = -1;

                return ret;
            }
            catch
            {
                return -1;
            }
        }
    }
}
