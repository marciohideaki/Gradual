using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Gradual.Site.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Site.Lib.Mensagens;
using Gradual.Site.Lib.Dados;
using Newtonsoft.Json;

namespace Gradual.Site.Lib.Widgets
{
    public class widLista : WidgetBase
    {
        #region Propriedades

        public string Texto { get; set; }

        public string TemplateDoItem { get; set; }

        public bool FlagListaEstatica { get; set; }
        
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

            if (this.IdDaLista > 0)
            {
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

                    List<ConteudoInfo> lListaDeConteudo = (List<ConteudoInfo>)Extensions.ClonarTudo(lResponse.Itens);

                    List<Newtonsoft.Json.Linq.JObject> lListaConvertida = new List<Newtonsoft.Json.Linq.JObject>(); //= lResponse.Itens.ParaListaJson(this.Ordenacao);

                    Extensions.OrdenarEConverterParaListaJson(ref lListaDeConteudo, ref lListaConvertida, this.Ordenacao);

                    foreach (Newtonsoft.Json.Linq.JObject lConteudo in lListaConvertida)
                    {
                        lTemplateProcessado = RenderizarElementoComTemplate(lConteudo, lTemplate);

                        lBuilder.AppendFormat("\r\n<li data-Texto='{0}'>{1}</li>", lTemplateProcessado, ProcessarTextoParaHTML(lTemplateProcessado));
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
                        lBuilder.AppendFormat("\r\n<li data-Texto='{0}'>{1}</li>", lLinha, ProcessarTextoParaHTML(lLinha));
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

        public override string Renderizar()
        {
            string lRetorno;

            this.ListaRenderizada = RenderizarLista();  //tem que ser antes pra pegar o IdTipoConteudoDaLista

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<ul id='widLista-{0}-{1}' class='{2}' style='{3}' data-IdDaLista='{4}' data-DescricaoDaLista='{5}' data-TemplateDoItem='{6}' data-IdTipoConteudoDaLista='{7}' data-Ordenacao='{8}'>\r\n{9}\r\n</ul>"
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
                                        );
            }
            else
            {
                lRetorno = string.Format("<ul id='widLista-{0}-{1}' class='{2}' style='{3}'>\r\n{4}\r\n</ul>"
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