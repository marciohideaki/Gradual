using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    [Serializable]
    public class ControleSerializacaoInfo
    {
        public ControleInfo ControleInfo { get; set; }
        public ObjetoSerializado ControleParametros { get; set; }
    }
}
