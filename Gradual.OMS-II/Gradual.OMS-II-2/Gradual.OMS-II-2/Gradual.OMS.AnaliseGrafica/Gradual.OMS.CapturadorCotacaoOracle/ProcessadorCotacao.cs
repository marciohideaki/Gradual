using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.AnaliseGrafica.Lib;
using System.Globalization;
using log4net;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using Gradual.OMS.Library;

namespace Gradual.OMS.CapturadorCotacaoOracle
{
    public class ProcessadorCotacao : IServicoControlavel
    {
        protected  ServicoStatus _srvstatus = ServicoStatus.Parado;
        private   static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected CultureInfo ciBR = CultureInfo.CreateSpecificCulture("pt-BR");
        protected Queue<CotacaoANG> queueCotacao = new Queue<CotacaoANG>();
        protected Queue<MDSMessageEventArgs> queueMensagemMds = new Queue<MDSMessageEventArgs>();
        protected  bool _bKeepRunning = false;

        protected ANGPersistenciaDB _db = null;
        private MDSPackageSocket _mds = null;
        protected ProcessadorCotacaoConfig _config = null;

        protected Thread _threadCotacao = null;

        /*
                // Obtem o tipo da bolsa
                if (pMensagem.Substring(2, 2).Equals("BV"))
                    mensagem.Bo = "BOV";
                else
                    mensagem.Bo = "BMF";

                mensagem.A = pMensagem.Substring(21, 20).Trim();
                String data = pMensagem.Substring(41, 8) + pMensagem.Substring(49, 6);
                mensagem.Dt = DateTime.ParseExact(data, "yyyyMMddHHmmss", ciBR);

                mensagem.Ab = Convert.ToDouble(pMensagem.Substring(58, 13), ciBR);
                mensagem.Fe = Convert.ToDouble(pMensagem.Substring(71, 13), ciBR);
                mensagem.Mi = Convert.ToDouble(pMensagem.Substring(110, 13), ciBR);
                mensagem.Mx = Convert.ToDouble(pMensagem.Substring(97, 13), ciBR);
                mensagem.Os = Convert.ToDouble(pMensagem.Substring(123, 9).Trim(), ciBR);
                mensagem.To = Convert.ToDouble(pMensagem.Substring(158, 8), ciBR);
                mensagem.Qt = Convert.ToDouble(pMensagem.Substring(166, 12), ciBR);
                mensagem.Vl = Convert.ToDouble(pMensagem.Substring(178, 13), ciBR);
        */

        protected void OnCotacao(object sender, MDSMessageEventArgs args)
        {
            lock (queueMensagemMds)
            {
                queueMensagemMds.Enqueue(args);
            }
        }

        public virtual void Run()
        {
            bool bWait = false;
            while (_bKeepRunning)
            {
                if (_mds == null || _mds.IsConectado() == false)
                {
                    _mds = new MDSPackageSocket();

                    _mds.IpAddr = _config.MDSAddress;
                    _mds.Port = _config.MDSPort;

                    _mds.OnFastQuoteReceived += new MDSMessageReceivedHandler(OnCotacao);
                    _mds.OpenConnection();
                }

                List<MDSMessageEventArgs> tmpQueue = new List<MDSMessageEventArgs>();

                lock (queueMensagemMds)
                {
                    tmpQueue = queueMensagemMds.ToList();
                    queueMensagemMds.Clear();
                }

                foreach (MDSMessageEventArgs mensagem in tmpQueue)
                {
                    switch (mensagem.TipoMsg)
                    {
                        case MDSPackageSocket.TIPOMSG_NEGOCIO:
                            ProcessarMensagemCotacao(mensagem.Message);
                            break;

                        case MDSPackageSocket.TIPOMSG_ABERTURA:
                            ProcessarMensagemAbertura(mensagem.Message);
                            break;

                        case MDSPackageSocket.TIPOMSG_FECHAMENTO:
                            ProcessarMensagemFechamento(mensagem.Message);
                            break;
                    }
                }

                lock (queueMensagemMds)
                {
                    if (queueMensagemMds.Count == 0)
                        bWait = true;
                    else
                        bWait = false;
                }

                if (bWait)
                    Thread.Sleep(250);
            }
        }

