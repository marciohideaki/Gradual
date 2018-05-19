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
    public class ServicoMarketData : IServicoMarketData
    {
        /// <summary>
        /// Lista de canais carregados
        /// </summary>
        public Dictionary<string, CanalMarketDataHelper> Canais { get; set; }

        /// <summary>
        /// Lista de séries disponibilizadas pelos canais
        /// </summary>
        public Dictionary<string, SerieInfo> Series { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public ServicoMarketData()
        {
            //ThreadPool.QueueUserWorkItem(
            //    new WaitCallback(
            //        delegate(object parametros) 
            //        {
                        inicializar();
                    //}));
        }

        private void inicializar()
        {
            // Inicializa
            this.Canais = new Dictionary<string, CanalMarketDataHelper>();
            this.Series = new Dictionary<string, SerieInfo>();

            // Pede config
            ServicoMarketDataConfig config = GerenciadorConfig.ReceberConfig<ServicoMarketDataConfig>();

            // Faz a carga dos canais e já faz a inicialização pedindo a lista de instrumentos e de séries
            foreach (CanalMarketDataInfo canalInfo in config.Canais)
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
                        Series = listaSeries.Series
                    });

                // Adiciona a lista de series do canal na lista global
                somaSeries(listaSeries.Series, canalInfo);
            }
        }

        private void somaSeries(List<SerieInfo> series, CanalMarketDataInfo canal)
        {
            // Varre as series
            foreach (SerieInfo serieInfo in series)
            {
                // Verifica se a serie já existe
                string chaveSerie = retornarChaveSerie(
                                        serieInfo.Tipo, 
                                        serieInfo.Instrumento.CodigoNegociacao, 
                                        serieInfo.Instrumento.Origem, 
                                        serieInfo.Instrumento.Detalhe);
                SerieInfo serieExistente = null;
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

        private string retornarChaveSerie(SerieInfo serie)
        {
            return retornarChaveSerie(serie.Tipo, serie.Instrumento.CodigoNegociacao, serie.Instrumento.Origem, serie.Instrumento.Detalhe);
        }

        private string retornarChaveSerie(SerieTipoEnum tipo, string instrumentoCodigoNegociacao, string instrumentoOrigem, string instrumentoDetalhe)
        {
            return tipo.ToString() + "-" + instrumentoCodigoNegociacao + "-" + instrumentoOrigem + "-" + (instrumentoDetalhe != null ? instrumentoDetalhe : "");
        }
        
        #region IServicoMarketData Members

        public AssinarBookResponse AssinarBook(AssinarBookRequest parametros)
        {
            return null;
        }

        public AssinarSerieResponse AssinarSerie(AssinarSerieRequest parametros)
        {
            return null;
        }

        public ReceberSerieHistoricoResponse ReceberSerieHistorico(ReceberSerieHistoricoRequest parametros)
        {
            // Decide para qual canal deve fazer o pedido
            // ** Por enquanto a origem é o nome do canal
            string nomeCanal = parametros.Serie.Instrumento.Origem;

            // Faz o pedido para o canal
            ReceberSerieHistoricoResponse response = 
                this.Canais[nomeCanal].Canal.ReceberSerieHistorico(parametros);

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
                    Series = this.Series.Values.ToList<SerieInfo>()
                };
        }

        #endregion
    }
}
