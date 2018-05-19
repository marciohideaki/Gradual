using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GradualForm.Controls
{
    /// <summary>
    /// Add column show/hide capability to a DataGridView. When user right-clicks 
    /// the cell origin a popup, containing a list of checkbox and column names, is
    /// shown. 
    /// </summary>
    public class DataGridViewColumnSelector
    {
        // the DataGridView to which the DataGridViewColumnSelector is attached
        private DataGridView mDataGridView = null;
        // a CheckedListBox containing the column header text and checkboxes
        private CheckedListBox mCheckedListBox;
        // a ToolStripDropDown object used to show the popup
        private ToolStripDropDown mPopup;
        public List<String> SupressedColumns = new List<string>();
        //Get information of all hidden columns of DataGridView
        private string[] HiddenColumnName;

        /// <summary>
        /// The max height of the popup
        /// </summary>
        public int MaxHeight = 300;
        /// <summary>
        /// The width of the popup
        /// </summary>
        public int Width = 200;

        /// <summary>
        /// Gets or sets the DataGridView to which the DataGridViewColumnSelector is attached
        /// </summary>
        public DataGridView DataGridView
        {
            get { return mDataGridView; }
            set
            {
                // If any, remove handler from current DataGridView 
                if (mDataGridView != null) mDataGridView.CellMouseClick -= new DataGridViewCellMouseEventHandler(mDataGridView_CellMouseClick);
                // Set the new DataGridView
                mDataGridView = value;
                // Attach CellMouseClick handler to DataGridView
                if (mDataGridView != null) mDataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(mDataGridView_CellMouseClick);
            }
        }

        // When user right-clicks the cell origin, it clears and fill the CheckedListBox with
        // columns header text. Then it shows the popup. 
        // In this way the CheckedListBox items are always refreshed to reflect changes occurred in 
        // DataGridView columns (column additions or name changes and so on).
        void mDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right && e.RowIndex == -1 && e.ColumnIndex == -1)
            {
                mCheckedListBox.Items.Clear();
                foreach (DataGridViewColumn c in mDataGridView.Columns)
                {
                    if (Array.IndexOf(HiddenColumnName, c.HeaderText) < 0)
                    {
                        mCheckedListBox.Items.Add(c.HeaderText, c.Visible);
                    }
                    else
                    {
                        if (!SupressedColumns.Contains(c.HeaderText))
                        {
                            mCheckedListBox.Items.Add(c.HeaderText, c.Visible);
                            
                        }
                        else
                        {
                            mCheckedListBox.Items.Add(c.HeaderText, CheckState.Indeterminate);
                        }
                    }
                }
                int PreferredHeight = (mCheckedListBox.Items.Count * 16) + 7;
                mCheckedListBox.Height = (PreferredHeight < MaxHeight) ? PreferredHeight : MaxHeight;
                mCheckedListBox.Width = this.Width;
                mPopup.Show(mDataGridView.PointToScreen(new Point(e.X, e.Y)));
            }
        }

        public void Show(Point Posicao)
        {
            //if (e.Button == MouseButtons.Right && e.RowIndex == -1 && e.ColumnIndex == -1)
            //{
                mCheckedListBox.Items.Clear();
                foreach (DataGridViewColumn c in mDataGridView.Columns)
                {

                        if (Array.IndexOf(HiddenColumnName, c.HeaderText) < 0)
                        {
                            mCheckedListBox.Items.Add(c.HeaderText, c.Visible);
                        }
                        else
                        {
                            if (!SupressedColumns.Contains(c.HeaderText))
                            {
                                mCheckedListBox.Items.Add(c.HeaderText, c.Visible);
                            }
                            else
                            {
                                mCheckedListBox.Items.Add(c.HeaderText, CheckState.Indeterminate);
                                
                            }
                        }
                }
                int PreferredHeight = (mCheckedListBox.Items.Count * 16) + 7;
                mCheckedListBox.Height = (PreferredHeight < MaxHeight) ? PreferredHeight : MaxHeight;
                mCheckedListBox.Width = this.Width;
                mPopup.Show(mDataGridView.PointToScreen(Posicao));
            //}
        }

        // The constructor creates an instance of CheckedListBox and ToolStripDropDown.
        // the CheckedListBox is hosted by ToolStripControlHost, which in turn is
        // added to ToolStripDropDown.
        public DataGridViewColumnSelector()
        {
            mCheckedListBox = new CheckedListBox();
            mCheckedListBox.CheckOnClick = true;
            mCheckedListBox.ItemCheck += new ItemCheckEventHandler(mCheckedListBox_ItemCheck);

            ToolStripControlHost mControlHost = new ToolStripControlHost(mCheckedListBox);
            mControlHost.Padding = Padding.Empty;
            mControlHost.Margin = Padding.Empty;
            mControlHost.AutoSize = false;

            mPopup = new ToolStripDropDown();
            mPopup.Padding = Padding.Empty;
            mPopup.Items.Add(mControlHost);
        }

        public DataGridViewColumnSelector(DataGridView dgv)
            : this()
        {
            int i = 0;
            this.DataGridView = dgv;
            HiddenColumnName = new string[dgv.ColumnCount];
            foreach (DataGridViewColumn c in dgv.Columns)
            {
                if (c.Visible == false)
                {
                    if (!SupressedColumns.Contains(c.HeaderText))
                    {
                        HiddenColumnName[i] = c.HeaderText;
                    }
                }
                i++;
            }
        }

        // When user checks / unchecks a checkbox, the related column visibility is 
        // switched.
        void mCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Indeterminate)
                e.NewValue = e.CurrentValue; // no change if CheckState is Indeterminate
            else
                mDataGridView.Columns[e.Index].Visible = (e.NewValue == CheckState.Checked);
        }


    }
}