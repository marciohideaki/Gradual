using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.Lib.Dados.MinhaConta.Comercial
{
    public class CompraInfo
    {
        #region Propriedades

        public VendaInfo DadosDaCompra { get; set; }

        public string Dados_Data
        {
            get
            {
                return this.DadosDaCompra.Data.ToString("dd/MM/yy HH:mm");
            }
        }

        public string Dados_Status
        {
            get
            {
                //direto da API do PagSeguro:
                /*
                    1 Aguardando pagamento: o comprador iniciou a transação, mas até o momento o PagSeguro não recebeu nenhuma informação sobre o pagamento.
                    2 Em análise: o comprador optou por pagar com um cartão de crédito e o PagSeguro está analisando o risco da transação.
                    3 Paga: a transação foi paga pelo comprador e o PagSeguro já recebeu uma confirmação da instituição financeira responsável pelo processamento.
                    4 Disponível: a transação foi paga e chegou ao final de seu prazo de liberação sem ter sido retornada e sem que haja nenhuma disputa aberta.
                    5 Em disputa: o comprador, dentro do prazo de liberação da transação, abriu uma disputa.
                    6 Devolvida: o valor da transação foi devolvido para o comprador.
                    7 Cancelada: a transação foi cancelada sem ter sido finalizada. 
                 */

                switch (this.DadosDaCompra.Status)
                {
                    case 1: return "Aguardando Pagamento";
                    case 2: return "Em Análise";
                    case 3: return "Paga";
                    case 4: return "Disponível";
                    case 5: return "Em Disputa";
                    case 6: return "Devolvida";
                    case 7: return "Cancelada";

                    default:
                        return Convert.ToString(this.DadosDaCompra.Status);

                }
            }
        }

        public string Dados_Total
        {
            get
            {
                if (this.Pagamentos != null && this.Pagamentos.Count > 0)
                {
                    if (this.DadosDaCompra.Status < 3)
                    {
                        return "(não processado)";
                    }
                    else
                    {
                        if (this.Pagamentos[0].ValorLiquido == 0)
                        {
                            //compra foi aprovada via intranet, não tem pagamento, então põe o preço do produto:
                            return string.Format("{0:n}", (this.Produtos[0].PrecoAtual * this.Produtos[0].PrecoAtual));
                        }
                        else
                        {
                            return string.Format("{0:n}", (this.Pagamentos[0].ValorBruto - this.Pagamentos[0].ValorDesconto));  //valorLiquido tira as taxas que o PagSeguro comprou
                        }
                    }
                }
                else
                {
                    return "-0";
                }
            }
        }

        public List<CompraProdutoInfo> Produtos { get; set; }

        public string ProdutosLegiveis
        {
            get
            {
                string lRetorno = "";

                if (this.Produtos != null)
                {
                    foreach (CompraProdutoInfo lProduto in this.Produtos)
                    {
                        lRetorno += string.Format("{0}x {1}<br />\r\n", lProduto.Quantidade, lProduto.Descricao);
                    }
                }

                return lRetorno;
            }
        }

        public List<PagamentoInfo> Pagamentos { get; set; }

        public string PagamentosLegiveis
        {
            get
            {
                string lRetorno = "";

                if (this.Pagamentos != null)
                {
                    if (this.DadosDaCompra.Status < 3)
                    {
                        return "(não processado)";
                    }
                    else
                    {
                        foreach (PagamentoInfo lPagamento in this.Pagamentos)
                        {
                            if (lPagamento.MetodoCodigo == 0)
                            {
                                //a compra foi aprovada (status >= 3) porém não tem pagamento; foi alterada manualmente na intranet
                                lRetorno += "Aprovação Direta";
                            }
                            else
                            {
                                lRetorno += string.Format("{1} ({0}) - {2}\r\n"
                                                            , lPagamento.MetodoCodigo
                                                            , lPagamento.MetodoCodigoDesc
                                                            , lPagamento.MetodoTipoDesc);
                            }
                        }
                    }
                }

                return lRetorno;
            }
        }

        #endregion

        #region Construtores

        public CompraInfo()
        {
            this.Produtos = new List<CompraProdutoInfo>();
            this.Pagamentos = new List<PagamentoInfo>();
        }


        #endregion
    }


    public class CompraProdutoInfo
    {
        #region Propriedades

        public int IdProduto { get; set; }

        public string Descricao { get; set; }

        public decimal PrecoNaCompra { get; set; }
        
        public decimal PrecoAtual { get; set; }

        public int Quantidade { get; set; }

        #endregion
    }
}
