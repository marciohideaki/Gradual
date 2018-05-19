/*****************************************************************************
 MainModule...: OfferBookGridRow
 SubModule....:
 Author.......:
 Date.........:
 Porpouse.....:

 Modifications:
 Author               Date       Reason
 -------------------- ---------- ---------------------------------------------
 
 *****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace GradualForm.Controls
{

    #region OfferBookGridRow - subclasses the DataGridView's DataGridViewRow class

    public class OfferBookGridRow : DataGridViewRow
    {
        private bool isGroupRow;
        private IOfferBookGridGroup group;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IOfferBookGridGroup Group
        {
            get { return group; }
            set { group = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsGroupRow
        {
            get { return isGroupRow; }
            set { isGroupRow = value; }
        }

        public OfferBookGridRow() : this(null, false)
        {
        }

        public OfferBookGridRow(IOfferBookGridGroup group)
            : this(group, false)
        {
        }

        public OfferBookGridRow(IOfferBookGridGroup group, bool isGroupRow)
            : base()
        {
            this.group = group;
            this.isGroupRow = isGroupRow;
        }

        public override DataGridViewElementStates GetState(int rowIndex)
        {
            if (!IsGroupRow && group != null && group.Collapsed)
            {
                return base.GetState(rowIndex) & DataGridViewElementStates.Selected;
            }

            return base.GetState(rowIndex);
        }

        /// <summary>
        /// the main difference with a Group row and a regular row is the way it is painted on the control.
        /// the Paint method is therefore overridden and specifies how the Group row is painted.
        /// Note: this method is not implemented optimally. It is merely used for demonstration purposes
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipBounds"></param>
        /// <param name="rowBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="rowState"></param>
        /// <param name="isFirstDisplayedRow"></param>
        /// <param name="isLastVisibleRow"></param>
        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, bool isFirstDisplayedRow, bool isLastVisibleRow)
        {
            if (this.isGroupRow)
            {
                OfferBookGrid grid = (OfferBookGrid)this.DataGridView;

                int rowHeadersWidth = grid.RowHeadersVisible ? grid.RowHeadersWidth : 0;

                // this can be optimized
                Color color  = base.DataGridView.AlternatingRowsDefaultCellStyle.BackColor;
                Brush brush  = new SolidBrush(grid.GroupHeaderBackColor);
                Brush brush2 = new SolidBrush(grid.GroupHeaderUnderlineColor);
                Brush brush3 = new SolidBrush(grid.GroupHeaderFontColor);

                int gridwidth = grid.Columns.GetColumnsWidth(DataGridViewElementStates.Displayed);
                Rectangle rowBounds2 = grid.GetRowDisplayRectangle(this.Index, true);

                // draw the background
                graphics.FillRectangle(brush, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset, rowBounds.Top, gridwidth, rowBounds.Height - 1);

                // draw text, using the current grid font
                string[] s = group.Text.Split('|');
                SizeF stringSize = new SizeF();
                
                stringSize = graphics.MeasureString(s[1], grid.GroupHeaderFont);
                
                // Não mostrar a corretora
                //graphics.DrawString(s[0], grid.GroupHeaderFont, brush3, grid.Columns[0].Width - stringSize.Width - 5, rowBounds.Bottom - 18);

                if (stringSize.Width > (grid.Columns[1].Width - 10))
                {
                    string texto = s[1].ToInt64().ToNumeroAbreviado();

                    stringSize = graphics.MeasureString(texto, grid.GroupHeaderFont);
                    graphics.DrawString(texto, grid.GroupHeaderFont, brush3, grid.Columns[0].Width + grid.Columns[1].Width - stringSize.Width - 5, rowBounds.Bottom - 18);
                }
                else
                {

                    switch (((OfferBookGrid)this.DataGridView).Type)
                    {
                        case GridType.Buy:
                            graphics.DrawString(s[1], grid.GroupHeaderFont, brush3, grid.Columns[0].Width + grid.Columns[1].Width - stringSize.Width - 5, rowBounds.Bottom - 18);
                            break;
                        case GridType.Sell:
                            graphics.DrawString(s[1], grid.GroupHeaderFont, brush3, grid.Columns[0].Width + 5, rowBounds.Bottom - 18);
                            break;
                    }
                }

                stringSize = graphics.MeasureString(s[2], grid.GroupHeaderFont);

                switch (((OfferBookGrid)this.DataGridView).Type)
                {
                    case GridType.Buy:
                        graphics.DrawString(s[2], grid.GroupHeaderFont, brush3, grid.Columns[0].Width + grid.Columns[1].Width + grid.Columns[2].Width - stringSize.Width - 5, rowBounds.Bottom - 18);
                        break;
                    case GridType.Sell:
                        graphics.DrawString(s[2], grid.GroupHeaderFont, brush3, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4, rowBounds.Bottom - 18);
                        break;
                }

                //draw bottom line
                graphics.FillRectangle(brush2, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset, rowBounds.Bottom - 2, gridwidth - 1, 1);

                // draw right vertical bar
                if (grid.CellBorderStyle == DataGridViewCellBorderStyle.SingleVertical || grid.CellBorderStyle == DataGridViewCellBorderStyle.Single)
                    graphics.FillRectangle(brush2, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + gridwidth - 1, rowBounds.Top, 1, rowBounds.Height);

                
                
                if (group.Collapsed)
                {
                    switch (((OfferBookGrid)this.DataGridView).Type)
                    {
                        case GridType.Buy:
                            if (grid.ExpandIcon != null)
                                graphics.DrawImage(grid.ExpandIcon, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4, rowBounds.Bottom - 18, 11, 11);
                            break;
                        case GridType.Sell:
                            if (grid.ExpandIcon != null)
                                //graphics.DrawImage(grid.ExpandIcon, grid.Columns[0].Width + grid.Columns[1].Width + grid.Columns[2].Width - 15, rowBounds.Bottom - 18, 11, 11);
                                graphics.DrawImage(grid.ExpandIcon, rowBounds.Right - 15, rowBounds.Bottom - 18, 11, 11);
                            break;
                    }
                }
                else
                {
                    //if (grid.CollapseIcon != null)
                    //  Graphics.DrawImage(grid.CollapseIcon, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4, rowBounds.Bottom - 18, 11, 11);
                    
                    switch (((OfferBookGrid)this.DataGridView).Type)
                    {
                        case GridType.Buy:
                            if (grid.ExpandIcon != null)
                                graphics.DrawImage(grid.CollapseIcon, rowBounds.Left + rowHeadersWidth - grid.HorizontalScrollingOffset + 4, rowBounds.Bottom - 18, 11, 11);
                            break;
                        case GridType.Sell:
                            if (grid.ExpandIcon != null)
                                graphics.DrawImage(grid.CollapseIcon, rowBounds.Right - 15, rowBounds.Bottom - 18, 11, 11);
                            break;
                    }
                }

                brush.Dispose();
                brush2.Dispose();
                brush3.Dispose();
            }

            base.Paint(graphics, clipBounds, rowBounds, rowIndex, rowState, isFirstDisplayedRow, isLastVisibleRow);
        }

        /// <summary>
        /// The PaintCells method is therefore overridden and specifies how and each cell paint
        /// Note: this method is not implemented optimally. It is merely used for demonstration purposes
        /// </summary>
        protected override void PaintCells(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, bool isFirstDisplayedRow, bool isLastVisibleRow, DataGridViewPaintParts paintParts)
        {
            if (!this.isGroupRow)
            {
                base.PaintCells(graphics, clipBounds, rowBounds, rowIndex, rowState, isFirstDisplayedRow, isLastVisibleRow, paintParts);
            }
        }

        /// <summary>
        /// this function checks if the user hit the expand (+) or collapse (-) icon.
        /// if it was hit it will return true
        /// </summary>
        /// <param name="e">mouse click event arguments</param>
        /// <returns>returns true if the icon was hit, false otherwise</returns>
        internal bool IsIconHit(DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return false;

            OfferBookGrid grid = (OfferBookGrid)this.DataGridView;
            Rectangle rowBounds = grid.GetRowDisplayRectangle(this.Index, false);
            int x = e.X;

            //DataGridViewColumn c = grid.Columns[e.ColumnIndex];
            //if (this.isGroupRow &&
            //    (c.DisplayIndex == 0) &&
            //    (x > rowBounds.Left + 4) &&
            //    (x < rowBounds.Left + 16) &&
            //    (e.Y > rowBounds.Height - 18) &&
            //    (e.Y < rowBounds.Height - 7))
            //    return true;

            DataGridViewColumn c;
            switch (((OfferBookGrid)this.DataGridView).Type)
            {
                case GridType.Buy:
                    c = grid.Columns[e.ColumnIndex];
                    if (this.isGroupRow &&
                        (c.DisplayIndex == 0) &&
                        (x > rowBounds.Left + 4) &&
                        (x < rowBounds.Left + 16) &&
                        (e.Y > rowBounds.Height - 18) &&
                        (e.Y < rowBounds.Height - 7))
                        return true;
                    break;
                case GridType.Sell:
                    c = grid.Columns[e.ColumnIndex];
                    if (this.isGroupRow &&
                        (c.DisplayIndex == 2) &&
                        (x > c.Width - 15) &&
                        (x < c.Width - 5) &&
                        (e.Y < rowBounds.Height - 7) &&
                        (e.Y > rowBounds.Height - 18))
                        return true;
                    break;
            }

            return false;
        }
    }
    #endregion OfferBookGridRow - subclasses the DataGridView's DataGridViewRow class

    #region OfferBookGridRowComparer implementation
    
    /// <summary>
    /// the OfferBookGridRowComparer object is used to sort unbound data in the OfferBookGrid.
    /// currently the comparison is only done for string values. 
    /// therefore dates or numbers may not be sorted correctly.
    /// Note: this class is not implemented optimally. It is merely used for demonstration purposes
    /// </summary>
    internal class OfferBookGridRowComparer : IComparer
    {
        ListSortDirection direction;
        int columnIndex;

        public OfferBookGridRowComparer(int columnIndex, ListSortDirection direction)
        {
            this.columnIndex = columnIndex;
            this.direction = direction;
        }

        #region IComparer Members

        public int Compare(object x, object y)
        {
            OfferBookGridRow obj1 = (OfferBookGridRow)x;
            OfferBookGridRow obj2 = (OfferBookGridRow)y;
            return string.Compare(obj1.Cells[this.columnIndex].Value.ToString(), obj2.Cells[this.columnIndex].Value.ToString()) * (direction == ListSortDirection.Ascending ? 1 : -1);
        }
        #endregion
    }
    #endregion OfferBookGridRowComparer implementation

}
