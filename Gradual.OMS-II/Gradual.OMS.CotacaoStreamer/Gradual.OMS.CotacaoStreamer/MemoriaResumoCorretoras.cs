using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Sockets;
using log4net;
using System.Globalization;
using Gradual.OMS.CotacaoStreamer.Dados;

namespace Gradual.OMS.CotacaoStreamer
{
    /// <summary>
    /// Classe responsável por armazenas as estruturas de Ranking de Corretoras e contabilizar o Resumo de Corretoras.   
    /// </summary>

    [Serializable]
    public static class MemoriaResumoCorretoras
    {
        #region Globais

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string PREFIXO_CORRETORA = "#"; 

        private const string SEGMENTOMERCADO_TODOS = "00";
        private const string SEGMENTOMERCADO_BOVESPA_VISTA = "01";
        private const string SEGMENTOMERCADO_BOVESPA_TERMO = "02";
        private const string SEGMENTOMERCADO_BOVESPA_FRACIONARIO = "03";
        private const string SEGMENTOMERCADO_BOVESPA_OPCOES = "04";

        public static string[] SEGMENTOS_MERCADO = { 
            SEGMENTOMERCADO_TODOS,
            SEGMENTOMERCADO_BOVESPA_VISTA,
            SEGMENTOMERCADO_BOVESPA_FRACIONARIO,
            SEGMENTOMERCADO_BOVESPA_OPCOES,
            SEGMENTOMERCADO_BOVESPA_TERMO
        };

        public const string TODAS_CORRETORAS = "00000000";
        public const string TODOS_INSTRUMENTOS = "XXXXXXXX";
        private const int MAX_ITENS_HOME_BROKER = 15;
        private const int COMPRADORA = 1;
        private const int VENDEDORA = 2;

        private const int PESQUISA_POR_SEGMENTO = 0;
        private const int PESQUISA_POR_INSTRUMENTO = 1;
        private const int PESQUISA_POR_CORRETORA = 2;

        private static DateTime dataAtual = DateTime.Now;

        /// <summary>
        /// Dictionary que mantém todos os instrumentos do cadastro básico da Bovespa
        /// </summary>
        private static Dictionary<string, InstrumentoInfo> dictInstrumentos = new Dictionary<string, InstrumentoInfo>();

        /// <summary>
        /// Dictionary que mantém as corretoras que negociaram na Bovespa
        /// </summary>
        private static Dictionary<string, InstrumentosPorCorretoraInfo> dictCorretoras = new Dictionary<string, InstrumentosPorCorretoraInfo>();

        /// <summary>
        /// Dictionaries que mantém os maiores volumes de todas as corretoras da Bovespa
        /// </summary>
        private static Dictionary<string, SortedDictionary<CorretorasInfo, string>> dictMaioresVolumes = new Dictionary<string, SortedDictionary<CorretorasInfo, string>>();
        private static Dictionary<string, SortedDictionary<string, CorretorasInfo>> dictMaioresVolumesPorCorretora = new Dictionary<string, SortedDictionary<string, CorretorasInfo>>();

        /// <summary>
        /// Dictionary que mantém todas as assinaturas de ranking de corretoras por instrumento
        /// </summary>
        private static Dictionary<string, int> dictInstrumentosAssinados = new Dictionary<string, int>();

        /// <summary>
        /// Dictionary que mantém todas as assinaturas de ranking de instrumentos por corretora
        /// </summary>
        private static Dictionary<string, int> dictCorretorasAssinadas = new Dictionary<string, int>();

        #endregion

        #region Métodos Públicos

