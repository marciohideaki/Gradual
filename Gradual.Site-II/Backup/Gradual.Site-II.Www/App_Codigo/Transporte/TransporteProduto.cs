using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Gradual.Site.DbLib.Dados.MinhaConta.Comercial;

namespace Gradual.Site.Www
{
    public class TransporteProduto
    {
        #region Propriedades

        public int CodigoProduto { get; set; }

        public int CodigoPlano { get; set; }

        public string NomeProduto { get; set; }

        public string DescricaoCobranca { get; set; }

        public int QuantidadeProduto { get; set; }

        public decimal ValorProduto { get; set; }

        public decimal ValorProdutoCartao { get; set; }

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

        public decimal TaxasTotal { get; set; }

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

        public string Tipo { get; set; }    //compra/recarga

        public string Modo { get; set; }

        public string ImagemExibida
        {
            get
            {
                if(this.Modo.ToLower() == "visa")
                {
                    return this.Imagem2;
                }
                else if(this.Modo.ToLower() == "master")
                {
                    return this.Imagem3;
                }
                else
                {
                    return this.Imagem;
                }
            }
        }

        /// <summary>
        /// Taxa baseada no tipo, utilizar essa.
        /// </summary>
        public decimal Taxa
        {
            get
            {
                if(this.Modo.ToLower() == "visa" || this.Modo.ToLower() == "master")
                {
                    return this.Taxas2;
                }
                else
                {
                    return this.Taxas;
                }
            }
        }

        #endregion

        #region Constructor

        public TransporteProduto()
        {
            this.Modo = "";
        }

        public TransporteProduto(Gradual.Site.DbLib.Dados.MinhaConta.Comercial.ProdutoInfo pProduto) : this()
        {
            this.CodigoProduto = pProduto.IdProduto;
            this.CodigoPlano = pProduto.IdPlano;

            this.NomeProduto = pProduto.Descricao;
            this.ValorProduto = pProduto.Preco;
            this.ValorProdutoCartao = pProduto.PrecoCartao;

            this.Taxas = pProduto.Taxa;
            this.Taxas2 = pProduto.Taxa2;

            this.Imagem = pProduto.UrlImagem;
            this.Imagem2 = pProduto.UrlImagem2;
            this.Imagem3 = pProduto.UrlImagem3;
            this.Imagem4 = pProduto.UrlImagem4;
        }

        #endregion

        #region Métodos Públicos

        public VendaProdutoInfo ToVendaProdutoInfo()
        {
            VendaProdutoInfo lRetorno = new VendaProdutoInfo();

            lRetorno.IdProduto = this.CodigoProduto;
            lRetorno.Preco = this.Total;
            lRetorno.Quantidade = this.QuantidadeProduto;
            lRetorno.Taxas = this.Taxas;
            lRetorno.TaxasTotal = this.TaxasTotal;
            lRetorno.Obervacoes = string.Format("Modo: [{0}]", this.Tipo);

            return lRetorno;
        }

        #endregion
    }
}