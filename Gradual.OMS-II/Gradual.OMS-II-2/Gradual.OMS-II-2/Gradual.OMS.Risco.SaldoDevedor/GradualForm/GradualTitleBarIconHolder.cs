using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace GradualForm
{
    public class GradualTitleBarIconHolder
    {

        public class GradualHolderButton
        {
            private Image m_btnIcon;
            private Rectangle m_rcBtn = new Rectangle();
            private bool m_bHot = false;
            private int m_lTop = 0;
            private int m_lLeft = 0;
            private string m_sCaption = "GradualHolderButton";
            private Font m_fntBtnFnt = new Font("Visitor TT2 BRK", 14);
            private Color m_clrCaptionColor = Color.YellowGreen;
            private string m_sDescription = "Description";
            private Font m_fntDescription = new Font("Visitor TT2 BRK", 9);
            private Color m_clrDescriptionColor = Color.White;
            private Image m_FrameBackImage;

            private Color m_clrFrameStartColor = Color.FromArgb(102, 102, 102);
            private Color m_clrFrameEndColor = Color.FromArgb(42, 42, 42);
            private int m_lFrameAlpha = 255;

            public GradualHolderButton(Image btn)
            {
                this.m_btnIcon = btn;
            }
            public GradualHolderButton(
                Image btn,
                string sCaption)
            {
                this.m_btnIcon = btn;
                this.m_sCaption = sCaption;
            }

            public Rectangle ButtonRectangle
            {
                get
                {
                    return this.m_rcBtn;
                }
                set
                {
                    this.m_rcBtn = value;
                }
            }
            public Image ButtonImage
            {
                get
                {
                    return this.m_btnIcon;
                }
                set
                {
                    this.m_btnIcon = value;
                }
            }
            public bool Hot
            {
                get
                {
                    return this.m_bHot;
                }
                set
                {
                    this.m_bHot = value;
                }
            }
            public int Top
            {
                get
                {
                    return this.m_lTop;
                }
                set
                {
                    this.m_lTop = value;
                }
            }
            public int Left
            {
                get
                {
                    return this.m_lLeft;
                }
                set
                {
                    this.m_lLeft = value;
                }
            }
            public string GradualHolderButtonCaption
            {
                get
                {
                    return this.m_sCaption;
                }
                set
                {
                    this.m_sCaption = value;
                }

            }
            public Color GradualHolderButtonCaptionColor
            {

                get
                {
                    return this.m_clrCaptionColor;
                }
                set
                {
                    this.m_clrCaptionColor = value;
                }
            }
            public Font GradualHolderButtonCaptionFont
            {
                get
                {
                    return this.m_fntBtnFnt;
                }
                set
                {
                    this.m_fntBtnFnt = value;
                }
            }
            public string GradualHolderButtonDescription
            {
                get
                {
                    return this.m_sDescription;
                }
                set
                {
                    this.m_sDescription = value;
                }

            }
            public Color GradualHolderButtonDescriptionColor
            {

                get
                {
                    return this.m_clrDescriptionColor;
                }
                set
                {
                    this.m_clrDescriptionColor = value;
                }
            }
            public Font GradualHolderButtonDescriptionFont
            {
                get
                {
                    return this.m_fntDescription;
                }
                set
                {
                    this.m_fntDescription = value;
                }
            }
            public Image FrameBackImage
            {
                get
                {
                    return m_FrameBackImage;
                }
                set
                {
                    m_FrameBackImage = value;
                }
            }
            public Color FrameStartColor
            {
                get
                {
                    return this.m_clrFrameStartColor;
                }
                set
                {
                    this.m_clrFrameStartColor = value;
                }
            }
            public Color FrameEndColor
            {
                get
                {
                    return this.m_clrFrameEndColor;
                }
                set
                {
                    this.m_clrFrameEndColor = value;
                }
            }
            public int FrameAlpha
            {
                get
                {
                    return this.m_lFrameAlpha;
                }
                set
                {
                    if (m_lFrameAlpha < 0) m_lFrameAlpha = 0;
                    if (m_lFrameAlpha > 255) m_lFrameAlpha = 255;
                    this.m_lFrameAlpha = value;
                }
            }
        }
        
        private GradualForm m_owner;
        private List<GradualHolderButton> m_xhBtn = new List<GradualHolderButton>();

        public List<GradualHolderButton> HolderButtons
        {
            get
            {
                return this.m_xhBtn;
            }
        }

        public GradualTitleBarIconHolder(
            GradualForm xcf)
        {
            m_owner = xcf;
        }
        public GradualTitleBarIconHolder()
        {
        }

        public void RenderHolderButtons(
             int x,
             int y,
             Graphics g
           )
        {
            int lX = x;
            Rectangle rcIcon = new Rectangle();
            RectangleF rcImage = new RectangleF();
            RectangleF rcFrame = new RectangleF();

            foreach (GradualHolderButton xbtn in m_xhBtn)
            {

                if (xbtn.ButtonImage != null)
                {
                    xbtn.Left = lX;
                    xbtn.Top = y + 1;

                    rcIcon = new Rectangle(
                    lX,
                    y + 1,
                    xbtn.ButtonImage.Size.Width,
                    xbtn.ButtonImage.Size.Height
                    );

                    if (xbtn.Hot)
                    {
                        using (GradualAntiAlias xaa = new GradualAntiAlias(g))
                        {
                            using (GraphicsPath XHolderBtnPath = BuildHolderButtonFrame(rcIcon, 100, 40))
                            {

                                using (LinearGradientBrush lgb = new LinearGradientBrush(
                                      XHolderBtnPath.GetBounds(),
                                      Color.FromArgb(xbtn.FrameAlpha, xbtn.FrameStartColor),
                                      Color.FromArgb(xbtn.FrameAlpha, xbtn.FrameEndColor),
                                      LinearGradientMode.Vertical
                                      ))
                                {
                                    g.FillPath(
                                       lgb,
                                       XHolderBtnPath
                                    );
                                }

                                rcFrame = XHolderBtnPath.GetBounds();

                            }
                            int lFrameImageWidth = 0;
                            if (xbtn.FrameBackImage != null)
                            {
                                // draw frame image:
                                rcImage = new RectangleF(
                                rcFrame.Right - xbtn.FrameBackImage.Width,
                                rcFrame.Bottom - xbtn.FrameBackImage.Height,
                                xbtn.FrameBackImage.Width,
                                xbtn.FrameBackImage.Height
                                );
                                g.DrawImage(xbtn.FrameBackImage, rcImage);
                                lFrameImageWidth = xbtn.FrameBackImage.Height;
                            }
                            // draw caption / description:
                            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                            g.DrawString(
                                xbtn.GradualHolderButtonCaption,
                                xbtn.GradualHolderButtonCaptionFont,
                                new SolidBrush(xbtn.GradualHolderButtonCaptionColor),
                                rcFrame.Left + 2,
                                rcIcon.Bottom + 4
                            );

                            StringFormat sf = new StringFormat();
                            sf.Alignment = StringAlignment.Near;
                            sf.LineAlignment = StringAlignment.Near;
                            sf.Trimming = StringTrimming.EllipsisCharacter;
                            sf.FormatFlags = StringFormatFlags.LineLimit;

                            float fCaptionWidth = g.MeasureString(xbtn.GradualHolderButtonCaption, xbtn.GradualHolderButtonCaptionFont).Height;

                            RectangleF rcDescription = new RectangleF(
                            rcFrame.Left + 2,
                            rcIcon.Bottom + fCaptionWidth,
                            rcFrame.Width,
                            rcFrame.Height - lFrameImageWidth);
                            g.DrawString(
                              xbtn.GradualHolderButtonDescription,
                              xbtn.GradualHolderButtonDescriptionFont,
                              new SolidBrush(xbtn.GradualHolderButtonDescriptionColor),
                              rcDescription,
                              sf);

                        }


                    }

                    // draw button:
                    g.DrawImage(
                    xbtn.ButtonImage,
                    rcIcon
                    );

                    xbtn.ButtonRectangle = rcIcon;

                    // update position:
                    lX += rcIcon.Width + 2;
                }

            }
        }


        private GraphicsPath BuildHolderButtonFrame(Rectangle rcBtn, int lFrameWidth, int lFrameHeight)
        {
            GraphicsPath XHolderBtnPath = new GraphicsPath();

            XHolderBtnPath.AddArc(
            rcBtn.Left - 2,
            rcBtn.Top,
            rcBtn.Height / 2,
            rcBtn.Height / 2,
            180,
            90);
            XHolderBtnPath.AddLine(
            rcBtn.Left + rcBtn.Height / 2,
            rcBtn.Top,
            rcBtn.Right - 2,
            rcBtn.Top);
            XHolderBtnPath.AddArc(
            rcBtn.Right - rcBtn.Height / 2 + 2,
            rcBtn.Top,
            rcBtn.Height / 2,
            rcBtn.Height / 2,
            -90,
            90);
            XHolderBtnPath.AddLine(
            rcBtn.Right + 2,
            rcBtn.Top + rcBtn.Height / 2,
            rcBtn.Right + 2,
            rcBtn.Top + rcBtn.Height + 2 + rcBtn.Height / 2 - 8);

            XHolderBtnPath.AddLine(
            rcBtn.Right + 2,
            rcBtn.Top + rcBtn.Height + 2 + rcBtn.Height / 2 - 8,
            rcBtn.Right + lFrameWidth,
            rcBtn.Top + rcBtn.Height + 2 + rcBtn.Height / 2 - 8);

            XHolderBtnPath.AddArc(
            rcBtn.Right + lFrameWidth,
            rcBtn.Top + rcBtn.Height + 2 + rcBtn.Height / 2 - 8,
            rcBtn.Height / 2,
            rcBtn.Height / 2,
            -90,
            90);

            XHolderBtnPath.AddLine(
            rcBtn.Right + lFrameWidth + rcBtn.Height / 2,
            rcBtn.Top + rcBtn.Height + 2 - 8 + rcBtn.Height / 2,
            rcBtn.Right + lFrameWidth + rcBtn.Height / 2,
            rcBtn.Top + rcBtn.Height + 2 - 8 + rcBtn.Height / 2 + lFrameHeight);

            XHolderBtnPath.AddLine(
            rcBtn.Right + lFrameWidth + rcBtn.Height / 2,
            rcBtn.Top + rcBtn.Height + 2 - 8 + rcBtn.Height / 2 + lFrameHeight,
            rcBtn.Left,
            rcBtn.Top + rcBtn.Height + 2 - 8 + rcBtn.Height / 2 + lFrameHeight
            );

            XHolderBtnPath.AddLine(
            rcBtn.Left - 2,
            rcBtn.Top + rcBtn.Height + 2 - 8 + rcBtn.Height / 2 + lFrameHeight,
            rcBtn.Left - 2,
            rcBtn.Top + rcBtn.Height / 2 - 4
            );

            return XHolderBtnPath;

        }

        public int HitTestHolderButton(
            int x,
            int y,
            Rectangle rcHolder
            )
        {
            int lBtnIndex = -1;
            if (x >= rcHolder.Left && x <= rcHolder.Right)
            {
                GradualHolderButton btn = null;
                for (int i = 0; i < m_xhBtn.Count; i++)
                {
                    btn = m_xhBtn[i];
                    if (y >= 4 && y <= btn.ButtonRectangle.Bottom)
                    {
                        if (x >= btn.Left)
                        {
                            if (x < btn.Left + btn.ButtonRectangle.Width)
                            {
                                lBtnIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            return lBtnIndex;

        }
    }
}
