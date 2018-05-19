using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.espertech.esper.client;
using com.espertech.esper.compat;
using com.espertech.esper.compat.collections;
using Gradual.OMS.Automacao.Lib;

namespace Gradual.OMS.AutomacaoDesktop
{
    public class DadosGlobais
    {
        private SortedDictionary<string, DadosInstrumentos> mapaInstrumento;
        private HashSet<string> listaComposicaoIndice;
        //private SortedDictionary<string, DadosGrupoCotacao> listaGrupoCotacao;
        private SortedDictionary<string, BovespaLivroOfertas> todosLivrosPersistencia;
        //private SortedDictionary<DadosRanking, string> maioresVolumes;
        //private SortedDictionary<string, DadosRanking> maioresVolumesPorMsgID;
        //private SortedDictionary<DadosRanking, string> maisNegociadas;
        //private SortedDictionary<string, DadosRanking> maisNegociadasPorMsgID;
        //private SortedDictionary<DadosRanking, string> maioresBaixas;
        //private SortedDictionary<DadosRanking, string> maioresAltas;
        //private SortedDictionary<string, DadosRanking> maioresMenoresPorMsgID;
        private string ultimoMsgIdLivroOfertasBovespa;
        private LinkedBlockingQueue<string> filaFeeder;

        /// <summary>
        /// Dicionario com todos os livros de ofertas de todos os papeis Bovespa
        /// </summary>
        public SortedDictionary<string, BovespaLivroOfertas> TodosLOF { get; set; }

        /// <summary>
        /// Dicionario de livros de ofertas BMF
        /// </summary>
        public SortedDictionary<string, BMFLivroOfertas> TodosLivrosBMF {get;set;}

        public DadosGlobais()
        {
            mapaInstrumento = new SortedDictionary<string, DadosInstrumentos>();
            listaComposicaoIndice =  new HashSet<string>();
            //listaGrupoCotacao = new SortedDictionary<string, DadosGrupoCotacao>();
            //todosLivrosBMF =  new SortedDictionary<string, BMFLivroOfertas>();
            TodosLOF = new SortedDictionary<string, BovespaLivroOfertas>();
            TodosLivrosBMF = new SortedDictionary<string, BMFLivroOfertas>();
            todosLivrosPersistencia = new SortedDictionary<string, BovespaLivroOfertas>();
            //maioresVolumes = new SortedDictionary<DadosRanking, string>(new ComparadorRankingDecrescente());
            //maioresVolumesPorMsgID = new SortedDictionary<string, DadosRanking>();
            //maisNegociadas = new SortedDictionary<DadosRanking, string>(new ComparadorRankingDecrescente());
            //maisNegociadasPorMsgID = new SortedDictionary<string, DadosRanking>();
            //maioresBaixas = new SortedDictionary<DadosRanking, string>();
            //maioresAltas = new SortedDictionary<DadosRanking, string>(new ComparadorRankingDecrescente());
            //maioresMenoresPorMsgID = new SortedDictionary<string, DadosRanking>();
            filaFeeder = new LinkedBlockingQueue<string>();
        }

        public void inicializaComposicaoIndice()
        {
            listaComposicaoIndice.Clear();
        }

        //public void inicializaGrupoCotacao()
        //{
        //    listaGrupoCotacao.clear();
        //}

        //public void inicializaRankingPapeis()
        //{
        //    maioresVolumes.clear();
        //    maioresVolumesPorMsgID.clear();
        //    maisNegociadas.clear();
        //    maisNegociadasPorMsgID.clear();
        //    maioresBaixas.clear();
        //    maioresAltas.clear();
        //    maioresMenoresPorMsgID.clear();
        //}

        /// <summary>
        /// Parametros da configuracao (arquivo .config)
        /// </summary>
        public AutomacaoConfig Parametros { get;set;}

        /// <summary>
        /// Instancia do NEsper
        /// </summary>
        public EPServiceProvider EpService { get;set;}

        /// <summary>
        /// Flag que indica se as threads devem ficar executando ou finalizar
        /// </summary>
        public bool KeepRunning { get; set; } 


        public SortedDictionary<string, DadosInstrumentos> getMapaInstrumento()
        {
            return mapaInstrumento;
        }

        public void setMapaInstrumento(SortedDictionary<string, DadosInstrumentos> mapaInstrumento)
        {
            this.mapaInstrumento = mapaInstrumento;
        }

