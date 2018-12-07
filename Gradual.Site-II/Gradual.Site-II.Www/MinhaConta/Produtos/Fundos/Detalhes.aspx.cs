using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;
using Gradual.Site.Www.Transporte;


namespace Gradual.Site.Www.MinhaConta.Produtos.Fundos
{
    public partial class Detalhes : PaginaFundos
    {
        #region Atributos
        #endregion

        #region Propriedades

        public string AplicacaoMinima                   { get; set; }

        public string AplicacaoMinimaAdicional          { get; set; }

        public string DiasConversaoAplicacao            { get; set; }

        public string ResgateMinimo                     { get; set; }

        public string SaldoMinimoPermanencia            { get; set; }

        public string DiasConversaoResgate              { get; set; }

        public string DiasConversaoResgateAntecipado    { get; set; }

        public string DiasPagamentoResgate              { get; set; }

        public string TaxaAdministracao                 { get; set; }

        public string TaxaAdministracaoMaxima           { get; set; }

        public string TaxaPerformance                   { get; set; }

        public string ResgateAntecipado                 { get; set; }

        public string PatrimonioLiquido                 { get; set; }

        public string IdFundo                           { get; set; }

        public string NomeFundo                         { get; set; }

        public string PerfilImagem                      { get; set; }

        public string BotaoAplicar { get; set; }

        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Request["i"] == "1")
                {
                    RodarJavascriptOnLoad("GradSite_ExibirMensagem('A', 'Este fundo está indisponível para aplicações no momento.');");
                }

                /*
                if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                {
                    base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                    //return;
                }
                */

                this.CarregarDados();
            }
        }
        #endregion

        #region Métodos
        public void CarregarDados()
        {
            int lIdFundo = Request["idFundo"].DBToInt32();

            this.IdFundo = Request["idFundo"].DBToString();

            /*
            string lPerfil = !string.IsNullOrEmpty(base.SessaoClienteLogado.PerfilSuitability) ? base.SessaoClienteLogado.PerfilSuitability.ToLower() : string.Empty;

            switch(lPerfil)
            {
                case "conservador":
                    PerfilImagem = "../../../Resc/skin/default/img/perfil-verde.png";
                    break;          
                case "moderado":    
                    PerfilImagem = "../../../Resc/skin/default/img/perfil-amarelo.png";
                    break;          
                case "arrojado":    
                    PerfilImagem = "../../../Resc/skin/default/img/perfil-vermelho.png";
                    break;
                default:
                    PerfilImagem = "../../../Resc/skin/default/img/perfil-verde.png";
                    break;

            }

            */

            ConteudoRequest lRequest = new ConteudoRequest();
            ConteudoResponse lResponse;

            lRequest.Conteudo = new DbLib.Dados.ConteudoInfo();
            
            lRequest.Conteudo.CodigoTipoConteudo = 17;      // fixo no banco, pegar da tabela tb_tipo_conteudo
            lRequest.Conteudo.ValorPropriedade1 = lIdFundo.DBToString();   // id do fundo

            lResponse = this.ServicoPersistenciaSite.SelecionarConteudoPorPropriedade(lRequest);

            if (lResponse.ListaConteudo != null && lResponse.ListaConteudo.Count > 0)
            {
                this.litConteudoCms.Text = lResponse.ListaConteudo[0].ConteudoHtml;
            }

            var lRequestRentabilidade = new CompararRentabilidadeIntegracaoFundosRequest();

            lRequestRentabilidade.Produtos = new List<int>() { lIdFundo };
            
            lRequestRentabilidade.Periodo = 5;

            var lResponseRentabilidade = base.ListarRentabilidadeMesDetalhes(lRequestRentabilidade);

            List<Transporte_RentabilidadeFundo> lTransporte = new Transporte_RentabilidadeFundo().TraduzirLista(lResponseRentabilidade.FundosSimulados[0]);

            this.rptRentabilidadeFundo.DataSource = lTransporte;
            this.rptRentabilidadeFundo.DataBind();

            this.trNenhumRentabilidadeItem.Visible = lTransporte.Count == 0;

            var lRequestFundos = new PesquisarIntegracaoFundosRequest();

            lRequestFundos.IdProduto = lIdFundo;

            var lResponseFundos = base.PesquisarFundos(lRequestFundos);

            if (lResponseFundos.Count > 0)
            {
                this.NomeFundo = lResponseFundos[0].Fundo;

                this.AplicacaoMinima                 = lResponseFundos[0].AplicacaoInicial;

                this.AplicacaoMinimaAdicional        = lResponseFundos[0].MinimoAplicacaoAdicional;

                this.DiasConversaoAplicacao          = string.IsNullOrEmpty(lResponseFundos[0].DiasConversaoAplicacao) ? "Sem Informações" : lResponseFundos[0].DiasConversaoAplicacao;

                this.ResgateMinimo                   = lResponseFundos[0].ResgateMinimo;

                this.SaldoMinimoPermanencia          = lResponseFundos[0].SaldoMinimo;

                this.DiasConversaoResgate            = string.IsNullOrEmpty(lResponseFundos[0].DiasConversaoResgate) ? "Sem Informações" : lResponseFundos[0].DiasConversaoResgate;

                this.DiasConversaoResgateAntecipado  = string.IsNullOrEmpty(lResponseFundos[0].DiasConversaoResgateAntecipado) ? "Sem Informações" : lResponseFundos[0].DiasConversaoResgateAntecipado;

                this.DiasPagamentoResgate            = string.IsNullOrEmpty(lResponseFundos[0].DiasPagamentoResgate) ? "Sem Informações" : lResponseFundos[0].DiasPagamentoResgate;

                this.TaxaAdministracao               = lResponseFundos[0].TaxaAdministracao;

                this.TaxaAdministracaoMaxima         = lResponseFundos[0].TaxaAdministracaoMaxima;

                this.TaxaPerformance                 = lResponseFundos[0].TaxaPerformance;

                this.ResgateAntecipado               = lResponseFundos[0].TaxaResgateAntecipado;

                this.PatrimonioLiquido               = lResponseFundos[0].PatrimonioLiquido;

                string lPerfilFundo = lResponseFundos[0].Risco;

                switch (lPerfilFundo.ToLower())
                {
                    case "conservador":
                    case "baixo":
                        PerfilImagem = "../../../Resc/skin/default/img/perfil-verde.png";
                        break;
                    case "moderado":
                        PerfilImagem = "../../../Resc/skin/default/img/perfil-amarelo.png";
                        break;
                    case "arrojado":
                    case "alto":
                        PerfilImagem = "../../../Resc/skin/default/img/perfil-vermelho.png";
                        break;
                    default:
                        PerfilImagem = "../../../Resc/skin/default/img/perfil-verde.png";
                        break;

                }

                if (SessaoClienteLogado != null)
                {
                    if (!ConfiguracoesValidadas.FundosInaplicaveis.Contains(Convert.ToInt32(lResponseFundos[0].IdProduto)))
                    {
                        if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                        {
                            base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                        }
                        else
                        {
                            this.BotaoAplicar = string.Format("<a class='botao btn-padrao btn-erica btn-voa' href='/MinhaConta/Produtos/Fundos/Aplicar.aspx?idFundo={0}'>Aplicar</a>", lResponseFundos[0].IdProduto);
                        }
                    }
                }
                
                pnlSemDados.Visible = false;
                pnlComDados.Visible = true;
            }
            else
            {
                pnlSemDados.Visible = true;
                pnlComDados.Visible = false;
            }
        }
        #endregion
    }
}
