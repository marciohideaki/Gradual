using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class AssessorDoClienteInfo : ICodigoEntidade
    {
        
        public int IdCliente { get; set; }

        public int CodigoAssessor { get; set; }

        public string NomeAssessor { get; set; }

        public string EmailAssessor { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
