using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

using Orbite.RV.Contratos.Integracao.BVMF;
using Orbite.RV.Contratos.Integracao.BVMF.Dados;
using Orbite.RV.Contratos.MarketData.Bovespa;
using Orbite.RV.Contratos.MarketData.Bovespa.Dados;
using Orbite.RV.Contratos.MarketData.Bovespa.Mensagens;

using Orbite.Comum;
using Orbite.RV.Sistemas.Integracao.BVMF;

namespace Orbite.RV.Sistemas.MarketData.Bovespa
{
    /// <summary>
    /// Implementação do serviço de market data bovespa
    /// </summary>
    public class ServicoMarketDataBovespa : IServicoMarketDataBovespa
    {
        #region Variaveis Locais

        /// <summary>
        /// Lista os arquivos do diretório apontado pelo config
        /// </summary>
        private List<ArquivoBVMFInfo> _diretorio = null;

        /// <summary>
        /// Mantem cache do arquivo PAP
        /// </summary>
        private DataSet _dsPAP = null;

        /// <summary>
        /// Mantem cache do arquivo PRO
        /// </summary>
        private DataSet _dsPRO = null;

        private Dictionary<string, DataSet> _tabelasHist = new Dictionary<string, DataSet>();
        private Dictionary<string, DataView> _viewHist = new Dictionary<string, DataView>();
        
        /// <summary>
        /// Dicionário para relacionar os tipos de séries do enumerador com os códigos bovespa.
        /// O valor1 é o código bovespa, e o valor2 é o enumerador
        /// </summary>
        private RelacaoValorValorHelper<string, SerieBovespaTipoEnum> _dicionarioSerieTipo = 
            new RelacaoValorValorHelper<string, SerieBovespaTipoEnum>();

        // Faz cache das referencias para os servicos abaixo
        private IServicoIntegracaoBVMFPersistencia _servicoBVMFPersistencia = null;
        private IServicoIntegracaoBVMFPersistenciaLayouts _servicoBVMFPersistenciaLayouts = null;
        private IServicoIntegracaoBVMFArquivos _servicoBVMFArquivos = null;
        private IServicoIntegracaoBVMF _servicoBVMFIntegracao = null;

        // Controle do histórico carregado
        private Dictionary<string, CotacaoBovespaInfo> _cotacoesAtuais = new Dictionary<string, CotacaoBovespaInfo>();
        private DateTime _dataHistoricoInicial = DateTime.MaxValue;
        private DateTime _dataHistoricoFinal = DateTime.MinValue;

        // Histórico de cotações
        // As chaves dos dicionários são: ativo, ano, mes e dataReferencia
        private Dictionary<string, Dictionary<int, Dictionary<int, Dictionary<DateTime, CotacaoBovespaInfo>>>> _cotacoesHistorico =
            new Dictionary<string, Dictionary<int, Dictionary<int, Dictionary<DateTime, CotacaoBovespaInfo>>>>();

        // Thread de inicialização
        private Thread _threadInicializacao = null;

        #endregion

        #region IServicoMarketDataBovespa Members

