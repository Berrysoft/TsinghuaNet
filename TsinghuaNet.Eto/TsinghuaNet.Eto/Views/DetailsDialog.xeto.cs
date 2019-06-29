using Eto.Forms;
using Eto.Serialization.Xaml;
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
            var DetailsView = FindChild<GridView>("DetailsView");
            DetailsView.DataStore = Model.DetailsSource;
        }

        private const char AscendingChar = '▲';
        private const char DescendingChar = '▼';

        // 打ち止めは最高だ！
        private NetDetailOrder? lastOrder;
        private bool lastDescending;

        private void DetailsView_ColumnHeaderClick(object sender, GridColumnEventArgs e)
        {
            if (Model.DetailsSource.Count > 0)
            {
                GridView DetailsView = (GridView)sender;
                var index = DetailsView.Columns.IndexOf(e.Column);
                if (lastOrder == null)
                {
                    lastOrder = (NetDetailOrder)index;
                    lastDescending = false;
                    e.Column.HeaderText += AscendingChar;
                }
                else if ((int)lastOrder.Value == index)
                {
                    var t = e.Column.HeaderText;
                    if (!lastDescending)
                    {
                        lastDescending = true;
                        e.Column.HeaderText = t.Substring(0, t.Length - 1) + DescendingChar;
                    }
                    else
                    {
                        lastOrder = null;
                        e.Column.HeaderText = t.Substring(0, t.Length - 1);
                    }
                }
                else
                {
                    var oldc = DetailsView.Columns[(int)lastOrder.Value];
                    var oldt = oldc.HeaderText;
                    oldc.HeaderText = oldt.Substring(0, oldt.Length - 1);
                    lastOrder = (NetDetailOrder)(index * 2);
                    e.Column.HeaderText += AscendingChar;
                }
                Model.SortSource(lastOrder, lastDescending);
            }
        }
    }
}
