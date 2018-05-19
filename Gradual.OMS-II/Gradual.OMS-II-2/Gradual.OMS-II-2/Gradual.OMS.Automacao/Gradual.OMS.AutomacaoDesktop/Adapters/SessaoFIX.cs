using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using com.espertech.esper.compat.collections;
using QuickFix44;
using QuickFix;
using System.Text.RegularExpressions;

namespace Gradual.OMS.AutomacaoDesktop.Adapters
{
    public class SessaoFIX : QuickFix44.MessageCracker, QuickFix.Application
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DadosGlobais dadosGlobais;

	    string senha = "";
	    string novaSenha = "";
	    Regex padraoInstrumentos = null;
	    SessionID sessao = null;
	    SortedDictionary<string, DadosInstrumentosBMF> instrumentosAssinados;
	    private LinkedBlockingQueue<string> filaNovaSenha;
	    private LinkedBlockingQueue<string> filaMensagensRetransmissorBMF;
	    private LinkedBlockingQueue<MarketDataSnapshotFullRefresh> filaMensagensFIXInstantaneo;
	    private LinkedBlockingQueue<MarketDataIncrementalRefresh> filaMensagensFIXIncremental;
	    private bool assinados = false;

	    /**
	     * Carrega dados do arquivo de configuracao quickfix.
	     * @param settings
	     * @param dadosGlobais
	     */
	    public SessaoFIX(
			    SessionSettings settings, 
			    DadosGlobais dadosGlobais,
			    LinkedBlockingQueue<MarketDataSnapshotFullRefresh> filaMensagensFIXInstantaneo,
			    LinkedBlockingQueue<MarketDataIncrementalRefresh> filaMensagensFIXIncremental,
			    LinkedBlockingQueue<string> filaMensagensRetransmissorBMF) 
	    {
		    instrumentosAssinados = 
			    new SortedDictionary<string, DadosInstrumentosBMF>();
		
		    this.filaMensagensFIXInstantaneo = filaMensagensFIXInstantaneo;
		    this.filaMensagensFIXIncremental = filaMensagensFIXIncremental;
		    this.filaMensagensRetransmissorBMF = filaMensagensRetransmissorBMF;
		
            object [] objsessions = settings.getSessions().ToArray();
            foreach ( object objsession in objsessions )
            {
                SessionID session = (SessionID) objsession;
    		    try 
    		    {
        		    Dictionary dictionary = settings.get(session);

        		    if ( dictionary.has(FIX_RAWDATA) )
    				    senha = dictionary.getString(FIX_RAWDATA);

    			    if ( dictionary.has(FIX_NEWPASSWORD) )
    				    novaSenha = dictionary.getString(FIX_NEWPASSWORD);
    			
    			    if ( dictionary.has(FIX_FILTRO_LISTA_INSTRUMENTOS) )
    				    padraoInstrumentos = new Regex(
    						    dictionary.getString(FIX_FILTRO_LISTA_INSTRUMENTOS) );
    		    } 
    		
    		    catch (ConfigError e) 
    		    {
    			    logger.Error("Falha de configuracao: " + e.Message);
    		    } 
    		    catch (FieldConvertError e) 
    		    {
    			    logger.Error("Falha de conversao: " + e.Message);
    		    }
            }
	    }
	
	    /**
	     * Carrega dados para troca de senha.
	     * @param settings
	     * @param filaNovaSenha
	     */
	    public SessaoFIX(
			    SessionSettings settings,
			    string novaSenha,
			    LinkedBlockingQueue<string> filaNovaSenha) 
	    {
		    this.filaNovaSenha = filaNovaSenha;
		    this.novaSenha = novaSenha;

            object[] objsessions = settings.getSessions().ToArray();
            foreach (object objsession in objsessions)
            {
                SessionID session = (SessionID)objsession;
                try 
    		    {
        		    Dictionary dictionary = settings.get(session);
        		    if ( dictionary.has(FIX_RAWDATA) )
    				    senha = dictionary.getString(FIX_RAWDATA);
    		    } 
    		
    		    catch (ConfigError e) 
    		    {
    			    logger.Error("Falha de configuracao: " + e.Message);
    		    } 
    		    catch (FieldConvertError e) 
    		    {
    			    logger.Error("Falha de conversao: " + e.Message);
    		    }
            }
	    }

