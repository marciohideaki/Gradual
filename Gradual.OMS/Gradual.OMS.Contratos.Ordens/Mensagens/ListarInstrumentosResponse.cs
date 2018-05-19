using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Resposta da solicitação de lista de instrumentos que podem ser negociados no canal.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DataContract]
    public class ListarInstrumentosResponse : MensagemOrdemResponseBase
    {
        [DataMember]
        public string[] Instrumentos { get; set; }
    }
}
