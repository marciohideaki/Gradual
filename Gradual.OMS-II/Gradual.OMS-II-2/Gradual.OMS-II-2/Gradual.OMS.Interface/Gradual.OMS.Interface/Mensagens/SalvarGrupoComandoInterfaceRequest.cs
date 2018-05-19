using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;
using Gradual.OMS.Interface.Dados;

namespace Gradual.OMS.Interface.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para salvar grupo de comandos de execução
    /// </summary>
    public class SalvarGrupoComandoInterfaceRequest : MensagemRequestBase
    {
        /// <summary>
        /// Grupo de comandos de interface a ser salvo
        /// </summary>
        public GrupoComandoInterfaceInfo GrupoComandoInterface { get; set; }
    }
}
