using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{

    public class SinacorClienteComplementoInfo : ICodigoEntidade
    {

        public string NM_CONJUGE { get; set; }

        public string NM_EMPRESA { get; set; }

        public Nullable<int> CD_EST_CIVIL { get; set; }

        public Nullable<int> CD_NACION { get; set; }

        public string NM_LOC_NASC { get; set; }

        public string NM_MAE { get; set; }

        public string NM_PAI { get; set; }

        public string SG_PAIS { get; set; }

        public Nullable<char> ID_SEXO { get; set; }

        public string SG_ESTADO_NASC { get; set; }

        public Nullable<int> CD_ATIV { get; set; }

        public Nullable<int> CD_ESCOLARIDADE { get; set; }

        public Nullable<Int64> CD_NIRE { get; set; }

        public string DS_CARGO { get; set; }

        //* Outros
        public string CD_DOC_IDENT { get; set; }// - Número - VARCHAR2(16)   
        public string CD_TIPO_DOC { get; set; }// - Tipo - VARCHAR2(2)   
        public string CD_ORG_EMIT { get; set; }// - Órgão - VARCHAR2(4)    
        public string SG_ESTADO_EMIS { get; set; }// - UF - VARCHAR2(4)
        public Nullable<DateTime> DT_DOC_IDENT { get; set; }// - Data - DATE

        //* RG 
        public string NR_RG { get; set; }// - Número - VARCHAR2(16)                 
        public string SG_ESTADO_EMISS_RG { get; set; }// - UF - VARCHAR2(4)
        public Nullable<DateTime> DT_EMISS_RG { get; set; }// - Data - DATE            
        public string CD_ORG_EMIT_RG { get; set; }// - Órgão - VARCHAR2(4)  
        

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
