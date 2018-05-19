using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.Web.App_Codigo.Transporte;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.FIDC.Adm.Web.CadastroFundos
{
    public partial class DireitosCreditorios : PaginaBase
    {
        #region Propriedades
        public string GetNomeFundo
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Request["NomeFundo"]))
                {
                    return this.Request["NomeFundo"].ToString();
                }

                return String.Empty;
            }
        }
        public string GetCNPJFundo
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Request["CNPJFundo"]))
                {
                    return this.Request["CNPJFundo"].ToString();
                }

                return String.Empty;
            }
        }
        public string GetNomeAdministrador
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Request["NomeAdministrador"]))
                {
                    return this.Request["NomeAdministrador"].ToString();
                }

                return String.Empty;
            }
        }
        public string GetCNPJAdministrador
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Request["CNPJAdministrador"]))
                {
                    return this.Request["CNPJAdministrador"].ToString();
                }

                return String.Empty;
            }
        }
        public string GetNomeCustodiante
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Request["NomeCustodiante"]))
                {
                    return this.Request["NomeCustodiante"].ToString();
                }

                return String.Empty;
            }
        }
        public string GetCNPJCustodiante
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Request["CNPJCustodiante"]))
                {
                    return this.Request["CNPJCustodiante"].ToString();
                }

                return String.Empty;
            }
        }
        public string GetNomeGestor
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Request["NomeGestor"]))
                {
                    return this.Request["NomeGestor"].ToString();
                }

                return String.Empty;
            }
        }
        public string GetCNPJGestor
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Request["CNPJGestor"]))
                {
                    return this.Request["CNPJGestor"].ToString();
                }

                return String.Empty;
            }
        }
        public int GetIdFundoCadastro
        {
            get
            {
                try
                {
                    return Convert.ToInt32(this.Request["IdFundoCadastro"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public bool GetIsAtivo
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Request["InativarFundo"]))
                {
                    return !(this.Request["InativarFundo"].ToString().ToUpper() == "TRUE");
                }

                return true;
            }
        }
        #endregion

        #region Eventos
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    base.Page_Load(sender, e);

                    base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados",
                                                            "AtualizarValor",
                                                            "EditarFundo"
                                                     },
                         new ResponderAcaoAjaxDelegate[] { 
                                                        this.ResponderCarregarHtmlComDados,
                                                        this.ResponderAtualizarValor,
                                                        this.ResponderEditarFundo
                                                     });

                    this.CarregarDadosIniciais();
                }
                catch (Exception ex)
                {
                    gLogger.Error("Erro ao carregar os dados de fundos na tela", ex);
                }
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Carregar dados iniciais da página de carteiras
        /// </summary>
        private string CarregarDadosIniciais()
        {
            string lRetorno = string.Empty;

            try
            {
                if (!Page.IsPostBack)
                {
                    base.TituloDaPagina = "Direitos Creditórios";
                    base.LinkPreSelecionado = "lnkTL_CadastroFundos";
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax(ex.Message, ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Atualiza ou insere valores de fundos
        /// </summary>
        /// <returns>Retorna string com a lista em Json</returns>
        public string ResponderAtualizarValor()
        {
            string lRetorno = string.Empty;
            
            try
            {
                var lRequest = new CadastroFundoRequest();

                #region Preenchimento objeto de Request

                lRequest.NomeFundo = this.GetNomeFundo;
                lRequest.CNPJFundo = this.GetCNPJFundo.Replace("/", "").Replace(".", "").Replace("-", "");
                lRequest.NomeAdministrador = this.GetNomeAdministrador;
                lRequest.CNPJAdministrador = this.GetCNPJAdministrador.Replace("/", "").Replace(".", "").Replace("-", "");
                lRequest.NomeCustodiante = this.GetNomeCustodiante;
                lRequest.CNPJCustodiante = this.GetCNPJCustodiante.Replace("/", "").Replace(".", "").Replace("-", "");
                lRequest.NomeGestor = this.GetNomeGestor;
                lRequest.CNPJGestor = this.GetCNPJGestor.Replace("/", "").Replace(".", "").Replace("-", "");
                lRequest.IdFundoCadastro = this.GetIdFundoCadastro;
                lRequest.IsAtivo = this.GetIsAtivo;

                lRequest.DescricaoUsuarioLogado = this.UsuarioLogado.Nome;

                #endregion

                #region Gravação Log4Net
                string mensagemLog = string.Empty;

                mensagemLog += "NomeFundo:" + lRequest.NomeFundo + ";";
                mensagemLog += "CNPJFundo:" + lRequest.CNPJFundo + ";";
                mensagemLog += "NomeAdministrador:" + lRequest.NomeAdministrador + ";";
                mensagemLog += "CNPJAdministrador:" + lRequest.CNPJAdministrador + ";";
                mensagemLog += "NomeCustodiante:" + lRequest.NomeCustodiante + ";";
                mensagemLog += "CNPJCustodiante:" + lRequest.CNPJCustodiante + ";";
                mensagemLog += "NomeGestor:" + lRequest.NomeGestor + ";";
                mensagemLog += "CNPJGestor:" + lRequest.CNPJGestor + ";";                
                mensagemLog += "IsAtivo:" + lRequest.IsAtivo + ";";
                mensagemLog += "TipoTransacao:" + (lRequest.IdFundoCadastro > 0 ? "UPDATE" : "INSERT") + ";";
                mensagemLog += "UsuarioTransacao:" + lRequest.DescricaoUsuarioLogado + ";";
                
                gLogger.Info(mensagemLog);
                #endregion

                CadastroFundoResponse lResponse = base.AtualizarCadastroFundo(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = base.RetornarSucessoAjax(lResponse.StatusResposta.ToString());

                    return lRetorno;
                }
                else if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.ErroNegocio)
                {
                    //erro de negócio
                    lRetorno = base.RetornarErroAjax(lResponse.DescricaoResposta);
                }
                else
                {
                    lRetorno = base.RetornarErroAjax("Erro ao cadastrar fundo.");
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro ao cadastrar fundo", ex);

                lRetorno = base.RetornarErroAjax("Erro no método ResponderAtualizarValor ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Carrega dados no grid de fundos
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarHtmlComDados()
        {
            string lRetorno = string.Empty;
                        
            try
            {
                var lRequest = new CadastroFundoRequest();
                
                CadastroFundoResponse lResponse = base.BuscarFundosCadastrados(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteCadastroFundos().TraduzirLista(lResponse.ListaFundos);

                    TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada(lListaTransporte);

                    lRetornoLista.TotalDeItens = lResponse.ListaFundos.Count;

                    lRetornoLista.PaginaAtual = 1;

                    lRetornoLista.TotalDePaginas = 0;

                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro ao carregar os dados de fundos na tela", ex);

                lRetorno = base.RetornarErroAjax("Erro no método ResponderCarregarHtmlComDados ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Carrega dados do fundo selecionado na tela
        /// </summary>
        /// <returns></returns>
        public string ResponderEditarFundo()
        {
            string lRetorno = string.Empty;

            try
            {
                CadastroFundoRequest lRequest = new CadastroFundoRequest();

                //Preenchimento objeto Request
                lRequest.IdFundoCadastro = this.GetIdFundoCadastro;

                CadastroFundoResponse lResponse = base.BuscarFundosCadastrados(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteCadastroFundos().TraduzirLista(lResponse.ListaFundos);

                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaFundos);

                    return lRetorno;
                }

            }
            catch (Exception ex)
            {
                gLogger.Error("Erro ao editar fundo. ", ex);

                lRetorno = base.RetornarErroAjax("Erro no método ResponderEditarFundo ", ex);
            }

            return lRetorno;
        }
        #endregion
    }
}