using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Cotacao.Mensagens.Enviadas
{
    public class Header
    {
        public string GetHeader(string tpMessage, string tpBolsa){

            tpMessage = GetPosition(2, tpMessage, ' ');
            tpBolsa = GetPosition(2, tpBolsa, ' ');
            string DataHora = DateTime.Now.ToString(string.Format("{0}{1}", "yyyyMMddHHmmssmmm", "0"));
            string CodigoInstrumento = GetPosition(20, "", ' ');
            return tpMessage + tpBolsa + DataHora + CodigoInstrumento;
        }

        public string GetPosition(int Size, string value, char Caracter){
            return value.PadLeft(value.Length + (Size - value.Length), Caracter);
        }

    }
}
