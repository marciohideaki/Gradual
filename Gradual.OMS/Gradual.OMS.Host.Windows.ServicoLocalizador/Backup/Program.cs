using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Host.Windows.ServicoLocalizador
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("*   S E R V I C O   L O C A L I Z A D O R   *");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("");

            Console.WriteLine("Carregando...");
            ServicoHostColecao.Default.CarregarConfig("Default");
            Console.WriteLine("OK");
            Console.WriteLine("Pressione qualquer tecla para finalizar");
            Console.ReadLine();
        }
    }
}
