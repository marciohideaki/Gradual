using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;
using Gradual.OMS.Interface.Dados;

namespace Gradual.OMS.Interface.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de detalhe de grupo
    /// de comandos de execução
    /// </summary>
    public class ReceberGrupoComandoInterfaceResponse : MensagemResponseBase
    {
        /// <summary>
        /// Grupo de comandos de interface encontrado
        /// </summary>
        public GrupoComandoInterfaceInfo GrupoComandoInterface { get; set; }
    }
}
