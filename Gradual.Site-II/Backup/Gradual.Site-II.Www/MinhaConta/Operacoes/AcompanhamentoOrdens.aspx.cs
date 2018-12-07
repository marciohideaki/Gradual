using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Info;
using Gradual.OMS.AcompanhamentoOrdens.Lib;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Dados.Enum;
using Microsoft.Reporting.WebForms;
using Gradual.OMS.Email.Lib;
using System.Text;

using System.Web.UI.HtmlControls;
using Gradual.Site.Www.Transporte;


namespace Gradual.Site.Www.MinhaConta.Operacoes
{
    public partial class AcompanhamentoOrdens : PaginaBase
    {
        #region Globais

        private byte gTipoDeOrdemParaBuscar = 0;        // 0 = Online  1 = Historico

        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        //private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Propriedades

        private int CodigoBovespaCliente
        {
            get
            {
                return Convert.ToInt32(base.SessaoClienteLogado.CodigoPrincipal);
            }
        }

        private int CodigoBMFCliente
        {
            get
            {
                return base.SessaoClienteLogado.CodigoBMF;
            }
        }

        private string Filtro_Ativo
        {
            get
            {
                if (txtAtivo.Text != null && txtAtivo.Text.Length > 3)
                {
                    return txtAtivo.Text.ToUpper();
                }
                else
                {
                    return null;
                }
            }
        }

        private string Filtro_AtivoHist
        {
            get
            {
                if (txtAtivoHist.Text != null && txtAtivoHist.Text.Length > 3)
                {
                    return txtAtivoHist.Text.ToUpper();
                }
                else
                {
                    return null;
                }
            }
        }

        private string Filtro_Direcao
        {
            get
            {
                return cboDirecao.SelectedValue.ToUpper();
            }
        }

        private string Filtro_DirecaoHist
        {
            get
            {
                return cboDirecaoHist.SelectedValue.ToUpper();
            }
        }

        private string Filtro_SituacaoHist
        {
            get
            {
                return cboSituacaoHist.SelectedValue.ToUpper();
            }
        }

        private string Filtro_Situacao
        {
            get
            {
                return cboSituacao.SelectedValue.ToUpper();
            }
        }

        private Nullable<DateTime> Filtro_DataDe
        {
            get
            {
                DateTime lData;

                if (DateTime.TryParseExact(txtDataDe.Text, new string[] { "dd/MM/yyyy HH:mm:ss", "d/M/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm", "dd/MM/yyyy", "dd/MM" }, gCultureInfo, DateTimeStyles.None, out lData))
                {
                    return lData;
                }
                else
                {
                    return null;
                }
            }
        }

        private Nullable<DateTime> Filtro_DataAte
        {
            get
            {
                DateTime lData;

                if (DateTime.TryParseExact(txtDataAte.Text, new string[] { "dd/MM/yyyy HH:mm:ss", "d/M/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm", "dd/MM/yyyy", "dd/MM" }, gCultureInfo, DateTimeStyles.None, out lData))
                {
                    if (lData.Hour == 0 && lData.Minute == 0 && lData.Second == 0)
                    {
                        lData = new DateTime(lData.Year, lData.Month, lData.Day, 23, 59, 59);
                    }

                    return lData;
                }
                else
                {
                    return null;
                }
            }
        }

        private Nullable<DateTime> Filtro_HoraDe
        {
            get
            {
                DateTime lData;

                if (DateTime.TryParseExact(txtHoraDe.Text, new string[] { "HH:mm", "HH:mm:ss" }, gCultureInfo, DateTimeStyles.None, out lData))
                {
                    return lData;
                }
                else
                {
                    return null;
                }
            }
        }

        private Nullable<DateTime> Filtro_HoraAte
        {
            get
            {
                DateTime lData;

                if (DateTime.TryParseExact(txtHoraAte.Text, new string[] { "HH:mm", "HH:mm:ss" }, gCultureInfo, DateTimeStyles.None, out lData))
                {
                    return lData;
                }
                else
                {
                    return null;
                }
            }
        }

        public string Ordenacao_Campo { get; set; }

        public string Ordenacao_Direcao { get; set; }

        private string _ClOrdId;

        public string ClOrdId
        {
            get { return _ClOrdId; }
            set { _ClOrdId = value; }
        }

        #endregion

        #region Métodos Private

        private void ExibirTextboxDeData(bool pComoData)
        {
            //pnlDataDe.Visible = pComoData;
            //pnlDataAte.Visible = pComoData;

            //pnlHoraDe.Visible = !pComoData;
            //pnlHoraAte.Visible = !pComoData;
        }

