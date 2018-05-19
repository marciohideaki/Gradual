using System;
using System.Windows.Forms;
using Gradual.OMS.ConsolidadorRelatorioCC;
using Gradual.OMS.ConsolidadorRelatorioCCLib.Mensageria;
using System.Collections.Generic;

namespace Teste.ConsolidadorRelatorioCC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonAtivar_Click(object sender, EventArgs e)
        {
            try
            {
                var lListaConsultaCPF = new List<string>();

                lListaConsultaCPF.Add("039.339.538-39");
                lListaConsultaCPF.Add("358.276.168-18");
                lListaConsultaCPF.Add("288.364.306-78");
                lListaConsultaCPF.Add("065.994.438-34");
                lListaConsultaCPF.Add("190.682.708-77");
                lListaConsultaCPF.Add("843.984.856-00");
                lListaConsultaCPF.Add("324.391.309-82");
                lListaConsultaCPF.Add("611.468.549-00");
                lListaConsultaCPF.Add("090.003.316-92");
                lListaConsultaCPF.Add("057.779.629-12");
                lListaConsultaCPF.Add("617.421.809-06");
                lListaConsultaCPF.Add("830.547.660-04");
                lListaConsultaCPF.Add("021.024.671-55");
                lListaConsultaCPF.Add("305.690.879-53");
                lListaConsultaCPF.Add("222.926.756-68");
                lListaConsultaCPF.Add("069.380.736-94");
                lListaConsultaCPF.Add("590.936.330-15");
                lListaConsultaCPF.Add("216.287.488-18");
                lListaConsultaCPF.Add("049.190.338-34");
                lListaConsultaCPF.Add("370.924.888-42");
                lListaConsultaCPF.Add("473.711.969-53");
                lListaConsultaCPF.Add("385.058.860-20");
                lListaConsultaCPF.Add("600.798.862-72");

                new ServicoConsolidadorContaCorrente().ConsultarSaldoCCProjetado(new SaldoContaCorrenteRiscoRequest() { IdCliente = 9935 });

                this.LabelRetorno.Text = "Operação realizada com sucesso.";
            }
            catch (Exception ex) { this.LabelRetorno.Text = ex.ToString(); }
        }

        private void buttonCarregarLista_Click(object sender, EventArgs e)
        {
            try
            {
                new ServicoConsolidadorContaCorrente().AlimentarConsultaDNDelegate(null);

                this.LabelRetorno.Text = "Operação realizada com sucesso.";
            }
            catch (Exception ex) { this.LabelRetorno.Text = ex.ToString(); }
        }
    }
}
