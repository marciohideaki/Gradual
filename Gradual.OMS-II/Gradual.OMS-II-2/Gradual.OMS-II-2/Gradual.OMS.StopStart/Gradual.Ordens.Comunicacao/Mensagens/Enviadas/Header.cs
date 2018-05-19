using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Cabecalho
{
    public class Header
    {
        public string GetHeader(string tpMessage, string tpBolsa){

            tpMessage = GetPosition(2, tpMessage, ' ');
            tpBolsa = GetPosition(2, tpBolsa, ' ');
            string DataHora = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string CodigoInstrumento = GetPosition(20, "", ' ');
            return tpMessage + tpBolsa + DataHora + CodigoInstrumento;
        }

        public string GetHeader(string tpMessage, string tpBolsa, string CodigoInstrumento)
        {

            tpMessage = GetPosition(2, tpMessage, ' ');
            tpBolsa = GetPosition(2, tpBolsa, ' ');
            string DataHora = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            return tpMessage + tpBolsa + DataHora + CodigoInstrumento.PadRight(20);
        }

        public string GetPosition(int Size, string value, char Caracter){
            return value.PadLeft(value.Length + (Size - value.Length), Caracter);
        }

    }
}
