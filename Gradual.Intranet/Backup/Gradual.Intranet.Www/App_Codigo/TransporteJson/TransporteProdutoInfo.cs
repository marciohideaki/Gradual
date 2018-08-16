using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using System.Globalization;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteProdutoInfo
    {
        #region Propriedades

        public string IdProduto { get; set; }

        public string NomeDoProduto { get; set; }

        public string Preco { get; set; }

        public string PrecoCartao { get; set; }

        public string Taxas { get; set; }

        public string Taxas2 { get; set; }

        public string ProdutoSuspenso { get; set; }

        public string DescrisaoSuspenso { get; set; }

        public string UrlImagem { get; set; }

        public string UrlImagem2 { get; set; }

        public string UrlImagem3 { get; set; }

        public string UrlImagem4 { get; set; }

        [JsonIgnore]
        public string Modo { get; set; }

        [JsonIgnore]
        public string ImagemDeExibicao
        {
            get
            {
                if (this.Modo == "visa")
                {
                    return this.UrlImagem2;
                }
                else if (this.Modo == "visa")
                {
                    return this.UrlImagem3;
                }
                else
                {
                    return this.UrlImagem;
                }
            }
        }

        #endregion

        #region Construtor

        public TransporteProdutoInfo()
        {
        }

        #endregion

        #region Métodos Públicos

        public ProdutoInfo ToProdutoInfo()
        {
            ProdutoInfo lRetorno = new ProdutoInfo();

            CultureInfo lInfo = new CultureInfo("pt-BR");

            if (!string.IsNullOrEmpty(this.IdProduto))
                lRetorno.IdProduto = Convert.ToInt32(this.IdProduto);

            lRetorno.DsNomeProduto = this.NomeDoProduto;

            lRetorno.VlPreco = Convert.ToDecimal(this.Preco, lInfo);
            
            lRetorno.VlPrecoCartao = Convert.ToDecimal(this.PrecoCartao, lInfo);

            lRetorno.VlTaxa = Convert.ToDecimal(this.Taxas, lInfo);
            
            lRetorno.VlTaxa2 = Convert.ToDecimal(this.Taxas2, lInfo);

            lRetorno.FlSuspenso = (this.ProdutoSuspenso.ToLower() == "true");

            lRetorno.UrlImagem = this.UrlImagem;

            lRetorno.UrlImagem2 = this.UrlImagem2;

            lRetorno.UrlImagem3 = this.UrlImagem3;

            lRetorno.UrlImagem4 = this.UrlImagem4;

            return lRetorno;
        }

        #endregion
    }
}