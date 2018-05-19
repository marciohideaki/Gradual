using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Indica um relação entre dois usuarios.
    /// </summary>
    [Serializable]
    public class UsuarioRelacaoInfo
    {
        /// <summary>
        /// Código do usuário 1
        /// </summary>
        public string CodigoUsuario1 { get; set; }

        /// <summary>
        /// Código do usuário 2
        /// </summary>
        public string CodigoUsuario2 { get; set; }

        /// <summary>
        /// Tipo da relação entre os dois usuários
        /// </summary>
        public UsuarioRelacaoTipoEnum TipoRelacao { get; set; }

        public override string ToString()
        {
            return Serializador.TransformarEmString(this);
        }
    }
}
