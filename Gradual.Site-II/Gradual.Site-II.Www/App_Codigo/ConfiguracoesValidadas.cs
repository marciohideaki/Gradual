using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using log4net;

namespace Gradual.Site.Www
{
    public static class ConfiguracoesValidadas
    {
        #region Globais

        private static bool gIniciado = false;

        #endregion

        #region Propriedades

        private static bool _AplicacaoEmModoDeTeste;

        private static bool _AnalyticsHabilitado;

        private static string _HostDoSite;

        private static string _RaizDoSite;

        private static string _VersaoDoSite;

        private static string _SkinPadrao;

        private static int _IdDaLista_OfertasPublicasParaHome;

        private static int _IdDaLista_DestaquesParaHome;

        private static int _IdDaLista_BannersParaHome;
        
        private static int _IdDaLista_VideoDaHome;

        private static int _IdDaLista_ChatPublicado;

        private static int _IdDaLista_CarteiraRecomendada;

        private static int _IdDaLista_ISIN;

        private static int _IdDaLista_BannersProRodape;

        private static string _CodigoAtualIbovFuturo;

        private static string _CodigoAtualDolarFuturo;

        private static string _CodigoAtualDIFuturo;

        private static int _IdPlanoPoupeGradual200;

        private static int _IdPlanoPoupeGradual500;

        private static int _IdPlanoPoupeGradual700;

        private static int _IdPlanoCalculadoraIRAberto;

        private static int _IdPlanoCalculadoraIRFeachado;

        private static int _IdDoProduto_GradualTraderInterface;

        private static int _IdDoProduto_GradualGraficos;

        private static int _IdDoProduto_CursoMarcioNoronha;

        private static int _IdDoProduto_StockMarket;

        private static int _IdDoProduto_GradualTravelCard;

        private static int _IdDoProduto_CursoAnaliseGrafica;

        private static string _TVGradual_UrlDaAPI;

        private static string _TVGradual_ChaveDaAPI;

        private static string _TVGradual_ChaveSecretaDaAPI;

        private static string _CodigoPermissao_EditarCMS;

        private static string _CodigoPermissao_EditarAnaliseEconomica;

        private static string _CodigoPermissao_EditarAnaliseFundamentalista;

        private static string _CodigoPermissao_EditarAnaliseGrafica;

        private static string _CodigoPermissao_EditarCarteirasRecomendadas;

        private static string _CodigoPermissao_EditarNikkei;

        private static string _CodigoPermissao_EditarGradiusGestao;

        private static string _Email_Movimentacao;

        private static string _Email_Movimentacao_Wealth;

        private static string _Email_Tesouraria;

        private static string _Email_Compliance;

        private static string _Email_Atendimento;

        private static string _Email_Ouvidoria;

        private static string _Email_RemetenteGradual;

        private static string _Email_NotificacaoDeposito_Remetente;

        private static string _Email_NotificacaoDeposito_Destinatarios;

        private static string _Email_CopiaDeEnvioDoCadastro;

        private static string _Email_CompraCambio;

        private static string _Email_CadastroCambio;

        private static string _Email_ReservaIPO;

        private static string _CalculadoraIR_IDCorretora;

        private static string _CalculadoraIR_EmailIR;

        private static string _CalculadoraIR_SiteMyCapital;

        private static string _Chat_URL;

        private static string _IPO_URL;

        private static string _Ordens_PortaDeControle;


        private static int _IdContrato_TermoParaRealizacaoOrdemStop;

        private static int _IdContrato_TermoAlavancagemFinanceira = -1;


        private static string _TravelCard_Url;

        private static int _IdDoTipo_BannerLateral;

        private static int _IdDoTipo_BannerLateralLink;

        private static int _IdDoTipo_MenuPrincipal;

        private static int _IdDoTipo_ConteudoGenerico;

        private static string _UsuarioFinancial;

        private static string _SenhaFinancial;

        private static string _CepLivreChave;

        private static string _UsuarioItau;

        private static string _SenhaItau;

        private static string _CodigoGestorItau;

        private static string _WSItauOperacao;

        private static string _WSItauCotista;

        private static List<string> _PaginasQueIgnoramExtenaoAspx = null;

        private static List<int> _FundosInaplicaveis = null;

        private static List<string> _IPsDeLogoutHB = null;
        
        private static List<string> _EstadosPermitidosEntregaCambio = null;

        private static Dictionary<string, string> _EmailsCompraCambio_PorEstado = new Dictionary<string, string>();

        private static bool _PermitirComprarDuasVezes = false;

        private static decimal _taxaTransferencia = 0;

        private static System.String _pathVirtualPortal = String.Empty;

        private static string _UrlPlataformaProduto;

        public static bool AplicacaoEmModoDeTeste
        {
            get
            {
                Iniciar();

                return _AplicacaoEmModoDeTeste;
            }
        }

