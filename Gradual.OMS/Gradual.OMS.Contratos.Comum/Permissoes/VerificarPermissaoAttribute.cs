using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Permissoes
{
    /// <summary>
    /// Atributo para realizar a verificacao de permissoes.
    /// Este atributo é utilizado pela regra de Verificacao de Permissao.
    /// </summary>
    public class VerificarPermissaoAttribute : Attribute
    {
        /// <summary>
        /// Item de seguranca a ser validado
        /// </summary>
        public ItemSegurancaInfo ItemSeguranca { get; set; }

        /// <summary>
        /// Grupos Permitidos
        /// </summary>
        public string Grupos 
        {
            get { return this.ItemSeguranca.GruposString; }
            set { this.ItemSeguranca.GruposString = value; }
        }

        ///// <summary>
        ///// Perfis permitidos
        ///// </summary>
        //public string Perfis
        //{
        //    get { return this.ItemSeguranca.Perfis; }
        //    set { this.ItemSeguranca.Perfis = value; }
        //}

        /// <summary>
        /// Construtor default
        /// </summary>
        public VerificarPermissaoAttribute()
        {
            this.ItemSeguranca = new ItemSegurancaInfo();
            this.ItemSeguranca.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.QualquerCondicao;
        }
    }
}
