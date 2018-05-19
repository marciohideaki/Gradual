using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace StopStartForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
