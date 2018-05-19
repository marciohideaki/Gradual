using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Interface.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de detalhe de grupo de comandos de execução
    /// </summary>
    public class ReceberGrupoComandoInterfaceRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do grupo de comandos de interface a recuperar
        /// </summary>
        public string CodigoGrupoComandoInterface { get; set; }
    }
}
