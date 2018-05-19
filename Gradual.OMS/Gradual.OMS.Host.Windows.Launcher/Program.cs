using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Host.Windows.Launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Carrega servicos do config
            ServicoHostColecao.Default.CarregarConfig("Default");

            // Pega o serviço de ordens
            IServicoInterface servicoInterface = Ativador.Get<IServicoInterface>();

            // Inicia o launcher
            servicoInterface.IniciarLauncher();

            // Lança a aplicação
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
