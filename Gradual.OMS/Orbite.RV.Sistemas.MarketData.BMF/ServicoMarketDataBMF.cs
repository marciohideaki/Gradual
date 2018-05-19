using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

using Orbite.RV.Contratos.Integracao.BVMF;
using Orbite.RV.Contratos.Integracao.BVMF.Dados;
using Orbite.RV.Contratos.MarketData.BMF;
using Orbite.RV.Contratos.MarketData.BMF.Dados;
using Orbite.RV.Contratos.MarketData.BMF.Mensagens;
using Orbite.RV.Sistemas.Integracao.BVMF;

namespace Orbite.RV.Sistemas.MarketData.BMF
{
    /// <summary>
    /// Implementação do serviço de market data BMF
    /// </summary>
    public class ServicoMarketDataBMF : IServicoMarketDataBMF
    {
        #region Variaveis Locais

        /// <summary>
        /// Dicionário para relacionar os tipos de mercados do enumerador com os códigos bmf.
        /// O valor1 é o código bmf, e o valor2 é o enumerador
        /// </summary>
        private RelacaoValorValorHelper<byte, InstrumentoBMFTipoMercadoEnum> _dicionarioTipoMercado =
            new RelacaoValorValorHelper<byte, InstrumentoBMFTipoMercadoEnum>();

        /// <summary>
        /// Dicionário para relacionar os tipos de opções do enumerador com os tipos bmf.
        /// O valor1 é o tipo bmf, e o valor2 é o enumerador
        /// </summary>
        private RelacaoValorValorHelper<string, InstrumentoBMFOpcaoTipoEnum> _dicionarioTipoOpcao =
            new RelacaoValorValorHelper<string, InstrumentoBMFOpcaoTipoEnum>();

        /// <summary>
        /// Dicionário para relacionar os modelos de opções do enumerador com os tipos bmf.
        /// O valor1 é o valor bmf, e o valor2 é o enumerador
        /// </summary>
        private RelacaoValorValorHelper<string, InstrumentoBMFOpcaoModeloEnum> _dicionarioModeloOpcao =
            new RelacaoValorValorHelper<string, InstrumentoBMFOpcaoModeloEnum>();

        /// <summary>
        /// Lista os arquivos do diretório apontado pelo config
        /// </summary>
        private List<ArquivoBVMFInfo> _diretorio = null;

        /// <summary>
        /// Lista de instrumentos BMF.
        /// A chave é o código de negociação
        /// </summary>
        private Dictionary<string, InstrumentoBMFInfo> _instrumentos = new Dictionary<string, InstrumentoBMFInfo>();

        // Controle do histórico carregado
        private Dictionary<string, CotacaoBMFInfo> _cotacoesAtuais = new Dictionary<string, CotacaoBMFInfo>();
        private DateTime _dataHistoricoInicial = DateTime.MaxValue;
        private DateTime _dataHistoricoFinal = DateTime.MinValue;

        // Histórico de cotações
        // As chaves dos dicionários são: ativo, ano, mes e dataReferencia
        private Dictionary<string, Dictionary<int, Dictionary<int, Dictionary<DateTime, CotacaoBMFInfo>>>> _cotacoesHistorico =
            new Dictionary<string, Dictionary<int, Dictionary<int, Dictionary<DateTime, CotacaoBMFInfo>>>>();

        // Faz cache das referencias para os servicos 
        private IServicoIntegracaoBVMFPersistencia _servicoBVMFPersistencia = null;
        private IServicoIntegracaoBVMFPersistenciaLayouts _servicoBVMFPersistenciaLayouts = null;
        private IServicoIntegracaoBVMFArquivos _servicoBVMFArquivos = null;
        
        #endregion

        #region IServicoMarketDataBMF Members

