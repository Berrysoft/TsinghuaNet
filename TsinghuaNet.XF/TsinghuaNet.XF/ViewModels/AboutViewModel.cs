﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PropertyChanged;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;
using Xamarin.Forms;

namespace TsinghuaNet.XF.ViewModels
{
    class AboutViewModel : NetViewModelBase
    {
        [DoNotNotify]
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        [DoNotNotify]
        public string Copyright => Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyCopyrightAttribute>().FirstOrDefault()?.Copyright;

        [DoNotNotify]
        public List<PackageBox> Packages { get; } = new List<PackageBox>
        {
            new PackageBox("Fody", "MIT"),
            new PackageBox("HtmlAgilityPack", "MIT"),
            new PackageBox("PropertyChanged.Fody", "MIT"),
            new PackageBox("Refractored.MvvmHelpers", "MIT"),
            new PackageBox("Syncfusion.Xamarin", "Community License"),
            new PackageBox("System.Linq.Async","Apache-2.0"),
        };

        public AboutViewModel() : base()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.UWP:
                    Packages.Add(new PackageBox("Microsoft.Toolkit.Uwp", "MIT"));
                    Packages.Add(new PackageBox("Microsoft.UI.Xaml", "MIT"));
                    break;
            }
        }
    }
}
