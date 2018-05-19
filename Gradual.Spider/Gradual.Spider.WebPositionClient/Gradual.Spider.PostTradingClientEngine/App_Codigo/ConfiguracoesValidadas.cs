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
using System.Collections.Generic;

namespace Gradual.Spider.PostTradingClientEngine.App_Codigo
{
    public static class ConfiguracoesValidadas
    {
        #region Globais

        private static bool gIniciado = false;

        #endregion

        #region Propriedades

        private static string _SkinPadrao = "Default";

        private static int _IdSistemaMDS = -1;

        private static string _VersaoDoSite;

        private static string _TipoDeObjetoAtivador;

        private static int _CodigoCorretora;

        private static string _EmailDeNotificacaoDeposito_Remetente;

        private static string _EmailDeNotificacaoDeposito_Destinatarios;

        private static int _IdContratoTermoParaRealizacaoOrdemStop = -1;

        private static int _IdContratoTermoAlavancagemFinanceira = -1;

        private static int _IdDoArquivoDeContratoTermoParaRealizacaoOrdemStop = -1;

        private static int _PortaControleOrdem;

        private static int _MsDeCacheDeDados = 300;

        private static int _MinutosParaFecharASessao = 0;

        private static int _TimeoutUsuarioJaLogadoMin = 30;

        private static int _IntervaloDoCentralizadorMs = 1000;

        private static string _IdDoSistemaParaOrdem = "";

        private static bool _EnvioDeOrdemParaTeste = false;

        private static bool _IgnorarAssinatura = false;

        private static bool _IgnorarLoginDeOutrasMaquinas = false;

        public static string SkinPadrao 
        {
            get
            {
                Iniciar();

                return _SkinPadrao;
            }
        }

