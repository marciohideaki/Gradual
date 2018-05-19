using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Implementação do serviço de autenticação
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoAutenticador : IServicoAutenticador
    {
        /// <summary>
        /// Referencia para o serviço de segurança
        /// </summary>
        private IServicoSeguranca _servicoSeguranca = null;

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoAutenticador()
        {
            // Mantem a referencia para o serviço de segurança
            _servicoSeguranca = Ativador.Get<IServicoSeguranca>();
        }

        #region IServicoAutenticador Members

        /// <summary>
        /// Faz o processamento da mensagem.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase ProcessarMensagem(MensagemRequestBase parametros)
        {
            // Repassa a mensagem para o serviço de segurança
            return _servicoSeguranca.ProcessarMensagem(parametros);
        }

        #endregion
    }
}