	    /**
	     * Callback chamado logo depois que uma sessão QuickFix é criada.
	     * 
	     */
	    public void onCreate(SessionID sessionId) 
	    {
		    logger.Info("Sessao [" + sessionId.ToString() + "] criada!");
		    sessao =  sessionId;
	    }

	    /**
	     * Callback chamado logo depois que o Logon com a BM&F é estabelecido com sucesso.
	     * 
	     */
	    public void onLogon(SessionID sessionId) 
	    {
		    logger.Info("Logon OK!");
		
		    // Após Logon efetuado, solicita lista de instrumentos
		    solicitaListaInstrumentos();
		
		    // Se não houver filtro de instrumentos cadastrado,
		    // efetua assinatura de todos os instrumentos
		    if ( padraoInstrumentos == null )
		    {
			    logger.Debug("Efetua assinatura de todos os instrumentos");
			    requisitaInstrumento(
					    SubscriptionRequestType.SNAPSHOT_PLUS_UPDATES, 
					    MARKETDATAREQUEST_ALL_INSTRUMENTS, 
					    "", 
					    "");
			    assinados = true;
		    }
	    }

	    /**
	     * Callback chamado logo depois que o Logout com a BM&F é executado com sucesso.
	     * 
	     */
	    public void onLogout(SessionID sessionId) 
	    {
		    logger.Info("Logoff OK!");
	    }

	    /**
	     * Callback chamado logo antes do envio da mensagem FIX de controle de sessão.
	     * 
	     */
	    public void toAdmin(QuickFix.Message message, SessionID sessionId) 
	    {
		    try 
		    {
			    // Se for mensagem de Logon, inclui o RawData e RawDataLength na mensagem
			    if (message.getHeader().getString(MsgType.FIELD).Equals("A") )
			    {
				    message.setBoolean(ResetSeqNumFlag.FIELD, true );
				    message.setInt(NextExpectedMsgSeqNum.FIELD, 
					    (new NextExpectedMsgSeqNum(ConstantesMDS.BMF_SEQNUM_INICIAL)).getValue() );

				    message.setField(RawData.FIELD, new RawData(senha).ToString() );
				    message.setField(RawDataLength.FIELD, new RawDataLength(senha.Length).ToString() );
				
				    if ( novaSenha != null && novaSenha.Length > 0 )
				    {
					    message.setField(NewPassword.FIELD, new NewPassword(novaSenha).ToString());
					    logger.Info("Enviada nova senha para alteracao!");
				    }
			    }
			    else if ( message.getHeader().getString(MsgType.FIELD).Equals("5") )
			    {
				    if ( assinados )
					    cancelaInstrumentos();
			    }
			
			    logger.Debug("SEND(SES) --> " + message.ToString());
		    }
		
		    catch (FieldNotFound e) 
		    {
			    logger.Error("Campo nao encontrado: " + e.Message, e);
		    }
	    }

	    /**
	     * Callback chamado logo depois do recebimento bem sucedido de 
	     * mensagem FIX de controle de sessão.
	     * 
	     */
	    public void fromAdmin(QuickFix.Message message, SessionID sessionId)
	    {
		    logger.Debug("RECV(SES) <-- " + message.ToString());
	    }

	    /**
	     * Callback chamado logo antes do envio de mensagem FIX de aplicação.
	     * 
	     */
	    public void toApp(QuickFix.Message message, SessionID sessionId)
	    {
		    logger.Debug("SEND(APP) --> " + message.ToString());
	    }

	    /**
	     * Callback chamado logo depois do recebimento bem sucedido de 
	     * mensagem FIX de aplicação.
	     * 
	     */
        public void fromApp(QuickFix.Message message, SessionID sessionId)
	    {
		    try
		    {
			    logger.Debug("RECV(APP) <-- " + message.ToString());
			    crack(message, sessionId);
		    }
		
		    catch (FieldNotFound e)
		    {
			    logger.Error("Campo nao encontrado na mensagem: " + e.Message, e);
		    }
		    catch (IncorrectTagValue e)
		    {
			    logger.Error("Valor da Tag incorreto na mensagem: " + e.Message);
		    }
		    catch (UnsupportedMessageType e)
		    {
			    logger.Error("Tipo de mensagem nao suportado na mensagem: " + e.Message);
		    }
	    }

