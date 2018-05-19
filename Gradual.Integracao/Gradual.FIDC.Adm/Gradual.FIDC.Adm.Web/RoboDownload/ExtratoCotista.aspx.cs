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
    public partial class ExtratoCotista : PaginaBase
    {
        #region Propriedades

        ///// <summary>
        ///// Data de filtro "DE"
        ///// </summary>
        //public DateTime GetDataDe
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(this.Request["DataDe"]))
        //            return DateTime.Now.AddDays(-30);

        //        return DateTime.Parse(this.Request["DataDe"]);
        //    }
        //}

        ///// <summary>
        ///// Data de filtro "ATE"
        ///// </summary>
        //public DateTime GetDataAte
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(this.Request["DataAte"]))
        //            return DateTime.Now.AddDays(1);

        //        return DateTime.Parse(this.Request["DataAte"]);
        //    }
        //}

        /// <summary>
        /// Data de filtro "data"
        /// </summary>
        public DateTime GetData
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["Data"]))
                    return DateTime.Now;

                return DateTime.ParseExact(this.Request["Data"],"MM/yy", null);
            }
        }


        /// <summary>
        /// Nome do fundo inserido no filtro
        /// </summary>
        public int? GetCodigoCotista
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodigoCotista"], out lRetorno))
                    return null;

                if (lRetorno == 0)
                    return null;

                return lRetorno;
            }
        }
        
        /// <summary>
        /// Código da CpfCnpj do filtro na tela
        /// </summary>
        public string GetCpfCnpj
        {
            get
            {
                return this.Request["CpfCnpj"].ToString();
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

                throw ex;
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
                string lScript = "Grid_Resultado_ExtratoCotista();";

                base.RodarJavascriptOnLoad(lScript);

                lScript = "btnFiltroExtratoCotista_Click();";

                base.RodarJavascriptOnLoad(lScript);

                if (!Page.IsPostBack)
                {
                    base.TituloDaPagina = "Extrato Cotista";
                    base.LinkPreSelecionado = "lnkRobo_ExtratoCotista";

                    var lListaCotista = base.BuscarListaCotistas();

                    this.cboExtratoCotistaNomeCotista.DataSource = lListaCotista.ListaCotista;
                    this.cboExtratoCotistaNomeCotista.DataValueField = "CodigoCotista";
                    this.cboExtratoCotistaNomeCotista.DataTextField = "NomeCotista";
                    this.cboExtratoCotistaNomeCotista.DataBind();

                    this.cboExtratoCotistaNomeCotista.Items.Insert(0, new ListItem("Nome Cotista", "0"));
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de Extrato Cotista na tela", ex);
            }
        }

        /// <summary>
        /// Carregar grid com os dados de Extrato de Cotista via ajax
        /// </summary>
        /// <returns>Retorna string com a lista em Json</returns>
        public string ResponderCarregarHtmlComDados()
        {
            string lRetorno = string.Empty;

            try
            {
                var lRequest = new ExtratoCotistaRequest();
                DateTime data = this.GetData;
                DateTime dataDe = new DateTime(data.Year, data.Month, 1);
                DateTime dataAte = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));


                if (!String.IsNullOrEmpty(this.GetNomeFundo))
                {
                    lRequest.NomeFundo = this.GetNomeFundo;
                }

                if (this.GetCodigoFundo.HasValue)
                {
                    lRequest.CodigoFundo = this.GetCodigoFundo;
                }

                lRequest.CodigoCotista = this.GetCodigoCotista;

                lRequest.CpfCnpj = this.GetCpfCnpj;
                


                lRequest.DataDe = dataDe;

                lRequest.DataAte = dataAte;

                lRequest.DownloadPendentes = this.GetDownloadsPendentes ? 'N' : 'S';

                ExtratoCotistaResponse lResponse = base.BuscarExtratoCotista(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteRoboExtratoCotista().TraduzirLista(lResponse.ListaExtrato);

                    TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada(lListaTransporte);

                    lRetornoLista.TotalDeItens = lResponse.ListaExtrato.Count;

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

        #endregion
    }
}