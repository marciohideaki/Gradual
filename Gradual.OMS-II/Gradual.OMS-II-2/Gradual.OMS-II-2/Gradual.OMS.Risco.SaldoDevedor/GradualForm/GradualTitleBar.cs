using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;


namespace GradualForm
{
    public class GradualTitleBar
    {

        private List<Color> m_TitleBarMix = new List<Color>();
        
        private Color m_clrFillStart = Color.FromArgb(60, 60, 60);
        private Color m_clrFillEnd = Color.FromArgb(42, 42, 42);

        private Color m_clrUpperFillStart  = Color.FromArgb(147,147,147);
        private Color m_clrUpperFillEnd = Color.FromArgb(93, 93, 93);

        private string m_sCaption = "";

        private Font m_fntCaption = new Font("Visitor TT2 BRK", 9);
        
        private Color m_clrCaptionFont = Color.White;

        private Color m_clrButtonBoxStart = Color.FromArgb(134, 125, 126);
        private Color m_clrButtonBoxEnd = Color.FromArgb(134, 125, 126);
        private Color m_clrButtonBoxInner = Color.FromArgb(29, 8, 5);
        private Color m_clrButtonBoxOuter = Color.FromArgb(60, 65, 68);
        private List<Color> m_ButtonBoxColors = new List<Color>();

        private Color m_clrOuterTitleBarColor = Color.FromArgb(0, 0, 0);
        private Color m_clrInnerTitleBarColor = Color.FromArgb(52, 52, 52);
        private Image m_TitleBarTexture;
        private Image m_TitleBarBackImage;

        private bool m_bShouldRenderButtonBox = true;
        
        public bool ShouldDrawButtonBox
        {
            get
            {
                return this.m_bShouldRenderButtonBox;
            }
        }
        public enum GradualTitleBarType
        { 
            Rounded,
            Angular,
            Rectangular
        }
        private GradualTitleBarType m_eTitleBarType = GradualTitleBarType.Rectangular;
        
        private List<GradualTitleBarButton> m_xtbButtons = new List<GradualTitleBarButton>();

        public enum GradualTitleBarBackImageAlign
        {
            /// <summary>
            /// Left aligned image.
            /// </summary>
            Left,
            /// <summary>
            /// Middle aligned image.
            /// </summary>
            Center,
            /// <summary>
            /// Rigth aligned image.
            /// </summary>
            Right
        }
        private GradualTitleBarBackImageAlign m_eTitleBarImageAlign = GradualTitleBarBackImageAlign.Right;

        public enum GradualTitleBarFill
        {   
            /// <summary>
            /// Just renders titlebar shape, without fill.
            /// </summary>
            None,
            /// <summary>
            /// Titlebar is rendering using linear gradient.
            /// </summary>
            LinearRendering,
            /// <summary>
            /// Titlebar is rendered using gradient mix and glow.
            /// </summary>
            AdvancedRendering,
            /// <summary>
            /// Titlebar is rendered with upper glow.
            /// </summary>
            UpperGlow,
            /// <summary>
            /// Titlebar is filled using two gradient rectangles.
            /// </summary>
            RectangleRendering,
            /// <summary>
            /// Titlebar is rendered using custom texture.
            /// </summary>
            Texture
        }
        public GradualTitleBarFill m_eTitleBarFill = GradualTitleBarFill.AdvancedRendering;
        
        public enum GradualButtonBoxFill
        {   
            /// <summary>
            /// Rendered using color mix.
            /// </summary>
            ColorMix,
            /// <summary>
            /// Rendered using titlebar start / end rectangle colors.
            /// </summary>
            TitleBarRectangleRendering
        }
        
        public GradualButtonBoxFill m_eButtonBoxFill = GradualButtonBoxFill.ColorMix;
        
        public GradualButtonBoxFill ButtonBoxFill
        {
            get
            {
                return this.m_eButtonBoxFill;
            }
            set
            {
                this.m_eButtonBoxFill = value;
            }
        }
        public GradualTitleBarBackImageAlign TitleBarImageAlign
        {
            get
            {
                return this.m_eTitleBarImageAlign;
            }
            set
            {
                this.m_eTitleBarImageAlign = value;
            }
        }
        public GradualTitleBarFill TitleBarFill
        {
            get
            {
                return this.m_eTitleBarFill;
            }
            set
            {
                this.m_eTitleBarFill = value;
            }
        }
        public List<Color> TitleBarMixColors
        {
            get
            {
                return this.m_TitleBarMix;
            }
            set
            {
                this.m_TitleBarMix = value;
            }
        }

