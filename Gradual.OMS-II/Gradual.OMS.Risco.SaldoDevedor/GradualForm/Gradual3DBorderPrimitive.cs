using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;


namespace GradualForm
{
    public class Gradual3DBorderPrimitive
    {
        public enum GradualBorderStyle
        { 
            /// <summary>
            /// Simple flat style border.
            /// </summary>
            Flat,
            /// <summary>
            /// Advanced 3D border rendering.
            /// </summary>
            X3D,
        }
        public enum GradualBorderType
        {
           /// <summary>
           /// Border is rendered as a rectangular area.
           /// </summary>
           Rectangular,
           /// <summary>
           /// Upper corners are rounded. Added: 04/03/09
           /// </summary>
           Rounded,
           /// <summary>
           /// Upper corners are inclinated. Added: 06/03/09
           /// </summary>
           Inclinated
        }
        private GradualBorderType m_eBorderType = GradualBorderType.Rectangular;
        private GradualBorderStyle m_eBorderStyle = GradualBorderStyle.X3D;
        private Color m_clrFlatBorder = Color.FromArgb(52, 52, 52);
        private int m_lTitleBarHeight = 30;
        private int m_lRadius = 6;
        private int m_lInclination = 6;
        private GraphicsPath m_BorderShape = new GraphicsPath();


        private Color[] m_clrOuterBorder = new Color[]
        {
            Color.FromArgb(0, 0, 0),
            Color.FromArgb(52,52,52)
        };

        private Color[] m_clrInnerBorder = new Color[]
        {
            Color.FromArgb(34,34,34),
            Color.FromArgb(31,31,31),
            Color.FromArgb(42,42,42),
            Color.FromArgb(6,7,9)
        };

        public GradualBorderType BorderType
        {
            get
            {
                return this.m_eBorderType;
            }
            set
            {
                this.m_eBorderType = value;
            }
        }
        public GradualBorderStyle BorderStyle
        {
            get
            {
                return this.m_eBorderStyle;
            }
            set
            {
                this.m_eBorderStyle = value;
            }
        }
        public int Inclination
        {
            get
            {
                return m_lInclination;
            }
            set
            {
                if (m_lInclination > 6) 
                    m_lInclination = 6;

                this.m_lInclination = value;
            }
        }
        public int Radius
        {
            get
            {
                return this.m_lRadius;
            }
            set
            {
                if (value > 9)
                    this.m_lRadius = 9;

                this.m_lRadius = value;

            }
        }
        public Color FlatBorder
        {
            get
            {
                return this.m_clrFlatBorder;
            }
            set
            {
                this.m_clrFlatBorder = value;
            }
        }
        public Color[] OuterBorderColors
        {
            get
            {
                return this.m_clrOuterBorder;
            }
            set
            {
                this.m_clrOuterBorder = value;
            }
        }
        public Color[] InnerBorderColors
        {
            get
            {
                return this.m_clrInnerBorder;
            }
            set
            {
                this.m_clrInnerBorder = value;
            }
        }
        public int TitleBarHeight
        {
            get
            {
                return this.m_lTitleBarHeight;
            }
            set
            {
                this.m_lTitleBarHeight = value;
            }
        }
        public GraphicsPath FindX3DBorderPrimitive(Rectangle rcBorder) {
            switch (m_eBorderType){
                case GradualBorderType.Rounded:
                    m_BorderShape = GradualFormHelper.RoundRect((RectangleF)rcBorder, m_lRadius, m_lRadius, 0, 0);
                    break;
                case GradualBorderType.Inclinated:
                    m_BorderShape = createInclinatedBorderPath(rcBorder);
                    break;
            }
            return m_BorderShape;
        }

        public void Render(Rectangle rcBorder, Graphics g)
        {
            Render(rcBorder, g, false);
        }

