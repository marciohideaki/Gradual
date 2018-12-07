using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.TesouroDireto.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Consultas;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta
{
    public partial class ConsultarProtocolo : UCTesouroDireto
    {
        #region Propriedades

        private IServicoTesouroDireto ServicoTesouro
        {
            get
            {
                return InstanciarServicoDoAtivador<IServicoTesouroDireto>();
            }
        }

        private String CPFdoCliente
        {
            get
            {
                return base.SessaoClienteLogado.CpfCnpj;
            }
        }

        private string Protocolo
        {
            get
            {
                return base.Request.QueryString["Protocolo"];
            }
        }

        #endregion

        #region Métodos Private

        private Boolean MostraWSErro(MensagemResponseBase pResposta)
        {
            if (pResposta.StatusResposta != MensagemResponseStatusEnum.OK)
            {
                string lMensagemErro = pResposta.DescricaoResposta;

                if (lMensagemErro.Contains("¬"))
                    lMensagemErro = lMensagemErro.Split('¬')[1];

                if (pResposta.DescricaoResposta.Contains("-2147220910")) { lMensagemErro = "Dados informados inválidos."; }
                else if (pResposta.DescricaoResposta.Contains("-2147217900")) { lMensagemErro = "Erro ao acessar o Banco de Dados."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220898")) { lMensagemErro = "Erro ao indentificar o cliente no sistema."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220187")) { lMensagemErro = "Mercado inválido."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220888")) { lMensagemErro = "Título inexistente no mercado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220865")) { lMensagemErro = "A(s) quantidade(s) deve(m) ser múltipla(s) de 0,2."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220912")) { lMensagemErro = "Erro ao validar os dados do cliente."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220902")) { lMensagemErro = "Valor inferior ao limite mínimo de compra."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220170")) { lMensagemErro = "Existem titulos que não fazem parte da cesta."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220188")) { lMensagemErro = "Cesta Inválida."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220166")) { lMensagemErro = "Item inexistente na cesta."; }
                else if (pResposta.DescricaoResposta.Contains("-2147217873")) { lMensagemErro = "Você não pode excluir o(s) registro(s) porque o(s) mesmo(s) faz(em) parte de outra(s) tabela(s)"; }
                else if (pResposta.DescricaoResposta.Contains("-2147220173")) { lMensagemErro = "Já existe uma cesta para este negociador."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220174")) { lMensagemErro = "A Cesta não pode ser alterada pois já foi fechada."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220175")) { lMensagemErro = "O CPF é diferente do CPF da cesta."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220851")) { lMensagemErro = "Saldo Bloqueado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220852")) { lMensagemErro = "Título Bloqueado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220889")) { lMensagemErro = "Saldo Insuficiente para venda."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220890")) { lMensagemErro = "A Quantidade de venda é maior que a Quantidade Disponivel."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220891")) { lMensagemErro = "O Limite mensal de vendas foi ultrapassado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220896")) { lMensagemErro = "Mercado Fechado."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220897")) { lMensagemErro = "Mercado Suspenso."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220900")) { lMensagemErro = "A Quantidade a comprar e maior que a Quantidade Disponivel."; }
                else if (pResposta.DescricaoResposta.Contains("-2147217833")) { lMensagemErro = "Um erro ocorreu. Contate o administrador."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220899")) { lMensagemErro = "Investidor Suspenso."; }
                else if (pResposta.DescricaoResposta.Contains("-2147220901")) { lMensagemErro = "O Limite mensal foi ultrapassado."; }
                else { lMensagemErro = "Mercado Suspenso."; }

                GlobalBase.ExibirMensagemJsOnLoad("E", lMensagemErro);

                return false;
            }

            return true;
        }

        private void CarregarProtocolo()
        {
            try
            {
                ConsultasConsultaCestaRequest lRequest = new ConsultasConsultaCestaRequest();
                ConsultasConsultaCestaResponse lResponse;

                byte lSituacao = 0;
                byte lTipo = 0;

                string lDataCompra = "";

                int lNumeroProtocolo = 0;
                int lMercado = 0;
                int lCodigoTitulo = 0;

                if (txtProtocolo_Mercado.Text.Replace(" ", "").Length > 0)
                    lMercado = int.Parse(txtProtocolo_Mercado.Text);

                if (cboProtocolo_Situacao.SelectedValue != "-")
                    lSituacao = byte.Parse(cboProtocolo_Situacao.SelectedValue);

                if (txtProtocolo_DataTransacao.Text.Replace(" ", "").Length > 0)
                    lDataCompra = DateTime.Parse(txtProtocolo_DataTransacao.Text).ToString("MM/dd/yyyy");

                List<byte> tipos = new List<byte>();

                if (chkProtocolo_Tipo_Compra.Checked) tipos.Add(1);

                if (chkProtocolo_Tipo_Venda.Checked) tipos.Add(2);

                if (tipos.Count != 1)
                {
                    lTipo = 0;
                }
                else
                {
                    lTipo = tipos[0];
                }

                if (txtProtocolo_Numero.Text.Replace(" ", "").Length > 0)
                    lNumeroProtocolo = int.Parse(txtProtocolo_Numero.Text);

                lRequest.CodigoMercado = lMercado;
                lRequest.CPFNegociador = this.CPFdoCliente;
                lRequest.Situacao = lSituacao;
                lRequest.Tipo = lTipo;
                lRequest.CodigoCesta = lNumeroProtocolo;
                lRequest.DataCompra = lDataCompra.DBToDateTime();
                lRequest.CodigoTitulo = lCodigoTitulo;
                lRequest.Cliente = 0;

                lResponse = this.ServicoTesouro.ConsultarCesta(lRequest);

                if (!MostraWSErro(lResponse))
                    return;

                this.rptConsultarProtocolo.DataSource = TransporteConsultaProtocolo.TraduzirLista(new List<OMS.TesouroDireto.Lib.Dados.TituloMercadoInfo>()); //Limpa o datasource
                this.rptConsultarProtocolo.DataBind();

                if (lResponse.Titulos != null && lResponse.Titulos.Count > 0)
                {
                    this.litNenhumRegistroEncontrado.Visible = false;

                    this.rptConsultarProtocolo.DataSource = TransporteConsultaProtocolo.TraduzirLista(lResponse.Titulos);
                    this.rptConsultarProtocolo.DataBind();
                }
                else
                {
                    this.litNenhumRegistroEncontrado.Visible = true;
                }

                this.pnlConsultarProtocolo.Visible = true;
            }
            catch (System.Exception ex)
            {
                GlobalBase.ExibirMensagemJsOnLoad("E", "Erro ao tentar realizar consulta.");

                gLogger.Error("[Erro ao tentar realizar consulta.]", ex);
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && !String.IsNullOrWhiteSpace(this.Protocolo))
            {
                this.txtProtocolo_Numero.Text = this.Protocolo;
                this.CarregarProtocolo();
            }
        }

        protected void btnConsultarCompra_Click(object sender, EventArgs e)
        {
            this.CarregarProtocolo();
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            //this.PaginaMaster.Secao = new SecaoDoSite("MinhaConta", "MinhaConta");

            //this.PaginaMaster.SubSecao = new SecaoDoSite("Tesouro Direto", HostERaizFormat("MinhaConta/TesouroDireto/ConsultarProtocolo.aspx"));
        }

        #endregion
    }
}