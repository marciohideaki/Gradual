using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Site.DbLib.Dados;
using Newtonsoft.Json;

namespace Gradual.Site.Www
{
    public class TransporteBannerLateral
    {
        #region Propriedades

        public int IdBanner { get; set; }
        
        public string IdBannerLink { get; set; }

        /// <summary>
        /// 15 é o banner, 16 é o link do banner com a página
        /// </summary>
        public int IdTipo { get; set; }

        public string Titulo { get; set; }

        public string Link { get; set; }

        public string LinkParaArquivo { get; set; }

        public string DataCadastro { get; set; }

        public string Tag { get; set; }

        public string UrlDaPagina { get; set; }

        public string Posicao { get; set; }

        #endregion

        #region Construtor

        public TransporteBannerLateral() { }

        public TransporteBannerLateral(ConteudoInfo pInfo)
        {
            this.IdTipo = pInfo.CodigoTipoConteudo;

            TransporteBannerLateral lTransporte = JsonConvert.DeserializeObject<TransporteBannerLateral>(pInfo.ConteudoJson);

            if (this.IdTipo == ConfiguracoesValidadas.IdDoTipo_BannerLateral)
            {
                this.IdBanner = pInfo.CodigoConteudo;
                this.Titulo = lTransporte.Titulo;
                this.LinkParaArquivo = lTransporte.LinkParaArquivo;
                this.Link = lTransporte.Link;
                this.DataCadastro = lTransporte.DataCadastro;
                this.Tag = lTransporte.Tag;
            }
            else
            {
                this.IdBanner = Convert.ToInt32(pInfo.ValorPropriedade2.Trim());
                this.IdBannerLink = pInfo.CodigoConteudo.ToString();

                this.UrlDaPagina = pInfo.ValorPropriedade1;

                this.Posicao = pInfo.ValorPropriedade3;
                this.Tag = pInfo.ValorPropriedade4;

            }
        }

        #endregion
    }
}