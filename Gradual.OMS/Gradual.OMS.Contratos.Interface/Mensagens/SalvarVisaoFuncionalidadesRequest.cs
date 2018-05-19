using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Interface.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para salvar a visao de funcionalidades
    /// </summary>
    public class SalvarVisaoFuncionalidadesRequest : MensagemRequestBase
    {
        /// <summary>
        /// Lista de seleção de funcionalidades
        /// </summary>
        public List<FuncionalidadeSelecaoInfo> SelecaoFuncionalidades { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public SalvarVisaoFuncionalidadesRequest()
        {
            this.SelecaoFuncionalidades = new List<FuncionalidadeSelecaoInfo>();
        }
    }
}
