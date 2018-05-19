using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    [Serializable]
    public class JanelaSerializacaoInfo
    {
        public JanelaInfo JanelaInfo { get; set; }
        public ObjetoSerializado JanelaParametros { get; set; }
        
        public List<ControleSerializacaoInfo> Controles { get; set; }

        public JanelaSerializacaoInfo()
        {
            this.Controles = new List<ControleSerializacaoInfo>();
        }
    }
}
