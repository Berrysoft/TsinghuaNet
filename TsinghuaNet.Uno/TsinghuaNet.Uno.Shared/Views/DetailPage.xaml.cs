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
            var oridir = e.Column.SortDirection;
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
            e.Column.SortDirection = dir;
            if (dir != null)
            {
                bool ascending = dir.Value == DataGridSortDirection.Ascending;
                switch (e.Column.Tag)
                {
                    case "LoginTime":
                        Model.SortSource(NetDetailOrder.LoginTime, !ascending);
                        break;
                    case "LogoutTime":
                        Model.SortSource(NetDetailOrder.LogoutTime, !ascending);
                        break;
                    case "Flux":
                        Model.SortSource(NetDetailOrder.Flux, !ascending);
                        break;
                }
            }
            else
                Model.SortSource(null, false);
        }
    }
}
