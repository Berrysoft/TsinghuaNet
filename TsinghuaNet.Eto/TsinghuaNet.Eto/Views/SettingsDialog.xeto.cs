using System.Collections.Generic;
using Eto.Forms;
using Eto.Serialization.Xaml;
using TsinghuaNet.Eto.ViewModels;
using TsinghuaNet.Models;

namespace TsinghuaNet.Eto.Views
{
    public class SettingsDialog : Dialog
    {
        public IReadOnlyList<PackageBox> Packages { get; } = new List<PackageBox>()
        {
            new PackageBox("Eto.Forms", "BSD-3许可证"),
            new PackageBox("Eto.Platform.Gtk", "BSD-3许可证"),
            new PackageBox("Eto.Platform.Mac64", "BSD-3许可证"),
            new PackageBox("Eto.Platform.Wpf", "BSD-3许可证"),
            new PackageBox("Eto.Serialization.Xaml", "BSD-3许可证"),
            new PackageBox("HtmlAgilityPack", "MIT许可证"),
            new PackageBox("Newtonsoft.Json", "MIT许可证"),
            new PackageBox("Refractored.MvvmHelpers", "MIT许可证")
        };

        private SettingsViewModel Model;

        public SettingsDialog(int page = 0)
        {
            XamlReader.Load(this);
            Model = new SettingsViewModel();
            DataContext = Model;
            FindChild<TabControl>("SettingsTab").SelectedIndex = page;
            FindChild<GridView>("PackageView").DataStore = Packages;
        }
    }
}
