/*****************************************************************************
 MainModule...: OfferBookgGridDefaultGroup
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
using System.Text;
using System.Windows.Forms;

namespace GradualForm.Controls
{
    #region IOfferBookGridGroup - declares the arrange/grouping interface here

    /// <summary>
    /// IOfferBookGridGroup specifies the interface of any implementation of a OfferBookGridGroup class
    /// Each implementation of the IOfferBookGridGroup can override the behaviour of the grouping mechanism
    /// Notice also that ICloneable must be implemented. The OfferBookGrid makes use of the Clone method of the Group
    /// to create new Group clones. Related to this is the OfferBookGrid.GroupTemplate property, which determines what
    /// type of Group must be cloned.
    /// </summary>
    public interface IOfferBookGridGroup : IComparable, ICloneable
    {
        /// <summary>
        /// the text to be displayed in the group row
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// determines the value of the current group. this is used to compare the group value
        /// against each item's value.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// indicates whether the group is collapsed. If it is collapsed, it group items (rows) will
        /// not be displayed.
        /// </summary>
        bool Collapsed { get; set; }

        /// <summary>
        /// specifies which column is associated with this group
        /// </summary>
        DataGridViewColumn Column { get; set; }

        /// <summary>
        /// specifies the number of items that are part of the current group
        /// this value is automatically filled each time the grid is re-drawn
        /// e.g. after sorting the grid.
        /// </summary>
        int ItemCount { get; set; }

        /// <summary>
        /// specifies the default height of the group
        /// each group is cloned from the GroupStyle object. Setting the height of this object
        /// will also set the default height of each group.
        /// </summary>
        int Height { get; set; }

        int GroupIndex {get; set;}
        
        long Quantidade { get; set; }

        GridType Type { get; set; }
    }
    #endregion define the arrange/grouping interface here

    #region OfferBookgGridDefaultGroup - implementation of the default grouping style

    /// <summary>
    /// each arrange/grouping class must implement the IOfferBookGridGroup interface
    /// the Group object will determine for each object in the grid, whether it
    /// falls in or outside its group.
    /// It uses the IComparable.CompareTo function to determine if the item is in the group.
    /// </summary>
    public class OfferBookgGridDefaultGroup : IOfferBookGridGroup
    {
        protected object val;
        protected string text;
        protected bool collapsed;
        protected DataGridViewColumn column;
        protected int itemCount;
        protected long quantidade;
        protected int height;
        protected int items;
        protected int groupIndex;
        protected GridType type;

        public OfferBookgGridDefaultGroup()
        {
            val = null;

            this.column = null;
            height = 20; // default height
        }

        #region IOfferBookGridGroup Members

        public virtual string Text
        {
            get 
            {
                if (column == null)
                {
                    return string.Format("Unbound group: {0} ({1})", Value.ToString(), itemCount == 1 ? "1 item" : itemCount.ToString() + " items");
                }
                else
                {
                    return string.Format("{0}|{1}|{2:0.0,0#}", ItemCount.ToString(), quantidade.ToString(), Value);
                }
            }
            set { text = value; }
        }

        public virtual object Value
        {
            get { return val; }
            set { val = value; }
        }

        public virtual bool Collapsed
        {
            get { return collapsed; }
            set { collapsed = value; }
        }

        public virtual DataGridViewColumn Column
        {
            get { return column; }
            set { column = value; }
        }

        public virtual int ItemCount
        {
            get { return itemCount; }
            set { itemCount = value; }
        }

        public virtual int Height
        {
            get { return height; }
            set { height = value; }
        }

        public virtual int Items
        {
            get { return items; }
            set { items = value; }
        }

        public virtual int GroupIndex
        {
            get { return groupIndex; }
            set { groupIndex = value; }
        }

        public virtual long Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }

        public virtual GridType Type
        {
            get { return type; }
            set { type = value; }
        }

        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            OfferBookgGridDefaultGroup gr = new OfferBookgGridDefaultGroup();
            gr.column = this.column;
            gr.val = this.val;
            gr.collapsed = this.collapsed;
            gr.text = this.text;
            gr.height = this.height;
            
            gr.items = this.items;
            gr.groupIndex = this.groupIndex;
            
            return gr;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// this is a basic string comparison operation. 
        /// all items are grouped and categorised based on their string-appearance.
        /// </summary>
        /// <param name="obj">the value in the related column of the item to compare to</param>
        /// <returns></returns>
        public virtual int CompareTo(object obj)
        {
            return string.Compare(val.ToString(), obj.ToString());
        }

        #endregion
    }
    #endregion OfferBookgGridDefaultGroup - implementation of the default grouping style

    #region OfferBookGridAlphabeticGroup - an alphabet group implementation
    /// <summary>
    /// this group simple example of an implementation which groups the items into Alphabetic categories
    /// based only on the first letter of each item
    /// 
    /// for this we need to override the Value property (used for comparison)
    /// and the CompareTo function.
    /// Also the Clone method must be overriden, so this Group object can create clones of itself.
    /// Cloning of the group is used by the OfferBookGrid
    /// </summary>
    public class OfferBookGridAlphabeticGroup : OfferBookgGridDefaultGroup
    {
        public OfferBookGridAlphabeticGroup()
            : base()
        {
            
        }

        public override string Text
        {
            get
            {
                return string.Format("Alphabetic: {1} ({2})", column.HeaderText, Value.ToString(), itemCount == 1 ? "1 item" : itemCount.ToString() + " items");
            }
            set { text = value; }
        }

        public override object Value
        {
            get { return val; }
            set { val = value.ToString().Substring(0,1).ToUpper(); }
        }

        #region ICloneable Members
        /// <summary>
        /// each group class must implement the clone function
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            OfferBookGridAlphabeticGroup gr = new OfferBookGridAlphabeticGroup();
            gr.column = this.column;
            gr.val = this.val;
            gr.collapsed = this.collapsed;
            gr.text = this.text;
            gr.height = this.height;
            return gr;
        }

        #endregion

        #region IComparable Members
        /// <summary>
        /// overide the CompareTo, so only the first character is compared, instead of the whole string
        /// this will result in classifying each item into a letter of the Alphabet.
        /// for instance, this is usefull when grouping names, they will be categorized under the letters A, B, C etc..
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int CompareTo(object obj)
        {
            return string.Compare(val.ToString(), obj.ToString().Substring(0, 1).ToUpper());
        }

        #endregion IComparable Members

    }
    #endregion OfferBookGridAlphabeticGroup - an alphabet group implementation
    
}
