using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Gradual.Site.DbLib.Dados.MinhaConta.Comercial;
using Gradual.Site.DbLib;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.Www.Async
{
    public partial class NotificacoesExternas : PaginaBase
    {
        #region Globais

        private CultureInfo gInfo = new CultureInfo("pt-BR");

        #endregion

        private string ResponderNotificarCompraDeTravelCard()
        {
            string lRetorno = "";

            int lQuantidade;

            string lMoeda;

            decimal lTaxa, lValorEmReais;

            string lCPF;

            lCPF = Request["CPF"];

            lMoeda = Request["Moeda"];

            gLogger.InfoFormat("Recebida notificação externa de TravelCard para CPF [{0}]", lCPF);

            string lMensagem;

            //   /Async/NotificacoesExternas.aspx?Acao=NotificarCompraDeTravelCard&CPF=84160556965&Moeda=Dolar&Quantidade=1&Taxa=1,51&ValorEmReais=123,45
            try
            {

                if (decimal.TryParse(Request["Taxa"], NumberStyles.Any, gInfo, out lTaxa))
                {
                    if (decimal.TryParse(Request["ValorEmReais"], NumberStyles.Any, gInfo, out lValorEmReais))
                    {
                        if (int.TryParse(Request["Quantidade"], NumberStyles.Any, gInfo, out lQuantidade))
                        {
                            if (!string.IsNullOrEmpty(lMoeda))
                            {
                                if (!string.IsNullOrEmpty(lCPF))
                                {
                                    gLogger.InfoFormat("Parâmetros: Taxa [{0}], Valor [{1}], Quantidade [{2}], Moeda [{3}]", lTaxa, lValorEmReais, lQuantidade, lMoeda);

                                    IServicoPersistenciaSite lServico = InstanciarServicoDoAtivador<IServicoPersistenciaSite>();

                                    InserirVendaRequest lVendaRequest = new InserirVendaRequest();
                                    InserirVendaResponse lVendaResponse;

                                    InserirPagamentoRequest lPagtoRequest = new InserirPagamentoRequest();
                                    InserirPagamentoResponse lPagtoResponse;

                                    VendaProdutoInfo lVendaProdutoInfo = new VendaProdutoInfo();

                                    lVendaRequest.Venda = new VendaInfo();

                                    lVendaRequest.Venda.CpfCnpjCliente = lCPF.Replace(".", "").Replace("-", "");
                                    
                                    /*
                                     Status:
                                        3 Paga: a transação foi paga pelo comprador e o PagSeguro já recebeu uma confirmação da instituição financeira responsável pelo processamento.
                                        4 Disponível: a transação foi paga e chegou ao final de seu prazo de liberação sem ter sido retornada e sem que haja nenhuma disputa aberta.
                                     */

                                    lVendaRequest.Venda.Data = DateTime.Now;
                                    lVendaRequest.Venda.ReferenciaDaVenda = string.Format("TravelCard_" + lCPF);
                                    lVendaRequest.Venda.Status = 3;

                                    lVendaProdutoInfo.IdProduto = ConfiguracoesValidadas.IdDoProduto_GradualTravelCard;
                                    lVendaProdutoInfo.Quantidade = lQuantidade;
                                    lVendaProdutoInfo.Preco = lValorEmReais;

                                    lVendaRequest.Venda.Produtos = new List<VendaProdutoInfo>();
                                    lVendaRequest.Venda.Produtos.Add(lVendaProdutoInfo);
                                    
                                    gLogger.InfoFormat("Inserindo venda [{0}]", lVendaRequest.Venda.ReferenciaDaVenda);

                                    lVendaResponse = lServico.InserirVenda(lVendaRequest);

                                    if (lVendaResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                                    {
                                        gLogger.InfoFormat("Resposta OK do serviço: [{0}]", lVendaResponse.IdDoRegistroIncluido);

                                        lPagtoRequest.Pagamento = new PagamentoInfo();

                                        lPagtoRequest.Pagamento.IdVenda = lVendaResponse.IdDoRegistroIncluido;

                                        lPagtoRequest.Pagamento.MetodoTipo = 1;
                                        lPagtoRequest.Pagamento.MetodoCodigo = 109;
                                        lPagtoRequest.Pagamento.ValorBruto = lValorEmReais;
                                        lPagtoRequest.Pagamento.ValorTaxas = lTaxa;
                                        lPagtoRequest.Pagamento.QuantidadeDeParcelas = 1;

                                        gLogger.Info("Inserindo pagamento...");

                                        lPagtoResponse = lServico.InserirPagamento(lPagtoRequest);

                                        if (lPagtoResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                                        {
                                            gLogger.InfoFormat("Pagamento inserido com sucesso! [{0}] Fim.", lPagtoResponse.IdDoRegistroIncluido);

                                            lRetorno = RetornarSucessoAjax("Notificação de venda recebida com sucesso!");
                                        }
                                        else
                                        {
                                            lMensagem = string.Format("Erro Interno: Resposta com erro do Serviço ao InserirPagamento: [{0}], [{1}]", lPagtoResponse.StatusResposta, lPagtoResponse.DescricaoResposta);

                                            gLogger.Error(lMensagem);

                                            lRetorno = RetornarErroAjax(lMensagem);
                                        }
                                    }
                                    else
                                    {
                                        lMensagem = string.Format("Erro Interno: Resposta com erro do Serviço ao InserirVenda: [{0}], [{1}]", lVendaResponse.StatusResposta, lVendaResponse.DescricaoResposta);

                                        gLogger.Error(lMensagem);

                                        lRetorno = RetornarErroAjax(lMensagem);
                                    }
                                }
                                else
                                {
                                    lRetorno = RetornarErroAjax("Validação: Valor vazio para CPF (esperando parâmetro [CPF]).");
                                }
                            }
                            else
                            {
                                lRetorno = RetornarErroAjax("Validação: Valor vazio para Moeda (esperando parâmetro [Moeda]).");
                            }
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(string.Format("Validação: Valor para Quantidade [{0}] não é um int válido (esperando parâmetro [Quantidade]).", Request["Quantidade"]));
                        }
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax(string.Format("Validação: Valor em Reais [{0}] não é um decimal válido (esperando parâmetro [ValorEmReais]).", Request["ValorEmReais"]));
                    }
                }
                else
                {
                    lRetorno = RetornarErroAjax(string.Format("Validação: Valor para Taxa [{0}] não é um decimal válido (esperando parâmetro [Taxa]).", Request["Taxa"]));
                }

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro inesperado em Async/NotificacoesExternas.aspx: [{0}]\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegistrarRespostasAjax(new string[]{
                                                    "NotificarCompraDeTravelCard"
                                                },
                    new ResponderAcaoAjaxDelegate[]{
                                                    ResponderNotificarCompraDeTravelCard
                                                });
        }
    }
}