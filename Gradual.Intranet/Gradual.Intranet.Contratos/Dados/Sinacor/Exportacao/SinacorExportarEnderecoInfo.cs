using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class SinacorExportarEnderecoInfo : ICodigoEntidade
    {

        public string CD_CEP { get; set; }
        public string CD_DDD_CELULAR1 { get; set; }
        public string CD_DDD_CELULAR2 { get; set; }
        public string CD_DDD_FAX { get; set; }
        public string CD_DDD_TEL { get; set; }
        public string IN_ENDE { get; set; }
        public string NM_BAIRRO { get; set; }
        public string NR_CELULAR1 { get; set; }
        public string NR_CELULAR2 { get; set; }
        public string NM_CIDADE { get; set; }
        public string NM_COMP_ENDE { get; set; }
        public string NR_FAX { get; set; }
        public string NM_CONTATO1 { get; set; }
        public string NM_CONTATO2 { get; set; }
        public string NM_LOGRADOURO { get; set; }
        public string NR_PREDIO { get; set; }
        public string NR_RAMAL { get; set; }
        public string NR_TELEFONE { get; set; }
        public string SG_ESTADO { get; set; }
        public string SG_PAIS_ENDE1 { get; set; }
        public string CD_USUARIO { get; set; }
        public string TP_OCORRENCIA { get; set; }
        public string DT_ATUALIZ_CCLI { get; set; }



        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
