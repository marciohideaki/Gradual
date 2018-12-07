using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Gradual.Site.Lib.Dados.MinhaConta.Comercial
{
    public class ProdutoInfo
    {
        #region Propriedades

        public int IdProduto { get; set; }

        public int IdPlano { get; set; }

        public string Descricao { get; set; }

        public decimal Preco { get; set; }

        public bool Suspenso { get; set; }

        public string MensagemSuspensao { get; set; }

        #endregion

        #region Métodos Públicos

        public static ProdutoInfo FromDataRow(DataRow pRow)
        {
            ProdutoInfo lRetorno = new ProdutoInfo();

            lRetorno.IdProduto  = Convert.ToInt32(pRow["id_produto_plano"]);
            lRetorno.IdPlano    =  Convert.ToInt32(pRow["id_plano"]);
            lRetorno.Descricao  = (pRow["ds_produto"] == DBNull.Value) ? null : Convert.ToString(pRow["ds_produto"]);
            lRetorno.Preco      = (pRow["vl_preco"] == DBNull.Value) ? 0 : Convert.ToDecimal(pRow["vl_preco"]);

            if (pRow["fl_suspenso"] != DBNull.Value)
                lRetorno.Suspenso = Convert.ToBoolean(pRow["fl_suspenso"]);

            if (pRow["ds_mensagem_suspenso"] != DBNull.Value)
                lRetorno.MensagemSuspensao = Convert.ToString(pRow["ds_mensagem_suspenso"]);

            return lRetorno;
        }

        #endregion
    }
}