        /// <summary>
        /// Garante que as cotações do período estão carregadas
        /// </summary>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        private void carregarPeriodo(DateTime dataInicial, DateTime dataFinal)
        {
            // Lista com os periodos a carregar
            List<DateTime[]> periodosCarregar = new List<DateTime[]>();

            // Limites a carregar
            DateTime dataInicioCarregar = DateTime.MaxValue;
            DateTime dataFimCarregar = DateTime.MinValue;
            
            // Verifica se o periodo já está carregado
            if (dataInicial < _dataHistoricoInicial || dataFinal > _dataHistoricoFinal)
            {
                // Acha as datas a serem carregadas
                dataInicioCarregar = dataInicial < _dataHistoricoInicial ? dataInicial : _dataHistoricoInicial;
                dataFimCarregar = dataFinal > _dataHistoricoFinal ? dataFinal : _dataHistoricoFinal;

                // Precisa carregar, verifica se de uma vez só ou em duas partes
                if (_dataHistoricoInicial != DateTime.MaxValue && 
                    _dataHistoricoFinal != DateTime.MinValue && 
                    _dataHistoricoInicial > dataInicioCarregar && 
                    _dataHistoricoFinal < dataFimCarregar)
                {
                    // Faz duas cargas
                    periodosCarregar.Add(new DateTime[] { dataInicioCarregar, _dataHistoricoInicial });
                    periodosCarregar.Add(new DateTime[] { _dataHistoricoFinal, dataFimCarregar });

                }
                else
                {
                    // Faz apenas uma carga
                    periodosCarregar.Add(new DateTime[] { dataInicioCarregar, dataFimCarregar });
                }
            }

            // Varre os periodos que precisam ser carregados
            foreach (DateTime[] periodoCarregar in periodosCarregar)
            {
                // Verifica o tipo de carga que deverá ser feito
                TimeSpan periodo = periodoCarregar[1] - periodoCarregar[0];
                if (periodoCarregar[1].Year - periodoCarregar[0].Year != 0)
                {
                    // Carrega por anos
                    for (int ano = periodoCarregar[0].Year; ano <= periodoCarregar[1].Year; ano++)
                    {
                        // Monta o nome do arquivo
                        DateTime dataReferencia = new DateTime(ano, 1, 1);
                        string nomeArquivo = _servicoBVMFIntegracao.ReceberNomeArquivoBovespa(PeriodoEnum.Ano, dataReferencia);

                        // Pede a carga do arquivo
                        carregarArquivo(nomeArquivo, PeriodoEnum.Ano, dataReferencia);

                        // Sinaliza
                        _dataHistoricoInicial = _dataHistoricoInicial < dataReferencia ? _dataHistoricoInicial : dataReferencia;
                        _dataHistoricoFinal = _dataHistoricoFinal > dataReferencia.AddYears(1).AddDays(-1) ? _dataHistoricoFinal : dataReferencia.AddYears(1).AddDays(-1);
                    }
                }
                else if (periodoCarregar[1].Month - periodoCarregar[0].Month != 0)
                {
                    // Carrega por meses
                    for (int mes = periodoCarregar[0].Month; mes <= periodoCarregar[1].Month; mes++)
                    {
                        // Monta o nome do arquivo
                        DateTime dataReferencia = new DateTime(periodoCarregar[0].Year, mes, 1);
                        string nomeArquivo = _servicoBVMFIntegracao.ReceberNomeArquivoBovespa(PeriodoEnum.Mes, dataReferencia);

                        // Pede a carga do arquivo
                        carregarArquivo(nomeArquivo, PeriodoEnum.Mes, dataReferencia);

                        // Sinaliza
                        _dataHistoricoInicial = _dataHistoricoInicial < dataReferencia ? _dataHistoricoInicial : dataReferencia;
                        _dataHistoricoFinal = _dataHistoricoFinal > dataReferencia.AddMonths(1).AddDays(-1) ? _dataHistoricoFinal : dataReferencia.AddMonths(1).AddDays(-1);
                    }
                }
                else
                {
                    // Verifica se tem arquivo mensal e tenta carregar
                    ArquivoBVMFInfo arquivo = null;
                    string nomeArquivo = null;
                    DateTime dataReferencia = new DateTime(periodoCarregar[0].Year, periodoCarregar[0].Month, 1);
                    nomeArquivo =
                        _servicoBVMFIntegracao.ReceberNomeArquivoBovespa(
                            PeriodoEnum.Mes, dataReferencia);
                    arquivo =
                        (from a in _diretorio
                         where a.NomeArquivo.StartsWith(nomeArquivo)
                         orderby a.DataMovimento descending
                         select a).FirstOrDefault();
                    if (arquivo != null)
                    {
                        // Carrega arquivo mensal
                        carregarArquivo(nomeArquivo, PeriodoEnum.Mes, dataReferencia);
                    } 
                    else
                    {
                        // Verifica se tem arquivo anual e tenta carregar
                        dataReferencia = new DateTime(periodoCarregar[0].Year, 1, 1);
                        nomeArquivo = 
                            _servicoBVMFIntegracao.ReceberNomeArquivoBovespa(
                                PeriodoEnum.Ano, dataReferencia);
                        arquivo =
                            (from a in _diretorio
                             where a.NomeArquivo.StartsWith(nomeArquivo)
                             orderby a.DataMovimento descending
                             select a).FirstOrDefault();
                        if (arquivo != null)
                        {
                            // Carrega arquivo anual
                            carregarArquivo(nomeArquivo, PeriodoEnum.Ano, dataReferencia);
                        }
                        else
                        {
                            // Carrega por dias
                            for (int dia = periodoCarregar[0].Day; dia <= periodoCarregar[1].Day; dia++)
                            {
                                // Monta o nome do arquivo
                                DateTime dataReferencia2 = new DateTime(periodoCarregar[0].Year, periodoCarregar[0].Day, 1);
                                string nomeArquivo2 = _servicoBVMFIntegracao.ReceberNomeArquivoBovespa(PeriodoEnum.Dia, dataReferencia2);

                                // Pede a carga do arquivo
                                carregarArquivo(nomeArquivo2, PeriodoEnum.Dia, dataReferencia2);

                                // Sinaliza
                                _dataHistoricoInicial = _dataHistoricoInicial < dataReferencia ? _dataHistoricoInicial : dataReferencia;
                                _dataHistoricoFinal = _dataHistoricoFinal > dataReferencia ? _dataHistoricoFinal : dataReferencia;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Faz a carga do arquivo de cotações Bovespa informado
        /// </summary>
        /// <param name="nomeArquivo"></param>
        /// <param name="periodo"></param>
        /// <param name="data"></param>
        private void carregarArquivo(string nomeArquivo, PeriodoEnum periodo, DateTime data)
        {
            // Cria informacoes para se localizar o arquivo
            ArquivoBVMFInfo arquivoInfo =
                new ArquivoBVMFInfo()
                {
                    NomeArquivo = nomeArquivo
                };

            // Pede arquivo para persistencia
            Stream stream = _servicoBVMFPersistencia.ReceberArquivo(arquivoInfo);

            // Se nao veio o arquivo, faz o download e salva na persistencia
            if (stream == null)
            {
                // Cria o stream
                MemoryStream ms = new MemoryStream();

                // Faz o download
                _servicoBVMFIntegracao.ReceberArquivoMarketDataBovespa(periodo, data, ms);

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

            // Faz a carga das cotações
            foreach (DataRow dr in ds.Tables["DETALHE"].Rows)
            {
                // Monta o objeto de cotação
                string ativo = (string)dr["CODNEG"];
                CotacaoBovespaInfo cotacao =
                    new CotacaoBovespaInfo()
                    {
                        //Ativo = (string)dr["CODNEG"],
                        Abertura = (double)dr["PREABE"],
                        Fechamento = (double)dr["PREULT"],
                        Maximo = (double)dr["PREMIN"],
                        Minimo = (double)dr["PREMAX"],
                        Quantidade = Convert.ToDouble(dr["TOTNEG"]),
                        Volume = (double)dr["VOLTOT"],
                        Data = (DateTime)dr["DATPRE"]
                    };

                // Adiciona ou recupera item do ativo
                Dictionary<int, Dictionary<int, Dictionary<DateTime, CotacaoBovespaInfo>>> dicionarioCotacoes = null;
                if (!_cotacoesHistorico.ContainsKey(ativo))
                    _cotacoesHistorico.Add(ativo, new Dictionary<int, Dictionary<int, Dictionary<DateTime, CotacaoBovespaInfo>>>());
                dicionarioCotacoes = _cotacoesHistorico[ativo];

                // Adiciona ou recupera item da cotação
                if (!dicionarioCotacoes.ContainsKey(cotacao.Data.Year))
                    dicionarioCotacoes.Add(cotacao.Data.Year, new Dictionary<int, Dictionary<DateTime, CotacaoBovespaInfo>>());
                if (!dicionarioCotacoes[cotacao.Data.Year].ContainsKey(cotacao.Data.Month))
                    dicionarioCotacoes[cotacao.Data.Year].Add(cotacao.Data.Month, new Dictionary<DateTime, CotacaoBovespaInfo>());
                if (!dicionarioCotacoes[cotacao.Data.Year][cotacao.Data.Month].ContainsKey(cotacao.Data))
                    dicionarioCotacoes[cotacao.Data.Year][cotacao.Data.Month].Add(cotacao.Data, cotacao);

                // Verifica se a lista de última cotação está com valores atualizados
                if (!_cotacoesAtuais.ContainsKey(ativo))
                    _cotacoesAtuais.Add(ativo, cotacao);
                else if (_cotacoesAtuais[ativo].Data < cotacao.Data)
                    _cotacoesAtuais[ativo] = cotacao;
            }

            // Libera o stream
            stream.Dispose();
        }

        /// <summary>
        /// Solicita histórico de cotações de um instrumento bovespa
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberHistoricoCotacaoBovespaResponse ReceberHistoricoCotacaoBovespa(ReceberHistoricoCotacaoBovespaRequest parametros)
        {
            // Garante a inicialização
            garantirInicializacao();

            // Prepara resposta
            ReceberHistoricoCotacaoBovespaResponse resposta =
                new ReceberHistoricoCotacaoBovespaResponse()
                {
                    DataFinal = parametros.DataFinal.Value,
                    DataInicial = parametros.DataInicial.Value,
                    Periodo = PeriodoEnum.Dia
                };

            // Garante cotações no periodo solicitado
            carregarPeriodo(parametros.DataInicial.Value, parametros.DataFinal.Value);

            // Tem historico do ativo informado?
            if (_cotacoesHistorico.ContainsKey(parametros.Instrumento.CodigoNegociacao))
            {
                // Se nao foram informadas, ajusta as datas
                if (!parametros.DataFinal.HasValue)
                    parametros.DataFinal = DateTime.Now;
                if (!parametros.DataInicial.HasValue)
                    parametros.DataInicial = new DateTime(1980, 1, 1);

                // Pega referencia para o dicionario
                Dictionary<int, Dictionary<int, Dictionary<DateTime, CotacaoBovespaInfo>>> historicoAtivo = 
                    _cotacoesHistorico[parametros.Instrumento.CodigoNegociacao];

                // Preenche com as cotações
                for (DateTime mesReferencia = parametros.DataInicial.Value; mesReferencia <= parametros.DataFinal.Value; mesReferencia = mesReferencia.AddMonths(1))
                {
                    Dictionary<DateTime, CotacaoBovespaInfo> cotacoes = null;
                    if (historicoAtivo.ContainsKey(mesReferencia.Year) && historicoAtivo[mesReferencia.Year].ContainsKey(mesReferencia.Month))
                        cotacoes = historicoAtivo[mesReferencia.Year][mesReferencia.Month];
                    if (cotacoes != null)
                        foreach (KeyValuePair<DateTime, CotacaoBovespaInfo> cotacao in cotacoes)
                            if (cotacao.Value.Data >= parametros.DataInicial.Value && cotacao.Value.Data <= parametros.DataFinal.Value)
                                resposta.Resultado.Add(cotacao.Value);
                }
            }
            
            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicita lista de custos bovespa
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberCustosBovespaResponse ReceberCustosBovespa(ReceberCustosBovespaRequest parametros)
        {
            // Garante a inicialização
            garantirInicializacao();
            
            // Prepara resposta
            ReceberCustosBovespaResponse resposta =
                new ReceberCustosBovespaResponse();

            // Pega objeto de série de custos de bolsa
            SerieCustoBolsaInfo serieCustoInfo = null;
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();
            List<SerieCustoBolsaInfo> seriesCusto =
                servicoPersistencia.ConsultarObjetos<SerieCustoBolsaInfo>(
                    new ConsultarObjetosRequest<SerieCustoBolsaInfo>()).Resultado;
            if (seriesCusto.Count == 0)
            {
                // Inicializa série
                serieCustoInfo = new SerieCustoBolsaInfo();

                // Adiciona primeiro elemento
                serieCustoInfo.Itens.Add(
                    new CustoBovespaInfo()
                    {
                        Bolsa = "BOVESPA",
                        CustoEmolumentosAcao = 0.4,
                        CustoEmolumentosDayTradeAcao = 0.5,
                        CustoEmolumentosDayTradeOpcao = 0.6,
                        CustoEmoulmentosOpcao = 0.7,
                        DataReferencia = new DateTime(1990, 1, 1)
                    });

                // Salva
                servicoPersistencia.SalvarObjeto<SerieCustoBolsaInfo>(
                    new SalvarObjetoRequest<SerieCustoBolsaInfo>()
                    {
                        Objeto = serieCustoInfo
                    });
            }
            else
            {
                // Deve existir apenas um
                serieCustoInfo = seriesCusto[0];
            }

            // Faz o filtro
            List<CustoBovespaInfo> custos =
                (from c in serieCustoInfo.Itens
                 where c.DataReferencia <= parametros.DataFinal
                 orderby c.DataReferencia
                 select c).ToList();
            resposta.CustosBolsa = custos;

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita detalhe de um instrumento bovespa
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberDetalheInstrumentoBovespaResponse ReceberDetalheInstrumentoBovespa(ReceberDetalheInstrumentoBovespaRequest parametros)
        {
            // Garante a inicialização
            garantirInicializacao();

            // Prepara resposta
            ReceberDetalheInstrumentoBovespaResponse resposta =
                new ReceberDetalheInstrumentoBovespaResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // String de consulta
            string sql = "";
            if (parametros.Instrumento.CodigoNegociacao != null)
                sql += "CODIGO_NEGOCIACAO = '" + parametros.Instrumento.CodigoNegociacao + "' AND ";
            if (parametros.Instrumento.Empresa != null)
                sql += "EMPRESA_EMITENTE = '" + parametros.Instrumento.Empresa + "' AND ";
            if (parametros.Instrumento.DataInicioNegociacao.HasValue)
                sql += "DATA_INICIO_NEGOCIACAO >= '" + parametros.Instrumento.DataInicioNegociacao.Value.ToString("yyyy/MM/dd") + "' AND ";
            if (parametros.Instrumento.DataInicioNegociacao.HasValue)
                sql += "DATA_LIMITE_NEGOCIACAO < '" + parametros.Instrumento.DataFimNegociacao.Value.ToString("yyyy/MM/dd") + "' AND ";
            if (parametros.Instrumento.DataInicioNegociacao.HasValue)
                sql += "DISTRIBUICAO = '" + parametros.Instrumento.Especificacao + "' AND ";
            if (parametros.DataReferencia.HasValue)
            {
                sql += "DATA_INICIO_NEGOCIACAO <= '" + parametros.DataReferencia.Value.ToString("yyyy/MM/dd") + "' AND ";
                sql += "DATA_LIMITE_NEGOCIACAO >= '" + parametros.DataReferencia.Value.ToString("yyyy/MM/dd") + "' AND ";
            }

            // Ajusta a string
            if (sql != "")
                sql = sql.Substring(0, sql.Length - 4);

            // Seleciona as linhas e varre traduzindo
            DataRow[] drs = _dsPAP.Tables["DADOS_DOS_PAPEIS"].Select(sql);
            foreach (DataRow dr in drs)
                resposta.Instrumentos.Add(traduzirLinhaPAP(dr));

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita a lista de instrumentos bovespa
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarInstrumentosBovespaResponse ListarInstrumentosBovespa(ListarInstrumentosBovespaRequest parametros)
        {
            // Garante a inicialização
            garantirInicializacao();

            // Inicializa
            ListarInstrumentosBovespaResponse response = 
                new ListarInstrumentosBovespaResponse();
            DataRow[] drInstrumentos = null;
            MarketDataBovespaParametrosInfo config = GerenciadorConfig.ReceberConfig<MarketDataBovespaParametrosInfo>();

            // Monta string com o filtro
            StringBuilder sql = new StringBuilder();

            // Filtro de apenas habilitados?
            if (parametros.TipoLista == ListarInstrumentosBovespaTipoListaEnum.ApenasHabilitados)
                sql.Append("DATA_LIMITE_NEGOCIACAO = '31/12/4000' and ");

            // Filtro por características de papel?
            if (parametros.Instrumento != null)
            {
                if (parametros.Instrumento.CodigoNegociacao != null)
                    sql.Append("CODIGO_NEGOCIACAO = '" + parametros.Instrumento.CodigoNegociacao + "' and ");
                if (parametros.Instrumento.DataInicioNegociacao.HasValue)
                    sql.Append("DATA_INICIO_NEGOCIACAO >= '" + parametros.Instrumento.DataInicioNegociacao.Value.ToString("yyyy/MM/dd") + "' and ");
                if (parametros.Instrumento.DataFimNegociacao.HasValue)
                    sql.Append("DATA_LIMITE_NEGOCIACAO <= '" + parametros.Instrumento.DataFimNegociacao.Value.ToString("yyyy/MM/dd") + "' and ");
            }

            // Ajusta a string
            if (sql.Length > 0)
                sql.Remove(sql.Length - 4, 4);

            // Faz a consulta de acordo com o tipo da lista
            if (sql.Length > 0)
                drInstrumentos = _dsPAP.Tables["DADOS_DOS_PAPEIS"].Select(sql.ToString());
            else
                drInstrumentos = _dsPAP.Tables["DADOS_DOS_PAPEIS"].Select();

            // Varre os registros criando as informacoes do instrumento
            foreach (DataRow dr in drInstrumentos)
                response.Instrumentos.Add(
                    new InstrumentoBovespaInfo()
                    {
                        CodigoNegociacao = (string)dr["CODIGO_NEGOCIACAO"],
                        Especificacao = dr["DISTRIBUICAO"].ToString(),
                        DataInicioNegociacao = (DateTime)dr["DATA_INICIO_NEGOCIACAO"],
                        DataFimNegociacao = (DateTime)dr["DATA_LIMITE_NEGOCIACAO"],
                        Status = (DateTime)dr["DATA_LIMITE_NEGOCIACAO"] > DateTime.Now ? InstrumentoBovespaStatusEnum.Habilitado : InstrumentoBovespaStatusEnum.Desabilitado,
                        Tipo = traduzirInstrumentoTipo(dr["TIPO_MERCADO"].ToString())
                    });

            // Retorna
            return response;
        }

        /// <summary>
        /// Solicita uma série bovespa. Pode ser série de desdobramentos, dividendos, juros, etc.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberSerieBovespaResponse ReceberSerieBovespa(ReceberSerieBovespaRequest parametros)
        {
            // Garante a inicialização
            garantirInicializacao();

            // Prepara o retorno
            ReceberSerieBovespaResponse resposta =
                new ReceberSerieBovespaResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Cria lista com os tipos de eventos a serem retornados
            // Traduz o tipo do evento para usar no filtro
            string tipoEventoStr = "";
            foreach (SerieBovespaTipoEnum tipoEvento in parametros.TiposEventos)
                tipoEventoStr += "'" + _dicionarioSerieTipo.DicionarioDe2Para1[tipoEvento].Valor1 + "',";
            if (tipoEventoStr != "")
                tipoEventoStr = tipoEventoStr.Substring(0, tipoEventoStr.Length - 1);

            // Monta o sql do filtro
            StringBuilder sql = new StringBuilder("CODIGO_NEGOCIACAO = '" + parametros.Instrumento.CodigoNegociacao + "' and ");
            if (tipoEventoStr != "") sql.Append("DESCRICAO_TIPO_PROVENTO_EVENTO IN (" + tipoEventoStr + ") and ");
            if (parametros.DataInicial.HasValue) sql.Append("DATA_APROVACAO_PROVENTO >= '" + parametros.DataInicial.Value.ToString("yyyy/MM/dd") + "' and ");
            if (parametros.DataFinal.HasValue) sql.Append("DATA_APROVACAO_PROVENTO < '" + parametros.DataFinal.Value.ToString("yyyy/MM/dd") + "' and ");
            sql.Remove(sql.Length - 4, 4);

            // Faz o filtro dos elementos
            DataRow[] drs = _dsPRO.Tables["DETALHE"].Select(sql.ToString());

            // Preenche a coleção
            foreach (DataRow dr in drs)
                resposta.Resultado.Add(
                    new EventoBovespaInfo()
                    {
                        Data = (DateTime)dr["DATA_APROVACAO_PROVENTO"],
                        Valor = (double)dr["VALOR_PROVENTO"],
                        TipoEvento =
                            _dicionarioSerieTipo.DicionarioDe1Para2.ContainsKey((string)dr["DESCRICAO_TIPO_PROVENTO_EVENTO"]) ? 
                            _dicionarioSerieTipo.DicionarioDe1Para2[(string)dr["DESCRICAO_TIPO_PROVENTO_EVENTO"]].Valor2 :
                            SerieBovespaTipoEnum.Desconhecido
                    });

            // Retorna a lista
            return resposta;
        }

        /// <summary>
        /// Solicita ultima cotação de ativos
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberUltimaCotacaoBovespaResponse ReceberUltimaCotacaoBovespa(ReceberUltimaCotacaoBovespaRequest parametros)
        {
            // Garante a inicialização
            garantirInicializacao();

            // Prepara resposta
            ReceberUltimaCotacaoBovespaResponse resposta =
                new ReceberUltimaCotacaoBovespaResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Se tem ativo, coloca na lista
            if (parametros.Ativo != null)
                parametros.Ativos.Add(parametros.Ativo);

            // Pega a lista de cotações desejada
            foreach (string ativo in parametros.Ativos)
            {
                if (_cotacoesAtuais.ContainsKey(ativo))
                {
                    CotacaoBovespaInfo cotacao = (CotacaoBovespaInfo)_cotacoesAtuais[ativo].Clone();
                    cotacao.Ativo = ativo;
                    resposta.Cotacoes.Add(cotacao);
                }
            }

            // Retorna
            return resposta;
        }

        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            // Verifica se deve fazer a inicialização de forma sincrona ou assincrona
            if (GerenciadorConfig.ReceberConfig<ServicoMarketDataBovespaConfig>().InicializarAssincrono)
            {
                // Pede inicialização assincrona
                ThreadStart threadStart = new ThreadStart(inicializar);
                _threadInicializacao = new Thread(threadStart);
                _threadInicializacao.Start();
            }
            else
            {
                // Pede a inicialização sincrona
                inicializar();
            }
        }

        public void PararServico()
        {
            // Verifica se a inicialização está em andamento
            if (_threadInicializacao != null && _threadInicializacao.ThreadState == ThreadState.Running)
                _threadInicializacao.Abort();
        }

        public Gradual.OMS.Library.Servicos.ServicoStatus ReceberStatusServico()
        {
            return Gradual.OMS.Library.Servicos.ServicoStatus.Indefinido;
        }

        #endregion

        #region Rotinas Locais

        /// <summary>
        /// Faz a tradução do tipo do instrumento de string (campo DESCRICAO_MERCADO do PAP) para o enumerador
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private InstrumentoBovespaTipoEnum traduzirInstrumentoTipo(string tipo)
        {
            // Inicializa
            InstrumentoBovespaTipoEnum retorno = InstrumentoBovespaTipoEnum.Desconhecido;

            // Descobre
            switch (tipo)
            {
                case "10":
                    retorno = InstrumentoBovespaTipoEnum.Acao;
                    break;
                case "30":
                    retorno = InstrumentoBovespaTipoEnum.Termo;
                    break;
                case "80":
                    retorno = InstrumentoBovespaTipoEnum.Opcao;
                    break;
                case "70":
                    retorno = InstrumentoBovespaTipoEnum.Opcao;
                    break;
                case "50":
                    retorno = InstrumentoBovespaTipoEnum.Futuro;
                    break;
                case "17":
                    retorno = InstrumentoBovespaTipoEnum.Leilao;
                    break;
            }

            // Retorna
            return retorno;
        }

        /// <summary>
        /// Faz a tradução de uma linha do PAP para um instrumento
        /// </summary>
        /// <param name="drPAP"></param>
        /// <returns></returns>
        private InstrumentoBovespaInfo traduzirLinhaPAP(DataRow drPAP)
        {
            // Inicializa
            InstrumentoBovespaInfo instrumentoInfo = null;

            // Pega o tipo do instrumento
            InstrumentoBovespaTipoEnum instrumentoTipo = traduzirInstrumentoTipo(drPAP["TIPO_MERCADO"].ToString());

            // Faz de acordo com o tipo
            switch (instrumentoTipo)
            {
                case InstrumentoBovespaTipoEnum.Acao:
                    instrumentoInfo =
                        new InstrumentoBovespaAcaoInfo()
                        {
                            CodigoNegociacao = drPAP["CODIGO_NEGOCIACAO"].ToString().Trim(),
                            Empresa = (string)drPAP["EMPRESA_EMITENTE"],
                            DataFimNegociacao = (DateTime)drPAP["DATA_LIMITE_NEGOCIACAO"],
                            DataInicioNegociacao = (DateTime)drPAP["DATA_INICIO_NEGOCIACAO"],
                            Especificacao = drPAP["DISTRIBUICAO"].ToString(),
                            Status = (DateTime)drPAP["DATA_LIMITE_NEGOCIACAO"] > DateTime.Now ? InstrumentoBovespaStatusEnum.Habilitado : InstrumentoBovespaStatusEnum.Desabilitado,
                            Tipo = InstrumentoBovespaTipoEnum.Acao
                        };
                    break;
                case InstrumentoBovespaTipoEnum.Opcao:
                    instrumentoInfo =
                        new InstrumentoBovespaOpcaoInfo()
                        {
                            CodigoNegociacao = drPAP["CODIGO_NEGOCIACAO"].ToString().Trim(),
                            DataFimNegociacao = (DateTime)drPAP["DATA_LIMITE_NEGOCIACAO"],
                            DataInicioNegociacao = (DateTime)drPAP["DATA_INICIO_NEGOCIACAO"],
                            Empresa = (string)drPAP["EMPRESA_EMITENTE"],
                            Especificacao = drPAP["DISTRIBUICAO"].ToString(),
                            Status = (DateTime)drPAP["DATA_LIMITE_NEGOCIACAO"] > DateTime.Now ? InstrumentoBovespaStatusEnum.Habilitado : InstrumentoBovespaStatusEnum.Desabilitado,
                            Tipo = InstrumentoBovespaTipoEnum.Opcao,
                            DataVencimento = (DateTime)drPAP["DATA_VENCIMENTO"],
                            InstrumentoBase =
                                new InstrumentoBovespaInfo()
                                {
                                    CodigoNegociacao = (string)drPAP["CODIGO_PAPEL_ABERTO"]
                                },
                            PrecoExercicio = Convert.ToDouble(drPAP["PRECO_EXERCICIO_OU_VALOR_CONTRATO"]),
                            TipoOpcao = drPAP["TIPO_MERCADO"].ToString() == "70" ? InstrumentoBovespaOpcaoTipoEnum.Call : InstrumentoBovespaOpcaoTipoEnum.Put
                        };
                    break;
            }

            // Retorna
            return instrumentoInfo;
        }

        /// <summary>
        /// Rotina de inicialização do serviço
        /// </summary>
        private void inicializar()
        {
            // Pega referencia para os serviços que serão utilizados
            _servicoBVMFPersistencia = Ativador.Get<IServicoIntegracaoBVMFPersistencia>();
            _servicoBVMFPersistenciaLayouts = Ativador.Get<IServicoIntegracaoBVMFPersistenciaLayouts>();
            _servicoBVMFArquivos = Ativador.Get<IServicoIntegracaoBVMFArquivos>();
            _servicoBVMFIntegracao = Ativador.Get<IServicoIntegracaoBVMF>();

            // Pega configuracoes
            ServicoMarketDataBovespaConfig config = GerenciadorConfig.ReceberConfig<ServicoMarketDataBovespaConfig>();

            // Garante a existencia do objeto de configurações de runtime
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();
            List<MarketDataBovespaParametrosInfo> canalInfoLista =
                servicoPersistencia.ConsultarObjetos<MarketDataBovespaParametrosInfo>(
                    new ConsultarObjetosRequest<MarketDataBovespaParametrosInfo>()).Resultado;
            if (canalInfoLista.Count == 0)
                servicoPersistencia.SalvarObjeto<MarketDataBovespaParametrosInfo>(
                    new SalvarObjetoRequest<MarketDataBovespaParametrosInfo>()
                    {
                        Objeto = new MarketDataBovespaParametrosInfo()
                    });

            // Pede o diretorio de arquivos
            IServicoIntegracaoBVMFArquivos servicoIntegracaoBVMFArquivos =
                Ativador.Get<IServicoIntegracaoBVMFArquivos>();
            _diretorio = servicoIntegracaoBVMFArquivos.ListarDiretorio(config.DiretorioArquivosBovespa);

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
            _dicionarioSerieTipo.Adicionar("DESDOBRAMENTO", SerieBovespaTipoEnum.Desdobramento);
            _dicionarioSerieTipo.Adicionar("DIVIDENDO", SerieBovespaTipoEnum.Dividendo);
            _dicionarioSerieTipo.Adicionar("GRUPAMENTO", SerieBovespaTipoEnum.Grupamento);
            _dicionarioSerieTipo.Adicionar("JRS CAP PROPRIO", SerieBovespaTipoEnum.JurosCapitalProprio);
            _dicionarioSerieTipo.Adicionar("RENDIMENTO", SerieBovespaTipoEnum.Rendimento);
            _dicionarioSerieTipo.Adicionar("SUBSCRICAO", SerieBovespaTipoEnum.Subscricao);

            // Carrega os arquivos iniciais solicitados
            if (config.MesesIniciaisACarregar > 0)
                carregarPeriodo(
                    DateTime.Now.Date.AddMonths(-config.MesesIniciaisACarregar), 
                    DateTime.Now.Date);
        }

        /// <summary>
        /// Garante que a inicialização já ocorreu ou espera terminar
        /// </summary>
        private void garantirInicializacao()
        {
            // Se tem a thread em execução, espera finalizar
            if (_threadInicializacao != null && _threadInicializacao.ThreadState == ThreadState.Running)
                _threadInicializacao.Join();
        }

        #endregion
    }
}
