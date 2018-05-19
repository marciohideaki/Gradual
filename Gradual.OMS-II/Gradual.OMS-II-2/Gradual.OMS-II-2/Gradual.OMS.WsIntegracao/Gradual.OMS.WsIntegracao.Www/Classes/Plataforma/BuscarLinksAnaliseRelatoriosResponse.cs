using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

namespace Gradual.OMS.WsIntegracao
{
    [Serializable]
    public class BuscarLinksAnaliseRelatoriosResposta : RespostaBase
    {
        #region Propriedades

        public List<LinkAnaliseRelatorios> Links { get; set; }

        #endregion

        #region Construtores

        public BuscarLinksAnaliseRelatoriosResposta()
        {
            this.Links = new List<LinkAnaliseRelatorios>();
        }

        #endregion
    }

    [Serializable]
    public class LinkAnaliseRelatorios
    {
        #region Propriedades

        public string Categoria { get; set; }

        public string Texto { get; set; }

        public string URL { get; set; }

        #endregion
        
        #region Construtores

        public LinkAnaliseRelatorios() { }
        
        public LinkAnaliseRelatorios(string pCategoria, string pTextoDoLink)
        {
            this.Categoria = pCategoria;

            Regex lRegexHref = new Regex("href='.*\\.pdf");

            Match lMatch = lRegexHref.Match(pTextoDoLink);

            if(lMatch.Success)
                this.URL = lMatch.Value.Substring(lMatch.Value.IndexOf("'") + 1);

            if (!string.IsNullOrEmpty(this.URL) && !this.URL.ToLower().StartsWith("http"))
                this.URL = "http://www.gradualinvestimentos.com.br" + this.URL;

            lRegexHref = new Regex(">.*</a");

            lMatch = lRegexHref.Match(pTextoDoLink);
            
            if(lMatch.Success)
            {
                this.Texto = lMatch.Value.Substring(lMatch.Value.LastIndexOf(">") + 1);
                this.Texto = this.Texto.Substring(0, this.Texto.IndexOf("<"));
            }
        }

        #endregion
    }
}