using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Solicita a assinatura de um evento de serviço.
    /// Função chamada pelos clientes da mensageria que utilizam callback
    /// para passar a receber os eventos do tipo solicitado.
    /// </summary>
    public class AssinarEventoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Tipo do serviço que contém o evento a ser assinado
        /// </summary>
        public string TipoServico  { get; set; }

        /// <summary>
        /// Nome do evento a ser assinado
        /// </summary>
        public string NomeEvento { get; set; }
    }
}