	    /**
	     * Callback chamado após o recebimento de mensagem FIX de rejeição de
	     * assinatura de Instrumento.
	     * 
	     */
	    public override void onMessage(
			    MarketDataRequestReject message, 
			    SessionID sessionId)
	    {
		    string texto = null;
		    string reqID = null;
		
		    try
		    {
			    if ( message.isSet(new Text()) )
				    texto = message.getField(new Text()).getValue();
			    if ( message.isSet(new MDReqID()) )
				    reqID = message.getField(new MDReqID()).getValue();
			
			    logger.Error("Requisicao de assinatura [" + 
					    reqID + "] rejeitada - Motivo: " + texto);
		    }

		    catch (FieldNotFound e)
		    {
			    logger.Error("Campo nao encontrado na mensagem: " + e.Message, e);
		    }
	    }

	    /**
	     * Callback chamado após o recebimento de mensagem FIX 
	     * de instantâneo de MarketData.
	     * 
	     */
	    public override void onMessage(
			    MarketDataSnapshotFullRefresh message, 
			    SessionID sessionId)
	    {
		    try 
		    {
			    filaMensagensFIXInstantaneo.Push(message);
		    }
		    catch (Exception e) 
		    {
			    logger.Error("Falha na leitura da fila filaMensagensFIXInstantaneo: " +
					    e.Message);
		    }
	    }
	
	    /**
	     * Callback chamado após o recebimento de mensagem FIX 
	     * de incremental de MarketData.
	     * 
	     */
	    public override void onMessage( MarketDataIncrementalRefresh message, SessionID sessionId)
	    {
		    try 
		    {
			    filaMensagensFIXIncremental.Push(message);
		    }
		    catch (Exception e) 
		    {
			    logger.Error("Falha na leitura da fila filaMensagensFIXIncremental: " +
					    e.Message);
		    }
	    }

	    /**
	     * Callback chamado após o recebimento de mensagem FIX de lista de
	     * Instrumentos.
	     * 
	     */
	    public override void onMessage( QuickFix44.SecurityList message, SessionID sessionId)
	    {		
		    try 
		    {
			    int ocorrencias =
				    message.getInt(NoRelatedSym.FIELD);
			
			    logger.Info("LISTA_INSTRUMENTO Sequencial[" + 
					    message.getHeader().getString(MsgSeqNum.FIELD) + 
					    "] Ocorrencias[" + ocorrencias + "]");
			
			    for ( uint ocorrencia = 1; 
				    ocorrencia <= ocorrencias; ocorrencia++ )
			    {
				    SecurityList.NoRelatedSym grupo = new SecurityList.NoRelatedSym();
				    message.getGroup(ocorrencia, ((QuickFix.Group) grupo));

				    if ( padraoInstrumentos != null )
				    {
					    // Se instrumento combinar com o padrao de instrumentos,
					    // efetua assinatura
	    			    if ( padraoInstrumentos.IsMatch(grupo.getString(Symbol.FIELD)) )
	    			    {
                            logger.InfoFormat("Requisitando assinatura {0}-{1}-{2}",
                                grupo.getString(Symbol.FIELD),
                                grupo.getString(SecurityID.FIELD),
                                grupo.getString(SecurityIDSource.FIELD));

						    requisitaInstrumento(
								    SubscriptionRequestType.SNAPSHOT_PLUS_UPDATES, 
								    grupo.getString(Symbol.FIELD), 
								    grupo.getString(SecurityID.FIELD), 
								    grupo.getString(SecurityIDSource.FIELD));

						    assinados = true;
				
						    if ( !instrumentosAssinados.ContainsKey(
								    grupo.getString(Symbol.FIELD)) )
						    {
							    instrumentosAssinados.Add(
									    grupo.getString(Symbol.FIELD), 
									    new DadosInstrumentosBMF(
											    grupo.getString(SecurityID.FIELD), 
											    grupo.getString(SecurityIDSource.FIELD)));
						    }
	    			    }
				    }
	    			
				    enviaInstrumento(
						    message.getHeader().getInt(MsgSeqNum.FIELD), 
						    ocorrencia, 
						    grupo);

				    logger.Debug("INSTRUMENTO [" + 
						    grupo.getString(Symbol.FIELD) + "] Descricao[" + 
						    grupo.getString(SecurityDesc.FIELD) + "]");
			    }
		    }
		
		    catch (FieldNotFound e) 
		    {
			    logger.Error("Campo nao encontrado na mensagem: " + e.Message, e);
		    }
	    }

