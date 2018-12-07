using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.Comercial
{
    public class ClienteProdutoInfo
    {
        #region Propriedades

        /// <summary>
        /// Somente usado quando for incluído via transação
        /// </summary>
        public int? IdDaTransacao { get; set; }

        public string CpfCnpj { get; set; }

        public int IdProduto { get; set; }

        public DateTime DataAdesao { get; set; }

        /// <summary>
        /// A: Ativo
        /// </summary>
        public string Situacao { get; set; }

        public DateTime? DataFimAdesao { get; set; }

        #endregion
    }
}
