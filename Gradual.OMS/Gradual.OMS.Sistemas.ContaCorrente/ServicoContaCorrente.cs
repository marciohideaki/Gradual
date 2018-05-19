using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.ContaCorrente;
using Gradual.OMS.Contratos.ContaCorrente.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.ContaCorrente
{
    /// <summary>
    /// Implementação do serviço de conta corrente
    /// </summary>
    public class ServicoContaCorrente : IServicoContaCorrente
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia para o serviço de persistencia
        /// </summary>
        private IServicoContaCorrentePersistencia _servicoPersistencia = Ativador.Get<IServicoContaCorrentePersistencia>();

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoContaCorrente()
        {
        }

        #endregion

        #region IServicoContaCorrente Members

        /// <summary>
        /// Lista ContaCorrentes de acordo com o filtro informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ConsultarContasCorrentesResponse ConsultarContasCorrentes(ConsultarContasCorrentesRequest parametros)
        {
            // Repassa a mensagem
            return _servicoPersistencia.ConsultarContasCorrentes(parametros);
        }

        /// <summary>
        /// Recebe detalhe de uma ContaCorrente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberContaCorrenteResponse ReceberContaCorrente(ReceberContaCorrenteRequest parametros)
        {
            // Repassa a mensagem
            return _servicoPersistencia.ReceberContaCorrente(parametros);
        }

        /// <summary>
        /// Remove uma conta corrente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverContaCorrenteResponse RemoverContaCorrente(RemoverContaCorrenteRequest parametros)
        {
            // Repassa a mensagem
            return _servicoPersistencia.RemoverContaCorrente(parametros);
        }

        /// <summary>
        /// Salva uma conta corrente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarContaCorrenteResponse SalvarContaCorrente(SalvarContaCorrenteRequest parametros)
        {
            // Repassa a mensagem
            return _servicoPersistencia.SalvarContaCorrente(parametros);
        }

        #endregion
    }
}