        public Color ButtonBoxInnerBorder
        {
            get
            {
                return this.m_clrButtonBoxInner;
            }
            set
            {
                this.m_clrButtonBoxInner = value;
            }
        
        }
        public Color ButtonBoxOuterBorder
        {
            get
            {
                return this.m_clrButtonBoxOuter;
            }
            set
            {
                this.m_clrButtonBoxOuter = value;
            }

        }
        public List<Color> ButtonBoxMixColors
        {
            get
            {
                return this.m_ButtonBoxColors;
            }
            set
            {
                this.m_ButtonBoxColors = value;
            }
        }

        public List<GradualTitleBarButton> TitleBarButtons
        {
            get
            {
                return this.m_xtbButtons;
            }
        }
        public GradualTitleBarType TitleBarType
        {
            get
            {
                return this.m_eTitleBarType;
            }
            set
            {
                this.m_eTitleBarType = value;
               
            }
        }
 
        public Color GlowFillStart
        {
            get
            {
                return this.m_clrUpperFillStart;
            }
            set
            {
                this.m_clrUpperFillStart = value;
            }
        }
        public Color GlowFillEnd
        {
            get
            {
                return this.m_clrUpperFillEnd;
            }
            set
            {
                this.m_clrUpperFillEnd = value;
            }
        }
        public string TitleBarCaption
        {
            get
            {
                return this.m_sCaption;
            }
            set
            {

                //TODO: repaint needed
                this.m_sCaption = value;
            }
        }
        public Font TitleBarCaptionFont
        {
            get
            {
                return this.m_fntCaption;
            }
            set
            {
                this.m_fntCaption = value;
            }
        }
        public Color TitleBarCaptionColor
        {
            get
            {
                return this.m_clrCaptionFont;
            }
            set
            {
                this.m_clrCaptionFont = value;
            }
        }
        public Color OuterTitleBarColor
        {
            get
            {
                return this.m_clrOuterTitleBarColor;
            }
            set
            {
                this.m_clrOuterTitleBarColor = value;
            }
        }
        public Color InnerTitleBarColor
        {
            get
            {
                return this.m_clrInnerTitleBarColor;
            }
            set
            {
                this.m_clrInnerTitleBarColor = value;
            }
        }
        public Image TitleBarBackImage
        {
            get
            {
                return this.m_TitleBarBackImage;
            }
            set
            {
                this.m_TitleBarBackImage = value;
            }
        }
        public Image TitleBarTexture
        {
            get
            {
                return this.m_TitleBarTexture;
            }
            set
            {
                this.m_TitleBarTexture = value;
            }
        }
        public GradualTitleBar()
        {
            
            // initialize button box mix colors:
            m_ButtonBoxColors.Add(Color.FromArgb(112, 106, 108));
            m_ButtonBoxColors.Add(Color.FromArgb(56, 52, 53));
            m_ButtonBoxColors.Add(Color.FromArgb(53, 49, 50));
            m_ButtonBoxColors.Add(Color.FromArgb(71, 71, 71));
            m_ButtonBoxColors.Add(Color.FromArgb(112, 106, 108));

            // initialize titlebar mix colors: 
            m_TitleBarMix.Add(Color.FromArgb(245, 245, 245));
            m_TitleBarMix.Add(Color.FromArgb(93, 93, 93));
            m_TitleBarMix.Add(Color.FromArgb(45, 45, 45));
            m_TitleBarMix.Add(Color.FromArgb(30, 30, 30));
            m_TitleBarMix.Add(Color.FromArgb(52, 52, 52));
        }

        public void RenderTitleBar(Graphics g, Rectangle rcTb)
        {
            FillTitleBar(g, rcTb);
            rcTb = new Rectangle(rcTb.X + 5, rcTb.Y, rcTb.Width - 5, rcTb.Height);
            DrawTitleBarText(g, m_clrCaptionFont, m_sCaption, rcTb);
        }


