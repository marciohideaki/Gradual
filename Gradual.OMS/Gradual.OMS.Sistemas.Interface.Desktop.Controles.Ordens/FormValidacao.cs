using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Mensagens;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Ordens
{
    public partial class FormValidacao : XtraForm
    {
        private ValidarOperacaoResponse _validarResponse = null;

        public FormValidacao(ValidarOperacaoResponse validarResponse)
        {
            InitializeComponent();
            
            _validarResponse = validarResponse;
            this.Load += new EventHandler(FormValidacao_Load);
            this.grdv.DoubleClick += new EventHandler(grdv_DoubleClick);
        }

        private void grdv_DoubleClick(object sender, EventArgs e)
        {
            object critica = grdv.GetRow(grdv.FocusedRowHandle);
            if (critica != null)
                new FormMensagem("Detalhe da Crítica", critica).ShowDialog();

        }

        private void FormValidacao_Load(object sender, EventArgs e)
        {
            if (_validarResponse.Validacao.ContextoValidacao.MensagemValida)
            {
                lbl.Text = "Operação Válida";
                lbl.BackColor = Color.FromArgb(192, 255, 192);
            }
            else
            {
                lbl.Text = "Operação Inválida";
                lbl.BackColor = Color.FromArgb(255, 192, 192);
            }
            grd.DataSource = _validarResponse.Validacao.Criticas;
        }

        private void cmdSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
