using System;
using System.Collections.Generic;
using System.Reflection;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;
using Windows.ApplicationModel;

namespace TsinghuaNet.Uno.ViewModels
{
    class AboutViewModel : NetViewModelBase
    {
#if WINDOWS_UWP
        public PackageVersion Version { get; } = Package.Current.Id.Version;
#else
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
#endif

        public IEnumerable<PackageBox> Packages { get; set; }
    }
}
