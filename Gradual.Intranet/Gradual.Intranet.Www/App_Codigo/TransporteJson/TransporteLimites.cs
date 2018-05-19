using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteLimites
    {
        #region Propriedades

        public TransporteLimiteBovespa Bovespa { get; set; }

        public TransporteLimiteBMF BMF { get; set; }

        #endregion

        #region Construtor

        public TransporteLimites()
        {
            this.Bovespa = new TransporteLimiteBovespa();
            this.BMF = new TransporteLimiteBMF();
        }

        #endregion
    }
}