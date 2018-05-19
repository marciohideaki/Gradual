using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Host.Windows.Interface
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Inicializa
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Carrega servicos do config
            ServicoHostColecao.Default.CarregarConfig("Default");

            // Pega o serviço de interface
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();

            // Inicia o launcher
            servicoInterface.IniciarLauncher();

            // Lança o sistema
            Application.Run((Form)servicoInterface.ReceberJanelaLauncher());
        }
    }
}
