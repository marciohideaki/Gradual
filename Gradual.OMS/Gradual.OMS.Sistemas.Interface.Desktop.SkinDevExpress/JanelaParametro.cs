using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gradual.OMS.Sistemas.Interface.Desktop.SkinDevExpress
{
    [Serializable]
    public class JanelaParametro
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public FormWindowState WindowState { get; set; }
        public bool MostrarToolbar { get; set; }
        public string Titulo { get; set; }

        public JanelaParametro()
        {
            this.MostrarToolbar = false;
        }
    }
}
