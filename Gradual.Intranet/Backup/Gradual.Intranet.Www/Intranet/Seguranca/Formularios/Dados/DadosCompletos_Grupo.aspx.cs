using System;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados
{
    [ValidarSegurancaAttribute("9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F", "1", "1")]
    public partial class DadosCompletos_Grupo : PaginaBaseAutenticada
    {

        public string ResponderCadastrar()
        {
            string lretorno = string.Empty;
            string lObjetoJson = Request["ObjetoJson"];

            MensagemResponseBase lResponse = null;
            TransporteSegurancaGrupo lDadosGrupo = null;
            SalvarUsuarioGrupoRequest lRequest = new SalvarUsuarioGrupoRequest();
            UsuarioGrupoInfo lUsuarioGrupoInfo = new UsuarioGrupoInfo();

            try
            {
                lDadosGrupo = JsonConvert.DeserializeObject<TransporteSegurancaGrupo>(lObjetoJson);

                lRequest.UsuarioGrupo = lDadosGrupo.ToUsuarioGrupoInfo();

                lRequest.UsuarioGrupo.CodigoUsuarioGrupo = "0";

                lRequest.CodigoSessao = this.CodigoSessao;

                lResponse = ServicoSeguranca.SalvarUsuarioGrupo(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogInclusao();

                    lretorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lUsuarioGrupoInfo.CodigoUsuarioGrupo), "Grupo cadastrado com sucesso");
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

        public string ResponderExcluir()
        {
            string lretorno = string.Empty;
            string lUsuarioGrupoId = Request["Id"];
            RemoverUsuarioGrupoRequest lRequest = new RemoverUsuarioGrupoRequest()
                {
                    CodigoSessao = this.CodigoSessao,
                    CodigoUsuarioGrupo = lUsuarioGrupoId

                };
            MensagemResponseBase lResponse = null;
            try
            {
                lResponse = this.ServicoSeguranca.RemoverUsuarioGrupo(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lretorno = RetornarSucessoAjax("Grupo excluido com sucesso!");
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

            TransporteSegurancaGrupo lDadosGrupo = null;

            MensagemResponseBase lResponse = null;

            SalvarUsuarioGrupoRequest lRequest = new SalvarUsuarioGrupoRequest();

            try
            {
                lDadosGrupo = JsonConvert.DeserializeObject<TransporteSegurancaGrupo>(lObjetoJson);
                ReceberUsuarioGrupoRequest lRequestGrupo = new ReceberUsuarioGrupoRequest()
                {
                    CodigoSessao = this.CodigoSessao,
                    CodigoUsuarioGrupo = lDadosGrupo.Id
                };

                UsuarioGrupoInfo lUsuarioGrupoInfo = ((ReceberUsuarioGrupoResponse)this.ServicoSeguranca.ReceberUsuarioGrupo(lRequestGrupo)).UsuarioGrupo;

                lUsuarioGrupoInfo.NomeUsuarioGrupo = lDadosGrupo.Nome;

                lRequest.UsuarioGrupo = lUsuarioGrupoInfo;
                lRequest.CodigoSessao = this.CodigoSessao;

                lResponse = ServicoSeguranca.SalvarUsuarioGrupo(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lretorno = RetornarSucessoAjax("Grupo alterado com sucesso");
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
            TransporteSegurancaGrupo lDadosUsuarioGrupo;
            ReceberUsuarioGrupoRequest lRequest = new ReceberUsuarioGrupoRequest()
            {
                CodigoSessao = this.CodigoSessao,
                CodigoUsuarioGrupo = Id
            };

            ReceberUsuarioGrupoResponse lResponse = this.ServicoSeguranca.ReceberUsuarioGrupo(lRequest) as ReceberUsuarioGrupoResponse;

            lDadosUsuarioGrupo = new TransporteSegurancaGrupo(lResponse.UsuarioGrupo);

            hidDadosCompletos_Seguranca_Grupo.Value = JsonConvert.SerializeObject(lDadosUsuarioGrupo);

            return string.Empty;
        }

        #region Event Handlers

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
