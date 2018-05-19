using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Gradual.SaldoDevedor.WinApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmSaldoDevedor frmMain = new frmSaldoDevedor();
            RunApp();
        }

        private static void RunApp()
        {

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            frmSaldoDevedor frmMain = new frmSaldoDevedor();
            frmLogin.FormularioDeLogin.Owner = frmMain;
            frmMain.Hide();

            if (frmLogin.FormularioDeLogin.ShowDialog().Equals(DialogResult.OK))
            {
                Application.Run(frmMain);
            }
            else
            {
                Application.Exit();
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;

                //Aplicacao.ReportarErro("MESSAGE[{0}] \n\n SOURCE[{1}] \n\n STACK[{2}]", ex.Message, ex.Source, ex.StackTrace);
                Classes.Aplicacao.ReportarErro("Program.CurrentDomain_UnhandledException()", ex);
            }
            finally
            {
            }
        }

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.Exception;

                //Aplicacao.ReportarErro("MESSAGE[{0}] \n\n SOURCE[{1}] \n\n STACK[{2}]", ex.Message, ex.Source, ex.StackTrace);
                Classes.Aplicacao.ReportarErro("Program.Application_ThreadException()", ex);
            }
            finally
            {
            }
        }
    }
}
