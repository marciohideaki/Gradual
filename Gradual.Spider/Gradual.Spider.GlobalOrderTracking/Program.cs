using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Gradual.Spider.GlobalOrderTracking
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

            log4net.Config.XmlConfigurator.Configure();

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            Preferencias.CaregarPreferencias();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RunApp();

            Environment.Exit(0);

        }

        private static void RunApp()
        {

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            Gradual.Spider.GlobalOrderTracking.Formularios.fOrder frmMain = new Gradual.Spider.GlobalOrderTracking.Formularios.fOrder();
            Gradual.Spider.GlobalOrderTracking.Formularios.frmLogin.FormularioDeLogin.Owner = frmMain;
            frmMain.Hide();

            if (Gradual.Spider.GlobalOrderTracking.Formularios.frmLogin.FormularioDeLogin.ShowDialog().Equals(DialogResult.OK))
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
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
            finally
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}:{1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "A aplicação será encerrada devido uma exceção desconhecida e não tratada"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            }
        }

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.Exception;
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
            finally
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}:{1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "A aplicação será encerrada devido uma exceção desconhecida e não tratada"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            }
        }
    }
}
