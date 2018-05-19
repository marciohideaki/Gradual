using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.BVMF
{
    /// <summary>
    /// Classe de configuração do serviço ServicoIntegracaoBVMF
    /// </summary>
    public class ServicoIntegracaoBVMFConfig
    {
        /// <summary>
        /// Indica se deve utilizar um servidor de proxy
        /// </summary>
        public bool UtilizarProxy { get; set; }
        
        /// <summary>
        /// Endereço do servidor de proxy
        /// </summary>
        public string ProxyEndereco { get; set; }
        
        /// <summary>
        /// Porta do servidor de proxy
        /// </summary>
        public int ProxyPorta { get; set; }

        /// <summary>
        /// Indica o diretório temporário para fazer a extração do arquivo zip
        /// </summary>
        public string TempDir { get; set; }
        
        /// <summary>
        /// Caminho do Winrar. Utilizado para fazer a extração do arquivo.
        /// </summary>
        public string CaminhoRar { get; set; }

        /// <summary>
        /// Endereço na bovespa para fazer a solicitação do arquivo
        /// </summary>
        public string EnderecoMarketDataBovespa { get; set; }
    }
}
