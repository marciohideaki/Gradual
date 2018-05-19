using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Interface.Dados
{
    /// <summary>
    /// Permite classificar as funcionalidades.
    /// </summary>
    [Serializable]
    public class FuncionalidadeGrupoInfo
    {
        /// <summary>
        /// Código do sistema para filtro das funcionalidades.
        /// Por exemplo: Cadastro, Risco, Segurança, etc.
        /// </summary>
        public string CodigoSistema { get; set; }

        /// <summary>
        /// Código de subsistema para filtro das funcionalidades.
        /// Por exemplo, em cadastro: telefones, endereços, contas, etc. Em risco: perfis, etc.
        /// </summary>
        public string CodigoSubSistema { get; set; }
    }
}
