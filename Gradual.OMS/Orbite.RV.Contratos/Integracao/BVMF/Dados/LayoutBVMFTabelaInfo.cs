using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Integracao.BVMF.Dados
{
    /// <summary>
    /// Classe de dados para representar uma tabela do layout
    /// </summary>
    [Serializable]
    public class LayoutBVMFTabelaInfo
    {
        public LayoutBVMFTabelaInfo()
        {
            this.Campos = new List<LayoutBVMFCampoInfo>();
        }

        /// <summary>
        /// Lista de campos da tabela
        /// </summary>
        public List<LayoutBVMFCampoInfo> Campos { get; set; }
        
        /// <summary>
        /// Descricao da tabela
        /// </summary>
        public string Descricao { get; set; }
        
        /// <summary>
        /// Nome da tabela
        /// </summary>
        public string Nome { get; set; }
        
        /// <summary>
        /// Ordem da tabela na lista de processamento
        /// </summary>
        public int Ordem { get; set; }
        
        /// <summary>
        /// Parametros da tabela
        /// </summary>
        public object Parametros { get; set; }
    }
}
