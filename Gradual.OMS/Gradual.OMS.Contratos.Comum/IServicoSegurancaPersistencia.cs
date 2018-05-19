using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Comum
{
    /// <summary>
    /// Interface para o serviço de persistencia de segurança
    /// </summary>
    public interface IServicoSegurancaPersistencia
    {
        #region Perfil

        /// <summary>
        /// Consulta de perfis de acordo com os filtros
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarPerfisResponse ListarPerfis(ListarPerfisRequest parametros);

        /// <summary>
        /// Salva um perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarPerfilResponse SalvarPerfil(SalvarPerfilRequest parametros);

        /// <summary>
        /// Recebe detalhe de um perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberPerfilResponse ReceberPerfil(ReceberPerfilRequest parametros);

        /// <summary>
        /// Remove um perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverPerfilResponse RemoverPerfil(RemoverPerfilRequest parametros);

        #endregion

        #region Sessao

        /// <summary>
        /// Faz a consulta de sessoes de acordo com os filtros
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ConsultarSessoesResponse ConsultarSessoes(ConsultarSessoesRequest parametros);

        /// <summary>
        /// Salva a sessao
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarSessaoResponse SalvarSessao(SalvarSessaoRequest parametros);

        /// <summary>
        /// Recebe o detalhe da sessao
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberSessaoResponse ReceberSessao(ReceberSessaoRequest parametros);

        /// <summary>
        /// Remove a sessao
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverSessaoResponse RemoverSessao(RemoverSessaoRequest parametros);

        #endregion

        #region Sistema Cliente

        /// <summary>
        /// Consulta sistemas cliente de acordo com o filtro
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ConsultarSistemasClienteResponse ConsultarSistemasCliente(ConsultarSistemasClienteRequest parametros);

        /// <summary>
        /// Salva um sistema cliente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarSistemaClienteResponse SalvarSistemaCliente(SalvarSistemaClienteRequest parametros);

        /// <summary>
        /// Recebe o detalhe de um sistema cliente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberSistemaClienteResponse ReceberSistemaCliente(ReceberSistemaClienteRequest parametros);

        /// <summary>
        /// Remove um sistema cliente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverSistemaClienteResponse RemoverSistemaCliente(RemoverSistemaClienteRequest parametros);

        #endregion

        #region Usuario Grupo

        /// <summary>
        /// Consulta Usuario Grupos de acordo com o filtro
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarUsuarioGruposResponse ListarUsuarioGrupos(ListarUsuarioGruposRequest parametros);

        /// <summary>
        /// Salva um usuário grupo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarUsuarioGrupoResponse SalvarUsuarioGrupo(SalvarUsuarioGrupoRequest parametros);

        /// <summary>
        /// Recebe detalhe do usuário grupo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberUsuarioGrupoResponse ReceberUsuarioGrupo(ReceberUsuarioGrupoRequest parametros);

        /// <summary>
        /// Remove um usuário grupo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverUsuarioGrupoResponse RemoverUsuarioGrupo(RemoverUsuarioGrupoRequest parametros);

        #endregion

        #region Usuario

        /// <summary>
        /// Faz consulta de usuários de acordo com o filtro especificado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarUsuariosResponse ListarUsuarios(ListarUsuariosRequest parametros);

        /// <summary>
        /// Salva usuario
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarUsuarioResponse SalvarUsuario(SalvarUsuarioRequest parametros);

        /// <summary>
        /// Recebe detalhe do usuario
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberUsuarioResponse ReceberUsuario(ReceberUsuarioRequest parametros);

        /// <summary>
        /// Remove usuario
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverUsuarioResponse RemoverUsuario(RemoverUsuarioRequest parametros);

        #endregion
    }
}
