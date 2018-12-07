using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using Gradual.Site.Lib;
using Gradual.Site.Lib.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.Site.Lib.Dados;

namespace Gradual.Site.Lib.Widgets
{
    public class widRepetidor : WidgetBase
    {
        #region Propriedades

        public string TemplateDoItem { get; set; }

        [JsonIgnore]
        private int IdTipoConteudoDaLista { get; set; }

        [JsonIgnore]
        private string ListaRenderizada { get; set; }

        #endregion

        #region Métodos Private

        private string RenderizarElementoComTemplate(Newtonsoft.Json.Linq.JObject pObjetoInstanciado, string pTemplate)
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

            return lRetorno;
        }

        private string RenderizarLista()
        {
            StringBuilder lBuilder = new StringBuilder();
            
            // está configurado pra pegar conteúdo dinâmico
            IServicoPersistenciaSite lServico = Ativador.Get<IServicoPersistenciaSite>();

            BuscarItensDaListaRequest lRequest = new BuscarItensDaListaRequest();
            BuscarItensDaListaResponse lResponse;

            lRequest.IdDaLista = this.IdDaLista;

            lResponse = lServico.BuscarItensDaLista(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                this.IdTipoConteudoDaLista = lResponse.IdTipoConteudo;
                this.DescricaoDaLista = lResponse.DescricaoDaLista;

                //renderiza cada item conforme o template

                string lTemplate = this.TemplateDoItem;

                string lTemplateProcessado;

                Newtonsoft.Json.Linq.JObject lConteudoJsonInstanciado;

                foreach (ConteudoInfo lConteudo in lResponse.Itens)
                {
                    lConteudoJsonInstanciado = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(lConteudo.ConteudoJson);

                    lTemplateProcessado = RenderizarElementoComTemplate(lConteudoJsonInstanciado, lTemplate);

                    lBuilder.AppendFormat("\r\n{0}", lTemplateProcessado);
                }
            }
            else
            {
                //coloca alguma mensagem?
            }

            return lBuilder.ToString();
        }

        #endregion

        #region Métodos Públicos

        public override string Renderizar()
        {
            this.ListaRenderizada = RenderizarLista();  //tem que ser antes pra pegar o IdTipoConteudoDaLista
            
            //é melhor deixar os atributos class e style sempre visíveis mesmo sem nada dentro pro js não pegar como "undefined', e sim como vazio, pra não estragar a comparação de json que verifica se o objeto teve alterações

            string lRetorno;

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<div id='widRepetidor-{0}-{1}' class='{2}' style='{3}' data-IdDaLista='{4}' data-DescricaoDaLista='{5}' data-IdTipoConteudoDaLista='{6}' data-Ordenacao='{7}'><code class='TemplateDoItem'>{8}</code>\r\n{9}\r\n</div>"
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoClass
                                            , this.AtributoStyle
                                            , this.IdDaLista
                                            , this.DescricaoDaLista
                                            , this.IdTipoConteudoDaLista
                                            , this.Ordenacao
                                            , this.TemplateDoItem
                                            , this.ListaRenderizada
                                        );
            }
            else
            {
                lRetorno = string.Format("<div id='widRepetidor-{0}-{1}' class='{2}' style='{3}'>\r\n{4}\r\n</div>"
                                            , this.IdDaEstrutura
                                            , this.IdDoWidget
                                            , this.AtributoClass
                                            , this.AtributoStyle
                                            , this.ListaRenderizada
                                        );
            }

            return lRetorno;
        }

        #endregion
    }
}