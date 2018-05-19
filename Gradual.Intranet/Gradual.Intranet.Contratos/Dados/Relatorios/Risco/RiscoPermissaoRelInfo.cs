using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Risco
{
    public class RiscoPermissaoRelInfo : ICodigoEntidade
    {
        public Nullable<int> ConsultaBolsa { get; set; }

        public Nullable<int> ConsultaPermissao { get; set; }

        public string DsBolsa { get; set; }

        public string DsPermissao { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
