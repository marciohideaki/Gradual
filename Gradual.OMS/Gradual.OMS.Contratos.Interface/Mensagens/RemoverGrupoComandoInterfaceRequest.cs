﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Interface.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de remoção de grupo de comandos de execução
    /// </summary>
    public class RemoverGrupoComandoInterfaceRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do grupo de comandos de interface a remover
        /// </summary>
        public string CodigoGrupoComandoInterface { get; set; }
    }
}