        private void CarregarDados()
        {
            ViewState["gTipoDeOrdemParaBuscar"] = gTipoDeOrdemParaBuscar;

            if (gTipoDeOrdemParaBuscar == 0)
            {
                //lnkAba_OrdensOnline.CssClass = "Selecionado";
                //lnkAba_OrdensHistorico.CssClass = "";
                //lnkAba_OrdensStopStartHistorico.CssClass = "";

                //pnlOrdens.Visible = true;
                //pnlOrdensStopStart.Visible = false;

                BuscarOrdensOnline();
            }
            else if (gTipoDeOrdemParaBuscar == 1)
            {
                //lnkAba_OrdensOnline.CssClass = "";
                //lnkAba_OrdensHistorico.CssClass = "Selecionado";
                //lnkAba_OrdensStopStartHistorico.CssClass = "";

                //pnlOrdens.Visible = true;
                //pnlOrdensStopStart.Visible = false;

                BuscarOrdensHistorico();
            }

            ExibirAvisoDeFiltro();
        }

        private List<Transporte_Ordens> BuscarOrdensOnline()
        {
            IServicoAcompanhamentoOrdens lServicoAcompanhamento = InstanciarServicoDoAtivador<IServicoAcompanhamentoOrdens>();

            VerificarStatusDasOrdensRequest lRequest;
            VerificarStatusDasOrdensResponse lResponse;

            lRequest = new VerificarStatusDasOrdensRequest();

            lRequest.ContaDoCliente = this.CodigoBovespaCliente;
            lRequest.CodigoBmfDoCliente = this.CodigoBMFCliente;

            List<Transporte_Ordens> lRetorno = null;

            try
            {
                lResponse = lServicoAcompanhamento.VerificarStatusDasOrdens(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    List<OrdemInfo> lListaOrdenada = lResponse.Ordens.OrderByDescending(ordem => ordem.RegisterTime).ToList();

                    IEnumerable<OrdemInfo> lListaFiltrada = lListaOrdenada;

                    EfetuaFiltroOrdensDataAtivo(ref lListaFiltrada);

                    EfetuaFiltroOrdens(ref lListaFiltrada);

                    OrdenarOrdens(ref lListaFiltrada);

                    rptOrdens.DataSource = lListaFiltrada;

                    lRetorno = new Transporte_Ordens().TraduzirList(  lListaFiltrada.ToList());
                    //rptOrdens.ItemDataBound += new RepeaterItemEventHandler(rptOrdens_ItemDataBound);

                    rptOrdens.DataBind();

                    trNenhumItemOrdens.Visible = lListaFiltrada.Count() == 0;
                }
                else
                {
                    gLogger.ErrorFormat("Erro do serviço em BuscarOrdensHistorico(): [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);

                    ExibirMensagemJsOnLoad("E", string.Format("Erro ao realizar busca: [{0}]\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta), false);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro inesperado em BuscarOrdensOnline(): [{0}] [{1}]", ex.Message, ex.StackTrace);

                ExibirMensagemJsOnLoad("E", "Erro ao realizar busca; é possível que existam muitos resultados, favor restringir os filtros de busca e tentar novamente.");
            }

            return lRetorno;
        }

        private List<Transporte_Ordens> BuscarOrdensHistorico()
        {
            IServicoAcompanhamentoOrdens lServicoAcompanhamento = InstanciarServicoDoAtivador<IServicoAcompanhamentoOrdens>();

            BuscarOrdensRequest lRequest = new BuscarOrdensRequest();
            BuscarOrdensResponse lResponse;

            lRequest.ContaDoCliente = this.CodigoBovespaCliente;
            lRequest.CodigoBmfDoCliente = this.CodigoBMFCliente;

            lRequest.DataDe      = this.Filtro_DataDe;
            lRequest.DataAte     = this.Filtro_DataAte;
            lRequest.Instrumento = this.Filtro_AtivoHist;

            List<Transporte_Ordens> lRetorno = null;

            try
            {
                lResponse = lServicoAcompanhamento.BuscarOrdens(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    IEnumerable<OrdemInfo> lListaFiltrada = lResponse.Ordens;

                    EfetuaFiltroOrdensHist(ref lListaFiltrada);

                    OrdenarOrdens(ref lListaFiltrada);

                    rptOrdensHistorica.DataSource = lListaFiltrada;

                    lRetorno = new Transporte_Ordens().TraduzirList( lListaFiltrada.ToList());

                    //rptOrdensHistorica.ItemDataBound += new RepeaterItemEventHandler(rptOrdens_ItemDataBound);

                    rptOrdensHistorica.DataBind();

                    trNenhumItemOrdensHistorico.Visible = lListaFiltrada.Count() == 0;
                }
                else
                {
                    gLogger.ErrorFormat("Erro do serviço em BuscarOrdensHistorico(): [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);

                    ExibirMensagemJsOnLoad("E", string.Format("Erro ao realizar busca: [{0}]\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta), false);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro inesperado em BuscarOrdensHistorico(): [{0}] [{1}]", ex.Message, ex.StackTrace);

                ExibirMensagemJsOnLoad("E", "Erro ao realizar busca; é possível que existam muitos resultados, favor restringir os filtros de busca e tentar novamente.", false);
            }

            return lRetorno;
        }

        private void EfetuaFiltroOrdensDataAtivo(ref IEnumerable<OrdemInfo> pLista)
        {
            if (pLista.Count() == 0)
                return;

            if (!string.IsNullOrEmpty(this.Filtro_Ativo))
            {
                pLista = from a in pLista where a.Symbol == this.Filtro_Ativo select a;
            }

            if (this.Filtro_HoraDe.HasValue)
            {
                pLista = from a in pLista where a.RegisterTime >= this.Filtro_HoraDe select a;
            }

            if (this.Filtro_HoraAte.HasValue)
            {
                pLista = from a in pLista where a.RegisterTime <= this.Filtro_HoraAte select a;
            }
        }

        private void EfetuaFiltroOrdens(ref IEnumerable<OrdemInfo> pLista)
        {
            if (pLista.Count() == 0)
                return;

            if (!string.IsNullOrEmpty(this.Filtro_Situacao))
            {
                OrdemStatusEnum OrdStatus = BuscarIdDoStatusOrdem(this.Filtro_Situacao);

                pLista = from a in pLista where a.OrdStatus == OrdStatus select a;
            }

            if (!string.IsNullOrEmpty(this.Filtro_Direcao))
            {
                if (this.Filtro_Direcao == "COMPRA")
                {
                    pLista = from a in pLista where a.Side == OrdemDirecaoEnum.Compra select a;
                }
                else
                {
                    pLista = from a in pLista where a.Side == OrdemDirecaoEnum.Venda select a;
                }
            }
        }

        private void EfetuaFiltroOrdensHist(ref IEnumerable<OrdemInfo> pLista)
        {
            if (pLista.Count() == 0)
                return;

            if (!string.IsNullOrEmpty(this.Filtro_SituacaoHist))
            {
                OrdemStatusEnum OrdStatus = BuscarIdDoStatusOrdem(this.Filtro_SituacaoHist);

                pLista = from a in pLista where a.OrdStatus == OrdStatus select a;
            }

            if (!string.IsNullOrEmpty(this.Filtro_DirecaoHist))
            {
                if (this.Filtro_DirecaoHist == "COMPRA")
                {
                    pLista = from a in pLista where a.Side == OrdemDirecaoEnum.Compra select a;
                }
                else
                {
                    pLista = from a in pLista where a.Side == OrdemDirecaoEnum.Venda select a;
                }
            }
        }

        private void EfetuaFiltroOrdensStopStart(ref IEnumerable<OrdemStopStartInfo> pLista)
        {
            if (pLista.Count() == 0)
                return;

            IEnumerable<OrdemStopStartInfo> lListaFiltrada = pLista;

            if (!string.IsNullOrEmpty(this.Filtro_Situacao))
            {
                List<int> lListaDosStatus = BuscarIDsDoStatus(this.Filtro_Situacao);

                lListaFiltrada = from a in lListaFiltrada where lListaDosStatus.Contains(a.StopStartStatusID) select a;
            }
            if (!string.IsNullOrEmpty(this.Filtro_Direcao))
            {
                if (this.Filtro_Direcao.ToLower() == "COMPRA")
                    lListaFiltrada = from a in lListaFiltrada where a.IdStopStartTipo == StopStartTipoEnum.StartCompra select a;
                else
                    lListaFiltrada = from a in lListaFiltrada where a.IdStopStartTipo != StopStartTipoEnum.StartCompra select a;
            }

            pLista = lListaFiltrada;
        }

        private void OrdenarOrdens(ref IEnumerable<OrdemInfo> pLista)
        {
            switch (this.Ordenacao_Campo)
            {
                case "ORDEM":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.ClOrdID);
                    else
                        pLista = pLista.OrderByDescending(x => x.ClOrdID);

                    break;

                case "ATIVO":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.Symbol);
                    else
                        pLista = pLista.OrderByDescending(x => x.Symbol);

                    break;

                case "DIRECAO":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.Side);
                    else
                        pLista = pLista.OrderByDescending(x => x.Side);

                    break;

                case "SOLICITADO":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.OrderQty);
                    else
                        pLista = pLista.OrderByDescending(x => x.OrderQty);

                    break;

                case "EXECUTADO":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.CumQty);
                    else
                        pLista = pLista.OrderByDescending(x => x.CumQty);

                    break;

                case "PRECO":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.Price);
                    else
                        pLista = pLista.OrderByDescending(x => x.Price);

                    break;

                case "SITUACAO":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.OrdStatus);
                    else
                        pLista = pLista.OrderByDescending(x => x.OrdStatus);

                    break;

                case "ENVIO":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.RegisterTime);
                    else
                        pLista = pLista.OrderByDescending(x => x.RegisterTime);

