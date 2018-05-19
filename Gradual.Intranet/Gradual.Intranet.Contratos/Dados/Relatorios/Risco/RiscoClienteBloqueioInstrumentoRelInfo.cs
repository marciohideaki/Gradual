using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Risco
{
    public class RiscoClienteBloqueioInstrumentoRelInfo : ICodigoEntidade
    {
        public string CdCodigo { get; set; }

        public string DsNome { get; set; }

        public string DsCpfCnpj { get; set; }

        public int? CdAssessor { get; set; }

        public string CdAtivo { get; set; }

        public string DsDirecao { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
