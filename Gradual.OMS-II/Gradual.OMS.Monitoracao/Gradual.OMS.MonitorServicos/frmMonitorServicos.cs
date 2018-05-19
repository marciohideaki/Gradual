using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.OMS.Library;

namespace Gradual.OMS.MonitorServicos
{
    public partial class frmMonitorServicos : Form
    {
        private MonitorConfig _config = null;

        public frmMonitorServicos()
        {
            InitializeComponent();
        }

        private void frmMonitorServicos_Load(object sender, EventArgs e)
        {
            _config = GerenciadorConfig.ReceberConfig<MonitorConfig>();
        }
    }
}
