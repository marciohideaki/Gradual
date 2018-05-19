using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Integracao.BVMF.Dados
{
    /// <summary>
    /// Classe de dados para representar um campo do layout
    /// </summary>
    [Serializable]
    public class LayoutBVMFCampoInfo
    {
        /// <summary>
        /// Descrição do campo
        /// </summary>
        public string Descricao { get; set; }
        
        /// <summary>
        /// Nome do campo. Este campo dá o nome do campo na tabela correspondente do layout gerado.
        /// </summary>
        public string Nome { get; set; }
        
        /// <summary>
        /// Ordem de processamento do campo. Utilizado para fazer a leitura sequencial da linha
        /// </summary>
        public int Ordem { get; set; }
        
        /// <summary>
        /// Parametros referentes ao campo
        /// </summary>
        public object Parametros { get; set; }

        /// <summary>
        /// Parametros referentes ao tipo (ex: formato da data, etc)
        /// </summary>
        public object ParametrosTipo { get; set; }
        
        /// <summary>
        /// Tamanho da informação no arquivo
        /// </summary>
        public int Tamanho { get; set; }
        
        /// <summary>
        /// Tipo do campo (ex: System.DateTime, System.String, etc)
        /// </summary>
        public Type Tipo { get; set; }
    }
}
