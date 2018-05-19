using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.ControleDeOrdens
{
    public class ClienteContaConsultaInfo : ClienteInfo
    {
        public int? CdCodigo { get; set; }
        public string CdSistema { get; set; }
    }
}
