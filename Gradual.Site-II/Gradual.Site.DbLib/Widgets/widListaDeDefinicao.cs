using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using Gradual.Site.DbLib;
using Gradual.Site.DbLib.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.DbLib.Widgets
{
    public class widListaDeDefinicao : WidgetBase
    {
        #region Propriedades

        public string TemplateDoItem { get; set; }

        [JsonIgnore]
        private int IdTipoConteudoDaLista { get; set; }

        [JsonIgnore]
        private string ListaRenderizada { get; set; }

        #endregion

        #region Métodos Private

        private string[] RenderizarElementoComTemplate(ConteudoInfo pConteudo, Newtonsoft.Json.Linq.JObject pObjetoInstanciado, string pTemplate)
        {
            string[] lRetorno = pTemplate.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            string lPropriedadeComMarcador, lValor;

            IEnumerable<Newtonsoft.Json.Linq.JProperty> lPropriedades =  pObjetoInstanciado.Properties();

            for (int a = 0; a < lRetorno.Length; a++)
            {
                foreach (Newtonsoft.Json.Linq.JProperty lPropriedade in lPropriedades)
                {
                    lPropriedadeComMarcador = "$[" + lPropriedade.Name.Replace("\"", "") + "]";

                    if (lRetorno[a].Contains(lPropriedadeComMarcador))
                    {
                        lValor = pObjetoInstanciado.Property(lPropriedade.Name).Value.ToString();

                        lValor = lValor.TrimStart('"').TrimEnd('"');

                        lValor = lValor.Replace("\\n", Environment.NewLine);

                        lRetorno[a] = lRetorno[a].Replace(lPropriedadeComMarcador, lValor);
                    }
                }

                if (lRetorno[a].Contains("$[ConteudoHTML]"))
                {
                    lRetorno[a] = lRetorno[a].Replace("$[ConteudoHTML]", pConteudo.ConteudoHtml.Replace("\"", "&quot;"));
                }
            }

            return lRetorno;
        }

        private string RenderizarLista()
        {
            StringBuilder lBuilder = new StringBuilder();

            int lCounter = 0;

            try
            {
                if (this.IdDaLista > 0)
                {
                    // está configurado pra pegar conteúdo dinâmico
                    IServicoPersistenciaSite lServico = Ativador.Get<IServicoPersistenciaSite>();

                    BuscarItensDaListaRequest lRequest = new BuscarItensDaListaRequest();
                    BuscarItensDaListaResponse lResponse;

                    lRequest.IdDaLista = this.IdDaLista;

                    lResponse = lServico.BuscarItensDaLista(lRequest);

                    lCounter = 1;

                    if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                    {
                        this.IdTipoConteudoDaLista = lResponse.IdTipoConteudo;
                        this.DescricaoDaLista = lResponse.DescricaoDaLista;

                        //renderiza cada item conforme o template

                        string lTemplate = this.TemplateDoItem;

                        string[] lTemplateProcessado = null;

                        string lEstiloVisivel = (this.AtributoClass == "ItensExpansiveis" || this.AtributoClass == "Acordeao") ? " style='display:none'" : "";

                        Newtonsoft.Json.Linq.JObject lConteudoJsonInstanciado;
                    
                        List<ConteudoInfo> lListaDeConteudo = (List<ConteudoInfo>)Extensions.ClonarTudo(lResponse.Itens);

                        List<Newtonsoft.Json.Linq.JObject> lListaConvertida = new List<Newtonsoft.Json.Linq.JObject>(); //= lResponse.Itens.ParaListaJson(this.Ordenacao);
                        
                        lCounter = 2;

                        Extensions.OrdenarEConverterParaListaJson(ref lListaDeConteudo, ref lListaConvertida, this.Ordenacao);
                        
                        lCounter = 9;

                        foreach (ConteudoInfo lConteudo in lListaDeConteudo)
                        {
                            lCounter++;

                            lConteudoJsonInstanciado = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(lConteudo.ConteudoJson);

                            try
                            {
                                lTemplateProcessado = RenderizarElementoComTemplate(lConteudo, lConteudoJsonInstanciado, lTemplate);

                                if (lTemplateProcessado.Length > 0)
                                {
                                    if (this.RenderizarHabilitandoCMS)
                                    {
                                        lBuilder.AppendFormat("\r\n<dt data-Texto='{0}'>{1}</dt>", lTemplateProcessado[0], ProcessarTextoParaHTML(lTemplateProcessado[0]));
                                    }
                                    else
                                    {
                                        lBuilder.AppendFormat("\r\n<dt>{0}</dt>", ProcessarTextoParaHTML(lTemplateProcessado[0]));
                                    }
                                }

                                if (lTemplateProcessado.Length > 1)
                                {
                                    if (this.RenderizarHabilitandoCMS)
                                    {
                                        lBuilder.AppendFormat("\r\n<dd data-Texto='{0}'{2}>{1}</dd>", lTemplateProcessado[1], ProcessarTextoParaHTML(lTemplateProcessado[1]), lEstiloVisivel);
                                    }
                                    else
                                    {
                                        lBuilder.AppendFormat("\r\n<dd {1}>{0}</dd>", ProcessarTextoParaHTML(lTemplateProcessado[1]), lEstiloVisivel);
                                    }
                                }

                                lTemplateProcessado = null;
                            }
                            catch (Exception erroRend)
                            {
                                lBuilder.AppendFormat("<!-- \r\n erro ao renderizar elemento: Nível [{0}]\r\n\r\n{1}\r\n{2}\r\n\r\n [{3}]\r\n {4} \r\n -->"
                                                        , lCounter
                                                        , lConteudo.ConteudoJson
                                                        , JsonConvert.SerializeObject(lTemplateProcessado)
                                                        , erroRend.Message
                                                        , erroRend.StackTrace
                                                      );
                            }
                        }
                    }
                    else
                    {
                        //coloca alguma mensagem?
                    }
                }
                else
                {
                    //coloca alguma mensagem?
                }
            }
            catch (Exception ex)
            {
                lBuilder.AppendFormat("<!--\r\n Erro durante a renderização da lista: Nível [{0}] \r\n{1}\r\n\r\n{2}\r\n\r\n-->", lCounter, ex.Message, ex.StackTrace);
            }

            return lBuilder.ToString();
        }

        #endregion

        #region Métodos Públicos

        public override string Renderizar(byte pRenderizandoNaAba)
        {
            OnMensagemDeWidget("widListaDeDefinicao[{0}].Renderizar({1})", this.IdDoWidget, this.IdDaEstrutura);

            this.ListaRenderizada = RenderizarLista();  //tem que ser antes pra pegar o IdTipoConteudoDaLista

            string lRetorno;

            string lPrefixo = (pRenderizandoNaAba == 0) ? "" : "_";

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<dl id='{10}widListaDeDefinicao-{0}-{1}' class='{2}' style='{3}' data-IdDaLista='{4}' data-DescricaoDaLista='{5}' data-TemplateDoItem='{6}' data-IdTipoConteudoDaLista='{7}' data-Ordenacao='{8}'>\r\n{9}\r\n</dl>"
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoClass
                                            , this.AtributoStyle
                                            , this.IdDaLista
                                            , this.DescricaoDaLista
                                            , this.TemplateDoItem
                                            , this.IdTipoConteudoDaLista
                                            , this.Ordenacao
                                            , this.ListaRenderizada
                                            , lPrefixo
                                        );
            }
            else
            {
                lRetorno = string.Format("<dl id='{5}widListaDeDefinicao-{0}-{1}' class='{2}' style='{3}'>\r\n{4}\r\n</dl>"
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoClass
                                            , this.AtributoStyle
                                            , this.ListaRenderizada
                                            , lPrefixo
                                        );
            }

            OnMensagemDeWidget("widListaDeDefinicao[{0}] Renderizado com [{1}] caracteres.", this.IdDoWidget, lRetorno.Length);

            return lRetorno;
        }

        #endregion
    }
}