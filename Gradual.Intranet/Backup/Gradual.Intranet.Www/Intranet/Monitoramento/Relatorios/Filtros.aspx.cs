using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet.Www.Intranet.Monitoramento.Relatorios
{
    public partial class Filtros : PaginaBaseAutenticada
    {
        #region | Atributos

        public string gIdAsessorLogado;

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