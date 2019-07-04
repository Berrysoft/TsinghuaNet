using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Eto.Forms;
using MvvmHelpers;
using TsinghuaNet.Helpers;

namespace TsinghuaNet.Eto.Controls
{
    public class SortableGridView : GridView
    {
        public SortableGridView() : base()
        {
            dataStore = new ObservableRangeCollection<object>();
            base.DataStore = dataStore;
        }

        private IEnumerable data;
        private ObservableRangeCollection<object> dataStore;

        public new IEnumerable DataStore
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
                    dataStore.ReplaceRange(data.OfType<object>());
                }
            }
        }

        private void OnDataChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var c in Columns)
            {
                if (c is SortableGridColumn sc)
                    sc.SortDirection = null;
            }
            dataStore.ReplaceRange(data.OfType<object>());
        }

        protected override void OnColumnHeaderClick(GridColumnEventArgs e)
        {
            if (data != null)
            {
                GridSortDirection? dir;
                if (e.Column is SortableGridColumn clickedColumn)
                {
                    var oridir = clickedColumn.SortDirection;
                    foreach (var c in Columns)
                    {
                        if (c is SortableGridColumn sc)
                            sc.SortDirection = null;
                    }
                    switch (oridir)
                    {
                        case GridSortDirection.Ascending:
                            dir = GridSortDirection.Descending;
                            break;
                        case GridSortDirection.Descending:
                            dir = null;
                            break;
                        default:
                            dir = GridSortDirection.Ascending;
                            break;
                    }
                    clickedColumn.SortDirection = dir;
                    if (dir != null)
                    {
                        bool ascending = dir.Value == GridSortDirection.Ascending;
                        dataStore.ReplaceRange(data.OfType<object>().OrderBy(o => o.GetType().GetProperty(clickedColumn.Tag).GetValue(o), !ascending));
                    }
                    else
                    {
                        dataStore.ReplaceRange(data.OfType<object>());
                    }
                }
            }
            base.OnColumnHeaderClick(e);
        }
    }
}
