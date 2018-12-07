using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Gradual.Site.DbLib.Widgets;
using Gradual.Site.DbLib.Dados;
using Gradual.Site.DbLib.Mensagens;
using Newtonsoft.Json;
using System.IO;

namespace Gradual.Site.Www.Async
{
    public partial class ModuloCMS : Gradual.Site.Www.PaginaBase
    {
        #region Globais

        private string[] gListaDeExtensoesDeImagem = { "bmp", "gif", "jpg", "jpeg", "jpe", "png", "psd", "raw", "tga", "tif" };

        private string[] gListaDeExtensoesDePDF = { "pdf", "pdp" };

        private string[] gListaDeExtensoesDeOffice = { "doc", "docx", "ppt", "pptx", "xls", "xlsx" };

        private string[] gListaDeExtensoesDeZip = { "zip", "rar", "gzip", "7zip" };

        #endregion

        #region Métodos Private

        private string SalvarWidgetTitulo()
        {
            string lRetorno = "";

            try
            {
                widTitulo lWidget = new widTitulo();

                this.CarregarWidgetBaseComum(lWidget);

                lWidget.Tipo            = WidgetBase.TipoWidget.Titulo.ToString();

                lWidget.NivelDeTitulo   = Convert.ToByte(Request.Form["Nivel"]);

                lWidget.Texto           = Request.Form["Texto"];

                lWidget.WidgetJSON      = JsonConvert.SerializeObject(lWidget);

                int lIdWidget = this.SalvarWidget(lWidget);

                lRetorno = RetornarSucessoAjax(lIdWidget, "Widget atualizado com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Widget Titulo.", ex);
            }

            return lRetorno;
        }

        private string SalvarWidgetTexto()
        {
            string lRetorno = "";

            try
            {
                widTexto lWidget = new widTexto();

                this.CarregarWidgetBaseComum(lWidget);

                lWidget.Tipo          = WidgetBase.TipoWidget.Texto.ToString();

                lWidget.Texto         = Request.Form["Texto"]                 ;

                lWidget.WidgetJSON    = JsonConvert.SerializeObject(lWidget) ;

                int lIdWidget  = this.SalvarWidget(lWidget);

                lRetorno = RetornarSucessoAjax(lIdWidget, "Widget atualizado com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Widget Texto.", ex);
            }

            return lRetorno;
        }

        private string SalvarWidgetImagem()
        {
            string lRetorno = "";

            try
            {
                widImagem lWidget = new widImagem();

                this.CarregarWidgetBaseComum(lWidget); //Carrega as partes em comum

                lWidget.Tipo                  = WidgetBase.TipoWidget.Imagem.ToString();

                lWidget.AtributoSrc           = Request.Form["AtributoSrc"];

                lWidget.AtributoAlt           = Request.Form["AtributoAlt"];

                lWidget.LinkPara              = Request.Form["LinkPara"];

                lWidget.FlagTamanhoAutomatico = Request.Form["FlagTamanhoAutomatico"].DBToBoolean();

                lWidget.AtributoWidth         = Request.Form["AtributoWidth"];

                lWidget.AtributoHeight        = Request.Form["AtributoHeight"];

                lWidget.FlagHabilitarZoom     = Request.Form["FlagHabilitarZoom"].DBToBoolean();

                lWidget.WidgetJSON            = JsonConvert.SerializeObject(lWidget) ;

                int lIdWidget = this.SalvarWidget(lWidget);

                lRetorno = RetornarSucessoAjax(lIdWidget, "Widget atualizado com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Widget Imagem.", ex);
            }

            return lRetorno;
        }

        private string SalvarWidgetLista()
        {
            string lRetorno = "";

            try
            {
                widLista lWidget = new widLista();

                this.CarregarWidgetBaseComum(lWidget);

                lWidget.Tipo               = WidgetBase.TipoWidget.Lista.ToString();

                lWidget.Texto              = Request.Form["Texto"];

                lWidget.Atributos          = Request.Form["Atributos"];

                lWidget.TemplateDoItem     = Request.Form["TemplateDoItem"];

                lWidget.FlagListaEstatica  = Request.Form["FlagListaEstatica"].DBToBoolean();

                lWidget.Ordenacao          = Request.Form["Ordenacao"];

                lWidget.WidgetJSON = JsonConvert.SerializeObject(lWidget);

                int lIdWidget = this.SalvarWidget(lWidget);

                lRetorno = RetornarSucessoAjax(lIdWidget, "Widget atualizado com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Widget Lista.", ex);
            }

            return lRetorno;
        }

        private string SalvarWidgetTabela()
        {
            string lRetorno = "";

            try
            {
                widTabela lWidget = new widTabela();

                this.CarregarWidgetBaseComum(lWidget);

                lWidget.Tipo               = WidgetBase.TipoWidget.Tabela.ToString();
                
                lWidget.Cabecalho          = Request.Form["Cabecalho"];

                lWidget.Texto              = Request.Form["Texto"];

                lWidget.FlagTabelaEstatica = Request.Form["FlagTabelaEstatica"].DBToBoolean();

                lWidget.Ordenacao          = Request.Form["Ordenacao"];

                lWidget.WidgetJSON = JsonConvert.SerializeObject(lWidget);

                int lIdWidget = this.SalvarWidget(lWidget);

                lRetorno = RetornarSucessoAjax(lIdWidget, "Widget atualizado com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Widget Lista.", ex);
            }

            return lRetorno;
        }

        private string SalvarWidgetRepetidor()
        {
            string lRetorno = "";

            try
            {
                widRepetidor lWidget = new widRepetidor();

                this.CarregarWidgetBaseComum(lWidget);

                lWidget.Tipo            = WidgetBase.TipoWidget.Repetidor.ToString();

                lWidget.Ordenacao       = Request.Form["Ordenacao"];

                lWidget.TemplateDoItem  = Request.Form["TemplateDoItem"];

                lWidget.WidgetJSON = JsonConvert.SerializeObject(lWidget);

                int lIdWidget = this.SalvarWidget(lWidget);

                lRetorno = RetornarSucessoAjax(lIdWidget, "Widget atualizado com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Widget Lista.", ex);
            }

            return lRetorno;
        }

        private string SalvarWidgetListaDeDefinicao()
        {
            string lRetorno = "";

            try
            {
                widListaDeDefinicao lWidget = new widListaDeDefinicao();

                this.CarregarWidgetBaseComum(lWidget);

                lWidget.Tipo               = WidgetBase.TipoWidget.ListaDeDefinicao.ToString();

                lWidget.TemplateDoItem     = Request.Form["TemplateDoItem"];

                lWidget.Ordenacao          = Request.Form["Ordenacao"];

                lWidget.WidgetJSON = JsonConvert.SerializeObject(lWidget);

                int lIdWidget = this.SalvarWidget(lWidget);

                lRetorno = RetornarSucessoAjax(lIdWidget, "Widget atualizado com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Widget Lista de Definição", ex);
            }

            return lRetorno;
        }

