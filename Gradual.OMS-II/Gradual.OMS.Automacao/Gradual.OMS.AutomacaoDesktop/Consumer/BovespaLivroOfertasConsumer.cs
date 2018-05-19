using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using com.espertech.esper.client;
using com.espertech.esper.compat.collections;
using Gradual.OMS.AutomacaoDesktop.Events;
using System.IO;
using System.Globalization;

namespace Gradual.OMS.AutomacaoDesktop.Consumer
{
    public class BovespaLivroOfertasConsumer
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private NormalizadorNumero normalizador;
        private EPServiceProvider epService;
        private SortedDictionary<string, BovespaLivroOfertas> todosLivros;
	    private LinkedBlockingQueue<EventoBovespa> filaMensagensLivroOfertas;
	    private int maximoItens;
	    private HashSet<string> papeisReenvio;
	    private bool emReenvio;
        private DadosGlobais _dadosGlobais;
    
	    public BovespaLivroOfertasConsumer(
			    DadosGlobais dadosGlobais,
			    LinkedBlockingQueue<EventoBovespa> filaMensagensLivroOfertas,
			    int maximoItens)
	    {
            this._dadosGlobais = dadosGlobais;
		    this.epService = dadosGlobais.EpService;
		    this.todosLivros = dadosGlobais.TodosLOF;
		    this.filaMensagensLivroOfertas = filaMensagensLivroOfertas;
		    this.maximoItens = maximoItens;
		    normalizador = new NormalizadorNumero();
		
		    papeisReenvio = new HashSet<string>();
		    emReenvio = false;
            

		    return;
	    }

