using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Classe de configurações do serviço de persistencia de mensagens em arquivo
    /// </summary>
    public class ServicoPersistenciaMensagensArquivoConfig
    {
        /// <summary>
        /// Aponta para o caminho do arquivo da persistencia das mensagens
        /// </summary>
        public string ArquivoPersistencia { get; set; }
    }
}
