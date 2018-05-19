using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Orbite.Comum;
using Orbite.RV.Contratos.Integracao.BVMF;
using Orbite.RV.Contratos.Integracao.BVMF.Dados;
using Orbite.RV.Contratos.MarketData;
using Orbite.RV.Contratos.MarketData.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;
using Orbite.RV.Sistemas.Integracao.BVMF;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Orbite.RV.Sistemas.MarketData.Bolsa
{
    /// <summary>
    /// É um proxy entre CanalMarketDataBase e a integração BVMF.
    /// Faz a leitura dos arquivos e responde a lista de instrumentos, de séries, 
    /// detalhe do instrumento, detalhe da série.
    /// </summary>
    public class CanalMarketDataBovespa : CanalMarketDataBase
    {
        private DateTime constHistoricoBovespaMenorData = new DateTime(1990, 01, 01);
        
        private List<ArquivoBVMFInfo> _diretorio = null;
        private DataSet _dsPAP = null;
        private DataSet _dsPRO = null;
        private Dictionary<string, DataSet> _tabelasHist = new Dictionary<string, DataSet>();
        private Dictionary<string, DataView> _viewHist = new Dictionary<string, DataView>();
        private ElementoExternoColecaoHelper<SerieTipoEnum> _dicionarioSerieTipo = new ElementoExternoColecaoHelper<SerieTipoEnum>();

        // Faz cache das referencias para os servicos abaixo
        private IServicoIntegracaoBVMFPersistencia _servicoBVMFPersistencia = Ativador.Get<IServicoIntegracaoBVMFPersistencia>();
        private IServicoIntegracaoBVMFPersistenciaLayouts _servicoBVMFPersistenciaLayouts = Ativador.Get<IServicoIntegracaoBVMFPersistenciaLayouts>();
        private IServicoIntegracaoBVMFArquivos _servicoBVMFArquivos = Ativador.Get<IServicoIntegracaoBVMFArquivos>();
        private IServicoIntegracaoBVMF _servicoBVMFIntegracao = Ativador.Get<IServicoIntegracaoBVMF>();

        public CanalMarketDataBovespa()
        {
            inicializar();
        }

        private void inicializar()
        {
            // Pega configuracoes
            CanalMarketDataBovespaConfig config = GerenciadorConfig.ReceberConfig<CanalMarketDataBovespaConfig>();

            // Pede o diretorio de arquivos
            IServicoIntegracaoBVMFArquivos servicoIntegracaoBVMFArquivos =
                Ativador.Get<IServicoIntegracaoBVMFArquivos>();
            _diretorio = servicoIntegracaoBVMFArquivos.ListarDiretorio(config.DiretorioArquivos);

            // Acha e carrega PAP
            ArquivoBVMFInfo arquivoPAP = (from d in _diretorio
                                      where d.Layout.Nome == "PAPH"
                                      orderby d.DataMovimento descending
                                      select d).FirstOrDefault();
            if (arquivoPAP != null)
                _dsPAP = servicoIntegracaoBVMFArquivos.InterpretarArquivo(
                    arquivoPAP.Layout,
                    arquivoPAP.NomeArquivo);

            // Acha e carrega PRO
            ArquivoBVMFInfo arquivoPRO = (from d in _diretorio
                                      where d.Layout.Nome == "PROH"
                                      orderby d.DataMovimento descending
                                      select d).FirstOrDefault();
            if (arquivoPRO != null)
                _dsPRO = servicoIntegracaoBVMFArquivos.InterpretarArquivo(
                    arquivoPRO.Layout,
                    arquivoPRO.NomeArquivo);

            // Cria o dicionario de SerieTipo do arquivo PRO
            _dicionarioSerieTipo.Adicionar("DESDOBRAMENTO", SerieTipoEnum.Desdobramento);
            _dicionarioSerieTipo.Adicionar("DIVIDENDO", SerieTipoEnum.Dividendo);
            _dicionarioSerieTipo.Adicionar("GRUPAMENTO", SerieTipoEnum.Grupamento);
            _dicionarioSerieTipo.Adicionar("JRS CAP PROPRIO", SerieTipoEnum.JurosCapitalProprio);
            _dicionarioSerieTipo.Adicionar("RENDIMENTO", SerieTipoEnum.Rendimento);
        }
        
        protected override ReceberListaInstrumentosResponse OnReceberListaInstrumentos(ReceberListaInstrumentosRequest parametros)
        {
            // Inicializa
            ReceberListaInstrumentosResponse response = new ReceberListaInstrumentosResponse();
            DataRow[] drInstrumentos = null;
            CanalMarketDataBovespaConfig config = GerenciadorConfig.ReceberConfig<CanalMarketDataBovespaConfig>();

            // Faz a consulta de acordo com o tipo da lista
            switch (parametros.TipoLista)
            {
                case ReceberListaInstrumentoTipoListaEnum.Padrao:
                case ReceberListaInstrumentoTipoListaEnum.ApenasHabilitados:
                    drInstrumentos = _dsPAP.Tables["DADOS_DOS_PAPEIS"].Select("DATA_LIMITE_NEGOCIACAO = '31/12/2040'");
                    break;
                case ReceberListaInstrumentoTipoListaEnum.HistoricoCompleto:
                    drInstrumentos = _dsPAP.Tables["DADOS_DOS_PAPEIS"].Select();
                    break;
            }

            // Lista que contem os papeis sem detalhes já informados como instrumento
            Dictionary<string, InstrumentoInfo> papeisSemDetalhe = new Dictionary<string, InstrumentoInfo>();
            
            // Varre os registros criando as informacoes do instrumento
            foreach (DataRow dr in drInstrumentos)
            {
                // Insere o papel sem detalhe apenas uma vez
                if (!papeisSemDetalhe.ContainsKey((string)dr["CODIGO_NEGOCIACAO"]))
                {
                    // Cria o papel sem detalhe
                    InstrumentoInfo papelSemDetalhe = 
                        new InstrumentoInfo()
                        {
                            CodigoNegociacao = (string)dr["CODIGO_NEGOCIACAO"],
                            Origem = config.Origem,
                            Tipo = traduzirInstrumentoTipo((string)dr["DESCRICAO_MERCADO"])
                        };
                    
                    // Adiciona na resposta e na lista de papeis sem detalhe
                    response.Instrumentos.Add(papelSemDetalhe);
                    papeisSemDetalhe.Add(papelSemDetalhe.CodigoNegociacao, papelSemDetalhe);
                }
                
                // Cria o papel
                InstrumentoInfo papel =
                    new InstrumentoInfo()
                    {
                        CodigoNegociacao = (string)dr["CODIGO_NEGOCIACAO"],
                        Detalhe = dr["DISTRIBUICAO"].ToString(),
                        DataInicioNegociacao = (DateTime)dr["DATA_INICIO_NEGOCIACAO"],
                        DataFimNegociacao = (DateTime)dr["DATA_LIMITE_NEGOCIACAO"],
                        Origem = config.Origem,
                        Habilitado = (DateTime)dr["DATA_LIMITE_NEGOCIACAO"] > DateTime.Now,
                        Tipo = traduzirInstrumentoTipo((string)dr["DESCRICAO_MERCADO"])
                    };
                
                // Adiciona na resposta
                response.Instrumentos.Add(papel);

                // Atualiza informacoes do papel sem detalhe
                InstrumentoInfo papelSemDetalhe2 = papeisSemDetalhe[papel.CodigoNegociacao];
                if (papel.DataInicioNegociacao < papelSemDetalhe2.DataInicioNegociacao) papelSemDetalhe2.DataInicioNegociacao = papel.DataInicioNegociacao;
                if (papel.DataFimNegociacao > papelSemDetalhe2.DataFimNegociacao) papelSemDetalhe2.DataFimNegociacao = papel.DataFimNegociacao;
                papelSemDetalhe2.Habilitado = papelSemDetalhe2.DataFimNegociacao > DateTime.Now;
            }
            
            // Retorna
            return response;
        }

        private InstrumentoTipoEnum traduzirInstrumentoTipo(string tipo)
        {
            // Inicializa
            InstrumentoTipoEnum retorno = InstrumentoTipoEnum.Desconhecido;

            // Descobre
            switch(tipo)
            {
                case "VISTA":
                    retorno = InstrumentoTipoEnum.Acao;
                    break;
                case "TERMO":
                    retorno = InstrumentoTipoEnum.Termo;
                    break;
                case "OPCAO VENDA":
                    retorno = InstrumentoTipoEnum.OpcaoVenda;
                    break;
                case "OPCAO COMPRA":
                    retorno = InstrumentoTipoEnum.OpcaoCompra;
                    break;
                case "FUTURO":
                    retorno = InstrumentoTipoEnum.Futuro;
                    break;
                case "LEILAO":
                    retorno = InstrumentoTipoEnum.Leilao;
                    break;
            }

            // Retorna
            return retorno;
        }

        protected override ReceberListaSeriesResponse OnReceberListaSeries(ReceberListaSeriesRequest parametros)
        {
            // Inicializa
            ReceberListaSeriesResponse response = new ReceberListaSeriesResponse();
            response.SeriesDescricao = new List<SerieDescricaoInfo>(50000);
            
            // Do cadastro de papeis, tudo que é acao e opcao tem historico de cotacoes
            // Varre a lista de instrumentos informando
            ReceberListaInstrumentosResponse listaInstrumentos =
                this.ReceberListaInstrumentos(
                    new ReceberListaInstrumentosRequest() 
                    { 
                        TipoLista = ReceberListaInstrumentoTipoListaEnum.HistoricoCompleto 
                    });
            var instrumentos = from i in listaInstrumentos.Instrumentos
                               where (i.Tipo == InstrumentoTipoEnum.Acao || 
                                      i.Tipo == InstrumentoTipoEnum.OpcaoCompra ||
                                      i.Tipo == InstrumentoTipoEnum.OpcaoVenda) &&
                                     i.Detalhe == null
                               select i;
            foreach (InstrumentoInfo instrumento in instrumentos)
                response.SeriesDescricao.Add(
                    new SerieDescricaoInfo() 
                    { 
                        Instrumento = instrumento,
                        TemHistorico = true,
                        TemOnLine = false,
                        Tipo = SerieTipoEnum.FechamentoDiario,
                        TipoDados = typeof(TickInfo),
                        TipoSerie = typeof(SerieElementosInfo<TickInfo>)
                    });

            // Lista de series adicionadas
            Dictionary<string, SerieDescricaoInfo> seriesAdicionadas = new Dictionary<string, SerieDescricaoInfo>(50000);
            
            // Cria indice de instrumentos
            Dictionary<string, InstrumentoInfo> indiceInstrumentos = new Dictionary<string, InstrumentoInfo>();
            foreach (InstrumentoInfo instrumentoInfo in listaInstrumentos.Instrumentos)
                if (!indiceInstrumentos.ContainsKey(instrumentoInfo.CodigoNegociacao + "-" + (instrumentoInfo.Detalhe != null ? instrumentoInfo.Detalhe : "")))
                    indiceInstrumentos.Add(instrumentoInfo.CodigoNegociacao + "-" + (instrumentoInfo.Detalhe != null ? instrumentoInfo.Detalhe : ""), instrumentoInfo);

            // Lista de series do cadastro de proventos e historicos
            foreach (DataRow dr in _dsPRO.Tables["DETALHE"].Rows)
            {
                // Traduz o tipo da serie
                SerieDescricaoInfo serieInfo = 
                    traduzirSerieTipo(
                        (string)dr["DESCRICAO_TIPO_PROVENTO_EVENTO"]);

                // Monta chave da serie
                string distribuicao = dr["DISTRIBUICAO_DESTINO"] != DBNull.Value ? dr["DISTRIBUICAO_DESTINO"].ToString() : null;
                string chave = serieInfo.Tipo.ToString() + "-" + serieInfo.EspecificacaoOutros + "-" + (string)dr["CODIGO_NEGOCIACAO"];
                if (distribuicao != null)
                    chave += "-" + distribuicao;

                // Verifica se já foi adicionado
                if (!seriesAdicionadas.ContainsKey(chave))
                {
                    // Acha o instrumento
                    InstrumentoInfo instrumento = null;
                    if (distribuicao != null && indiceInstrumentos.ContainsKey((string)dr["CODIGO_NEGOCIACAO"] + "-" + distribuicao))
                        instrumento = indiceInstrumentos[(string)dr["CODIGO_NEGOCIACAO"] + "-" + distribuicao];
                    else if (indiceInstrumentos.ContainsKey((string)dr["CODIGO_NEGOCIACAO"] + "-"))
                        instrumento = indiceInstrumentos[(string)dr["CODIGO_NEGOCIACAO"] + "-"];

                    // Se ainda não achou, ignora
                    if (instrumento == null)
                        continue;

                    // Adiciona a serie
                    serieInfo.Instrumento = instrumento;
                    serieInfo.TemHistorico = true;
                    serieInfo.TemOnLine = false;
                    seriesAdicionadas.Add(chave, serieInfo);
                    response.SeriesDescricao.Add(serieInfo);

                    // Caso o instrumento tenha detalhe...
                    if (instrumento.Detalhe != null)
                    {
                        // Referencia para a serie do instrumento sem detalhe
                        SerieDescricaoInfo serieSemDetalhe = null;

                        // Verifica se a serie do instrumento sem detalhe foi adicionada
                        string chaveSemSerie = serieInfo.Tipo.ToString() + "-" + serieInfo.EspecificacaoOutros + "-" + instrumento.CodigoNegociacao;
                        if (!seriesAdicionadas.ContainsKey(chaveSemSerie))
                        {
                            // Acha o instrumento sem detalhe
                            InstrumentoInfo instrumentoSemDetalhe = null;
                            if (indiceInstrumentos.ContainsKey((string)dr["CODIGO_NEGOCIACAO"] + "-"))
                                instrumentoSemDetalhe = indiceInstrumentos[(string)dr["CODIGO_NEGOCIACAO"] + "-"];

                            // Achou?
                            if (instrumentoSemDetalhe == null)
                                continue;

                            // Adiciona nova serie
                            serieSemDetalhe = new SerieDescricaoInfo()
                            {
                                Instrumento = instrumentoSemDetalhe,
                                TemHistorico = true,
                                TemOnLine = false,
                                Tipo = serieInfo.Tipo,
                                EspecificacaoOutros = serieInfo.EspecificacaoOutros,
                                TipoDados = serieInfo.TipoDados,
                                TipoSerie = serieInfo.TipoSerie
                            };
                            seriesAdicionadas.Add(chaveSemSerie, serieSemDetalhe);
                            response.SeriesDescricao.Add(serieSemDetalhe);
                        }

                        // Atualiza a serie do instrumento sem detalhe
                        
                    }
                }
            }

            // Retorna
            return response;
        }

        private SerieDescricaoInfo traduzirSerieTipo(string tipo)
        {
            // Inicializa 
            SerieDescricaoInfo retorno = new SerieDescricaoInfo();
            retorno.EspecificacaoOutros = tipo;

            // Tenta fazer a tradução
            if (_dicionarioSerieTipo.DicionarioChaveExterno.ContainsKey(tipo))
                retorno.Tipo = _dicionarioSerieTipo.DicionarioChaveExterno[tipo].ElementoInterno;

            // Retorna
            return retorno;
        }

        protected override ReceberSerieHistoricoResponse OnReceberSerieHistorico(ReceberSerieHistoricoRequest parametros)
        {
            // Inicializa
            ReceberSerieHistoricoResponse response = new ReceberSerieHistoricoResponse();

            // Faz a correção das datas
            if (!parametros.DataInicial.HasValue || parametros.DataInicial < constHistoricoBovespaMenorData)
                parametros.DataInicial = constHistoricoBovespaMenorData;
            if (!parametros.DataFinal.HasValue || parametros.DataFinal > DateTime.Now)
                parametros.DataFinal = DateTime.Now;
            
            // Processa de acordo com o tipo de série solicitada
            switch (parametros.Serie.Tipo)
            {
                case SerieTipoEnum.Desdobramento:
                    response.SerieElementos = serieHistoricoEventos(parametros);
                    break;
                case SerieTipoEnum.Dividendo:
                    response.SerieElementos = serieHistoricoEventos(parametros);
                    break;
                case SerieTipoEnum.FechamentoDiario:
                    response.SerieElementos = serieHistoricoFechamentoDiario(parametros);
                    break;
                case SerieTipoEnum.Grupamento:
                    response.SerieElementos = serieHistoricoEventos(parametros);
                    break;
                case SerieTipoEnum.JurosCapitalProprio:
                    response.SerieElementos = serieHistoricoEventos(parametros);
                    break;
                case SerieTipoEnum.Rendimento:
                    response.SerieElementos = serieHistoricoEventos(parametros);
                    break;
                case SerieTipoEnum.Outros:
                    break;
            }

            // Retorna
            return response;
        }

        private SerieElementosInfo<TickInfo> serieHistoricoFechamentoDiario(ReceberSerieHistoricoRequest parametros)
        {
            // Cria a coleção resultado
            SerieElementosInfo<TickInfo> resultado =
                new SerieElementosInfo<TickInfo>()
                {
                    Canal = this.Info.Nome,
                    DataFinal = parametros.DataFinal.Value,
                    DataInicial = parametros.DataInicial.Value,
                    OnLine = false,
                    Periodo = PeriodoEnum.Dia,
                    Serie = parametros.Serie
                };

            // Varre os arquivos criando o resultado final
            for (DateTime data = parametros.DataInicial.Value; data.Year <= parametros.DataFinal.Value.Year; data = data.AddYears(1))
            {
                // Monta o nome do arquivo
                string arquivo = _servicoBVMFIntegracao.ReceberNomeArquivoBovespa(PeriodoEnum.Ano, data);

                // Verifica se o arquivo está carregado
                if (!_tabelasHist.ContainsKey(arquivo))
                {
                    // Cria informacoes para se localizar o arquivo
                    ArquivoBVMFInfo arquivoInfo = 
                        new ArquivoBVMFInfo() 
                        {
                            NomeArquivo = arquivo 
                        };

                    // Pede arquivo para persistencia
                    Stream stream = _servicoBVMFPersistencia.ReceberArquivo(arquivoInfo);

                    // Se nao veio o arquivo, faz o download e salva na persistencia
                    if (stream == null)
                    {
                        // Cria o stream
                        MemoryStream ms = new MemoryStream();

                        // Faz o download
                        _servicoBVMFIntegracao.ReceberArquivoMarketDataBovespa(PeriodoEnum.Ano, data, ms);

                        // Salva na persistencia
                        _servicoBVMFPersistencia.PersistirArquivo(arquivoInfo, ms);

                        // Repassa
                        ms.Position = 0;
                        stream = ms;
                    }

                    // Pega o layout COTAHIST
                    LayoutBVMFInfo layoutCOTA = _servicoBVMFPersistenciaLayouts.ReceberLayout(typeof(ConversorLayoutBovespa), "COTAHIST");

                    // Faz o parse do arquivo
                    DataSet ds = _servicoBVMFArquivos.InterpretarArquivo(layoutCOTA, stream);
                    
                    // Adiciona na colecao
                    _tabelasHist.Add(arquivo, ds);
                    _viewHist.Add(arquivo, new DataView(ds.Tables["DETALHE"], null, "CODNEG", DataViewRowState.CurrentRows));
                }

                // Faz o filtro
                List<DataRowView> drss = new List<DataRowView>();
                IEnumerable<DataRowView> drs = 
                    from r in _viewHist[arquivo].FindRows(parametros.Serie.Instrumento.CodigoNegociacao)
                    where (DateTime)r["DATPRE"] >= parametros.DataInicial && (DateTime)r["DATPRE"] < parametros.DataFinal
                    select r;

                //DataRow[] drs = 
                //    _tabelasHist[arquivo].Tables["DETALHE"].Select(
                //        "CODNEG = '" + parametros.Serie.Instrumento.CodigoNegociacao + "' and " +
                //        "DATPRE >= '" + parametros.DataInicial.Value.ToString("yyyy/MM/dd") + "' and " +
                //        "DATPRE <= '" + parametros.DataFinal.Value.ToString("yyyy/MM/dd") + "'");

                // Preenche a colecao
                foreach (DataRowView dr in drs)
                    resultado.Elementos.Add(
                        new TickInfo() 
                        {
                            Abertura = (double)dr["PREABE"],
                            Fechamento = (double)dr["PREULT"],
                            Maximo = (double)dr["PREMIN"],
                            Minimo = (double)dr["PREMAX"],
                            Quantidade = Convert.ToDouble(dr["TOTNEG"]),
                            Volume = (double)dr["VOLTOT"],
                            Data = (DateTime)dr["DATPRE"]
                        });
            }

            // Retorna a lista
            return resultado;
        }

        private SerieElementosInfo<EventoBolsaInfo> serieHistoricoEventos(ReceberSerieHistoricoRequest parametros)
        {
            // Traduz o tipo do evento para usar no filtro
            string tipoEvento = _dicionarioSerieTipo.DicionarioChaveInterno[parametros.Serie.Tipo].ElementoExterno;

            // Faz o filtro dos elementos
            DataRow[] drs = 
                _dsPRO.Tables["DETALHE"].Select(
                    "CODIGO_NEGOCIACAO = '" + parametros.Serie.Instrumento.CodigoNegociacao + "' and " +
                    "DESCRICAO_TIPO_PROVENTO_EVENTO = '" + tipoEvento + "' and " +
                    "DATA_APROVACAO_PROVENTO >= '" + parametros.DataInicial.Value.ToString("yyyy/MM/dd") + "' and " +
                    "DATA_APROVACAO_PROVENTO <= '" + parametros.DataFinal.Value.ToString("yyyy/MM/dd") + "' ");

            // Prepara retorno
            SerieElementosInfo<EventoBolsaInfo> resultado = new SerieElementosInfo<EventoBolsaInfo>();

            // Preenche a coleção
            foreach (DataRow dr in drs)
                resultado.Elementos.Add(
                    new EventoBolsaInfo()
                    {
                        Data = (DateTime)dr["DATA_APROVACAO_PROVENTO"],
                        Valor = (double)dr["VALOR_PROVENTO"]
                    });

            // Retorna a lista
            return resultado;
        }
    }
}
