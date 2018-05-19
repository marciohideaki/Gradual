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

namespace Gradual.Spider
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

        private static string _TipoDeObjetoAtivador;

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
        
        public static string TipoDeObjetoAtivador
        {
            get
            {
                Iniciar();

                return _TipoDeObjetoAtivador;
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

                    if (!string.IsNullOrEmpty(_RaizDoSite) && !_RaizDoSite.StartsWith("/"))
                        _RaizDoSite = "/" + _RaizDoSite;

                    //
                    // TipoDeObjetoAtivador: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["TipoDeObjetoAtivador"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "Mock";

                    _TipoDeObjetoAtivador = lValor;


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
