using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.MDS.Core.Lib;

namespace Gradual.MDS.Core.Sinal
{
    public class LivroOfertasBase
    {
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
	
	    private List<LOAGrupoOfertas> agregadoCompra = new List<LOAGrupoOfertas>();
	    private List<LOAGrupoOfertas> agregadoVenda = new List<LOAGrupoOfertas>();
	    private List<LOFDadosOferta> livroCompra = new List<LOFDadosOferta>();
	    private List<LOFDadosOferta> livroVenda = new List<LOFDadosOferta>();

        public int CasasDecimais { get; set; }
        public string Instrumento { get; set; }
        public Decimal BestBidPx { get; set; }
        public Decimal BestOfferPx { get; set; }
        public string TipoBolsa { get; set; }

        public LivroOfertasBase()
        {
            CasasDecimais = 2;
        }
	
	    //TODO: em lugar de retornar um int, retornar um objeto
	    //TODO: que contenha a acao, indice, preco, e quantidade
        public static List<LOAItemOferta> insereOferta(List<LOAGrupoOfertas> agregado,
			    List<LOFDadosOferta> livro,
			    LOFDadosOferta oferta,
			    int sentido)
	    {
		    String nomelivro = "Compra";
		    Decimal preco = oferta.Preco;

            List<LOAItemOferta> retorno = new List<LOAItemOferta>();

		    LOAItemOferta itemLOA = new LOAItemOferta();

		    if ( sentido == LIVRO_VENDA)
			    nomelivro = "Venda";

            if (logger.IsDebugEnabled)
            {
                logger.Debug("Insere oferta " + nomelivro + " [" +
                        oferta.IDOferta + "][" +
                        oferta.Preco + "][" +
                        oferta.Quantidade + "][" + oferta.Posicao + "]");
            }

		    itemLOA.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_INCLUIR;
		    itemLOA.Preco = preco;

		    // Insere a oferta no livro
		    livro.Insert(oferta.Posicao,oferta);
		
		    // Se eh a melhor oferta, e nao eh leilao
		    // armazena valor para tratamento no leilao
		    if ( oferta.Posicao == 0 && !oferta.Leilao )
		    {
		    }
		
		    int i=0;
		    while (i < agregado.Count)
		    {
			    LOAGrupoOfertas grupo = agregado[i];

			    if ( (preco.CompareTo(grupo.Preco) > 0 && sentido==LIVRO_COMPRA) ||
					    (preco.CompareTo(grupo.Preco) < 0 && sentido==LIVRO_VENDA) )
			    {
                    if (logger.IsDebugEnabled )
				        logger.Debug(nomelivro + ": Inclui novo grupo na posicao [" + i + "]");

				    LOAGrupoOfertas novogrupo = new LOAGrupoOfertas( preco, oferta, i );
				    agregado.Insert(i, novogrupo);
				
				    itemLOA.Quantidade = novogrupo.Quantidade;
				    itemLOA.Indice = i;
				    itemLOA.QtdeOrdens = novogrupo.QtdeOrdens;

                    retorno.Add(itemLOA);

                    return retorno;
			    }
			
			    if ( preco.CompareTo(grupo.Preco) == 0)
			    {
                    if ( logger.IsDebugEnabled )
				        logger.Debug(nomelivro + ": Encontrou grupo na posicao [" + i + "]");

				    LOAGrupoOfertas.incluiOferta(grupo, oferta);
				    grupo.Indice= i;
				
				    agregado[i] = grupo;

				    itemLOA.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR;
				    itemLOA.Quantidade = grupo.Quantidade;
				    itemLOA.Indice = i;
				    itemLOA.QtdeOrdens= grupo.QtdeOrdens;

                    retorno.Add(itemLOA);

                    return retorno;
                }

                if (oferta.Posicao == 0)
                {
                    if ((preco.CompareTo(grupo.Preco) < 0 && sentido == LIVRO_COMPRA) ||
                            (preco.CompareTo(grupo.Preco) > 0 && sentido == LIVRO_VENDA))
                    {
                        logger.Error(nomelivro + ": Inconsistencia, inserindo na posicao 0 com preco pior que o topo");

                        itemLOA = new LOAItemOferta();
                        itemLOA.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR;
                        itemLOA.Quantidade = grupo.Quantidade;
                        itemLOA.Indice = i;
                        itemLOA.QtdeOrdens = grupo.QtdeOrdens;
                        itemLOA.Preco = grupo.Preco;

                        agregado.RemoveAt(i);

                        retorno.Add(itemLOA);

                        continue;
                    }
                }

			    i++;
		    }

            if ( logger.IsDebugEnabled )
		        logger.Debug( nomelivro + ": Inclui novo grupo no final do livro, posicao [" + i + "]");

		    LOAGrupoOfertas novogrupo1 = new LOAGrupoOfertas( preco, oferta, i );

            itemLOA = new LOAItemOferta();
            itemLOA.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_INCLUIR;

            itemLOA.Preco = preco;
		    itemLOA.Indice = i;
		    itemLOA.Quantidade = novogrupo1.Quantidade;
		    itemLOA.QtdeOrdens = novogrupo1.QtdeOrdens;
		    agregado.Add(novogrupo1);

            retorno.Add(itemLOA);

            return retorno;
        }
	
	
	    public static List<LOAItemOferta> alteraOferta(List<LOAGrupoOfertas> agregado,
			    List<LOFDadosOferta> livro,
			    LOFDadosOferta oferta, 
			    int	sentido)
	    {
		    List<LOAItemOferta> retorno = new List<LOAItemOferta>();
		    Decimal preco = oferta.Preco;

            if ( logger.IsDebugEnabled )
		        logger.Debug("Altera oferta [" +oferta.IDOferta + "][" + oferta.Preco + "][" + oferta.Quantidade +"] pos [" + oferta.Posicao + "]");
		
		    LOFDadosOferta old = livro[oferta.Posicao];
		    old.Posicao = oferta.Posicao;
		
		    // Se for alteracao da oferta sem mudar o preco
		    if ( old.Preco.CompareTo(oferta.Preco)==0 )
		    {
			    livro[oferta.Posicao] = oferta;
			
			    for(int i=0; i<agregado.Count;i++)
			    {
				    LOAGrupoOfertas grupo = agregado[i];
	
				    if ( grupo.Preco.CompareTo(oferta.Preco) == 0)
				    {
					    LOAItemOferta itemOferta = new LOAItemOferta();
					    LOAGrupoOfertas.alteraOferta(grupo, old, oferta);
					
					    agregado[i] = grupo;
					
					    itemOferta.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR;
					    itemOferta.Indice = i;
					    itemOferta.Quantidade = grupo.Quantidade;
					    itemOferta.Preco = preco;
					    itemOferta.QtdeOrdens = grupo.QtdeOrdens;
					
					    retorno.Add(itemOferta);
					    return retorno;
				    }
			    }
			
			    logger.Error("alteraOferta(): Nao encontrou indice para o grupo de preco [" + oferta.Preco + "]");
		    }
		    else
		    {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("alteraoferta: [" + old.IDOferta + "] mudou para [" +
                            oferta.IDOferta + "] preco [" +
                            old.Preco + "]->[" + oferta.Preco + "]");
                }
			    List<LOAItemOferta> itemsOfertas = LivroOfertasBase.removeOferta(agregado, livro, old);
			    if ( itemsOfertas.Count > 0 )
                    retorno.AddRange(itemsOfertas);

                itemsOfertas = LivroOfertasBase.insereOferta(agregado, livro, oferta, sentido);
                if ( itemsOfertas.Count > 0 )
                    retorno.AddRange(itemsOfertas);
            }

