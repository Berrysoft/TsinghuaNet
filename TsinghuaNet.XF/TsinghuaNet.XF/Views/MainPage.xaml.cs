using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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

        internal Task SaveSettingsAsync()
        {
            var p = (InfoPage)Children.First();
            return p.SaveSettingsAsync();
        }
    }
}
