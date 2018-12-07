using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;
using System.Globalization;
using Gradual.Site.DbLib.Dados.MinhaConta.Suitability;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Site.Www.Transporte;

namespace Gradual.Site.Www.MinhaConta.Produtos.Fundos
{
    public partial class Aplicar : PaginaFundos
    {
        #region Propriedades
        public string NomeFundo 
        { 
            get
            {
                return ViewState["NomeFundo"].ToString();
            }
            set
            {
                ViewState["NomeFundo"] = value;
            }
        }

        public string AplicacaoMinima { get; set; }

        public string NomeFundoPathTermo
        {
            get
            {
                return ViewState["NomeFundoPathTermo"].ToString();
            }
            set
            {
                ViewState["NomeFundoPathTermo"] = value;
            }
        }

        public int CodigoFundo
        {
            get
            {
                return ViewState["CodigoFundo"].DBToInt32();

            }
            set
            {
                ViewState["CodigoFundo"] = value;
            }
        }

        public string CodigoAnbima
        {
            get
            {
                return ViewState["CodigoAnbima"].ToString();
            }
            set
            {
                ViewState["CodigoAnbima"] = value;
            }
        }

        public string PerfilSuitability
        {
            get
            {
                return ViewState["PerfilSuitability"].DBToString();
            }

            set
            {
                ViewState["PerfilSuitability"] = value;
            }
        }

        public string PerfilFundo
        {
            get
            {
                return ViewState["PerfilFundo"].DBToString();
            }

            set
            {
                ViewState["PerfilFundo"] = value;
            }
        }

        public bool EhFundoItau 
        {
            get
            {
                return Convert.ToBoolean(ViewState["EhFundoItau"]);
            }
            set 
            {
                ViewState["EhFundoItau"] = value;
            } 
        }

