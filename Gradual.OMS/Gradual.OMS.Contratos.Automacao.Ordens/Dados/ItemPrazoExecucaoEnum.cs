using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Dados{
    [Serializable]
    public enum ItemPrazoExecucaoEnum : int
    {
        Default = 0,
        Hoje = 1,
        Para30Dias = 2
   }
}
