using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteDadosIPOInfo
    {
        #region Properties 
        public string Id { get; set; }

        public string CodigoDeReferencia { get; set; }

        public string CBLC { get; set; }

        public string CpfCnpj { get; set; }

        public string Status { get; set; }

        public string DescricaoStatus { get; set; }

        public string Data { get; set; }
        #endregion
    }
}