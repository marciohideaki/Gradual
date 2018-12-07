using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;

using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library;
using Gradual.Site.Www.Transporte;

namespace Gradual.Site.Www.MinhaConta.Produtos.Fundos
{
    public partial class Resgate : PaginaFundos
    {
        #region Proriedades
        
        public int IdFundo 
        { 
            get
            {
                return int.Parse( ViewState["IdFundo"].ToString()) ;
            }
            set
            {
                ViewState["IdFundo"] = value;
            }
        }

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

        public decimal ValorDisponivel 
        {
            set
            {
                ViewState["ValorDisponivel"] = value;
            }
            get
            {
                return decimal.Parse(ViewState["ValorDisponivel"].ToString());
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

        public string DataLiquidacao
        {
            get
            {
                return ViewState["DataLiquidacao"].ToString();
            }
            set
            {
                ViewState["DataLiquidacao"] = value;
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

        public bool EhResgateAgendado
        {
            get
            {
                return Convert.ToBoolean(ViewState["EhResgateAgendado"]);
            }
            set
            {
                ViewState["EhResgateAgendado"] = value;
            }
        }
        #endregion

        #region Métodos
        protected void CarregaDados()
        {
            this.lblCliente.Text = string.Concat(SessaoClienteLogado.CodigoPrincipal, " - ", SessaoClienteLogado.Nome);
            
            IntegracaoFundosInfo lInfo = base.GetNomeRiscoFundo(string.Empty, IdFundo);
            
            List<Transporte_PosicaoCotista> lPosicao = base.GetPosicaoFundosFinancial();

            this.NomeFundo = lInfo.NomeProduto;
            this.rdTotal.Checked = true;

            this.CodigoAnbima = lInfo.IdCodigoAnbima;
            this.DataLiquidacao = lInfo.DadosMovimentacao.DsDiasPagResgate.DBToString();

            decimal lPosicaoDisponivelFundo = SomaTotalPosicaoFundos(lPosicao, lInfo.IdCodigoAnbima);
            
            this.txtTotalDisponivel.Text = lPosicaoDisponivelFundo.ToString("N2");
            this.ValorDisponivel = lPosicaoDisponivelFundo;

            this.txtAgendar.Text = DateTime.Now.ToString("dd/MM/yyyy");

        }

        private decimal SomaTotalPosicaoFundos(List<Transporte_PosicaoCotista> lPosicao, string CodigoAnbima)
        {
            decimal lRetorno = 0M;

            lPosicao.ForEach(p => {

                if (p.CodigoAnbima == CodigoAnbima)
                {
                    lRetorno += decimal.Parse(p.ValorLiquido, base.gCultureInfoBR);
                }
            
            });

            return lRetorno;
        }

        private bool ValidaResgate()
        {
            bool lRetorno = true;

            decimal lValorSolicitado = 0M;

            if (!base.ValidarAssinaturaEletronica(this.txtAssDigital.Text))
            {
                lRetorno = false;

                ExibirMensagemJsOnLoad("I", "Assinatura eletrônica inválida.");
            }

            if (this.rdParcial.Checked) decimal.TryParse(this.txtParcial.Text, out lValorSolicitado);

            if (this.rdTotal.Checked) decimal.TryParse(this.txtTotalDisponivel.Text, out lValorSolicitado);

            if (lValorSolicitado == 0M)
            {
                lRetorno = false;
                base.ExibirMensagemJsOnLoad("I", "Valor solicitado inválido");
            }

            if (lValorSolicitado > this.ValorDisponivel)
            {
                lRetorno = false;
                base.ExibirMensagemJsOnLoad("I", "Valor solicitado maior que a posição do fundo");
            }

            if (this.txtAgendar.Text != string.Empty)
            {
                DateTime lData = default(DateTime);

                DateTime.TryParse(this.txtAgendar.Text, out lData);

                if (lData == default(DateTime))
                {
                    lRetorno = false;
                    base.ExibirMensagemJsOnLoad("I", "Data de agendamento inválida");
                }
            }
            else
            {
                lRetorno = false;
                base.ExibirMensagemJsOnLoad("I", "Data de agendamento inválida");
            }

            return lRetorno;
        }

        private void LimpaDados()
        {
            this.txtAgendar.Text = "";
            this.txtParcial.Text = "";
            this.rdTotal.Checked = true;
        }
        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (base.ValidarSessao())
                {
                    if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                    {
                        base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                    }

                    IdFundo = Request["IdFundo"].DBToInt32();

                    CarregaDados();
                }
            }
        }


        protected void btnResgatar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.ValidaResgate())
                {
                    return;
                }

                decimal lValorSolicitado = 0M;

                bool lResgateTotal = true;

                if (this.rdTotal.Checked)
                {
                    lResgateTotal = true;
                    lValorSolicitado = this.txtTotalDisponivel.Text.DBToDecimal();
                    
                }
                else if (this.rdParcial.Checked)
                {
                    lResgateTotal = false;
                    lValorSolicitado = this.txtParcial.Text.DBToDecimal();
                }

                var lRequest =  new SolicitarIntegracaoFundosOperacaoRequest();

                lRequest.Operacao = new IntegracaoFundosOperacaoInfo();

                lRequest.Operacao.Produto = new IntegracaoFundosInfo();

                lRequest.Operacao.AntecipaAplicacao   = false;
                lRequest.Operacao.AplicacaoProgramada = false;
                lRequest.Operacao.IdCliente           = SessaoClienteLogado.CodigoPrincipal.DBToInt32();
                lRequest.Operacao.TipoOperacao        = IntegracaoFundosTipoOperacaoEnum.RESGATE;
                lRequest.Operacao.Status              = IntegracaoFundosStatusOperacaoEnum.SOLICITADO;
                lRequest.Operacao.Produto.IdProduto   = IdFundo;
                lRequest.Operacao.ResgateTotal        = lResgateTotal;
                lRequest.Operacao.ValorSolicitado     = lValorSolicitado;

                lRequest.Operacao.DataAgendamento = DateTime.Parse(this.txtAgendar.Text + " 23:59:59");

                if (lRequest.Operacao.DataAgendamento.Value.ToString("dd/MM/yyyy") != DateTime.Now.ToString("dd/MM/yyyy"))
                {
                    lRequest.Operacao.AplicacaoProgramada = true;

                    EhResgateAgendado = true;
                }
                else
                {
                    EhResgateAgendado = false;
                }

                lRequest.PosicaoCotista = new List<DbLib.Dados.MinhaConta.FundoInfo>();

                var lPosicaoFundos = base.PosicaoFundos();

                lRequest.PosicaoCotista = new Transporte_PosicaoCotista().TraduzirLista(lPosicaoFundos);
                
                var lRetornoSolicitacao = base.SolicitarOperacao(lRequest);

                if (lRetornoSolicitacao.Criticas != null && lRetornoSolicitacao.Criticas.Count > 0)
                {
                    string lCriticas = string.Empty;

                    foreach (CriticaInfo critica in lRetornoSolicitacao.Criticas)
                    {
                        lCriticas += string.Concat(critica.Status.ToString(), " - ",  critica.Descricao, "\n\n");
                    }

                    base.ExibirMensagemJsOnLoad("E",lCriticas);

                    return;
                }

                

                Nullable<int> lCodigoFundoItau = base.VerificaExistenciaFundoItau(this.CodigoAnbima);

                if (lCodigoFundoItau.HasValue)  //--> Se houver CodigoItau então tenta integrar com o itau
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

                if (!EhResgateAgendado)
                {
                    this.EmailAvisoResgate();
                }

                //Sempre efetuar Movimento para a financial

                if (!base.VerificaCotistaFinancial())
                {
                    base.ExportaCotistaParaFinancial();
                }

                //base.ExportaMovimentoParaFinancial();
                
                base.ExibirMensagemJsOnLoad("I", "Resgate solicitado com sucesso \n\nVerifique o status da sua solicitação da aba Relatório da página de Operações de fundos de investimentos");

                this.LimpaDados();

            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad("E", "Ocorreu um erro no Envio do Resgate: " + ex.StackTrace);
            }
        }
        
