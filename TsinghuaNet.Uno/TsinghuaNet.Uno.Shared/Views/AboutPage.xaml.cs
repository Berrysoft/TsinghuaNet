using System.Collections.Generic;
using System.Threading.Tasks;
using TsinghuaNet.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TsinghuaNet.Uno.Views
{
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(500);
            Model.Packages = new List<PackageBox>()
            {
                new PackageBox("Fody", "MIT许可证"),
                new PackageBox("HtmlAgilityPack", "MIT许可证"),
#if WINDOWS_UWP
                new PackageBox("Microsoft.Toolkit.Uwp", "MIT许可证"),
#endif
                new PackageBox("Newtonsoft.Json", "MIT许可证"),
                new PackageBox("PropertyChanged.Fody", "MIT许可证"),
#if __ANDROID__ || __IOS__
                new PackageBox("Uno.Microsoft.Toolkit.Uwp", "MIT许可证"),
                new PackageBox("Uno.UI","Apache 2.0 许可证"),
#endif
#if __ANDROID__
                new PackageBox("Uno.UniversalImageLoader","Apache 2.0 许可证"),
#endif
            };
        }
    }
}
