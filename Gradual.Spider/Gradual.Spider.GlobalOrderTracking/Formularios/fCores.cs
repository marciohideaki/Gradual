using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gradual.Spider.GlobalOrderTracking.Formularios
{
    public partial class fCores : GradualForm.GradualForm
    {
		bool isSelecting;
        bool ExisteAlteracao = false;

        private static Dictionary<string, System.Drawing.Color> lDefaultCoresStatus = new Dictionary<string, System.Drawing.Color>();

		public fCores()
		{
			InitializeComponent();

            lDefaultCoresStatus = Aplicacao.CoresStatus;

            if (Parametros.lListaOrderStatus != null)
            {
                if (Parametros.lListaOrderStatus.Count > 0)
                {

                    foreach (Gradual.Spider.Servicos.Configuracoes.Lib.Classes.Parametro lParametro in Parametros.lListaOrderStatus)
                    {
                        listBox1.Items.Add(lParametro.Description);
                    }
                }
            }
		}

		private void fCores_Load(object sender, EventArgs e)
		{
			isSelecting = false;
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
            //Bitmap bmpImage = (Bitmap)pictureBox1.Image;
            //if (e.X > 0 && e.Y > 0)
            //{
            //    Color clr = bmpImage.GetPixel(e.X, e.Y);
            //    label1.BackColor = clr;
            //    Double _media = (clr.R + clr.G + clr.B) / 3;
            //    if (_media > 128)
            //    {
            //        label1.ForeColor = Color.Black;
            //    }
            //    else
            //    {
            //        label1.ForeColor = Color.White;
            //    }

            //    label1.BackColor = clr;
            //}
		}


		private void listBox1_Click(object sender, EventArgs e)
		{
			label1.ForeColor = Color.White;
			label1.Text = String.Format("Exemplo: {0}",listBox1.Items[listBox1.SelectedIndex].ToString());
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
            Bitmap bmpImage = (Bitmap)pictureBox1.Image;
            if (mouseDownLocation.X > 0 && mouseDownLocation.Y > 0)
            {
                Color clr = bmpImage.GetPixel(mouseDownLocation.X, mouseDownLocation.Y);
                label1.BackColor = clr;
                Double _media = (clr.R + clr.G + clr.B) / 3;
                if (_media > 128)
                {
                    label1.ForeColor = Color.Black;
                }
                else
                {
                    label1.ForeColor = Color.White;
                }

                label1.BackColor = clr;
            }

			if (listBox1.SelectedIndex >= 0)
			{
                String _status = listBox1.Items[listBox1.SelectedIndex].ToString();

                if (!Aplicacao.CoresStatus.ContainsKey(_status))
                {
                    Aplicacao.CoresStatus.Add(_status, label1.BackColor);
                }
                else
                {
                    Aplicacao.CoresStatus[_status] = label1.BackColor;
                }
                ExisteAlteracao = true;
			}
		}

		private void btnRedefinir_Click(object sender, EventArgs e)
		{
			Aplicacao.CoresStatus.Clear();
            ExisteAlteracao = true;
		}

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ExisteAlteracao)
            {
                Aplicacao.AlterarCores(this, null);
            }
        }

        Point mouseDownLocation;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownLocation = new Point(e.X, e.Y);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExisteAlteracao = false;
            Aplicacao.CoresStatus = lDefaultCoresStatus;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Aplicacao.CoresStatus = new Dictionary<string, Color>();
            ExisteAlteracao = true;
        }

    }
}
