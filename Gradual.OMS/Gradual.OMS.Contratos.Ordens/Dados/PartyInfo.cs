using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// Estrutura para conter informações de um parte envolvida no processo de negociação.
    /// Utilizado para montagem das mensagens fix.
    /// </summary>
    [Serializable]
    public class PartyInfo
    {
        public string PartyID { get; set; }
        public string PartyIDSource { get; set; }
        public int PartyRole { get; set; }
    }
}
