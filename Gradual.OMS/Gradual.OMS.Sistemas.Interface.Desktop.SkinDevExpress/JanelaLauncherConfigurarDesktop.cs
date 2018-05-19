using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Gradual.OMS.Contratos.Interface.Desktop.Dados;

using DevExpress.XtraEditors;

namespace Gradual.OMS.Sistemas.Interface.Desktop.SkinDevExpress
{
    public partial class JanelaLauncherConfigurarDesktop : DevExpress.XtraEditors.XtraForm
    {
        public DesktopInfo DesktopInfo { get; set; }

        public JanelaLauncherConfigurarDesktop(DesktopInfo desktopInfoInicial)
        {
            InitializeComponent();

            this.DesktopInfo = (DesktopInfo)desktopInfoInicial.Clone();
            txtNome.Text = this.DesktopInfo.Nome;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.DesktopInfo.Nome = txtNome.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}