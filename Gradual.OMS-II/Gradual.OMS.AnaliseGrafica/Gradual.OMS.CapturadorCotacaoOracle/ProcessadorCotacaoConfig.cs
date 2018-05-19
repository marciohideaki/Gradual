using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.CapturadorCotacaoOracle
{
    public class ProcessadorCotacaoConfig
    {
        /// <summary>
        /// Endereco IP do servidor do MDS
        /// </summary>
        public string MDSAddress { get; set; }

        /// <summary>
        ///  Porta de conexao ao MDS
        /// </summary>
        public int MDSPort { get; set; }

        /// <summary>
        /// String de conexao com a base Oracle do SINACOR
        /// </summary>
        public string OracleConnectionString { get; set; }
    }
}
