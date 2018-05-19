﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de validação de operação
    /// </summary>
    public class ValidarOperacaoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Mensagem de resultado da validação
        /// </summary>
        public ValidarMensagemResponse Validacao { get; set; }
    }
}
