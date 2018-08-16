using System;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Library;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Cliente;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R014 : PaginaBase
    {
        #region | Propriedades

        private int GetAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                var lRetorno = default(int);

                if (int.TryParse(this.Request["Assessor"], out lRetorno))
                    return lRetorno;

                return lRetorno;
            }
        }

        private string GetPapel
        {
            get
            {
                var lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request["papel"]))
                    lRetorno = this.Request["papel"];

                return lRetorno;
            }
        }

        #endregion

        #region | Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            //base.Page_Load(sender, e);

            if (base.Acao == "BuscarItensParaListagemSimples")
            {
                this.ResponderBuscarItensParaListagemSimples();
            }
            else if (this.Acao == "BuscarParte")
            {
                this.Response.Clear();

                this.Response.End();
            }
            else if (base.Acao == "CarregarComoCSV")
            {
                this.ResponderArquivoCSV();
            }
        }

        #endregion

        #region | Métodos

        private string ResponderBuscarItensParaListagemSimples()
        {
            var lRetorno = string.Empty;

            var lListaAssessores = base.ConsultarCodigoAssessoresVinculadosString(this.GetAssessor);

            var lResponse = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<PosicaoConsolidadaPorPapelRelInfo>(
                new ConsultarEntidadeCadastroRequest<PosicaoConsolidadaPorPapelRelInfo>()
                {
                    EntidadeCadastro = new PosicaoConsolidadaPorPapelRelInfo()
                    {
                        ConsultaCodigoAssessor = lListaAssessores,//this.GetAssessor,
                        ConsultaInstrumento = this.GetPapel,
                    }
                });

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK && lResponse.Resultado.Count > 0)
            {
                this.rptGradIntra_Clientes_Relatorios_PosicaoConsolidadaPorPapel.DataSource = new TransporteRelatorio_014().TraduzirLista(lResponse.Resultado);
                this.rptGradIntra_Clientes_Relatorios_PosicaoConsolidadaPorPapel.DataBind();
                
                this.rowLinhaDeNenhumItem.Visible = false;
            }
            else 
            {
                this.rowLinhaDeNenhumItem.Visible = true;
            }

            this.Response.Clear();

            return lRetorno;
        }

        private string ResponderArquivoCSV()
        {
            var lRetorno = string.Empty;


            return lRetorno;
        }

        #endregion
    }
}