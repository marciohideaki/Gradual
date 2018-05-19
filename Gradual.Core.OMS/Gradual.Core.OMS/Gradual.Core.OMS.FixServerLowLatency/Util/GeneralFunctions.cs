using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Core.OMS.FixServerLowLatency.Util
{
    public class GeneralFunctions
    {

        public static int CalcularCodigoCliente(int CodigoCorretora, int CodigoCliente)
        {

            int valor = 0;
            valor = (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(1 - 1, 1)) * 5) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(2 - 1, 1)) * 4) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(1 - 1, 1)) * 3) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(2 - 1, 1)) * 2) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(3 - 1, 1)) * 9) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(4 - 1, 1)) * 8) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(5 - 1, 1)) * 7) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(3 - 1, 1)) * 6) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(4 - 1, 1)) * 5) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(5 - 1, 1)) * 4) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(6 - 1, 1)) * 3) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(7 - 1, 1)) * 2);

            valor = valor % 11;

            if (valor == 0 || valor == 1)
            {
                valor = 0;
            }
            else
            {
                valor = 11 - valor;
            }

            return int.Parse(string.Format("{0}{1}", CodigoCliente, valor));

        }
    }
}
