using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.Web.App_Codigo.Transporte;
using Newtonsoft.Json;
using System;
using System.Web.UI;

namespace Gradual.FIDC.Adm.Web.TitulosLiquidados
{
    public partial class TitulosLiquidados : PaginaBase
    {
        #region Propriedades
        /// <summary>
        /// Codigo do fundo inserido no fundo
        /// </summary>
        public int? GetCodigoFundoID
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["id"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        /// <summary>
        /// Data de filtro "DE"
        /// </summary>
        public DateTime GetDataDe
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["DataDe"]))
                    return DateTime.Now.AddDays(-30);

                return DateTime.Parse(this.Request["DataDe"]);
            }
        }

        /// <summary>
        /// Data de filtro "ATE"
        /// </summary>
        public DateTime GetDataAte
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["DataAte"]))
                    return DateTime.Now.AddDays(1);

                return DateTime.Parse(this.Request["DataAte"]);
            }
        }
        /// <summary>
        /// Nome do fundo inserido no filtro
        /// </summary>
        public string GetNomeFundo
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Request["NomeFundo"]))
                {
                    return this.Request["NomeFundo"].ToString();
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Codigo do fundo inserido no fundo
        /// </summary>
        public int? GetCodigoFundo
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodigoFundo"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        /// <summary>
        /// Valor do Fundo a ser alterado 
        /// </summary>
        public decimal? GetValorFundo
        {
            get
            {
                var lRetorno = default(decimal);

                if (!decimal.TryParse(this.Request["Valor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }


        #endregion

        #region Events
        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados",
                                                            "AtualizarValor"
                                                     },
                     new ResponderAcaoAjaxDelegate[] {
                                                        this.ResponderCarregarHtmlComDados,
                                                        this.ResponderAtualizarValor
                                                     });

                this.CarregarDadosIniciais();
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de Títulos Liquidados na tela", ex);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Carregar dados iniciais da página de carteiras
        /// </summary>
        private string CarregarDadosIniciais()
        {
            string lRetorno = string.Empty;

            try
            {
                string lScript = "Grid_Resultado_TitulosLiquidadosADM();";

                base.RodarJavascriptOnLoad(lScript);

                lScript = "btnFiltroTitulosLiquidadosADM_Click();";

                base.RodarJavascriptOnLoad(lScript);

                if (!Page.IsPostBack)
                {
                    base.TituloDaPagina = "Títulos Liquidados";
                    base.LinkPreSelecionado = "lnkTL_TitulosLiquidados";
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax(ex.Message, ex);
            }
            return lRetorno;
        }

        /// <summary>
        /// Carregar grid com os dados de Títulos Liquidados via ajax
        /// </summary>
        /// <returns>Retorna string com a lista em Json</returns>
        public string ResponderAtualizarValor()
        {
            string lRetorno = string.Empty;

            try
            {
                var lRequest = new TitulosLiquidadosRequest()
                {
                    CodigoFundo = this.GetCodigoFundoID.Value,
                    ValorLiquidacao = this.GetValorFundo.Value,
                    DataReferencia = DateTime.Now.Date
                };

                TitulosLiquidadosResponse lResponse = base.AplicarValorTitulosLiquidadosADM(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteADMTitulosLiquidados().TraduzirLista(lResponse.ListaTitulos);

                    TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada(lListaTransporte)
                    {
                        TotalDeItens = lResponse.ListaTitulos.Count,
                        PaginaAtual = 1,
                        TotalDePaginas = 0
                    };

                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de Títulos Liquidados na tela", ex);
                lRetorno = base.RetornarErroAjax("Erro no método ResponderCarregarHtmlComDados ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarHtmlComDados()
        {
            string lRetorno = string.Empty;

            try
            {
                var lRequest = new TitulosLiquidadosRequest();

                if (this.GetCodigoFundo.HasValue && this.GetCodigoFundo.Value != 0)
                {
                    lRequest.CodigoFundo = this.GetCodigoFundo.Value;
                }

                lRequest.DataDe = this.GetDataDe;

                lRequest.DataAte = this.GetDataAte;

                TitulosLiquidadosResponse lResponse = base.BuscarTitulosLiquidadosADM(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteADMTitulosLiquidados().TraduzirLista(lResponse.ListaTitulos);

                    TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada(lListaTransporte)
                    {
                        TotalDeItens = lResponse.ListaTitulos.Count,
                        PaginaAtual = 1,
                        TotalDePaginas = 0
                    };

                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de Títulos Liquidados na tela", ex);
                lRetorno = base.RetornarErroAjax("Erro no método ResponderCarregarHtmlComDados ", ex);
            }

            return lRetorno;
        }
        #endregion
    }
}