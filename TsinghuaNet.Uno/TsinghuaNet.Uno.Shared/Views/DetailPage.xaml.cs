using System;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;

namespace TsinghuaNet.Uno.Views
{
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            InitializeComponent();
        }

        private void DetailsView_Sorting(object sender, DataGridColumnEventArgs e)
        {
            DataGridSortDirection? dir;
            var clickedColumn = e.Column;
            var oridir = clickedColumn.SortDirection;
            foreach (var c in DetailsView.Columns)
                c.SortDirection = null;
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
                if (Enum.TryParse(clickedColumn.Tag.ToString(), out NetDetailOrder order))
                {
                    Model.SortSource(order, !ascending);
                }
            }
            else
                Model.SortSource(null, false);
        }
    }
}
