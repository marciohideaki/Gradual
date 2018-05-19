using System.Collections.Generic;

namespace Gradual.Pendencias.Entidades
{
    public class ClienteInfo
    {
        public string NomeCliente { get; set; }
        public string CpfCnpjCliente { get; set; }
        public string EmailCliente { get; set; }
        public string CodigoBovespa { get; set; }
        public int IdAssessor { get; set; }
        public List<PendenciaInfo> Pendencias { get; set; }
    }
}
