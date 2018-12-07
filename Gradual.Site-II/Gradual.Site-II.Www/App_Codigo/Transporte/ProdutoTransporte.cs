using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Gradual.Site.Www
{
    public class ProdutoTransporte
    {
        public int CodigoProduto { get; set; }

        public int CodigoPlano { get; set; }

        public string NomeProduto { get; set; }

        public string DescricaoCobranca { get; set; }

        public int QuantidadeProduto { get; set; }

        public decimal ValorProduto { get; set; }

        public decimal Total { get; set; }

        public string PreId { get; set; }

        [JsonIgnore]
        public bool ChekTermo { get; set; }

        [JsonIgnore]
        public bool CheckTermoObrigatorio { get; set; }

        public string TextoTermo { get; set; }

        public string CaminhoPDF { get; set; }

        [JsonIgnore]
        public string Css { get; set; }

        public decimal Taxas { get; set; }

        public decimal Taxas2 { get; set; }

        [JsonIgnore]
        public DateTime DataInicio { get; set; }

        [JsonIgnore]
        public DateTime DataFim { get; set; }

        [JsonIgnore]
        public string Imagem { get; set; }

        [JsonIgnore]
        public string Imagem2 { get; set; }

        [JsonIgnore]
        public string Imagem3 { get; set; }

        [JsonIgnore]
        public string Imagem4 { get; set; }

        public ProdutoTransporte()
        {
        }

        public ProdutoTransporte(Gradual.Site.DbLib.Dados.MinhaConta.Comercial.ProdutoInfo pProduto)
        {
            this.CodigoProduto = pProduto.IdProduto;
            this.CodigoPlano = pProduto.IdPlano;

            this.NomeProduto = pProduto.Descricao;
            this.ValorProduto = pProduto.Preco;
            this.Taxas = pProduto.Taxa;
            this.Imagem = pProduto.UrlImagem;
        }
    }
}