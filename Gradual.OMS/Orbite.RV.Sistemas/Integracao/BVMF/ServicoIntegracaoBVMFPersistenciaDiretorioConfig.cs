using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    /// <summary>
    /// Classe de configurações para o servico de persistencias de arquivos BVMF em diretório
    /// </summary>
    public class ServicoIntegracaoBVMFPersistenciaDiretorioConfig
    {
        /// <summary>
        /// Indica o caminho do diretorio aonde serão persistidos os arquivos
        /// </summary>
        public string Diretorio { get; set; }
    }
}
