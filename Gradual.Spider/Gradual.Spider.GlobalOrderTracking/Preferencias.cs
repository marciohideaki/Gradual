using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Spider.GlobalOrderTracking
{
    public class Preferencias
    {
        private static string gCaminho = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AssemblyHelper.CompanyName, AssemblyHelper.ProductName);

        public static void SalvarPreferencias()
        {
            string lCaminho = gCaminho;
            string lNomeArquivo = "";

            //if (ContextoGlobal.Usuario.TipoAcesso == TipoAcesso.Assessor)
            //{
            //    lNomeArquivo = ContextoGlobal.Usuario.CodAssessor.ToString();
            //}
            //else if (ContextoGlobal.Usuario.TipoAcesso == TipoAcesso.Cliente)
            //{
            //    lNomeArquivo = string.IsNullOrEmpty(ContextoGlobal.Usuario.Cliente.CodBovespa) ? ContextoGlobal.Usuario.Cliente.CodBMF : ContextoGlobal.Usuario.Cliente.CodBovespa;
            //}
            //else if (ContextoGlobal.Usuario.TipoAcesso == TipoAcesso.PontaMesa)
            //{
            //    lNomeArquivo = ContextoGlobal.CodigoUsuario;
            //}
            //else
            //{
            //    lNomeArquivo = ContextoGlobal.Usuario.NomeUsuario;
            //}

            lCaminho = System.IO.Path.Combine(lCaminho, string.Concat(lNomeArquivo, "_PREFERENCIAS_", ".gti"));

            System.IO.FileStream wFile;
            wFile = new System.IO.FileStream(lCaminho, System.IO.FileMode.Create);

            try
            {
                byte[] byteData = null;
                byteData = Encoding.ASCII.GetBytes(String.Format("\tCORES_STATUS="));
                wFile.Write(byteData, 0, byteData.Length);
                System.Text.StringBuilder lCoresStatus= new StringBuilder();
                foreach (var lPair in Aplicacao.CoresStatus)
                {
                    lCoresStatus.AppendFormat("{0}:{1},{2},{3},{4};", lPair.Key.ToString(), lPair.Value.A, lPair.Value.R, lPair.Value.G, lPair.Value.B);
                }
                lCoresStatus.Append("\r\n");
                byteData = Encoding.ASCII.GetBytes(lCoresStatus.ToString());
                wFile.Write(byteData, 0, byteData.Length);

            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
            finally
            {
                wFile.Close();
            }
        }

        public static void CaregarPreferencias()
        {
            string lCaminho = gCaminho;
            string lNomeArquivo = "";
            //if (Arquivo.Equals(""))
            //{
            //    if (ContextoGlobal.Usuario.TipoAcesso == TipoAcesso.Assessor)
            //    {
            //        lNomeArquivo = ContextoGlobal.Usuario.CodAssessor.ToString();
            //    }
            //    else if (ContextoGlobal.Usuario.TipoAcesso == TipoAcesso.Cliente)
            //    {
            //        lNomeArquivo = string.IsNullOrEmpty(ContextoGlobal.Usuario.Cliente.CodBovespa) ? ContextoGlobal.Usuario.Cliente.CodBMF : ContextoGlobal.Usuario.Cliente.CodBovespa;
            //    }
            //    else if (ContextoGlobal.Usuario.TipoAcesso == TipoAcesso.PontaMesa)
            //    {
            //        lNomeArquivo = ContextoGlobal.CodigoUsuario;
            //    }
            //    else
            //        lNomeArquivo = ContextoGlobal.Usuario.NomeUsuario;

                lCaminho = System.IO.Path.Combine(lCaminho, string.Concat(lNomeArquivo, "_PREFERENCIAS_", ".gti"));
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}: {2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Path:", lCaminho), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            //}
            //else
            //{
            //  lNomeArquivo = Arquivo;
            //  lCaminho = System.IO.Path.Combine(lCaminho, lNomeArquivo);
            //}

                if (System.IO.File.Exists(lCaminho))
                {

                    System.IO.StreamReader objReader     = null;
                    string sLine                         = String.Empty;
                    System.Collections.ArrayList arrText = null;

                    try
                    {
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
                        Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                    }

                    try
                    {
                        foreach (string sOutput in arrText)
                        {
                            string _linha = sOutput;
                            _linha = _linha.Replace("\r", String.Empty);
                            _linha = _linha.Replace("\n", String.Empty);
                            _linha = _linha.Replace("\t", String.Empty);

                            if (_linha.Contains("CORES_STATUS"))
                            {
                                _linha = _linha.Replace("CORES_STATUS=", String.Empty);

                                string[] _configuracoes = _linha.Split(';');

                                for (int i = 0; i < _configuracoes.Length; i++)
                                {

                                    if (_configuracoes[i].Length > 0)
                                    {
                                        string[] _configuracao = _configuracoes[i].Split(':');

                                        string[] _propriedade = _configuracao[1].Split(',');
                                        if (Aplicacao.CoresStatus.ContainsKey(_configuracao[0].ToString()))
                                        {
                                            Aplicacao.CoresStatus[_configuracao[0].ToString()] = System.Drawing.Color.FromArgb(_propriedade[0].ToInt32(), _propriedade[1].ToInt32(), _propriedade[2].ToInt32(), _propriedade[3].ToInt32());
                                        }
                                        else
                                        {
                                            Aplicacao.CoresStatus.Add(_configuracao[0].ToString(), System.Drawing.Color.FromArgb(_propriedade[0].ToInt32(), _propriedade[1].ToInt32(), _propriedade[2].ToInt32(), _propriedade[3].ToInt32()));
                                        }
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                    }
                }
        }

        public static void CarregarUsuarios()
        {

            string lCaminho = gCaminho;

            string lNomeArquivo = "";

            lCaminho = System.IO.Path.Combine(lCaminho, string.Concat(lNomeArquivo, "dados", ".dat"));

            Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}: {2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Path:", lCaminho), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

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
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
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
                                Aplicacao.UsuarioPadrao = new Gradual.Spider.GlobalOrderTracking.Classes.UsuarioPadrao(_usuario[0].ToString(), _usuario[1].ToString());
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
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                throw ex;
            }
            finally
            {

            }
        }

        public static void SalvarUsuario(System.String Usuario, System.String Senha, bool Remover = false)
        {
            try
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

                lCaminho = System.IO.Path.Combine(lCaminho, string.Concat(lNomeArquivo, "dados", ".dat"));

                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}: {2}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Path:", lCaminho), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

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
                    //Aplicacao.ReportarErro("SalvarUsuario()", ex);
                    Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                    throw ex;
                }
                finally
                {
                    wFile.Close();
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Interface", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

    }
}
