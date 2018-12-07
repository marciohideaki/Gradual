using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Gradual.Site.DbLib;
using Gradual.OMS.Library.Servicos;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.DbLib.Dados;
using Newtonsoft.Json;

namespace Gradual.Site.DbLib.Widgets
{
    public class widLista : WidgetBase
    {
        #region Propriedades

        public string Texto { get; set; }

        public string TemplateDoItem { get; set; }

        public string Atributos { get; set; }

        public bool FlagListaEstatica { get; set; }

        [JsonIgnore]
        private int IdTipoConteudoDaLista { get; set; }

        [JsonIgnore]
        private string ListaRenderizada { get; set; }

        #endregion

        #region Métodos Private

        private string RenderizarElementoComTemplate(ConteudoInfo pConteudo, Newtonsoft.Json.Linq.JObject pObjetoInstanciado, string pTemplate)
        {
            string lRetorno = pTemplate;

            string lPropriedadeComMarcador, lValor;

            IEnumerable<Newtonsoft.Json.Linq.JProperty> lPropriedades =  pObjetoInstanciado.Properties();

            foreach (Newtonsoft.Json.Linq.JProperty lPropriedade in lPropriedades)
            {
                lPropriedadeComMarcador = "$[" + lPropriedade.Name.Replace("\"", "") + "]";

                if (lRetorno.Contains(lPropriedadeComMarcador))
                {
                    lValor = pObjetoInstanciado.Property(lPropriedade.Name).Value.ToString();

                    lValor = lValor.TrimStart('"').TrimEnd('"');

                    lValor = lValor.Replace("\\n", Environment.NewLine);

                    lRetorno = lRetorno.Replace(lPropriedadeComMarcador, lValor);
                }
            }

            if (lRetorno.Contains("$[ConteudoHTML]"))
            {
                lRetorno = lRetorno.Replace("$[ConteudoHTML]", pConteudo.ConteudoHtml.Replace("\"", "&quot;"));
            }

            return lRetorno;
        }
        
        private string RenderizarAtributos(ConteudoInfo pConteudo, Newtonsoft.Json.Linq.JObject pObjetoInstanciado, string pAtributos)
        {
            if (string.IsNullOrEmpty(pAtributos))
                return "";

            string lRetorno = pAtributos;

            string lPropriedadeComMarcador, lValor;

            IEnumerable<Newtonsoft.Json.Linq.JProperty> lPropriedades =  pObjetoInstanciado.Properties();

            foreach (Newtonsoft.Json.Linq.JProperty lPropriedade in lPropriedades)
            {
                lPropriedadeComMarcador = "$[" + lPropriedade.Name.Replace("\"", "") + "]";

                if (lRetorno.Contains(lPropriedadeComMarcador))
                {
                    lValor = pObjetoInstanciado.Property(lPropriedade.Name).Value.ToString();

                    lValor = lValor.TrimStart('"').TrimEnd('"');

                    lValor = lValor.Replace("\\n", Environment.NewLine);

                    lRetorno = lRetorno.Replace(lPropriedadeComMarcador, lValor);
                }
            }

            return lRetorno;
        }

        private string RenderizarLista()
        {
            StringBuilder lBuilder = new StringBuilder();

            if (this.IdDaLista > 0)
            {
                // está configurado pra pegar conteúdo dinâmico
                IServicoPersistenciaSite lServico = Ativador.Get<IServicoPersistenciaSite>();

                BuscarItensDaListaRequest lRequest = new BuscarItensDaListaRequest();
                BuscarItensDaListaResponse lResponse;

                lRequest.IdDaLista = this.IdDaLista;

                lResponse = lServico.BuscarItensDaLista(lRequest);

                Newtonsoft.Json.Linq.JObject lConteudoJsonInstanciado;

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    this.IdTipoConteudoDaLista = lResponse.IdTipoConteudo;
                    this.DescricaoDaLista = lResponse.DescricaoDaLista;

                    //renderiza cada item conforme o template

                    string lTemplate = this.TemplateDoItem;
                    string lAtributos = this.Atributos;

                    string lTemplateProcessado;
                    string lAtributosProcessados;

                    List<ConteudoInfo> lListaDeConteudo = (List<ConteudoInfo>)Extensions.ClonarTudo(lResponse.Itens);

                    List<Newtonsoft.Json.Linq.JObject> lListaConvertida = new List<Newtonsoft.Json.Linq.JObject>(); //= lResponse.Itens.ParaListaJson(this.Ordenacao);

                    Extensions.OrdenarEConverterParaListaJson(ref lListaDeConteudo, ref lListaConvertida, this.Ordenacao);

                    foreach (ConteudoInfo lConteudo in lListaDeConteudo)
                    {
                        lConteudoJsonInstanciado = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(lConteudo.ConteudoJson);

                        lTemplateProcessado = RenderizarElementoComTemplate(lConteudo, lConteudoJsonInstanciado, lTemplate);

                        lAtributosProcessados = RenderizarAtributos(lConteudo, lConteudoJsonInstanciado, lAtributos);

                        if (this.RenderizarHabilitandoCMS)
                        {
                            lBuilder.AppendFormat("\r\n<li {2}><code class='dataTexto'>{0}</code>{1}</li>", lTemplateProcessado, ProcessarTextoParaHTML(lTemplateProcessado), lAtributosProcessados);
                        }
                        else
                        {
                            lBuilder.AppendFormat("\r\n<li {1}>{0}</li>", ProcessarTextoParaHTML(lTemplateProcessado), lAtributosProcessados);
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
                string[] lLinhas = this.Texto.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string lLinha in lLinhas)
                {
                    if (this.RenderizarHabilitandoCMS)
                    {
                        lBuilder.AppendFormat("\r\n<li><code class='dataTexto'>{0}</code>{1}</li>", lLinha, ProcessarTextoParaHTML(lLinha));
                    }
                    else
                    {
                        lBuilder.AppendFormat("\r\n<li>{0}</li>", ProcessarTextoParaHTML(lLinha));
                    }
                }
            }

            return lBuilder.ToString();
        }

        #endregion

        #region Métodos Públicos

        public override string Renderizar(byte pRenderizandoNaAba)
        {
            OnMensagemDeWidget("widLista[{0}].Renderizar({1})", this.IdDoWidget, this.IdDaEstrutura);

            string lRetorno;

            this.ListaRenderizada = RenderizarLista();  //tem que ser antes pra pegar o IdTipoConteudoDaLista
            
            string lPrefixo = (pRenderizandoNaAba == 0) ? "" : "_";

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<ul id='{11}widLista-{0}-{1}' class='{2}' style='{3}' data-IdDaLista='{4}' data-DescricaoDaLista='{5}' data-IdTipoConteudoDaLista='{7}' data-Ordenacao='{8}'><code class='dataTemplateDoItem'>{6}</code><code class='dataAtributosDoItem'>{10}</code>\r\n{9}\r\n</ul>"
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
                                          , this.Atributos
                                          , lPrefixo
                                        );
            }
            else
            {
                lRetorno = string.Format("<ul id='{5}widLista-{0}-{1}' class='{2}' style='{3}'>\r\n{4}\r\n</ul>"
                                          , this.IdDaEstrutura
                                          , this.IdDoWidget
                                          , this.AtributoClass
                                          , this.AtributoStyle
                                          , this.ListaRenderizada
                                          , lPrefixo
                                        );
            }

            OnMensagemDeWidget("widLista[{0}] Renderizado com [{1}] caracteres.", this.IdDoWidget, lRetorno.Length);

            return lRetorno;
        }

        #endregion
    }
}