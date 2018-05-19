using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Suitability
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Gradual.Utils.Logger.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Application.Run(new Relatorio());
        }
    }
}