        public HashSet<string> getListaComposicaoIndice()
        {
            return listaComposicaoIndice;
        }

        public void setListaComposicaoIndice(HashSet<string> listaComposicaoIndice)
        {
            this.listaComposicaoIndice = listaComposicaoIndice;
        }

        public string LastMdgIDBov { get; set; }

        //public Dictionary<string, DadosGrupoCotacao> getListaGrupoCotacao()
        //{
        //    return listaGrupoCotacao;
        //}

        //public void setListaGrupoCotacao(Dictionary<string, DadosGrupoCotacao> listaGrupoCotacao)
        //{
        //    this.listaGrupoCotacao = listaGrupoCotacao;
        //}

        //public Dictionary<string, BMFLivroOfertas> getTodosLivrosBMF()
        //{
        //    return todosLivrosBMF;
        //}

        //public void setTodosLivrosBMF(Dictionary<string, BMFLivroOfertas> todosLivrosBMF)
        //{
        //    this.todosLivrosBMF = todosLivrosBMF;
        //}

        //public Dictionary<string, BovespaLivroOfertas> getTodosLivros()
        //{
        //    return todosLivros;
        //}

        //public void setTodosLivros(HashMap<string, BovespaLivroOfertas> todosLivros)
        //{
        //    this.todosLivros = todosLivros;
        //}

        //public Dictionary<string, BovespaLivroOfertas> getTodosLivrosPersistencia()
        //{
        //    return todosLivrosPersistencia;
        //}

        //public void setTodosLivrosPersistencia(HashMap<string, BovespaLivroOfertas> todosLivrosPersistencia)
        //{
        //    this.todosLivrosPersistencia = todosLivrosPersistencia;
        //}

        //public Dictionary<DadosRanking, string> getMaioresVolumes()
        //{
        //    return maioresVolumes;
        //}

        //public void setMaioresVolumes(SortedDictionary<DadosRanking, string> maioresVolumes)
        //{
        //    this.maioresVolumes = maioresVolumes;
        //}

        //public Dictionary<DadosRanking, string> getMaisNegociadas()
        //{
        //    return maisNegociadas;
        //}

        //public void setMaisNegociadas(SortedDictionary<DadosRanking, string> maisNegociadas)
        //{
        //    this.maisNegociadas = maisNegociadas;
        //}

        //public Dictionary<string, DadosRanking> getMaioresVolumesPorMsgID()
        //{
        //    return maioresVolumesPorMsgID;
        //}

        //public void setMaioresVolumesPorMsgID( SortedDictionary<string, DadosRanking> maioresVolumesPorMsgID)
        //{
        //    this.maioresVolumesPorMsgID = maioresVolumesPorMsgID;
        //}

        //public Dictionary<string, DadosRanking> getMaisNegociadasPorMsgID()
        //{
        //    return maisNegociadasPorMsgID;
        //}

        //public void setMaisNegociadasPorMsgID( SortedDictionary<string, DadosRanking> maisNegociadasPorMsgID)
        //{
        //    this.maisNegociadasPorMsgID = maisNegociadasPorMsgID;
        //}

        //public Dictionary<DadosRanking, string> getMaioresBaixas()
        //{
        //    return maioresBaixas;
        //}

        //public void setMaioresBaixas(SortedDictionary<DadosRanking, string> maioresBaixas)
        //{
        //    this.maioresBaixas = maioresBaixas;
        //}

        //public Dictionary<DadosRanking, string> getMaioresAltas()
        //{
        //    return maioresAltas;
        //}

        //public void setMaioresAltas(SortedDictionary<DadosRanking, string> maioresAltas)
        //{
        //    this.maioresAltas = maioresAltas;
        //}

        //public Dictionary<string, DadosRanking> getMaioresMenoresPorMsgID()
        //{
        //    return maioresMenoresPorMsgID;
        //}

        //public void setMaioresMenoresPorMsgID(SortedDictionary<string, DadosRanking> maioresMenoresPorMsgID)
        //{
        //    this.maioresMenoresPorMsgID = maioresMenoresPorMsgID;
        //}


        public LinkedBlockingQueue<string> getFilaFeeder()
        {
            return filaFeeder;
        }

        public void setFilaFeeder(LinkedBlockingQueue<string> filaFeeder)
        {
            this.filaFeeder = filaFeeder;
        }

    }
}
