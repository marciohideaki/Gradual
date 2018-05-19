using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Risco
{
    public class ParametroAlavancagemConsultaInfo : ICodigoEntidade
    {
        #region | Propriedades de Consulta

        public int? ConsultaCdCliente { get; set; }

        public int? ConsultaCdAssessor { get; set; }

        public int? ConsultaIdGrupo { get; set; }

        #endregion

        #region | Propriedades de Dados

        public int CdCliente { get; set; }

        public int CdAssessor { get; set; }

        public string DsGrupo { get; set; }

        public DateTime DtInclusao { get; set; }

        #endregion

        #region | Implementação da Interface

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