        private string SalvarWidgetEmbed()
        {
            string lRetorno = "";

            try
            {
                widEmbed lWidget = new widEmbed();

                this.CarregarWidgetBaseComum(lWidget);

                lWidget.Tipo          = WidgetBase.TipoWidget.Embed.ToString();

                lWidget.Codigo        = Request.Form["Codigo"];

                lWidget.WidgetJSON    = JsonConvert.SerializeObject(lWidget) ;

                int lIdWidget  = this.SalvarWidget(lWidget);

                lRetorno = RetornarSucessoAjax(lIdWidget, "Widget atualizado com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Widget Embed.", ex);
            }

            return lRetorno;
        }

        private string SalvarWidgetTextoHTML()
        {
            string lRetorno = "";

            try
            {
                widTextoHTML lWidget = new widTextoHTML();

                this.CarregarWidgetBaseComum(lWidget);

                lWidget.Tipo       = WidgetBase.TipoWidget.TextoHTML.ToString();

                lWidget.ConteudoHTML  = Request.Form["ConteudoHTML"];

                if(!string.IsNullOrEmpty(lWidget.ConteudoHTML))
                    lWidget.ConteudoHTML = lWidget.ConteudoHTML.Replace('"', '\'');

                lWidget.WidgetJSON = JsonConvert.SerializeObject(lWidget);

                int lIdWidget  = this.SalvarWidget(lWidget);

                lRetorno = RetornarSucessoAjax(lIdWidget, "Widget atualizado com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Widget Texto HTML.", ex);
            }

            return lRetorno;
        }

        private string SalvarWidgetAbas()
        {
            string lRetorno = "";

            try
            {
                widAbas lWidget = new widAbas();

                this.CarregarWidgetBaseComum(lWidget);

                lWidget.Tipo          = WidgetBase.TipoWidget.Abas.ToString();

                lWidget.ListaDeAbas   = JsonConvert.DeserializeObject<List<widAbaItem>>(Request.Form["ListaDeAbas"]);

                lWidget.WidgetJSON    = JsonConvert.SerializeObject(lWidget) ;

                int lIdWidget  = this.SalvarWidget(lWidget);

                lRetorno = RetornarSucessoAjax(lIdWidget, "Widget atualizado com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Widget Abas.", ex);
            }

            return lRetorno;
        }

