using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AS.Messages
{
   public static class GenericMessage
    {
        public static string GetPosition(int Size, string value, char Caracter){
            return value.PadLeft(value.Length + (Size - value.Length), Caracter);
        }
    }
}
