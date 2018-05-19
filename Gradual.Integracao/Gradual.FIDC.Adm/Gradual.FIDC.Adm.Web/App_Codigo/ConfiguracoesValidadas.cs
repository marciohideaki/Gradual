using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using log4net;

namespace Gradual.FIDC.Adm.Web
{
    public static class ConfiguracoesValidadas
    {
        #region Globais

        private static bool gIniciado = false;

        #endregion

        #region Propriedades

        private static string _SkinPadrao = "Default";

        private static string _VersaoDoSite;
        
        private static string _RaizDoSite;

        private static string _HostDoSite;

        private static string _TipoDeObjetoAtivador;

        private static string _pathTermoAdesao;

        private static string _pathFIDC;

        private static string _pathCarteiraDiaria;

        private static string _pathExtratoCotista;

        private static string _pathMEC;

        public static string SkinPadrao 
        {
            get
            {
                Iniciar();

                return _SkinPadrao;
            }
        }

        public static string VersaoDoSite 
        {
            get
            {
                Iniciar();

                return _VersaoDoSite;
            }
        }
        
        public static string RaizDoSite 
        {
            get
            {
                Iniciar();

                return _RaizDoSite;
            }
        }

        public static string HostDoSite
        {
            get
            {
                Iniciar();

                return _HostDoSite;
            }
        }

        public static string TipoDeObjetoAtivador
        {
            get
            {
                Iniciar();

                return _TipoDeObjetoAtivador;
            }
        }

        public static string PathTermoAdesao
        {
            get
            {
                Iniciar();

                return _pathTermoAdesao;
            }
        }

        public static string PathFIDC
        {
            get
            {
                Iniciar();

                return _pathFIDC;
            }
        }

        public static string PathCarteiraDiaria
        {
            get
            {
                Iniciar();

                return _pathCarteiraDiaria;
            }
        }

        public static string PathExtratoCotista
        {
            get
            {
                Iniciar();

                return _pathExtratoCotista;
            }
        }

        public static string PathMEC
        {
            get
            {
                Iniciar();

                return _pathMEC;
            }
        }
        #endregion

        #region Métodos

        private static void Iniciar()
        {
            try
            {
                if (!gIniciado)
                {
                    gIniciado = true;

                    string lValor;

                    //
                    // Skin Padrão: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["SkinPadrao"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "Default";

                    _SkinPadrao = lValor;

                    //
                    // Versão do Site: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["VersaoDoSite"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "00-00-00-0000";

                    _VersaoDoSite = lValor;
                    

                    //
                    // Rais do Site: -------------------------------------------------------------------
                    //

                    _RaizDoSite = ConfigurationManager.AppSettings["RaizDoSite"];

                    if (!string.IsNullOrEmpty(_RaizDoSite) && !_RaizDoSite.StartsWith("/") && !_RaizDoSite.Contains("localhost") && !_RaizDoSite.Contains("http://") && !_RaizDoSite.Contains("https://"))
                    {
                        _RaizDoSite = "/" + _RaizDoSite;
                    }
                        

                    //
                    // TipoDeObjetoAtivador: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["TipoDeObjetoAtivador"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "Mock";

                    _TipoDeObjetoAtivador = lValor;

                    //
                    // Rais do Site: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["HostDoSite"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "";

                    _HostDoSite = lValor;

                    //
                    // Path do upload do termo de adesão do fundo : -------------------------------------------------------------------
                    //


                    lValor = ConfigurationManager.AppSettings["PathUploadTermoAdesaoFundo"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "";

                    _pathTermoAdesao = lValor;

                    //
                    // Path do upload do Download de adesão de FIDC : -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["RaizDownloadsFIDC"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "";

                    _pathFIDC = lValor;

                    //
                    // Path do upload do Download de adesão de Carteira Diaria : -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["RaizDownloadsCarteiraDiaria"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "";

                    _pathCarteiraDiaria = lValor;

                    //
                    // Path do upload do Download de adesão de Extrato Cotista : -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["RaizDownloadsExtratoCotista"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "";

                    _pathExtratoCotista = lValor;

                    //
                    // Path do upload do Download de adesão de Extrato Cotista : -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["RaizDownloadsMEC"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "";

                    _pathMEC = lValor;

                    //
                    // Adicionar outras configurações como propriedades abaixo ----------------------
                    //

                }
            }
            catch (Exception ex)
            {
                ILog lLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

                lLogger.ErrorFormat("Erro [{0}] em ConfiguracoesValidadas.Iniciar()\r\n    >>Stack:\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);
            }
        }

        #endregion
    }
}
