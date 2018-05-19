using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.SaldoDevedor.WinApp.Classes
{
    public class UsuarioPadrao
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }

        public UsuarioPadrao()
        {
            Usuario = String.Empty;
            Senha = String.Empty;
        }

        public UsuarioPadrao(string pUsuario, string pSenha)
        {
            Usuario = pUsuario;
            Senha = pSenha;
        }
    }
}
