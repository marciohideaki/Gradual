using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.SaldoDevedor.WinApp.Classes
{
    class Aplicacao
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        private static UsuarioPadrao _usuarioPadrao = null;

        public static UsuarioPadrao UsuarioPadrao
        {
            get
            {
                if (_usuarioPadrao == null)
                {
                    _usuarioPadrao = new UsuarioPadrao();
                }

                return _usuarioPadrao;
            }

            set
            {
                _usuarioPadrao = value;
            }
        }

        public static void ReportarErro(string pProcesso, string pMensagem, params object[] pParams)
        {
            try
            {
                ReportarErro(pProcesso, new Exception(string.Format(pMensagem, pParams)));
            }
            catch (Exception ex)
            {
                logger.InfoFormat("Deu exception [{0}]:\r\n\r\n{1}\r\n\r\n{2}", "AplicacaoGeral > ReportarErro", ex.Message, ex.StackTrace);
            }
        }

        public static void ReportarErro(string pProcesso, Exception pException)
        {
            try
            {
                logger.ErrorFormat(String.Format("Processo:{0} Exception:{1}", pProcesso, pException));

                /*
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    RelatorioDeErro lRelatorio = new RelatorioDeErro();

                    Controlador lControlador = new Controlador();

                    lRelatorio.OS_Nome = Environment.OSVersion.Platform.ToString();
                    lRelatorio.OS_Versao = Environment.OSVersion.VersionString;

                    lRelatorio.Programa_Id = CONST_ID_DESKTOP_TRADER;
                    lRelatorio.Programa_Versao = Application.ProductVersion;

                    lRelatorio.Usuario_Id = string.Format("Código Sessão: [{0}], Cod Bovespa: [{1}], Cod BMF: [{2}], Cod Assessor: [{3}], Email: [{4}], Tipo Assesso: [{5}], Sessão MDS: [{6}]"
                                                          , ContextoGlobal.CodigoSessao
                                                          , ContextoGlobal.Usuario.CodBovespa
                                                          , ContextoGlobal.Usuario.CodBMF
                                                          , ContextoGlobal.Usuario.CodAssessor
                                                          , ContextoGlobal.Usuario.Email
                                                          , ContextoGlobal.Usuario.TipoAcesso
                                                          , MdsBayeuxClient.MdsHttpClient.Instance.Session);

                    lRelatorio.Erro_Descricao = string.Format("Erro durante a execução de [{0}]: [{1}]", pProcesso, pException.Message);
                    lRelatorio.Erro_Pilha = pException.StackTrace;

                    lControlador.EnviarRelatorioDeErro(lRelatorio);
                }
                else
                {
                    logger.InfoFormat("Deu exception [{0}]:\r\n\r\n{1}\r\n\r\n{2}", pProcesso, pException.Message, pException.StackTrace);
                    MessageBox.Show(string.Format("Deu exception [{0}]:\r\n\r\n{1}\r\n\r\n{2}", pProcesso, pException.Message, pException.StackTrace));

                }
                */
            }
            catch (Exception ex)
            {
                logger.InfoFormat("Deu exception [{0}]:\r\n\r\n{1}\r\n\r\n{2}", "AplicacaoGeral > ReportarErro", ex.Message, ex.StackTrace);
                logger.InfoFormat("Deu exception [{0}]:\r\n\r\n{1}\r\n\r\n{2}", pProcesso, pException.Message, pException.StackTrace);
            }
        }

        public static void GravarLog(string pClasse, string pProcesso, System.String pMensagem)
        {
            GravarLog(String.Format("{0} -> {1}", pClasse, pProcesso), pMensagem);
        }

        public static void GravarLog(string pProcesso, System.String pMensagem)
        {
            try
            {
                logger.InfoFormat(String.Format("Processo:{0} Mensagem:{1}", pProcesso, pMensagem));

                /*
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    RelatorioDeErro lRelatorio = new RelatorioDeErro();

                    Controlador lControlador = new Controlador();

                    lRelatorio.OS_Nome = Environment.OSVersion.Platform.ToString();
                    lRelatorio.OS_Versao = Environment.OSVersion.VersionString;

                    lRelatorio.Programa_Id = CONST_ID_DESKTOP_TRADER;
                    lRelatorio.Programa_Versao = Application.ProductVersion;

                    lRelatorio.Usuario_Id = string.Format("Código Sessão: [{0}], Cod Bovespa: [{1}], Cod BMF: [{2}], Cod Assessor: [{3}], Email: [{4}], Tipo Assesso: [{5}], Sessão MDS: [{6}]"
                                                          , ContextoGlobal.CodigoSessao
                                                          , ContextoGlobal.Usuario.CodBovespa
                                                          , ContextoGlobal.Usuario.CodBMF
                                                          , ContextoGlobal.Usuario.CodAssessor
                                                          , ContextoGlobal.Usuario.Email
                                                          , ContextoGlobal.Usuario.TipoAcesso
                                                          , MdsBayeuxClient.MdsHttpClient.Instance.Session);

                    lRelatorio.Erro_Descricao = string.Format("Erro durante a execução de [{0}]: [{1}]", pProcesso, pException.Message);
                    lRelatorio.Erro_Pilha = pException.StackTrace;

                    lControlador.EnviarRelatorioDeErro(lRelatorio);
                }
                else
                {
                    logger.InfoFormat("Deu exception [{0}]:\r\n\r\n{1}\r\n\r\n{2}", pProcesso, pException.Message, pException.StackTrace);
                    MessageBox.Show(string.Format("Deu exception [{0}]:\r\n\r\n{1}\r\n\r\n{2}", pProcesso, pException.Message, pException.StackTrace));

                }
                */
            }
            catch (Exception ex)
            {
                logger.InfoFormat("Deu exception [{0}]:\r\n\r\n{1}\r\n\r\n{2}", "AplicacaoGeral > ReportarErro", ex.Message, ex.StackTrace);
            }
        }
    }
}
