using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.Site.DbLib.Dados.MinhaConta;
using System.IO;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.Custodia.Lib;
using Gradual.OMS.Custodia.Lib.Mensageria;
using Gradual.OMS.Custodia.Lib.Info;

namespace Gradual.Site.Www.MinhaConta.Financeiro
{
    public partial class Retiradas : PaginaBase
    {
        
        #region Metodos Private

        private void CarregarContaBancaria()
        {
            try
            {
                rptContasBancarias.DataSource = SessaoClienteLogado.ContasBancarias;
                rptContasBancarias.DataBind();
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Financeiro/Retirada.aspx > CarregarContaBancaria() [{0}]\r\n[{1}]", ex.Message, ex.StackTrace);

                ExibirMensagemJsOnLoad(ex);
            }
        }

        private void DefinirEstadoDaTela()
        {
            try
            {
                TransporteSaldoDeConta lTransporte = this.BuscarSaldoEmContaNoServico();

                bool lSaldoPositivo = (lTransporte.Acoes_SaldoD0 > 0L);

                txtValorTotal.Text = lTransporte.Acoes_SaldoD0.ToString("N2");

                //txtValorTotal.ForeColor = lSaldoPositivo ? Color.Blue : Color.Red;

                btnRetirada.Enabled = this.btnRetirada.Visible = lSaldoPositivo;

                if (lTransporte.Acoes_SaldoD0 < 0)
                    ExibirMensagemJsOnLoad("E", "Não é possível fazer retirada de uma conta com o valor negativo.");

                //txtValorParcial.Attributes.Add("Onkeydown", "javascript:{Validacao_SomenteNumeros_OnKeyDown(event)};");
                //txtValorParcial.Attributes.Add("OnKeyUp", "javascript:{Validacao_NumerosFormatados_OnKeyUp(event)};");
            }
            catch (Exception ex)
            {
                ExibirMensagemJsOnLoad(ex);
            }
        }

