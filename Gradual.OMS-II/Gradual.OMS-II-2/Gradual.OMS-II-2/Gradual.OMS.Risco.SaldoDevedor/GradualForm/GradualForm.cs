using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace GradualForm
{
    public class GradualForm : System.Windows.Forms.Form
    {

        public bool HasTitleBar = true; // Annonymous
        public bool HasMenuIcon = true; // Annonymous
        public bool HasStatusBar = true; // Annonymous

        private GradualTitleBarButton _buttonClose = new GradualTitleBarButton(GradualTitleBarButton.GradualTitleBarButtonType.Close); // Annonymous
        private GradualTitleBarButton _buttonMaximize = new GradualTitleBarButton(GradualTitleBarButton.GradualTitleBarButtonType.Maximize, Color.FromArgb(3, 63, 126), Color.FromArgb(119, 217, 246)); // Annonymous
        private GradualTitleBarButton _buttonMinimize = new GradualTitleBarButton(GradualTitleBarButton.GradualTitleBarButtonType.Minimize, Color.FromArgb(124, 13, 2), Color.FromArgb(251, 164, 48)); // Annonymous

        private bool _hasMinimize = false;
        public bool HasMinimize
        {
            get
            {
                return _hasMinimize;
            }

            set
            {
                _hasMinimize = value;
                if (_hasMinimize)
                {
                    m_GradualTitleBar.TitleBarButtons.Add(_buttonMinimize);
                    this.Invalidate();
                }
                else
                {
                    m_GradualTitleBar.TitleBarButtons.Remove(_buttonMinimize);
                    this.Invalidate();
                }
            }
        }


        private bool _OcultarBarraDeTitulo = false;
        public bool OcultarBarraDeTitulo
        {
            get
            {
                return _OcultarBarraDeTitulo;
            }

            set
            {
                _OcultarBarraDeTitulo = value;

                if (_OcultarBarraDeTitulo)
                {
                    //HACK: validação para evitar que o formulário recalcule o cabeçalho quando o mesmo é ocultado antes da exibição
                    if (gFlagSemBarra)
                    {
                        return;
                    }
                    this.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
                    gFlagSemBarra = true;
                    HasTitleBar = !gFlagSemBarra;

                    foreach (Control lControl in this.Controls)
                    {
                        lControl.Top -= 26;

                        // Corrige dimensão do Form, qdo o controle tem Anchor = Bottom
                        if (lControl.Anchor.HasFlag(AnchorStyles.Bottom))
                            lControl.Size = new Size(lControl.Size.Width, lControl.Size.Height + 22);
                    }

                    if (this.Height > 2 * 26)
                    {
                        this.Height -= 26;
                    }
                }
                else
                {
                    gFlagSemBarra = false;
                    HasTitleBar = !gFlagSemBarra;
                    this.Padding = new System.Windows.Forms.Padding(6, 36, 6, 6);

                    foreach (Control lControl in this.Controls)
                    {
                        lControl.Top += 26;

                        // Corrige dimensão do Form, qdo o controle tem Anchor = Bottom
                        if (lControl.Anchor.HasFlag(AnchorStyles.Bottom))
                            lControl.Size = new Size(lControl.Size.Width, lControl.Size.Height - 26);
                    }
                    this.Height += 26;
                }

                this.Invalidate();
            }
        }

        //private string _text = String.Empty;

        //public string Extension { get; set; }

        [System.ComponentModel.SettingsBindable(true)]
        public override sealed string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                base.Text = value;
                this.TitleBar.TitleBarCaption = base.Text;
                //TODO: find another way
                //this.Invalidate(m_rcBox);
                this.Invalidate();
            }
        }
        
        private SerializableFont _fonte;
        public SerializableFont Fonte
        {
            get { return _fonte; }
            set { _fonte = value; }
        }

        public bool HasMaximize = true; // Annonymous
        public bool HasClose = true; // Annonymous

        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;
        const int RESIZE_HANDLE_SIZE = 10;

        const UInt32 NCLBUTTONDOWN = 0x00A1;
        const UInt32 WM_SYSCOMMAND = 0x112;
        const UInt32 HTCAPTION = 2;
        const UInt32 SC_SIZE = 0xF000;
        const UInt32 SC_MAXIMIZE = 0xF030;
        const UInt32 SC_RESTORE = 0xF120;
        const UInt32 SC_RESTORE2 = 0xF122;
        const UInt32 SC_MINIMIZE = 0xF020;
        const UInt32 WM_NCHITTEST = 0x0084;
        const UInt32 WM_MOUSEMOVE = 0x0200;
        const UInt32 HTLEFT = 10;
        const UInt32 HTRIGHT = 11;
        const UInt32 HTBOTTOMRIGHT = 17;
        const UInt32 HTBOTTOM = 15;
        const UInt32 HTBOTTOMLEFT = 16;
        const UInt32 HTTOP = 12;
        const UInt32 HTTOPLEFT = 13;
        const UInt32 HTTOPRIGHT = 14;
        const UInt32 WM_NCLBUTTONDBLCLK = 0x00A3;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private bool gFlagSemBarra;

        #region PInvoke

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern int SendMessage(
            IntPtr hWnd,
            int wMsg,
            IntPtr wParam,
            IntPtr lParam
            );
        #endregion

        #region Private members
        private Rectangle m_rcTitleBar = new Rectangle();
        private Image m_MenuIcon;
        private Image m_bmpBackImage;
        private ContentAlignment m_ImageAlign = ContentAlignment.TopLeft;
      
        private Rectangle m_rcGradualTitleBarIconHolder = new Rectangle();
        private Rectangle m_rcSizeGrip = new Rectangle();
        private Rectangle m_rcXMenuIcon = new Rectangle();
        private Rectangle m_rcBox = new Rectangle();
        
        // Form Backgroud color
        private Color m_clrBackground = Color.FromArgb(22, 22, 22);

        public Rectangle m_rcRestoreBounds = new Rectangle();
        public FormWindowState PriorState = FormWindowState.Normal;
        private Rectangle m_rcIconHolder = new Rectangle();
        
        private int m_lTitleBarHeight = 35;
     
        private Rectangle m_rcTitleBarIcon = new Rectangle();
        private Rectangle m_rcGradualStatusBar = new Rectangle();

        private bool m_bMouseDown = false;
        private bool m_bMaximized = false;

        private GradualTitleBarIconHolder m_xtbHolder = new GradualTitleBarIconHolder();
        private GradualTitleBar m_GradualTitleBar = new GradualTitleBar();
        private GradualStatusBar m_xsbStatusBar = new GradualStatusBar();
        private Gradual3DBorderPrimitive m_x3dx = new Gradual3DBorderPrimitive();

        private GraphicsPath m_TitleBarButtonsBox = new GraphicsPath();

        private List<Color> m_MenuIconMix = new List<Color>();
        
        // Form border color
        private Color m_clrMenuIconBorderInner = Color.FromArgb(52, 52, 52);
        private Color m_clrMenuIconBorderOuter = Color.FromArgb(0,0,0);
       
        #endregion

        #region GradualFormHolderButtonClickArgs
        public class GradualFormHolderButtonClickArgs : EventArgs
        {
            /// <summary>
            /// Button index.
            /// </summary>
            private int m_lIndex;

            public int ButtonIndex
            {
                get
                {
                    return m_lIndex;
                }
            }

            public GradualFormHolderButtonClickArgs(
                int lIndex
                )
                : base()
            {
                m_lIndex = lIndex;
            }
        }
        #endregion      
        
        #region Events / Delegates
        public delegate void GradualFormHolderButtonClickHandler(GradualFormHolderButtonClickArgs e);
        public event GradualFormHolderButtonClickHandler GradualFormHolderButtonClick;
        #endregion
        
        #region Properties
        public GradualTitleBarIconHolder IconHolder
        {
            get
            {
                return this.m_xtbHolder;
            }
        }
        public GradualTitleBar TitleBar
        {
            get
            {
                return this.m_GradualTitleBar;
            }
        }
        public GradualStatusBar StatusBar
        {
            get
            {
                return this.m_xsbStatusBar;
            }
        }
        public Gradual3DBorderPrimitive Border
        {
            get
            {
                return this.m_x3dx;
            }
        }

        public Color XFormBackColor
        {
            get
            {
                return this.m_clrBackground;
            }
            set
            {
                this.m_clrBackground = value;
            }
        }
        public Color MenuIconInnerBorder
        {
            get
            {
                return this.m_clrMenuIconBorderInner;
            }
            set
            {
                this.m_clrMenuIconBorderInner = value;
            }
        }
        public Color MenuIconOuterBorder
        {
            get
            {
                return this.m_clrMenuIconBorderOuter;
            }
            set
            {
                this.m_clrMenuIconBorderOuter = value;
            }
        }
        public Image MenuIcon
        {
            get
            {
                return this.m_MenuIcon;
            }
            set
            {
                this.m_MenuIcon = value;
            }
        }
        public Image BackImage
        {
            get
            {
                return this.m_bmpBackImage;
            }
            set
            {
                this.m_bmpBackImage = value;
            }
        }
        public List<Color> MenuIconMix
        {
            get
            {
                return this.m_MenuIconMix;
            }
            set
            {
                this.m_MenuIconMix = value;
            }
        }
        private bool _TamanhoFixo;

        public bool TamanhoFixo
        {
            get
            {
                return _TamanhoFixo;
            }

            set
            {
                _TamanhoFixo = value;
            }
        }

        private bool _TamanhoFixoLaterais;

        public bool TamanhoFixoLaterais
        {
            get
            {
                return _TamanhoFixoLaterais;
            }

            set
            {
                _TamanhoFixoLaterais = value;
            }
        }

        private bool _TamanhoFixoRodape;

        public bool TamanhoFixoRodape
        {
            get
            {
                return _TamanhoFixoRodape;
            }

            set
            {
                _TamanhoFixoRodape = value;
            }
        }

        #endregion

        protected StickyWindow StickyWindowInstance;
        public System.String gInstrumento = String.Empty;

        #region Métodos Virtuais

        public virtual void AlternarVisibilidadeBarraFerramentas(bool Exibir) { }
        public virtual void AlternarVisibilidadeTicker(bool Exibir) { }
        public virtual string RetornarInstrumento() { return gInstrumento; }
        public virtual void AlterarConfiguracaoSeguirCorretoras(object sender) { }
        public virtual void AtribuirInstrumento(String Instrumento) { gInstrumento = Instrumento; }
        public virtual string RetornarConfiguracao() { return ""; }
        public virtual void AplicarConfiguracao(String Configuracao) { }
        public virtual void CarregarIconesClaros(){}
        public virtual void CarregarIconesEscuros(){}

        public virtual SerializableFont RetornarFonteConfigurada()
        {
            if (this.Fonte != null)
            {
                return this.Fonte;
            }
            else
            {
                //return Gradual.GTI.MotorVisual.Lib.ConfiguracoesDeEstilo.Fonte_Grid;
                return StyleSettings.Fonte_Grid;
                //return new SerializableFont(GradualForm.StyleSettings.Fonte_Grid);
            }
        }

        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            StickyWindowInstance = new StickyWindow(this) { StickGap = 15 };
        }

        public GradualForm()
        {

            InitializeComponent();
            // set control styles:
            this.SetStyle(
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.ResizeRedraw |
                 ControlStyles.DoubleBuffer |
                 ControlStyles.SupportsTransparentBackColor |
                 ControlStyles.OptimizedDoubleBuffer,
                 true
            );

            this.FormBorderStyle = FormBorderStyle.None;
            //this.MinimumSize = new Size(400, 400);
            this.Padding = new Padding(6,36,6,6);
           
            // initialize titlebar buttons:
            m_GradualTitleBar.TitleBarButtons.Add(_buttonClose);
            m_GradualTitleBar.TitleBarButtons.Add(_buttonMaximize);
            m_GradualTitleBar.TitleBarButtons.Add(_buttonMinimize);

            //this.TitleBar.TitleBarButtons.RemoveAt(2); // remove minimize button
            //this.TitleBar.TitleBarButtons.RemoveAt(1); // remove maximize/restore button
            //this.TitleBar.TitleBarButtons.RemoveAt(0); // remove close button

            // initialize mix:
            m_MenuIconMix.Add(Color.FromArgb(112, 106, 108));
            m_MenuIconMix.Add(Color.FromArgb(56, 52, 53));
            m_MenuIconMix.Add(Color.FromArgb(53, 49, 50));
            m_MenuIconMix.Add(Color.FromArgb(71, 71, 71));
            m_MenuIconMix.Add(Color.FromArgb(112, 106, 108));

            m_xtbHolder = new GradualTitleBarIconHolder(this);
        }
        
        // Annonymous
        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);
        //    Invalidate();
        //}

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            bool bShouldReset = false;
            if (m_x3dx.BorderType != Gradual3DBorderPrimitive.GradualBorderType.Rectangular)
            {
                e.Graphics.Clip = new Region(m_x3dx.FindX3DBorderPrimitive(this.ClientRectangle));
                bShouldReset = true;
                this.BackColor = Color.Fuchsia;
                this.TransparencyKey = Color.Fuchsia;
            }

            using (SolidBrush sb = new SolidBrush(m_clrBackground))
            {
                e.Graphics.FillRectangle(sb, this.ClientRectangle);
                if (bShouldReset)
                {
                    e.Graphics.ResetClip();
                }
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_bMouseDown = true;
            }

            #region Titlebar buttons

            if (m_GradualTitleBar.ShouldDrawButtonBox)
            {
                foreach (GradualTitleBarButton xbtn in m_GradualTitleBar.TitleBarButtons)
                {

                    if (m_TitleBarButtonsBox.IsVisible(e.Location))
                    {
                        if (PointInRect(e.Location, new Rectangle(xbtn.XButtonLeft, xbtn.XButtonTop, xbtn.XButtonWidth,xbtn.XButtonHeight)))
                        {
                            if (xbtn.Disabled)
                            {
                                return;
                            }

                            if (xbtn.XButtonType == GradualTitleBarButton.GradualTitleBarButtonType.Minimize)
                            {
                                PriorState = this.WindowState;
                                if (!PriorState.Equals(FormWindowState.Maximized))
                                {
                                    m_rcRestoreBounds = new Rectangle(this.Location, this.Size);
                                }
                                this.WindowState = FormWindowState.Minimized;
                                return;
                            }
                            else if (xbtn.XButtonType == GradualTitleBarButton.GradualTitleBarButtonType.Maximize)
                            {
                                if (this.WindowState.Equals(FormWindowState.Normal))
                                {
                                    PriorState = this.WindowState;
                                    m_rcRestoreBounds = new Rectangle(this.Location, this.Size);
                                    this.WindowState = FormWindowState.Maximized;
                                    return;
                                }
                                else if (this.WindowState.Equals(FormWindowState.Maximized))
                                {
                                    PriorState = this.WindowState;
                                    this.WindowState = FormWindowState.Normal;
                                    this.Size = new Size(m_rcRestoreBounds.Width, m_rcRestoreBounds.Height);
                                    this.Location = new Point(m_rcRestoreBounds.Left, m_rcRestoreBounds.Top);
                                    return;
                                }
                                return;
                            }
                            else if (xbtn.XButtonType == GradualTitleBarButton.GradualTitleBarButtonType.Close)
                            {
                                this.Close();
                            }
                        }
                    }

                }
            }
            else
            {
                foreach (GradualTitleBarButton xbtn in m_GradualTitleBar.TitleBarButtons)
                {
                    if (PointInRect(e.Location, new Rectangle( xbtn.XButtonLeft, xbtn.XButtonTop, xbtn.XButtonWidth, xbtn.XButtonHeight )))
                    {
                        if (xbtn.Disabled)
                        {
                            return;
                        }

                        if (xbtn.XButtonType == GradualTitleBarButton.GradualTitleBarButtonType.Minimize)
                        {
                            this.WindowState = FormWindowState.Minimized;
                        }
                        else if (xbtn.XButtonType == GradualTitleBarButton.GradualTitleBarButtonType.Maximize)
                        {
                            //if (m_bMaximized)
                            //{
                            //    m_bMaximized = false;
                            //    this.Size = new Size(m_rcRestoreBounds.Width, m_rcRestoreBounds.Height);
                            //    this.Location = new Point(m_rcRestoreBounds.Left, m_rcRestoreBounds.Top);
                            //}
                            //else
                            //{
                            //    m_rcRestoreBounds = new Rectangle(this.Location, this.Size);
                            //    Rectangle wa = Screen.GetWorkingArea(this);
                            //    this.Size = new Size(wa.Width, wa.Height);
                            //    this.Location = new Point(wa.Left, wa.Top);
                            //    m_bMaximized = true;
                            //}

                            if (this.WindowState.Equals(FormWindowState.Normal))
                            {
                                this.WindowState = FormWindowState.Maximized;
                            }
                            else if(this.WindowState.Equals(FormWindowState.Maximized))
                            {
                                this.WindowState = FormWindowState.Normal;
                            }
                        }
                        else if (xbtn.XButtonType == GradualTitleBarButton.GradualTitleBarButtonType.Close)
                        {
                            Application.Exit();
                        }
                    }
                }
            }
            #endregion

            #region Titlebar icon holder
            // mouse over TitleBarIconHolder :
            if (PointInRect(e.Location, m_rcGradualTitleBarIconHolder))
            {
                if (e.Button == MouseButtons.Left)
                {
                    // find hovering button:
                    int lIdx = m_xtbHolder.HitTestHolderButton(
                         e.X,
                         e.Y,
                         m_rcGradualTitleBarIconHolder
                    );

                    for (int i = 0; i < m_xtbHolder.HolderButtons.Count; i++)
                    {
                        if (i == lIdx)
                        {
                            GradualFormHolderButtonClickArgs XCoolHolderButton =
                                new GradualFormHolderButtonClickArgs(i);
                            if (GradualFormHolderButtonClick != null)
                                GradualFormHolderButtonClick(XCoolHolderButton);
                        }
                    }
                }
            }
            #endregion

            base.OnMouseDown(e);

        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_bMouseDown = false;
            }

            base.OnMouseUp(e);
        }

        // Annonymous
        //protected override void OnResizeBegin(EventArgs e)
        //{
        //    this.Invalidate();
        //    base.OnResizeBegin(e);
        //}

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            #region TitleBarIconHolder
            // mouse over TitleBarIconHolder ?:
            if (PointInRect(e.Location, m_rcGradualTitleBarIconHolder))
            {
                // find hovering button:
                int lIdx = m_xtbHolder.HitTestHolderButton(
                     e.X,
                     e.Y,
                     m_rcGradualTitleBarIconHolder
                );

                for (int i = 0; i < m_xtbHolder.HolderButtons.Count; i++)
                {
                    if (i == lIdx)
                    {
                        if (!m_xtbHolder.HolderButtons[i].Hot)
                        {
                            m_xtbHolder.HolderButtons[i].Hot = true;
                            Invalidate(m_rcGradualTitleBarIconHolder);
                        }

                    }
                    else
                    {
                        if (m_xtbHolder.HolderButtons[i].Hot)
                        {
                            m_xtbHolder.HolderButtons[i].Hot = false;
                            Invalidate(m_rcGradualTitleBarIconHolder);
                        }
                    }
                }

            }
            else
            {
                for (int i = 0; i < m_xtbHolder.HolderButtons.Count; i++)
                {
                    if (m_xtbHolder.HolderButtons[i].Hot)
                    {
                        m_xtbHolder.HolderButtons[i].Hot = false;
                        Invalidate(m_rcGradualTitleBarIconHolder);
                    }
                }
            }
            #endregion
            
            #region TitleBar buttons
            HitTestTitleBarButtons(e.Location);
            #endregion

            #region Form moving
            // Annonymous
            //HitTestMoveTitleBar(e);
            #endregion

            #region Sizing
            // Annonymous
            //ResizeWindow(e);
            #endregion

            base.OnMouseMove(e);
        }

        private void HitTestMoveTitleBar(MouseEventArgs e)
        {
            if (m_bMouseDown)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (PointInRect(e.Location, m_rcTitleBar))
                    {
                        ReleaseCapture();
                        //SendMessage(this.Handle,NCLBUTTONDOWN,(IntPtr)HTCAPTION,IntPtr.Zero);
                        SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, MakeLParam(((System.Windows.Forms.Form)(this)).Location.X + e.Location.X, ((System.Windows.Forms.Form)(this)).Location.Y + e.Location.Y));
                    }
                }
            }
        }
        private void HitTestTitleBarButtons(Point pos)
        {
            bool bChanged = false;
           
            if (m_GradualTitleBar.ShouldDrawButtonBox)
            {
                if (m_TitleBarButtonsBox.IsVisible(pos))
                {
                    foreach (GradualTitleBarButton xbtn in m_GradualTitleBar.TitleBarButtons)
                    {
                        if (PointInRect(pos,new Rectangle(xbtn.XButtonLeft,xbtn.XButtonTop,xbtn.XButtonWidth,xbtn.XButtonHeight)))
                        {
                            if (!xbtn.Disabled)
                            {
                                if (!xbtn.Hovering)
                                {
                                    xbtn.Hovering = true;
                                    bChanged = true;

                                }
                            }
                        }
                        else
                        {
                            if (xbtn.Hovering)
                            {
                                xbtn.Hovering = false;
                                bChanged = true;

                            }
                        }
                    }

                }
                else
                {
                    foreach (GradualTitleBarButton xbtn in m_GradualTitleBar.TitleBarButtons)
                    {
                        if (xbtn.Hovering)
                        {
                            xbtn.Hovering = false;
                            bChanged = true;
                        }
                    }
                }
            }
            else
            {
                foreach (GradualTitleBarButton xbtn in m_GradualTitleBar.TitleBarButtons)
                {
                        if (PointInRect(pos, new Rectangle(xbtn.XButtonLeft, xbtn.XButtonTop, xbtn.XButtonWidth, xbtn.XButtonHeight)))
                        {
                            if (!xbtn.Hovering)
                            {
                                if (!xbtn.Disabled)
                                {
                                    xbtn.Hovering = true;
                                    Invalidate(new Rectangle(xbtn.XButtonLeft, xbtn.XButtonTop, xbtn.XButtonWidth, xbtn.XButtonHeight));
                                }
                            }
                        }
                        else
                        {
                            if (xbtn.Hovering)
                            {
                                xbtn.Hovering = false;
                                Invalidate(new Rectangle(xbtn.XButtonLeft, xbtn.XButtonTop, xbtn.XButtonWidth, xbtn.XButtonHeight));
                            }
                        }
                }
            }

            if (bChanged)
            {
                Invalidate(m_rcBox);
            }
            
        }
        
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Rectangle rcBorder;
            rcBorder = new Rectangle(0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);

            if (HasTitleBar)
            {

                m_rcTitleBarIcon = new Rectangle(7, 5, 40, 40);

                DrawStatusBar(e.Graphics);
                DrawSysIcon(e.Graphics);
                DrawButtonsBox(e.Graphics);
                DrawTitleBar(e.Graphics);

                // build titlebar buttons box:
                m_TitleBarButtonsBox = m_GradualTitleBar.BuildTitleBarButtonsBox(m_rcBox);

                // render holder buttons:
                m_xtbHolder.RenderHolderButtons(m_rcIconHolder.X, m_rcIconHolder.Y, e.Graphics);
            }

            // Render border:
            m_x3dx.Render(rcBorder, e.Graphics, HasTitleBar);
        }
        private void DrawButtonsBox(Graphics g)
        {
            int lBoxTop = 0;
            int lBtnWidth = 0;
            int lBtnHeight = 0;
            int lBorder = 6;
            int lBoxWidth = 0;
            int lBoxHeight = 0;
            int x = this.ClientRectangle.Right - lBorder - 14;
            int y = 9;
            
            if (m_GradualTitleBar.TitleBarType == GradualTitleBar.GradualTitleBarType.Angular || m_GradualTitleBar.TitleBarType == GradualTitleBar.GradualTitleBarType.Rectangular)
            {
                lBoxTop += 4;
            }
          
            foreach (GradualTitleBarButton btn in m_GradualTitleBar.TitleBarButtons)
            {
                lBtnWidth = btn.XButtonWidth;
                lBtnHeight = btn.XButtonHeight;
            }

            if (m_GradualTitleBar.ShouldDrawButtonBox)
            {
                lBoxWidth = lBtnWidth * 3 - 1;
            }
            else
            {
                lBoxWidth = 60;
            }
            
            lBoxHeight = lBtnHeight;
              
            m_rcBox = new Rectangle(
                ClientRectangle.Right - lBorder - lBoxWidth,
                lBoxTop,
                lBoxWidth,
                lBoxHeight
                );
          
            m_GradualTitleBar.RenderTitleBarButtonsBox(m_rcBox, g, x, y);
        }

        private void DrawSysIcon(Graphics g)
        {
           
            int lLeft = 6; int lTop = 3;
            int lWidth = 47;
            int lHeight = m_lTitleBarHeight - 6;
            m_rcXMenuIcon = new Rectangle(lLeft, lTop, lWidth, lHeight);

            if (HasMenuIcon) // Annonymous
            {
                RenderSysMenuIcon(m_rcXMenuIcon, g);
            }
        }

        private void DrawStatusBar(Graphics g)
        {
            if (HasStatusBar)
            {
                int lBorderExcess = 7;
                if (m_x3dx.BorderStyle == Gradual3DBorderPrimitive.GradualBorderStyle.Flat)
                    lBorderExcess = 2;

                m_rcGradualStatusBar = new Rectangle(1, ClientRectangle.Bottom - lBorderExcess - m_xsbStatusBar.BarHeight, ClientRectangle.Right - ClientRectangle.Left, m_xsbStatusBar.BarHeight);
                m_xsbStatusBar.RenderStatusBar(g, m_rcGradualStatusBar.Left, m_rcGradualStatusBar.Top, m_rcGradualStatusBar.Width, m_rcGradualStatusBar.Height);
                m_rcSizeGrip = m_xsbStatusBar.XGripRect;
            }
        }
        private void DrawTitleBar(Graphics g)
        {
            int lTitleBarWidth;
            if (HasMenuIcon) // Annonymous
            {
                lTitleBarWidth = m_rcBox.Left - m_rcXMenuIcon.Width - 12;
            }
            else
            {
                lTitleBarWidth = m_rcBox.Left - 5;
            }

            int lTopOffset = 5;
            int lRectOffset;
            
            if (HasMenuIcon) // Annonymous
            {
                lRectOffset = m_rcXMenuIcon.Right - 2;
            }
            else
            {
                lRectOffset = 3;
            }

            if (m_GradualTitleBar.TitleBarType == GradualTitleBar.GradualTitleBarType.Angular)
            {
                if (HasMenuIcon) // Annonymous
                {
                    lTitleBarWidth += 25;
                }
                
            }
            if (m_GradualTitleBar.TitleBarType == GradualTitleBar.GradualTitleBarType.Rounded)
            {
                if (HasMenuIcon) // Annonymous
                {
                    lTitleBarWidth -= 10;
                }
            }
            if (m_GradualTitleBar.TitleBarType == GradualTitleBar.GradualTitleBarType.Rectangular)
            {
                if (HasMenuIcon) // Annonymous
                {
                    lRectOffset += 5;
                }
            }

            m_rcTitleBar = new Rectangle(lRectOffset, lTopOffset, lTitleBarWidth, 25);
            int lIconHolderOffset = m_rcTitleBar.Left + 4;
            if (m_GradualTitleBar.TitleBarType == GradualTitleBar.GradualTitleBarType.Angular)
            {
                lIconHolderOffset += 15;
            }
            if (m_GradualTitleBar.TitleBarType == GradualTitleBar.GradualTitleBarType.Rounded)
            {
                lIconHolderOffset += 4;
            }

            m_rcGradualTitleBarIconHolder = new Rectangle(
              55,
              m_rcTitleBar.Top + 3,
              255,
              m_lTitleBarHeight + 60
              );
            m_rcIconHolder = new Rectangle(
              lIconHolderOffset,
              7,
              200,
              400);


            if (HasTitleBar)
            {
                m_GradualTitleBar.RenderTitleBar(g, m_rcTitleBar);
            }
        }

        private void RenderSysMenuIcon(
            Rectangle rcMenuIcon,
            Graphics g)
        {
            using (GraphicsPath XMenuIconPath = BuildMenuIconShape(ref rcMenuIcon))
            {


                FillMenuIconGradient(XMenuIconPath, g, m_MenuIconMix);

                using (GradualAntiAlias xaa = new GradualAntiAlias(g))
                {
                    DrawInnerMenuIconBorder(rcMenuIcon, g, m_clrMenuIconBorderInner);
                    g.DrawPath(
                     new Pen(m_clrMenuIconBorderOuter),
                      XMenuIconPath
                     );
                    
                }

            }

            #region Draw icon
            if (m_MenuIcon != null) {
                int lH = m_MenuIcon.Height;
                int lW = m_MenuIcon.Width;

                Rectangle rcImage = new Rectangle((rcMenuIcon.Right - rcMenuIcon.Width / 2) - lW / 2 - 2, (rcMenuIcon.Bottom - rcMenuIcon.Height / 2) - lH / 2, lW, lH);
                g.DrawImage(
                  m_MenuIcon,
                  rcImage
                );
            }
            #endregion
        }
        private void FillMenuIconGradient(
            GraphicsPath XFillPath,
            Graphics g,
            List<Color> mix
            )
        {


            using (GradualAntiAlias xaa = new GradualAntiAlias(g))
            {
                using (LinearGradientBrush lgb = new LinearGradientBrush
                    (XFillPath.GetBounds(),
                     mix[0],
                     mix[4],
                     LinearGradientMode.Vertical))
                {
                   
                    lgb.InterpolationColors = GradualFormHelper.ColorMix(mix, false);
                    
                    g.FillPath(
                      lgb,
                      XFillPath
                    );

                }
            }
        }
       
        
        private GraphicsPath BuildMenuIconShape( ref Rectangle rcMenuIcon)
        {
            GraphicsPath XMenuIconPath = new GraphicsPath();
            switch (m_GradualTitleBar.TitleBarType)
            {
                case GradualTitleBar.GradualTitleBarType.Rounded:
                    XMenuIconPath.AddArc(
                    rcMenuIcon.Left,
                    rcMenuIcon.Top,
                    rcMenuIcon.Height,
                    rcMenuIcon.Height,
                    90,
                    180);
                    XMenuIconPath.AddLine(
                    rcMenuIcon.Left + rcMenuIcon.Height / 2,
                    rcMenuIcon.Top,
                    rcMenuIcon.Right,
                    rcMenuIcon.Top
                    );
                    XMenuIconPath.AddBezier(
                    new Point(rcMenuIcon.Right, rcMenuIcon.Top),
                    new Point(rcMenuIcon.Right - 10, rcMenuIcon.Bottom / 2 - 5),
                    new Point(rcMenuIcon.Right - 12, rcMenuIcon.Bottom / 2 + 5),
                    new Point(rcMenuIcon.Right, rcMenuIcon.Bottom)
                    );
                    XMenuIconPath.AddLine(
                    rcMenuIcon.Right,
                    rcMenuIcon.Bottom,
                    rcMenuIcon.Left + rcMenuIcon.Height / 2,
                    rcMenuIcon.Bottom
                    );
                    break;
                case GradualTitleBar.GradualTitleBarType.Angular:
                    XMenuIconPath.AddArc(
                    rcMenuIcon.Left,
                    rcMenuIcon.Top,
                    rcMenuIcon.Height,
                    rcMenuIcon.Height,
                    90,
                    180);
                    XMenuIconPath.AddLine(
                    rcMenuIcon.Left + rcMenuIcon.Height / 2,
                    rcMenuIcon.Top,
                    rcMenuIcon.Right + 18,
                    rcMenuIcon.Top
                    );
                    XMenuIconPath.AddLine(
                    rcMenuIcon.Right + 18,
                    rcMenuIcon.Top,
                    rcMenuIcon.Right - 5,
                    rcMenuIcon.Bottom
                    );
                    XMenuIconPath.AddLine(
                    rcMenuIcon.Right - 5,
                    rcMenuIcon.Bottom,
                    rcMenuIcon.Left + rcMenuIcon.Height / 2,
                    rcMenuIcon.Bottom
                    );
                    break;
                case GradualTitleBar.GradualTitleBarType.Rectangular:
                    XMenuIconPath.AddArc(
                    rcMenuIcon.Left,
                    rcMenuIcon.Top,
                    rcMenuIcon.Height,
                    rcMenuIcon.Height,
                    90,
                    180);
                    XMenuIconPath.AddLine(
                    rcMenuIcon.Left + rcMenuIcon.Height / 2,
                    rcMenuIcon.Top,
                    rcMenuIcon.Right,
                    rcMenuIcon.Top
                    );
                    XMenuIconPath.AddLine(
                    rcMenuIcon.Right,
                    rcMenuIcon.Top,
                    rcMenuIcon.Right,
                    rcMenuIcon.Bottom
                    );
                    XMenuIconPath.AddLine(
                    rcMenuIcon.Right,
                    rcMenuIcon.Bottom,
                    rcMenuIcon.Left + rcMenuIcon.Height / 2,
                    rcMenuIcon.Bottom
                    );
                    break;
                    

            }
            return XMenuIconPath;
        }
        private void DrawInnerMenuIconBorder(
            Rectangle rcMenuIcon,
            Graphics g,
            Color clr)
        {

            
            rcMenuIcon.Inflate(-1, -1);
            using (GraphicsPath XMenuIconPath = BuildMenuIconShape(ref rcMenuIcon))
            {
                using (Pen pInner = new Pen(clr))
                {
                    g.DrawPath(
                      pInner,
                      XMenuIconPath
                    );
                }
            }
            
        
        }
        /// <summary>
        /// Checks if point is inside specific rectangle.
        /// </summary>
        /// <param name="p"> Point to check.</param>
        /// <param name="rc">Rectangle area.</param>
        /// <returns></returns>
        private bool PointInRect(Point p, Rectangle rc)
        { 
            if ((p.X > rc.Left && p.X < rc.Right &&
                p.Y > rc.Top && p.Y < rc.Bottom))
               return true;
            else
               return false;
        }
        private void DrawBackImage(
            Graphics gfx,
            Rectangle rc
            )
        {
            if (m_bmpBackImage != null)
            {
                int lW = m_bmpBackImage.Width;
                int lH = m_bmpBackImage.Height;
                Rectangle rcImage = new Rectangle(
                    0,
                    0,
                    lW,
                    lH
                    );

                switch (m_ImageAlign)
                {
                    case ContentAlignment.BottomCenter:
                        rcImage.X = rc.Width / 2 - lW / 2;
                        rcImage.Y = rc.Height - lH - 2;
                        break;
                    case ContentAlignment.BottomLeft:
                        rcImage.X = rc.Left + 2;
                        rcImage.Y = rc.Height - lH - 2;
                        break;
                    case ContentAlignment.BottomRight:
                        rcImage.X = rc.Right - lW - 2;
                        rcImage.Y = rc.Height - lH - 2;
                        break;
                    case ContentAlignment.MiddleCenter:
                        rcImage.X = rc.Width / 2 - lW / 2;
                        rcImage.Y = rc.Height / 2 - lH / 2;
                        break;
                    case ContentAlignment.MiddleLeft:
                        rcImage.X = rc.Left + 2;
                        rcImage.Y = rc.Height / 2 - lH / 2;
                        break;
                    case ContentAlignment.MiddleRight:
                        rcImage.X = rc.Right - lW - 2;
                        rcImage.Y = rc.Height / 2 - lH / 2;
                        break;
                    case ContentAlignment.TopCenter:
                        rcImage.X = rc.Width / 2 - lW / 2;
                        rcImage.Y = rc.Top + 2;
                        break;
                    case ContentAlignment.TopLeft:
                        rcImage.X = rc.Left + 2;
                        rcImage.Y = rc.Top + 2;
                        break;
                    case ContentAlignment.TopRight:
                        rcImage.X = rc.Right - lW - 2;
                        rcImage.Y = rc.Top + 2;
                        break;

                }

                gfx.DrawImage(
                    m_bmpBackImage,
                    rcImage
                );

            }

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GradualForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "GradualForm";
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.GradualForm_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GradualForm_MouseDown);
            this.ResumeLayout(false);

        }

        private int MakeLParam(int LoWord, int HiWord)
        {
            return (int)((HiWord << 16) | (LoWord & 0xffff));
        }

        private void GradualForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (!this.IsDisposed)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, MakeLParam(((System.Windows.Forms.Form)(sender)).Location.X + e.Location.X, ((System.Windows.Forms.Form)(sender)).Location.Y + e.Location.Y));
                }
            }
        }

        //const int WS_CLIPCHILDREN = 0x2000000;
        //const int WS_MINIMIZEBOX = 0x20000;
        //const int WS_MAXIMIZEBOX = 0x10000;
        //const int WS_SYSMENU = 0x80000;
        //const int CS_DBLCLKS = 0x8;
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;

        //        cp.Style = WS_CLIPCHILDREN | WS_MINIMIZEBOX | WS_SYSMENU;
        //        cp.ClassStyle = CS_DBLCLKS;

        //        return cp;
        //    }
        //}

        protected override void WndProc(ref Message m)
        {
            bool handled = false;

            if (m.Msg == WM_SYSCOMMAND)
            {
                // Check your window state here
                if (m.WParam == new IntPtr(SC_MAXIMIZE))
                {
                }
                
                if (m.WParam == new IntPtr(SC_RESTORE))
                {
                }
                if (m.WParam == new IntPtr(SC_MINIMIZE))
                {
                }
                if (m.WParam == new IntPtr(SC_RESTORE2)) // Restore form start menu
                {
                    // THe window is being maximized
                    if (this.WindowState.Equals(FormWindowState.Minimized))
                    {
                        if (PriorState.Equals(FormWindowState.Maximized))
                        {
                            this.WindowState = FormWindowState.Maximized;
                            handled = true;
                        }
                        else if (PriorState.Equals(FormWindowState.Normal))
                        {
                            this.WindowState = FormWindowState.Normal;
                            this.Size = new Size(m_rcRestoreBounds.Width, m_rcRestoreBounds.Height);
                            this.Location = new Point(m_rcRestoreBounds.Left, m_rcRestoreBounds.Top);
                            handled = true;
                        }
                    }
                }
            }
            else if (m.Msg == WM_NCLBUTTONDBLCLK) {}
            else if (m.Msg == WM_NCHITTEST || m.Msg == WM_MOUSEMOVE)
            {

                if (!TamanhoFixo)
                {
                    Size formSize = this.Size;
                    Point screenPoint = new Point(m.LParam.ToInt32());
                    Point clientPoint = this.PointToClient(screenPoint);
                    Dictionary<UInt32, Rectangle> boxes = null;

                    if (TamanhoFixoLaterais)
                    {
                        boxes = new Dictionary<UInt32, Rectangle>() 
                        {
                            {HTBOTTOM, new Rectangle(RESIZE_HANDLE_SIZE, formSize.Height - RESIZE_HANDLE_SIZE, formSize.Width - 2*RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE)},
                            {HTTOP, new Rectangle(RESIZE_HANDLE_SIZE, 0, formSize.Width - 2*RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE) },
                        };
                    }

                    if (TamanhoFixoRodape)
                    {
                        boxes = new Dictionary<UInt32, Rectangle>() 
                        {
                            {HTRIGHT, new Rectangle(formSize.Width - RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, formSize.Height - 2*RESIZE_HANDLE_SIZE)},
                            {HTLEFT, new Rectangle(0, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, formSize.Height - 2*RESIZE_HANDLE_SIZE) }
                        };
                    }

                    if (!TamanhoFixoLaterais && !TamanhoFixoRodape)
                    {
                        boxes = new Dictionary<UInt32, Rectangle>() 
                        {
                            {HTBOTTOMLEFT, new Rectangle(0, formSize.Height - RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE)},
                            {HTBOTTOM, new Rectangle(RESIZE_HANDLE_SIZE, formSize.Height - RESIZE_HANDLE_SIZE, formSize.Width - 2*RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE)},
                            {HTBOTTOMRIGHT, new Rectangle(formSize.Width - RESIZE_HANDLE_SIZE, formSize.Height - RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE)},
                            {HTRIGHT, new Rectangle(formSize.Width - RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, formSize.Height - 2*RESIZE_HANDLE_SIZE)},
                            {HTTOPRIGHT, new Rectangle(formSize.Width - RESIZE_HANDLE_SIZE, 0, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE) },
                            {HTTOP, new Rectangle(RESIZE_HANDLE_SIZE, 0, formSize.Width - 2*RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE) },
                            {HTTOPLEFT, new Rectangle(0, 0, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE) },
                            {HTLEFT, new Rectangle(0, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, formSize.Height - 2*RESIZE_HANDLE_SIZE) }
                        };
                    }
                    

                    foreach (KeyValuePair<UInt32, Rectangle> hitBox in boxes)
                    {

                        if (hitBox.Value.Contains(clientPoint))
                        {
                            m.Result = (IntPtr)hitBox.Key;
                            handled = true;
                            break;
                        }
                    }
                }
            }

            if (!handled)
            {
                base.WndProc(ref m);
            }
        }

        private void GradualForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    if (PointInRect(e.Location, m_rcTitleBar))
            //    {
            //        MouseEventArgs MaximizeMouseClick = new MouseEventArgs(e.Button, 1, m_GradualTitleBar.TitleBarButtons[Convert.ToInt16(GradualTitleBarButton.GradualTitleBarButtonType.Maximize)].XButtonLeft + 1,
            //            m_GradualTitleBar.TitleBarButtons[Convert.ToInt16(GradualTitleBarButton.GradualTitleBarButtonType.Maximize)].XButtonTop + 1,
            //            0);
            //        OnMouseUp(MaximizeMouseClick);
            //    }
            //}
        }

        public void MinimizarJanela()
        {
            PriorState = this.WindowState;
            if (!PriorState.Equals(FormWindowState.Maximized))
            {
                m_rcRestoreBounds = new Rectangle(this.Location, this.Size);
            }
            this.WindowState = FormWindowState.Minimized;
        }

        public void RestaurarJanela()
        {
            this.WindowState = FormWindowState.Normal;
        }

        public void MaximizarJanela()
        {
            if (this.WindowState.Equals(FormWindowState.Normal))
            {
                PriorState = this.WindowState;
                m_rcRestoreBounds = new Rectangle(this.Location, this.Size);
                this.WindowState = FormWindowState.Maximized;
                return;
            }
            else if (this.WindowState.Equals(FormWindowState.Maximized))
            {
                PriorState = this.WindowState;
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(m_rcRestoreBounds.Width, m_rcRestoreBounds.Height);
                this.Location = new Point(m_rcRestoreBounds.Left, m_rcRestoreBounds.Top);
                return;
            }
            return;
        }
    }
    
    public class Switch
    {
        public Switch(Object o)
        {
            Object = o;
        }

        public Object Object { get; private set; }
    }


    /// <summary>
    /// Extensions, because otherwise casing fails on Switch==null
    /// </summary>
    public static class SwitchExtensions
    {
        public static Switch Case<T>(this Switch s, Action<T> a)
              where T : class
        {
            return Case(s, o => true, a, false);
        }

        public static Switch Case<T>(this Switch s, Action<T> a,
             bool fallThrough) where T : class
        {
            return Case(s, o => true, a, fallThrough);
        }

        public static Switch Case<T>(this Switch s,
            Func<T, bool> c, Action<T> a) where T : class
        {
            return Case(s, c, a, false);
        }

        public static Switch Case<T>(this Switch s,
            Func<T, bool> c, Action<T> a, bool fallThrough) where T : class
        {
            if (s == null)
            {
                return null;
            }

            T t = s.Object as T;
            if (t != null)
            {
                if (c(t))
                {
                    a(t);
                    return fallThrough ? s : null;
                }
            }

            return s;
        }

    }
}
