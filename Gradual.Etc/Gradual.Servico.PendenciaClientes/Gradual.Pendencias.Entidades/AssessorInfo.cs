using System.Collections.Generic;

namespace Gradual.Pendencias.Entidades
{
    public class AssessorInfo
    {
        public int IdAssessor { get; set; }
        public string NomeAssessor { get; set; }
        public string EmailAssessor { get; set; }
        public List<ClienteInfo> Clientes { get; set; }
    }
}
