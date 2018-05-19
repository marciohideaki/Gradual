using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Risco
{
    /// <summary>
    /// Implementação do serviço de persistencia do risco
    /// </summary>
    public class ServicoRiscoPersistencia : IServicoRiscoPersistencia
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia para o servico de persistencia
        /// </summary>
        private IServicoPersistencia _servicoPersistencia = Ativador.Get<IServicoPersistencia>();

        #endregion

        #region IServicoRiscoPersistencia Members

        #region RegraRisco

        /// <summary>
        /// Lista regras de risco de acordo com o filtro informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarRegraRiscoResponse ListarRegraRisco(ListarRegraRiscoRequest parametros)
        {
            // Prepara resposta
            ListarRegraRiscoResponse resposta =
                new ListarRegraRiscoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            
            // Solicita a lista
            ConsultarObjetosResponse<RegraRiscoInfo> retorno =
                _servicoPersistencia.ConsultarObjetos<RegraRiscoInfo>(
                    new ConsultarObjetosRequest<RegraRiscoInfo>()
                    {
                    });

            // Se tem agrupamento, faz o filtro por agrupamento
            if (parametros.FiltroAgrupamento != null)
            {
                // Filtra
                resposta.Resultado =
                    (from r in retorno.Resultado
                     where (parametros.FiltroAgrupamento.CodigoAtivo == null || r.Agrupamento.CodigoAtivo == parametros.FiltroAgrupamento.CodigoAtivo) &&
                           (parametros.FiltroAgrupamento.CodigoAtivoBase == null || r.Agrupamento.CodigoAtivoBase == parametros.FiltroAgrupamento.CodigoAtivoBase) &&
                           (parametros.FiltroAgrupamento.CodigoBolsa == null || r.Agrupamento.CodigoBolsa == parametros.FiltroAgrupamento.CodigoBolsa) &&
                           (parametros.FiltroAgrupamento.CodigoPerfilRisco == null || r.Agrupamento.CodigoPerfilRisco == parametros.FiltroAgrupamento.CodigoPerfilRisco) &&
                           (parametros.FiltroAgrupamento.CodigoSistemaCliente == null || r.Agrupamento.CodigoSistemaCliente == parametros.FiltroAgrupamento.CodigoSistemaCliente) &&
                           (parametros.FiltroAgrupamento.CodigoUsuario == null || r.Agrupamento.CodigoUsuario == parametros.FiltroAgrupamento.CodigoUsuario)
                     select r).ToList();
            }
            else
            {
                // Retorna o resultado completo
                resposta.Resultado = retorno.Resultado;
            }
            
            // Retorna
            return resposta;
        }

        /// <summary>
        /// Recebe a regra de risco solicitada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberRegraRiscoResponse ReceberRegraRisco(ReceberRegraRiscoRequest parametros)
        {
            // Retorna o objeto solicitado
            return
                new ReceberRegraRiscoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    RegraRiscoInfo =
                        _servicoPersistencia.ReceberObjeto<RegraRiscoInfo>(
                            new ReceberObjetoRequest<RegraRiscoInfo>()
                            {
                                CodigoObjeto = parametros.CodigoRegraRisco
                            }).Objeto
                };
        }

        /// <summary>
        /// Remove a regra de risco informada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverRegraRiscoResponse RemoverRegraRisco(RemoverRegraRiscoRequest parametros)
        {
            // Remove
            _servicoPersistencia.RemoverObjeto<RegraRiscoInfo>(
                new RemoverObjetoRequest<RegraRiscoInfo>()
                {
                    CodigoObjeto = parametros.CodigoRegraRisco
                });

            // Avisa o risco
            Ativador.Get<IServicoRisco>().RecarregarListaRegras(new RecarregarListaRegrasRequest());

            // Retorna
            return
                new RemoverRegraRiscoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Salva a regra de risco informada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarRegraRiscoResponse SalvarRegraRisco(SalvarRegraRiscoRequest parametros)
        {
            // Salva
            SalvarObjetoResponse<RegraRiscoInfo> respostaSalvar =
                _servicoPersistencia.SalvarObjeto<RegraRiscoInfo>(
                    new SalvarObjetoRequest<RegraRiscoInfo>()
                    {
                        Objeto = parametros.RegraRiscoInfo
                    });

            // Avisa o risco
            Ativador.Get<IServicoRisco>().RecarregarListaRegras(new RecarregarListaRegrasRequest());

            // Retorna
            return
                new SalvarRegraRiscoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    RegraRisco = respostaSalvar.Objeto
                };
        }

        #endregion

        #endregion
    }
}
