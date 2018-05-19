using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Comum
{
    /// <summary>
    /// Interface para o serviço que contem as funções de segurança.
    /// Permite o gerenciamento de sessão, usuários, perfis, permissões, grupos de usuários.
    /// </summary>
    public interface IServicoSeguranca
    {
        #region Comum

        /// <summary>
        /// Método genérico para processamento de mensagem
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        MensagemResponseBase ProcessarMensagem(MensagemRequestBase parametros);

        /// <summary>
        /// Inicializa o serviço de segurança.
        /// Garante a existencia do usuário administrador
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        InicializarSegurancaResponse InicializarSeguranca(InicializarSegurancaRequest parametros);

        #endregion

        #region Autenticacao

        /// <summary>
        /// Solicita autenticação do usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        AutenticarUsuarioResponse AutenticarUsuario(AutenticarUsuarioRequest parametros);

        #endregion

        #region Permissões

        /// <summary>
        /// Faz a consulta para saber se a sessao possui determinada permissao
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        VerificarPermissaoResponse VerificarPermissao(VerificarPermissaoRequest parametros);

        /// <summary>
        /// Solicita a lista de permissões do sistema
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarPermissoesResponse ListarPermissoes(ListarPermissoesRequest parametros);

        /// <summary>
        /// Solicita a adição de uma permissão a usuários, grupos ou perfis
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        AssociarPermissaoResponse AssociarPermissao(AssociarPermissaoRequest parametros);

        /// <summary>
        /// Solicita a validação de um item de segurança.
        /// Verifica se a sessão uma ou mais permissões informadas no item de segurança.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ValidarItemSegurancaResponse ValidarItemSeguranca(ValidarItemSegurancaRequest parametros);

        /// <summary>
        /// Solicita que sejam salvas todas as permissões do sistema.
        /// A persistencia se encarregará de salvar no local apropriado.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarPermissoesResponse SalvarPermissoes(SalvarPermissoesRequest parametros);

        #endregion

        #region Usuário

        /// <summary>
        /// Solicitação para salvar um usuario
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarUsuarioResponse SalvarUsuario(SalvarUsuarioRequest parametros);

        /// <summary>
        /// Solicita lista de usuários
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarUsuariosResponse ListarUsuarios(ListarUsuariosRequest parametros);

        /// <summary>
        /// Solicita detalhe do usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberUsuarioResponse ReceberUsuario(ReceberUsuarioRequest parametros);

        /// <summary>
        /// Solicitação remoção do usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverUsuarioResponse RemoverUsuario(RemoverUsuarioRequest parametros);

        /// <summary>
        /// Solicita a validação da assinatura eletrônica
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ValidarAssinaturaEletronicaResponse ValidarAssinaturaEletronica(ValidarAssinaturaEletronicaRequest parametros);

        /// <summary>
        /// Solicita alteração da assinatura eletrônica para a sessão
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        AlterarAssinaturaEletronicaResponse AlterarAssinaturaEletronica(AlterarAssinaturaEletronicaRequest parametros);

        /// <summary>
        /// Solicita alteração de senha do usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        AlterarSenhaResponse AlterarSenha(AlterarSenhaRequest parametros);

        #endregion

        #region UsuarioGrupo

        /// <summary>
        /// Solicitação para salvar um grupo de usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarUsuarioGrupoResponse SalvarUsuarioGrupo(SalvarUsuarioGrupoRequest parametros);

        /// <summary>
        /// Solicita lista de grupos de usuários
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarUsuarioGruposResponse ListarUsuarioGrupos(ListarUsuarioGruposRequest parametros);

        /// <summary>
        /// Solicita a associação de um usuário grupo a outra entidade
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        AssociarUsuarioGrupoResponse AssociarUsuarioGrupo(AssociarUsuarioGrupoRequest parametros);

        /// <summary>
        /// Solicita detalhe do grupo de usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberUsuarioGrupoResponse ReceberUsuarioGrupo(ReceberUsuarioGrupoRequest parametros);

        /// <summary>
        /// Solicitação remoção do grupo de usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverUsuarioGrupoResponse RemoverUsuarioGrupo(RemoverUsuarioGrupoRequest parametros);
        
        #endregion

        #region Perfil

        /// <summary>
        /// Solicitação para salvar um perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarPerfilResponse SalvarPerfil(SalvarPerfilRequest parametros);

        /// <summary>
        /// Solicitação de lista de perfis
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarPerfisResponse ListarPerfis(ListarPerfisRequest parametros);

        /// <summary>
        /// Solicita a associação de um perfil com alguma entidade
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        AssociarPerfilResponse AssociarPerfil(AssociarPerfilRequest parametros);

        /// <summary>
        /// Solicita detalhe do perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberPerfilResponse ReceberPerfil(ReceberPerfilRequest parametros);

        /// <summary>
        /// Solicitação remoção do perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverPerfilResponse RemoverPerfil(RemoverPerfilRequest parametros);

        #endregion

        #region Sessao

        /// <summary>
        /// Retorna informações da sessão solicitada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberSessaoResponse ReceberSessao(ReceberSessaoRequest parametros);

        #endregion
    }
}
