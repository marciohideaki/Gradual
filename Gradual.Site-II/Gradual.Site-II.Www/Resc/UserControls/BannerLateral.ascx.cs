using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class BannerLateral : UserControlBase
    {
        #region Propriedades

        public string Posicao { get; set; }

        private string _UrlDaPagina;

        public string UrlDaPagina
        {
            get
            {
                if (string.IsNullOrEmpty(_UrlDaPagina))
                {
                    _UrlDaPagina = Request.Url.AbsolutePath.ToLower();

                    if(!string.IsNullOrEmpty(this.PaginaBase.RaizDoSite.ToLower()) && this.PaginaBase.RaizDoSite != "/")
                    {
                        _UrlDaPagina = _UrlDaPagina.Replace(this.PaginaBase.RaizDoSite.ToLower(), "");

                        if (_UrlDaPagina.StartsWith("//"))
                            _UrlDaPagina = _UrlDaPagina.Substring(1);
                    }
                }

                return _UrlDaPagina;
            }
        }

        public bool TemConteudo
        {
            get
            {
                return (this.Banner != null);
            }
        }

        public TransporteBannerLateral Banner { get; set; }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            string lURL = Request.Url.AbsolutePath.ToLower();

            if(!string.IsNullOrEmpty(this.PaginaBase.RaizDoSite.ToLower()) && this.PaginaBase.RaizDoSite != "/")
            {
                lURL = lURL.Replace(this.PaginaBase.RaizDoSite.ToLower(), "");

                if (lURL.StartsWith("//"))
                    lURL = lURL.Substring(1);
            }

            if (this.PaginaBase.BannersLateraisPorPagina.ContainsKey(lURL))
            {
                foreach (TransporteBannerLateral lBanner in this.PaginaBase.BannersLateraisPorPagina[lURL])
                {
                    if (this.PaginaBase.BannersLateraisDisponiveis.ContainsKey(lBanner.IdBanner))
                    {
                        if (lBanner.Posicao == this.Posicao)
                        {
                            this.Banner = lBanner;
                        }
                    }
                }
            }

        }

        #endregion
    }
}