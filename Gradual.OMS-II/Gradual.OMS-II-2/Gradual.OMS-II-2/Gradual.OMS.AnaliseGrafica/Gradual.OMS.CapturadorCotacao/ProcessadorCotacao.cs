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
using Gradual.OMS.AnaliseGrafica.Lib.Dados;
using System.Collections.Concurrent;
using System.Configuration;

namespace Gradual.OMS.CapturadorCotacao
{
    public class ProcessadorCotacao : IServicoControlavel
    {
        protected  ServicoStatus _srvstatus = ServicoStatus.Parado;
        private  static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected CultureInfo ciBR = CultureInfo.CreateSpecificCulture("pt-BR");
        protected ConcurrentQueue<MDSMessageEventArgs> queueMensagemMds = new ConcurrentQueue<MDSMessageEventArgs>();
        protected ConcurrentDictionary<string, long> dctTimeStamp = new ConcurrentDictionary<string, long>();

        protected Dictionary<string, CotacaoANG> dctCotacao = new Dictionary<string, CotacaoANG>();
        protected  bool _bKeepRunning = false;

        protected ANGPersistenciaDB _db = null;
        MDSPackageSocket _mds = null;

        protected Thread _threadCotacao = null;

        // Fase de negociacao - flag para definir envio de mensagens de negocio p/ 
        // atualizacao dos dados de Analise Grafica
        public const String FASE_NEGOCIACAO_ATUALIZAR_ANALISE_GRAFICA = "0";
        public const String FASE_NEGOCIACAO_CONGELAR_HORARIO_ANALISE_GRAFICA = "1";
        public const String FASE_NEGOCIACAO_SUSPENDER_ANALISE_GRAFICA = "3";
        public const String FASE_NEGOCIACAO_ENVIADO_SERIE_HISTORICA = "4";


        protected  void OnCotacao(object sender, MDSMessageEventArgs args)
        {
            queueMensagemMds.Enqueue(args);
        }

        public virtual void Run()
        {
            long lastlog = 0;
            while (_bKeepRunning)
            {
                if (_mds == null || _mds.IsConectado() == false)
                {
                    _mds = new MDSPackageSocket();

                    _mds.IpAddr = ConfigurationManager.AppSettings["MDSAddress"].ToString();
                    _mds.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MDSPort"].ToString());

                    _mds.OnMensagemMdsReceived +=new MDSMessageReceivedHandler(OnCotacao);
                    _mds.OpenConnection();
                }

                List<MDSMessageEventArgs> tmpQueue = new List<MDSMessageEventArgs>();

                MDSMessageEventArgs mensagem = null;

                if ( queueMensagemMds.TryDequeue(out mensagem) )
                {
                    switch (mensagem.TipoMsg)
                    {
                        case MDSPackageSocket.TIPOMSG_NEGOCIO:
                            ProcessarMensagemCotacao(mensagem.Message);
                            break;

                        case MDSPackageSocket.TIPOMSG_CADASTROBASICO:
                            ProcessarMensagemCadastroBasico(mensagem.Message);
                            break;

                        case MDSPackageSocket.TIPOMSG_ABERTURA:
                            ProcessarMensagemAbertura(mensagem.Message);
                            break;

                        case MDSPackageSocket.TIPOMSG_FECHAMENTO:
                            ProcessarMensagemFechamento(mensagem.Message);
                            break;

                        case MDSPackageSocket.TIPOMSG_AJUSTE:
                            ProcessarMensagemAjuste(mensagem.Message);
                            break;

                        case MDSPackageSocket.TIPOMSG_UNITARIO:
                            ProcessarMensagemUnitario(mensagem.Message);
                            break;

                        case MDSPackageSocket.TIPOMSG_COMPOSICAOINDICE:
                            ProcessarMensagemComposicaoIndice(mensagem.Message);
                            break;
                    }

                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - lastlog);
                    if (ts.TotalMilliseconds > 2000)
                    {
                        logger.Info("Fila de mensagens a processar: " + queueMensagemMds.Count);
                        lastlog = DateTime.Now.Ticks;
                    }

                    continue;
                }

                Thread.Sleep(250);
            }
        }

        private void ProcessarMensagemCotacao(string mensagem)
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

                string dataNegocio = mensagem.Substring(41, 14);
                string estado = mensagem.Substring(155, 1);
                string fase = mensagem.Substring(156, 1);

