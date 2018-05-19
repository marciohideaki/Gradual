using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Interface.Mensagens;

namespace Gradual.OMS.Interface
{
    /// <summary>
    /// Interface para o serviço de persistencia de interface (visual)
    /// </summary>
    public interface IServicoInterfacePersistencia
    {
        #region GrupoComandoInterface

        /// <summary>
        /// Faz consulta de grupos de comandos de interface de acordo com o filtro especificado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarGruposComandoInterfaceResponse ListarGruposComandoInterface(ListarGruposComandoInterfaceRequest parametros);

        /// <summary>
        /// Salva GrupoComandoInterface
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarGrupoComandoInterfaceResponse SalvarGrupoComandoInterface(SalvarGrupoComandoInterfaceRequest parametros);

        /// <summary>
        /// Recebe detalhe do GrupoComandoInterface
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberGrupoComandoInterfaceResponse ReceberGrupoComandoInterface(ReceberGrupoComandoInterfaceRequest parametros);

        /// <summary>
        /// Remove GrupoComandoInterface
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverGrupoComandoInterfaceResponse RemoverGrupoComandoInterface(RemoverGrupoComandoInterfaceRequest parametros);

        #endregion
    }
}
