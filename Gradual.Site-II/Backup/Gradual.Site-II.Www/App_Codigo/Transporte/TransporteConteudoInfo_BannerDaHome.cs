using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Www
{
    public class TransporteConteudoInfo_BannerDaHome
    {
        #region Propriedades
        
        public string Titulo { get; set; }

        public string Imagem { get; set; }

        public string Css { get; set; }

        public string Opcoes { get; set; }

        public string Link { get; set; }

        public string Classe
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Imagem))
                {
                    string lExtensao = this.Imagem.ToLower();

                    if (lExtensao.Length > 4)
                        lExtensao = lExtensao.Substring(lExtensao.Length - 4);

                    if (lExtensao == ".flv" || lExtensao == ".mp4" || lExtensao == ".m4v")
                    {
                        return "video-banner";
                    }
                }

                return "imagem";
            }
        }

        public string TagRenderizada
        {
            get
            {
                if (this.Classe == "video-banner")
                {
                    return string.Format("<div class='video-content' style='{0}'></div>", this.Css);
                }
                else
                {
                    return string.Format("<img src='{0}' />", this.Imagem);
                }
            }
        }

        #endregion
    }
    
}