                cotacao.A = mensagem.Substring(21, 20).Trim();
                //cotacao.Dt = DateTime.ParseExact(dataNegocio, "yyyyMMddHHmmss", ciBR);
                cotacao.Pr = Convert.ToDouble(mensagem.Substring(74, 13), ciBR);
                cotacao.Os = Convert.ToDouble(mensagem.Substring(146, 9).Trim(), ciBR);
                cotacao.Vl = Convert.ToDouble(mensagem.Substring(126, 13), ciBR);
                cotacao.To = Convert.ToDouble(mensagem.Substring(138, 8), ciBR);

                // Grava instrumentos em negociação e com data válida
                if (estado.Equals("2") && dataNegocio.StartsWith("000") == false)
                {
                    cotacao.Dt = DateTime.ParseExact(dataNegocio, "yyyyMMddHHmmss", ciBR);

                    _db.GravaCotacao(cotacao);
                    _db.GravaCotacaoOMS(cotacao);

                    //if (!dctTimeStamp.ContainsKey(cotacao.A))
                    //{
                    //    _db.GravaCotacaoPLD(cotacao);
                    //    dctTimeStamp.AddOrUpdate(cotacao.A, DateTime.Now.Ticks, (key, oldValue) => DateTime.Now.Ticks);
                    //}
                    //else
                    //{
                    //    long lastsignal = 0;
                    //    if (dctTimeStamp.TryGetValue(cotacao.A, out lastsignal))
                    //    {
                    //        TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - lastsignal);

                    //        if (ts.TotalSeconds > 1.0)
                    //        {
                    //            _db.GravaCotacaoPLD(cotacao);
                    //        }
                    //    }
                    //}

                    logger.DebugFormat("Gravado Cotacao: Ativo[{0}] Data[{1}] Preco[{2}] Variacao[{3}]",
                        cotacao.A, cotacao.Dt, cotacao.Pr, cotacao.Os);
                }

