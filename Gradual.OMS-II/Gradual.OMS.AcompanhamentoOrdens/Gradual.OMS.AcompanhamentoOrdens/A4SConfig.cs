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
    }
}
