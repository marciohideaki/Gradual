﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de inicialização de usuário
    /// </summary>
    public class InicializarUsuarioResponse : MensagemResponseBase
    {
        /// <summary>
        /// Usuário inicializado
        /// </summary>
        public UsuarioInfo Usuario { get; set; }
    }
}