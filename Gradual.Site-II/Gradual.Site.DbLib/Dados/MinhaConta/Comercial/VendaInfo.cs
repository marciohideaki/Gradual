using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.Comercial
{
    public class VendaInfo
    {
        #region Propriedades

        public int IdVenda { get; set; }

        public int CblcCliente { get; set; }

        public string CpfCnpjCliente { get; set; }

        public string ReferenciaDaVenda { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// Valor a mais que pode ser adicionado/removido do preço em caso de descontos, etc. (existe na API do MercadoPago)
        /// </summary>
        public decimal ValorExtra { get; set; }

        public DateTime Data { get; set; }

        public List<VendaProdutoInfo> Produtos { get; set; }

        public Intranet.Contratos.Dados.ClienteEnderecoInfo EnderecoDeEntrega { get; set; }

        public Intranet.Contratos.Dados.ClienteTelefoneInfo CelularDeEntrega { get; set; }
        
        public Intranet.Contratos.Dados.ClienteTelefoneInfo TelefoneDeEntrega { get; set; }

        #endregion

        #region Construtores

        public VendaInfo()
        {
            this.Data = DateTime.Now;

            this.Produtos = new List<VendaProdutoInfo>();

            this.ReferenciaDaVenda = string.Format("{0}_{1}", DateTime.Now.ToString("HHmmssffff"), Guid.NewGuid().ToString());
        }

        #endregion
    }
}
