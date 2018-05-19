using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Mensageria;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet.Servicos.Mock
{
    public class ServicoMensageria : IServicoMensageria
    {

        #region IServicoMensageria Members

        public MensagemResponseBase ProcessarMensagem(MensagemRequestBase parametros)
        {
            Type lTipoParametro = parametros.GetType();

            if (lTipoParametro == typeof(AutenticarUsuarioRequest))
            {
                AutenticarUsuarioResponse lResposta = new AutenticarUsuarioResponse();

                lResposta.StatusResposta = MensagemResponseStatusEnum.OK;

                lResposta.Sessao = new SessaoInfo();

                lResposta.Sessao.CodigoSessao = Guid.NewGuid().ToString();  

                return lResposta;
            }
            else if (lTipoParametro == typeof(ValidarItemSegurancaRequest))
            {
                ValidarItemSegurancaResponse lResposta = new ValidarItemSegurancaResponse();

                lResposta.StatusResposta = MensagemResponseStatusEnum.OK;

                lResposta.ItensSeguranca = ((ValidarItemSegurancaRequest)parametros).ItensSeguranca;

                foreach (ItemSegurancaInfo lItem in lResposta.ItensSeguranca)
                {
                    lItem.Valido = true;
                }

                return lResposta;
            }
            else if (lTipoParametro == typeof(ListarUsuariosRequest))
            {
                ListarUsuariosResponse lResposta = new ListarUsuariosResponse();

                lResposta.Usuarios = new List<UsuarioInfo>();

                lResposta.Usuarios.Add(new UsuarioInfo()
                {
                    Nome = "Teste",
                    Email = "teste@teste.com",
                    CodigoUsuario = "1"
                });

                return lResposta;
            }
            else
            {
                throw new NotImplementedException(string.Format("Mock não implementado para [{0}]", lTipoParametro));
            }
        }

        #endregion
    }
}
