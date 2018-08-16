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

namespace Gradual.Intranet
{
    public static class ConfiguracoesValidadas
    {
        #region Globais

        private static bool gIniciado = false;

        #endregion

        #region Propriedades

        private static string _SkinEmUso = "Default";
        
        private static string _TipoDeObjetoAtivador;
        
        private static string _VersaoDoSite;
        
        private static string _EmailCompliance;

        private static string _UsuarioFinancial;

        private static string _SenhaFinancial;

        private static string _Id_Termo_Alavancagem;

        private static string _PastaDeUpload_DeclaracaoSuitability;

        private static string _PermissaoDeAcesso_GTI;

        private static string _PermissaoDeAcesso_Stock;

        private static string _EmailNotificacaoLiberacaoFerramenta;
        
        private static string _EmailComCopiaOculta;

        public static string SkinEmUso 
        {
            get
            {
                Iniciar();

                return _SkinEmUso;
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
        
        public static string IDTermoAlavancagem
        {
            get
            {
                Iniciar();

                return _Id_Termo_Alavancagem;
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

        public static string EmailCompliance
        {
            get
            {
                Iniciar();

                return _EmailCompliance;
            }
        }

        public static string UsuarioFinancial
        {
            get
            {
                Iniciar();

                return _UsuarioFinancial;
            }
        }

        public static string SenhaFinancial
        {
            get
            {
                Iniciar();

                return _SenhaFinancial;
            }
        }

        public static string PastaDeUpload_DeclaracaoSuitability
        {
            get
            {
                Iniciar();

                return _PastaDeUpload_DeclaracaoSuitability;
            }
        }

        public static string PermissaoDeAcesso_GTI
        {
            get
            {
                Iniciar();

                return _PermissaoDeAcesso_GTI;
            }
        }
        
        public static string PermissaoDeAcesso_Stock
        {
            get
            {
                Iniciar();

                return _PermissaoDeAcesso_Stock;
            }
        }

        public static string EmailNotificacaoLiberacaoFerramenta
        {
            get
            {
                Iniciar();

                return _EmailNotificacaoLiberacaoFerramenta;
            }
        }

        public static string EmailComCopiaOculta
        {
            get
            {
                Iniciar();

                return _EmailComCopiaOculta;
            }
        }

        #endregion

        #region Métodos

        public static void Iniciar()
        {
            if (!gIniciado)
            {
                gIniciado = true;

                string lValor;

                //
                // Skin Padrão: -------------------------------------------------------------------
                //

                lValor = ConfigurationManager.AppSettings["SkinEmUso"];

                if (string.IsNullOrEmpty(lValor))
                    lValor = "Default";

                _SkinEmUso = lValor;

                //
                // TipoDeObjetoAtivador: -------------------------------------------------------------------
                //

                lValor = ConfigurationManager.AppSettings["TipoDeObjetoAtivador"];
                

                if (string.IsNullOrEmpty(lValor))
                    lValor = "Mock";

                _TipoDeObjetoAtivador = lValor;
                
                //
                // Versão do Site: -------------------------------------------------------------------
                //

                lValor = ConfigurationManager.AppSettings["VersaoDoSite"];

                if (string.IsNullOrEmpty(lValor))
                    lValor = "00-00-00-0000";

                _VersaoDoSite = lValor;
                
                //
                // Email Compliance: -------------------------------------------------------------------
                //

                lValor = ConfigurationManager.AppSettings["EmailCompliance"];

                if (string.IsNullOrEmpty(lValor))
                    lValor = "compliance@gradualinvestimentos.com.br";

                _EmailCompliance = lValor;

                lValor = ConfigurationManager.AppSettings["UsuarioFinancial"];

                if (string.IsNullOrEmpty(lValor))
                    lValor = "brocha1";

                _UsuarioFinancial = lValor;


                lValor = ConfigurationManager.AppSettings["SenhaFinancial"];

                if (string.IsNullOrEmpty(lValor))
                    lValor = "gradual12345*";

                _SenhaFinancial = lValor;

                //
                // Id do termo de alavancagem: -------------------------------------------------------------------
                //

                lValor = ConfigurationManager.AppSettings["ID_TERMO_ALAVANCAGEM"];

                if(string.IsNullOrEmpty(lValor))
                    lValor = "92";

                _Id_Termo_Alavancagem = lValor;
                
                //
                // Pasta pra upload do termo de suitability:   --------------------------------------------------
                //

                lValor = ConfigurationManager.AppSettings["PastaDeUpload_DeclaracaoSuitability"];

                if(string.IsNullOrEmpty(lValor))
                    lValor = "Intranet/Upload";

                _PastaDeUpload_DeclaracaoSuitability = lValor;

                
                //
                // Permissão do GTI : -------------------------------------------------------------------
                //

                lValor = ConfigurationManager.AppSettings["PermissaoDeAcesso_GTI"];

                if (string.IsNullOrEmpty(lValor))
                    lValor = "xxx_sem_permissao_definida_para_gti_xxx";

                _PermissaoDeAcesso_GTI = lValor;

                
                //
                // Permissão do Stock Market : -------------------------------------------------------------------
                //

                lValor = ConfigurationManager.AppSettings["PermissaoDeAcesso_Stock"];

                if (string.IsNullOrEmpty(lValor))
                    lValor = "xxx_sem_permissao_definida_para_stock_market_xxx";

                _PermissaoDeAcesso_Stock = lValor;


                //
                // Permissão do GTI : -------------------------------------------------------------------
                //

                lValor = ConfigurationManager.AppSettings["EmailNotificacaoLiberacaoFerramenta"];

                if (string.IsNullOrEmpty(lValor))
                    lValor = "solucoes@gradualinvestimentos.com.br";

                _EmailNotificacaoLiberacaoFerramenta = lValor;

                //
                // Destino dos emails em cópia oculta : -------------------------------------------------------------------
                //
                lValor = ConfigurationManager.AppSettings["EmailComCopiaOculta"];

                if (string.IsNullOrEmpty(lValor))
                    lValor = "solucoes@gradualinvestimentos.com.br";

                _EmailComCopiaOculta= lValor;

            }
        }

        #endregion
    }
}
