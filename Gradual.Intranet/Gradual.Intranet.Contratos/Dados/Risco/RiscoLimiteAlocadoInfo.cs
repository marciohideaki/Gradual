using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Risco
{
    public class RiscoLimiteAlocadoInfo : ICodigoEntidade
    {
        public int ConsultaIdCliente { get; set; }

        public int  IdParametro { get; set; }

        public string DsParametro { get; set; }

        public decimal VlParametro { get; set; }

        public decimal VlAlocado { get; set; }

        public decimal VlDisponivel { get; set; }

        public bool NovoOMS { get; set; }

        public bool Spider { get; set; }

        public string ReceberCodigo()
        {
            throw new System.NotImplementedException();
        }
    }
}
