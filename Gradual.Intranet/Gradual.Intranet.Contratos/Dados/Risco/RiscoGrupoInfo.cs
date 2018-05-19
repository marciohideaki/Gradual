using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra.Lib.Dados;

namespace Gradual.Intranet.Contratos.Dados.Risco
{
    public class RiscoGrupoInfo : ICodigoEntidade
    {
        public int IdGrupo { get; set; }

        public string DsGrupo { get; set; }

        public EnumRiscoRegra.TipoGrupo TipoGrupo { get; set; }

        public string ReceberCodigo()
        {
            throw new System.NotImplementedException();
        }
    }
}
