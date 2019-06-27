using System.Collections.Generic;
using TsinghuaNet.Models;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;

namespace TsinghuaNet.Uno.Views
{
    public sealed partial class AboutDialog : ContentDialog
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        public readonly PackageVersion Version = Package.Current.Id.Version;

        public string GetVersionString(PackageVersion ver)
        {
            return $"版本 {ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
        }

        public readonly List<PackageBox> Packages = new List<PackageBox>()
        {
            new PackageBox("HtmlAgilityPack", "MIT许可证"),
            new PackageBox("Microsoft.Toolkit.Uwp", "MIT许可证"),
            new PackageBox("Microsoft.Toolkit.Uwp.Connectivity", "MIT许可证"),
            new PackageBox("Microsoft.Toolkit.Uwp.UI.Controls.DataGrid", "MIT许可证"),
            new PackageBox("Microsoft.UI.Xaml", "MIT许可证"),
            new PackageBox("Newtonsoft.Json", "MIT许可证"),
            new PackageBox("Refractored.MvvmHelpers", "MIT许可证")
        };
    }
}
