using System;
using System.Collections.Generic;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.DBM.Relatorios
{
    public partial class Filtros : PaginaBaseAutenticada
    {
        #region | Atributos

        public string gIdAsessorLogado;

        public string gIdAssessorLogadoFilial;

        #endregion

        #region | Propriedades

        private int? GetCodigoAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                return null;
            }
        }

        private int? GetCodigoFilial
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.GetIdAssessorFilialLogado;

                return null;
            }
        }

        private List<KeyValuePair<string, string>> GetListaRelatorios
        {
            get
            {
                var lRetorno = new List<KeyValuePair<string, string>>();

                lRetorno.Add(new KeyValuePair<string, string>("R001", "R-001: Resumo da Praça"));
                lRetorno.Add(new KeyValuePair<string, string>("R002", "R-002: Resumo Gerencial"));
                lRetorno.Add(new KeyValuePair<string, string>("R003", "R-003: Resumo do Cliente"));
                lRetorno.Add(new KeyValuePair<string, string>("R004", "R-004: LTV do Cliente"));
                lRetorno.Add(new KeyValuePair<string, string>("R005", "R-005: Total de Cliente por Assessor"));
                lRetorno.Add(new KeyValuePair<string, string>("R006", "R-006: Saldo e Projeções em Conta Corrente"));
                lRetorno.Add(new KeyValuePair<string, string>("R007", "R-007: Movimento de Conta Corrente"));

                return lRetorno;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (null != this.GetCodigoAssessor)
            {
                this.txtDBM_FiltroRelatorio_CodAssessor.Value = this.GetCodigoAssessor.Value.ToString();
                this.txtDBM_FiltroRelatorio_CodAssessor.Disabled = true;
                gIdAsessorLogado = this.GetCodigoAssessor.Value.ToString();
            }

            if (null != this.GetCodigoFilial)
            {
                gIdAssessorLogadoFilial = this.GetCodigoFilial.DBToString();
            }

            if (!Page.IsPostBack)
            {
                this.CarregarFilial();
                this.CarregarListaDeRelatorios();
            }
        }

        #endregion

        #region | Métodos

        private void CarregarFilial()
        {
            var lRetornoFilial = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<FilialInfo>(
                new ConsultarEntidadeCadastroRequest<FilialInfo>()
                {
                    EntidadeCadastro = new FilialInfo()
                });

            if (lRetornoFilial.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.rptFilial.DataSource = lRetornoFilial.Resultado;
                this.rptFilial.DataBind();
            }
        }

        private void CarregarListaDeRelatorios()
        {
            var lListaRelatorios = this.GetListaRelatorios;

            var lUsuarioPodeVisualizarResumoGerencial = base.UsuarioPode("Consultar", "6f94ac37-a66d-4199-a181-37b6b3b05bee"); //--> Permissão de visualização do 'Resumo Gerencial'.

            if (!lUsuarioPodeVisualizarResumoGerencial)
                lListaRelatorios.RemoveAt(1);

            this.rptRelatorios.DataSource = lListaRelatorios;
            this.rptRelatorios.DataBind();
        }

        #endregion
    }
}