using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    /// <summary>
    /// Classe de auxilio para salvar e restaurar uma janela
    /// </summary>
    [Serializable]
    public class LayoutJanelaHelper
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public FormWindowState State { get; set; }

        public void SalvarJanela(Form janela)
        {
            this.State = janela.WindowState;
            if (janela.WindowState != FormWindowState.Normal)
                janela.WindowState = FormWindowState.Normal;
            this.Width = janela.Width;
            this.Height = janela.Height;
            this.Left = janela.Left;
            this.Top = janela.Top;
        }

        public void RecuperarJanela(Form janela)
        {
            if (this.Width != 0)
            {
                janela.Top = this.Top;
                janela.Left = this.Left;
                janela.Height = this.Height;
                janela.Width = this.Width;
                janela.WindowState = this.State;
            }
        }
    }
}
