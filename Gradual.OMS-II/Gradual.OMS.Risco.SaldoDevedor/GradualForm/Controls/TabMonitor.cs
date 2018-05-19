using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradualForm.Controls
{
    public class TabMonitor : System.Windows.Forms.TabPage
    {
        public System.String Instrumento { get; set; }
        public Gradual.OMS.CadastroCliente.Lib.CarteiraInfo Carteira { get; set; }
        public bool Controlado { get; set; }

        public TabMonitor()
        {
            this.Instrumento = String.Empty;
        }

        public TabMonitor(System.String Instrumento)
        {
            this.Instrumento = Instrumento;
            this.Text = Instrumento;

            if (Controlado)
            {
                this.BackColor = System.Drawing.Color.Black;
                this.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                this.BackColor = System.Drawing.Color.MidnightBlue;
                this.ForeColor = System.Drawing.Color.White;
            }
        }

        public TabMonitor(System.String Instrumento, System.String Descricao)
        {
            this.Instrumento = Instrumento;
            this.Text = Descricao;
            this.BackColor = System.Drawing.Color.Black;
            this.ForeColor = System.Drawing.Color.White;
        }

        public TabMonitor(System.String Instrumento, System.String Descricao, bool Controlado)
        {
            this.Controlado = Controlado;
            this.Instrumento = Instrumento;
            this.Text = Instrumento;

            if (Controlado)
            {
                this.BackColor = System.Drawing.Color.Black;
                this.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                this.BackColor = System.Drawing.Color.MidnightBlue;
                this.ForeColor = System.Drawing.Color.White;
            }
        }

    }
}
