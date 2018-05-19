using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Risco.Regras
{
    /// <summary>
    /// Biblioteca de rotinas auxiliares das regras de risco
    /// </summary>
    public static class RegrasRiscoLib
    {
        /// <summary>
        /// Referencia para o servico de integracao com o sinacor
        /// </summary>
        private static IServicoIntegracaoSinacorOMS _servicoIntegracaoOMS = null;

        /// <summary>
        /// Carrega o usuário através do código cblc.
        /// Caso não consiga pegar o usuário, adiciona as críticas no contexto.
        /// </summary>
        /// <returns></returns>
        public static UsuarioInfo CarregarUsuarioCBLC(ContextoValidacaoInfo contexto, string codigoClienteCBLC)
        {
            // Verifica se o usuário está no contexto
            UsuarioInfo usuarioInfo = contexto.Complementos.ReceberItem<UsuarioInfo>();

            // Se não está, carrega
            if (usuarioInfo == null || usuarioInfo.Complementos.ReceberItem<ContextoOMSInfo>().CodigoCBLC != codigoClienteCBLC)
            {
                // Acha o usuário relativo ao cliente
                if (_servicoIntegracaoOMS == null)
                    _servicoIntegracaoOMS = Ativador.Get<IServicoIntegracaoSinacorOMS>();
                List<UsuarioInfo> usuarios =
                    _servicoIntegracaoOMS.TraduzirCodigoCBLC(
                        new TraduzirCodigoCBLCRequest()
                        {
                            CodigoSessao = contexto.CodigoSessao,
                            CodigoCBLC = codigoClienteCBLC
                        }).Usuarios;

                // Valida se encontrou usuário
                if (usuarios.Count != 1)
                {
                    // Adiciona crítica
                    contexto.Criticas.Add(
                        new CriticaInfo()
                        {
                            Status = CriticaStatusEnum.ErroNegocio,
                            Descricao =
                                string.Format(
                                    "Usuário (CBLC: {0}) inválido. Retornou {1} resultado(s). Esperado é retorno de 1 item.",
                                    codigoClienteCBLC,
                                    usuarios.Count)
                        });

                    // Retorna inválido
                    return null;
                }

                // Pega o usuário
                usuarioInfo = usuarios[0];

                // Coloca no contexto
                contexto.Complementos.AdicionarItem(usuarioInfo);
            }

            // Retorna
            return usuarioInfo;
        }

        /// <summary>
        /// Faz o filtro na custodia informada relativa ao agrupamento
        /// </summary>
        /// <returns></returns>
        public static List<CustodiaPosicaoInfo> ReceberCustodiaDoAgrupamento(CustodiaInfo custodia, RiscoGrupoInfo agrupamento)
        {
            // Faz o filtro das posições
            return 
                (from p in custodia.Posicoes
                 where (agrupamento.CodigoAtivo == null || p.CodigoAtivo == agrupamento.CodigoAtivo) &&
                       (agrupamento.CodigoAtivoBase == null || p.CodigoAtivo.StartsWith(agrupamento.CodigoAtivoBase)) &&
                       (agrupamento.CodigoBolsa == null || p.CodigoBolsa == agrupamento.CodigoBolsa)
                 select p).ToList();
        }
    }
}
