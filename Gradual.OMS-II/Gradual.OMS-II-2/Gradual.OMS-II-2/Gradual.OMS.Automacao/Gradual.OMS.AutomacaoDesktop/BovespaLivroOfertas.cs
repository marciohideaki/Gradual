using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.OMS.Automacao.Lib;
using com.espertech.esper.compat;
using System.Collections.ObjectModel;

namespace Gradual.OMS.AutomacaoDesktop
{
	public class BovespaLivroOfertasException : Exception
	{
		public BovespaLivroOfertasException() {}
		public BovespaLivroOfertasException(string msg) : base(msg) {}
	}

    public class LivroPorSentido
    {
	    public SortedDictionary<string, LivroOfertasEntry> porOferta {get;set;}
	    public SortedDictionary<Decimal, SortedSet<string>> porPreco  {get;set;}
    }

    class ComparadorPrecoCompra : IComparer<Decimal>
    {
        // implementação da interface Comparator para 
        // ordenar livro de ofertas de compra (ordem decrescente)
        public int Compare(Decimal a, Decimal b)
        {
            return -( (Decimal)a ).CompareTo( (Decimal)b );
        }
    }

    public class BovespaLivroOfertas
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
	    private LivroPorSentido[] sentidos = new LivroPorSentido[NUM_SENTIDOS];
	
