using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Spider.GlobalOrderTracking
{
    public class PropertyComparer<T> : IComparer<T>
    {
        private readonly System.Collections.IComparer comparer;
        private System.ComponentModel.PropertyDescriptor propertyDescriptor;
        private int reverse;

        public PropertyComparer(System.ComponentModel.PropertyDescriptor property, System.ComponentModel.ListSortDirection direction)
        {
            this.propertyDescriptor = property;
            Type comparerForPropertyType = typeof(Comparer<>).MakeGenericType(property.PropertyType);
            this.comparer = (System.Collections.IComparer)comparerForPropertyType.InvokeMember("Default", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Public, null, null, null);
            this.SetListSortDirection(direction);
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return this.reverse * this.comparer.Compare(this.propertyDescriptor.GetValue(x), this.propertyDescriptor.GetValue(y));
        }

        #endregion

        private void SetPropertyDescriptor(System.ComponentModel.PropertyDescriptor descriptor)
        {
            this.propertyDescriptor = descriptor;
        }

        private void SetListSortDirection(System.ComponentModel.ListSortDirection direction)
        {
            this.reverse = direction == System.ComponentModel.ListSortDirection.Ascending ? 1 : -1;
        }

        public void SetPropertyAndDirection(System.ComponentModel.PropertyDescriptor descriptor, System.ComponentModel.ListSortDirection direction)
        {
            this.SetPropertyDescriptor(descriptor);
            this.SetListSortDirection(direction);
        }
    }

}
