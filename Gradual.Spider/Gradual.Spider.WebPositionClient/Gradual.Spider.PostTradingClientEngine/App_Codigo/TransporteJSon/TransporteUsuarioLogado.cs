using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Spider.PostTradingClientEngine.App_Codigo
{
    public class TransporteUsuarioLogado
    {
        #region Propriedades

        public int CBLC { get; set; }

        public string IP { get; set; }

        public DateTime Data { get; set; }

        #endregion

        #region Constructor

        public TransporteUsuarioLogado()
        {
            this.Data = DateTime.Now;
        }

        public TransporteUsuarioLogado(int pCBLC, string pIP) : this()
        {
            this.CBLC = pCBLC;
            this.IP = pIP;
        }

        #endregion
    }
}