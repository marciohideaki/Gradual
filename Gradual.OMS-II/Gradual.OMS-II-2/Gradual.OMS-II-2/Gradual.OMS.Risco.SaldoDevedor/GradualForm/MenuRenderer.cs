using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradualForm
{
    public class MenuRenderer:System.Windows.Forms.ToolStripRenderer
    {
        protected override void OnRenderMenuItemBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            //base.OnRenderMenuItemBackground(e);
            e.Graphics.FillRectangle(System.Drawing.Brushes.Red, e.Item.ContentRectangle);

            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(StyleSettings.Brush_Fd_ItemMenu_Hover, -10, -10, 2000, 2000);
            }
            else
            {
                e.Graphics.FillRectangle(StyleSettings.Brush_Fd_Form, -10, -10, 2000, 2000);
            }

            //e.Graphics.DrawRectangle(StyleSettings.Pen_Borda_BotaoToolBar, e.Item.ContentRectangle);
            //e.Item.ForeColor = Color.White;
        }

        protected override void OnRenderSeparator(System.Windows.Forms.ToolStripSeparatorRenderEventArgs e)
        {
            //base.OnRenderSeparator(e);

            //e.Graphics.FillRectangle(System.Drawing.Brushes.Black, 0, 0, 2000, 2000);
            e.Graphics.FillRectangle(StyleSettings.Brush_Fd_Form, 0, 0, 2000, 2000);

            e.Graphics.DrawLine(StyleSettings.Pen_Borda_TextBox, 4, 0, e.Item.ContentRectangle.Width - 2, 0);
        }

        protected override void OnRenderDropDownButtonBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            //base.OnRenderDropDownButtonBackground(e);

            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(StyleSettings.Brush_Fd_BotaoToolBar, e.Item.ContentRectangle);
                e.Graphics.DrawRectangle(StyleSettings.Pen_Borda_BotaoToolBar, e.Item.ContentRectangle);
            }
        }

        protected override void OnRenderToolStripContentPanelBackground(System.Windows.Forms.ToolStripContentPanelRenderEventArgs e)
        {
            //base.OnRenderToolStripContentPanelBackground(e);
            e.Graphics.FillRectangle(System.Drawing.Brushes.Red, e.ToolStripContentPanel.ClientRectangle);

            e.Handled = true;

            //TODO: Ver se esse evento é necessário para algo
        }

        protected override void OnRenderItemBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            //base.OnRenderItemBackground(e);
            //e.Graphics.FillRectangle(System.Drawing.Brushes.Red, e.Item.ContentRectangle);
        }

        protected override void OnRenderSplitButtonBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            //base.OnRenderSplitButtonBackground(e);

            //e.Graphics.FillRectangle(System.Drawing.Brushes.Red, e.Item.ContentRectangle);
        }

        protected override void OnRenderButtonBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            //base.OnRenderButtonBackground(e);

            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(StyleSettings.Brush_Fd_BotaoToolBar, e.Item.ContentRectangle);
                e.Graphics.DrawRectangle(StyleSettings.Pen_Borda_BotaoToolBar, e.Item.ContentRectangle);
            }
        }

        protected override void OnRenderToolStripBackground(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            //base.OnRenderToolStripBackground(e);
            e.Graphics.FillRectangle(StyleSettings.Brush_Fd_Form, 0, 0, 2000, 2000);
        }

        protected override void OnRenderToolStripBorder(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            //base.OnRenderToolStripBorder(e);
        }

        protected override void OnRenderItemText(System.Windows.Forms.ToolStripItemTextRenderEventArgs e)
        {
            base.OnRenderItemText(e);
        }

        protected override void OnRenderItemCheck(System.Windows.Forms.ToolStripItemImageRenderEventArgs e)
        {
            base.OnRenderItemCheck(e);

            System.Drawing.Bitmap imagem = new System.Drawing.Bitmap(Properties.Resources.Check);
            //e.Graphics.DrawImage(imagem, new Rectangle(e.CellBounds.X + e.CellBounds.Width - imagem.Width - StyleSettings.DeslocaImagem_Botao, e.CellBounds.Y + StyleSettings.DeslocaImagem_Botao, imagem.Width, imagem.Height));


            if (e.Item.Selected)
            {
                var rect = new System.Drawing.Rectangle(3, 1, 20, 20);
                var rect2 = new System.Drawing.Rectangle(4, 2, 18, 18);
                System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(StyleSettings.Form_BackGround_Color);
                System.Drawing.SolidBrush b2 = new System.Drawing.SolidBrush(StyleSettings.Button_BackGround_Color_Start);

                e.Graphics.FillRectangle(b, rect);
                e.Graphics.FillRectangle(b2, rect2);
                //e.Graphics.DrawImage(e.Image, new Point(5, 3));
                e.Graphics.Clear(System.Drawing.Color.Black);
                e.Graphics.DrawImage(imagem, new System.Drawing.Point(5, 3));
            }
            else
            {
                var rect = new System.Drawing.Rectangle(3, 1, 20, 20);
                var rect2 = new System.Drawing.Rectangle(4, 2, 18, 18);
                System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                System.Drawing.SolidBrush b2 = new System.Drawing.SolidBrush(System.Drawing.Color.Silver);

                e.Graphics.FillRectangle(b, rect);
                e.Graphics.FillRectangle(b2, rect2);
                //e.Graphics.DrawImage(e.Image, new Point(5, 3));
                e.Graphics.Clear(System.Drawing.Color.Black);
                e.Graphics.DrawImage(imagem, new System.Drawing.Point(5, 3));
            }
        }
    }
}
