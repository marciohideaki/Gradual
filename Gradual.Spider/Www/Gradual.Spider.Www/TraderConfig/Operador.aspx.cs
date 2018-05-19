using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Spider.Www.App_Codigo.Transporte;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;
using Gradual.OMS.Library;
using Gradual.Spider.Lib;
using Gradual.Spider.Lib.Dados;

namespace Gradual.Spider.Www
{
    public partial class CadastroOperador : PaginaBase
    {
        #region Propriedades
        public static Dictionary<int, string> gListaSessao = null;
        #endregion

        #region Eventos
        
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] 
                                            { "Cadastrar"
                                            , "Excluir"
                                            , "Atualizar"
                                            , "Selecionar"
                                            , "Buscar"
                                            },
            new ResponderAcaoAjaxDelegate[] { this.ResponderCadastrar
                                            , this.ResponderExcluir 
                                            , this.ResponderAtualizar
                                            , this.ResponderSelecionar
                                            , this.ResponderBuscar
                                            });
            if (!this.IsPostBack)
            {
                this.CarregarCombos();
            }
        }

        #endregion

        #region Métodos
        
        public void CarregarCombos()
        {
            this.rptAssessorSinacor.DataSource = base.ListarSinacor(new Lib.Dados.SinacorListaInfo() { Informacao = eInformacao.Assessor });
            this.rptAssessorSinacor.DataBind();

            Dictionary<int,string> lListaSessaoFix =  base.ListarSessaoFix();
            Dictionary<int,string> lListaLocalidade =  base.ListarLocalidadeAssessor();

            gListaSessao = lListaSessaoFix;

            this.rptSessaoAssessor.DataSource   = lListaSessaoFix;
            this.rptSessaoAssessor.DataBind();

            this.rptAssessorFiltro.DataSource   = lListaSessaoFix;
            this.rptAssessorFiltro.DataBind();

            this.rptLocalidadeFiltro.DataSource = lListaLocalidade;
            this.rptLocalidadeFiltro.DataBind();

            this.rptLocalidade.DataSource = lListaLocalidade;
            this.rptLocalidade.DataBind();

            var lRequest = new AssessorInfo();

            var lListaAssessor = base.ListarAssessorComplemento(lRequest);

            Transporte_Operadores lLista = new Transporte_Operadores().TraduzirLista(lListaAssessor, lListaSessaoFix);

            var lOperador =  JsonConvert.SerializeObject(lLista);

            string lScript = "Operador_PreecheGrid("+ lOperador +")";

            base.RodarJavascriptOnLoad(lScript);
        }

        public string ResponderBuscar()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = Request["ObjetoJson"];

            var lDados = JsonConvert.DeserializeObject<Transporte_Operadores>(lObjetoJson);

            var lRequest = new AssessorInfo();

            lRequest.NomeAssessor     = lDados.Nome;
            lRequest.CodigoSessao     = lDados.Sessao;
            lRequest.CodigoLocalidade = lDados.CodigoLocalidade.DBToInt32();
            lRequest.CodigoSigla      = lDados.Sigla.ToString();

            var lListaAssessor = base.ListarAssessorComplemento(lRequest);

            Transporte_Operadores lLista = new Transporte_Operadores().TraduzirLista(lListaAssessor, gListaSessao);

            lRetorno = RetornarSucessoAjax(lLista, "Retorno com sucesso");

            return lRetorno;
        }

        public string ResponderSelecionar()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = Request["ObjetoJson"];

            Transporte_Operadores lDadosUsuario = null;

            MensagemResponseBase lResponse = null;
            MensagemRequestBase lRequest = null;

            UsuarioInfo lUsuarioInfo = new UsuarioInfo();

            try
            {
                lDadosUsuario = JsonConvert.DeserializeObject<Transporte_Operadores>(lObjetoJson);

                lUsuarioInfo = lDadosUsuario.ToUsuarioInfo();

                lRequest = new ReceberUsuarioRequest()
                    {
                        CodigoUsuario = lDadosUsuario.Id,
                        CodigoSessao = this.CodigoSessao
                    };

                //lRequest.Usuario.Perfis.Add("6"); //-->> Perfil Assessor

                lResponse = ServicoSeguranca.ReceberUsuario((ReceberUsuarioRequest)lRequest);

                List<AssessorInfo> lAssessor = base.ListarAssessorComplemento(new AssessorInfo() { IdLoginAssessor = lDadosUsuario.Id.DBToInt32() });

                AssessorInfo lAssesorComplemento = null;

                if (lAssessor != null && lAssessor.Count > 0)
                {
                    lAssesorComplemento = lAssessor[0];
                }

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    var lTransporte = new Transporte_Operadores(((ReceberUsuarioResponse)lResponse).Usuario, lAssesorComplemento);

                    lRetorno = RetornarSucessoAjax(lTransporte, "Usuário encontrado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro durante o envio do pedido para selecionar um usuário", ex);
            }

            return lRetorno;
        }

        public string ResponderCadastrar()
        {
            string lretorno = string.Empty;
            string lObjetoJson = Request["ObjetoJson"];

            Transporte_Operadores lDadosUsuario = null;

            SalvarUsuarioResponse lResponse = null;
            SalvarUsuarioRequest lRequest = new SalvarUsuarioRequest();

            UsuarioInfo lUsuarioInfo = new UsuarioInfo();

            try
            {
                lDadosUsuario = JsonConvert.DeserializeObject<Transporte_Operadores>(lObjetoJson);

                lUsuarioInfo = lDadosUsuario.ToUsuarioInfo();

                lUsuarioInfo.CodigoUsuario = "0";

                lUsuarioInfo.Senha = base.CalculateMD5Hash("gradual123*");

                lRequest.Usuario = lUsuarioInfo;

                lRequest.CodigoSessao = this.CodigoSessao;

                lRequest.Usuario.Perfis = new List<string>();

                lRequest.Usuario.Perfis.Add("6"); //-->> Perfil Assessor

                lResponse = ServicoSeguranca.SalvarUsuario(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    ReceberUsuarioResponse lResponseUsuario = ServicoSeguranca.ReceberUsuario(
                    new ReceberUsuarioRequest()
                    {
                        CodigoSessao = this.CodigoSessao,
                        Email = lUsuarioInfo.Email
                    });

                    var lAssessor = new AssessorInfo();

                    lAssessor.IdLoginAssessor       = lResponseUsuario.Usuario.CodigoUsuario.DBToInt32();
                    lAssessor.Email                 = lResponseUsuario.Usuario.Email;
                    lAssessor.CodigoLocalidade      = lDadosUsuario.CodigoLocalidade.DBToInt32();
                    lAssessor.CodigoSessao          = lDadosUsuario.Sessao;
                    lAssessor.CodigoSigla           = lDadosUsuario.Sigla;
                    lAssessor.CodigoOperador        = lDadosUsuario.CodigoOperador;
                    lAssessor.ListaAssessoresFilhos = lDadosUsuario.CodAssessorAssociado;
                    lAssessor.NomeAssessor          = lDadosUsuario.Nome;
                    lAssessor.StAtivo               = Convert.ToBoolean(lDadosUsuario.AcessaPlataforma);

                    base.InserirAtualizarAssessorComplemento(lAssessor);

                    if (lResponseUsuario.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        //lretorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lREsponseUsuario.Usuario.CodigoUsuario), "Usuário cadastrado com sucesso");
                        base.RegistrarLogInclusao(string.Concat("Usuário incluído: ", lDadosUsuario.Email));
                    }
                    else
                    {
                        lretorno = RetornarErroAjax(lResponseUsuario.DescricaoResposta);
                    }
                }
                else
                {
                    lretorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lretorno = RetornarErroAjax("Erro durante o envio do pedido para cadastrar usuário", ex);
            }
            return lretorno;
        }

        public string ResponderAtualizar()
        {
            string lretorno = string.Empty;
            
            string lObjetoJson = this.Request.Form["ObjetoJson"];

            Transporte_Operadores lDadosUsuario = null;

            SalvarUsuarioResponse lResponseSalvar = null;

            SalvarUsuarioRequest lRequestSalvar = new SalvarUsuarioRequest()
            {
                CodigoSessao = this.CodigoSessao
            };

            UsuarioInfo lUsuarioInfo = null;

            try
            {
                lDadosUsuario = JsonConvert.DeserializeObject<Transporte_Operadores>(lObjetoJson);

                ReceberUsuarioResponse lResponseReceber = ServicoSeguranca.ReceberUsuario(
                    new ReceberUsuarioRequest()
                    {
                        CodigoSessao  = this.CodigoSessao,
                        CodigoUsuario = lDadosUsuario.Id
                    });

                if (lResponseReceber.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lUsuarioInfo       = lResponseReceber.Usuario;
                    lUsuarioInfo.Nome  = lDadosUsuario.Nome;
                    lUsuarioInfo.Email = lDadosUsuario.Email;

                    int lTipoAcesso = 0;

                    if (int.TryParse(lDadosUsuario.TipoAcesso, out lTipoAcesso))
                    {
                        lUsuarioInfo.CodigoTipoAcesso = lTipoAcesso;
                    }

                    if ((eTipoAcesso)lTipoAcesso == eTipoAcesso.Assessor)
                    {
                        int lCodAssessor = -1;

                        if (int.TryParse(lDadosUsuario.CodAssessor, out lCodAssessor))
                        {
                            lUsuarioInfo.CodigoAssessor       = lCodAssessor;
                            lUsuarioInfo.CodigosFilhoAssessor = lDadosUsuario.CodAssessorAssociado;
                        }
                        else
                        {
                            throw new Exception("Codigo de assessor inválido.");
                        }
                    }

                    lRequestSalvar.Usuario = lUsuarioInfo;

                    lRequestSalvar.Usuario.Perfis = new List<string>();

                    lRequestSalvar.Usuario.Perfis.Add("6"); //-->> Perfil Assessor

                    lResponseSalvar = ServicoSeguranca.SalvarUsuario(lRequestSalvar);

                    if (lResponseSalvar.StatusResposta == MensagemResponseStatusEnum.OK)
                    {

                        var lAssessor = new AssessorInfo();

                        lAssessor.IdLoginAssessor       = lDadosUsuario.Id.DBToInt32();
                        lAssessor.Email                 = lDadosUsuario.Email;
                        lAssessor.CodigoLocalidade      = lDadosUsuario.CodigoLocalidade.DBToInt32();
                        lAssessor.CodigoSessao          = lDadosUsuario.Sessao;
                        lAssessor.CodigoSigla           = lDadosUsuario.Sigla;
                        lAssessor.CodigoOperador        = lDadosUsuario.CodigoOperador;
                        lAssessor.ListaAssessoresFilhos = lDadosUsuario.CodAssessorAssociado;
                        lAssessor.NomeAssessor          = lDadosUsuario.Nome;
                        lAssessor.StAtivo               = Convert.ToBoolean(lDadosUsuario.AcessaPlataforma);

                        base.InserirAtualizarAssessorComplemento(lAssessor);


                        lretorno = RetornarSucessoAjax("Operador alterado com sucesso");

                        //base.RegistrarLogAlteracao(string.Concat("Usuário alterado: ", lDadosUsuario.Email));

                    }
                    else
                    {
                        lretorno = RetornarErroAjax(lResponseSalvar.DescricaoResposta);
                    }
                }
                else
                {
                    lretorno = RetornarErroAjax(lResponseReceber.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lretorno = RetornarErroAjax("Erro durante o envio do request para cadastrar usuário", ex);
            }

            return lretorno;
        }

        public string ResponderExcluir()
        {
            return string.Empty;
        }
        
        #endregion

    }
}