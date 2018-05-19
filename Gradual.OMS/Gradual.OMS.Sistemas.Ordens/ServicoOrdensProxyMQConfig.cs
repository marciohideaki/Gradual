using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Sistemas.Ordens
{
    /// <summary>
    /// Configurações do serviço de proxy para MQ
    /// </summary>
    public class ServicoOrdensProxyMQConfig
    {
        /// <summary>
        /// Caminho para a fila de saida
        /// </summary>
        public string CaminhoFilaSaida { get; set; }

        /// <summary>
        /// Caminho para a fila de entrada
        /// </summary>
        public string CaminhoFilaEntrada { get; set; }
    }
}
