using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Host.WindowsService.AtivadorServicos
{
    /// <summary>
    /// Configurações do seviço de host
    /// </summary>
    public class WindowsServiceHostConfig
    {
        /// <summary>
        /// Id do servico host a carregar. 
        /// Default para 'Default'
        /// </summary>
        public string ServicoHostId { get; set; }

        /// <summary>
        /// Display name do serviço
        /// </summary>
        public string ServiceDisplayName { get; set; }

        /// <summary>
        /// Nome do serviço
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Descrição do serviço
        /// </summary>
        public string ServiceDescription { get; set; }

        /// <summary>
        /// Dependências do serviço
        /// </summary>
        public string[] ServiceDependedOn { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public WindowsServiceHostConfig()
        {
            this.ServicoHostId = "Default";
            this.ServiceName = "Servico1";
            this.ServiceDescription = "Descrição serviço 1";
            this.ServiceDisplayName = "Servico1";
        }
    }
}
