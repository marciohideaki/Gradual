using System;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Cadastro;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class ClienteGrupo : PaginaBaseAutenticada
    {
        #region | Propriedades

        private int GetIdGrupo
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["IdGrupo"], out lRetorno);

                return lRetorno;
            }
        }

        private int? GetCdCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CdCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCdAssessor
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CdAssessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if ("CarregarHtml".Equals(base.Acao))
                this.ResponderCarregarHtml();

            base.RegistrarRespostasAjax(new string[] { "Gravar"
                                                     , "CarregarHtmlComDados"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { ResponderGravar
                                                     , ResponderCarregarHtml
                                                     });
        }

        #endregion

        #region | Métodos

        private string ResponderCarregarHtml()
        {
            base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptGradIntra_Risco_ClienteGrupo_Assessor);

            var lGrupos = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoGrupoInfo>(
                new ConsultarEntidadeCadastroRequest<RiscoGrupoInfo>()
                {
                    EntidadeCadastro = new RiscoGrupoInfo
                    {
                        TipoGrupo = EnumRiscoRegra.TipoGrupo.GrupoAlavancagem
                    }
                });

            if (lGrupos.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.rptGradIntra_Risco_ClienteGrupo_Grupo.DataSource = lGrupos.Resultado;
                this.rptGradIntra_Risco_ClienteGrupo_Grupo.DataBind();

                return base.RetornarSucessoAjax("");
            }
            else
            {
                return base.RetornarErroAjax(lGrupos.DescricaoResposta);
            }
        }

        private string ResponderGravar()
        {
            var lRetorno = string.Empty;

            var lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<RiscoClienteGrupoInfo>(
                new SalvarEntidadeCadastroRequest<RiscoClienteGrupoInfo>()
                {
                    EntidadeCadastro = new RiscoClienteGrupoInfo()
                    {
                        CdAssessor = this.GetCdAssessor,
                        CdCliente = this.GetCdCliente,
                        IdGrupo = this.GetIdGrupo
                    }
                });
            
            base.RegistrarLogInclusao(new LogIntranetInfo() //--> Registrando o Log
            {
                IdClienteAfetado = this.GetCdCliente,
                DsObservacao = string.Concat("Alterção de Grupo do Cliente:", this.GetCdCliente)
            });

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lRetorno = base.RetornarSucessoAjax("Dados inseridos com sucesso.");
            }
            else
            {
                lRetorno = base.RetornarErroAjax(lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        #endregion
    }
}