        /// <summary>
        /// 
        /// </summary>
        public BovespaLivroOfertas()
        {
	        // Inicializa livro de ofertas - Lado compra
	        inicializarLivro(LIVRO_COMPRA);
		
	        // Inicializa livro de ofertas - Lado venda
	        inicializarLivro(LIVRO_VENDA);
		
	        return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentido"></param>
        private void inicializarLivro(int sentido)
        {
            sentidos[sentido] = new LivroPorSentido();
            sentidos[sentido].porOferta = new SortedDictionary<string, LivroOfertasEntry>();

            if (sentido == LIVRO_COMPRA)
	            sentidos[sentido].porPreco = new SortedDictionary<Decimal, SortedSet<string>>(new ComparadorPrecoCompra());
            else if (sentido == LIVRO_VENDA)
	            sentidos[sentido].porPreco = new SortedDictionary<Decimal, SortedSet<string>>();
            //sentidos[sentido].qtdPorOferta = new HashMap<string, string>();
		
        }
	
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="idOferta"></param>
	    /// <param name="sentido"></param>
	    /// <param name="preco"></param>
	    /// <param name="quantidade"></param>
	    public void inserir(string idOferta, char sentido, Decimal preco, long quantidade, string instrumento, string tipopreco, string datahoraoferta)
	    {
		    int sentidoOferta = determinaSentido(sentido);
		    if (sentidoOferta == -1)
			    return;

		    LivroOfertasEntry dadosDestaOferta = new LivroOfertasEntry();
		    dadosDestaOferta.Preco = preco;
		    dadosDestaOferta.Quantidade = quantidade;
            dadosDestaOferta.LadoOferta = Convert.ToString(sentido);
            dadosDestaOferta.Corretora = idOferta.Substring(POS_COD_CORRETORA_INI, POS_COD_CORRETORA_FIM - POS_COD_CORRETORA_INI);
            dadosDestaOferta.DataHora = datahoraoferta;
            dadosDestaOferta.Instrumento = instrumento;
            dadosDestaOferta.TipoPreco = tipopreco;
		
		    // atualiza Map de Ofertas
		    sentidos[sentidoOferta].porOferta.Add(idOferta, dadosDestaOferta);
		
		    // atualiza Mapa de Preços
		    if (sentidos[sentidoOferta].porPreco.ContainsKey(preco))
		    {
			    sentidos[sentidoOferta].porPreco[preco].Add(idOferta);
		    }
		    else
		    {
			    // se o preço ainda não foi inserido no mapa,
			    // criar um novo Set de ofertas para esse preço
			    SortedSet<string> ofertasDoPreco = new SortedSet<string>();
			    ofertasDoPreco.Add(idOferta);
			    sentidos[sentidoOferta].porPreco.Add(preco, ofertasDoPreco);
		    }
		
		    return;
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="idOferta"></param>
	    /// <param name="sentido"></param>
	    /// <param name="preco"></param>
	    /// <param name="quantidade"></param>
	    public void alterar(string idOferta, char sentido, Decimal preco, long quantidade)
	    {
		    int sentidoOferta = determinaSentido(sentido);
		    if (sentidoOferta == -1)
			    return;
		
		    // Atualiza preco da oferta e armazena preco anterior
            if ( sentidos[sentidoOferta].porOferta.ContainsKey(idOferta)==false )
		    {
			    throw new BovespaLivroOfertasException("Oferta não encontrada:" + idOferta);
		    }

            LivroOfertasEntry dadosDestaOferta = sentidos[sentidoOferta].porOferta[idOferta];

		    Decimal precoAnterior = dadosDestaOferta.Preco;
		    dadosDestaOferta.Preco = preco;
		    dadosDestaOferta.Quantidade = quantidade;
		
		    sentidos[sentidoOferta].porOferta[idOferta]=dadosDestaOferta;
		
		    // Remove oferta para este preco
		    SortedSet<string> ofertasDoPrecoAnterior = 
			    (SortedSet<string>)sentidos[sentidoOferta].porPreco[precoAnterior];
		    ofertasDoPrecoAnterior.Remove(idOferta);

		    // Adiciona a oferta no novo preco
		    sentidos[sentidoOferta].porPreco[preco].Add(idOferta);
		
		    return;
	    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOferta"></param>
        /// <param name="sentido"></param>
	    public void cancelarOfertaSomente(string idOferta, char sentido) 
	    {
		    int sentidoOferta = determinaSentido(sentido);
		    if (sentidoOferta == -1)
			    return;
		
		    // Verifica preco anterior desta oferta
		    LivroOfertasEntry dadosDestaOferta = 
			    sentidos[sentidoOferta].porOferta.RemoveAndReturn(idOferta);

		    if (dadosDestaOferta == null)
		    {
			    throw new BovespaLivroOfertasException("Oferta não encontrada");
		    }

		    Decimal precoAnterior = dadosDestaOferta.Preco;

		    // Remove oferta para este preco
		    SortedSet<string> ofertasDoPrecoAnterior = 
			    (SortedSet<string>)sentidos[sentidoOferta].porPreco[precoAnterior];
		    ofertasDoPrecoAnterior.Remove(idOferta);
		
		    return;
	    }

	    /// <summary>
	    /// Cancela uma oferta e as ofertas melhores
	    /// </summary>
	    /// <param name="idOferta"></param>
	    /// <param name="sentido"></param>
	    public void cancelarOfertaEMelhores(string idOferta, char sentido) 
	    {
            List<string> listaCancelarOfertas = new List<string>();
            List<KeyValuePair<Decimal, SortedSet<string>>> listaCancelarPreco = new List<KeyValuePair<Decimal, SortedSet<string>>>();

		    int sentidoOferta = determinaSentido(sentido);
		    if (sentidoOferta == -1)
			    return;
		
		    // Remove oferta do mapa de Ofertas
		    LivroOfertasEntry dadosDestaOferta = 
			    sentidos[sentidoOferta].porOferta.RemoveAndReturn(idOferta);
		    if (dadosDestaOferta == null)
		    {
			    throw new BovespaLivroOfertasException("Oferta não encontrada");
		    }

		    Decimal precoReferencia = dadosDestaOferta.Preco;

		    // obtem o entrySet do mapa de preços
		    HashSet<KeyValuePair<Decimal,SortedSet<string>>> todosPrecos = sentidos[sentidoOferta].porPreco.AsHashSet();
            IEnumerator<KeyValuePair<Decimal, SortedSet<string>>> it = todosPrecos.GetEnumerator();
		    while (it.MoveNext())
		    {
			    // itera o mapa de preços
			    KeyValuePair<Decimal, SortedSet<string>> entry = it.Current;
			    int comparResult = entry.Key.CompareTo(precoReferencia);
			    // para ofertas de compra, é necessário inverter o sinal
			    // do resultado da comparação
			    comparResult *= (sentidoOferta == LIVRO_COMPRA) ? -1 : 1; 
			    if (comparResult <= 0)
			    {
				    // para ofertas melhores (preço maior no caso de compra e
				    // preço menor no caso de venda), remove as ofertas e o preço
				    SortedSet<string> ofertas = entry.Value;
				    IEnumerator<string> itOfertas = ofertas.GetEnumerator();
				    while(itOfertas.MoveNext())
				    {
					    string estaOferta = itOfertas.Current;

					    sentidos[sentidoOferta].porOferta.Remove(estaOferta);
					    //itOfertas.remove();
                        listaCancelarOfertas.Add(estaOferta);
                        //ofertas.Remove(estaOferta);

					    // para ofertas com mesmo preço, remove
					    // somente as ofertas anteriores à corrente (id menor)
					    if (estaOferta.CompareTo(idOferta) == 0)
						    break;
				    }

                    foreach (string oferta in listaCancelarOfertas)
                        ofertas.Remove(oferta);
                    listaCancelarOfertas.Clear();

				    //if (ofertas.Count==0)
                    //    todosPrecos.Remove(it.Current);
                    if (ofertas.Count == 0)
                        listaCancelarPreco.Add(it.Current);
                }
			    else
				    break;
		    }

            // Remove os precos sem ofertas correspondentes.
            foreach (KeyValuePair<Decimal, SortedSet<string>> item in listaCancelarPreco)
                todosPrecos.Remove(item);
		
		    return;
	    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentido"></param>
        /// <param name="numItens"></param>
        /// <returns></returns>
	    public string imprimirLivro(int sentido, int numItens)
	    {
            string livroSerializado = "";
		
		    try 
		    {
			    int contadorItens = 0;
			
			    HashSet<KeyValuePair<Decimal,SortedSet<string>>> todasOfertas = sentidos[sentido].porPreco.AsHashSet();
                IEnumerator<KeyValuePair<Decimal, SortedSet<string>>> itOfertas = todasOfertas.GetEnumerator();
			    while (itOfertas.MoveNext())
			    {
				    // itera o mapa de preços
                    KeyValuePair<Decimal, SortedSet<string>> entry = itOfertas.Current;

                    Decimal precoAtual = entry.Key;
				
				    SortedSet<string> ofertasDoPreco = entry.Value;
				    IEnumerator<string> itOfertasDoPreco = ofertasDoPreco.GetEnumerator();
				    while(itOfertasDoPreco.MoveNext())
				    {
					    string estaOferta = itOfertasDoPreco.Current;
					    long qtdDestaOferta = sentidos[sentido].porOferta[estaOferta].Quantidade;
					
					    livroSerializado = livroSerializado +
							    ((sentido == LIVRO_COMPRA) ? "C" : "V") + 
							    estaOferta.Substring(POS_COD_CORRETORA_INI) +
                                precoAtual.ToString() + 
							    qtdDestaOferta.ToString("D10");
					
					    contadorItens++;
					    if (contadorItens == numItens)
						    break;
				    }
				    if (contadorItens == numItens)
					    break;
			    }
		    }
		    catch(Exception e)
		    {
			    logger.Error("Exception em serializarLivro: " + e.Message);
		    }

		    return livroSerializado.ToString();
	    }



        /// <summary>
        /// Serializa o livro de ofertas para ser tratado pelo robot
        /// </summary>
        /// <param name="sentido"></param>
        /// <param name="numItens"></param>
        /// <returns></returns>
        public List<LivroOfertasEntry> serializarLivroRobot(int sentido, int depth = Int32.MaxValue)
        {
            List<LivroOfertasEntry> offers = sentidos[sentido].porOferta.Values.ToList();

            if (depth >= offers.Count || depth == Int32.MaxValue)
                return offers;

            return offers.GetRange(0, depth);
        }


        /// <summary>
        /// Gera lista de ofertas do sentido indicado
        /// </summary>
        /// <param name="sentido"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, LivroOfertasEntry>> ToList(int sentido)
	    {
            List<KeyValuePair<string, LivroOfertasEntry>> livro = new List<KeyValuePair<string, LivroOfertasEntry>>();
		
		    try 
		    {
                IEnumerator<KeyValuePair<string, LivroOfertasEntry>> itOfertas = sentidos[sentido].porOferta.GetEnumerator();
			    while (itOfertas.MoveNext())
			    {
                    livro.Add(itOfertas.Current);
			    }
		    }
		    catch(Exception e)
		    {
			    logger.Error("Exception em gerarCopiaLivro: " + e.Message);
		    }

		    return livro;
	    }

        /// <summary>
        /// Carrega uam lista de ofertas no sentido indicado
        /// </summary>
        /// <param name="sentido"></param>
        /// <param name="listaofertas"></param>
        public void AddList(int sentido, List<KeyValuePair<string, LivroOfertasEntry>> listaofertas)
        {
            // Limpa o livro e carregas as ofertas
            lock (sentidos)
            {
                sentidos[sentido].porOferta.Clear();
                sentidos[sentido].porPreco.Clear();

                foreach (KeyValuePair<string, LivroOfertasEntry> entry in listaofertas)
                {
                    string idOferta = entry.Key;
                    LivroOfertasEntry oferta = entry.Value;

                    sentidos[sentido].porOferta.Add(idOferta, oferta);

                    // atualiza Mapa de Preços
                    if (sentidos[sentido].porPreco.ContainsKey(oferta.Preco))
                    {
                        sentidos[sentido].porPreco[oferta.Preco].Add(idOferta);
                    }
                    else
                    {
                        // se o preço ainda não foi inserido no mapa,
                        // criar um novo Set de ofertas para esse preço
                        SortedSet<string> ofertasDoPreco = new SortedSet<string>();
                        ofertasDoPreco.Add(idOferta);
                        sentidos[sentido].porPreco.Add(oferta.Preco, ofertasDoPreco);
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentido"></param>
        /// <returns></returns>
        public List<string> imprimirLivro(int sentido)
        {
            // MUITO CUIDADO com a chamada desta rotina!!! Para altos
            // volumes, o tempo de processamento aumenta exponencialmente

            List<string> livro = new List<string>();
        
            try 
            {
            
                livro.Add(("Livro de " + ((sentido == LIVRO_COMPRA) ? "Compra:" : "Venda:")).PadRight(52));
                livro.Add("-".PadRight(52, '-'));
            
                HashSet<KeyValuePair<Decimal,SortedSet<string>>> todasOfertas = sentidos[sentido].porPreco.AsHashSet();
                IEnumerator<KeyValuePair<Decimal, SortedSet<string>>> itOfertas = todasOfertas.GetEnumerator();
                while (itOfertas.MoveNext())
                {
                    // itera o mapa de preços
                    KeyValuePair<Decimal, SortedSet<string>> entry = itOfertas.Current;
                
                    Decimal precoAtual = entry.Key;
                
                    SortedSet<string> ofertasDoPreco = entry.Value;
                    IEnumerator<string> itOfertasDoPreco = ofertasDoPreco.GetEnumerator();
                    while(itOfertasDoPreco.MoveNext())
                    {
                        string estaOferta = itOfertasDoPreco.Current;
                        long qtdDestaOferta = sentidos[sentido].porOferta[estaOferta].Quantidade;
                    
                        StringBuilder livroSerializado = new StringBuilder();
                        livroSerializado.Append(
                                estaOferta.Substring(POS_ID_OFERTA_INI, POS_ID_OFERTA_FIM - POS_ID_OFERTA_INI) + " - " +
                                estaOferta.Substring(POS_COD_CORRETORA_INI) + " - " +
                                precoAtual.ToString() + " - " +
                                qtdDestaOferta.ToString("D10"));
                    
                        livro.Add(livroSerializado.ToString());
                    }
                }

                livro.Add("-".PadRight(52,'-'));
            }
            catch(Exception e)
            {
                logger.Error("Exception em serializarLivro: " + e.Message);
            }

            return livro;
        }
	
	    public int determinaSentido(char sentido)
	    {
		    return((sentido == 'A') ? 0 : ((sentido == 'V') ? 1 : -1));
	    }
		
        public void limpar()
        {
            foreach(LivroPorSentido livroDoSentido in sentidos)
            {
	            livroDoSentido.porOferta.Clear();
			
	            foreach(SortedSet<string> idsDoPreco in livroDoSentido.porPreco.Values)
		            idsDoPreco.Clear();
			
	            livroDoSentido.porPreco.Clear();
            }
			
            return;
        }
	
        public void recarregar(int sentidoOferta, string idOferta, LivroOfertasEntry dadosOferta)
        {
	        LivroPorSentido livroDoSentido = sentidos[sentidoOferta];

	        livroDoSentido.porOferta.Put(idOferta, dadosOferta);
	        Decimal precoOferta = dadosOferta.Preco;

	        SortedSet<string> ofertasDoPreco = livroDoSentido.porPreco[precoOferta];

	        if (ofertasDoPreco == null)
                ofertasDoPreco = new SortedSet<string>();

	        ofertasDoPreco.Add(idOferta);
	        livroDoSentido.porPreco.Put(precoOferta, ofertasDoPreco);

     	    return;
        }
	

	    public const int NUM_SENTIDOS = 2;
	    public const int LIVRO_COMPRA = 0;
	    public const int LIVRO_VENDA = 1;
	    public const int POS_ID_OFERTA_INI = 0;
	    public const int POS_ID_OFERTA_FIM = 14;
	    public const int POS_COD_CORRETORA_INI = 14;
	    public const int POS_COD_CORRETORA_FIM = 22;
	    public const int POS_PRECO_INI = 3;
	    public const int POS_PRECO_FIM = 16;
	    public const int POS_QUANTIDADE_INI = 0;
    }
}
