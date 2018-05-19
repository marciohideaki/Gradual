using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MigrationService
{
    public partial class frmTester : Form
    {
        public frmTester()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //int lCodigoAssessor = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CodigoAssessor"]);
            Gradual.MigrationService.Service service = new Gradual.MigrationService.Service();
            service.Migrate();
        }
    }
}
