using System;
using Eto.Forms;
using Eto.Serialization.Xaml;
using TsinghuaNet.Eto.Controls;
using TsinghuaNet.Eto.ViewModels;

namespace TsinghuaNet.Eto.Views
{
    public class DetailsDialog : Dialog
    {
        private DetailViewModel Model;

        public DetailsDialog()
        {
            XamlReader.Load(this);
            Model = new DetailViewModel();
            DataContext = Model;
        }

        private void DetailsView_ColumnHeaderClick(object sender, GridColumnEventArgs e)
        {
            if (Model.DetailsSource.Count > 0)
            {
                GridSortDirection? dir;
                GridView DetailsView = (GridView)sender;
                var clickedColumn = (SortableGridColumn)e.Column;
                var oridir = clickedColumn.SortDirection;
                foreach (var c in DetailsView.Columns)
                    ((SortableGridColumn)c).SortDirection = null;
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
                    if (Enum.TryParse(clickedColumn.Tag, out NetDetailOrder order))
                    {
                        Model.SortSource(order, !ascending);
                    }
                }
                else
                    Model.SortSource(null, false);
            }
        }
    }
}
