using System;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados
{
    public partial class DadosCompletos_Perfil : PaginaBaseAutenticada
    {
        #region | Métodos

        [ValidarSegurancaAttribute("9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F", "1", "1")]
        public string  ResponderCadastrar()
        {
            string lretorno = string.Empty;
            string lObjetoJson = Request["ObjetoJson"];

            TransporteSegurancaPerfil lDadosPerfil = null;

            SalvarPerfilRequest lRequest = new SalvarPerfilRequest();

            PerfilInfo lPerfilInfo = new PerfilInfo();

            try
            {
                lDadosPerfil = JsonConvert.DeserializeObject<TransporteSegurancaPerfil>(lObjetoJson);

                lPerfilInfo = lDadosPerfil.ToPerfilInfo();

                lRequest.Perfil = lPerfilInfo;

                lRequest.CodigoSessao = this.CodigoSessao;

                lRequest.Perfil.CodigoPerfil = "0";

                var lResponse = ServicoSeguranca.SalvarPerfil(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogInclusao();

                    //lretorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.ResponseTag.DBToInt32()), "Perfil cadastrado com sucesso");

                    lretorno = RetornarSucessoAjax("Perfil cadastrado com sucesso");
                }
                else
                {
                    lretorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lretorno = RetornarErroAjax("Erro durante o envio do request para cadastrar perfil", ex);
            }

            return lretorno;
        }

        public string ResponderExcluir()
        {
            string lretorno = string.Empty;
            string lPerfilId = Request["Id"];
            RemoverPerfilRequest lRequest = new RemoverPerfilRequest()
            {
                CodigoSessao = this.CodigoSessao,
                CodigoPerfil = lPerfilId

            };
            MensagemResponseBase lResponse = null;
            try
            {
                lResponse = this.ServicoSeguranca.RemoverPerfil(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lretorno = RetornarSucessoAjax("Perfil excluido com sucesso!");
                    base.RegistrarLogExclusao();
                }
                else
                {
                    lretorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lretorno = RetornarErroAjax(ex.Message);
            }
            return lretorno;
        }

        public string ResponderAtualizar()
        {
            string lretorno = string.Empty;
            string lObjetoJson = Request["ObjetoJson"];

            TransporteSegurancaPerfil lDadosPerfil = null;

            MensagemResponseBase lResponse = null;

            SalvarPerfilRequest lRequest = new SalvarPerfilRequest();

            try
            {
                lDadosPerfil = JsonConvert.DeserializeObject<TransporteSegurancaPerfil>(lObjetoJson);
                ReceberPerfilRequest lRequestPerfil = new ReceberPerfilRequest()
                {
                    CodigoSessao = this.CodigoSessao,
                    CodigoPerfil = lDadosPerfil.Id
                };

                PerfilInfo lPerfilInfo = ((ReceberPerfilResponse)this.ServicoSeguranca.ReceberPerfil(lRequestPerfil)).Perfil;

                lPerfilInfo.NomePerfil = lDadosPerfil.Nome;

                lRequest.Perfil = lPerfilInfo;
                lRequest.CodigoSessao = this.CodigoSessao;

                lResponse = ServicoSeguranca.SalvarPerfil(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lretorno = RetornarSucessoAjax("Perfil alterado com sucesso");
                    base.RegistrarLogAlteracao();
                }
                else
                {
                    lretorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lretorno = RetornarErroAjax(ex.Message);
            }
            return lretorno;

        }

        public string ResponderCarregarHtmlComDados()
        {

            string Id = Request["Id"];
            TransporteSegurancaPerfil lDadosPerfil;
            ReceberPerfilRequest lRequest = new ReceberPerfilRequest()
            {
                CodigoSessao = this.CodigoSessao,
                CodigoPerfil = Id
            };

            ReceberPerfilResponse lResponse = this.ServicoSeguranca.ReceberPerfil(lRequest) as ReceberPerfilResponse;

            lDadosPerfil = new TransporteSegurancaPerfil(lResponse.Perfil);

            hidDadosCompletos_Seguranca_Perfil.Value = JsonConvert.SerializeObject(lDadosPerfil);

            return string.Empty;
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] {
                                                    "Cadastrar"
                                                  , "Excluir"
                                                  , "Atualizar"
                                                  , "CarregarHtmlComDados"
                                                },
            new ResponderAcaoAjaxDelegate[] { 
                                                    ResponderCadastrar
                                                  , ResponderExcluir 
                                                  , ResponderAtualizar
                                                  , ResponderCarregarHtmlComDados
                                            });

        }

        #endregion
    }
}
