using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;

using Orbite.RV.Contratos.MarketData;
using Orbite.RV.Contratos.MarketData.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Orbite.RV.Sistemas.MarketData
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoMarketData_Old : IServicoMarketData
    {
        /// <summary>
        /// Referencia à classe de configurações
        /// </summary>
        private ServicoMarketDataConfig _config = GerenciadorConfig.ReceberConfig<ServicoMarketDataConfig>();
        
        /// <summary>
        /// Lista de canais carregados
        /// </summary>
        public Dictionary<string, CanalMarketDataHelper> Canais { get; set; }

        /// <summary>
        /// Lista de séries disponibilizadas pelos canais
        /// </summary>
        public Dictionary<string, SerieDescricaoInfo> Series { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public ServicoMarketData_Old()
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
            this.Canais = new Dictionary<string, CanalMarketDataHelper>();
            this.Series = new Dictionary<string, SerieDescricaoInfo>();

            // Faz a carga dos canais e já faz a inicialização pedindo a lista de instrumentos e de séries
            foreach (CanalInfo canalInfo in _config.Canais)
            {
                // Cria a instancia do canal
                CanalMarketDataBase canal =
                    (CanalMarketDataBase)
                        Activator.CreateInstance(
                            Type.GetType(canalInfo.TipoCanal));

                // Seta o info
                canal.Info = canalInfo;

                // Pede lista de instrumentos
                ReceberListaInstrumentosResponse listaInstrumentos =
                    canal.ReceberListaInstrumentos(
                        new ReceberListaInstrumentosRequest()
                        {
                            TipoLista = ReceberListaInstrumentoTipoListaEnum.HistoricoCompleto
                        });

                // Pede lista de séries
                ReceberListaSeriesResponse listaSeries =
                    canal.ReceberListaSeries(
                        new ReceberListaSeriesRequest());

                // Adiciona na colecao
                this.Canais.Add(
                    canal.Info.Nome,
                    new CanalMarketDataHelper()
                    {
                        Canal = canal,
                        Instrumentos = listaInstrumentos.Instrumentos,
                        Series = listaSeries.SeriesDescricao
                    });

                // Adiciona a lista de series do canal na lista global
                somaSeries(listaSeries.SeriesDescricao, canalInfo);
            }
        }

        /// <summary>
        /// Adiciona a lista de séries informada na lista global do serviço.
        /// A lista do serviço age como uma espécie de diretório de séries.
        /// </summary>
        /// <param name="series"></param>
        /// <param name="canal"></param>
        private void somaSeries(List<SerieDescricaoInfo> series, CanalInfo canal)
        {
            // Varre as series
            foreach (SerieDescricaoInfo serieInfo in series)
            {
                // Verifica se a serie já existe
                string chaveSerie = retornarChaveSerie(
                                        serieInfo.Tipo, 
                                        serieInfo.Instrumento.CodigoNegociacao, 
                                        serieInfo.Instrumento.Origem, 
                                        serieInfo.Instrumento.Detalhe);
                SerieDescricaoInfo serieExistente = null;
                if (this.Series.ContainsKey(chaveSerie))
                    serieExistente = this.Series[chaveSerie];

                // Se não existe, adiciona
                if (serieExistente == null)
                {
                    this.Series.Add(chaveSerie, serieInfo);
                    serieExistente = serieInfo;
                }

                // Informa que este canal também fornece o sinal
                if (!serieExistente.Canais.ContainsKey(canal))
                    serieExistente.Canais.Add(canal, serieInfo);
            }
        }

        /// <summary>
        /// Função que retorna a chave da série.
        /// Overload que recebe a série como parâmetro.
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        private string retornarChaveSerie(SerieDescricaoInfo serie)
        {
            return retornarChaveSerie(serie.Tipo, serie.Instrumento.CodigoNegociacao, serie.Instrumento.Origem, serie.Instrumento.Detalhe);
        }

        /// <summary>
        /// Função que retorna a chave da série.
        /// O resultado é a concatenação de Tipo-CodigoNegociacao-InstrumentoOrigem-InstrumentoDetalhe.
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="instrumentoCodigoNegociacao"></param>
        /// <param name="instrumentoOrigem"></param>
        /// <param name="instrumentoDetalhe"></param>
        /// <returns></returns>
        private string retornarChaveSerie(SerieTipoEnum tipo, string instrumentoCodigoNegociacao, string instrumentoOrigem, string instrumentoDetalhe)
        {
            return tipo.ToString() + "-" + instrumentoCodigoNegociacao + "-" + instrumentoOrigem + "-" + (instrumentoDetalhe != null ? instrumentoDetalhe : "");
        }
        
        #region IServicoMarketData Members

        public AssinarBookResponse AssinarBook(AssinarBookRequest parametros)
        {
            return null;
        }

        public AssinarEventoResponse AssinarEvento(AssinarEventoRequest parametros)
        {
            return null;
        }

        public ReceberSerieItensResponse ReceberSerieItens(ReceberSerieItensRequest parametros)
        {
            // Decide para qual canal deve fazer o pedido
            // ** Por enquanto a origem é o nome do canal
            string nomeCanal = parametros.Serie.Instrumento.Origem;

            // Faz o pedido para o canal
            ReceberSerieItensResponse response = 
                this.Canais[nomeCanal].Canal.ReceberSerieItens(parametros);

            // Retorna
            return response;
        }

        public ReceberListaInstrumentosResponse ReceberListaInstrumentos(ReceberListaInstrumentosRequest parametros)
        {
            // Inicializa
            ReceberListaInstrumentosResponse retorno = new ReceberListaInstrumentosResponse();
            
            // Soma a lista de instrumentos de cada canal
            foreach (KeyValuePair<string, CanalMarketDataHelper> item in this.Canais)
            {
                CanalMarketDataHelper canal = item.Value;

                switch (parametros.TipoLista)
                {
                    case ReceberListaInstrumentoTipoListaEnum.Padrao:
                    case ReceberListaInstrumentoTipoListaEnum.HistoricoCompleto:
                        retorno.Instrumentos.AddRange(canal.Instrumentos);
                        break;
                    case ReceberListaInstrumentoTipoListaEnum.ApenasHabilitados:
                        retorno.Instrumentos.AddRange(
                            from i in canal.Instrumentos
                            where i.Habilitado == true
                            select i);
                        break;
                    case ReceberListaInstrumentoTipoListaEnum.NegociadosNaDataReferencia:
                        retorno.Instrumentos.AddRange(
                            (from i in canal.Instrumentos
                            where i.DataInicioNegociacao <= parametros.DataReferencia
                            orderby i.DataInicioNegociacao descending
                            group i by i.CodigoNegociacao into g
                            select g.First()).OrderBy(f => f.CodigoNegociacao));
                        break;
                }
            }

            // Aplica o restante dos filtros
            retorno.Instrumentos = new List<InstrumentoInfo>(
                from i in retorno.Instrumentos
                where (!parametros.InstrumentoTipo.HasValue || i.Tipo == parametros.InstrumentoTipo.Value)
                select i);

            // Retorna
            return retorno;
        }

        public ReceberListaSeriesResponse ReceberListaSeries(ReceberListaSeriesRequest parametros)
        {
            // Retorna
            return 
                new ReceberListaSeriesResponse()
                {
                    SeriesDescricao = this.Series.Values.ToList<SerieDescricaoInfo>()
                };
        }

        #endregion
    }
}
