using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Contratos.Dados.Vendas;
using Gradual.OMS.Library.Servicos;

using Gradual.OMS.ContaCorrente.Lib;
using Gradual.Site.DbLib.Dados.MinhaConta;
using Gradual.OMS.Cotacao.Lib;
using Gradual.Site.Www.Transporte;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.Www.MinhaConta.Produtos.IPO
{
    public partial class ReservaIPO : PaginaIPO
    {

        #region Viewstates
        /// <summary>
        /// Propriedade viewstate para amazenar o objeto de IPO do Cliente
        /// </summary>
        public IPOInfo IPOClienteViewState
        {
            get
            {
                return (IPOInfo)ViewState["ValorMaximo"] ;
            }
            set
            {
                ViewState["ValorMaximo"] = value;
            }
        }

        /// <summary>
        /// Propriedade viewstate para armazenar a Url de onde veio o request para chamar essa página como popup
        /// Se veio do portal ou se veio da intranet.
        /// </summary>
        public string IPOURLReferrer
        {
            get
            {
                return ViewState["IPOURLReferrer"].ToString();
            }
            set
            {
                ViewState["IPOURLReferrer"] = value;
            }
        }

        /// <summary>
        /// Propriedade viewstate para armazanar o objeto de dados de 
        /// cliente para futuro envio de email.
        /// </summary>
        public ClienteSinacorInfo DadosClienteEmailViewState
        {
            get
            {
                return (ClienteSinacorInfo)ViewState["DadosClienteEmail"];
            }

            set
            {
                ViewState["DadosClienteEmail"] = value;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Propriedade se o request para o popup veio da intranet ou do portal
        /// </summary>
        public bool EhOrigemIntranet
        {
            get
            {
                return (Request["ISIN"] == null && IPOURLReferrer.Contains("intranet"));
            }
        }
        #endregion

        #region Events
        
        /// <summary>
        /// Evento de Load da página
        /// </summary>
        /// <param name="sender">Nâo está sendo usado</param>
        /// <param name="e">Não está smendo usado</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (base.SessaoClienteLogado != null && string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                {
                    base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                }
                else 
                {
                    IPOURLReferrer = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToLower();

                    if (!EhOrigemIntranet)
                    {
                        this.hddCodigoISIN.Value = Request["ISIN"].ToString();
                    }

                    this.CarregarDadosFormulario();
                    
                    this.CarregarDadosIniciais();
                }
            }
        }

        /// <summary>
        /// Evento de Click do botão de confirmação de Reserva de IPO
        /// </summary>
        /// <param name="sender">Não está sendo usado</param>
        /// <param name="e">Não está senod usado</param>
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (this.ValidarReservaIPO())
            {
                this.EfetuarReserva();
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Carrega informações básicas para o formulário de 
        /// solicitação de reserva de Oferta Pública
        /// </summary>
        private void CarregarDadosFormulario()
        {
            try
            {
                var lRequest = new ConsultarEntidadeRequest<IPOInfo>();

                lRequest.Objeto = new IPOInfo();

                var lResponse = base.ConsultarIPOSite(lRequest);

                /*
                this.cbo_IPO_Empresa.DataSource = lResponse;
                this.cbo_IPO_Empresa.DataValueField = "DsEmpresa";
                this.cbo_IPO_Empresa.DataTextField  = "DsEmpresa";
                this.cbo_IPO_Empresa.DataBind();
                */

                lResponse.ForEach(empresa => {
                    ListItem lItem = new ListItem(empresa.DsEmpresa + "||" + empresa.CodigoISIN, empresa.DsEmpresa);
                    this.cbo_IPO_Empresa.Items.Add(lItem);
                });

                var lInfo = new IPOInfo();

                lInfo.DsEmpresa = "selecione";

                lResponse.Insert(0, lInfo);

                var lIpoModalidade = new Dictionary<string,string>();
                var lIpoGarantia   = new Dictionary<string, string>();
                foreach (var lIpo in lResponse)
                {
                    if (!string.IsNullOrEmpty(lIpo.Modalidade) && !lIpoModalidade.ContainsKey(lIpo.DsEmpresa))
                    {
                        lIpoModalidade.Add(lIpo.DsEmpresa, lIpo.Modalidade);
                    }

                    if (!lIpoGarantia.ContainsKey(lIpo.DsEmpresa))
                    {
                        lIpoGarantia.Add(lIpo.DsEmpresa, lIpo.VlPercentualGarantia.ToString("N3"));
                    }
                }
                
                hddListaIpoModalidade.Value = RetornarSucessoAjax( lIpoModalidade,"");
                hddListaIpoGarantia.Value = RetornarSucessoAjax(lIpoGarantia, "");
            }
            catch (Exception ex)
            {
                ExibirMensagemJsOnLoad("E", ex.StackTrace);
            }
        }

        /// <summary>
        /// Métodoque carrega os dados inicias como os dado de cliente e dos 
        /// dados do IPO selecionado pelo cliente ou pelo assessor
        /// </summary>
        private void CarregarDadosIniciais()
        {
            try
            {
                bool lNaoHaReservas = true;
                string lCodigoCliente = Request["COD"];
                string lNomeCliente = string.Empty;
                string lCpfCnpj     = string.Empty;

                int lCliente = base.SessaoClienteLogado != null ? base.SessaoClienteLogado.CodigoPrincipal.DBToInt32() : lCodigoCliente.DBToInt32();

                if (base.SessaoClienteLogado == null && EhOrigemIntranet)
                {
                    var lRequest = new ClienteSinacorRequest();
                    lRequest.ClienteSinacor = new ClienteSinacorInfo();
                    lRequest.ClienteSinacor.CodigoCliente = lCodigoCliente.DBToInt32();

                    var lResponse = this.ServicoPersistenciaSite.BuscaInformacoesClienteSinacor(lRequest);

                    if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                    {
                        if (lResponse.ListaClienteSinacor.Count > 0)
                            DadosClienteEmailViewState = lResponse.ListaClienteSinacor[0];
                    }
                }
                else
                {
                    ClienteSinacorInfo lInfo = new ClienteSinacorInfo();

                    if (base.SessaoClienteLogado == null)
                    {
                        ExibirMensagemJsOnLoad("E", "Sem usuário na sessão, efetue o login no portal");

                        return;
                    }

                    lInfo.CodigoAssessor = base.SessaoClienteLogado.AssessorPrincipal.DBToInt32();
                    lInfo.CodigoCliente  = base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();
                    lInfo.EmailCliente   = base.SessaoClienteLogado.Email;
                    lInfo.EmailAssessor  = "";

                    DadosClienteEmailViewState = lInfo;
                }

                if (Request["ISIN"] != null)
                {
                    string lCodigoISIN = Request["ISIN"];

                    //Dados do cliente
                    this.txt_IPO_Conta.Text = lCliente.ToString();
                    this.txt_IPO_CPF.Text   = base.SessaoClienteLogado.CpfCnpj;
                    this.txt_IPO_Nome.Text  = base.SessaoClienteLogado.Nome;

                    var lRequestIPO = new ReceberEntidadeRequest<IPOInfo>();

                    lRequestIPO.Objeto             = new IPOInfo();
                    lRequestIPO.Objeto.CodigoISIN  = lCodigoISIN;
                    lRequestIPO.Objeto.DataFinal   = DateTime.Now;
                    lRequestIPO.Objeto.DataInicial = DateTime.Now;

                    var lResponseIPO = base.SelecionarIPOSite(lRequestIPO);

                    //Dados da reserva de IPO
                    if (lResponseIPO != null && !string.IsNullOrEmpty(lResponseIPO.Ativo))
                    {
                        this.cbo_IPO_Empresa.SelectedValue    = lResponseIPO.DsEmpresa;
                        //this.txt_IPO_ValorMaximo.Text         = lResponseIPO.VlMaximo.ToString();
                        this.txt_IPO_PercentualGarantias.Text = lResponseIPO.VlPercentualGarantia.ToString();
                        this.cbo_IPO_Modalidade.SelectedValue = lResponseIPO.Modalidade;
                        this.IPOClienteViewState = lResponseIPO;
                    }
                    else 
                    {
                        lNaoHaReservas = false;
                    }
                }
                else
                {
                    this.txt_IPO_Conta.Text = Request["COD"].ToString();
                    this.txt_IPO_CPF.Text   = Request["CPFCNPJ"].ToString();
                    this.txt_IPO_Nome.Text  = Request["NOME"].ToString();
                }

                //buscar o saldo conta corrente
                var lServCC = Ativador.Get<IServicoContaCorrente>();

                decimal lTotalSaldoCC = 0M;

                var lResponseSaldo = lServCC.ObterSaldoContaCorrente(new Gradual.OMS.ContaCorrente.Lib.Mensageria.SaldoContaCorrenteRequest()
                {
                    IdCliente =lCliente

                });

                if (lResponseSaldo != null && lResponseSaldo.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    this.txt_IPO_SaldoDisponivel.Text = lResponseSaldo.Objeto.SaldoD0.ToString("N2");
                    lTotalSaldoCC = lResponseSaldo.Objeto.SaldoD0;
                }

                ///Buscanso valor de fundos
                var lBaseFundos = new PaginaFundos();

                List<Transporte_PosicaoCotista> lListaFundos = null;

                if (EhOrigemIntranet)
                {
                    lListaFundos = lBaseFundos.PosicaoFundosSumarizada(int.Parse(lCodigoCliente), lCpfCnpj);
                }else
                {
                    lListaFundos = lBaseFundos.PosicaoFundosSumarizada();
                    this.pnlAssinaturaEletronica.Style["display"] = "block";
                }

                decimal lTotalFundos = 0M;

                foreach (var lFundo in lListaFundos)
                {
                    var lInfo = new FundoInfo();

                    lInfo.ValorLiquido = lFundo.ValorLiquido.DBToDecimal();
                    
                    if (lInfo.ValorLiquido.HasValue)
                    {
                        lTotalFundos += lInfo.ValorLiquido.Value;
                    }
                }

                //Buscando dados de Custodia
                var lListaCustodia = base.BuscarCustodiasDoCliente(lCliente);
                
                decimal lSomatoriaCustodia = 0M;

                IServicoCotacao lServicoCotacao = InstanciarServicoDoAtivador<IServicoCotacao>();

                foreach (TransporteCustodiaInfo lCus in lListaCustodia)
                {
                    var lCotacao = lServicoCotacao.ReceberTickerCotacao(lCus.CodigoNegocio);

                    TransporteMensagemDeNegocio lMsgCotacao = new TransporteMensagemDeNegocio(lCotacao);

                    lSomatoriaCustodia += (lCus.QtdAtual.DBToDecimal() * lMsgCotacao.Preco.DBToDecimal());
                }

                this.txt_IPO_ValorCustodia.Text = lSomatoriaCustodia.ToString("N2");

                this.txt_IPO_SaldoFundos.Text = lTotalFundos.ToString("N2");

                this.txt_IPO_LimiteTotal.Text = (lTotalFundos + lTotalSaldoCC + lSomatoriaCustodia).ToString("N2");

                this.txt_IPO_Conta  .Enabled             = false;
                this.txt_IPO_CPF    .Enabled             = false;
                this.txt_IPO_Nome   .Enabled             = false;
                this.txt_IPO_Data.Text                   = DateTime.Now.ToString("dd/MM/yyyy");
                this.txt_IPO_Data               .Enabled = false;
                this.txt_IPO_PercentualGarantias.Enabled = false;
                this.txt_IPO_SaldoFundos        .Enabled = false;
                this.txt_IPO_SaldoDisponivel    .Enabled = false;
                this.cbo_IPO_Modalidade.Enabled        = false;
                //this.txt_IPO_ValorMaximo      .Enabled = false;
                this.txt_IPO_ValorCustodia.Enabled       = false;
                this.txt_IPO_LimiteTotal.Enabled         = false;

                ///se for origem da intranet é importante deixar o 
                ///combo de empresa ser selecionavel.
                if (!EhOrigemIntranet)
                {
                    this.cbo_IPO_Empresa.Enabled = false;
                    this.cbo_IPO_Modalidade.Enabled = false;
                }

                if (!lNaoHaReservas)
                {
                    ExibirMensagemJsOnLoad("I", "Não há reserva de Ofertas pública para serem efetuadas");

                    RodarJavascriptOnLoad("\r\n\t\tSolicitacoesGerenciamentoIPO_DesabilitaFormularioReserva();");


                }
            }
            catch (Exception ex)
            {
               ExibirMensagemJsOnLoad("E", ex.StackTrace);
            }
        }

        /// <summary>
        /// Método que efetua a reserva de IPO, grava a reserva no banco de dados e envia email de aviso.
        /// </summary>
        private void EfetuarReserva()
        {
            try
            {
                var lClienteIPO = new IPOClienteInfo();

                lClienteIPO.CodigoCliente = int.Parse(this.txt_IPO_Conta.Text);
                lClienteIPO.CpfCnpj       = this.txt_IPO_CPF.Text;
                lClienteIPO.NomeCliente   = this.txt_IPO_Nome.Text;
                lClienteIPO.Empresa       = this.cbo_IPO_Empresa.SelectedValue;
                
                if (EhOrigemIntranet)
                {
                    lClienteIPO.CodigoISIN = IPOClienteViewState.CodigoISIN;
                }
                else
                {
                    lClienteIPO.CodigoISIN = Request["ISIN"].ToString();
                }
                decimal lValorLimite      = 0m;

                decimal.TryParse(this.txt_IPO_LimiteTotal.Text, out lValorLimite);

                lClienteIPO.Limite = lValorLimite;

                lClienteIPO.Modalidade = cbo_IPO_Modalidade.SelectedValue;
                lClienteIPO.Status = Intranet.Contratos.Dados.Enumeradores.eStatusIPO.Solicitada;

                decimal lValorTaxaMaxima = 0M;

                string lTaxaMaximaString = this.txt_IPO_TaxaMaxima.Text.Replace("%", "");

                decimal.TryParse(lTaxaMaximaString, out lValorTaxaMaxima);

                lClienteIPO.TaxaMaxima = lValorTaxaMaxima;

                decimal lValorReserva = 0M;

                decimal.TryParse(this.txt_IPO_ValorReserva.Text, out lValorReserva);

                lClienteIPO.ValorReserva = lValorReserva;

                decimal lValorMaximo = 0M;

                decimal.TryParse(this.txt_IPO_ValorMaximo.Text, out lValorMaximo);

                lClienteIPO.ValorMaximo = lValorMaximo;

                lClienteIPO.Data = DateTime.Now;

                lClienteIPO.PessoaVinculada = this.chk_IPO_PessoaVinculada.Checked;

                lClienteIPO.VlPercentualGarantia = IPOClienteViewState.VlPercentualGarantia;

                lClienteIPO.NumeroProtocolo =  this.GerarNumeroProtocolo() + lClienteIPO.CodigoCliente ;

                lClienteIPO.Observacoes = string.Empty ;

                var lRequest = new SalvarEntidadeRequest<IPOClienteInfo>();

                lRequest.Objeto = lClienteIPO;

                base.InserirIPOCliente(lRequest);

                this.EnviaEmailAvisoReservaIPO(lClienteIPO, DadosClienteEmailViewState);

                base.ExibirMensagemJsOnLoad("I", "Reserva efetuada com sucesso");

                base.RodarJavascriptOnLoad("\r\n\t\tSolicitacoesGerenciamentoIPO_DesabilitaFormularioReserva();");

                this.RetornaCamposInativado();
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
                base.ExibirMensagemJsOnLoad("E", ex.StackTrace);
                this.RetornaCamposInativado();
            }
        }

        /// <summary>
        /// Envia Email para as parte envolvidas na reserva de IPO
        /// </summary>
        /// <param name="pInfo">Dados de Reserva do IPO </param>
        /// <param name="pDadosCliente">Dados de cliente encapsulado dentro de um objeto.</param>
        /// <returns>Retorna true se conseguiu enviar email a todos os envolvidos</returns>
        public bool EnviaEmailAvisoReservaIPO(IPOClienteInfo pInfo, ClienteSinacorInfo pDadosCliente)
        {
            bool lRetorno = false;

            try
            {
                string lAssunto = "Reserva de Oferta Publica Efetuada - Cliente: " + pInfo.CodigoCliente + ", Oferta da Empresa : " + pInfo.Empresa;
            
                var lVariaves = new Dictionary<string, string>();

                IPOInfo lIpo = IPOClienteViewState;

                lVariaves.Add("##nomecliente##"     ,       pInfo.NomeCliente);
                lVariaves.Add("##numeroprotocolo##" ,       pInfo.NumeroProtocolo);
                lVariaves.Add("##modalidade##",             pInfo.Modalidade);
                lVariaves.Add("##valormaximo##",            pInfo.ValorMaximo.ToString("N2"));
                lVariaves.Add("##horamaxima##",             lIpo.HoraMaxima);
                lVariaves.Add("##valorminimo##",            lIpo.VlMinimo.ToString("N2"));
                lVariaves.Add("##datainicial##",            lIpo.DataInicial.ToString("dd/MM/yyyy"));
                lVariaves.Add("##datafinal##",              lIpo.DataFinal.ToString("dd/MM/yyyy"));
                lVariaves.Add("##percentualgarantias##",    lIpo.VlPercentualGarantia.ToString("N2"));
                lVariaves.Add("##codigoisin##",             lIpo.CodigoISIN);
                lVariaves.Add("##empresa##",                lIpo.DsEmpresa);
                lVariaves.Add("##ativo##",                  lIpo.Ativo);

                ///Envia email para o atendimento ou custódia
                base.EnviarEmail(ConfiguracoesValidadas.Email_ReservaIPO, lAssunto, "AvisoReservaIPO.htm", lVariaves, eTipoEmailDisparo.Todos, null, null);

                ///Envia email para o cliente
                base.EnviarEmail(pDadosCliente.EmailCliente, lAssunto, "AvisoReservaIPO.htm", lVariaves, eTipoEmailDisparo.Todos, null, null);

                ///Envia o email para o assessor se for origem da intranet
                if (EhOrigemIntranet)
                {
                    //base.EnviarEmail(pDadosCliente.EmailAssessor, lAssunto, ".htm", lVariaves, eTipoEmailDisparo.Todos, null, null);
                }

                lRetorno = true;
            }
            catch (Exception ex)
            {
                ExibirMensagemJsOnLoad("E", ex.StackTrace);
                lRetorno = false;
                this.RetornaCamposInativado();
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que gera o protocolo com os dados da Reserva de oferta pública do cliente
        /// </summary>
        /// <returns></returns>
        public string GerarNumeroProtocolo()
        {
            string lRetorno = string.Empty;

            lRetorno = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            return lRetorno;
        }

        /// <summary>
        /// Validação da reserva de IPO
        /// </summary>
        /// <returns>Retorna true se validou o objeto de reserva de IPO com sucesso</returns>
        public bool ValidarReservaIPO()
        {
            bool lRetorno = true;

            this.txt_IPO_Conta              .Enabled = true;
            this.txt_IPO_CPF                .Enabled = true;
            this.txt_IPO_Nome               .Enabled = true;
            this.txt_IPO_Data               .Enabled = true;
            this.txt_IPO_PercentualGarantias.Enabled = true;
            this.txt_IPO_SaldoFundos        .Enabled = true;
            this.txt_IPO_SaldoDisponivel    .Enabled = true;
            this.cbo_IPO_Empresa            .Enabled = true;
            this.cbo_IPO_Modalidade         .Enabled = true;
            //this.txt_IPO_ValorMaximo      .Enabled = true;
            this.txt_IPO_ValorCustodia      .Enabled = true;
            this.txt_IPO_LimiteTotal        .Enabled = true;

            ///Validando o valor reserva
            decimal lValorReserva = 0M;

            decimal.TryParse(this.txt_IPO_ValorReserva.Text, out lValorReserva);

            if (lValorReserva.Equals(0))
            {
                ExibirMensagemJsOnLoad("I", "Valor de reserva solicitado inválido");

                this.RetornaCamposInativado();

                return false;
            }

            ///Validando o valor Maximo
            decimal lValorMaximo = 0M;

            decimal.TryParse(this.txt_IPO_ValorMaximo.Text, out lValorMaximo);
            /*
            if (lValorMaximo.Equals(0))
            {
                ExibirMensagemJsOnLoad("I", "Valor de máximo de reserva solicitado inválido");

                this.RetornaCamposInativado();

                return false;
            }
            */
            //Validando a taxa máxima
            decimal lTaxaMaxima = 0M;

            string lTaxaMaximaString = this.txt_IPO_TaxaMaxima.Text.Replace("%","");

            decimal.TryParse(lTaxaMaximaString, out lTaxaMaxima);

            if (lTaxaMaximaString != "0" && lTaxaMaxima.Equals(0))
            {
                ExibirMensagemJsOnLoad("I", "Valor de taxa máxima de reserva solicitada inválida");

                this.RetornaCamposInativado();

                return false;
            }
            
            if (!EhOrigemIntranet)
            {
                if (IPOClienteViewState == null)
                {
                    ExibirMensagemJsOnLoad("I", "Erro ao efetuar a reserva para essa Oferta, os dados para essa Oferta não estão corretos");

                    this.RetornaCamposInativado();

                    return false;
                }
            }
            else
            {
                var lRequestIPO = new ReceberEntidadeRequest<IPOInfo>();

                string cboEmpresa = this.cbo_IPO_Empresa.SelectedItem.Text;

                string lCodigoISIN = cboEmpresa.Trim().Substring(cboEmpresa.Trim().IndexOf("||") + 2);// cboEmpresa.Substring(cboEmpresa.IndexOf("||"));

                lRequestIPO.Objeto             = new IPOInfo();
                lRequestIPO.Objeto.CodigoISIN  = lCodigoISIN.Trim();
                lRequestIPO.Objeto.DataFinal   = DateTime.Now;
                lRequestIPO.Objeto.DataInicial = DateTime.Now;

                var lResponseIPO = base.SelecionarIPOSite(lRequestIPO);
                IPOClienteViewState = lResponseIPO;
            }

            ///Validando horário máximo da reserva....
            DateTime lDatafinal    = IPOClienteViewState.DataFinal;
            string[] lHorafinalSplit = IPOClienteViewState.HoraMaxima.Split(':');

            int lHoraFinal = lHorafinalSplit[0].DBToInt32();
            int lMinutofinal = lHorafinalSplit[1].DBToInt32();

            DateTime lDataHoraMaxima = new DateTime(lDatafinal.Year, lDatafinal.Month,lDatafinal.Day, lHoraFinal, lMinutofinal,0);
            
            if (lDataHoraMaxima < DateTime.Now)
            {
                ExibirMensagemJsOnLoad("I", "Não é permitido mais efetuar a reserva para essa Oferta, a data de reserva expirou.");

                this.RetornaCamposInativado();

                return false;
            }

            //Validando a assinatura eletronica do cliente, caso ele tenha entrado pelo site.
            if (!EhOrigemIntranet)
            {
                string lAssinaturaEletronica = this.txt_IPO_AssinaturaEletronica.Text;

                if (!ValidarAssinaturaEletronica(lAssinaturaEletronica))
                {
                    ExibirMensagemJsOnLoad("I", "Assinatura eletrônica inválida");

                    this.RetornaCamposInativado();

                    return false;
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que Inabilita os campos do formulário
        /// </summary>
        public void RetornaCamposInativado()
        {
            this.txt_IPO_Conta.Enabled               = false;
            this.txt_IPO_CPF.Enabled                 = false;
            this.txt_IPO_Nome.Enabled                = false;
            this.txt_IPO_Data.Enabled                = false;
            //this.txt_IPO_PercentualGarantias.Enabled = false;
            this.txt_IPO_SaldoFundos.Enabled         = false;
            this.txt_IPO_SaldoDisponivel.Enabled     = false;
            this.cbo_IPO_Empresa.Enabled             = false;
            this.cbo_IPO_Modalidade.Enabled          = false;
            //this.txt_IPO_ValorMaximo      .Enabled = false;
            this.txt_IPO_ValorCustodia.Enabled       = false;
            this.txt_IPO_LimiteTotal.Enabled         = false;
        }
        #endregion
    }
}