	    /**
	     * Envia a mensagem FIX de SecurityListRequest, solicitando a lista de
	     * todos os instrumentos do sinal de difusão BM&F.
	     * 
	     */
	    private void solicitaListaInstrumentos()
	    {
		    SecurityListRequest mensagemFIX = new SecurityListRequest();
		
		    mensagemFIX.setField( 
				    new SecurityReqID(sessao.getSenderCompID()) );
		    mensagemFIX.setField( 
				    new SubscriptionRequestType(SubscriptionRequestType.SNAPSHOT_PLUS_UPDATES) );
		    mensagemFIX.setField( 
				    new SecurityListRequestType(SecurityListRequestType.ALL_SECURITIES) );

		    mensagemFIX.getHeader().setField(
				    new SenderCompID( sessao.getSenderCompID() ));
		    mensagemFIX.getHeader().setField(
				    new TargetCompID( sessao.getTargetCompID() ));
		
		    try 
		    {
			    logger.Info("Requisita lista de todos os instrumentos");
			    Session.sendToTarget(mensagemFIX, sessao);
		    }
		    catch (SessionNotFound e) 
		    {
			    logger.Error("Sessao invalida: " + e.Message);
		    }
	    }

	    /*
	     * Monta e envia requisição para cancelamento de assinatura de instrumentos.
	     * 
	     */
	    private void cancelaInstrumentos()
	    {
            try
            {
                if (instrumentosAssinados.Count == 0)
                {
                    logger.Info("Cancelando assinatura de todos os instrumentos");
                    requisitaInstrumento(
                            SubscriptionRequestType.DISABLE_PREVIOUS_SNAPSHOT_PLUS_UPDATE_REQUEST,
                            MARKETDATAREQUEST_ALL_INSTRUMENTS,
                            "",
                            "");
                }
                else
                {
                    logger.Info("Cancelando assinatura dos instrumentos assinados");
                    IEnumerator<KeyValuePair<string, DadosInstrumentosBMF>> itens =
                        instrumentosAssinados.GetEnumerator();
                    while (itens.MoveNext())
                    {
                        KeyValuePair<string, DadosInstrumentosBMF> item = itens.Current;
                        requisitaInstrumento(
                                SubscriptionRequestType.DISABLE_PREVIOUS_SNAPSHOT_PLUS_UPDATE_REQUEST,
                                item.Key,
                                item.Value.SecurityID,
                                item.Value.SecurityIDSource);
                    }
                }
                assinados = false;
            }
            catch (Exception ex)
            {
                logger.Error("requisitaMensagem: " + ex.Message, ex);
            }
	    }
	
	    /**
	     * Monta e envia requisição para assinatura de um instrumento.
	     * 
	     */
	    private void requisitaInstrumento(
			    char tipoAssinatura, 
			    string instrumento, 
			    string codigo, 
			    string identificacao )
	    {
            try
            {
                string mdReqID;
                MarketDataRequest mensagemFIX = new MarketDataRequest();

                if (codigo != null && codigo.Length > 0)
                    mdReqID = codigo;
                else
                    mdReqID = FIX_MDREQID_PADRAO;

                mensagemFIX.setField(new MDReqID(mdReqID));
                mensagemFIX.setField(new SubscriptionRequestType(tipoAssinatura));
                mensagemFIX.setField(new MarketDepth(0));
                mensagemFIX.setField(new MDUpdateType(MDUpdateType.INCREMENTAL_REFRESH));

                MarketDataRequest.NoMDEntryTypes grupoTipo = new MarketDataRequest.NoMDEntryTypes();
                grupoTipo.set(new QuickFix.MDEntryType(MDEntryType.BID));
                mensagemFIX.addGroup(grupoTipo);
                grupoTipo.set(new QuickFix.MDEntryType(MDEntryType.OFFER));
                mensagemFIX.addGroup(grupoTipo);
                grupoTipo.set(new QuickFix.MDEntryType(MDEntryType.TRADE));
                mensagemFIX.addGroup(grupoTipo);
                grupoTipo.set(new QuickFix.MDEntryType(MDEntryType.OPENING_PRICE));
                mensagemFIX.addGroup(grupoTipo);
                grupoTipo.set(new QuickFix.MDEntryType(MDEntryType.CLOSING_PRICE));
                mensagemFIX.addGroup(grupoTipo);
                grupoTipo.set(new QuickFix.MDEntryType(MDEntryType.TRADING_SESSION_HIGH_PRICE));
                mensagemFIX.addGroup(grupoTipo);
                grupoTipo.set(new QuickFix.MDEntryType(MDEntryType.TRADING_SESSION_LOW_PRICE));
                mensagemFIX.addGroup(grupoTipo);
                grupoTipo.set(new QuickFix.MDEntryType(MDEntryType.TRADING_SESSION_VWAP_PRICE));
                mensagemFIX.addGroup(grupoTipo);
                grupoTipo.set(new QuickFix.MDEntryType(MDEntryType.TRADE_VOLUME));
                mensagemFIX.addGroup(grupoTipo);
                grupoTipo.set(new QuickFix.MDEntryType('a'));
                mensagemFIX.addGroup(grupoTipo);
                grupoTipo.set(new QuickFix.MDEntryType('b'));
                mensagemFIX.addGroup(grupoTipo);
                grupoTipo.set(new QuickFix.MDEntryType('c'));
                mensagemFIX.addGroup(grupoTipo);

                MarketDataRequest.NoRelatedSym grupo = new MarketDataRequest.NoRelatedSym();

                grupo.set(new Symbol(instrumento));

                if (codigo != null && codigo.Length > 0)
                    grupo.set(new SecurityID(codigo));

                if (identificacao != null && identificacao.Length > 0)
                    grupo.set(new SecurityIDSource(identificacao));

                mensagemFIX.addGroup(grupo);

                mensagemFIX.getHeader().setField(
                        new SenderCompID(sessao.getSenderCompID()));
                mensagemFIX.getHeader().setField(
                        new TargetCompID(sessao.getTargetCompID()));

                try
                {
                    Session.sendToTarget(mensagemFIX, sessao);
                }
                catch (SessionNotFound e)
                {
                    logger.Error("Sessao invalida: " + e.Message, e);
                }
            }
            catch (Exception ex)
            {
                logger.Error("requisitaMensagem: " + ex.Message, ex);
            }
        }
	
