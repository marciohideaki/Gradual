using System;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.SAC.Sistema
{
    public partial class Variaveis : PaginaBaseAutenticada
    {
        #region | Métodos

        private string ResponderSalvar()
        {
            string lRetorno = string.Empty;
            string lVariavel = this.Request.Form["SalvarVariavel"];
            string lIDVariavel = this.Request.Form["Id"];

            if (!string.IsNullOrEmpty(lVariavel))
            {
                SalvarEntidadeCadastroRequest<ConfiguracaoInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lRequest = new SalvarEntidadeCadastroRequest<ConfiguracaoInfo>();

                    lRequest.EntidadeCadastro = new ConfiguracaoInfo()
                    {
                        Valor = lVariavel,
                        Configuracao = EConfiguracaoDescricao.PeriodicidadeRenovacaoCadastral,
                        IdConfiguracao = int.Parse(lIDVariavel)
                    };

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ConfiguracaoInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Dados alterados com sucesso");
                            base.RegistrarLogAlteracao();
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                        }
                    }
                    catch (Exception exEnvioRequest)
                    {
                        lRetorno = RetornarErroAjax("Erro durante o envio do request para salvar os dados", exEnvioRequest);
                    }
                }
                catch (Exception exConversao)
                {
                    lRetorno = RetornarErroAjax("Erro ao converter os dados", exConversao);
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para alterar");
            }

            return lRetorno;
        }

        /// <summary>
        /// Carrega os dados no formulário
        /// </summary>
        /// <returns></returns>
        private string ResponderCarregarHtmlComDados()
        {
            ReceberEntidadeCadastroRequest<ConfiguracaoInfo> req = new ReceberEntidadeCadastroRequest<ConfiguracaoInfo>()
            {
                EntidadeCadastro = new ConfiguracaoInfo()
                {
                    Configuracao = EConfiguracaoDescricao.PeriodicidadeRenovacaoCadastral
                },
                IdUsuarioLogado = base.UsuarioLogado.Id,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome
            };

            ConfiguracaoInfo lInfo = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ConfiguracaoInfo>(req).EntidadeCadastro;

            TransporteVariavelDoSistema lTransporteSistema = new TransporteVariavelDoSistema(lInfo);

            hidSistema_Variaveis_PeriodicidadeRenovacaoCadastral.Value = JsonConvert.SerializeObject(lTransporteSistema);

            txtSistema_Variaveis_PeriodicidadeRenovacaoCadastral.Value = lTransporteSistema.Valor;

            hidSistema_Variaveis_PeriodicidadeRenovacaoCadastral_Id.Value = lTransporteSistema.Id.ToString();

            return string.Empty;    //só para obedecer assinatura
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "SalvarVariaveis" },
                new ResponderAcaoAjaxDelegate[] { ResponderSalvar });

            if (!Page.IsPostBack)
                this.ResponderCarregarHtmlComDados();
        }

        #endregion
    }
}
