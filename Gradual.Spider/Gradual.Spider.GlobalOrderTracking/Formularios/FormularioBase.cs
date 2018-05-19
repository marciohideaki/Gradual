using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gradual.Spider.GlobalOrderTracking
{
    public partial class FormularioBase : GradualForm.GradualForm
    {
        public FormularioBase()
        {
            InitializeComponent();

            this.HasMenuIcon = false;
            GradualForm.StyleSettings.CarregarSkin("Gradual.GlobalOrderTracking");
            //GradualForm.StyleSettings.CarregarSkin("MissionControl");
            GradualForm.Engine.ConfigureFormRender(this);
        }

        public override void Form_HandleCreated(object sender, EventArgs e)
        {
        }
    }
}
