using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de tradução do código CBLC para código de usuário
    /// </summary>
    public class TraduzirCodigoCBLCRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código CBLC a ser traduzido
        /// </summary>
        public string CodigoCBLC { get; set; }
    }
}
