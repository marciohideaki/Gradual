using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Faz a solicitação de lista de instrumentos que podem ser negociados no canal.
    /// Equivalente à mensagem SecurityListRequest do Fix
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DataContract]
    [Mensagem(TipoMensagemResponse = typeof(ListarInstrumentosResponse))]
    public class ListarInstrumentosRequest : MensagemOrdemRequestBase
    {
        /// <summary>
        /// Código do sistema cliente.
        /// Preenchido pelo servidor através da sessão do usuário
        /// </summary>
        public string CodigoSistemaCliente { get; set; }

        [DataMember]
        public string CodigoBolsa { get; set; }

        public ListarInstrumentosRequest()
        {
            this.Status = MensagemStatusEnum.SolicitacaoEfetuada;
        }
    }
}
