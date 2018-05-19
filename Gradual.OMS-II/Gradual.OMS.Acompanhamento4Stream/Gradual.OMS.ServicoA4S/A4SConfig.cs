using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.ServicoA4S
{
    public class A4SConfig
    {
        /// <summary>
        /// Porta de conexao oferecida ao MDS
        /// </summary>
        public int ListenPort { get; set; }

        /// <summary>
        /// Flag: debugar evento de status de conexao de bolsa
        /// </summary>
        public bool DebugStatus { get; set; }

        /// <summary>
        /// Flag: debugar evento de acompanhamento de ordens
        /// </summary>
        public bool DebugOrdem { get; set; }

        /// <summary>
        ///  Lista de portas Bovespa, separados por ;
        /// </summary>
        public string PortasOMS { get; set; }

        /// <summary>
        /// Intervalo de acesso ao sinacor em segundos
        /// </summary>
        public int IntervaloSinacor { get; set; }
    }
}
