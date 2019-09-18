using System;
using System.Collections.Generic;
using System.Reflection;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;
using Xamarin.Forms;

namespace TsinghuaNet.XF.ViewModels
{
    class AboutViewModel : NetViewModelBase
    {
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public List<PackageBox> Packages { get; } = new List<PackageBox>
        {
            new PackageBox("Fody", "MIT"),
            new PackageBox("HtmlAgilityPack", "MIT"),
            new PackageBox("Microcharts", "MIT"),
            new PackageBox("PropertyChanged.Fody", "MIT"),
            new PackageBox("Refractored.MvvmHelpers", "MIT"),
            new PackageBox("Rg.Plugins.Popup", "MIT"),
            new PackageBox("SkiaSharp", "MIT"),
            new PackageBox("System.Linq.Async","Apache-2.0"),
            new PackageBox("Xamarin.Forms.DataGrid", "MIT")
        };

        public AboutViewModel() : base()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.UWP:
                    Packages.Add(new PackageBox("Microsoft.Toolkit.Uwp", "MIT"));
                    break;
            }
        }
    }
}
