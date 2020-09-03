using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;

namespace TsinghuaNet.Models
{
    public abstract class NetSettingsBase : INotifyPropertyChanged
    {
        protected static readonly List<int> PredefinedAcIds = new List<int> { 1, 25, 33, 35, 37, 159 };

#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        [SuppressPropertyChangedWarnings]
        public abstract bool AutoLogin { get; set; }

        [SuppressPropertyChangedWarnings]
        public abstract bool EnableRelogin { get; set; }

        [SuppressPropertyChangedWarnings]
        public abstract bool EnableFluxLimit { get; set; }

        [SuppressPropertyChangedWarnings]
        public abstract ByteSize FluxLimit { get; set; }

        public List<int> AcIds { get; set; } = new List<int>(PredefinedAcIds);
    }
}
