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
    /// Classe responsável por armazenas as estruturas de Negocios de Abertura, para acompanhamento dos negocios em leilão.
    /// </summary>

    [Serializable]
    public static class MemoriaAcompanhamentoLeilao
    {
        #region Globais

        private const int MAX_ITENS_HOME_BROKER = 9999;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string DESCRICAO_ACOMPANHAMENTO_LEILAO = "LEILAO";

        private static DadosFilaAcompanhamentoLeilao gDadosFilaAcompanhamentoLeilao = new DadosFilaAcompanhamentoLeilao();
        private static DateTime gUltimaAtualizacaoLeilao = DateTime.Now;
        private static DateTime gUltimaMensagemEnviada = DateTime.Now;
        private static DateTime dataAtual = DateTime.Now;

        /// <summary>
        /// Dictionary que mantém os instrumentos em leilão da Bovespa e BM&F
        /// </summary>
        private static SortedDictionary<string, NegocioInfo> dictInstrumentosLeilao = new SortedDictionary<string, NegocioInfo>();

        #endregion

        #region Métodos Públicos

        public static void AcompanhamentoLeilao(NegocioInfo negocio, Queue<DadosFilaAcompanhamentoLeilao> filaLeilao)
        {
            try
            {
                // Contabiliza apenas as mensagens na data atual
                if (!DateTime.Now.ToString("yyyyMMdd").Equals(negocio.DataHora.ToString("yyyyMMdd")))
                    return;

                // Se mensagem não for de leilão, remove instrumento do acompanhamento de leilao e envia lista atualizada para o Streamer
                if (negocio.Status != 1)
                {
                    if (dictInstrumentosLeilao.ContainsKey(negocio.Instrumento))
                    {
                        logger.Info("Remove instrumento[" + negocio.Instrumento + "] do acompanhamento de leilao");
                        dictInstrumentosLeilao.Remove(negocio.Instrumento);
                        MontagemAcompanhamentoLeilaoStreamer(null);
                    }
                    return;
                }

                if (negocio.PrecoTeoricoAbertura > 0)
                {
                    if (dictInstrumentosLeilao.ContainsKey(negocio.Instrumento))
                    {
                        dictInstrumentosLeilao[negocio.Instrumento] = negocio;
                        logger.Info("Atualiza instrumento[" + negocio.Instrumento + "] do acompanhamento de leilao");
                    }
                    else
                    {
                        dictInstrumentosLeilao.Add(negocio.Instrumento, negocio);
                        logger.Info("Adiciona instrumento[" + negocio.Instrumento + "] do acompanhamento de leilao");
                    }
                    MontagemAcompanhamentoLeilaoStreamer(null);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("AcompanhamentoLeilao(): " + ex.Message, ex);
            }
        }

        public static void MontagemAcompanhamentoLeilaoStreamer(Socket socketClient)
        {
            try
            {
                AcompanhamentoLeilaoInfo acompanhamentoLeilao = new AcompanhamentoLeilaoInfo();
                acompanhamentoLeilao.instrumento = DESCRICAO_ACOMPANHAMENTO_LEILAO;
                acompanhamentoLeilao.cabecalho = new CabecalhoInfo();
                acompanhamentoLeilao.cabecalho.tp = DESCRICAO_ACOMPANHAMENTO_LEILAO;
                acompanhamentoLeilao.cabecalho.d = DateTime.Now.ToString("yyyyMMdd");
                acompanhamentoLeilao.cabecalho.h = DateTime.Now.ToString("HHmmss") + "000";
                acompanhamentoLeilao.negocio = new List<LeilaoInfo>();

                foreach (KeyValuePair<string, NegocioInfo> info in dictInstrumentosLeilao)
                {
                    LeilaoInfo leilao = new LeilaoInfo();
                    leilao.dt = info.Value.HorarioTeorico.ToString("yyyyMMddHHmmss");
                    leilao.tb = info.Value.TipoBolsa;
                    leilao.i = info.Key;
                    leilao.p = info.Value.PrecoTeoricoAbertura.ToString().Replace('.', ',');
                    leilao.qt = info.Value.Quantidade.ToString().Replace('.', ',');
                    leilao.v = info.Value.VariacaoTeorica.ToString().Replace('.', ',');
                    acompanhamentoLeilao.negocio.Add(leilao);
                }

                lock (gDadosFilaAcompanhamentoLeilao)
                {
                    gDadosFilaAcompanhamentoLeilao.socketClient = socketClient;
                    gDadosFilaAcompanhamentoLeilao.acompanhamentoLeilaoInfo = acompanhamentoLeilao;
                    gUltimaAtualizacaoLeilao = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("MontagemAcompanhamentoLeilaoStreamer(): " + ex.Message, ex);
            }
        }

        public static void EnviaAcompanhamentoLeilaoStreamer(Queue<DadosFilaAcompanhamentoLeilao> filaLeilao, int intervaloEnvioStreamer)
        {
            if (gUltimaMensagemEnviada.Equals(gUltimaAtualizacaoLeilao))
                return;

            try
            {
                lock (gDadosFilaAcompanhamentoLeilao)
                {
                    logger.Info("Enviando Acompanhamento de leilao para Streamer: " + gDadosFilaAcompanhamentoLeilao.acompanhamentoLeilaoInfo.negocio.Count + " itens");
                    filaLeilao.Enqueue(gDadosFilaAcompanhamentoLeilao);
                    gUltimaMensagemEnviada = gUltimaAtualizacaoLeilao;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("EnviaAcompanhamentoLeilaoStreamer(): " + ex.Message, ex);
            }
        }

        public static string ReceberAcompanhamentoLeilao()
        {
            logger.Info("HomeBroker solicitando AcompanhamentoLeilao");

            // Monta mensagem de Acompanhamento de Leilao
            string mensagem = "LE" + DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                lock (dictInstrumentosLeilao)
                {
                    int ocorrencias = 0;
                    string listaOcorrencias = "";
                    foreach (KeyValuePair<string, NegocioInfo> info in dictInstrumentosLeilao)
                    {
                        listaOcorrencias += String.Format("{0,-20}", info.Key);
                        listaOcorrencias += info.Value.HorarioTeorico.ToString("yyyyMMddHHmmss");
                        listaOcorrencias += String.Format("{0,-2}", info.Value.TipoBolsa);
                        listaOcorrencias += String.Format("{0:0000000000.00}", info.Value.PrecoTeoricoAbertura).Replace('.', ',');
                        listaOcorrencias += String.Format("{0:000000000000}", info.Value.Quantidade);
                        listaOcorrencias += String.Format("{0: 00000.00;-00000.00}", info.Value.VariacaoTeorica).Replace('.', ',');

                        if (++ocorrencias >= MAX_ITENS_HOME_BROKER)
                            break;
                    }
                    mensagem += ocorrencias.ToString("D4");
                    mensagem += listaOcorrencias;

                    logger.Debug("HomeBroker enviando AcompanhamentoLeilao com " + ocorrencias + " ocorrencias");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("ReceberAcompanhamentoLeilao(): " + ex.Message, ex);
            }

            return mensagem;
        }

        #endregion
    }
}
