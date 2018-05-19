using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class AssessorFilialInfo : ICodigoEntidade
    {
        public int? ConsultaCdAssessor { get; set; }

        public int? CdAssessor { get; set; }

        public int? CdFilial { get; set; }

        public string DsFilial { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
