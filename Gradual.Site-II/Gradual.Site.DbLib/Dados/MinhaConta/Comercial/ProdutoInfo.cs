using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Gradual.Site.DbLib.Dados.MinhaConta.Comercial
{
    public class ProdutoInfo
    {
        #region Propriedades

        public int IdProduto { get; set; }

        public int IdPlano { get; set; }

        public string Descricao { get; set; }

        public decimal Preco { get; set; }

        public decimal PrecoCartao { get; set; }

        public decimal Taxa { get; set; }

        public decimal Taxa2 { get; set; }

        public bool Suspenso { get; set; }

        public string MensagemSuspensao { get; set; }

        public string UrlImagem { get; set; }

        public string UrlImagem2 { get; set; }

        public string UrlImagem3 { get; set; }

        public string UrlImagem4 { get; set; }

        #endregion

        #region Métodos Públicos

        public static ProdutoInfo FromDataRow(DataRow pRow)
        {
            ProdutoInfo lRetorno = new ProdutoInfo();

            lRetorno.IdProduto  = Convert.ToInt32(pRow["id_produto_plano"]);
            lRetorno.IdPlano    =  Convert.ToInt32(pRow["id_plano"]);
            lRetorno.Descricao  = (pRow["ds_produto"] == DBNull.Value) ? null : Convert.ToString(pRow["ds_produto"]);
            lRetorno.Preco      = (pRow["vl_preco"] == DBNull.Value) ? 0 : Convert.ToDecimal(pRow["vl_preco"]);
            
            lRetorno.PrecoCartao = (pRow["vl_preco_cartao"] == DBNull.Value) ? 0 : Convert.ToDecimal(pRow["vl_preco_cartao"]);

            if (pRow["fl_suspenso"] != DBNull.Value)
                lRetorno.Suspenso = Convert.ToBoolean(pRow["fl_suspenso"]);

            if (pRow["ds_mensagem_suspenso"] != DBNull.Value)
                lRetorno.MensagemSuspensao = Convert.ToString(pRow["ds_mensagem_suspenso"]);

            if (pRow["vl_taxa"] != DBNull.Value)
                lRetorno.Taxa = Convert.ToDecimal(pRow["vl_taxa"]);
            
            if (pRow["vl_taxa2"] != DBNull.Value)
                lRetorno.Taxa2 = Convert.ToDecimal(pRow["vl_taxa2"]);

            if (pRow["url_imagem"] != DBNull.Value)
                lRetorno.UrlImagem = Convert.ToString(pRow["url_imagem"]);

            if (pRow["url_imagem2"] != DBNull.Value)
                lRetorno.UrlImagem2 = Convert.ToString(pRow["url_imagem2"]);

            if (pRow["url_imagem3"] != DBNull.Value)
                lRetorno.UrlImagem3 = Convert.ToString(pRow["url_imagem3"]);

            if (pRow["url_imagem4"] != DBNull.Value)
                lRetorno.UrlImagem4 = Convert.ToString(pRow["url_imagem4"]);

            return lRetorno;
        }

        #endregion
    }
}
