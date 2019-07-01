using System.Collections.Generic;
using TsinghuaNet.Models;
using Windows.ApplicationModel;

namespace TsinghuaNet.Uno.ViewModels
{
    class AboutViewModel : NetObservableBase
    {
        public PackageVersion Version { get; } = Package.Current.Id.Version;

        public IEnumerable<PackageBox> Packages { get; set; }
    }
}
