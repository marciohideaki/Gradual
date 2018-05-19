using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gradual.OMS.Host.Windows.Teste
{
    public partial class frmObjeto : Form
    {
        public frmObjeto(object objeto)
        {
            InitializeComponent();

            ppg.SelectedObject = objeto;
        }
    }
}
