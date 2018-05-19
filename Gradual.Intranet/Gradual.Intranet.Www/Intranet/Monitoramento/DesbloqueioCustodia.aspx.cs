using System;
using Gradual.Intranet.Contratos.Dados.Monitoramento;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;

namespace Gradual.Intranet.Www.Intranet.Monitoramento
{
    public partial class DesbloqueioCustodia : PaginaBaseAutenticada
    {
        #region | Propriedades

        public string GetInstrumento
        {
            get
            {
                return Request.Form["Papel"].ToString();
            }
        }

        public int GetQuantidade
        {
            get
            {
                return Convert.ToInt32(Request.Form["Quantidade"]);
            }
        }

        public int GetCodBovespa
        {
            get
            {
                return Convert.ToInt32(Request.Form["CodBovespa"]);
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            base.RegistrarRespostasAjax(new string[] { "DesbloqueioCustodia" },
                     new ResponderAcaoAjaxDelegate[] { ResponderDesbloqueioCustodia });
        }

        #endregion

        #region ResponderDesbloqueioCustodia
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ResponderDesbloqueioCustodia()
        {
            string lRetorno = string.Empty;

            var lRequest = new SalvarEntidadeCadastroRequest<MonitoramentoDesbloqueioCustodiaInfo>();

            lRequest.EntidadeCadastro = new MonitoramentoDesbloqueioCustodiaInfo();

            lRequest.EntidadeCadastro.Instrumento = this.GetInstrumento;

            lRequest.EntidadeCadastro.Quantidade = this.GetQuantidade;

            lRequest.EntidadeCadastro.CodBovespa = this.GetCodBovespa;

            var lResponse = MonitoramentoDesbloqueioCustodiaDbLib.DesbloqueiaCustodia(lRequest);

            lRetorno = lResponse.DescricaoResposta;

            if (OMS.Library.MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
            {
                base.RegistrarLogAlteracao(new Contratos.Dados.Cadastro.LogIntranetInfo()
                {
                    CdBovespaClienteAfetado = this.GetCodBovespa,
                    DsObservacao = string.Format("Instrumento desbloqueado: {0}; quantidade: {1}; cd_bovespa cliente: {2}", this.GetInstrumento, this.GetQuantidade, this.GetCodBovespa),
                });

                return RetornarSucessoAjax(lRetorno);
            }
            else
            {
                return RetornarErroAjax(lRetorno);
            }
        }

        #endregion
    }
}