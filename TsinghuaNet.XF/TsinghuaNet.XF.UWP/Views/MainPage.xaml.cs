using Xamarin.Forms.Platform.UWP;

namespace TsinghuaNet.XF.UWP.Views
{
    public sealed partial class MainPage : WindowsPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadApplication(new TsinghuaNet.XF.App());
        }
    }
}
