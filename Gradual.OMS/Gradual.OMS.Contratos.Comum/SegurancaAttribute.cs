using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum
{
    /// <summary>
    /// Atributo que representa uma validação de segurança que deve ser feita no 
    /// tipo que declarou o atributo.
    /// </summary>
    public class SegurancaAttribute : Attribute
    {
        /// <summary>
        /// Informações de segurança.
        /// </summary>
        public ItemSegurancaInfo Seguranca { get; set; }

        /// <summary>
        /// Lista de grupos da ativação
        /// </summary>
        public List<string> Grupos 
        {
            get { return this.Seguranca.Grupos; }
            set { this.Seguranca.Grupos = value; } 
        }

        /// <summary>
        /// Lista de perfis da ativação
        /// </summary>
        public List<string> Perfis 
        {
            get { return this.Seguranca.Perfis; }
            set { this.Seguranca.Perfis = value; }
        }

        /// <summary>
        /// Lista de permissões da ativação
        /// </summary>
        public List<string> Permissoes 
        {
            get { return this.Seguranca.Permissoes; }
            set { this.Seguranca.Permissoes = value; } 
        }

        /// <summary>
        /// Tipo da ativação.
        /// </summary>
        public ItemSegurancaAtivacaoTipoEnum TipoAtivacao 
        {
            get { return this.Seguranca.TipoAtivacao; }
            set { this.Seguranca.TipoAtivacao = value; }
        }

        /// <summary>
        /// Construtor default
        /// </summary>
        public SegurancaAttribute()
        {
            this.Seguranca = new ItemSegurancaInfo();
        }
    }
}
