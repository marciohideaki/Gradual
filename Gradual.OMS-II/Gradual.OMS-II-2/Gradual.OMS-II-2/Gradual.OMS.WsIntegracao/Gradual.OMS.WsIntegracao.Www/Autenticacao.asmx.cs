using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;
using log4net;

namespace Gradual.OMS.WsIntegracao
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Autenticacao : System.Web.Services.WebService
    {
        #region Propriedades

        private static ILog _Logger = null;

        private static ILog Logger
        {
            get
            {
                if (_Logger == null)
                {
                    log4net.Config.XmlConfigurator.Configure();

                    _Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                }

                return _Logger;
            }
        }

        public Dictionary<string, DateTime> CodigosDeSessao
        {
            get
            {
                if (Application["CodigosDeSessao"] == null)
                    Application["CodigosDeSessao"] = new Dictionary<string, DateTime>();

                return (Dictionary<string, DateTime>)Application["CodigosDeSessao"];
            }
        }
        
        public Dictionary<string, DateTime> TokensDeSessao
        {
            get
            {
                if (Application["TokensDeSessao"] == null)
                    Application["TokensDeSessao"] = new Dictionary<string, DateTime>();

                return (Dictionary<string, DateTime>)Application["TokensDeSessao"];
            }
        }

        #endregion

        #region Métodos Públicos

        [WebMethod]
        public AutenticarUsuarioResponse AutenticarUsuario(AutenticarUsuarioRequest pRequest)
        {
            IServicoSeguranca lServicoSeguranca;
            AutenticarUsuarioResponse lResposta = new AutenticarUsuarioResponse();
            //ReceberSessaoRequest lRequestSessao;
            //ReceberSessaoResponse lResponseSessao;

            try
            {
                Gradual.OMS.Seguranca.Lib.AutenticarUsuarioRequest lRequest = new Gradual.OMS.Seguranca.Lib.AutenticarUsuarioRequest();
                Gradual.OMS.Seguranca.Lib.AutenticarUsuarioResponse lResponse;

                ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");

                lServicoSeguranca = Ativador.Get<IServicoSeguranca>();

                lRequest.Email = pRequest.CodigoOuEmailDoUsuario;
                lRequest.Senha = Gradual.OMS.Seguranca.Lib.Criptografia.CalculateMD5Hash(pRequest.Senha);
                lRequest.IP = HttpContext.Current.Request.UserHostAddress;

                lResponse = lServicoSeguranca.AutenticarUsuario(lRequest);

                if (lResponse.StatusResposta == Library.MensagemResponseStatusEnum.OK)
                {
                    lResposta.StatusResposta = lResponse.StatusResposta.ToString();
                    lResposta.DescricaoResposta = lResponse.DescricaoResposta;

                    lResposta.CodigoDaSessao = lResponse.Sessao.CodigoSessao;
                    lResposta.CodigoDeAcessoDoUsuario = Convert.ToString(lResponse.CodigoAcessoSistema);
                    lResposta.IdLogin = Convert.ToString(lResponse.Sessao.CodigoUsuario);

                    this.CodigosDeSessao.Add(lResposta.CodigoDaSessao, DateTime.Now);

                    if (!string.IsNullOrEmpty(pRequest.Token))
                    {
                        // o token é o nome da máquina com três números randômicos no final; vamos guardar somente o nome da máquina, então:

                        string lTokenDecript = Criptografia.Descriptografar(pRequest.Token, true);

                        lTokenDecript = lTokenDecript.Substring(0, lTokenDecript.Length - 3);

                        if (!this.TokensDeSessao.ContainsKey(lTokenDecript))
                        {
                            this.TokensDeSessao.Add(lTokenDecript, DateTime.Now);
                        }
                        else
                        {
                            this.TokensDeSessao[lTokenDecript] = DateTime.Now;
                        }
                    }

                    Logger.InfoFormat("Resposta OK para login de Código de Acesso [{0}] Sessão [{1}] ", lResposta.CodigoDeAcessoDoUsuario, lResposta.CodigoDaSessao);

                    /*
                    lRequestSessao = new ReceberSessaoRequest();

                    lRequestSessao.CodigoSessao = lResponse.Sessao.CodigoSessao;
                    lRequestSessao.CodigoSessaoARetornar = lResponse.Sessao.CodigoSessao;

                    lServicoSeguranca = Ativador.Get<IServicoSeguranca>();

                    lResponseSessao = lServicoSeguranca.ReceberSessao(lRequestSessao);

                    if (lResponseSessao.StatusResposta == Library.MensagemResponseStatusEnum.OK)
                    {
                        lResposta.CodigoCblcDoUsuario = lResponseSessao.Usuario.Complementos.ReceberItem<ContextoOMSInfo>().CodigoCBLC;
                        lResposta.CodigoBmfDoUsuario  = lResponseSessao.Usuario.Complementos.ReceberItem<ContextoOMSInfo>().CodigoBMF;
                        
                        Logger.InfoFormat("Resposta OK para login de CBLC [{0}] BMF [{1}] Sessão [{2}]", lResposta.CodigoCblcDoUsuario, lResposta.CodigoBmfDoUsuario, lResposta.CodigoDaSessao);
                    }
                    else
                    {
                        lResposta.StatusResposta = "Erro CBLC";
                        lResposta.DescricaoResposta = "Erro ao receber código CBLC: " + lResponseSessao.DescricaoResposta;

                        Logger.Error(lResposta.DescricaoResposta);
                    }*/
                }
                else
                {
                    lResposta.StatusResposta = "Erro Segurança";
                    lResposta.DescricaoResposta = "Erro ao autenticar usuário: " + lResponse.DescricaoResposta;

                    Logger.Error(lResposta.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lResposta.DescricaoResposta = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);

                lResposta.StatusResposta = "Erro";

                Logger.Error(lResposta.DescricaoResposta);
            }

            return lResposta;
        }

        [WebMethod]
        public VerificarAutenticacaoResponse VerificarAutenticacao(VerificarAutenticacaoRequest pRequest)
        {
            VerificarAutenticacaoResponse lResponse = new VerificarAutenticacaoResponse();

            lResponse.StatusResposta = "OK";
            lResponse.AutenticacaoVerificada = false;

            // o token é o nome da máquina com três números randômicos no final; vamos guardar somente o nome da máquina, então:

            string lTokenDecript = Criptografia.Descriptografar(pRequest.TokenSessao, true);

            lTokenDecript = lTokenDecript.Substring(0, lTokenDecript.Length - 3);

            if (this.TokensDeSessao.ContainsKey(lTokenDecript))
            {
                if (new TimeSpan(DateTime.Now.Ticks - this.TokensDeSessao[lTokenDecript].Ticks).TotalMinutes <= 10)
                {
                    lResponse.AutenticacaoVerificada = true;
                }
                else
                {
                    this.CodigosDeSessao.Remove(lTokenDecript);
                }
            }

            return lResponse;
        }
        
        [WebMethod]
        public BuscarPermissoesDoUsuarioResponse BuscarPermissoesDoUsuario(BuscarPermissoesDoUsuarioRequest pRequest)
        {
            BuscarPermissoesDoUsuarioResponse lReturn = new BuscarPermissoesDoUsuarioResponse();

            ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");

            IServicoSeguranca lServico = Ativador.Get<IServicoSeguranca>();

            ReceberUsuarioRequest lRequest = new ReceberUsuarioRequest();
            ReceberUsuarioResponse lResponse;

            try
            {
                lRequest.CodigoUsuario = pRequest.CodigoDoUsuario;
                lRequest.CodigoSessao = pRequest.CodigoDaSessao;

                Logger.InfoFormat("BuscarPermissoesDoUsuario(CodigoSessao [{0}], IdLogin [{1}])", lRequest.CodigoSessao, lRequest.CodigoUsuario);

                lResponse = lServico.ReceberUsuario(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Usuario != null)
                    {
                        Logger.InfoFormat("Retorno OK do serviço: [{0}] permissões para [{1}])", lResponse.Usuario.Permissoes.Count, lRequest.CodigoUsuario);

                        lReturn.StatusResposta = "OK";

                        lReturn.Permissoes = lResponse.Usuario.Permissoes;
                    }
                    else
                    {
                        throw new Exception("Serviço não identificou o usuário nessa sessão");
                    }
                }
                else
                {
                    Logger.ErrorFormat("Retorno com erro do ServicoSeguranca em BuscarPermissoesDoUsuario(CodigoSessao [{0}], IdLogin [{1}]) [{2}] \r\n{3}"
                                        , lRequest.CodigoSessao
                                        , lRequest.CodigoUsuario
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);

                    lReturn.StatusResposta = "Erro Serviço";

                    lReturn.DescricaoResposta = lResponse.DescricaoResposta;
                }
            }
            catch (Exception ex)
            {
                string lMensagem = string.Format("Exception em BuscarPermissoesDoUsuario: [{0}] \r\n{1}", ex.Message, ex.StackTrace);

                Logger.ErrorFormat(lMensagem);

                lReturn.StatusResposta = "Exception";
                lReturn.DescricaoResposta = lMensagem;
            }

            return lReturn;
        }

        #endregion
    }
}
