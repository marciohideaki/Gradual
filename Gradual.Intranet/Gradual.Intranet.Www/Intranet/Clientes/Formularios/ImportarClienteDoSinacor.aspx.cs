using System;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class ImportarClienteDoSinacor : PaginaBaseAutenticada
    {
        #region | Atributos

        private bool gStReimportacao = false;

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                , "ResponderImportacao"
                                                , "ResponderReimportacao"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderCarregarHtmlComDados
                                                , ResponderImportacao
                                                , ResponderReimportacao
                                                });
        }

        #endregion

        #region | Métodos

        private string ResponderCarregarHtmlComDados()
        {
            return string.Empty;
        }

        private string ResponderImportacao()
        {
            this.gStReimportacao = false;
            return this.RealizarImportacao();
        }

        private string ResponderReimportacao()
        {
            this.gStReimportacao = true;
            return this.RealizarImportacao();
        }

        private string RealizarImportacao()
        {
            string lRetorno = string.Empty;
            string lObjetoJson = this.Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteImportacao lTransporteImportacao = JsonConvert.DeserializeObject<TransporteImportacao>(lObjetoJson);

                if (string.IsNullOrWhiteSpace(lTransporteImportacao.CPF_CNPJ) || string.IsNullOrWhiteSpace(lTransporteImportacao.DataNascimento))
                    return RetornarErroAjax("Todos os Campos precisam ser preenchidos");

                ReceberEntidadeCadastroResponse<SinacorClienteInfo> RetornoClienteSinacor = new ReceberEntidadeCadastroResponse<SinacorClienteInfo>();
                ReceberEntidadeCadastroRequest<SinacorClienteInfo> EntradaClienteSinacor = new ReceberEntidadeCadastroRequest<SinacorClienteInfo>();
                EntradaClienteSinacor.EntidadeCadastro = new SinacorClienteInfo();
                EntradaClienteSinacor.IdUsuarioLogado = base.UsuarioLogado.Id;
                EntradaClienteSinacor.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                EntradaClienteSinacor.EntidadeCadastro.ChaveCliente = lTransporteImportacao.ToSinacorChaveInfo();

                //Pegando o cliente completo do Sinacor
                RetornoClienteSinacor = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<SinacorClienteInfo>(EntradaClienteSinacor);

                if (RetornoClienteSinacor.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    SalvarEntidadeCadastroRequest<SinacorClienteInfo> EntradaCliente = new SalvarEntidadeCadastroRequest<SinacorClienteInfo>();
                    SalvarEntidadeCadastroResponse RetornoCliente = new SalvarEntidadeCadastroResponse();
                    EntradaCliente.EntidadeCadastro = RetornoClienteSinacor.EntidadeCadastro;
                    EntradaCliente.IdUsuarioLogado = base.UsuarioLogado.Id;
                    EntradaCliente.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                    EntradaCliente.EntidadeCadastro.StReimportacao = this.gStReimportacao;

                    //Salvando no Cadastro
                    RetornoCliente = ServicoPersistenciaCadastro.SalvarEntidadeCadastro<SinacorClienteInfo>(EntradaCliente);

                    if (RetornoCliente.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = base.RetornarSucessoAjax("Cliente Importado com sucesso");
                        
                        string lLogFraseObservacao = string.Empty;

                        if (gStReimportacao)
                        {
                            lLogFraseObservacao = string.Concat("Requisitada a REIMPORTAÇÃO do cliente: cd_cliente = ", RetornoCliente.DescricaoResposta);
                        }
                        else
                        {
                            lLogFraseObservacao = string.Concat("Requisitada a IMPORTAÇÃO do cliente: cd_cliente = ", RetornoCliente.DescricaoResposta);
                        }

                        base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = RetornoCliente.DescricaoResposta.DBToInt32(), DsObservacao = lLogFraseObservacao });
                    }
                    else
                    {
                        if (RetornoCliente.DescricaoResposta.ToUpper().Contains("CPF/CNPJ já cadastrado para outro Cliente".ToUpper()))
                        {
                            lRetorno = base.RetornarSucessoAjax("Erro ao Tentar Importar o Cliente: CPF/CNPJ já cadastrado");
                        }
                        else
                        {
                            lRetorno = base.RetornarErroAjax("Erro ao Tentar Importar o Cliente", RetornoCliente.DescricaoResposta);
                        }
                    }
                }
                else
                {
                    if (RetornoClienteSinacor.DescricaoResposta.ToUpper().Contains("Cliente não encontrado no Sinacor".ToUpper()))
                    {
                        lRetorno = base.RetornarErroAjax("Erro ao Tentar Importar o Cliente: Cliente não encontrado no Sinacor");
                    }
                    else
                    {
                        lRetorno = base.RetornarErroAjax("Erro ao Tentar Importar o Cliente", RetornoClienteSinacor.DescricaoResposta);
                    }
                }
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Foi enviada ação de cadastro sem objeto para alterar");
            }

            return lRetorno;
        }

        #endregion
    }
}
