/*****************************************************************************
 MainModule...: OfferBookGrid
 SubModule....:
 Author.......:
 Date.........:
 Porpouse.....:

 Modifications:
 Author               Date       Reason
 -------------------- ---------- ---------------------------------------------        }
 
 *****************************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using log4net;

namespace GradualForm.Controls
{
    #region implementation of the OfferBookGrid!

    public enum GridType
    { 
        Sell, 
        Buy
    };
    
    public enum GroupState
    {
        Collapsed,
        Expanded
    };

    public partial class OfferBookGrid : DataGridView
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);    

        private readonly object syncRoot = new object();

        public object SyncRoot
        {
            get { return this.syncRoot; }
        }

        public String Stock
        {
            get;
            set;
        }
        private System.Collections.Generic.List<IOfferBookGridGroup> _groups = new System.Collections.Generic.List<IOfferBookGridGroup>();
        
        const int BUYSIDE_COL_BROKER = 0;
        const int BUYSIDE_COL_QTY = 1;
        const int BUYSIDE_COL_PRC = 2;

        const int SELLSIDE_COL_BROKER = 2;
        const int SELLSIDE_COL_QTY = 1;
        const int SELLSIDE_COL_PRC = 0;

        //bool Logar = false;

        #region OfferBookGrid constructor
        public OfferBookGrid()
        {
            DoubleBuffered = true;

            InitializeComponent();

            logger.Info("Criou");

            // very important, this indicates that a new default row class is going to be used to fill the grid
            // in this case our custom OfferBookGridRow class
            base.RowTemplate = new OfferBookGridRow();
            this.groupTemplate = new OfferBookgGridDefaultGroup();

        }
        #endregion OfferBookGrid constructor

        #region OfferBookGrid property definitions

        private Hashtable Groups = new Hashtable();
        private Font groupHeaderFont = new Font(OfferBookGrid.DefaultFont, FontStyle.Regular);
        [Category("GroupHeaderAppearance")]
        public Font GroupHeaderFont
        {
            get { return groupHeaderFont; }
            set { groupHeaderFont = value; }
        }

        private Color groupHeaderFontColor = Color.Black;
        [Category("GroupHeaderAppearance")]
        public Color GroupHeaderFontColor
        {
            get { return groupHeaderFontColor; }
            set { groupHeaderFontColor = value; }
        }

        private Color groupHeaderUnderlineColor = Color.Navy;
        [Category("GroupHeaderAppearance")]
        public Color GroupHeaderUnderlineColor
        {
            get { return groupHeaderUnderlineColor; }
            set { groupHeaderUnderlineColor = value; }
        }

        private Color groupHeaderBackColor = Color.White;
        [Category("GroupHeaderAppearance")]
        public Color GroupHeaderBackColor
        {
            get { return groupHeaderBackColor; }
            set { groupHeaderBackColor = value; }
        }

        [Category("GroupHeaderAppearance")]
        public DataGridViewCellStyle groupHeaderCellStyle;
        public DataGridViewCellStyle GroupHeaderCellStyle
        {
            get { return groupHeaderCellStyle; }
            set { groupHeaderCellStyle = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewRow RowTemplate
        {
            get { return base.RowTemplate; }
        }

        private IOfferBookGridGroup groupTemplate;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IOfferBookGridGroup GroupTemplate
        {
            get
            {
                return groupTemplate;
            }
            set
            {
                groupTemplate = value;
            }
        }

        private Image iconCollapse;
        [Category("Appearance")]
        public Image CollapseIcon
        {
            get { return iconCollapse; }
            set { iconCollapse = value; }
        }

        private Image iconExpand;
        [Category("Appearance")]
        public Image ExpandIcon
        {
            get { return iconExpand; }
            set { iconExpand = value; }
        }

        private bool grouped;
        [Category("Appearance")]
        public bool isGrouped
        {
            get { return grouped; }
            set { grouped = value; }
        }

        private GridType type;
        [Category("Appearance")]
        public GridType Type
        {
            get { return type; }
            set { type = value; }
        }

        private IOfferBookGridGroup groupingStyle;
        [Category("Appearance")]
        public IOfferBookGridGroup GroupingStyle
        {
            get { return groupingStyle; }
            set { groupingStyle = value; }
        }

        private DataSourceManager dataSource;
        public new object DataSource
        {
            get
            {
                if (dataSource == null) return null;

                // special case, datasource is bound to itself.
                // for client it must look like no binding is set,so return null in this case
                if (dataSource.DataSource.Equals(this)) return null;

                // return the origional datasource.
                return dataSource.DataSource;
            }
        }
        #endregion OfferBookGrid property definitions

        #region OfferBookGrid new methods
        public void CollapseAll()
        {
            SetGroupCollapse(true);
        }

        public void ExpandAll()
        {
            SetGroupCollapse(false);
        }

        public void ClearGroups(bool RemoveTemplate)
        {
            if(RemoveTemplate)
                groupTemplate.Column = null; //reset

            _groups.Clear();

            //lock (this.Rows)
            lock(syncRoot)
            {
                FillGrid(null);
            }
            
            this.grouped = false;
        }

        public void BindData(object dataSource, string dataMember)
        {
            this.DataMember = DataMember;
            if (dataSource == null)
            {
                this.dataSource = null;
                Columns.Clear();
            }
            else
            {
                this.dataSource = new DataSourceManager(dataSource, dataMember);
                SetupColumns();
                FillGrid(null);
            }
        }

        public void Group()
        {
            if (dataSource == null) // if no datasource is set, then bind to the grid itself
                dataSource = new DataSourceManager(this, null);

            this.grouped = true;
            
            //lock (this.Rows)
            lock (syncRoot)
            {
                FillGrid(groupTemplate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dataGridViewRow"></param>
        /// <returns></returns>
        public int InsertAt(int index, OfferBookGridRow dataGridViewRow)
        {
            int _ret = 0;
            try
            {
                if (this.grouped)
                {
                    int pos;
                    //lock (this.Rows)
                    lock (syncRoot)
                    {
                        pos = ValidatePosition(index, ref dataGridViewRow);

                        if (pos > 0)
                        {
                            this.Rows.Insert(pos, dataGridViewRow);
                        }
                        else
                        {
                            this.Rows.Insert(1, dataGridViewRow);
                        }
                        _ret = pos;
                    }
                }
                else
                {
                    //Workaround: the broadcasting service is sending the signal even after the cancellation. 
                    //We must make sure that the list is empty after cancellation
                    if (index <= this.Rows.Count + 1)
                    {
                        //lock (this.Rows)
                        lock (syncRoot)
                        {
                            this.Rows.Insert(index, dataGridViewRow);
                            _ret = index;
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException outOfRangeEx)
            {
                logger.Warn(String.Format("OfferBookGrid > InsertAt({0}): a posição {1} de {2} do livro (Agrupado:{3}) não existe mais ({4}).", this.Stock, index, this.RowCount, this.grouped, outOfRangeEx.Message));
            }
            catch (Exception ex)
            {
                //AplicacaoGeral.ReportarErro("OfferBookGrid > InsertAt", ex + " | " + index.ToString() + " | " + this.RowCount.ToString() + " | " + this.grouped.ToString());
                logger.ErrorFormat("Exception [{0}] [{1}] [{2}] [{3}]", ex, index, this.RowCount, this.grouped);
            }

            return _ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            try
            {
                if (this.grouped)
                {
                    int pos;
                    //lock (this.Rows)
                    lock (syncRoot)
                    {
                        pos = ValidatePosition(index);

                        if (!this.Rows[pos].Visible)
                        {
                            this.Rows[pos].Visible = true;
                        }

                        this.Rows.RemoveAt(pos);
                    }
                }
                else
                {
                    //lock (this.Rows)
                    lock (syncRoot)
                    {
                        //Workaround: the broadcasting service is sending the signal even after the cancellation
                        //We must make sure that the list is empty after cancellation
                        if (this.Rows.Count > 0)
                        {
                            this.Rows.RemoveAt(index);
                        }
                    }
                }
            }
            catch(ArgumentOutOfRangeException outOfRangeEx)
            {
                logger.Warn(String.Format("OfferBookGrid > RemoveAt({0}): a posição {1} de {2} do livro (Agrupado:{3}) não existe mais ({4}).", this.Stock, index, this.RowCount, this.grouped, outOfRangeEx.Message));
            }
            catch (Exception ex)
            {
                //AplicacaoGeral.ReportarErro("OfferBookGrid > RemoveAt", ex + " | " + index.ToString() + " | " + this.RowCount.ToString() + " | " + this.grouped.ToString());
                logger.ErrorFormat("Exception [{0}] [{1}] [{2}] [{3}]", ex, index, this.RowCount, this.grouped);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dataGrigViewRow"></param>
        /// <returns></returns>
        private int ValidatePosition(int index, ref OfferBookGridRow dataGrigViewRow)
        {
            StringBuilder str = new StringBuilder();

            int _groupsCount    = 0;
            int _itemsCount     = 0;
            int _return         = 0;

            foreach (IOfferBookGridGroup _group in _groups)
            {
                lock (this.SyncRoot)
                {
                        _groupsCount++;
                        _itemsCount += _group.ItemCount;

                        if (index <= _itemsCount)
                        {
                            /********************************************************************************************/
                            /***************************************** Buy side *****************************************/
                            /********************************************************************************************/
                            if (this.Type.Equals(GridType.Buy))
                            {
                                /********************************************************************************************/
                                /********** When the row has a valid group, insert it into the corresponding group **********/
                                /********************************************************************************************/

                                /* The row belongs to the previous group and direction are ascending */
                                if (Double.Parse(dataGrigViewRow.Cells[BUYSIDE_COL_PRC].Value.ToString()).Equals(Double.Parse(_group.Value.ToString())))
                                {

                                    _return = index + _groupsCount;
                                    _group.ItemCount++;
                                    _group.Quantidade += Int32.Parse(dataGrigViewRow.Cells[BUYSIDE_COL_QTY].Value.ToString());
                                    dataGrigViewRow.Group = _group;
                                    break;
                                }

                                /* The row belongs to the previous following */
                                if (_groups.Count > _groupsCount)
                                {
                                    if (Double.Parse(dataGrigViewRow.Cells[BUYSIDE_COL_PRC].Value.ToString()).Equals(Double.Parse(_groups[_groupsCount].Value.ToString())))
                                    {
                                        _return = index + _groupsCount + 1;
                                        _groups[_groupsCount].ItemCount++;
                                        _groups[_groupsCount].Quantidade += Int32.Parse(dataGrigViewRow.Cells[BUYSIDE_COL_QTY].Value.ToString());
                                        dataGrigViewRow.Group = _groups[_groupsCount];
                                        break;
                                    }
                                }


                                /********************************************************************************************/
                                /*********** When the row does not have a valid group, insert it into a new group ***********/
                                /********************************************************************************************/

                                // Before
                                if (Double.Parse(dataGrigViewRow.Cells[BUYSIDE_COL_PRC].Value.ToString()) > Double.Parse(_group.Value.ToString()))
                                {
                                    //if (index.Equals(0))
                                    //{
                                    IOfferBookGridGroup grp = CreateGroup(_groupsCount - 1, _groupsCount + index - 1, Double.Parse(dataGrigViewRow.Cells[BUYSIDE_COL_PRC].Value.ToString()));
                                    _return = _groupsCount + index;
                                    grp.Quantidade += Int32.Parse(dataGrigViewRow.Cells[BUYSIDE_COL_QTY].Value.ToString());
                                    dataGrigViewRow.Group = grp;
                                    break;
                                    //}
                                }

                                // After
                                if (Double.Parse(dataGrigViewRow.Cells[BUYSIDE_COL_PRC].Value.ToString()) < Double.Parse(_group.Value.ToString()))
                                {
                                    IOfferBookGridGroup grp = CreateGroup(_groupsCount, _groupsCount + _itemsCount, Double.Parse(dataGrigViewRow.Cells[BUYSIDE_COL_PRC].Value.ToString()));
                                    _return = _groupsCount + _itemsCount + 1;
                                    grp.Quantidade += Int32.Parse(dataGrigViewRow.Cells[BUYSIDE_COL_QTY].Value.ToString());
                                    dataGrigViewRow.Group = grp;
                                    break;
                                }
                            }

                            /********************************************************************************************/
                            /***************************************** Sell side ****************************************/
                            /********************************************************************************************/

                            if (this.Type.Equals(GridType.Sell))
                            {
                                /********************************************************************************************/
                                /********** When the row has a valid group, insert it into the corresponding group **********/
                                /********************************************************************************************/

                                /* The row belongs to the previous group and direction are ascending */
                                if (Double.Parse(dataGrigViewRow.Cells[SELLSIDE_COL_PRC].Value.ToString()).Equals(Double.Parse(_group.Value.ToString())))
                                {
                                    _return = index + _groupsCount;
                                    _group.ItemCount++;
                                    _group.Quantidade += Int32.Parse(dataGrigViewRow.Cells[SELLSIDE_COL_QTY].Value.ToString());
                                    dataGrigViewRow.Group = _group;
                                    break;
                                }

                                /* The row belongs to the previous following */
                                if (_groups.Count > _groupsCount)
                                {
                                    if (Double.Parse(dataGrigViewRow.Cells[SELLSIDE_COL_PRC].Value.ToString()).Equals(Double.Parse(_groups[_groupsCount].Value.ToString())))
                                    {
                                        _return = index + _groupsCount + 1;
                                        _groups[_groupsCount].ItemCount++;
                                        _groups[_groupsCount].Quantidade += Int32.Parse(dataGrigViewRow.Cells[SELLSIDE_COL_QTY].Value.ToString());
                                        dataGrigViewRow.Group = _groups[_groupsCount];
                                        break;
                                    }
                                }

                                /********************************************************************************************/
                                /*********** When the row does not have a valid group, insert it into a new group ***********/
                                /********************************************************************************************/

                                // After
                                if (Double.Parse(dataGrigViewRow.Cells[SELLSIDE_COL_PRC].Value.ToString()) > Double.Parse(_group.Value.ToString()))
                                {
                                    IOfferBookGridGroup grp = CreateGroup(_groupsCount, _groupsCount + _itemsCount, Double.Parse(dataGrigViewRow.Cells[0].Value.ToString()));
                                    _return = _groupsCount + _itemsCount + 1;
                                    grp.Quantidade += Int32.Parse(dataGrigViewRow.Cells[SELLSIDE_COL_QTY].Value.ToString());
                                    dataGrigViewRow.Group = grp;
                                    break;
                                }

                                // Before
                                if (Double.Parse(dataGrigViewRow.Cells[SELLSIDE_COL_PRC].Value.ToString()) < Double.Parse(_group.Value.ToString()))
                                {
                                    //if (index.Equals(0))
                                    //{
                                    IOfferBookGridGroup grp = CreateGroup(_groupsCount - 1, _groupsCount + index - 1, Double.Parse(dataGrigViewRow.Cells[SELLSIDE_COL_PRC].Value.ToString()));
                                    _return = _groupsCount + index;
                                    grp.Quantidade += Int32.Parse(dataGrigViewRow.Cells[SELLSIDE_COL_QTY].Value.ToString());
                                    dataGrigViewRow.Group = grp;
                                    break;
                                    //}
                                }
                            }
                        }
                    }
            }
            return _return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int ValidatePosition(int index)
        {
            int _groupsCount = 0;
            int _itemsCount = 0;
            int _return = 0;

            foreach (IOfferBookGridGroup group in _groups)
            {
                _groupsCount++;
                _itemsCount += group.ItemCount;

                if (index <= _itemsCount - 1)
                {
                    _return = index + _groupsCount;

                    group.ItemCount--;

                    if (!((OfferBookGridRow)this.Rows[_return]).IsGroupRow)
                    {
                        group.Quantidade -= Int32.Parse(Rows[_return].Cells[1].Value.ToString());
                    }

                    if (group.ItemCount.Equals(0))
                    {
                        Rows.RemoveAt(_itemsCount + _groupsCount - 1);
                        _groups.RemoveAt(_groupsCount - 1);
                        _return--;
                    }

                    break;
                }
            }

            return _return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupIndex"></param>
        /// <param name="position"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private IOfferBookGridGroup CreateGroup(int groupIndex, int position, double value)
        {

            OfferBookGridRow row;
            IOfferBookGridGroup groupCur = null;

            groupCur = (IOfferBookGridGroup)this.GroupingStyle.Clone(); // init
            groupCur.Value = value;

            //TODO: can be optimized and generalized
            groupCur.GroupIndex = groupIndex -1;
            groupCur.ItemCount = 1;

            _groups.Insert(groupIndex, groupCur);
            
            row = (OfferBookGridRow)this.RowTemplate.Clone();

            row.Group = groupCur;
            row.IsGroupRow = true;
            row.Height = groupCur.Height;
            row.CreateCells(this, groupCur.Value);

            Rows.Insert(position, row);

            return groupCur;
        }

        #endregion OfferBookGrid new methods

        #region OfferBookGrid event handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            OfferBookGridRow row = (OfferBookGridRow)base.Rows[e.RowIndex];
            if (row.IsGroupRow)
                e.Cancel = true;
            else
                base.OnCellBeginEdit(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellDoubleClick(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.RowIndex >= 0)
            {

                OfferBookGridRow row = (OfferBookGridRow)base.Rows[e.RowIndex];
                if (row.IsGroupRow)
                {
                    row.Group.Collapsed = !row.Group.Collapsed;

                    //this is a workaround to make the grid re-calculate it's contents and backgroun bounds
                    // so the background is updated correctly.
                    // this will also invalidate the control, so it will redraw itself

                    row.Visible = false;
                    row.Visible = true;

                    return;
                }
            }
            base.OnCellClick(e);
        }

        // the OnCellMouseDown is overriden so the control can check to see if the
        // user clicked the + or - sign of the group-row
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            OfferBookGridRow row = (OfferBookGridRow)base.Rows[e.RowIndex];
            if (row.IsGroupRow && row.IsIconHit(e))
            {
                System.Diagnostics.Debug.WriteLine("OnCellMouseDown " + DateTime.Now.Ticks.ToString());
                row.Group.Collapsed = !row.Group.Collapsed;

                //this is a workaround to make the grid re-calculate it's contents and backgroun bounds
                // so the background is updated correctly.
                // this will also invalidate the control, so it will redraw itself
                row.Visible = false;
                row.Visible = true;
            }
            else
                base.OnCellMouseDown(e);
        }
        #endregion OfferBookGrid event handlers

        #region Grid Fill functions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collapsed"></param>
        private void SetGroupCollapse(bool collapsed)
        {
            if (Rows.Count == 0) return;
            if (groupTemplate == null) return;

            // set the default grouping style template collapsed property
            groupTemplate.Collapsed = collapsed;

            // loop through all rows to find the GroupRows
            //lock (this.Rows)
            lock (syncRoot)
            {
                foreach (OfferBookGridRow row in Rows)
                {
                    if (row.IsGroupRow)
                        row.Group.Collapsed = collapsed;
                }
            }

            // workaround, make the grid refresh properly
            Rows[0].Visible = !Rows[0].Visible;
            Rows[0].Visible = !Rows[0].Visible;
        }
        /// <summary>
        /// 
        /// </summary>
        private void SetupColumns()
        {
            ArrayList list;

            // clear all columns, this is a somewhat crude implementation
            // refinement may be welcome.
            Columns.Clear();
            // start filling the grid
            if (dataSource == null)
                return;
            else
            {
                list = dataSource.Rows;
            }
                
            if (list.Count <= 0) return;

            foreach (string c in dataSource.Columns)
            {
                int index;
                DataGridViewColumn column = Columns[c];
                if (column == null)
                    index = Columns.Add(c, c);
                else
                    index = column.Index;
                Columns[index].SortMode = DataGridViewColumnSortMode.Programmatic; // always programmatic!
            }
        }

        
        /// <summary>
        ///     the fill grid method fills the grid with the data from the DataSourceManager
        ///     It takes the grouping style into account, if it is set.
        /// </summary>
        /// <param name="groupingStyle"></param>
        private void FillGrid(IOfferBookGridGroup groupingStyle)
        {
            //TODO: efetuar teste e implementar o retorno booleano informando o resultado da operação
            /*
            if (this.Rows.Count > 0)
            {
                if (this.Type.Equals(GridType.Buy))
                {
                    if (this.Rows[0].Cells[2].ToString().Equals("Abert."))
                    {
                        return;
                    }
                }
                else
                {
                    if (this.Rows[0].Cells[0].ToString().Equals("Abert."))
                    {
                        return;
                    }
                }
            }
            */

            if (groupingStyle != null)
                this.GroupingStyle = groupingStyle;

            ArrayList list;
            OfferBookGridRow row;

            this.Rows.Clear();

            // start filling the grid
            if (dataSource == null)
                return;
            else
                list = dataSource.Rows;

            if (list.Count <= 0) return;

            // this block is used of grouping is turned off
            // this will simply list all attributes of each object in the list
            if (groupingStyle == null)
            {
                foreach (DataSourceRow r in list)
                {
                    row = (OfferBookGridRow)this.RowTemplate.Clone();
                    foreach (object val in r)
                    {
                        DataGridViewCell cell = new DataGridViewTextBoxCell();
                        cell.Value = val.ToString();
                        row.Cells.Add(cell);
                    }

                    Rows.Add(row);
                }
            }

            // this block is used when grouping is used
            // items in the list must be sorted, and then they will automatically be grouped
            else
            {
                IOfferBookGridGroup groupCur = null;
                //object result = null;
                Double result = 0.0;
                int idx;

                //TODO: optimization needed
                foreach (DataSourceRow r in list)
                {
                    row = (OfferBookGridRow)this.RowTemplate.Clone();
                    //result = r[groupingStyle.Column.Index];
                    result = Double.Parse(r[groupingStyle.Column.Index].ToString());

                    //if (groupCur != null && groupCur.CompareTo(result) == 0) // item is part of the group
                    if (groupCur != null && groupCur.Value.Equals(result))
                    {
                        row.Group = groupCur;
                        groupCur.ItemCount++;
                    }
                    else // item is not part of the group, so create new group
                    {
                        groupCur = (IOfferBookGridGroup)groupingStyle.Clone(); // init
                        groupCur.Type = this.Type;

                        if(this.Type.Equals(GridType.Sell))
                        {
                            groupCur.Value = Double.Parse(r[0].ToString());
                        }

                        if (this.Type.Equals(GridType.Buy))
                        {
                            groupCur.Value = Double.Parse(r[2].ToString());
                        }

                        _groups.Add(groupCur);

                        row.Group = groupCur;
                        row.IsGroupRow = true;
                        row.Height = groupCur.Height;
                        row.CreateCells(this, groupCur.Value);
                        idx = Rows.Add(row);

                        // add content row after this
                        row = (OfferBookGridRow)this.RowTemplate.Clone();
                        row.Group = groupCur;
                        groupCur.ItemCount = 1;
                    }

                    foreach (object obj in r)
                    {
                        DataGridViewCell cell = new DataGridViewTextBoxCell();
                        cell.Value = obj.ToString();
                        row.Cells.Add(cell);
                    }

                    Rows.Add(row);
                 
                    groupCur.Quantidade += Int64.Parse(row.Cells[1].Value.ToString());
                }
            }

        }

        #endregion Grid Fill functions
    }

    #endregion implementation of the OfferBookGrid!
}
