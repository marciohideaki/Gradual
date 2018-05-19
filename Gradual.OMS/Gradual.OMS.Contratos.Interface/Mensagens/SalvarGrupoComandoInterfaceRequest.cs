using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Interface.Dados;

namespace Gradual.OMS.Contratos.Interface.Mensagens
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
