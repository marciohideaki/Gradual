using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Risco
{
    public class RiscoClienteGrupoInfo : ICodigoEntidade
    {
        public int IdGrupo { get; set; }

        public int? CdCliente { get; set; }

        public int? CdAssessor { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
