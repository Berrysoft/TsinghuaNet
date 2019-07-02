using Eto;
using Eto.Forms;

namespace TsinghuaNet.Eto.Controls
{
    public enum GridSortDirection
    {
        Ascending,
        Descending
    }

    [ContentProperty("DataCell")]
    public class SortableGridColumn : GridColumn
    {
        private const char AscendingChar = '▲';
        private const char DescendingChar = '▼';

        private string headerText;
        public new string HeaderText
        {
            get => headerText;
            set
            {
                headerText = value;
                SetBaseHeaderText();
            }
        }

        private void SetBaseHeaderText()
        {
            if (SortDirection.HasValue)
            {
                switch (SortDirection.Value)
                {
                    case GridSortDirection.Ascending:
                        base.HeaderText = headerText + AscendingChar;
                        break;
                    case GridSortDirection.Descending:
                        base.HeaderText = headerText + DescendingChar;
                        break;
                }
            }
            else
            {
                base.HeaderText = headerText;
            }
        }

        public string Tag { get; set; }

        private GridSortDirection? sortDirection;
        public GridSortDirection? SortDirection
        {
            get => sortDirection;
            set
            {
                sortDirection = value;
                SetBaseHeaderText();
            }
        }
    }
}
