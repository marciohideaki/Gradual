using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Spider.GlobalOrderTracking
{
    public class Aplicacao
    {

        public delegate void OnAlterarCoresHandler(object sender, EventArgs e);
        public static event OnAlterarCoresHandler OnAlterarCoresEvent;

        private static Dictionary<string, System.Drawing.Color> _coresStatus = new Dictionary<string,System.Drawing.Color>();

        public static Dictionary<string, System.Drawing.Color> CoresStatus
        {
            get
            {
                if (_coresStatus == null)
                {
                }

                return _coresStatus;
            }

            set
            {
                _coresStatus = value;
            }
        }

        public static void AlterarCores(object sender, EventArgs e)
        {
            if (OnAlterarCoresEvent != null)
            {
                OnAlterarCoresEvent(sender, null);
            }

            Preferencias.SalvarPreferencias();
        }

        private static System.Windows.Forms.AutoCompleteStringCollection _ListaUsuarios = null;

        public static System.Windows.Forms.AutoCompleteStringCollection ListaUsuarios
        {
            get
            {
                if (_ListaUsuarios == null)
                {
                    _ListaUsuarios = new System.Windows.Forms.AutoCompleteStringCollection();

                    lock (_ListaUsuarios)
                    {
                        foreach (var pair in Aplicacao.Usuarios)
                        {
                            _ListaUsuarios.Add(pair.Key.ToString());
                        }
                    }
                }

                return _ListaUsuarios;
            }
        }

        private static Dictionary<string, string> _usuarios = null;

        public static Dictionary<string, string> Usuarios
        {
            get
            {
                if (_usuarios == null)
                {
                    _usuarios = new Dictionary<string, string>();
                    Preferencias.CarregarUsuarios();
                }

                return _usuarios;
            }
        }

        private static Gradual.Spider.GlobalOrderTracking.Classes.UsuarioPadrao _usuarioPadrao = null;

        public static Gradual.Spider.GlobalOrderTracking.Classes.UsuarioPadrao UsuarioPadrao
        {
            get
            {
                if (_usuarioPadrao == null)
                {
                    _usuarioPadrao = new Gradual.Spider.GlobalOrderTracking.Classes.UsuarioPadrao();
                }

                return _usuarioPadrao;
            }

            set
            {
                _usuarioPadrao = value;
            }
        }

        public static String GerarClOrdId(String pAccount, String pClOrdId)
        {
            return String.Format("{1}", pAccount, pClOrdId);
        }

    }
}
