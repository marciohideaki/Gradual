using System.Collections.Generic;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class ListaEmailAssessorInfo : ICodigoEntidade
    {
        public int IdAssessor { get; set; }

        public List<string> ListaEmailAssessor { get; set; }

        public string ReceberCodigo()
        {
            throw new System.NotImplementedException();
        }
    }
}
