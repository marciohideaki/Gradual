using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;


namespace Gradual.Intranet.Contratos
{
    public interface ICodigoEntidadeControleDeOrdens : ICodigoEntidade
    {
        AutenticarUsuarioRequest AutenticarUsuarioRequest { get; set; }
    }
}
