using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using Gradual.Site.DbLib;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.DbLib.Dados;
using Gradual.OMS.Library.Servicos;
using System.Text.RegularExpressions;

namespace Gradual.Site.DbLib.Widgets
{
    public class widTabela : WidgetBase
    {
        #region Propriedades

        public string Texto { get; set; }

        public string Cabecalho { get; set; }

        public bool FlagTabelaEstatica { get; set; }

        private int QuantidadeDeColunas { get; set; }

        [JsonIgnore]
        private int IdTipoConteudoDaLista { get; set; }
        
        [JsonIgnore]
        private string CabecalhoRenderizado { get; set; }

        [JsonIgnore]
        private string LinhasRenderizadas { get; set; }

        #endregion

        #region Métodos Private

        private string RenderizarCabecalho()
        {
            StringBuilder lBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(this.Cabecalho))
            {
                string[] lColunas = this.Cabecalho.Split("|".ToCharArray());        //tem que contar os itens vazios

                string lValorCelula, lAtributoClass, lAtributoStyle;

                int lContador, lQuantidadeDeColunas;

                bool lPreencherHeader = true;

                lQuantidadeDeColunas = 0;

                if (string.IsNullOrEmpty(this.Cabecalho.Replace(" ", "").Replace("|", "")))
                {
                    lPreencherHeader = false;
                }

                if (lPreencherHeader)
                {
                    lBuilder.AppendFormat("<thead data-Cabecalho='{0}'>\r\n", this.Cabecalho);

                    lBuilder.Append("\t</tr>\r\n");
                }
                else
                {
                    lBuilder.AppendFormat("<thead data-Cabecalho='{0}' style='display:none'>\r\n</thead>", this.Cabecalho);
                }

                foreach (string lColuna in lColunas)
                {
                    lValorCelula = lColuna.Trim();

                    lQuantidadeDeColunas++;

                    lContador = 1; //começa no 1 porque colspan="1" não adianta, se tiver > tem que colocar colspan="2" e assim por diante
                    lAtributoStyle = "";
                    lAtributoClass = "";

                    if (!string.IsNullOrEmpty(lValorCelula))
                    {
                        //se começar com > então é colspan, conta quantas colunas tem conforme a qtd do colspan
                        while (lValorCelula[0] == '>')
                        {
                            lContador++;
                            lQuantidadeDeColunas++;

                            lValorCelula = lValorCelula.Substring(1);
                        }
                    }

                    if(lValorCelula.IndexOf("style:") != -1)
                    {
                        //tem que ser (style:___valores____) obrigatoriamente no final da string
                        lAtributoStyle = lValorCelula.Substring(lValorCelula.IndexOf("style:") - 1);

                        lValorCelula = lValorCelula.Replace(lAtributoStyle, "");

                        lAtributoStyle = lAtributoStyle.Substring(lAtributoStyle.IndexOf("style:") + 6);

                        lAtributoStyle = lAtributoStyle.TrimEnd(") ".ToCharArray());
                    }
                    else if(lValorCelula.IndexOf("class:") != -1)
                    {
                        //ou style ou class, não ambos...
                        lAtributoClass = lValorCelula.Substring(lValorCelula.IndexOf("class:") - 1, lValorCelula.Length);

                        lValorCelula = lValorCelula.Replace(lAtributoClass, "");

                        lAtributoClass = lAtributoClass.Substring(lAtributoClass.IndexOf("class:") + 6, lAtributoClass.Length - 8);
                    }

                    if(lValorCelula == "__") lValorCelula = "&nbsp;";       //dois underscores é uma célula em branco

                    if(lPreencherHeader)
                    {
                        lBuilder.AppendFormat("\t\t<td data-Texto='{0}'{1}{2}{3}>{4}</td>\r\n"
                                                    , (this.RenderizarHabilitandoCMS) ? lColuna.Trim() : ""
                                                    , (lContador > 1) ? string.Format(" colspan='{0}'", lContador) : ""
                                                    , string.IsNullOrEmpty(lAtributoStyle) ? "" : string.Format(" style='{0}'", lAtributoStyle)
                                                    , string.IsNullOrEmpty(lAtributoClass) ? "" : string.Format(" class='{0}'", lAtributoClass)
                                                    , ProcessarTextoParaHTML(lValorCelula));
                    }
                }

                if (lPreencherHeader)
                {
                    lBuilder.Append("\t</tr>\r\n");

                    lBuilder.Append("</thead>\r\n");
                }

                this.QuantidadeDeColunas = lQuantidadeDeColunas;
            }

            return lBuilder.ToString();
        }

