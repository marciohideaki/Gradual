using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Risco.Lib;
using Gradual.OMS.Risco.Lib.Mensageria;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Spider.PositionClient.DbLib;
using Gradual.Spider.PositionClient.Lib.Messages;
using Gradual.Spider.PostTradingClientEngine.App_Codigo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using appCodigo = Gradual.Spider.PostTradingClientEngine.App_Codigo;
using Gradual.Spider.PositionClient.Monitor.Lib;
using Gradual.Spider.PositionClient.Monitor.Lib.Message;
using Gradual.Spider.PositionClient.Monitor.Lib.Dados;
using Gradual.Spider.PostTradingClientEngine.App_Codigo.TransporteJSon;

namespace Gradual.Spider.PostTradingClientEngine.Backend
{
    /// <summary>
    /// Página para consulta de risco de posção do cliente online 
    /// de Operações Intraday
    /// </summary>
    public partial class OperacoesIntraday : PaginaBase
    {
        #region Atributos

        #endregion

        #region Propriedades
        /// <summary>
        /// Código de cliente para filtro
        /// </summary>
        public int CodigoCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CodigoCliente"], out lRetorno);

                return lRetorno;
            }
        }

        /// <summary>
        /// Código de Instrumento para filtro
        /// </summary>
        public string CodigoInstrumento
        {
            get
            {
                var lRetorno = default(string);

                if (this.Request["CodigoInstrumento"] != null)
                {
                    lRetorno = this.Request["CodigoInstrumento"].ToString();
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Market Todos os Mercados
        /// </summary>
        public bool OpcaoMarketTodosMercados
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoMarketTodosMercados"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoMarketTodosMercados"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Market A Vista
        /// </summary>
        public bool OpcaoMarketAVista
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoMarketAVista"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoMarketAVista"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Market Futuros
        /// </summary>
        public bool OpcaoMarketFuturos
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoMarketFuturos"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoMarketFuturos"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Market Opcao
        /// </summary>
        public bool OpcaoMarketOpcao
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoMarketOpcao"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoMarketOpcao"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Parametro Intraday Ofertas Pedra
        /// </summary>
        public bool OpcaoParametroIntradayOfertasPedra
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoParametroIntradayOfertasPedra"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoParametroIntradayOfertasPedra"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Parametro Intraday Net Negativo
        /// </summary>
        public bool OpcaoParametroIntradayNetNegativo
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoParametroIntradayNetNegativo"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoParametroIntradayNetNegativo"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Parametro Intraday PL Negativo
        /// </summary>
        public bool OpcaoParametroIntradayPLNegativo
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoParametroIntradayPLNegativo"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoParametroIntradayPLNegativo"]);
                }

                return lRetorno;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento de load da pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                RegistrarRespostasAjax(new string[] {
                "CarregarOperacoesCliente"
                }, new ResponderAcaoAjaxDelegate[] 
                {
                    ResponderCarregarOperacoesCliente
                });

                
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método que efetua de busca de operações no banco de dados
        /// </summary>
        /// <returns>Retorna uma string serializado json</returns>
        public string ResponderCarregarOperacoesCliente()
        {
            string lRetorno = string.Empty;

            try
            {
                var lServico = Ativador.Get<IServicoPositionClientMonitor>();

                var lRequest = new BuscarOperacoesIntradayRequest();

                lRequest.CodigoCliente = CodigoCliente;

                lRequest.Ativo = this.CodigoInstrumento;

                if (this.OpcaoMarketTodosMercados)
                {
                    lRequest.OpcaoMarket = OpcaoMarket.TodosMercados;
                }
                else
                {
                    if (this.OpcaoMarketOpcao)
                        lRequest.OpcaoMarket = OpcaoMarket.Opcoes;

                    if (this.OpcaoMarketAVista)
                        lRequest.OpcaoMarket |= OpcaoMarket.Avista;

                    if (this.OpcaoMarketFuturos)
                        lRequest.OpcaoMarket |= OpcaoMarket.Futuros;
                }

                if (this.OpcaoParametroIntradayOfertasPedra)
                {
                    lRequest.OpcaoParametrosIntraday |= OpcaoParametrosIntraday.OfertasPedra;
                }

                if (this.OpcaoParametroIntradayNetNegativo)
                {
                    lRequest.OpcaoParametrosIntraday |= OpcaoParametrosIntraday.NetIntradayNegativo;
                }

                if (this.OpcaoParametroIntradayPLNegativo)
                {
                    lRequest.OpcaoParametrosIntraday |= OpcaoParametrosIntraday.PLNegativo;
                }

                var lResponse = lServico.BuscarOperacoesIntraday(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lTrans = new TransporteOperacoesIntraday(lResponse.ListOperacoesIntraday);

                    lRetorno = RetornarSucessoAjax(lTrans.ListaTransporte, "Foram encontrados [{0}] operacoes" + lTrans.ListaTransporte.Count, lTrans.ListaTransporte.Count);
                }
                else
                {
                    lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao deserializar objeto JSON [{0}]", ex);
            }

            return lRetorno;
        }

        #endregion
    }
}