        private static int IdSistemaMDS
        {
            get
            {
                Iniciar();

                return _IdSistemaMDS;
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

        public static string TipoDeObjetoAtivador
        {
            get
            {
                Iniciar();

                return _TipoDeObjetoAtivador;
            }
        }

        public static int CodigoCorretora
        {
            get
            {
                Iniciar();

                return _CodigoCorretora;
            }
        }

        public static string EmailDeNotificacaoDeposito_Remetente
        {
            get
            {
                Iniciar();

                return _EmailDeNotificacaoDeposito_Remetente;
            }
        }

        public static string EmailDeNotificacaoDeposito_Destinatarios
        {
            get
            {
                Iniciar();

                return _EmailDeNotificacaoDeposito_Destinatarios;
            }
        }

        public static int IdContratoTermoParaRealizacaoOrdemStop
        {
            get
            {
                Iniciar();

                return _IdContratoTermoParaRealizacaoOrdemStop;
            }
        }

        public static int IdDoArquivoDeContratoTermoParaRealizacaoOrdemStop
        {
            get
            {
                Iniciar();

                return _IdDoArquivoDeContratoTermoParaRealizacaoOrdemStop;
            }
        }

        public static int IdContratoTermoAlavancagemFinanceira
        {
            get
            {
                Iniciar();

                return _IdContratoTermoAlavancagemFinanceira;
            }
        }

        public static int PortaControleOrdem 
        {
            get
            {
                Iniciar();

                return _PortaControleOrdem;
            }
        }

        public static int MsDeCacheDeDados
        {
            get
            {
                return _MsDeCacheDeDados;
            }
        }

        public static int MinutosParaFecharASessao
        {
            get
            {
                Iniciar();

                return _MinutosParaFecharASessao;
            }
        }

        public static int TimeoutUsuarioJaLogadoMin
        {
            get
            {
                Iniciar();

                return _TimeoutUsuarioJaLogadoMin;
            }
        }

        public static int IntervaloDoCentralizadorMs
        {
            get
            {
                Iniciar();

                return _IntervaloDoCentralizadorMs;
            }
        }

        public static string IdDoSistemaParaOrdem
        {
            get
            {
                Iniciar();

                return _IdDoSistemaParaOrdem;
            }
        }

        public static bool EnvioDeOrdemParaTeste
        {
            get
            {
                Iniciar();

                return _EnvioDeOrdemParaTeste;
            }
        }

        public static bool IgnorarAssinatura
        {
            get
            {
                Iniciar();

                return _IgnorarAssinatura;
            }
        }

        public static bool IgnorarLoginDeOutrasMaquinas
        {
            get
            {
                Iniciar();

                return _IgnorarLoginDeOutrasMaquinas;
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
                        lValor = "Gradual-Azul";

                    _SkinPadrao = lValor;

                    //
                    // IdSistemaMDS: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdSistemaMDS"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no web.config para 'IdSistemaMDS' está vazio ou nulo.");

                    if (!int.TryParse(lValor, out _IdSistemaMDS))
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'IdSistemaMDS' [{0}] não é um número Int32 válido.", lValor));

                    //
                    // TipoDeObjetoAtivador: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["TipoDeObjetoAtivador"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "Mock";

                    _TipoDeObjetoAtivador = lValor;

                    //
                    // CodigoCorretora: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["CodigoCorretora"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no web.config para 'CodigoCorretora' está vazio ou nulo.");

                    if (!int.TryParse(lValor, out _CodigoCorretora))
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'CodigoCorretora' [{0}] não é um número Int32 válido.", lValor));


                    //
                    // Versão do Site: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["VersaoDoSite"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "00-00-00-0000";

                    _VersaoDoSite = lValor;

                    //
                    // EmailDeNotificacaoDeposito_Remetente: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["EmailDeNotificacaoDeposito_Remetente"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "admin@gradualinvestimentos.com.br";

                    _EmailDeNotificacaoDeposito_Remetente = lValor;

                    //
                    // EmailDeNotificacaoDeposito_Destinatario: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["EmailDeNotificacaoDeposito_Destinatarios"];

                    if (string.IsNullOrEmpty(lValor))
                        lValor = "contato@gradualinvestimentos.com.br";

                    _EmailDeNotificacaoDeposito_Destinatarios = lValor;

                    //
                    // IdContratoTermoParaRealizacaoOrdemStop: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdContratoTermoParaRealizacaoOrdemStop"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no web.config para 'IdContratoTermoParaRealizacaoOrdemStop' está vazio ou nulo.");

                    if (!int.TryParse(lValor, out _IdContratoTermoParaRealizacaoOrdemStop))
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'IdContratoTermoParaRealizacaoOrdemStop' [{0}] não é um número Int32 válido.", lValor));

                    if (_IdContratoTermoParaRealizacaoOrdemStop < 1)
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'IdContratoTermoParaRealizacaoOrdemStop' [{0}] não pode ser zero ou negativo.", lValor));

                    //
                    // IdDoArquivoDeContratoTermoParaRealizacaoOrdemStop: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdDoArquivoDeContratoTermoParaRealizacaoOrdemStop"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no web.config para 'IdDoArquivoDeContratoTermoParaRealizacaoOrdemStop' está vazio ou nulo.");

                    if (!int.TryParse(lValor, out _IdDoArquivoDeContratoTermoParaRealizacaoOrdemStop))
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'IdDoArquivoDeContratoTermoParaRealizacaoOrdemStop' [{0}] não é um número Int32 válido.", lValor));

                    if (_IdDoArquivoDeContratoTermoParaRealizacaoOrdemStop < 1)
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'IdDoArquivoDeContratoTermoParaRealizacaoOrdemStop' [{0}] não pode ser zero ou negativo.", lValor));


                    //
                    // IdContratoTermoAlavancagemFinanceira: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IdContratoTermoAlavancagemFinanceira"];

                    if (string.IsNullOrEmpty(lValor))
                        throw new Exception("Erro de configuração: Valor registrado no web.config para 'IdContratoTermoAlavancagemFinanceira' está vazio ou nulo.");

                    if (!int.TryParse(lValor, out _IdContratoTermoAlavancagemFinanceira))
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'IdContratoTermoAlavancagemFinanceira' [{0}] não é um número Int32 válido.", lValor));

                    if (_IdContratoTermoAlavancagemFinanceira < 1)
                        throw new Exception(string.Format("Erro de configuração: Valor registrado no web.config para 'IdContratoTermoAlavancagemFinanceira' [{0}] não pode ser zero ou negativo.", lValor));


                    //
                    // PortaControleOrdem -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["PortaControleOrdem"];
                    
                    if (!int.TryParse(lValor, out _PortaControleOrdem))
                    {
                        throw new Exception("Erro de configuração: Valor registrado no web.config para 'PortaControleOrdem' está vazio ou nulo.");
                    }


                    //
                    // MsDeCacheDeDados: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["MsDeCacheDeDados"];

                    if (!int.TryParse(lValor, out _MsDeCacheDeDados))
                    {
                        _MsDeCacheDeDados = 300;
                    }

                    if (_MsDeCacheDeDados > 1000) _MsDeCacheDeDados = 300;


                    //
                    // MinutosParaFecharASessao: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["MinutosParaFecharASessao"];

                    if (!int.TryParse(lValor, out _MinutosParaFecharASessao))
                    {
                        _MinutosParaFecharASessao = 0;
                    }

                    
                    //
                    // TimeoutUsuarioJaLogadoMin: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["TimeoutUsuarioJaLogadoMin"];

                    if (!int.TryParse(lValor, out _TimeoutUsuarioJaLogadoMin))
                    {
                        _TimeoutUsuarioJaLogadoMin = 30;
                    }


                    //
                    // IntervaloDoCentralizadorMs: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IntervaloDoCentralizadorMs"];

                    if (!int.TryParse(lValor, out _IntervaloDoCentralizadorMs))
                    {
                        _IntervaloDoCentralizadorMs = 1000;
                    }


                    //
                    // IdDoSistemaParaOrdem: -------------------------------------------------------------------
                    //

                    _IdDoSistemaParaOrdem = ConfigurationManager.AppSettings["IdDoSistemaParaOrdem"];


                    //
                    // EnvioDeOrdemParaTeste: -------------------------------------------------------------------
                    //

                    _EnvioDeOrdemParaTeste = (string.Format("{0}", ConfigurationManager.AppSettings["EnvioDeOrdemParaTeste"]).ToLower() == "true");
                    

                    //
                    // IgnorarAssinatura: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IgnorarAssinatura"];

                    _IgnorarAssinatura = (lValor == "true");


                    //
                    // IgnorarLoginDeOutrasMaquinas: -------------------------------------------------------------------
                    //

                    lValor = ConfigurationManager.AppSettings["IgnorarLoginDeOutrasMaquinas"];

                    _IgnorarLoginDeOutrasMaquinas = (lValor == "true");


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
