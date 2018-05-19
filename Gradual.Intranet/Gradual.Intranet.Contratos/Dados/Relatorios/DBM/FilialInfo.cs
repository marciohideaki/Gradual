using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class FilialInfo : ICodigoEntidade
    {
        public int CodigoFilial { get; set; }
        public string NomeFilial { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
