using System.Collections.Generic;
using TsinghuaNet.Models;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;

namespace TsinghuaNet.Uno.Views
{
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
            PackagesView.ItemsSource = packages;
        }

        public PackageVersion Version { get; } = Package.Current.Id.Version;

        private readonly List<PackageBox> packages = new List<PackageBox>()
        {
            new PackageBox("HtmlAgilityPack", "MIT许可证"),
#if WINDOWS_UWP
            new PackageBox("Microsoft.Toolkit.Uwp", "MIT许可证"),
            new PackageBox("Microsoft.Toolkit.Uwp.Connectivity", "MIT许可证"),
            new PackageBox("Microsoft.Toolkit.Uwp.UI.Controls", "MIT许可证"),
            new PackageBox("Microsoft.Toolkit.Uwp.UI.Controls.DataGrid", "MIT许可证"),
#endif
            new PackageBox("Newtonsoft.Json", "MIT许可证"),
            new PackageBox("Refractored.MvvmHelpers", "MIT许可证"),
#if __ANDROID__ || __IOS__
            new PackageBox("Uno.Microsoft.Toolkit.Uwp", "MIT许可证"),
            new PackageBox("Uno.Microsoft.Toolkit.Uwp.UI.Controls", "MIT许可证"),
            new PackageBox("Uno.Microsoft.Toolkit.Uwp.UI.Controls.DataGrid", "MIT许可证"),
            new PackageBox("Uno.UI","Apache 2.0 许可证"),
#if !__IOS__
            new PackageBox("Uno.UniversalImageLoader","Apache 2.0 许可证"),
#endif
#endif
        };
    }
}
