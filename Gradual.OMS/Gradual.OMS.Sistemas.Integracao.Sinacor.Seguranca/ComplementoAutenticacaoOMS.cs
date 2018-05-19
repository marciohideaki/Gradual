using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;
using Gradual.OMS.Sistemas.Comum;

namespace Gradual.OMS.Sistemas.Integracao.Sinacor.OMS
{
    /// <summary>
    /// Classe para complementar informações do usuário, com informações
    /// do sinacor. Irá associar um ContextoSinacor ao usuário.
    /// Aqui assume-se que o recurso de pipeline de autenticação de usuários
    /// já esteja funcionando e esta classe irá implementar a interface deste
    /// pipeline.
    /// </summary>
    public class ComplementoAutenticacaoOMS : IComplementoAutenticacao
    {
        #region IComplementoAutenticacao Members

        /// <summary>
        /// Complementa autenticação com informações do OMS
        /// </summary>
        /// <param name="parametros"></param>
        /// <param name="sessao"></param>
        /// <returns></returns>
        public AutenticarUsuarioResponse ComplementarAutenticacao(AutenticarUsuarioRequest parametros, Sessao sessao)
        {
            // Garante que o usuário terá o complemento do sinacor
            this.ComplementarUsuario(sessao.Usuario.UsuarioInfo);
            
            // Inicializa
            ContextoOMSInfo contexto = sessao.Usuario.UsuarioInfo.Complementos.ReceberItem<ContextoOMSInfo>();

            // Verifica se foi feita a consulta do usuário no sinacor
            if (contexto.CodigoCBLC != null && !contexto.SinacorVerificado)
            {
                // Não foi, acessa o cadastro do usuário no sinacor para complementar o assessor
                contexto.CodigoCBLCAssessor = "xxx";
            }

            // Retorna 
            return 
                new AutenticarUsuarioResponse() 
                { 
                    StatusResposta = MensagemResponseStatusEnum.OK
                };
        }

        /// <summary>
        /// Complementa usuário com contexto do OMS
        /// </summary>
        /// <param name="usuarioInfo"></param>
        public void ComplementarUsuario(UsuarioInfo usuarioInfo)
        {
            // Inicializa
            ContextoOMSInfo contexto = usuarioInfo.Complementos.ReceberItem<ContextoOMSInfo>();

            // Se não possui o contexto, adiciona
            if (contexto == null)
            {
                contexto = new ContextoOMSInfo();
                usuarioInfo.Complementos.AdicionarItem(contexto);
            }
        }

        public UsuarioInfo LocalizarUsuario(AutenticarUsuarioRequest parametros)
        {
            return null;
        }

        #endregion
    }
}
