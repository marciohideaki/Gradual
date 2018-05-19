using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Mensagem(TipoMensagemResponse = typeof(SinalizarListaInstrumentosResponse))]
    public class SinalizarListaInstrumentosRequest : MensagemSinalizacaoBase
    {
        public string CodigoCanal { get; set; }
        public string SecurityReqID { get; set; }
        public List<InstrumentoInfo> SecurityListMessages { get; set; }
    }
}
