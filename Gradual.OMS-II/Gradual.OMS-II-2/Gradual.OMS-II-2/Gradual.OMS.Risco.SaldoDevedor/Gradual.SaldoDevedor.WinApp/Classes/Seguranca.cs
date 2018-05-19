using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.SaldoDevedor.WinApp.Classes
{
    public class Seguranca
    {
        #region Globais

        private Gradual.OMS.Seguranca.Lib.IServicoSeguranca gServicoseguranca;

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Construtores

        public Seguranca()
        {
            gServicoseguranca = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.Seguranca.Lib.IServicoSeguranca>();
        }

        #endregion

        #region Métodos Públicos

        public Gradual.OMS.Seguranca.Lib.AutenticarUsuarioResponse AutenticarUsuario(string pUsuario, string pSenha, string pMachineNameOrIP, string pSistema)
        {
            //bool lRetorno = false;

            Gradual.OMS.Seguranca.Lib.AutenticarUsuarioRequest lRequest = new Gradual.OMS.Seguranca.Lib.AutenticarUsuarioRequest();
            Gradual.OMS.Seguranca.Lib.AutenticarUsuarioResponse lResponse;

            lRequest.Email = pUsuario;
            lRequest.Senha = Gradual.OMS.Seguranca.Lib.Criptografia.CalculateMD5Hash(pSenha);
            lRequest.IP = pMachineNameOrIP;

            lRequest.CodigoSistemaCliente = pSistema;

            lResponse = this.gServicoseguranca.AutenticarUsuario(lRequest);

            try
            {
                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(IniciarContexto), lResponse.Sessao.CodigoSessao);

                    //this.IniciarContexto(lResponse.Sessao.CodigoSessao);

                    //lRetorno = true;

                }
                else
                {
                    logger.Warn(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }

            return lResponse;
        }

        public bool VerificarPermissoes(System.String CodigoDeAcessoDoUsuario, String CodigoDaSessao)
        {

            bool _resposta = false;

            WsAutenticacao.AutenticacaoSoapClient lServicoAutenticacao = InstanciarClientAutenticacao();
            WsAutenticacao.BuscarPermissoesDoUsuarioRequest lRequest = new WsAutenticacao.BuscarPermissoesDoUsuarioRequest();
            WsAutenticacao.BuscarPermissoesDoUsuarioResponse lResponse;

            lRequest.CodigoDoUsuario = CodigoDeAcessoDoUsuario;
            lRequest.CodigoDaSessao = CodigoDaSessao;

            lResponse = lServicoAutenticacao.BuscarPermissoesDoUsuario(lRequest);

            if (lResponse.StatusResposta == "OK")
            {
                foreach (WsAutenticacao.PermissaoAssociadaInfo lPermissao in lResponse.Permissoes)
                {
                    if (lPermissao.CodigoPermissao == Classes.Constantes.GUID_ACESSO)
                    {
                        _resposta = true;
                        IniciarContexto(CodigoDaSessao, CodigoDeAcessoDoUsuario);
                        break;
                    }
                }
            }

            return _resposta;
        }

        private WsAutenticacao.AutenticacaoSoapClient InstanciarClientAutenticacao()
        {
            string lUrl;

            WsAutenticacao.AutenticacaoSoapClient lRetorno;

            System.ServiceModel.BasicHttpBinding lBinding;

            System.ServiceModel.EndpointAddress lAddress;

            lUrl = "http://wsplataforma.gradualinvestimentos.com.br:8080/Gradual.WsIntegracao/Autenticacao.asmx";

            lBinding = new System.ServiceModel.BasicHttpBinding();

            lAddress = new System.ServiceModel.EndpointAddress(lUrl);

            lRetorno = new WsAutenticacao.AutenticacaoSoapClient(lBinding, lAddress);

            return lRetorno;
        }

        public void IniciarContexto(object pCodigoSessao, object pCodigoUsuario)
        {
            //MdsBayeuxClient.MdsHttpClient.Instance.Conecta(System.Configuration.ConfigurationManager.AppSettings["StreamerDeCotacao"]);

            ContextoGlobal.CodigoSessao = pCodigoSessao.ToString();

            ContextoGlobal.CodigoUsuario = pCodigoUsuario.ToString();
        }

        public void EfetuarLogOut()
        {
            try
            {
                //OMS.Library.MensagemRequestBase
                Gradual.OMS.Library.MensagemRequestBase lRequest = new Gradual.OMS.Library.MensagemRequestBase();
                Gradual.OMS.Library.MensagemResponseBase lResponse;

                //lRequest.CodigoSessao = ContextoGlobal.CodigoSessao;

                lResponse = gServicoseguranca.EfetuarLogOut(lRequest);

                if (lResponse.StatusResposta != Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    logger.Warn(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        public bool ValidarAssinaturaEletronica(string pAssinaturaEletronica)
        {
            try
            {
                gServicoseguranca = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.Seguranca.Lib.IServicoSeguranca>();

                Gradual.OMS.Library.MensagemResponseBase lResponse = gServicoseguranca.ValidarAssinaturaEletronica(new Gradual.OMS.Seguranca.Lib.ValidarAssinaturaEletronicaRequest()
                {
                    AssinaturaEletronica = Gradual.OMS.Seguranca.Lib.Criptografia.CalculateMD5Hash(pAssinaturaEletronica),

                    //CodigoSessao = ContextoGlobal.CodigoSessao,
                });

                if (lResponse.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
