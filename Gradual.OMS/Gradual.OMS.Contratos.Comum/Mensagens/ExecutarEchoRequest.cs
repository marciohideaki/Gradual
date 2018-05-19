using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Solicita execução do eco
    /// </summary>
    public class ExecutarEchoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Mensagem a ser rebatida
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Tempo do timer (em segundos)
        /// </summary>
        public int TempoTimer { get; set; }

        /// <summary>
        /// Indica a função desejada 
        /// </summary>
        public ExecutarEchoTipoFuncaoEnum TipoFuncao { get; set; }
    }
}
