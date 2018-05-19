using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using log4net;
using System.Globalization;
using System.Collections.Concurrent;
using System.Text;

namespace Gradual.OMS.Cotacao
{
    /// <summary>
    /// Classe responsável por armazenas as estruturas de Livro de Oferta e Mensagem de Negocios para o sistema Homebroker.   
    /// </summary>

    [Serializable]
    public static class MemoriaCotacao
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Globais

        private const string DESTAQUES = "DESTAQUES";

        #endregion

        #region Propriedades (membros públicos)

        public static int NumeroItemHash
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["NumeroItemsHash"]);
            }
        }

        public static DateTime LastPacket { get; set; }
        public static DateTime LastNegocioPacket { get; set; }
        public static DateTime LastLofPacket { get; set; }
        public static DateTime LastDestaquePacket { get; set; }
        public static DateTime LastRankingPacket { get; set; }
        public static DateTime LastAgregadoPacket { get; set; }
        public static string LastMsg { get; set; }
        public static string LastNegocioMsg { get; set; }
        public static string LastLofMsg { get; set; }
        public static string LastDestaqueMsg { get; set; }
        public static string LastRankingMsg { get; set; }
        public static string LastSondaMsg { get; set; }
        public static string LastAgregadoMsg { get; set; }


        /// <summary>
        /// HashTable que armazena todas as mensagens do cotação em tempo real 
        /// </summary>
        public static ConcurrentDictionary<string, List<string>> hstCotacoes = new ConcurrentDictionary<string, List<string>>();
        //public static Hashtable hstCotacoes = new Hashtable();

        //public static Hashtable hstDadosPapel = new Hashtable();
        public static ConcurrentDictionary<string, string> hstDadosPapel = new ConcurrentDictionary<string, string>();

        public static ConcurrentDictionary<string, string> hstCotacoesIndiceGradual = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// HashTable que armazena os destaques dos papeis do IBOV.
        /// </summary>
        public static Hashtable hstDestaquesIbovespa = new Hashtable();

        /// <summary>
        /// HashTable que armazena os ranks de corretora dos papeis da bolsa.
        /// </summary>
        public static Hashtable hstRankCorretora = new Hashtable();

        /// <summary>
        /// HashTable que armazena todas as mensagens de livro de ofertas em tempo real
        /// </summary>
        public static ConcurrentDictionary<string, string> hstLivroOferta = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Hashtable com dados de exercicos de opcoes ativas/validas (dtvenc >= data atual)
        /// </summary>
        //public static Hashtable hstDadosOpcoes = new Hashtable();
        public static ConcurrentDictionary<string, string> hstDadosOpcoes = new ConcurrentDictionary<string, string>();

        public static DateTime HorarioUltimaSonda { get; set; }

        /// <summary>
        /// HashTable que armazena todas as mensagens de livro de ofertas agregado em tempo real
        /// </summary>
        //public static Hashtable hstLivroAgregado = new Hashtable();
        public static ConcurrentDictionary<string, string> hstLivroAgregado = new ConcurrentDictionary<string, string>();
        #endregion

        #region Métodos Públicos

        /// <summary>
        ///  Adiciona o ultimo ticker de rank de corretora par um determinado papel;
        /// </summary>
        /// <param name="Instrumento"> Nome do Instrumento </param>
        /// <param name="Mensagem">Mensagem de Cotação </param>
        public static void AdicionarTickerRankCorretora(string Instrumento, string Mensagem)
        {
            try
            {
                lock (hstRankCorretora)
                {
                    hstRankCorretora.Remove(Instrumento);
                    hstRankCorretora.Add(Instrumento, Mensagem);
                }
            }
            catch (KeyNotFoundException knfEx)
            {
                throw knfEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  Adiciona o ultimo Tiker de cotação enviado pelo CEP
        /// </summary>
        /// <param name="Instrumento"> Nome do Instrumento </param>
        /// <param name="Mensagem">Mensagem de Cotação </param>
        public static void AdicionarNegocio(string Instrumento, string Mensagem)
        {
            try
            {
                List<string> livroNegocios = hstCotacoes.GetOrAdd(Instrumento, new List<string>());
                if (livroNegocios.Count == 0)
                    livroNegocios.Insert(0, Mensagem);
                else
                    livroNegocios[0] = Mensagem;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro Negocio [" + Mensagem + "] " + ex.Message, ex);
            }
        }

        /// <summary>
        ///  Adiciona o ultimo livro de negocios enviado pelo CEP
        /// </summary>
        public static void AdicionarLivroNegocios(string Instrumento, string Mensagem)
        {
            try
            {
                // Mantém sempre o 1o.item do Livro de Negocios como o ultimo ticker da cotação
                // Os itens de 2 a 11 representa o Livro de Negocios
                List<string> livroNegocios = hstCotacoes.GetOrAdd(Instrumento, new List<string>());
                livroNegocios.RemoveRange(1, livroNegocios.Count - 1);

                int numNeg = 0;
                StringBuilder msgNeg = new StringBuilder();
                string dataHoraAtual = DateTime.Now.ToString("yyyyMMddHHmmss000");
                int pos = 41;
                for (int i = 0; i < 10; i++)
                {
                    if (Mensagem.Length <= pos + (i * 55))
                        break;
                    string ocor = Mensagem.Substring(pos + (i * 55), 55);
                    int.TryParse(ocor.Substring(0, 8), out numNeg);
                    if (numNeg == 0)
                        continue;

                    msgNeg = new StringBuilder();
                    msgNeg.Append("NE");
                    msgNeg.Append(Mensagem.Substring(2, 2));
                    msgNeg.Append(dataHoraAtual);
                    msgNeg.Append(Mensagem.Substring(21, 20));
                    msgNeg.Append(Mensagem.Substring(4, 17));
                    msgNeg.Append(ocor.Substring(39, 8));
                    msgNeg.Append(ocor.Substring(47, 8));
                    msgNeg.Append(ocor.Substring(15, 12).Replace('.', ',') + "0");
                    msgNeg.Append(ocor.Substring(27, 12));
                    msgNeg.Append("000000000,000");
                    msgNeg.Append("000000000,000");
                    msgNeg.Append("000000000,000");
                    msgNeg.Append(ocor.Substring(0, 8));
                    msgNeg.Append(" 00000,00");
                    msgNeg.Append("2");
                    livroNegocios.Insert(i + 1, msgNeg.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro LivroNegocio [" + Mensagem + "] " + ex.Message, ex);
            }
        }

        /// <summary>
        ///  Adiciona o ultimo Tiker de cotação enviado pelo CEP (apenas cotacao em negociacao)
        /// </summary>
        /// <param name="Instrumento"> Nome do Instrumento </param>
        /// <param name="Mensagem">Mensagem de Cotação </param>
        public static void AdicionarNegocioIndiceGradual(string Instrumento, string Mensagem)
        {
            try
            {
                int status = int.Parse(Mensagem.Substring(155, 1));
                if (status == 2)
                {
                    hstCotacoesIndiceGradual.AddOrUpdate(Instrumento,  Mensagem, (key,oldvalue) => Mensagem);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AdicionarNegocioIndiceGradual() Msg[" + Mensagem + "] " + ex.Message, ex);
            }
        }

        //static void AdicionaValores(string Instrumento,string Mensagem)
        //{
        //    List<string> Valores = (List<string>)(hstCotacoes[Instrumento]);

        //    AddList(Valores, Instrumento, Mensagem);
        //}

        //public static void AddList(List<string> Lista, string Instrumento,string Mensagem)
        //{
        //    int count = Lista.Count;

        //    if (count == NumeroItemHash)
        //    {
        //        Lista.RemoveAt(0);
        //    }

        //    Lista.Add(Mensagem);
        //}

        /// <summary>
        /// Adiciona o ultimo Tiker dos destaques ( Rank de papeis ) da Bolsa.
        /// </summary>        
        /// <param name="Mensagem"> Mensagem de destaques </param>
        public static void AdicionarTikerDestaques(string Mensagem)
        {
            try
            {
                lock (hstDestaquesIbovespa)
                {
                    hstDestaquesIbovespa.Remove(DESTAQUES);
                    hstDestaquesIbovespa.Add(DESTAQUES, Mensagem);
                }
            }
            catch (KeyNotFoundException knfEx)
            {
                throw knfEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Adiciona o ultimo Tiker dos destaques ( Rank de papeis ) da Bolsa.
        /// </summary>        
        /// <param name="Mensagem"> Mensagem de destaques </param>
        public static void AdicionarTikerRankCorretora(string Instrumento, string Mensagem)
        {
            try
            {
                lock (hstRankCorretora)
                {
                    hstRankCorretora.Remove(Instrumento);
                    hstRankCorretora.Add(Instrumento, Mensagem);
                }
            }
            catch (KeyNotFoundException knfEx)
            {
                throw knfEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Adiciona o ultimo Tiker de livro de oferta enviado pelo CEP
        /// </summary>
        /// <param name="Instrumento">  Nome do Instrumento </param>
        /// <param name="Mensagem"> Mensagem de Cotação </param>
        public static void AdicionarTikerLivroOferta(string Instrumento, string Mensagem)
        {
            try
            {
                hstLivroOferta.AddOrUpdate(Instrumento, Mensagem, (key,oldvalue) => Mensagem);
                //lock (hstLivroOferta)
                //{
                //    hstLivroOferta.Remove(Instrumento);
                //    hstLivroOferta.Add(Instrumento, Mensagem);
                //}
            }
            catch (KeyNotFoundException knfEx)
            {
                throw knfEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        ///  Retorna um ticker com os destaque do IBOV
        /// </summary>       
        /// <returns>Ticker com os destaques do Indice Bovespa</returns>
        public static string ReceberDestaques()
        {
            lock (hstDestaquesIbovespa)
            {
                if (hstDestaquesIbovespa[DESTAQUES] != null)
                {
                    return hstDestaquesIbovespa[DESTAQUES].ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        ///  Retorna um ticker com o rank de corretoras.
        /// </summary>
        /// <param name="Instrumento">Código do Instrumento</param>
        /// <returns>Ticker com o rank de corretoras </returns>
        public static string ReceberRankCorretora(string instrumento)
        {
            lock (hstRankCorretora)
            {
                if (hstRankCorretora[instrumento] != null)
                {
                    return hstRankCorretora[instrumento].ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        ///  Retorna um ticker de Livro de Ofertas para o Instrumento solicitado
        /// </summary>
        /// <param name="Instrumento">Código do Instrumento</param>
        /// <returns>Ticker de Cotação do instrumento requisitado</returns>
        public static string ReceberBook(string Instrumento)
        {
            string retorno;

            if ( hstLivroOferta.TryGetValue(Instrumento, out retorno) )
            {
                return retorno;
            }

            return string.Empty;
        }

        public static void ProcessaSonda(string Instrumento, string Mensagem)
        {

            //SDBV20110627151711021SONDA               20110627151636000
            //012345678901234567890123456789012345678901234567890123456789
            //          1         2         3         4         5
            if (Mensagem.Length > 41)
            {
                string horario = Mensagem.Substring(41, 14);
                if (!horario.Equals("00000000000000"))
                {
                    DateTime agora = DateTime.ParseExact(horario, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    if ( agora.CompareTo(HorarioUltimaSonda) > 0 )
                        HorarioUltimaSonda = agora;
                }
            }
        }

        /// <summary>
        ///  Retorna um ticker de Livro de Ofertas Agregado para o Instrumento solicitado
        /// </summary>
        /// <param name="Instrumento">Código do Instrumento</param>
        /// <returns>Ticker com os precos do instrumento requisitado</returns>
        public static string ReceberAgregado(string Instrumento)
        {
            string retorno = string.Empty;

            hstLivroAgregado.TryGetValue(Instrumento, out retorno);

            return retorno;
        }

        /// <summary>
        /// Adiciona o ultimo Tiker de livro agregado enviado pelo MDS
        /// </summary>
        /// <param name="Instrumento">Nome do Instrumento </param>
        /// <param name="Mensagem">Mensagem de Livro Agregado</param>
        public static void AdicionarTikerLivroAgregado(string Instrumento, string Mensagem)
        {
            try
            {
                hstLivroAgregado.AddOrUpdate(Instrumento, Mensagem, (key, oldvalue) => Mensagem);
            }
            catch (KeyNotFoundException knfEx)
            {
                throw knfEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
