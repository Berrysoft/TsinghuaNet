using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace TsinghuaNet.XF.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        internal void SaveSettings()
        {
            var p = (InfoPage)Children.First();
            p.SaveSettings();
        }
    }
}
