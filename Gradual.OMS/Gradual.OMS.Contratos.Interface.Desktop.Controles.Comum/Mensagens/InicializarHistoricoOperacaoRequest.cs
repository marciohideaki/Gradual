using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Contratos.Ordens.Mensagens;

namespace Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de requisição para que o controle de historico de operações
    /// se inicialize
    /// </summary>
    public class InicializarHistoricoOperacaoRequest : MensagemInterfaceRequestBase
    {
        /// <summary>
        /// Contem a lista do historico da operação a ser apresentado
        /// </summary>
        public List<SinalizarExecucaoOrdemRequest> Historico { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public InicializarHistoricoOperacaoRequest()
        {
            this.Historico = new List<SinalizarExecucaoOrdemRequest>();
        }
    }
}
