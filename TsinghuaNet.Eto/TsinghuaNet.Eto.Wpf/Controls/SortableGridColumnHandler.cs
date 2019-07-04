using System.ComponentModel;
using Eto.Wpf.Forms.Controls;
using TsinghuaNet.Eto.Controls;

namespace TsinghuaNet.Eto.Wpf.Controls
{
    public class SortableGridColumnHandler : GridColumnHandler, SortableGridColumn.IHandler
    {
        public GridSortDirection? SortDirection
        {
            get => (GridSortDirection?)Control.SortDirection;
            set => Control.SortDirection = (ListSortDirection?)value;
        }
    }
}
