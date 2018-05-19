using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StockMarket.Excel2007
{
    public partial class frmInputBox_SelecionarEstilo : Form
    {
        #region Propriedades

        public byte EstiloSelecionado { get; set; }

        private string _TipoDeEstiloParaSelecionar;

        public string TipoDeEstiloParaSelecionar
        {
            get
            {
                return _TipoDeEstiloParaSelecionar;
            }

            set
            {
                _TipoDeEstiloParaSelecionar = value;

                if (_TipoDeEstiloParaSelecionar.ToLower() == "monitor")
                {
                    pnlEstilo_MonitorCotacao.Visible = true;
                    pnlEstilo_TickerCotacao.Visible = false;
                    pnlEstilo_LivroOfertas.Visible = false;
                }

                if (_TipoDeEstiloParaSelecionar.ToLower() == "ticker")
                {
                    pnlEstilo_MonitorCotacao.Visible = false;
                    pnlEstilo_TickerCotacao.Visible = true;
                    pnlEstilo_LivroOfertas.Visible = false;
                }

                if (_TipoDeEstiloParaSelecionar.ToLower() == "livro")
                {
                    pnlEstilo_MonitorCotacao.Visible = false;
                    pnlEstilo_TickerCotacao.Visible = false;
                    pnlEstilo_LivroOfertas.Visible = true;
                }
            }
        }

        #endregion

        #region Métodos Private

        private void SelecionarEFechar(byte pEstilo)
        {
            this.EstiloSelecionado = pEstilo;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        #region Event Handlers

        public frmInputBox_SelecionarEstilo()
        {
            InitializeComponent();

            pnlEstilo_TickerCotacao.Location = pnlEstilo_MonitorCotacao.Location;
            pnlEstilo_LivroOfertas.Location = pnlEstilo_MonitorCotacao.Location;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void picEstilo_MonitorCotacao_01_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(1);
        }

        private void picEstilo_MonitorCotacao_02_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(2);
        }

        private void picEstilo_MonitorCotacao_03_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(3);
        }

        private void picEstilo_MonitorCotacao_04_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(4);
        }

        private void picEstilo_MonitorCotacao_05_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(5);
        }

        private void picEstilo_MonitorCotacao_06_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(6);
        }

        private void picEstilo_TickerCotacao_01_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(1);
        }

        private void picEstilo_TickerCotacao_02_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(2);
        }

        private void picEstilo_TickerCotacao_03_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(3);
        }

        private void picEstilo_TickerCotacao_04_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(4);
        }

        private void picEstilo_TickerCotacao_05_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(5);
        }

        private void picEstilo_TickerCotacao_06_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(6);
        }

        private void picEstilo_LivroOferta_01_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(1);
        }

        private void picEstilo_LivroOferta_02_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(2);
        }

        private void picEstilo_LivroOferta_03_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(3);
        }

        private void picEstilo_LivroOferta_04_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(4);
        }

        private void picEstilo_LivroOferta_05_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(5);
        }

        private void picEstilo_LivroOferta_06_Click(object sender, EventArgs e)
        {
            SelecionarEFechar(6);
        }

        #endregion

    }
}
