using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Automacao.Lib;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using log4net;

namespace Gradual.OMS.AutomacaoDesktop
{
    public class PersistenciaMarketData
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DadosGlobais _dadosglobais;
        AutomacaoConfig _config;

        public PersistenciaMarketData(DadosGlobais dadosglobais)
        {
            _dadosglobais = dadosglobais;
            _config = dadosglobais.Parametros;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="livrosBovespa"></param>
        public void SaveLOFBovespa( )
        {
            string file = _config.DiretorioDB + "\\LofBov.dat";
            string bkpfile = _config.DiretorioDB + "\\LofBov.dat" + DateTime.Now.ToString("yyyyMMddHHmm");

            try
            {
                // Efetua backup do snapshot existente
                // (macaco véio)
                if (File.Exists(file))
                {
                    File.Copy(file, bkpfile, true);
                }

                // Copia o Livro pra uma lista
                List<KeyValuePair<string, BovespaLivroOfertas>> listaLivros = null;
                lock (_dadosglobais.TodosLOF)
                {
                    listaLivros = _dadosglobais.TodosLOF.ToList();
                }

                FileStream fs = File.Open(file, FileMode.OpenOrCreate, FileAccess.Write);
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(fs, _dadosglobais.LastMdgIDBov);

                long numLivros = listaLivros.Count;

                logger.Info("Serializando " + numLivros + " livros de ofertas");

                bformatter.Serialize(fs, numLivros);

                foreach (KeyValuePair<string, BovespaLivroOfertas> entry in listaLivros)
                {
                    string instrumento = entry.Key;

                    bformatter.Serialize(fs, instrumento);

                    BovespaLivroOfertas livro = entry.Value;

                    // Serializa o livro de compra
                    List<KeyValuePair<string, LivroOfertasEntry>> ofertasC = entry.Value.ToList(BovespaLivroOfertas.LIVRO_COMPRA);

                    long numOfertasCompras = ofertasC.Count;

                    bformatter.Serialize(fs, numOfertasCompras);

                    if (ofertasC.Count > 0)
                    {
                        foreach (KeyValuePair<string, LivroOfertasEntry> oferta in ofertasC)
                        {
                            bformatter.Serialize(fs, oferta.Key);
                            bformatter.Serialize(fs, oferta.Value);
                        }
                    }

                    // Serializa o livro de venda
                    List<KeyValuePair<string, LivroOfertasEntry>> ofertasV = entry.Value.ToList(BovespaLivroOfertas.LIVRO_VENDA);

                    long numOfertasVenda = ofertasV.Count;

                    bformatter.Serialize(fs, numOfertasVenda);

                    if (ofertasV.Count > 0)
                    {
                        foreach (KeyValuePair<string, LivroOfertasEntry> oferta in ofertasV)
                        {
                            bformatter.Serialize(fs, oferta.Key);
                            bformatter.Serialize(fs, oferta.Value);
                        }
                    }

                    logger.InfoFormat("Serializou {0} ofertas de compra, {1} de venda do instrumento {2}",
                        numOfertasCompras,
                        numOfertasVenda,
                        instrumento);

                }

                fs.Close();
            }
            catch (Exception ex)
            {
                logger.Error("Erro: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void LoadLOFBovespa()
        {
            string file = _config.DiretorioDB + "\\LofBov.dat";

            try
            {
                SortedDictionary<string, BovespaLivroOfertas> livrosDic = new SortedDictionary<string, BovespaLivroOfertas>();

                FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read);

                BinaryFormatter bformatter = new BinaryFormatter();

                string msgID = (string) bformatter.Deserialize(fs);

                long numLivros = (long) bformatter.Deserialize(fs);

                logger.Info("Existem " + numLivros + " livros a serem carregados");

                for (int i = 0; i < numLivros; i++)
                {
                    string instrumento = (string)bformatter.Deserialize(fs);

                    BovespaLivroOfertas livro = new BovespaLivroOfertas();

                    long numOfertasCompras = (long)bformatter.Deserialize(fs);
                    if (numOfertasCompras > 0)
                    {
                        List<KeyValuePair<string, LivroOfertasEntry>> ofertasC = new List<KeyValuePair<string, LivroOfertasEntry>>();

                        for (int j=0; j < numOfertasCompras; j++)
                        {
                            string key = (string)bformatter.Deserialize(fs);
                            LivroOfertasEntry entry = (LivroOfertasEntry)bformatter.Deserialize(fs);

                            KeyValuePair<string, LivroOfertasEntry> oferta = new KeyValuePair<string, LivroOfertasEntry>(key, entry);

                            ofertasC.Add(oferta);
                        }

                        livro.AddList(BovespaLivroOfertas.LIVRO_COMPRA, ofertasC);
                    }


                    long numOfertasVenda = (long)bformatter.Deserialize(fs);
                    if (numOfertasVenda > 0)
                    {
                        List<KeyValuePair<string, LivroOfertasEntry>> ofertasV = new List<KeyValuePair<string, LivroOfertasEntry>>();

                        for (int j=0; j < numOfertasVenda; j++)
                        {
                            string key = (string)bformatter.Deserialize(fs);
                            LivroOfertasEntry entry = (LivroOfertasEntry)bformatter.Deserialize(fs);

                            KeyValuePair<string, LivroOfertasEntry> oferta = new KeyValuePair<string, LivroOfertasEntry>(key, entry);

                            ofertasV.Add(oferta);
                        }

                        livro.AddList(BovespaLivroOfertas.LIVRO_VENDA, ofertasV);
                    }

                    logger.InfoFormat("Carregou {0} ofertas de compra, {1} de venda para instrumento {2}",
                        numOfertasCompras,
                        numOfertasVenda,
                        instrumento);

                    livrosDic.Add(instrumento, livro);
                }

                // Limpa o livro anterior e seta o novo livro
                lock (_dadosglobais.TodosLOF)
                {
                    _dadosglobais.TodosLOF.Clear();
                    _dadosglobais.TodosLOF = livrosDic;
                }

                _dadosglobais.LastMdgIDBov = msgID;

                fs.Close();
            }
            catch (Exception ex)
            {
                logger.Error("Erro: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="livrosBovespa"></param>
        public void SaveLOFBVMF(SortedDictionary<string, BMFLivroOfertas> livrosBmf)
        {

        }



    }
}
