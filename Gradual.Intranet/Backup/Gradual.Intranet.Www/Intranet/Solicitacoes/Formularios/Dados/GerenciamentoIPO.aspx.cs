using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Library;
using Newtonsoft.Json;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados.Vendas;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Monitor.Custodia.Lib;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using Gradual.OMS.Monitor.Custodia.Lib.Info;

namespace Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Dados
{
    /// <summary>
    /// Classe responsável pelo gerenciamento do IPO - 
    /// Cadastro e manutenção de Ofertas públicas e reservas efetuadas pelo site e pela intranet
    /// </summary>
    public partial class GerenciamentoIPO : PaginaBaseAutenticada
    {
        #region Propriedades
        /// <summary>
        /// Request do Código de cliente para ser usado na atualização de limites
        /// </summary>
        public int? RequestCodigoCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodigoCliente"], out lRetorno))
                    return null;

                return lRetorno;

            }
        }

        /// <summary>
        /// Request do cpf do cliente para ser usado na atualização de limites
        /// </summary>
        public string CpfCnpj
        {
            get 
            {
                return Request["CpfCnpj"].ToString();
            }
        }

        /// <summary>
        /// Request do CodigoIPOCliente para ser usado na atualização de limites
        /// </summary>
        public string RequestCodigoIPOCliente
        {
            get 
            {
                return Request["CodigoIPOCliente"].ToString();
            }
        }
        #endregion
        
        #region Métodos
        /// <summary>
        /// Carrega os dados no formulário
        /// </summary>
        /// <returns></returns>
        private string ResponderCarregarHtmlComDados()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<IPOInfo>();
            ConsultarEntidadeCadastroResponse<IPOInfo> lResponse = null ;

            lRequest.EntidadeCadastro = new IPOInfo();

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<IPOInfo>(lRequest);

            base.RegistrarLogConsulta("Consulta de Dados de Gerencimaneto de Reserva de IPO ");

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                rptListaDeProdutos.DataSource = lResponse.Resultado;
                rptListaDeProdutos.DataBind();

                rowLinhaDeNenhumItem.Visible = (lResponse.Resultado.Count == 0);
            }

            return string.Empty;    //só para obedecer assinatura
        }

        /// <summary>
        /// Seleciona IPO 
        /// </summary>
        /// <param name="pCodigoISIN">Código ISIN do IPO a ser selecionado</param>
        /// <returns>REtorna o Objeto IPO </returns>
        public IPOInfo SelecionarIPOInfo(string pCodigoISIN)
        {
            var lRetorno = new IPOInfo();
            var lRequest = new ConsultarEntidadeCadastroRequest<IPOInfo>();
            ConsultarEntidadeCadastroResponse<IPOInfo> lResponse = null;

            lRequest.EntidadeCadastro = new IPOInfo();

            lRequest.EntidadeCadastro.CodigoISIN = pCodigoISIN;

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<IPOInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lRetorno = lResponse.Resultado[0];
            }

            return lRetorno;
        }

        /// <summary>
        /// Salva os dados de IPO de Cliente (Solicitação de Reserva de oferta publica)
        /// </summary>
        /// <returns>Retorna uma string com a mensagem de sucesso ou erro com os dados do IPO cliente encapsulado</returns>
        public string ResponderSalvarIPOCliente()
        {
            string lRetorno = "";

            string lJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lJson))
            {
                try
                {
                    var lTransporte = JsonConvert.DeserializeObject<TransporteDadosIPOCliente>(lJson);

                    try
                    {
                        var lProduto = new IPOClienteInfo();

                        lProduto = lTransporte.ToProdutoIPOClienteInfo();

                        //lProduto.IdPlano = 2;   //fixo: "Cambio"

                        var lRequest = new SalvarEntidadeCadastroRequest<IPOClienteInfo>();

                        SalvarEntidadeCadastroResponse lResponse = null;

                        lRequest.EntidadeCadastro = lProduto;

                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<IPOClienteInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lTransporte.CodigoIPOCliente = ((IPOClienteInfo)lResponse.Objeto).CodigoIPOCliente.Value.ToString();


                            lRetorno = RetornarSucessoAjax(lTransporte, "Dados salvos com sucesso");
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                        }

                        this.EnviarEmailMudancaStatusReserva(lProduto);

                        base.RegistrarLogAlteracao("Alteração de Solicitação de reserva de IPO ->" + this.FormatarDadosLog(lProduto));
                    }
                    catch (Exception ex)
                    {
                        lRetorno = RetornarErroAjax("Erro ao salvar objeto", ex);
                    }
                }
                catch (Exception exJson)
                {
                    lRetorno = RetornarErroAjax("Erro ao deserializar objeto JSON [{0}]", exJson, lJson);
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Salva os dados de IPO no banco de dados 
        /// </summary>
        /// <returns>Retorna uma stringcom a mnesagem de sucesso ou erro</returns>
        public string ResponderSalvarIPO()
        {
            string lRetorno = "";

            string lJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lJson))
            {
                try
                {
                    var lTransporte = JsonConvert.DeserializeObject<TransporteDadosIPO>(lJson);

                    IPOInfo lProduto = null;
                    
                    try
                    {
                        lProduto = lTransporte.ToProdutoIPOClienteInfo();

                        //lProduto.IdPlano = 2;   //fixo: "Cambio"

                        var lRequest = new SalvarEntidadeCadastroRequest<IPOInfo>();

                        SalvarEntidadeCadastroResponse lResponse = null ;

                        lRequest.EntidadeCadastro = lProduto;

                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<IPOInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lTransporte.CodigoIPO = ((IPOInfo)lResponse.Objeto).CodigoIPO.Value.ToString();

                            lRetorno = RetornarSucessoAjax(lTransporte, "Dados salvos com sucesso");
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                        }
                    }
                    catch (Exception ex)
                    {
                        lRetorno = RetornarErroAjax("Erro ao salvar objeto", ex);
                    }

                    base.RegistrarLogAlteracao("Inclusão/Alteração de Gerencimento de IPO ->" + this.FormatarDadosLog(lProduto));
                }
                catch (Exception exJson)
                {
                    lRetorno = RetornarErroAjax("Erro ao deserializar objeto JSON [{0}]", exJson, lJson);
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que dados de log com  objeto e seus valores para 
        /// </summary>
        /// <param name="pInfo">INfo com as classes e suas propriedades que irão ser gravadas</param>
        /// <returns>Retorna uma string formatada com as propriedades e seus valores</returns>
        public string FormatarDadosLog(object pInfo)
        {
            string lConteudo = string.Empty ;
            
            System.Reflection.PropertyInfo[] propriedades = pInfo.GetType().GetProperties();

            foreach (System.Reflection.PropertyInfo item in propriedades)
            {
                lConteudo += " ; [" + item.Name + "] ";
                lConteudo += item.GetValue(pInfo, null).ToString();
            }

            return lConteudo;
            
        }

        /// <summary>
        /// Método que busca os valores de conta-corrente no serviço de contacorrente
        /// </summary>
        /// <returns>Retorna uma string json com os valores de contacorrente do cliente selecionado</returns>
        public string ResponderBuscarLimites()
        {
            string lRetorno = string.Empty;

            int lCodigoCliente = RequestCodigoCliente.Value;

            try
            {
                IServicoContaCorrente servicoCC = this.InstanciarServico<IServicoContaCorrente>();
                
                decimal lTotalSaldoCC = 0M;

                var lResponseSaldo = servicoCC.ObterSaldoContaCorrente(new SaldoContaCorrenteRequest()
                {
                    IdCliente = lCodigoCliente
                });

                if (lResponseSaldo != null && lResponseSaldo.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    lTotalSaldoCC = lResponseSaldo.Objeto.SaldoD0;
                }

                ///Buscanso valor de fundos
                List<Transporte_PosicaoCotista> ListaPosicao = base.PosicaoFundosSumarizado(lCodigoCliente, CpfCnpj);

                decimal lTotalFundos = 0M;

                foreach (var lFundo in ListaPosicao)
                {
                    lTotalFundos += decimal.Parse(lFundo.ValorLiquido);
                }


                var lRequestCustodia = new MonitorCustodiaRequest();

                var lResponseCustodia = new MonitorCustodiaResponse();

                var gServicoCustodia = Ativador.Get<IServicoMonitorCustodia>();

                lRequestCustodia.CodigoCliente = lCodigoCliente;

                lResponseCustodia = gServicoCustodia.ObterMonitorCustodiaMemoria(lRequestCustodia);

                var lRetornoCustodia = new List<TransporteCustodiaInfo>();

                IEnumerable<MonitorCustodiaInfo.CustodiaPosicao> Lista = from a in lResponseCustodia.MonitorCustodia.ListaCustodia orderby a.Resultado descending select a;

                lRetornoCustodia = TransporteCustodiaInfo.TraduzirCustodiaInfo(Lista.ToList());

                var lListaCustodia =lRetornoCustodia;

                decimal lSomatoriaCustodia = 0M;

                IServicoCotacao lServicoCotacao = Ativador.Get<IServicoCotacao>();

                foreach (TransporteCustodiaInfo lCus in lListaCustodia)
                {
                    var lCotacao = lServicoCotacao.ReceberTickerCotacao(lCus.CodigoNegocio);

                    var lMsgCotacao = new Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteMensagemDeNegocio(lCotacao);

                    lSomatoriaCustodia += ( Convert.ToDecimal( lCus.QtdAtual)* Convert.ToDecimal( lMsgCotacao.Preco));
                }

                decimal lTotalLimiteSomado = (lTotalFundos + lSomatoriaCustodia + lTotalSaldoCC);

                var lObjetoRetorno = new ClienteCallBackLimite( lCodigoCliente, RequestCodigoIPOCliente, lTotalLimiteSomado );

                lRetorno = RetornarSucessoAjax(lObjetoRetorno, "Limite do cliente " + lCodigoCliente + " atualizado com sucesso"); 
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao Buscar dados de limites", ex);
            }
            
            return lRetorno;
        }

        /// <summary>
        /// Struct apara tratamento do callback no objeto de retorno
        /// </summary>
        public class ClienteCallBackLimite
        {
            /// <summary>
            /// Código do Cliente
            /// </summary>
            public int    CodigoCliente {get; set;}

            /// <summary>
            /// Código IPO do Cliente solicitado 
            /// </summary>
            public string CodigoIPOCliente { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public decimal TotalLimiteSomado { get; set; }

            /// <summary>
            /// Construtor para recer os requests da atualização de limites de clientes
            /// </summary>
            /// <param name="pCodigoCliente">Código do cliente</param>
            /// <param name="pCodigoIPOCliente">Código do IPO do cliente</param>
            /// <param name="pTotalLimiteSomado">Limite atualizado e somado do cliente</param>
            public ClienteCallBackLimite(int pCodigoCliente, string pCodigoIPOCliente, decimal pTotalLimiteSomado)
            {
                this.CodigoCliente     = pCodigoCliente;
                this.CodigoIPOCliente  = pCodigoIPOCliente;
                this.TotalLimiteSomado = pTotalLimiteSomado;
            }
        }

        /// <summary>
        /// Envia email de mudança de troca de status de reserva de ipo
        /// </summary>
        /// <returns>Retorna uma string com o status do método de envio de email</returns>
        public string EnviarEmailMudancaStatusReserva(IPOClienteInfo pInfo)
        {
            string lRetorno = string.Empty;

            try
            {
                List<Contratos.Dados.ClienteResumidoInfo> lDadosCliente = base.ConsultarClienteResumido_DadosBasicos(pInfo.CpfCnpj);

                if (lDadosCliente == null || lDadosCliente.Count == 0)
                {
                    return RetornarErroAjax("O Status foi alterado, porém, não foi possível o envio de email para o cliente pois o cliente não foi encontrado.");
                }

                var lEmailCliente = lDadosCliente[0].Email;

                var lDicVariaveis = new Dictionary<string,string>();

                var lIpo = this.SelecionarIPOInfo(pInfo.CodigoISIN);

                //lDicVariaveis.Add("##ativo##", pInfo.)

                lDicVariaveis.Add("##numeroprotocolo##", pInfo.NumeroProtocolo);

                lDicVariaveis.Add("##nomecliente##", pInfo.NomeCliente);

                lDicVariaveis.Add("##codigoisin##", pInfo.CodigoISIN);

                lDicVariaveis.Add("##empresa##", pInfo.Empresa);

                lDicVariaveis.Add("##modalidade##", pInfo.Modalidade);

                lDicVariaveis.Add("##datainicial##", lIpo.DataInicial.ToString("dd/MM/yyyy"));

                lDicVariaveis.Add("##datafinal##", lIpo.DataFinal.ToString("dd/MM/yyyy") );

                lDicVariaveis.Add("##horamaxima##", lIpo.HoraMaxima);

                lDicVariaveis.Add("##valorminimo##", lIpo.VlMinimo.ToString("n2"));

                lDicVariaveis.Add("##valormaximo##", lIpo.VlMaximo.ToString("n2"));

                lDicVariaveis.Add("##percentualgarantia##", lIpo.VlPercentualGarantia.ToString("n2"));

                lDicVariaveis.Add("##ativo##", lIpo.Ativo);

                lDicVariaveis.Add("##observacoes##", pInfo.Observacoes);

                if (pInfo.Status == Contratos.Dados.Enumeradores.eStatusIPO.Cancelada)
                {
                    base.EnviarEmail(lEmailCliente, "Reserva de oferta pública cancelada - protocolo - " + pInfo.NumeroProtocolo,"AvisoReservaIPOCancelada.htm",lDicVariaveis, Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos);
                }else if (pInfo.Status == Contratos.Dados.Enumeradores.eStatusIPO.Executada)
                {
                    base.EnviarEmail(lEmailCliente,"Reserva de oferta pública executada - protocolo - " + pInfo.NumeroProtocolo, "AvisoReservaIPOExecutada.htm", lDicVariaveis, Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos);
                }
            }
            catch (Exception ex)
            {
               lRetorno = RetornarErroAjax("Erro ao enviar email de aviso de troca de status de de reserva", ex);
            }

            return lRetorno;
        }
        #endregion

        #region Eventos
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] {
                                                    "CarregarHtmlComDados"
                                                  , "Salvar"
                                                  , "SalvarSolicitacao"
                                                  , "BuscarLimites"
                                                },
                new ResponderAcaoAjaxDelegate[] {
                                                    ResponderCarregarHtmlComDados
                                                  , ResponderSalvarIPO
                                                  , ResponderSalvarIPOCliente
                                                  , ResponderBuscarLimites
                                                });
        }
        #endregion
    }
}