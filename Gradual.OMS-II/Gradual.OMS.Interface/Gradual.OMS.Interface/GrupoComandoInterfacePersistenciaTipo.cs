using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Interface
{
    /// <summary>
    /// Indica como será considerada a persistencia do grupo de comandos de interface
    /// </summary>
    public enum GrupoComandoInterfacePersistenciaTipo
    {
        /// <summary>
        /// Indica que os itens de grupo irão vir do arquivo de configuração
        /// </summary>
        ArquivoConfiguracao,

        /// <summary>
        /// Indica que os itens de grupo irão vir da persistencia
        /// </summary>
        Persistencia,

        /// <summary>
        /// Indica que os itens irão vir da persistencia e do arquivo de configuração.
        /// Os grupos contidos no arquivo de configuração são apenas leitura.
        /// </summary>
        Misto
    }
}
