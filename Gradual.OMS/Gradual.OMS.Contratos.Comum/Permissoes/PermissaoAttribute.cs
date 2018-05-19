using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    /// <summary>
    /// Definição do atributo de permissão para ser utilizado
    /// pelas classes de permissões.
    /// </summary>
    public class PermissaoAttribute : Attribute
    {
        /// <summary>
        /// Informações da permissao.
        /// </summary>
        public PermissaoInfo PermissaoInfo { get; set; }
        
        /// <summary>
        /// Codigo da permissao
        /// </summary>
        public string CodigoPermissao
        {
            get { return this.PermissaoInfo.CodigoPermissao; }
            set { this.PermissaoInfo.CodigoPermissao = value; } 
        }

        /// <summary>
        /// Nome da permissão
        /// </summary>
        public string NomePermissao 
        {
            get { return this.PermissaoInfo.NomePermissao; }
            set { this.PermissaoInfo.NomePermissao = value; } 
        }

        /// <summary>
        /// Descricao da permissao
        /// </summary>
        public string DescricaoPermissao 
        {
            get { return this.PermissaoInfo.DescricaoPermissao; }
            set { this.PermissaoInfo.DescricaoPermissao = value; }
        }

        /// <summary>
        /// Construtor default
        /// </summary>
        public PermissaoAttribute()
        {
            this.PermissaoInfo = new PermissaoInfo();
        }

        /// <summary>
        /// Construtor que recebe todos os parametros
        /// </summary>
        /// <param name="codigoPermissao"></param>
        /// <param name="nomePermissao"></param>
        /// <param name="descricaoPermissao"></param>
        public PermissaoAttribute(string codigoPermissao, string nomePermissao, string descricaoPermissao)
        {
            this.PermissaoInfo = new PermissaoInfo();
            this.CodigoPermissao = codigoPermissao;
            this.NomePermissao = nomePermissao;
            this.DescricaoPermissao = descricaoPermissao;
        }
    }
}