        private void ProcessarMensagemCotacao(string pMensagem)
        {
            try
            {
                // Layout mensagem NEGOCIO:
                //
                // Tipo de Mensagem     X(2)
                // Tipo de Bolsa	    X(2)        Espaços, ou Bovespa = BV, ou BM&F = BF
                // Data                 N(8)        Formato AAAAMMDD
                // Hora                 N(9)        Formato HHMMSSmmm (mmm = milisegundos)
                // Código Instrumento   X(20)
                //
                // Data                 N(8)        Formato AAAAMMDD
                // Hora                 N(9)        Formato HHMMSSmmm (mmm = milisegundos)
                // Corretora Compradora N(8)
                // Corretora Vendedora  N(8)
                // Preço                N(13)
                // Quantidade           N(12)
                // Máxima do dia        N(13)
                // Mínima do dia        N(13)
                // Volume Acumulado     N(13)
                // Número de Negócios   N(8)
                // Indicador Variação   X(1)        Variação positiva: “ “ (espaço em branco), Variação negativa: “-“
                // Variação             N(8)
                // Estado do Papel      N(1)        0 – não negociado, 1 – em leilão, 2 – em negociação, 3 – suspenso, 4 – congelado, 5 – inibido
                //
                CotacaoANG cotacao = new CotacaoANG();

                string bolsa = pMensagem.Substring(2, 2);
                string ativo = pMensagem.Substring(21, 20).Trim();
                string dataNegocio = pMensagem.Substring(41, 8);
                string horaNegocio = pMensagem.Substring(49, 6);
                string preco = pMensagem.Substring(74, 13);

                string minimo = pMensagem.Substring(112, 13);
                string maximo = pMensagem.Substring(99, 13);
                string volume = pMensagem.Substring(125, 13);
                string numeroNegocios = pMensagem.Substring(138, 8);

                string oscilacao = pMensagem.Substring(146, 9).Trim();
                string estado = pMensagem.Substring(155, 1);

                if (bolsa.Equals("BV"))
                {
                    // Ignora mensagens que não estiverem no estado "Papel em negociação", para BOVESPA
                    if (!estado.Equals("2"))
                        return;
                }
                else
                {
                    // Ignora mensagens que não estiverem no estado "Papel em negociação" e "Em leilão", para BMF
                    if (!estado.Equals("2") && !estado.Equals("1"))
                        return;
                }

                // Grava instrumentos em negociação e com data válida
                cotacao.Dt = DateTime.Now;
                if (!dataNegocio.StartsWith("000"))
                {
                    string dataHoraNegocio = dataNegocio + horaNegocio;
                    cotacao.Dt = DateTime.ParseExact(dataHoraNegocio, "yyyyMMddHHmmss", ciBR);
                }

                logger.DebugFormat("Instrumento[{0}]: Cotacao [{1}]", ativo, pMensagem);
                    
                // Obtem o tipo da bolsa
                if (bolsa.Equals("BV"))
                    cotacao.Bo = "BOV";
                else
                    cotacao.Bo = "BMF";

                cotacao.A = ativo;
                cotacao.Pr = Convert.ToDouble(preco, ciBR);
                cotacao.Os = Convert.ToDouble(oscilacao, ciBR);
                cotacao.Mi = Convert.ToDouble(minimo, ciBR);
                cotacao.Mx = Convert.ToDouble(maximo, ciBR);
                cotacao.Vl = Convert.ToDouble(volume, ciBR);
                cotacao.To = Convert.ToDouble(numeroNegocios, ciBR);

                _db.GravaCotacaoOracle(cotacao);
                logger.DebugFormat("Gravado Cotacao: Ativo[{0}] Data[{1}] Preco[{2}] Variacao[{3}]",
                    cotacao.A, cotacao.Dt, cotacao.Pr, cotacao.Os);
            }
            catch (Exception ex)
            {
                logger.Error("ProcessarMensagem: " + ex.Message, ex);
            }
        }