	    public void Run() 
        {
		    EventoBovespa evento;
		    long antes;
		    long depois;

            logger.Info("Iniciando thread BovespaLivroOfertasConsumer");
		
		    while ( _dadosGlobais.KeepRunning )
		    {
			    evento = null;
			    try
			    {
				    evento = filaMensagensLivroOfertas.Pop();
			    }
			    catch (Exception intExcept)
			    {
				    logger.Error("InterruptedException na leitura da fila de mensagens do retransmissor:");
				    logger.Debug(intExcept.Message);
				    continue;
			    }

			    antes = DateTime.Now.Ticks;

			    string instrumento = evento.Instrumento;
			    string tipo = evento.Tipo;
			    string corpo = evento.Corpo;

			    logger.Debug(evento.MsgID + " " + evento.Cabecalho + " S4 " + instrumento);

			    try
			    {
				    if (tipo.Equals("S0"))
				    {
					    processReenvio(corpo);
					    logger.Debug(evento.Cabecalho + " S0 " + instrumento);
				    }
				    else
				    {
                        BovespaLivroOfertas livro = null;

					    // S3 e S4 irão manipular o livro correspondente ao instrumento
                        if ( todosLivros.ContainsKey(instrumento) )
					        livro = todosLivros[instrumento];
                        else
					    {
						    logger.Debug("Livro do instrumento " + instrumento + " não existe, criando novo livro");
						    livro = new BovespaLivroOfertas();
						    todosLivros.Add(instrumento, livro);
					    }

					    if (tipo.Equals("S3"))
					    {
						    processAtualizacao(instrumento, livro, corpo);
					    }
					    else if (tipo.Equals("S4"))
					    {
						    processCancelamento(instrumento, livro, corpo);
					    }
	
					    if (!emReenvio)
					    {
						    // Buffer de montagem da mensagem
						    StringBuilder cabecalho = new StringBuilder();
						
						    // Cabeçalho
						    cabecalho.Append(ConstantesMDS.TIPO_REQUISICAO_LIVRO);
						    cabecalho.Append(ConstantesMDS.DESCRICAO_DE_BOLSA_BOVESPA);
						    cabecalho.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
						    cabecalho.Append(string.Format("%1$-20s", instrumento));

                            EventoAtualizacaoLivroOfertas atualof =
                                new EventoAtualizacaoLivroOfertas( instrumento,
                                    ConstantesMDS.PLATAFORMA_TODAS, 
                                    cabecalho.ToString(),
                                    livro.serializarLivroRobot(BovespaLivroOfertas.LIVRO_COMPRA),
                                    livro.serializarLivroRobot(BovespaLivroOfertas.LIVRO_VENDA));

						
						    // Gera evento de atualização
                            // ATP: implementar ?
                            //o eventoAtualizaLivro =
                            //    new EventoAtualizacaoLivroOfertas(
                            //        instrumento, ConstantesMDS.PLATAFORMA_TODAS, 
                            //        cabecalho.toString(), 
                            //        livroCompra.toString(), 
                            //        livroVenda.toString());
		
                            //epService.EPRuntime.SendEvent(eventoAtualizaLivro);
					    }
					
    					if ( this._dadosGlobais.Parametros.DebugLOFBovespa &&
                            this._dadosGlobais.Parametros.DebugLOFBovPapel != null &&
                            this._dadosGlobais.Parametros.DebugLOFBovPapel.Equals(instrumento))
					    {
                            string loffile = string.Format("{0}\\{1}.txt",
                                _dadosGlobais.Parametros.DiretorioDump,
                                _dadosGlobais.Parametros.DebugLOFBovPapel);

                            FileStream fs = File.Open(loffile, FileMode.Create, FileAccess.Write);
                            StreamWriter writer = new StreamWriter(fs,Encoding.ASCII);

						
						    writer.WriteLine("=".PadRight(100,'='));
						    writer.WriteLine(evento.MsgID + "," + evento.Cabecalho + evento.Corpo);
						    List<string> livroCompraSerializado = livro.imprimirLivro(BovespaLivroOfertas.LIVRO_COMPRA);
						    List<string> livroVendaSerializado = livro.imprimirLivro(BovespaLivroOfertas.LIVRO_VENDA);
                            for( int i=0; (i < livroCompraSerializado.Count || i <livroVendaSerializado.Count); i++)
                            {
                                string linha = "";
                                if ( i < livroCompraSerializado.Count )
                                    linha += livroCompraSerializado[i];
                                else
                                    linha += " ".PadLeft(20);

                                linha += "|";
                                if ( i < livroVendaSerializado.Count )
                                    linha += livroVendaSerializado[i];
                                else
                                    linha += " ".PadLeft(20);

                                writer.WriteLine(linha);

                            }

                            writer.WriteLine("=".PadRight(100, '='));
                            writer.Close();
                            fs.Close();
					    }
    				}

                    _dadosGlobais.LastMdgIDBov = evento.MsgID;
			    }
			    catch (Exception e)
			    {
				    logger.Error("Exception em BovespaLivroOfertasListener: ", e);
				    logger.Error("Processando a mensagem:");
				    logger.Error(evento.MsgID + "," + evento.Cabecalho + evento.Corpo);
			    }

			    depois = DateTime.Now.Ticks;
			    TimeSpan duracao = new TimeSpan(depois - antes);
			    logger.Debug("Duracao do processamento: " + duracao.TotalMilliseconds + 
					    " ms (Mensagens na fila: " + filaMensagensLivroOfertas.Count + ")");
		    }
	    }

	    private void processAtualizacao(string instrumento, BovespaLivroOfertas livro, string linha)
	    {
		    // caso esteja iniciando reenvio, "limpa" o livro
		    if (emReenvio)
		    {
			    if (!papeisReenvio.Contains(instrumento))
			    {
				    livro.limpar();
				    papeisReenvio.Add(instrumento);
			    }
		    }
		
		    // temporariamente descartando mensagens com oferta a preço de abertura
		    //if (linha.charAt(TIPO_PRECO_REGISTRADO_INI) == 'O')
		    //	return;
		
		    char tipoAcao = linha[TIPO_ACAO_INI];
		    switch(tipoAcao)
		    {
		    case 'C':
			    atualizacaoCriacao(livro, linha, instrumento);
			    break;
		    case 'M':
			    atualizacaoAlteracao(instrumento, livro, linha);
			    break;
		    case 'R':
			    atualizacaoRetransmissao(livro, linha, instrumento);
			    break;
		    default:
			    break;
		    }
		
	    }
	
