using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBlotterLib
{
    public class OpenBlotterConfig
    {
        /// <summary>
        /// String de conexao ao Sinacor
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Usuario para autenticacao da  JPMorgan
        /// </summary>
        public string OpenBlotterUsr { get; set; }

        /// <summary>
        /// Senha para autenticacao da  JPMorgan
        /// </summary>
        public string OpenBlotterPwd { get; set; }

        /// <summary>
        /// Lista de clientes conforme cadastrada em tsccliger e tscclibmf (cd_cliente)
        /// </summary>
        public string ClientIDList { get; set; } 
    }
}
