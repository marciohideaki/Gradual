using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{

    public class SinacorClienteGeralInfo : ICodigoEntidade
    {

        /// <summary>
        /// Física ou Jurídica
        /// </summary>
        public Nullable<char> TP_PESSOA { get; set; }
        /// <summary>
        /// 1,2,8,18
        /// </summary>
        public Nullable<int> TP_CLIENTE { get; set; }

        public Nullable<char> IN_PESS_VINC { get; set; }

        public Nullable<char> IN_POLITICO_EXP { get; set; }

        public Nullable<DateTime> DT_CRIACAO { get; set; }

        public string NM_CLIENTE { get; set; }

        public Nullable<char> IN_SITUAC { get; set; }

        public bool OPERA_CONTA_PROPRIA { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
