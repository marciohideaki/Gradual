using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradualForm.Controls
{
    public class CustomPanel: System.Windows.Forms.Panel
    {
        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.Handle != null)
            {
                this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate
                {
                    base.OnSizeChanged(e);

                });

            }

        } 
    }
}
