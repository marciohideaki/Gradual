using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.DbLib.Widgets
{
    public class widAcordeon : WidgetBase
    {
        #region Propriedades

        public List<widAbaItem> ListaDeAbas { get; set; }

        [JsonIgnore]
        private string AbasRenderizadas { get; set; }

        #endregion

        #region Constructor

        public widAcordeon()
        {
            this.ListaDeAbas = new List<widAbaItem>();
        }

        #endregion

        #region Métodos Private

        private string RenderizarAbas(byte pRenderizandoNaAba)
        {
            StringBuilder lBuilder = new StringBuilder();

            Gradual.Site.DbLib.Persistencias.ServicoPersistenciaSite lServico = new Persistencias.ServicoPersistenciaSite();

            BuscarHtmlDaPaginaRequest lRequestPaginaConteudo;
            BuscarHtmlDaPaginaResponse lResponsePaginaConteudo;

            string lPaginaConteudo, lBotao;

            string lCache = "(db)";

            lBotao = this.RenderizarHabilitandoCMS ? "<button title='Visitar página' onclick='return btnWidAcordeon_VisitarPaginaConteudo_Click(this)' class='btnVisitarPaginaConteudo'></button>" : "";

            foreach (widAbaItem lItem in this.ListaDeAbas)
            {
                OnMensagemDeWidget("widAcordeon -> Renderizando widAbaItem[{0}] para [{1}]...", lItem.IdConteudo, this.IdDaEstrutura);

                if (lItem.TipoLink == "Embutida" && (pRenderizandoNaAba < 6))   //limite teórico de 5 abas dentro de abas Abinception
                {
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
                        lPaginaConteudo = string.Format("Erro em widAcordeon > RenderizarConteudos(): [{0}]");
                    }

                    lBuilder.AppendFormat("<li onclick='return lnkAcordeon_Conteudo_Click(this, event)' data-IdConteudo='{0}' data-URL='{1}' data-TipoLink='{2}'> {5} <div class='acordeon-opcao'>{3}</div> <div class='acordeon-conteudo'>{4}</div> </li>"
                                            , lItem.IdConteudo
                                            , lItem.URL
                                            , lItem.TipoLink
                                            , lItem.Titulo
                                            , lPaginaConteudo
                                            , lBotao
                                         );
                }

                OnMensagemDeWidget("widAcordeon -> Renderizado (widAbaItem[{0}], [{1}]) {2}", lItem.IdConteudo, this.IdDaEstrutura, lCache);
            }

            return lBuilder.ToString();
        }


        #endregion

        #region Métodos Públicos

        public override string Renderizar(byte pRenderizandoNaAba)
        {
            OnMensagemDeWidget("widAcordeon[{0}].Renderizar({1})", this.IdDoWidget, this.IdDaEstrutura);

            string lRetorno;

            this.AbasRenderizadas = RenderizarAbas(pRenderizandoNaAba);

            string lPrefixo = (pRenderizandoNaAba == 0) ? "" : "_";

            //string lBotaoVisitarPagina = "<button class='btnVisitarPaginaConteudo' onclick='return btnWidAbas_VisitarPaginaConteudo_Click(this)' title='Visitar página da aba selecionada'></button>";

            if (this.RenderizarHabilitandoCMS)
            {
                lRetorno = string.Format("<div id='{6}widAcordeon-{0}-{1}' data-ListaDeAbas='{5}'><ul class='{2}' style='{3}'>\r\n{4}\r\n</ul></div>"
                                          , this.IdDaEstrutura
                                          , this.IdDoWidget
                                          , this.AtributoClass
                                          , this.AtributoStyle
                                          , this.AbasRenderizadas
                                          , JsonConvert.SerializeObject(this.ListaDeAbas)
                                          , lPrefixo
                                        );
            }
            else
            {
                lRetorno = string.Format("<div id='{5}widAcordeon-{0}-{1}'> <ul class='{2}' style='{3}'>\r\n{4}\r\n</ul> </div>"
                                          , this.IdDaEstrutura
                                          , this.IdDoWidget
                                          , this.AtributoClass
                                          , this.AtributoStyle
                                          , this.AbasRenderizadas
                                          , lPrefixo
                                        );
            }

            OnMensagemDeWidget("widAcordeon[{0}] Renderizado com [{1}] caracteres.", this.IdDoWidget, lRetorno.Length);

            return lRetorno;
        }

        #endregion
    }

}
