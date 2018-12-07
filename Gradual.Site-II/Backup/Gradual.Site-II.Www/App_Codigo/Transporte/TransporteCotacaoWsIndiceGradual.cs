using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Www
{
    public class TransporteCotacaoWsIndiceGradual
    {
        #region Propriedades

        public string NomeIndice { get; set; }

        public string Cotacao { get; set; }

        public string Fechamento { get; set; }

        public string Variacao { get; set; }

        public string DataCotacao { get; set; }

        #endregion

        #region Private Methods

        private string PegarValorDaTag(string pMensagem, string pTag)
        {
            string lParte = pMensagem.Substring(pMensagem.IndexOf(pTag + ">") + pTag.Length + 1);

            lParte = lParte.Substring(0, lParte.IndexOf("</"));

            return lParte;
        }

        #endregion

        public TransporteCotacaoWsIndiceGradual()
        {
        }

        public TransporteCotacaoWsIndiceGradual(string pMensagem)
        {
            //<indices><indice><nome-indice>IBG30</nome-indice><cotacao>9897,89</cotacao><fechamento>9984,97</fechamento><variacao>-0,87</variacao><data-cotacao>19-03-2013 13:14:30</data-cotacao></indice></indices>

            try {   this.NomeIndice  = PegarValorDaTag(pMensagem, "nome-indice");   } catch { }
            try {   this.Cotacao     = PegarValorDaTag(pMensagem, "cotacao");       } catch { }
            try {   this.Fechamento  = PegarValorDaTag(pMensagem, "fechamento");    } catch { }
            try {   this.Variacao    = PegarValorDaTag(pMensagem, "variacao");      } catch { }
            try {   this.DataCotacao = PegarValorDaTag(pMensagem, "data-cotacao");  } catch { }
        }
    }
}