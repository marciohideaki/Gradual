using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Dados
{
    [Serializable]
    [DataContract]
    public class ItemAutomacaoOrdemHistoricoInfo
    {
        [DataMember]
        public string DescricaoDoStatus { get; set; }
        [DataMember]
        public DateTime DataDoEvento { get; set; }
    }
}
