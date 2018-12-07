using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Site.DbLib.Dados;
using System.Text.RegularExpressions;

namespace Gradual.Site.Www
{
    public class TransportePaginaConteudoInfo_Busca
    {
        #region Propriedades

        public string Texto { get; set; }

        public string NomePagina { get; set; }

        public string DescURL { get; set; }

        public string LinkPara { get; set; }

        public string Titulo { get; set; }

        public string LinkParaArquivo { get; set; }

        public string ConteudoHTML { get; set; }

        public string AtributoSrc { get; set; }

        public string DsConteudo { get; set; }

        public string DescHtml { get; set; }

        #endregion

        #region Métodos Private

        private string BuscarTagDoTexto(ref int pPosicao)
        {
            string lRetorno = "";

            int lTotal = pPosicao + 40;

            for (var a = (pPosicao + 1); a < lTotal; a++)
            {
                if (this.DescHtml.Length <= lTotal)
                    break;

                if (this.DescHtml[a] == ' ')
                    break;

                if (this.DescHtml[a] == '>')
                    break;

                if (this.DescHtml[a] != '/')
                {
                    lRetorno += this.DescHtml[a];
                }
            }

            return lRetorno;
        }

        private void ConsertarTags()
        {
            /*
            List<string> lAbertas = new List<string>();
            List<string> lFechadas = new List<string>();

            string lTag;

            for (int a = 0; a < this.DescHtml.Length; a++)
            {
                if (this.DescHtml[a] == '<')
                {
                    if (this.DescHtml[a + 1] == '/')
                    {
                        //é uma tag que fechou
                        lTag = BuscarTagDoTexto(ref a);

                        lFechadas.Add(lTag);
                    }
                    else
                    {
                        //é uma tag que abriu
                        lTag = BuscarTagDoTexto(ref a);

                        lAbertas.Add(lTag);
                    }
                }
            }

            int lDiferenca;

            if (lAbertas.Count > lFechadas.Count)
            {
                lDiferenca = lAbertas.Count - lFechadas.Count;

                for (var a = 0; a < lDiferenca; a++)
                {
                    this.DescHtml += string.Format("</{0}>", lAbertas[a]);  //very unlikely to work, but it's a patch anyway... let's hope.
                }
            }

            if (lFechadas.Count > lAbertas.Count)
            {
                lDiferenca = lFechadas.Count - lAbertas.Count;

                for (var a = 0; a < lDiferenca; a++)
                {
                    this.DescHtml = string.Format("<{0}>", lFechadas[a]) + this.DescHtml;  //very unlikely to work, but it's a patch anyway... let's hope.
                }
            }

            */
        }

        #endregion

        public TransportePaginaConteudoInfo_Busca()
        {
        }

        public bool CompletarDados(PaginaBuscaInfo pInfo, ref List<PaginaInfo> pPaginas)
        {
            foreach (PaginaInfo lPagina in pPaginas)
            {
                if (lPagina.CodigoPagina.HasValue)
                {
                    if (lPagina.CodigoPagina.Value == pInfo.IdPagina)
                    {
                        this.DescURL    = lPagina.DescURL;
                        this.NomePagina = lPagina.NomePagina;
                        this.DescHtml   = pInfo.ResumoHTML;

                        ConsertarTags();

                        return true;
                    }
                }
            }

            return false;
        }
    }
}