        /// <summary>
        /// Main rendering method.
        /// </summary>
        /// <param name="rcBorder"> Border bounds</param>
        /// <param name="g"> Graphics object</param>
        public void Render(Rectangle rcBorder, Graphics g, bool EntireClient)
        {
            GraphicsPath XBorderPath = new GraphicsPath();
            switch (m_eBorderStyle)
            {
                case GradualBorderStyle.X3D:
                   switch (m_eBorderType)
                   {
                            case GradualBorderType.Rectangular:
                                using (GradualAntiAlias xaa = new GradualAntiAlias(g))
                                {
                                    DrawBorderLine(g, XBorderPath, rcBorder, 0, false, EntireClient);
                                }
                                break;
                            case GradualBorderType.Rounded:
                                DrawBorderLine(g, XBorderPath, rcBorder, m_lRadius, false, EntireClient);
                                break;
                            case GradualBorderType.Inclinated:
                                DrawBorderLine(g, XBorderPath, rcBorder, 0, false, EntireClient);
                                break;
                    }
                    break;
                case GradualBorderStyle.Flat:
                    switch (m_eBorderType)
                    {
                            case GradualBorderType.Rectangular:
                                using (GradualAntiAlias xaa = new GradualAntiAlias(g))
                                {
                                    DrawBorderLine(g, XBorderPath, rcBorder, 0, true, EntireClient);
                                }
                                break;
                            case GradualBorderType.Rounded:
                                {
                                    DrawBorderLine(g, XBorderPath, rcBorder, m_lRadius, true, EntireClient);
                                }
                                break;
                            case GradualBorderType.Inclinated:
                                {
                                    DrawBorderLine(g, XBorderPath, rcBorder, 0, false, EntireClient);
                                }
                                break;
                     }
                     break;
              }
        }
        /// <summary>
        /// Helper method for rectangle deflating.
        /// </summary>
        /// <param name="rcBorder"> Rectangle to deflate</param>
        private void DeflateRect(ref Rectangle rcBorder)
        {
            rcBorder.X += 1; rcBorder.Y += 1;
            rcBorder.Width -= 2; rcBorder.Height -= 2;
        }
        /// <summary>
        /// Draws inner & outer 3D borders.
        /// </summary>
        /// <param name="g"> Graphics object</param>
        /// <param name="XBorderPath"> Border path</param>
        /// <param name="rcBorder"> Border bounds</param>
        /// <param name="lCorner"> Radius of a rounded rectangle</param>
        /// <param name="bFlat"> Controls border type mode</param>
        private void DrawBorderLine(Graphics g, GraphicsPath XBorderPath, Rectangle rcBorder, int lCorner, bool bFlat, bool EntireClient)
        {
           int lC = lCorner;

           #region Draw outer border
           if (bFlat)
           {
               switch (m_eBorderType)
               {
                   case GradualBorderType.Rectangular:
                       XBorderPath = GradualFormHelper.RoundRect((RectangleF)rcBorder, lC, lC, lC, lC);
                       break;
                   case GradualBorderType.Rounded:
                       XBorderPath = GradualFormHelper.RoundRect((RectangleF)rcBorder, lC, lC, 0, 0);
                       break;
                   case GradualBorderType.Inclinated:
                       XBorderPath = createInclinatedBorderPath(rcBorder);
                       break;
               }
               using (Pen pFlat = new Pen(m_clrFlatBorder))
               {
                   g.DrawPath(pFlat, XBorderPath);
               }
           }
           else
           {
               for (int o = 0; o < m_clrOuterBorder.Length; o++)
               {
                   switch (m_eBorderType)
                   {
                       case GradualBorderType.Rectangular:
                           XBorderPath = GradualFormHelper.RoundRect((RectangleF)rcBorder, lC, lC, lC, lC);
                           break;
                       case GradualBorderType.Rounded:
                           XBorderPath = GradualFormHelper.RoundRect((RectangleF)rcBorder, lC, lC, 0, 0);
                           break;
                       case GradualBorderType.Inclinated:
                           XBorderPath = createInclinatedBorderPath(rcBorder);
                           break;

                   }
                   Pen pen = new Pen(m_clrOuterBorder[o]);
                   g.DrawPath(
                     pen,
                     XBorderPath
                   );
                   DeflateRect(ref rcBorder);
                   if (m_eBorderType != GradualBorderType.Rectangular)
                       lC--;
               }

           #endregion

           #region Draw inner border

               //if (EntireClient)
               //{
               //    rcBorder.Y += m_lTitleBarHeight;
               //    rcBorder.Height -= m_lTitleBarHeight;
               //}

               //for (int i = 0; i < m_clrInnerBorder.Length; i++)
               //{
               //    Pen penInner = new Pen(m_clrInnerBorder[i]);
               //    g.DrawRectangle(
               //      penInner,
               //      rcBorder
               //    );
               //    DeflateRect(ref rcBorder);
               //}
           }
           #endregion
          

        }
        private GraphicsPath createInclinatedBorderPath(Rectangle rcBorder){
            GraphicsPath i = new GraphicsPath();
            i.AddLine(rcBorder.X, rcBorder.Y + m_lInclination, rcBorder.X, rcBorder.Bottom);
            i.AddLine(rcBorder.X, rcBorder.Bottom, rcBorder.Right, rcBorder.Bottom);
            i.AddLine(rcBorder.Right, rcBorder.Bottom, rcBorder.Right, rcBorder.Top + m_lInclination);
            i.AddLine(rcBorder.Right, rcBorder.Top + m_lInclination, rcBorder.Right - m_lInclination, rcBorder.Top);
            i.AddLine(rcBorder.Right - m_lInclination, rcBorder.Top, rcBorder.Left + m_lInclination, rcBorder.Top);
            i.AddLine(rcBorder.Left + m_lInclination, rcBorder.Top, rcBorder.Left, rcBorder.Top + m_lInclination);

            return i;
        }
    
    
    }
}
