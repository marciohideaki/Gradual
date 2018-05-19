using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.GravadorNegociosLog
{
    public class ProcessadorCotacaoConfig
    {
        /// <summary>
        /// String de conexao com o Banco do MDS no SQL Server
        /// </summary>
        public string ConnectionString { get;set; }

        /// <summary>
        /// Diretório dos Logs do Capturador ANG
        /// </summary>
        public string DiretorioLogs { get; set; }

        /// <summary>
        /// Nome do Log original
        /// </summary>
        public string NomeLogPadrao { get; set; }
    }
}