        private string SalvarWidgetAcordeon()
        {
            string lRetorno = "";

            try
            {
                widAcordeon lWidget = new widAcordeon();

                this.CarregarWidgetBaseComum(lWidget);

                lWidget.Tipo          = WidgetBase.TipoWidget.Acordeon.ToString();

                lWidget.ListaDeAbas   = JsonConvert.DeserializeObject<List<widAbaItem>>(Request.Form["ListaDeAbas"]);

                lWidget.WidgetJSON    = JsonConvert.SerializeObject(lWidget) ;

                int lIdWidget  = this.SalvarWidget(lWidget);

                lRetorno = RetornarSucessoAjax(lIdWidget, "Widget atualizado com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Widget Abas.", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Carrega todos as propriedades em comum dos Widget
        /// </summary>
        /// <param name="pWidget">Objeto Widget</param>
        private void CarregarWidgetBaseComum(WidgetBase pWidget)
        {
            pWidget.IdDaEstrutura = Request.Form["IDdaEstrutura"].DBToInt32();

            pWidget.IdDoWidget    = Request.Form["IDdoWidget"].DBToInt32();

            pWidget.IdDaLista     = Request.Form["IdDaLista"].DBToInt32();

            pWidget.OrdemNaPagina = Request.Form["OrdemNaPagina"].DBToInt32();

            pWidget.AtributoClass = Request.Form["AtributoClass"];

            pWidget.AtributoStyle = Request.Form["AtributoStyle"];
        }

        private void AtualizarWidgetNaListaDePaginas(int pIdPagina, WidgetInfo pWidget, byte pIncluirAtualizarExcluir)
        {
            foreach (PaginaInfo lPagina in this.ListaDePaginas)
            {
                if (lPagina.CodigoPagina == pIdPagina)
                {
                    foreach (VersaoInfo lVersao in lPagina.Versoes)
                    {
                        foreach (EstruturaInfo lEstrutura in lVersao.ListaEstrutura)
                        {
                            if (lEstrutura.CodigoEstrutura == pWidget.CodigoEstrutura)
                            {
                                if (pIncluirAtualizarExcluir == 0)
                                {
                                    lEstrutura.ListaWidget.Add(pWidget);

                                    LimparCacheParaPagina(pIdPagina);

                                    return;
                                }
                                else
                                {
                                    if (pIncluirAtualizarExcluir == 1)
                                    {
                                        // ao atualizar, pode ser somente dados do widget mesmo ou ordem na página; 
                                        // precisa pegar a lista inteira novamente; zerando ela o código da página base vai buscar novamente:

                                        lEstrutura.ListaWidget.Clear();

                                        LimparCacheParaPagina(pIdPagina);

                                        return;
                                    }
                                    else
                                    {
                                        for (int a = 0; a < lEstrutura.ListaWidget.Count; a++)
                                        {
                                            if (lEstrutura.ListaWidget[a].CodigoWidget == pWidget.CodigoWidget)
                                            {
                                                lEstrutura.ListaWidget.RemoveAt(a);

                                                LimparCacheParaPagina(pIdPagina);

                                                return;
                                            }
                                        } 
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private int SalvarWidget(WidgetBase pWidget)
        {
            int lRetorno = 0;

            int lIdPagina = Convert.ToInt32(Request["IdDaPagina"]);

            WidgetRequest lRequest = new WidgetRequest();
            WidgetResponse lResponse;

            bool lNovoWidget = (pWidget.IdDoWidget == 0);

            lRequest.Widget = new WidgetInfo();

            lRequest.Widget.CodigoWidget        = pWidget.IdDoWidget;

            lRequest.Widget.CodigoEstrutura     = pWidget.IdDaEstrutura;

            lRequest.Widget.CodigoListaConteudo = pWidget.IdDaLista;

            lRequest.Widget.OrdemPagina         = pWidget.OrdemNaPagina;

            lRequest.Widget.WidgetJson          = pWidget.WidgetJSON;

            //melhor não atualizar o "OrdemNaPagina" quando salvar o widget, caso o número saia de sincronia; só atualiza esse campo se ele tiver mexendo nos botoes específicos
            //pRequest.Widget.OrdemPagina     = pWidget.OrdemNaPagina;  

            lResponse = base.ServicoPersistenciaSite.InserirWidget(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                lRetorno = lRequest.Widget.CodigoWidget;

                //tem que atualizar a lista "offline" de widgets:
                AtualizarWidgetNaListaDePaginas(lIdPagina, lRequest.Widget, Convert.ToByte((lNovoWidget ? 0 : 1)));

                LimparCacheParaPagina(lIdPagina);
            }
            else
            {
                throw new Exception("Erro ao salvar o widget: " + lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        private string ResponderIncluirAtualizarWidget()
        {
            string lRetorno = "";

            try
            {
                WidgetBase.TipoWidget lTipo = (WidgetBase.TipoWidget)Enum.Parse(typeof(WidgetBase.TipoWidget), Request.Form["Tipo"]);

                switch (lTipo)
                {
                    case WidgetBase.TipoWidget.Titulo:

                        lRetorno = SalvarWidgetTitulo();

                        break;

                    case WidgetBase.TipoWidget.Texto:

                        lRetorno = SalvarWidgetTexto();

                        break;

                    case WidgetBase.TipoWidget.Imagem:

                        lRetorno = SalvarWidgetImagem();

                        break;

                    case WidgetBase.TipoWidget.Lista:

                        lRetorno = SalvarWidgetLista();

                        break;

                    case WidgetBase.TipoWidget.Tabela:

                        lRetorno = SalvarWidgetTabela();

                        break;

                    case WidgetBase.TipoWidget.Repetidor:

                        lRetorno = SalvarWidgetRepetidor();

                        break;

                    case WidgetBase.TipoWidget.ListaDeDefinicao:

                        lRetorno = SalvarWidgetListaDeDefinicao();

                        break;

                    case WidgetBase.TipoWidget.Embed:

                        lRetorno = SalvarWidgetEmbed();

                        break;

                    case WidgetBase.TipoWidget.Abas:

                        lRetorno = SalvarWidgetAbas();

                        break;

                    case WidgetBase.TipoWidget.Acordeon:

                        lRetorno = SalvarWidgetAcordeon();

                        break;

                    case WidgetBase.TipoWidget.TextoHTML:

                        lRetorno = SalvarWidgetTextoHTML();

                        break;

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderIncluirAtualizarWidget()", ex);
            }

            return lRetorno;
        }

        private string ResponderExcluirWidget()
        {
            string lRetorno = "";

            WidgetRequest lRequest = new WidgetRequest();
            WidgetResponse lResponse;

            lRequest.Widget = new WidgetInfo();
            lRequest.Widget.CodigoWidget = Request.Form["IdDoWidget"].DBToInt32();
            lRequest.Widget.CodigoEstrutura = Request.Form["IdDaEstrutura"].DBToInt32();

            lResponse = base.ServicoPersistenciaSite.ApagarWidget(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                AtualizarWidgetNaListaDePaginas(Convert.ToInt32(Request["IdDaPagina"]), lRequest.Widget, 2);

                lRetorno = RetornarSucessoAjax(lRequest.Widget.CodigoWidget.ToString());
            }
            else
            {
                lRetorno =RetornarErroAjax("Erro ao excluir Widget", lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        private string ResponderMoverWidget()
        {
            string lRetorno = "";

            try
            {
                AtualizarOrdemDoWidgetNaPaginaRequest lRequest = new AtualizarOrdemDoWidgetNaPaginaRequest();
                AtualizarOrdemDoWidgetNaPaginaResponse lResponse;

                lRequest.IdDaEstrutura  = Request.Form["IDdaEstrutura"].DBToInt32();
                
                lRequest.OrdemDeWidgets = new List<int>(  );

                string[] lIDsRequest = Convert.ToString(Request["OrdemDeWidgets"]).Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string id in lIDsRequest)
                {
                    lRequest.OrdemDeWidgets.Add(Convert.ToInt32(id));
                }

                lResponse = base.ServicoPersistenciaSite.AtualizarOrdemDoWidgetNaPagina(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    WidgetInfo lTempWidget = new WidgetInfo();

                    lTempWidget.CodigoEstrutura = lRequest.IdDaEstrutura;

                    AtualizarWidgetNaListaDePaginas(Convert.ToInt32(Request["IdDaPagina"]), lTempWidget, 1);

                    lRetorno = RetornarSucessoAjax("ok");
                }
                else
                {
                    lRetorno = RetornarErroAjax("Erro em ResponderMoverWidget()", lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderMoverWidget()", ex);
            }

            return lRetorno;
        }

        private string ResponderCarregarConteudoDinamico()
        {
            string lRetorno = "";

            string lObjetoJSON = "";

            int lIdTipoConteudo = Convert.ToInt32(Request.Form["IdTipoConteudo"]);

            ConteudoRequest lRequest = new ConteudoRequest();
            ConteudoResponse lResponse;

            List<object> lListaRetorno = new List<object>();

            lRequest.Conteudo = new ConteudoInfo();

            lRequest.Conteudo.CodigoTipoConteudo = lIdTipoConteudo;

            lResponse = base.ServicoPersistenciaSite.SelecionarConteudo(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                foreach (ConteudoInfo lConteudo in lResponse.ListaConteudo)
                {
                    lObjetoJSON = lConteudo.ConteudoJson.Trim().TrimStart('{'); //retira a primeira abertura do objeto

                    lObjetoJSON = "{\"IdTipoConteudo\": \"" + lConteudo.CodigoTipoConteudo + "\",\"CodigoConteudo\": \"" + lConteudo.CodigoConteudo + "\", " + lObjetoJSON;

                    lListaRetorno.Add(JsonConvert.DeserializeObject(lObjetoJSON));
                }

                if (lResponse.ListaConteudo.Count > 0)
                {
                    lRetorno = RetornarSucessoAjax(lListaRetorno, "Conteúdo carregado com sucesso.");
                }
                else
                {
                    lRetorno = RetornarSucessoAjax(lListaRetorno, "Não existe item cadastrado.");
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Erro ao selecionar o Conteúdo", lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        private string ResponderBuscarDadosDeConteudoDinamico()
        {
            string lRetorno = "";

            string lObjetoJSON = "";

            int lCodigoConteudo = Convert.ToInt32(Request.Form["CodigoConteudo"]);

            List<object> lListaRetorno = new List<object>();

            ConteudoRequest lRequest = new ConteudoRequest();
            ConteudoResponse lResponse;

            lRequest.Conteudo = new ConteudoInfo();

            lRequest.Conteudo.CodigoConteudo = lCodigoConteudo;

            lResponse = base.ServicoPersistenciaSite.SelecionarConteudo(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (lResponse.ListaConteudo[0] != null)
                {
                    lObjetoJSON = lResponse.ListaConteudo[0].ConteudoJson.Substring(0, lResponse.ListaConteudo[0].ConteudoJson.LastIndexOf("}")); //Retira o fecha do objeto

                    lObjetoJSON += ", \"IdTipoConteudo\": \"" + lResponse.ListaConteudo[0].CodigoTipoConteudo + "\",\"CodigoConteudo\": \"" + lResponse.ListaConteudo[0].CodigoConteudo + "\" }";

                    lResponse.ListaConteudo[0].ConteudoJson = lObjetoJSON;
                }

                lRetorno = RetornarSucessoAjax(lResponse.ListaConteudo[0], "OK");
            }
            else
            {
                lRetorno = RetornarErroAjax("Erro ao buscar o conteúdo", lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        private string ResponderCarregarListas()
        {
            string lRetorno = "";

            int lIdTipoConteudo = Convert.ToInt32(Request.Form["IdTipoConteudo"]);

            ListaConteudoRequest lRequest = new ListaConteudoRequest();
            ListaConteudoResponse lResponse;

            List<object> lListaRetorno = new List<object>();

            lRequest.Conteudo = new ListaConteudoInfo();

            lRequest.Conteudo.CodigoTipoConteudo = lIdTipoConteudo;

            lResponse = base.ServicoPersistenciaSite.SelecionarListaConteudo(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                foreach (ListaConteudoInfo lConteudo in lResponse.ListaConteudo)
                {
                    lListaRetorno.Add(lConteudo);
                }

                if (lResponse.ListaConteudo.Count > 0)
                {
                    lRetorno = RetornarSucessoAjax(lListaRetorno, "Conteúdo carregado com sucesso.");
                }
                else
                {
                    lRetorno = RetornarSucessoAjax(lListaRetorno, "Não existe item cadastrado.");
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Erro ao selecionar o Conteúdo", lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        private string ResponderUploadDeArquivo()
        {
            string lRetorno = "";

            try
            {
                List<string> lListaDeURLs = new List<string>();

                string lExtensaoDoArquivo, lPastaDeDestino, lDiretorio;

                for (int a = 0; a < Request.Files.Count; a++)
                {
                    lExtensaoDoArquivo = Path.GetExtension(Request.Files[a].FileName).ToLower().TrimStart('.');

                    if (gListaDeExtensoesDeImagem.Contains(lExtensaoDoArquivo))
                    {
                        lPastaDeDestino = "Imagens";
                    }
                    else if (gListaDeExtensoesDePDF.Contains(lExtensaoDoArquivo))
                    {
                        lPastaDeDestino = "PDFs";
                    }
                    else if (gListaDeExtensoesDeOffice.Contains(lExtensaoDoArquivo))
                    {
                        lPastaDeDestino = "Office";
                    }
                    else if (gListaDeExtensoesDeZip.Contains(lExtensaoDoArquivo))
                    {
                        lPastaDeDestino = "Zips";
                    }
                    else
                    {
                        lPastaDeDestino = "Outros";
                    }

                    lPastaDeDestino = string.Format("{0}/{1}-{2}", lPastaDeDestino, DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'));

                    string lCaminho = Path.Combine(Server.MapPath("~/Resc/Upload/"), lPastaDeDestino, Path.GetFileName(Request.Files[a].FileName));

                    if (File.Exists(lCaminho))
                    {
                        //lCaminho = lCaminho.Insert(lCaminho.LastIndexOf('.'), "-" + DateTime.Now.Ticks.ToString());

                        File.Delete(lCaminho);
                    }

                    lDiretorio = Path.GetDirectoryName(lCaminho);

                    if(!Directory.Exists(lDiretorio))
                    {
                        gLogger.InfoFormat("Diretório inexistente; tentando criar [{0}]...", lDiretorio);

                        Directory.CreateDirectory(lDiretorio);

                        gLogger.InfoFormat("Diretório criado com sucesso");
                    }

                    Request.Files[a].SaveAs(lCaminho);

                    lListaDeURLs.Add(this.HostERaiz + "/Resc/Upload/" + lPastaDeDestino + "/" + Path.GetFileName(lCaminho));
                }

                //zera o cache da lista de imagens:
                Application["ModuloCMS_ListaDeArquivos"] = null;

                lRetorno = RetornarSucessoAjax(lListaDeURLs, "ok!");

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao realizar upload", ex);
            }

            return lRetorno;
        }

        private string ResponderExcluirArquivo()
        {
            string lRetorno = "";

            try
            {
                string lDiretorio = Request["NomeDiretorio"];
                string lArquivo   = Request["NomeArquivo"];

                string lCaminho = Path.Combine(Server.MapPath( string.Format("~/Resc/Upload/{0}/{1}", lDiretorio.Replace("\\", "/"), lArquivo)));

                if (File.Exists(lCaminho))
                {
                    File.Delete(lCaminho);
                }

                //zera o cache da lista de imagens:
                Application["ModuloCMS_ListaDeArquivos"] = null;

                lRetorno = RetornarSucessoAjax((object)Request["NomeArquivo"], "Arquivo excluído com sucesso");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao excluir arquivo", ex);
            }

            return lRetorno;
        }

        private string ResponderExcluirConteudo()
        {
            string lRetorno = "";

            try
            {
                int lIdConteudo = Convert.ToInt32(Request.Form["CodigoConteudo"]);

                int lIdPagina = Convert.ToInt32(Request.Form["IdDaPagina"]);

                ConteudoRequest lRequest = new ConteudoRequest();
                ConteudoResponse lResponse;

                lRequest.Conteudo = new ConteudoInfo();

                lRequest.Conteudo.CodigoConteudo = lIdConteudo;

                lResponse = base.ServicoPersistenciaSite.ApagarConteudo(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lIdConteudo, "Conteúdo excluído com sucesso.");

                    ServicoPersistenciaSite.LimparCache(lIdPagina);
                }
                else
                {
                    lRetorno = RetornarErroAjax("Erro ao excluir o Conteúdo", lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderExcluirConteudo()", ex);
            }

            return lRetorno;  
        }

        private string ResponderExcluirBannerLateral()
        {
            string lRetorno = "";

            try
            {
                int lIdConteudo = Convert.ToInt32(Request.Form["IdBannerLink"]);

                int lIdPagina = Convert.ToInt32(Request.Form["IdPagina"]);

                ConteudoRequest lRequest = new ConteudoRequest();
                ConteudoResponse lResponse;

                lRequest.Conteudo = new ConteudoInfo();

                lRequest.Conteudo.CodigoConteudo = lIdConteudo;

                lResponse = base.ServicoPersistenciaSite.ApagarConteudo(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lIdConteudo, "OK");

                    LimparCacheDeBanners();

                    if (lIdPagina != 0)
                    {
                        //algumas páginas, como MinhaConta/Login.aspx deixam o IdPagina zerado mesmo, não tem cache.
                        ServicoPersistenciaSite.LimparCache(lIdPagina);
                    }
                }
                else
                {
                    lRetorno = RetornarErroAjax("Erro ao excluir o Banner", lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderExcluirBannerLateral()", ex);
            }

            return lRetorno;  
        }

        private string ResponderSalvarBannerLateralLink()
        {
            string lRetorno = "";

            int lIdConteudo, lIdPagina;

            ConteudoRequest lRequest = new ConteudoRequest();
            ConteudoResponse lResponse;

            try
            {
                lRequest = new ConteudoRequest();

                lRequest.Conteudo = new ConteudoInfo();

                lIdPagina = Convert.ToInt32(Request["IdDaPagina"]);

                this.CarregarObjetoConteudo(lRequest.Conteudo);

                this.MontarPropriedadeConteudo(lRequest); //Monta o Valor das propriedades do objeto.

                lResponse = base.ServicoPersistenciaSite.InserirConteudo(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lResponse.Conteudo.CodigoConteudo, "Conteúdo foi salvo com sucesso.");

                    LimparCacheDeBanners();

                    if (lIdPagina != 0)
                    {
                        //MinhaConta/Login.aspx não tem id de página, por exemplo
                        ServicoPersistenciaSite.LimparCache(lIdPagina);
                    }
                }
                else
                {
                    lRetorno = RetornarErroAjax("Erro no salvar link banner lateral", lResponse.DescricaoResposta);
                }

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderSalvarBannerLateralLink()", ex);
            }

            return lRetorno;
        }

        private string ResponderExcluirTipoConteudo()
        {
            string lRetorno = "";

            int lIdTipoConteudo = Convert.ToInt32(Request.Form["CodigoTipoConteudo"]);

            TipoConteudoRequest lRequest = new TipoConteudoRequest();
            TipoConteudoResponse lResponse;

            lRequest.TipoConteudo = new TipoDeConteudoInfo();

            lRequest.TipoConteudo.IdTipoConteudo = lIdTipoConteudo;

            lResponse = base.ServicoPersistenciaSite.ApagarTipoConteudo(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                lRetorno = RetornarSucessoAjax("Item excluído com sucesso.");
            }
            else
            {
                lRetorno = RetornarErroAjax("Erro ao apgar o TipoConteúdo", lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        private string ResponderSalvarConteudo()
        {
            string lRetorno = "";

            int lIdConteudo, lIdCodigoConteudo, lIdPagina;

            ConteudoRequest lRequest = new ConteudoRequest();
            ConteudoResponse lResponse;

            try
            {
                lIdConteudo = Request["CodigoConteudo"].DBToInt32();
                lIdCodigoConteudo = Request["CodigoTipoConteudo"].DBToInt32();
                lIdPagina = Request["IdDaPagina"].DBToInt32();

                if (lIdCodigoConteudo == ConfiguracoesValidadas.IdDoTipo_BannerLateral || lIdCodigoConteudo == ConfiguracoesValidadas.IdDoTipo_BannerLateralLink)
                {
                    Application["BannersLateraisDisponiveis"] = null; //zera pra recarregar depois novamente
                    Application["BannersLateraisPorPagina"] = null; //zera pra recarregar depois novamente
                }

                if (lIdCodigoConteudo == ConfiguracoesValidadas.IdDoTipo_MenuPrincipal)
                {
                    Application["HtmlDoMenuPrincipal"] = null;  //zera pra recarregar depois novamente
                }

                if (lIdConteudo > 0) //alterar Conteúdo
                {
                    lRequest.Conteudo = new ConteudoInfo();

                    lRequest.Conteudo.CodigoConteudo = lIdConteudo;

                    lResponse = base.ServicoPersistenciaSite.SelecionarConteudo(lRequest); //recupera as outras propriedades do conteudo pelo ID

                    if (lResponse.ListaConteudo.Count > 0)
                    {
                        this.CarregarObjetoConteudo(lResponse.ListaConteudo[0]);

                        lRequest = new ConteudoRequest();

                        lRequest.Conteudo = lResponse.ListaConteudo[0];

                        this.MontarPropriedadeConteudo(lRequest); //Monta o Valor das propriedades do objeto.

                        lResponse = base.ServicoPersistenciaSite.InserirConteudo(lRequest);

                        if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax(lResponse.Conteudo.CodigoConteudo, "Conteúdo foi salvo com sucesso.");

                            ServicoPersistenciaSite.LimparCache(lIdPagina);
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax("Erro no salvar Conteudo", lResponse.DescricaoResposta);
                        }
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax("Não foi possível recuperar o conteúdo");
                    }
                }
                else // Inserir novo conteúdo
                {
                    lRequest = new ConteudoRequest();

                    lRequest.Conteudo = new ConteudoInfo();

                    this.CarregarObjetoConteudo(lRequest.Conteudo);

                    this.MontarPropriedadeConteudo(lRequest); //Monta o Valor das propriedades do objeto.

                    lResponse = base.ServicoPersistenciaSite.InserirConteudo(lRequest);

                    if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = RetornarSucessoAjax(lResponse.Conteudo.CodigoConteudo, "Conteúdo foi salvo com sucesso.");

                        ServicoPersistenciaSite.LimparCache(lIdPagina);
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax("Erro no salvar Conteudo", lResponse.DescricaoResposta);
                    }
                }

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderSalvarConteudo()", ex);
            }

            return lRetorno;
        }

        private void CarregarObjetoConteudo(ConteudoInfo pConteudo)
        {
            pConteudo.CodigoTipoConteudo = Request["CodigoTipoConteudo"].DBToInt32();
            pConteudo.ValorTag           = Request["Tag"].DBToString();
            pConteudo.ConteudoJson       = Request["ConteudoJson"];
            pConteudo.ConteudoHtml       = Request["ConteudoHtml"];

            if (!string.IsNullOrEmpty(pConteudo.ConteudoHtml))
                pConteudo.ConteudoHtml = pConteudo.ConteudoHtml.Replace('"', '\'');

            if (Request["DataInicial"] != null)
            {
                pConteudo.DtInicio = Request["DataInicial"].DBToDateTime();
            }
            else
            {
                pConteudo.DtInicio = null;
            }

            if (Request["DataFinal"] != null)
            {
                pConteudo.DtFim = Request["DataFinal"].DBToDateTime();
            }
            else
            {
                pConteudo.DtFim = null;
            }

            if (Request["DataCadastro"] != null)
            {
                pConteudo.DtCriacao = Request["DataCadastro"].DBToDateTime();
            }
            else
            {
                pConteudo.DtCriacao = null;
            }
        }

        private void MontarPropriedadeConteudo(ConteudoRequest pConteudo)
        {
            TipoConteudoRequest lRequest = new TipoConteudoRequest();
            TipoConteudoResponse lResponse;

            lRequest.TipoConteudo = new TipoDeConteudoInfo();

            lRequest.TipoConteudo.IdTipoConteudo = pConteudo.Conteudo.CodigoTipoConteudo;

            lResponse = base.ServicoPersistenciaSite.SelecionarTipoConteudo(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                pConteudo.Conteudo.ValorPropriedade1 = this.RecuperarValorPropriedade(lResponse.ListaTipoConteudo[0].NomePropriedade1);

                pConteudo.Conteudo.ValorPropriedade2 = this.RecuperarValorPropriedade(lResponse.ListaTipoConteudo[0].NomePropriedade2);

                pConteudo.Conteudo.ValorPropriedade3 = this.RecuperarValorPropriedade(lResponse.ListaTipoConteudo[0].NomePropriedade3);

                pConteudo.Conteudo.ValorPropriedade4 = this.RecuperarValorPropriedade(lResponse.ListaTipoConteudo[0].NomePropriedade4);

                pConteudo.Conteudo.ValorPropriedade5 = this.RecuperarValorPropriedade(lResponse.ListaTipoConteudo[0].NomePropriedade5);
            }
            else
            {
                throw new Exception("Erro ao selecionar o Tipo de Conteúdo");
            }
        }

        private string RecuperarValorPropriedade(string pPropriedade)
        {
            string lRetorno = "";

            switch (pPropriedade)
            {
                case "UrlDaPagina":

                    lRetorno = Request["UrlDaPagina"];

                    break;

                case "Banner":

                    lRetorno = Request["Banner"];

                    break;

                case "CodigoFundo":

                    lRetorno = Request["CodigoFundo"];

                    break;

                case "Link":

                    lRetorno = Request["Link"];

                    break;

                case "LinkParaArquivo":

                    lRetorno = Request["LinkParaArquivo"];

                    break;

                case "DataCadastro":

                    lRetorno = Request["DataCadastro"];

                    break;

                case "DataInicial":

                    lRetorno = Request["DataInicial"];

                    break;

                case "DataFinal":

                    lRetorno = Request["DataFinal"];

                    break;

                case "Tag":

                    lRetorno = Request["Tag"];

                    break;

                case "Posicao":

                    lRetorno = Request["Posicao"];

                    break;

                case "FlagPublicado":

                    lRetorno = Request["FlagPublicado"];

                    break;

                case "Ativo":

                    lRetorno = Request["Ativo"];

                    break;

                default:

                    lRetorno = "";

                    break;
            }

            return lRetorno;
        }

        private string ResponderSalvarLista()
        {
            string lRetorno = "";

            ListaConteudoRequest lRequest = new ListaConteudoRequest();
            ListaConteudoResponse lResponse;

            lRequest.Conteudo = new ListaConteudoInfo();

            lRequest.Conteudo.CodigoLista        = Request["CodigoLista"].DBToInt32();
            lRequest.Conteudo.CodigoTipoConteudo = Request["CodigoTipoConteudo"].DBToInt32();
            lRequest.Conteudo.Regra              = Request["Regra"];
            lRequest.Conteudo.DescricaoLista     = Request["Descricao"];

            lResponse = base.ServicoPersistenciaSite.InserirListaConteudo(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                lRetorno = RetornarSucessoAjax(lResponse.Conteudo.CodigoLista, "Lista salva com sucesso.");
            }
            else
            {
                lRetorno = RetornarErroAjax("Erro ao salvar Lista", lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        private string ResponderExcluirLista()
        {
            string lRetorno = "";

            try
            {
                int lIdListaConteudo = Convert.ToInt32(Request.Form["CodigoLista"]);

                ListaConteudoRequest lRequest = new ListaConteudoRequest();
                ListaConteudoResponse lResponse;

                lRequest.Conteudo = new ListaConteudoInfo();

                lRequest.Conteudo.CodigoLista = lIdListaConteudo;

                lResponse = base.ServicoPersistenciaSite.ApagarListaConteudo(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lIdListaConteudo, "Lista excluída com sucesso.");
                }
                else if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.ErroValidacao)
                {
                    lRetorno = RetornarErroAjax("Conflito: widgets utilizando a lista", lResponse.DescricaoResposta);
                }
                else
                {
                    lRetorno = RetornarErroAjax("Erro ao excluir a Lista", lResponse.DescricaoResposta);
                }

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderExcluirLista()", ex);
            }

            return lRetorno;
        }

        private string ResponderCarregarItensDaLista()
        {
            string lRetorno = "";

            BuscarItensDaListaRequest lRequest = new BuscarItensDaListaRequest();
            BuscarItensDaListaResponse lResponse;

            if (!string.IsNullOrEmpty(Request["IdDaLista"]))
            {
                lRequest.IdDaLista = Convert.ToInt32(Request["IdDaLista"]);
            }
            else
            {
                lRequest.IdDaLista = -1;

                lRequest.CodigoMensagem = string.Format("{0};{1}", Request["IdTipoConteudo"], Request["RegraDaLista"]);
            }

            lResponse = base.ServicoPersistenciaSite.BuscarItensDaLista(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                //lRetorno.

                string lObjetoJSON;

                List<object> lListaRetorno = new List<object>();

                foreach (ConteudoInfo lConteudo in lResponse.Itens)
                {
                    lObjetoJSON = lConteudo.ConteudoJson.Substring(0, lConteudo.ConteudoJson.LastIndexOf('}')); //Retira o fecha do objeto

                    lObjetoJSON += ", \"IdTipoConteudo\": \"" + lConteudo.CodigoTipoConteudo + "\",\"CodigoConteudo\": \"" + lConteudo.CodigoConteudo + "\", }";

                    lListaRetorno.Add(JsonConvert.DeserializeObject(lObjetoJSON));
                }

                if (lResponse.Itens.Count > 0)
                {
                    lRetorno = RetornarSucessoAjax(lListaRetorno, "Conteúdo carregado com sucesso.");
                }
                else
                {
                    lRetorno = RetornarSucessoAjax(lListaRetorno, "Não existe item cadastrado.");
                }
            }
            else
            {
                //coloca alguma mensagem?
            }

            return lRetorno;
        }

        private string BuscarConteudoHTML()
        {
            string lRetorno = "";

            try
            {
                int lCodigoConteudo = Convert.ToInt32(Request["CodigoConteudo"]);

                ConteudoRequest lRequest = new ConteudoRequest();
                ConteudoResponse lResponse;

                lRequest.Conteudo = new ConteudoInfo();

                lRequest.Conteudo.CodigoConteudo = lCodigoConteudo;

                lResponse = base.ServicoPersistenciaSite.SelecionarConteudo(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.ListaConteudo[0] != null)
                    {
                        lRetorno = lResponse.ListaConteudo[0].ConteudoHtml;

                        lRetorno = lRetorno.Replace("\\n", Environment.NewLine);
                    }
                    else
                    {
                        lRetorno = string.Format("<p>Nenhum Conteúdo Dinâmico com ID [{0}]</p>", lCodigoConteudo);
                    }
                }
                else
                {
                    lRetorno = string.Format("<p>Erro em BuscarConteudoHTML(): <br /><br />{0}</p>", lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = string.Format("<p>Erro em BuscarConteudoHTML(): <br /><br />{0} <br /><br />{1}</p>", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        private string ResponderMudarTipoDeCliente()
        {
            string lRetorno = "";

            try
            {
                int lID = Convert.ToInt32(Request["IdTipoDeCliente"]);

                Session["TipoDeClientePreview"] = lID;

                lRetorno = RetornarSucessoAjax("ok");
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderMudarTipoDeCliente()", ex);
            }

            return lRetorno;
        }

        private string ResponderCopiarEstrutura()
        {
            string lRetorno = "";

            try
            {
                EstruturaRequest lRequest = new EstruturaRequest();
                EstruturaResponse lResposta;

                lRequest.Estrutura = new EstruturaInfo();

                lRequest.Estrutura.CodigoPagina    = Request["IdDaPagina"].DBToInt32();
                lRequest.Estrutura.TipoUsuario     = Request["IdTipoUsuarioPara"].DBToInt32();
                lRequest.Estrutura.CodigoEstrutura = Request["IdDaEstrutura"].DBToInt32();

                //base.gIDPagina, Request["IdTipoUsuarioDe"].DBToInt32(), 

                lResposta = base.ServicoPersistenciaSite.CopiarEstrutra(lRequest);

                if (lResposta.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax("ok");

                    LimparCacheParaPagina(lRequest.Estrutura.CodigoPagina);
                }
                else
                {
                    gLogger.ErrorFormat("Resposta com erro de ServicoPersistenciaSite.CopiarEstrutra(pIdPagina: [{0}], pIdEstrutura: [{1}], IdTipoUsuarioPara: [{2}] em ModuloCMS.ResponderCopiarEstrutura > [{3}]\r\n{4}"
                                        , lRequest.Estrutura.CodigoPagina
                                        , lRequest.Estrutura.CodigoEstrutura
                                        , lRequest.Estrutura.TipoUsuario
                                        , lResposta.StatusResposta
                                        , lResposta.DescricaoResposta);

                    lRetorno = RetornarErroAjax("Erro ao copiar a estrutra");
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderCopiarEstrutura()", ex);
            }

            return lRetorno;
        }

        private string ResponderSalvarPagina()
        {
            PaginaRequest lRequest = new PaginaRequest();
            PaginaResponse lResponse;

            lRequest.Pagina = new PaginaInfo();

            string lRetorno = "";

            try
            {
                if (!string.IsNullOrEmpty(Request["IdPagina"]))
                {
                    lRequest.Pagina.CodigoPagina = Convert.ToInt32(Request["IdPagina"]);
                }

                lRequest.Pagina.TipoEstrutura = Request["TipoDaEstrutura"];
                lRequest.Pagina.NomePagina    = Request["Titulo"];
                lRequest.Pagina.DescURL       = Request["Url"];
                
                if(lRequest.Pagina.DescURL.Contains("\\"))
                    lRequest.Pagina.DescURL = lRequest.Pagina.DescURL.Replace("\\", "");
                
                if(lRequest.Pagina.DescURL.Contains("?"))
                    lRequest.Pagina.DescURL = lRequest.Pagina.DescURL.Replace("?", "");
                
                if(lRequest.Pagina.DescURL.Contains("&"))
                    lRequest.Pagina.DescURL = lRequest.Pagina.DescURL.Replace("&", "");
                
                if(lRequest.Pagina.DescURL.Contains(" "))
                    lRequest.Pagina.DescURL = lRequest.Pagina.DescURL.Replace(" ", "");

                if(!string.IsNullOrEmpty(Request["Manter"]))
                    lRequest.MergeFrom = Convert.ToInt32(Request["Manter"]);

                if (!string.IsNullOrEmpty(Request["Versao"]))
                {
                    lRequest.Pagina.Versoes = new List<VersaoInfo>();

                    lRequest.Pagina.Versoes.Add(new VersaoInfo() { CodigoDeIdentificacao=Request["versao"], Publicada = true });
                }

                lResponse = this.ServicoPersistenciaSite.InserirPagina(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax( lResponse.Pagina.CodigoPagina.Value.ToString() );

                    base.ZerarCacheDePaginas();
                }
                else if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.ErroValidacao)
                {
                    if (lResponse.DescricaoResposta == "url_ja_existe")
                    {
                        lRetorno = RetornarSucessoAjax("url_ja_existe");
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax("Erro de validação: " + lResponse.DescricaoResposta);
                    }
                }
                else
                {
                        lRetorno = RetornarErroAjax("Erro do serviço: " + lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderSalvarPagina()", ex);
            }

            return lRetorno;
        }

        private string ResponderExcluirPagina()
        {
            PaginaRequest lRequest = new PaginaRequest();
            PaginaResponse lResponse;

            lRequest.Pagina = new PaginaInfo();

            string lRetorno = "";

            try
            {
                lRequest.Pagina.CodigoPagina = Convert.ToInt32(Request["IdPagina"]);

                lResponse = this.ServicoPersistenciaSite.ExcluirPagina(lRequest);

                lRetorno = RetornarSucessoAjax("ok");

                base.ZerarCacheDePaginas();
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderExcluirPagina()", ex);
            }

            return lRetorno;
        }

        private string ResponderBuscarHtmlParaAba()
        {
            string lRetorno = "";

            try
            {
                int lCodigo = Convert.ToInt32(Request["IdConteudo"]);

                BuscarHtmlDaPaginaRequest  lRequest = new BuscarHtmlDaPaginaRequest();
                BuscarHtmlDaPaginaResponse lResponse;

                lRequest.IdDaPagina = lCodigo;
                lRequest.RenderizandoNaAba = 1;

                lResponse = base.ServicoPersistenciaSite.BuscarHtmlPagina(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = lResponse.HTML;

                    if (string.IsNullOrEmpty(lRetorno))
                    {
                        lRetorno = "(página vazia)";
                    }
                }
                else
                {
                    lRetorno = string.Format("<p>Erro em ResponderBuscarHtmlParaAba(): <br /><br />{0}</p>", lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = string.Format("<p>Erro em ResponderBuscarHtmlParaAba(): <br /><br />{0} <br /><br />{1}</p>", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        private string ResponderCriarVersao()
        {
            string lRetorno = "";

            VersaoRequest lRequest = new VersaoRequest();
            VersaoResponse lResponse;

            lRequest.Versao = new VersaoInfo();

            try
            {
                lRequest.Versao.CodigoPagina = Convert.ToInt32(Request["IdPagina"]);
                lRequest.Versao.CodigoDeIdentificacao = Request["versao"];

                if (!string.IsNullOrEmpty(lRequest.Versao.CodigoDeIdentificacao))
                {
                    lResponse = this.ServicoPersistenciaSite.IncluirVersao(lRequest);

                    lRetorno = RetornarSucessoAjax((object)lResponse.Versao.CodigoDeIdentificacao, "ok");

                    base.ZerarCacheDePaginas();

                    this.ListaDeVersoes.Add(lResponse.Versao.CodigoDeIdentificacao);
                }
                else
                {
                    lRetorno = RetornarErroAjax("Versão vazia");
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderCriarVersao()", ex);
            }

            return lRetorno;
        }

        private string ResponderPublicarVersao()
        {
            string lRetorno = "";

            VersaoRequest lRequest = new VersaoRequest();
            VersaoResponse lResponse;

            lRequest.Versao = new VersaoInfo();

            try
            {
                lRequest.Versao.CodigoPagina = Convert.ToInt32(Request["IdPagina"]);
                lRequest.Versao.CodigoDeIdentificacao = Request["versao"];

                if (!string.IsNullOrEmpty(lRequest.Versao.CodigoDeIdentificacao))
                {
                    lResponse = this.ServicoPersistenciaSite.PublicarVersao(lRequest);

                    LimparCacheParaPagina(lRequest.Versao.CodigoPagina);

                    ZerarCacheDePaginas();

                    lRetorno = RetornarSucessoAjax("ok");
                }
                else
                {
                    lRetorno = RetornarErroAjax("Versão vazia");
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro em ResponderPublicarVersao()", ex);
            }

            return lRetorno;
        }


        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessaoClienteLogado == null)
            {
                Response.Clear();

                Response.Write( RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO) );

                Response.End();
            }

            bool lPodeEditar = SessaoClienteLogado.Pode(Gradual.Site.Www.TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCMS);

            if (!lPodeEditar)
            {
                string lIdPagina = Request["IdDaPagina"];

                int lID;

                if (int.TryParse(lIdPagina, out lID))
                {
                    if (lID == DadosDeAplicacao.IDsDasPaginas[DadosDeAplicacao.ANALISESEMERCADO_ANALISESECONOMICAS]
                    &&  SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseEconomica)
                        )
                    {
                        lPodeEditar = true;
                    }

                    if (lID == DadosDeAplicacao.IDsDasPaginas[DadosDeAplicacao.ANALISESEMERCADO_ANALISESFUNDAMENTALISTAS]
                    &&  SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseFundamentalista)
                        )
                    {
                        lPodeEditar = true;
                    }

                    if (lID == DadosDeAplicacao.IDsDasPaginas[DadosDeAplicacao.ANALISESEMERCADO_ANALISESGRAFICAS]
                    &&  SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseGrafica)
                        )
                    {
                        lPodeEditar = true;
                    }

                    if (lID == DadosDeAplicacao.IDsDasPaginas[DadosDeAplicacao.ANALISESEMERCADO_CARTEIRASRECOMENDADAS]
                    &&  SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCarteirasRecomendadas)
                        )
                    {
                        lPodeEditar = true;
                    }
                    
                    if (lID == DadosDeAplicacao.IDsDasPaginas[DadosDeAplicacao.INSTITUCIONAL_NIKKEIDESK]
                    &&  SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarNikkei)
                        )
                    {
                        lPodeEditar = true;
                    }
                    
                    if (lID == DadosDeAplicacao.IDsDasPaginas[DadosDeAplicacao.INSTITUCIONAL_NIKKEIDESK_JP]
                    &&  SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarNikkei)
                        )
                    {
                        lPodeEditar = true;
                    }
                    
                    if (lID == DadosDeAplicacao.IDsDasPaginas[DadosDeAplicacao.INSTITUCIONAL_GRADIUSGESTAO]
                    &&  SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarGradiusGestao)
                        )
                    {
                        lPodeEditar = true;
                    }
                }

                //upload pode se tiver qualquer permissão:
                if (Request.Files.Count > 0
                &&  (
                      SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCarteirasRecomendadas)   ||
                      SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseFundamentalista)  ||
                      SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseGrafica)          ||
                      SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarGradiusGestao)           ||
                      SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarNikkei)                  ||
                      SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCarteirasRecomendadas)
                    )
                   )
                {
                    lPodeEditar = true;
                }
            }

            if (lPodeEditar)
            {
                if (Request["Acao"] == "BuscarConteudoHTML")
                {
                    //busca o conteudoHTML de um Conteudo Dinâmico e responde como HTML mesmo, não JSON

                    Response.Clear();

                    string lConteudo = BuscarConteudoHTML();

                    Response.Write(lConteudo);

                    Response.End();
                }
                else if (Request.Files.Count > 0)
                {
                    Response.Clear();

                    Response.Write(ResponderUploadDeArquivo());

                    Response.End();
                }
                else
                {
                    RegistrarRespostasAjax(new string[]{
                                                        "IncluirAtualizarWidget",
                                                        "ExcluirWidget",
                                                        "CarregarConteudoDinamico",
                                                        "BuscarDadosDeConteudoDinamico",
                                                        "CarregarListas",
                                                        "UploadDeArquivo",
                                                        "ExcluirArquivo",
                                                        "MoverWidget",
                                                        "ExcluirConteudoDinamico",
                                                        "ExcluirBannerLateral",
                                                        "ExcluirTipoConteudo",
                                                        "SalvarConteudoDinamico",
                                                        "SalvarBannerLateralLink",
                                                        "Salvarlista",
                                                        "ExcluirLista",
                                                        "CarregarItensDaLista",
                                                        "MudarTipoDeCliente",
                                                        "CopiarEstrutura",
                                                        "SalvarPagina",
                                                        "ExcluirPagina",
                                                        "BuscarHtmlParaAba",
                                                        "PublicarVersao",
                                                        "CriarVersao"
                                                        },
                            new ResponderAcaoAjaxDelegate[]{
                                                        ResponderIncluirAtualizarWidget,
                                                        ResponderExcluirWidget,
                                                        ResponderCarregarConteudoDinamico,
                                                        ResponderBuscarDadosDeConteudoDinamico,
                                                        ResponderCarregarListas,
                                                        ResponderUploadDeArquivo,
                                                        ResponderExcluirArquivo,
                                                        ResponderMoverWidget,
                                                        ResponderExcluirConteudo,
                                                        ResponderExcluirBannerLateral,
                                                        ResponderExcluirTipoConteudo,
                                                        ResponderSalvarConteudo,
                                                        ResponderSalvarBannerLateralLink,
                                                        ResponderSalvarLista,
                                                        ResponderExcluirLista,
                                                        ResponderCarregarItensDaLista,
                                                        ResponderMudarTipoDeCliente,
                                                        ResponderCopiarEstrutura,
                                                        ResponderSalvarPagina,
                                                        ResponderExcluirPagina,
                                                        ResponderBuscarHtmlParaAba,
                                                        ResponderPublicarVersao,
                                                        ResponderCriarVersao
                                                        });
                }
            }
            else
            {
                Response.Clear();

                Response.Write(RetornarErroAjax("SEM_ACESSO"));

                Response.End();
            }
        }
        
        #endregion
    }
}