        /// <summary>
        /// Draws titlebar caption.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clrCaption"></param>
        /// <param name="sCaption"></param>
        /// <param name="rc"></param>
        private void DrawTitleBarText(
            Graphics g,
            Color clrCaption,
            string sCaption,
            Rectangle rc
            )
        {
            StringFormat sf = new StringFormat();
            //sf.Alignment = StringAlignment.Center;
            //sf.LineAlignment = StringAlignment.Center;
            
            //sf.Alignment = StringAlignment.Near;
            //sf.LineAlignment = StringAlignment.Near;
            //sf.Trimming = StringTrimming.EllipsisCharacter;

            sf.LineAlignment = StringAlignment.Near;
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Near;

            SolidBrush sb = new SolidBrush(clrCaption);
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.DrawString(
              sCaption,
              m_fntCaption,
              sb,
              rc,
              sf);

            sb.Dispose();
            sf.Dispose();


        }
        /// <summary>
        /// Draws outer border for titlebar area.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rcTitleBar"></param>
        /// <param name="clrOuterBorder"></param>
        private void DrawOuterTitleBarBorder(Graphics g, Rectangle rcTitleBar, Color clrOuterBorder)
        {
            using (GraphicsPath GradualTitleBarPath = BuildTitleBarShape(rcTitleBar))
            {
                using (Pen pOuter = new Pen(clrOuterBorder))
                {
                    g.DrawPath(
                      pOuter,
                      GradualTitleBarPath
                    );
                }
            }
        }
        /// <summary>
        /// Draws inner border for titlebar area.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rcTitleBar"></param>
        /// <param name="clrInnerBorder"></param>
        private void DrawInnerTitleBarBorder(Graphics g, Rectangle rcTitleBar, Color clrInnerBorder)
        {

            if (m_eTitleBarType == GradualTitleBarType.Rectangular)
                rcTitleBar.Inflate(-1, -1);
            else{
                rcTitleBar.X += 1;
                rcTitleBar.Inflate(0, -1);
            }
            using (GraphicsPath GradualTitleBarPath = BuildTitleBarShape(rcTitleBar))
            {
                using (Pen pInner = new Pen(clrInnerBorder))
                {
                    g.DrawPath(
                      pInner,
                      GradualTitleBarPath
                    );
                }
            }
        }