        private string RenderizarLinhaDeConteudoDinamico(Newtonsoft.Json.Linq.JObject pObjeto)
        {
            StringBuilder lBuilder = new StringBuilder();

            string[] lColunas;

            string lPropriedade, lValor, lTextoDaCelula;

            Regex lRegexPropriedades = new Regex(@"\$\[.*?\]");

            MatchCollection lMatches;

            lColunas = this.Texto.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            lBuilder.AppendFormat("\r\n\t<tr>");

            if (this.QuantidadeDeColunas == 0)
            {
                //o cabecalho estava vazio e não preencheu a quantidade de colunas, então vamos assumir a da primeira linha:
                this.QuantidadeDeColunas = lColunas.Length;
            }

            for (int a = 0; a < this.QuantidadeDeColunas; a++)
            {
                if (a < lColunas.Length)
                {
                    lTextoDaCelula = string.Format(" {0}", lColunas[a]).Trim();

                    if (!string.IsNullOrEmpty(lTextoDaCelula))
                    {
                        lMatches = lRegexPropriedades.Matches(lTextoDaCelula);

                        foreach (Match lMatch in lMatches)
                        {
                            lPropriedade = lMatch.Value.Substring(2, lMatch.Value.Length - 3);

                            lValor = pObjeto.Property(lPropriedade).Value.ToString();

                            lValor = lValor.Trim("\"".ToCharArray());

                            lValor = lValor.Replace("\\n", Environment.NewLine);

                            lTextoDaCelula = lTextoDaCelula.Replace("$[" + lPropriedade + "]", lValor);
                        }

                        lBuilder.AppendFormat("\r\n\t\t<td>{0}</td>", ProcessarTextoParaHTML(lTextoDaCelula));
                    }
                    else
                    {
                        lBuilder.Append("\r\n\t\t<td>&nbsp;</td>");
                    }
                }
                else
                {
                    lBuilder.Append("\r\n\t\t<td>&nbsp;</td>");
                }
            }

            lBuilder.Append("\r\n\t</tr>");

            return lBuilder.ToString();
        }

        private string RenderizarLinhas()
        {
            StringBuilder lBuilder = new StringBuilder();

            if (this.IdDaLista > 0)
            {
                IServicoPersistenciaSite lServico = Ativador.Get<IServicoPersistenciaSite>();

                BuscarItensDaListaRequest lRequest = new BuscarItensDaListaRequest();
                BuscarItensDaListaResponse lResponse;

                lRequest.IdDaLista = this.IdDaLista;

                lResponse = lServico.BuscarItensDaLista(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    this.IdTipoConteudoDaLista = lResponse.IdTipoConteudo;
                    this.DescricaoDaLista = lResponse.DescricaoDaLista;

                    //tabela dinamica

                    //string lTemplate = this.TemplateDoItem;

                    string lLinhaProcessada;

                    List<ConteudoInfo> lListaDeConteudo = (List<ConteudoInfo>)Extensions.ClonarTudo(lResponse.Itens);

                    List<Newtonsoft.Json.Linq.JObject> lListaConvertida = new List<Newtonsoft.Json.Linq.JObject>(); //= lResponse.Itens.ParaListaJson(this.Ordenacao);

                    Extensions.OrdenarEConverterParaListaJson(ref lListaDeConteudo, ref lListaConvertida, this.Ordenacao);

                    foreach (Newtonsoft.Json.Linq.JObject lConteudo in lListaConvertida)
                    {

                        lLinhaProcessada = RenderizarLinhaDeConteudoDinamico(lConteudo);

                        lBuilder.AppendFormat("\r\n{0}", lLinhaProcessada);
                        /*
                        lTemplateProcessado = RenderizarElementoComTemplate(lConteudo, lTemplate);

                        lBuilder.AppendFormat("\r\n<li data-Texto='{0}'>{1}</li>", lTemplateProcessado, ProcessarTextoParaHTML(lTemplateProcessado));
                            * */
                    }

                    /*
                    string lLinhaProcessada;

                    Newtonsoft.Json.Linq.JObject lConteudoJsonInstanciado;

                    foreach (ConteudoInfo lConteudo in lResponse.Itens)
                    {
                        lConteudoJsonInstanciado = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(lConteudo.ConteudoJsonComPropriedadesExtras);

                        lLinhaProcessada = RenderizarLinhaDeConteudoDinamico(lConteudoJsonInstanciado);

                        lBuilder.AppendFormat("\r\n{0}", lLinhaProcessada);
                    }*/
                }
            }
            else
            {
                //tabela estática
                string[] lLinhas = this.Texto.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] lColunas;

                string lValorCelula, lAtributoClass, lAtributoStyle, lValorOriginal;

                int lContador;

                foreach (string lLinha in lLinhas)
                {
                    lColunas = lLinha.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    lBuilder.AppendFormat("\r\n\t<tr data-Texto='{0}'>", (this.RenderizarHabilitandoCMS ? lLinha : ""));

                    for (int a = 0; a < this.QuantidadeDeColunas; a++)
                    {
                        if (a < lColunas.Length)
                        {
                            lContador = 1;

                            lAtributoStyle = lAtributoClass = "";

                            lValorCelula = lColunas[a].Trim();

                            lValorOriginal = lValorCelula;

                            if (!string.IsNullOrEmpty(lValorCelula))
                            {
                                //se começar com > então é colspan, conta quantas colunas tem conforme a qtd do colspan
                                while (lValorCelula[0] == '>')
                                {
                                    lContador++;
                                    a++;
                                    lValorCelula = lValorCelula.Substring(1);
                                }
                            }

                            if(lValorCelula.IndexOf("style:") != -1)
                            {
                                //tem que ser (style:___valores____) obrigatoriamente no final da string
                                lAtributoStyle = lValorCelula.Substring(lValorCelula.IndexOf("style:") - 1);

                                lValorCelula = lValorCelula.Replace(lAtributoStyle, "");

                                lAtributoStyle = lAtributoStyle.Substring(lAtributoStyle.IndexOf("style:") + 6, lAtributoStyle.Length - 8);
                            }
                            else if(lValorCelula.IndexOf("class:") != -1)
                            {
                                //ou style ou class, não ambos...
                                lAtributoClass = lValorCelula.Substring(lValorCelula.IndexOf("class:") - 1);

                                lValorCelula = lValorCelula.Replace(lAtributoClass, "");

                                lAtributoClass = lAtributoClass.Substring(lAtributoClass.IndexOf("class:") + 6, lAtributoClass.Length - 8);
                            }

                            if(lValorCelula == "__") lValorCelula = "&nbsp;";       //dois underscores é uma célula em branco

                            lBuilder.AppendFormat("\t\t<td data-Texto='{0}'{1}{2}{3}>{4}</td>\r\n"
                                                        , (this.RenderizarHabilitandoCMS ? lValorOriginal : "")
                                                        , (lContador > 1) ? string.Format(" colspan='{0}'", lContador) : ""
                                                        , string.IsNullOrEmpty(lAtributoStyle) ? "" : string.Format(" style='{0}'", lAtributoStyle)
                                                        , string.IsNullOrEmpty(lAtributoClass) ? "" : string.Format(" class='{0}'", lAtributoClass)
                                                        , ProcessarTextoParaHTML(lValorCelula));
                        }
                        else
                        {
                            lBuilder.Append("\r\n\t\t<td>&nbsp;</td>");
                        }
                    }

                    lBuilder.Append("\r\n\t</tr>");
                }
            }