        /// <summary>
        /// Solicita a lista de instrumentos BMF
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarInstrumentosBMFResponse ReceberListaInstrumentosBMF(ListarInstrumentosBMFRequest parametros)
        {
            // Prepara resposta
            ListarInstrumentosBMFResponse resposta =
                new ListarInstrumentosBMFResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Faz o filtro
            resposta.Instrumentos =
                (from i in _instrumentos
                 where (parametros.FiltroCodigoMercadoria == null || i.Value.CodigoMercadoria == parametros.FiltroCodigoMercadoria) &&
                       (parametros.FiltroTipoMercado == InstrumentoBMFTipoMercadoEnum.NaoInformado || i.Value.TipoMercado == parametros.FiltroTipoMercado)
                 select i.Value).ToList();

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita detalhe de um instrumento BMF
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberDetalheInstrumentoBMFResponse ReceberDetalheInstrumentoBMF(ReceberDetalheInstrumentoBMFRequest parametros)
        {
            // Prepara resposta
            ReceberDetalheInstrumentoBMFResponse resposta =
                new ReceberDetalheInstrumentoBMFResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Tem o instrumento?
            if (_instrumentos.ContainsKey(parametros.CodigoNegociacao))
                resposta.Instrumento = _instrumentos[parametros.CodigoNegociacao];

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita histórico de cotações de um instrumento BMF
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberHistoricoCotacaoBMFResponse ReceberHistoricoCotacaoBMF(ReceberHistoricoCotacaoBMFRequest parametros)
        {
            // Prepara resposta
            ReceberHistoricoCotacaoBMFResponse resposta =
                new ReceberHistoricoCotacaoBMFResponse()
                {
                    DataFinal = parametros.DataFinal.Value,
                    DataInicial = parametros.DataInicial.Value
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
                Dictionary<int, Dictionary<int, Dictionary<DateTime, CotacaoBMFInfo>>> historicoAtivo =
                    _cotacoesHistorico[parametros.Instrumento.CodigoNegociacao];

                // Preenche com as cotações
                for (DateTime mesReferencia = parametros.DataInicial.Value; mesReferencia <= parametros.DataFinal.Value; mesReferencia = mesReferencia.AddMonths(1))
                {
                    Dictionary<DateTime, CotacaoBMFInfo> cotacoes = null;
                    if (historicoAtivo.ContainsKey(mesReferencia.Year) && historicoAtivo[mesReferencia.Year].ContainsKey(mesReferencia.Month))
                        cotacoes = historicoAtivo[mesReferencia.Year][mesReferencia.Month];
                    if (cotacoes != null)
                        foreach (KeyValuePair<DateTime, CotacaoBMFInfo> cotacao in cotacoes)
                            if (cotacao.Value.DataReferencia >= parametros.DataInicial.Value && cotacao.Value.DataReferencia <= parametros.DataFinal.Value)
                                resposta.Resultado.Add(cotacao.Value);
                }
            }

            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Garante que o periodo solicitado esteja carregado
        /// </summary>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        private void carregarPeriodo(DateTime dataInicial, DateTime dataFinal)
        {
            // Os arquivos são diários, e não há download automático ainda. Assim a rotina fica mais fácil
            if (dataInicial < _dataHistoricoInicial || dataFinal > _dataHistoricoFinal)
            {
                // Filtra os arquivos a serem carregados
                List<ArquivoBVMFInfo> arquivosCarregar = 
                    (from d in _diretorio
                     where d.Layout.Nome == "PS_BD_FINAL" &&
                           d.DataMovimento >= dataInicial && d.DataMovimento < _dataHistoricoInicial &&
                           d.DataMovimento <= dataFinal && d.DataMovimento > _dataHistoricoFinal 
                     select d).ToList();
                
                // Carrega os arquivos encotnrados
                arquivosCarregar.ForEach(new Action<ArquivoBVMFInfo>(carregarArquivo));

                // Informa que o período está carregado
                _dataHistoricoInicial = dataInicial;
                _dataHistoricoFinal = dataFinal;
            }
        }

        /// <summary>
        /// Faz a carga do arquivo de cotações BMF informado
        /// </summary>
        /// <param name="nomeArquivo"></param>
        /// <param name="periodo"></param>
        /// <param name="data"></param>
        private void carregarArquivo(ArquivoBVMFInfo arquivoInfo)
        {
            // Pega o layout 
            LayoutBVMFInfo layout = _servicoBVMFPersistenciaLayouts.ReceberLayout(typeof(ConversorLayoutBMF), "PS_BD_FINAL");

            // Faz o parse do arquivo
            FileStream stream = File.Open(arquivoInfo.NomeArquivo, FileMode.Open, FileAccess.Read);
            DataSet ds = _servicoBVMFArquivos.InterpretarArquivo(layout, stream);
            stream.Close();

            // Faz a carga das cotações
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                // Monta o objeto de cotação
                string ativo = (string)dr["CODIGO_NEGOCIACAO_GTS"];
                CotacaoBMFInfo cotacao =
                    new CotacaoBMFInfo()
                    {
                        DataReferencia = (DateTime)dr["DATA_GERACAO"],
                        CodigoMercadoria = (string)dr["CODIGO_MERCADORIA"],
                        CodigoNegociacao = (string)dr["CODIGO_NEGOCIACAO_GTS"],
                        CotacaoAjuste = Convert.ToDouble(dr["COT_AJUSTE"]),
                        CotacaoFechamentoDia = Convert.ToDouble(dr["COT_FECH_DIA"]),
                        CotacaoMaiorNegocioDia = Convert.ToDouble(dr["COT_MAIOR_NEG_DIA"]),
                        CotacaoMediaNegociosDia = Convert.ToDouble(dr["COT_MEDIA_NEG_DIA"]),
                        CotacaoMenorNegocioDia = Convert.ToDouble(dr["COT_MENOR_NEG_DIA"]),
                        CotacaoPrimeiroNegocioDia = Convert.ToDouble(dr["COT_PRIMEIRO_NEG_DIA"]),
                        CotacaoUltimaOfertaCompraDia = Convert.ToDouble(dr["COTACAO_ULTIMA_OFERTA_COMPRA_DIA"]),
                        CotacaoUltimaOfertaVendaDia = Convert.ToDouble(dr["COT_ULT_OFERTA_VENDA_DIA"]),
                        CotacaoUltimoNegocioAnterior = Convert.ToDouble(dr["COT_ULT_NEG_ANTERIOR"]),
                        CotacaoUltimoNegocioDia = Convert.ToDouble(dr["COT_ULT_NEG_DIA"]),
                        DataUltimoNegocioDia = dr["DATA_ULT_NEG"] != DBNull.Value ? new DateTime?((DateTime)dr["DATA_ULT_NEG"]) : null,
                        DataVencimento = dr["DATA_VENCIMENTO"] != DBNull.Value ? new DateTime?((DateTime)dr["DATA_VENCIMENTO"]) : null,
                        PrecoExercicio = Convert.ToDouble(dr["PRECO_EXERCICIO"]),
                        QuantidadeContratosAberto = (int)dr["QTD_CONTRATOS_ABERTO"],
                        QuantidadeContratosNegociadosDia = (int)dr["QTD_CONTRATOS_NEGOCIADOS"],
                        QuantidadeContratosUltimaOfertaCompraDia = (int)dr["QTD_CONTRATOS_ULTIMA_OFERTA_DIA"],
                        QuantidadeContratosUltimaOfertaVendaDia = (int)dr["QTD_CONTRATOS_ULTIMA_OFERTA_VENDA_DIA"],
                        QuantidadeContratosUltimoNegocioDia = (int)dr["QTD_CONTRATOS_ULT_NEG_DIA"],
                        QuantidadeNegociosDia = (int)dr["QTD_NEGOCIOS_EFETUADOS"],
                        SerieVencimento = (string)dr["SERIE_VENCIMENTO"],
                        SituacaoAjusteDia = CotacaoBMFSituacaoAjusteEnum.NaoInformado,
                        TipoMercado = InstrumentoBMFTipoMercadoEnum.NaoInformado,
                        TipoSerie = CotacaoBMFTipoSerieEnum.NaoInformado,
                        ValorOuTamanhoContrato = Convert.ToDouble(dr["VALOR_TAMANHO_CONTRATO"]),
                        VolumeDiaEmDolar = Convert.ToDouble(dr["VOLUME_DIA_US$"]),
                        VolumeDiaEmReais = Convert.ToDouble(dr["VOLUME_DIA_R$"])
                    };

                // Adiciona ou recupera item do ativo
                Dictionary<int, Dictionary<int, Dictionary<DateTime, CotacaoBMFInfo>>> dicionarioCotacoes = null;
                if (!_cotacoesHistorico.ContainsKey(ativo))
                    _cotacoesHistorico.Add(ativo, new Dictionary<int, Dictionary<int, Dictionary<DateTime, CotacaoBMFInfo>>>());
                dicionarioCotacoes = _cotacoesHistorico[ativo];

                // Adiciona ou recupera item da cotação
                if (!dicionarioCotacoes.ContainsKey(cotacao.DataReferencia.Year))
                    dicionarioCotacoes.Add(cotacao.DataReferencia.Year, new Dictionary<int, Dictionary<DateTime, CotacaoBMFInfo>>());
                if (!dicionarioCotacoes[cotacao.DataReferencia.Year].ContainsKey(cotacao.DataReferencia.Month))
                    dicionarioCotacoes[cotacao.DataReferencia.Year].Add(cotacao.DataReferencia.Month, new Dictionary<DateTime, CotacaoBMFInfo>());
                if (!dicionarioCotacoes[cotacao.DataReferencia.Year][cotacao.DataReferencia.Month].ContainsKey(cotacao.DataReferencia))
                    dicionarioCotacoes[cotacao.DataReferencia.Year][cotacao.DataReferencia.Month].Add(cotacao.DataReferencia, cotacao);

                // Verifica se a lista de última cotação está com valores atualizados
                if (!_cotacoesAtuais.ContainsKey(ativo))
                    _cotacoesAtuais.Add(ativo, cotacao);
                else if (_cotacoesAtuais[ativo].DataReferencia < cotacao.DataReferencia)
                    _cotacoesAtuais[ativo] = cotacao;
            }
        }

        /// <summary>
        /// Solicita ultima cotação de ativos
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberUltimaCotacaoBMFResponse ReceberUltimaCotacaoBMF(ReceberUltimaCotacaoBMFRequest parametros)
        {
            // Prepara resposta
            ReceberUltimaCotacaoBMFResponse resposta =
                new ReceberUltimaCotacaoBMFResponse()
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
                    CotacaoBMFInfo cotacao = (CotacaoBMFInfo)_cotacoesAtuais[ativo].Clone();
                    cotacao.CodigoNegociacao = ativo;
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
            inicializar();
        }

        public void PararServico()
        {
        }

        public Gradual.OMS.Library.Servicos.ServicoStatus ReceberStatusServico()
        {
            return ServicoStatus.Indefinido;
        }

        #endregion

        #region Rotinas Locais

        /// <summary>
        /// Rotina de inicialização do serviço
        /// </summary>
        private void inicializar()
        {
            // Pega referencia para os serviços que serão utilizados
            _servicoBVMFPersistencia = Ativador.Get<IServicoIntegracaoBVMFPersistencia>();
            _servicoBVMFPersistenciaLayouts = Ativador.Get<IServicoIntegracaoBVMFPersistenciaLayouts>();
            _servicoBVMFArquivos = Ativador.Get<IServicoIntegracaoBVMFArquivos>();

            // Cria o dicionario de tipos de mercados
            _dicionarioTipoMercado.Adicionar(1, InstrumentoBMFTipoMercadoEnum.Disponivel);
            _dicionarioTipoMercado.Adicionar(2, InstrumentoBMFTipoMercadoEnum.Futuro);
            _dicionarioTipoMercado.Adicionar(3, InstrumentoBMFTipoMercadoEnum.OpcaoDisponivel);
            _dicionarioTipoMercado.Adicionar(4, InstrumentoBMFTipoMercadoEnum.OpcaoFuturo);
            _dicionarioTipoMercado.Adicionar(5, InstrumentoBMFTipoMercadoEnum.Termo);

            // Cria o dicionario de tipos de opções
            _dicionarioTipoOpcao.Adicionar("C", InstrumentoBMFOpcaoTipoEnum.Compra);
            _dicionarioTipoOpcao.Adicionar("V", InstrumentoBMFOpcaoTipoEnum.Venda);

            // Cria o dicionario de modelos de opções
            _dicionarioModeloOpcao.Adicionar("A", InstrumentoBMFOpcaoModeloEnum.Americana);
            _dicionarioModeloOpcao.Adicionar("E", InstrumentoBMFOpcaoModeloEnum.Europeia);

            // Pega configuracoes
            ServicoMarketDataBMFConfig config =
                GerenciadorConfig.ReceberConfig<ServicoMarketDataBMFConfig>();

            // Pede o diretorio de arquivos
            IServicoIntegracaoBVMFArquivos servicoIntegracaoBVMFArquivos =
                Ativador.Get<IServicoIntegracaoBVMFArquivos>();
            _diretorio = servicoIntegracaoBVMFArquivos.ListarDiretorio(config.DiretorioArquivosBMF);

            // Acha e carrega arquivo de papeis
            ArquivoBVMFInfo arquivoPapeis = (from d in _diretorio
                                             where d.Layout.Nome == "PS_PR_D100_0199"
                                             orderby d.DataMovimento descending
                                             select d).FirstOrDefault();
            if (arquivoPapeis != null)
                carregarInstrumentos(
                    servicoIntegracaoBVMFArquivos.InterpretarArquivo(
                        arquivoPapeis.Layout,
                        arquivoPapeis.NomeArquivo));
        }

        /// <summary>
        /// Faz a carga da lista de instrumentos
        /// </summary>
        /// <param name="dsInstrumentos"></param>
        private void carregarInstrumentos(DataSet dsInstrumentos)
        {
            // Varre a tabela criando os instrumentos
            foreach (DataRow dr in dsInstrumentos.Tables[0].Rows)
            {
                // Faz a tradução do tipo de mercado
                InstrumentoBMFTipoMercadoEnum tipoMercado = 
                    _dicionarioTipoMercado.DicionarioDe1Para2[(byte)dr["TIPO_MERCADO"]].Valor2;

                // Cria o objeto correspondente
                InstrumentoBMFInfo instrumentoBMF = null;
                switch (tipoMercado)
                {
                    case InstrumentoBMFTipoMercadoEnum.Disponivel:
                    case InstrumentoBMFTipoMercadoEnum.Futuro:
                    case InstrumentoBMFTipoMercadoEnum.Termo:
                        instrumentoBMF = new InstrumentoBMFInfo();
                        break;
                    case InstrumentoBMFTipoMercadoEnum.OpcaoDisponivel:
                    case InstrumentoBMFTipoMercadoEnum.OpcaoFuturo:
                        instrumentoBMF = new InstrumentoBMFOpcaoInfo();
                        break;
                }

                // Preenche as informações básicas
                instrumentoBMF.CodigoMercadoria = dr["CODIGO_MERCADORIA"] != DBNull.Value ? (string)dr["CODIGO_MERCADORIA"] : null;
                instrumentoBMF.CodigoNegociacao = dr["CODIGO_NEGOCIACAO_GTS"] != DBNull.Value ? (string)dr["CODIGO_NEGOCIACAO_GTS"] : null;
                instrumentoBMF.DataInicioNegociacao = dr["DATA_INICIO_NEGOCIACAO"] != DBNull.Value ? (DateTime)dr["DATA_INICIO_NEGOCIACAO"] : DateTime.MinValue;
                instrumentoBMF.DataLimiteNegociacao = dr["DATA_LIMITE_NEGOCIACAO"] != DBNull.Value ? (DateTime)dr["DATA_LIMITE_NEGOCIACAO"] : DateTime.MinValue;
                instrumentoBMF.DataVencimento = dr["DATA_VENCIMENTO_CONTRATO"] != DBNull.Value ? new DateTime?((DateTime)dr["DATA_VENCIMENTO_CONTRATO"]) : null;
                instrumentoBMF.SerieVencimento = dr["SERIE_VENCIMENTO"] != DBNull.Value ? (string)dr["SERIE_VENCIMENTO"] : null;
                instrumentoBMF.TipoMercado = tipoMercado;

                // Preenche as informações adicionais
                switch (tipoMercado)
                {
                    case InstrumentoBMFTipoMercadoEnum.OpcaoDisponivel:
                    case InstrumentoBMFTipoMercadoEnum.OpcaoFuturo:
                        InstrumentoBMFOpcaoInfo instrumentoOpcao = (InstrumentoBMFOpcaoInfo)instrumentoBMF;
                        instrumentoOpcao.ModeloOpcao = InstrumentoBMFOpcaoModeloEnum.NaoInformado;
                        instrumentoOpcao.TipoOpcao = InstrumentoBMFOpcaoTipoEnum.NaoInformado;
                        break;
                }

                // Adiciona na coleção
                _instrumentos.Add(instrumentoBMF.CodigoNegociacao, instrumentoBMF);
            }
        }

        #endregion
    }
}