        private TransporteSaldoDeConta BuscarSaldoEmContaNoServico()
        {
            TransporteSaldoDeConta lRetorno = new TransporteSaldoDeConta();

            try
            {
                IServicoContaCorrente lServico = InstanciarServicoDoAtivador<IServicoContaCorrente>();

                SaldoContaCorrenteRequest lRequestCC = new SaldoContaCorrenteRequest();
                SaldoContaCorrenteResponse<ContaCorrenteInfo> lResponseCC;
                
                SaldoContaCorrenteRequest lRequestBFM = new SaldoContaCorrenteRequest();
                SaldoContaCorrenteResponse<ContaCorrenteBMFInfo> lResponseBMF;

                lRequestCC.IdCliente = SessaoClienteLogado.CodigoPrincipal.DBToInt32();

                gLogger.InfoFormat("Chamando Financeiro/Retirada.aspx: BuscarSaldoEmContaNoServico - Bovespa(pCodigo [{0}])", lRequestCC.IdCliente);

                lResponseCC = lServico.ObterSaldoContaCorrente(lRequestCC);
                
                gLogger.InfoFormat("Resposta de Financeiro/Retirada.aspx: BuscarSaldoEmContaNoServico - Bovespa(pCodigo [{0}]) > [{1}] - [{2}] - [{3}]"
                                    , lRequestCC.IdCliente
                                    , lResponseCC.StatusResposta
                                    , lResponseCC.DescricaoResposta
                                    , (lResponseCC.Objeto != null) ? "" : lResponseCC.Objeto.SaldoD0.ToString());

                lRequestBFM.IdCliente = SessaoClienteLogado.CodigoBMF.DBToInt32();
                
                gLogger.InfoFormat("Chamando Financeiro/Retirada.aspx: BuscarSaldoEmContaNoServico - BMF(pCodigo [{0}])", lRequestBFM.IdCliente);

                lResponseBMF = lServico.ObterSaldoContaCorrenteBMF(lRequestBFM);
                
                gLogger.InfoFormat("Resposta de Financeiro/Retirada.aspx: BuscarSaldoEmContaNoServico - BMF(pCodigo [{0}]) > [{1}] - [{2}]"
                                    , lRequestBFM.IdCliente
                                    , lResponseBMF.StatusResposta
                                    , lResponseBMF.DescricaoResposta);

                if (lResponseCC.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && lResponseBMF.StatusResposta != OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    lRetorno = new TransporteSaldoDeConta(lResponseCC.Objeto);
                }
                else if (lResponseCC.StatusResposta != OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && lResponseBMF.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    lRetorno = new TransporteSaldoDeConta(lResponseBMF.Objeto);
                }
                else if (lResponseCC.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && lResponseBMF.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    lRetorno = new TransporteSaldoDeConta(lResponseCC.Objeto, lResponseBMF.Objeto);
                }
                else
                {
                    ExibirMensagemJsOnLoad("E", string.Format("Erro: {0}\r\n{1}", lResponseCC.StatusResposta, lResponseCC.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemJsOnLoad(ex);
            }

            return lRetorno;
        }

        private void SolicitarRetirada()
        {
            try
            {
                string lCorpoDoEmail;
                decimal lTotalTaxas = 0;

                decimal lValor = 0;

                ContaBancariaInfo lContaSelecionadaNaTela = null;

                string lContaBancaria = string.Concat("", this.hidValorEscolhido.Value);

                string lIdFalsoDaConta;

                gLogger.InfoFormat("Financeiro/Retirada.aspx: SolicitarRetirada() > Buscando saldo em conta...");

                TransporteSaldoDeConta lTransporte = this.BuscarSaldoEmContaNoServico();

                TransporteCustodiaInfo lCustodia = this.BuscarTaxaCustodia();
                
                if(null != lCustodia.Taxas)
                {
                    foreach(TaxaCustodiaClienteInfo lTaxa in lCustodia.Taxas)
                    {
                        lTotalTaxas += lTaxa.TaxaCustodia;
                    }
                }

                decimal.TryParse(txtValorParcial.Text, out lValor);

                if (lValor == 0)
                {
                    ExibirMensagemJsOnLoad("I", "Antes de prosseguir, favor informar o valor do resgate.");

                    return;
                }

                gLogger.InfoFormat("Financeiro/Retirada.aspx: SolicitarRetirada() > Valor solicitado pelo cliente [{0}]: [{1}]; Saldo: [{2}]"
                                    , SessaoClienteLogado.CodigoPrincipal
                                    , lValor
                                    , lTransporte.Acoes_SaldoD0);

                if (lTransporte.Acoes_SaldoD0 <= 0)
                {
                    ExibirMensagemJsOnLoad("I", String.Format("Saldo disponível insuficiente para valor da Retirada + taxas."));

                    return;
                }

                if ((lValor + lTotalTaxas + ConfiguracoesValidadas.TaxaTransferencia) > lTransporte.Acoes_SaldoD0)
                {
                    ExibirMensagemJsOnLoad("I", String.Format("Saldo disponível insuficiente para valor da Retirada + taxas.<br>Solicitado: {0}<br>Taxa(s) de Custódia: {1}<br>Taxa de Transf.: {2}", String.Format("{0:C2}", lValor), String.Format("{0:C2}", lTotalTaxas), String.Format("{0:C2}", ConfiguracoesValidadas.TaxaTransferencia)));

                    return;
                }

                gLogger.InfoFormat("Financeiro/Retirada.aspx: SolicitarRetirada() > Verificando conta solicitada para retirada pelo cliente [{0}]", SessaoClienteLogado.CodigoPrincipal);

                foreach (ContaBancariaInfo lConta in SessaoClienteLogado.ContasBancarias)
                {
                    lIdFalsoDaConta = string.Format("rdoConta-{0}-{1}-{2}", lConta.CodigoDoBanco, lConta.NumeroDaAgencia, lConta.NumeroDaConta);

                    if (lContaBancaria.EndsWith(lIdFalsoDaConta))
                    {
                        gLogger.InfoFormat("Financeiro/Retirada.aspx: SolicitarRetirada() > conta solicitada: [{0}]", lIdFalsoDaConta);

                        lContaSelecionadaNaTela = lConta;

                        break;
                    }
                }

                if (lContaSelecionadaNaTela == null)
                {
                    ExibirMensagemJsOnLoad("I", "Antes de prosseguir, favor indicar a conta para transferência dos recursos.");

                    return;
                }
                
                gLogger.InfoFormat("Financeiro/Retirada.aspx: SolicitarRetirada() > Verificando assinatura do cliente [{0}]", SessaoClienteLogado.CodigoPrincipal);

                if (!base.ValidarAssinaturaEletronica(txtAssDigital.Text))
                {
                    ExibirMensagemJsOnLoad("I", "Assinatura eletrônica inválida.");

                    return;
                }

                try
                {
                    //using (var client = new System.Net.WebClient())
                    //{
                    //    client.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";

                    //    Solicitacao solicitacao = new Solicitacao();

                    //    solicitacao.Cliente                     = SessaoClienteLogado.CodigoPrincipal;
                    //    solicitacao.Assessor                    = SessaoClienteLogado.AssessorPrincipal;
                    //    solicitacao.Valor                       = lValor;
                    //    solicitacao.SaldoD0                     = lTransporte.Acoes_SaldoD0;
                    //    solicitacao.SaldoProjetado              = lTransporte.Acoes_SaldoD3;
                    //    solicitacao.Banco                       = lContaSelecionadaNaTela.CodigoDoBanco;
                    //    solicitacao.Agencia                     = lContaSelecionadaNaTela.NumeroDaAgencia;
                    //    solicitacao.AgenciaDig                  = lContaSelecionadaNaTela.DigitoDaAgencia;
                    //    solicitacao.Conta                       = lContaSelecionadaNaTela.NumeroDaConta;
                    //    solicitacao.ContaDig                    = lContaSelecionadaNaTela.DigitoDaAgencia;
                    //    solicitacao.CpfCnpj                     = SessaoClienteLogado.CpfCnpj;
                    //    solicitacao.Solicitante                 = SessaoClienteLogado.CodigoPrincipal;
                    //    solicitacao.Codigo_Origem_Solicitacao   = 2;

                    //    string postObject = Newtonsoft.Json.JsonConvert.SerializeObject(solicitacao);
                    //    var postData = System.Text.Encoding.UTF8.GetBytes(postObject);
                    //    byte[] result = client.UploadData("http://10.0.11.94:8090/Solicitacoes/Create", "POST", postData);
                    //    string resultado = System.Text.Encoding.ASCII.GetString(result);
                    //}

                    gLogger.InfoFormat("Financeiro/Retirada.aspx: SolicitarRetirada() > Assinatura do cliente OK, buscando arquivo para email...");

                    ExibirMensagemJsOnLoad("I", "Solicitação de retirada realizada com sucesso.");

#region Requisição enviada por email
                    lCorpoDoEmail = File.ReadAllText(MapPath(@"..\..\Resc\Emails\NotificacaoRetirada.html"));

                    try
                    {
                        IServicoEmail lServico = InstanciarServicoDoAtivador<IServicoEmail>();

                        EmailInfo lEmail = new EmailInfo();

                        EnviarEmailRequest lRequest = new EnviarEmailRequest();
                        EnviarEmailResponse lResponse;

                        gLogger.InfoFormat("Financeiro/Retirada.aspx: SolicitarRetirada() > Arquivo de email OK, montando conteúdo...");

                        lCorpoDoEmail = lCorpoDoEmail.Replace("###COD_CLIENTE###", SessaoClienteLogado.CodigoPrincipal)
                                                        .Replace("###NOME_CLIENTE###", SessaoClienteLogado.Nome)
                                                        .Replace("###DATA###", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                                                        .Replace("###BANCO###", string.Format("{0} - {1}", lContaSelecionadaNaTela.CodigoDoBanco, lContaSelecionadaNaTela.NomeDoBanco))
                                                        .Replace("###AGENCIA###", string.Format("{0} - {1}", lContaSelecionadaNaTela.NumeroDaAgencia, lContaSelecionadaNaTela.DigitoDaAgencia))
                                                        .Replace("###CONTA###", string.Format("{0} - {1}", lContaSelecionadaNaTela.NumeroDaConta, lContaSelecionadaNaTela.DigitoDaConta))
                                                        .Replace("###VALOR###", lValor.ToString());

                        lEmail.CorpoMensagem = lCorpoDoEmail;
                        lEmail.Assunto = "Notificação de Retirada - Portal Gradual";

                        List<string> lListaDeDestinatarios = new List<string>();

                        lListaDeDestinatarios.Add(ConfiguracoesValidadas.Email_NotificacaoDeposito_Destinatarios);

                        lEmail.Destinatarios = lListaDeDestinatarios;
                        lEmail.Remetente = ConfiguracoesValidadas.Email_NotificacaoDeposito_Remetente;

                        lRequest.Objeto = lEmail;

                        gLogger.InfoFormat("Financeiro/Retirada.aspx: SolicitarRetirada() > Enviando email:\r\n{0}\r\n", lCorpoDoEmail);

                        lResponse = lServico.Enviar(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            gLogger.InfoFormat("Financeiro/Retirada.aspx: SolicitarRetirada() > Email enviado com sucesso.");

                            ExibirMensagemJsOnLoad("I", "Solicitação de retirada realizada com sucesso.");

                            this.LimpaCampos();
                        }
                        else
                        {
                            gLogger.ErrorFormat("Resposta com erro do serviço IServicoEmail.Enviar() em Financeiro/Retirada.aspx: [{0}]\r\n{1}"
                                                , lResponse.StatusResposta
                                                , lResponse.DescricaoResposta);

                            ExibirMensagemJsOnLoad("E", "Erro ao enviar requisição de retirada; favor entrar em contato com o atendimento.");
                        }
                    }
                    catch (Exception ex)
                    {
                        gLogger.ErrorFormat("Erro em Financeiro/Retirada.aspx: SolicitarRetirada(pCodigo [{0}]) > [{1}]\r\n{2}"
                                            , SessaoClienteLogado.CodigoPrincipal
                                            , ex.Message
                                            , ex.StackTrace);

                        ExibirMensagemJsOnLoad("E", "Erro ao gerar requisição de retirada; favor entrar em contato com o atendimento.");
                    }
#endregion

                }
                catch (Exception exLeitura)
                {
                    gLogger.ErrorFormat("Erro em de leitura do arquivo de email em Financeiro/Retirada.aspx: SolicitarRetirada(pCodigo [{0}]) > [{1}]\r\n{2}"
                                        , SessaoClienteLogado.CodigoPrincipal
                                        , exLeitura.Message
                                        , exLeitura.StackTrace);

                    ExibirMensagemJsOnLoad("E", "Erro ao gerar requisição de retirada; favor entrar em contato com o atendimento.");
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemJsOnLoad(ex);
            }

            
        }

        private void LimpaCampos()
        {
            this.txtValorParcial.Text = string.Empty;
            this.txtAssDigital.Text = string.Empty;
        }

        #endregion

        #region Event Handlers

        protected string lCodigoClienteformatado = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.ValidarSessao())
            {
                if (!Page.IsPostBack)
                {
                    if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                    {
                        base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                    }

                    CarregarContaBancaria();

                    lCodigoClienteformatado = this.SessaoClienteLogado.CodigoPrincipal.ToCodigoClienteFormatado();

                    DefinirEstadoDaTela();
                }
            }
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            this.PaginaMaster.BreadCrumbVisible = true;

            this.PaginaMaster.Crumb1Text = "Minha Conta";
            this.PaginaMaster.Crumb2Text = "Financeiro";
            this.PaginaMaster.Crumb3Text = "Retiradas";
        }


        protected void btnRetirada_OnClick(object sender, EventArgs e)
        {
            try
            {
                this.SolicitarRetirada();
            }
            catch (Exception ex)
            {
                ExibirMensagemJsOnLoad(ex);
            }
        }

        private TransporteCustodiaInfo BuscarTaxaCustodia()
        {
            TransporteCustodiaInfo lRetorno = new TransporteCustodiaInfo();

            try
            {
                IServicoCustodia lServico = InstanciarServicoDoAtivador<IServicoCustodia>();

                TaxaCustodiaRequest lRequest = new TaxaCustodiaRequest();
                TaxaCustodiaResponse<TaxaCustodiaClienteInfo> lResponse;

                //lRequest.CodigoCliente = SessaoClienteLogado.CodigoPrincipal.DBToInt32();
                lRequest.CodigoCliente = 8;

                gLogger.InfoFormat("", lRequest.CodigoCliente);

                lResponse = lServico.ObterTaxaCustodiaCliente(lRequest);

                gLogger.InfoFormat("", lRequest.CodigoCliente);

                if (lResponse.StatusResposta.Equals(Gradual.OMS.Custodia.Lib.Enum.CriticaMensagemEnum.Sucesso))
                {
                    lRetorno = new TransporteCustodiaInfo(lResponse.ColecaoObjetos);
                }
                else
                {
                    ExibirMensagemJsOnLoad("E", string.Format("Erro: {0}\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemJsOnLoad(ex);
            }

            return lRetorno;
        }

        #endregion
    }

    public class Solicitacao
    {
        public string Cliente { get; set; }
        public string Assessor { get; set; }
        public decimal? Valor { get; set; }
        public decimal? SaldoD0 { get; set; }
        public decimal? SaldoProjetado { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string AgenciaDig { get; set; }
        public string Conta { get; set; }
        public string ContaDig { get; set; }
        public string CpfCnpj { get; set; }
        public string Solicitante { get; set; }
        public Int32 Codigo_Origem_Solicitacao { get; set; }
    }

}