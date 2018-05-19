using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Newtonsoft.Json;
using Gradual.Intranet.Www.App_Codigo;

namespace Gradual.Intranet.Www.Intranet.Sistema
{
    [ValidarSegurancaAttribute("A4E18E5F-99E0-4182-9945-6F270D61E329", "1", "1")]
    public partial class Autorizacoes : PaginaBaseAutenticada
    {
        #region Métodos Private
        
        private string ResponderCarregarHtmlComDados()
        {
            ConsultarEntidadeCadastroRequest<ClienteAutorizacaoInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteAutorizacaoInfo>();
            ConsultarEntidadeCadastroResponse<ClienteAutorizacaoInfo> lResponse;

            Logger.ErrorFormat("Autorizacoes.aspx: ResponderCarregarHtmlComDados");

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteAutorizacaoInfo>(lRequest);

            bool lPodeD1, lPodeD2, lPodeP1, lPodeP2, lPodeT1, lPodeT2;

            lPodeD1 = UsuarioLogado.VerificarPermissaoSimples("CA541A7E-A5AA-4F93-BE90-F63C2807E9D5");
            lPodeD2 = UsuarioLogado.VerificarPermissaoSimples("52B3C383-94CD-499A-A13C-8184C96937ED");

            lPodeP1 = UsuarioLogado.VerificarPermissaoSimples("9AE9DCFF-5081-439F-AD1A-A6942E4B6906");
            lPodeP2 = UsuarioLogado.VerificarPermissaoSimples("D52E4828-8989-419C-ABB2-06DF12B025DE");

            lPodeT1 = UsuarioLogado.VerificarPermissaoSimples("1E290142-6F81-4C76-B9A1-5349DF6161E4");
            lPodeT2 = UsuarioLogado.VerificarPermissaoSimples("DC78D2C4-E8A6-489F-A5D0-603493F417F4");
            
            Logger.ErrorFormat("Autorizacoes.aspx: ResponderCarregarHtmlComDados -> B");

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                IEnumerable<TransporteClienteAutorizacaoCadastral> lLista = from ClienteAutorizacaoInfo t
                                                                              in lResponse.Resultado
                                                                            select new TransporteClienteAutorizacaoCadastral(t, lPodeD1, lPodeD2, lPodeP1, lPodeP2, lPodeT1, lPodeT2);

                rptListaDeAutorizacoesPendentes.DataSource = lLista;
                rptListaDeAutorizacoesPendentes.DataBind();
                rowLinhaDeNenhumItem.Visible = (lLista.Count().Equals(0));
            }
            else
            {
                string lMsg = string.Format("Resposta com erro do serviço em Autorizacoes.aspx ResponderCarregarHtmlComDados: [{0}][{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);

                Logger.Error(lMsg);

                throw new Exception(lMsg);
            }

            return string.Empty;    //só para obedecer assinatura
        }

        public string ResponderAutorizarCadastros()
        {
            string lRetorno;

            try
            {
                string[] lCadastros = Request["Codigos"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                string[] lCodigo;

                List<string> lRetornos = new List<string>();

                SalvarEntidadeCadastroRequest<ClienteAutorizacaoInfo> lRequest;
                SalvarEntidadeCadastroResponse lResponse;

                for (int a = 0; a < lCadastros.Length; a++)
                {
                    lCodigo = lCadastros[a].Split('_');

                    lRequest = new SalvarEntidadeCadastroRequest<ClienteAutorizacaoInfo>();

                    lRequest.EntidadeCadastro = new ClienteAutorizacaoInfo();

                    /*
                     atenção no mapeamento sem vergonha:
                         * 
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@tipo", DbType.Int16, pParametros.Objeto.IdLogin_D1);  //sempre esse campo para o tipo
                    lAcessaDados.AddInParameter(lDbCommand, "@numero", DbType.Int16, pParametros.Objeto.IdLogin_D2);  //sempre esse campo para o numero
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login_autorizador", DbType.Int32, pParametros.Objeto.IdLogin_P1);  //sempre esse campo para o login auth
                     */

                    lRequest.EntidadeCadastro.IdCliente = Convert.ToInt32(lCodigo[0]);
                    lRequest.EntidadeCadastro.IdLogin_D1 = Convert.ToInt16(lCodigo[1][0].ToString());
                    lRequest.EntidadeCadastro.IdLogin_D2 = Convert.ToInt16(lCodigo[1][1].ToString());
                    lRequest.EntidadeCadastro.IdLogin_P1 = UsuarioLogado.Id;

                    lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteAutorizacaoInfo>(lRequest);

                    //lResponse = new SalvarEntidadeCadastroResponse();
                    //lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                    //lResponse.Objeto = new ClienteAutorizacaoInfo();

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        string lData = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                        if (((ClienteAutorizacaoInfo)lResponse.Objeto).StAutorizado == "S")
                        {
                            lData = "S";
                        }

                        lRetornos.Add(string.Format("{0}_{1}", lRequest.EntidadeCadastro.IdCliente, lData));
                    }
                    else
                    {
                        lRetornos.Add(string.Format("{0}_E", lRequest.EntidadeCadastro.IdCliente));

                        Logger.ErrorFormat("Erro durante a autorização de cadastro para [{0}]: [{1}] [{2}] [{3}]"
                                            , lRequest.EntidadeCadastro.IdCliente
                                            , JsonConvert.SerializeObject(lRequest.EntidadeCadastro)
                                            , lResponse.StatusResposta
                                            , lResponse.DescricaoResposta);
                    }
                }

                lRetorno = RetornarSucessoAjax(lRetornos, "ok");

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao autorizar cadastros", ex);
            }

            return lRetorno;
        }

        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (base.UsuarioLogado != null && base.UsuarioLogado.VerificarPermissaoSimples("A4E18E5F-99E0-4182-9945-6F270D61E329"))
                {
                    if (string.IsNullOrEmpty(Request["acao"]))
                    {
                        ResponderCarregarHtmlComDados();
                    }
                    else
                    {
                        base.RegistrarRespostasAjax(new string[] { "AutorizarCadastros"
                                                                    },
                                    new ResponderAcaoAjaxDelegate[] { ResponderAutorizarCadastros
                                                                    });
                    }

                }
                else
                {
                    try
                    {
                    
                    Logger.InfoFormat("Usuario: [{0}], Verifica: [{1}]",
                                        base.UsuarioLogado.Id
                                        , base.UsuarioLogado.VerificarPermissaoSimples("A4E18E5F-99E0-4182-9945-6F270D61E329"));
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Erro em Autorizções.aspx: [{0}]\r\n[{1}]", ex.Message, ex.StackTrace);
            }

            //if (!Page.IsPostBack)
            //    this.ResponderCarregarHtmlComDados();
        }

        #endregion

    }
}