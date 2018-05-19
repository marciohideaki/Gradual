using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Gradual.OMS.Interface.Dados;

namespace Gradual.OMS.Interface
{
    /// <summary>
    /// Classe de configurações para o serviço de interface
    /// </summary>
    [Serializable]
    public class ServicoInterfaceConfig
    {
        /// <summary>
        /// Permite cadastrar na configuração, grupos de comandos de interface
        /// </summary>
        public List<GrupoComandoInterfaceInfo> GruposComandoInterface { get; set; }

        /// <summary>
        /// Indica como será tratada a persitencia de grupos de comandos de interface
        /// </summary>
        public GrupoComandoInterfacePersistenciaTipo TipoPersistenciaGrupoComandoInterface { get; set; }

        /// <summary>
        /// Lista de funcionalidades
        /// </summary>
        public List<FuncionalidadeInfo> Funcionalidades { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoInterfaceConfig()
        {
            this.TipoPersistenciaGrupoComandoInterface = GrupoComandoInterfacePersistenciaTipo.Persistencia;
            this.GruposComandoInterface = new List<GrupoComandoInterfaceInfo>();
            this.Funcionalidades = new List<FuncionalidadeInfo>();
        }
    }
}
