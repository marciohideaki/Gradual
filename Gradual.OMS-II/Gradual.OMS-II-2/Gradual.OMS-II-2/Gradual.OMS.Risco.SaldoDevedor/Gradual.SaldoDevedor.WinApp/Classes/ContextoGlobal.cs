using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.SaldoDevedor.WinApp.Classes
{
    public delegate void LoginEfetuadoEventHandler();

    public static class ContextoGlobal
    {
        #region Globais

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Usuario _usuario;

        private static string _codigoSessao;

        #endregion

        #region Eventos

        public static event LoginEfetuadoEventHandler DoLogin;

        private static void OnDoLogin()
        {
            _usuario = new Usuario(CodigoSessao);

            if (DoLogin != null)
            {
                DoLogin();
            }
        }

        #endregion

        #region Propriedades

        public static Usuario Usuario
        {
            get
            {
                return _usuario;
            }
        }

        public static string CodigoSessao
        {
            get
            {
                return _codigoSessao;
            }
            set
            {
                _codigoSessao = value;
                OnDoLogin();
            }
        }
        public static string CodigoUsuario { get; set; }
        #endregion
    }
}
