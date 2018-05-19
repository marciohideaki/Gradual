using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using log4net;
using MdsBayeuxClient;
using System.Configuration;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;
using System.Globalization;


namespace Gradual.Core.OMS.LimiteManager.Streamer
{
    public class StreamerManager
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        // Mds members
        MdsHttpClient.OnNegociosHandler OnNegociosHandler;
        Queue<MdsNegociosEventArgs> _queueMsgs;
        Thread _thProcessTrading;
        bool _running = false;
        Dictionary<string, MdsTradingInfo> _dicTrading;
        #endregion

        private static StreamerManager _me = null;

        public StreamerManager()
        {

        }
        public static StreamerManager GetInstance()
        {
            if (_me == null)
            {
                _me = new StreamerManager();
            }
            return _me;
        }

        public void Start()
        {
            try
            {
                // Assinar o recebimento de 
                this.OnNegociosHandler = new MdsHttpClient.OnNegociosHandler(OnNegocios);
                MdsHttpClient.Instance.OnNegociosEvent += this.OnNegociosHandler;
                _running = true;

                _queueMsgs = new Queue<MdsNegociosEventArgs>();
                _dicTrading = new Dictionary<string, MdsTradingInfo>();
                // Conectar ao streamer
                if (!this.ConnectToStreamer())
                {
                    logger.Info("Nao foi possivel conectar ao servidor Streamer");
                }

                _thProcessTrading = new Thread(new ThreadStart(this.ProcessMdsNegocios));
                _thProcessTrading.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start do StreamerManager: " + ex.Message, ex);
            }
        }


        public void Stop()
        {
            try
            {
                _running = false;
                

                MdsHttpClient.Instance.OnNegociosEvent -= this.OnNegociosHandler;
                this.OnNegociosHandler = null;

                // Desassinar os papeis
                foreach (KeyValuePair<string, MdsTradingInfo> item in _dicTrading)
                {
                    MdsAssinatura mdsSign = new MdsAssinatura();
                    mdsSign.Instrumento = item.Key.ToUpper().Trim();
                    mdsSign.Sinal = TipoSinal.CotacaoRapida;
                    MdsHttpClient.Instance.CancelaAssinatura(mdsSign);
                    logger.Info("Desassinando instrumento: " + item.Key);
                    mdsSign = null;
                }

                // Desconectar ao streamer
                if (!this.DisconnectStreamer())
                    logger.Info("Nao foi possivel desconectar o Streamer!");
                else
                    logger.Info("Streamer desconectado!");

                // Limpar controle de assinaturas
                if (null != _dicTrading)
                {
                    _dicTrading.Clear();
                    _dicTrading = null;
                }

                if (null != _queueMsgs)
                {
                    _queueMsgs.Clear();
                    _queueMsgs = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop do StreamerManager: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Efetuar instanciacao e conectar
        /// </summary>
        private bool ConnectToStreamer()
        {
            try
            {
                if (!MdsHttpClient.Conectado)
                {
                    MdsHttpClient.Instance.Conecta(ConfigurationManager.AppSettings["StreamerDeCotacao"]);
                }
                return MdsHttpClient.Conectado;
            }
            catch (Exception ex)
            {
                logger.Error("Nao foi possivel conectar ao streamer: " + ex.Message, ex);
                return false;
            }
        }

        /// <summary>
        /// Efetuar desconexao 
        /// </summary>
        /// <returns></returns>
        private bool DisconnectStreamer()
        {
            try
            {
                logger.Info("Desconectando streamer...");
                if (MdsHttpClient.Conectado)
                {
                    MdsHttpClient.Instance.Desconecta();
               }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Nao foi possivel desconectar ao streamer: " + ex.Message, ex);
                return false;
            }
        }

        private void OnNegocios(object sender, MdsNegociosEventArgs e)
        {
            try
            {
                lock (_queueMsgs)
                {
                    _queueMsgs.Enqueue(e);
                    Monitor.Pulse(_queueMsgs);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro no enfileiramento do evento de negocios: " + ex.Message, ex);
            }
        }

        private void ProcessMdsNegocios()
        {
            while (_running)
            {
                try
                {
                    MdsNegociosEventArgs e = null;
                    lock (_queueMsgs)
                    {
                        if (_queueMsgs.Count > 0)
                        {
                            e = _queueMsgs.Dequeue();
                        }
                        else
                            Monitor.Wait(_queueMsgs, 100);
                    }
                    if (null != e)
                    {
                        if (_dicTrading.ContainsKey(e.negocios.cb.i))
                        {
                            _dicTrading[e.negocios.cb.i].Quotes = e;
                        }
                    }
                    e = null;
                }
                catch (Exception ex)
                {
                    logger.Error("Problemas no processamento da mensagem de negocio: " + ex.Message, ex);
                }
            }
        }

        public void AddInstrument(string instrument, SymbolInfo symb = null)
        {
            try
            {
                if (!_dicTrading.ContainsKey(instrument))
                {
                    logger.Info("Assinando instrumento: " + instrument);
                    MdsAssinatura mdsSign = new MdsAssinatura();
                    mdsSign.Instrumento = instrument.ToUpper().Trim();
                    mdsSign.Sinal = TipoSinal.CotacaoRapida;
                    MdsNegociosEventArgs _mensagemNegocio = (MdsBayeuxClient.MdsNegociosEventArgs)MdsBayeuxClient.MdsHttpClient.Instance.Assina(mdsSign);
                    MdsTradingInfo mdsInstrument = new MdsTradingInfo();
                    if (null != symb)
                    {
                        mdsInstrument.Trading = symb.Trading;
                    }
                    if (null != _mensagemNegocio)
                    {
                        mdsInstrument.Quotes = _mensagemNegocio;
                    }
                    _dicTrading.Add(instrument, mdsInstrument); // primeira assinatura
                    mdsSign = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na assinatura do ativo: " + ex.Message, ex);
            }
        }

        public decimal GetLastPrice(string symbol)
        {
            decimal ret;
            if (_dicTrading.ContainsKey(symbol) && null != _dicTrading[symbol] )
            {
                MdsTradingInfo item = _dicTrading[symbol];
                // Buscar cotacao
                if (null != item.Quotes)
                {
                    ret = Convert.ToDecimal(_dicTrading[symbol].Quotes.negocios.ng.pr, new CultureInfo("pt-BR"));
                    // Se mesmo vindo cotacao e estiver zero, entao retornar valor fechamento (ex: hora de leilao)
                    if (ret == Decimal.Zero)
                        ret = item.Trading.VlrFechamento;
                }
                // Tentar retornar valor de fechamento
                else
                {
                    if (null != item.Trading)
                        ret = item.Trading.VlrFechamento;
                    else
                        ret = Decimal.Zero;
                }
            }
            else
                ret = Decimal.Zero;

            return ret;
        }
    }
}
