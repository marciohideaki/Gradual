using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Integracao.BVMF.Dados
{
    /// <summary>
    /// Classe de dados para layouts BVMF
    /// </summary>
    [Serializable]
    public class LayoutBVMFInfo
    {
        public LayoutBVMFInfo()
        {
            this.Tabelas = new List<LayoutBVMFTabelaInfo>();
        }

        /// <summary>
        /// Tipo do conversor a ser usado para interpretar os arquivos
        /// </summary>
        public Type ConversorTipo { get; set; }

        /// <summary>
        /// Descricao do layout
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Nome do layout
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Parâmetros para o conversor.
        /// </summary>
        public object Parametros { get; set; }

        /// <summary>
        /// Lista de tabelas do layout. No caso de bovespa, lista dos tipos de registro
        /// </summary>
        public List<LayoutBVMFTabelaInfo> Tabelas { get; set; }
    }
}