            return lBuilder.ToString();
        }

        #endregion

        #region Métodos Públicos

        public override string Renderizar(byte pRenderizandoNaAba)
        {
            OnMensagemDeWidget("widTabela[{0}].Renderizar({1})", this.IdDoWidget, this.IdDaEstrutura);

            this.CabecalhoRenderizado = RenderizarCabecalho();

            this.LinhasRenderizadas = RenderizarLinhas();

            //é melhor deixar os atributos class e style sempre visíveis mesmo sem nada dentro pro js não pegar como "undefined', e sim como vazio, pra não estragar a comparação de json que verifica se o objeto teve alterações

            string lRetorno;

            string lPrefixo = (pRenderizandoNaAba == 0) ? "" : "_";

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<table id='{11}widTabela-{0}-{1}' cellspacing='0' class='{2}' style='{3}' data-IdDaLista='{4}' data-DescricaoDaLista='{5}' data-TemplateDaLinha='{8}' data-IdTipoConteudoDaLista='{9}' data-Ordenacao='{10}'>\r\n{6}{7}\r\n</table>"
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoClass
                                            , this.AtributoStyle
                                            , this.IdDaLista
                                            , this.DescricaoDaLista
                                            , this.CabecalhoRenderizado
                                            , this.LinhasRenderizadas
                                            , (this.FlagTabelaEstatica ? "" : this.Texto)
                                            , this.IdTipoConteudoDaLista
                                            , this.Ordenacao
                                            , lPrefixo
                                        );
            }
            else
            {
                lRetorno = string.Format("<table id='{6}widTabela-{0}-{1}' cellspacing='0' class='{2}' style='{3}'>\r\n{4}{5}\r\n</table>"
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoClass
                                            , this.AtributoStyle
                                            , this.CabecalhoRenderizado
                                            , this.LinhasRenderizadas
                                            , lPrefixo
                                        );
            }

            OnMensagemDeWidget("widTabela[{0}] Renderizado com [{1}] caracteres.", this.IdDoWidget, lRetorno.Length);

            return lRetorno;
        }

        #endregion
    }
}