        public static void AssinarRankingPorInstrumento(string instrumento)
        {
            try
            {
                if (instrumento.StartsWith(PREFIXO_CORRETORA))
                {
                    string corretora = Int32.Parse(instrumento.Substring(1)).ToString("D8");
                    lock (dictCorretorasAssinadas)
                    {
                        if (dictCorretorasAssinadas.ContainsKey(corretora))
                        {
                            dictCorretorasAssinadas[corretora]++;
                            logger.Info("Incrementando assinatura para a corretora[" + corretora + "] existente");
                        }
                        else
                        {
                            dictCorretorasAssinadas.Add(corretora, 1);
                            logger.Info("Criando assinatura para a corretora[" + corretora + "]");
                        }

                        foreach (KeyValuePair<string, int> assinatura in dictCorretorasAssinadas)
                            logger.Debug("Corretora[" + assinatura.Key + "]: " + assinatura.Value + " assinatura(s)");
                    }
                }
                else
                {
                    lock (dictInstrumentosAssinados)
                    {
                        if (dictInstrumentosAssinados.ContainsKey(instrumento))
                        {
                            dictInstrumentosAssinados[instrumento]++;
                            logger.Info("Incrementando assinatura para o instrumento[" + instrumento + "] existente");
                        }
                        else
                        {
                            dictInstrumentosAssinados.Add(instrumento, 1);
                            logger.Info("Criando assinatura para o instrumento[" + instrumento + "]");
                        }

                        foreach (KeyValuePair<string, int> assinatura in dictInstrumentosAssinados)
                            logger.Debug("Instrumento[" + assinatura.Key + "]: " + assinatura.Value + " assinatura(s)");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("AssinarRankingPorInstrumento(): " + ex.Message, ex);
            }
        }

        public static void DesassinarRankingPorInstrumento(string instrumento)
        {
            try
            {
                if (instrumento.StartsWith(PREFIXO_CORRETORA))
                {
                    string corretora = Int32.Parse(instrumento.Substring(1)).ToString("D8");
                    lock (dictCorretorasAssinadas)
                    {
                        if (dictCorretorasAssinadas.ContainsKey(corretora))
                        {
                            if (dictCorretorasAssinadas[corretora] == 1)
                            {
                                dictCorretorasAssinadas.Remove(corretora);
                                logger.Info("Removendo assinatura para o corretora[" + corretora + "]");
                            }
                            else
                            {
                                dictCorretorasAssinadas[corretora]--;
                                logger.Info("Decrementando assinatura para o corretora[" + corretora + "] existente");
                            }
                        }

                        foreach (KeyValuePair<string, int> assinatura in dictCorretorasAssinadas)
                            logger.Debug("Corretora[" + assinatura.Key + "]: " + assinatura.Value + " assinatura(s)");
                    }
                }
                else
                {
                    lock (dictInstrumentosAssinados)
                    {
                        if (dictInstrumentosAssinados.ContainsKey(instrumento))
                        {
                            if (dictInstrumentosAssinados[instrumento] == 1)
                            {
                                dictInstrumentosAssinados.Remove(instrumento);
                                logger.Info("Removendo assinatura para o instrumento[" + instrumento + "]");
                            }
                            else
                            {
                                dictInstrumentosAssinados[instrumento]--;
                                logger.Info("Decrementando assinatura para o instrumento[" + instrumento + "] existente");
                            }
                        }

                        foreach (KeyValuePair<string, int> assinatura in dictInstrumentosAssinados)
                            logger.Debug("Instrumento[" + assinatura.Key + "]: " + assinatura.Value + " assinatura(s)");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("DesassinarRankingPorInstrumento(): " + ex.Message, ex);
            }
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

                    // Contabiliza os instrumentos do segmento de mercado fracionario junto com os instrumentos a vista
                    if (segmentoMercado.Equals(SEGMENTOMERCADO_BOVESPA_FRACIONARIO))
                        segmentoMercado = SEGMENTOMERCADO_BOVESPA_VISTA;

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

                    // Despreza mensagens onde a corretora compradora ou vendedora esta zerada
                    if (negocio.Compradora == 0 || negocio.Vendedora == 0)
                        return;

                    // Despreza mensagens de negocios ja contabilizados
                    if (dictInstrumentos[negocio.Instrumento].NumeroNegocios >= negocio.NumeroNegocio)
                        return;

                    logger.Debug(String.Format("Contabiliza Instrumento[{0,19:dd/MM/yyyy HH:mm:ss} {1,2} {2,-14} {3,-2} {4,8} {5,13} {6,12} {7,6}% {8,8} {9,8}]",
                        negocio.DataHora, negocio.Status, negocio.Instrumento, negocio.TipoBolsa, negocio.NumeroNegocio, negocio.Preco,
                        negocio.Quantidade, negocio.Variacao, negocio.Compradora, negocio.Vendedora));

                    string compradora = negocio.Compradora.ToString("D8");
                    string vendedora = negocio.Vendedora.ToString("D8");
                    double valorNegocio = negocio.Preco * negocio.Quantidade;

                    lock (dictMaioresVolumesPorCorretora)
                    {
                        AtualizarDicionariosSegmentos(COMPRADORA, SEGMENTOMERCADO_TODOS, compradora, valorNegocio);
                        AtualizarDicionariosSegmentos(VENDEDORA, SEGMENTOMERCADO_TODOS, vendedora, valorNegocio);

                        AtualizarDicionariosSegmentos(COMPRADORA, segmentoMercado, compradora, valorNegocio);
                        AtualizarDicionariosSegmentos(VENDEDORA, segmentoMercado, vendedora, valorNegocio);
                    }

                    lock (dictInstrumentos)
                    {
                        dictInstrumentos[negocio.Instrumento].DataUltimoNegocio = negocio.DataHora;
                        dictInstrumentos[negocio.Instrumento].NumeroNegocios = negocio.NumeroNegocio;
                        dictInstrumentos[negocio.Instrumento].VolumeNegocios = negocio.Volume;
                        dictInstrumentos[negocio.Instrumento].Preco = negocio.Preco;
                        dictInstrumentos[negocio.Instrumento].Variacao = negocio.Variacao;
                        dictInstrumentos[negocio.Instrumento].QuantidadePapeis += negocio.Quantidade;

                        AtualizarDicionarioInstrumento(COMPRADORA, compradora, valorNegocio, dictInstrumentos[negocio.Instrumento]);
                        AtualizarDicionarioInstrumento(VENDEDORA, vendedora, valorNegocio, dictInstrumentos[negocio.Instrumento]);
                    }

                    lock (dictCorretoras)
                    {
                        AtualizarDicionarioCorretora(COMPRADORA, compradora, negocio.Instrumento, valorNegocio);
                        AtualizarDicionarioCorretora(VENDEDORA, vendedora, negocio.Instrumento, valorNegocio);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("ContabilizaNegocio(): " + ex.Message, ex);
            }
        }

        private static void AtualizarDicionariosSegmentos(int compradoraVendedora, string segmentoMercado, string corretora, double valorNegocio)
        {
            if (int.Parse(corretora) == 0)
                return;

            if (!dictMaioresVolumesPorCorretora[segmentoMercado].ContainsKey(corretora))
            {
                dictMaioresVolumes[segmentoMercado].Add(new CorretorasInfo(0, corretora, 0, 0, 0), corretora);
                dictMaioresVolumesPorCorretora[segmentoMercado].Add(corretora, new CorretorasInfo(0, corretora, 0, 0, 0));
            }

            CorretorasInfo rankingAtual = dictMaioresVolumesPorCorretora[segmentoMercado][corretora];
            CorretorasInfo rankingNovo = new CorretorasInfo();

            if (compradoraVendedora == COMPRADORA)
            {
                dictMaioresVolumesPorCorretora[segmentoMercado][TODAS_CORRETORAS].VolumeCompra += valorNegocio;
                rankingNovo.VolumeCompra = rankingAtual.VolumeCompra + valorNegocio;
                rankingNovo.VolumeVenda = rankingAtual.VolumeVenda;
            }
            else
            {
                dictMaioresVolumesPorCorretora[segmentoMercado][TODAS_CORRETORAS].VolumeVenda += valorNegocio;
                rankingNovo.VolumeCompra = rankingAtual.VolumeCompra;
                rankingNovo.VolumeVenda = rankingAtual.VolumeVenda + valorNegocio;
            }
            rankingNovo.Corretora = corretora;
            rankingNovo.VolumeBruto = rankingNovo.VolumeCompra + rankingNovo.VolumeVenda;
            rankingNovo.VolumeLiquido = rankingNovo.VolumeCompra - rankingNovo.VolumeVenda;

            dictMaioresVolumes[segmentoMercado].Remove(rankingAtual);
            dictMaioresVolumes[segmentoMercado].Add(rankingNovo, corretora);
            dictMaioresVolumesPorCorretora[segmentoMercado][corretora] = rankingNovo;
        }

        private static void AtualizarDicionarioCorretora(int compradoraVendedora, string corretora, string instrumento, double valorNegocio)
        {
            if (!dictCorretoras.ContainsKey(corretora))
            {
                dictCorretoras.Add(corretora, new InstrumentosPorCorretoraInfo());
                dictCorretoras[corretora].DictMaioresVolumes = new SortedDictionary<CorretorasInfo, string>(new ComparadorDecrescenteCorretoras());
                dictCorretoras[corretora].DictMaioresVolumesPorInstrumento = new SortedDictionary<string, CorretorasInfo>();

                dictCorretoras[corretora].DictMaioresVolumesPorInstrumento.Add(TODOS_INSTRUMENTOS, new CorretorasInfo(0, TODOS_INSTRUMENTOS, 0, 0, 0));
            }

            if (!dictCorretoras[corretora].DictMaioresVolumesPorInstrumento.ContainsKey(instrumento))
            {
                dictCorretoras[corretora].DictMaioresVolumes.Add(new CorretorasInfo(0, instrumento, 0, 0, 0), instrumento);
                dictCorretoras[corretora].DictMaioresVolumesPorInstrumento.Add(instrumento, new CorretorasInfo(0, instrumento, 0, 0, 0));
            }

            CorretorasInfo corretoraAtual = dictCorretoras[corretora].DictMaioresVolumesPorInstrumento[instrumento];
            CorretorasInfo corretoraNovo = new CorretorasInfo();

            if (compradoraVendedora == COMPRADORA)
            {
                corretoraNovo.VolumeCompra = corretoraAtual.VolumeCompra + valorNegocio;
                corretoraNovo.VolumeVenda = corretoraAtual.VolumeVenda;
                dictCorretoras[corretora].DictMaioresVolumesPorInstrumento[TODOS_INSTRUMENTOS].VolumeCompra += valorNegocio;
            }
            else
            {
                corretoraNovo.VolumeVenda = corretoraAtual.VolumeVenda + valorNegocio;
                corretoraNovo.VolumeCompra = corretoraAtual.VolumeCompra;
                dictCorretoras[corretora].DictMaioresVolumesPorInstrumento[TODOS_INSTRUMENTOS].VolumeVenda += valorNegocio;
            }

            corretoraNovo.Corretora = instrumento;
            corretoraNovo.VolumeBruto = corretoraNovo.VolumeCompra + corretoraNovo.VolumeVenda;
            corretoraNovo.VolumeLiquido = corretoraNovo.VolumeCompra - corretoraNovo.VolumeVenda;

            dictCorretoras[corretora].DictMaioresVolumes.Remove(corretoraAtual);
            dictCorretoras[corretora].DictMaioresVolumes.Add(corretoraNovo, instrumento);
            dictCorretoras[corretora].DictMaioresVolumesPorInstrumento[instrumento] = corretoraNovo;
        }

        private static void AtualizarDicionarioInstrumento(int compradoraVendedora, string corretora, double valorNegocio, InstrumentoInfo dadosInstrumento)
        {
            if (int.Parse(corretora) == 0)
                return;

            if (!dadosInstrumento.DictMaioresVolumesPorCorretora.ContainsKey(corretora))
            {
                dadosInstrumento.DictMaioresVolumes.Add(new CorretorasInfo(0, corretora, 0, 0, 0), corretora);
                dadosInstrumento.DictMaioresVolumesPorCorretora.Add(corretora, new CorretorasInfo(0, corretora, 0, 0, 0));
            }

            CorretorasInfo rankingAtual = dadosInstrumento.DictMaioresVolumesPorCorretora[corretora];
            CorretorasInfo rankingNovo = new CorretorasInfo();

            if (compradoraVendedora == COMPRADORA)
            {
                dadosInstrumento.DictMaioresVolumesPorCorretora[TODAS_CORRETORAS].VolumeCompra += valorNegocio;

                rankingNovo.VolumeCompra = rankingAtual.VolumeCompra + valorNegocio;
                rankingNovo.VolumeVenda = rankingAtual.VolumeVenda;
            }
            else
            {
                dadosInstrumento.DictMaioresVolumesPorCorretora[TODAS_CORRETORAS].VolumeVenda += valorNegocio;

                rankingNovo.VolumeCompra = rankingAtual.VolumeCompra;
                rankingNovo.VolumeVenda = rankingAtual.VolumeVenda + valorNegocio;
            }
            rankingNovo.Corretora = corretora;
            rankingNovo.VolumeBruto = rankingNovo.VolumeCompra + rankingNovo.VolumeVenda;
            rankingNovo.VolumeLiquido = rankingNovo.VolumeCompra - rankingNovo.VolumeVenda;

            dadosInstrumento.DictMaioresVolumes.Remove(rankingAtual);
            dadosInstrumento.DictMaioresVolumes.Add(rankingNovo, corretora);
            dadosInstrumento.DictMaioresVolumesPorCorretora[corretora] = rankingNovo;
        }

        public static string ReceberResumoCorretoras(string instrumento)
        {
            int tipoPesquisa;

            if (((IList<string>)SEGMENTOS_MERCADO).Contains(instrumento))
                tipoPesquisa = PESQUISA_POR_SEGMENTO;
            else if (instrumento.StartsWith(PREFIXO_CORRETORA))
                tipoPesquisa = PESQUISA_POR_CORRETORA;
            else
                tipoPesquisa = PESQUISA_POR_INSTRUMENTO;

            // Monta mensagem de Resumo de Corretoras
            string mensagem = "RE" + DateTime.Now.ToString("yyyyMMddHHmmss");

            int ocorrencias = 0;

            try
            {
                mensagem += String.Format("{0,-20}", instrumento);

                string listaOcorrencias = "";

                switch (tipoPesquisa)
                {
                    case PESQUISA_POR_SEGMENTO:
                        lock (dictInstrumentos)
                        {
                            lock (dictMaioresVolumesPorCorretora)
                            {
                                logger.Info("HomeBroker solicitando ResumoCorretoras segmentoMercado[" + instrumento + "]");
                                foreach (KeyValuePair<CorretorasInfo, string> info in dictMaioresVolumes[instrumento])
                                {
                                    double porcentagemCompra = (info.Key.VolumeCompra / dictMaioresVolumesPorCorretora[instrumento][TODAS_CORRETORAS].VolumeCompra) * 100;
                                    double porcentagemVenda = (info.Key.VolumeVenda / dictMaioresVolumesPorCorretora[instrumento][TODAS_CORRETORAS].VolumeVenda) * 100;

                                    listaOcorrencias += String.Format("{0:00}", ocorrencias + 1);
                                    listaOcorrencias += info.Value;
                                    listaOcorrencias += String.Format("{0:0000000000000}", info.Key.VolumeCompra);
                                    listaOcorrencias += String.Format("{0:00000.00}", porcentagemCompra).Replace('.', ',');
                                    listaOcorrencias += String.Format("{0:0000000000000}", info.Key.VolumeVenda);
                                    listaOcorrencias += String.Format("{0:00000.00}", porcentagemVenda).Replace('.', ',');
                                    listaOcorrencias += String.Format("{0:0000000000000}", info.Key.VolumeBruto);
                                    listaOcorrencias += String.Format("{0: 0000000000000;-0000000000000}", info.Key.VolumeLiquido);

                                    logger.Debug(String.Format("Segmento[{0,-2}] ({1,3}) {2,-9} {3,15} {4,8}% {5,15} {6,8}% {7,15} {8,15}",
                                        instrumento, ocorrencias + 1, info.Value,
                                        info.Key.VolumeCompra.ToString("N2"), porcentagemCompra.ToString("N2"),
                                        info.Key.VolumeVenda.ToString("N2"), porcentagemVenda.ToString("N2"),
                                        info.Key.VolumeBruto.ToString("N2"), info.Key.VolumeLiquido.ToString("N2")));

                                    if (++ocorrencias >= MAX_ITENS_HOME_BROKER)
                                        break;
                                }
                            }
                        }
                        break;

                    case PESQUISA_POR_INSTRUMENTO:
                        lock (dictInstrumentos)
                        {
                            logger.Info("HomeBroker solicitando ResumoCorretoras instrumento[" + instrumento + "]");
                            if (!dictInstrumentos.ContainsKey(instrumento))
                            {
                                logger.Error("HomeBroker instrumento[" + instrumento + "] não encontrado!");
                            }
                            else
                            {
                                foreach (KeyValuePair<CorretorasInfo, string> info in dictInstrumentos[instrumento].DictMaioresVolumes)
                                {
                                    double porcentagemCompra = (info.Key.VolumeCompra / dictInstrumentos[instrumento].DictMaioresVolumesPorCorretora[TODAS_CORRETORAS].VolumeCompra) * 100;
                                    double porcentagemVenda = (info.Key.VolumeVenda / dictInstrumentos[instrumento].DictMaioresVolumesPorCorretora[TODAS_CORRETORAS].VolumeVenda) * 100;

                                    listaOcorrencias += String.Format("{0:00}", ocorrencias + 1);
                                    listaOcorrencias += info.Value;
                                    listaOcorrencias += String.Format("{0:0000000000000}", info.Key.VolumeCompra);
                                    listaOcorrencias += String.Format("{0:00000.00}", porcentagemCompra).Replace('.', ',');
                                    listaOcorrencias += String.Format("{0:0000000000000}", info.Key.VolumeVenda);
                                    listaOcorrencias += String.Format("{0:00000.00}", porcentagemVenda).Replace('.', ',');
                                    listaOcorrencias += String.Format("{0:0000000000000}", info.Key.VolumeBruto);
                                    listaOcorrencias += String.Format("{0: 0000000000000;-0000000000000}", info.Key.VolumeLiquido);


                                    logger.Debug(String.Format("Instrumento[{0,-14}] ({1,3}) {2,-9} {3,15} {4,8}% {5,15} {6,8}% {7,15} {8,15}",
                                        instrumento, ocorrencias + 1, info.Value,
                                        info.Key.VolumeCompra.ToString("N2"), porcentagemCompra.ToString("N2"),
                                        info.Key.VolumeVenda.ToString("N2"), porcentagemVenda.ToString("N2"),
                                        info.Key.VolumeBruto.ToString("N2"), info.Key.VolumeLiquido.ToString("N2")));

                                    if (++ocorrencias >= MAX_ITENS_HOME_BROKER)
                                        break;
                                }
                            }
                        }
                        break;

                    case PESQUISA_POR_CORRETORA:
                        lock (dictCorretoras)
                        {
                            string corretora = Int32.Parse(instrumento.Substring(1)).ToString("D8");
                            logger.Info("HomeBroker solicitando ResumoCorretoras corretora[" + corretora + "]");
                            if (!dictCorretoras.ContainsKey(corretora))
                            {
                                logger.Error("HomeBroker corretora[" + corretora + "] não encontrada!");
                            }
                            else
                            {
                                foreach (KeyValuePair<CorretorasInfo, string> info in dictCorretoras[corretora].DictMaioresVolumes)
                                {
                                    double porcentagemCompra = (info.Key.VolumeCompra / dictCorretoras[corretora].DictMaioresVolumesPorInstrumento[TODOS_INSTRUMENTOS].VolumeCompra) * 100;
                                    double porcentagemVenda = (info.Key.VolumeVenda / dictCorretoras[corretora].DictMaioresVolumesPorInstrumento[TODOS_INSTRUMENTOS].VolumeVenda) * 100;

                                    listaOcorrencias += String.Format("{0:00}", ocorrencias + 1);
                                    listaOcorrencias += String.Format("{0,-20}", info.Value);
                                    listaOcorrencias += String.Format("{0:0000000000000}", info.Key.VolumeCompra);
                                    listaOcorrencias += String.Format("{0:00000.00}", porcentagemCompra).Replace('.', ',');
                                    listaOcorrencias += String.Format("{0:0000000000000}", info.Key.VolumeVenda);
                                    listaOcorrencias += String.Format("{0:00000.00}", porcentagemVenda).Replace('.', ',');
                                    listaOcorrencias += String.Format("{0:0000000000000}", info.Key.VolumeBruto);
                                    listaOcorrencias += String.Format("{0: 0000000000000;-0000000000000}", info.Key.VolumeLiquido);

                                    logger.Debug(String.Format("Corretora[{0,-14}] ({1,3}) {2,-20} {3,15} {4,8}% {5,15} {6,8}% {7,15} {8,15}",
                                        corretora, ocorrencias + 1, info.Value,
                                        info.Key.VolumeCompra.ToString("N2"), porcentagemCompra.ToString("N2"),
                                        info.Key.VolumeVenda.ToString("N2"), porcentagemVenda.ToString("N2"),
                                        info.Key.VolumeBruto.ToString("N2"), info.Key.VolumeLiquido.ToString("N2")));

                                    if (++ocorrencias >= MAX_ITENS_HOME_BROKER)
                                        break;
                                }
                            }
                        }
                        break;
                }

                mensagem += ocorrencias.ToString("D2");
                mensagem += listaOcorrencias;
            }
            catch (Exception ex)
            {
                logger.Fatal("ReceberResumoCorretoras(): " + ex.Message, ex);
            }

            return mensagem;
        }

        public static void ListaRanking(Queue<DadosFilaResumoCorretoras> fila, Socket socketClient)
        {
            ResumoCorretorasInfo resumoCorretoras = null;

            logger.Info("Montando nova lista de resumo de corretoras e enviando para fila");

            try
            {
                foreach (string segmentoMercado in SEGMENTOS_MERCADO)
                {
                    lock (dictInstrumentos)
                    {
                        lock (dictMaioresVolumesPorCorretora)
                        {
                            resumoCorretoras = new ResumoCorretorasInfo();
                            resumoCorretoras.instrumento = segmentoMercado;
                            resumoCorretoras.cabecalho = new CabecalhoInfo();
                            resumoCorretoras.cabecalho.tp = resumoCorretoras.instrumento;
                            resumoCorretoras.cabecalho.d = DateTime.Now.ToString("yyyyMMdd");
                            resumoCorretoras.cabecalho.h = DateTime.Now.ToString("HHmmssSSS");
                            resumoCorretoras.resumo = new List<CorretoraInfo>();

                            foreach (KeyValuePair<CorretorasInfo, string> info in dictMaioresVolumes[segmentoMercado])
                            {
                                double porcentagemCompra = (info.Key.VolumeCompra / dictMaioresVolumesPorCorretora[segmentoMercado][TODAS_CORRETORAS].VolumeCompra) * 100;
                                double porcentagemVenda = (info.Key.VolumeVenda / dictMaioresVolumesPorCorretora[segmentoMercado][TODAS_CORRETORAS].VolumeVenda) * 100;

                                CorretoraInfo corretora = new CorretoraInfo();
                                corretora.r = (resumoCorretoras.resumo.Count + 1).ToString();
                                corretora.c = info.Value;
                                corretora.vc = info.Key.VolumeCompra.ToString("F0").Replace('.', ',');
                                corretora.pc = porcentagemCompra.ToString("F2").Replace('.', ',');
                                corretora.vv = info.Key.VolumeVenda.ToString("F0").Replace('.', ',');
                                corretora.pv = porcentagemVenda.ToString("F2").Replace('.', ',');
                                corretora.vb = info.Key.VolumeBruto.ToString("F0").Replace('.', ',');
                                corretora.vl = info.Key.VolumeLiquido.ToString("F0").Replace('.', ',');
                                resumoCorretoras.resumo.Add(corretora);

                                logger.Debug(String.Format("Segmento[{0,-2}] ({1,3}) {2,-9} {3,15} {4,8}% {5,15} {6,8}% {7,15} {8,15}",
                                    segmentoMercado, resumoCorretoras.resumo.Count + 1, info.Value,
                                    info.Key.VolumeCompra.ToString("N2"), porcentagemCompra.ToString("N2"),
                                    info.Key.VolumeVenda.ToString("N2"), porcentagemVenda.ToString("N2"),
                                    info.Key.VolumeBruto.ToString("N2"), info.Key.VolumeLiquido.ToString("N2")));
                            }
                            DadosFilaResumoCorretoras dadosFilaResumoCorretoras = new DadosFilaResumoCorretoras();
                            dadosFilaResumoCorretoras.socketClient = socketClient;
                            dadosFilaResumoCorretoras.resumoCorretorasInfo = resumoCorretoras;
                            fila.Enqueue(dadosFilaResumoCorretoras);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("ListaRanking(): " + ex.Message, ex);
            }
        }

        public static void ListaRankingPorInstrumentoOuCorretora(Queue<DadosFilaResumoCorretoras> fila, Socket socketClient, string instrumento)
        {
            int tipoPesquisa = PESQUISA_POR_INSTRUMENTO;

            if (instrumento != null)
            {
                if (!instrumento.StartsWith(PREFIXO_CORRETORA))
                    tipoPesquisa = PESQUISA_POR_INSTRUMENTO;
                else
                    tipoPesquisa = PESQUISA_POR_CORRETORA;
            }

            ResumoCorretorasInfo resumoCorretoras = null;
            try
            {
                if (instrumento == null || tipoPesquisa == PESQUISA_POR_INSTRUMENTO)
                {
                    lock (dictInstrumentosAssinados)
                    {
                        foreach (KeyValuePair<string, int> assinatura in dictInstrumentosAssinados)
                        {
                            lock (dictInstrumentos)
                            {
                                if (instrumento == null || assinatura.Key.Equals(instrumento))
                                {
                                    resumoCorretoras = new ResumoCorretorasInfo();
                                    resumoCorretoras.instrumento = assinatura.Key;
                                    resumoCorretoras.cabecalho = new CabecalhoInfo();
                                    resumoCorretoras.cabecalho.tp = assinatura.Key;
                                    resumoCorretoras.cabecalho.d = DateTime.Now.ToString("yyyyMMdd");
                                    resumoCorretoras.cabecalho.h = DateTime.Now.ToString("HHmmssSSS");
                                    resumoCorretoras.resumo = new List<CorretoraInfo>();

                                    foreach (KeyValuePair<CorretorasInfo, string> info in dictInstrumentos[assinatura.Key].DictMaioresVolumes)
                                    {
                                        double porcentagemCompra = (info.Key.VolumeCompra / dictInstrumentos[assinatura.Key].DictMaioresVolumesPorCorretora[TODAS_CORRETORAS].VolumeCompra) * 100;
                                        double porcentagemVenda = (info.Key.VolumeVenda / dictInstrumentos[assinatura.Key].DictMaioresVolumesPorCorretora[TODAS_CORRETORAS].VolumeVenda) * 100;

                                        CorretoraInfo corretora = new CorretoraInfo();
                                        corretora.r = (resumoCorretoras.resumo.Count + 1).ToString();
                                        corretora.c = info.Value;
                                        corretora.vc = info.Key.VolumeCompra.ToString("F0").Replace('.', ',');
                                        corretora.pc = porcentagemCompra.ToString("F2").Replace('.', ',');
                                        corretora.vv = info.Key.VolumeVenda.ToString("F0").Replace('.', ',');
                                        corretora.pv = porcentagemVenda.ToString("F2").Replace('.', ',');
                                        corretora.vb = info.Key.VolumeBruto.ToString("F0").Replace('.', ',');
                                        corretora.vl = info.Key.VolumeLiquido.ToString("F0").Replace('.', ',');
                                        resumoCorretoras.resumo.Add(corretora);

                                        logger.Debug(String.Format("Instrumento[{0,-14}] ({1,3}) {2,-9} {3,15} {4,8}% {5,15} {6,8}% {7,15} {8,15}",
                                            assinatura.Key, resumoCorretoras.resumo.Count + 1, info.Value,
                                            info.Key.VolumeCompra.ToString("N2"), porcentagemCompra.ToString("N2"),
                                            info.Key.VolumeVenda.ToString("N2"), porcentagemVenda.ToString("N2"),
                                            info.Key.VolumeBruto.ToString("N2"), info.Key.VolumeLiquido.ToString("N2")));
                                    }
                                    DadosFilaResumoCorretoras dadosFilaResumoCorretoras = new DadosFilaResumoCorretoras();
                                    dadosFilaResumoCorretoras.socketClient = socketClient;
                                    dadosFilaResumoCorretoras.resumoCorretorasInfo = resumoCorretoras;
                                    fila.Enqueue(dadosFilaResumoCorretoras);
                                }
                            }
                        }
                    }
                }

                if (instrumento == null || tipoPesquisa == PESQUISA_POR_CORRETORA)
                {
                    string corretora = null;
                    if (instrumento != null)
                        corretora = Int32.Parse(instrumento.Substring(1)).ToString("D8");

                    lock (dictCorretorasAssinadas)
                    {
                        foreach (KeyValuePair<string, int> assinatura in dictCorretorasAssinadas)
                        {
                            lock (dictCorretoras)
                            {
                                if (instrumento == null || assinatura.Key.Equals(corretora))
                                {
                                    resumoCorretoras = new ResumoCorretorasInfo();
                                    resumoCorretoras.instrumento = PREFIXO_CORRETORA + Int32.Parse(assinatura.Key).ToString();
                                    resumoCorretoras.cabecalho = new CabecalhoInfo();
                                    resumoCorretoras.cabecalho.tp = PREFIXO_CORRETORA + Int32.Parse(assinatura.Key).ToString();
                                    resumoCorretoras.cabecalho.d = DateTime.Now.ToString("yyyyMMdd");
                                    resumoCorretoras.cabecalho.h = DateTime.Now.ToString("HHmmssSSS");
                                    resumoCorretoras.resumo = new List<CorretoraInfo>();

                                    if (dictCorretoras.ContainsKey(assinatura.Key))
                                    {
                                        foreach (KeyValuePair<CorretorasInfo, string> info in dictCorretoras[assinatura.Key].DictMaioresVolumes)
                                        {
                                            double volumeCompraCorretora = dictCorretoras[assinatura.Key].DictMaioresVolumesPorInstrumento[TODOS_INSTRUMENTOS].VolumeCompra;
                                            double volumeVendaCorretora = dictCorretoras[assinatura.Key].DictMaioresVolumesPorInstrumento[TODOS_INSTRUMENTOS].VolumeVenda;
                                            double porcentagemCompra = (volumeCompraCorretora == 0 ? 0 : (info.Key.VolumeCompra / volumeCompraCorretora) * 100);
                                            double porcentagemVenda = (volumeVendaCorretora == 0 ? 0 : (info.Key.VolumeVenda / volumeVendaCorretora) * 100);

                                            CorretoraInfo intrumentoDados = new CorretoraInfo();
                                            intrumentoDados.r = (resumoCorretoras.resumo.Count + 1).ToString();
                                            intrumentoDados.c = info.Value;
                                            intrumentoDados.vc = info.Key.VolumeCompra.ToString("F0").Replace('.', ',');
                                            intrumentoDados.pc = porcentagemCompra.ToString("F2").Replace('.', ',');
                                            intrumentoDados.vv = info.Key.VolumeVenda.ToString("F0").Replace('.', ',');
                                            intrumentoDados.pv = porcentagemVenda.ToString("F2").Replace('.', ',');
                                            intrumentoDados.vb = info.Key.VolumeBruto.ToString("F0").Replace('.', ',');
                                            intrumentoDados.vl = info.Key.VolumeLiquido.ToString("F0").Replace('.', ',');
                                            resumoCorretoras.resumo.Add(intrumentoDados);

                                            logger.Debug(String.Format("Corretora[{0,-9}] ({1,3}) {2,-20} {3,15} {4,8}% {5,15} {6,8}% {7,15} {8,15}",
                                                assinatura.Key, resumoCorretoras.resumo.Count + 1, info.Value,
                                                info.Key.VolumeCompra.ToString("N2"), porcentagemCompra.ToString("N2"),
                                                info.Key.VolumeVenda.ToString("N2"), porcentagemVenda.ToString("N2"),
                                                info.Key.VolumeBruto.ToString("N2"), info.Key.VolumeLiquido.ToString("N2")));
                                        }
                                    }
                                    DadosFilaResumoCorretoras dadosFilaResumoCorretoras = new DadosFilaResumoCorretoras();
                                    dadosFilaResumoCorretoras.socketClient = socketClient;
                                    dadosFilaResumoCorretoras.resumoCorretorasInfo = resumoCorretoras;
                                    fila.Enqueue(dadosFilaResumoCorretoras);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("ListaRankingPorInstrumento(): " + ex.Message, ex);
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
                lock (dictMaioresVolumesPorCorretora)
                {
                    dictMaioresVolumes.Clear();
                    dictMaioresVolumesPorCorretora.Clear();
                }

                foreach (string segmentoMercado in SEGMENTOS_MERCADO)
                {
                    lock (dictMaioresVolumesPorCorretora)
                    {
                        dictMaioresVolumes.Add(segmentoMercado, new SortedDictionary<CorretorasInfo, string>(new ComparadorDecrescenteCorretoras()));
                        dictMaioresVolumesPorCorretora.Add(segmentoMercado, new SortedDictionary<string, CorretorasInfo>());
                        dictMaioresVolumesPorCorretora[segmentoMercado].Add(TODAS_CORRETORAS, new CorretorasInfo(0, TODAS_CORRETORAS, 0, 0, 0));
                    }
                }

                // Inicializa o dicionario de corretoras
                dictCorretoras.Clear();
            }
            catch (Exception ex)
            {
                logger.Fatal("InicializarDicionarios(): " + ex.Message, ex);
            }
        }

        #endregion
    }
}

