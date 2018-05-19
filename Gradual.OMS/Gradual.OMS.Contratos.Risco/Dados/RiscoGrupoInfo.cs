using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Risco.Dados
{
    /// <summary>
    /// Classe de dados que representa uma relação de um elemento de risco.
    /// Esta classe armazena informações como o código do ativo, código do sistema cliente,
    /// etc, que podem ser relacionados a alguma entidade de risco como, por exemplo, 
    /// um saldo, ou um conjunto de regras.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RiscoGrupoInfo
    {
        /// <summary>
        /// Indica filtro por código do ativo ou qualquer codigo de ativo (caso seja nulo).
        /// Entende-se por código de ativo o código completo do ativo, por exemplo: 
        /// USIM5, INDJ10, PETRE44, etc.
        /// </summary>
        public string CodigoAtivo { get; set; }

        /// <summary>
        /// Indica filtro por codigo ativo base ou qualquer código de ativo base (caso seja nulo).
        /// Entende-se por código ativo base o início do código do ativo, por exemplo:
        /// USIM, refere-se a qualquer ativo iniciando por USIM, ou seja, USIM5, USIM3, 
        /// USIMC32, etc.
        /// </summary>
        public string CodigoAtivoBase { get; set; }
        
        /// <summary>
        /// Indica filtro por código da bolsa ou qualquer bolsa (caso seja nulo).
        /// </summary>
        public string CodigoBolsa { get; set; }

        /// <summary>
        /// Indica filtro por usuário ou qualquer usuário (caso seja nulo).
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Indica filtro por perfil de risco.
        /// </summary>
        public string CodigoPerfilRisco { get; set; }

        /// <summary>
        /// Indica filtro por sistema ou qualquer sistema (caso seja nulo).
        /// </summary>
        public string CodigoSistemaCliente { get; set; }

        /// <summary>
        /// Conversão para string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Serializador.TransformarEmString(this);
        }
    }
}