        private void ProcessarMensagemAbertura(string mensagem)
        {
            try
            {
                // Layout mensagem ABERTURA:
                //
                // Tipo de Mensagem     X(2)
                // Data                 N(8)        Formato AAAAMMDD
                // Hora                 N(9)        Formato HHMMSSmmm (mmm = milisegundos)
                // Código Instrumento   X(20)
                //
                // Data mensagem        N(8)       Formato AAAAMMDD
                // Hora mensagem        N(6)       Formato HHMMSS
                // Preço Abertura       N(13)
                // Tipo de Bolsa	    X(2)        Espaços, ou Bovespa = BV, ou BM&F = BF
                //
                CotacaoANG cotacao = new CotacaoANG();

                string ativo = mensagem.Substring(19, 20).Trim();
                string dataHora = mensagem.Substring(39, 14);
                string abertura = mensagem.Substring(53, 13);
                string bolsa = mensagem.Substring(66, 2);

                cotacao.Dt = DateTime.Now;
                if (!dataHora.StartsWith("000"))
                {
                    cotacao.Dt = DateTime.ParseExact(dataHora, "yyyyMMddHHmmss", ciBR);
                }

                logger.DebugFormat("Instrumento[{0}]: Abertura [{1}]", ativo, mensagem);

                if (bolsa.Equals("BV"))
                    cotacao.Bo = "BOV";
                else
                    cotacao.Bo = "BMF";

                cotacao.A = ativo;
                cotacao.Ab = Convert.ToDouble(abertura, ciBR);

                _db.GravaCotacaoOracle(cotacao);
                logger.DebugFormat("Gravado Abertura: Ativo[{0}] Data[{1}] PrecoAbertura[{2}]",
                    cotacao.A, cotacao.Dt, cotacao.Ab);
            }
            catch (Exception ex)
            {
                logger.Error("ProcessarMensagemAbertura: " + ex.Message, ex);
            }
        }

        private void ProcessarMensagemFechamento(string mensagem)
        {
            try
            {
                // Layout mensagem FECHAMENTO:
                //
                // Tipo de Mensagem     X(2)
                // Data                 N(8)        Formato AAAAMMDD
                // Hora                 N(9)        Formato HHMMSSmmm (mmm = milisegundos)
                // Código Instrumento   X(20)
                //
                // Data mensagem        N(8)       Formato AAAAMMDD
                // Hora mensagem        N(6)       Formato HHMMSS
                // Preço Fechamento     N(13)
                // Tipo de Bolsa	    X(2)        Espaços, ou Bovespa = BV, ou BM&F = BF
                //
                CotacaoANG cotacao = new CotacaoANG();

                string ativo = mensagem.Substring(19, 20).Trim();
                string dataHora = mensagem.Substring(39, 14);
                string fechamento = mensagem.Substring(53, 13);
                string bolsa = mensagem.Substring(66, 2);

                cotacao.Dt = DateTime.Now;
                
                if (!dataHora.StartsWith("000") )
                {
                    cotacao.Dt = DateTime.ParseExact(dataHora, "yyyyMMddHHmmss", ciBR);
                }

                logger.DebugFormat("Instrumento[{0}]: Fechamento [{1}]", ativo, mensagem);

                if (bolsa.Equals("BV"))
                    cotacao.Bo = "BOV";
                else
                    cotacao.Bo = "BMF";

                cotacao.A = ativo;
                cotacao.Fe = Convert.ToDouble(fechamento, ciBR);

                _db.GravaCotacaoOracle(cotacao);
                logger.DebugFormat("Gravado Fechamento: Ativo[{0}] Data[{1}] PrecoFechamento[{2}]",
                    cotacao.A, cotacao.Dt, cotacao.Fe);
            }
            catch (Exception ex)
            {
                logger.Error("ProcessarMensagemFechamento: " + ex.Message, ex);
            }
        }

        #region IServicoControlavel Members
        /// <summary>
        /// Invocado pelo framework ao iniciar o serviço
        /// </summary>
        public virtual void IniciarServico()
        {
            logger.Info("*** Iniciando Processador de Cotacao ***");

            _bKeepRunning = true;

            _config = GerenciadorConfig.ReceberConfig<ProcessadorCotacaoConfig>();

            _db = new ANGPersistenciaDB();

            _threadCotacao = new Thread(new ThreadStart(Run));
            _threadCotacao.Start();

            _srvstatus = ServicoStatus.EmExecucao;
        }

        /// <summary>
        /// Invocado pelo framework ao parar o servico
        /// </summary>
        public void PararServico()
        {
            logger.Info("*** Finalizando  Processador de Cotacao ***");

            _bKeepRunning = false;

            while (_threadCotacao.IsAlive)
            {
                Thread.Sleep(250);
            }

            _srvstatus = ServicoStatus.Parado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServicoStatus ReceberStatusServico()
        {
            return _srvstatus;
        }
        #endregion // IServicoControlavel Members
    }
}
