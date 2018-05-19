using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace BoletadorFIX
{
    public partial class frmOfertasEnviadas : Form
    {
        public OrdemInfo SelectedOrder { get; set; }
        private List<OrdemInfo> enviadas = SerializadorOfertas.LoadOfertas();

        public frmOfertasEnviadas()
        {
            InitializeComponent();

            for (int i = 0; i < enviadas.Count; i++)
            {
                OrdemInfo info = enviadas[i];
                cmbOfertas.Items.Add(info.ClOrdID + " - " + info.Symbol + " - " + info.Price);
            }

            if (cmbOfertas.Items.Count > 0 )
                cmbOfertas.SelectedIndex = 0;

        }

        public new DialogResult ShowDialog()
        {
            DialogResult result =  base.ShowDialog();

            return result;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            SelectedOrder = enviadas[cmbOfertas.SelectedIndex];
            this.Close();
        }
    }
}
