using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Risco
{
    public class RiscoParametrosInfo : ICodigoEntidade
    {
        public int IdParametro { get; set; }

        public string DsParametro { get; set; }

        public int? IdBolsa { get; set; }

        public string UrlNamespace { get; set; }

        public string NomeMetodos { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
