using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.ContaCorrente.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.ContaCorrente
{
    /// <summary>
    /// Implementação do serviço de persistencia de conta corrente
    /// </summary>
    public class ServicoContaCorrentePersistencia : IServicoContaCorrentePersistencia
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia para o servico de persistencia
        /// </summary>
        private IServicoPersistencia _servicoPersistencia = Ativador.Get<IServicoPersistencia>();

        #endregion

        #region IServicoContaCorrentePersistencia Members

        /// <summary>
        /// Lista Contas Correntes de acordo com o filtro informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ConsultarContasCorrentesResponse ConsultarContasCorrentes(ConsultarContasCorrentesRequest parametros)
        {
            // Solicita a lista
            ConsultarObjetosResponse<ContaCorrenteInfo> retorno =
                _servicoPersistencia.ConsultarObjetos<ContaCorrenteInfo>(
                    new ConsultarObjetosRequest<ContaCorrenteInfo>()
                    {
                    });

            // Retorna
            return
                new ConsultarContasCorrentesResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    ContasCorrentes = retorno.Resultado
                };
        }

        /// <summary>
        /// Recebe detalhe de uma ContaCorrente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberContaCorrenteResponse ReceberContaCorrente(ReceberContaCorrenteRequest parametros)
        {
            // Retorna o objeto solicitado
            return
                new ReceberContaCorrenteResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    ContaCorrenteInfo =
                        _servicoPersistencia.ReceberObjeto<ContaCorrenteInfo>(
                            new ReceberObjetoRequest<ContaCorrenteInfo>()
                            {
                                CodigoObjeto = parametros.CodigoContaCorrente
                            }).Objeto
                };
        }

        /// <summary>
        /// Remove uma custódia
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverContaCorrenteResponse RemoverContaCorrente(RemoverContaCorrenteRequest parametros)
        {
            // Remove
            _servicoPersistencia.RemoverObjeto<ContaCorrenteInfo>(
                new RemoverObjetoRequest<ContaCorrenteInfo>()
                {
                    CodigoObjeto = parametros.CodigoContaCorrente
                });

            // Retorna
            return
                new RemoverContaCorrenteResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Salva uma custódia
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarContaCorrenteResponse SalvarContaCorrente(SalvarContaCorrenteRequest parametros)
        {
            // Salva
            SalvarObjetoResponse<ContaCorrenteInfo> respostaSalvar =
                _servicoPersistencia.SalvarObjeto<ContaCorrenteInfo>(
                    new SalvarObjetoRequest<ContaCorrenteInfo>()
                    {
                        Objeto = parametros.ContaCorrenteInfo
                    });

            // Retorna
            return
                new SalvarContaCorrenteResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    ContaCorrente = respostaSalvar.Objeto
                };
        }

        #endregion
    }
}
