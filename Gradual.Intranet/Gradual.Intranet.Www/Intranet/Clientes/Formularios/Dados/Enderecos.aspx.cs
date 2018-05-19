using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class Enderecos : PaginaBaseAutenticada
    {
        #region | Propriedades

        private string GetClienteEmail
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["ClienteEmail"]))
                    return string.Empty;

                return this.Request.Form["ClienteEmail"];
            }
        }

        private string GetClienteNome
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["ClienteNome"]))
                    return string.Empty;

                return this.Request.Form["ClienteNome"].ToStringFormatoNome();
            }
        }

        #endregion

        #region | Métodos

        private string ResponderSalvar()
        {
            string lRetorno = "";

            string lObjetoJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteEndereco lDados;

                SalvarEntidadeCadastroRequest<ClienteEnderecoInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDados = JsonConvert.DeserializeObject<TransporteEndereco>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<ClienteEnderecoInfo>();

                    lRequest.EntidadeCadastro = lDados.ToClienteEnderecoInfo();

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteEnderecoInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Dados alterados com sucesso");

                            if (lDados.Id > 0)         //--> Registrando o Log.
                                base.RegistrarLogAlteracao(new Gradual.Intranet.Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lDados.ParentId });
                            else
                                base.RegistrarLogInclusao(new Gradual.Intranet.Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lDados.ParentId });
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

        private string ResponderExcluirEndereco()
        {
            string lRetorno = "";

            string lID = Request["Id"];

            if (!string.IsNullOrEmpty(lID))
            {
                RemoverEntidadeCadastroRequest<ClienteEnderecoInfo> lRequest;

                RemoverEntidadeCadastroResponse lResponse;

                try
                {
                    ClienteEnderecoInfo lEndereco = new ClienteEnderecoInfo();
                    lEndereco.IdEndereco = int.Parse(lID);
                    lRequest = new RemoverEntidadeCadastroRequest<ClienteEnderecoInfo>()
                    {
                        EntidadeCadastro = lEndereco,
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
                    };
                    try
                    {
                        lResponse = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteEnderecoInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax("Dados Excluidos com sucesso", new object[] { });
                            base.RegistrarLogExclusao();
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                        }
                    }
                    catch (Exception exEnvioRequest)
                    {
                        lRetorno = RetornarErroAjax("Erro durante o envio do request para excluir os dados", exEnvioRequest);
                    }
                }
                catch (Exception exConversao)
                {
                    lRetorno = RetornarErroAjax("Erro ao converter os dados", exConversao);
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para excluir");
            }

            return lRetorno;

        }

        private string ResponderCarregarHtmlComDados()
        {
            ConsultarEntidadeCadastroRequest<ClienteEnderecoInfo> lRequest;
            ConsultarEntidadeCadastroResponse<ClienteEnderecoInfo> lResponse;

            bool lExcluir = true;

            ClienteEnderecoInfo lDados = new ClienteEnderecoInfo(Request["Id"]);

            lRequest = new ConsultarEntidadeCadastroRequest<ClienteEnderecoInfo>()
            {
                EntidadeCadastro = lDados,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };


            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteEnderecoInfo>(lRequest);

            btnSalvar.Visible = UsuarioPode("Salvar", "10FEAC2B-7E38-4922-A8E5-8E1EF331E92C");

            NovoEndereco.Visible = UsuarioPode("Incluir", "4239f749-5c0c-4fd0-abdd-281d94a11744", "10FEAC2B-7E38-4922-A8E5-8E1EF331E92C");

            lExcluir = UsuarioPode("Excluir", "8BEF04C4-7F63-49ec-9518-2DE25DDA667F");

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (NovoEndereco.Visible && lResponse.Resultado.Count == 0)
                    btnSalvar.Visible = true;

                IEnumerable<TransporteEndereco> lLista = from ClienteEnderecoInfo t
                                                           in lResponse.Resultado
                                                         select new TransporteEndereco(t, lExcluir);

                hidClientes_Enderecos_ListaJson.Value = JsonConvert.SerializeObject(lLista);
            }
            else
            {
                //RetornarErroAjax("Erro ao consultar os telefones do cliente", lResponse.DescricaoResposta);
            }

            //hidDadosCompletos_PF.Value = JsonConvert.SerializeObject(lClientePf);

            return string.Empty;    //só para obedecer assinatura
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "Salvar"
                                                , "CarregarHtmlComDados"
                                                , "Excluir"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                                , ResponderCarregarHtmlComDados 
                                                , ResponderExcluirEndereco});

            if (!Page.IsPostBack)
            {
                this.PopularControleComListaDoCadastro<TipoEnderecoInfo>(new TipoEnderecoInfo(), rptClientes_Enderecos_Tipo);

                this.PopularControleComListaDoSinacor(eInformacao.Estado, rptClientes_Enderecos_Estado);

                this.PopularControleComListaDoSinacor(eInformacao.Pais, rptClientes_Enderecos_Pais);
            }
        }

        #endregion
    }
}
