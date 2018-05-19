using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using Orbite.Comum;
using Orbite.RV.Contratos.MarketData;
using Orbite.RV.Contratos.MarketData.Bolsa.Dados;
using Orbite.RV.Contratos.MarketData.Bolsa.Mensagens;
using Orbite.RV.Contratos.MarketData.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;
using Orbite.RV.Contratos.Integracao.BVMF;
using Orbite.RV.Contratos.Integracao.BVMF.Dados;
using Orbite.RV.Sistemas.Integracao.BVMF;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Orbite.RV.Sistemas.MarketData.Bolsa
{
    /// <summary>
    /// Implementa o canal de market data de bolsa (bovespa e bmf)
    /// É um proxy entre CanalMarketDataBase e a integração BVMF.
    /// Faz a leitura dos arquivos e responde a lista de instrumentos, de séries, 
    /// detalhe do instrumento, detalhe da série.
    /// </summary>
    public class CanalMarketDataBolsa : CanalMarketDataBase
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
        private ElementoExternoColecaoHelper<SerieBolsaTipoEnum> _dicionarioSerieTipo = new ElementoExternoColecaoHelper<SerieBolsaTipoEnum>();

        // Faz cache das referencias para os servicos abaixo
        private IServicoIntegracaoBVMFPersistencia _servicoBVMFPersistencia = Ativador.Get<IServicoIntegracaoBVMFPersistencia>();
        private IServicoIntegracaoBVMFPersistenciaLayouts _servicoBVMFPersistenciaLayouts = Ativador.Get<IServicoIntegracaoBVMFPersistenciaLayouts>();
        private IServicoIntegracaoBVMFArquivos _servicoBVMFArquivos = Ativador.Get<IServicoIntegracaoBVMFArquivos>();
        private IServicoIntegracaoBVMF _servicoBVMFIntegracao = Ativador.Get<IServicoIntegracaoBVMF>();

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public CanalMarketDataBolsa()
        {
            inicializar();
        }

        private void inicializar()
        {
            // Pega configuracoes
            CanalMarketDataBolsaConfig config = GerenciadorConfig.ReceberConfig<CanalMarketDataBolsaConfig>();

            // Garante a existencia do objeto de configurações de runtime
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();
            List<CanalMarketDataBolsaInfo> canalInfoLista = 
                servicoPersistencia.ConsultarObjetos<CanalMarketDataBolsaInfo>(
                    new ConsultarObjetosRequest<CanalMarketDataBolsaInfo>()).Resultado;
            if (canalInfoLista.Count == 0)
                servicoPersistencia.SalvarObjeto<CanalMarketDataBolsaInfo>(
                    new SalvarObjetoRequest<CanalMarketDataBolsaInfo>() 
                    { 
                        Objeto = new CanalMarketDataBolsaInfo()
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
            _dicionarioSerieTipo.Adicionar("DESDOBRAMENTO", SerieBolsaTipoEnum.Desdobramento);
            _dicionarioSerieTipo.Adicionar("DIVIDENDO", SerieBolsaTipoEnum.Dividendo);
            _dicionarioSerieTipo.Adicionar("GRUPAMENTO", SerieBolsaTipoEnum.Grupamento);
            _dicionarioSerieTipo.Adicionar("JRS CAP PROPRIO", SerieBolsaTipoEnum.JurosCapitalProprio);
            _dicionarioSerieTipo.Adicionar("RENDIMENTO", SerieBolsaTipoEnum.Rendimento);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Retorna a lista de séries trabalhadas pelo canal
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected override ReceberListaSeriesResponse OnReceberListaSeries(ReceberListaSeriesRequest parametros)
        {
            // Prepara resposta
            ReceberListaSeriesResponse resposta = new ReceberListaSeriesResponse();

            // Varre os tipos marcados com o atributo de série
            foreach (Type tipo in typeof(ReceberSerieCotacaoRequest).Assembly.GetTypes())
            {
                // Tenta pegar o atributo
                object[] attrs = tipo.GetCustomAttributes(typeof(SerieMarketDataAttribute), true);
                if (attrs.Length > 0)
                {
                    // Pega o atributo
                    SerieMarketDataAttribute attr = (SerieMarketDataAttribute)attrs[0];

                    // Monta a descrição e adiciona na coleção
                    resposta.SeriesDescricao.Add(
                        new SerieDescricaoInfo()
                        {
                            CodigoSerie = attr.CodigoSerie,
                            NomeSerie = attr.NomeSerie,
                            DescricaoSerie = attr.DescricaoSerie,
                            TipoMensagemRequest = attr.TipoMensagemRequest != null ? attr.TipoMensagemRequest : tipo,
                            TipoMensagemResponse = attr.TipoMensagemResponse
                        });
                }
            }
            
            // Retorna
            return resposta;
        }

        /// <summary>
        /// Retorna os itens de uma série
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        protected override ReceberSerieItensResponse OnReceberSerieItens(ReceberSerieItensRequest parametros)
        {
            // Prepara retorno
            ReceberSerieItensResponse resposta = null;

            // Verifica o tipo da requisicao
            Type tipoMensagem = parametros.GetType();
            if (tipoMensagem == typeof(ReceberSerieCotacaoRequest))
                resposta = processarSerieCotacao((ReceberSerieCotacaoRequest)parametros);
            else if (tipoMensagem == typeof(ReceberSerieInstrumentosRequest))
                resposta = processarSerieInstrumentosBovespa((ReceberSerieInstrumentosRequest)parametros);
            else if (tipoMensagem == typeof(ReceberSerieEventoRequest))
                resposta = processarSerieEvento((ReceberSerieEventoRequest)parametros);
            else if (tipoMensagem == typeof(ReceberSerieCustosBolsaRequest))
                resposta = processarSerieCustosBolsa((ReceberSerieCustosBolsaRequest)parametros);
            else if (tipoMensagem == typeof(ReceberSerieDetalheInstrumentoRequest))
                resposta = processarSerieDetalheInstrumento((ReceberSerieDetalheInstrumentoRequest)parametros);
            
            // Retorna
            return resposta;
        }

        #endregion

        #region Rotinas de Processamento das Séries

        /// <summary>
        /// Processa série de cotações
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        private ReceberSerieCotacaoResponse processarSerieCotacao(ReceberSerieCotacaoRequest parametros)
        {
            // Prepara resposta
            ReceberSerieCotacaoResponse resposta =
                new ReceberSerieCotacaoResponse()
                {
                    Canal = this.Info.Nome,
                    DataFinal = parametros.DataFinal.Value,
                    DataInicial = parametros.DataInicial.Value,
                    Periodo = PeriodoEnum.Dia
                }; 

            // Carrega objeto de configuração
            CanalMarketDataBolsaInfo configInfo = 
                Ativador.Get<IServicoPersistencia>().ConsultarObjetos<CanalMarketDataBolsaInfo>(
                    new ConsultarObjetosRequest<CanalMarketDataBolsaInfo>()).Resultado[0];

            // Varre os arquivos criando o resultado final
            for (DateTime data = parametros.DataInicial.Value; data.Year <= parametros.DataFinal.Value.Year; data = data.AddYears(1))
            {
                // Monta o nome do arquivo
                string arquivo = _servicoBVMFIntegracao.ReceberNomeArquivoBovespa(PeriodoEnum.Ano, data);

                // Verifica se o arquivo está carregado
                if (!_tabelasHist.ContainsKey(arquivo) || (data.Year == DateTime.Now.Year && parametros.DataFinal > configInfo.DataReferenciaUltimaRequisicao))
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
                    // Também carrega o arquivo se a data da última requisição for menor
                    // que a data final da lista de cotações
                    if (stream == null || (data.Year == DateTime.Now.Year && parametros.DataFinal > configInfo.DataReferenciaUltimaRequisicao))
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

                        // Salva objeto de configurações
                        Ativador.Get<IServicoPersistencia>().SalvarObjeto<CanalMarketDataBolsaInfo>(
                            new SalvarObjetoRequest<CanalMarketDataBolsaInfo>() { Objeto = configInfo });
                    }

                    // Pega o layout COTAHIST
                    LayoutBVMFInfo layoutCOTA = _servicoBVMFPersistenciaLayouts.ReceberLayout(typeof(ConversorLayoutBovespa), "COTAHIST");

                    // Faz o parse do arquivo
                    DataSet ds = _servicoBVMFArquivos.InterpretarArquivo(layoutCOTA, stream);

                    // Adiciona na colecao
                    // Os arquivos já podem existir anteriormente quando o arquivo foi recarregado por causa
                    // da ultima data de carga
                    if (!_tabelasHist.ContainsKey(arquivo))
                    {
                        _tabelasHist.Add(arquivo, ds);
                        _viewHist.Add(arquivo, new DataView(ds.Tables["DETALHE"], null, "CODNEG", DataViewRowState.CurrentRows));
                    }
                    else
                    {
                        _tabelasHist[arquivo] = ds;
                        _viewHist[arquivo] = new DataView(ds.Tables["DETALHE"], null, "CODNEG", DataViewRowState.CurrentRows);
                    }
                }

                // Faz o filtro
                List<DataRowView> drss = new List<DataRowView>();
                List<DataRowView> drs =
                    (from r in _viewHist[arquivo].FindRows(parametros.Instrumento.CodigoNegociacao)
                     where (DateTime)r["DATPRE"] >= parametros.DataInicial && (DateTime)r["DATPRE"] < parametros.DataFinal
                     select r).ToList();

                // Se não trouxe nada e foi pedido para trazer pelo menos uma, consulta novamente
                if (drs.Count == 0 && parametros.SeNaoEncontradoTrazerUltimo)
                    drs = 
                        new List<DataRowView>(
                            (from r in _viewHist[arquivo].FindRows(parametros.Instrumento.CodigoNegociacao)
                             where (DateTime)r["DATPRE"] <= parametros.DataInicial 
                             orderby (DateTime)r["DATPRE"] descending 
                             select r).Take(1));

                // Preenche a colecao
                foreach (DataRowView dr in drs)
                    resposta.Resultado.Add(
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

            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Processa série de lista de instrumentos
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        private ReceberSerieInstrumentosResponse processarSerieInstrumentosBovespa(ReceberSerieInstrumentosRequest parametros)
        {
            // Inicializa
            ReceberSerieInstrumentosResponse response = new ReceberSerieInstrumentosResponse();
            DataRow[] drInstrumentos = null;
            CanalMarketDataBolsaConfig config = GerenciadorConfig.ReceberConfig<CanalMarketDataBolsaConfig>();

            // Faz a consulta de acordo com o tipo da lista
            switch (parametros.TipoLista)
            {
                case ReceberSerieInstrumentosTipoListaEnum.Padrao:
                case ReceberSerieInstrumentosTipoListaEnum.ApenasHabilitados:
                    drInstrumentos = _dsPAP.Tables["DADOS_DOS_PAPEIS"].Select("DATA_LIMITE_NEGOCIACAO = '31/12/2040'");
                    break;
                case ReceberSerieInstrumentosTipoListaEnum.HistoricoCompleto:
                    drInstrumentos = _dsPAP.Tables["DADOS_DOS_PAPEIS"].Select();
                    break;
            }

            // Varre os registros criando as informacoes do instrumento
            foreach (DataRow dr in drInstrumentos)
                response.Instrumentos.Add(
                    new InstrumentoInfo()
                    {
                        CodigoNegociacao = (string)dr["CODIGO_NEGOCIACAO"],
                        Especificacao = dr["DISTRIBUICAO"].ToString(),
                        DataInicioNegociacao = (DateTime)dr["DATA_INICIO_NEGOCIACAO"],
                        DataFimNegociacao = (DateTime)dr["DATA_LIMITE_NEGOCIACAO"],
                        CodigoBolsa = "BOVESPA",
                        Status = (DateTime)dr["DATA_LIMITE_NEGOCIACAO"] > DateTime.Now ? InstrumentoStatusEnum.Habilitado : InstrumentoStatusEnum.Desabilitado,
                        Tipo = traduzirInstrumentoTipo(dr["TIPO_MERCADO"].ToString())
                    });

            // Retorna
            return response;
        }

        /// <summary>
        /// Processa série de eventos financeiros
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        private ReceberSerieEventoResponse processarSerieEvento(ReceberSerieEventoRequest parametros)
        {
            // Prepara o retorno
            ReceberSerieEventoResponse resposta = new ReceberSerieEventoResponse();
            
            // Cria lista com os tipos de eventos a serem retornados
            // Traduz o tipo do evento para usar no filtro
            string tipoEventoStr = "";
            foreach (SerieBolsaTipoEnum tipoEvento in parametros.TiposEventos)
                tipoEventoStr += "'" + _dicionarioSerieTipo.DicionarioChaveInterno[tipoEvento].ElementoExterno + "',";
            tipoEventoStr = tipoEventoStr.Substring(0, tipoEventoStr.Length - 1);

            // Faz o filtro dos elementos
            DataRow[] drs =
                _dsPRO.Tables["DETALHE"].Select(
                    "CODIGO_NEGOCIACAO = '" + parametros.Instrumento.CodigoNegociacao + "' and " +
                    "DESCRICAO_TIPO_PROVENTO_EVENTO IN (" + tipoEventoStr + ") and " +
                    "DATA_APROVACAO_PROVENTO >= '" + parametros.DataInicial.ToString("yyyy/MM/dd") + "' and " +
                    "DATA_APROVACAO_PROVENTO < '" + parametros.DataFinal.ToString("yyyy/MM/dd") + "' ");

            // Preenche a coleção
            foreach (DataRow dr in drs)
                resposta.Resultado.Add(
                    new EventoBolsaInfo()
                    {
                        Data = (DateTime)dr["DATA_APROVACAO_PROVENTO"],
                        Valor = (double)dr["VALOR_PROVENTO"],
                        TipoEvento = _dicionarioSerieTipo.DicionarioChaveExterno["DESCRICAO_TIPO_PROVENTO_EVENTO"].ElementoInterno
                    });

            // Retorna a lista
            return resposta;
        }

        /// <summary>
        /// Processa série de custos de bolsa
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        private ReceberSerieCustosBolsaResponse processarSerieCustosBolsa(ReceberSerieCustosBolsaRequest parametros)
        {
            // Prepara resposta
            ReceberSerieCustosBolsaResponse resposta = 
                new ReceberSerieCustosBolsaResponse();

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
                    new CustoBolsaInfo()
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
            List<CustoBolsaInfo> custos =
                (from c in serieCustoInfo.Itens
                 where c.DataReferencia <= parametros.DataFinal
                 orderby c.DataReferencia
                 select c).ToList();
            resposta.CustosBolsa = custos;

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Processa série de detalhe de instrumento
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        private ReceberSerieDetalheInstrumentoResponse processarSerieDetalheInstrumento(ReceberSerieDetalheInstrumentoRequest parametros)
        {
            // Prepara resposta
            ReceberSerieDetalheInstrumentoResponse resposta = 
                new ReceberSerieDetalheInstrumentoResponse();

            // Faz a consulta no PAP 
            if (parametros.InstrumentoInfo.CodigoBolsa == "BOVESPA")
            {
                // String de consulta
                string sql = "";
                if (parametros.InstrumentoInfo.CodigoNegociacao != null)
                    sql += "CODIGO_NEGOCIACAO = '" + parametros.InstrumentoInfo.CodigoNegociacao + "' AND ";
                if (parametros.InstrumentoInfo.Empresa != null)
                    sql += "EMPRESA_EMITENTE = '" + parametros.InstrumentoInfo.Empresa + "' AND ";
                if (parametros.InstrumentoInfo.DataInicioNegociacao.HasValue)
                    sql += "DATA_INICIO_NEGOCIACAO >= '" + parametros.InstrumentoInfo.DataInicioNegociacao.Value.ToString("yyyy/MM/dd") + "' AND ";
                if (parametros.InstrumentoInfo.DataInicioNegociacao.HasValue)
                    sql += "DATA_LIMITE_NEGOCIACAO < '" + parametros.InstrumentoInfo.DataFimNegociacao.Value.ToString("yyyy/MM/dd") + "' AND ";
                if (parametros.InstrumentoInfo.DataInicioNegociacao.HasValue)
                    sql += "DISTRIBUICAO = '" + parametros.InstrumentoInfo.Especificacao + "' AND ";
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
            }

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Faz a tradução de uma linha do PAP para um instrumento
        /// </summary>
        /// <param name="drPAP"></param>
        /// <returns></returns>
        private InstrumentoInfo traduzirLinhaPAP(DataRow drPAP)
        {
            // Inicializa
            InstrumentoInfo instrumentoInfo = null;

            // Pega o tipo do instrumento
            InstrumentoTipoEnum instrumentoTipo = traduzirInstrumentoTipo(drPAP["TIPO_MERCADO"].ToString());

            // Faz de acordo com o tipo
            switch (instrumentoTipo)
            {
                case InstrumentoTipoEnum.Acao:
                    instrumentoInfo = 
                        new InstrumentoAcaoInfo() 
                        { 
                            CodigoBolsa = "BOVESPA",
                            CodigoNegociacao = drPAP["CODIGO_NEGOCIACAO"].ToString().Trim(),
                            Empresa = (string)drPAP["EMPRESA_EMITENTE"],
                            DataFimNegociacao = (DateTime)drPAP["DATA_LIMITE_NEGOCIACAO"],
                            DataInicioNegociacao = (DateTime)drPAP["DATA_INICIO_NEGOCIACAO"],
                            Especificacao = drPAP["DISTRIBUICAO"].ToString(),
                            Status = (DateTime)drPAP["DATA_LIMITE_NEGOCIACAO"] > DateTime.Now ? InstrumentoStatusEnum.Habilitado : InstrumentoStatusEnum.Desabilitado,
                            Tipo = InstrumentoTipoEnum.Acao
                        };
                    break;
                case InstrumentoTipoEnum.Opcao:
                    instrumentoInfo =
                        new InstrumentoOpcaoInfo()
                        {
                            CodigoBolsa = "BOVESPA",
                            CodigoNegociacao = drPAP["CODIGO_NEGOCIACAO"].ToString().Trim(),
                            DataFimNegociacao = (DateTime)drPAP["DATA_LIMITE_NEGOCIACAO"],
                            DataInicioNegociacao = (DateTime)drPAP["DATA_INICIO_NEGOCIACAO"],
                            Empresa = (string)drPAP["EMPRESA_EMITENTE"],
                            Especificacao = drPAP["DISTRIBUICAO"].ToString(),
                            Status = (DateTime)drPAP["DATA_LIMITE_NEGOCIACAO"] > DateTime.Now ? InstrumentoStatusEnum.Habilitado : InstrumentoStatusEnum.Desabilitado,
                            Tipo = InstrumentoTipoEnum.Opcao,
                            DataVencimento = (DateTime)drPAP["DATA_VENCIMENTO"],
                            InstrumentoBase = 
                                new InstrumentoInfo() 
                                { 
                                    CodigoBolsa = "BOVESPA", 
                                    CodigoNegociacao = (string)drPAP["CODIGO_PAPEL_ABERTO"] 
                                },
                            PrecoExercicio = Convert.ToDouble(drPAP["PRECO_EXERCICIO_OU_VALOR_CONTRATO"]),
                            TipoOpcao = drPAP["TIPO_MERCADO"].ToString() == "70" ? InstrumentoOpcaoTipoEnum.Call : InstrumentoOpcaoTipoEnum.Put
                        };
                    break;
            }

            // Retorna
            return instrumentoInfo;
        }

        #endregion

        #region Rotinas Locais

        /// <summary>
        /// Faz a tradução do tipo do instrumento de string (campo DESCRICAO_MERCADO do PAP) para o enumerador
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private InstrumentoTipoEnum traduzirInstrumentoTipo(string tipo)
        {
            // Inicializa
            InstrumentoTipoEnum retorno = InstrumentoTipoEnum.Desconhecido;

            // Descobre
            switch (tipo)
            {
                case "10":
                    retorno = InstrumentoTipoEnum.Acao;
                    break;
                case "30":
                    retorno = InstrumentoTipoEnum.Termo;
                    break;
                case "80":
                    retorno = InstrumentoTipoEnum.Opcao;
                    break;
                case "70":
                    retorno = InstrumentoTipoEnum.Opcao;
                    break;
                case "50":
                    retorno = InstrumentoTipoEnum.Futuro;
                    break;
                case "17":
                    retorno = InstrumentoTipoEnum.Leilao;
                    break;
            }

            // Retorna
            return retorno;
        }

        #endregion
    }
}
