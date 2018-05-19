using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using log4net;
using System.Globalization;
using System.Threading;

namespace Gradual.OMS.Cotacao
{
    /// <summary>
    /// Classe responsável por armazenas as estruturas de Livro de Oferta e Mensagem de Negocios para o sistema Homebroker.   
    /// </summary>
    
    [Serializable]
    public class MemoriaCotacaoDelay
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Propriedades (membros públicos)

        public static int NumeroItemHash
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["NumeroItemsHash"]);
            }
        }

        public class MessageItem
        {
            public string Instrumento { get; set; }
            public string MessageBody { get; set; }
        }

        public bool DelayTickerOn { get; set; }
        public int DelayTickerAmount { get; set; }

        /// <summary>
        /// HashTable que armazena todas as mensagens do cotação com delay 
        /// </summary>
        public Hashtable hstCotacoes = new Hashtable();
        public Hashtable hstDadosPapel = new Hashtable();

        /// <summary>
        /// Queue para armazenar todas as mensagens de cotação em tempo real 
        /// </summary>
        public Queue<MessageItem> FilaMensagens = new Queue<MessageItem>();
        public DateTime HorarioUltimaSonda { get; set; }

        private static MemoriaCotacaoDelay _me = null;
        private bool bKeepRunning = false;
        private Thread delayedProcessor = null;
        #endregion

        #region Métodos Públicos

        public static MemoriaCotacaoDelay GetInstance()
        {
            if (_me == null)
            {
                _me = new MemoriaCotacaoDelay();
            }

            return _me;
        }

        public void StartProcessamento()
        {
            bKeepRunning = true;
            delayedProcessor = new Thread(new ThreadStart(ProcessaLivroNegociosDelay));
            delayedProcessor.Start();
        }

        public void StopProcessamento()
        {
            bKeepRunning = false;

            logger.Info("Solicitando finalizacao da thread de processamento de sinal com atraso");

            while (delayedProcessor != null && delayedProcessor.IsAlive)
            {
                logger.Info("Aguardando finalizacao da thread de processamento de sinal com atraso");
                Thread.Sleep(250);
            }

            logger.Info("Thread de processamento de sinal com atraso finalizada");
        }
        /// <summary>
        ///  Adiciona o ultimo Tiker de cotação enviado pelo CEP
        /// </summary>
        /// <param name="Instrumento"> Nome do Instrumento </param>
        /// <param name="Mensagem">Mensagem de Cotação </param>
        public void AdicionarFilaLivroNegocios(string Instrumento,string Mensagem)
        {
            try
            {
                lock (FilaMensagens)
                {
                    MessageItem item = new MessageItem();
                    item.Instrumento = Instrumento;
                    item.MessageBody = Mensagem;

                    FilaMensagens.Enqueue(item);
                }

            }
            catch (Exception ex)
            {
                logger.Error("AdicionarFilaLivroNegocios(): " + ex.Message, ex);
            }
        }

        /// <summary>
       ///  Retorna um ticker de cotação para o Instrumento solicitado
       /// </summary>
       /// <param name="Instrumento">Código do Instrumento</param>
       /// <returns>Ticker de Cotação do instrumento requisitado</returns>
        public string ReceberCotacao(string Instrumento)
        {
            lock (hstCotacoes)
            {
                if (hstCotacoes[Instrumento] != null)
                {
                    return hstCotacoes[Instrumento].ToString();
                }

                return string.Empty;
            }
        }

        public void ProcessaSonda(string Instrumento, string Mensagem)
        {

            //SDBV20110627151711021SONDA               20110627151636000
            //012345678901234567890123456789012345678901234567890123456789
            //          1         2         3         4         5
            if (Mensagem.Length > 41)
            {
                string horario = Mensagem.Substring(41, 14);
                HorarioUltimaSonda = DateTime.ParseExact(horario, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }
        }

        public void ProcessaLivroNegociosDelay()
        {
            logger.Info("Inicio da thread de processamento de sinais com atraso");

            while(bKeepRunning)
            {
                MessageItem mensagem = null;

                try
                {
                    if (FilaMensagens.Count > 0)
                    {
                        lock (FilaMensagens)
                        {
                            mensagem = FilaMensagens.Dequeue();
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    DateTime ItemDataHora = DateTime.ParseExact(mensagem.MessageBody.Substring(41, 14), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                    logger.Debug("ItemDataHora [" + ItemDataHora.ToString("yyyyMMddHHmmss") + "]");

                    double delayMsg = (DateTime.Now - ItemDataHora).TotalMilliseconds;

                    logger.Debug("delayMsg = " + delayMsg);

                    if (delayMsg < DelayTickerAmount)
                    {
                        double waitTime = 0;

                        if (delayMsg <= 0)
                            waitTime = DelayTickerAmount;
                        else
                            waitTime = DelayTickerAmount - delayMsg;

                        logger.Debug("waitTime = " + waitTime);

                        double vezes = (waitTime / 100) + 1;

                        logger.Debug("vezes = " + vezes);

                        for (double i = 0; i < vezes; i++)
                        {
                            if (bKeepRunning == false)
                            {
                                logger.Info("Finalizando thread de processamento de sinais com atraso");
                                return;
                            }

                            Thread.Sleep(100);
                        }
                    }

                    lock (hstCotacoes)
                    {
                        if (hstCotacoes[mensagem.Instrumento] == null)
                        {
                            hstCotacoes.Add(mensagem.Instrumento, new List<string>());
                        }

                        List<string> valores = (List<string>)(hstCotacoes[mensagem.Instrumento]);

                        if (valores.Count >= NumeroItemHash)
                        {
                            valores.RemoveAt(0);
                        }

                        // Se a ultima mensagem de negocio tiver o mesmo numero
                        // substitui.
                        if (valores.Count > 0 && mensagem.MessageBody.Length >= 146)
                        {
                            int lastone = valores.Count - 1;
                            string ultimacotacao = valores[lastone];

                            if (ultimacotacao.Length > 146)
                            {
                                string numeroNegocio = ultimacotacao.Substring(138, 8);
                                string numnegocioMsg = mensagem.MessageBody.Substring(138, 8);

                                if (numeroNegocio.Equals(numnegocioMsg))
                                    valores.RemoveAt(lastone);
                            }
                            else
                            {
                                logger.Error("Msg invalida [" + ultimacotacao + "]");
                            }
                        }

                        valores.Add(mensagem.MessageBody);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("ProcessaLivroNegociosDelay(): " + ex.Message, ex);
                }
            }

            logger.Info("Finalizando thread de processamento de sinais com atraso (1)");
        }

        #endregion

    }
}
