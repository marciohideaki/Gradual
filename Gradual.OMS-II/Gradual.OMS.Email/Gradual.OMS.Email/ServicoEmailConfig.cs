using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Email
{
    /// <summary>
    /// Classe de configurações do serviço de email.
    /// </summary>
    public class ServicoEmailConfig
    {
        /// <summary>
        /// Aponta para o arquivo de configuração do servico e contém
        /// a configuração do SMTP para o envio de mensagem via servidor Exchange.
        /// </summary>
        public string SMTPHost { get; set; }
    }
}
