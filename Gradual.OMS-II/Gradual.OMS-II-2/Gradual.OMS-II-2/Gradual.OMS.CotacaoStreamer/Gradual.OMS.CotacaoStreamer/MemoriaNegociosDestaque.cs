using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Sockets;
using log4net;
using System.Globalization;
using Gradual.OMS.CotacaoStreamer.Dados;
using Gradual.OMS.CotacaoStreamer.Lib.Dados;

namespace Gradual.OMS.CotacaoStreamer
{
    /// <summary>
    /// Classe responsável por armazenas as estruturas de Ranking e contabilizar os Negocios em Destaque.   
    /// </summary>

    [Serializable]
    public static class MemoriaNegociosDestaque
    {
        #region Globais

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string SEGMENTOMERCADO_BOVESPA_VISTA = "01";
        private const string SEGMENTOMERCADO_BOVESPA_TERMO = "02";
        private const string SEGMENTOMERCADO_BOVESPA_OPCOES = "04";

        private const string SEGMENTOMERCADO_BMF_FUTURO = "FUT";
        private const string SEGMENTOMERCADO_BMF_VISTA = "SPOT";
        private const string SEGMENTOMERCADO_BMF_OPCAO_VISTA = "SOPT";
        private const string SEGMENTOMERCADO_BMF_OPCAO_FUTURO = "FOPT";

        private static string[] SEGMENTOS_MERCADO = { 
            SEGMENTOMERCADO_BOVESPA_VISTA,
            SEGMENTOMERCADO_BOVESPA_OPCOES,
            SEGMENTOMERCADO_BOVESPA_TERMO,
            SEGMENTOMERCADO_BMF_FUTURO,
            SEGMENTOMERCADO_BMF_VISTA,
            SEGMENTOMERCADO_BMF_OPCAO_VISTA,
            SEGMENTOMERCADO_BMF_OPCAO_FUTURO
        };

        private enum FiltroMercado
        {
            BovespaVista = 1,
            BovespaTermo,
            BovespaOpcoes,
            BMFFuturo,
            BMFVista,
            BMFOpcaoVista,
            BMFOpcaoFuturo
        };

        private const int MAX_ITENS_DESTAQUES = 100;
        private const int MAX_ITENS_HOME_BROKER = 15;

        private static DateTime dataAtual = DateTime.Now;

        /// <summary>
        /// Dictionary que mantém todos os instrumentos do cadastro básico da Bovespa e BM&F
        /// </summary>
        private static Dictionary<string, InstrumentoInfo> dictInstrumentos = new Dictionary<string, InstrumentoInfo>();

        private static Dictionary<string, SortedDictionary<RankingInfo, string>> dictMaioresAltas = new Dictionary<string, SortedDictionary<RankingInfo, string>>();
        private static Dictionary<string, SortedDictionary<RankingInfo, string>> dictMaioresBaixas = new Dictionary<string, SortedDictionary<RankingInfo, string>>();
        private static Dictionary<string, SortedDictionary<string, RankingInfo>> dictMaioresAltasBaixasPorInstrumento = new Dictionary<string, SortedDictionary<string, RankingInfo>>();

        private static Dictionary<string, SortedDictionary<RankingInfo, string>> dictMaioresVolumes = new Dictionary<string, SortedDictionary<RankingInfo, string>>();
        private static Dictionary<string, SortedDictionary<string, RankingInfo>> dictMaioresVolumesPorInstrumento = new Dictionary<string, SortedDictionary<string, RankingInfo>>();

        private static Dictionary<string, SortedDictionary<RankingInfo, string>> dictMaisNegociados = new Dictionary<string, SortedDictionary<RankingInfo, string>>();
        private static Dictionary<string, SortedDictionary<string, RankingInfo>> dictMaisNegociadosPorInstrumento = new Dictionary<string, SortedDictionary<string, RankingInfo>>();

        #endregion

        #region Métodos Públicos

