using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    /// <summary>
    /// Classe de configuracao do servico de integracao BVMF Arquivos
    /// </summary>
    public class ServicoIntegracaoBVMFArquivosConfig
    {
        /// <summary>
        /// Diretório default dos arquivos BVMF.
        /// Utilizado nos serviços de lista caso não seja informado nenhum diretório.
        /// </summary>
        public string DiretorioDefault { get; set; }
    }
}
