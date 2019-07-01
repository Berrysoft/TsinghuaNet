using System.Collections.Generic;
using Eto;
using TsinghuaNet.Models;

namespace TsinghuaNet.Eto.ViewModels
{
    public class SettingsViewModel : NetObservableBase
    {
        public new NetSettings Settings
        {
            get => (NetSettings)base.Settings;
            set => base.Settings = value;
        }

        public List<PackageBox> Packages { get; } = new List<PackageBox>()
        {
            new PackageBox("Eto.Forms", "BSD-3许可证"),
            new PackageBox("Eto.Serialization.Xaml", "BSD-3许可证"),
            new PackageBox("Fody", "MIT许可证"),
            new PackageBox("HtmlAgilityPack", "MIT许可证"),
            new PackageBox("Newtonsoft.Json", "MIT许可证"),
            new PackageBox("PropertyChanged.Fody", "MIT许可证"),
            new PackageBox("Refractored.MvvmHelpers", "MIT许可证")
        };

        public SettingsViewModel() : base()
        {
            var platform = Platform.Instance;
            if (platform.IsWpf)
                Packages.Add(new PackageBox("Eto.Platform.Wpf", "BSD-3许可证"));
            else if (platform.IsGtk)
            {
                Packages.Add(new PackageBox("Eto.Platform.Gtk", "BSD-3许可证"));
                Packages.Add(new PackageBox("GtkSharp", "LGPLv2许可证"));
            }
            else if (platform.IsMac)
                Packages.Add(new PackageBox("Eto.Platform.Mac64", "BSD-3许可证"));
            Packages.Sort((PackageBox p1, PackageBox p2) => p1.Name.CompareTo(p2.Name));
        }
    }
}
