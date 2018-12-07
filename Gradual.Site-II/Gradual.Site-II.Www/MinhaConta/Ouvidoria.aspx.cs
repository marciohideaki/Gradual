using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Site.Www
{
    public partial class Ouvidoria : PaginaBase
    {
        #region Métodos Private

        private void LimparFormulario()
        {
            txtOuvidora_Nome.Value = 
            txtOuvidora_CodigoCliente.Value = 
            txtOuvidora_Assessor.Value = 
            txtOuvidora_Protocolo.Value = 
            txtOuvidora_CpfCnpj.Value = 
            txtOuvidora_Email.Value = 
            txtOuvidora_Cidade.Value  = 
            txtOuvidora_Telefone_DDD.Value = 
            txtOuvidora_Telefone_Numero.Value = 
            txtOuvidora_Mensagem.Value = "";
        }

        private void EnviarEmailParaOuvidoria()
        {
            string lMensagem;

            try
            {
                MensagemResponseStatusEnum lRetorno;

                Dictionary<string, string> lVariaveisDoEmail = new Dictionary<string, string>();

                lVariaveisDoEmail.Add("###nome###", txtOuvidora_Nome.Value);

                lVariaveisDoEmail.Add("###codigo###", txtOuvidora_CodigoCliente.Value);

                lVariaveisDoEmail.Add("###assessor###", txtOuvidora_Assessor.Value);

                lVariaveisDoEmail.Add("###protocolo###", txtOuvidora_Protocolo.Value);

                lVariaveisDoEmail.Add("###cpfcnpj###", txtOuvidora_CpfCnpj.Value);

                lVariaveisDoEmail.Add("###email###", txtOuvidora_Email.Value);

                lVariaveisDoEmail.Add("###cidadeestado###", string.Format("{0}/{1}", txtOuvidora_Cidade.Value, cboOuvidora_Estado.SelectedValue));

                lVariaveisDoEmail.Add("###telefone###", string.Format("({0}) {1}", txtOuvidora_Telefone_DDD.Value, txtOuvidora_Telefone_Numero.Value));

                lVariaveisDoEmail.Add("###mensagem###", txtOuvidora_Mensagem.Value);

                lRetorno = base.EnviarEmail(ConfiguracoesValidadas.Email_Ouvidoria, "Ouvidoria", "InvestimentosOuvidoriaInterno.html", lVariaveisDoEmail, eTipoEmailDisparo.Todos);

                if (lRetorno == MensagemResponseStatusEnum.OK)
                {
                    LimparFormulario();

                    ExibirMensagemJsOnLoad("I", "Sua mensagem foi enviada com sucesso para Ouvidoria!");
                }
                else
                {
                    lMensagem = string.Format("Resposta com erro do serviço para enviar mensagem para ouvidoria: [{0}]", lRetorno);

                    gLogger.Error(lMensagem);

                    ExibirMensagemJsOnLoad("E", "Erro ao enviar mensagem para ouvidoria", false, lMensagem);
                }
            }
            catch (Exception ex)
            {
                lMensagem = string.Format("Erro ao enviar mensagem para ouvidoria: [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                gLogger.Error(lMensagem);

                ExibirMensagemJsOnLoad("E", "Erro ao enviar mensagem para ouvidoria", false, lMensagem);
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            cboOuvidora_Estado.DataSource = ListaComSelecione(DadosDeAplicacao.Estados);
            cboOuvidora_Estado.DataBind();

            cboOuvidora_Pais.DataSource = ListaComSelecione(DadosDeAplicacao.Paises);
            cboOuvidora_Pais.DataBind();
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            PaginaMaster.BreadCrumbVisible = true;

            PaginaMaster.Crumb1Text = "Início";
            PaginaMaster.Crumb2Text = "Ouvidoria";
        }

        protected void btnOuvidoria_Enviar_Click(object sender, EventArgs e)
        {
            EnviarEmailParaOuvidoria();
        }

        #endregion
    }
}