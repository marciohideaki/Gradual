using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Host.Windows.ConsoleServicos
{
    class Program
    {
        static void Main(string[] args)
        {
            // Carrega servicos do config
            Console.WriteLine("Carregando serviços...");
            ServicoHostColecao.Default.CarregarConfig("Default");
            Console.WriteLine("Iniciando serviços...");
            ServicoHostColecao.Default.IniciarServicos();

            // Informa e aguarda
            Console.WriteLine("OK... digite qualquer tecla para finalizar");
            Console.ReadLine();

            // Finaliza os serviços 
            Console.WriteLine("Finalizando serviços...");
            ServicoHostColecao.Default.PararServicos();

            // Informa e aguarda
            Console.WriteLine("Serviços finalizados... digite qualquer tecla para sair");
            Console.ReadLine();
        }
    }
}
