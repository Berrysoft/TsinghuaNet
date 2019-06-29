using System.Collections.Generic;
using TsinghuaNet.Models;
using Windows.ApplicationModel;

namespace TsinghuaNet.Uno.ViewModels
{
    class AboutViewModel : NetObservableBase
    {
        public PackageVersion Version { get; } = Package.Current.Id.Version;

        private IEnumerable<PackageBox> packages;
        public IEnumerable<PackageBox> Packages
        {
            get => packages;
            set => SetProperty(ref packages, value);
        }
    }
}
