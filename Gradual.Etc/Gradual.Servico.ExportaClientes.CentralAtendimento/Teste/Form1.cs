using System;
using System.Windows.Forms;
using Gradual.Cadastro.Exportacao;

namespace Teste
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region | Propriedades

        private string getBancoSql
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["BancoSql"].ToString();
            }
        }

        private string getBancoOracle
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["BancoOracle"].ToString();
            }
        }

        private string getCaminhoCliente
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ArquivoClientes"].ToString();
            }
        }

        private string getCaminhoAssessor
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ArquivoAssessores"].ToString();
            }
        }

        private int getHora
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["Schedule"].ToString());
            }
        }

        #endregion

        #region | Eventos

        private void btnExportarAssessores_Click(object sender, EventArgs e)
        {
            try
            {
                bool lRealizarGeracaoDoArquivo = !DayOfWeek.Saturday.Equals(DateTime.Today.DayOfWeek)
                              && !DayOfWeek.Sunday.Equals(DateTime.Today.DayOfWeek)
                              && DateTime.Now.Hour >= 6
                              && DateTime.Now.Hour <= 20;

                if (lRealizarGeracaoDoArquivo)
                    new Exporta().ExportarAssessores(getCaminhoAssessor, getBancoOracle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExportarClientes_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                new Exporta().ExportarClientes(getCaminhoCliente, getBancoSql, getBancoOracle);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion
    }
}
