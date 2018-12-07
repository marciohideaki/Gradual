using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.Comercial
{
    public class VendaProdutoInfo
    {
        #region Propriedades

        public int IdVenda { get; set; }

        public int IdProduto { get; set; }

        public int Quantidade { get; set; }

        private decimal _Preco { get; set; }

        public decimal Preco
        {
            get
            {
                return decimal.Round(_Preco, 2);    //"hack" pra ele serializar com duas casas
            }

            set
            {
                _Preco = value;
            }
        }

        public decimal Taxas { get; set; }

        public decimal TaxasTotal { get; set; }

        public string DescricaoProduto { get; set; }

        public string Obervacoes { get; set; }

        #endregion
    }
}
