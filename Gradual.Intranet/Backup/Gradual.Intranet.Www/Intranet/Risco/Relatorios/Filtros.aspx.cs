using System;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Relatorios
{
    public partial class Filtros : PaginaBaseAutenticada
    {
        #region | Atributos

        public static string gIdAsessorLogado;

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);
                this.BindCombos();
                this.DefinirExibicaoModoAssessor();
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion

        #region | Métodos de apoio

        private void BindCombos()
        {
            {   //--> Combo Grupos
                var listaGrupos = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoGrupoInfo>(new ConsultarEntidadeCadastroRequest<RiscoGrupoInfo>() { DescricaoUsuarioLogado = base.UsuarioLogado.Nome, IdUsuarioLogado = base.UsuarioLogado.Id });
                listaGrupos.Resultado.Insert(0, new RiscoGrupoInfo() { IdGrupo = 0, DsGrupo = "[ Todos ]" });
                base.PopularComboComListaGenerica<RiscoGrupoInfo>(listaGrupos.Resultado, this.rptRisco_FiltroRelatorioRisco_Grupo);
            }

            {   //--> Combo Permissao
                var listaPermissao = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoPermissaoInfo>(new ConsultarEntidadeCadastroRequest<RiscoPermissaoInfo>() { DescricaoUsuarioLogado = base.UsuarioLogado.Nome, IdUsuarioLogado = base.UsuarioLogado.Id });
                listaPermissao.Resultado.Insert(0, new RiscoPermissaoInfo() { IdPermissao = 0, DsPermissao = "[ Todas ]" });
                base.PopularComboComListaGenerica<RiscoPermissaoInfo>(listaPermissao.Resultado, this.rptRisco_FiltroRelatorioRisco_Permissao);
            }

            {   //--> Combo Parametros
                var listaParametros = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoParametrosInfo>(new ConsultarEntidadeCadastroRequest<RiscoParametrosInfo>() { DescricaoUsuarioLogado = base.UsuarioLogado.Nome, IdUsuarioLogado = base.UsuarioLogado.Id });
                listaParametros.Resultado.Insert(0, new RiscoParametrosInfo() { IdParametro = 0, DsParametro = "[ Todos ]" });
                base.PopularComboComListaGenerica<RiscoParametrosInfo>(listaParametros.Resultado, this.rptRisco_FiltroRelatorioRisco_Parametro);
            }

            {   //--> Combo Assessores Sinacor
                base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptRisco_FiltroRelatorioRisco_Assessor);
            }
        }

        private void DefinirExibicaoModoAssessor()
        {
            ReceberSessaoRequest lReq = new ReceberSessaoRequest();
            lReq.CodigoSessao = base.CodigoSessao;
            lReq.CodigoSessaoARetornar = base.CodigoSessao;
            ReceberSessaoResponse lRes = ServicoSeguranca.ReceberSessao(lReq);

            if (lRes.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK
            && (lRes.Usuario.Perfis.Contains("6")))
            {
                gIdAsessorLogado = lRes.Usuario.CodigoAssessor.ToString();
            }
        }

        #endregion
    }
}