	    /**
	     * Monta e envia mensagem de Instrumento da BM&F para o ESPER.
	     * Layout da mensagem:
	     *   SeqNum						9(15)
	     *   type							X(1)	( '.' - Instrumento )
	     *   symbol							X(20)
	     *   securityID					X(20)
	     *   body:
	     *    	SecurityIDSource (22)		X(1)
	     *    	Product (460)				9(3)
	     *    	CFICode (461)				X(6)
	     *    	SecurityType (167)			X(32)
	     *    	SecuritySubType (762)		X(32)
	     *    	MaturityMonthYear (200)	9(6)
	     *    	MaturityDate (541)		9(8)
	     *    	IssueDate (225)			9(8)
	     *    	CountryOfIssue (470)		X(2)
	     *    	ContractMultiplier (231)	X(15)
	     *    	SecurityExchange (207)	X(10)
	     *    	SecurityDesc (107)			X(100)
	     *    	ContractSettlMonth (667)	9(6)
	     *    	DatedDate (873)			9(8)
	     *    	Currency (15)				X(10)
	     *    	RoundLot (561)				9(15)
	     *    	MinTradeVol (562)			9(15)
	     *    	Asset (6937)				X(10)
	     *    	StrikePrice (202)			X(15)
	     *    	SecurityGroup (9918)		X(15)
	     *    
	     * @param msgSeqNum
	     * @param ocorrencia
	     * @param grupo
	     *    
	     */
	    private void enviaInstrumento(
			    int msgSeqNum, 
			    uint ocorrencia, 
			    SecurityList.NoRelatedSym grupo)
	    {
		    string mensagemMDS;
		
		    try
		    {
			    // SeqNum - 9(15)
			    mensagemMDS = msgSeqNum.ToString("D10");
			    mensagemMDS = mensagemMDS + ocorrencia.ToString("D5");

			    // type - X(1)
                mensagemMDS = mensagemMDS + '.';  // QuickFix.MDEntryType.INSTRUMENT;

			    // symbol - X(20)
			    mensagemMDS = mensagemMDS + grupo.getString(Symbol.FIELD).PadLeft(20);
			
			    // securityID - X(20)
			    mensagemMDS = mensagemMDS + grupo.getString(SecurityID.FIELD).PadLeft(20);

			    // body
			    mensagemMDS = mensagemMDS + (grupo.isSet(new SecurityIDSource()) ? grupo.getString(SecurityIDSource.FIELD) : " " ).PadLeft(1);
			    mensagemMDS = mensagemMDS + (grupo.isSet(new Product()) ? grupo.getInt(Product.FIELD) : 0 ).ToString("D3");
                mensagemMDS = mensagemMDS + (grupo.isSet(new CFICode()) ? grupo.getString(CFICode.FIELD) : " ").PadLeft(6);
                mensagemMDS = mensagemMDS + (grupo.isSet(new SecurityType()) ? grupo.getString(SecurityType.FIELD) : " ").PadLeft(32);
                mensagemMDS = mensagemMDS + (grupo.isSet(new SecuritySubType()) ? grupo.getString(SecuritySubType.FIELD) : " ").PadLeft(32);
			    mensagemMDS = mensagemMDS + (grupo.isSet(new MaturityMonthYear()) ? grupo.getInt(MaturityMonthYear.FIELD) : 0 ).ToString("D6");
			    mensagemMDS = mensagemMDS + (grupo.isSet(new MaturityDate()) ? grupo.getInt(MaturityDate.FIELD) : 0 ).ToString("D8");
			    mensagemMDS = mensagemMDS + (grupo.isSet(new IssueDate()) ? grupo.getInt(IssueDate.FIELD) : 0 ).ToString("D8");
                mensagemMDS = mensagemMDS + (grupo.isSet(new CountryOfIssue()) ? grupo.getString(CountryOfIssue.FIELD) : " ").PadLeft(2);
                mensagemMDS = mensagemMDS + (grupo.isSet(new ContractMultiplier()) ? grupo.getString(ContractMultiplier.FIELD) : " ").PadLeft(15);
                mensagemMDS = mensagemMDS + (grupo.isSet(new SecurityExchange()) ? grupo.getString(SecurityExchange.FIELD) : " ").PadLeft(10);
                mensagemMDS = mensagemMDS + (grupo.isSet(new SecurityDesc()) ? grupo.getString(SecurityDesc.FIELD) : "").PadLeft(100);
			    mensagemMDS = mensagemMDS + (grupo.isSet(new ContractSettlMonth()) ? grupo.getInt(ContractSettlMonth.FIELD) : 0 ).ToString("D6");
			    mensagemMDS = mensagemMDS + (grupo.isSet(new DatedDate()) ? grupo.getInt(DatedDate.FIELD) : 0 ).ToString("D8");
			    mensagemMDS = mensagemMDS + (grupo.isSet(new Currency()) ? grupo.getString(Currency.FIELD) : " " ).PadLeft(10);
			    mensagemMDS = mensagemMDS + (grupo.isSet(new RoundLot()) ? grupo.getInt(RoundLot.FIELD) : 0 ).ToString("D15");
			    mensagemMDS = mensagemMDS + (grupo.isSet(new MinTradeVol()) ? grupo.getInt(MinTradeVol.FIELD) : 0 ).ToString("D15");
                // Asset
			    mensagemMDS = mensagemMDS + (grupo.isSetField(6937) ? grupo.getString(6937) : " ").PadLeft(10);

			    mensagemMDS = mensagemMDS + (grupo.isSet(new StrikePrice()) ? grupo.getString(StrikePrice.FIELD) : " " ).PadLeft(15);

                // SecurityGroup
			    mensagemMDS = mensagemMDS + (grupo.isSetField(9918) ? grupo.getString(9918) : " ").PadLeft(15);
			
			    try 
			    {
				    filaMensagensRetransmissorBMF.Push(mensagemMDS);
			    }
			    catch (Exception e) 
			    {
				    logger.Error("Falha na leitura da fila filaMensagensRetransmissorBMF: " +
						    e.Message);
			    }

			    logger.Debug("INSTRUMENT - Instrumento[" + 
					    grupo.getString(Symbol.FIELD) + "] Mensagem[" + 
					    mensagemMDS + "]");
		    }
		    catch (FieldNotFound e) 
		    {
			    logger.Error("Campo nao encontrado na mensagem: " + e.Message, e);
			    return;
		    }
            catch (Exception ex)
            {
                logger.Error("enviaInstrumento(): " + ex.Message, ex);
                return;
            }
	    }
	
	    public const string FIX_NEWPASSWORD = "NewPassword";
	    public const string FIX_RAWDATA = "RawData";
        public const string FIX_FILTRO_LISTA_INSTRUMENTOS = "FiltroListaInstrumentos";
        public const string FIX_MDREQID_PADRAO = "GradualMDS";
        public const string MARKETDATAREQUEST_ALL_INSTRUMENTS = "*";
    }
}
