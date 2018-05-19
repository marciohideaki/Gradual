using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Risco
{
    public class RiscoParametrosRelInfo : ICodigoEntidade
    {
        public int? ConsultaIdBolsa { get; set; }

        public int? ConsultaIdParametro { get; set; }

        public string DsParametro { get; set; }

        public string DsBolsa { get; set; }

        public string ReceberCodigo()
        {
            throw new System.NotImplementedException();
        }
    }
}
