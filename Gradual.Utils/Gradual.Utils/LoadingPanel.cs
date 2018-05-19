using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gradual.Utils
{
    public partial class LoadingPanel : UserControl
    {
        #region Constants
        private readonly Color BackgroundFadeColor = Color.FromArgb(75, Color.Black);
        #endregion

        #region Constructors
        public LoadingPanel()
        {
            DoubleBuffered = true;

            InitializeComponent();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or Sets the main form that will be used as a background canvas for the loading form.
        /// </summary>
        public Form BackgroundForm { get; set; }

        /// <summary>
        /// Gets or Sets the text to displayed as the progress text.
        /// </summary>
        public string Title
        {
            get
            {
                return lblText.Text;
            }

            set
            {
                lblText.Text = value;
            }
        }

        /// <summary>
        /// Gets or Sets the value of the progress bar.
        /// </summary>
        public int? Progress
        {
            get
            {
                if (progressBar.Style == ProgressBarStyle.Marquee)
                {
                    return null;
                }
                else
                {
                    return progressBar.Value;
                }
            }

            set
            {
                if (value == null)
                {
                    progressBar.Style = ProgressBarStyle.Marquee;
                    progressBar.Value = 100;

                    lblProgress.Visible = false;
                }
                else
                {
                    //progressBar.Style = ProgressBarStyle.Continuous;
                    //progressBar.Value = value.Value;

                    lblProgress.Text = string.Format("{0}", value);
                    
                    if (!lblProgress.Visible)
                    {
                        lblProgress.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or Sets a value to indicate if the background form should be faded out.
        /// </summary>
        public bool UseFadedBackground { get; set; }
        #endregion

        #region Base Events
        private void ModalLoadingUI_Load(object sender, EventArgs e)
        {
            if (this.BackgroundForm != null)
            {
                this.Location = this.BackgroundForm.Location;
                this.Size = this.BackgroundForm.Size;
            }
        }

        private void ModalLoadingUI_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                if (this.BackgroundForm != null)
                {
                    this.Location = this.BackgroundForm.Location;
                }
            }
        }

        private void ModalLoadingUI_Shown(object sender, EventArgs e)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Paints the background form as the background of this form, if one is defined.
        /// </summary>
        public void CaptureBackgroundForm()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(CaptureBackgroundForm));
                return;
            }

            if (this.BackgroundForm == null)
            {
                return;
            }


            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmpScreenshot);

            try
            {
                // COPY BACKGROUND
                int x = this.BackgroundForm.Left + this.BackgroundForm.Padding.Left;
                int y = this.BackgroundForm.Top + this.BackgroundForm.Padding.Top;
                var size = this.BackgroundForm.Size;
                size.Height -= this.BackgroundForm.Padding.Vertical;
                size.Width -= this.BackgroundForm.Padding.Horizontal;
                g.CopyFromScreen(x, y, 0, 0, size, CopyPixelOperation.SourceCopy);

                // FADE IF DESIRED
                if (this.UseFadedBackground == true)
                {
                    var rect = new Rectangle(0, 0, size.Width, size.Height);

                    g.FillRectangle(new SolidBrush(BackgroundFadeColor), rect);
                }
            }
            catch (Exception ex)
            {
                g.Clear(Color.White);
            }

            this.BackgroundImage = bmpScreenshot;
        }

        private void frmBase_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals((char)Keys.Escape))
            {
                this.Visible = false;
            }

        }

        #endregion

    }
}
