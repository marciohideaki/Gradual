using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.Web.App_Codigo.Transporte;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.FIDC.Adm.Web.RoboDownload
{
    public partial class Carteiras : PaginaBase
    {
        #region Propriedade
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
                if(!String.IsNullOrEmpty(this.Request["NomeFundo"]))
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
        /// Código da Localidade(Origem) do filtro na tela
        /// </summary>
        public int? GetCodigoLocalidade
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodigoLocalidade"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        /// <summary>
        /// Bolleano de download pendentes
        /// </summary>
        public bool GetDownloadsPendentes
        {
            get
            {
                return bool.Parse(this.Request["DownloadsPendentes"].ToString());
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Carregar dados iniciais da página de carteiras
        /// </summary>
        private void CarregarDadosIniciais()
        {
            try
            {
                string lScript = "Grid_Resultado_Carteiras();";

                base.RodarJavascriptOnLoad(lScript);

                lScript = "btnFiltroCarteiras_Click();";

                base.RodarJavascriptOnLoad(lScript);

                if (!Page.IsPostBack)
                {
                    base.TituloDaPagina = "Carteiras";
                    base.LinkPreSelecionado = "lnkRobo_Carteiras";
                }

                CarregarOrigens();
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de carteira na tela", ex);
            }
        }

        public void CarregarOrigens()
        {
            string lRetorno = string.Empty;

            try
            {
                OrigemResponse lResponse = base.BuscarOrigens();

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteOrigens().TraduzirLista(lResponse.ListaOrigens);

                    this.rptOrigens.DataSource = lResponse.ListaOrigens;
                    this.rptOrigens.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de carteira na tela", ex);

                lRetorno = base.RetornarErroAjax("Erro no método ResponderCarregarHtmlComDados ", ex);
            }
        }

        /// <summary>
        /// Carregar grid com os dados de carteiras via ajax
        /// </summary>
        /// <returns>Retorna string com a lista em Json</returns>
        public string ResponderCarregarHtmlComDados()
        {
            string lRetorno = string.Empty;

            try
            {
                var lRequest= new CarteiraRequest();

                if(!String.IsNullOrEmpty(this.GetNomeFundo))
                {
                    lRequest.NomeFundo = this.GetNomeFundo;
                }

                if (this.GetCodigoFundo.HasValue && this.GetCodigoFundo.Value != 0)
                {
                    lRequest.CodigoFundo = this.GetCodigoFundo.Value;
                }

                lRequest.DataDe = this.GetDataDe;

                lRequest.DataAte = this.GetDataAte;

                if (this.GetCodigoLocalidade != 0)
                {
                    lRequest.CodigoLocalidade = this.GetCodigoLocalidade;
                }

                lRequest.DownloadsPendentes = this.GetDownloadsPendentes ? 'N' : 'S';

                CarteiraResponse lResponse = base.BuscarCarteiras(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteRoboCarteira().TraduzirLista(lResponse.ListaCarteira);

                    TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada(lListaTransporte);

                    lRetornoLista.TotalDeItens = lResponse.ListaCarteira.Count;

                    lRetornoLista.PaginaAtual = 1;

                    lRetornoLista.TotalDePaginas = 0;

                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de carteira na tela", ex);

                lRetorno = base.RetornarErroAjax("Erro no método ResponderCarregarHtmlComDados ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Carregar lista de fundos
        /// </summary>
        /// <returns>Retorna string com a lista de fundos em Json</returns>
        public string BuscarListaDefundos()
        {
            string lRetorno = string.Empty;
            try
            {

                string lNomeParcialFundo = this.GetNomeFundo;
                var lListaFundos = base.BuscarListaFundos();

                var lListaFundosFiltrada = lListaFundos.ListaFundos.Where(f => f.NomeFundo.Contains(lNomeParcialFundo));
                lRetorno = JsonConvert.SerializeObject(lListaFundosFiltrada);
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar lista de fundos.", ex);

                lRetorno = base.RetornarErroAjax("Erro no método BuscarListaDeFundos", ex);
            }

            return lRetorno;
        }

        #endregion

        #region Events
        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                            , "BuscarListaDeFundos"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderCarregarHtmlComDados
                                                       , BuscarListaDefundos
                                                     });

                this.CarregarDadosIniciais();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        #endregion
    }
}