	    private void atualizacaoCriacao(BovespaLivroOfertas livro, string linha, string instrumento)
	    {
		    logger.Debug("Ação: Criação");
		
		    string idOferta = 
			    linha.Substring(
				    ID_OFERTA_DATA_OFERTA_INI,
                    ID_OFERTA_NUM_SEQUENCIAL_OFERTA_FIM - ID_OFERTA_DATA_OFERTA_INI) +
			    linha.Substring(
				    ID_OFERTA_ID_CORRETORA_INI,
                    ID_OFERTA_ID_CORRETORA_FIM - ID_OFERTA_ID_CORRETORA_INI);

		    char sentidoOferta = linha[SENTIDO_OFERTA_INI];
		
		    Decimal preco;
		
		    char tipoPrecoRegistrado = linha[TIPO_PRECO_REGISTRADO_INI];

		    if (tipoPrecoRegistrado == TIPO_PRECO_REGISTRADO_PRECO_ABERTURA && 
				    sentidoOferta == SENTIDO_OFERTA_COMPRA)
		    {
			    //preco = PRECO_ABERTURA;
                preco = Decimal.MaxValue;
		    }
		    else
		    {
			    string strpreco = normalizador.normaliza(
					    linha[PRECO_OFERTA_FORMATO_INI], 
					    linha.Substring(PRECO_OFERTA_PRECO_INI,
                        PRECO_OFERTA_PRECO_FIM - PRECO_OFERTA_PRECO_INI));

                preco = Convert.ToDecimal(strpreco, CultureInfo.CreateSpecificCulture("pt-BR"));
		    }

            long quantidade = Convert.ToInt64(linha.Substring(QTD_EXIBIDA_INI, QTD_EXIBIDA_FIM - QTD_EXIBIDA_INI));

            string DataHoraOferta = linha.Substring(DATA_HORA_OFERTA_INI, DATA_HORA_OFERTA_FIM - DATA_HORA_OFERTA_INI);

		    livro.inserir(idOferta,
                sentidoOferta,
                preco,
                quantidade,
                instrumento,
                tipoPrecoRegistrado.ToString(),
                DataHoraOferta);
	    }
	
	    private void atualizacaoAlteracao(string instrumento, BovespaLivroOfertas livro, string linha)
	    {
		    logger.Debug("Ação: Alteração");

		    string idOferta = 
			    linha.Substring(
				    ID_OFERTA_DATA_OFERTA_INI,
                    ID_OFERTA_NUM_SEQUENCIAL_OFERTA_FIM - ID_OFERTA_DATA_OFERTA_INI) +
			    linha.Substring(
                        ID_OFERTA_ID_CORRETORA_INI,
                        ID_OFERTA_ID_CORRETORA_FIM - ID_OFERTA_ID_CORRETORA_INI);
			
		    string strpreco = 
			    normalizador.normaliza(
				    linha[PRECO_OFERTA_FORMATO_INI], 
				    linha.Substring(PRECO_OFERTA_PRECO_INI,
                    PRECO_OFERTA_PRECO_FIM - PRECO_OFERTA_PRECO_INI));

            Decimal preco = Convert.ToDecimal(strpreco, CultureInfo.CreateSpecificCulture("pt-BR"));

            long quantidade = Convert.ToInt64(linha.Substring(QTD_EXIBIDA_INI, QTD_EXIBIDA_FIM - QTD_EXIBIDA_INI));
		
		    char sentidoOferta = linha[SENTIDO_OFERTA_INI];
		
		    try
		    {
			    livro.alterar(idOferta, sentidoOferta, preco, quantidade);
		    }
		    catch(BovespaLivroOfertasException bloe)
		    {
			    logger.Error("Erro na alteração" +
					    " - Id Oferta: " + idOferta +
					    " - Sentido Oferta: " + sentidoOferta +
					    " - Instrumento: " + instrumento +
					    " - Descrição: " + bloe.Message);
		    }
	    }
	
	    private void atualizacaoRetransmissao(BovespaLivroOfertas livro, string linha, string instrumento)
	    {
		    logger.Debug("Ação: Retransmissão");
		    atualizacaoCriacao(livro, linha, instrumento);
		    return;
	    }

