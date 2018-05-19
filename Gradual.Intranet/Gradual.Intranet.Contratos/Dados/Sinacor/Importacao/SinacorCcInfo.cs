using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{

    public class SinacorCcInfo : ICodigoEntidade
    {

        /// <summary>
        /// S para PJ
        /// </summary>
        public Nullable<char> IN_CARPRO { get; set; } //S [ara PJ
        /// <summary>
        /// Null para PJ
        /// </summary>
         public Nullable<int> CD_ASSESSOR { get; set; }  //Null para PJ


         #region ICodigoEntidade Members

         public string ReceberCodigo()
         {
             throw new NotImplementedException();
         }

         #endregion
    }
}