        public bool EhAplicacaoAgendada 
        {
            get 
            { 
                return Convert.ToBoolean(ViewState["EhAplicacaoAgendada"]); 
            }
            set 
            { 
                ViewState["EhAplicacaoAgendada"] = value; 
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            #region Eventos
            #endregion
            if (base.ValidarSessao())
            {
                if (!this.IsPostBack)
                {
                    if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                    {
                        base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                    }

                    if (Request["idFundo"] == null)
                    {
                        Response.Redirect("Recomendadaos.aspx");
                    }

                    if (ConfiguracoesValidadas.FundosInaplicaveis.Contains(Convert.ToInt32(Request["idFundo"])))
                    {
                        Response.Redirect( string.Format("Detalhes.aspx?idfundo={0}&i=1", Request["idFundo"]));
                    }
                    else
                    {
                        this.hddProduto.Value               = Request["idFundo"].ToString();
                        this.CodigoFundo                    = int.Parse(Request["idFundo"].ToString());
                        this.rdAplicarHoje.Checked          = true;
                        this.rdAntecipaAplicacaoSim.Checked = true;

                        this.CarregarDadosIniciais();
                    }
                }
            }
        }

        #region Métodos
        private string CarregarDadosIniciais()
        {
            this.lblCliente.Text = base.SessaoClienteLogado.CodigoPrincipal + " - " + base.SessaoClienteLogado.Nome;

            try
            { // buscar o saldo
                var lServCC = Ativador.Get<IServicoContaCorrente>();

                var lResSaldo = lServCC.ObterSaldoContaCorrente(new Gradual.OMS.ContaCorrente.Lib.Mensageria.SaldoContaCorrenteRequest()
                {
                    IdCliente = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32()

                });

                decimal lLanceFuturo = 0;

                var lSaldoProjetado = lResSaldo.Objeto.SaldoD0 + lResSaldo.Objeto.SaldoD1 + lResSaldo.Objeto.SaldoD2 + lResSaldo.Objeto.SaldoD3;

                var lTotal = ((lSaldoProjetado - lResSaldo.Objeto.SaldoBloqueado.DBToDecimal()) - lLanceFuturo);

                this.lblSaldoDisponivel.Text = string.Format(new System.Globalization.CultureInfo("pt-BR"), "{0:C}", lTotal);
            }
            catch
            {
                this.lblSaldoDisponivel.Text = " - ";
            }

            var lRequest = new PesquisarIntegracaoFundosRequest();

            lRequest.IdProduto = int.Parse(Request["idFundo"].ToString());

            var lProdutoRes = base.PesquisarFundos(lRequest);

            if (lProdutoRes.Count > 0)
            {
                //lblNomeFundo.Text     = lProdutoRes[0].Fundo;
                //lblnomeProduto.Text   = lProdutoRes[0].Fundo;
                //txtIdProduto.Value    = lProdutoRes.[0].IdProduto.ToString();
                //this.NomeFundoPathTermo = lProdutoRes[0].PathTermo;

                this.NomeFundo          = lProdutoRes[0].Fundo;
                this.AplicacaoMinima    = lProdutoRes[0].AplicacaoAdicional;
                this.PerfilFundo        = lProdutoRes[0].Risco;
                this.CodigoAnbima       = lProdutoRes[0].CodigoAnbima;

                if (lProdutoRes[0].PathTermoPF != string.Empty)
                {
                    this.lnkTermoAdesaoPF.NavigateUrl = "~/Resc/PDFs/AdesaoFundos/" + lProdutoRes[0].PathTermoPF;
                    this.lnkTermoAdesaoPF.Visible = true;
                }
                else
                {
                    this.lnkTermoAdesaoPF.Visible = false;
                }

                if (lProdutoRes[0].PathTermoPJ != string.Empty)
                {
                    this.lnkTermoAdesaoPJ.NavigateUrl = "~/Resc/PDFs/AdesaoFundos/" + lProdutoRes[0].PathTermoPJ;
                    this.lnkTermoAdesaoPJ.Visible = true;
                }
                else
                {
                    this.lnkTermoAdesaoPJ.Visible = false;
                }
            }

            this.PerfilSuitability = base.SessaoClienteLogado.PerfilSuitability;

            var lRequestTermo = new PesquisarTermoIntegracaoFundosRequest();

            lRequestTermo.CodigoCliente = SessaoClienteLogado.CodigoPrincipal.DBToInt32();
            lRequestTermo.CodigoFundo = lRequest.IdProduto;

            var lResponseTermo = base.GetTermoFundosSituacao(lRequestTermo);

            if (lResponseTermo.ListTermo != null && lResponseTermo.ListTermo.Count > 0)
            {
                this.lnkTermoAdesaoPF.Visible = false ;
                this.lnkTermoAdesaoPJ.Visible = false;
                this.divTermoAdesao.Visible = false;
            }
            else
            {
                this.divTermoAdesao.Visible = true;
                //this.lnkTermoAdesaoPF.NavigateUrl = 
                //this.lnkTermoAdesaoPF.Visible = true;
                //this.lnkTermoAdesaoPJ.Visible = true;
            }

            this.divTermoSuitability.Visible = false;

            if (this.PerfilSuitability.ToLower() != "arrojado")
            {
                switch(this.PerfilFundo.ToLower())
                {
                    case "alto":
                        this.divTermoSuitability.Visible = true;
                        break;
                    case "moderado":
                        this.divTermoSuitability.Visible = this.PerfilSuitability.ToLower() == "conservador";
                        break;
                    case "baixo":
                        this.divTermoSuitability.Visible = false;
                        break;
                }
            }

            return string.Empty;
        }

        public void EmailAvisoAplicacao()
        {
            try
            {
                string lAssunto = "Aplicação Efetuada - Cliente: " + SessaoClienteLogado.CodigoPrincipal + " no Fundo " + this.NomeFundo;

                var lVariaves =  new Dictionary<string, string>();

                decimal lValorSolicitado = Convert.ToDecimal( this.txtValorSolicitado.Text);

                lVariaves.Add("##NomeCliente##", SessaoClienteLogado.Nome);

                lVariaves.Add("##CodigoCliente##", SessaoClienteLogado.CodigoPrincipal);

                lVariaves.Add("##NomeFundo##", this.NomeFundo);

                lVariaves.Add("##ValorSolicitado##", lValorSolicitado.ToString("N2"));

                lVariaves.Add("##Data##", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                lVariaves.Add("##Operacao##", "Aplicação");

                lVariaves.Add("##TotalParcial##", "-");

                lVariaves.Add("##DataLiquidacao##", "n\\d");

                if (EhFundoItau)
                {
                    //base.EnviarEmail(ConfiguracoesValidadas.Email_Tesouraria, lAssunto, "AvisoAplicacaoResgate.htm", lVariaves, eTipoEmailDisparo.Todos, null, null);

                    base.EnviarEmail(ConfiguracoesValidadas.Email_Movimentacao, lAssunto, "AvisoAplicacaoResgate.htm", lVariaves, eTipoEmailDisparo.Todos, null, null);
                }
                else
                {
                    base.EnviarEmail(ConfiguracoesValidadas.Email_Movimentacao_Wealth, lAssunto, "AvisoAplicacaoResgate.htm", lVariaves, eTipoEmailDisparo.Todos, null, null);
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemJsOnLoad("E", ex.StackTrace);
            }
        }

        public void EfetuarAplicacao()
        {
            var lOperacoes = new IntegracaoFundosOperacaoInfo();
          
            lOperacoes.IdCliente = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();

            lOperacoes.Produto = new IntegracaoFundosInfo()
            {
                IdProduto = this.hddProduto.Value.DBToInt32()
            };

            lOperacoes.Status = IntegracaoFundosStatusOperacaoEnum.SOLICITADO;

            lOperacoes.TipoOperacao = IntegracaoFundosTipoOperacaoEnum.APLICACAO;

            lOperacoes.ValorSolicitado = Convert.ToDecimal(this.txtValorSolicitado.Text);

            SolicitarIntegracaoFundosOperacaoRequest lRequest = new SolicitarIntegracaoFundosOperacaoRequest();

            if (this.rdAplicarHoje.Checked)
            {
                lOperacoes.DataAgendamento = DateTime.Today.AddDays(1).AddSeconds(-1);

                EhAplicacaoAgendada = false;
            }

            if (this.rdAplicarAgendado.Checked)
            {
                DateTime lDateTime = DateTime.Parse(this.txtAgendarAplicacao.Text);

                lOperacoes.DataAgendamento = lDateTime.AddDays(1).AddSeconds(-1);

                EhAplicacaoAgendada = true;
            }
            else
            {
                EhAplicacaoAgendada = false;
            }

            if (this.rdMensalDia.Checked)
            {
                lOperacoes.DiaAplicacaoProgramada = Convert.ToInt32(cboDiaProgramado.SelectedValue);
                lOperacoes.AntecipaAplicacao      = this.rdAntecipaAplicacaoSim.Checked;
                EhAplicacaoAgendada = true;
            }

            lRequest.Operacao = lOperacoes;

            var lPosicaoFundos = base.PosicaoFundos();

            lRequest.PosicaoCotista = new Transporte_PosicaoCotista().TraduzirLista(lPosicaoFundos);

            var lResponse = SolicitarOperacao(lRequest);

            string lResposta = string.Empty;

            if (lResponse.Criticas.Count > 0)
            {
                foreach (var msg in lResponse.Criticas)
                {
                    lResposta += string.Concat(msg.Descricao, "\n");
                }

                ExibirMensagemJsOnLoad("E", lResposta);
            }
            else
            {
                if (this.divTermoAdesao.Visible)
                {
                    var lRequestTermo = new SalvarTermoIntegracaoFundosRequest();

                    lRequestTermo.Adesao = new IntegracaoFundosTermoAdesaoInfo();

                    lRequestTermo.Adesao.CodigoFundo = this.CodigoFundo;

                    lRequestTermo.Adesao.CodigoCliente = this.SessaoClienteLogado.CodigoPrincipal;

                    lRequestTermo.Adesao.DtHoraAdesao = DateTime.Now;

                    lRequestTermo.Adesao.Origem = "Portal";

                    lRequestTermo.Adesao.CodigoUsuarioLogado = this.SessaoClienteLogado.IdLogin;

                    lRequestTermo.Adesao.DsUsuarioLogado = this.SessaoClienteLogado.Email;

                    base.SalvarTermoAdesao(lRequestTermo);
                }

                var lConsultaSuitability = base.ObterSuitabilityCliente(SessaoClienteLogado.CodigoPrincipal.DBToInt32());

                if (this.divTermoSuitability.Visible == true)
                {
                    ClienteInfo lClienteSuitabilityInfo = new ClienteInfo();

                    lClienteSuitabilityInfo.IdCliente = SessaoClienteLogado.IdCliente;
                    lClienteSuitabilityInfo.ds_perfil = this.GetPerfilFundoCliente();
                    lClienteSuitabilityInfo.ds_status = "Finalizado";
                    lClienteSuitabilityInfo.dt_realizacao = DateTime.Now;
                    lClienteSuitabilityInfo.st_preenchidopelocliente = true;
                    lClienteSuitabilityInfo.ds_loginrealizado = SessaoClienteLogado.Nome;
                    lClienteSuitabilityInfo.ds_fonte = "Portal - Minha conta/Aplicação de Fundos";
                    //lClienteSuitabilityInfo.ds_respostas = lConsultaSuitability.Respostas;
                    lClienteSuitabilityInfo.ds_respostas = "";
                    lClienteSuitabilityInfo.IdClienteSuitability = lConsultaSuitability.IdClienteSuitability;

                    base.AtualizaSuitability(lClienteSuitabilityInfo);
                }

                ExibirMensagemJsOnLoad("I", "Aplicação efetuada com sucesso.");


                Nullable<int> lCodigoFundoItau = base.VerificaExistenciaFundoItau(this.CodigoAnbima);

                if (lCodigoFundoItau.HasValue && lCodigoFundoItau != 0)  //--> Se houver CodigoItau então tenta integrar com o itau
                {
                    EhFundoItau = true;

                    //if (!base.VerificaExistenciaClienteItau(SessaoClienteLogado.CodigoPrincipal.DBToInt32()))
                    //{
                    //    base.ExportaCotistaParaItau();
                    //}

                    //base.ExportaMovimentoParaItau();
                }
                else
                {
                    EhFundoItau = false;
                }

                if (!EhAplicacaoAgendada)
                {
                    this.EmailAvisoAplicacao();
                }

                if (!base.VerificaCotistaFinancial())
                {
                    base.ExportaCotistaParaFinancial();
                }

                //base.ExportaMovimentoParaFinancial();
            }
        }

        public string GetPerfilFundoCliente()
        {
            string lRetorno = string.Empty;

            switch(this.PerfilFundo.ToLower())
            {
                case "alto":
                    lRetorno = "Arrojado";
                    break;
                case "moderado":
                    lRetorno = "Moderado";
                    break;
                case "baixo":
                    lRetorno = "Conservador";
                    break;
            }

            return lRetorno;
        }

        protected bool ValidaAplicacao()
        {
            bool lRetorno = true;

            string lAssinatura = this.txtAssDigital.Text;
            
            decimal lValorSolicitado = 0M;

            decimal.TryParse(this.txtValorSolicitado.Text, out lValorSolicitado);

            if (txtValorSolicitado.Equals(0))
            {
                ExibirMensagemJsOnLoad("I", "Valor solicitado inválido");

                lRetorno = false;
            }

            if (this.divTermoSuitability.Visible && !this.chkSuitability.Checked)
            {
                ExibirMensagemJsOnLoad("I", "Seu perfil não é compátivel com o Risco do fundo. É necessário concordar com o enquadramento do seu perfil.");

                lRetorno = false;
            }

            if (this.divTermoAdesao.Visible && !this.chkTermoAdesao.Checked)
            {
                ExibirMensagemJsOnLoad("I", "É necessário marcar ler o termo de adesão e aceitar os termo de adesão, prospecto e lâminas");

                lRetorno = false;
            }

            if (!base.ValidarAssinaturaEletronica(lAssinatura))
            {
                ExibirMensagemJsOnLoad("I", "Assinatura eletrônica inválida.");

                lRetorno = false;
            }

            return lRetorno;
        }
        #endregion

        protected void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidaAplicacao())
                {
                    this.EfetuarAplicacao();
                }

                //this.limp
            }
            catch (Exception ex)
            {
                ExibirMensagemJsOnLoad("E", ex.StackTrace);
            }
        }
    }
}
