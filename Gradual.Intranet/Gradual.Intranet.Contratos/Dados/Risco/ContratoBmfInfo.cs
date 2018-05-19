using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Risco
{
    public class ContratoBmfInfo : ICodigoEntidade
    {
        public string CodigoContrato { get; set; }
        public string DescricaoContrato { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
