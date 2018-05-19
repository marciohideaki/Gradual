using System.Collections.Generic;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class ListaAssessoresVinculadosInfo : ICodigoEntidade
    {
        public int? ConsultaCodigoAssessor { get; set; }

        public int? CodigoLogin { get; set; }

        public List<int> ListaCodigoAssessoresVinculados { get; set; }

        public ListaAssessoresVinculadosInfo()
        {
            this.ListaCodigoAssessoresVinculados = new List<int>();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
