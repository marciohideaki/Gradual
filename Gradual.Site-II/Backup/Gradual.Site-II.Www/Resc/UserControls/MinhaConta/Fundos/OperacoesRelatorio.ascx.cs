using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;

using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;
using Gradual.Site.Www.Transporte;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos
{
    public partial class OperacoesRelatorio : System.Web.UI.UserControl
    {
        #region Propriedades
        public List<Transporte_PosicaoCotista> PosicaoCotista
        {
            get 
            { 
                return ViewState["PosicaoCotista"] as List<Transporte_PosicaoCotista>; 
            }

            set
            {
                ViewState["PosicaoCotista"] = value;
            }

        }
        #endregion
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var lBase = (PaginaBase)this.Page;

                if (string.IsNullOrEmpty(lBase.SessaoClienteLogado.CodigoPrincipal))
                {
                    if (!lBase.JavascriptParaRodarOnLoad.Contains("Você ainda não possui código de conta Gradual"))
                    {
                        lBase.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                    }
                    return;
                }
                

                this.CarregaDados();
            }
        }

        protected void btnBuscarSolicitacoes_Click(object sender, EventArgs e)
        {
            var lBase = (PaginaFundos)this.Page;

            if (string.IsNullOrEmpty(lBase.SessaoClienteLogado.CodigoPrincipal))
            {
                if (!lBase.JavascriptParaRodarOnLoad.Contains("Você ainda não possui código de conta Gradual"))
                {
                    lBase.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                }
                return;
            }


            var lRequest = new PesquisarMovimentoOperacoesIntegracaoFundosRequest();

            lRequest.BuscaDeCliente = new PosicaoConsolidadaIntregacaoFundosRequest();

            lRequest.BuscaDeCliente.BuscarPor = OpcaoBuscaClienteIntegracaoFundosEnum.CodigoCBLC;

            lRequest.BuscaDeCliente.TermoDeBusca = lBase.SessaoClienteLogado.CodigoPrincipal.DBToString();

            if (this.ddlFiltroFundos.SelectedIndex != 0) lRequest.IdProduto = this.ddlFiltroFundos.SelectedValue.DBToInt32();

            lRequest.DataDe = this.txtDataDe.Text;

            lRequest.DataAte = this.txtDataAte.Text;

            lRequest.TipoOperacao = this.GetTipoOperacaoRequest() ;

            lRequest.Status = this.GetStatusRequest();

            lRequest.HorarioSolicitacaoIni = this.txtHoraDe.Text;

            lRequest.HorarioSolicitacaoFim = this.txtHoraAte.Text;

            var lResponse = new PesquisarMovimentoOperacoesIntegracaoFundosResponse();

            ContaCorrenteExtratoInfo lContaCorrente = BuscarDadosContaCorrente();

            decimal lSaldoCC = lContaCorrente.SaldoTotal;

            lResponse = lBase.PesquisarMovimentoOperacoes(lRequest);

            List<IntegracaoFundosMovimentoOperacoesInfo> ListaOrdenada = lResponse.Resultado.OrderByDescending(mov => mov.IdMovimento).ToList();

            var lTranporte = new TransporteMovimentoOperacoes().TraduzirLista(ListaOrdenada, lSaldoCC, this.PosicaoCotista);

            this.rptListaDeSolicitacoes.DataSource = lTranporte;
            this.rptListaDeSolicitacoes.DataBind();

            this.trNenhumSolicitacoes.Visible = lResponse.Resultado.Count == 0;
        }
        #endregion

        #region Metodos
        private string GetStatusRequest()
        {
            string lRetorno = string.Empty;

            if (this.chkSolicitado.Checked)         lRetorno += "1,";
            if (this.chkCancelado.Checked)          lRetorno += "2,";
            if (this.chkEmProcessamento.Checked)    lRetorno += "3,";
            if (this.chkExecutado.Checked)          lRetorno += "4";

            lRetorno = (lRetorno.LastIndexOf(",") == (lRetorno.Length - 1) && !string.IsNullOrEmpty(lRetorno)) ? lRetorno.Remove(lRetorno.LastIndexOf(",")) : lRetorno;
 
            return lRetorno;
        }

        private string GetTipoOperacaoRequest()
        {
            string lRetorno = string.Empty;

            if (this.chkAplicacao.Checked) lRetorno += "1,";
            if (this.chkResgate.Checked)    lRetorno += "2";

            lRetorno = (lRetorno.LastIndexOf(",") == (lRetorno.Length - 1) && !string.IsNullOrEmpty(lRetorno)) ? lRetorno.Remove(lRetorno.LastIndexOf(",")) : lRetorno;

            return lRetorno;
        }

        private void CarregaDados()
        {
            this.txtDataDe.Text = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy");
            this.txtDataAte.Text = DateTime.Now.ToString("dd/MM/yyyy");

            this.txtHoraDe.Text = "00:00";
            this.txtHoraAte.Text = "23:59";

            PaginaFundos lServico = new PaginaFundos();
            List<Transporte_IntegracaoFundos> ListaFundos = lServico.PesquisarFundosAplicar(new PesquisarIntegracaoFundosRequest());

            this.ddlFiltroFundos.Items.Clear();

            this.ddlFiltroFundos.Items.Add(new ListItem("Todos", ""));

            this.PosicaoCotista = lServico.PosicaoFundos();

            foreach (var fundo in ListaFundos)
            {
                ListItem lFundo = new ListItem(fundo.Fundo, fundo.IdProduto);

                this.ddlFiltroFundos.Items.Add(lFundo);
            }
        }

        private ContaCorrenteExtratoInfo BuscarDadosContaCorrente()
        {
            ContaCorrenteExtratoInfo lRetorno = null;

            var lBase = (PaginaBase)this.Page;

            IServicoExtratos lServico = lBase.InstanciarServicoDoAtivador<IServicoExtratos>();

            ContaCorrenteExtratoRequest lRequest = new ContaCorrenteExtratoRequest();
            ContaCorrenteExtratoResponse lResponse;

            lRequest.ConsultaCodigoCliente      = lBase.SessaoClienteLogado.CodigoPrincipal.DBToInt32();
            lRequest.ConsultaDataInicio         = DateTime.Now.AddDays(-1);
            lRequest.ConsultaDataFim            = DateTime.Now;
            lRequest.ConsultaNomeCliente        = lBase.SessaoClienteLogado.Nome;
            lRequest.ConsultaTipoExtratoDeConta = OMS.ContaCorrente.Lib.Info.Enum.EnumTipoExtradoDeConta.Movimento;

            lResponse = lServico.ConsultarExtratoContaCorrente(lRequest);

            if (lResponse.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
            {
                //lRetorno = new TransporteRelatorioExtratoContaCorrente( lResponse.Relatorio );

                lRetorno = lResponse.Relatorio;
            }
            else
            {
                throw new Exception(string.Format("{0}-{1}", lResponse.StatusResposta, lResponse.StackTrace));
            }

            return lRetorno;
        }
        #endregion
    }
}