        public static string ReceberNegociosDestaque(string segmentoMercado, TipoDestaqueEnum tipoDestaque)
        {
            logger.Debug("HomeBroker solicitando NegociosDestaque segmentoMercado[" + segmentoMercado + "] tipoDestaque[" + tipoDestaque + "]");

            // Monta mensagem de Negocios em Destaque
            string mensagem = "ND" + DateTime.Now.ToString("yyyyMMddHHmmss");

            int ocorrencias = 0;

            try
            {
                if (!((IList<string>)SEGMENTOS_MERCADO).Contains(segmentoMercado))
                {
                    logger.Error("HomeBroker segmentoMercado[" + segmentoMercado + "] invalido!");
                    mensagem += ocorrencias.ToString("D2");
                    return mensagem;
                }

                if (tipoDestaque != TipoDestaqueEnum.MaioresAltas &&
                    tipoDestaque != TipoDestaqueEnum.MaioresBaixas &&
                    tipoDestaque != TipoDestaqueEnum.MaioresVolumes &&
                    tipoDestaque != TipoDestaqueEnum.MaisNegociadas)
                {
                    logger.Error("HomeBroker tipoDestaque[" + tipoDestaque + "] invalido!");
                    mensagem += ocorrencias.ToString("D2");
                    return mensagem;
                }

                mensagem += String.Format("{0,-4}", segmentoMercado);
                mensagem += ((int)Enum.Parse(typeof(TipoDestaqueEnum), tipoDestaque.ToString())).ToString();

                string listaOcorrencias = "";

                lock (dictInstrumentos)
                {
                    if (tipoDestaque == TipoDestaqueEnum.MaioresAltas)
                    {
                        lock (dictMaioresAltasBaixasPorInstrumento)
                        {
                            foreach (KeyValuePair<RankingInfo, string> info in dictMaioresAltas[segmentoMercado])
                            {
                                // Apenas lista os destaques da mesma data atual
                                if (!((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMdd").Equals(DateTime.Now.ToString("yyyyMMdd")))
                                    break;

                                if (dictInstrumentos[info.Value].NumeroNegocios == 0)
                                    continue;

                                listaOcorrencias += ((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMddHHmmss");
                                listaOcorrencias += String.Format("{0,-20}", info.Value);
                                listaOcorrencias += String.Format("{0: 00000.00;-00000.00}", info.Key.Valor).Replace('.', ',');
                                listaOcorrencias += String.Format("{0:000000000000}", dictInstrumentos[info.Value].QuantidadePapeis);
                                listaOcorrencias += String.Format("{0:0000000000.00}", dictInstrumentos[info.Value].Preco).Replace('.', ',');
                                listaOcorrencias += String.Format("{0:00000000}", dictInstrumentos[info.Value].NumeroNegocios);
                                listaOcorrencias += String.Format("{0:0000000000000}", dictInstrumentos[info.Value].VolumeNegocios);

                                logger.Debug(String.Format("HomeBroker Segmento[{0,-4}] Destaque[{1,1}] ({2,2}) {3,19:dd/MM/yyyy HH:mm:ss} {4,-14} {5,6}% {6, 12} {7, 13} {8, 13}",
                                    segmentoMercado, tipoDestaque, ocorrencias + 1, dictInstrumentos[info.Value].DataUltimoNegocio, info.Value, info.Key.Valor,
                                    dictInstrumentos[info.Value].QuantidadePapeis, dictInstrumentos[info.Value].Preco,
                                    dictInstrumentos[info.Value].VolumeNegocios));

                                if (++ocorrencias >= MAX_ITENS_HOME_BROKER)
                                    break;
                            }
                        }
                    }

                    else if (tipoDestaque == TipoDestaqueEnum.MaioresBaixas)
                    {
                        lock (dictMaioresAltasBaixasPorInstrumento)
                        {
                            foreach (KeyValuePair<RankingInfo, string> info in dictMaioresBaixas[segmentoMercado])
                            {
                                // Apenas lista os destaques da mesma data atual
                                if (!((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMdd").Equals(DateTime.Now.ToString("yyyyMMdd")))
                                    break;

                                if (dictInstrumentos[info.Value].NumeroNegocios == 0)
                                    continue;

                                listaOcorrencias += ((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMddHHmmss");
                                listaOcorrencias += String.Format("{0,-20}", info.Value);
                                listaOcorrencias += String.Format("{0: 00000.00;-00000.00}", info.Key.Valor).Replace('.', ',');
                                listaOcorrencias += String.Format("{0:000000000000}", dictInstrumentos[info.Value].QuantidadePapeis);
                                listaOcorrencias += String.Format("{0:0000000000.00}", dictInstrumentos[info.Value].Preco).Replace('.', ',');
                                listaOcorrencias += String.Format("{0:00000000}", dictInstrumentos[info.Value].NumeroNegocios);
                                listaOcorrencias += String.Format("{0:0000000000000}", dictInstrumentos[info.Value].VolumeNegocios);

                                logger.Debug(String.Format("HomeBroker Segmento[{0,-4}] Destaque[{1,1}] ({2,2}) {3,19:dd/MM/yyyy HH:mm:ss} {4,-14} {5,6}% {6, 12} {7, 13} {8, 13}",
                                    segmentoMercado, tipoDestaque, ocorrencias + 1, dictInstrumentos[info.Value].DataUltimoNegocio, info.Value, info.Key.Valor,
                                    dictInstrumentos[info.Value].QuantidadePapeis, dictInstrumentos[info.Value].Preco,
                                    dictInstrumentos[info.Value].VolumeNegocios));

                                if (++ocorrencias >= MAX_ITENS_HOME_BROKER)
                                    break;
                            }
                        }
                    }

                    else if (tipoDestaque == TipoDestaqueEnum.MaioresVolumes)
                    {
                        lock (dictMaioresVolumesPorInstrumento)
                        {
                            foreach (KeyValuePair<RankingInfo, string> info in dictMaioresVolumes[segmentoMercado])
                            {
                                // Apenas lista os destaques da mesma data atual
                                if (!((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMdd").Equals(DateTime.Now.ToString("yyyyMMdd")))
                                    break;

                                if (dictInstrumentos[info.Value].NumeroNegocios == 0)
                                    continue;

                                string compradoraTopo = "";
                                foreach (KeyValuePair<RankingInfo, string> compradora in dictInstrumentos[info.Value].DictCompradorasMaioresVolumes)
                                {
                                    compradoraTopo = compradora.Value;
                                    break;
                                }
                                string vendedoraTopo = "";
                                foreach (KeyValuePair<RankingInfo, string> vendedora in dictInstrumentos[info.Value].DictVendedorasMaioresVolumes)
                                {
                                    vendedoraTopo = vendedora.Value;
                                    break;
                                }

                                listaOcorrencias += ((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMddHHmmss");
                                listaOcorrencias += String.Format("{0,-20}", info.Value);
                                listaOcorrencias += String.Format("{0:0000000000000}", info.Key.Valor);
                                listaOcorrencias += String.Format("{0:000000000000}", dictInstrumentos[info.Value].QuantidadePapeis);
                                listaOcorrencias += String.Format("{0:0000000000.00}", dictInstrumentos[info.Value].Preco).Replace('.', ',');
                                listaOcorrencias += String.Format("{0:00000000}", dictInstrumentos[info.Value].NumeroNegocios);
                                listaOcorrencias += compradoraTopo;
                                listaOcorrencias += vendedoraTopo;

                                logger.Debug(String.Format("HomeBroker Segmento[{0,-4}] Destaque[{1,1}] ({2,2}) {3,19:dd/MM/yyyy HH:mm:ss} {4,-14} {5, 13} {6, 12} {7, 13} {8,-8} {9,-8}",
                                    segmentoMercado, tipoDestaque, ocorrencias + 1, dictInstrumentos[info.Value].DataUltimoNegocio, info.Value, info.Key.Valor,
                                    dictInstrumentos[info.Value].QuantidadePapeis, dictInstrumentos[info.Value].Preco,
                                    compradoraTopo, vendedoraTopo));

                                if (++ocorrencias >= MAX_ITENS_HOME_BROKER)
                                    break;
                            }
                        }
                    }

                    else if (tipoDestaque == TipoDestaqueEnum.MaisNegociadas)
                    {
                        lock (dictMaisNegociadosPorInstrumento)
                        {
                            foreach (KeyValuePair<RankingInfo, string> info in dictMaisNegociados[segmentoMercado])
                            {
                                // Apenas lista os destaques da mesma data atual
                                if (!((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMdd").Equals(DateTime.Now.ToString("yyyyMMdd")))
                                    break;

                                if (dictInstrumentos[info.Value].NumeroNegocios == 0)
                                    continue;

                                string compradoraTopo = "";
                                foreach (KeyValuePair<RankingInfo, string> compradora in dictInstrumentos[info.Value].DictCompradorasMaisNegociadas)
                                {
                                    compradoraTopo = compradora.Value;
                                    break;
                                }
                                string vendedoraTopo = "";
                                foreach (KeyValuePair<RankingInfo, string> vendedora in dictInstrumentos[info.Value].DictVendedorasMaisNegociadas)
                                {
                                    vendedoraTopo = vendedora.Value;
                                    break;
                                }

                                listaOcorrencias += ((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMddHHmmss");
                                listaOcorrencias += String.Format("{0,-20}", info.Value);
                                listaOcorrencias += String.Format("{0:0000000000000}", dictInstrumentos[info.Value].VolumeNegocios);
                                listaOcorrencias += String.Format("{0:000000000000}", dictInstrumentos[info.Value].QuantidadePapeis);
                                listaOcorrencias += String.Format("{0:0000000000.00}", dictInstrumentos[info.Value].Preco).Replace('.', ',');
                                listaOcorrencias += String.Format("{0:00000000}", info.Key.Valor);
                                listaOcorrencias += compradoraTopo;
                                listaOcorrencias += vendedoraTopo;

                                logger.Debug(String.Format("HomeBroker Segmento[{0,-4}] Destaque[{1,1}] ({2,2}) {3,19:dd/MM/yyyy HH:mm:ss} {4,-14} {5, 12} {6, 13} {7, 13} {8,-8} {9,-8}",
                                    segmentoMercado, tipoDestaque, ocorrencias + 1, dictInstrumentos[info.Value].DataUltimoNegocio, info.Value, info.Key.Valor,
                                    dictInstrumentos[info.Value].QuantidadePapeis, dictInstrumentos[info.Value].Preco,
                                    compradoraTopo, vendedoraTopo));

                                if (++ocorrencias >= MAX_ITENS_HOME_BROKER)
                                    break;
                            }
                        }
                    }
                }

                mensagem += ocorrencias.ToString("D2");
                mensagem += listaOcorrencias;
            }
            catch (Exception ex)
            {
                logger.Fatal("ReceberNegociosDestaque(): " + ex.Message, ex);
            }

            return mensagem;
        }

        public static void ContabilizaNegocio(NegocioInfo negocio)
        {
            try
            {
                if (dictInstrumentos.ContainsKey(negocio.Instrumento))
                {
                    // Ignora instrumentos que não constar na lista predefinida de segmentos de mercado
                    string segmentoMercado = dictInstrumentos[negocio.Instrumento].SegmentoMercado;
                    if (!((IList<string>)SEGMENTOS_MERCADO).Contains(segmentoMercado))
                        return;

                    // Contabiliza apenas as mensagens de negocio, na data atual, e com número de negocio > 0
                    if (!DateTime.Now.ToString("yyyyMMdd").Equals(negocio.DataHora.ToString("yyyyMMdd")) || negocio.NumeroNegocio == 0)
                        return;

                    // Inicializa dicionarios, se mensagem de negociacao em novo dia
                    if (negocio.DataHora.ToString("yyyyMMdd").CompareTo(dataAtual.ToString("yyyyMMdd")) > 0 && negocio.NumeroNegocio > 0)
                    {
                        dataAtual = negocio.DataHora;
                        logger.Info("Novo dia de negociacoes [" + negocio.DataHora.ToString("dd/MM/yyyy") + "]. Inicializando contabilizacao!");
                        InicializarDicionarios();
                    }

                    /*
                    logger.Debug(String.Format("Contabiliza Instrumento[{0,19:dd/MM/yyyy HH:mm:ss} {1,2} {2,-14} {3,-2} {4,8} {5,13} {6,12} {7,6}% {8,8} {9,8}]",
                        negocio.DataHora, negocio.Status, negocio.Instrumento, negocio.TipoBolsa, negocio.NumeroNegocio, negocio.Preco,
                        negocio.Quantidade, negocio.Variacao, negocio.Compradora, negocio.Vendedora));
                    */

                    RankingInfo rankingNovo = null;
                    SortedDictionary<RankingInfo, string> dict = null;
                    SortedDictionary<string, RankingInfo> dictPorCorretora = null;

                    string compradora = negocio.Compradora.ToString("D8");
                    string vendedora = negocio.Vendedora.ToString("D8");

                    lock (dictMaioresAltasBaixasPorInstrumento)
                    {
                        // Atualiza dicionarios de maiores altas e baixas do instrumento
                        rankingNovo = new RankingInfo(negocio.Variacao, negocio.Instrumento);
                        if (dictMaioresAltasBaixasPorInstrumento[segmentoMercado].ContainsKey(negocio.Instrumento))
                        {
                            RankingInfo rankingAtual = dictMaioresAltasBaixasPorInstrumento[segmentoMercado][negocio.Instrumento];
                            dictMaioresAltas[segmentoMercado].Remove(rankingAtual);
                            dictMaioresBaixas[segmentoMercado].Remove(rankingAtual);
                            dictMaioresAltasBaixasPorInstrumento[segmentoMercado][negocio.Instrumento] = rankingNovo;
                        }
                        else
                        {
                            dictMaioresAltasBaixasPorInstrumento[segmentoMercado].Add(negocio.Instrumento, rankingNovo);
                        }
                        dictMaioresAltas[segmentoMercado].Add(rankingNovo, negocio.Instrumento);
                        dictMaioresBaixas[segmentoMercado].Add(rankingNovo, negocio.Instrumento);
                    }

                    lock (dictMaioresVolumesPorInstrumento)
                    {
                        // Atualiza dicionarios de maiores volumes do instrumento
                        rankingNovo = new RankingInfo(negocio.Volume, negocio.Instrumento);
                        if (dictMaioresVolumesPorInstrumento[segmentoMercado].ContainsKey(negocio.Instrumento))
                        {
                            RankingInfo rankingAtual = dictMaioresVolumesPorInstrumento[segmentoMercado][negocio.Instrumento];
                            dictMaioresVolumes[segmentoMercado].Remove(rankingAtual);
                            dictMaioresVolumesPorInstrumento[segmentoMercado][negocio.Instrumento] = rankingNovo;
                        }
                        else
                        {
                            dictMaioresVolumesPorInstrumento[segmentoMercado].Add(negocio.Instrumento, rankingNovo);
                        }
                        dictMaioresVolumes[segmentoMercado].Add(rankingNovo, negocio.Instrumento);
                    }

                    lock (dictMaisNegociadosPorInstrumento)
                    {
                        // Atualiza dicionarios de mais negociados do instrumento
                        rankingNovo = new RankingInfo(negocio.NumeroNegocio, negocio.Instrumento);
                        if (dictMaisNegociadosPorInstrumento[segmentoMercado].ContainsKey(negocio.Instrumento))
                        {
                            RankingInfo rankingAtual = dictMaisNegociadosPorInstrumento[segmentoMercado][negocio.Instrumento];
                            dictMaisNegociados[segmentoMercado].Remove(rankingAtual);
                            dictMaisNegociadosPorInstrumento[segmentoMercado][negocio.Instrumento] = rankingNovo;
                        }
                        else
                        {
                            dictMaisNegociadosPorInstrumento[segmentoMercado].Add(negocio.Instrumento, rankingNovo);
                        }
                        dictMaisNegociados[segmentoMercado].Add(rankingNovo, negocio.Instrumento);
                    }

                    lock (dictInstrumentos)
                    {
                        dictInstrumentos[negocio.Instrumento].DataUltimoNegocio = negocio.DataHora;
                        dictInstrumentos[negocio.Instrumento].NumeroNegocios = negocio.NumeroNegocio;
                        dictInstrumentos[negocio.Instrumento].VolumeNegocios = negocio.Volume;
                        dictInstrumentos[negocio.Instrumento].Preco = negocio.Preco;
                        dictInstrumentos[negocio.Instrumento].Variacao = negocio.Variacao;

                        dictInstrumentos[negocio.Instrumento].QuantidadePapeis += negocio.Quantidade;

                        // Atualiza dicionarios de Corretoras Compradoras mais negociadas do instrumento
                        dict = dictInstrumentos[negocio.Instrumento].DictCompradorasMaisNegociadas;
                        dictPorCorretora = dictInstrumentos[negocio.Instrumento].DictCompradorasMaisNegociadasPorCorretora;
                        AcumulaCorretoras(1, compradora, 1, dict, dictPorCorretora);

                        // Atualiza dicionarios de Corretoras Vendedoras mais negociadas do instrumento
                        dict = dictInstrumentos[negocio.Instrumento].DictVendedorasMaisNegociadas;
                        dictPorCorretora = dictInstrumentos[negocio.Instrumento].DictVendedorasMaisNegociadasPorCorretora;
                        AcumulaCorretoras(1, vendedora, 1, dict, dictPorCorretora);

                        double valor = negocio.Preco * negocio.Quantidade;

                        // Atualiza dicionarios de Corretoras Compradoras com mais volume do instrumento
                        dict = dictInstrumentos[negocio.Instrumento].DictCompradorasMaioresVolumes;
                        dictPorCorretora = dictInstrumentos[negocio.Instrumento].DictCompradorasMaioresVolumesPorCorretora;
                        AcumulaCorretoras(0, compradora, valor, dict, dictPorCorretora);

                        // Atualiza dicionarios de Corretoras Vendedoras com mais volume do instrumento
                        dict = dictInstrumentos[negocio.Instrumento].DictVendedorasMaioresVolumes;
                        dictPorCorretora = dictInstrumentos[negocio.Instrumento].DictVendedorasMaioresVolumesPorCorretora;
                        AcumulaCorretoras(0, vendedora, valor, dict, dictPorCorretora);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("ContabilizaNegocio(): " + ex.Message, ex);
            }
        }

        private static void AcumulaCorretoras(double valorInicial, string corretora, double valor, SortedDictionary<RankingInfo, string> dict, SortedDictionary<string, RankingInfo> dictPorCorretora)
        {
            try
            {
                RankingInfo rankingNovo = new RankingInfo(valorInicial, corretora);
                if (dictPorCorretora.ContainsKey(corretora))
                {
                    RankingInfo rankingAtual = dictPorCorretora[corretora];
                    dict.Remove(rankingAtual);
                    rankingNovo.Valor = rankingAtual.Valor + valor;
                    dictPorCorretora[corretora] = rankingNovo;
                }
                else
                {
                    dictPorCorretora.Add(corretora, rankingNovo);
                }
                dict.Add(rankingNovo, corretora);
            }
            catch (Exception ex)
            {
                logger.Fatal("AcumulaCorretoras(): " + ex.Message, ex);
            }
        }

        private static string DefinirTipoDestaque(string segmentoMercado, string TipoDestaqueEnum)
        {
            string tipoDestaque = "";

            try
            {
                if (segmentoMercado.Equals(SEGMENTOMERCADO_BOVESPA_VISTA))
                    tipoDestaque = ((int)FiltroMercado.BovespaVista).ToString();
                else if (segmentoMercado.Equals(SEGMENTOMERCADO_BOVESPA_TERMO))
                    tipoDestaque = ((int)FiltroMercado.BovespaTermo).ToString();
                else if (segmentoMercado.Equals(SEGMENTOMERCADO_BOVESPA_OPCOES))
                    tipoDestaque = ((int)FiltroMercado.BovespaOpcoes).ToString();
                else if (segmentoMercado.Equals(SEGMENTOMERCADO_BMF_FUTURO))
                    tipoDestaque = ((int)FiltroMercado.BMFFuturo).ToString();
                else if (segmentoMercado.Equals(SEGMENTOMERCADO_BMF_VISTA))
                    tipoDestaque = ((int)FiltroMercado.BMFVista).ToString();
                else if (segmentoMercado.Equals(SEGMENTOMERCADO_BMF_OPCAO_VISTA))
                    tipoDestaque = ((int)FiltroMercado.BMFOpcaoVista).ToString();
                else if (segmentoMercado.Equals(SEGMENTOMERCADO_BMF_OPCAO_FUTURO))
                    tipoDestaque = ((int)FiltroMercado.BMFOpcaoFuturo).ToString();

                tipoDestaque += TipoDestaqueEnum;
            }
            catch (Exception ex)
            {
                logger.Fatal("DefinirTipoDestaque(): " + ex.Message, ex);
            }

            return tipoDestaque;
        }

        public static void ListaRanking(Queue<DadosFilaNegociosDestaque> filaDestaques, Socket socketClient)
        {
            NegociosDestaqueInfo negociosDestaques = null;

            logger.Info("Montando nova lista de destaques e enviando para fila");

            try
            {
                foreach (string segmentoMercado in SEGMENTOS_MERCADO)
                {
                    lock (dictInstrumentos)
                    {
                        lock (dictMaioresAltasBaixasPorInstrumento)
                        {
                            logger.Debug("MAIORES ALTAS:");
                            negociosDestaques = new NegociosDestaqueInfo();
                            negociosDestaques.tipo = DefinirTipoDestaque(segmentoMercado, ((int)TipoDestaqueEnum.MaioresAltas).ToString());
                            negociosDestaques.cabecalho = new CabecalhoInfo();
                            negociosDestaques.cabecalho.tp = negociosDestaques.tipo;
                            negociosDestaques.cabecalho.d = DateTime.Now.ToString("yyyyMMdd");
                            negociosDestaques.cabecalho.h = DateTime.Now.ToString("HHmmssSSS");
                            negociosDestaques.destaque = new List<DestaqueInfo>();

                            foreach (KeyValuePair<RankingInfo, string> info in dictMaioresAltas[segmentoMercado])
                            {
                                // Apenas lista os destaques da mesma data atual
                                if (!((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMdd").Equals(DateTime.Now.ToString("yyyyMMdd")))
                                    break;

                                if (dictInstrumentos[info.Value].NumeroNegocios == 0)
                                    continue;

                                logger.Debug(String.Format("Segmento[{0,-4}] ({1,1}) {2,19:dd/MM/yyyy HH:mm:ss} {3,-14} {4,6}% {5, 12} {6, 13} {7, 13}",
                                    segmentoMercado, negociosDestaques.destaque.Count + 1, dictInstrumentos[info.Value].DataUltimoNegocio, info.Value, info.Key.Valor,
                                    dictInstrumentos[info.Value].QuantidadePapeis, dictInstrumentos[info.Value].Preco,
                                    dictInstrumentos[info.Value].VolumeNegocios));

                                DestaqueInfo destaque = new DestaqueInfo();
                                destaque.dt = ((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMddHHmmss");
                                destaque.i = info.Value;
                                destaque.v = info.Key.Valor.ToString().Replace('.', ',');
                                destaque.qt = dictInstrumentos[info.Value].QuantidadePapeis.ToString().Replace('.', ',');
                                destaque.p = dictInstrumentos[info.Value].Preco.ToString().Replace('.', ',');
                                destaque.vl = dictInstrumentos[info.Value].VolumeNegocios.ToString().Replace('.', ',');
                                destaque.n = dictInstrumentos[info.Value].NumeroNegocios.ToString();
                                destaque.cp = "0";
                                destaque.vd = "0";

                                negociosDestaques.destaque.Add(destaque);

                                if (negociosDestaques.destaque.Count > MAX_ITENS_DESTAQUES)
                                    break;
                            }
                            DadosFilaNegociosDestaque dadosFilaNegociosDestaque = new DadosFilaNegociosDestaque();
                            dadosFilaNegociosDestaque.socketClient = socketClient;
                            dadosFilaNegociosDestaque.negociosDestaqueInfo = negociosDestaques;
                            filaDestaques.Enqueue(dadosFilaNegociosDestaque);

                            logger.Debug("MAIORES BAIXAS:");
                            negociosDestaques = new NegociosDestaqueInfo();
                            negociosDestaques.tipo = DefinirTipoDestaque(segmentoMercado, ((int)TipoDestaqueEnum.MaioresBaixas).ToString());
                            negociosDestaques.cabecalho = new CabecalhoInfo();
                            negociosDestaques.cabecalho.tp = negociosDestaques.tipo;
                            negociosDestaques.cabecalho.d = DateTime.Now.ToString("yyyyMMdd");
                            negociosDestaques.cabecalho.h = DateTime.Now.ToString("HHmmssSSS");
                            negociosDestaques.destaque = new List<DestaqueInfo>();

                            foreach (KeyValuePair<RankingInfo, string> info in dictMaioresBaixas[segmentoMercado])
                            {
                                // Apenas lista os destaques da mesma data atual
                                if (!((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMdd").Equals(DateTime.Now.ToString("yyyyMMdd")))
                                    break;

                                if (dictInstrumentos[info.Value].NumeroNegocios == 0)
                                    continue;

                                logger.Debug(String.Format("Segmento[{0,-4}] ({1,1}) {2,19:dd/MM/yyyy HH:mm:ss} {3,-14} {4,6}% {5, 12} {6, 13} {7, 13}",
                                    segmentoMercado, negociosDestaques.destaque.Count + 1, dictInstrumentos[info.Value].DataUltimoNegocio, info.Value, info.Key.Valor,
                                    dictInstrumentos[info.Value].QuantidadePapeis, dictInstrumentos[info.Value].Preco,
                                    dictInstrumentos[info.Value].VolumeNegocios));

                                DestaqueInfo destaque = new DestaqueInfo();
                                destaque.dt = ((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMddHHmmss");
                                destaque.i = info.Value;
                                destaque.v = info.Key.Valor.ToString().Replace('.', ',');
                                destaque.qt = dictInstrumentos[info.Value].QuantidadePapeis.ToString().Replace('.', ',');
                                destaque.p = dictInstrumentos[info.Value].Preco.ToString().Replace('.', ',');
                                destaque.vl = dictInstrumentos[info.Value].VolumeNegocios.ToString().Replace('.', ',');
                                destaque.n = dictInstrumentos[info.Value].NumeroNegocios.ToString();
                                destaque.cp = "0";
                                destaque.vd = "0";

                                negociosDestaques.destaque.Add(destaque);

                                if (negociosDestaques.destaque.Count > MAX_ITENS_DESTAQUES)
                                    break;
                            }
                            dadosFilaNegociosDestaque = new DadosFilaNegociosDestaque();
                            dadosFilaNegociosDestaque.socketClient = socketClient;
                            dadosFilaNegociosDestaque.negociosDestaqueInfo = negociosDestaques;
                            filaDestaques.Enqueue(dadosFilaNegociosDestaque);
                        }

                        lock (dictMaioresVolumesPorInstrumento)
                        {
                            logger.Debug("MAIORES VOLUMES:");
                            negociosDestaques = new NegociosDestaqueInfo();
                            negociosDestaques.tipo = DefinirTipoDestaque(segmentoMercado, ((int)TipoDestaqueEnum.MaioresVolumes).ToString());
                            negociosDestaques.cabecalho = new CabecalhoInfo();
                            negociosDestaques.cabecalho.tp = negociosDestaques.tipo;
                            negociosDestaques.cabecalho.d = DateTime.Now.ToString("yyyyMMdd");
                            negociosDestaques.cabecalho.h = DateTime.Now.ToString("HHmmssSSS");
                            negociosDestaques.destaque = new List<DestaqueInfo>();

                            foreach (KeyValuePair<RankingInfo, string> info in dictMaioresVolumes[segmentoMercado])
                            {
                                // Apenas lista os destaques da mesma data atual
                                if (!((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMdd").Equals(DateTime.Now.ToString("yyyyMMdd")))
                                    break;

                                if (dictInstrumentos[info.Value].NumeroNegocios == 0)
                                    continue;

                                string compradoraTopo = "";
                                foreach (KeyValuePair<RankingInfo, string> compradora in dictInstrumentos[info.Value].DictCompradorasMaioresVolumes)
                                {
                                    compradoraTopo = compradora.Value;
                                    break;
                                }
                                string vendedoraTopo = "";
                                foreach (KeyValuePair<RankingInfo, string> vendedora in dictInstrumentos[info.Value].DictVendedorasMaioresVolumes)
                                {
                                    vendedoraTopo = vendedora.Value;
                                    break;
                                }

                                logger.Debug(String.Format("Segmento[{0,-4}] ({1,1}) {2,19:dd/MM/yyyy HH:mm:ss} {3,-14} {4,6}% {5, 12} {6, 13} {7, 13} {8,-8} {9,-8}",
                                    segmentoMercado, negociosDestaques.destaque.Count + 1, dictInstrumentos[info.Value].DataUltimoNegocio, info.Value,
                                    dictInstrumentos[info.Value].Variacao, dictInstrumentos[info.Value].QuantidadePapeis, dictInstrumentos[info.Value].Preco,
                                    info.Key.Valor, compradoraTopo, vendedoraTopo));

                                DestaqueInfo destaque = new DestaqueInfo();
                                destaque.dt = ((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMddHHmmss");
                                destaque.i = info.Value;
                                destaque.v = dictInstrumentos[info.Value].Variacao.ToString().Replace('.', ',');
                                destaque.qt = dictInstrumentos[info.Value].QuantidadePapeis.ToString().Replace('.', ',');
                                destaque.p = dictInstrumentos[info.Value].Preco.ToString().Replace('.', ',');
                                destaque.vl = info.Key.Valor.ToString().Replace('.', ',');
                                destaque.n = dictInstrumentos[info.Value].NumeroNegocios.ToString();
                                destaque.cp = (Convert.ToInt32(compradoraTopo)).ToString();
                                destaque.vd = (Convert.ToInt32(vendedoraTopo)).ToString();

                                negociosDestaques.destaque.Add(destaque);

                                if (negociosDestaques.destaque.Count > MAX_ITENS_DESTAQUES)
                                    break;
                            }
                            DadosFilaNegociosDestaque dadosFilaNegociosDestaque = new DadosFilaNegociosDestaque();
                            dadosFilaNegociosDestaque.socketClient = socketClient;
                            dadosFilaNegociosDestaque.negociosDestaqueInfo = negociosDestaques;
                            filaDestaques.Enqueue(dadosFilaNegociosDestaque);
                        }

                        lock (dictMaisNegociadosPorInstrumento)
                        {
                            logger.Debug("MAIS NEGOCIADOS:");
                            negociosDestaques = new NegociosDestaqueInfo();
                            negociosDestaques.tipo = DefinirTipoDestaque(segmentoMercado, ((int)TipoDestaqueEnum.MaisNegociadas).ToString());
                            negociosDestaques.cabecalho = new CabecalhoInfo();
                            negociosDestaques.cabecalho.tp = negociosDestaques.tipo;
                            negociosDestaques.cabecalho.d = DateTime.Now.ToString("yyyyMMdd");
                            negociosDestaques.cabecalho.h = DateTime.Now.ToString("HHmmssSSS");
                            negociosDestaques.destaque = new List<DestaqueInfo>();

                            foreach (KeyValuePair<RankingInfo, string> info in dictMaisNegociados[segmentoMercado])
                            {
                                // Apenas lista os destaques da mesma data atual
                                if (!((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMdd").Equals(DateTime.Now.ToString("yyyyMMdd")))
                                    break;

                                if (dictInstrumentos[info.Value].NumeroNegocios == 0)
                                    continue;

                                string compradoraTopo = "";
                                foreach (KeyValuePair<RankingInfo, string> compradora in dictInstrumentos[info.Value].DictCompradorasMaisNegociadas)
                                {
                                    compradoraTopo = compradora.Value;
                                    break;
                                }
                                string vendedoraTopo = "";
                                foreach (KeyValuePair<RankingInfo, string> vendedora in dictInstrumentos[info.Value].DictVendedorasMaisNegociadas)
                                {
                                    vendedoraTopo = vendedora.Value;
                                    break;
                                }

                                logger.Debug(String.Format("Segmento[{0,-4}] ({1,1}) {2,19:dd/MM/yyyy HH:mm:ss} {3,-14} {4,6}% {5, 12} {6, 13} {7, 13} {8,-8} {9,-8}",
                                    segmentoMercado, negociosDestaques.destaque.Count + 1, dictInstrumentos[info.Value].DataUltimoNegocio, info.Value,
                                    dictInstrumentos[info.Value].Variacao, dictInstrumentos[info.Value].QuantidadePapeis, dictInstrumentos[info.Value].Preco,
                                    info.Key.Valor, compradoraTopo, vendedoraTopo));

                                DestaqueInfo destaque = new DestaqueInfo();
                                destaque.dt = ((DateTime)dictInstrumentos[info.Value].DataUltimoNegocio).ToString("yyyyMMddHHmmss");
                                destaque.i = info.Value;
                                destaque.v = dictInstrumentos[info.Value].Variacao.ToString().Replace('.', ',');
                                destaque.qt = dictInstrumentos[info.Value].QuantidadePapeis.ToString().Replace('.', ',');
                                destaque.p = dictInstrumentos[info.Value].Preco.ToString().Replace('.', ',');
                                destaque.vl = dictInstrumentos[info.Value].VolumeNegocios.ToString().Replace('.', ',');
                                destaque.n = info.Key.Valor.ToString();
                                destaque.cp = (Convert.ToInt32(compradoraTopo)).ToString();
                                destaque.vd = (Convert.ToInt32(vendedoraTopo)).ToString();

                                negociosDestaques.destaque.Add(destaque);

                                if (negociosDestaques.destaque.Count > MAX_ITENS_DESTAQUES)
                                    break;
                            }
                            DadosFilaNegociosDestaque dadosFilaNegociosDestaque = new DadosFilaNegociosDestaque();
                            dadosFilaNegociosDestaque.socketClient = socketClient;
                            dadosFilaNegociosDestaque.negociosDestaqueInfo = negociosDestaques;
                            filaDestaques.Enqueue(dadosFilaNegociosDestaque);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("ListaRanking(): " + ex.Message, ex);
            }
        }

        public static void InicializarDicionarios()
        {
            DCotacoes _DCotacoes = new DCotacoes();

            try
            {
                logger.Info("Carregando lista de instrumentos do Banco de Dados");
                Dictionary<string, InstrumentoInfo> listaInstrumentos = new Dictionary<string, InstrumentoInfo>();
                listaInstrumentos = _DCotacoes.ObterListaInstrumentos();
                logger.Info("Total de instrumentos carregados = " + listaInstrumentos.Count);

                lock (dictInstrumentos)
                {
                    logger.Info("Atualizando lista de instrumentos da memoria");
                    foreach (KeyValuePair<string, InstrumentoInfo> instrumento in listaInstrumentos)
                    {
                        if (dictInstrumentos.ContainsKey(instrumento.Key))
                            dictInstrumentos[instrumento.Key] = instrumento.Value;
                        else
                            dictInstrumentos.Add(instrumento.Key, instrumento.Value);
                    }
                    logger.Info("Lista de instrumentos da memoria atualizada!");
                }

                logger.Info("Inicializando dicionarios");

                // Inicializa todos os dicionarios, ja separando por segmento de mercado
                lock (dictMaioresAltasBaixasPorInstrumento)
                {
                    dictMaioresAltas.Clear();
                    dictMaioresBaixas.Clear();
                    dictMaioresAltasBaixasPorInstrumento.Clear();
                }

                lock (dictMaioresVolumesPorInstrumento)
                {
                    dictMaioresVolumes.Clear();
                    dictMaioresVolumesPorInstrumento.Clear();
                }

                lock (dictMaisNegociadosPorInstrumento)
                {
                    dictMaisNegociados.Clear();
                    dictMaisNegociadosPorInstrumento.Clear();
                }

                foreach (string segmentoMercado in SEGMENTOS_MERCADO)
                {
                    lock (dictMaioresAltasBaixasPorInstrumento)
                    {
                        dictMaioresAltas.Add(segmentoMercado, new SortedDictionary<RankingInfo, string>(new ComparadorDecrescente()));
                        dictMaioresBaixas.Add(segmentoMercado, new SortedDictionary<RankingInfo, string>());
                        dictMaioresAltasBaixasPorInstrumento.Add(segmentoMercado, new SortedDictionary<string, RankingInfo>());
                    }

                    lock (dictMaioresVolumesPorInstrumento)
                    {
                        dictMaioresVolumes.Add(segmentoMercado, new SortedDictionary<RankingInfo, string>(new ComparadorDecrescente()));
                        dictMaioresVolumesPorInstrumento.Add(segmentoMercado, new SortedDictionary<string, RankingInfo>());
                    }

                    lock (dictMaisNegociadosPorInstrumento)
                    {
                        dictMaisNegociados.Add(segmentoMercado, new SortedDictionary<RankingInfo, string>(new ComparadorDecrescente()));
                        dictMaisNegociadosPorInstrumento.Add(segmentoMercado, new SortedDictionary<string, RankingInfo>());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("InicializarDicionarios(): " + ex.Message, ex);
            }
        }

        #endregion
    }
}
