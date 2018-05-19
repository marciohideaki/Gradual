using System;
using System.Windows.Forms;
using Gradual.MinhaConta.Servico.ImportacaoFundos;

namespace Gradual.MinhaConta.Servico.ImportacaoFundos.Teste
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonIniciarImportacaoFundos_Click(object sender, EventArgs e)
        {
            new ImportacaoClubesEFundos().ProcessarArquivosFundos();
        }

        private void buttonIniciarImportacaoClubes_Click(object sender, EventArgs e)
        {
            new ImportacaoClubesEFundos().ProcessarArquivosClubes();
        }
    }
}
