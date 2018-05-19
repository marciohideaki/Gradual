using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Risco
{
    public class RiscoPermissaoInfo : ICodigoEntidade
    {
        public int IdPermissao { get; set; }

        public string DsPermissao { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
