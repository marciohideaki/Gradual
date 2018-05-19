using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;
using Gradual.OMS.Interface.Dados;

namespace Gradual.OMS.Interface.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de árvore de 
    /// comandos de interface
    /// </summary>
    public class ReceberArvoreComandosInterfaceResponse : MensagemResponseBase
    {
        /// <summary>
        /// Código do grupo de comandos de interface que foi processado
        /// </summary>
        public string CodigoGrupoComandoInterface { get; set; }

        /// <summary>
        /// Lista de comandos na raiz.
        /// Início da árvore
        /// </summary>
        public List<ComandoInterfaceInfo> ComandosInterfaceRaiz { get; set; }
    }
}
