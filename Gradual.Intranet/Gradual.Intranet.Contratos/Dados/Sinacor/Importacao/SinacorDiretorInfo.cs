using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{

    public class SinacorDiretorInfo : ICodigoEntidade
    {
    
        public string NM_DIRETOR_1 { get; set; }
       
        public string CD_DOC_IDENT_DIR1 { get; set; }
   
        public Nullable<Int64> CD_CPFCGC_DIR1 { get; set; }
   
        public string NM_DIRETOR_2 { get; set; }
   
        public string CD_DOC_IDENT_DIR2 { get; set; }
     
        public Nullable<Int64> CD_CPFCGC_DIR2 { get; set; }

        public string NM_DIRETOR_3 { get; set; }
      
        public string CD_DOC_IDENT_DIR3 { get; set; }
   
        public Nullable<Int64> CD_CPFCGC_DIR3 { get; set; }

        /// <summary>
        /// Forma de Constituição da Empresa. Ex: LTDA
        /// </summary>
        public string DS_FORMACAO { get; set; }

        /// <summary>
        /// Inscrição Estadual
        /// </summary>
        public string NR_INSCRICAO { get; set; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
