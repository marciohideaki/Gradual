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
    public class widAbas : WidgetBase
    {
        #region Propriedades

        public List<widAbaItem> ListaDeAbas { get; set; }

        [JsonIgnore]
        private string AbasRenderizadas { get; set; }

        [JsonIgnore]
        private string ConteudosRenderizados { get; set; }

        #endregion

        #region Constructor

        public widAbas()
        {
            this.ListaDeAbas = new List<widAbaItem>();
        }

        #endregion

        #region Métodos Private

        private string RenderizarAbas()
        {
            StringBuilder lBuilder = new StringBuilder();

            string lClasse = "class='ativo'";

            foreach (widAbaItem lItem in this.ListaDeAbas)
            {
                lBuilder.AppendFormat("<li data-IdConteudo='{0}' data-URL='{1}' data-TipoLink='{2}' {3}> <a id='Aba-{5}' href='#'>{4}</a> </li>"
                                        , lItem.IdConteudo
                                        , lItem.URL
                                        , lItem.TipoLink
                                        , lClasse
                                        , lItem.Titulo
                                        , lItem.Titulo.ToStringSemAcentos().Replace(" ", "")
                                     );

                lClasse = "";
            }

            return lBuilder.ToString();
        }
        
        private string RenderizarConteudos(byte pRenderizandoNaAba)
        {
            StringBuilder lBuilder = new StringBuilder();

            Gradual.Site.DbLib.Persistencias.ServicoPersistenciaSite lServico = new Persistencias.ServicoPersistenciaSite();

            BuscarHtmlDaPaginaRequest lRequestPaginaConteudo;
            BuscarHtmlDaPaginaResponse lResponsePaginaConteudo;

            //IServicoPersistenciaSite 

            string lStyle = "";

            string lPaginaConteudo;

            string lCache = "(db)";

            foreach (widAbaItem lItem in this.ListaDeAbas)
            {
                if (lItem.TipoLink == "Embutida" && (pRenderizandoNaAba < 6))   //limite teórico de 5 abas dentro de abas Abinception
                {
                    OnMensagemDeWidget("widAbas -> Renderizando widAbaItem[{0}] para [{1}]...", lItem.IdConteudo, this.IdDaEstrutura);

                    lRequestPaginaConteudo = new BuscarHtmlDaPaginaRequest();

                    lRequestPaginaConteudo.IdDaPagina = Convert.ToInt32(lItem.IdConteudo);

                    lRequestPaginaConteudo.RenderizandoNaAba = Convert.ToByte(pRenderizandoNaAba + 1);

                    lRequestPaginaConteudo.ExisteClienteLogado = this.ExisteUsuarioLogado;

                    lRequestPaginaConteudo.HostERaiz = this.HostERaiz;

                    lRequestPaginaConteudo.Versao = this.VersaoDaEstrutura;

                    lResponsePaginaConteudo = lServico.BuscarHtmlPagina(lRequestPaginaConteudo);

                    if (lResponsePaginaConteudo.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                    {
                        lPaginaConteudo = lResponsePaginaConteudo.HTML;

                        if (lResponsePaginaConteudo.UtilizadoCache)
                        {
                            lCache = "(cc)";
                        }
                    }
                    else
                    {
                        lPaginaConteudo = string.Format("Erro em widAbas > RenderizarConteudos(): [{0}]", lResponsePaginaConteudo.DescricaoResposta);
                    }

                    if (string.IsNullOrEmpty(lPaginaConteudo))
                    {
                        lPaginaConteudo = string.Format("&nbsp;\r\n\r\n<!-- Conteúdo vazio página [{0}] estrutura [{1}] -->\r\n\r\n", lItem.IdConteudo, this.IdDaEstrutura);
                    }

                    lBuilder.AppendFormat("<div data-IdConteudo='{0}' {2}> {1} </div>", lItem.IdConteudo, lPaginaConteudo, lStyle);

                    lStyle = " style='display:none' ";

                    OnMensagemDeWidget("widAbas -> Renderizado (widAbaItem[{0}], [{1}]) {2}", lItem.IdConteudo, this.IdDaEstrutura, lCache);
                }
            }


            return lBuilder.ToString();
        }

        #endregion

        #region Métodos Públicos

        public override string Renderizar(byte pRenderizandoNaAba)
        {
            OnMensagemDeWidget("widAbas[{0}].Renderizar({1})", this.IdDoWidget, this.IdDaEstrutura);

            string lRetorno;

            this.AbasRenderizadas = RenderizarAbas();

            this.ConteudosRenderizados = RenderizarConteudos(pRenderizandoNaAba);

            string lPrefixo = (pRenderizandoNaAba == 0) ? "" : "_";

            string lBotaoVisitarPagina = "<button class='btnVisitarPaginaConteudo' onclick='return btnWidAbas_VisitarPaginaConteudo_Click(this)' title='Visitar página da aba selecionada'></button>";

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<div id='{8}widAbas-{0}-{1}' data-ListaDeAbas='{6}'><ul class='{2}' style='{3}'>\r\n{4}\r\n</ul>\r\n{7}\r\n{5}</div>"
                                          , this.IdDaEstrutura
                                          , this.IdDoWidget
                                          , this.AtributoClass
                                          , this.AtributoStyle
                                          , this.AbasRenderizadas
                                          , this.ConteudosRenderizados
                                          , JsonConvert.SerializeObject(this.ListaDeAbas)
                                          , lBotaoVisitarPagina
                                          , lPrefixo
                                        );
            }
            else
            {
                lRetorno = string.Format("<div id='{6}widAbas-{0}-{1}'><ul class='{2}' style='{3}'>\r\n{4}\r\n</ul>{5}</div>"
                                          , this.IdDaEstrutura
                                          , this.IdDoWidget
                                          , this.AtributoClass
                                          , this.AtributoStyle
                                          , this.AbasRenderizadas
                                          , this.ConteudosRenderizados
                                          , lPrefixo
                                        );
            }

            OnMensagemDeWidget("widAbas[{0}] Renderizado com [{1}] caracteres.", this.IdDoWidget, lRetorno.Length);

            return lRetorno;
        }

        #endregion
    }

    public class widAbaItem
    {
        #region Propriedades

        public string URL { get; set; }

        public string IdConteudo { get; set; }

        public string TipoLink { get; set; }
        
        public string Titulo { get; set; }

        #endregion
    }
}