	    private void processCancelamento(string instrumento, BovespaLivroOfertas livro, string linha)
	    {
		    // caso esteja iniciando reenvio, "limpa" o livro
		    if (emReenvio)
		    {
			    if (!papeisReenvio.Contains(instrumento))
			    {
				    livro.limpar();
				    papeisReenvio.Add(instrumento);
			    }
		    }
		
		    string idOferta = 
			    linha.Substring(
				    S4_ID_OFERTA_DATA_OFERTA_INI,
                    S4_ID_OFERTA_NUM_SEQUENCIAL_OFERTA_FIM - S4_ID_OFERTA_DATA_OFERTA_INI) +
			    linha.Substring(
				    S4_ID_OFERTA_ID_CORRETORA_INI,
                    S4_ID_OFERTA_ID_CORRETORA_FIM - S4_ID_OFERTA_ID_CORRETORA_INI);
		
		    char sentidoOferta = linha[S4_ID_OFERTA_SENTIDO_OFERTA_INI];

		    char tipoCancelamento = linha[TIPO_CANCELAMENTO_INI];
		
		    switch(tipoCancelamento) {
		    case '1':
			    cancelaOfertaSomente(instrumento, livro, idOferta, sentidoOferta);
			    break;
		    case '2':
			    cancelaOfertaEMelhores(instrumento, livro, idOferta, sentidoOferta);
			    break;
		    case '3':
			    cancelaTodasOfertas(livro, idOferta, sentidoOferta);
			    break;
		    }
	    }
	
	    private void cancelaOfertaSomente(
			    string instrumento, 
			    BovespaLivroOfertas livro, 
			    string idOferta, 
			    char sentidoOferta)
	    {
		    logger.Debug("Ação: Cancela somente esta oferta");

		    try
		    {
			    livro.cancelarOfertaSomente(idOferta, sentidoOferta);
		    }
		    catch(BovespaLivroOfertasException bloe)
		    {
			    logger.Error("Erro no cancelamento (Oferta somente)" +
					    " - Id Oferta: " + idOferta +
					    " - Sentido Oferta: " + sentidoOferta +
					    " - Instrumento: " + instrumento +
					    " - Descrição: " + bloe.Message);
		    }
		
		    return;
	    }
	
	    private void cancelaOfertaEMelhores(
			    string instrumento, 
			    BovespaLivroOfertas livro, 
			    string idOferta, 
			    char sentidoOferta)
	    {
		    logger.Debug("Ação: Cancela esta oferta e melhores");

		    try
		    {
			    livro.cancelarOfertaEMelhores(idOferta, sentidoOferta);
		    }
		    catch(BovespaLivroOfertasException bloe)
		    {
			    logger.Error("Erro no cancelamento (Oferta e melhores)" +
					    " - Id Oferta: " + idOferta +
					    " - Sentido Oferta: " + sentidoOferta +
					    " - Instrumento: " + instrumento +
					    " - Descrição: " + bloe.Message);
		    }
		
		    return;
	    }
	
	    private void cancelaTodasOfertas(BovespaLivroOfertas livro, string idOferta, char sentidoOferta)
	    {
		    logger.Debug("Ação: Cancela todas as ofertas");
		
		    return;
	    }

	    private void processReenvio(string linha)
	    {
		    char marcadorReenvio = linha[S0_INDICADOR_INICIO_FIM_REENVIO_INI];
		
		    if (marcadorReenvio == REENVIO_INDICADOR_INICIO)
		    {
			    if (emReenvio)
			    {
				    logger.Error("Recebida mensagem de início de reenvio com flag de reenvio setada");
			    }
			
			    if (papeisReenvio.Count > 0)
			    {
				    logger.Error("Recebida mensagem de início de reenvio e Set de papéis em reenvio contem itens.");
				    papeisReenvio.Clear();
			    }
			
			    emReenvio = true;
			    logger.Info("Processada mensagem de início de reenvio");
		    }
		    else if (marcadorReenvio == REENVIO_INDICADOR_FIM)
		    {
			    if (!emReenvio)
			    {
				    logger.Error("Recebida mensagem de fim de reenvio com flag de reenvio resetada");
			    }
			
			    if (papeisReenvio.Count == 0)
			    {
				    logger.Warn("Recebida mensagem de fim de reenvio e Set de papéis em reenvio está vazia.");
			    }

			    papeisReenvio.Clear();
			
			    emReenvio = false;
			    logger.Info("Processada mensagem de fim de reenvio");
		    }
	    }
	
