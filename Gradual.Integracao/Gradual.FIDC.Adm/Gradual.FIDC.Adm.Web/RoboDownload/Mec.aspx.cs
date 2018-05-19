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
    public partial class Mec : PaginaBase
    {
        #region Propriedades
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
                return this.Request["NomeFundo"].ToString();
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

        #region Events
        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderCarregarHtmlComDados
                                                     });

                this.CarregarDadosIniciais();
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de Mapa de evolução de cotas na tela", ex);
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
                string lScript = "Grid_Resultado_Mec();";

                base.RodarJavascriptOnLoad(lScript);

                lScript = "btnFiltroMec_Click();";

                base.RodarJavascriptOnLoad(lScript);

                if (!Page.IsPostBack)
                {
                    base.TituloDaPagina = "Mapa de evolução de cotas";
                    base.LinkPreSelecionado = "lnkRobo_Mec";
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de Mapa de evolução de cotas na tela", ex);
            }
        }

        /// <summary>
        /// Carregar grid com os dados de Mapa de evolução de cotas via ajax
        /// </summary>
        /// <returns>Retorna string com a lista em Json</returns>
        public string ResponderCarregarHtmlComDados()
        {
            string lRetorno = string.Empty;

            try
            {
                var lRequest = new MecRequest();

                lRequest.NomeFundo = this.GetNomeFundo;

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

                MecResponse lResponse = base.BuscarMec(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteRoboMec().TraduzirLista(lResponse.ListaMec);

                    TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada(lListaTransporte);

                    lRetornoLista.TotalDeItens = lResponse.ListaMec.Count;

                    lRetornoLista.PaginaAtual = 1;

                    lRetornoLista.TotalDePaginas = 0;

                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de Mapa de evolução de cotas na tela", ex);

                lRetorno = base.RetornarErroAjax("Erro no método ResponderCarregarHtmlComDados ", ex);
            }

            return lRetorno;
        }

        #endregion
    }
}