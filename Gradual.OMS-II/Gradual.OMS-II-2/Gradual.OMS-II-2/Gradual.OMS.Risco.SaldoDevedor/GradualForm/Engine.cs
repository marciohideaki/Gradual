using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradualForm
{
    public static class Engine
    {

        /********** Filter **********/
        private static MontagemFiltro telaFiltro = null;
        private static System.Windows.Forms.Panel painelControle = new System.Windows.Forms.Panel();
        static List<string> gListaClientes = new List<string>();
        private static List<int> gListaClienteSelecionadosFiltro = new List<int>();
        public static event EventHandler<FilterEventArgs> EventFiltrar;
        /****************************/

        public static void ConfigureFormRender(System.Windows.Forms.Control pControl)
        {
            foreach (System.Windows.Forms.Control lControl in pControl.Controls)
            {
                ConfigureControl(lControl);

                lControl.Paint += new System.Windows.Forms.PaintEventHandler(Control_Paint);

                if (lControl.GetType().Equals(typeof(System.Windows.Forms.Panel)) || lControl.GetType().Equals(typeof(Controls.FlatTabControl)) || lControl.GetType().Equals(typeof(System.Windows.Forms.TabPage)))
                {
                    ConfigureFormRender(lControl);
                }

                if (pControl.ContextMenuStrip != null)
                {
                    ConfigureControl_ContextMenuStrip(pControl.ContextMenuStrip);
                }
            }
        }

        public static void ConfigureControl(System.Windows.Forms.Control pControl)
        {
            new Switch(pControl)
                .Case<System.Windows.Forms.ToolStrip>
                    (
                        action =>
                        {
                            ConfigureControl_ToolStrip((System.Windows.Forms.ToolStrip)pControl);
                        }
                    )
                .Case<System.Windows.Forms.StatusStrip>
                    (
                        action =>
                        {
                            ConfigureControl_StatusStrip((System.Windows.Forms.StatusStrip)pControl);

                            for (int i = 0; i < ((System.Windows.Forms.StatusStrip)pControl).Items.Count; i++)
                            {
                                if (((System.Windows.Forms.StatusStrip)pControl).Items[i].GetType() == typeof(System.Windows.Forms.ToolStripButton))
                                {
                                    ConfigureControl_ToolStripButton((System.Windows.Forms.ToolStripButton)((System.Windows.Forms.StatusStrip)pControl).Items[i]);
                                }
                            }
                        }
                    )
                .Case<System.Windows.Forms.TextBox>
                    (
                        action =>
                        {
                            //ConfigureControl_TextBox((System.Windows.Forms.TextBox)pControl);
                        }
                    )
                .Case<System.Windows.Forms.RadioButton>
                    (
                        action =>
                        {
                            ConfigureControl_RadioButton((System.Windows.Forms.RadioButton)pControl);
                        }
                    )
                .Case<System.Windows.Forms.Button>
                    (
                        action =>
                        {
                            //OnPaint_Button((System.Windows.Forms.Button)sender, e);
                        }
                    )
                .Case<System.Windows.Forms.DataGridView>
                    (
                        action =>
                        {
                            ConfigureControl_DataGridView((System.Windows.Forms.DataGridView)pControl);
                        }
                    )
                .Case<Controls.CustomDataGridView>
                    (
                        action =>
                        {
                            ConfigureControl_DataGridView((System.Windows.Forms.DataGridView)pControl);
                        }
                    )
                .Case<Controls.CustomDataGridViewResumo>
                    (
                        action =>
                        {
                            ConfigureControl_DataGridView((System.Windows.Forms.DataGridView)pControl);
                        }
                    )
                .Case<System.Windows.Forms.TreeView>
                    (
                        action =>
                        {
                            ConfigureControl_TreeView((System.Windows.Forms.TreeView)pControl);
                        }
                    );

            if (pControl.ContextMenuStrip != null)
            {
                ConfigureControl_ContextMenuStrip(pControl.ContextMenuStrip);
            }
        }

        public static void ConfigureControl_ComboBox(System.Windows.Forms.ComboBox pControl)
        {
            pControl.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            pControl.BackColor = System.Drawing.Color.White; // ConfiguracoesDeEstilo.Cor_Fd_TextBox;
            pControl.ForeColor = System.Drawing.Color.Black;// ConfiguracoesDeEstilo.Cor_Txt_TextBox;

            pControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            System.Windows.Forms.Control lParent = pControl.Parent;

            System.Windows.Forms.Panel lPanel = new System.Windows.Forms.Panel();

            ////Linha interna de cima:
            //DesenharLinhaViaPainel(ref lParent, pControl.Location.X, pControl.Location.Y, pControl.Width, 1, StyleSettings.Cor_Borda_TextBox);

            ////Linha interna direita:
            //DesenharLinhaViaPainel(ref lParent, pControl.Location.X + pControl.Width - 1, pControl.Location.Y, 1, pControl.Height, StyleSettings.Cor_Borda_TextBox);

            ////Linha interna baixo:
            //DesenharLinhaViaPainel(ref lParent, pControl.Location.X, pControl.Location.Y + pControl.Height - 1, pControl.Width, 1, StyleSettings.Cor_Borda_TextBox);

            ////Linha interna esquerda:
            //DesenharLinhaViaPainel(ref lParent, pControl.Location.X, pControl.Location.Y, 1, pControl.Height, StyleSettings.Cor_Borda_TextBox);

            //pComboBox.SendToBack();
        }


        public static void ConfigureControl_ContextMenuStrip(System.Windows.Forms.ContextMenuStrip pControl)
        {
            if (pControl.Tag == null || (pControl.Tag != null && pControl.Tag != "SemRenderizacao"))
            {
                pControl.BackColor = StyleSettings.Form_BackGround_Color;

                pControl.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
                pControl.Renderer = new MenuRenderer();

                //foreach (ToolStripMenuItem lItem in pMenuStrip.Items)
                for (int i = 0; i < pControl.Items.Count; i++)
                {
                    if (pControl.Items[i].GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
                    {
                        //lItem.ForeColor = ConfiguracoesDeEstilo.Cor_Txt_MenuItems;
                        //PrepararToolStripMenuItem(lItem);
                        ConfigureControl_ToolStripMenuItem((System.Windows.Forms.ToolStripMenuItem)pControl.Items[i]);
                    }
                }
            }
        }

        public static void ConfigureControl_ToolStrip(System.Windows.Forms.ToolStrip pControl)
        {
            //if (pControl.FindForm().Name.Equals("frmMenu"))
            //{
            //    pControl.FindForm().Height = 46;
            //    pControl.BringToFront();
            //}

            if (pControl.Tag == null || (pControl.Tag != null && pControl.Tag != "SemRenderizacao"))
            {
                pControl.AllowDrop = false;
                pControl.AllowMerge = false;
                pControl.AllowItemReorder = false;
                pControl.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;

                pControl.Renderer = new MenuRenderer();
                //pToolStrip.RenderMode = ToolStripRenderMode.Custom;

                pControl.Font = new System.Drawing.Font("Tahoma", 8.25F);

                //pControl.Dock = System.Windows.Forms.DockStyle.None;

                //pControl.Width = pControl.FindForm().Width + 4;

                //pControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left;

                //pControl.Location = new System.Drawing.Point(StyleSettings.LarguraDaBorda_Form, StyleSettings.LarguraDaBorda_Form);

                foreach (System.Windows.Forms.ToolStripItem lItem in pControl.Items)
                {
                    lItem.ForeColor = StyleSettings.Cor_Txt_MenuItems;

                    if (lItem.GetType() == typeof(System.Windows.Forms.ToolStripDropDownButton))
                    {
                        System.Windows.Forms.ToolStripDropDownButton lButton = (System.Windows.Forms.ToolStripDropDownButton)lItem;

                        if (lButton.DropDownItems != null)
                        {
                            try
                            {
                                for (int a = 0; a < lButton.DropDownItems.Count; a++)
                                {
                                    if (lButton.DropDownItems[a].GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
                                    {
                                        ConfigureControl_ToolStripMenuItem((System.Windows.Forms.ToolStripMenuItem)lButton.DropDownItems[a]);
                                    }
                                }
                            }
                            catch { }   //separators dão exception...
                        }
                    }
                }

                //TODO: Review... new implementation is needed
                //pToolStrip.MouseDown += new MouseEventHandler(((frmBase)pToolStrip.FindForm()).frmBase_MouseDown);
            }
        }

        public static void ConfigureControl_ToolStripButton(System.Windows.Forms.ToolStripButton pControl)
        {
            if (pControl.Tag == null || (pControl.Tag != null && pControl.Tag != "SemRenderizacao"))
            {
                pControl.BackColor = StyleSettings.Button_BackGround_ColorStatus;
                pControl.ForeColor = StyleSettings.Button_Text_ForecolorStatus;
            }
        }

        public static void ConfigureControl_ToolStripMenuItem(System.Windows.Forms.ToolStripMenuItem pControl)
        {
            if (pControl.Tag == null || (pControl.Tag != null && pControl.Tag != "SemRenderizacao"))
            {
                pControl.ForeColor = StyleSettings.Cor_Txt_MenuItems;
                pControl.BackColor = StyleSettings.Form_BackGround_Color;

                if (pControl.HasDropDownItems)
                {
                    foreach (object lItem in pControl.DropDownItems)
                    {
                        ConfigureControl_ToolStripMenuItem((System.Windows.Forms.ToolStripMenuItem)lItem);
                    }
                }
            }
        }

        public static void ConfigureControl_StatusStrip(System.Windows.Forms.StatusStrip pControl)
        {
            if (pControl.Tag == null || (pControl.Tag != null && pControl.Tag != "SemRenderizacao"))
            {
                //TODO: criar parametro proprio
                pControl.BackColor = StyleSettings.Form_BackGround_Color;
            }
        }

        public static void ConfigureControl_Label(System.Windows.Forms.Label pControl)
        {
            if (pControl.Tag == null || (pControl.Tag != null && pControl.Tag != "SemRenderizacao"))
            {
                //TODO: implementation needed
            }
        }

        public static void ConfigureControl_TextBox(System.Windows.Forms.TextBox pControl)
        {
            if (pControl.Tag == null || (pControl.Tag != null && pControl.Tag != "SemRenderizacao"))
            {
                //pTextBox.BorderStyle = BorderStyle.None;
                pControl.BackColor = StyleSettings.TextBox_BackGround_Color_Start;
                pControl.ForeColor = StyleSettings.TextBox_Text_ForeColor;

                // Guardar as dimensões do TextBox na Tag do controle
                if (pControl.Tag == null)
                {
                    System.Drawing.Rectangle lRect = new System.Drawing.Rectangle(pControl.Location.X, pControl.Location.Y, pControl.Width - 1, pControl.Height - 4);
                    pControl.Tag = lRect;

                    pControl.Location = new System.Drawing.Point(pControl.Location.X + 1, pControl.Location.Y + 1);
                    pControl.Size = new System.Drawing.Size(pControl.Width - 2, pControl.Height);
                }
            }
        }

        public static void ConfigureControl_RadioButton(System.Windows.Forms.RadioButton pControl)
        {
            if (pControl.Tag == null || (pControl.Tag != null && pControl.Tag != "SemRenderizacao"))
            {
                //pTextBox.BorderStyle = BorderStyle.None;
                pControl.BackColor = StyleSettings.Label_BackGround_Color_Start;
                pControl.ForeColor = StyleSettings.Label_Text_ForeColor;
            }
        }

        public static void ConfigureControl_DataGridView(System.Windows.Forms.DataGridView pControl)
        {
            if (pControl.Tag == null || (pControl.Tag != null && pControl.Tag != "SemRenderizacao"))
            {
                //pControl.BackgroundColor = StyleSettings.Cor_Fd_Grid;
                //pGrid.GridColor = ConfiguracoesDeEstilo.Cor_LinhaGrid;

                pControl.ColumnHeadersDefaultCellStyle.ForeColor = StyleSettings.Cor_Txt_CabecalhoGrid;
                pControl.ColumnHeadersDefaultCellStyle.BackColor = StyleSettings.Cor_Fd_CabecalhoGrid;
                pControl.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                pControl.ColumnHeadersDefaultCellStyle.Font = StyleSettings.Fonte_Grid;

                pControl.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;

                //pControl.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(DataGridView_OnCellPainting);
                pControl.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(DataGridView_OnColumnHeaderMouseClick);

                pControl.RowsDefaultCellStyle.ForeColor = StyleSettings.Cor_Txt_LinhaGrid;
                pControl.RowsDefaultCellStyle.BackColor = StyleSettings.Cor_Fd_LinhaGridA;

                if (pControl.GetType() == typeof(Controls.CustomDataGridViewResumo))
                {
                    pControl.RowsDefaultCellStyle.Font = StyleSettings.Fonte_Gabriel;
                    pControl.AlternatingRowsDefaultCellStyle.Font = StyleSettings.Fonte_Gabriel;
                }
                else
                {
                    pControl.RowsDefaultCellStyle.Font = StyleSettings.Fonte_Grid;
                    pControl.AlternatingRowsDefaultCellStyle.Font = StyleSettings.Fonte_Grid;
                }

                pControl.AlternatingRowsDefaultCellStyle.ForeColor = StyleSettings.Cor_Txt_LinhaGrid;
                pControl.AlternatingRowsDefaultCellStyle.BackColor = StyleSettings.Cor_Fd_LinhaGridB;

                pControl.RowsDefaultCellStyle.SelectionBackColor = StyleSettings.Cor_Fd_LinhaGrid_Selecionada;
                pControl.RowsDefaultCellStyle.SelectionForeColor = StyleSettings.Cor_Txt_LinhaGrid_Selecionada;

                pControl.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
                pControl.RowHeadersVisible = false;
            }
        }

        public static void ConfigureControl_DataGridView_Verdana(System.Windows.Forms.DataGridView pControl)
        {
            if (pControl.Tag == null || (pControl.Tag != null && pControl.Tag != "SemRenderizacao"))
            {
                ConfigureControl_DataGridView(pControl);
                pControl.AlternatingRowsDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, System.Drawing.FontStyle.Regular);
                pControl.RowsDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, System.Drawing.FontStyle.Regular);
            }
        }

        public static void ConfigureControl_TreeView(System.Windows.Forms.TreeView pControl)
        {
            if (pControl.Tag == null || (pControl.Tag != null && pControl.Tag != "SemRenderizacao"))
            {
                pControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
                pControl.BackColor = StyleSettings.TextBox_BackGround_Color_Start;
                pControl.ForeColor = StyleSettings.TextBox_Text_ForeColor;
                pControl.LineColor = StyleSettings.TextBox_Text_ForeColor;
            }
        }


        #region Paint
        
        private static void Control_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            new Switch(sender)
                .Case<System.Windows.Forms.Panel>
                    (
                        action => 
                        { 
                            OnPaint_Panel((System.Windows.Forms.Panel)sender, e); 
                        }
                    )
                .Case<System.Windows.Forms.Label>
                    (
                        action => 
                        { 
                            OnPaint_Label((System.Windows.Forms.Label)sender, e); 
                        }
                    )
                .Case<System.Windows.Forms.TextBox>
                    (
                        action => 
                        { 
                            OnPaint_TextBox((System.Windows.Forms.TextBox)sender, e); 
                        }
                    )
                .Case<System.Windows.Forms.CheckBox>
                    (
                        action =>
                        {
                            OnPaint_CheckBox((System.Windows.Forms.CheckBox)sender, e);
                        }
                    )
                .Case<System.Windows.Forms.Button>
                    (   
                        action => 
                        { 
                            OnPaint_Button((System.Windows.Forms.Button)sender, e); 
                        }
                    );

        }

        private static void OnPaint_Panel(System.Windows.Forms.Panel pPanel, System.Windows.Forms.PaintEventArgs e)
        {
            if (pPanel.ClientRectangle.X > 0 && pPanel.ClientRectangle.Y > 0)
            {
                if (pPanel.Tag == null || (pPanel.Tag != null && pPanel.Tag != "SemRenderizacao"))
                {
                    using (GradualAntiAlias xaa = new GradualAntiAlias(e.Graphics))
                    {
                        using (System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(pPanel.ClientRectangle, StyleSettings.Panel_BackGround_Color_Start, StyleSettings.Panel_BackGround_Color_Finish, 90F))
                        {
                            e.Graphics.FillRectangle(brush, pPanel.ClientRectangle);

                            e.Graphics.DrawRectangle(
                                new System.Drawing.Pen(
                                    new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(42, 42, 42)), 0.5F),
                                    e.ClipRectangle);
                        }
                    }
                }
            }
        }

        private static void OnPaint_Label(System.Windows.Forms.Label pLabel, System.Windows.Forms.PaintEventArgs e)
        {
            if (pLabel.Tag == null || (pLabel.Tag != null && !pLabel.Tag.Equals("SemRenderizacao")))
            {
                using (GradualAntiAlias xaa = new GradualAntiAlias(e.Graphics))
                {
                    using (System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(pLabel.ClientRectangle, StyleSettings.Label_BackGround_Color_Start, StyleSettings.Label_BackGround_Color_Finish, 45F))
                    {

                        e.Graphics.FillRectangle(brush, pLabel.ClientRectangle);

                        e.Graphics.DrawRectangle(
                            new System.Drawing.Pen(
                                new System.Drawing.SolidBrush(StyleSettings.Panel_BackGround_Color_Start), 0.5F),
                                e.ClipRectangle);

                        Int32 numAlign = (Int32)Math.Log((Double)pLabel.TextAlign, 2);
                        System.Drawing.StringFormat formatoLabel = new System.Drawing.StringFormat() { Alignment = (System.Drawing.StringAlignment)(numAlign % 4), LineAlignment = (System.Drawing.StringAlignment)(numAlign / 4) };

                        if (pLabel.Image != null)
                        {
                            if (string.IsNullOrEmpty(pLabel.Text))
                            {
                                e.Graphics.DrawImage(pLabel.Image, new System.Drawing.Rectangle(pLabel.ClientRectangle.X + StyleSettings.DeslocaImagem_Label, pLabel.ClientRectangle.Y + StyleSettings.DeslocaImagem_Label, pLabel.Image.Width, pLabel.Image.Height));
                            }
                            else
                            {
                                //e.Graphics.DrawImage(pLabel.Image, new System.Drawing.Rectangle(pLabel.ClientRectangle.X + StyleSettings.DeslocaImagem_Label, pLabel.ClientRectangle.Y + StyleSettings.DeslocaImagem_Label, pLabel.Image.Width, pLabel.Image.Height));
                                e.Graphics.DrawString(pLabel.Text, pLabel.Font, StyleSettings.Brush_Txt_Botao, pLabel.ClientRectangle, formatoLabel);
                            }
                        }
                        else
                        {
                            e.Graphics.DrawString(pLabel.Text, pLabel.Font, StyleSettings.Brush_Txt_Label, pLabel.ClientRectangle, formatoLabel);
                        }
                    }
                }
            }
        }

        private static void OnPaint_TextBox(System.Windows.Forms.TextBox pTextBox, System.Windows.Forms.PaintEventArgs e)
        {
            //TODO: implementation needed
        }

        private static void OnPaint_CheckBox(System.Windows.Forms.CheckBox pCheckBox, System.Windows.Forms.PaintEventArgs e)
        {
            if (pCheckBox.Tag == null || (pCheckBox.Tag != null && !pCheckBox.Tag.Equals("SemRenderizacao")))
            {
                using (GradualAntiAlias xaa = new GradualAntiAlias(e.Graphics))
                {
                    System.Drawing.Rectangle clientRect = new System.Drawing.Rectangle(pCheckBox.ClientRectangle.X, pCheckBox.ClientRectangle.Y + (pCheckBox.Height - 16) / 2, 16, 16);

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                    e.Graphics.FillRectangle(new System.Drawing.SolidBrush(StyleSettings.Panel_BackGround_Color_Start), pCheckBox.ClientRectangle);

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    e.Graphics.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(clientRect, StyleSettings.CheckBox_BackGround_Color_Start, StyleSettings.CheckBox_BackGround_Color_Finish, 90F), clientRect.X + 2, clientRect.Y + 2, 12, 12);

                    e.Graphics.DrawRectangle(StyleSettings.Pen_Borda_TextBox, clientRect.X + 2, clientRect.Y + 2, 12, 12);

                    if (pCheckBox.Checked)
                    {
                        e.Graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), clientRect.X + 4, clientRect.Y + 4, clientRect.Width - 8, clientRect.Height - 8);
                    }

                    //clientRect = new System.Drawing.Rectangle(clientRect.X + 20, clientRect.Y + 3, pCheckBox.ClientRectangle.Width - 20, clientRect.Height);
                    clientRect = new System.Drawing.Rectangle(clientRect.X - 15, clientRect.Y + 1, pCheckBox.ClientRectangle.Width - 15, clientRect.Height);

                    e.Graphics.DrawString(pCheckBox.Text, pCheckBox.Font, StyleSettings.Brush_Txt_Label, clientRect, StyleSettings.FormatoDaFonte_Botao);
                }
            }
        }

        private static void OnPaint_Button(System.Windows.Forms.Button pButton, System.Windows.Forms.PaintEventArgs e)
        {
            if (pButton.Tag == null || (pButton.Tag != null && !pButton.Tag.Equals("SemRenderizacao")))
            {
                using (GradualAntiAlias xaa = new GradualAntiAlias(e.Graphics))
                {
                    using (System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(pButton.ClientRectangle, StyleSettings.Button_BackGround_Color_Start, StyleSettings.Button_BackGround_Color_Finish, 45F))
                    {
                        e.Graphics.FillRectangle(brush, pButton.ClientRectangle);
                    }
                }

                e.Graphics.DrawRectangle(StyleSettings.Pen_Borda_Botao
                                        , pButton.ClientRectangle.X
                                        , pButton.ClientRectangle.Y
                                        , pButton.ClientRectangle.Width - 1
                                        , pButton.ClientRectangle.Height - 1);

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                if (pButton.Image != null)
                {
                    if (string.IsNullOrEmpty(pButton.Text))
                    {
                        e.Graphics.DrawImage(pButton.Image, new System.Drawing.Rectangle(pButton.ClientRectangle.X + StyleSettings.DeslocaImagem_Botao, pButton.ClientRectangle.Y + StyleSettings.DeslocaImagem_Botao, pButton.Image.Width, pButton.Image.Height));
                    }
                    else
                    {
                        e.Graphics.DrawImage(pButton.Image, new System.Drawing.Rectangle(pButton.ClientRectangle.X + StyleSettings.DeslocaImagem_Botao, pButton.ClientRectangle.Y + StyleSettings.DeslocaImagem_Botao, pButton.Image.Width, pButton.Image.Height));
                        e.Graphics.DrawString(pButton.Text, pButton.Font, StyleSettings.Brush_Txt_Botao, pButton.ClientRectangle, StyleSettings.FormatoDaFonte_Botao);
                    }
                }
                else
                {
                    e.Graphics.DrawString(pButton.Text, pButton.Font, StyleSettings.Brush_Txt_Botao, pButton.ClientRectangle, StyleSettings.FormatoDaFonte_Botao);
                }
            }
        }
        #endregion

        private static void DataGridView_OnCellPainting(object sender, System.Windows.Forms.DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                e.PaintBackground(e.CellBounds, true);
                //e.Graphics.FillRectangle(Brushes.Red, e.CellBounds);

                e.PaintContent(e.CellBounds);
                //(((System.Windows.Forms.Control)(sender)).TopLevelControl).GetType().Name
                e.Graphics.DrawLine(StyleSettings.Pen_CabecalhoGrid_Div1
                                    , e.CellBounds.X + e.CellBounds.Width - 2
                                    , 3
                                    , e.CellBounds.X + e.CellBounds.Width - 2
                                    , e.CellBounds.Height - 2
                                    );

                System.Windows.Forms.DataGridView grid = (System.Windows.Forms.DataGridView)sender;

                if (grid.Columns[e.ColumnIndex].Tag != null)
                {
                    System.Drawing.Bitmap imagem = new System.Drawing.Bitmap(Properties.Resources.Ico_Filtrar_8x8);
                    e.Graphics.DrawImage(imagem, new System.Drawing.Rectangle(e.CellBounds.X + e.CellBounds.Width - imagem.Width - StyleSettings.DeslocaImagem_Botao, e.CellBounds.Y + StyleSettings.DeslocaImagem_Botao, imagem.Width, imagem.Height));
                    ((FilterSettings.Filtro)grid.Columns[e.ColumnIndex].Tag).Nome = grid.Columns[e.ColumnIndex].HeaderText;
                    ((FilterSettings.Filtro)grid.Columns[e.ColumnIndex].Tag).PosicaoCabecalhoGrid = e.CellBounds;
                }

                e.Handled = true;
            }
        }

        private static void DataGridView_OnColumnHeaderMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            System.Windows.Forms.DataGridView grid = (System.Windows.Forms.DataGridView)sender;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (grid.Columns[e.ColumnIndex].Tag != null && telaFiltro == null)
                {
                    telaFiltro = new MontagemFiltro();

                    FilterSettings.Filtro filtro = (FilterSettings.Filtro)grid.Columns[e.ColumnIndex].Tag;

                    telaFiltro.TituloFiltro = new System.Windows.Forms.Label();
                    telaFiltro.TituloFiltro.Location = new System.Drawing.Point(filtro.PosicaoCabecalhoGrid.X + 1, filtro.PosicaoCabecalhoGrid.Y + 1);
                    telaFiltro.TituloFiltro.Size = new System.Drawing.Size(filtro.PosicaoCabecalhoGrid.Width - 3, filtro.PosicaoCabecalhoGrid.Height - 1);
                    telaFiltro.TituloFiltro.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    telaFiltro.TituloFiltro.Font = StyleSettings.Fonte_Grid;
                    telaFiltro.TituloFiltro.Text = filtro.Nome;
                    telaFiltro.TituloFiltro.BackColor = System.Drawing.Color.FromArgb(120, 120, 120);
                    telaFiltro.TituloFiltro.ForeColor = System.Drawing.Color.White;
                    grid.Controls.Add(telaFiltro.TituloFiltro);

                    telaFiltro.PainelFiltro = new System.Windows.Forms.Panel();
                    telaFiltro.PainelFiltro.Location = new System.Drawing.Point(filtro.PosicaoCabecalhoGrid.X, filtro.PosicaoCabecalhoGrid.Y + filtro.PosicaoCabecalhoGrid.Height);
                    telaFiltro.PainelFiltro.Size = filtro.Tamanho;
                    telaFiltro.PainelFiltro.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    telaFiltro.PainelFiltro.BackColor = System.Drawing.Color.FromArgb(120, 120, 120);
                    grid.Controls.Add(telaFiltro.PainelFiltro);


                    painelControle.Name = "painelControle";
                    painelControle.Location = new System.Drawing.Point(1, 1);
                    painelControle.Size = new System.Drawing.Size(telaFiltro.PainelFiltro.Size.Width - 2, telaFiltro.PainelFiltro.Size.Height - 2);
                    painelControle.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
                    telaFiltro.PainelFiltro.Controls.Add(painelControle);

                    System.Windows.Forms.RadioButton chkAscendente = new System.Windows.Forms.RadioButton();
                    chkAscendente.Text = "Ascendente";
                    chkAscendente.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold);
                    chkAscendente.Location = new System.Drawing.Point(10, 5);
                    chkAscendente.BackColor = System.Drawing.Color.Transparent;
                    chkAscendente.ForeColor = System.Drawing.Color.White;
                    chkAscendente.Checked = filtro.Ascendente;
                    painelControle.Controls.Add(chkAscendente);

                    System.Windows.Forms.RadioButton chkDescendente = new System.Windows.Forms.RadioButton();
                    chkDescendente.Text = "Descendente";
                    chkDescendente.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold);
                    chkDescendente.Location = new System.Drawing.Point(10, 25);
                    chkDescendente.BackColor = System.Drawing.Color.Transparent;
                    chkDescendente.ForeColor = System.Drawing.Color.White;
                    chkDescendente.Checked = !filtro.Ascendente;
                    painelControle.Controls.Add(chkDescendente);

                    System.Windows.Forms.GroupBox linha = new System.Windows.Forms.GroupBox();
                    linha.Location = new System.Drawing.Point(5, 50);
                    linha.Size = new System.Drawing.Size(telaFiltro.PainelFiltro.Size.Width - 10, 1);
                    linha.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
                    painelControle.Controls.Add(linha);

                    int posY = 65;
                    foreach (FilterSettings.CamposFiltro campo in filtro.CamposTextBox)
                    {
                        System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                        label.Text = campo.Descricao;
                        label.Location = new System.Drawing.Point(10, posY);
                        label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                        label.Size = campo.DescricaoTamanho;
                        label.ForeColor = System.Drawing.Color.White;
                        painelControle.Controls.Add(label);

                        System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
                        textBox.Text = (String)campo.Conteudo;
                        textBox.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold);
                        textBox.Location = new System.Drawing.Point(20 + label.Size.Width, posY - 5);
                        textBox.Size = campo.ConteudoTamanho;
                        painelControle.Controls.Add(textBox);

                        posY += 28;
                    }

                    foreach (FilterSettings.CamposFiltro campo in filtro.CamposBuscaTextBox)
                    {
                        System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                        label.Text = campo.Descricao;
                        label.Location = new System.Drawing.Point(10, posY);
                        label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                        label.Size = campo.DescricaoTamanho;
                        label.ForeColor = System.Drawing.Color.White;
                        painelControle.Controls.Add(label);

                        System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
                        textBox.Text = (String)campo.Conteudo;
                        textBox.Font = new System.Drawing.Font("Calibri", 11.25f, System.Drawing.FontStyle.Bold);
                        textBox.Location = new System.Drawing.Point(20 + label.Size.Width, posY - 5);
                        textBox.Size = campo.ConteudoTamanho;
                        //textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                        textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(textBox_KeyDown);
                        painelControle.Controls.Add(textBox);

                        posY += 28;
                    }

                    foreach (FilterSettings.CamposFiltro campo in filtro.CamposComboBox)
                    {
                        System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                        label.Text = campo.Descricao;
                        label.Location = new System.Drawing.Point(10, posY);
                        label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                        label.Size = campo.DescricaoTamanho;
                        label.ForeColor = System.Drawing.Color.White;
                        label.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
                        painelControle.Controls.Add(label);

                        System.Windows.Forms.ComboBox comboBox = new System.Windows.Forms.ComboBox();
                        comboBox.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
                        foreach (String item in (List<String>)campo.Conteudo)
                            comboBox.Items.Add(item);
                        comboBox.SelectedIndex = 0;
                        comboBox.Location = new System.Drawing.Point(15 + label.Size.Width, posY);
                        comboBox.Size = campo.ConteudoTamanho;
                        painelControle.Controls.Add(comboBox);

                        posY += 28;
                    }

                    foreach (FilterSettings.CamposFiltro campo in filtro.CamposCheckBox)
                    {
                        System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                        label.Text = campo.Descricao;
                        label.Location = new System.Drawing.Point(10, posY);
                        label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                        label.Size = campo.DescricaoTamanho;
                        label.ForeColor = System.Drawing.Color.White;
                        painelControle.Controls.Add(label);

                        System.Windows.Forms.CheckedListBox checkedListBox = new System.Windows.Forms.CheckedListBox();
                        checkedListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        checkedListBox.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
                        foreach (String item in (List<String>)campo.Conteudo)
                        {
                            if (!gListaClientes.Contains(item))
                            {
                                gListaClientes.Add(item);
                            }

                            checkedListBox.Items.Add(item);
                        }
                        //checkedListBox.SelectedIndex = 0;
                        checkedListBox.Location = new System.Drawing.Point(15 + label.Size.Width, posY);
                        checkedListBox.Size = campo.ConteudoTamanho;
                        checkedListBox.CheckOnClick = true;
                        checkedListBox.Name = "checkedListBox";
                        checkedListBox.SelectedIndexChanged += new EventHandler(CheckedListBox_OnSelectedIndexChanged);
                        painelControle.Controls.Add(checkedListBox);

                        posY += 28;
                    }

                    if (filtro.BotaoOK != null)
                    {
                        System.Windows.Forms.Button botaoOk = new System.Windows.Forms.Button();
                        botaoOk.Text = "Filtrar";
                        botaoOk.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
                        botaoOk.Location = new System.Drawing.Point(painelControle.Size.Width - 160, painelControle.Size.Height - 35);
                        botaoOk.Size = new System.Drawing.Size(70, 25);
                        //botaoOk.Click += new EventHandler<FiltrarEventArgs>(filtro.BotaoOK.Handler);
                        botaoOk.Click -= new EventHandler(FilterButton_OnClick);
                        botaoOk.Click += new EventHandler(FilterButton_OnClick);
                        //EventFiltrar += filtro.BotaoOK.Handler;
                        //botaoOk.Click += new EventHandler(filtro.BotaoOK.Handler);
                        painelControle.Controls.Add(botaoOk);
                    }
                    else
                    {
                        System.Windows.Forms.Button botaoOk = new System.Windows.Forms.Button();
                        botaoOk.Text = "Filtrar";
                        botaoOk.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
                        botaoOk.Location = new System.Drawing.Point(painelControle.Size.Width - 160, painelControle.Size.Height - 35);
                        botaoOk.Size = new System.Drawing.Size(70, 25);
                        botaoOk.Click += new EventHandler(FilterButtonOK_OnClick);
                        painelControle.Controls.Add(botaoOk);
                    }
                    System.Windows.Forms.Button botaoCancel = new System.Windows.Forms.Button();
                    botaoCancel.Text = "Cancelar";
                    botaoCancel.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold);
                    botaoCancel.Location = new System.Drawing.Point(painelControle.Size.Width - 80, painelControle.Size.Height - 35);
                    botaoCancel.Size = new System.Drawing.Size(70, 25);
                    botaoCancel.Click += new EventHandler(FilterButtonCancel_OnClick);
                    painelControle.Controls.Add(botaoCancel);

                    painelControle.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(PanelControle_OnPreviewKeyDown);

                    ConfigureControl(telaFiltro.PainelFiltro);
                    telaFiltro.PainelFiltro.BringToFront();
                }
                else
                {
                    if (telaFiltro != null)
                    {
                        telaFiltro.PainelFiltro.BringToFront();
                    }
                }
            }
        }

        static bool IsNumpad(System.Windows.Forms.KeyEventArgs e)
        {
            return (e.KeyValue >= ((int)System.Windows.Forms.Keys.NumPad0) && e.KeyValue <= ((int)System.Windows.Forms.Keys.NumPad9));
        }

        static void textBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string lCodigo = ((System.Windows.Forms.TextBox)sender).Text;

            int lCodigoInt = 0;

            int lValue = -1;

            if (!IsNumpad(e))
            {
                lCodigo = lCodigo + Convert.ToChar(e.KeyCode);
            }
            else
            {
                lValue = e.KeyValue - ((int)System.Windows.Forms.Keys.NumPad0);
                lCodigo = lCodigo + lValue;
            }

            System.Windows.Forms.CheckedListBox.ObjectCollection lItens = ((System.Windows.Forms.CheckedListBox)painelControle.Controls["checkedListBox"]).Items;

            try
            {
                lCodigoInt = int.Parse(lCodigo);
            }
            catch
            {
                for (int i = 0; i < gListaClientes.Count; i++)
                {
                    if (!((System.Windows.Forms.CheckedListBox)painelControle.Controls["checkedListBox"]).Items.Contains(gListaClientes[i]))
                    {
                        lItens.Add(gListaClientes[i]);
                    }
                }

                return;
            }

            bool lAchou = lItens.Contains(lCodigo);

            if (lAchou)
            {
                lItens.Clear();

                lItens.Add("Selecionar Todos");
                lItens.Add(lCodigo);

                for (int i = 0; i < gListaClienteSelecionadosFiltro.Count; i++)
                {
                    if (gListaClienteSelecionadosFiltro[i] != lCodigoInt)
                    {
                        lItens.Add(gListaClienteSelecionadosFiltro[i]);

                        ((System.Windows.Forms.CheckedListBox)painelControle.Controls["checkedListBox"]).SetItemChecked(lItens.IndexOf(gListaClienteSelecionadosFiltro[i]), true);
                    }
                    else
                    {
                        ((System.Windows.Forms.CheckedListBox)painelControle.Controls["checkedListBox"]).SetItemChecked(i + 1, true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < gListaClientes.Count; i++)
                {
                    if (!((System.Windows.Forms.CheckedListBox)painelControle.Controls["checkedListBox"]).Items.Contains(gListaClientes[i]))
                    {
                        lItens.Add(gListaClientes[i]);
                    }
                }
            }

        }

        static void CheckedListBox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckedListBox lCheck = (System.Windows.Forms.CheckedListBox)sender;

            if (lCheck.SelectedItem.ToString().Equals("Selecionar Todos") && lCheck.GetItemChecked(0))
            {
                for (int i = 0; i < ((System.Windows.Forms.CheckedListBox)sender).Items.Count; i++)
                {
                    lCheck.SetItemChecked(i, true);

                    if (!lCheck.SelectedItem.ToString().Equals("Selecionar Todos"))
                    {
                        gListaClienteSelecionadosFiltro.Add((int)(lCheck.Items[i]));
                    }

                }

            }
            else if (lCheck.SelectedItem.ToString().Equals("Selecionar Todos") && !lCheck.GetItemChecked(0))
            {
                for (int i = 0; i < ((System.Windows.Forms.CheckedListBox)sender).Items.Count; i++)
                {
                    lCheck.SetItemChecked(i, false);

                    if (!lCheck.SelectedItem.ToString().Equals("Selecionar Todos"))
                    {
                        gListaClienteSelecionadosFiltro.Remove((int)(lCheck.Items[i]));
                    }
                }
            }
            else
            {
                if (!lCheck.SelectedItem.ToString().Equals("Selecionar Todos"))
                {
                    int lConta = Convert.ToInt32(lCheck.SelectedItem);

                    if (gListaClienteSelecionadosFiltro.Contains(lConta))
                    {
                        gListaClienteSelecionadosFiltro.Remove(lConta);
                    }
                    else
                    {
                        gListaClienteSelecionadosFiltro.Add(lConta);
                    }
                }
            }
        }

        static void FilterButton_OnClick(object sender, EventArgs e)
        {
            FilterEventArgs lArg = new FilterEventArgs();

            lArg.ListaClientes = new List<string>();

            foreach (var control in painelControle.Controls.OfType<System.Windows.Forms.RadioButton>())
            {
                if (control.Text == "Ascendente")
                {
                    lArg.Ascendente = control.Checked;
                }
            }

            foreach (var control in painelControle.Controls.OfType<System.Windows.Forms.CheckedListBox>())
            {
                if (control.Name == "checkedListBox")
                {
                    var lst = control.CheckedItems;

                    foreach (object obj in lst)
                    {
                        lArg.ListaClientes.Add(obj.ToString());
                    }
                }
            }

            if (EventFiltrar != null)
            {
                EventFiltrar(sender, lArg);
            }

        }

        static void FilterButtonOK_OnClick(object sender, EventArgs e)
        {
            System.Windows.Forms.DataGridView grid = (System.Windows.Forms.DataGridView)telaFiltro.PainelFiltro.Parent;

            ((System.Windows.Forms.Control)telaFiltro.PainelFiltro.Controls["painelControle"]).Controls.Clear();
            telaFiltro.PainelFiltro.Controls.Clear();
            grid.Controls.Remove(telaFiltro.TituloFiltro);
            grid.Controls.Remove(telaFiltro.PainelFiltro);
            telaFiltro.PainelFiltro.Dispose();
            telaFiltro = null;
        }

        static void FilterButtonCancel_OnClick(object sender, EventArgs e)
        {
            System.Windows.Forms.DataGridView grid = (System.Windows.Forms.DataGridView)telaFiltro.PainelFiltro.Parent;

            ((System.Windows.Forms.Control)telaFiltro.PainelFiltro.Controls["painelControle"]).Controls.Clear();
            telaFiltro.PainelFiltro.Controls.Clear();
            grid.Controls.Remove(telaFiltro.TituloFiltro);
            grid.Controls.Remove(telaFiltro.PainelFiltro);
            telaFiltro.PainelFiltro.Dispose();
            telaFiltro = null;
        }

        static void PanelControle_OnPreviewKeyDown(object sender, System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            System.Windows.Forms.Keys tecla = e.KeyCode;
        }
    }
}
