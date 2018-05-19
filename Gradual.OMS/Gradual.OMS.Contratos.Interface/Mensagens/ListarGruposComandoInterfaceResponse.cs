using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Interface.Dados;

namespace Gradual.OMS.Contratos.Interface.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de grupo de comandos
    /// de execução
    /// </summary>
    public class ListarGruposComandoInterfaceResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de grupos encontrados
        /// </summary>
        public List<GrupoComandoInterfaceInfo> Resultado { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListarGruposComandoInterfaceResponse()
        {
            this.Resultado = new List<GrupoComandoInterfaceInfo>();
        }
    }
}
