using System;
using System.Collections.Generic;
using System.Reflection;
using Eto;
using PropertyChanged;
using TsinghuaNet.Eto.Models;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.Eto.ViewModels
{
    public class SettingsViewModel : NetViewModelBase
    {
        [DoNotNotify]
        public new NetEtoSettings Settings
        {
            get => (NetEtoSettings)base.Settings;
            set => base.Settings = value;
        }

        public List<PackageBox> Packages { get; } = new List<PackageBox>()
        {
            new PackageBox("Eto.Forms", "BSD-3"),
            new PackageBox("Eto.Serialization.Xaml", "BSD-3"),
            new PackageBox("Fody", "MIT"),
            new PackageBox("HtmlAgilityPack", "MIT"),
            new PackageBox("Portable.Xaml", "MIT"),
            new PackageBox("PropertyChanged.Fody", "MIT"),
            new PackageBox("Refractored.MvvmHelpers", "MIT"),
            new PackageBox("System.Linq.Async", "Apache-2.0")
        };

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public SettingsViewModel() : base()
        {
            var platform = Platform.Instance;
            if (platform.IsWpf)
            {
                Packages.Add(new PackageBox("Eto.Platform.Wpf", "BSD-3"));
            }
            else if (platform.IsGtk)
            {
                Packages.Add(new PackageBox("Eto.Platform.Gtk", "BSD-3"));
                Packages.Add(new PackageBox("GtkSharp", "LGPLv2"));
            }
            else if (platform.IsMac)
            {
                Packages.Add(new PackageBox("Eto.Platform.Mac64", "BSD-3"));
            }
        }
    }
}