                    break;

                case "ULTIMA":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.TransactTime);
                    else
                        pLista = pLista.OrderByDescending(x => x.TransactTime);

                    break;

                case "VALIDADE":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.ExpireDate);
                    else
                        pLista = pLista.OrderByDescending(x => x.ExpireDate);

                    break;
            }
        }

        private void OrdenarOrdensStopStart(ref IEnumerable<TransporteStartStop> pLista)
        {
            switch (this.Ordenacao_Campo)
            {
                case "CODIGO":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.Codigo);
                    else
                        pLista = pLista.OrderByDescending(x => x.Codigo);

                    break;


                case "CODIGODOPAPEL":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.CodigoDoPapel);
                    else
                        pLista = pLista.OrderByDescending(x => x.CodigoDoPapel);

                    break;


                case "QUANTIDADE":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.Quantidade);
                    else
                        pLista = pLista.OrderByDescending(x => x.Quantidade);

                    break;


                case "PRECODEDISPARO":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.PrecoDeDisparo);
                    else
                        pLista = pLista.OrderByDescending(x => x.PrecoDeDisparo);

                    break;


                case "PRECOLIMITE":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.PrecoLimite);
                    else
                        pLista = pLista.OrderByDescending(x => x.PrecoLimite);

                    break;


                case "INICIOMOVEL":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.InicioMovel);
                    else
                        pLista = pLista.OrderByDescending(x => x.InicioMovel);

                    break;


                case "AJUSTEMOVEL":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.AjusteMovel);
                    else
                        pLista = pLista.OrderByDescending(x => x.AjusteMovel);

                    break;


                case "ENVIADO":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.Enviado);
                    else
                        pLista = pLista.OrderByDescending(x => x.Enviado);

                    break;


                case "CANCELADO":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.Cancelado);
                    else
                        pLista = pLista.OrderByDescending(x => x.Cancelado);

                    break;


                case "STATUS":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.Status);
                    else
                        pLista = pLista.OrderByDescending(x => x.Status);

                    break;


                case "LOSSOUGAIN":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.LossOuGain);
                    else
                        pLista = pLista.OrderByDescending(x => x.LossOuGain);

                    break;


                case "DATA":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.Data);
                    else
                        pLista = pLista.OrderByDescending(x => x.LossOuGain);

                    break;


                case "DATADEVALIDADE":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.DataDeValidade);
                    else
                        pLista = pLista.OrderByDescending(x => x.DataDeValidade);

                    break;


                case "DATACANCELADAOUENVIADA":

                    if (this.Ordenacao_Direcao == "ASC")
                        pLista = pLista.OrderBy(x => x.DataCanceladaOuEnviada);
                    else
                        pLista = pLista.OrderByDescending(x => x.DataCanceladaOuEnviada);

                    break;

            }
        }

        private void ExibirAvisoDeFiltro()
        {
            string lAvisoDeFiltro = "";

            if (!string.IsNullOrEmpty(this.Filtro_Ativo))
            {
                lAvisoDeFiltro += string.Format("Ativo: [{0}]   ", this.Filtro_Ativo);
            }

            if (!string.IsNullOrEmpty(this.Filtro_Direcao))
            {
                lAvisoDeFiltro += string.Format("Direção: [{0}]   ", this.Filtro_Direcao);
            }

            if (!string.IsNullOrEmpty(this.Filtro_Situacao))
            {
                lAvisoDeFiltro += string.Format("Situação: [{0}]   ", this.Filtro_Situacao);
            }

            if (this.Filtro_DataDe.HasValue)
            {
                lAvisoDeFiltro += string.Format("Data De: [{0}]   ", this.Filtro_DataDe);
            }

            if (this.Filtro_DataAte.HasValue)
            {
                lAvisoDeFiltro += string.Format("Data Até: [{0}]   ", this.Filtro_DataAte);
            }

            //if (string.IsNullOrEmpty(lAvisoDeFiltro))
            //{
            //    pnlFiltro.Visible = false;
            //}
            //else
            //{
            //    pnlFiltro.Visible = true;

            //    lblFiltro.Text = string.Format("Filtro(s): {0}", lAvisoDeFiltro.Replace(" ", "&nbsp;"));
            //}
        }

        private OrdemStatusEnum BuscarIdDoStatusOrdem(string pSituacao)
        {
            OrdemStatusEnum lRetorno = OrdemStatusEnum.CANCELADA;

            switch (pSituacao)
            {
                case "NOVA":
                    lRetorno = OrdemStatusEnum.NOVA;
                    break;

                case "PARCIALMENTEEXECUTADA":
                    lRetorno = OrdemStatusEnum.PARCIALMENTEEXECUTADA;
                    break;

                case "EXECUTADA":
                    lRetorno = OrdemStatusEnum.EXECUTADA;
                    break;

                case "CANCELADA":
                    lRetorno = OrdemStatusEnum.CANCELADA;
                    break;

                case "SUBSTITUIDA":
                    lRetorno = OrdemStatusEnum.SUBSTITUIDA;
                    break;

                case "REJEITADA":
                    lRetorno = OrdemStatusEnum.REJEITADA;
                    break;

                case "SUSPENSA":
                    lRetorno = OrdemStatusEnum.SUSPENSA;
                    break;

                case "PEDENTE":
                    lRetorno = OrdemStatusEnum.PENDENTE;
                    break;
            }

            return lRetorno;
        }

        private List<int> BuscarIDsDoStatus(string pStatus)
        {
            List<int> lRetorno = new List<int>();

            switch (pStatus)
            {
                case "TENTANDOENVIAR":
                    lRetorno = new List<int> { 1, 2 };
                    break;

                case "ABERTA":
                    lRetorno = new List<int> { 3 };
                    break;

                case "REJEITADA":
                    lRetorno = new List<int> { 4 };
                    break;

                case "CANCELADA":
                    lRetorno = new List<int> { 6, 7, 8, 12 };
                    break;

                case "CANCELAMENTOREJEITADO":
                    lRetorno = new List<int> { 9 };
                    break;

                case "ENVIADA":
                    lRetorno = new List<int> { 5, 10 };
                    break;

                case "EXECUTADA":
                    lRetorno = new List<int> { 11 };
                    break;

                case "EXPIRADA":
                    lRetorno = new List<int> { 13 };
                    break;
            }

            return lRetorno;
        }

        private byte[] GerarRelatorio(string pCaminhoRelatorio,
            string pCaminhoDoArquivo,
            out string pMymeType)
        {
            string lNome = base.SessaoClienteLogado.Nome;

            LocalReport lReport = new LocalReport();

            lReport.ReportPath = pCaminhoRelatorio;

            List<Transporte_Ordens> lHistorico = this.BuscarOrdensHistorico();

            List<Transporte_Ordens> lOnline = this.BuscarOrdensOnline();

            List<ReportParameter> lParametros = new List<ReportParameter>();

            ReportParameter lParamData           = new ReportParameter("pData", DateTime.Now.ToString("dd/MM/yyyy"));
            lParametros.Add(lParamData);

            ReportParameter lParamCliente        = new ReportParameter("pCliente", base.SessaoClienteLogado.CodigoPrincipal + "-" + base.SessaoClienteLogado.Nome);
            lParametros.Add(lParamCliente);

            ReportParameter lParamCpfCnpjCliente = new ReportParameter("pCpfCnpj", base.SessaoClienteLogado.CpfCnpj);
            lParametros.Add(lParamCpfCnpjCliente);

            ReportDataSource lSourceOnline    = new ReportDataSource("EAcompanhamento", lOnline);

            ReportDataSource lSourceHistorico = new ReportDataSource("EAcompanhamentoHistorico", lHistorico);

            lReport.DataSources.Add(lSourceOnline);

            lReport.DataSources.Add(lSourceHistorico);

            lReport.SetParameters(lParametros);

            string lReportType, lEncoding, lFileNameExtension, lFileName, lDeviceInfo;

            Warning[] lWarnings;
            string[] lStreams;
            byte[] lRenderedBytes;

            lReportType = "PDF";
            lFileName = pCaminhoDoArquivo;

            lDeviceInfo =
            "<DeviceInfo> <OutputFormat>PDF</OutputFormat> <PageWidth>10in</PageWidth> <PageHeight>11in</PageHeight> <MarginTop>0.5in</MarginTop> <MarginLeft>0.5in</MarginLeft> <MarginRight>0.5in</MarginRight> <MarginBottom>0.5in</MarginBottom> </DeviceInfo>";

            //Render the report
            lRenderedBytes = lReport.Render(lReportType, lDeviceInfo, out pMymeType, out lEncoding, out lFileNameExtension, out lStreams, out lWarnings);

            return lRenderedBytes;
        }

        public string RetornarStatusTd(string pOrderStatus)
        {
            string lRetorno = string.Empty;

            switch (pOrderStatus.ToUpper())
            {
                case "NOVA":
                    lRetorno = "A ordem está na bolsa aguardando execução";
                    break;

                case "PARCIALMENTEEXECUTADA":
                    lRetorno = "Houve execução de parte da quantidade enviada, a quantidade restante permanece aguardando execução";
                    break;

                case "EXECUTADA":
                    lRetorno = "Ordem executada na bolsa";
                    break;

                case "CANCELADA":
                    lRetorno = "Ordem cancelada pelo cliente, pela área de risco da Corretora ou pela bolsa";
                    break;

                case "SUBSTITUIDA":
                    lRetorno = "Ordem enviada originalmente foi substituída e aguarda execução";
                    break;

                case "REJEITADA":
                    lRetorno = "Ordem rejeitada pela Bolsa de Valores";
                    break;

                case "EXPIRADA":
                    lRetorno = "Ordem com data de validade expirada";
                    break;
                default:
                    lRetorno = "";
                    break;
            }

            return lRetorno;
        }

        public string RetornaCorTr(string pOrderStatus)
        {
            string lRetorno = string.Empty;

            switch (pOrderStatus.ToUpper())
            {
                case "NOVA":
                    lRetorno = "tr_verdeclaro";
                    break;

                case "PARCIALMENTEEXECUTADA":
                    lRetorno = "tr_verdeclaro";
                    break;

                case "EXECUTADA":
                    lRetorno = "tr_verdeescuro";
                    break;

                case "CANCELADA":
                    lRetorno = "tr_vermelhoclaro";
                    break;

                case "SUBSTITUIDA":
                    lRetorno = "tr_verdeclaro";
                    break;

                case "REJEITADA":
                    lRetorno = "tr_vermelhoclaro";
                    break;

                case "EXPIRADA":
                    lRetorno = "tr_vermelhoclaro";
                    break;
                default:
                    lRetorno = "tr_cinzaclaro";
                    break;
            }

            return lRetorno;
        }
        #endregion

        #region Eventos
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.ValidarSessao())
            {
                if (!this.IsPostBack)
                {
                    if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                    {
                        base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                        return;
                    }

                    this.txtDataDe.Text = DateTime.Now.AddDays(-5).ToString("dd/MM/yyyy");
                    this.txtDataAte.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    this.txtHoraDe.Text = "00:00";
                    this.txtHoraAte.Text = "23:59";

                    this.BuscarOrdensOnline();
                }
                else
                {
                    if (this.Operacoes_Aba_selecionada.Value == "AbaOrdensHistorico" || this.Operacoes_Aba_selecionada.Value == "AbaOrdensOnline")
                    {
                        RodarJavascriptOnLoad("MinhaConta_Operacoes_Acompanhamento_Click('" + this.Operacoes_Aba_selecionada.Value + "');");
                    }
                }
            }
        }

        protected void lnkAba_OrdensOnline_Click(object sender, EventArgs e)
        {
            gTipoDeOrdemParaBuscar = 0;

            ExibirTextboxDeData(false);

            CarregarDados();
        }

        protected void lnkAba_OrdensHistorico_Click(object sender, EventArgs e)
        {
            gTipoDeOrdemParaBuscar = 1;

            ExibirTextboxDeData(true);

            if (string.IsNullOrEmpty(txtDataDe.Text))
                txtDataDe.Text = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy");

            if (string.IsNullOrEmpty(txtDataAte.Text))
                txtDataAte.Text = DateTime.Now.ToString("dd/MM/yyyy");

            CarregarDados();
        }

        public void btnBuscar_Click(object sender, EventArgs e)
        {
            this.BuscarOrdensOnline();
        }

        public void btnConsultar_Click(object sender, EventArgs e)
        {
            this.BuscarOrdensHistorico();
        }

        protected void rptOrdens_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater lrptAcompanhamentos;

            lrptAcompanhamentos = (Repeater)e.Item.FindControl("rptAcompanhamentos");

            lrptAcompanhamentos.DataSource = ((OrdemInfo)e.Item.DataItem).Acompanhamentos;

            lrptAcompanhamentos.DataBind();

            string lStatus = string.Format("{0}", ((OrdemInfo)e.Item.DataItem).OrdStatus).ToUpper();

            if (lStatus == "NOVA" || lStatus == "ABERTA" || lStatus == "SUBSTITUIDA")
            {
                Literal lblBotaoCancelar;

                lblBotaoCancelar = (Literal)e.Item.FindControl("lblBotaoCancelar");

                lblBotaoCancelar.Text = string.Format("<button class='IconCancelar' title='Cancelar Ordem' onclick='return AcompanhamentoDeOrdem_CancelarOrdem(this, \"{0}\", \"{1}\", \"{2}\")'></button>"
                                                      , ((OrdemInfo)e.Item.DataItem).IdOrdem
                                                      //, ((OrdemInfo)e.Item.DataItem).OrigClOrdID == null ? ((OrdemInfo)e.Item.DataItem).ClOrdID : ((OrdemInfo)e.Item.DataItem).OrigClOrdID
                                                      ,((OrdemInfo)e.Item.DataItem).ClOrdID
                                                      , ((OrdemInfo)e.Item.DataItem).Symbol);


                Button btnBotaoAlterar;

                btnBotaoAlterar = (Button)e.Item.FindControl("btnBotaoAlterar");

                btnBotaoAlterar.Visible = true;

                string lOrdemId = string.IsNullOrEmpty(((OrdemInfo)e.Item.DataItem).OrigClOrdID) ?
                    ((OrdemInfo)e.Item.DataItem).ClOrdID :
                    ((OrdemInfo)e.Item.DataItem).OrigClOrdID;

                //string lClOrdId = ((OrdemInfo)e.Item.DataItem).ClOrdID;

                btnBotaoAlterar.CommandArgument = lOrdemId;
            }

            HtmlTableCell lblTdOrdem = (HtmlTableCell)e.Item.FindControl("td_ordemonline_status");

            string lStatusTd = this.RetornarStatusTd(lStatus);

            lblTdOrdem.Attributes.Add("title", lStatusTd);

            HtmlTableRow lblTrOrdem = (HtmlTableRow)e.Item.FindControl("tr_OrdemOnline");

            string lCorTR = this.RetornaCorTr(lStatus);

            lblTrOrdem.Attributes.Add("class", "tabela-type-very-small " + lCorTR);
        }

        protected void rptOrdensHistorica_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater lrptAcompanhamentos;

            lrptAcompanhamentos = (Repeater)e.Item.FindControl("rptAcompanhamentosHistorico");

            lrptAcompanhamentos.DataSource = ((OrdemInfo)e.Item.DataItem).Acompanhamentos;

            lrptAcompanhamentos.DataBind();

            string lStatus = string.Format("{0}", ((OrdemInfo)e.Item.DataItem).OrdStatus).ToUpper();

            HtmlTableCell lblTdOrdem = (HtmlTableCell)e.Item.FindControl("td_ordemhistorico_status");

            string lStatusTd = this.RetornarStatusTd(lStatus);

            lblTdOrdem.Attributes.Add("title", lStatusTd);


            HtmlTableRow lblTrOrdem = (HtmlTableRow)e.Item.FindControl("tr_OrdemHistorica");

            string lCorTR = this.RetornaCorTr(lStatus);

            lblTrOrdem.Attributes.Add("class", "tabela-type-very-small " + lCorTR);

        }

        protected void btnBotaoAlterar_Click(object sender, CommandEventArgs e)
        {
            this.ClOrdId = e.CommandArgument.ToString();

            Server.Transfer("Operacoes.aspx");
        }

        protected void lnkHeader_Click(object sender, EventArgs e)
        {
            string lCampo = ((LinkButton)sender).ID;

            if (lCampo.Contains("_"))
            {
                lCampo = lCampo.Substring(lCampo.IndexOf("_") + 1);

                this.Ordenacao_Campo = lCampo.ToUpper();

                if (ViewState["Ordenacao_Direcao"] == null)
                    this.Ordenacao_Direcao = "DESC";
                else
                    this.Ordenacao_Direcao = ViewState["Ordenacao_Direcao"].ToString();

                this.Ordenacao_Direcao = (this.Ordenacao_Direcao == "ASC") ? "DESC" : "ASC";

                ViewState["Ordenacao_Direcao"] = this.Ordenacao_Direcao;

                BuscarOrdensOnline();
                //CarregarDados();
            }
        }

        protected void lnkHeaderHist_Click(object sender, EventArgs e)
        {
            string lCampo = ((LinkButton)sender).ID;

            if (lCampo.Contains("_"))
            {
                lCampo = lCampo.Substring(lCampo.IndexOf("_") + 1);

                this.Ordenacao_Campo = lCampo.ToUpper();

                if (ViewState["Ordenacao_Direcao"] == null)
                    this.Ordenacao_Direcao = "DESC";
                else
                    this.Ordenacao_Direcao = ViewState["Ordenacao_Direcao"].ToString();

                this.Ordenacao_Direcao = (this.Ordenacao_Direcao == "ASC") ? "DESC" : "ASC";

                ViewState["Ordenacao_Direcao"] = this.Ordenacao_Direcao;

                BuscarOrdensHistorico();
            }
        }

        protected void btnImprimirPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string lNomeDoArquivo = string.Format("Ordens_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                    Server.MapPath(@"..\Reports\AcompanhamentoOrdens.rdlc"),
                    lNomeDoArquivo,
                    out lMimeType);

                //Clear the response stream and write the bytes to the outputstream  //Set content-disposition to "attachment" so that user is prompted to take an action  //on the file (open or save)
                Response.Clear();
                Response.ContentType = lMimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + lNomeDoArquivo + ".pdf");
                Response.BinaryWrite(lRenderedBytes);
                Response.End();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar o PDF de Acompanhamento de ordens.");
            }
        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                string lNomeDoArquivo = string.Format("Ordens_{0}_{1}", base.SessaoClienteLogado.CodigoPrincipal, DateTime.Now.ToString("yyyyMMddHHmmss"));

                string lMimeType;

                byte[] lRenderedBytes = this.GerarRelatorio(
                    Server.MapPath(@"..\Reports\AcompanhamentoOrdens.rdlc"),
                    lNomeDoArquivo,
                    out lMimeType);

                var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();
                EmailAnexoInfo lEmailInfo = new EmailAnexoInfo();
                lEmailInfo.Arquivo = lRenderedBytes;
                lEmailInfo.Nome = string.Concat(lNomeDoArquivo, ".pdf");
                lAnexos.Add(lEmailInfo);

                Dictionary<string, string> lVariaveis = new Dictionary<string, string>();
                //base.EnviarEmail("bribeiro@gradualinvestimentos.com.br", "Acompanhamento de ordens - Gradual Investimentos", "AcompanhamentoOrdens.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);
                base.EnviarEmail(base.SessaoClienteLogado.Email, "Acompanhamento de ordens - Gradual Investimentos", "AcompanhamentoOrdens.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar o PDF de Acompanhamento de ordens.");
            }
        }

        protected void btnImprimirExcel_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder lBuilder = new StringBuilder();

                List<Transporte_Ordens> lHistorico = this.BuscarOrdensHistorico();

                List<Transporte_Ordens> lOnline = this.BuscarOrdensOnline();

                lBuilder.AppendLine("Ordens Online\r\n");

                lBuilder.AppendLine("Ordem\tAtivo\tDireção\tSolicitado\tExecutado\tPreço\tPreço stop/start\tSituação\tEnvio\tÚlt.Atualização\tValidade\t\r\n");

                foreach (Transporte_Ordens lOrdem in lOnline)
                {
                    lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t\r\n",
                        lOrdem.ClOrdID,
                        lOrdem.Symbol,
                        lOrdem.Side,
                        lOrdem.OrderQty,
                        lOrdem.CumQty,
                        lOrdem.Price,
                        lOrdem.StopPrice,
                        lOrdem.OrdStatus,
                        lOrdem.RegisterTime,
                        lOrdem.TransactTime,
                        lOrdem.ExpireDate
                        );
                }

                lBuilder.AppendLine("Ordens Historico\r\n");

                lBuilder.AppendLine("Ordem\tAtivo\tDireção\tSolicitado\tExecutado\tPreço\tPreço stop/start\tSituação\tEnvio\tÚlt.Atualização\tValidade\t\r\n");

                foreach (Transporte_Ordens lOrdem in lHistorico)
                {
                    lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t\r\n",
                        lOrdem.ClOrdID,
                        lOrdem.Symbol,
                        lOrdem.Side,
                        lOrdem.OrderQty,
                        lOrdem.CumQty,
                        lOrdem.Price,
                        lOrdem.StopPrice,
                        lOrdem.OrdStatus,
                        lOrdem.RegisterTime,
                        lOrdem.TransactTime,
                        lOrdem.ExpireDate
                        );
                }

                Response.Clear();
                Response.ContentType = "text/xls";
                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
                Response.Charset = "iso-8859-1";
                Response.AddHeader("content-disposition", "attachment;filename=AcompanhamentoOrdens.xls");
                Response.Write(lBuilder.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar o Excell de Acompanhamento de ordens.");
            }
        }
        #endregion
    }
}