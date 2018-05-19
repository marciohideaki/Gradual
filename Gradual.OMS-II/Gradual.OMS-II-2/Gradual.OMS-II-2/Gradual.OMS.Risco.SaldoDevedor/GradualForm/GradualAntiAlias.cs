using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace GradualForm
{
    internal class GradualAntiAlias : IDisposable
    {

        private Graphics m_g;
        private SmoothingMode m_eMode;
        
        public GradualAntiAlias(Graphics g)
        {
            m_g = g;
            m_eMode = g.SmoothingMode;
            m_g.SmoothingMode = SmoothingMode.AntiAlias;
        }
        
        public void Dispose()
        {
            m_g.SmoothingMode = m_eMode;
        }

        
    }
}
