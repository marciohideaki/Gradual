using System;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios
{
    public partial class LinkParaProspect : PaginaBaseAutenticada
    {
        public struct sAssessor
        {
            public string Id { get; set; }
            public string Value { get; set; }
        }

        public string GetNomePesquisa
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["Nome"]))
                    return string.Empty;

                return this.Request.Form["Nome"];
            }
        }

        private string ResponderCarregarHtmlComDados()
        {
            string lRetorno = string.Empty;

            var lLogin = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<LoginInfo>(
                new Contratos.Mensagens.ReceberEntidadeCadastroRequest<LoginInfo>()
                {
                    IdUsuarioLogado = base.UsuarioLogado.Id,
                    DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                    EntidadeCadastro = new LoginInfo()
                    {
                        IdLogin = base.UsuarioLogado.Id
                    }
                });

            if (lLogin.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                //Se Assessor
                if (lLogin.EntidadeCadastro.TpAcesso == Contratos.Dados.Enumeradores.eTipoAcesso.Assessor)
                {
                    divClientes_LinkProspect_Pesquisa.Visible = false;
                    List<sAssessor> lListaAssessor = new List<sAssessor>();
                    sAssessor lAssessor = new sAssessor() { Id = lLogin.EntidadeCadastro.CdAssessor.Value.ToString(), Value = base.UsuarioLogado.Nome };
                    lListaAssessor.Add(lAssessor);
                    rptClientes_LinkProspect_Resultado.DataSource = lListaAssessor;
                    rptClientes_LinkProspect_Resultado.DataBind();
                }
                else
                {
                    divClientes_LinkProspect_Pesquisa.Visible = true;
                }
            }
            else
            {
                lRetorno = RetornarErroAjax(lLogin.DescricaoResposta);
            }

            return lRetorno;

        }

        private string ResponderConsultar()
        {
            string lRetorno = string.Empty;

            var lAssessores = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaInfo>(
                new Contratos.Mensagens.ConsultarEntidadeCadastroRequest<SinacorListaInfo>()
                {
                    IdUsuarioLogado = base.UsuarioLogado.Id,
                    DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                    EntidadeCadastro = new SinacorListaInfo()
                    {
                        Informacao = Contratos.Dados.Enumeradores.eInformacao.Assessor
                    }
                }
                );

            if (lAssessores.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                var lAssessoresFiltrado = (from filtro in lAssessores.Resultado
                                           where filtro.Value.ToUpper().Contains(GetNomePesquisa.ToUpper())
                                           select filtro).ToList();

                //rptClientes_LinkProspect_Resultado.DataSource = lAssessoresFiltrado;
                //rptClientes_LinkProspect_Resultado.DataBind();

                lRetorno = RetornarSucessoAjax(lAssessoresFiltrado, lAssessoresFiltrado.Count.ToString() + " Assessores Encontrados");
            }
            else
            {
                lRetorno = RetornarErroAjax(lAssessores.DescricaoResposta);
            }

            return lRetorno;
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                , "ConsultarLinkProspect"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderCarregarHtmlComDados 
                                                , ResponderConsultar 
            });

        }
    }
}