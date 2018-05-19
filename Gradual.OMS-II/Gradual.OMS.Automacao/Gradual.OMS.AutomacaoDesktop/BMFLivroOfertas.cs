using System.Collections.Generic;
using Gradual.OMS.Automacao.Lib;
using System;
using System.Text;
using log4net;

namespace Gradual.OMS.AutomacaoDesktop
{
    public class BMFLivroOfertas
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<LivroOfertasEntry> LivrosCompraBMF;
        public List<LivroOfertasEntry> LivrosVendaBMF;

        /**
         * Construtor que inicializa as listas <livrosCompraBMF> e 
         * <livrosVendaBMF>, do tipo {@link LinkedList} e implementando
         * sincronização synchronizedList.
         *  
         */
        public BMFLivroOfertas()
        {
            LivrosCompraBMF = new List<LivroOfertasEntry>();
            LivrosVendaBMF = new List<LivroOfertasEntry>();
        }

        public void inicializaBMFLivrosOfertas()
        {
            LivrosCompraBMF.Clear();
            LivrosVendaBMF.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentido"></param>
        /// <returns></returns>
        public List<string> serializarLivro(int sentido)
        {
            // MUITO CUIDADO com a chamada desta rotina!!! Para altos
            // volumes, o tempo de processamento aumenta exponencialmente

            List<string> livro = new List<string>();

            try
            {
                List<LivroOfertasEntry> ofertas = null;
                
                if ( sentido == LIVRO_COMPRA )
                    ofertas = this.LivrosCompraBMF;
                else
                    ofertas = this.LivrosVendaBMF;


                livro.Add(("Livro de " + ((sentido == LIVRO_COMPRA) ? "Compra:" : "Venda:")).PadRight(30));
                livro.Add("-".PadLeft(30,'-'));

                IEnumerator<LivroOfertasEntry> itOfertas = ofertas.GetEnumerator();
                while (itOfertas.MoveNext())
                {
                    // itera o mapa de preços
                    LivroOfertasEntry entry = itOfertas.Current;

                    Decimal precoAtual = entry.Preco;

                    StringBuilder livroSerializado = new StringBuilder();

                    livroSerializado.Append( entry.Corretora + " - " +
                            entry.Preco.ToString() + " - " + 
                            entry.Quantidade.ToString("D10"));

                    livro.Add(livroSerializado.ToString());
                }

                livro.Add("-".PadLeft(30, '-'));
            }
            catch (Exception e)
            {
                logger.Error("Exception em serializarLivro: " + e.Message);
            }

            return livro;
        }

        public const int LIVRO_COMPRA = 0;
        public const int LIVRO_VENDA = 1;

    }
}
