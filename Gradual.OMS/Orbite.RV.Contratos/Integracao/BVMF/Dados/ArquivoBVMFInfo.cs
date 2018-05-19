using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Integracao.BVMF.Dados
{
    /// <summary>
    /// Contem informações de um arquivo BVMF.
    /// Utilizado na inferencia de layouts e na lista de diretório.
    /// </summary>
    public class ArquivoBVMFInfo
    {
        /// <summary>
        /// Indica o layout referente ao arquivo
        /// </summary>
        public LayoutBVMFInfo Layout { get; set; }

        /// <summary>
        /// Contem o caminho do arquivo
        /// </summary>
        public string NomeArquivo { get; set; }

        /// <summary>
        /// Contém a data de movimento do arquivo
        /// </summary>
        public DateTime? DataMovimento { get; set; }
    }
}
