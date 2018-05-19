using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Gradual.OMS.Library;
using System.Globalization;
using Gradual.OMS.Library.Servicos;
using Gradual.Core.OMS.LimiteManager.Lib;
using Gradual.Core.OMS.LimiteManager.Lib.Mensageria;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class TravaExposicaoSpider : PaginaBaseAutenticada
    {
        #region Propriedades
        private decimal? GetPrejuizoMaximo
        {
            get
            {
                var lRetorno = default(decimal);

                decimal.TryParse(this.Request.Form["VlPrejuizoMaximo"], out lRetorno);

                return lRetorno;
            }
        }

        private decimal? GetPercentualOscilacao
        {
            get
            {
                var lRetorno = default(decimal);

                decimal.TryParse(this.Request.Form["VlPercentualOscilacao"], out lRetorno);

                return lRetorno;
            }
        }
        
        #endregion

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            //if (base.Acao.Equals("CarregarHtmlComDados"))
            //    this.ResponderCarregarHtmlComDados();
            //else
            //{
            if (!this.IsPostBack) ResponderCarregarHtmlComDados();

                base.RegistrarRespostasAjax(new string[] { 
                                                        "SalvarDadosTravaExposicao"
                                                        },
                         new ResponderAcaoAjaxDelegate[] { 
                             // this.ResponderCarregarHtmlComDados,
                             this.ResponderSalvarTravaDadosExposicao
                                                     });
            //}
        }

        private string ResponderSalvarTravaDadosExposicao()
        {
            string lRetorno = string.Empty;

            try
            {
                SalvarTravaExposicaoRequest lRequest = new SalvarTravaExposicaoRequest();

                lRequest.Exposicao                     = new OMS.Risco.Regra.Lib.Dados.TravaExposicaoInfo();
                lRequest.Exposicao.PrecentualOscilacao = this.GetPercentualOscilacao.Value;
                lRequest.Exposicao.PrejuizoMaximo      = this.GetPrejuizoMaximo.Value;

                var lRetornoInclusao = base.ServicoRegrasRisco.SalvarTravaExposicaoSpider(lRequest);

                if (lRetornoInclusao.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lRetornoInclusao, "Trava de Exposicão atualizada com sucesso");

                    string lLog = "Trava de Exposicão atualizada com sucesso" + lRetorno;

                    base.RegistrarLogInclusao(lLog);

                    this.ResponderCarregarHtmlComDados();
                }
                else
                {
                    lRetorno = RetornarErroAjax(lRetornoInclusao.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
            }

            return lRetorno;
        }

        private string ResponderCarregarHtmlComDados()
        {
            string lRetorno = string.Empty;

            ReceberTravaExposicaoRequest lRequest = new ReceberTravaExposicaoRequest();

            var lResponse = base.ServicoRegrasRisco.ReceberTravaExposicaoSpider(lRequest);

            try
            {

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    CultureInfo lCultura = new CultureInfo("pt-BR");

                    txtPercentagem_Restricoes_PerdaMaxima.Value = lResponse.Exposicao.PrejuizoMaximo.ToString("N2", lCultura);
                    txtPrejuizo_Restricoes_MaximoAtingido.Value = lResponse.Exposicao.PrecentualOscilacao.ToString("N2", lCultura);
                    
                    lRetorno = RetornarSucessoAjax(lResponse, "Trava Exposição carregada com sucesso");

                    string lLog = "Trava Exposição carregada com sucesso : " + lRetorno;

                    base.RegistrarLogConsulta(lLog);
                }
                else
                {
                    lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao converter os dados", ex);
            }

            return lRetorno;
        }
    }
}