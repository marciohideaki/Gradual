using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Classe de cliente para guardar informações do banco de dados sinacor
    /// </summary>
    [Serializable]
    public class ClienteInfo
    {
        /// <summary>
        /// Nome do cliente no SINACOR
        /// </summary>
        public string NomeCliente   { set; get; }

        /// <summary>
        /// Código do assessor no 
        /// </summary>
        public string Assessor      { set; get; }

        /// <summary>
        /// Código BMF do cliente
        /// </summary>
        public string CodigoBMF     { set; get; }

        /// <summary>
        /// Código Bovespa do Cliente
        /// </summary>
        public string CodigoBovespa { set; get; }

        /// <summary>
        /// Nome do assessor
        /// </summary>
        public string NomeAssessor { get; set; }
    }
}