                // Grava fechamento
                if (!estado.Equals("1") && 
                    !estado.Equals("2") && 
                    !dataNegocio.StartsWith("000") && 
                    fase.Equals(FASE_NEGOCIACAO_ENVIADO_SERIE_HISTORICA) )
                {
                    cotacao.Dt = DateTime.ParseExact(dataNegocio, "yyyyMMddHHmmss", ciBR);
                    cotacao.Fe = cotacao.Pr;

                    DateTime inicioPregao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10,0,0);
                    DateTime hj = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                    if (cotacao.Dt.CompareTo(hj) < 0 || cotacao.Dt.CompareTo(inicioPregao) > 0)
                    {
                        if (cotacao.Fe > 0 && cotacao.Vl > 0 && cotacao.To > 0)
                        {
                            _db.GravaCotacaoPLD(cotacao);

                            logger.DebugFormat("Gravado Fechamento PLD: Ativo[{0}] Data[{1}] Fechamento [{2}] Volume[{3}] Qtde Negocios [{4}]",
                                cotacao.A,
                                cotacao.Dt,
                                cotacao.Fe,
                                cotacao.Vl,
                                cotacao.To);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(mensagem))
                    logger.Error("MENSAGEM [" + mensagem + "]");
                logger.Error("ProcessarMensagemCotacao: " + ex.Message, ex);
            }
        }

        private void ProcessarMensagemCadastroBasico(string mensagem)
        {
            try
            {
                ANGCadastroBasico xxx = Utils.FromBinaryString<ANGCadastroBasico>(mensagem);
                // Layout mensagem CADASTRO BASICO:
                //
                // Tipo de Mensagem          X(2)
                // Data                      N(8)       Formato AAAAMMDD
                // Hora                      N(9)       Formato HHMMSSmmm (mmm = milisegundos)
                // Código Instrumento        X(20)
                //
                // Código ISIN               X(20)
                // Instrumento Principal     X(20)
                // Lote padrão               N(8)
                // Segmento Mercado          X(8)
                // Forma Cotação             N(8)
                // Grupo Cotação             X(4)
                // Data Vencimento           N(8)
                // Preço Exercício           N(13)
                // Indicador Opção           X(1)
                // Coeficiente Multiplicação N(13)
                // Data último negócio       N(8)       Formato AAAAMMDD
                // Hora último negócio       N(6)       Formato HHMMSS
                // Security Id Source        X(4)
                // Especificação             X(100)
                CadastroBasico cadastroBasico = new CadastroBasico();


                cadastroBasico.Instrumento = xxx.Instrumento.ByteArrayToString().Trim();
                cadastroBasico.CodigoISIN = xxx.CodigoIsin.ByteArrayToString().Trim();
                cadastroBasico.InstrumentoPrincipal = xxx.CodigoPapelObjeto.ByteArrayToString().Trim();
                cadastroBasico.LotePadrao = Convert.ToInt32(xxx.LotePadrao.ByteArrayToString().Trim());
                cadastroBasico.SegmentoMercado = xxx.SegmentoMercado.ByteArrayToString().Trim();
                cadastroBasico.FormaCotacao = Convert.ToInt16(xxx.FormaCotacao.ByteArrayToString());
                cadastroBasico.GrupoCotacao = xxx.GrupoCotacao.ByteArrayToString().Trim();
                if (!mensagem.Substring(107, 8).Equals("00000000"))
                    cadastroBasico.DataVencimento = DateTime.ParseExact(xxx.DataVencimento.ByteArrayToString(), "yyyyMMdd", ciBR);
                cadastroBasico.PrecoExercicio = Convert.ToDouble(xxx.PrecoExercicio.ByteArrayToString(), ciBR);
                cadastroBasico.IndicadorOpcao = xxx.IndicadorOpcao.ByteArrayToString();
                cadastroBasico.CoeficienteMultiplicacao = Convert.ToDouble(xxx.CoeficienteMultiplicacao.ByteArrayToString(), ciBR);
                if (!mensagem.Substring(142, 14).Equals("00000000000000"))
                    cadastroBasico.DataUltimoNegocio = DateTime.ParseExact(xxx.DataHoraNegocio.ByteArrayToString(), "yyyyMMddHHmmss", ciBR);
                cadastroBasico.SecurityIDSource = xxx.SecurityIDSource.ByteArrayToString().Trim();
                cadastroBasico.Especificacao = xxx.Especificacao.ByteArrayToString().Trim();



                //cadastroBasico.Instrumento = mensagem.Substring(19, 20).Trim();
                //cadastroBasico.CodigoISIN = mensagem.Substring(39, 20).Trim();
                //cadastroBasico.InstrumentoPrincipal = mensagem.Substring(59, 20).Trim();
                //cadastroBasico.LotePadrao = Convert.ToInt32(mensagem.Substring(79, 8));
                //cadastroBasico.SegmentoMercado = mensagem.Substring(87, 8).Trim();
                //cadastroBasico.FormaCotacao = Convert.ToInt16(mensagem.Substring(95, 8));
                //cadastroBasico.GrupoCotacao = mensagem.Substring(103, 4).Trim();
                //if (!mensagem.Substring(107, 8).Equals("00000000"))
                //    cadastroBasico.DataVencimento = DateTime.ParseExact(mensagem.Substring(107, 8), "yyyyMMdd", ciBR);
                //cadastroBasico.PrecoExercicio = Convert.ToDouble(mensagem.Substring(115, 13), ciBR);
                //cadastroBasico.IndicadorOpcao = mensagem.Substring(128, 1);
                //cadastroBasico.CoeficienteMultiplicacao = Convert.ToDouble(mensagem.Substring(129, 13), ciBR);
                //if (!mensagem.Substring(142, 14).Equals("00000000000000"))
                //    cadastroBasico.DataUltimoNegocio = DateTime.ParseExact(mensagem.Substring(142, 14), "yyyyMMddHHmmss", ciBR);
                //cadastroBasico.SecurityIDSource = mensagem.Substring(156, 4).Trim();
                //cadastroBasico.Especificacao = mensagem.Substring(160).Trim();

                _db.GravaCadastroBasico(cadastroBasico);
                _db.GravaCadastroBasicoOMS(cadastroBasico);
                logger.DebugFormat("Gravado Cadastro Basico: Instrumento[{0}] DataUltimoNegocio[{1}] CodigoISIN[{2}] InstrumentoPrincipal[{3}] LotePadrao[{4}] SegmentoMercado[{5}] FormaCotacao[{6}] GrupoCotacao[{7}] DataVencimento[{8}] PrecoExercicio[{9}] IndicadorOpcao[{10}] CoeficienteMultiplicacao[{11}] Especificacao[{12}]",
                    cadastroBasico.Instrumento,
                    cadastroBasico.DataUltimoNegocio,
                    cadastroBasico.CodigoISIN,
                    cadastroBasico.InstrumentoPrincipal,
                    cadastroBasico.LotePadrao,
                    cadastroBasico.SegmentoMercado,
                    cadastroBasico.FormaCotacao,
                    cadastroBasico.GrupoCotacao,
                    cadastroBasico.DataVencimento,
                    cadastroBasico.PrecoExercicio,
                    cadastroBasico.IndicadorOpcao,
                    cadastroBasico.CoeficienteMultiplicacao,
                    cadastroBasico.Especificacao);

                _db.GravaSecurityList(cadastroBasico);
                logger.DebugFormat("Gravado SecurityList: Instrumento[{0}] SecurityID[{1}]", cadastroBasico.Instrumento, cadastroBasico.CodigoISIN);
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(mensagem))
                    logger.Error("MENSAGEM [" + mensagem + "]");

                logger.Error("ProcessarMensagemCadastroBasico: " + ex.Message, ex);
            }
        }

        private void ProcessarMensagemComposicaoIndice(string mensagem)
        {
            try
            {
                // Layout mensagem COMPOSICAO INDICE:
                //
                // Tipo de Mensagem     X(2)
                // Data                 N(8)        Formato AAAAMMDD
                // Hora                 N(9)        Formato HHMMSSmmm (mmm = milisegundos)
                // Código Instrumento   X(20)
                //
                // Quantidade itens     N(4)
                // Item 1               X(20)
                // ...
                // Item n               X(20)
                //
                string indice = mensagem.Substring(19, 20).Trim();
                int qtdItens = Convert.ToInt16(mensagem.Substring(39, 4), ciBR);

                for (int ct = 0; ct < qtdItens; ct++)
                {
                    CadastroBasico cadastroBasico = new CadastroBasico();

                    cadastroBasico.Instrumento = indice + "@" + mensagem.Substring(43 + 20*ct, 20).Trim();
                    cadastroBasico.InstrumentoPrincipal = indice;

                    _db.GravaCadastroBasico(cadastroBasico);
                    logger.DebugFormat("Gravado Composicao Indice: Indice/Instrumento[{0}]", cadastroBasico.Instrumento);
                }
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(mensagem))
                    logger.Error("MENSAGEM [" + mensagem + "]");

                logger.Error("ProcessarMensagemComposicaoIndice: " + ex.Message, ex);
            }
        }

        private void ProcessarMensagemAbertura(string mensagem)
        {
            try
            {
                ANGAbertura angAbertura = Utils.FromBinaryString<ANGAbertura>(mensagem);

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
                //
                CotacaoANG cotacao = new CotacaoANG();

                cotacao.A = angAbertura.Instrumento.ByteArrayToString().Trim();

                cotacao.Dt = DateTime.Now;
                if ( !angAbertura.DataHoraAbertura.ByteArrayToString().StartsWith("000"))
                    cotacao.Dt = DateTime.ParseExact(angAbertura.DataHoraAbertura.ByteArrayToString(), "yyyyMMddHHmmss", ciBR);
                cotacao.Ab = Convert.ToDouble(angAbertura.PrecoAbertura.ByteArrayToString(), ciBR);

                //cotacao.A = mensagem.Substring(19, 20).Trim();
                //if (!mensagem.Substring(39, 14).Equals("00000000000000"))
                //    cotacao.Dt = DateTime.ParseExact(mensagem.Substring(39, 14), "yyyyMMddHHmmss", ciBR);
                //cotacao.Ab = Convert.ToDouble(mensagem.Substring(53, 13), ciBR);

                _db.GravaCotacao(cotacao);
                _db.GravaCotacaoOMS(cotacao);
                logger.DebugFormat("Gravado Abertura: Ativo[{0}] Data[{1}] PrecoAbertura[{2}]",
                    cotacao.A, cotacao.Dt, cotacao.Ab);
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(mensagem))
                    logger.Error("MENSAGEM [" + mensagem + "]");

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
                //
                CotacaoANG cotacao = new CotacaoANG();

                cotacao.A = mensagem.Substring(19, 20).Trim();
                cotacao.Dt = DateTime.Now;
                if (!mensagem.Substring(39, 14).StartsWith("000"))
                    cotacao.Dt = DateTime.ParseExact(mensagem.Substring(39, 14), "yyyyMMddHHmmss", ciBR);
                cotacao.Fe = Convert.ToDouble(mensagem.Substring(53, 13), ciBR);
                cotacao.Vl = Convert.ToDouble(mensagem.Substring(68, 13), ciBR);
                cotacao.To = Convert.ToDouble(mensagem.Substring(81, 8), ciBR);

                _db.GravaCotacao(cotacao);
                _db.GravaCotacaoOMS(cotacao);

                DateTime inicioPregao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0);
                DateTime hj = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                if ( (cotacao.Dt.CompareTo(hj) < 0 || cotacao.Dt.CompareTo(inicioPregao) > 0) &&
                    cotacao.Fe > 0 && cotacao.Vl > 0 && cotacao.To > 0 )
                {
                    _db.GravaCotacaoPLD(cotacao);

                    logger.DebugFormat("Gravado Fechamento PLD: Ativo[{0}] Data[{1}] Fechamento [{2}] Volume[{3}] Qtde Negocios [{4}]",
                        cotacao.A,
                        cotacao.Dt,
                        cotacao.Fe,
                        cotacao.Vl,
                        cotacao.To);
                }

                logger.DebugFormat("Gravado Fechamento: Ativo[{0}] Data[{1}] PrecoFechamento[{2}]",
                    cotacao.A, cotacao.Dt, cotacao.Fe);
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(mensagem))
                    logger.Error("MENSAGEM [" + mensagem + "]");

                logger.Error("ProcessarMensagemFechamento: " + ex.Message, ex);
            }
        }

        public void ProcessarMensagemSerieHistorica(string pMensagem)
        {
            try
            {
                CotacaoANG mensagem = new CotacaoANG();

                // Layout da mensagem de Serie Historica
                //
                // Nome	                    Tipo(Tamanho)   Observação
                // Tipo de Mensagem         X(2)            'SH'
                // Tipo de Bolsa            X(2)            Bovespa = BV, BM&F = BF
                // Data                     N(8)            Formato AAAAMMDD
                // Hora                     N(9)            Formato HHMMSSmmm (mmm = milisegundos)
                // Código do Instrumento    X(20)
                // 
                // Data da Cotação          N(8)            Formato AAAAMMDD
                // Hora da Cotação          N(9)            Formato HHMMSSmmm (mmm = milisegundos)
                // Preço Abertura           N(13)
                // Preço Fechamento         N(13)
                // Preço Médio              N(13)
                // Preço Máximo             N(13)
                // Preço Mínimo             N(13)
                // Ind Oscilação            X(1)            positiva: “ “ (espaço em branco), Variação negativa: “-“
                // Percentual Oscilação     N(8)
                // Melhor Oferta de Compra  N(13)
                // Melhor Oferta de Venda   N(13)
                // Quantidade de Negócios   N(8)
                // Quantidade de Papéis     N(12)
                // Volume Acumulado         N(13)
                // Preço de Ajuste          N(13)

                mensagem.A = pMensagem.Substring(21, 20).Trim();

                // Obtem o tipo da bolsa
                if (pMensagem.Substring(2, 2).Equals("BV"))
                    mensagem.Bo = "BOV";
                else
                    mensagem.Bo = "BMF";

                mensagem.Ab = Convert.ToDouble(pMensagem.Substring(58, 13), ciBR);
                mensagem.Mx = Convert.ToDouble(pMensagem.Substring(97, 13), ciBR);
                mensagem.Mi = Convert.ToDouble(pMensagem.Substring(110, 13), ciBR);
                mensagem.Me = Convert.ToDouble(pMensagem.Substring(84, 13), ciBR);
                mensagem.Fe = Convert.ToDouble(pMensagem.Substring(71, 13), ciBR);
                mensagem.Os = Convert.ToDouble(pMensagem.Substring(123, 9).Trim(), ciBR);
                mensagem.OfC = Convert.ToDouble(pMensagem.Substring(132, 13), ciBR);
                mensagem.OfV = Convert.ToDouble(pMensagem.Substring(145, 13), ciBR);
                mensagem.To = Convert.ToDouble(pMensagem.Substring(158, 8), ciBR);
                mensagem.Qt = Convert.ToDouble(pMensagem.Substring(166, 12), ciBR);
                mensagem.Vl = Convert.ToDouble(pMensagem.Substring(178, 13), ciBR);
                mensagem.Aj = Convert.ToDouble(pMensagem.Substring(191, 13), ciBR);

                String data = pMensagem.Substring(41, 8) + "000000";
                mensagem.Dt = DateTime.ParseExact(data, "yyyyMMddHHmmss", ciBR);
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(pMensagem))
                    logger.Error("MENSAGEM [" + pMensagem + "]");

                logger.Error("ProcessarMensagemSerieHistorica: " + ex.Message, ex);
            }
        }

        private void ProcessarMensagemAjuste(string mensagem)
        {
            try
            {
                // Layout mensagem AJUSTE:
                //
                // Tipo de Mensagem     X(2)
                // Data                 N(8)        Formato AAAAMMDD
                // Hora                 N(9)        Formato HHMMSSmmm (mmm = milisegundos)
                // Código Instrumento   X(20)
                //
                // Data mensagem        N(8)       Formato AAAAMMDD
                // Hora mensagem        N(6)       Formato HHMMSS
                // Preço Ajuste         N(13)
                //
                CotacaoANG cotacao = new CotacaoANG();

                cotacao.A = mensagem.Substring(19, 20).Trim();
                cotacao.Dt = DateTime.Now;
                if (!mensagem.Substring(39, 14).StartsWith("000"))
                    cotacao.Dt = DateTime.ParseExact(mensagem.Substring(39, 14), "yyyyMMddHHmmss", ciBR);
                cotacao.Aj = Convert.ToDouble(mensagem.Substring(53, 13), ciBR);

                _db.GravaCotacao(cotacao);
                _db.GravaCotacaoOMS(cotacao);

                logger.DebugFormat("Gravado Ajuste: Ativo[{0}] Data[{1}] PrecoAjuste[{2}]",
                    cotacao.A, cotacao.Dt, cotacao.Aj);

                DateTime inicioPregao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10,0,0);
                DateTime hj = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                if (cotacao.Dt.CompareTo(inicioPregao) > 0 && cotacao.Aj > 0 && cotacao.Vl > 0 && cotacao.To > 0)
                {
                    cotacao.Fe = cotacao.Aj;

                    _db.GravaCotacaoPLD(cotacao);

                    logger.DebugFormat("Gravado Ajuste PLD: Ativo[{0}] Data[{1}] Fechamento [{2}] Volume[{3}] Qtde Negocios [{4}]",
                        cotacao.A,
                        cotacao.Dt,
                        cotacao.Fe,
                        cotacao.Vl,
                        cotacao.To);
                }

            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(mensagem))
                    logger.Error("MENSAGEM [" + mensagem + "]");

                logger.Error("ProcessarMensagemAjuste: " + ex.Message, ex);
            }
        }

        private void ProcessarMensagemUnitario(string mensagem)
        {
            try
            {
                // Layout mensagem UNITARIO:
                //
                // Tipo de Mensagem     X(2)
                // Data                 N(8)        Formato AAAAMMDD
                // Hora                 N(9)        Formato HHMMSSmmm (mmm = milisegundos)
                // Código Instrumento   X(20)
                //
                // Data mensagem        N(8)       Formato AAAAMMDD
                // Hora mensagem        N(6)       Formato HHMMSS
                // Preço Unitario       N(13)
                //
                CotacaoANG cotacao = new CotacaoANG();

                cotacao.A = mensagem.Substring(19, 20).Trim();
                cotacao.Dt = DateTime.Now;
                if (!mensagem.Substring(39, 14).StartsWith("000"))
                    cotacao.Dt = DateTime.ParseExact(mensagem.Substring(39, 14), "yyyyMMddHHmmss", ciBR);

                cotacao.Pu = Convert.ToDouble(mensagem.Substring(53, 13), ciBR);

                _db.GravaCotacao(cotacao);
                _db.GravaCotacaoOMS(cotacao);
                logger.DebugFormat("Gravado Unitario: Ativo[{0}] Data[{1}] PrecoUnitario[{2}]",
                    cotacao.A, cotacao.Dt, cotacao.Pu);
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(mensagem))
                    logger.Error("MENSAGEM [" + mensagem + "]");

                logger.Error("ProcessarMensagemUnitario: " + ex.Message, ex);
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
            logger.Info("*** Finalizando Processador de Cotacao ***");

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
