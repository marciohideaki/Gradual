using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class ResumoAssessorInfo : ICodigoEntidade
    {
        public string NomeAssessor { get; set; }
        public string CodigoAssessor { get; set; }
        public decimal Corretagem { get; set; }
        public decimal Volume { get; set; }
        public decimal Custodia { get; set; }
        public string Tipo { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
