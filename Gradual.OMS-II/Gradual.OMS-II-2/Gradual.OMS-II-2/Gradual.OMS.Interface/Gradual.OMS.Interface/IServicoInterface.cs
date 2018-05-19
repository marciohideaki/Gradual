using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Interface.Mensagens;

namespace Gradual.OMS.Interface
{
    /// <summary>
    /// Interface para o serviço de interface (visual)
    /// </summary>
    public interface IServicoInterface
    {
        #region Persistencia de GrupoComandoInterface

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

        #region Árvore de Comandos

        /// <summary>
        /// Solicita o processamento dos comandos de interface. Verifica os comandos permitidos e retorna
        /// a árvore.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberArvoreComandosInterfaceResponse ReceberArvoreComandosInterface(ReceberArvoreComandosInterfaceRequest parametros);

        #endregion

        #region Funcionalidades

        /// <summary>
        /// Recebe a visao de funcionalidades para um usuário ou grupo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberVisaoFuncionalidadesResponse ReceberVisaoFuncionalidades(ReceberVisaoFuncionalidadesRequest parametros);

        /// <summary>
        /// Salva a visao de funcionalidades para um usuário ou grupo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarVisaoFuncionalidadesResponse SalvarVisaoFuncionalidades(SalvarVisaoFuncionalidadesRequest parametros);

        #endregion
    }
}