		    return retorno;
	    }

        public static List<LOAItemOferta> removeOferta(List<LOAGrupoOfertas> agregado,
			    List<LOFDadosOferta> livro,
			    LOFDadosOferta oferta)
	    {
            List<LOAItemOferta> retorno = new List<LOAItemOferta>();

		    LOAItemOferta itemLOA = new LOAItemOferta();
		
		    LOFDadosOferta ofertaremovida = livro[oferta.Posicao];

            if (logger.IsDebugEnabled)
            {
                logger.Debug("Remover oferta [" + ofertaremovida.IDOferta + "] do grupo [" + ofertaremovida.Preco +
                        "][" + ofertaremovida.Quantidade + "] posicao [" + oferta.Posicao + "]");
            }

            livro.RemoveAt(oferta.Posicao);
            itemLOA.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR;
				
		    for(int i=0; i < agregado.Count; i++)
		    {
			    LOAGrupoOfertas grupo = agregado[i];

			    if ( grupo.Preco.CompareTo(ofertaremovida.Preco)==0)
			    {
				    if ( LOAGrupoOfertas.excluiOferta(grupo, ofertaremovida, 1) )
				    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("Oferta [" + oferta.IDOferta + "] eh a ultima do grupo [" + ofertaremovida.Preco + "] removendo grupo");
                        }
					    itemLOA.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR;
					    agregado.RemoveAt(i);
				    }
				    else
					    agregado[i] = grupo;
				
				    itemLOA.Indice = i;
				    itemLOA.Quantidade = grupo.Quantidade;
				    itemLOA.Preco = grupo.Preco;
				    itemLOA.QtdeOrdens = grupo.QtdeOrdens;

                    retorno.Add(itemLOA);

                    return retorno;
			    }
		    }

		    logger.Error("removeOferta: Nao encontrou indice para o grupo de preco [" + ofertaremovida.Preco + "]");
	
		    return retorno;
	    }
	
	    public static List<LOAItemOferta> removeOfertaEMelhores(List<LOAGrupoOfertas> agregado,
			    List<LOFDadosOferta> livro,
			    LOFDadosOferta oferta,
			    int sentido,
                List<LOFDadosOferta> listaLOF) 
	    {
		    String nomelivro = "Compra";
		    long quantidadeRemovida=0;
		    List<LOAItemOferta> retorno = new List<LOAItemOferta>();
		
		    if ( sentido == LIVRO_VENDA)
			    nomelivro = "Venda";

		
		    // Remove a oferta do livro
		    LOFDadosOferta ofertaReferencia = livro[oferta.Posicao];

            if (logger.IsDebugEnabled )
                logger.Debug(nomelivro + ":Remover oferta [" + ofertaReferencia.IDOferta + "] do grupo [" + ofertaReferencia.Preco + "] posicao [" + oferta.Posicao + "] e melhores");

		    long qtdeOrdens = 0;
		    for( int posdel = oferta.Posicao; posdel >=0; posdel--)
		    {
			    LOFDadosOferta ofertaremovida = livro[posdel];
                ofertaremovida.Posicao = posdel;
                livro.RemoveAt(posdel);

                listaLOF.Add(ofertaremovida);
			
			    // Se for do mesmo grupo de ofertas, acumula pra remover de uma unica vez
			    if ( ofertaremovida.Preco.CompareTo(ofertaReferencia.Preco) == 0)
			    {
				    quantidadeRemovida += ofertaremovida.Quantidade;
				    qtdeOrdens++;
			    }
		    }

		    int i=0;
		    while (i < agregado.Count)
		    {
			    LOAGrupoOfertas grupo = agregado[i];
			
			    // Remove as melhores ofertas dos grupos melhores
			    // que o grupo da oferta
			    if ( (grupo.Preco.CompareTo(ofertaReferencia.Preco) > 0 && sentido==LIVRO_COMPRA) ||
				    (grupo.Preco.CompareTo(ofertaReferencia.Preco) < 0 && sentido==LIVRO_VENDA) )
			    {
				    // Remove o elemento. A lista sobe, entao novamente
				    // pegamos o elemento no indice atual
                    if ( logger.IsDebugEnabled )
				        logger.Debug(nomelivro + ":Remover grupo do preco [" + grupo.Preco + "]");
				
				    LOAItemOferta itemOferta = new LOAItemOferta();
				    itemOferta.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR;
				    itemOferta.Indice = i;
				    itemOferta.Preco = grupo.Preco;
				    itemOferta.Quantidade = grupo.Quantidade;
				    itemOferta.QtdeOrdens = grupo.QtdeOrdens;
				
				    retorno.Add(itemOferta);
				    agregado.RemoveAt(i);
				    continue;
			    }

			    // Remove as melhores ofertas do mesmo grupo da oferta
			    if (grupo.Preco.CompareTo(ofertaReferencia.Preco) == 0)
			    {
                    if (logger.IsDebugEnabled )
				        logger.Debug(nomelivro + ":Remover a oferta do grupo do preco [" + grupo.Preco + "] indice [" + i+ "]");

				    ofertaReferencia.Quantidade = quantidadeRemovida;
				
				    LOAItemOferta itemOferta = new LOAItemOferta();
				    itemOferta.Indice = i;
				    itemOferta.Preco = grupo.Preco;

				    // Na verdade, ele so recalcula a quantidade,
				    // se zerar, exclui o grupo
				    if ( LOAGrupoOfertas.excluiOferta(grupo, ofertaReferencia, qtdeOrdens) )
				    {
                        if (logger.IsDebugEnabled )
					        logger.Debug(nomelivro + ":Oferta [" + ofertaReferencia.IDOferta + "] eh a ultima do grupo [" + ofertaReferencia.Preco + "] removendo grupo");
					    itemOferta.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR;
					    agregado.RemoveAt(i);
				    }
				    else
					    itemOferta.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR;

				    itemOferta.Quantidade = grupo.Quantidade;
				    itemOferta.QtdeOrdens = grupo.QtdeOrdens;
				    retorno.Add(itemOferta);
				    break;
			    }
			
			    if ( (grupo.Preco.CompareTo(ofertaReferencia.Preco) < 0 && sentido==LIVRO_COMPRA) ||
					    (grupo.Preco.CompareTo(ofertaReferencia.Preco) > 0 && sentido==LIVRO_VENDA) )
				    break;
			
			    i++;
		    }
		
		    return retorno;
	    }


	    public static List<LOAItemOferta> removeOfertaEPiores(List<LOAGrupoOfertas> agregado,
			    List<LOFDadosOferta> livro,
			    LOFDadosOferta oferta,
			    int sentido,
                List<LOFDadosOferta> listaLOF)
	    {
		    String nomelivro="Compra";
		
		    long quantidadeRemovida = 0;
		    List<LOAItemOferta> retorno = new List<LOAItemOferta>();

		    if ( sentido == LIVRO_VENDA)
			    nomelivro = "Venda";

            if (logger.IsDebugEnabled )
		        logger.Debug(nomelivro + ":Remover oferta [" +oferta.IDOferta + "] do grupo [" + oferta.Preco + "] e piores");

		    LOFDadosOferta ofertaReferencia = livro[oferta.Posicao];
		    long qtdeOrdens = 0;
		    for( int posdel = livro.Count-1; posdel >= oferta.Posicao; posdel--)
		    {
                LOFDadosOferta ofertaremovida = livro[posdel];
                ofertaremovida.Posicao = posdel;
                livro.RemoveAt(posdel);
                listaLOF.Add(ofertaremovida);
			
			    // Se for do mesmo grupo de ofertas, acumula pra remover de uma unica vez
			    if ( ofertaremovida.Preco.CompareTo(ofertaReferencia.Preco) == 0)
			    {
				    quantidadeRemovida += ofertaremovida.Quantidade;
				    qtdeOrdens++;
			    }
		    }
		
		    int i = agregado.Count -1;
		    while (i >=0 )
		    {
			    LOAGrupoOfertas grupo = agregado[i];
			
			    if ( (grupo.Preco.CompareTo(ofertaReferencia.Preco) < 0 && sentido==LIVRO_COMPRA) ||
				    (grupo.Preco.CompareTo(ofertaReferencia.Preco) > 0 && sentido==LIVRO_VENDA) )
			    {
                    if ( logger.IsDebugEnabled )
				        logger.Debug(nomelivro + ":Remover grupo do preco [" + grupo.Preco + "]");
				
				    // Remove o elemento. A lista sobe, entao novamente
				    // pegamos o elemento no indice atual
				    LOAItemOferta itemOferta = new LOAItemOferta();
				    itemOferta.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR;
				    itemOferta.Indice = i;
				    itemOferta.Preco = grupo.Preco;
				    itemOferta.Quantidade = grupo.Quantidade;
				    itemOferta.QtdeOrdens = grupo.QtdeOrdens;
				
				    retorno.Add(itemOferta);

				    agregado.RemoveAt(i);
				    //continue;
			    }
			
			    // Remove as melhores ofertas do mesmo grupo da oferta
			    if (grupo.Preco.CompareTo(ofertaReferencia.Preco) == 0)
			    {
                    if ( logger.IsDebugEnabled )
				        logger.Debug(nomelivro + ":Remover a oferta do grupo do preco [" + grupo.Preco + "] indice [" + i + "]");

				    ofertaReferencia.Quantidade = quantidadeRemovida;
				
				    LOAItemOferta itemOferta = new LOAItemOferta();
				    itemOferta.Indice = i;
				    itemOferta.Preco = grupo.Preco;

				    // Na verdade, ele so recalcula a quantidade,
				    // se zerar, exclui o grupo
				    if ( LOAGrupoOfertas.excluiOferta(grupo, ofertaReferencia, qtdeOrdens) )
				    {
                        if ( logger.IsDebugEnabled )
					        logger.Debug(nomelivro + ":Oferta [" +ofertaReferencia.IDOferta + "] eh a ultima do grupo [" + ofertaReferencia.Preco + "] removendo grupo");

					    itemOferta.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR;
					    agregado.RemoveAt(i);
				    }
				    else
					    itemOferta.Acao = ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_ALTERAR;

				    itemOferta.Quantidade = grupo.Quantidade;
				    itemOferta.QtdeOrdens = grupo.QtdeOrdens;
				    retorno.Add(itemOferta);
				    break;
			    }
			
			    if ( (grupo.Preco.CompareTo(ofertaReferencia.Preco) > 0 && sentido==LIVRO_COMPRA) ||
					    (grupo.Preco.CompareTo(ofertaReferencia.Preco) < 0 && sentido==LIVRO_VENDA) )
				    break;
			
			    i--;
		    }
		
		    return retorno;
	    }

	    public static List<LOAItemOferta> resetLivro(List<LOAGrupoOfertas> agregado,
			    List<LOFDadosOferta> livro,
                List<LOFDadosOferta> listaLOF) 
	    {
		    List<LOAItemOferta> retorno = new List<LOAItemOferta>();

            for (int i = 0; i < livro.Count; i++)
            {
                LOFDadosOferta oferta = livro[i];
                oferta.Posicao = i;
                listaLOF.Add(oferta);
                livro.RemoveAt(i);
            }
		
		    for( int i=agregado.Count-1; i >=0; i--)
		    {
			    LOAGrupoOfertas grupo = agregado[i];
                agregado.RemoveAt(i);
			
			    LOAItemOferta itemOferta = new LOAItemOferta();
			    itemOferta.Indice = i;
			    itemOferta.Preco = grupo.Preco;
			    itemOferta.Quantidade = grupo.Quantidade;
			    itemOferta.QtdeOrdens = grupo.QtdeOrdens;
			    itemOferta.Acao= ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_EXCLUIR;
				
			    retorno.Add(itemOferta);
		    }

		    return retorno;
	    }

	    public static List<LOAItemOferta> dumpLivro( List<LOAGrupoOfertas> agregado, int sentido) 
	    {
		    List<LOAItemOferta> retorno = new List<LOAItemOferta>();
		
		    if ( sentido==LIVRO_COMPRA )
			    logger.Debug("Livro Compra");
		    else
			    logger.Debug("Livro Venda");

		    for ( int i=0; i < agregado.Count; i ++ )
		    {
			    LOAGrupoOfertas grupo = agregado[i];

			    logger.Debug( i + ": " + grupo.Preco + "|" + grupo.Quantidade);
		    }
		
		    return retorno;
	    }

        public static List<Dictionary<String, String>> montaLivroOfertasCompleto(
                LivroOfertasBase livroOferta, int sentido, int casasDecimais)
        {
            List<Dictionary<String, String>> mensagem =
                    new List<Dictionary<String, String>>();

            List<LOFDadosOferta> livro = null;

            if (sentido == LivroOfertasBase.LIVRO_COMPRA)
                livro = livroOferta.getLivroCompra();
            else
                livro = livroOferta.getLivroVenda();


            for (int posicao = 0; posicao < livro.Count; posicao++)
            {
                LOFDadosOferta item = livro[posicao];

                item.Posicao = posicao;

                mensagem.Add(LOFDadosOferta.montarRegistroOferta(ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_INCLUIR, item, casasDecimais));
            }

            return mensagem;
        }
	
	    public static List<Dictionary<String, String>> montaLivroAgregadoCompleto(
			    LivroOfertasBase livroOfertaAgregado, int sentido, int casasDecimais)
	    {
		    List<Dictionary<String, String>> mensagem = 
				    new List<Dictionary<String, String>>();
		
		    List<LOAGrupoOfertas> agregado = null;
		
		    if ( sentido == LivroOfertasBase.LIVRO_COMPRA)
			    agregado = livroOfertaAgregado.getAgregadoCompra();
		    else
			    agregado = livroOfertaAgregado.getAgregadoVenda();
					

		    for ( int posicao = 0; posicao < agregado.Count; posicao++ )
		    {
			    LOAGrupoOfertas grupo = agregado[posicao];
			
			    //Decimal preco = grupo.Preco;
			    long quantidade = grupo.Quantidade;
			    long qtdeOrdens = grupo.QtdeOrdens;

			    String preco = "";
			    if (grupo.Preco == ConstantesMDS.PRECO_LIVRO_OFERTAS_MIN_VALUE ||
                        grupo.Preco == ConstantesMDS.PRECO_LIVRO_OFERTAS_MAX_VALUE)
			    {
				    preco = ConstantesMDS.DESCRICAO_OFERTA_ABERTURA;
			    }
			    else
			    {
				    if ( grupo.Preco != Decimal.Zero )
				    {
					    preco = String.Format("{0:f" + casasDecimais + "}", grupo.Preco).Replace('.', ',');
				    }
			    }
			
			    Dictionary<String, String> itemMensagem = new Dictionary<String, String>();

        	    itemMensagem.Add(
    				    ConstantesMDS.HTTP_OFERTAS_ACAO, 
    				    String.Format("{0:d}", ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_INCLUIR));
                itemMensagem.Add(
					    ConstantesMDS.HTTP_OFERTAS_POSICAO, 
					    String.Format("{0:d}", posicao));
                itemMensagem.Add(
					    ConstantesMDS.HTTP_OFERTAS_PRECO, 
					    preco);
                itemMensagem.Add(
					    ConstantesMDS.HTTP_OFERTAS_QUANTIDADE, 
					    String.Format("{0:d}", quantidade));
                itemMensagem.Add(
					    ConstantesMDS.HTTP_OFERTAS_QTDE_ORDENS, 
					    String.Format("{0:d}", qtdeOrdens));
			
			    mensagem.Add(itemMensagem);
		    }
		
		    return mensagem; 
	    }
	

	    public static String montaMensagemHomeBrokerAgregado (
			    List<LOAGrupoOfertas> agregado,
			    int numeroItens,
			    int sentido)
	    {

		    StringBuilder mensagem = new StringBuilder();
		
		    String side = (sentido == LIVRO_COMPRA) ? "C" : "V";
		    int posicaoItem = 0;

		    while ( posicaoItem < numeroItens &&
				    posicaoItem < agregado.Count )
		    {
			    LOAGrupoOfertas grupo = agregado[posicaoItem];

			    mensagem.Append(side);
                if (grupo.Preco == ConstantesMDS.PRECO_LIVRO_OFERTAS_MIN_VALUE || grupo.Preco == ConstantesMDS.PRECO_LIVRO_OFERTAS_MAX_VALUE)
                    mensagem.Append(String.Format("{0,-13}", ConstantesMDS.DESCRICAO_OFERTA_ABERTURA));
                else
                    mensagem.Append(grupo.Preco.ToString("0000000000.00"));
			    mensagem.Append(String.Format("{0:d12}", grupo.Quantidade));
                mensagem.Append(String.Format("{0:d12}", grupo.QtdeOrdens));
			
			    posicaoItem++;
		    }

            mensagem.Replace('.', ',');

            return mensagem.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="livroOferta"></param>
        /// <param name="numeroItens"></param>
        /// <param name="sentido"></param>
        /// <returns></returns>
        public static string montaMensagemHomeBrokerLOF(List<LOFDadosOferta> livroOferta, int numeroItens, int sentido)
        {
            StringBuilder mensagem = new StringBuilder();

            String side = (sentido == LIVRO_COMPRA) ? "C" : "V";
            int posicaoItem = 0;

            while (posicaoItem < numeroItens &&
                    posicaoItem < livroOferta.Count)
            {
                LOFDadosOferta itemOferta = livroOferta[posicaoItem];

                mensagem.Append(side);
                mensagem.Append(String.Format("{0:d8}", itemOferta.Corretora));
                if (itemOferta.Preco == ConstantesMDS.PRECO_LIVRO_OFERTAS_MIN_VALUE || itemOferta.Preco == ConstantesMDS.PRECO_LIVRO_OFERTAS_MAX_VALUE)
                    mensagem.Append(String.Format("{0,-13}", ConstantesMDS.DESCRICAO_OFERTA_ABERTURA));
                else
                    mensagem.Append(itemOferta.Preco.ToString("0000000000.00"));
                mensagem.Append(String.Format("{0:d12}", itemOferta.Quantidade));

                posicaoItem++;
            }

            mensagem.Replace('.', ',');

            return mensagem.ToString();
        }

        public static void insereOfertaLOF(String instrumento, List<LOFDadosOferta> livro, LOFDadosOferta oferta, int sentido)
        {
            String descricaoSentido = (sentido == LIVRO_VENDA ? "Compra" : "Venda");

            if (logger.IsDebugEnabled)
            {
                logger.DebugFormat("LOF [{0}]: Insere oferta {1} [{2}] [{3}] [{4}] [{5}]",
                    instrumento, descricaoSentido, oferta.IDOferta, oferta.Preco, oferta.Quantidade, oferta.Posicao);
            }

            // Insere a oferta no livro
            livro.Insert(oferta.Posicao, oferta);
        }

        public static void insereOfertaLOA(String instrumento, List<LOAGrupoOfertas> livro, LOAGrupoOfertas oferta, int sentido)
        {
            String descricaoSentido = (sentido == LIVRO_VENDA ? "Compra" : "Venda");
            
            if (logger.IsDebugEnabled)
            {
                logger.DebugFormat("LOA [{0}]: Insere oferta {1} [{2}] [{3}] [{4}] [{5}]",
                    instrumento, descricaoSentido, oferta.Preco, oferta.Quantidade, oferta.Indice, oferta.QtdeOrdens);
            }

            // Insere a oferta no livro
            livro.Insert(oferta.Indice, oferta);
        }

        public static void removeOfertaLOF(String instrumento, List<LOFDadosOferta> livro, LOFDadosOferta oferta, int sentido)
        {
            String descricaoSentido = (sentido == LIVRO_VENDA ? "Compra" : "Venda");

            if (logger.IsDebugEnabled)
            {
                logger.DebugFormat("LOF [{0}]: Remove oferta {1} [{2}] [{3}]",
                    instrumento, descricaoSentido, oferta.Posicao, oferta.IDOferta);
            }

            livro.RemoveAt(oferta.Posicao);
        }

        public static void removeOfertaLOA(String instrumento, List<LOAGrupoOfertas> livro, LOAGrupoOfertas oferta, int sentido)
        {
            String descricaoSentido = (sentido == LIVRO_VENDA ? "Compra" : "Venda");

            if (logger.IsDebugEnabled)
            {
                logger.DebugFormat("LOA [{0}]: Remove oferta {1} [{2}] [{3}]",
                    instrumento, descricaoSentido, oferta.Indice, oferta.QtdeOrdens);
            }

            livro.RemoveAt(oferta.Indice);
        }

        public static void alteraOfertaLOF(String instrumento, List<LOFDadosOferta> livro, LOFDadosOferta oferta, int sentido)
        {
            LOFDadosOferta ofertaVelha = livro[oferta.Posicao];
            ofertaVelha.Posicao = oferta.Posicao;

            String descricaoSentido = (sentido == LIVRO_VENDA ? "Compra" : "Venda");

            // Se for alteracao da oferta sem mudar o preco
            if (ofertaVelha.Preco.CompareTo(oferta.Preco) == 0)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat("LOF [{0}]: Altera oferta {1} [{2}] [{3}] [{4}] [{5}]",
                        instrumento, descricaoSentido, oferta.IDOferta, oferta.Preco, oferta.Quantidade, oferta.Posicao);
                }

                livro[oferta.Posicao] = oferta;
            }
            else
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat("LOF [{0}]: Altera oferta {1} [{2}] [{3}] --> [{4}] [{5}]",
                        instrumento, descricaoSentido, ofertaVelha.IDOferta, ofertaVelha.Preco, oferta.IDOferta, oferta.Preco);
                }

                removeOfertaLOF(instrumento, livro, ofertaVelha, sentido);
                insereOfertaLOF(instrumento, livro, oferta, sentido);
            }
        }

        public static void alteraOfertaLOA(String instrumento, List<LOAGrupoOfertas> livro, LOAGrupoOfertas oferta, int sentido)
        {
            LOAGrupoOfertas ofertaVelha = livro[oferta.Indice];
            ofertaVelha.Indice = oferta.Indice;

            String descricaoSentido = (sentido == LIVRO_VENDA ? "Compra" : "Venda");

            // Se for alteracao da oferta sem mudar o preco
            if (ofertaVelha.Preco.CompareTo(oferta.Preco) == 0)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat("LOA [{0}]: Altera oferta {1} [{2}] [{3}] [{4}] [{5}]",
                        instrumento, descricaoSentido, oferta.Preco, oferta.Quantidade, oferta.Indice, oferta.QtdeOrdens);
                }

                livro[oferta.Indice] = oferta;
            }
            else
            {
                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat("LOA [{0}]: Altera oferta {1} [{2}] [{3}] [{4}] --> [{5}] [{6}] [{7}]",
                        instrumento, descricaoSentido, ofertaVelha.Quantidade, ofertaVelha.Preco, ofertaVelha.QtdeOrdens,
                        oferta.Quantidade, oferta.Preco, oferta.QtdeOrdens);
                }

                removeOfertaLOA(instrumento, livro, ofertaVelha, sentido);
                insereOfertaLOA(instrumento, livro, oferta, sentido);
            }
        }

        public static void removeOfertaEMelhoresLOF(
            String instrumento,
            List<LOFDadosOferta> livro,
            LOFDadosOferta oferta,
            int sentido,
            List<LOFDadosOferta> listaLOF)
        {
            String descricaoSentido = (sentido == LIVRO_VENDA ? "Compra" : "Venda");

            if (logger.IsDebugEnabled)
            {
                logger.DebugFormat("LOF [{0}]: Remove oferta {1} [{2}] e melhores",
                    instrumento, descricaoSentido, oferta.Posicao);
            }

            for (int posdel = oferta.Posicao; posdel >= 0; posdel--)
            {
                LOFDadosOferta ofertaremovida = livro[posdel];
                ofertaremovida.Posicao = posdel;
                livro.RemoveAt(posdel);

                listaLOF.Add(ofertaremovida);
            }
        }

        public static void removeOfertaEMelhoresLOA(
            String instrumento,
            List<LOAGrupoOfertas> livro,
            LOAGrupoOfertas oferta,
            int sentido,
            List<LOAGrupoOfertas> listaLOA)
        {
            String descricaoSentido = (sentido == LIVRO_VENDA ? "Compra" : "Venda");

            if (logger.IsDebugEnabled)
            {
                logger.DebugFormat("LOA [{0}]: Remove oferta {1} [{2}] e melhores",
                    instrumento, descricaoSentido, oferta.Indice);
            }

            for (int posdel = oferta.Indice; posdel >= 0; posdel--)
            {
                LOAGrupoOfertas ofertaremovida = livro[posdel];
                ofertaremovida.Indice = posdel;
                livro.RemoveAt(posdel);

                listaLOA.Add(ofertaremovida);
            }
        }

        public static void removeOfertaEPioresLOF(
            String instrumento,
            List<LOFDadosOferta> livro,
            LOFDadosOferta oferta,
            int sentido,
            List<LOFDadosOferta> listaLOF)
        {
            String descricaoSentido = (sentido == LIVRO_VENDA ? "Compra" : "Venda");

            if (logger.IsDebugEnabled)
            {
                logger.DebugFormat("LOF [{0}]: Remove oferta {1} [{2}] e piores",
                    instrumento, descricaoSentido, oferta.Posicao);
            }
            
            for (int posdel = livro.Count - 1; posdel >= oferta.Posicao; posdel--)
            {
                LOFDadosOferta ofertaremovida = livro[posdel];
                ofertaremovida.Posicao = posdel;
                livro.RemoveAt(posdel);
                listaLOF.Add(ofertaremovida);
            }
        }

        public static void removeOfertaEPioresLOA(
            String instrumento,
            List<LOAGrupoOfertas> livro,
            LOAGrupoOfertas oferta,
            int sentido,
            List<LOAGrupoOfertas> listaLOA)
        {
            String descricaoSentido = (sentido == LIVRO_VENDA ? "Compra" : "Venda");

            if (logger.IsDebugEnabled)
            {
                logger.DebugFormat("LOA [{0}]: Remove oferta {1} [{2}] e piores",
                    instrumento, descricaoSentido, oferta.Indice);
            }

            for (int posdel = livro.Count - 1; posdel >= oferta.Indice; posdel--)
            {
                LOAGrupoOfertas ofertaremovida = livro[posdel];
                ofertaremovida.Indice = posdel;
                livro.RemoveAt(posdel);
                listaLOA.Add(ofertaremovida);
            }
        }

        public static void resetLivroLOF(String instrumento, List<LOFDadosOferta> livro, List<LOFDadosOferta> listaLOF)
        {
            logger.InfoFormat("LOF [{0}]: Reset Livro", instrumento);

            for (int i = 0; i < livro.Count; i++)
            {
                LOFDadosOferta oferta = livro[i];
                oferta.Posicao = i;
                listaLOF.Add(oferta);
                livro.RemoveAt(i);
            }
        }

        public static void resetLivroLOA(String instrumento, List<LOAGrupoOfertas> livro, List<LOAGrupoOfertas> listaLOA)
        {
            logger.InfoFormat("LOA [{0}]: Reset Livro", instrumento);
            for (int i = 0; i < livro.Count; i++)
            {
                LOAGrupoOfertas oferta = livro[i];
                oferta.Indice = i;
                listaLOA.Add(oferta);
                livro.RemoveAt(i);
            }
        }
        
        /**
         * @return the livroCompra
         */
	    public List<LOFDadosOferta> getLivroCompra() {
		    return livroCompra;
	    }


	    /**
	     * @param livroCompra the livroCompra to set
	     */
	    public void setLivroCompra(List<LOFDadosOferta> livroCompra) {
		    this.livroCompra = livroCompra;
	    }

	    /**
	     * @return the livroVenda
	     */
	    public List<LOFDadosOferta> getLivroVenda() {
		    return livroVenda;
	    }


	    /**
	     * @param livroVenda the livroVenda to set
	     */
	    public void setLivroVenda(List<LOFDadosOferta> livroVenda) {
		    this.livroVenda = livroVenda;
	    }


	    /**
	     * @return the agregadoCompra
	     */
	    public List<LOAGrupoOfertas> getAgregadoCompra() {
		    return agregadoCompra;
	    }


	    /**
	     * @param agregadoCompra the agregadoCompra to set
	     */
	    public void setAgregadoCompra(List<LOAGrupoOfertas> agregadoCompra) {
		    this.agregadoCompra = agregadoCompra;
	    }


	    /**
	     * @return the agregadoVenda
	     */
	    public List<LOAGrupoOfertas> getAgregadoVenda() {
		    return agregadoVenda;
	    }


	    /**
	     * @param agregadoVenda the agregadoVenda to set
	     */
	    public void setAgregadoVenda(List<LOAGrupoOfertas> agregadoVenda) {
		    this.agregadoVenda = agregadoVenda;
	    }


	    public const int LIVRO_COMPRA = 0;
	    public const int LIVRO_VENDA = 1;
    }
}
