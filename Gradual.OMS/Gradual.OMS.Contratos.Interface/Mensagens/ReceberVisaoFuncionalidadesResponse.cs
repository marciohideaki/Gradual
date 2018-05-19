using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Interface.Dados;

namespace Gradual.OMS.Contratos.Interface.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de visao de funcionalidades
    /// </summary>
    public class ReceberVisaoFuncionalidadesResponse : MensagemResponseBase
    {
        /// <summary>
        /// Seleções das funcionalidades para o contexto solicitado
        /// </summary>
        public List<FuncionalidadeSelecaoInfo> SelecaoFuncionalidades { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberVisaoFuncionalidadesResponse()
        {
            this.SelecaoFuncionalidades = new List<FuncionalidadeSelecaoInfo>();
        }
    }
}