        public void EmailAvisoResgate()
        {
            try
            {
                string lAssunto = "Resgate solicitado - Cliente: " + SessaoClienteLogado.CodigoPrincipal + " no Fundo " + this.NomeFundo;

                var lVariaves = new Dictionary<string, string>();

                decimal lValorSolicitado = 0M;

                string lTotalParcial = string.Empty;

                if (this.rdParcial.Checked) { decimal.TryParse(this.txtParcial.Text, out lValorSolicitado); lTotalParcial = "Resgate Parcial"; }

                if (this.rdTotal.Checked  ) { decimal.TryParse(this.txtTotalDisponivel.Text, out lValorSolicitado); lTotalParcial = "Resgate Total"; }

                lVariaves.Add("##NomeCliente##", SessaoClienteLogado.Nome);

                lVariaves.Add("##CodigoCliente##", SessaoClienteLogado.CodigoPrincipal);

                lVariaves.Add("##NomeFundo##", this.NomeFundo);

                lVariaves.Add("##ValorSolicitado##", lValorSolicitado.ToString("N2"));

                lVariaves.Add("##Data##", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                lVariaves.Add("##Operacao##", "Resgate");

                lVariaves.Add("##TotalParcial##", lTotalParcial);

                lVariaves.Add("##DataLiquidacao##", this.DataLiquidacao);

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
        #endregion
    }
}
