using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.DbLib.Persistencias
{
    public static class CMSCache
    {
        #region Propriedades

        private static Dictionary<string, CMSCachePagina> _Paginas = new Dictionary<string, CMSCachePagina>();

        public static Dictionary<string, CMSCachePagina> Paginas
        {
            get
            {
                return _Paginas;
            }

            set
            {
                _Paginas = value;
            }
        }

        private static Dictionary<int, List<EstruturaInfo>> _Estruturas = new Dictionary<int, List<EstruturaInfo>>();

        public static Dictionary<int, List<EstruturaInfo>> Estruturas
        {
            get
            {
                return _Estruturas;
            }

            set
            {
                _Estruturas = value;
            }
        }

        #endregion

        #region Métodos Públicos

        public static string GerarIdCache(int pIdPagina, int pIdEstrutura)
        {
            return string.Format("{0}-{1}", pIdPagina, pIdEstrutura);
        }

        public static void AdicionarOuAtualizar(int pIdPagina, List<EstruturaInfo> pEstrutura)
        {
            if (!CMSCache.Estruturas.ContainsKey(pIdPagina))
            {
                CMSCache.Estruturas.Add(pIdPagina, pEstrutura);
            }
            else
            {
                CMSCache.Estruturas[pIdPagina] = pEstrutura;
            }
        }

        public static void AdicionarOuAtualizar(int pIdPagina, int pIdEstrutura, string pHTML)
        {
            string lIdCache = CMSCache.GerarIdCache(pIdPagina, pIdEstrutura);

            CMSCachePagina lPagina = new CMSCachePagina(pHTML);

            if (!CMSCache.Paginas.ContainsKey(lIdCache))
            {
                CMSCache.Paginas.Add(lIdCache, lPagina);
            }
            else
            {
                CMSCache.Paginas[lIdCache] = lPagina;
            }
        }

        public static bool ContemHTML(int pIdPagina, int pIdEstrutura, out string pHTML)
        {
            string lIdCache = CMSCache.GerarIdCache(pIdPagina, pIdEstrutura);

            if (!CMSCache.Paginas.ContainsKey(lIdCache))
            {
                pHTML = string.Empty; 

                return false;
            }
            else
            {
                pHTML = CMSCache.Paginas[lIdCache].HTML;

                return true;
            }
        }

        public static void LimparCache(int pIdPagina)
        {
            if (CMSCache.Estruturas.ContainsKey(pIdPagina))
            {
                string lNovoId;

                foreach (EstruturaInfo lLista in CMSCache.Estruturas[pIdPagina])
                {
                    lNovoId = CMSCache.GerarIdCache(lLista.CodigoPagina, lLista.CodigoEstrutura);

                    CMSCache.Paginas.Remove(lNovoId);
                }

                CMSCache.Estruturas.Remove(pIdPagina);
            }
        }

        #endregion
    }

    public class CMSCachePagina
    {
        public string HTML { get; set; }

        public int IdPagina { get; set; }

        public int IdEstrutura { get; set; }

        public DateTime DataAdicionada { get; set; }

        public double MinutosEmCache
        {
            get
            {
                return new TimeSpan(DateTime.Now.Ticks - DataAdicionada.Ticks).TotalMinutes;
            }
        }

        public CMSCachePagina(string pHTML)
        {
            this.HTML = pHTML;

            this.DataAdicionada = DateTime.Now;
        }
    }
}
