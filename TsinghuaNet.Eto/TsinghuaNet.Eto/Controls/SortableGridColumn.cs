using Eto;
using Eto.Forms;

namespace TsinghuaNet.Eto.Controls
{
    public enum GridSortDirection
    {
        Ascending,
        Descending
    }

    [Handler(typeof(IHandler))]
    [ContentProperty("DataCell")]
    public class SortableGridColumn : GridColumn
    {
        new IHandler Handler { get => (IHandler)base.Handler; }

        public new string HeaderText
        {
            get => Handler.HeaderText;
            set => Handler.HeaderText = value;
        }

        public string Tag { get; set; }

        public GridSortDirection? SortDirection
        {
            get => Handler.SortDirection;
            set => Handler.SortDirection = value;
        }

        private const char AscendingChar = '▲';
        private const char DescendingChar = '▼';

        public void RefreshBaseHeaderText()
        {
            if (SortDirection.HasValue)
            {
                switch (SortDirection.Value)
                {
                    case GridSortDirection.Ascending:
                        base.HeaderText = HeaderText + AscendingChar;
                        break;
                    case GridSortDirection.Descending:
                        base.HeaderText = HeaderText + DescendingChar;
                        break;
                }
            }
            else
            {
                base.HeaderText = HeaderText;
            }
        }

        public new interface IHandler : GridColumn.IHandler
        {
            new string HeaderText { get; set; }
            GridSortDirection? SortDirection { get; set; }
        }
    }
}
