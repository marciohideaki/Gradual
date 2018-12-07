using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Www
{
    public class SecaoDoSite
    {
        #region Propriedades

        public string Nome { get; set; }

        public string URL { get; set; }

        #endregion

        #region Construtores

        public SecaoDoSite() { }

        public SecaoDoSite(string pNome, string pURL)
        {
            this.Nome = pNome;
            this.URL = pURL;
        }

        #endregion
    }
}