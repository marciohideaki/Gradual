using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Interface.Dados
{
    /// <summary>
    /// Classe de informações de funcionalidade.
    /// </summary>
    [Serializable]
    public class FuncionalidadeInfo : ICodigoEntidade
    {
        /// <summary>
        /// Chave primária da funcionalidade
        /// </summary>
        [Description("Contém o código de identificação da funcionalidade")]
        public string CodigoFuncionalidade { get; set; }

        /// <summary>
        /// Código do sistema que a funcionalidade pertence
        /// </summary>
        [Description("Contém o código do sistema que a funcionalidade pertence")]
        public int CodigoSistema { get; set; }

        /// <summary>
        /// Permite classificar as funcionalidades.
        /// </summary>
        public FuncionalidadeGrupoInfo Agrupamento { get; set; }

        /// <summary>
        /// Nome da funcionalidade
        /// </summary>
        [Description("Descrição da funcionalidade.")]
        public string NomeFuncionalidade { get; set; }

        /// <summary>
        /// Contém a lista de perfis vinculados a esta funcionalidade.
        /// </summary>
        [Description("Lista de código de perfis associados a esta funcionalidade.")]
        public List<string> Perfis { get; set; }

        /// <summary>
        /// Lista de códigos de permissões associadas a esta funcionalidade
        /// </summary>
        [Description("Lista de códigos de permissões associadas a esta funcionalidade")]
        public List<string> Permissoes { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public FuncionalidadeInfo()
        {
            this.Permissoes = new List<string>();
            this.Perfis = new List<string>();
            this.Agrupamento = new FuncionalidadeGrupoInfo();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoFuncionalidade;
        }

        #endregion

        public override string ToString()
        {
            return Serializador.TransformarEmString(this);
        }
    }
}
