using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Www
{
    public class TransporteDadosCarrinho
    {
        #region Propriedades

        public List<TransporteProduto> Produtos { get; set; }

        public TransporteCadastroEndereco EndEntrega { get; set; }

        #endregion

        #region Construtor

        public TransporteDadosCarrinho()
        {
            this.Produtos = new List<TransporteProduto>();

            this.EndEntrega = new TransporteCadastroEndereco();
        }

        #endregion
    }
}