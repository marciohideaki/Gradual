using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.Library.Servicos;
using Gradual.SaldoDevedor.lib;
using Gradual.SaldoDevedor.lib.Info;
using Gradual.SaldoDevedor.lib.Mensagens;

namespace Gradual.SaldoDevedor.WinApp
{
    public partial class frmConfiguracao : GradualForm.GradualForm
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IFormatProvider culture = new System.Globalization.CultureInfo("pt-BR", true);

        private List<int> ListaExcecaoAssessor = new List<int>();
        private List<int> ListaExcecaoCliente = new List<int>();
        private decimal LimiteSaldoDisponivel = 0;
        private decimal TaxaJuros = 0;
        private int CodigoArquivoTesouraria = 0;

        public event EventHandler<GravaConfiguracaoEventArgs> GravaConfiguracao;

        public frmConfiguracao()
        {
            InitializeComponent();
            GradualForm.StyleSettings.CarregarSkin("Gradual.Consulta");
            GradualForm.Engine.ConfigureFormRender(this);

            ObterParametros();
        }

        private void OnGravaConfiguracao()
        {
            if (GravaConfiguracao != null)
            {
                GravaConfiguracao(this, new GravaConfiguracaoEventArgs());
            }
        }

        private void ObterParametros()
        {
            try
            {
                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                ParametroResponse par = serv.ObterParametros();

                LimiteSaldoDisponivel = par.Parametro.LimiteSaldoMulta;
                txtLimiteSaldo.Text = LimiteSaldoDisponivel.ToString("N0");

                TaxaJuros = par.Parametro.TaxaJuros;
                txtTaxaJuros.Text = ((TaxaJuros - 1) * 100).ToString("N0");

                CodigoArquivoTesouraria = par.Parametro.CodigoArquivoTesouraria;
                txtCodArqTesouraria.Text = CodigoArquivoTesouraria.ToString();

                ListaExcecaoAssessor = par.Parametro.ListaExcecaoAssessor;
                foreach (int item in ListaExcecaoAssessor)
                    lstAssessoresIncluidos.Items.Add(item);

                ListaExcecaoCliente = par.Parametro.ListaExcecaoCliente;
                foreach (int item in ListaExcecaoCliente)
                    lstClientesIncluidos.Items.Add(item);
            }
            catch (Exception ex)
            {
                string msg = "Obter Parâmetros: " + ex.Message;
                MessageBox.Show(msg, "Configuração", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFecharConfig_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSalvarConfiguracoes_Click(object sender, EventArgs e)
        {
            try
            {
                ParametroRequest request = new ParametroRequest();
                request.Parametro = new ParametroInfo();

                bool tryParseOk = false;
                decimal decimalOut = 0;
                int integerOut = 0;

                tryParseOk = decimal.TryParse(txtLimiteSaldo.Text, out decimalOut);
                if (!tryParseOk)
                {
                    MessageBox.Show("Limite do Saldo Disponível inválido.", "Configuração",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                request.Parametro.LimiteSaldoMulta = decimalOut;

                tryParseOk = decimal.TryParse(txtTaxaJuros.Text, out decimalOut);
                if (!tryParseOk)
                {
                    MessageBox.Show("Taxa de Juros inválido.", "Configuração",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                request.Parametro.TaxaJuros = 1 + (decimalOut / 100);

                tryParseOk = Int32.TryParse(txtCodArqTesouraria.Text, out integerOut);
                if (!tryParseOk)
                {
                    MessageBox.Show("Código do Arquivo Tesouraria inválido.", "Configuração",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                request.Parametro.CodigoArquivoTesouraria = integerOut;

                request.Parametro.ListaExcecaoAssessor = new List<int>();
                foreach (object item in lstAssessoresIncluidos.Items)
                    request.Parametro.ListaExcecaoAssessor.Add((int)item);

                request.Parametro.ListaExcecaoCliente = new List<int>();
                foreach (object item in lstClientesIncluidos.Items)
                    request.Parametro.ListaExcecaoCliente.Add((int)item);

                ISaldoDevedor serv = Ativador.Get<ISaldoDevedor>();

                ParametroResponse resp = serv.GravarParametros(request);
                if (resp.Retorno == ParametroResponse.RETORNO_OK)
                {
                    MessageBox.Show("Configuração gravada com sucesso!", "Configuração",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Falha na gravação da Configuração!", "Configuração",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string msg = "Salvar Configurações: " + ex.Message;
                MessageBox.Show(msg, "Configuração", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            OnGravaConfiguracao();
            this.Close();
        }

        private void btnExcluirAssessor_Click(object sender, EventArgs e)
        {
            if (lstAssessoresIncluidos.SelectedIndex == -1)
            {
                MessageBox.Show("Favor selecionar um Assessor para excluir", "Configuração", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            lstAssessoresIncluidos.Items.RemoveAt(lstAssessoresIncluidos.SelectedIndex);
        }

        private void txtIncluirAssessor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtIncluirAssessor.Text.Length < 1)
                    return;
                int assessor = 0;
                int.TryParse(txtIncluirAssessor.Text, out assessor);
                if (assessor > 0 && !lstAssessoresIncluidos.Items.Contains(assessor))
                {
                    lstAssessoresIncluidos.Items.Add(assessor);
                    txtIncluirAssessor.Text = "";
                }
            }
        }

        private void btnExcluirCliente_Click(object sender, EventArgs e)
        {
            if (lstClientesIncluidos.SelectedIndex == -1)
            {
                MessageBox.Show("Favor selecionar um Cliente para excluir", "Configuração",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            lstClientesIncluidos.Items.RemoveAt(lstClientesIncluidos.SelectedIndex);
        }

        private void txtIncluirCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtIncluirCliente.Text.Length < 1)
                    return;
                int cliente = 0;
                int.TryParse(txtIncluirCliente.Text, out cliente);
                if (cliente > 0 && !lstClientesIncluidos.Items.Contains(cliente))
                {
                    lstClientesIncluidos.Items.Add(cliente);
                    txtIncluirCliente.Text = "";
                }
            }
        }
    }

    public class GravaConfiguracaoEventArgs : EventArgs
    {
        public GravaConfiguracaoEventArgs()
        { }
    }
}