        public static bool AnalyticsHabilitado
        {
            get
            {
                Iniciar();

                return _AnalyticsHabilitado;
            }
        }

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

        public static string HostDoSite 
        {
            get
            {
                Iniciar();

                return _HostDoSite;
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

        public static int IdDaLista_OfertasPublicasParaHome
        {
            get
            {
                Iniciar();

                return _IdDaLista_OfertasPublicasParaHome;
            }
        }

        public static int IdDaLista_DestaquesParaHome
        {
            get
            {
                Iniciar();

                return _IdDaLista_DestaquesParaHome;
            }
        }
        
        public static int IdDaLista_BannersParaHome
        {
            get
            {
                Iniciar();

                return _IdDaLista_BannersParaHome;
            }

            set
            {
                _IdDaLista_BannersParaHome = value;
            }
        }

        public static int IdDaLista_VideoDaHome
        {
            get
            {
                Iniciar();

                return _IdDaLista_VideoDaHome;
            }

            set
            {
                _IdDaLista_VideoDaHome = value;
            }
        }

        public static int IdDaLista_ChatPublicado
        {
            get 
            {
                Iniciar();

                return _IdDaLista_ChatPublicado;
            }
            set 
            {
                _IdDaLista_ChatPublicado = value;
            }
        }

        public static int IdDaLista_CarteiraRecomendada
        {
            get
            {
                Iniciar();

                return _IdDaLista_CarteiraRecomendada;
            }
            set
            {
                _IdDaLista_CarteiraRecomendada = value;
            }
        }

        public static int IdDaLista_ISIN
        {
            get
            {
                Iniciar();

                return _IdDaLista_ISIN;
            }
            set
            {
                _IdDaLista_ISIN = value;
            }
        }

        public static int IdPlanoPoupeGradual200
        {
            get
            {
                Iniciar();

                return _IdPlanoPoupeGradual200;
            }
            set
            {
                _IdPlanoPoupeGradual200 = value;
            }
        }

        public static int IdPlanoPoupeGradual500
        {
            get
            {
                Iniciar();

                return _IdPlanoPoupeGradual500;
            }
            set
            {
                _IdPlanoPoupeGradual500 = value;
            }
        }

        public static int IdPlanoPoupeGradual700
        {
            get
            {
                Iniciar();

                return _IdPlanoPoupeGradual700;
            }
            set
            {
                _IdPlanoPoupeGradual700 = value;
            }
        }

        public static int IdPlanoCalculadoraIRAberto
        {
            get
            {
                Iniciar();

                return _IdPlanoCalculadoraIRAberto;
            }
            set
            {
                _IdPlanoCalculadoraIRAberto = value;
            }
        }

        public static int IdPlanoCalculadoraIRFeachado
        {
            get
            {
                Iniciar();

                return _IdPlanoCalculadoraIRFeachado;
            }
            set
            {
                _IdPlanoCalculadoraIRFeachado = value;
            }
        }

        public static int IdDaLista_BannersProRodape
        {
            get
            {
                Iniciar();

                return _IdDaLista_BannersProRodape;
            }

            set
            {
                _IdDaLista_BannersProRodape = value;
            }
        }

        public static string CodigoAtualIbovFuturo 
        {
            get
            {
                Iniciar();

                return _CodigoAtualIbovFuturo;
            }
        }

        public static string CodigoAtualDolarFuturo 
        {
            get
            {
                Iniciar();

                return _CodigoAtualDolarFuturo;
            }
        }

        public static string CodigoAtualDIFuturo 
        {
            get
            {
                Iniciar();

                return _CodigoAtualDIFuturo;
            }
        }

        public static int IdDoProduto_GradualTraderInterface
        {
            get
            {
                Iniciar();

                return _IdDoProduto_GradualTraderInterface;
            }
        }

        public static int IdDoProduto_GradualGraficos
        {
            get
            {
                Iniciar();

                return _IdDoProduto_GradualGraficos;
            }
        }

        public static int IdDoProduto_CursoMarcioNoronha
        {
            get
            {
                Iniciar();

                return _IdDoProduto_CursoMarcioNoronha;
            }
        }

        public static int IdDoProduto_StockMarket
        {
            get
            {
                Iniciar();

                return _IdDoProduto_StockMarket;
            }
        }
        
        public static int IdDoProduto_GradualTravelCard
        {
            get
            {
                Iniciar();

                return _IdDoProduto_GradualTravelCard;
            }
        }
        
        public static int IdDoProduto_CursoAnaliseGrafica
        {
            get
            {
                Iniciar();

                return _IdDoProduto_CursoAnaliseGrafica;
            }
        }

        public static string TVGradual_UrlDaAPI
        {
            get
            {
                Iniciar();

                return _TVGradual_UrlDaAPI;
            }
        }

        public static string TVGradual_ChaveDaAPI
        {
            get
            {
                Iniciar();

                return _TVGradual_ChaveDaAPI;
            }
        }

        public static string TVGradual_ChaveSecretaDaAPI
        {
            get
            {
                Iniciar();

                return _TVGradual_ChaveSecretaDaAPI;
            }
        }


        public static string CodigoPermissao_EditarCMS
        {
            get
            {
                Iniciar();

                return _CodigoPermissao_EditarCMS;
            }
        }

        public static string CodigoPermissao_EditarAnaliseEconomica
        {
            get
            {
                Iniciar();

                return _CodigoPermissao_EditarAnaliseEconomica;
            }
        }

        public static string CodigoPermissao_EditarAnaliseFundamentalista
        {
            get
            {
                Iniciar();

                return _CodigoPermissao_EditarAnaliseFundamentalista;
            }
        }

        public static string CodigoPermissao_EditarAnaliseGrafica
        {
            get
            {
                Iniciar();

                return _CodigoPermissao_EditarAnaliseGrafica;
            }
        }

        public static string CodigoPermissao_EditarCarteirasRecomendadas
        {
            get
            {
                Iniciar();

                return _CodigoPermissao_EditarCarteirasRecomendadas;
            }
        }

        public static string CodigoPermissao_EditarNikkei
        {
            get
            {
                Iniciar();

                return _CodigoPermissao_EditarNikkei;
            }
        }

        public static string CodigoPermissao_EditarGradiusGestao
        {
            get
            {
                Iniciar();

                return _CodigoPermissao_EditarGradiusGestao;
            }
        }

        public static string Email_Atendimento
        {
            get
            {
                Iniciar();

                return _Email_Atendimento;
            }
        }
        
        public static string Email_Compliance
        {
            get
            {
                Iniciar();

                return _Email_Compliance;
            }
        }

        public static string Email_Ouvidoria
        {
            get
            {
                Iniciar();

                return _Email_Ouvidoria;
            }
        }

        public static string Email_RemetenteGradual
        {
            get
            {
                Iniciar();

                return _Email_RemetenteGradual;
            }
        }

        public static string Email_Tesouraria
        {
            get
            {
                Iniciar();

                return _Email_Tesouraria;
            }
        }

        public static string Email_Movimentacao
        {
            get
            {
                Iniciar();

                return _Email_Movimentacao;
            }
        }

        public static string Email_Movimentacao_Wealth
        {
            get
            {
                Iniciar();

                return _Email_Movimentacao_Wealth;
            }
        }

        public static string Email_NotificacaoDeposito_Destinatarios
        {
            get
            {
                Iniciar();

                return _Email_NotificacaoDeposito_Destinatarios;
            }
        }

        public static string Email_NotificacaoDeposito_Remetente
        {
            get
            {
                Iniciar();

                return _Email_NotificacaoDeposito_Remetente;
            }
        }

        public static string Email_CopiaDeEnvioDoCadastro
        {
            get
            {
                Iniciar();

                return _Email_CopiaDeEnvioDoCadastro;
            }
        }

        public static string Email_CompraCambio
        {
            get
            {
                Iniciar();

                return _Email_CompraCambio;
            }
        }

        public static string Email_CadastroCambio
        {
            get
            {
                Iniciar();

                return _Email_CadastroCambio;
            }
        }

        public static string Email_ReservaIPO
        {
            get
            {
                Iniciar();

                return _Email_ReservaIPO;
            }
        }
        
        public static Dictionary<string, string> EmailsCompraCambio_PorEstado
        {
            get
            {
                Iniciar();

                return _EmailsCompraCambio_PorEstado;
            }
        }

        public static string CalculadoraIR_IDCorretora
        {
            get
            {
                Iniciar();

                return _CalculadoraIR_IDCorretora;
            }
        }

        public static string CalculadoraIR_EmailIR
        {
            get
            {
                Iniciar();

                return _CalculadoraIR_EmailIR;
            }
        }

        public static string CalculadoraIR_SiteMyCapital
        {
            get
            {
                Iniciar();

                return _CalculadoraIR_SiteMyCapital;
            }
        }


        public static string Chat_URL
        {
            get
            {
                Iniciar();

                return _Chat_URL;
            }
        }

        public static string IPO_URL
        {
            get
            {
                Iniciar();

                return _IPO_URL;
            }
        }


        public static string Ordens_PortaDeControle
        {
            get
            {
                Iniciar();

                return _Ordens_PortaDeControle;
            }
        }


        public static int IdContrato_TermoParaRealizacaoOrdemStop
        {
            get
            {
                Iniciar();

                return _IdContrato_TermoParaRealizacaoOrdemStop;
            }
        }

        public static int IdContrato_TermoAlavancagemFinanceira
        {
            get
            {
                Iniciar();

                return _IdContrato_TermoAlavancagemFinanceira;
            }
        }


        public static string TravelCard_Url
        {
            get
            {
                Iniciar();

                return _TravelCard_Url;
            }
        }


        public static int IdDoTipo_BannerLateral
        {
            get
            {
                Iniciar();

                return _IdDoTipo_BannerLateral;
            }
        }

        public static int IdDoTipo_BannerLateralLink
        {
            get
            {
                Iniciar();

                return _IdDoTipo_BannerLateralLink;
            }
        }
        
        public static int IdDoTipo_MenuPrincipal
        {
            get
            {
                Iniciar();

                return _IdDoTipo_MenuPrincipal;
            }
        }

        public static int IdDoTipo_ConteudoGenerico
        {
            get
            {
                Iniciar();

                return _IdDoTipo_ConteudoGenerico;
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

        public static string CepLivreChave
        {
            get
            {
                Iniciar();

                return _CepLivreChave;
            }
        }


        public static string UsuarioItau
        {
            get
            {
                Iniciar();

                return _UsuarioItau;
            }
        }

        public static string SenhaItau
        {
            get
            {
                Iniciar();

                return _SenhaItau;
            }
        }

        public static string CodigoGestorItau
        {
            get
            {
                Iniciar();

                return _CodigoGestorItau;
            }

        }

        public static string WSItauOperacao
        {
            get
            {
                Iniciar();

                return _WSItauOperacao;
            }
        }

        public static string WSItauCotista
        {
            get
            {
                Iniciar();

                return _WSItauCotista;
            }
        }

        public static List<string> PaginasQueIgnoramExtenaoAspx
        {
            get
            {
                Iniciar();

                return _PaginasQueIgnoramExtenaoAspx;
            }
        }

        public static List<int> FundosInaplicaveis
        {
            get
            {
                Iniciar();

                return _FundosInaplicaveis;
            }
        }
        
        public static List<string> IPsDeLogoutHB
        {
            get
            {
                Iniciar();

                return _IPsDeLogoutHB;

            }
        }

        public static List<string> EstadosPermitidosEntregaCambio
        {
            get
            {
                Iniciar();

                return _EstadosPermitidosEntregaCambio;

            }
        }

        public static bool PermitirComprarDuasVezes
        {
            get
            {
                Iniciar();

                return _PermitirComprarDuasVezes;
            }
        }

        public static decimal TaxaTransferencia
        {
            get
            {
                Iniciar();

                return _taxaTransferencia;
            }
        }

        public static System.String PathVirtualPortal
        {
            get
            {
                Iniciar();

                return _pathVirtualPortal;
            }
        }

        public static string UrlPlataformaProduto
        {
            get
            {
                Iniciar();

                return _UrlPlataformaProduto;
            }
        }
        #endregion

        #region Métodos Private

        private static void Iniciar()
        {
            try
            {
                if (!gIniciado)
                {
                    //gIniciado = true;

                    string lValor;
                    
                    //
                    // Skin Padrão: -------------------------------------------------------------------
                    //

                    _AplicacaoEmModoDeTeste = (Convert.ToString(ConfigurationManager.AppSettings["AplicacaoEmModoDeTeste"]).ToLower() == "true");

                    //
                    // Analytics Habilitado:  -------------------------------------------------------------------
                    //

                    _AnalyticsHabilitado = (Convert.ToString(ConfigurationManager.AppSettings["AnalyticsHabilitado"]).ToLower() == "true");


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
                    // Host do Site: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["HostDoSite"];
                    

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception(string.Format("Valor [{0}] para HostDoSite inválido!", lValor));

                    if(lValor.EndsWith("/"))
                        lValor = lValor.TrimEnd('/');

                    lValor = lValor.TrimEnd();

                    _HostDoSite = lValor;


                    //
                    // Raiz do Site: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["RaizDoSite"];

                    _RaizDoSite = lValor;

                    if (!string.IsNullOrEmpty(_RaizDoSite))
                    {
                        if(lValor.EndsWith("/"))
                            lValor = lValor.TrimEnd('/');
                        
                        //se tem a raiz, precisa de uma barra no final do host para poder concatenar direito com a raiz e NAO ter barra no final

                        _HostDoSite = _HostDoSite + "/";
                    }


                    //
                    // IdDaLista_DestaquesParaHome: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDaLista_OfertasPublicasParaHome"];

                    if (!int.TryParse(lValor, out _IdDaLista_OfertasPublicasParaHome))
                        throw new Exception(string.Format("Valor [{0}] para IdDaLista_OfertasPublicasParaHome inválido!", lValor));

                    //
                    // IdDaLista_DestaquesParaHome: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDaLista_DestaquesParaHome"];

                    if (!int.TryParse(lValor, out _IdDaLista_DestaquesParaHome))
                        throw new Exception(string.Format("Valor [{0}] para IdDaLista_DestaquesParaHome inválido!", lValor));
                    
                    //
                    // IdDaLista_BannersParaHome: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDaLista_BannersParaHome"];

                    if (!int.TryParse(lValor, out _IdDaLista_BannersParaHome))
                        throw new Exception(string.Format("Valor [{0}] para IdDaLista_BannersParaHome inválido!", lValor));

                    //
                    // IdDaLista_VideoDaHome: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDaLista_VideoDaHome"];

                    if (!int.TryParse(lValor, out _IdDaLista_VideoDaHome))
                        throw new Exception(string.Format("Valor [{0}] para IdDaLista_VideoDaHome inválido!", lValor));

                    //
                    // IdDaLista_BannersProRodape: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDaLista_BannersProRodape"];

                    if (!int.TryParse(lValor, out _IdDaLista_BannersProRodape))
                        throw new Exception(string.Format("Valor [{0}] para IdDaLista_BannersProRodape inválido!", lValor));

                    
                    //
                    // CodigoAtualIbovFuturo: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoAtualIbovFuturo"];

                    _CodigoAtualIbovFuturo = lValor;
                    
                    //
                    // CodigoAtualDolarFuturo: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoAtualDolarFuturo"];

                    _CodigoAtualDolarFuturo = lValor;
                    
                    //
                    // CodigoAtualDIFuturo: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoAtualDIFuturo"];

                    _CodigoAtualDIFuturo = lValor;


                    //
                    // CodigoListadeChatPublicados: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDaLista_ChatPublicado"];

                    if (!int.TryParse(lValor, out _IdDaLista_ChatPublicado))
                        throw new Exception(string.Format("Valor [{0}] para IdDaLista_ChatPublicado inválido!", lValor));

                    //
                    // CodigoListaCarteiraRecomendada: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDaLista_CarteiraRecomendada"];

                    if (!int.TryParse(lValor, out _IdDaLista_CarteiraRecomendada))
                        throw new Exception(string.Format("Valor [{0}] para IdDaLista_CarteiraRecomendada inválido!", lValor));

                    //
                    // CodigoListaDeISIN: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDaLista_ISIN"];

                    if (!int.TryParse(lValor, out _IdDaLista_ISIN))
                        throw new Exception(string.Format("Valor [{0}] para IdDaLista_ISIN inválido!", lValor));


                    //
                    // CodigoPlanoPoupeGradual200: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdPlanoPoupeGradual200"];

                    if (!int.TryParse(lValor, out _IdPlanoPoupeGradual200))
                        throw new Exception(string.Format("Valor [{0}] para IdPlanoPoupeGradual200 inválido!", lValor));

                    //
                    // CodigoPlanoPoupeGradual500: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdPlanoPoupeGradual500"];

                    if (!int.TryParse(lValor, out _IdPlanoPoupeGradual500))
                        throw new Exception(string.Format("Valor [{0}] para IdPlanoPoupeGradual500 inválido!", lValor));


                    //
                    // CodigoPlanoPoupeGradual700: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdPlanoPoupeGradual700"];

                    if (!int.TryParse(lValor, out _IdPlanoPoupeGradual700))
                        throw new Exception(string.Format("Valor [{0}] para IdPlanoPoupeGradual700 inválido!", lValor));

                    //
                    // IdPlanoCalculadoraIRAberto: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdPlanoCalculadoraIRAberto"];

                    if (!int.TryParse(lValor, out _IdPlanoCalculadoraIRAberto))
                        throw new Exception(string.Format("Valor [{0}] para IdPlanoCalculadoraIRAberto inválido!", lValor));

                    //
                    // IdPlanoCalculadoraIRFeachado: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdPlanoCalculadoraIRFeachado"];

                    if (!int.TryParse(lValor, out _IdPlanoCalculadoraIRFeachado))
                        throw new Exception(string.Format("Valor [{0}] para IdPlanoCalculadoraIRFeachado inválido!", lValor));


                    //
                    // IdDoProduto_GradualTraderInterface: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDoProduto_GradualTraderInterface"];

                    if (!int.TryParse(lValor, out _IdDoProduto_GradualTraderInterface))
                        throw new Exception(string.Format("Valor [{0}] para IdDoProduto_GradualTraderInterface inválido!", lValor));


                    //
                    // IdDoProduto_GradualGraficos: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDoProduto_GradualGraficos"];

                    if (!int.TryParse(lValor, out _IdDoProduto_GradualGraficos))
                        throw new Exception(string.Format("Valor [{0}] para IdDoProduto_GradualGraficos inválido!", lValor));


                    //
                    // IdDoProduto_CursoMarcioNoronha: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDoProduto_CursoMarcioNoronha"];

                    if (!int.TryParse(lValor, out _IdDoProduto_CursoMarcioNoronha))
                        throw new Exception(string.Format("Valor [{0}] para IdDoProduto_CursoMarcioNoronha inválido!", lValor));


                    //
                    // IdDoProduto_StockMarket: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDoProduto_StockMarket"];

                    if (!int.TryParse(lValor, out _IdDoProduto_StockMarket))
                        throw new Exception(string.Format("Valor [{0}] para IdDoProduto_StockMarket inválido!", lValor));
                    

                    //
                    // IdDoProduto_GradualTravelCard: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDoProduto_GradualTravelCard"];

                    if (!int.TryParse(lValor, out _IdDoProduto_GradualTravelCard))
                        throw new Exception(string.Format("Valor [{0}] para IdDoProduto_GradualTravelCard inválido!", lValor));

                    
                    //
                    // IdDoProduto_CursoAnaliseGrafica: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDoProduto_CursoAnaliseGrafica"];

                    if (!int.TryParse(lValor, out _IdDoProduto_CursoAnaliseGrafica))
                        throw new Exception(string.Format("Valor [{0}] para IdDoProduto_CursoAnaliseGrafica inválido!", lValor));


                    //
                    // TVGradual_UrlDaAPI: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["TVGradual_UrlDaAPI"];

                    _TVGradual_UrlDaAPI = lValor;
                    
                    //
                    // TVGradual_ChaveDaAPI: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["TVGradual_ChaveDaAPI"];

                    _TVGradual_ChaveDaAPI = lValor;
                    
                    //
                    // TVGradual_ChaveSecretaDaAPI: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["TVGradual_ChaveSecretaDaAPI"];

                    _TVGradual_ChaveSecretaDaAPI = lValor;

                    
                    //
                    // CodigoPermissao_EditarCMS: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoPermissao_EditarCMS"];

                    _CodigoPermissao_EditarCMS = lValor;

                    //
                    // CodigoPermissao_EditarAnaliseEconomica: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoPermissao_EditarAnaliseEconomica"];

                    _CodigoPermissao_EditarAnaliseEconomica = lValor;
                    
                    //
                    // CodigoPermissao_EditarAnaliseFundamentalista: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoPermissao_EditarAnaliseFundamentalista"];

                    _CodigoPermissao_EditarAnaliseFundamentalista = lValor;

                    //
                    // CodigoPermissao_EditarAnaliseGrafica: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoPermissao_EditarAnaliseGrafica"];

                    _CodigoPermissao_EditarAnaliseGrafica = lValor;

                    //
                    // CodigoPermissao_EditarCarteirasRecomendadas: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoPermissao_EditarCarteirasRecomendadas"];

                    _CodigoPermissao_EditarCarteirasRecomendadas = lValor;
                    
                    //
                    // CodigoPermissao_EditarCarteirasRecomendadas: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoPermissao_EditarNikkei"];

                    _CodigoPermissao_EditarNikkei = lValor;

                    //
                    // CodigoPermissao_EditarCarteirasRecomendadas: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoPermissao_EditarGradiusGestao"];

                    _CodigoPermissao_EditarGradiusGestao = lValor;

                    //
                    // Email_Tesouraria: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Email_Tesouraria"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Email_Tesouraria!");

                    _Email_Tesouraria = lValor;

                    //
                    // Email_Movimentacao: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Email_Movimentacao"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Email_Movimentacao!");

                    _Email_Movimentacao = lValor;

                    //
                    // Email_Movimentacao_Wealth: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Email_Movimentacao_Wealth"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Email_Movimentacao_Wealth!");

                    _Email_Movimentacao_Wealth = lValor;
                    
                    //
                    // Email_Atendimento: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Email_Atendimento"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Email_Atendimento!");

                    _Email_Atendimento = lValor;
                    
                    //
                    // Email_Compliance: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Email_Compliance"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Email_Compliance!");

                    _Email_Compliance = lValor;

                    //
                    // Email_Ouvidoria: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Email_Ouvidoria"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "solucoes@gradualinvestimentos.com.br";

                    _Email_Ouvidoria = lValor;


                    //
                    // Email_RemetenteGradual: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Email_RemetenteGradual"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Email_RemetenteGradual!");

                    _Email_RemetenteGradual = lValor;
                    
                    
                    //
                    // Email_NotificacaoDeposito_Remetente: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Email_NotificacaoDeposito_Remetente"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Email_NotificacaoDeposito_Remetente!");

                    _Email_NotificacaoDeposito_Remetente = lValor;


                    //
                    // Email_CopiaDeEnvioDoCadastro: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Email_CopiaDeEnvioDoCadastro"];

                    _Email_CopiaDeEnvioDoCadastro = lValor;

                    
                    //
                    // Email_NotificacaoDeposito_Destinatarios: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Email_NotificacaoDeposito_Destinatarios"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Email_NotificacaoDeposito_Destinatarios!");

                    _Email_NotificacaoDeposito_Destinatarios = lValor;


                    //
                    // Email_CompraCambio: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Email_CompraCambio"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Email_CompraCambio!");

                    if (lValor.Contains(":"))
                    {
                        //valores separados por estado, assim:
                        //SP:compra_sp@lalala.com,RJ:compra_rj@lalala.com

                        string[] lEmails = lValor.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] lEmailVals;

                        foreach (string lEmailStr in lEmails)
                        {
                            lEmailVals = lEmailStr.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                            if (lEmailVals.Length == 2)
                            {
                                try
                                {
                                    _EmailsCompraCambio_PorEstado.Add(lEmailVals[0].Trim().ToUpper(), lEmailVals[1].Trim());

                                    if (string.IsNullOrEmpty(_Email_CompraCambio))
                                        _Email_CompraCambio = lEmailVals[1];        //o primeiro fica como "padrão" para emails de compra de câmbio quando houver mais de um.
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        _Email_CompraCambio = lValor;
                    }

                    //
                    // Email_CadastroCambio: -------------------------------------------------------------------
                    //
                    lValor = ConfigurationManager.AppSettings["Email_CadastroCambio"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Email_CadastroCambio!");

                    _Email_CadastroCambio = lValor;

                    //
                    // Email_ReservaIPO: -------------------------------------------------------------------
                    //
                    lValor = ConfigurationManager.AppSettings["Email_ReservaIPO"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Email_ReservaIPO!");

                    _Email_ReservaIPO = lValor;

                    //
                    // CalculadoraIR_IDCorretora: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CalculadoraIR_IDCorretora"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para CalculadoraIR_IDCorretora!");

                    _CalculadoraIR_IDCorretora = lValor;

                    //
                    // CalculadoraIR_EmailIR: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CalculadoraIR_EmailIR"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para CalculadoraIR_EmailIR!");

                    _CalculadoraIR_EmailIR = lValor;

                    //
                    // CalculadoraIR_SiteMyCapital: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CalculadoraIR_SiteMyCapital"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para CalculadoraIR_SiteMyCapital!");

                    _CalculadoraIR_SiteMyCapital = lValor;


                    //
                    // _Chat_URL: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Chat_URL"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Chat_URL!");

                    _Chat_URL = lValor;

                    //
                    // IPO_URL: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IPO_URL"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para IPO_URL!");

                    _IPO_URL = lValor;


                    //
                    // Ordens_PortaDeControle: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["Ordens_PortaDeControle"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Valor vazio para Ordens_PortaDeControle!");

                    _Ordens_PortaDeControle = lValor;

                    
                    //
                    // IdContrato_TermoParaRealizacaoOrdemStop: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdContrato_TermoParaRealizacaoOrdemStop"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no web.config para 'IdContrato_TermoParaRealizacaoOrdemStop' está vazio ou nulo.");

                    if (!int.TryParse(lValor, out _IdContrato_TermoParaRealizacaoOrdemStop))
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'IdContrato_TermoParaRealizacaoOrdemStop' [{0}] não é um número Int32 válido.", lValor));

                    if (_IdContrato_TermoParaRealizacaoOrdemStop < 1)
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'IdContrato_TermoParaRealizacaoOrdemStop' [{0}] não pode ser zero ou negativo.", lValor));


                    //
                    // IdContrato_TermoAlavancagemFinanceira: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdContrato_TermoAlavancagemFinanceira"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no web.config para 'IdContrato_TermoAlavancagemFinanceira' está vazio ou nulo.");

                    if (!int.TryParse(lValor, out _IdContrato_TermoAlavancagemFinanceira))
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'IdContrato_TermoAlavancagemFinanceira' [{0}] não é um número Int32 válido.", lValor));

                    if (_IdContrato_TermoAlavancagemFinanceira < 1)
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'IdContrato_TermoAlavancagemFinanceira' [{0}] não pode ser zero ou negativo.", lValor));


                    //
                    // TravelCard_Url: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["TravelCard_Url"];

                    _TravelCard_Url = lValor;

                    
                    //
                    // _IdDoTipo_BannerLateral: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDoTipo_BannerLateral"];

                    if (!int.TryParse(lValor, out _IdDoTipo_BannerLateral))
                        throw new Exception(string.Format("Valor [{0}] para _IdDoTipo_BannerLateral inválido!", lValor));

                    //
                    // _IdDoTipo_BannerLateralLink: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDoTipo_BannerLateralLink"];

                    if (!int.TryParse(lValor, out _IdDoTipo_BannerLateralLink))
                        throw new Exception(string.Format("Valor [{0}] para IdDoTipo_BannerLateralLink inválido!", lValor));
                    
                    //
                    // _IdDoTipo_MenuPrincipal: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDoTipo_MenuPrincipal"];

                    if (!int.TryParse(lValor, out _IdDoTipo_MenuPrincipal))
                        throw new Exception(string.Format("Valor [{0}] para IdDoTipo_MenuPrincipal inválido!", lValor));

                    
                    //
                    // IdDoTipo_ConteudoGenerico: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDoTipo_ConteudoGenerico"];

                    if (!int.TryParse(lValor, out _IdDoTipo_ConteudoGenerico))
                        throw new Exception(string.Format("Valor [{0}] para IdDoTipo_ConteudoGenerico inválido!", lValor));


                    //
                    // _UsuarioFinancial:------------------------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["UsuarioFinancial"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no Appsettings.config para 'UsuarioFinancial' está vazio ou nulo.");

                    _UsuarioFinancial = lValor;

                    //
                    // _SenhaFinancial:------------------------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["SenhaFinancial"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no Appsettings.config para 'SenhaFinancial' está vazio ou nulo.");

                    _SenhaFinancial = lValor;

                    //
                    // _UsuarioItau:------------------------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["UsuarioItau"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no Appsettings.config para 'UsuarioItau' está vazio ou nulo.");

                    _UsuarioItau = lValor;

                    //
                    // _SenhaItau:------------------------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["SenhaItau"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no Appsettings.config para 'SenhaItau' está vazio ou nulo.");

                    _SenhaItau = lValor;

                    //
                    // _CodigoGestorItau:------------------------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoGestorItau"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no Appsettings.config para 'CodigoGestorItau' está vazio ou nulo.");

                    _CodigoGestorItau = lValor;

                    //
                    // _WSItauOperacao:------------------------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["GradualInvestimento_WSFundoItauOperacao"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no Appsettings.config para 'GradualInvestimento_WSFundoItauOperacao' está vazio ou nulo.");

                    _WSItauOperacao = lValor;

                    //
                    // _WSItauCotista:------------------------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["GradualInvestimento_WSFundoItauCotista"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no Appsettings.config para 'GradualInvestimento_WSFundoItauCotista' está vazio ou nulo.");

                    _WSItauCotista = lValor;
                    
                    //
                    // _CepLivreKey
                    //

                    _CepLivreChave = ConfigurationManager.AppSettings["CepLivreChave"];

                    lValor = ConfigurationManager.AppSettings["PaginasQueIgnoramExtensaoAspx"];

                    if (string.IsNullOrEmpty(lValor))
                    {
                        _PaginasQueIgnoramExtenaoAspx = new List<string>();
                    }
                    else
                    {
                        _PaginasQueIgnoramExtenaoAspx = new List<string>(lValor.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                    }

                    
                    //
                    // FundosInaplicaveis:------------------------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["FundosInaplicaveis"];

                    _FundosInaplicaveis = new List<int>();

                    if (!string.IsNullOrEmpty(lValor))
                    {
                        foreach (string lItem in lValor.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                        {
                            try
                            {
                                _FundosInaplicaveis.Add(Convert.ToInt32(lItem.Trim()));
                            }
                            catch { }
                        }
                    }
                    
                    //
                    // IPsDeLogoutHB:------------------------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IPsDeLogoutHB"];

                    _IPsDeLogoutHB = new List<string>();

                    if (!string.IsNullOrEmpty(lValor))
                    {
                        foreach (string lItem in lValor.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                        {
                            try
                            {
                                _IPsDeLogoutHB.Add(lItem.Trim());
                            }
                            catch { }
                        }
                    }
                    
                    //
                    // EstadosPermitidosEntregaCambio:------------------------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["EstadosPermitidosEntregaCambio"];

                    _EstadosPermitidosEntregaCambio = new List<string>();

                    if (!string.IsNullOrEmpty(lValor))
                    {
                        foreach (string lItem in lValor.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                        {
                            try
                            {
                                _EstadosPermitidosEntregaCambio.Add(lItem.Trim());
                            }
                            catch { }
                        }
                    }

                    
                    //
                    // PermitirComprarDuasVezes:------------------------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["PermitirComprarDuasVezes"];

                    _PermitirComprarDuasVezes = false;

                    if (!string.IsNullOrEmpty(lValor) && lValor.Trim().ToLower() == "sim")
                    {
                        _PermitirComprarDuasVezes = true;
                    }


                    lValor = ConfigurationManager.AppSettings["TaxaTransferencia"];

                    if(!string.IsNullOrEmpty(lValor))
                    {
                        decimal.TryParse(lValor, out _taxaTransferencia);
                    }

                    //
                    // PathVirtualPortal:------------------------------------------------------------------------------------
                    //
                    lValor = ConfigurationManager.AppSettings["pathVirtualPortal"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no Appsettings.config para 'pathVirtualPortal' está vazio ou nulo.");

                    _pathVirtualPortal = lValor;


                    //
                    // UrlPlataformaProduto:------------------------------------------------------------------------------------
                    //
                    lValor = ConfigurationManager.AppSettings["UrlPlataformaProdutos"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no Appsettings.config para 'urlPlataformaProdutos' está vazio ou nulo.");

                    _UrlPlataformaProduto = lValor;

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
