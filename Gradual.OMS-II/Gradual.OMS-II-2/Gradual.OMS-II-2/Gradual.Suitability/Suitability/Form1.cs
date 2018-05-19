using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Suitability
{
    public partial class Form1 : Form
    {

        
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCarregarListaClientesSuitability_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterListaClientes();
        }

        private void btnObterListaExclusao_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterListaPerfilExclusao();
        }

        private void btnMontarForaPerfil_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().MontarForaPerfil();
        }

        private void btnVerificarBovespa_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().VerificarBovespa();
        }

        private void btnVerificarBMF_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().VerificarBMF();
        }

        private void btnVerificarBTC_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().VerificarBTC();
        }

        private void btnGerarNotificacoes_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().GerarNotificacoes();
        }

        private void btnLimparTemporarios_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().LimparDadosTemporariosOracle();
        }

        private void btnObterPosicaoFundos_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterPosicaoFundosFinancial();
            grdFinancial.DataSource = Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().CotistasFinancial;
        }

        private void btnObterPosicaoFundosItau_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterPosicaoFundosItau();
            
        }

        private void btnVerificarFundosFinancial_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().VerificarFundosFinancial();
        }

        private void btnVerificarFundosItau_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().VerificarFundosItau();
        }

        private void btnObterListaFundos_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterListaFundos();
        }

        private void btnObterListadePara_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterListaDePara();
        }

        private void btnGerarNotificacoesErro_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().GerarNotificacaoErros();
        }

        private void btnExclusao_Click(object sender, EventArgs e)
        {
            Gradual.Suitability.Service.SuitabilityMonitor.GetInstance().ObterExclusao();
        }

        
    }
}
