using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Classe para implementação de complementos de autenticação.
    /// Permite que um complemento realize outras validações no usuário
    /// e adicione informações pertinentes.
    /// </summary>
    public interface IComplementoAutenticacao
    {
        /// <summary>
        /// Método disparado no momento da autenticação.
        /// Permite que o complemento adicione informações na sessão do usuário
        /// </summary>
        /// <returns>Mensagem de resposta indicando se a autenticação está ok</returns>
        AutenticarUsuarioResponse ComplementarAutenticacao(AutenticarUsuarioRequest parametros, Sessao sessao);

        /// <summary>
        /// Faz a complementação do usuário, caso necessário
        /// </summary>
        /// <param name="usuarioInfo"></param>
        void ComplementarUsuario(UsuarioInfo usuarioInfo);

        /// <summary>
        /// Chamada realizada quando o serviço de autenticação não
        /// encontra o usuário solicitado. Permite que o complemento crie e
        /// retorne um usuário.
        /// </summary>
        /// <returns></returns>
        UsuarioInfo LocalizarUsuario(AutenticarUsuarioRequest parametros);
    }
}
