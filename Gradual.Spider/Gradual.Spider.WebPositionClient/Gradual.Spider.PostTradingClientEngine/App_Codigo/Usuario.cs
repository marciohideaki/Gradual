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

//using Gradual.OMS.Contratos.Comum.Mensagens;


using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;
using System.Collections.Generic;
using log4net;
using System.Data.Common;
//using Gradual.HomeBroker.Www.WsCadastro;

namespace Gradual.Spider.PostTradingClientEngine.App_Codigo
{
    [Serializable]
    public class Usuario
    {
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DateTime gUltimaAtualizacaoCustodia = DateTime.Now;

        #region Propriedades

        /// <summary>
        /// Sessão do cliente obtida na autenticação do usuário.
        /// </summary>
        public string CodigoDaSessao { get; set; }
        
        /// <summary>
        /// Código de conta corrente do cliente.
        /// </summary>
        public string IdDoUsuario { get; set; }

        /// <summary>
        /// (get) ID do Usuário já convertido pra Int32
        /// </summary>
        public int IdDoUsuarioTipoInt
        {
            get
            {
                return Convert.ToInt32(this.IdDoUsuario);
            }
        }

        public string CodBovespa { get; set; }

        public int CodBovespaTipoInt
        {
            get
            {
                int lCodBMF = 0;

                if (this.CodBovespa != null && int.TryParse(this.CodBovespa.Trim(), out lCodBMF))
                {
                    return lCodBMF;
                }
                else
                {
                    return 0;
                }
            }
        }
        
        public int CodBmf { get; set; }

        public string Nome { get; set; }

        public string PrimeiroNome
        {
            get
            {
                if (this.Nome.Contains(" "))
                {
                    return this.Nome.Substring(0, this.Nome.IndexOf(" "));
                }
                else
                {
                    return this.Nome;
                }
            }
        }

        /// <summary>
        /// Essa propriedade deve estar depreciada; o HB só vai usar CC. Está aqui para refatoramento posterior
        /// </summary>
        public string CodigoDaContaInvestimento
        {
            get
            {
                /*
                if (Session["UserIDCI"] == null)
                {
                    BuscarContaInvestimentoResponse lRespostaContaInvestimento;

                    lRespostaContaInvestimento = ServicoDeCliente.BuscarContaInvestimento(new BuscarContaInvestimentoRequest()
                                                                                          {
                                                                                              CodigoCliente = this.IdDoUsuarioLogadoTipoInt
                                                                                          });

                    if (lRespostaContaInvestimento.Sucesso)
                    {
                        Session["UserIDCI"] = lRespostaContaInvestimento.Resposta;
                    }
                    else
                    {
                        Session["UserIDCI"] = "-1";
                    }
                }

                return Session["UserIDCI"].ToString();
                 */

                return "00000000";
            }
        }

        

        public int ErrosDeBuscarResposta { get; set; }

        public string IdentificadorDeSessao { get; set; }

        #endregion

        #region Métodos Públicos

        public void AssumirIdentificador(string pIP, string pBrowser)
        {
            this.IdentificadorDeSessao = string.Format("{0}-{1}", pIP, pBrowser.Replace(" ", "_"));
        }

        /*
        public TransporteSaldoDeConta BuscarSaldoELimitesNoServicoNovoOMS()
        {
            this.gRetornoLimitePorCliente = new ServicoRegrasRisco().ListarLimitePorClienteNovoOMS(
                new ListarParametrosRiscoClienteRequest
                {
                    CodigoCliente = this.GetCodBovespa,
                        
                });

            var lConsultaSaldo =
                base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoLimiteAlocadoInfo>(
                    new ConsultarEntidadeCadastroRequest<RiscoLimiteAlocadoInfo>(
                        new RiscoLimiteAlocadoInfo()
                        {
                            ConsultaIdCliente = this.GetCodBovespa,
                            NovoOMS = true
                        }));

            this.gDetalhesDoLimite = new TransporteRelatorio_005().TraduzirListaSaldo(lConsultaSaldo.Resultado);
        }
        */
        


        #endregion

        #region Métodos Private

        private bool RequerAtualizacaoDaCustodia()
        {
            //TimeSpan lSpan = new TimeSpan(DateTime.Now.Ticks - gUltimaAtualizacaoCustodia.Ticks);

            //return (_AtivosEmCustodia == null || lSpan.TotalMinutes > 1);

            return true;
        }

        #endregion

        #region Construtores

        public Usuario()
        {
            
        }

        /// <summary>
        /// Construtor da classe do Usuario
        /// </summary>
        /// <param name="pCodigoDaSessao">Sessão obtida na autenticação do cliente.</param>
        public Usuario(string pCodigoDaSessao):this()
        {
            this.CodigoDaSessao = pCodigoDaSessao;
        }

        #endregion
    }
}
