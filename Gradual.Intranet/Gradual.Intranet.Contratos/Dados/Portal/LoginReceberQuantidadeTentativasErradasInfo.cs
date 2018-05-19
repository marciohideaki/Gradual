using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Portal
{
    public class LoginReceberQuantidadeTentativasErradasInfo : EfetuarLoginInfo, ICodigoEntidade
    {
        public int QtTentativasErradas { get; set; }
    }
}
