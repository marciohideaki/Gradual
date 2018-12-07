using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.Www
{
    public partial class PaginaDinamica : PaginaBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            base.Page_Init(sender, e);

            gLogger.InfoFormat("Recebido PaginaDinamica.aspx?url={0}", Request["url"]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string lUrl = Request["url"];

            string lParams;

            string lVersao = "";

            string lIdRequisitada = "";

            if (!string.IsNullOrEmpty(lUrl))
            {
                try
                {
                    if (!string.IsNullOrEmpty(RaizDoSite))
                    {
                        lUrl = lUrl.Substring(RaizDoSite.Length + 1);
                    }

                    if (lUrl.Contains('?'))
                    {
                        lParams = lUrl.Substring(lUrl.IndexOf("?") + 1);

                        lUrl = lUrl.Substring(0, lUrl.IndexOf("?"));

                        if (!string.IsNullOrEmpty(lParams))
                        {
                            string[] lParamsDecode = lParams.Split('&');

                            string[] lValuesDecode;

                            for (int a = 0; a < lParamsDecode.Length; a++)
                            {
                                lValuesDecode = lParamsDecode[a].Split('=');

                                if (lValuesDecode[0].ToLower() == "versao")
                                {
                                    lVersao = lValuesDecode[1];
                                }

                                if (lValuesDecode[0].ToLower() == "id")
                                {
                                    lIdRequisitada = lValuesDecode[1];

                                    this.PaginaMaster.ExibindoUmItem = true;
                                }
                            }
                        }
                    }

                    //quirk mode: a montagem de links pelos templates de widget precisa do ".aspx" para consequir identificar um link,
                    //portanto precisamos deixar o link como "OerftasPublicas.aspx"

                    if (lUrl.ToLower().Contains(".aspx"))
                    {
                        foreach (string lPaginaQueIgnoraAspx in ConfiguracoesValidadas.PaginasQueIgnoramExtenaoAspx)
                        {
                            if (lUrl.ToLower().Contains(lPaginaQueIgnoraAspx.ToLower()))
                            {
                                lUrl = lUrl.ToLower().Replace(".aspx", "");

                                if (string.IsNullOrEmpty(lIdRequisitada))
                                {
                                    //se o usuário acessou "ofertapuvblica.aspx" sem ?id=X, então melhor redirecionar pra sem .aspx 
                                    //porque se não o CMS vai reclamar que a página .aspx não existe
                                    Response.Redirect("/" + Request["url"].Replace(".aspx", ""));
                                }

                                break;
                            }
                        }
                    }

                    if (lUrl.EndsWith("?"))
                        lUrl = lUrl.TrimEnd('?');

                    if (lUrl.EndsWith("/"))
                        lUrl = lUrl.TrimEnd('/');
                }
                catch { }

                if (!ModuloCMSSeraExibido() && lUrl.ToLower().Contains("/abas/"))
                {
                    //não deixa visitantes verem uma página "aba" como página sozinha, substitui pela página pai:

                    string lUrlPai = BuscarPaginaPai(lUrl);

                    foreach (PaginaInfo lInfo in base.ListaDePaginas)
                    {
                        if (   lInfo.DescURL.ToLower() == lUrlPai.ToLower()
                            || lInfo.DescURL.ToLower().Replace("/default.aspx", "") == lUrlPai.ToLower()
                            || lInfo.DescURL.ToLower().Replace("/default", "")      == lUrlPai.ToLower())
                        {
                            lUrl = lInfo.DescURL;

                            //achou a página pai, redireciona:
                            Response.Redirect("~/" + lUrl);

                            return;
                        }
                    }
                }

                if (string.IsNullOrEmpty(lUrl) || lUrl.ToLower() == "default.aspx")
                {
                    // a regra de "unless" do redirecionamento no web.config acaba pegando o valor em branco da raiz
                    Server.Transfer("Default.aspx");
                }
                else
                {
                    try
                    {
                        CarregarPagina(rdrConteudo, lUrl, lVersao, lIdRequisitada);

                        if (this.IDEstruturaCorrente == 0)
                        {
                            pnlMensagemSemEstrutura.Visible = true;

                            lblURLNaoEncontrada.Text = lUrl;

                            lblVersaoNaoEncontrada.Text = lVersao;
                        }

                        if (ModuloCMSSeraExibido())
                        {
                            RodarJavascriptOnLoad("ModuloCMS_Load();");
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Sem página cadastrada"))
                        {
                            string lURL = ex.Message.Substring(ex.Message.IndexOf('['));

                            lUrl = lUrl.TrimEnd("] ".ToCharArray());

                            Response.Redirect("~/404.aspx?url=" + lUrl);
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
}