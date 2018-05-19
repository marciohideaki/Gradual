using System;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Library;
using Gradual.Intranet.Www.App_Codigo;
using System.Collections.Generic;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class EfetivarRenovacao : PaginaBaseAutenticada
    {
        #region | Propriedades

        private DateTime GetDataRenovacao
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["Data"].ToString(), out lRetorno);

                return lRetorno;
            }
        }

        private string GetCpfCnpj
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["CPF"]))
                    return null;

                return this.Request.Form["CPF"];
            }
        }

        private int GetCdCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["CdClienteBovespa"], out lRetorno);

                if (lRetorno == 0) int.TryParse(this.Request.Form["CdClienteBMF"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetAssessor
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["IdAssessor"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetIdCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["idcliente"], out lRetorno);

                return lRetorno;
            }
        }

        #endregion

        #region | Métodos

        private string ResponderCarregarHtmlComDados()
        {
            return string.Empty;    //só para obedecer assinatura
        }

        private string ResponderEfetivarRenovacao()
        {
            string lRetorno = string.Empty;

            if (DateTime.MinValue.Equals(this.GetDataRenovacao))
                lRetorno = base.RetornarErroAjax("Existem campos inválidos, favor verificar");
            else
            {
                var lInfo = new ClienteRenovacaoCadastralInfo()
                {
                    DsCpfCnpj = this.GetCpfCnpj,
                    DtRenovacao = this.GetDataRenovacao
                };

                if (lInfo.DtRenovacao > DateTime.Now)
                    lRetorno = base.RetornarErroAjax("A Data de Renovação não pode ser maior que hoje");
                else if (!string.IsNullOrEmpty(lInfo.DsCpfCnpj))
                {
                    lInfo.DsCpfCnpj = lInfo.DsCpfCnpj.Replace(".", "").Replace("-", "");

                    var lRequest = new SalvarEntidadeCadastroRequest<ClienteRenovacaoCadastralInfo>(){EntidadeCadastro = lInfo};

                    var lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteRenovacaoCadastralInfo>(lRequest);

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = RetornarSucessoAjax("Renovação efetivada com sucesso");

                        this.EnviarEmailDeNotificacaoAoAssessor();

                        base.RegistrarLogAlteracao(new Contratos.Dados.Cadastro.LogIntranetInfo()
                        {
                            DsCpfCnpjClienteAfetado = lInfo.DsCpfCnpj,
                            DsObservacao = string.Concat("Renovação cadastral realizada para o cliente: cd_cpfcnpj = ", lInfo.DsCpfCnpj)
                        });
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax(string.Format("Renovação efetivada com erro: [{0}]", lResponse.StatusResposta), lResponse.DescricaoResposta);
                    }
                }
                else
                {
                    lRetorno = RetornarErroAjax("Sem CPF para efetivar renovação");
                }
            }
            return lRetorno;
        }

        private void EnviarEmailDeNotificacaoAoAssessor()
        {   //--> Enviando e-mail de notificação ao assessor do cliente.
            var lVariaveisEmail = new Dictionary<string, string>();

            var lAssessor = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<AssessorDoClienteInfo>(
                new ReceberEntidadeCadastroRequest<AssessorDoClienteInfo>()
                {   //--> Recebendo dados do Assessor.
                    IdUsuarioLogado = base.UsuarioLogado.Id,
                    DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                    EntidadeCadastro = new AssessorDoClienteInfo()
                    {
                        IdCliente = this.GetIdCliente
                    }
                });

            lVariaveisEmail.Add("@NomeAssessor", lAssessor.EntidadeCadastro.NomeAssessor);
            lVariaveisEmail.Add("@cdCliente", this.GetCdCliente.ToCodigoClienteFormatado());

            var lListaAssessores = base.ConsultarListaEmailAssessor(this.GetAssessor);

            if (null != lListaAssessores && null != lListaAssessores.ListaEmailAssessor && lListaAssessores.ListaEmailAssessor.Count > 0)
                lListaAssessores.ListaEmailAssessor.ForEach(lEmailAssessor =>
                {
                    base.EnviarEmail(lEmailAssessor, "Notificação de Renovação Cadastral", "CadastroNotificarRenovacaoCadastral.htm", lVariaveisEmail, Contratos.Dados.Enumeradores.eTipoEmailDisparo.Assessor);
                });
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                     , "EfetivarRenovacao"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { ResponderCarregarHtmlComDados
                                                     , ResponderEfetivarRenovacao
                                                     });
        }

        #endregion
    }
}