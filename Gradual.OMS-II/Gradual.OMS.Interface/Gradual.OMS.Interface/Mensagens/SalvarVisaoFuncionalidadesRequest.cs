using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Gradual.OMS.Library;
using Gradual.OMS.Interface.Dados;

namespace Gradual.OMS.Interface.Mensagens
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
