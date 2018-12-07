using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Gradual.Site.Www.MinhaConta.Produtos.Fundos
{
    public partial class Operacoes : PaginaFundos
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.ValidarSessao())
            {
                if (!this.IsPostBack)
                {
                    if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                    {
                        base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                        return;
                    }

                    base.RodarJavascriptOnLoad("MinhaConta_GerarGrafico_Fundos_ExtratoMensal(); ");
                }
                else
                {
                    if (this.Posicao_Aba_Simular_Selecionada.Value == "AbaSimular")
                    {
                        base.RodarJavascriptOnLoad("MinhaConta_Operacoes_Fundos_Simular_LoadAccordeon_Click('" + this.Posicao_Aba_Simular_Selecionada.Value + "');");
                        base.RodarJavascriptOnLoad("MinhaConta_GerarGrafico_Fundos_ExtratoMensal();");
                        //base.RodarJavascriptOnLoad("MinhaConta_GerarGrafico_Fundos_ExtratoMensal();MinhaConta_GerarGrafico_Fundos_Simular(); ");
                    }
                    else if (this.Posicao_Aba_Simular_Selecionada.Value == "AbaRelatorios")
                    {
                        base.RodarJavascriptOnLoad("MinhaConta_Operacoes_Fundos_Relatorios_LoadAccordeon_Click('" + this.Posicao_Aba_Simular_Selecionada.Value + "');");
                        base.RodarJavascriptOnLoad("MinhaConta_GerarGrafico_Fundos_ExtratoMensal();");
                    }
                }
            }
        }
    }
}