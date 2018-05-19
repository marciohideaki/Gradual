using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Interface.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação da árvore de comandos de interface
    /// para uma sessão
    /// </summary>
    public class ReceberArvoreComandosInterfaceRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do grupo de comandos na qual os comandos devem ser processados
        /// </summary>
        public string CodigoGrupoComandoInterface { get; set; }
    }
}
