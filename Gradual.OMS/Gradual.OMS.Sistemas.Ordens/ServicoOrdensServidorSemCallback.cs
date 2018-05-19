using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Ordens
{
    /// <summary>
    /// Implementação do serviço de ordens servidor.
    /// Apenas repassa as chamadas para o servico de ordens.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoOrdensServidorSemCallback : IServicoOrdensServidorSemCallback
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia ao servico de ordens
        /// </summary>
        private IServicoOrdens _servicoOrdens = null;

        #endregion

        #region Construtor e Rotinas Locais

        /// <summary>
        /// Construtor. Pega referencia para o servico de ordens.
        /// Na sequencia de ativação de serviços, o serviço de ordens servidor tem que ser mencionado
        /// depois do serviço de ordens para que neste momento a referencia possa ser pega sem erros.
        /// </summary>
        public ServicoOrdensServidorSemCallback()
        {
            // Mantem instancia para o servico de ordens
            _servicoOrdens = Ativador.Get<IServicoOrdens>();
        }

        #endregion

        #region IServicoOrdensServidor Members

        /// <summary>
        /// Pede o processamento da mensagem. Repassa a mensagem para o serviço de ordens.
        /// </summary>
        /// <param name="mensagem"></param>
        /// <returns></returns>
        public MensagemResponseBase ProcessarMensagem(MensagemRequestBase mensagem)
        {
            // Faz o pedido do processamento e retorna a resposta
            return _servicoOrdens.ProcessarMensagem(mensagem);
        }

        #endregion
    }
}
