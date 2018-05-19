using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class Resgate : PaginaBaseAutenticada
    {
        List<Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteDadosBancarios> lLista;
        public TransporteSaldoDeConta SaldoDeConta { get; set; }

        private int GetCodigoBMF
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CodBMF"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodigoBovespa
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CodBovespa"], out lRetorno);

                return lRetorno;
            }
        }

        private string ResponderCarregarHtmlComDados()
        {
            Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteSaldoDeConta lSaldo = new Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteSaldoDeConta();

            ClienteInfo lDadosDoCliente = new ClienteInfo(Request["Id"]);

            Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteDadosCompletosPF lClientePf;

            ReceberEntidadeCadastroRequest<ClienteInfo> req = new ReceberEntidadeCadastroRequest<ClienteInfo>()
            {
                CodigoEntidadeCadastro = Request["Id"],
                EntidadeCadastro = lDadosDoCliente,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            lDadosDoCliente = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(req).EntidadeCadastro;

            //lDadosDoCliente.DadosClienteNaoOperaPorContaPropria = this.RecuperarDadosDeClienteNaoOperaPorContaPropria(lDadosDoCliente.IdCliente.Value);

            lClientePf = new Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteDadosCompletosPF(lDadosDoCliente);
            hidAssessor.Value = lClientePf.Assessor.ToString();
            hidUsuarioLogado.Value = base.UsuarioLogado.Id.ToString();
            hidDadosCompletos_PF.Value = Newtonsoft.Json.JsonConvert.SerializeObject(lClientePf);
            
            
            Gradual.OMS.ContaCorrente.Lib.IServicoContaCorrente servicoCC = this.InstanciarServico<Gradual.OMS.ContaCorrente.Lib.IServicoContaCorrente>();
            Gradual.OMS.ContaCorrente.Lib.Mensageria.SaldoContaCorrenteResponse<ContaCorrenteInfo> resCC = servicoCC.ObterSaldoContaCorrente(new Gradual.OMS.ContaCorrente.Lib.Mensageria.SaldoContaCorrenteRequest()
            {
                IdCliente = this.GetCodigoBovespa
            });

            Gradual.OMS.ContaCorrente.Lib.Mensageria.SaldoContaCorrenteResponse<ContaCorrenteBMFInfo> resBMF = servicoCC.ObterSaldoContaCorrenteBMF(new Gradual.OMS.ContaCorrente.Lib.Mensageria.SaldoContaCorrenteRequest()
            {
                IdCliente = this.GetCodigoBMF
            });

            if (resCC.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && resBMF.StatusResposta != OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
            {
                SaldoDeConta = new TransporteSaldoDeConta(resCC.Objeto);
                hidConta.Value = this.GetCodigoBovespa.ToString();
            }
            else if (resCC.StatusResposta != OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && resBMF.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
            {
                SaldoDeConta = new TransporteSaldoDeConta(resBMF.Objeto);
                hidConta.Value = this.GetCodigoBMF.ToString();
            }
            else if (resCC.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && resBMF.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
            {
                SaldoDeConta = new TransporteSaldoDeConta(resCC.Objeto, resBMF.Objeto);
                hidConta.Value = this.GetCodigoBovespa.ToString();
            }

            ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteBancoInfo> lRequest;
            ConsultarEntidadeCadastroResponse<Gradual.Intranet.Contratos.Dados.ClienteBancoInfo> lResponse;

            Gradual.Intranet.Contratos.Dados.ClienteBancoInfo lDados = new Gradual.Intranet.Contratos.Dados.ClienteBancoInfo(Request["Id"]);

            lRequest = new ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteBancoInfo>()
            {
                EntidadeCadastro = lDados,
                IdUsuarioLogado = base.UsuarioLogado.Id,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<Gradual.Intranet.Contratos.Dados.ClienteBancoInfo>(lRequest);

            if (lResponse.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
            {
                var lListaDeBancos = this.BuscarListaDoSinacor(new Gradual.Intranet.Contratos.Dados.SinacorListaInfo(Gradual.Intranet.Contratos.Dados.Enumeradores.eInformacao.Banco));

                Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteDadosBancarios transporte = new App_Codigo.TransporteJson.TransporteDadosBancarios();

                lLista = transporte.TraduzirClienteBancoInfo(lResponse.Resultado, lListaDeBancos);

                rptResgate_Contas.DataSource = lLista;
                rptResgate_Contas.DataBind();
            }
            //else
            //{
            //    RetornarErroAjax("Erro ao consultar os bancos do cliente", lResponse.DescricaoResposta);
            //}

            //hidDadosCompletos_PF.Value = JsonConvert.SerializeObject(lClientePf);

            return string.Empty;    //só para obedecer assinatura
        }

        private string ResponderSolicitarResgate()
        {
            string lResposta = string.Empty;

            try
            {
                string sol = this.Request.Form["Solicitacao"];
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return lResposta;
        }

        private string EnviarEmailResgate()
        {
            string lRetorno = "Erro...";

            var lDicVariaveis = new Dictionary<string, string>();

            string conta = String.Concat("Banco: ", this.Request.Form["Solicitacao[Banco]"], " Agencia: ", this.Request.Form["Solicitacao[Agencia]"], "-", this.Request.Form["Solicitacao[AgnciaDig]"], " Conta: ", this.Request.Form["Solicitacao[Conta]"], "-", this.Request.Form["ContaDig"]);

            lDicVariaveis.Add("###NOME###"  , this.Request.Form["Solicitacao[Nome]"]);
            lDicVariaveis.Add("###CODIGO###", this.Request.Form["Solicitacao[Cliente]"]);
            lDicVariaveis.Add("###CPF###"   , this.Request.Form["Solicitacao[CpfCnpj]"]);
            lDicVariaveis.Add("###CONTA###" , conta);
            lDicVariaveis.Add("###VALOR###" , this.Request.Form["Solicitacao[Valor]"]);

            base.EnviarEmail("mmaebara@gradualinvestimentos.com.br", "Reserva", "EmailResgate.htm", lDicVariaveis, Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos);

            lRetorno = RetornarSucessoAjax("Email enviado com sucesso!");

            return lRetorno;
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] {   
                                                    "CarregarHtmlComDados"
                                                    , "ResponderSolicitarResgate"
                                                    , "EnviaremailResgate"
                                                },
            new ResponderAcaoAjaxDelegate[] { 
                                                ResponderCarregarHtmlComDados
                                              , ResponderSolicitarResgate
                                              , EnviarEmailResgate
                                            });
            
            if (!Page.IsPostBack)
            {
                //this.PopularControleComListaDoCadastro<Gradual.Intranet.Contratos.Dados.ClienteBancoInfo>(lLista, rptResgate_Contas);
            }
        }
    }
}