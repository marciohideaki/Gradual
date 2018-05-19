using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Interface.Dados
{
    /// <summary>
    /// Representa um grupo de funcionalidades
    /// </summary>
    public class GrupoFuncionalidadeInfo
    {
        /// <summary>
        /// Código do grupo de funcionalidades
        /// </summary>
        public string CodigoGrupoFuncionalidade { get; set; }

        /// <summary>
        /// Lista de funcionalidades deste grupo
        /// </summary>
        public List<FuncionalidadeInfo> Funcionalidades { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public GrupoFuncionalidadeInfo()
        {
            this.Funcionalidades = new List<FuncionalidadeInfo>();
        }
    }
}
