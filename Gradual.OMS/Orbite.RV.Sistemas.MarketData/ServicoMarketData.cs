using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Orbite.RV.Contratos.MarketData;
using Orbite.RV.Contratos.MarketData.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Orbite.RV.Sistemas.MarketData
{
    /// <summary>
    /// Implementação do serviço de market data
    /// </summary>
    public class ServicoMarketData : IServicoMarketData
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia à classe de configurações
        /// </summary>
        private ServicoMarketDataConfig _config = GerenciadorConfig.ReceberConfig<ServicoMarketDataConfig>();

        /// <summary>
        /// Lista de canais carregados
        /// </summary>
        public Dictionary<Type, CanalMarketDataBase> Canais { get; set; }

        /// <summary>
        /// Lista de séries disponibilizadas pelos canais
        /// </summary>
        public Dictionary<Type, SerieDescricaoInfo> Series { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoMarketData()
        {
            // Verifica se a inicialização é síncrona ou assíncrona
            if (_config.InicializarEmThreadSeparada)
            {
                // Faz a inicialização em uma thread separada
                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(
                        delegate(object parametros)
                        {
                            inicializar();
                        }));
            }
            else
            {
                // Inicialização sincrona
                inicializar();
            }
        }

        /// <summary>
        /// Rotina de inicialização do serviço
        /// </summary>
        private void inicializar()
        {
            // Inicializa
            this.Canais = new Dictionary<Type, CanalMarketDataBase>();
            this.Series = new Dictionary<Type, SerieDescricaoInfo>();

            // Faz a carga dos canais e já faz a inicialização pedindo a lista de instrumentos e de séries
            foreach (CanalInfo canalInfo in _config.Canais)
            {
                // Cria a instancia do canal
                CanalMarketDataBase canal =
                    (CanalMarketDataBase)
                        Activator.CreateInstance(canalInfo.TipoCanal);

                // Seta o info
                canal.Info = canalInfo;

                // Adiciona na colecao
                this.Canais.Add(canalInfo.TipoCanal, canal);

                // Pede lista de séries
                ReceberListaSeriesResponse listaSeries =
                    canal.ReceberListaSeries(
                        new ReceberListaSeriesRequest());

                // Adiciona a lista de series do canal na lista global
                foreach (SerieDescricaoInfo serieInfo in listaSeries.SeriesDescricao)
                {
                    // Adiciona informações do canal
                    serieInfo.CanalInfo = canalInfo;

                    // Adiciona série na coleção
                    this.Series.Add(serieInfo.TipoMensagemRequest, serieInfo);
                }
            }
        }

        #endregion

        #region IServicoMarketData Members

        /// <summary>
        /// Faz a assinatura de uma série online.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AssinarEventoMarketDataResponse AssinarEvento(AssinarEventoMarketDataRequest parametros)
        {
            return null;
        }

        /// <summary>
        /// Recebe a lista de eventos disponíveis, de acordo com os parametros informados
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberListaEventosResponse ReceberListaEventos(ReceberListaEventosRequest parametros)
        {
            return null;
        }

        /// <summary>
        /// Recebe o histórico de uma série, de acordo com os parametros informados.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberSerieItensResponse ReceberSerieItens(ReceberSerieItensRequest parametros)
        {
            // Repassa a mensagem
            return 
                this.Canais[this.Series[parametros.GetType()].CanalInfo.TipoCanal].ReceberSerieItens(parametros);
        }

        /// <summary>
        /// Consulta lista de séries disponíveis. Permite listar todos ou aplicar filtros.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberListaSeriesResponse ReceberListaSeries(ReceberListaSeriesRequest parametros)
        {
            // Monta lista de séries 
            ReceberListaSeriesResponse resposta = 
                new ReceberListaSeriesResponse() 
                { 
                    SeriesDescricao =
                        (from s in this.Series 
                         select s.Value).ToList()
                };

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Recebe a lista de canais registrados no market data
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberListaCanaisResponse ReceberListaCanais(ReceberListaCanaisRequest parametros)
        {
            // Monta lista de canais
            ReceberListaCanaisResponse resposta = 
                new ReceberListaCanaisResponse() 
                { 
                    Canais =
                        (from c in this.Canais 
                         select c.Value.Info).ToList()
                };

            // Retorna
            return resposta;
        }

        #endregion
    }
}
