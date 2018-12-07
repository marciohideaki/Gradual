using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Www
{
    public class TransporteOrdemCriticas
    {
        #region Propriedades
        
        #endregion

        public string MensagemCritica { get; set; }

        public List<string> DataHoras { get; set; }

        public List<string> Criticas { get; set; }

        #region Construtores

        public TransporteOrdemCriticas()
        {
            this.DataHoras = new List<string>();

            this.Criticas = new List<string>();
        }

        #endregion
    }
}