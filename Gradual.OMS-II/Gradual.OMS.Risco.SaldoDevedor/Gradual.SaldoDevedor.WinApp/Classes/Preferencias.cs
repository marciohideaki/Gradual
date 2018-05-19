using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gradual.SaldoDevedor.WinApp.Classes
{
    public class Preferencias
    {
        private static string gCaminho = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Classes.AssemblyHelper.ProductName);

        public static void SalvarUsuario(System.String Usuario, System.String Senha, bool Remover = false)
        {
            string lCaminho = gCaminho;
            string lNomeArquivo = "";

            /*
            if (ContextoGlobal.Usuario.TipoAcesso == TipoAcesso.Assessor)
                lNomeArquivo = ContextoGlobal.Usuario.CodAssessor.ToString();
            else if (ContextoGlobal.Usuario.TipoAcesso == TipoAcesso.Cliente)
                lNomeArquivo = ContextoGlobal.Usuario.CodBovespa;
            else
                lNomeArquivo = ContextoGlobal.Usuario.NomeUsuario;
            */

            if (!Directory.Exists(gCaminho))
            {
                Directory.CreateDirectory(gCaminho);
            }

            lCaminho = System.IO.Path.Combine(lCaminho, string.Concat(lNomeArquivo, "dados", ".dat"));

            System.IO.FileStream wFile;
            wFile = new System.IO.FileStream(lCaminho, System.IO.FileMode.Create);

            try
            {
                byte[] byteData = null;

                #region Usuarios Autenticados

                byteData = Encoding.ASCII.GetBytes(String.Format("\tUSUARIO_PADRAO="));
                wFile.Write(byteData, 0, byteData.Length);
                System.Text.StringBuilder _usuario = new StringBuilder();
                if (!Remover)
                {
                    _usuario.AppendFormat("{0}:{1},", Usuario.ToString(), Senha.ToString());
                }

                _usuario.Append("\r\n");
                byteData = Encoding.ASCII.GetBytes(_usuario.ToString());
                wFile.Write(byteData, 0, byteData.Length);

                #endregion

                #region Usuarios Autenticados

                byteData = Encoding.ASCII.GetBytes(String.Format("\tUSUARIOS="));
                wFile.Write(byteData, 0, byteData.Length);
                System.Text.StringBuilder _usuarios = new StringBuilder();

                if (Aplicacao.Usuarios.ContainsKey(Usuario.ToString()))
                {
                    if (Remover)
                    {
                        Aplicacao.Usuarios.Remove(Usuario.ToString());
                    }
                    else
                    {
                        Aplicacao.Usuarios[Usuario.ToString()] = Senha.ToString();
                    }

                }
                else
                {
                    Aplicacao.Usuarios.Add(Usuario.ToString(), Senha.ToString());
                }

                foreach (var pair in Aplicacao.Usuarios)
                {
                    if (Remover)
                    {
                        if (pair.Key.Equals(Usuario))
                        {
                            continue;
                        }
                    }

                    _usuarios.AppendFormat("{0}:{1},", pair.Key.ToString(), pair.Value.ToString());
                }

                _usuarios.Append("\r\n");
                byteData = Encoding.ASCII.GetBytes(_usuarios.ToString());
                wFile.Write(byteData, 0, byteData.Length);

                #endregion
            }
            catch (Exception ex)
            {
                Aplicacao.ReportarErro("SalvarUsuario()", ex);
                throw ex;
            }
            finally
            {
                wFile.Close();
            }
        }

        public static void CarregarUsuarios()
        {

            string lCaminho = gCaminho;

            string lNomeArquivo = "";

            lCaminho = System.IO.Path.Combine(lCaminho, string.Concat(lNomeArquivo, "dados", ".dat"));

            System.IO.StreamReader objReader = null;
            string sLine = String.Empty;
            System.Collections.ArrayList arrText = null;
            List<System.Windows.Forms.DataGridViewColumn> Colunas = new List<System.Windows.Forms.DataGridViewColumn>();

            try
            {
                #region Leitura dos Usuarios

                try
                {
                    if (!System.IO.File.Exists(lCaminho))
                    {
                        return;
                    }

                    objReader = new System.IO.StreamReader(lCaminho);
                    arrText = new System.Collections.ArrayList();

                    while (sLine != null)
                    {
                        sLine = objReader.ReadLine();
                        if (sLine != null)
                            arrText.Add(sLine);
                    }

                    objReader.Close();
                }
                catch (Exception ex)
                {
                    Aplicacao.ReportarErro("CarregarUsuarios.LEITURA", ex);
                }

                #endregion

                #region Carregamento das configurações

                foreach (string sOutput in arrText)
                {
                    string _linha = sOutput;
                    _linha = _linha.Replace("\r", String.Empty);
                    _linha = _linha.Replace("\n", String.Empty);
                    _linha = _linha.Replace("\t", String.Empty);


                    if (_linha.Contains("USUARIO_PADRAO"))
                    {
                        _linha = _linha.Replace("USUARIO_PADRAO=", String.Empty);
                        _linha = _linha.Replace(";", String.Empty);
                        string[] _usuarios = _linha.Split(',');

                        for (int i = 0; i < _usuarios.Length; i++)
                        {
                            string[] _usuario = _usuarios[i].Split(':');
                            if (_usuario[0].Length > 0)
                            {
                                //Aplicacao.Usuarios.Add(_usuario[0].ToString(), _usuario[1].ToString());
                                Aplicacao.UsuarioPadrao = new UsuarioPadrao(_usuario[0].ToString(), _usuario[1].ToString());
                            }
                        }
                    }

                    if (_linha.Contains("USUARIOS"))
                    {
                        _linha = _linha.Replace("USUARIOS=", String.Empty);
                        _linha = _linha.Replace(";", String.Empty);
                        string[] _usuarios = _linha.Split(',');


                        for (int i = 0; i < _usuarios.Length; i++)
                        {
                            string[] _usuario = _usuarios[i].Split(':');
                            if (_usuario[0].Length > 0)
                            {
                                Aplicacao.Usuarios.Add(_usuario[0].ToString(), _usuario[1].ToString());
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Aplicacao.ReportarErro("CarregarPreferencias()", ex);
                throw ex;
            }
            finally
            {

            }
        }
    }
}
