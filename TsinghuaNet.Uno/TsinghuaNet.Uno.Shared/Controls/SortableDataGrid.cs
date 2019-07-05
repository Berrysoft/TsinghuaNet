using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls;
using TsinghuaNet.Helpers;

namespace TsinghuaNet.Uno.Controls
{
    public partial class SortableDataGrid : DataGrid
    {
        public SortableDataGrid() : base()
        {
            Sorting += SortableDataGrid_Sorting;
        }

        private IEnumerable data;
        public new IEnumerable ItemsSource
        {
            get => data;
            set
            {
                if (data is INotifyCollectionChanged ncc)
                    ncc.CollectionChanged -= OnDataChanged;
                data = value;
                if (data != null)
                {
                    if (data is INotifyCollectionChanged ncc2)
                        ncc2.CollectionChanged += OnDataChanged;
                    base.ItemsSource = data;
                }
            }
        }

        private void OnDataChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var c in Columns)
            {
                c.SortDirection = null;
            }
            base.ItemsSource = data;
        }

        private void SortableDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            DataGridSortDirection? dir;
            var clickedColumn = e.Column;
            var oridir = clickedColumn.SortDirection;
            foreach (var c in Columns)
            {
                c.SortDirection = null;
            }
            switch (oridir)
            {
                case DataGridSortDirection.Ascending:
                    dir = DataGridSortDirection.Descending;
                    break;
                case DataGridSortDirection.Descending:
                    dir = null;
                    break;
                default:
                    dir = DataGridSortDirection.Ascending;
                    break;
            }
            clickedColumn.SortDirection = dir;
            if (dir != null)
            {
                bool ascending = dir.Value == DataGridSortDirection.Ascending;
                base.ItemsSource = data.OfType<object>().OrderBy(o => o.GetType().GetProperty(clickedColumn.Tag.ToString()).GetValue(o), !ascending);
            }
            else
            {
                base.ItemsSource = data;
            }
        }
    }
}
