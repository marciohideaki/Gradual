using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.Comercial
{
    /// <summary>
    /// Classe de transação, que vai para a tabela tb_transacao; são dados resumidos do XML que vem do gateway (resumidos porque dados extensos do cliente e de shipment não nos interessam)
    /// </summary>
    public class TransacaoInfo
    {
        #region Globais

        private Dictionary<int, string> gDescricoesDeMetodoTipo;

        private Dictionary<int, string> gDescricoesDeMetodoCodigo;

        #endregion

        #region Propriedades

        public int IdTransacao { get; set; }

        public DateTime Data { get; set; }

        public string CodigoDaTransacaoNoGateway { get; set; }

        public string CodigoDeReferenciaDaVenda { get; set; }

        /// <summary>
        /// Por enquanto, segundo a api do PagSeguro, é sempre 1
        /// </summary>
        public int Tipo { get; set; }
        
        public int Status { get; set; }

        public int MetodoTipo { get; set; }

        public int MetodoTipoDesc { get; set; }

        public int MetodoCodigo { get; set; }

        public string MetodoCodigoDesc
        {
            get
            {
                if (gDescricoesDeMetodoCodigo.ContainsKey(this.MetodoCodigo))
                {
                    return gDescricoesDeMetodoCodigo[this.MetodoCodigo];
                }
                else
                {
                    return null;
                }
            }
        }

        public decimal ValorBruto { get; set; }

        public decimal ValorDesconto { get; set; }

        public decimal ValorTaxas { get; set; }

        public decimal ValorLiquido { get; set; }

        public int QuantidadeDeParcelas { get; set; }

        /// <summary>
        /// Quantidade de itens na compra; praticamente sempre 1
        /// </summary>
        public int QuantidadeDeItens { get; set; }

        /// <summary>
        /// Descrição do primeiro item da venda; só o primeiro porque em praticamente todos os casos só haverá 1, e simplifica o log
        /// </summary>
        public string Item1_Descricao { get; set; }

        public string Cliente_Email { get; set; }

        public string Cliente_Nome { get; set; }

        /// <summary>
        /// Adicionando essa informação porque se vier do PagSeguro podemos aproveitar...
        /// </summary>
        public string Cliente_Telefone { get; set; }

        /// <summary>
        /// XML completo do que foi recebido do gateway, para consulta caso a "simplificação" dessa classe omite algum dado que precise ser consultado
        /// </summary>
        public string XmlRecebidoDoGateway { get; set; }

        #endregion

        #region Construtores

        public TransacaoInfo()
        {
            this.Tipo = 1;
            
            gDescricoesDeMetodoCodigo = new Dictionary<int, string>();
            gDescricoesDeMetodoTipo = new Dictionary<int, string>();
            
            gDescricoesDeMetodoTipo.Add(1, "Cartão de crédito");
            gDescricoesDeMetodoTipo.Add(2, "Boleto");
            gDescricoesDeMetodoTipo.Add(3, "Débito Online");
            gDescricoesDeMetodoTipo.Add(4, "Saldo PagSeguro");
            gDescricoesDeMetodoTipo.Add(5, "Oi Paggo");

            gDescricoesDeMetodoCodigo.Add(101, "Cartão de crédito Visa");
            gDescricoesDeMetodoCodigo.Add(102, "Cartão de crédito MasterCard");
            gDescricoesDeMetodoCodigo.Add(103, "Cartão de crédito American Express");
            gDescricoesDeMetodoCodigo.Add(104, "Cartão de crédito Diners");
            gDescricoesDeMetodoCodigo.Add(105, "Cartão de crédito Hipercard");
            gDescricoesDeMetodoCodigo.Add(106, "Cartão de crédito Aura");
            gDescricoesDeMetodoCodigo.Add(107, "Cartão de crédito Elo");
            gDescricoesDeMetodoCodigo.Add(108, "Cartão de crédito PLENOCard");
            gDescricoesDeMetodoCodigo.Add(109, "Cartão de crédito PersonalCard");
            gDescricoesDeMetodoCodigo.Add(110, "Cartão de crédito JCB");
            gDescricoesDeMetodoCodigo.Add(111, "Cartão de crédito Discover");
            gDescricoesDeMetodoCodigo.Add(112, "Cartão de crédito BrasilCard");
            gDescricoesDeMetodoCodigo.Add(113, "Cartão de crédito FORTBRASIL");
            gDescricoesDeMetodoCodigo.Add(201, "Boleto Bradesco");
            gDescricoesDeMetodoCodigo.Add(202, "Boleto Santander");
            gDescricoesDeMetodoCodigo.Add(301, "Débito online Bradesco");
            gDescricoesDeMetodoCodigo.Add(302, "Débito online Itaú");
            gDescricoesDeMetodoCodigo.Add(303, "Débito online Unibanco *");
            gDescricoesDeMetodoCodigo.Add(304, "Débito online Banco do Brasil");
            gDescricoesDeMetodoCodigo.Add(305, "Débito online Banco Real. *");
            gDescricoesDeMetodoCodigo.Add(306, "Débito online Banrisul");
            gDescricoesDeMetodoCodigo.Add(307, "Débito online HSBC");
            gDescricoesDeMetodoCodigo.Add(401, "Saldo PagSeguro");
            gDescricoesDeMetodoCodigo.Add(501, "Oi Paggo");

        }

        #endregion
    }
}
