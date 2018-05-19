using System;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;
using Gradual.Intranet.Contratos.Dados.Portal;
using Gradual.Intranet.Contratos.Mensagens;

namespace Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados
{
    public partial class AlterarSenha : PaginaBaseAutenticada
    {
        public string GetSenhaAtual
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["SenhaAtual"]))
                    return string.Empty;

                return this.Request.Form["SenhaAtual"];
            }
        }

        public string GetSenhaNova
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["SenhaNova"]))
                    return string.Empty;

                return this.Request.Form["SenhaNova"];
            }
        }

        public string GetSenhaConfirma
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["SenhaConfirma"]))
                    return string.Empty;

                return this.Request.Form["SenhaConfirma"];
            }
        }

        public string ResponderSalvar()
        {
            try
            {
                ValidarHistoricoDeSenha();

                if (GetSenhaNova.Length <= 5)
                {
                    return RetornarErroAjax("Nova senha deve conter mais que 6 digitos.");
                }
                else if (GetSenhaNova != GetSenhaConfirma)
                {
                    return RetornarErroAjax("Nova senha não confere com a comfirmação da senha.");
                }
                else
                {
                    AlterarSenhaRequest lReq = new AlterarSenhaRequest()
                    {
                        SenhaAtual = Criptografia.CalculateMD5Hash(GetSenhaAtual),
                        NovaSenha = Criptografia.CalculateMD5Hash(GetSenhaNova),
                        CodigoSessao = this.CodigoSessao,
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
                    };

                    MensagemResponseBase lRes = ServicoSeguranca.AlterarSenha(lReq);

                    GravarHistoricoSenha();

                    if (lRes.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        

                        base.RegistrarLogAlteracao(new Contratos.Dados.Cadastro.LogIntranetInfo() { IdLoginClienteAfetado = base.UsuarioLogado.Id, DsObservacao = string.Concat("Senha alterada para o cliente: ", base.UsuarioLogado.Id) });
                        return RetornarSucessoAjax("Senha alterada com sucesso");
                    }
                    else
                    {
                        if (lRes.StatusResposta == MensagemResponseStatusEnum.ErroNegocio)
                        {
                            string lCritica = "";
                            foreach (var item in lRes.Criticas)
                            {
                                lCritica += item.Descricao + Environment.NewLine;
                            }
                            return RetornarErroAjax(lCritica);
                        }
                        else
                        {
                            return RetornarErroAjax(lRes.DescricaoResposta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return RetornarErroAjax("Não foi possível alterar a senha: " + ex.Message);
            }
        }

        private void ValidarHistoricoDeSenha()
        {
            ConsultarEntidadeCadastroRequest<HistoricoSenhaInfo> lRequest = new ConsultarEntidadeCadastroRequest<HistoricoSenhaInfo>();
            ConsultarEntidadeCadastroResponse<HistoricoSenhaInfo> lResponse;

            lRequest.EntidadeCadastro = new HistoricoSenhaInfo();

            lRequest.EntidadeCadastro.CdSenha = this.GetSenhaNova;
            lRequest.EntidadeCadastro.IdLogin = base.UsuarioLogado.Id;

            //lRequest.EntidadeCadastro.IdLogin = 222044;

            lResponse = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<HistoricoSenhaInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count > 0)
                {
                    throw new Exception("JA_UTILIZADA");
                }
            }
            else
            {
                //gLogger.ErrorFormat("Erro em AlterarSenha.aspx > ValidarHistoricoDeSenha(IdLogin [{0}]) [{1}]\r\n{2}"
                //                    , lRequest.EntidadeCadastro.IdLogin
                //                    , lResponse.StatusResposta
                //                    , lResponse.DescricaoResposta);

                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        private void GravarHistoricoSenha()
        {
            SalvarEntidadeCadastroRequest<HistoricoSenhaInfo> lRequest = new SalvarEntidadeCadastroRequest<HistoricoSenhaInfo>();
            SalvarEntidadeCadastroResponse lResponse;

            lRequest.EntidadeCadastro = new HistoricoSenhaInfo();

            lRequest.EntidadeCadastro.CdSenha = this.GetSenhaNova;
            lRequest.EntidadeCadastro.IdLogin = base.UsuarioLogado.Id;

            lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<HistoricoSenhaInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                //gLogger.InfoFormat("Histórico de Nova senha para usuário [{0}] gravado com sucesso.", lRequest.EntidadeCadastro.IdLogin);
            }
            else
            {
                //gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaCadastro.SalvarEntidadeCadastro<HistoricoSenhaInfo>(pIdLogin: [{0}]) em AlterarSenha.aspx > GravarHistoricoSenha() > [{1}]\r\n{2}"
                //                    , lRequest.EntidadeCadastro.IdLogin
                //                    , lResponse.StatusResposta
                //                    , lResponse.DescricaoResposta);

                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        public string ResponderCarregarHtmlComDados()
        {
            return string.Empty;
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados",
                                                  "AlterarSenha" },
                new ResponderAcaoAjaxDelegate[] { ResponderCarregarHtmlComDados,
                                                  ResponderSalvar });
        }
    }
}