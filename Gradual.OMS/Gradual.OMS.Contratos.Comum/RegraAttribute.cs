using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum
{
    /// <summary>
    /// Contem definições para um atributo de regra.
    /// Permite que a regra possa informar um código, o tipo do config que espera
    /// e um tipo de tela esperado para cadastro.
    /// </summary>
    public class RegraAttribute : Attribute
    {
        /// <summary>
        /// Chave primária da regra.
        /// </summary>
        public string CodigoRegra { get; set; }

        /// <summary>
        /// Nome da regra
        /// </summary>
        public string NomeRegra { get; set; }

        /// <summary>
        /// Descrição da regra
        /// </summary>
        public string DescricaoRegra { get; set; }

        /// <summary>
        /// Tipo do config utilizado pela regra.
        /// </summary>
        public Type TipoConfig { get; set; }

        /// <summary>
        /// Indica se esta é uma regra que pode ser editada pelo
        /// usuário. Existem regras, como por exemplo a que trata
        /// o login e logout (no sistema de risco) que não pode 
        /// ser utilizada pelos usuários finais.
        /// </summary>
        public bool RegraDeUsuario { get; set; }
    }
}
