using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using Gradual.Site.Lib.Dados;
using Newtonsoft.Json;

namespace Gradual.Site.Lib.Widgets
{
    public abstract class WidgetBase
    {
        #region Propriedades

        public int IdDaEstrutura { get; set; }

        public int IdDoWidget { get; set; }

        public int IdDaLista { get; set; }
        
        private string _Ordenacao;
        private string _DirecaoDeOrdenacao;

        /// <summary>
        /// Propriedade pela qual ordenar a lista, com " A" para crescente e " D" para decrescente adicionado no final
        /// </summary>
        public string Ordenacao
        {
            get
            {
                return _Ordenacao;
            }

            set
            {
                _Ordenacao = value;

                if (!string.IsNullOrEmpty(_Ordenacao))
                {
                    if (_Ordenacao.Contains(' '))
                    {
                        _DirecaoDeOrdenacao = _Ordenacao[_Ordenacao.Length - 1].ToString();
                    }
                }
            }
        }
        
        public string DirecaoDeOrdenacao { get; set; }

        [JsonIgnore]
        public string DescricaoDaLista { get; set; }

        public int OrdemNaPagina { get; set; }

        public string AtributoStyle { get; set; }

        public string AtributoClass { get; set; }

        public string Tipo {get; set;}

        [JsonIgnore]
        public bool RenderizarHabilitandoCMS { get; set; }

        [JsonIgnore]
        public string WidgetJSON { get; set; }

        #endregion

        #region Métodos Públicos

        public string ProcessarTextoParaHTML(string pTexto)
        {
            //substitui os textos que estejam "codificados" pra incluir formatação simples:
            /*
                *italics*                               =>  italics
                **bold**                                =>  bold
                [texto](http://wwww.site.com)           =>  <a href="http://wwww.site.com">texto</a>
                [img](http://wwww.site.com/imagem.jpg)  =>  <img src="http://wwww.site.com/imagem.jpg" />
                [texto](AlgumaClasse)                   =>  <span class="AlgumaClasse">texto</span>
             */

            string lOutPut = pTexto;

            string lParte1, lParte2, lParte2_href, lParte2_target, lParte2_class, lParte2_style, lParte2_onclick;

            string[] lParte2_Split;

            Regex lRegex;

            MatchCollection lMatches;

            //verifica se tem texto que seja link ou classe:

            lRegex = new Regex("\\[(.*?)\\]\\((.*?)\\)");

            lMatches = lRegex.Matches(lOutPut);

            foreach (Match lMatch in lMatches)
            {
                lParte2 = lMatch.Value.Substring(lMatch.Value.IndexOf('(') + 1, lMatch.Value.IndexOf(')') - lMatch.Value.IndexOf('(') - 1);
                lParte1 = lMatch.Value.Substring(lMatch.Value.IndexOf('[') + 1, lMatch.Value.IndexOf(']') - 1);

                if (lParte1 == "img")
                {
                }
                else
                {
                    lParte2_href    = "";
                    lParte2_target  = "";
                    lParte2_class   = "";
                    lParte2_style   = "";
                    lParte2_onclick = "";

                    lParte2_Split = lParte2.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    for(var a = 0; a < lParte2_Split.Length; a++)
                    {
                        lParte2 = lParte2_Split[a];

                        if (lParte2.Contains("http:") || lParte2.Contains("wwww") || lParte2.Contains(".com") || lParte2[0] == '/' || lParte2.IndexOf("..") == 0 || lParte2.Contains(".aspx") || lParte2.Contains(".htm"))
                        {
                            lParte2_href = lParte2;

                            if (lParte2_href.Contains(":_blank"))
                            {
                                lParte2_href = lParte2_href.Replace(":_blank", "");
                                lParte2_target = "_blank";
                            }
                        }
                        else if (lParte2.IndexOf("js:") == 0)
                        {
                            lParte2_href = "#";

                            string lFuncao = lParte2.Substring(3).Replace("{", "(").Replace("}", ")");        //se precisar de parametros pra funcao, tem que usar {} ao inves de () porque o parêntese vai zuar o parse anterior

                            if(lFuncao.IndexOf("(") == -1)  //os {} são opcionais... pode ficar só [texto do link]
                                lFuncao += "()";

                            lParte2_href    = "#";
                            lParte2_onclick = "return " + lFuncao;
                        }
                        else if (lParte2.Contains(":") || lParte2.Contains(";"))
                        {
                            lParte2_style = lParte2;
                        }
                        else
                        {
                            lParte2_class = lParte2;
                        }
                    }

                    if ( !string.IsNullOrEmpty(lParte2_href) )
                    {
                        lOutPut = lOutPut.Replace(lMatch.Value, string.Format("<a href='{0}'{1}{2}{3}{4}>{5}</a>"
                                                                                , lParte2_href
                                                                                , ( string.IsNullOrEmpty(lParte2_target)  ? "" : "  target='" + lParte2_target  + "'" )
                                                                                , ( string.IsNullOrEmpty(lParte2_class)   ? "" : "   class='" + lParte2_class   + "'" )
                                                                                , ( string.IsNullOrEmpty(lParte2_style)   ? "" : "   style='" + lParte2_style   + "'" )
                                                                                , ( string.IsNullOrEmpty(lParte2_onclick) ? "" : " onclick='" + lParte2_onclick + "'" )
                                                                                , lParte1));
                    }
                    else
                    {
                        lOutPut = lOutPut.Replace(lMatch.Value, string.Format("<span{0}{1}>{2}</span>"
                                                                                , lParte2_href
                                                                                , ( string.IsNullOrEmpty(lParte2_class) ? "" : " class='" + lParte2_class + "'" )
                                                                                , ( string.IsNullOrEmpty(lParte2_style) ? "" : " style='" + lParte2_style + "'" )
                                                                                , lParte1));
                    }
                }
            }

            // verifica se tem bold:

            lRegex = new Regex("\\*{2}(.*?)\\*{2}");

            lMatches = lRegex.Matches(lOutPut);

            foreach (Match lMatch in lMatches)
            {
                lOutPut = lOutPut.Replace(lMatch.Value
                                            , string.Format("<span style='font-weight:bold'>{0}</span>"
                                                            , lMatch.Value.Substring(2, lMatch.Value.Length - 4))
                                         );
            }

            // verifica se tem italico:

            lRegex = new Regex("\\*(.*?)\\*");

            lMatches = lRegex.Matches(lOutPut);

            foreach (Match lMatch in lMatches)
            {
                lOutPut = lOutPut.Replace(lMatch.Value
                                            , string.Format("<span style='font-style:italic'>{0}</span>"
                                                            , lMatch.Value.Substring(1, lMatch.Value.Length - 2))
                                         );
            }

            return lOutPut;
        }

        public abstract string Renderizar();

        #endregion

        public enum TipoWidget
        {
            Titulo, Imagem, Lista, Texto, Tabela, Repetidor, ListaDeDefinicao, Embed, Abas
        };
            
    }
}

