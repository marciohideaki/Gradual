using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;

namespace ConsoleMdsProfileApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            // Carrega servicos do config
            ServicoHostColecao.Default.CarregarConfig("Default");
            ServicoHostColecao.Default.IniciarServicos();

            System.Console.ReadLine();
        }
    }
}
