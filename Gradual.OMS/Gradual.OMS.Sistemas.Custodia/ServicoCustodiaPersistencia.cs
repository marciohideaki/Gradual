using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Custodia;
using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Custodia.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Custodia
{
    /// <summary>
    /// Implementação do serviço de persistencia de custódia
    /// </summary>
    public class ServicoCustodiaPersistencia : IServicoCustodiaPersistencia
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia para o servico de persistencia
        /// </summary>
        private IServicoPersistencia _servicoPersistencia = Ativador.Get<IServicoPersistencia>();

        #endregion

        #region IServicoCustodiaPersistencia Members

        /// <summary>
        /// Lista custodias de acordo com o filtro informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ConsultarCustodiasResponse ConsultarCustodias(ConsultarCustodiasRequest parametros)
        {
            // Solicita a lista
            ConsultarObjetosResponse<CustodiaInfo> retorno =
                _servicoPersistencia.ConsultarObjetos<CustodiaInfo>(
                    new ConsultarObjetosRequest<CustodiaInfo>()
                    {
                    });

            // Retorna
            return
                new ConsultarCustodiasResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Custodias = retorno.Resultado
                };
        }

        /// <summary>
        /// Recebe detalhe de uma custodia
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberCustodiaResponse ReceberCustodia(ReceberCustodiaRequest parametros)
        {
            // Retorna o objeto solicitado
            return
                new ReceberCustodiaResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    CustodiaInfo =
                        _servicoPersistencia.ReceberObjeto<CustodiaInfo>(
                            new ReceberObjetoRequest<CustodiaInfo>()
                            {
                                CodigoObjeto = parametros.CodigoCustodia
                            }).Objeto
                };
        }

        /// <summary>
        /// Remove uma custódia
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverCustodiaResponse RemoverCustodia(RemoverCustodiaRequest parametros)
        {
            // Remove
            _servicoPersistencia.RemoverObjeto<CustodiaInfo>(
                new RemoverObjetoRequest<CustodiaInfo>()
                {
                    CodigoObjeto = parametros.CodigoCustodia
                });

            // Retorna
            return
                new RemoverCustodiaResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Salva uma custódia
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarCustodiaResponse SalvarCustodia(SalvarCustodiaRequest parametros)
        {
            // Salva
            SalvarObjetoResponse<CustodiaInfo> respostaSalvar =
                _servicoPersistencia.SalvarObjeto<CustodiaInfo>(
                    new SalvarObjetoRequest<CustodiaInfo>()
                    {
                        Objeto = parametros.CustodiaInfo
                    });

            // Retorna
            return
                new SalvarCustodiaResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Custodia = respostaSalvar.Objeto
                };
        }

        #endregion
    }
}
