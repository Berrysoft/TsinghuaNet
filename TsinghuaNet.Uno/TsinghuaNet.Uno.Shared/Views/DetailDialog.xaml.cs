using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls;
using MvvmHelpers;
using TsinghuaNet.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TsinghuaNet.Uno.Views
{
    public sealed partial class DetailDialog : ContentDialog
    {
        public ObservableRangeCollection<KeyValuePair<int, double>> Details { get; } = new ObservableRangeCollection<KeyValuePair<int, double>>();

        public DetailDialog()
        {
            InitializeComponent();
            //XAxis.Interval = 1;
            //YAxis.Minimum = 0;
        }

        public static readonly DependencyProperty ChartBrushOffsetProperty = DependencyProperty.Register(nameof(ChartBrushOffset), typeof(double), typeof(DetailDialog), new PropertyMetadata(1.0));
        public double ChartBrushOffset
        {
            get
            {
                return (double)GetValue(ChartBrushOffsetProperty);
            }
            set
            {
                SetValue(ChartBrushOffsetProperty, value);
            }
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

        private void Model_DetailsInitialized(object sender, IEnumerable<NetDetail> e)
        {
            Details.ReplaceRange(e.GetDailyDetails());
        }
    }

    static class NetDetailExtensions
    {
        public static IEnumerable<KeyValuePair<int, double>> GetDailyDetails(this IEnumerable<NetDetail> ds)
        {
            double totalf = 0;
            DateTime now = DateTime.Now;
            foreach (var b in from d in ds group d.Flux by d.LogoutTime.Day into g select new { Day = g.Key, Flux = g.Sum() })
            {
                totalf += b.Flux.GigaBytes;
                yield return new KeyValuePair<int, double>(b.Day, totalf);
            }
        }
    }
}
