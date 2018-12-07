using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Gradual.Site.DbLib.Dados;
using Gradual.Site.DbLib;
using Gradual.OMS.Library.Servicos;
using Gradual.Site.DbLib.Mensagens;
using Newtonsoft.Json;

namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class ModuloCMS : UserControlBase
    {
        #region Propriedades

        public string IdEstrutura { get; set; }

        public List<int> TiposDeUsuarioDisponiveis
        {
            get
            {
                return ((PaginaBase)this.Page).TiposDeUsuarioLogadoDisponiveis;
            }
        }

        private IEnumerable<RecursoArquivo> ListaDeArquivos
        {
            get
            {
                return (IEnumerable<RecursoArquivo>)Application["ModuloCMS_ListaDeArquivos"]; 
            }

            set
            {
                Application["ModuloCMS_ListaDeArquivos"] = value;
            }
        }

        private List<string> ListaDeDiretoriosDeUpload
        {
            get
            {
                return (List<string>)Application["ModuloCMS_ListaDeDiretorios"];
            }

            set
            {
                Application["ModuloCMS_ListaDeDiretorios"] = value;
            }
        }

        #endregion 

        #region Métodos Private

        private void CarregarListaDeImagens(bool pForcarRecarregamento)
        {
            if (this.ListaDeDiretoriosDeUpload == null || pForcarRecarregamento)
            {
                this.ListaDeDiretoriosDeUpload = new List<string>();
            }

            if (this.ListaDeArquivos == null || pForcarRecarregamento)
            {
                string lBase = ((PaginaBase)this.Page).RaizDoSite;

                if(!string.IsNullOrEmpty(lBase))
                    lBase = "/" + lBase;

                string lCaminho = Server.MapPath(string.Format("{0}/Resc/Upload", lBase));

                string[] lDiretorios = Directory.GetDirectories(lCaminho, "*.*", SearchOption.AllDirectories);

                string[] lArquivos;

                string lDiretorioShort;

                string lRaiz = ((PaginaBase)this.Page).RaizDoSite;

                string lUrlDaPasta, lSubDiretorio;

                List<RecursoArquivo> lImagens = new List<RecursoArquivo>();

                foreach (string lDiretorio in lDiretorios)
                {
                    lArquivos = Directory.GetFiles(lDiretorio);

                    lSubDiretorio = lDiretorio.Substring(lDiretorio.IndexOf("\\Resc\\Upload\\") + 13);

                    lUrlDaPasta = string.Format("{0}/Resc/Upload/{1}/", Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToLower().IndexOf(lRaiz.ToLower()) + lRaiz.Length), lSubDiretorio.Replace("\\", "/"));

                    lDiretorioShort = lDiretorio.Substring(lDiretorio.IndexOf("\\Resc\\Upload\\") + 13);

                    foreach (string lArquivo in lArquivos)
                    {
                        lImagens.Add(new RecursoArquivo(lUrlDaPasta, lArquivo, lDiretorioShort));
                    }

                    this.ListaDeDiretoriosDeUpload.Add(lDiretorioShort);
                }

                this.ListaDeArquivos = from i in lImagens orderby i.DataDeCriacao descending select i;
            }

            this.ListaDeDiretoriosDeUpload.Sort();

            rptDiretorios.DataSource = this.ListaDeDiretoriosDeUpload;
            rptDiretorios.DataBind();

            rptImagens.DataSource = this.ListaDeArquivos;
            rptImagens.DataBind();
        }

        private void CarregarTiposDeConteudo()
        {
            TipoConteudoRequest lRequest = new TipoConteudoRequest();

            lRequest.TipoConteudo = new TipoDeConteudoInfo();

            TipoConteudoResponse lResponse = this.PaginaBase.ServicoPersistenciaSite.SelecionarTipoConteudo(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                rptTipoConteudo.DataSource = lResponse.ListaTipoConteudo;
                rptTipoConteudo.DataBind();

                rptTipoConteudo_WidgetLista.DataSource = lResponse.ListaTipoConteudo;
                rptTipoConteudo_WidgetLista.DataBind();

                rptTipoConteudo_WidgetTabela.DataSource = lResponse.ListaTipoConteudo;
                rptTipoConteudo_WidgetTabela.DataBind();

                rptTipoConteudo_WidgetRepetidor.DataSource = lResponse.ListaTipoConteudo;
                rptTipoConteudo_WidgetRepetidor.DataBind();

                rptTipoConteudo_WidgetListaDeDefinicao.DataSource = lResponse.ListaTipoConteudo;
                rptTipoConteudo_WidgetListaDeDefinicao.DataBind();
            }
        }

        private void CarregarPaginas()
        {
            hidListaDePastas.Value = JsonConvert.SerializeObject(PaginaBase.ListaDePaginasComDiretorios);

            rptPaginas.DataSource = PaginaBase.ListaDePaginas;
            rptPaginas.DataBind();

            rptPaginasParaAba.DataSource = PaginaBase.ListaDePaginas;
            rptPaginasParaAba.DataBind();

            rptPaginasParaAcordeon.DataSource = PaginaBase.ListaDePaginas;
            rptPaginasParaAcordeon.DataBind();

            rptEdicaoPagina_Versao.DataSource = PaginaBase.ListaDeVersoes;
            rptEdicaoPagina_Versao.DataBind();

            //as versões disponíveis para visualizar a página são somente as da própria página:

            rptInfoPagina_Versao.DataSource = PaginaBase.ListaDeVersoesDaPagina;
            rptInfoPagina_Versao.DataBind();

            lblInfoPagina_Versao.Text = PaginaBase.Versao;

            if (PaginaBase.Versao == PaginaBase.VersaoPublicada)
            {
                    imgVersaoPublicada.Attributes["style"] = "display:inline";
                btnInfoPagina_Publicar.Attributes["style"] = "display:none";
            }
            else
            {
                    imgVersaoPublicada.Attributes["style"] = "display:none";
                btnInfoPagina_Publicar.Attributes["style"] = "display:inline";
            }
        }

        private bool VerificarEstruturaTipoUsuario(int TipoUsuario)
        {
            bool lRetorno = false;

            if (this.PaginaBase.TipoDeUsuarioLogado != TipoUsuario && this.PaginaBase.TipoDeUsuarioLogado != 1)
                lRetorno = true;

            return lRetorno;
        }

        #endregion
 
        #region Event Handlers
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                btnTipoDeUsuario_Todos.Visible = this.TiposDeUsuarioDisponiveis.Contains(1);
                btnTipoDeUsuario_Visitante.Visible = this.TiposDeUsuarioDisponiveis.Contains(2);
                btnTipoDeUsuario_Cliente.Visible = this.TiposDeUsuarioDisponiveis.Contains(3);

                btnCopiar_Visitante.Visible = this.VerificarEstruturaTipoUsuario(2);
                btnCopiar_Cliente.Visible = this.VerificarEstruturaTipoUsuario(3);

                CarregarListaDeImagens((Request["RecarregarImagens"] == "true"));

                CarregarTiposDeConteudo();

                if (!this.PaginaMaster.EPaginaDoMinhaConta)
                {
                    CarregarPaginas();
                }
            }
        }

        #endregion
    }

    public class RecursoArquivo
    {
        #region Globais

        private string[] gListaDeExtensoesDeImagem = { "bmp", "gif", "jpg", "jpeg", "jpe", "png", "psd", "raw", "tga", "tif" };

        private string[] gListaDeExtensoesDePDF = { "pdf", "pdp" };

        private string[] gListaDeExtensoesDeOffice = { "doc", "docx", "ppt", "pptx", "xls", "xlsx" };

        private string[] gListaDeExtensoesDeZip = { "zip", "rar", "gzip", "7zip" };

        #endregion

        #region Propriedades

        public string NomeDoArquivo { get; set; }

        public string Diretorio { get; set; }

        public string URL { get; set; }

        public string URLDoThumbnail { get; set; }

        public string DataDeCriacao { get; set; }

        public string Extensao { get; set; }

        public string Tipo { get; set; }

        #endregion

        #region Construtor

        public RecursoArquivo() { }

        public RecursoArquivo(string pUrlDaPastaDeImagens, string pCaminhoDoArquivo, string pDiretorio)
        {
            this.NomeDoArquivo = Path.GetFileName(pCaminhoDoArquivo);

            this.Diretorio = pDiretorio;

            this.URL = pUrlDaPastaDeImagens + this.NomeDoArquivo;

            string lExtensao = Path.GetExtension(this.NomeDoArquivo).TrimStart('.').ToLower();

            string lUrlDaPastaDeSkin = pUrlDaPastaDeImagens.Substring(0, pUrlDaPastaDeImagens.IndexOf("Upload"));

            lUrlDaPastaDeSkin += "Skin/Default/Img/";

            this.Extensao = lExtensao;

            this.Tipo = "outros";

            if(gListaDeExtensoesDeImagem.Contains(lExtensao))
            {
                this.URLDoThumbnail = this.URL;

                this.Tipo = "imagem";
            }
            else if(gListaDeExtensoesDePDF.Contains(lExtensao))
            {
                this.URLDoThumbnail = lUrlDaPastaDeSkin + "Imagem-TipoDeArquivo-PDF.jpg";

                this.Tipo = "pdf";
            }
            else if(gListaDeExtensoesDeOffice.Contains(lExtensao))
            {
                if(lExtensao == "xls" || lExtensao == "xlsx")
                {
                    this.URLDoThumbnail = lUrlDaPastaDeSkin + "Imagem-TipoDeArquivo-XLS.jpg";

                    this.Tipo = "planilha";
                }
                else if(lExtensao == "ppt" || lExtensao == "pptx")
                {
                    this.URLDoThumbnail = lUrlDaPastaDeSkin + "Imagem-TipoDeArquivo-PPT.jpg";
                    
                    this.Tipo = "apresentacao";
                }
                else
                {
                    this.URLDoThumbnail = lUrlDaPastaDeSkin + "Imagem-TipoDeArquivo-DOC.jpg";

                    this.Tipo = "documento";
                }
            }
            else if (gListaDeExtensoesDeZip.Contains(lExtensao))
            {
                this.URLDoThumbnail = lUrlDaPastaDeSkin + "Imagem-TipoDeArquivo-ZIP.jpg";

                this.Tipo = "zip";
            }
            else
            {
                this.URLDoThumbnail = lUrlDaPastaDeSkin + "Imagem-TipoDeArquivo-Outro.jpg";
            }

            this.DataDeCriacao = File.GetCreationTime(pCaminhoDoArquivo).ToString("yyyy-MM-dd-HH-mm-ss");
        }

        #endregion
    }
}
