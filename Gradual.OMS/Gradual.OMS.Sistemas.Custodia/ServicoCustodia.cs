using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Custodia;
using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Custodia.Mensagens;
using Gradual.OMS.Library.Servicos;

using Orbite.RV.Contratos.MarketData.Bovespa;
using Orbite.RV.Contratos.MarketData.Bovespa.Dados;
using Orbite.RV.Contratos.MarketData.Bovespa.Mensagens;

namespace Gradual.OMS.Sistemas.Custodia
{
    public class ServicoCustodia : IServicoCustodia
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia para o serviço de persistencia
        /// </summary>
        private IServicoCustodiaPersistencia _servicoPersistencia = Ativador.Get<IServicoCustodiaPersistencia>();

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoCustodia()
        {
        }

        #endregion

        #region IServicoCustodia Members

        /// <summary>
        /// Lista custodias de acordo com o filtro informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ConsultarCustodiasResponse ConsultarCustodias(ConsultarCustodiasRequest parametros)
        {
            // Repassa a mensagem
            return _servicoPersistencia.ConsultarCustodias(parametros);
        }

        /// <summary>
        /// Recebe detalhe de uma custodia
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberCustodiaResponse ReceberCustodia(ReceberCustodiaRequest parametros)
        {
            // Pede a custodia
            ReceberCustodiaResponse resposta = 
                _servicoPersistencia.ReceberCustodia(parametros);

            // Verifica se deve carregar as cotações 
            if (resposta.CustodiaInfo != null && parametros.CarregarCotacoes)
            {
                // ***********
                //   BOVESPA
                // ***********

                // Filtra as custodias bovespa
                List<CustodiaPosicaoInfo> posicoesBovespa =
                    (from p in resposta.CustodiaInfo.Posicoes
                     where p.CodigoBolsa == "BOVESPA"
                     select p).ToList();

                // Cria lista com os ativos para pedir as cotações
                List<string> listaAtivosBovespa =
                    (from p in posicoesBovespa
                     select p.CodigoAtivo).Distinct().ToList();

                // Pede as cotações
                IServicoMarketDataBovespa servicoMarketDataBovespa =
                    Ativador.Get<IServicoMarketDataBovespa>();
                ReceberUltimaCotacaoBovespaResponse respostaUltimaCotacaoBovespa =
                    servicoMarketDataBovespa.ReceberUltimaCotacaoBovespa(
                        new ReceberUltimaCotacaoBovespaRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            Ativos = listaAtivosBovespa
                        });

                // Insere as cotações
                foreach (CotacaoBovespaInfo cotacaoBovespa in respostaUltimaCotacaoBovespa.Cotacoes)
                {
                    // Acha as posições do ativo informado
                    IEnumerable<CustodiaPosicaoInfo> posicoesDoAtivo =
                        from p in posicoesBovespa
                        where p.CodigoAtivo == cotacaoBovespa.Ativo
                        select p;

                    // Insere a cotação
                    foreach (CustodiaPosicaoInfo custodiaPosicao in posicoesDoAtivo)
                    {
                        custodiaPosicao.ValorCotacao = cotacaoBovespa.Fechamento;
                        custodiaPosicao.DataCotacao = cotacaoBovespa.Data;
                    }
                }
            }

            // Repassa a mensagem
            return resposta;
        }

        /// <summary>
        /// Remove uma custódia
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverCustodiaResponse RemoverCustodia(RemoverCustodiaRequest parametros)
        {
            // Repassa a mensagem
            return _servicoPersistencia.RemoverCustodia(parametros);
        }

        /// <summary>
        /// Salva uma custódia
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarCustodiaResponse SalvarCustodia(SalvarCustodiaRequest parametros)
        {
            // Repassa a mensagem
            return _servicoPersistencia.SalvarCustodia(parametros);
        }

        #endregion
    }
}
