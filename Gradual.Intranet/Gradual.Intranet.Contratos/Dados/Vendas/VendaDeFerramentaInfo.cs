using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class VendaDeFerramentaInfo : ICodigoEntidade
    {
        #region Propriedades
        
        public Nullable<int> Busca_IdVenda { get; set; }

        public Nullable<int> Busca_CdCBLC { get; set; }

        public string Busca_DsCpfCnpj { get; set; }

        public Nullable<int> Busca_StStatus { get; set; }

        public Nullable<DateTime> Busca_DataDe { get; set; }

        public Nullable<DateTime> Busca_DataAte { get; set; }



        public int IdVenda { get; set; }

        public string DsCodigoReferencia { get; set; }

        public int CdCBLC { get; set; }

        public string DsCpfCnpj { get; set; }

        public int StStatus { get; set; }

        
        public string DescricaoStatus
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

                switch (this.StStatus)
                {
                    case 1: return "Aguardando Pagamento";
                    case 2: return "Em Análise";
                    case 3: return "Paga";
                    case 4: return "Disponível";
                    case 5: return "Em Disputa";
                    case 6: return "Devolvida";
                    case 7: return "Cancelada";

                    default:
                        return Convert.ToString(this.StStatus);

                }
            }
        }


        public DateTime DtData { get; set; }

        public int VlQuantidade { get; set; }

        public decimal VlPreco { get; set; }

        public int IdProduto { get; set; }

        public string DsProduto { get; set; }

        public int IdPagamento { get; set; }

        public int CdTipo { get; set; }

        public int CdMetodoTipo { get; set; }

        public int CdMetodoCodigo { get; set; }

        public string DsMetodoDesc { get; set; }

        public decimal VlValorBruto { get; set; }

        public decimal VlValorDesconto { get; set; }

        public decimal VlValorTaxas { get; set; }
        
        public decimal VlValorTaxaProduto { get; set; }

        public decimal VlValorLiquido { get; set; }

        public decimal VlQuantidadeParcelas { get; set; }

        public string DsObservacoes { get; set; }

        public ClienteEnderecoInfo EnderecoDeEntrega { get; set; }

        private string _TelDeEntrega;
        
        private string _CelDeEntrega;

        public string TelDeEntrega
        {
            get
            {
                return _TelDeEntrega;
            }

            set
            {
                _TelDeEntrega = value;

                if (string.IsNullOrEmpty(_TelDeEntrega.Replace("(", "").Replace(")", "").Trim()))
                    _TelDeEntrega = "";
            }
        }

        public string CelDeEntrega
        {
            get
            {
                return _CelDeEntrega;
            }

            set
            {
                _CelDeEntrega = value;

                if (string.IsNullOrEmpty(_CelDeEntrega.Replace("(", "").Replace(")", "").Trim()))
                    _CelDeEntrega = "";
            }
        }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
