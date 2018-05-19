using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Risco.Regras
{
    /// <summary>
    /// Gerador de regras do risco.
    /// </summary>
    public class GeradorRegraRisco : IGeradorRegra
    {
        /// <summary>
        /// Referencia para o serviço de risco
        /// </summary>
        private IServicoRisco _servicoRisco = Ativador.Get<IServicoRisco>();

        /// <summary>
        /// Referencia para o serviço de segurança
        /// </summary>
        private IServicoSeguranca _servicoSeguranca = Ativador.Get<IServicoSeguranca>();

        /// <summary>
        /// Construtor default
        /// </summary>
        public GeradorRegraRisco()
        {
        }
        
        #region IGeradorRegra Members

        /// <summary>
        /// Realiza a geração de regras para o contexto informado.
        /// </summary>
        /// <param name="contexto"></param>
        public void GerarSequencia(ContextoValidacaoInfo contexto)
        {
            // De acordo com o tipo da mensagem
            if (contexto.TipoMensagem == typeof(AutenticarUsuarioRequest))
            {
                // Regra de inicialização de cliente
            }
            if (contexto.TipoMensagem == typeof(ExecutarOrdemRequest))
            {
                // Pega a mensagem com o tipo correto
                ExecutarOrdemRequest mensagem = (ExecutarOrdemRequest)contexto.Mensagem;

                // Pega informações da sessao do cliente
                SessaoInfo sessao =
                    _servicoSeguranca.ReceberSessao(
                        new ReceberSessaoRequest()
                        {
                            CodigoSessaoARetornar = mensagem.CodigoSessao
                        }).Sessao;

                // Insere sessao no contexto
                contexto.Complementos.AdicionarItem<SessaoInfo>(sessao);

                // Realiza as traduções de usuário necessárias
                string codigoUsuario = null;
                if (mensagem.CodigoUsuarioDestino != null && mensagem.CodigoUsuarioDestino != "")
                {
                    // Pega pelo cache pelo código do usuário
                    codigoUsuario = mensagem.CodigoUsuarioDestino;
                }
                else if (mensagem.Account != null && mensagem.Account != "")
                {
                    // Traduz o código cblc para o código do usuário
                    codigoUsuario = RegrasRiscoLib.CarregarUsuarioCBLC(contexto, mensagem.Account).CodigoUsuario;
                }
                else
                {
                    // Pega código do usuário pelo código da sessão
                    codigoUsuario = sessao.CodigoUsuario;
                }
                
                // Solicita o cache do risco
                CacheRiscoInfo cacheRisco = 
                    _servicoRisco.ReceberCacheRisco(
                        new ReceberCacheRiscoRequest()
                        {
                            CodigoSessao = mensagem.CodigoSessao,
                            CodigoUsuario = codigoUsuario
                        }).CacheRisco;

                // Adiciona no contexto
                contexto.Complementos.AdicionarItem<CacheRiscoInfo>(cacheRisco);
                
                // Gera o agrupamento através da mensagem 
                RiscoGrupoInfo agrupamento = new RiscoGrupoInfo();
                agrupamento.CodigoAtivo = mensagem.Symbol;
                agrupamento.CodigoBolsa = mensagem.CodigoBolsa;
                agrupamento.CodigoUsuario = codigoUsuario;
                agrupamento.CodigoSistemaCliente = sessao.CodigoSistemaCliente;
                agrupamento.CodigoPerfilRisco = cacheRisco.Usuario.Complementos.ReceberItem<ContextoOMSInfo>().CodigoPerfilRisco;

                // Adiciona o agrupamento no contexto
                contexto.Complementos.AdicionarItem(agrupamento);

                // Pede na arvore, as regras a serem executadas e adiciona na lista de regras a serem executadas
                ConsultarRegrasDoAgrupamentoResponse resposteRegras = 
                    _servicoRisco.ConsultarRegrasDoAgrupamento(
                        new ConsultarRegrasDoAgrupamentoRequest() 
                        { 
                            Agrupamento = agrupamento 
                        });
                contexto.RegrasAValidar.AddRange(resposteRegras.Regras);
            }
        }

        #endregion
    }
}