        private void FillTitleBar(Graphics g, Rectangle rcTitleBar)
        {
            GraphicsPath GradualTitleBarPath = new GraphicsPath();
            GradualTitleBarPath = BuildTitleBarShape(rcTitleBar);
            using (GradualAntiAlias xaa = new GradualAntiAlias(g))
            {

                    #region Fill titlebar 
                    switch (m_eTitleBarFill)
                    {
                        case GradualTitleBarFill.AdvancedRendering:
                           using (LinearGradientBrush lgb = new LinearGradientBrush(rcTitleBar,m_TitleBarMix[0], m_TitleBarMix[4], LinearGradientMode.Vertical))
                            {
                                lgb.InterpolationColors = GradualFormHelper.ColorMix(m_TitleBarMix, true);
                                g.FillPath(lgb, GradualTitleBarPath);
                            }
                            

                            #region Draw titlebar glow
                            using (GraphicsPath XGlow = new GraphicsPath())
                            {
                                XGlow.AddEllipse(rcTitleBar.Left, rcTitleBar.Bottom / 2 + 4, rcTitleBar.Width, rcTitleBar.Height);

                                using (PathGradientBrush pgb = new PathGradientBrush(XGlow))
                                {
                                    pgb.CenterColor = Color.White;
                                    pgb.SurroundColors = new Color[] { Color.FromArgb(0, 229, 121, 13) };

                                    g.SetClip(GradualTitleBarPath);
                                    g.FillPath(pgb, XGlow);
                                    g.ResetClip();

                                }
                            }
                            #endregion
                             
                             break;
                          case GradualTitleBarFill.Texture:
                              if (m_TitleBarTexture != null) 
                              {                                  
                                  using (TextureBrush tb = new TextureBrush(m_TitleBarTexture))
                                  {
                                      
                                      g.FillPath(tb, GradualTitleBarPath);
                                  }
                              }
                             break;
                         case GradualTitleBarFill.LinearRendering:
                            RectangleF rcLinearFill = GradualTitleBarPath.GetBounds();
                            g.SetClip(GradualTitleBarPath);
                            using (LinearGradientBrush lgbLinearFill = new LinearGradientBrush(
                                  rcLinearFill,
                                  m_clrFillStart,
                                  m_clrFillEnd,
                                  LinearGradientMode.Vertical
                                  ))
                            {
                                
                                g.FillRectangle(
                                  lgbLinearFill,
                                  rcLinearFill
                                );
                            }
                            
                            g.ResetClip();
                            break;
                        case GradualTitleBarFill.UpperGlow:
                            RectangleF rcGlow = GradualTitleBarPath.GetBounds();
                            g.SetClip(GradualTitleBarPath);
                            rcGlow.Height /= 2;
                            using (LinearGradientBrush lgbUpperGlow = new LinearGradientBrush(
                                  rcGlow,
                                  m_clrUpperFillStart,
                                  m_clrUpperFillEnd,
                                  LinearGradientMode.Vertical
                                  ))
                            {
                                
                                g.FillRectangle(
                                  lgbUpperGlow,
                                  rcGlow
                                );
                            }

                            g.ResetClip();
                            break;
                        case GradualTitleBarFill.RectangleRendering:
                            RectangleF rcDownRect = GradualTitleBarPath.GetBounds();
                            RectangleF rcUpRect = GradualTitleBarPath.GetBounds();
                            g.SetClip(GradualTitleBarPath);
                            rcUpRect.Height /= 2;
                            using (LinearGradientBrush lgbUpperRect = new LinearGradientBrush(
                                  rcUpRect,
                                  m_clrUpperFillStart,
                                  m_clrUpperFillEnd,
                                  LinearGradientMode.Vertical
                                  ))
                            {

                                lgbUpperRect.WrapMode = WrapMode.TileFlipY;
                                g.FillRectangle(
                                  lgbUpperRect,
                                  rcUpRect
                                );
                            }

                            rcDownRect.Height = rcDownRect.Height / 2;
                            rcDownRect.Y = rcUpRect.Bottom;
                            using (LinearGradientBrush lgbDwnRect = new LinearGradientBrush(
                                  rcDownRect,
                                  m_clrFillStart,
                                  m_clrFillEnd,
                                  LinearGradientMode.Vertical
                                  ))
                            {

                                g.FillRectangle(
                                  lgbDwnRect,
                                  rcDownRect
                                );
                            }

                            g.ResetClip();
                            break;

                            
                    }

                    

                    #endregion
                  

                    #region Draw back image
                    DrawTitleBarBackImage(g, rcTitleBar, GradualTitleBarPath);
                    #endregion
                   
                DrawOuterTitleBarBorder(g, rcTitleBar, m_clrOuterTitleBarColor);
                DrawInnerTitleBarBorder(g, rcTitleBar, m_clrInnerTitleBarColor);

            }
            GradualTitleBarPath.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        private GraphicsPath BuildTitleBarShape(Rectangle rc)
        {
            GraphicsPath e = new GraphicsPath();
            switch (m_eTitleBarType)
            {
                case GradualTitleBarType.Rounded:
                    e.AddArc(
                      rc.Left,
                      rc.Top,
                      rc.Height,
                      rc.Height,
                      90,
                      180);
                    e.AddLine(
                      rc.Left + rc.Height / 2,
                      rc.Top,
                      rc.Right,
                      rc.Top
                    );
                    e.AddArc(
                     rc.Right,
                     rc.Top,
                     rc.Height,
                     rc.Height,
                     -90,
                     180);
                    e.AddLine(
                     rc.Right,
                     rc.Bottom,
                     rc.Left + rc.Height / 2,
                     rc.Bottom);
                    break;
                case GradualTitleBarType.Angular:
                    e.AddLine(
                      rc.Left,
                      rc.Bottom,
                      rc.Left + 20,
                      rc.Top);
                    e.AddLine(
                      rc.Left + 20,
                      rc.Top,
                      rc.Right,
                      rc.Top);
                    e.AddLine(
                      rc.Right,
                      rc.Top,
                      rc.Right - 20,
                      rc.Bottom
                    );
                    e.AddLine(
                      rc.Right - 20,
                      rc.Bottom,
                      rc.Left,
                      rc.Bottom
                    );
                    break;
                case GradualTitleBarType.Rectangular:
                    e.AddRectangle(rc);
                    break;

            }
            return e;

        }

        private void DrawTitleBarBackImage(
            Graphics g,
            Rectangle rcTitlebar,
            GraphicsPath clip
            )
        {
            if (m_TitleBarBackImage != null) {
                int lH = m_TitleBarBackImage.Height;
                int lW = m_TitleBarBackImage.Width;

                Rectangle rcIcon = new Rectangle(
                0,
                0,
                lW,
                lH
                );
                switch (m_eTitleBarImageAlign)
                {
                    case GradualTitleBarBackImageAlign.Right:
                        rcIcon.X = rcTitlebar.Right  - lW ;
                        rcIcon.Y = rcTitlebar.Bottom / 2 - lH / 2;
                        break;
                    case GradualTitleBarBackImageAlign.Center:
                        rcIcon.X = rcTitlebar.Right / 2 - lW / 2 + rcTitlebar.Height / 2;
                        rcIcon.Y = rcTitlebar.Bottom / 2 - lH / 2;
                        break;
                    case GradualTitleBarBackImageAlign.Left:
                        rcIcon.X = rcTitlebar.Left - rcTitlebar.Height / 2;
                        rcIcon.Y = rcTitlebar.Bottom / 2 - lH / 2;
                        break;

                }

                // draw image:
                g.SetClip(clip);
                g.DrawImage(
                   m_TitleBarBackImage,
                   rcIcon
                );
                g.ResetClip();

            }


        }
        public void RenderTitleBarButtonsBox(Rectangle rcBox, Graphics g, int lSinglePosX, int lSinglePosY)
        {
            using (GradualAntiAlias xxx = new GradualAntiAlias(g))
            {
                int lBtnWidth = 0;
                int lBtnHeight = 0;
                
                foreach (GradualTitleBarButton btn in m_xtbButtons)
                {
                    if (btn.ButtonStyle == GradualTitleBarButton.GradualTitleBarButtonStyle.MacStyle)
                    {
                        m_bShouldRenderButtonBox = false;
                    }
                    
                     lBtnWidth = btn.XButtonWidth;
                     lBtnHeight = btn.XButtonHeight;
                }

                int lX = rcBox.Right - lBtnWidth;
                int lY = rcBox.Bottom - lBtnHeight;

                if (m_bShouldRenderButtonBox)
                {
                    using (GraphicsPath XButtonBox = BuildTitleBarButtonsBox(rcBox))
                    {
                        switch (m_eButtonBoxFill){
                            case GradualButtonBoxFill.ColorMix:
                                using (LinearGradientBrush lgb = new LinearGradientBrush(
                                rcBox,
                                m_ButtonBoxColors[0],
                                m_ButtonBoxColors[4],
                                LinearGradientMode.Vertical))
                                {

                                    lgb.InterpolationColors = GradualFormHelper.ColorMix
                                    (
                                        m_ButtonBoxColors,
                                        false
                                    );

                                    g.FillPath(
                                    lgb,
                                    XButtonBox
                                    );
                                }
                                break;
                            case GradualButtonBoxFill.TitleBarRectangleRendering:
                                RectangleF rcDownRect = XButtonBox.GetBounds();
                                RectangleF rcUpRect = XButtonBox.GetBounds();
                                g.SetClip(XButtonBox);
                                rcUpRect.Height /= 2;
                                using (LinearGradientBrush lgbUpperRect = new LinearGradientBrush(
                                      rcUpRect,
                                      m_clrUpperFillStart,
                                      m_clrUpperFillEnd,
                                      LinearGradientMode.Vertical
                                      ))
                                {

                                    lgbUpperRect.WrapMode = WrapMode.TileFlipY;
                                    g.FillRectangle(
                                      lgbUpperRect,
                                      rcUpRect
                                    );
                                }

                                rcDownRect.Height = rcDownRect.Height / 2;
                                rcDownRect.Y = rcUpRect.Bottom;
                                using (LinearGradientBrush lgbDwnRect = new LinearGradientBrush(
                                      rcDownRect,
                                      m_clrFillStart,
                                      m_clrFillEnd,
                                      LinearGradientMode.Vertical
                                      ))
                                {

                                    g.FillRectangle(
                                      lgbDwnRect,
                                      rcDownRect
                                    );
                                }

                                g.ResetClip();
                                break;

                        }


                        #region Draw button separators
                        g.DrawLine(
                          new Pen(m_clrButtonBoxOuter),
                          rcBox.Right - lBtnWidth,
                          rcBox.Bottom,
                          rcBox.Right - lBtnWidth,
                          rcBox.Top + 1);
                        g.DrawLine(
                          new Pen(m_clrButtonBoxInner),
                          rcBox.Right - lBtnWidth - 1,
                          rcBox.Bottom,
                          rcBox.Right - lBtnWidth - 1,
                          rcBox.Top + 1);
                        g.DrawLine(
                          new Pen(m_clrButtonBoxOuter),
                          rcBox.Right - lBtnWidth * 2,
                          rcBox.Bottom - 2,
                          rcBox.Right - lBtnWidth * 2,
                          rcBox.Top + 1);
                        g.DrawLine(
                          new Pen(m_clrButtonBoxInner),
                          rcBox.Right - lBtnWidth * 2 - 1,
                          rcBox.Bottom - 2,
                          rcBox.Right - lBtnWidth * 2 - 1,
                          rcBox.Top + 1);
                        #endregion

                        #region Render buttons

                        g.SetClip(XButtonBox);

                        foreach (GradualTitleBarButton btn in m_xtbButtons)
                        {
                            btn.XButtonLeft = lX;
                            btn.XButtonTop = lY;

                            btn.RenderTitleBarButton(btn.XButtonLeft, btn.XButtonTop, g, XButtonBox);
                            lX -= btn.XButtonWidth + 1;
                        }

                        g.ResetClip();
                        #endregion


                        g.DrawPath(
                          new Pen(m_clrButtonBoxOuter),
                          XButtonBox
                        );

                        DrawInnerTitleBarBoxBorder(g, rcBox, m_clrButtonBoxInner);
                       
                        
                    }
                    
                }
                else
                {
                    int lSP = lSinglePosX;
                    foreach (GradualTitleBarButton btn in m_xtbButtons)
                    {
                        btn.XButtonHeight = 13;
                        btn.XButtonWidth = 13;
                        btn.XButtonLeft = lSP;
                        btn.XButtonTop = lSinglePosY;

                        btn.RenderTitleBarButton(
                            btn.XButtonLeft,
                            btn.XButtonTop,
                            g,
                            null
                        );
                        lSP -= btn.XButtonWidth + 4;

                    }
                }
            }
           
             
        }
        private void DrawInnerTitleBarBoxBorder(Graphics g, Rectangle rcBox,Color clrInnerBorder)
        {

            rcBox.Inflate(-1, -1);
            using (GraphicsPath GradualTitleBarBox = BuildTitleBarButtonsBox(rcBox))
            {
                using (Pen pInner = new Pen(clrInnerBorder))
                {
                    g.DrawPath(
                      pInner,
                      GradualTitleBarBox
                    );
                }
            }
        }
        public GraphicsPath BuildTitleBarButtonsBox(Rectangle r)
        {
            GraphicsPath XButtonBox = new GraphicsPath();
            switch (m_eTitleBarType)
            {
                case GradualTitleBarType.Rounded:
                    XButtonBox.AddBezier(
                    new Point(r.Left - 5, r.Top + 1),
                    new Point(r.Left + 20, r.Top + 5),
                    new Point(r.Left - 5, r.Bottom + 2),
                    new Point(r.Left + 45, r.Bottom));
                    XButtonBox.AddLine(
                    r.Left + 45,
                    r.Bottom,
                    r.Right - 5,
                    r.Bottom);
                    XButtonBox.AddBezier(
                    new Point(r.Right - 5, r.Bottom),
                    new Point(r.Right - 2, r.Bottom - 1),
                    new Point(r.Right - 2, r.Bottom - 1),
                    new Point(r.Right, r.Bottom - 5));
                    XButtonBox.AddLine(
                    r.Right,
                    r.Bottom - 5,
                    r.Right,
                    r.Top + 1);
                    break;
                case GradualTitleBarType.Angular:
                   XButtonBox.AddLine(
                   r.Left + 18,
                   r.Top,
                   r.Left,
                   r.Bottom);
                   XButtonBox.AddLine(
                   r.Left + 18,
                   r.Top,
                   r.Right - r.Height ,
                   r.Top);
                   XButtonBox.AddArc(
                   r.Right - r.Height ,
                   r.Top,
                   r.Height,
                   r.Height,
                   -90,
                   180);
                   XButtonBox.AddLine(
                   r.Right - r.Height ,
                   r.Bottom,
                   r.Left,
                   r.Bottom);
                   break;
                case GradualTitleBarType.Rectangular:
                   XButtonBox.AddLine(
                   r.Left,
                   r.Top,
                   r.Left,
                   r.Bottom);
                   XButtonBox.AddLine(
                   r.Left,
                   r.Top,
                   r.Right - r.Height,
                   r.Top);
                   XButtonBox.AddArc(
                   r.Right - r.Height,
                   r.Top,
                   r.Height,
                   r.Height,
                   -90,
                   180);
                   XButtonBox.AddLine(
                   r.Right - r.Height,
                   r.Bottom,
                   r.Left,
                   r.Bottom);
                   break;

            }
            return XButtonBox;
        }
    }
}
