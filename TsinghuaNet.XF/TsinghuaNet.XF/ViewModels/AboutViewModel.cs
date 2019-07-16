using System;
using System.Collections.Generic;
using System.Reflection;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.XF.ViewModels
{
    class AboutViewModel : NetViewModelBase
    {
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public IEnumerable<PackageBox> Packages { get; } = new List<PackageBox>
        {
            new PackageBox("Fody", "MIT"),
            new PackageBox("HtmlAgilityPack", "MIT"),
            new PackageBox("PropertyChanged.Fody", "MIT"),
            new PackageBox("Rg.Plugins.Popup", "MIT"),
            new PackageBox("System.Linq.Async","Apache-2.0"),
            new PackageBox("Xamarin.Forms.DataGrid", "MIT")
        };
    }
}