	    // Campos da mensagem S3
	    public const int TIPO_ACAO_INI = 0;
        public const int TIPO_ACAO_FIM = 1;
        public const int SENTIDO_OFERTA_INI =  1;
        public const int SENTIDO_OFERTA_FIM = 2;
        public const int PRECO_OFERTA_FORMATO_INI = 2; 
        public const int PRECO_OFERTA_FORMATO_FIM = 3;
        public const int PRECO_OFERTA_PRECO_INI = 3;
        public const int PRECO_OFERTA_PRECO_FIM = 16;
        public const int QTD_EXIBIDA_INI = 16;
        public const int QTD_EXIBIDA_FIM = 28;
        public const int PRECO_TEORICO_ABERTURA_FORMATO_INI = 40;
        public const int PRECO_TEORICO_ABERTURA_FORMATO_FIM = 41;
        public const int PRECO_TEORICO_ABERTURA_PRECO_INI = 41;
        public const int PRECO_TEORICO_ABERTURA_PRECO_FIM = 54;
        public const int ID_OFERTA_ID_CORRETORA_INI = 57;
        public const int ID_OFERTA_ID_CORRETORA_FIM = 65;
        public const int ID_OFERTA_DATA_OFERTA_INI = 65;
        public const int ID_OFERTA_DATA_OFERTA_FIM = 73;
        public const int ID_OFERTA_NUM_SEQUENCIAL_OFERTA_INI = 73; 
        public const int ID_OFERTA_NUM_SEQUENCIAL_OFERTA_FIM = 79;
        public const int TIPO_PRECO_REGISTRADO_INI = 79;
        public const int TIPO_PRECO_REGISTRADO_FIM = 80;
        public const int DATA_HORA_OFERTA_INI = 80;
        public const int DATA_HORA_OFERTA_FIM = 99;

        // Campos da mensagem S4
        public const int TIPO_CANCELAMENTO_INI = 0;
        public const int TIPO_CANCELAMENTO_FIM = 1;
        public const int S4_ID_OFERTA_ID_CORRETORA_INI = 1;
        public const int S4_ID_OFERTA_ID_CORRETORA_FIM = 9;
        public const int S4_ID_OFERTA_DATA_OFERTA_INI = 9;
        public const int S4_ID_OFERTA_DATA_OFERTA_FIM = 17;
        public const int S4_ID_OFERTA_NUM_SEQUENCIAL_OFERTA_INI = 17; 
        public const int S4_ID_OFERTA_NUM_SEQUENCIAL_OFERTA_FIM = 23;
        public const int S4_ID_OFERTA_SENTIDO_OFERTA_INI = 23;
        public const int S4_ID_OFERTA_SENTIDO_OFERTA_FIM = 24;

        // Campos da mensagem S0
        public const int S0_INDICADOR_INICIO_FIM_REENVIO_INI = 0;
        public const int S0_INDICADOR_INICIO_FIM_REENVIO_FIM = 1;

        // Número de itens do livro a serem serializados
        public const int NUM_ITENS_LIVRO_SERIALIZADO = 10;
        public const char REENVIO_INDICADOR_INICIO = 'D';
        public const char REENVIO_INDICADOR_FIM = 'F';
    
        // Tipos de Preco Registrado
        public const char TIPO_PRECO_REGISTRADO_PRECO_ABERTURA = 'O';
        public const char TIPO_PRECO_REGISTRADO_LIMITADA_L = 'L';
        public const char TIPO_PRECO_REGISTRADO_LIMITADA_S = 'S'; 
        public const char TIPO_PRECO_REGISTRADO_PRECO_MERCADO = 'M';
        public const char TIPO_PRECO_REGISTRADO_MELHOR_PRECO = 'X';
    
        // string fixa para indicar oferta a preço de abertura
        public const string PRECO_ABERTURA = "0000000000000,000000000";
    
        // Indicadores de sentido de oferta
        public const char SENTIDO_OFERTA_COMPRA = 'A';
        public const char SENTIDO_OFERTA_VENDA = 'V';

    }
}
