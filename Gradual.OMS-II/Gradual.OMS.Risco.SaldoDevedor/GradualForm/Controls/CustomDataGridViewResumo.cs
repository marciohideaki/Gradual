using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GradualForm.Controls
{
    public partial class CustomDataGridViewResumo : CustomDataGridView
    {
        public CustomDataGridViewResumo()
        {
            InitializeComponent();
        }

        public CustomDataGridViewResumo(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
